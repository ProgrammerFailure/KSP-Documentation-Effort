using System;
using System.Collections.Generic;
using TMPro;

namespace ns9;

[Serializable]
public class FontLangSettings
{
	public string langCode;

	public List<TMP_FontAsset> FallbackFonts;

	public FontLangSettings()
	{
		langCode = "en-us";
		FallbackFonts = new List<TMP_FontAsset>();
	}
}
