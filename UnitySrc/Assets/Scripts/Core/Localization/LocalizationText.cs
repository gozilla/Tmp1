using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Project.Core.Localization
{
	[DisallowMultipleComponent]
	public class LocalizationText : MonoBehaviour
	{
		protected Text _text;

		private string _localizationKey = "";
		public string LocalizationKey
		{
			set { _localizationKey = value; }
			get { return _localizationKey; }
		}

		private void Start () 
		{
			ChangeLanguageCommand();
			Localization.Instance.AllLocalizedText.Add(this);
		}

		protected virtual string GetKey()
		{
			return _text != null ? _text.text : string.Empty;
		}

		public void ChangeLanguageCommand()
		{
			if (_text == null)
				_text = GetComponent<Text>();

			if (_text != null)
			{
				if(string.IsNullOrEmpty( _localizationKey ))
					_localizationKey = GetKey();

				Localization.Instance.LocalizeText(_text, _localizationKey, _localizationKey);
			}
		}

		public void ReLocalize()
		{
			if (_text == null)
				_text = GetComponent<Text>();

			if (_text != null)
			{
				_localizationKey = GetKey();
				Localization.Instance.LocalizeText(_text, _localizationKey, _localizationKey);
			}
		}
	}
}
