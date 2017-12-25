using System;

using UnityEngine;
using Project.UI.MainMenu;
using UnityEngine.UI;

namespace Project.UI.InGameMenu
{
	public class EndMenuView : UIView
	{
		public event Action OnQuitEvent;
		public event Action OnNewEvent;

		[SerializeField]
		private MainMenuController _mainMenu = null;

		public MainMenuController MainMenu
		{
			get { return _mainMenu; }
		}

		public Text YourScore = null;
		public Text BestScore = null;

		public GameObject NewScore = null;

		public GameObject Hint1 = null;
		public GameObject Hint2 = null;


		private void Awake()
		{
		}

		public void OnBtnMenuClick()
		{
			if (OnQuitEvent != null )
				OnQuitEvent();
		}

		public void OnBtnNewClick()
		{
			if (OnNewEvent != null)
				OnNewEvent();
		}

	}
}

