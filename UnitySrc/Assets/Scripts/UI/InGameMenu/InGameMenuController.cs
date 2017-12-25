
using UnityEngine;
using Project.Core;
using System.Collections;
using System;
using Project.App;
using Project.Utils;
using System.Collections.Generic;
using Project.Core.Localization;
using Project.UI.MainMenu;
using Advertising;

namespace Project.UI.InGameMenu
{
	public class InGameMenuController : UIController<InGameMenuView>
	{
		private Color colorBaseImage = new Color(1, 1, 1, 1);
		private Color colorBaseText = new Color(1, 1, 1, 1);
		private Color colorGoodText = new Color(92 / 255f, 1, 92 / 255f, 1);
		private Color colorGoodImage = new Color(0, 197 / 255f, 0, 255);
		private Color colorBadText = new Color(1, 92 / 255f, 92 / 255f, 1);
		private Color colorBadImage = new Color(197 / 255f, 0, 0, 1);

		private bool _langRuNow = true;
		private int milliSecPerRound = StoredInPlayerPrefsParameters.secPerRound * 1000;

		private bool GetBonus1 = false;
		private bool GetBonus2 = false;

		protected override void OnStart()
		{
			base.OnStart();
			CheckAds();
		}

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBack;
			View.OnNewEvent += OnNew;
			View.OnPauseOffEvent += OnBtnPauseOff;
			View.OnPauseOnEvent += OnBtnPauseOn;
			View.OnEndGameEvent += OnEndGame;
			View.OnHint50Event += OnBtnHint50Click;
			View.OnHintNextEvent += OnBtnHintNextClick;
			if (View.ImageIngameBack.sprite != null && View.ImageIngameBack.sprite.name != "gui_ingame_back")
			{
				Destroy(View.ImageIngameBack.sprite.texture);
				Destroy(View.ImageEndGameBack.sprite.texture);
			}

			ResetTimer();
			StartCoroutine("TimeTick", 0.1f);

			_langRuNow = Localization.Instance.Language == "Russian";
			FillFirstSlide();

			AppData.Instance.PlayerScore = 0;
			View.ScoreText = "0";
			//AppData.Instance.PlayerBestScore = 0;
			//AppData.Instance.Gold = 0;
			AppData.Instance.Hint50Active = true;
			View.ButtonHint50.SetActivity(true);
			AppData.Instance.HintNextActive = true;
			View.ButtonHintNext.SetActivity(true);
			ResetBonusesState();
			View.Bonus2Time = Time.time;
			GetBonus1 = false;
			GetBonus2 = false;
			View.CanLastChance = true;
			SetGameOption();
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBack;
			View.OnNewEvent -= OnNew;
			View.OnPauseOffEvent -= OnBtnPauseOff;
			View.OnPauseOnEvent -= OnBtnPauseOn;
			View.OnEndGameEvent -= OnEndGame;
			View.OnHint50Event -= OnBtnHint50Click;
			View.OnHintNextEvent -= OnBtnHintNextClick;

			foreach (var item in View.Options)
			{
				item.IsRight = false;
			}
			View.BonusPanel1.gameObject.SetActive(false);
			View.BonusPanel2.gameObject.SetActive(false);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
		}

		public void CheckAds()
		{
			if (AppData.Instance.AdsExist)
				SetUpBtnsPos(-230);
			else
			{
				SetUpBtnsPos(0);
			}
		}

		private void SetUpBtnsPos(int dyPos)
		{
			if (View.Options != null)
			{
				View.Options[2].RectTransform.anchoredPosition = new Vector2(-685, -180 + dyPos);
				View.Options[3].RectTransform.anchoredPosition = new Vector2(685, -180 + dyPos);
				View.ScorePanel.anchoredPosition = new Vector2(0, dyPos);
			}
		}

		private void OnBack()
		{
			OnBtnPauseOff();
			View.MainMenu.gameObject.SetActive(true);
			View.PanelRight._rectTransform.anchoredPosition = new Vector2(1366, 0);
			gameObject.SetActive(false);
		}

		private void OnEndGame()
		{
			if (AppData.Instance.GameOption == 1 && View.CanLastChance)
			{
				ShowLoosePopup();
			}
			else if (AppData.Instance.GameOption == 2 && View.CanLastChance)
			{
				ShowLoosePopup();
			}
			else
			{
				OnBtnPauseOff();
				View.EndMenu.gameObject.SetActive(true);
				View.PanelRight._rectTransform.anchoredPosition = new Vector2(1366, 0);
				ResetBonusesState();
				gameObject.SetActive(false);
			}
		}

		public void OnDontLastChance()
		{
			View.CanLastChance = false;
			OnEndGame();
		}

		public void OnUseLastChance()
		{
			if (View.CanLastChance)
			{
				if (AppData.Instance.GameOption == 1)
				{
					AppData.Instance.PlayerHP = 1;
					View.HPData.Count.text = AppData.Instance.PlayerHP.ToString();
					//AppData.Instance.PlayerScore -= StoredInPlayerPrefsParameters.scorePerRound;
					View.ScoreText = AppData.Instance.PlayerScore.ToString();
					View.TimeRemain = new TimeSpan(0, 0, StoredInPlayerPrefsParameters.scorePerRound);
					View.TimeData.Count.text = bl_UtilityHelper.GetTimeFormat(View.TimeRemain);
					ResetBonusesState();
					NextSlide();
				}
				else if (AppData.Instance.GameOption == 2)
				{
					AppData.Instance.PlayerScore -= StoredInPlayerPrefsParameters.scorePerRound;
					View.ScoreText = AppData.Instance.PlayerScore.ToString();
					milliSecPerRound = StoredInPlayerPrefsParameters.secLastChance * 1000;
					View.TimeRemain = new TimeSpan(0, 0, StoredInPlayerPrefsParameters.secLastChance);
					View.TimeData.Count.text = bl_UtilityHelper.GetTimeFormat(View.TimeRemain);
					ResetBonusesState();
					NextSlide();
				}
			}
			View.CanLastChance = false;
		}
		


		private void OnNew()
		{
			OnBtnPauseOff();
			View.PanelRight._rectTransform.anchoredPosition = new Vector2(1366, 0);
			gameObject.SetActive(false);
			View.MainMenu.OnBtnStart();
		}

		private void OnBtnPauseOn()
		{
			View.BtnLeft.gameObject.SetActive(false);
			View.BtnRight.gameObject.SetActive(true);
			View.PanelMid.gameObject.SetActive(true);
			View.PanelRight.Movie(885);
		}

		private void OnBtnPauseOff()
		{
			View.BtnLeft.gameObject.SetActive(true);
			View.BtnRight.gameObject.SetActive(false);
			View.PanelMid.gameObject.SetActive(false);
			View.PanelRight.Movie(1335);
		}

		private void FillFirstSlide()
		{
			if (View.Options != null)
			{
				ImageDownloadData downloadedData = AppData.Instance.GetDownloadedImage();

				ArrayData curItem = downloadedData.WebDataItem;
				List<ArrayData> curItems = downloadedData.WebDataItems;

				View.TextureBackSprite = downloadedData.Sprite;
				View.TextureBackVec = downloadedData.SizeDelta;
				int rnd = UnityEngine.Random.Range(0, 3);
				View.Options[rnd].Text.text = _langRuNow ? curItem.Name : curItem.Name;
				View.Options[rnd].IsRight = true;
				for (int i = 0; i < 4; i++)
				{
					if (rnd != i)
					{
						View.Options[i].Text.text = _langRuNow ? curItems[i].Name : curItems[i].Name;
					}
				}

			}
		}

		private void NextSlide()
		{
			Destroy(View.ImageIngameBack.sprite.texture);
			Destroy(View.ImageEndGameBack.sprite.texture);
			View.BonusPanel1.gameObject.SetActive(false);
			View.BonusPanel2.gameObject.SetActive(false);

			ResetTimer();

			if (View.Options != null)
			{
				ImageDownloadData downloadedData = AppData.Instance.GetDownloadedImage();
				ArrayData curItem = downloadedData.WebDataItem;
				List<ArrayData> curItems = downloadedData.WebDataItems;

				View.TextureBackSprite = downloadedData.Sprite;
				View.TextureBackVec = downloadedData.SizeDelta;
				int rnd = UnityEngine.Random.Range(0, 3);
				View.Options[rnd].Text.text = _langRuNow ? curItem.Name : curItem.Name;
				View.Options[rnd].IsRight = true;
				for (int i = 0; i < 4; i++)
				{
					if (rnd != i)
					{
						View.Options[i].Text.text = _langRuNow ? curItems[i].Name : curItems[i].Name;
						View.Options[i].IsRight = false;
					}
				}
				AppData.Instance.DownloadImage();
				StartCoroutine("TimeTick", 0.1f);
				//Debug.Log("Remain count = " + AppData.Instance.GameWebData.Data.Count.ToString());
				if (AppData.Instance.GameWebData.Data.Count <= 5)
				{
					WWWUtils.Instance.InitializeGameData(OnGameData, OnGameDataError);
				}
			}
			View.RAnswersCountBonus1++;
			View.RAnswersCountBonus2++;
			if (View.RAnswersCountBonus1 >= 10 && !GetBonus1)
				OnGetBonus1();
			if (View.RAnswersCountBonus2 >= 3 && !GetBonus2)
			{
				Debug.Log("Dx Bonus2 Time " + (Time.time - View.Bonus2Time).ToString("0.0"));
				if ((Time.time - View.Bonus2Time) <= StoredInPlayerPrefsParameters.bonusPerSec)
					OnGetBonus2();
				else
				{
					View.Bonus2Time = Time.time;
					View.RAnswersCountBonus2 = 0;
				}
			}
		}

		private void OnGetBonus1()
		{
			AppData.Instance.PlayerScore += 100;
			View.ScoreText = AppData.Instance.PlayerScore.ToString();
			if (View.NeedCapBonus)
			{
				GetBonus1 = true;
			}
			View.RAnswersCountBonus1 = 0;
			View.BonusPanel1.gameObject.SetActive(true);
		}

		private void OnGetBonus2()
		{
			AppData.Instance.PlayerScore += 75;
			View.ScoreText = AppData.Instance.PlayerScore.ToString();
			if (View.NeedCapBonus)
			{
				GetBonus2 = true;
			}
			View.RAnswersCountBonus2 = 0;
			View.BonusPanel2.gameObject.SetActive(true);
			View.Bonus2Time = Time.time;
		}

		private void OnGameData(GameWebData data)
		{
			AppData.Instance.GameWebData.AddData(data.Data);
		}

		private void OnGameDataError()
		{
			PopupNoConnectionView popup = PopupManager.Instance.GetRegisteredPopup<PopupNoConnectionView>();
			if (popup != null)
				popup.Show();
		}

		public void OnBtnOption1Click()
		{
			BtnOptionClick(0);
		}

		public void OnBtnOption2Click()
		{
			BtnOptionClick(1);
		}

		public void OnBtnOption3Click()
		{
			BtnOptionClick(2);
		}

		public void OnBtnOption4Click()
		{
			BtnOptionClick(3);
		}

		private void BtnOptionClick(int number)
		{
			StopCoroutine("TimeTick");
			View.Options[number].Button.interactable = false;
			if (View.Options[number].IsRight)
			{
				ResetTimer();
				AppData.Instance.PlayerScore += StoredInPlayerPrefsParameters.scorePerRound;
				View.ScoreText = AppData.Instance.PlayerScore.ToString();
				StartCoroutine(ShowEffect(0.3f, true));
			}
			else
			{
				ResetTimer();
				StartCoroutine(ShowEffect(0.3f, false));
			}
		}

		public void OnBtnHint50Click()
		{
			if (AppData.Instance.Gold < 10)
			{
				PopupAddCurrensyView popup = PopupManager.Instance.GetRegisteredPopup<PopupAddCurrensyView>();
				if (popup != null)
				{
					StopCoroutine("TimeTick");
					popup.Show();
				}
			} else if (AppData.Instance.Hint50Active)
			{
				AppData.Instance.Gold -= 10;
				OnBtnPauseOff();
				HideWrongAnswers();
				View.ButtonHint50.SetActivity(false);
				AppData.Instance.Hint50Active = false;
				View.RAnswersCountBonus1 = 0;
			}
		}

		public void OnBtnHintNextClick()
		{
			if (AppData.Instance.Gold < 10)
			{
				PopupAddCurrensyView popup = PopupManager.Instance.GetRegisteredPopup<PopupAddCurrensyView>();
				if (popup != null)
				{
					StopCoroutine("TimeTick");
					popup.Show();
				}
			} else if (AppData.Instance.HintNextActive)
			{
				AppData.Instance.Gold -= 10;
				OnBtnPauseOff();
				StartCoroutine(ShowEffect(0.3f, true));
				View.ButtonHintNext.SetActivity(false);
				AppData.Instance.HintNextActive = false;
				View.RAnswersCountBonus1 = 0;
			}
		}

		public void OnContinueGame()
		{
			StartCoroutine("TimeTick", 0.2f);
		}

		private void HideWrongAnswers()
		{

			ResetTimer();
			int hideAnswersCount = 0;
			foreach (var item in View.Options)
			{
				if (!item.IsRight)
				{
					item.Text.text = "";
					hideAnswersCount++;
				}
				if (hideAnswersCount == 2)
				{
					return;
				}
			}
		}

		private void SetGameOption()
		{
			HideGameOption();
			uint curOption = AppData.Instance.GameOption;
			if (curOption == 1)
			{
				
				milliSecPerRound = StoredInPlayerPrefsParameters.secPerRound * 1000;
				AppData.Instance.PlayerHP = 3;
				View.HPData.Panel.SetActive(true);
				View.HPData.Count.text = AppData.Instance.PlayerHP.ToString();
			}
			else if (curOption == 2)
			{
				milliSecPerRound = StoredInPlayerPrefsParameters.secAllGame * 1000;
				View.TimeData.Panel.SetActive(true);
				View.TimeRemain = new TimeSpan(0, 0, StoredInPlayerPrefsParameters.secAllGame);
				View.TimeData.Count.text = bl_UtilityHelper.GetTimeFormat(View.TimeRemain);
			}
			else if (curOption == 3)
			{
				milliSecPerRound = StoredInPlayerPrefsParameters.secPerRound * 1000;
			}
		}

		private void HideGameOption()
		{
			View.HPData.Panel.SetActive(false);
			View.TimeData.Panel.SetActive(false);
		}

		public void ResetTimer()
		{
			if (AppData.Instance.GameOption != 2)
			{
				View.TimeRemain = new TimeSpan(0, 0, StoredInPlayerPrefsParameters.secPerRound);
				View.ProgressImage.fillAmount = 1;
			}
		}
		
		private void ShowLoosePopup()
		{
			if (AdsManager.Instance.InterestialAdsAvailable)
			{
				PopupGoodsForAdsView popup = PopupManager.Instance.GetRegisteredPopup<PopupGoodsForAdsView>();
				if (popup != null)
				{
					StopCoroutine("TimeTick");
					popup.Show();
				}
			}
			else
				OnDontLastChance();
		}

		private void ResetBonusesState()
		{
			View.RAnswersCountBonus1 = 0;
			View.RAnswersCountBonus2 = 0;
		}

		IEnumerator TimeTick(float sec)
		{
			yield return new WaitForSeconds(sec);
			TimeSpan dateTimeRemain = new TimeSpan(0, 0, 0, 0, 200);
			View.TimeRemain -= dateTimeRemain;
			double progress = View.TimeRemain.TotalMilliseconds / (float)milliSecPerRound;
			if (progress <= 0)
			{
				progress = 1;
				View.ProgressImage.fillAmount = (float)progress;
				StartCoroutine(ShowEffect(0.3f, false));
			}
			else
			{
				View.ProgressImage.fillAmount = (float)progress;
				View.Timer.text = bl_UtilityHelper.GetTimeFormat(View.TimeRemain);
				if (AppData.Instance.GameOption == 2)
					View.TimeData.Count.text = View.Timer.text;
				StartCoroutine("TimeTick", 0.2f);
			}
		}

		IEnumerator ShowEffect(float sec, bool isGood)
		{
			
			foreach (var item in View.Options)
			{
				if (item.IsRight)
				{
					item.Text.color = colorGoodText;
					item.Image.color = colorGoodImage;
				}
				else
				{
					item.Text.color = colorBadText;
					item.Image.color = colorBadImage;
				}
			}

			yield return new WaitForSeconds(sec);
			foreach (var item in View.Options)
			{
				item.IsRight = false;
				item.Text.color = colorBaseText;
				item.Image.color = colorBaseImage;
				item.Button.interactable = true;
			}

			if (isGood)
			{
				NextSlide();
			}
			else if (AppData.Instance.GameOption == 1)
			{
				AppData.Instance.PlayerHP -= 1;
				View.HPData.Count.text = AppData.Instance.PlayerHP.ToString();
				ResetBonusesState();
				if (AppData.Instance.PlayerHP <= 0)
					OnEndGame();
				else
					NextSlide();
			}
			else if (AppData.Instance.GameOption == 2)
			{
				AppData.Instance.PlayerScore -= StoredInPlayerPrefsParameters.scorePerRound;
				View.ScoreText = AppData.Instance.PlayerScore.ToString();
				ResetBonusesState();
				if (View.TimeRemain.TotalMilliseconds <= 0)
					OnEndGame();
				else
					NextSlide();
			}
			else
			{
				OnEndGame();
			}
		}
	}
}

