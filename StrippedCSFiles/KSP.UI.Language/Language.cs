using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace KSP.UI.Language;

public class Language : MonoBehaviour
{
	public class FontData
	{
		public string name;

		public string filePath;

		public string assetPath;

		public AssetBundle bundle;

		public Font font;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FontData()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadBundle_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AssetBundle assetBundle;

		public string filePath;

		public Callback<AssetBundle, FontData, string, bool> onLoaded;

		public FontData font;

		private string _003Curl_003E5__2;

		private UnityWebRequest _003Cuwr_003E5__3;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CLoadBundle_003Ed__15(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	private static Dictionary<string, LanguageDefinition> allLanguages;

	private static Dictionary<string, FontData> allFonts;

	private static string languageName;

	private static string fontName;

	private static bool allFontsActive;

	private static LanguageDefinition language;

	private static Language _instance;

	private List<string> fontBundles;

	private bool clearLanguages;

	public static Language Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Language()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Language()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetLanguage(string langName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSetLanguage(AssetBundle bundle, FontData font, string filePath, bool success)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadBundle_003Ed__15))]
	private static IEnumerator LoadBundle(AssetBundle assetBundle, FontData font, string filePath, Callback<AssetBundle, FontData, string, bool> onLoaded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetLanguage(bool translate = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string Translate(string input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string TranslateWithFormat(string baseString, params object[] args)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> LoadedLanguages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> LoadedFonts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateText(Text text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFont(Text text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateAllFonts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onActivateAllFonts(AssetBundle bundle, FontData font, string filePath, bool success)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeactivateAllFonts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string LanguageName(string language, out Font newFont)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadLanguage(string path, bool localPath = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadLanguages(bool clear = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLoadLanguages(AssetBundle bundle, FontData font, string filePath, bool success)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string Save()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveLanguages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveUntranslated()
	{
		throw null;
	}
}
