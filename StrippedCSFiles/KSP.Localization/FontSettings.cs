using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;

namespace KSP.Localization;

[Serializable]
public class FontSettings
{
	public TMP_FontAsset Font;

	public List<FontLangSettings> LanguageSettings;

	public FontLangSettings this[string langCode]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FontSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddSubFont(string langCode, bool append, params TMP_FontAsset[] fonts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanSubFonts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ChangeLanguage(string langCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ChangeLanguage()
	{
		throw null;
	}
}
