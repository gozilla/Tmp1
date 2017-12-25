using System;
using TouchControlsKit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class LeftTouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

		public int MovementRange = 100; // for 800x600

		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		
public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis

		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

		void OnEnable()
		{
			CreateVirtualAxes();
		}

		void UpdateVirtualAxes(Vector3 value)
		{
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(value.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(value.y);
			}
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


		void Update()
		{
			Vector3 axes = Vector3.zero;		

			if (m_UseX)
			{
				axes.x = TCKInput.GetAxis("Joystick", EAxisType.Horizontal);
			}

			if (m_UseY)
			{
				axes.y = TCKInput.GetAxis("Joystick", EAxisType.Vertical);
			}
			
			UpdateVirtualAxes(axes);
		}

		private string buttonName = "";

		public void OnPointerDown(PointerEventData data)
		{
			if (!string.IsNullOrEmpty(buttonName))
				CrossPlatformInputManager.SetButtonDown(buttonName);

			buttonName = "";
		}


		public void OnPointerUp(PointerEventData data)
		{
			//_touchPad.position = m_StartPos;
			UpdateVirtualAxes(Vector3.zero);
		}


		void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
			}
		}
	}
}