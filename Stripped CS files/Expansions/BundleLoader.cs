using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

namespace Expansions;

public class BundleLoader : MonoBehaviour
{
	public class ABAssetInfo
	{
		public string assetName;

		public bool isScene;

		public string bundleName;

		public ABAssetInfo(string name, string bundleName, bool isScene)
		{
			assetName = name;
			this.bundleName = bundleName;
			this.isScene = isScene;
		}
	}

	public class ABInfo
	{
		public AssetBundle bundle;

		public string path;

		public byte[] hash;

		public string BundleName => bundle.name;

		public ABInfo(AssetBundle bundle, string path, byte[] hash)
		{
			this.bundle = bundle;
			this.path = path;
			this.hash = hash;
		}
	}

	public static Dictionary<string, ABInfo> loadedBundles = new Dictionary<string, ABInfo>();

	public static Dictionary<string, ABAssetInfo> loadedAssets = new Dictionary<string, ABAssetInfo>();

	public static BundleLoader Instance { get; set; }

	public Dictionary<string, ABAssetInfo> LoadedAssets => loadedAssets;

	public Dictionary<string, ABInfo> LoadedBundles => loadedBundles;

	public static bool IsBundleLoaded(string bundleName)
	{
		return loadedBundles.ContainsKey(bundleName);
	}

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("BundleLoader Instance already exists, destroying!");
			Object.Destroy(this);
		}
		Instance = this;
	}

	public static void LoadScene(GameScenes scene, string bundleName, string sceneUrl)
	{
		if (IsBundleLoaded(bundleName))
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sceneUrl);
			HighLogic.LoadSceneFromBundle(scene, fileNameWithoutExtension);
		}
	}

	public static Object LoadAsset<T>(string bundleName, string assetUrl) where T : MonoBehaviour
	{
		Object result = null;
		if (IsBundleLoaded(bundleName))
		{
			result = loadedBundles[bundleName].bundle.LoadAsset(assetUrl);
		}
		return result;
	}

	public static Object LoadAsset(string bundleName, string assetUrl)
	{
		Object result = null;
		if (IsBundleLoaded(bundleName))
		{
			result = loadedBundles[bundleName].bundle.LoadAsset(assetUrl);
		}
		return result;
	}

	public static Texture2D LoadTextureAsset(string bundleName, string assetUrl)
	{
		Texture2D result = null;
		if (IsBundleLoaded(bundleName))
		{
			result = (Texture2D)loadedBundles[bundleName].bundle.LoadAsset(assetUrl);
		}
		return result;
	}

	public static IEnumerator LoadAssetBundle(string bundleName, string folderPath)
	{
		if (IsBundleLoaded(bundleName))
		{
			Debug.Log(bundleName + " already loaded - skipping...");
			yield break;
		}
		byte[] hash = null;
		AssetBundle bundle = null;
		using (UnityWebRequest webRequest = new UnityWebRequest(folderPath + bundleName))
		{
			webRequest.downloadHandler = new DownloadHandlerBuffer();
			yield return webRequest.SendWebRequest();
			while (!webRequest.isDone)
			{
				yield return null;
			}
			if (!webRequest.isNetworkError && !webRequest.isHttpError)
			{
				hash = new MD5CryptoServiceProvider().ComputeHash(webRequest.downloadHandler.data);
				bundle = AssetBundle.LoadFromMemory(webRequest.downloadHandler.data);
			}
		}
		if (bundle == null)
		{
			Debug.LogError("Failed to load asset bundle from " + folderPath + bundleName);
			yield break;
		}
		string[] allAssetNames = bundle.GetAllAssetNames();
		if (allAssetNames.Length != 0)
		{
			Debug.Log("Assets:");
		}
		for (int i = 0; i < allAssetNames.Length; i++)
		{
			Debug.Log(Path.GetFileName(allAssetNames[i]));
			ABAssetInfo value = new ABAssetInfo(allAssetNames[i], bundleName, isScene: false);
			loadedAssets.Add(allAssetNames[i], value);
		}
		string[] allScenePaths = bundle.GetAllScenePaths();
		if (allScenePaths.Length != 0)
		{
			Debug.Log("Scenes:");
		}
		for (int j = 0; j < allScenePaths.Length; j++)
		{
			Debug.Log(Path.GetFileName(allScenePaths[j]));
			ABAssetInfo value2 = new ABAssetInfo(allScenePaths[j], bundleName, isScene: true);
			loadedAssets.Add(allScenePaths[j], value2);
		}
		loadedBundles.Add(bundle.name, new ABInfo(bundle, folderPath + bundleName, hash));
	}
}
