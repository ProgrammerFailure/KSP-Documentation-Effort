using System;
using System.Collections.Generic;
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
	}

	public List<MaterialSwapInformation> materialSwaps;

	public MeshRenderer meshRenderer;

	public bool updateWhenGameSettingsApplied;

	public void Awake()
	{
		if (materialSwaps.Count > 0)
		{
			ApplyMaterials();
			if (updateWhenGameSettingsApplied)
			{
				GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
			}
		}
	}

	public void OnDestroy()
	{
		if (updateWhenGameSettingsApplied)
		{
			GameEvents.OnGameSettingsApplied.Remove(OnGameSettingsApplied);
		}
	}

	public void OnGameSettingsApplied()
	{
		if (HighLogic.LoadedScene == GameScenes.SETTINGS)
		{
			ApplyMaterials();
		}
	}

	public void ApplyMaterials()
	{
		if (materialSwaps.Count <= 0)
		{
			return;
		}
		if (meshRenderer == null)
		{
			meshRenderer = GetComponent<MeshRenderer>();
		}
		if (!(meshRenderer != null))
		{
			return;
		}
		Material[] materials = meshRenderer.materials;
		for (int i = 0; i < materialSwaps.Count; i++)
		{
			if (materialSwaps[i].materialToBeSwappedIndex < materials.Length)
			{
				if (GameSettings.TERRAIN_SHADER_QUALITY >= 3 && materialSwaps[i].ultraQualityMaterial != null)
				{
					materials[materialSwaps[i].materialToBeSwappedIndex] = materialSwaps[i].ultraQualityMaterial;
				}
				else if (GameSettings.TERRAIN_SHADER_QUALITY >= 2 && materialSwaps[i].highQualityMaterial != null)
				{
					materials[materialSwaps[i].materialToBeSwappedIndex] = materialSwaps[i].highQualityMaterial;
				}
				else if (GameSettings.TERRAIN_SHADER_QUALITY >= 1 && materialSwaps[i].mediumQualityMaterial != null)
				{
					materials[materialSwaps[i].materialToBeSwappedIndex] = materialSwaps[i].mediumQualityMaterial;
				}
				else if (materialSwaps[i].lowQualityMaterial != null)
				{
					materials[materialSwaps[i].materialToBeSwappedIndex] = materialSwaps[i].lowQualityMaterial;
				}
			}
		}
		meshRenderer.materials = materials;
	}
}
