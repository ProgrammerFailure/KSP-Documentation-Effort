using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("KSP/Part Tools Library")]
public class PartToolsLibrary : MonoBehaviour
{
	public List<PartTools> partPrefabs;

	public string loaderLevelName;

	public bool forceTextureFormat;

	public PartTools.TextureFormat textureFormat = PartTools.TextureFormat.TGA_Smallest;

	public void Awake()
	{
		if (loaderLevelName != string.Empty)
		{
			Object.DontDestroyOnLoad(base.gameObject);
			SceneManager.LoadScene(loaderLevelName);
		}
	}
}
