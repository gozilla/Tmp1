using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace Advertising
{
    public class AppodealIAds : MonoBehaviour,  IRewardedVideoAdListener, IAdvertising
	{

#if UNITY_ANDROID
		string appKey = "fee50c333ff3825fd6ad6d38cff78154de3025546d47a84f";
#elif UNITY_IPHONE
		string appKey = "217b3582a4117fe00d88e864b5cd37a7e35b468245d8c292";
#else
		string appKey = "";
#endif

		public bool InterestialAdsAvailable
        {
            get
            {
                return Appodeal.isLoaded(Appodeal.INTERSTITIAL);
            }
        }

        public bool RewardedVideoAdsAvailable
        {
            get
            {
                return Appodeal.isLoaded(Appodeal.REWARDED_VIDEO);
            }
        }
		public void ShowInterestial()
		{
			if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
				Appodeal.show(Appodeal.INTERSTITIAL);
		}


        public void Initialize()
        {
            Appodeal.setTesting(false);

            Appodeal.disableLocationPermissionCheck();

            Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO);

            Appodeal.setRewardedVideoCallbacks(this);

            ShowBanner();
        }

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

		public void ShowInterstitial()
        {
            if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
            {
                Appodeal.show(Appodeal.INTERSTITIAL);
            }
        }

        public void ShowRewardedVideo()
        {
            if (Appodeal.canShow(Appodeal.REWARDED_VIDEO))
            {
                Appodeal.show(Appodeal.REWARDED_VIDEO);
            }
        }

        public void ShowBanner()
        {
            Appodeal.show(Appodeal.BANNER_TOP);
        }

        public void HideBanner()
        {
            Appodeal.hide(Appodeal.BANNER);
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Appodeal.onResume();
            }
        }

#region Rewarded Video callback handlers
        public void onRewardedVideoLoaded() { Debug.Log("Rewarded Video loaded"); }
        public void onRewardedVideoFailedToLoad() { Debug.Log("Rewarded Video failed to load"); }
        public void onRewardedVideoShown() { Debug.Log("Rewarded Video opened"); }
        public void onRewardedVideoClosed(bool isFinished)
		{
			Debug.Log("Rewarded Video closed, finished:" + isFinished);
			if (onRewardedVideoClosedEvent != null)
				onRewardedVideoClosedEvent();
		}
        public void onRewardedVideoFinished(int amount, string name)
        {
            if (onRewardedVideoFinishedEvent != null)
                onRewardedVideoFinishedEvent();
        }
#endregion


    }
}