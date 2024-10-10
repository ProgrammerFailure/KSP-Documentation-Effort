using System;
using System.Collections.Generic;
using UnityEngine;

namespace ns5;

public class IconLoader : MonoBehaviour
{
	[SerializeField]
	public string loaderName = "IconLoader";

	[SerializeField]
	public string iconPath = "Squad/PartList/Icons/";

	[SerializeField]
	public string simpleIconPath = "Squad/PartList/SimpleIcons/";

	[SerializeField]
	public Icon[] publicIcons;

	[SerializeField]
	public Icon[] internalIcons;

	[SerializeField]
	public Icon fallbackIcon;

	[NonSerialized]
	public List<Icon> icons = new List<Icon>();

	public Dictionary<string, Icon> iconDictionary = new Dictionary<string, Icon>();

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
		if (iconPath != string.Empty && GameDatabase.Instance != null)
		{
			List<GameDatabase.TextureInfo> allTexturesInFolder = GameDatabase.Instance.GetAllTexturesInFolder(iconPath);
			for (int j = 0; j < allTexturesInFolder.Count - 1; j += 2)
			{
				LoadIcon(allTexturesInFolder[j], allTexturesInFolder[j + 1].texture, useStockPath: true);
			}
		}
		if (simpleIconPath != string.Empty && GameDatabase.Instance != null)
		{
			List<GameDatabase.TextureInfo> allTexturesInFolder2 = GameDatabase.Instance.GetAllTexturesInFolder(simpleIconPath);
			for (int k = 0; k < allTexturesInFolder2.Count; k++)
			{
				LoadIcon(allTexturesInFolder2[k], null, useStockPath: true);
			}
		}
		for (int l = 0; l < internalIcons.Length; l++)
		{
			if (iconDictionary.ContainsKey(internalIcons[l].GetName()))
			{
				Debug.LogError(loaderName + " An icon with this name is already loaded");
			}
			else
			{
				iconDictionary.Add(internalIcons[l].GetName(), internalIcons[l]);
			}
		}
	}

	public Icon GetIcon(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			Debug.LogError($"Couldn't find loaded texture at path: {name}");
			return fallbackIcon;
		}
		if (iconDictionary.ContainsKey(name))
		{
			return iconDictionary[name];
		}
		GameDatabase.TextureInfo textureInfo = (name.Contains("/") ? null : GameDatabase.Instance.GetTextureInfo(simpleIconPath + name));
		if (textureInfo == null)
		{
			textureInfo = (name.Contains("/") ? null : GameDatabase.Instance.GetTextureInfo(iconPath + name));
		}
		if (textureInfo == null)
		{
			textureInfo = GameDatabase.Instance.GetTextureInfo(name);
		}
		if (textureInfo != null)
		{
			LoadIcon(textureInfo, null, useStockPath: false);
			if (!iconDictionary.ContainsKey(name))
			{
				return fallbackIcon;
			}
			return iconDictionary[name];
		}
		Debug.LogError($"Couldn't find loaded texture at path: {name}");
		return fallbackIcon;
	}

	public void LoadIcon(GameDatabase.TextureInfo texInfo, Texture2D altTexture, bool useStockPath)
	{
		string text = texInfo.name;
		if (useStockPath)
		{
			int num = texInfo.name.LastIndexOf('/') + 1;
			text = texInfo.name.Substring(num, texInfo.name.Length - num);
		}
		Icon icon = new Icon(text, texInfo.texture, (altTexture == null) ? texInfo.texture : altTexture, altTexture == null);
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