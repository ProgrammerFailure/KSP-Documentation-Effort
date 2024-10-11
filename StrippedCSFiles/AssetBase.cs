using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;

public class AssetBase : MonoBehaviour
{
	private static AssetBase fetch;

	public GameObject[] prefabs;

	public Texture2D[] textures;

	private Dictionary<string, Texture2D> texturesLookup;

	public Sprite[] sprites;

	private Dictionary<string, Sprite> spritesLookup;

	public GUISkin[] guiSkins;

	public GameObject[] prefabsAutoSpawn;

	public RDTechTree rdTechTree;

	public static RDTechTree RnDTechTree
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AssetBase()
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
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T GetPrefab<T>(string prefabName) where T : MonoBehaviour
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameObject GetPrefab(string prefabName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D GetTexture(string textureName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Sprite GetSprite(string spriteName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GUISkin GetGUISkin(string skinName)
	{
		throw null;
	}
}
