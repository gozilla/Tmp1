using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Advertising
{
    public interface IAdvertising
    {

        bool InterestialAdsAvailable
        {
            get;
        }

        bool RewardedVideoAdsAvailable
        {
            get;
        }

        Action OnRewardedVideoFinishedEvent
        {
            get;
            set;
        }

		Action OnRewardedVideoClosedEvent
		{
			get;
			set;
		}

		Action OnRewardedVideoIsNotReadyEvent
        {
            get;
            set;
        }

        void Initialize();
        void ShowBanner();
        void HideBanner();
        void ShowInterestial();
        void ShowRewardedVideo();
    }

}
