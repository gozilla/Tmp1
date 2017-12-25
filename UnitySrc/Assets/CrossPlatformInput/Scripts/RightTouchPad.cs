using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	[RequireComponent(typeof(Image))]
	public class RightTouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// Options for which axes to use
		public enum AxisOption
		{
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}


		public enum ControlStyle
		{
			Absolute, // operates from teh center of the image
			Relative, // operates from the center of the initial touch
			Swipe, // swipe to touch touch no maintained center
		}


		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public ControlStyle controlStyle = ControlStyle.Absolute; // control style to use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
		public float Xsensitivity = 1f;
		public float Ysensitivity = 1f;

		public int MovementRange = 100;

		Vector3 m_StartPos;
		Vector2 m_PreviousDelta;
		Vector3 m_JoytickOutput;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input
		bool m_Dragging;
		Vector2 m_PreviousTouchPos; // swipe style control touch


#if !UNITY_EDITOR
    private Vector3 m_Center;
    private Image m_Image;
#else
		Vector3 m_PreviousMouse;
#endif

		void OnEnable()
		{
			CreateVirtualAxes();
		}

        void Start()
        {
#if !UNITY_EDITOR
            m_Image = GetComponent<Image>();
            m_Center = m_Image.transform.position;
#endif
        }

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}

		void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;

			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(value.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(value.y);
			}
		}

		void LateUpdate()
		{
			if (!m_Dragging)
			{
				return;
			}
	
#if UNITY_EDITOR
			Vector2 pointerDelta;
			pointerDelta.x = Input.mousePosition.x - m_PreviousMouse.x;
			pointerDelta.y = Input.mousePosition.y - m_PreviousMouse.y;
			m_PreviousMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

			//UpdateVirtualAxes(new Vector3(pointerDelta.x, pointerDelta.y, 0));
#endif
		}
		
		public void RotatePlayer(BaseEventData data)
		{
			PointerEventData pointerData = data as PointerEventData;

			RotatePlayer(pointerData.delta.x, pointerData.delta.y);
        }

		public void RotatePlayer(float horizontal, float vertical)
		{
			UpdateVirtualAxes(new Vector3( horizontal, vertical, 0.0f));

			if(CrossPlatformInputManager._updateTouchPad != null)
				CrossPlatformInputManager._updateTouchPad(horizontal, vertical);
		}

		private string buttonName = "";

		public void OnPointerDown(PointerEventData data)
		{
			m_Dragging = true;
#if !UNITY_EDITOR
        if (controlStyle != ControlStyle.Absolute )
            m_Center = data.position;
#endif

			if (data.pointerPressRaycast.gameObject.name == "Attack")
				buttonName = "Attack";

			if (data.pointerPressRaycast.gameObject.name == "Reload")
				buttonName = "Reload";

			if (data.pointerPressRaycast.gameObject.name == "NextWeapon")
				buttonName = "SetNextWeapon";

			if (data.pointerPressRaycast.gameObject.name == "Grenade")
				buttonName = "AttackGrenade";

			if (data.pointerPressRaycast.gameObject.name == "Knife")
				buttonName = "AttackKnife";

			if (!string.IsNullOrEmpty(buttonName))
				CrossPlatformInputManager.SetButtonDown(buttonName);

		}

		public void OnPointerUp(PointerEventData data)
		{
			m_Dragging = false;
			UpdateVirtualAxes(Vector3.zero);

			if (!string.IsNullOrEmpty(buttonName))
				CrossPlatformInputManager.SetButtonUp(buttonName);

			buttonName = "";

		}

		void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);

			if (CrossPlatformInputManager.AxisExists(verticalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
		}
	}
}