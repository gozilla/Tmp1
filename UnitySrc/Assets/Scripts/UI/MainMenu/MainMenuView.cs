using Project.UI.InGameMenu;
using System;

using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainMenu
{
	public class MainMenuView : UIView
	{
		public event Action OnStartEvent;
		public event Action OnSwitchLangEvent;
		public event Action OnAddMoneyEvent;
		public event Action<uint> OnGameOptionEvent;
		public Action OnRemoveAdsClickEvent;

        [SerializeField]
		private InGameMenuController _inGameMenuController;

		public InGameMenuController InGameMenuController
		{
			get { return _inGameMenuController; }
		}

		[SerializeField]
		private GameObject[] _langRu;

		public GameObject[] LangRu
		{
			get { return _langRu; }
		}
		

		[SerializeField]
		private GameObject[] _langEng;

		public GameObject[] LangEng
		{
			get { return _langEng; }
		}

		[SerializeField]
		private Loading _loading;

		public Loading Loading
		{
			get { return _loading; }
		}

		[SerializeField]
		private Button _removeAdsBtn;

		public Button RemoveAdsBtn
		{
			get { return _removeAdsBtn; }
		}

		public GameObject InactiveAdsBtn = null;

		[SerializeField]
		private RectTransform _startBtnRect;

		public RectTransform StartBtnRect
		{
			get { return _startBtnRect; }
		}

		public RectTransform LangPanel = null;
		public RectTransform MoneyPanel = null;

		public PopupRateStar ButtonOption1 = null;
		public PopupRateStar ButtonOption2 = null;
		public PopupRateStar ButtonOption3 = null;

		private void Awake()
		{
		}

		public void OnBtnStartClick()
		{
			if (OnStartEvent != null )
				OnStartEvent();
		}

		public void OnSwitchLangClick()
		{
			if (OnSwitchLangEvent != null)
				OnSwitchLangEvent();
		}

		public void OnAddMoneyClick()
		{
			if (OnAddMoneyEvent != null)
				OnAddMoneyEvent();
		}

        public void OnRemoveAdsClick()
        {
            if (OnRemoveAdsClickEvent != null)
                OnRemoveAdsClickEvent();
        }

		public void OnOption1Click()
		{
			if (OnGameOptionEvent != null)
				OnGameOptionEvent(1);
		}

		public void OnOption2Click()
		{
			if (OnGameOptionEvent != null)
				OnGameOptionEvent(2);
		}

		public void OnOption3Click()
		{
			if (OnGameOptionEvent != null)
				OnGameOptionEvent(3);
		}

	}
}

