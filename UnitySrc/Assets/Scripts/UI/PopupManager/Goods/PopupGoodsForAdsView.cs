using Advertising;
using Project.App;
using Project.Core.Localization;
using Project.UI.InGameMenu;
using System;

using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainMenu
{
	public class PopupGoodsForAdsView : PopupView
	{
		//public event Action OnUseGoodEvent;
		public InGameMenuController inGameMenuController;
		public Text labelDesc = null;
		private string desc_key = null;

        private bool isRewardedFinished = false;

		[RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
        }

		private void Awake()
		{
			PopupManager.Instance.RegisterPopup(this);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
            isRewardedFinished = false;
			AdsManager.Instance.OnRewardedVideoFinishedEvent += OnVideoFinished;
            AdsManager.Instance.OnRewardedVideoClosedEvent += OnCloseVideo;
			LocalizationText loc = labelDesc.GetComponent<LocalizationText>();
			if (loc != null)
			{
				loc.LocalizationKey = desc_key;
				loc.ChangeLanguageCommand();
			}

		}

		protected override void OnDisable()
		{
			base.OnDisable();
			AdsManager.Instance.OnRewardedVideoFinishedEvent -= OnVideoFinished;
            AdsManager.Instance.OnRewardedVideoClosedEvent -= OnCloseVideo;
        }

		public void OnYesClick()
		{
			AdsManager.Instance.ShowRewardedVideo();
		}

		public void OnNoClick()
		{
			inGameMenuController.OnDontLastChance();
			Close();
		}

		private void OnVideoFinished()
		{
            isRewardedFinished = true;
			
		}

        private void OnCloseVideo()
        {
            if(isRewardedFinished)
            {
                inGameMenuController.OnUseLastChance();
                Close();
            }
        }

		public void Show()
		{
            if (!AdsManager.Instance.RewardedVideoAdsAvailable)
            {
                OnNoClick();
                return;
            }

			desc_key = "popup_goods_0" + AppData.Instance.GameOption.ToString() + "_desc";
			gameObject.SetActive(true);
		}

		public override void Close()
		{
			gameObject.SetActive(false);
            if (inGameMenuController.gameObject.activeInHierarchy)
			    inGameMenuController.OnContinueGame();
		}
    }
}

