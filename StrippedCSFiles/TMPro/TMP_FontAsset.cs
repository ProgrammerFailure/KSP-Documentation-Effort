using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public class TMP_FontAsset : TMP_Asset
{
	public enum FontAssetTypes
	{
		None,
		SDF,
		Bitmap
	}

	private static TMP_FontAsset s_defaultFontAsset;

	public FontAssetTypes fontAssetType;

	[SerializeField]
	private FaceInfo m_fontInfo;

	[SerializeField]
	public Texture2D atlas;

	[SerializeField]
	private List<TMP_Glyph> m_glyphInfoList;

	private Dictionary<int, TMP_Glyph> m_characterDictionary;

	private Dictionary<int, KerningPair> m_kerningDictionary;

	[SerializeField]
	private KerningTable m_kerningInfo;

	[SerializeField]
	private KerningPair m_kerningPair;

	[SerializeField]
	public List<TMP_FontAsset> fallbackFontAssets;

	[SerializeField]
	public FontCreationSetting fontCreationSettings;

	[SerializeField]
	public TMP_FontWeights[] fontWeights;

	private int[] m_characterSet;

	public float normalStyle;

	public float normalSpacingOffset;

	public float boldStyle;

	public float boldSpacing;

	public byte italicStyle;

	public byte tabSize;

	private byte m_oldTabSize;

	public static TMP_FontAsset defaultFontAsset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public FaceInfo fontInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Dictionary<int, TMP_Glyph> characterDictionary
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Dictionary<int, KerningPair> kerningDictionary
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public KerningTable kerningInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_FontAsset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddFaceInfo(FaceInfo faceInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddGlyphInfo(TMP_Glyph[] glyphInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddKerningInfo(KerningTable kerningTable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReadFontDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SortGlyphs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasCharacter(int character)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasCharacter(char character)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasCharacter(char character, bool searchFallbacks)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasCharacter_Internal(char character, bool searchFallbacks)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasCharacters(string text, out List<char> missingCharacters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasCharacters(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetCharacters(TMP_FontAsset fontAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int[] GetCharactersArray(TMP_FontAsset fontAsset)
	{
		throw null;
	}
}
