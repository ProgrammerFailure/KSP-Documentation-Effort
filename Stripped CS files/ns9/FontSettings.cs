using System;
using System.Collections.Generic;
using TMPro;

namespace ns9;

[Serializable]
public class FontSettings
{
	public TMP_FontAsset Font;

	public List<FontLangSettings> LanguageSettings;

	public FontLangSettings this[string langCode]
	{
		get
		{
			int count = LanguageSettings.Count;
			do
			{
				if (count-- <= 0)
				{
					return null;
				}
			}
			while (!langCode.Equals(LanguageSettings[count].langCode));
			return LanguageSettings[count];
		}
	}

	public FontSettings()
	{
		Font = null;
		LanguageSettings = new List<FontLangSettings>();
	}

	public void AddSubFont(string langCode, bool append, params TMP_FontAsset[] fonts)
	{
		FontLangSettings fontLangSettings = this[langCode];
		if (fontLangSettings == null)
		{
			fontLangSettings = new FontLangSettings();
			fontLangSettings.langCode = langCode;
			LanguageSettings.Add(fontLangSettings);
		}
		if (!append)
		{
			fontLangSettings.FallbackFonts.Clear();
		}
		fontLangSettings.FallbackFonts.AddRange(fonts);
	}

	public void CleanSubFonts()
	{
		LanguageSettings.Clear();
		Font.fallbackFontAssets.Clear();
	}

	public bool ChangeLanguage(string langCode)
	{
		FontLangSettings fontLangSettings = this[langCode];
		if (fontLangSettings == null)
		{
			return false;
		}
		Font.fallbackFontAssets.Clear();
		Font.fallbackFontAssets.AddRange(fontLangSettings.FallbackFonts);
		return true;
	}

	public bool ChangeLanguage()
	{
		Font.fallbackFontAssets.Clear();
		int count = LanguageSettings.Count;
		while (count-- > 0)
		{
			Font.fallbackFontAssets.AddRange(LanguageSettings[count].FallbackFonts);
		}
		return true;
	}
}
