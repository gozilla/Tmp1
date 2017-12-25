using UnityEngine;
using System.Collections;

namespace Project.Core.Localization
{
	[DisallowMultipleComponent]
	public class LocalizationTextMesh : MonoBehaviour
	{
		protected TextMesh _textMesh;

		private string _localizationKey = "";

		private void Start () 
		{
			ChangeLanguageCommand();
			Localization.Instance.AllLocalizedTextMeshes.Add(this);
		}

		protected virtual string GetKey()
		{
			return _textMesh != null ? _textMesh.text : string.Empty;
		}

		public void ChangeLanguageCommand()
		{
			if (_textMesh == null)
				_textMesh = GetComponent<TextMesh>();

			if (_textMesh != null)
			{
				if(string.IsNullOrEmpty( _localizationKey ))
					_localizationKey = GetKey();

                Localization.Instance.LocalizeTextMesh(_textMesh, _localizationKey, _localizationKey);
			}
		}
	}
}
