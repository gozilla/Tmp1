using UnityEngine;
using UnityEngine.UI;

namespace Project.App
{
	public class CurrensyController : MonoBehaviour
	{
		public Text GoldText = null;

		void OnEnable()
		{
			AppData.Instance.OnGoldChangeEvent += OnGoldChange;
			GoldText.text = AppData.Instance.Gold.ToString();
		}

		void OnDisable()
		{
			if (AppData.IsAlive)
			{
				AppData.Instance.OnGoldChangeEvent -= OnGoldChange;
			}
		}

		private void OnGoldChange(uint gold)
		{
			GoldText.text = gold.ToString();
		}

	}
}

