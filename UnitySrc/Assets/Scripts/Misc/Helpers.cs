#if !UNITY_WSA
using UnityEngine;
using System;
using System.Reflection;

namespace Utils2
{
	public class Helpers
	{
		public static void SetParentRelative(GameObject attachment, GameObject parent)
		{
			if (attachment == null || parent == null)
				return;
			
			Vector3 localPos = attachment.transform.localPosition;
			Quaternion localRot = attachment.transform.localRotation;
			Vector3 localScale = attachment.transform.localScale;
			attachment.transform.parent = parent.transform;
			attachment.transform.localPosition = localPos;
			attachment.transform.localRotation = localRot;
			attachment.transform.localScale = localScale;
		}
	
		public static GameObject FindGameObject(GameObject parent, string name, bool strongMatches = true)
		{
			foreach (Transform child in parent.transform)
			{
				if (strongMatches ? 
					child.name == name :
					child.name.Contains(name))
					return child.gameObject;
				GameObject found = FindGameObject(child.gameObject, name, strongMatches);
				if (found != null)
					return found;
			}
			return null;
		}
		//public static Vector3 WorldObjectToScreenPos(GameObject go, UIRoot uiRoot)
		//{
		//	var uiCamera = UICamera.FindCameraForLayer(go.layer);
		//	var camera = uiCamera.GetComponent<Camera>();
		//	var pos = camera.WorldToScreenPoint(go.transform.position);

		//	float pixelSizeAdjustment = uiRoot.pixelSizeAdjustment;
		//	float screenSizeX = Screen.width * pixelSizeAdjustment;
		//	float screenSizeY = Screen.height * pixelSizeAdjustment;
		//	pos *= pixelSizeAdjustment;
		//	pos.x = pos.x - screenSizeX * 0.5f;
		//	pos.y = pos.y - screenSizeY * 0.5f;
		//	return pos;
		//}
		
		public static Vector3 ScreenObjectToWorldPos(Vector3 screenPos, Camera Worldcamera, int  height)
		{	
			var pos = Worldcamera.ScreenToWorldPoint(screenPos);
			float posY = pos.y - height;
			float posZ = pos.z;
			posZ = posZ + posY * 2 - posY/4;
			pos = new Vector3(pos.x, height, posZ);
			return pos;
		}
		
		public static void SendMessageForce(GameObject receiver, string methodName, object value)
		{
			if (receiver != null)
			{
				foreach (var component in receiver.GetComponents<Component>())
				{
					var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
					var method = component.GetType().GetMethod(methodName, bindingFlags);
					if (method != null)
					{
						ParameterInfo[] parameters = method.GetParameters();
						object[] actualValues = {};
						if (parameters.Length > 0 && parameters[0].ParameterType == value.GetType())
							actualValues = new object[] { value };
						method.Invoke(component, actualValues);
					}
				}
			}
		}
	}
}
#endif