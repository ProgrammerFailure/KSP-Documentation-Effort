using System.Collections.Generic;
using ns16;
using TMPro;
using UnityEngine;

public class UISkinManager : MonoBehaviour
{
	public List<UISkinDefSO> skins;

	public List<GameObject> prefabs;

	public TMP_FontAsset font;

	public static UISkinManager fetch;

	public static Dictionary<string, UISkinDef> loadedDefs = new Dictionary<string, UISkinDef>();

	public static Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();

	public static UISkinDef defaultSkin;

	public static TMP_FontAsset TMPFont => fetch.font;

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

	public static UISkinDef GetSkin(string name)
	{
		if (!fetch)
		{
			return null;
		}
		if (loadedDefs.ContainsKey(name))
		{
			return loadedDefs[name];
		}
		return defaultSkin;
	}

	public void Start()
	{
		int count = skins.Count;
		for (int i = 0; i < count; i++)
		{
			loadedDefs.Add(skins[i].SkinDef.name, skins[i].SkinDef);
		}
		defaultSkin = GetSkin("KSP window 7");
		count = prefabs.Count;
		for (int j = 0; j < count; j++)
		{
			GameObject gameObject = prefabs[j];
			if (gameObject != null)
			{
				loadedPrefabs.Add(gameObject.name, gameObject);
			}
		}
	}

	public static GameObject GetPrefab(string prefabName)
	{
		if (!fetch)
		{
			return null;
		}
		if (loadedPrefabs.ContainsKey(prefabName))
		{
			return loadedPrefabs[prefabName];
		}
		return new GameObject();
	}
}
