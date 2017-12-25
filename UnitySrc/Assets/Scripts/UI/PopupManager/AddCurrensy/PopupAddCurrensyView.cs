using Advertising;
using Project.App;
using Project.UI.InGameMenu;
using System;

using UnityEngine;

namespace Project.UI.MainMenu
{
	public class PopupAddCurrensyView : PopupView
	{
		public event Action OnAddMoneyEvent;
		public InGameMenuController inGameMenuController;

		private void Awake()
		{
			PopupManager.Instance.RegisterPopup(this);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			AdsManager.Instance.OnRewardedVideoFinishedEvent += OnVideoFinished;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (AdsManager.IsAlive)
				AdsManager.Instance.OnRewardedVideoFinishedEvent -= OnVideoFinished;
		}

		public void OnAddMoneyClick()
		{
			if (OnAddMoneyEvent != null)
				OnAddMoneyEvent();
		}

		public void OnAddMoney100Click()
		{
			AppData.Instance.Gold += 100;
			Close();
		}

		public void OnAddMoney250Click()
		{
			AppData.Instance.Gold += 250;
			Close();
		}

		public void OnAddMoney500Click()
		{
			AppData.Instance.Gold += 500;
			Close();
		}

		public void OnAddMoneyFreeClick()
		{
			AdsManager.Instance.ShowRewardedVideo();
		}

		private void OnVideoFinished()
		{
			AppData.Instance.Gold += 15;
			Close();
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
	}
}

