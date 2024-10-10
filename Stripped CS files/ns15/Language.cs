using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ns15;

public class Language : MonoBehaviour
{
	public class FontData
	{
		public string name;

		public string filePath;

		public string assetPath;

		public AssetBundle bundle;

		public Font font;
	}

	public static Dictionary<string, LanguageDefinition> allLanguages;

	public static Dictionary<string, FontData> allFonts;

	public static string languageName = "Default";

	public static string fontName = "Default";

	public static bool allFontsActive = false;

	public static LanguageDefinition language;

	public static Language _instance = null;

	public List<string> fontBundles = new List<string>();

	public bool clearLanguages;

	public static Language Instance => _instance;

	public Language()
	{
		if (allLanguages == null)
		{
			language = new LanguageDefinition(languageName, fontName);
			allLanguages = new Dictionary<string, LanguageDefinition>();
			allLanguages[languageName] = language;
			allFonts = new Dictionary<string, FontData>();
		}
	}

	public void Awake()
	{
		if (_instance != null)
		{
			Object.Destroy(this);
			return;
		}
		_instance = this;
		Object.DontDestroyOnLoad(this);
		LoadLanguages();
	}

	public void OnDestroy()
	{
		if (_instance != null && _instance == this)
		{
			_instance = null;
		}
	}

	public bool SetLanguage(string langName)
	{
		if (langName == languageName)
		{
			return true;
		}
		if (allLanguages.TryGetValue(langName, out var value))
		{
			if (value.Font != fontName)
			{
				FontData value2 = null;
				bool flag;
				if ((flag = fontName != "Default") && !allFonts.TryGetValue(fontName, out value2))
				{
					flag = false;
				}
				FontData value3 = null;
				bool flag2;
				if ((flag2 = value.Font != "Default") && !allFonts.TryGetValue(value.Font, out value3))
				{
					return false;
				}
				if (flag && value2.font != null && !allFontsActive)
				{
					Object.DestroyImmediate(value2.font, allowDestroyingAssets: true);
				}
				AssetBundle assetBundle = null;
				if (!flag2)
				{
					if (flag && !allFontsActive)
					{
						value2.bundle.Unload(unloadAllLoadedObjects: true);
						Object.DestroyImmediate(value2.bundle);
						value2.bundle = null;
					}
				}
				else if (value3.font == null)
				{
					if (flag && value2.filePath == value3.filePath)
					{
						assetBundle = value2.bundle;
						if (!allFontsActive)
						{
							value2.bundle = null;
						}
					}
					else
					{
						if (flag && !allFontsActive)
						{
							value2.bundle.Unload(unloadAllLoadedObjects: true);
							Object.DestroyImmediate(value2.bundle);
							value2.bundle = null;
						}
						StartCoroutine(LoadBundle(assetBundle, value3, value3.filePath, onSetLanguage));
					}
					if (assetBundle == null)
					{
						Debug.LogError("AssetLoader: Bundle is null");
						return false;
					}
					value3.font = assetBundle.LoadAsset<Font>(value3.assetPath);
					if (value3.font == null)
					{
						if (!allFontsActive)
						{
							assetBundle.Unload(unloadAllLoadedObjects: true);
							Object.DestroyImmediate(assetBundle);
						}
						return false;
					}
					value3.bundle = assetBundle;
				}
			}
			languageName = value.Name;
			fontName = value.Font;
			language = value;
			Debug.Log("Localiser: Set language to " + languageName + " with font " + fontName);
			return true;
		}
		return false;
	}

	public void onSetLanguage(AssetBundle bundle, FontData font, string filePath, bool success)
	{
	}

	public static IEnumerator LoadBundle(AssetBundle assetBundle, FontData font, string filePath, Callback<AssetBundle, FontData, string, bool> onLoaded)
	{
		if (!(assetBundle == null))
		{
			yield break;
		}
		string url = KSPUtil.ApplicationFileProtocol + filePath;
		using UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url);
		yield return uwr.SendWebRequest();
		while (!uwr.isDone)
		{
			yield return null;
		}
		if (!uwr.isNetworkError && !uwr.isHttpError && string.IsNullOrEmpty(uwr.error))
		{
			assetBundle = DownloadHandlerAssetBundle.GetContent(uwr);
			if (assetBundle == null)
			{
				Debug.LogError("AssetLoader: Bundle is null");
				onLoaded(null, font, filePath, arg4: false);
			}
			else
			{
				onLoaded(assetBundle, font, filePath, arg4: true);
			}
			yield break;
		}
		Debug.LogError("AssetLoader - WWW error in " + url + " : " + uwr.error);
		onLoaded?.Invoke(null, font, filePath, arg4: false);
	}

	public string GetLanguage(bool translate = false)
	{
		if (translate)
		{
			return Translate(languageName);
		}
		return languageName;
	}

	public string Translate(string input)
	{
		if (language != null)
		{
			return language.Translation(input);
		}
		return input;
	}

	public string TranslateWithFormat(string baseString, params object[] args)
	{
		if (language != null)
		{
			return language.TranslateWithFormat(baseString, args);
		}
		return baseString;
	}

	public List<string> LoadedLanguages()
	{
		List<string> list = new List<string>();
		foreach (string key in allLanguages.Keys)
		{
			list.Add(key);
		}
		return list;
	}

	public List<string> LoadedFonts()
	{
		List<string> list = new List<string>();
		foreach (string key in allFonts.Keys)
		{
			list.Add(key);
		}
		return list;
	}

	public void UpdateText(Text text)
	{
		UpdateFont(text);
		text.text = Translate(text.text);
	}

	public void UpdateFont(Text text)
	{
		if (fontName != "Default" && allFonts.TryGetValue(fontName, out var value))
		{
			text.font = value.font;
		}
	}

	public void ActivateAllFonts()
	{
		if (allFontsActive)
		{
			return;
		}
		allFontsActive = true;
		string value = "NoValidPath";
		AssetBundle assetBundle = null;
		if (fontName != "Default" && allFonts.TryGetValue(fontName, out var value2))
		{
			value = value2.assetPath;
			assetBundle = value2.bundle;
		}
		foreach (KeyValuePair<string, FontData> allFont in allFonts)
		{
			if (allFont.Key != fontName && allFont.Value.font == null)
			{
				AssetBundle assetBundle2 = null;
				value2 = allFont.Value;
				if (value2.filePath.Equals(value))
				{
					assetBundle2 = assetBundle;
				}
				else
				{
					StartCoroutine(LoadBundle(assetBundle2, allFont.Value, value2.filePath, onActivateAllFonts));
				}
			}
		}
	}

	public void onActivateAllFonts(AssetBundle bundle, FontData font, string filePath, bool success)
	{
		if (bundle != null && font != null)
		{
			font.font = bundle.LoadAsset<Font>(font.assetPath);
			font.bundle = bundle;
		}
	}

	public void DeactivateAllFonts()
	{
		if (!allFontsActive)
		{
			return;
		}
		allFontsActive = false;
		AssetBundle assetBundle = null;
		if (fontName != "Default" && allFonts.TryGetValue(fontName, out var value))
		{
			assetBundle = value.bundle;
		}
		foreach (KeyValuePair<string, FontData> allFont in allFonts)
		{
			value = allFont.Value;
			if (allFont.Key != fontName && value.font != null)
			{
				Object.DestroyImmediate(value.font, allowDestroyingAssets: true);
				value.font = null;
			}
			if (value.bundle != assetBundle && value.bundle != null)
			{
				value.bundle.Unload(unloadAllLoadedObjects: true);
				Object.DestroyImmediate(value.bundle);
				value.bundle = null;
			}
		}
	}

	public string LanguageName(string language, out Font newFont)
	{
		newFont = null;
		if (allLanguages.TryGetValue(language, out var value))
		{
			if (value.Font != "Default" && allFonts.TryGetValue(value.Font, out var value2))
			{
				newFont = value2.font;
			}
			return value.Translation(language);
		}
		return language;
	}

	public void LoadLanguage(string path, bool localPath = true)
	{
		LanguageDefinition languageDefinition = new LanguageDefinition(path, localPath);
		LanguageDefinition value = null;
		if (allLanguages.TryGetValue(languageDefinition.Name, out value))
		{
			languageDefinition.MergeInto(value);
			Debug.Log("Localiser: Updated language " + languageDefinition.Name + " from path " + path);
		}
		else
		{
			allLanguages[languageDefinition.Name] = languageDefinition;
			Debug.Log("Localiser: Loaded language " + languageDefinition.Name + " from path " + path);
		}
	}

	public void LoadLanguages(bool clear = true)
	{
		bool flag = allFontsActive;
		clearLanguages = clear;
		if (clear)
		{
			if (flag)
			{
				DeactivateAllFonts();
			}
			if (fontName != "Default" && allFonts.TryGetValue(fontName, out var value))
			{
				Object.DestroyImmediate(value.font, allowDestroyingAssets: true);
				value.bundle.Unload(unloadAllLoadedObjects: true);
				Object.DestroyImmediate(value.bundle);
			}
			allLanguages.Clear();
			allFonts.Clear();
		}
		string path = Path.Combine(KSPUtil.ApplicationRootPath, "GameData/Localisation");
		if (Directory.Exists(path))
		{
			fontBundles = new List<string>();
			string[] files = Directory.GetFiles(path, "*.unity3d");
			for (int i = 0; i < files.Length; i++)
			{
				fontBundles.Add(files[i]);
			}
			string[] array = files;
			foreach (string filePath in array)
			{
				AssetBundle assetBundle = null;
				StartCoroutine(LoadBundle(assetBundle, null, filePath, onLoadLanguages));
			}
		}
	}

	public void onLoadLanguages(AssetBundle bundle, FontData font, string filePath, bool success)
	{
		string[] allAssetNames;
		if (bundle != null)
		{
			allAssetNames = bundle.GetAllAssetNames();
			foreach (string assetPath in allAssetNames)
			{
				Font font2 = bundle.LoadAsset<Font>(assetPath);
				if (font2 != null)
				{
					FontData fontData = new FontData();
					fontData.name = font2.name;
					fontData.assetPath = assetPath;
					fontData.filePath = filePath;
					allFonts[font2.name] = fontData;
					Debug.Log("Localiser: Loaded font " + fontData.name + " from path " + filePath);
				}
				bundle.Unload(unloadAllLoadedObjects: true);
				Object.DestroyImmediate(bundle);
			}
		}
		if (fontBundles.Contains(filePath))
		{
			fontBundles.Remove(filePath);
		}
		if (fontBundles.Count != 0)
		{
			return;
		}
		allAssetNames = Directory.GetFiles(Path.Combine(KSPUtil.ApplicationRootPath, "GameData/Localisation"), "*.xml");
		foreach (string path in allAssetNames)
		{
			LoadLanguage(path, localPath: false);
		}
		foreach (LanguageDefinition value in allLanguages.Values)
		{
			if (value.Font != "Default" && !string.IsNullOrEmpty(value.Font) && !allFonts.ContainsKey(value.Font))
			{
				Debug.LogError("Localiser: Language " + value.Name + " has font " + value.Font + " but no such font available!");
			}
		}
		if (clearLanguages)
		{
			string text = languageName;
			fontName = "";
			languageName = "";
			SetLanguage(text);
			if (allFontsActive)
			{
				ActivateAllFonts();
			}
		}
	}

	public string Save()
	{
		return languageName;
	}

	public void SaveLanguages()
	{
		foreach (KeyValuePair<string, LanguageDefinition> allLanguage in allLanguages)
		{
			allLanguage.Value.Serialize("Localisation/" + allLanguage.Key + "_export.xml");
		}
	}

	public void SaveUntranslated()
	{
		foreach (KeyValuePair<string, LanguageDefinition> allLanguage in allLanguages)
		{
			allLanguage.Value.SerializeUntranslated("Localisation/" + allLanguage.Key + "_untranslated.xml");
		}
	}
}
