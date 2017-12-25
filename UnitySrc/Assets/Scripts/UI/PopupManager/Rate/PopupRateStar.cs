
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainMenu
{
	public class PopupRateStar:MonoBehaviour
	{
		public Image ImageOn = null;
		public Image ImageOff = null;

		public void SetState(bool state)
		{
			ImageOn.gameObject.SetActive(state);
			ImageOff.gameObject.SetActive(!state);
		}
	}
}

