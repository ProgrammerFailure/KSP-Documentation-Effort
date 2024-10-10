using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

[Serializable]
public sealed class KSPFontAsset
{
	[SerializeField]
	public string name;

	[SerializeField]
	public TMP_FontAsset.FontAssetTypes fontAssetType;

	[SerializeField]
	public FaceInfo fontInfo;

	[SerializeField]
	public TMP_Glyph[] glyphInfoList;

	[SerializeField]
	public KSPKerningPairList kerningInfo;

	[SerializeField]
	public float normalStyle;

	[SerializeField]
	public float normalSpacingOffset;

	[SerializeField]
	public float boldStyle = 0.75f;

	[SerializeField]
	public float boldSpacing = 7f;

	[SerializeField]
	public byte italicStyle = 35;

	[SerializeField]
	public byte tabSize = 10;

	[SerializeField]
	public int material;

	[SerializeField]
	public int atlas;

	[SerializeField]
	public KSPFontAssetList fallbackFontAssets;

	public KSPFontAsset()
	{
		name = null;
		fontInfo = null;
		glyphInfoList = null;
		kerningInfo = null;
		normalStyle = 0f;
		normalSpacingOffset = 0f;
		boldStyle = 0.75f;
		boldSpacing = 7f;
		italicStyle = 35;
		tabSize = 10;
		kerningInfo = new KSPKerningPairList();
		fallbackFontAssets = new KSPFontAssetList();
	}

	public KSPFontAsset(TMP_FontAsset font)
	{
		name = font.name;
		fontAssetType = font.fontAssetType;
		material = TMP_TextUtilities.GetSimpleHashCode(font.material.name);
		atlas = TMP_TextUtilities.GetSimpleHashCode(font.atlas.name);
		fontInfo = font.fontInfo;
		List<TMP_Glyph> list = (List<TMP_Glyph>)typeof(TMP_FontAsset).GetField("m_glyphInfoList", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(font);
		glyphInfoList = list.ToArray();
		kerningInfo = new KSPKerningPairList();
		kerningInfo.AddRange(font.kerningInfo.kerningPairs);
		normalStyle = font.normalStyle;
		normalSpacingOffset = font.normalSpacingOffset;
		boldStyle = font.boldStyle;
		boldSpacing = font.boldSpacing;
		italicStyle = font.italicStyle;
		tabSize = font.tabSize;
		fallbackFontAssets = new KSPFontAssetList();
		if (font.fallbackFontAssets == null || font.fallbackFontAssets.Count <= 0)
		{
			return;
		}
		foreach (TMP_FontAsset fallbackFontAsset in font.fallbackFontAssets)
		{
			fallbackFontAssets.Add(new KSPFontAsset(fallbackFontAsset));
		}
	}

	public TMP_FontAsset GetFontAsset(UnityEngine.Object[] fontMaterials, UnityEngine.Object[] fontAtlas)
	{
		TMP_FontAsset tMP_FontAsset = ScriptableObject.CreateInstance<TMP_FontAsset>();
		tMP_FontAsset.name = name;
		tMP_FontAsset.atlas = GetAtlasFromHash(fontAtlas);
		tMP_FontAsset.material = GetMaterialFromHash(fontMaterials);
		tMP_FontAsset.material.shader = Shader.Find("TextMeshPro/Distance Field");
		tMP_FontAsset.material.SetFloat("_TextureWidth", tMP_FontAsset.atlas.width);
		tMP_FontAsset.material.SetFloat("_TextureHeight", tMP_FontAsset.atlas.height);
		tMP_FontAsset.material.SetTexture("_MainTex", tMP_FontAsset.atlas);
		tMP_FontAsset.fontAssetType = fontAssetType;
		tMP_FontAsset.AddFaceInfo(fontInfo);
		tMP_FontAsset.AddGlyphInfo(glyphInfoList);
		KerningTable kerningTable = new KerningTable();
		kerningTable.kerningPairs.AddRange(kerningInfo);
		tMP_FontAsset.AddKerningInfo(kerningTable);
		tMP_FontAsset.normalStyle = normalStyle;
		tMP_FontAsset.normalSpacingOffset = normalSpacingOffset;
		tMP_FontAsset.boldStyle = boldStyle;
		tMP_FontAsset.boldSpacing = boldSpacing;
		tMP_FontAsset.italicStyle = italicStyle;
		tMP_FontAsset.tabSize = tabSize;
		tMP_FontAsset.fallbackFontAssets = new List<TMP_FontAsset>();
		tMP_FontAsset.ReadFontDefinition();
		if (fallbackFontAssets != null && fallbackFontAssets.Count > 0)
		{
			foreach (KSPFontAsset fallbackFontAsset in fallbackFontAssets)
			{
				tMP_FontAsset.fallbackFontAssets.Add(fallbackFontAsset.GetFontAsset(fontMaterials, fontAtlas));
			}
		}
		return tMP_FontAsset;
	}

	public Material GetMaterialFromHash(UnityEngine.Object[] materials)
	{
		int num = materials.Length;
		do
		{
			if (num-- <= 0)
			{
				return null;
			}
		}
		while (TMP_TextUtilities.GetSimpleHashCode(materials[num].name) != material);
		return (Material)materials[num];
	}

	public Texture2D GetAtlasFromHash(UnityEngine.Object[] textures)
	{
		int num = textures.Length;
		do
		{
			if (num-- <= 0)
			{
				return null;
			}
		}
		while (TMP_TextUtilities.GetSimpleHashCode(textures[num].name) != atlas);
		return (Texture2D)textures[num];
	}
}
