using Project.App;
using Project.Core;
using Project.Core.Localization;
using Project.UI.InGameMenu;
using Project.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainMenu
{
	public class Loading : MonoBehaviour {
		[SerializeField]
		Image _progressImage = null;

		[SerializeField]
		Image _backImage = null;

		public InGameMenuController _inGameMenu = null;

		public Text _hint = null;
		public LocalizationText _hintLocalization = null;

		float _time = 0;
		float _duration = 65; //Интервал ожидание если нет загрузки.
		float _curXPos = 1024;
		float _xPosDx = 2;

		float _progressDx = 0;
		float _progressDxMax = 0;
		bool useProgress = false;

		void OnEnable()
		{
			_progressImage.fillAmount = 0;
			_time = 0;
			_curXPos = 1024;
			_backImage.rectTransform.localPosition = new Vector3(_curXPos, 0, 0);
			
			if (AppData.Instance.GameWebData == null || AppData.Instance.GameWebData.Data.Count <= 5)
			{
				WWWUtils.Instance.InitializeGameData(OnGameData, OnGameDataError);
			}
			else
			{
				OnGameData(AppData.Instance.GameWebData);
			}

			WWWUtils.Instance.DowloadProgress += DowloadProgress;
			if (_hintLocalization == null)
			{
				_hintLocalization = _hint.GetComponent<LocalizationText>();
			}
			int rnd = Random.Range(1, 20);
			string locKey = "text_hint_" + rnd.ToString();
			_hint.text = locKey;
			_hintLocalization.ReLocalize();
		}

		void OnDisable()
		{
			WWWUtils.Instance.DowloadProgress -= DowloadProgress;
			useProgress = false;
			_progressDx = 0;
		}

			void Update()
		{
			if (!useProgress)
			{
				_time += Time.deltaTime;
				float progress = _time / _duration;
				progress = _time / _duration;
				if (progress >= 1f)
				{
					progress = 1;
					AppData.Instance.DownloadImages(4);
					NextScreen();
				}
				FillProgress(progress);
			}
		}

		void NextScreen()
		{
			gameObject.SetActive(false);
			if (AppData.Instance.GameOption == 2)
				_inGameMenu.View.TimeRemain = new System.TimeSpan(0, 0, StoredInPlayerPrefsParameters.secAllGame);
			else
				_inGameMenu.View.TimeRemain = new System.TimeSpan(0, 0, StoredInPlayerPrefsParameters.secPerRound);

			_inGameMenu.gameObject.SetActive(true);
		}

		private void FillProgress(float progress)
		{
			_progressImage.fillAmount = progress;
			_curXPos -= _xPosDx;
			if (_curXPos <= -1024)
				_curXPos = 1024;
			_backImage.rectTransform.localPosition = new Vector3(_curXPos, 0, 0);
		}

		private void OnGameData(GameWebData data)
		{
			if (AppData.Instance.GameWebData.Data.Count <= 5)
				AppData.Instance.GameWebData.AddData(data.Data);
			AppData.Instance.ImagesMaxCount = 1;
			if (AppData.Instance.CheckImages())
			{
				_progressDx = 0.3f;
				_progressDxMax = 1 - 0.3f;
				useProgress = true;
			}
			else
			{
				_progressImage.fillAmount = 1;
				NextScreen();
			}
		}

        private void OnGameDataError()
        {
            PopupNoConnectionView popup = PopupManager.Instance.GetRegisteredPopup<PopupNoConnectionView>();
            if (popup != null)
                popup.Show();            
        }

        private void DowloadProgress(float progress)
		{
			if (useProgress)
			{
				float curProgress = _progressDx + ((_progressDxMax * progress) / 1.0f);
				FillProgress(curProgress);
				if (curProgress >= 1f)
				{
					_progressImage.fillAmount = 1;
					NextScreen();
					AppData.Instance.DownloadImages(4);
				}
			}
		}
	}
}
