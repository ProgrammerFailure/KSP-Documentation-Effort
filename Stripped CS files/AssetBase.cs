using System.Collections.Generic;
using ns11;
using UnityEngine;

public class AssetBase : MonoBehaviour
{
	public static AssetBase fetch;

	public GameObject[] prefabs;

	public Texture2D[] textures;

	public Dictionary<string, Texture2D> texturesLookup = new Dictionary<string, Texture2D>();

	public Sprite[] sprites;

	public Dictionary<string, Sprite> spritesLookup = new Dictionary<string, Sprite>();

	public GUISkin[] guiSkins;

	public GameObject[] prefabsAutoSpawn;

	public RDTechTree rdTechTree;

	public static RDTechTree RnDTechTree => fetch.rdTechTree;

	public void Awake()
	{
		if ((bool)fetch)
		{
			Object.Destroy(this);
		}
		else
		{
			fetch = this;
		}
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void Start()
	{
		int i = 0;
		for (int num = textures.Length; i < num; i++)
		{
			if (textures[i] != null)
			{
				texturesLookup.Add(textures[i].name, textures[i]);
			}
		}
		int j = 0;
		for (int num2 = sprites.Length; j < num2; j++)
		{
			if (sprites[j] != null)
			{
				spritesLookup.Add(sprites[j].name, sprites[j]);
			}
		}
		int k = 0;
		for (int num3 = prefabsAutoSpawn.Length; k < num3; k++)
		{
			Object.Instantiate(prefabsAutoSpawn[k]).transform.NestToParent(base.transform);
		}
	}

	public static T GetPrefab<T>(string prefabName) where T : MonoBehaviour
	{
		if (!fetch)
		{
			return null;
		}
		int num = fetch.prefabs.Length;
		GameObject gameObject;
		do
		{
			if (num-- > 0)
			{
				gameObject = fetch.prefabs[num];
				continue;
			}
			return null;
		}
		while (!(gameObject != null) || !(gameObject.name == prefabName));
		return gameObject.GetComponent<T>();
	}

	public static GameObject GetPrefab(string prefabName)
	{
		if (!fetch)
		{
			return null;
		}
		int num = fetch.prefabs.Length;
		GameObject gameObject;
		do
		{
			if (num-- > 0)
			{
				gameObject = fetch.prefabs[num];
				continue;
			}
			return null;
		}
		while (!(gameObject != null) || !(gameObject.name == prefabName));
		return gameObject;
	}

	public static Texture2D GetTexture(string textureName)
	{
		if (!fetch)
		{
			return null;
		}
		if (fetch.texturesLookup.ContainsKey(textureName))
		{
			return fetch.texturesLookup[textureName];
		}
		return null;
	}

	public static Sprite GetSprite(string spriteName)
	{
		if (!fetch)
		{
			return null;
		}
		if (fetch.texturesLookup.ContainsKey(spriteName))
		{
			return fetch.spritesLookup[spriteName];
		}
		return null;
	}

	public static GUISkin GetGUISkin(string skinName)
	{
		if (!fetch)
		{
			return null;
		}
		int num = fetch.guiSkins.Length;
		GUISkin gUISkin;
		do
		{
			if (num-- > 0)
			{
				gUISkin = fetch.guiSkins[num];
				continue;
			}
			return null;
		}
		while (!(gUISkin.name == skinName));
		return gUISkin;
	}
}
