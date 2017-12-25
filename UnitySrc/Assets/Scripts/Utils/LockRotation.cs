using Project.App;
using System;

using UnityEngine;

namespace Project.Utils
{
	public class LockRotation : MonoBehaviour
	{
		[SerializeField]
		private bool _lockCameraAngle;

		[SerializeField]
		private Vector3 _lockAngles;

		private Transform _cachedTransform;

		private Transform _cachedCameraTransform;
		private Transform _cameraTransform
		{
			get
			{
				if ( _cachedCameraTransform == null && AppController.Instance.MainCamera != null )
					_cachedCameraTransform = AppController.Instance.MainCamera.transform;

				return _cachedCameraTransform;
			}
		}


		private void Start ()
		{
			_cachedTransform = transform;
		}

		private void Update ()
		{
			if ( _cameraTransform == null )
				return;

			if ( _lockCameraAngle )
				_cachedTransform.eulerAngles = _cameraTransform.eulerAngles;
			else
				_cachedTransform.eulerAngles = _lockAngles;
		}
	}
}

