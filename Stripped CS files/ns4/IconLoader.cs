using System;
using System.Collections.Generic;
using UnityEngine;

namespace ns4;

public class IconLoader : MonoBehaviour
{
	[SerializeField]
	public string loaderName = "IconLoader";

	[SerializeField]
	public string simpleIconPath = "Squad/Resources/RDSimpleIcons/";

	[SerializeField]
	public Icon[] publicIcons;

	[SerializeField]
	public Icon[] internalIcons;

	[SerializeField]
	public Icon fallbackIcon;

	[NonSerialized]
	public List<Icon> icons = new List<Icon>();

	public Dictionary<string, Icon> iconDictionary = new Dictionary<string, Icon>();

	public Icon FallbackIcon => fallbackIcon;

	public void Awake()
	{
		SetupIcons();
	}

	public void SetupIcons()
	{
		icons.Add(fallbackIcon);
		iconDictionary.Add(fallbackIcon.GetName(), fallbackIcon);
		for (int i = 0; i < publicIcons.Length; i++)
		{
			icons.Add(publicIcons[i]);
			iconDictionary.Add(publicIcons[i].GetName(), publicIcons[i]);
		}
		if (simpleIconPath != string.Empty && GameDatabase.Instance != null)
		{
			List<GameDatabase.TextureInfo> allTexturesInFolder = GameDatabase.Instance.GetAllTexturesInFolder(simpleIconPath);
			for (int j = 0; j < allTexturesInFolder.Count; j++)
			{
				LoadIcon(allTexturesInFolder[j], useStockPath: true);
			}
		}
		for (int k = 0; k < internalIcons.Length; k++)
		{
			if (iconDictionary.ContainsKey(internalIcons[k].GetName()))
			{
				Debug.LogError(loaderName + " An icon with this name is already loaded");
			}
			else
			{
				iconDictionary.Add(internalIcons[k].GetName(), internalIcons[k]);
			}
		}
	}

	public Icon GetIcon(string name)
	{
		if (iconDictionary.ContainsKey(name))
		{
			return iconDictionary[name];
		}
		GameDatabase.TextureInfo textureInfo = (name.Contains("/") ? null : GameDatabase.Instance.GetTextureInfo(simpleIconPath + name));
		if (textureInfo == null)
		{
			textureInfo = GameDatabase.Instance.GetTextureInfo(name);
		}
		if (textureInfo != null)
		{
			LoadIcon(textureInfo, useStockPath: false);
			if (!iconDictionary.ContainsKey(name))
			{
				return fallbackIcon;
			}
			return iconDictionary[name];
		}
		Debug.LogError($"Couldn't find loaded texture at path: {name}");
		return fallbackIcon;
	}

	public void LoadIcon(GameDatabase.TextureInfo texInfo, bool useStockPath)
	{
		string text = texInfo.name;
		if (useStockPath)
		{
			int num = texInfo.name.LastIndexOf('/') + 1;
			text = texInfo.name.Substring(num, texInfo.name.Length - num);
		}
		Icon icon = new Icon(text, texInfo.texture);
		icons.Add(icon);
		if (iconDictionary.ContainsKey(icon.GetName()))
		{
			Debug.LogWarning(loaderName + " An icon with this name is already loaded : " + icon.GetName());
		}
		else
		{
			iconDictionary.Add(icon.GetName(), icon);
		}
	}
}
