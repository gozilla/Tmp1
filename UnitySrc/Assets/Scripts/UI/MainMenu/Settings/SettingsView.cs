using System;

using UnityEngine;

namespace Project.UI.MainMenu
{
	public class SettingsView : UIView
	{
		public event Action OnBackEvent;
		public event Action OnApplyEvent;

		[SerializeField]
		private MainMenuController _mainMenu;

		public MainMenuController MainMenu
		{
			get { return _mainMenu; }
		}

		private void Awake()
		{
		}

		private void OnBtnBackClick ()
		{
			if (OnBackEvent != null )
				OnBackEvent();
		}

		private void OnBtnApplyClick()
		{
			if (OnApplyEvent != null )
				OnApplyEvent();
		}
	}
}

