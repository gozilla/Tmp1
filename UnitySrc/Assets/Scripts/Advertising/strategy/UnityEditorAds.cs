using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace Advertising
{
    public class UnityEditorAds : MonoBehaviour,  IRewardedVideoAdListener, IAdvertising
	{
		//string appKey = "";
		public bool InterestialAdsAvailable
		{
			get
			{
				return true;
			}
		}

		public bool RewardedVideoAdsAvailable
		{
			get
			{
				return true;
			}
		}

		public void ShowInterestial()
		{
			if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
				Appodeal.show(Appodeal.INTERSTITIAL);
		}

		void Awake()
        {
            Initialize();
            ShowBanner();
        }

        public void Initialize()
        {
			Debug.Log("Ads Initialize");
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
			Debug.Log("Showing INTERSTITIAL");
			if (OnRewardedVideoFinishedEvent != null)
				OnRewardedVideoFinishedEvent();
			if (OnRewardedVideoClosedEvent != null)
				OnRewardedVideoClosedEvent();
		}

        public void ShowRewardedVideo()
        {
			Debug.Log("Showing REWARDED_VIDEO");
			if (OnRewardedVideoFinishedEvent != null)
				OnRewardedVideoFinishedEvent();
			if (OnRewardedVideoClosedEvent != null)
				OnRewardedVideoClosedEvent();
		}

        public void ShowBanner()
        {
			Debug.Log("Showing ShowBanner");
        }

        public void HideBanner()
        {
			Debug.Log("Showing HideBanner");
        }

        void OnApplicationFocus(bool hasFocus)
        {
        }

#region Rewarded Video callback handlers
        public void onRewardedVideoLoaded() { Debug.Log("Rewarded Video loaded"); }
        public void onRewardedVideoFailedToLoad() { Debug.Log("Rewarded Video failed to load"); }
        public void onRewardedVideoShown() { Debug.Log("Rewarded Video opened"); }
        public void onRewardedVideoClosed(bool isFinished) { Debug.Log("Rewarded Video closed, finished:" + isFinished); }
        public void onRewardedVideoFinished(int amount, string name)
        {
            if (onRewardedVideoFinishedEvent != null)
                onRewardedVideoFinishedEvent();
        }
#endregion


    }
}