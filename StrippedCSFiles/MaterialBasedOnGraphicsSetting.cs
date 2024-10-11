using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaterialBasedOnGraphicsSetting : MonoBehaviour
{
	[Serializable]
	public class MaterialSwapInformation
	{
		public int materialToBeSwappedIndex;

		public Material lowQualityMaterial;

		public Material mediumQualityMaterial;

		public Material highQualityMaterial;

		public Material ultraQualityMaterial;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MaterialSwapInformation()
		{
			throw null;
		}
	}

	public List<MaterialSwapInformation> materialSwaps;

	private MeshRenderer meshRenderer;

	public bool updateWhenGameSettingsApplied;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MaterialBasedOnGraphicsSetting()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSettingsApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyMaterials()
	{
		throw null;
	}
}
