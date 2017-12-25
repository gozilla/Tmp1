
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Project.UI.MainMenu
{
	public class SettingsController : UIController<SettingsView>
	{

		protected override void OnStart()
		{
			base.OnStart();
			gameObject.SetActive(false);
		}

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBtnBackClick;
			View.OnApplyEvent += OnBtnApplyClick;
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBtnBackClick;
			View.OnApplyEvent -= OnBtnApplyClick;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
		}

		private void OnBtnBackClick()
		{
			if (View.MainMenu != null)
			{
				View.MainMenu.gameObject.SetActive(true);
				gameObject.SetActive(false);
			}
		}

		private void OnBtnApplyClick()
		{
			//gameObject.SetActive(false);
		}

	}
}

