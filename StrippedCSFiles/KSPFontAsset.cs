using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[Serializable]
public sealed class KSPFontAsset
{
	[SerializeField]
	private string name;

	[SerializeField]
	private TMP_FontAsset.FontAssetTypes fontAssetType;

	[SerializeField]
	private FaceInfo fontInfo;

	[SerializeField]
	private TMP_Glyph[] glyphInfoList;

	[SerializeField]
	private KSPKerningPairList kerningInfo;

	[SerializeField]
	private float normalStyle;

	[SerializeField]
	private float normalSpacingOffset;

	[SerializeField]
	private float boldStyle;

	[SerializeField]
	private float boldSpacing;

	[SerializeField]
	private byte italicStyle;

	[SerializeField]
	private byte tabSize;

	[SerializeField]
	private int material;

	[SerializeField]
	private int atlas;

	[SerializeField]
	private KSPFontAssetList fallbackFontAssets;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPFontAsset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPFontAsset(TMP_FontAsset font)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_FontAsset GetFontAsset(UnityEngine.Object[] fontMaterials, UnityEngine.Object[] fontAtlas)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Material GetMaterialFromHash(UnityEngine.Object[] materials)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Texture2D GetAtlasFromHash(UnityEngine.Object[] textures)
	{
		throw null;
	}
}
