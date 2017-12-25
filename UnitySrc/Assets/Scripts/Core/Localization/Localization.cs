using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

using Newtonsoft.Json;
using Project.Utils;
using UnityEngine.UI;

namespace Project.Core.Localization
{
	public class LocalizeRules
	{
		public Dictionary<TextAlignment, TextAlignment> alignments = new Dictionary<TextAlignment, TextAlignment>();
		public Dictionary<string, string> fonts = new Dictionary<string, string>();

		public LocalizeRules() { }
	}

	public class KeyValueLocalize
	{
		public LocalizeRules rules = new LocalizeRules();

		public Dictionary<string, string> localizeItems = new Dictionary<string, string>();

		public KeyValueLocalize() { }
	}

	public class LocalizeLanguages
	{
		public Dictionary<string, string> fonts = new Dictionary<string, string>();
		public Dictionary<string, string> fontsCustomMaterials = new Dictionary<string, string>();

		public Dictionary<string, string> locales = new Dictionary<string, string>();
		public Dictionary<string, KeyValueLocalize> localization = new Dictionary<string, KeyValueLocalize>();

		public LocalizeLanguages() { }
	}

	[AppSingleton]
	public class Localization : MonoSingleton<Localization>
	{
		public static readonly string EnglishLocale = "en";
		public static readonly string RussianLocale = "ru";
		public static readonly string EnglishLanguage = "English";

		private LocalizeLanguages localizations = null;
		private KeyValueLocalize currentLocalization = null;
		private ResourceLoader<Font> fonts = new ResourceLoader<Font>();
		private ResourceLoader<Material> fontMaterials = new ResourceLoader<Material>();

		private string _locale = EnglishLocale;
		private string _language = EnglishLanguage;
		//private bool _arabic = false;

		public List<LocalizationText> AllLocalizedText = new List<LocalizationText>();
		public List<LocalizationTextMesh> AllLocalizedTextMeshes = new List<LocalizationTextMesh>();

		public string Language
		{
			get { return _language; }
		}

		public string Locale
		{
			get { return _locale; }
		}

		public string this[string key]
		{
			get { return GetValueByKey(key); }
		}

		protected override void OnCreate()
		{
			Load();

			ChangeLanguage(true);

			//Debug.Log("Localization::OnCreate currentLanguage " + _language);
		}

		public void ChangeLanguage(bool isLaunch = false)
		{
			if (!PlayerPrefs.HasKey(StoredInPlayerPrefsParameters.currentLanguage))
				_locale = GetCurrentLocale();
			else
				_locale = PlayerPrefs.GetString(StoredInPlayerPrefsParameters.currentLanguage);

			#region This is fucking awesome
			if (!isLaunch)
			{
				string[] availableLanguages = new string[localizations.localization.Keys.Count];
				localizations.localization.Keys.CopyTo(availableLanguages, 0);

				int languageNumber = 0;

				if (localizations.locales.ContainsKey(_locale))
				{
					for (int i = 0; i < availableLanguages.Length; i++)
					{
						if (availableLanguages[i].ToLower() == localizations.locales[_locale].ToLower())
						{
							languageNumber = i;
							break;
						}
					}
				}

				languageNumber++;

				languageNumber = languageNumber % availableLanguages.Length;

				string newLanguage = availableLanguages[languageNumber];
				foreach (var keyValue in localizations.locales)
				{
					if (keyValue.Value == newLanguage)
					{
						_locale = keyValue.Key;
						break;
					}
				}
			}
#endregion

			PlayerPrefs.SetString(StoredInPlayerPrefsParameters.currentLanguage, _locale);

			_language = LocaleToLanguage(_locale);

			if (localizations != null)
			{
				currentLocalization = localizations.localization[_language];

				if (localizations.fonts != null && localizations.fonts.Count == 0)
					fonts.Add(localizations.fonts);

				if (localizations.fontsCustomMaterials != null && localizations.fontsCustomMaterials.Count == 0)
					fontMaterials.Add(localizations.fontsCustomMaterials);
			}

			for (int i = 0; i < AllLocalizedTextMeshes.Count; i++)
				AllLocalizedTextMeshes[i].ChangeLanguageCommand();

			for (int i = 0; i < AllLocalizedText.Count; i++)
				AllLocalizedText[i].ChangeLanguageCommand();
		}

		private string GetCurrentLocale()
		{
			string locale = string.Empty;

			try
			{
				locale = Application.systemLanguage == SystemLanguage.Russian ? RussianLocale : EnglishLocale;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}

			if (string.IsNullOrEmpty(locale))
				locale = EnglishLocale;

			return locale.ToLower();
		}

		private string LocaleToLanguage(string locale)
		{
			string language = EnglishLanguage;

			//if (localizations != null && localizations.locales != null)
			{
				if (localizations.locales.ContainsKey(locale))
					language = localizations.locales[locale];
				else
				{
					if (locale.Length > 2)
					{
						string _tempLocale = locale.Substring(0, 2);

						if (localizations.locales.ContainsKey(_tempLocale))
							language = localizations.locales[_tempLocale];
						else
							Debug.LogWarning("Language not Found " + locale);
					}
				}
			}


			return language;
		}

		private void Load()
		{
			TextAsset txt = (TextAsset)Resources.Load("Localization/GameLocalize", typeof(TextAsset));
			if (txt != null)
			{
				localizations = JsonConvert.DeserializeObject<LocalizeLanguages>(txt.text);
			}
		}

		public bool ChangeFontByRule(string fontName, out string newFont)
		{
			if (currentLocalization != null && currentLocalization.rules != null)
			{
				newFont = "";
				var fonts = currentLocalization.rules.fonts;

				return fonts != null && fonts.TryGetValue(fontName, out newFont);
			}

			newFont = fontName;

			return false;
		}

		public bool ChangeAlignmentByRule(TextAlignment alignment, out TextAlignment newAlignment)
		{
			if (currentLocalization != null && currentLocalization.rules != null)
			{
				newAlignment = new TextAlignment();
				var alignments = currentLocalization.rules.alignments;

				return alignments != null && alignments.TryGetValue(alignment, out newAlignment);
			}

			newAlignment = alignment;

			return false;
		}

		public string GetValueByKey(string key)
		{
			return GetValueByKey(key, "", false);
		}

		public string GetValueByKey(string key, string defaultValue)
		{
			return GetValueByKey(key, defaultValue, false);
		}

		public string GetValueByKey(string key, string defaultValue, bool ignoreArabicFormat)
		{
			if (currentLocalization != null && currentLocalization.localizeItems != null)
			{
				string value = string.Empty;

				if (currentLocalization.localizeItems.TryGetValue(key, out value))
				{
					//if (ignoreArabicFormat)
						return value;
					//else
					//	return _arabic ? ArabicFixer.Fix(value, false, false) : value;
				}
			}

			return defaultValue;
		}

		public void LocalizeTextMesh(TextMesh textMesh, string key, string defaultValue = "")
		{
			if (textMesh != null)
			{
				textMesh.text = GetValueByKey(key, defaultValue);

				ConfigTextMesh(textMesh);
			}
		}

		public void LocalizeText(Text text, string key, string defaultValue = "")
		{
			if (text != null)
			{
				text.text = GetValueByKey(key, defaultValue);
			}
		}

		public void ConfigTextMesh(TextMesh textMesh)
		{
			if (textMesh != null)
			{
				string newFont = string.Empty;
				if (ChangeFontByRule(textMesh.font.name, out newFont))
				{
					if (!string.IsNullOrEmpty(newFont) && textMesh.font.name != newFont)
					{
						Font font = fonts[newFont];

						if (font != null)
						{
							Renderer renderer = textMesh.gameObject.GetComponent<Renderer>();
							Material prevFontMaterial = textMesh.font.material;
							Material material = font.material;

							if (prevFontMaterial.name != renderer.material.name)  //check custom material
							{
								Material customMaterial = fontMaterials[newFont];

								if (customMaterial != null)
									material = customMaterial;
							}

							textMesh.font = font;
							renderer.material = material;
						}
					}
				}

				TextAlignment alignment = TextAlignment.Center;
				if (ChangeAlignmentByRule(textMesh.alignment, out alignment))
				{
					textMesh.alignment = alignment;
				}
			}
		}

		public static string GetValue(string key, string defaultValue = "")
		{
			return Instance.GetValueByKey(key, defaultValue);
		}
	}
}
