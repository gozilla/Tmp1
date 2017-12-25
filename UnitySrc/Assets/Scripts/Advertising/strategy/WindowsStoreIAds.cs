using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdDuplexUnitySDK.Assets.Plugins;

namespace Advertising
{
    public class WindowsStoreIAds : MonoBehaviour, IAdvertising
    {

        private Action onRewardedVideoFinishedEvent = null;
        public Action OnRewardedVideoFinishedEvent
        {
            get
            {
                return onRewardedVideoFinishedEvent;
            }

            set
            {
                onRewardedVideoFinishedEvent = value;
            }
        }

        private Action onRewardedVideoIsNotReadyEvent = null;
        public Action OnRewardedVideoIsNotReadyEvent
        {
            get
            {
                return onRewardedVideoIsNotReadyEvent;
            }

            set
            {
                onRewardedVideoIsNotReadyEvent = value;
            }
        }

		private Action onRewardedVideoClosedEvent = null;
		public Action OnRewardedVideoClosedEvent
		{
			get
			{
				return onRewardedVideoClosedEvent;
			}

			set
			{
				onRewardedVideoClosedEvent = value;
			}
		}

		public bool RewardedVideoAdsAvailable
        {
            get
            {
                return Vungle.isAdvertAvailable();
            }
        }

        public bool InterestialAdsAvailable
        {
            get
            {
                return true;
            }
        }

        public void HideBanner()
        {
            AdDuplexInterop.HideAdControl();
        }

        private void OnEnable()
        {
            Vungle.onAdFinishedEvent += onRewardedVideoFinished;
        }

        private void OnDisable()
        {
            Vungle.onAdFinishedEvent -= onRewardedVideoFinished;
        }

        private bool isInit = false;
        public void Initialize()
        {
            if (isInit)
                return;
            isInit = true;
            Vungle.init("", "", "5a36bfe0cbc18a6325006d3f");
        }

        public void ShowBanner()
        {
            AdDuplexInterop.ShowAdControl();
        }

        public void ShowInterestial()
        {
            AdDuplexInterop.ShowInterstitialAd();
        }

        public void ShowRewardedVideo()
        {
            Debug.LogWarning("ShowAds");
            if (Vungle.isAdvertAvailable())
                Vungle.playAd(true);
            else if (onRewardedVideoIsNotReadyEvent != null)
                onRewardedVideoIsNotReadyEvent();

        }

        private void onRewardedVideoFinished(AdFinishedEventArgs e)
        {
            if (e.IsCompletedView && onRewardedVideoFinishedEvent != null)
            {
                onRewardedVideoFinishedEvent();

                if (onRewardedVideoClosedEvent != null)
                    onRewardedVideoClosedEvent();
            }
        }
    }
}