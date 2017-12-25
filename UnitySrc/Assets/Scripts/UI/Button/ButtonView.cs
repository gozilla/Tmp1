using System;

using UnityEngine;
using Project.UI.MainMenu;
using UnityEngine.UI;

namespace Project.UI.InGameMenu
{
	public class ButtonView : UIView
	{
		//public event Action OnNewEvent;

		public Button Button = null;
		public GameObject BgInactive = null;


		private void Awake()
		{
		}


		//public void OnBtnNewClick()
		//{
		//	if (OnNewEvent != null)
		//		OnNewEvent();
		//}

	}
}

