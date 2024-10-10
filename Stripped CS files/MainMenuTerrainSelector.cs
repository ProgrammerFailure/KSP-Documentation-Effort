using System.Collections.Generic;
using UnityEngine;

public class MainMenuTerrainSelector : MonoBehaviour
{
	public List<GameObject> lowQualityTerrainShaderDisplayObjects = new List<GameObject>();

	public List<GameObject> mediumQualityTerrainShaderDisplayObjects = new List<GameObject>();

	public List<GameObject> highQualityTerrainShaderDisplayObjects = new List<GameObject>();

	public void Start()
	{
		List<GameObject> list = null;
		list = ((GameSettings.TERRAIN_SHADER_QUALITY <= 0) ? lowQualityTerrainShaderDisplayObjects : ((GameSettings.TERRAIN_SHADER_QUALITY != 1) ? highQualityTerrainShaderDisplayObjects : mediumQualityTerrainShaderDisplayObjects));
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetActive(value: true);
		}
	}
}
