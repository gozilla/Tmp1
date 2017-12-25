using UnityEngine;

namespace Project.UI.InGameMenu
{
	public class HorizontalPanelMovier : MonoBehaviour
	{
		public float speed = 10f;
		public RectTransform _rectTransform = null;
		
		private float EndPosX;
		private float StartPosX;
		private int sign = 0;
		private bool isMoving = false;

		void OnStart()
		{

		}

		void Update()
		{
			if (isMoving)
			{
				if (_rectTransform != null)
				{
					_rectTransform.anchoredPosition += new Vector2(sign * speed, 0);
					float curPosX = _rectTransform.anchoredPosition.x;
					float dist = EndPosX - curPosX;
					if ((sign > 0 & dist <= 0f) || (sign < 0 & dist >= 0f))
					{
						isMoving = false;
					}
				}
				else
				{
					isMoving = false;
				}

			}
			

		}

		public void Movie(float endPos)
		{
			if (_rectTransform != null)
			{
				StartPosX = _rectTransform.anchoredPosition.x;
				EndPosX = endPos;
				sign = (StartPosX - EndPosX) > 0 ? -1 : 1;
				isMoving = true;
			}
		}

	}
}

