using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utils;
using System;

namespace Advertising
{
    public class AdsManager : DontDestroyMonoSingleton<AdsManager>, IAdvertising
    {

        private IAdvertising strategy = null;
        private bool isRemoveAds;

        public bool InterestialAdsAvailable
        {
            get
            {
                return strategy.InterestialAdsAvailable;
            }
        }

        public Action OnRewardedVideoFinishedEvent
        {
            get
            {
                return strategy.OnRewardedVideoFinishedEvent;
            }

            set
            {
                strategy.OnRewardedVideoFinishedEvent = value;
            }
        }

		public Action OnRewardedVideoClosedEvent
		{
			get
			{
				return strategy.OnRewardedVideoClosedEvent;
			}

			set
			{
				strategy.OnRewardedVideoClosedEvent = value;
			}
		}

		public Action OnRewardedVideoIsNotReadyEvent
        {
            get
            {
                return strategy.OnRewardedVideoIsNotReadyEvent;
            }

            set
            {
                strategy.OnRewardedVideoIsNotReadyEvent = value;
            }
        }

        public bool RewardedVideoAdsAvailable
        {
            get
            {
                return strategy.RewardedVideoAdsAvailable;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            Instance.Initialize();
        }

        public void Initialize()
        {
#if UNITY_EDITOR
			strategy = gameObject.AddComponent<UnityEditorAds>();
#elif UNITY_IOS || UNITY_ANDROID
            strategy = gameObject.AddComponent<AppodealIAds>();
#elif UNITY_WSA
            strategy = gameObject.AddComponent<WindowsStoreIAds>();
#endif
			strategy.Initialize();
            isRemoveAds = PlayerPrefs.GetInt("RemoveAds") == 1;
            if(!isRemoveAds)
                ShowBanner();
        }

        public void ShowBanner()
        {
            if (!isRemoveAds)
                strategy.ShowBanner();
        }

        public void HideBanner()
        {
            strategy.HideBanner();
        }

        public void ShowInterestial()
        {
            if (!isRemoveAds)
                strategy.ShowInterestial();
        }

        public void ShowRewardedVideo()
        {
            strategy.ShowRewardedVideo();
        }

        public void RemoveAds()
        {
            HideBanner();
            PlayerPrefs.SetInt("RemoveAds",1);
        }
    }
}
