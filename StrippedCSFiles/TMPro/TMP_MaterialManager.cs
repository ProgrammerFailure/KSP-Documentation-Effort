using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro;

public static class TMP_MaterialManager
{
	private class FallbackMaterial
	{
		public int baseID;

		public Material baseMaterial;

		public long fallbackID;

		public Material fallbackMaterial;

		public int count;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FallbackMaterial()
		{
			throw null;
		}
	}

	private class MaskingMaterial
	{
		public Material baseMaterial;

		public Material stencilMaterial;

		public int count;

		public int stencilID;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MaskingMaterial()
		{
			throw null;
		}
	}

	private static List<MaskingMaterial> m_materialList;

	private static Dictionary<long, FallbackMaterial> m_fallbackMaterials;

	private static Dictionary<int, long> m_fallbackMaterialLookup;

	private static List<FallbackMaterial> m_fallbackCleanupList;

	private static bool isFallbackListDirty;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_MaterialManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void OnPreRender(Camera cam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void OnPreRenderCanvas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material GetStencilMaterial(Material baseMaterial, int stencilID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ReleaseStencilMaterial(Material stencilMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material GetBaseMaterial(Material stencilMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material SetStencil(Material material, int stencilID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddMaskingMaterial(Material baseMaterial, Material stencilMaterial, int stencilID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveStencilMaterial(Material stencilMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ReleaseBaseMaterial(Material baseMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearMaterials()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetStencilID(GameObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material GetMaterialForRendering(MaskableGraphic graphic, Material baseMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Transform FindRootSortOverrideCanvas(Transform start)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material GetFallbackMaterial(Material sourceMaterial, Material targetMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddFallbackMaterialReference(Material targetMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveFallbackMaterialReference(Material targetMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CleanupFallbackMaterials()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ReleaseFallbackMaterial(Material fallackMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CopyMaterialPresetProperties(Material source, Material destination)
	{
		throw null;
	}
}
