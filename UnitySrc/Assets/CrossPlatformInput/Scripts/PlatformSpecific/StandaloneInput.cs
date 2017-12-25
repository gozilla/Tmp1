using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
	public class StandaloneInput : VirtualInput
	{
		public StandaloneInput()
		{
			
		}

		public override float GetAxis(string name, bool raw)
		{
			return raw ? Input.GetAxisRaw(name) : Input.GetAxis(name);
		}


		public override bool GetButton(string name)
		{
			if (m_VirtualButtons.ContainsKey(name))
			{
				return m_VirtualButtons[name].GetButton;
			}
			return Input.GetButton(name);
		}


		public override bool GetButtonDown(string name)
		{
			if (m_VirtualButtons.ContainsKey(name))
			{
				return m_VirtualButtons[name].GetButtonDown;
			}
			return Input.GetButtonDown(name);
		}


		public override bool GetButtonUp(string name)
		{
			if (m_VirtualButtons.ContainsKey(name))
			{
				return m_VirtualButtons[name].GetButtonUp;
			}
			return Input.GetButtonUp(name);
		}


		public override void SetButtonDown(string name)
		{
			if (!m_VirtualButtons.ContainsKey(name))
			{
				AddButton(name);
			}
			m_VirtualButtons[name].Pressed();
		}


		public override void SetButtonUp(string name)
		{
			if (!m_VirtualButtons.ContainsKey(name))
			{
				AddButton(name);
			}
			m_VirtualButtons[name].Released();
		}


		public override void SetAxisPositive(string name)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override void SetAxisNegative(string name)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override void SetAxisZero(string name)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override void SetAxis(string name, float value)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		public override Vector3 MousePosition()
		{
			return Input.mousePosition;
		}

		private void AddButton(string name)
		{
			CrossPlatformInputManager.RegisterVirtualButton(new CrossPlatformInputManager.VirtualButton(name));
		}

	}
}