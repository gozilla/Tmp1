
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Project.Core.Localization;
using Project.App;

namespace Project.UI.MainMenu
{
	public class MainMenuController : UIController<MainMenuView>
	{
		private bool langRuNow = true;

		protected override void OnStart()
		{
			base.OnStart();
		}

		protected override void _OnEnable()
		{
			View.OnStartEvent += OnBtnStart;
			View.OnSwitchLangEvent += OnSwitchLang;
			View.OnAddMoneyEvent += OnAddMoney;
            View.OnRemoveAdsClickEvent += OnRemoveAds;
			View.OnGameOptionEvent += OnGameOption;

			ChangeLangElements();
			//PlayerPrefs.SetInt("RemoveAds", 0); //for testing
			CheckAdsButton();
			CheckAds();
			CheckGameMode();
		}

		protected override void _OnDisable()
		{
			View.OnStartEvent -= OnBtnStart;
			View.OnSwitchLangEvent -= OnSwitchLang;
			View.OnAddMoneyEvent -= OnAddMoney;
            View.OnRemoveAdsClickEvent -= OnRemoveAds;
			View.OnGameOptionEvent -= OnGameOption;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
		}

		public void OnBtnStart()
		{
			if (View.Loading != null)
			{
				View.Loading.gameObject.SetActive(true);
				gameObject.SetActive(false);
			}
		}

		private void OnSwitchLang()
		{
			Localization.Instance.ChangeLanguage();
			ChangeLangElements();
            AppData.Instance.ClearWebData();
        }

		private void ChangeLangElements()
		{
			langRuNow = Localization.Instance.Language == "Russian";
			if (View.LangRu !=  null)
				foreach (var item in View.LangRu)
				{
					item.gameObject.SetActive(langRuNow);
				}
			if (View.LangEng != null)
				foreach (var item in View.LangEng)
				{
					item.gameObject.SetActive(!langRuNow);
				}
		}

		private void CheckAdsButton()
		{
			if (!AppData.Instance.AdsExist)
			{
				View.RemoveAdsBtn.gameObject.SetActive(false);
				View.StartBtnRect.anchoredPosition = new Vector2(0, 180);
			}
		}

		public void OnAddMoney()
		{
			PopupAddCurrensyView popup = PopupManager.Instance.GetRegisteredPopup<PopupAddCurrensyView>();
			if (popup != null)
				popup.Show();
		}

        private void OnRemoveAds()
        {
			Advertising.AdsManager.Instance.RemoveAds();
			CheckAdsButton();
			CheckAds();
			View.InGameMenuController.CheckAds();
#if UNITY_EDITOR

#endif
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
			View.MoneyPanel.anchoredPosition = new Vector2(0, dyPos);
			View.LangPanel.anchoredPosition = new Vector2(0, dyPos + 20);
		}

		private void OnGameOption(uint curState)
		{
			OffAllStates();
			AppData.Instance.GameOption = curState;
			SetState(curState);
		}

		private void CheckGameMode()
		{
			OffAllStates();
			SetState(AppData.Instance.GameOption);
		}

		private void OffAllStates()
		{
			View.ButtonOption1.SetState(false);
			View.ButtonOption2.SetState(false);
			View.ButtonOption3.SetState(false);
		}

		private void SetState(uint curState)
		{
			if (curState == 1)
				View.ButtonOption1.SetState(true);
			else if (curState == 2)
				View.ButtonOption2.SetState(true);
			else if (curState == 3)
				View.ButtonOption3.SetState(true);
		}

	}
}

