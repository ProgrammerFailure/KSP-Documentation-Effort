using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KSP.UI.Language;

[Serializable]
public class LanguageDefinition
{
	private string name;

	private string font;

	private Dictionary<string, string> translationDict;

	private Dictionary<string, string> untranslated;

	private bool storeUntranslated;

	public List<DictionarySerializerHelper> translations;

	public bool overwrite;

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string Font
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool StoreUntranslated
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LanguageDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LanguageDefinition(string langName, string fontName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LanguageDefinition(string path, bool localPath = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string Translation(string input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string TranslateWithFormat(string baseString, params object[] args)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddTranslation(string input, string output)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveTranslation(string input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MergeInto(LanguageDefinition parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string GetFullPath(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Serialize(string path, bool localPath = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SerializeUntranslated(string path, bool localPath = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Deserialize(string path, bool localPath = true)
	{
		throw null;
	}
}
