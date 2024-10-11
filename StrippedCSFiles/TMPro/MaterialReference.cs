using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

public struct MaterialReference
{
	public int index;

	public TMP_FontAsset fontAsset;

	public TMP_SpriteAsset spriteAsset;

	public Material material;

	public bool isDefaultMaterial;

	public bool isFallbackMaterial;

	public Material fallbackMaterial;

	public float padding;

	public int referenceCount;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MaterialReference(int index, TMP_FontAsset fontAsset, TMP_SpriteAsset spriteAsset, Material material, float padding)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool Contains(MaterialReference[] materialReferences, TMP_FontAsset fontAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int AddMaterialReference(Material material, TMP_FontAsset fontAsset, MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int AddMaterialReference(Material material, TMP_SpriteAsset spriteAsset, MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
	{
		throw null;
	}
}
