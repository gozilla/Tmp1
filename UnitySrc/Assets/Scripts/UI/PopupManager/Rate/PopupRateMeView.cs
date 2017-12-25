using Advertising;
using Project.App;
using Project.UI.InGameMenu;
using System;

using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainMenu
{
	public class PopupRateMeView : PopupView
	{
		//public event Action OnRateEvent;
		//public event Action OnDontRateEvent;
		public InGameMenuController inGameMenuController;

        private string WindowsStoreID = "5533d6b3-7d83-4daf-bd68-70281376e88a";
        private string AppStoreID = "";


        public PopupRateStar ButtonStar1 = null;
		public PopupRateStar ButtonStar2 = null;
		public PopupRateStar ButtonStar3 = null;
		public PopupRateStar ButtonStar4 = null;
		public PopupRateStar ButtonStar5 = null;


        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            int CountSessions = PlayerPrefs.GetInt("CountSessions");
            PlayerPrefs.SetInt("CountSessions",++CountSessions);

            if ( !PlayerPrefs.HasKey("Rate") && CountSessions % 4 == 0)
            {
                PopupRateMeView popup = PopupManager.Instance.GetRegisteredPopup<PopupRateMeView>();

                popup.gameObject.SetActive(true);
            }
        }

		private void Awake()
		{
			PopupManager.Instance.RegisterPopup(this);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
		}

		public void OnDontRateClick()
		{
            //if (OnDontRateEvent != null)
            //OnDontRateEvent();
            PlayerPrefs.SetInt("Rate", 1);
            Close();
		}

		public void OnRate1Click()
		{
			ButtonStar1.SetState(true);
			ButtonStar2.SetState(false);
			ButtonStar3.SetState(false);
			ButtonStar4.SetState(false);
			ButtonStar5.SetState(false);
			PlayerPrefs.SetInt("Rate", 1);
			Close();
		}

		public void OnRate2Click()
		{
			ButtonStar1.SetState(true);
			ButtonStar2.SetState(true);
			ButtonStar3.SetState(false);
			ButtonStar4.SetState(false);
			ButtonStar5.SetState(false);
			PlayerPrefs.SetInt("Rate", 1);
			Close();
		}

		public void OnRate3Click()
		{
			ButtonStar1.SetState(true);
			ButtonStar2.SetState(true);
			ButtonStar3.SetState(true);
			ButtonStar4.SetState(false);
			ButtonStar5.SetState(false);
			PlayerPrefs.SetInt("Rate", 1);
			Close();
		}

		public void OnRate4Click()
		{
			ButtonStar1.SetState(true);
			ButtonStar2.SetState(true);
			ButtonStar3.SetState(true);
			ButtonStar4.SetState(true);
			ButtonStar5.SetState(false);
            Invoke("OpenStore", 0.2f);
            //Close();
        }

		public void OnRate5Click()
		{
			ButtonStar1.SetState(true);
			ButtonStar2.SetState(true);
			ButtonStar3.SetState(true);
			ButtonStar4.SetState(true);
			ButtonStar5.SetState(true);
            Invoke("OpenStore", 0.2f);
			//Close();
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public override void Close()
		{
			gameObject.SetActive(false);
            if (inGameMenuController.gameObject.activeInHierarchy)
			    inGameMenuController.OnContinueGame();
		}

        public void OpenStore()
        {

#if UNITY_ANDROID
            Application.OpenURL("market://details?id=" + Application.identifier;
#elif UNITY_IOS
            Application.OpenURL("itms-apps://itunes.apple.com/app/id"+AppStoreID);
#elif NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                var uri = new Uri(string.Format("ms-windows-store:reviewapp?appid={0}", WindowsStoreID));
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }, false);
#endif
            PlayerPrefs.SetInt("Rate", 1);
            Invoke("Close",1);
        }
    }
}

