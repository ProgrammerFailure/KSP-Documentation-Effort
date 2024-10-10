using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FontLoader : LoadingSystem
{
	[SerializeField]
	public FontSettings GameFontSettings;

	[SerializeField]
	public FontSettings MenuFontSettings;

	public bool isReady;

	public string progressTitle = "";

	public float progressFraction;

	public List<TMP_FontAsset> LoadedFonts { get; set; }

	public override bool IsReady()
	{
		return isReady;
	}

	public override string ProgressTitle()
	{
		return progressTitle;
	}

	public override float ProgressFraction()
	{
		return progressFraction;
	}

	public override void StartLoad()
	{
		if (isReady)
		{
			Debug.LogError("PartLoader: Already started and finished. Aborting.");
			return;
		}
		LoadedFonts = new List<TMP_FontAsset>();
		StartCoroutine(LoadFonts());
	}

	public override float LoadWeight()
	{
		return 0.01f;
	}

	public void SwitchFontLanguage(string langCode)
	{
		MenuFontSettings.ChangeLanguage(langCode);
	}

	public void AddGameSubFont(string langCode, bool append, params TMP_FontAsset[] fonts)
	{
		GameFontSettings.AddSubFont(langCode, append, fonts);
		GameFontSettings.ChangeLanguage();
	}

	public void AddMenuSubFont(string langCode, bool append, params TMP_FontAsset[] fonts)
	{
		MenuFontSettings.AddSubFont(langCode, append, fonts);
	}

	public void ChangeFontLanguage(FontSettings settings, string langCode)
	{
		settings.ChangeLanguage(langCode);
	}

	public IEnumerator LoadFonts()
	{
		List<FileInfo> definitions = FindFonts();
		int i = definitions.Count;
		while (i-- > 0)
		{
			progressTitle = definitions[i].Name;
			yield return StartCoroutine(LoadBundle(definitions[i]));
			progressFraction = (float)i / (float)definitions.Count;
		}
		MenuFontSettings.ChangeLanguage(Localizer.GetLanguageIdFromFile());
		GameFontSettings.ChangeLanguage();
		isReady = true;
		progressFraction = 1f;
		yield return 0;
	}

	public List<FileInfo> FindFonts()
	{
		FileInfo[] files = new DirectoryInfo(Path.Combine(KSPUtil.ApplicationRootPath, "GameData/Squad")).GetFiles("*.fnt", SearchOption.AllDirectories);
		FileInfo[] files2 = new DirectoryInfo(Path.Combine(KSPUtil.ApplicationRootPath, "GameData")).GetFiles("*.fnt", SearchOption.AllDirectories);
		List<FileInfo> list = new List<FileInfo>(files);
		for (int i = 0; i < files2.Length; i++)
		{
			bool flag = false;
			string fullName = files2[i].FullName;
			for (int j = 0; j < files.Length; j++)
			{
				if (files[j].FullName == fullName)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(files2[i]);
			}
		}
		return list;
	}

	public IEnumerator LoadBundle(FileInfo fontBundle)
	{
		using UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(KSPUtil.ApplicationFileProtocol + fontBundle.FullName);
		yield return www.SendWebRequest();
		while (!www.isDone)
		{
			yield return null;
		}
		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError("FontLoader - WWW error: " + www.error);
			yield break;
		}
		AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
		if (bundle == null)
		{
			Debug.LogError("FontLoader: Bundle is null");
			yield break;
		}
		Debug.Log("FontLoader: Loaded bundle '" + fontBundle.FullName + "'");
		AssetBundleRequest matRequest = bundle.LoadAllAssetsAsync<Material>();
		yield return matRequest;
		AssetBundleRequest texRequest = bundle.LoadAllAssetsAsync<Texture>();
		yield return texRequest;
		AssetBundleRequest fontRequest = bundle.LoadAllAssetsAsync<TextAsset>();
		yield return fontRequest;
		int num = fontRequest.allAssets.Length - 1;
		while (num-- > 0)
		{
			TextAsset textAsset = fontRequest.allAssets[num] as TextAsset;
			try
			{
				KSPFontAsset kSPFontAsset;
				using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
				{
					kSPFontAsset = new BinaryFormatter
					{
						Binder = new KSPFontTypeConverter()
					}.Deserialize(memoryStream) as KSPFontAsset;
					memoryStream.Close();
				}
				LoadedFonts.Add(kSPFontAsset.GetFontAsset(matRequest.allAssets, texRequest.allAssets));
			}
			catch (Exception ex)
			{
				Debug.LogError("FontLoader: Error loading font " + ex.Message);
			}
		}
		bundle.Unload(unloadAllLoadedObjects: false);
	}
}
