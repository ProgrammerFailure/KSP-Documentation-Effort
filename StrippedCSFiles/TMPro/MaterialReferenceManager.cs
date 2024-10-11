using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

public class MaterialReferenceManager
{
	private static MaterialReferenceManager s_Instance;

	private Dictionary<int, Material> m_FontMaterialReferenceLookup;

	private Dictionary<int, TMP_FontAsset> m_FontAssetReferenceLookup;

	private Dictionary<int, TMP_SpriteAsset> m_SpriteAssetReferenceLookup;

	private Dictionary<int, TMP_ColorGradient> m_ColorGradientReferenceLookup;

	public static MaterialReferenceManager instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MaterialReferenceManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddFontAsset(TMP_FontAsset fontAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddFontAssetInternal(TMP_FontAsset fontAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddSpriteAsset(TMP_SpriteAsset spriteAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddSpriteAssetInternal(TMP_SpriteAsset spriteAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddSpriteAsset(int hashCode, TMP_SpriteAsset spriteAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddSpriteAssetInternal(int hashCode, TMP_SpriteAsset spriteAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddFontMaterial(int hashCode, Material material)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddFontMaterialInternal(int hashCode, Material material)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddColorGradientPreset(int hashCode, TMP_ColorGradient spriteAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddColorGradientPreset_Internal(int hashCode, TMP_ColorGradient spriteAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(TMP_FontAsset font)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(TMP_SpriteAsset sprite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryGetFontAsset(int hashCode, out TMP_FontAsset fontAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TryGetFontAssetInternal(int hashCode, out TMP_FontAsset fontAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryGetSpriteAsset(int hashCode, out TMP_SpriteAsset spriteAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TryGetSpriteAssetInternal(int hashCode, out TMP_SpriteAsset spriteAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryGetColorGradientPreset(int hashCode, out TMP_ColorGradient gradientPreset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TryGetColorGradientPresetInternal(int hashCode, out TMP_ColorGradient gradientPreset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryGetMaterial(int hashCode, out Material material)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TryGetMaterialInternal(int hashCode, out Material material)
	{
		throw null;
	}
}
