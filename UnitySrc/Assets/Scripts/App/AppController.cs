using Project.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Project.App
{
	public class AppController : DontDestroyMonoSingleton<AppController>
	{
		private Camera _mainCamera;
		public Camera MainCamera
		{
			get
			{
				if (_mainCamera == null || !_mainCamera.isActiveAndEnabled)
					_mainCamera = Camera.main;
				return _mainCamera;
			}
			set { _mainCamera = value; }
		}

		public Action OnMainCameraChange = null;

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		private void Start()
		{
			MainCameraChange();
		}

		public void MainCameraChange()
		{
			_mainCamera = AppController.Instance.MainCamera;
			if (OnMainCameraChange != null)
				OnMainCameraChange();
		}
	}
}