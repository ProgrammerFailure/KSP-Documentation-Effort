using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;

namespace KSP.Localization;

[Serializable]
public class FontLangSettings
{
	public string langCode;

	public List<TMP_FontAsset> FallbackFonts;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FontLangSettings()
	{
		throw null;
	}
}
