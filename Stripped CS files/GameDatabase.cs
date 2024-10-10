using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Experience;
using KSPAssets;
using KSPAssets.Loaders;
using ns9;
using UniLinq;
using UnityEngine;

public class GameDatabase : LoadingSystem
{
	[Serializable]
	public class TextureInfo
	{
		public string name;

		public UrlDir.UrlFile file;

		public Texture2D texture;

		public bool isNormalMap;

		public bool isReadable;

		public bool isCompressed;

		public Texture2D normalMap
		{
			get
			{
				if (isNormalMap)
				{
					return texture;
				}
				if (isReadable)
				{
					texture = BitmapToUnityNormalMap(texture);
					isNormalMap = true;
					isReadable = false;
					return texture;
				}
				return texture;
			}
		}

		public TextureInfo(UrlDir.UrlFile file, Texture2D texture, bool isNormalMap, bool isReadable, bool isCompressed)
		{
			this.file = file;
			this.texture = texture;
			this.isNormalMap = isNormalMap;
			this.isReadable = isReadable;
			this.isCompressed = isCompressed;
		}
	}

	public Dictionary<string, string> flagSwaps = new Dictionary<string, string>
	{
		{ "Squad/Flags/esa_dark_blue", "Squad/FlagsAgency/esa_dark_blue" },
		{ "Squad/Flags/NASA", "Squad/FlagsAgency/NASA" },
		{ "Squad/Flags/uk_space_agency", "Squad/FlagsAgency/uk_space_agency" },
		{ "Squad/Flags/B612_Foundation_flag", "Squad/FlagsOrganization/B612_Foundation_flag" },
		{ "Squad/Flags/electron", "Squad/FlagsOrganization/electron" },
		{ "Squad/Flags/ESA_Arianespace", "Squad/FlagsOrganization/ESA_Arianespace" },
		{ "Squad/Flags/rocketlab", "Squad/FlagsOrganization/rocketlab" }
	};

	[SerializeField]
	public string _settingsFileName;

	[SerializeField]
	public List<UrlDir.ConfigDirectory> urlConfig;

	[SerializeField]
	public string pluginDataFolder;

	[SerializeField]
	public List<string> _assemblyTypes;

	[SerializeField]
	public List<AudioClip> databaseAudio = new List<AudioClip>();

	[SerializeField]
	public List<UrlDir.UrlFile> databaseAudioFiles = new List<UrlDir.UrlFile>();

	[SerializeField]
	public List<TextureInfo> databaseTexture = new List<TextureInfo>();

	[SerializeField]
	public List<GameObject> databaseModel = new List<GameObject>();

	[SerializeField]
	public ExperienceSystemConfig experienceConfigs;

	[SerializeField]
	public List<UrlDir.UrlFile> databaseModelFiles = new List<UrlDir.UrlFile>();

	[SerializeField]
	public List<Shader> databaseShaders = new List<Shader>();

	[SerializeField]
	[HideInInspector]
	public UrlDir _root;

	public static GameDatabase _instance;

	public bool isReady;

	public string progressTitle = "";

	public float progressFraction;

	public List<DatabaseLoader<AudioClip>> loadersAudio;

	public List<DatabaseLoader<TextureInfo>> loadersTexture;

	public List<DatabaseLoader<GameObject>> loadersModel;

	public static IntPtr intPtr = new IntPtr(long.MaxValue);

	public static bool modded = false;

	public static string environemntInfo = "";

	public static HashSet<string> loaderInfo = new HashSet<string>();

	public static List<string> loadedModsInfo = new List<string>();

	public string[] assemblyWhitelist = new string[9] { "Assembly-CSharp", "SaveUpgradePipeline.Scripts", "KSPExternalModules", "TestTypeBans", "UnitTests", "Steamworks.NET", "KSPSteamCtrlr", "KSPCore", "KSPUtil" };

	public string[] folderWhitelist = new string[4] { "Squad", "SquadExpansion", "PluginData", ".DS_Store" };

	public AssetLoader.Loader assetLoader;

	public bool _recompileModels;

	public bool _recompile;

	public DateTime lastLoadTime = new DateTime(1, 1, 1);

	public string settingsFileName => _settingsFileName;

	public string PluginDataFolder => Path.Combine(UrlDir.ApplicationRootPath, pluginDataFolder);

	public ExperienceSystemConfig ExperienceConfigs => experienceConfigs;

	public UrlDir root => _root;

	public static GameDatabase Instance => _instance;

	public static bool Modded => modded;

	public static string EnvironmentInfo => environemntInfo;

	public bool RecompileModels
	{
		get
		{
			return _recompileModels;
		}
		set
		{
			_recompileModels = value;
			if (_recompileModels)
			{
				isReady = false;
			}
		}
	}

	public bool Recompile
	{
		get
		{
			return _recompile;
		}
		set
		{
			_recompile = value;
			if (_recompile)
			{
				isReady = false;
			}
		}
	}

	public bool ExistsShader(string url)
	{
		int num = 0;
		while (true)
		{
			if (num < databaseShaders.Count)
			{
				if (databaseShaders[num].name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public Shader GetShader(string url)
	{
		int num = 0;
		Shader shader;
		while (true)
		{
			if (num < databaseShaders.Count)
			{
				shader = databaseShaders[num];
				if (shader.name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return shader;
	}

	public bool RemoveShader(string url)
	{
		Shader shader = null;
		for (int i = 0; i < databaseShaders.Count; i++)
		{
			Shader shader2 = databaseShaders[i];
			if (shader2.name == url)
			{
				shader = shader2;
				break;
			}
		}
		if (shader != null)
		{
			databaseShaders.Remove(shader);
			return true;
		}
		return false;
	}

	public bool ExistsAudioClip(string url)
	{
		int num = 0;
		while (true)
		{
			if (num < databaseAudio.Count)
			{
				if (databaseAudio[num] != null && databaseAudio[num].name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public AudioClip GetAudioClip(string url)
	{
		int num = 0;
		AudioClip audioClip;
		while (true)
		{
			if (num < databaseAudio.Count)
			{
				if (databaseAudio[num] != null)
				{
					audioClip = databaseAudio[num];
					if (audioClip.name == url)
					{
						break;
					}
				}
				num++;
				continue;
			}
			return null;
		}
		return audioClip;
	}

	public List<AudioClip> AudioClipNameContains(string nameContains)
	{
		List<AudioClip> list = new List<AudioClip>();
		for (int i = 0; i < databaseAudio.Count; i++)
		{
			if (databaseAudio[i].name.Contains(nameContains))
			{
				list.Add(databaseAudio[i]);
			}
		}
		return list;
	}

	public UrlDir.UrlFile GetAudioFile(string url)
	{
		int num = 0;
		while (true)
		{
			if (num < databaseAudio.Count)
			{
				if (databaseAudio[num] != null && databaseAudio[num].name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return databaseAudioFiles[num];
	}

	public bool RemoveAudioClip(string url)
	{
		AudioClip audioClip = null;
		for (int i = 0; i < databaseAudio.Count; i++)
		{
			if (databaseAudio[i] != null)
			{
				AudioClip audioClip2 = databaseAudio[i];
				if (audioClip2.name == url)
				{
					audioClip = audioClip2;
					break;
				}
			}
		}
		if (audioClip != null)
		{
			databaseAudio.Remove(audioClip);
			return true;
		}
		return false;
	}

	public bool ExistsTexture(string url)
	{
		if (url == null)
		{
			return false;
		}
		int num = 0;
		while (true)
		{
			if (num < databaseTexture.Count)
			{
				TextureInfo textureInfo = databaseTexture[num];
				if (textureInfo != null && string.Equals(textureInfo.name, url, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				num++;
				continue;
			}
			if (flagSwaps.ContainsKey(url))
			{
				url = flagSwaps[url];
				return ExistsTexture(url);
			}
			return false;
		}
		return true;
	}

	public UrlDir.UrlFile GetTextureFile(string url)
	{
		if (url == null)
		{
			return null;
		}
		int num = 0;
		TextureInfo textureInfo;
		while (true)
		{
			if (num < databaseTexture.Count)
			{
				textureInfo = databaseTexture[num];
				if (textureInfo != null && string.Equals(textureInfo.name, url, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				num++;
				continue;
			}
			if (flagSwaps.ContainsKey(url))
			{
				url = flagSwaps[url];
				return GetTextureFile(url);
			}
			return null;
		}
		return textureInfo.file;
	}

	public TextureInfo GetTextureInfo(string url)
	{
		if (url == null)
		{
			return null;
		}
		int num = 0;
		TextureInfo textureInfo;
		while (true)
		{
			if (num < databaseTexture.Count)
			{
				textureInfo = databaseTexture[num];
				if (textureInfo != null && string.Equals(textureInfo.name, url, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				num++;
				continue;
			}
			if (flagSwaps.ContainsKey(url))
			{
				url = flagSwaps[url];
				return GetTextureInfo(url);
			}
			return null;
		}
		return textureInfo;
	}

	public TextureInfo GetTextureInfoIn(string url, string textureName)
	{
		if (url != null && textureName != null)
		{
			string text = url.Substring(0, url.LastIndexOf('/') + 1) + textureName;
			int num = 0;
			TextureInfo textureInfo;
			while (true)
			{
				if (num < databaseTexture.Count)
				{
					textureInfo = databaseTexture[num];
					if (textureInfo != null && string.Equals(textureInfo.name, text, StringComparison.OrdinalIgnoreCase))
					{
						break;
					}
					num++;
					continue;
				}
				if (flagSwaps.ContainsKey(text))
				{
					text = flagSwaps[text];
					return GetTextureInfo(text);
				}
				return null;
			}
			return textureInfo;
		}
		return null;
	}

	public Texture2D GetTexture(string url, bool asNormalMap)
	{
		if (url == null)
		{
			return null;
		}
		int num = 0;
		TextureInfo textureInfo;
		while (true)
		{
			if (num < databaseTexture.Count)
			{
				textureInfo = databaseTexture[num];
				if (textureInfo != null && string.Equals(textureInfo.name, url, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				num++;
				continue;
			}
			if (flagSwaps.ContainsKey(url))
			{
				url = flagSwaps[url];
				return Instance.GetTexture(url, asNormalMap);
			}
			return null;
		}
		if (asNormalMap)
		{
			return textureInfo.normalMap;
		}
		return textureInfo.texture;
	}

	public Texture2D GetTextureIn(string url, string textureName, bool asNormalMap)
	{
		if (url != null && textureName != null)
		{
			string text = url.Substring(0, url.LastIndexOf('/') + 1) + textureName;
			int num = 0;
			TextureInfo textureInfo;
			while (true)
			{
				if (num < databaseTexture.Count)
				{
					textureInfo = databaseTexture[num];
					if (textureInfo != null && string.Equals(textureInfo.name, text, StringComparison.OrdinalIgnoreCase))
					{
						break;
					}
					num++;
					continue;
				}
				if (flagSwaps.ContainsKey(text))
				{
					text = flagSwaps[text];
					return GetTexture(text, asNormalMap);
				}
				return null;
			}
			if (asNormalMap)
			{
				return textureInfo.normalMap;
			}
			return textureInfo.texture;
		}
		return null;
	}

	public bool RemoveTexture(string url)
	{
		int num = 0;
		while (true)
		{
			if (num < databaseTexture.Count)
			{
				TextureInfo textureInfo = databaseTexture[num];
				if (textureInfo != null && string.Equals(textureInfo.name, url, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		databaseTexture.RemoveAt(num);
		return true;
	}

	public bool ReplaceTexture(string url, TextureInfo newTex)
	{
		int num = 0;
		TextureInfo textureInfo;
		while (true)
		{
			if (num < databaseTexture.Count)
			{
				textureInfo = databaseTexture[num];
				if (textureInfo != null && textureInfo.name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		databaseTexture.RemoveAt(num);
		UnityEngine.Object.Destroy(textureInfo.texture);
		databaseTexture.Add(newTex);
		return true;
	}

	public List<TextureInfo> GetAllTexturesInFolderType(string folderName, bool caseInsensitive = false)
	{
		List<TextureInfo> list = new List<TextureInfo>();
		for (int i = 0; i < databaseTexture.Count; i++)
		{
			TextureInfo textureInfo = databaseTexture[i];
			string text = textureInfo.name.Substring(0, textureInfo.name.LastIndexOf('/'));
			int num = text.LastIndexOf('/');
			if (num != -1)
			{
				text = text.Substring(num + 1, text.Length - num - 1);
			}
			if (text == folderName || (caseInsensitive && text.ToLower() == folderName.ToLower()))
			{
				list.Add(textureInfo);
			}
		}
		return list;
	}

	public List<TextureInfo> GetAllTexturesInFolder(string folderURL)
	{
		List<TextureInfo> list = new List<TextureInfo>();
		for (int i = 0; i < databaseTexture.Count; i++)
		{
			TextureInfo textureInfo = databaseTexture[i];
			if (textureInfo.name.Substring(0, textureInfo.name.LastIndexOf('/') + 1) == folderURL)
			{
				list.Add(textureInfo);
			}
		}
		return list;
	}

	public static Texture2D BitmapToUnityNormalMap(Texture2D tex)
	{
		int width = tex.width;
		int height = tex.height;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, mipChain: true);
		texture2D.wrapMode = TextureWrapMode.Repeat;
		Color white = Color.white;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Color pixel = tex.GetPixel(i, j);
				white.r = pixel.g;
				white.g = pixel.g;
				white.b = pixel.g;
				white.a = pixel.r;
				texture2D.SetPixel(i, j, white);
			}
		}
		texture2D.Apply(updateMipmaps: true, makeNoLongerReadable: true);
		UnityEngine.Object.Destroy(tex);
		return texture2D;
	}

	public bool ExistsModel(string url)
	{
		int num = 0;
		while (true)
		{
			if (num < databaseModel.Count)
			{
				if (databaseModel[num].name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public GameObject GetModel(string url)
	{
		int num = 0;
		GameObject gameObject;
		while (true)
		{
			if (num < databaseModel.Count)
			{
				gameObject = databaseModel[num];
				if (gameObject.name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return UnityEngine.Object.Instantiate(gameObject);
	}

	public GameObject GetModelPrefab(string url)
	{
		int num = 0;
		GameObject gameObject;
		while (true)
		{
			if (num < databaseModel.Count)
			{
				gameObject = databaseModel[num];
				if (gameObject.name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return gameObject;
	}

	public UrlDir.UrlFile GetModelFile(string url)
	{
		int num = 0;
		while (true)
		{
			if (num < databaseModel.Count)
			{
				if (databaseModel[num].name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return databaseModelFiles[num];
	}

	public UrlDir.UrlFile GetModelFile(GameObject modelPrefab)
	{
		int num = 0;
		while (true)
		{
			if (num < databaseModel.Count)
			{
				if (databaseModel[num] == modelPrefab)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return databaseModelFiles[num];
	}

	public GameObject GetModelPrefabIn(string url)
	{
		int num = 0;
		GameObject gameObject;
		while (true)
		{
			if (num < databaseModel.Count)
			{
				gameObject = databaseModel[num];
				if (gameObject.name.Substring(0, gameObject.name.LastIndexOf('/')) == url)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return gameObject;
	}

	public GameObject GetModelIn(string url)
	{
		int num = 0;
		GameObject gameObject;
		while (true)
		{
			if (num < databaseModel.Count)
			{
				gameObject = databaseModel[num];
				if (gameObject.name.Substring(0, gameObject.name.LastIndexOf('/')) == url)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return UnityEngine.Object.Instantiate(gameObject);
	}

	public bool RemoveModel(string url)
	{
		int num = 0;
		while (true)
		{
			if (num < databaseModel.Count)
			{
				if (databaseModel[num].name == url)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		databaseModel.RemoveAt(num);
		databaseModelFiles.RemoveAt(num);
		return true;
	}

	public bool ExistsConfigNode(string url)
	{
		if (root.GetConfig(url) == null)
		{
			return false;
		}
		return true;
	}

	public ConfigNode GetConfigNode(string url)
	{
		return root.GetConfig(url)?.config.CreateCopy();
	}

	public ConfigNode[] GetConfigNodes(string typeName)
	{
		List<ConfigNode> list = new List<ConfigNode>();
		foreach (UrlDir.UrlConfig config in root.GetConfigs(typeName))
		{
			list.Add(config.config);
		}
		return list.ToArray();
	}

	public ConfigNode[] GetConfigNodes(string baseUrl, string typeName)
	{
		List<ConfigNode> list = new List<ConfigNode>();
		UrlDir directory = root.GetDirectory(baseUrl);
		if (directory == null)
		{
			return list.ToArray();
		}
		foreach (UrlDir.UrlConfig config in directory.GetConfigs(typeName))
		{
			list.Add(config.config);
		}
		return list.ToArray();
	}

	public ConfigNode GetMergedConfigNodes(string nodeName, bool mergeChildren = false)
	{
		ConfigNode configNode = new ConfigNode(nodeName);
		IEnumerable<UrlDir.UrlConfig> configs = root.GetConfigs(nodeName);
		for (int i = 0; i < configs.Count(); i++)
		{
			UrlDir.UrlConfig urlConfig = configs.ElementAt(i);
			for (int j = 0; j < urlConfig.config.nodes.Count; j++)
			{
				ConfigNode configNode2 = urlConfig.config.nodes[j];
				ConfigNode configNode3 = ((!configNode.HasNode(configNode2.name) || !mergeChildren) ? configNode.AddNode(configNode2.name) : configNode.GetNode(configNode2.name));
				for (int k = 0; k < configNode2.values.Count; k++)
				{
					configNode3.AddValue(configNode2.values[k].name, configNode2.values[k].value);
				}
			}
		}
		return configNode;
	}

	public UrlDir.UrlConfig[] GetConfigs(string typeName)
	{
		List<UrlDir.UrlConfig> list = new List<UrlDir.UrlConfig>(root.GetConfigs(typeName));
		UrlDir.UrlConfig[] array = new UrlDir.UrlConfig[list.Count];
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			array[i] = list[i];
		}
		return array;
	}

	public void Reset()
	{
		urlConfig = new List<UrlDir.ConfigDirectory>();
		urlConfig.Add(new UrlDir.ConfigDirectory("parts", "parts", UrlDir.DirectoryType.Parts));
		urlConfig.Add(new UrlDir.ConfigDirectory("internals", "internals", UrlDir.DirectoryType.Internals));
		urlConfig.Add(new UrlDir.ConfigDirectory("data", "GameData", UrlDir.DirectoryType.GameData));
		_assemblyTypes = new List<string>();
		_assemblyTypes.Add("Part");
		_assemblyTypes.Add("PartModule");
		_assemblyTypes.Add("InternalModule");
		_assemblyTypes.Add("ScenarioModule");
		_assemblyTypes.Add("UnitTest");
		_assemblyTypes.Add("Expansions.Missions.TestModule");
		_assemblyTypes.Add("Expansions.Missions.ActionModule");
		_assemblyTypes.Add("Expansions.Missions.DynamicModule");
		_assemblyTypes.Add("Expansions.Serenity.RobotArmFX.RobotArmScannerFX");
		LoadingScreen.minFrameTime = 0.5f;
	}

	public void Awake()
	{
		if (_instance != null)
		{
			UnityEngine.Debug.Log("GameDatabase cannot exist on two gameobjects in scene");
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		_instance = this;
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public void OnDestroy()
	{
		if (_instance != null && _instance == this)
		{
			_instance = null;
		}
	}

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
			UnityEngine.Debug.LogError("GameDatabase: Already started and finished. Aborting.");
		}
		else
		{
			StartCoroutine(CreateDatabase());
		}
	}

	public IEnumerator CreateDatabase()
	{
		progressTitle = "";
		progressFraction = 0f;
		float startTime = Time.realtimeSinceStartup;
		yield return StartCoroutine(LoadObjects());
		yield return StartCoroutine(LoadAssetBundleObjects());
		UnityEngine.Debug.Log("Compiling Configs:");
		foreach (UrlDir.UrlConfig allConfig in root.AllConfigs)
		{
			UnityEngine.Debug.Log("Config(" + allConfig.type + ") " + allConfig.url);
			CompileConfig(allConfig.config);
		}
		UnityEngine.Debug.Log("Compiling Configs Completed.");
		progressTitle = Localizer.Format("#autoLOC_158078");
		PartResourceLibrary.Instance.LoadDefinitions();
		PhysicMaterialLibrary.Instance.LoadDefinitions();
		yield return null;
		progressTitle = Localizer.Format("#autoLOC_158082");
		if (experienceConfigs == null)
		{
			experienceConfigs = new ExperienceSystemConfig();
		}
		else
		{
			experienceConfigs.LoadTraitConfigs();
		}
		yield return null;
		progressTitle = Localizer.Format("#autoLOC_158089");
		PartUpgradeManager.Handler.FillUpgrades();
		GameEvents.OnUpgradesFilled.Fire();
		yield return null;
		_recompileModels = false;
		CleanupLoaders();
		progressTitle = "";
		progressFraction = 1f;
		isReady = true;
		GameEvents.OnGameDatabaseLoaded.Fire();
		UnityEngine.Debug.Log("GameDatabase: Assets loaded in " + (Time.realtimeSinceStartup - startTime).ToString("F3") + "s");
	}

	public List<UrlDir.ConfigFileType> SetupAssemblyLoader()
	{
		List<UrlDir.ConfigFileType> list = new List<UrlDir.ConfigFileType>();
		UrlDir.ConfigFileType configFileType = new UrlDir.ConfigFileType(UrlDir.FileType.Assembly);
		list.Add(configFileType);
		configFileType.extensions.Add("dll");
		return list;
	}

	public List<UrlDir.ConfigFileType> SetupMainLoaders()
	{
		loadersAudio = new List<DatabaseLoader<AudioClip>>();
		loadersTexture = new List<DatabaseLoader<TextureInfo>>();
		loadersModel = new List<DatabaseLoader<GameObject>>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(DatabaseLoader<AudioClip>)))
			{
				loadersAudio.Add((DatabaseLoader<AudioClip>)Activator.CreateInstance(t));
			}
			else if (t.IsSubclassOf(typeof(DatabaseLoader<TextureInfo>)))
			{
				loadersTexture.Add((DatabaseLoader<TextureInfo>)Activator.CreateInstance(t));
			}
			else if (t.IsSubclassOf(typeof(DatabaseLoader<GameObject>)))
			{
				loadersModel.Add((DatabaseLoader<GameObject>)Activator.CreateInstance(t));
			}
		});
		List<UrlDir.ConfigFileType> list = new List<UrlDir.ConfigFileType>();
		UrlDir.ConfigFileType configFileType = new UrlDir.ConfigFileType(UrlDir.FileType.Assembly);
		list.Add(configFileType);
		configFileType.extensions.Add("dll");
		UrlDir.ConfigFileType configFileType2 = new UrlDir.ConfigFileType(UrlDir.FileType.Audio);
		list.Add(configFileType2);
		foreach (DatabaseLoader<AudioClip> item in loadersAudio)
		{
			configFileType2.extensions.AddRange(item.extensions);
		}
		UrlDir.ConfigFileType configFileType3 = new UrlDir.ConfigFileType(UrlDir.FileType.Texture);
		list.Add(configFileType3);
		foreach (DatabaseLoader<TextureInfo> item2 in loadersTexture)
		{
			configFileType3.extensions.AddRange(item2.extensions);
		}
		UrlDir.ConfigFileType configFileType4 = new UrlDir.ConfigFileType(UrlDir.FileType.Model);
		list.Add(configFileType4);
		foreach (DatabaseLoader<GameObject> item3 in loadersModel)
		{
			configFileType4.extensions.AddRange(item3.extensions);
		}
		return list;
	}

	public void LoadEmpty()
	{
		Reset();
		AssemblyLoader.Initialize(new string[0]);
		_root = new UrlDir(urlConfig.ToArray(), new UrlDir.ConfigFileType[0]);
	}

	public static void UpdateLoaderInfo(Dictionary<string, bool> dest)
	{
		foreach (string item in dest.Keys.ToList())
		{
			if (!loaderInfo.Contains(item))
			{
				dest[item] = false;
			}
			else
			{
				dest[item] = true;
			}
		}
		foreach (string item2 in loaderInfo)
		{
			if (!dest.ContainsKey(item2))
			{
				dest[item2] = true;
			}
		}
	}

	public static void SaveLoaderInfo(ConfigNode node, Dictionary<string, bool> dest)
	{
		UpdateLoaderInfo(dest);
		foreach (KeyValuePair<string, bool> item in dest)
		{
			node.AddValue(item.Key, item.Value.ToString());
		}
	}

	public static void LoadLoaderInfo(ConfigNode node, Dictionary<string, bool> dest)
	{
		foreach (ConfigNode.Value value in node.values)
		{
			if (!dest.ContainsKey(value.name))
			{
				dest[value.name] = false;
			}
		}
		UpdateLoaderInfo(dest);
	}

	public void translateLoadedNodes()
	{
		int num = root.AllConfigs.Count();
		for (int i = 0; i < num; i++)
		{
			Localizer.TranslateBranch(root.AllConfigs.ElementAt(i).config);
		}
	}

	public IEnumerator LoadObjects()
	{
		PartLoader.Instance.Recompile = true;
		AssemblyLoader.Initialize(_assemblyTypes.ToArray());
		List<UrlDir.ConfigFileType> list = SetupAssemblyLoader();
		_root = new UrlDir(urlConfig.ToArray(), list.ToArray());
		Localizer.Init();
		LoadingScreen.StartLoadingScreens();
		AssemblyLoader.ClearPlugins();
		progressTitle = "Caching Assembly info";
		foreach (UrlDir.UrlFile file4 in root.GetFiles(UrlDir.FileType.Assembly))
		{
			ConfigNode assemblyNode = null;
			List<UrlDir.UrlConfig> list2 = new List<UrlDir.UrlConfig>(file4.parent.GetConfigs("ASSEMBLY", file4.name, recursive: false));
			if (list2.Count > 0)
			{
				assemblyNode = list2[0].config;
			}
			AssemblyLoader.LoadPluginInfo(new FileInfo(file4.fullPath), file4.parent.url, assemblyNode);
		}
		if (!Instance.Recompile)
		{
			AssemblyLoader.FlagDuplicatedPlugins();
		}
		foreach (UrlDir.UrlFile file5 in root.GetFiles(UrlDir.FileType.Assembly))
		{
			progressTitle = file5.url;
			UnityEngine.Debug.Log("Load(Assembly): " + file5.url);
			ConfigNode assemblyNode2 = null;
			List<UrlDir.UrlConfig> list3 = new List<UrlDir.UrlConfig>(file5.parent.GetConfigs("ASSEMBLY", file5.name, recursive: false));
			if (list3.Count > 0)
			{
				assemblyNode2 = list3[0].config;
			}
			AssemblyLoader.LoadPlugin(new FileInfo(file5.fullPath), file5.parent.url, assemblyNode2);
		}
		AssemblyLoader.LoadAssemblies();
		VesselModuleManager.CompileModules();
		string text = "\n************************************************************************\n";
		string text2 = "\nEnvironment Info\n";
		text2 = string.Concat(text2, Environment.OSVersion.Platform, " ", intPtr.ToInt64().ToString("X16"));
		string text3 = Environment.GetCommandLineArgs()[0];
		text2 = text2 + "  Args: " + text3.Split(Path.DirectorySeparatorChar).Last() + " " + string.Join(" ", Environment.GetCommandLineArgs().Skip(1).ToArray()) + "\n";
		text += text2;
		environemntInfo = text2.Replace("\n\n", " *** ");
		environemntInfo = environemntInfo.Replace("\n", " - ");
		modded = false;
		string text4 = "\nMod DLLs found:\n";
		foreach (AssemblyLoader.LoadedAssembly loadedAssembly in AssemblyLoader.loadedAssemblies)
		{
			if (!string.IsNullOrEmpty(loadedAssembly.assembly.Location))
			{
				AssemblyName assemblyName = loadedAssembly.assembly.GetName();
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(loadedAssembly.assembly.Location);
				string text5 = string.Concat(assemblyName.Name, " v", assemblyName.Version, (!(versionInfo.ProductVersion != " ") || !(versionInfo.ProductVersion != assemblyName.Version.ToString())) ? "" : (" / v" + versionInfo.ProductVersion), (!(versionInfo.FileVersion != " ") || !(versionInfo.FileVersion != assemblyName.Version.ToString()) || !(versionInfo.FileVersion != versionInfo.ProductVersion)) ? "" : (" / v" + versionInfo.FileVersion));
				if (assemblyWhitelist.Contains(assemblyName.Name))
				{
					text5 = "Stock assembly: " + text5;
				}
				else
				{
					modded = true;
					loaderInfo.Add(text5);
					loadedModsInfo.Add(text5);
				}
				text4 = text4 + text5 + "\n";
			}
		}
		text += text4;
		string text6 = "\nFolders and files in GameData:\n";
		string[] directories = Directory.GetDirectories(KSPUtil.ApplicationRootPath + "GameData");
		for (int i = 0; i < directories.Length; i++)
		{
			string text7 = new DirectoryInfo(directories[i]).Name;
			if (folderWhitelist.Contains(text7))
			{
				text7 = "Stock folder: " + text7 + "\n";
			}
			else
			{
				modded = true;
				loaderInfo.Add(text7);
				loadedModsInfo.Add(text7);
			}
			text6 = text6 + text7 + "\n";
		}
		directories = Directory.GetFiles(KSPUtil.ApplicationRootPath + "GameData");
		for (int i = 0; i < directories.Length; i++)
		{
			string text8 = new FileInfo(directories[i]).Name;
			if (folderWhitelist.Contains(text8))
			{
				text8 = "Stock file: " + text8 + "\n";
			}
			else
			{
				modded = true;
				loaderInfo.Add(text8);
				loadedModsInfo.Add(text8);
			}
			text6 = text6 + text8 + "\n";
		}
		text = text + text6 + "\n************************************************************************\n";
		UnityEngine.Debug.Log(text);
		AddonLoader.Instance.StartAddons(KSPAddon.Startup.Instantly);
		list = SetupMainLoaders();
		_root = new UrlDir(urlConfig.ToArray(), list.ToArray());
		translateLoadedNodes();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (UrlDir.UrlFile file6 in root.GetFiles(UrlDir.FileType.Audio))
		{
			if (file6 != null)
			{
				num++;
			}
		}
		foreach (UrlDir.UrlFile file7 in root.GetFiles(UrlDir.FileType.Texture))
		{
			if (file7 != null)
			{
				num2++;
			}
		}
		foreach (UrlDir.UrlFile file8 in root.GetFiles(UrlDir.FileType.Model))
		{
			if (file8 != null)
			{
				num3++;
			}
		}
		float delta = 1f / (float)(num + num3 + num2);
		progressFraction = 0f;
		float nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
		bool loadTextures = true;
		bool loadAudio = true;
		bool loadParts = true;
		foreach (UrlDir.UrlFile file3 in root.GetFiles(UrlDir.FileType.Audio))
		{
			if (ExistsAudioClip(file3.url))
			{
				if (!(file3.fileTime > lastLoadTime))
				{
					continue;
				}
				UnityEngine.Debug.Log("Load(Audio): " + file3.url + " OUT OF DATE");
				RemoveAudioClip(file3.url);
				PartLoader.Instance.Recompile = true;
			}
			progressTitle = file3.url;
			UnityEngine.Debug.Log("Load(Audio): " + file3.url);
			foreach (DatabaseLoader<AudioClip> loader3 in loadersAudio)
			{
				if (loader3.extensions.Contains(file3.fileExtension))
				{
					if (loadAudio)
					{
						yield return StartCoroutine(loader3.Load(file3, new FileInfo(file3.fullPath)));
					}
					if (loader3.successful)
					{
						loader3.obj.name = file3.url;
						databaseAudio.Add(loader3.obj);
						databaseAudioFiles.Add(file3);
					}
				}
				if (!(Time.realtimeSinceStartup <= nextFrameTime))
				{
					nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
					yield return null;
				}
			}
			progressFraction += delta;
		}
		foreach (UrlDir.UrlFile file3 in root.GetFiles(UrlDir.FileType.Texture))
		{
			if (ExistsTexture(file3.url))
			{
				if (!(file3.fileTime > lastLoadTime))
				{
					continue;
				}
				UnityEngine.Debug.Log("Load(Texture): " + file3.url + " OUT OF DATE");
				RemoveTexture(file3.url);
				PartLoader.Instance.Recompile = true;
			}
			progressTitle = file3.url;
			UnityEngine.Debug.Log("Load(Texture): " + file3.url);
			foreach (DatabaseLoader<TextureInfo> loader2 in loadersTexture)
			{
				if (loader2.extensions.Contains(file3.fileExtension))
				{
					_recompileModels = true;
					if (loadTextures)
					{
						yield return StartCoroutine(loader2.Load(file3, new FileInfo(file3.fullPath)));
					}
					if (loader2.successful)
					{
						loader2.obj.name = file3.url;
						loader2.obj.texture.name = file3.url;
						databaseTexture.Add(loader2.obj);
					}
				}
				if (!(Time.realtimeSinceStartup <= nextFrameTime))
				{
					nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
					yield return null;
				}
			}
			progressFraction += delta;
		}
		if (_recompileModels)
		{
			foreach (GameObject item in databaseModel)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			databaseModel.Clear();
		}
		foreach (UrlDir.UrlFile file3 in root.GetFiles(UrlDir.FileType.Model))
		{
			if (ExistsModel(file3.url))
			{
				if (!(file3.fileTime > lastLoadTime))
				{
					continue;
				}
				UnityEngine.Debug.Log("Load(Model): " + file3.url + " OUT OF DATE");
				RemoveModel(file3.url);
				PartLoader.Instance.Recompile = true;
			}
			progressTitle = file3.url;
			UnityEngine.Debug.Log("Load(Model): " + file3.url);
			foreach (DatabaseLoader<GameObject> loader in loadersModel)
			{
				if (loader.extensions.Contains(file3.fileExtension))
				{
					if (loadParts)
					{
						yield return StartCoroutine(loader.Load(file3, new FileInfo(file3.fullPath)));
					}
					if (loader.successful)
					{
						GameObject obj = loader.obj;
						obj.transform.name = file3.url;
						obj.transform.parent = base.transform;
						obj.transform.localPosition = Vector3.zero;
						obj.transform.localRotation = Quaternion.identity;
						obj.SetActive(value: false);
						databaseModel.Add(obj);
						databaseModelFiles.Add(file3);
					}
				}
				if (!(Time.realtimeSinceStartup <= nextFrameTime))
				{
					nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
					yield return null;
				}
			}
			progressFraction += delta;
		}
		lastLoadTime = KSPUtil.SystemDateTime.DateTimeNow();
		progressFraction = 1f;
	}

	public void CleanupLoaders()
	{
		foreach (DatabaseLoader<AudioClip> item in loadersAudio)
		{
			item?.CleanUp();
		}
		loadersAudio = null;
		foreach (DatabaseLoader<TextureInfo> item2 in loadersTexture)
		{
			item2?.CleanUp();
		}
		loadersTexture = null;
		foreach (DatabaseLoader<GameObject> item3 in loadersModel)
		{
			item3?.CleanUp();
		}
		loadersModel = null;
	}

	public void LoadAssetBlacklist()
	{
		AssetLoader.AssetBlacklist.Clear();
		ConfigNode[] configNodes = Instance.GetConfigNodes("ASSETBUNDLE_BLACKLIST");
		for (int i = 0; i < configNodes.Length; i++)
		{
			foreach (ConfigNode.Value value in configNodes[i].values)
			{
				AssetLoader.AssetBlacklist.Add(value.value);
			}
		}
	}

	public IEnumerator LoadAssetBundleObjects()
	{
		UnityEngine.Debug.Log("Loading Asset Bundle Definitions");
		progressTitle = Localizer.Format("#autoLOC_158628");
		LoadAssetBlacklist();
		yield return StartCoroutine(AssetLoader.Instance.LoadDefinitionsAsync());
		List<AssetDefinition> list = new List<AssetDefinition>();
		for (int i = 0; i < AssetLoader.AssetDefinitions.Count; i++)
		{
			if (AssetLoader.AssetDefinitions[i].autoLoad)
			{
				list.Add(AssetLoader.AssetDefinitions[i]);
			}
		}
		progressTitle = Localizer.Format("#autoLOC_158641", list.Count);
		assetLoader = null;
		if (!AssetLoader.LoadAssets(delegate(AssetLoader.Loader l)
		{
			assetLoader = l;
		}, list.ToArray()))
		{
			yield break;
		}
		while (assetLoader == null)
		{
			yield return null;
		}
		for (int j = 0; j < assetLoader.definitions.Length; j++)
		{
			AssetDefinition assetDefinition = assetLoader.definitions[j];
			UnityEngine.Object @object = assetLoader.objects[j];
			if (!(@object == null))
			{
				switch (assetDefinition.type)
				{
				case "Shader":
					databaseShaders.Add(@object as Shader);
					break;
				case "Texture2D":
					databaseTexture.Add(new TextureInfo(null, @object as Texture2D, isNormalMap: false, isReadable: false, isCompressed: true));
					break;
				case "AudioClip":
					databaseAudio.Add(@object as AudioClip);
					break;
				case "GameObject":
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(@object as GameObject);
					gameObject.name = assetDefinition.path;
					gameObject.transform.SetParent(base.transform);
					gameObject.SetActive(value: false);
					databaseModel.Add(gameObject);
					break;
				}
				}
			}
		}
	}

	[ContextMenu("Write Debug Log")]
	public void WriteDebugLog()
	{
		if (Application.isPlaying && isReady)
		{
			File.WriteAllText(Path.Combine(Application.dataPath, "GameDatabaseLog.txt"), ToString());
		}
	}

	public override string ToString()
	{
		string text = "GameDatabase\r\n\r\n";
		foreach (UrlDir.UrlFile file in root.GetFiles(UrlDir.FileType.Audio))
		{
			text = text + "Audio: " + file.url + "\r\n";
		}
		foreach (UrlDir.UrlFile file2 in root.GetFiles(UrlDir.FileType.Texture))
		{
			text = text + "Texture: " + file2.url + "\r\n";
		}
		foreach (UrlDir.UrlFile file3 in root.GetFiles(UrlDir.FileType.Model))
		{
			text = text + "Model: " + file3.url + "\r\n";
		}
		foreach (UrlDir.UrlConfig config in root.GetConfigs("RESOURCE_DEFINITION"))
		{
			text = text + config.type + " " + config.url + "\r\n";
		}
		foreach (UrlDir.UrlConfig config2 in root.GetConfigs("PROP"))
		{
			text = text + config2.type + " " + config2.url + "\r\n";
		}
		foreach (UrlDir.UrlConfig config3 in root.GetConfigs("INTERNAL"))
		{
			text = text + config3.type + " " + config3.url + "\r\n";
		}
		foreach (UrlDir.UrlConfig config4 in root.GetConfigs("PART"))
		{
			text = text + config4.type + " " + config4.url + "\r\n";
		}
		foreach (UrlDir.UrlConfig config5 in root.GetConfigs("PHYSICMATERIAL_DEFINITION"))
		{
			text = text + config5.type + " " + config5.url + "\r\n";
		}
		foreach (UrlDir.UrlConfig config6 in root.GetConfigs("FIREWORKFX_DEFINITION"))
		{
			text = text + config6.type + " " + config6.url + "\r\n";
		}
		return text;
	}

	public static void CompileConfig(ConfigNode node)
	{
		CompileConfigRecursive(node);
	}

	public static void CompileConfigRecursive(ConfigNode node)
	{
		if (node.HasValue("config"))
		{
			ConfigNode configNode = new ConfigNode(node.name);
			string[] values = node.GetValues("config");
			node.RemoveValues("config");
			int i = 0;
			for (int num = values.Length; i < num; i++)
			{
				UnityEngine.Debug.Log("CloneURL: " + values);
				string text = values[i];
				ConfigNode configNode2 = Instance.GetConfigNode(text);
				if (configNode2 == null)
				{
					UnityEngine.Debug.LogError("ConfigNode: Cannot compile from config '" + text + "' as it does not exist");
					continue;
				}
				CompileConfigRecursive(configNode2);
				ConfigNode.Merge(configNode, configNode2);
				foreach (ConfigNode node2 in configNode2.nodes)
				{
					CompileConfigRecursive(node2);
				}
			}
			ConfigNode.Merge(node, configNode);
		}
		for (int j = 0; j < node.nodes.Count; j++)
		{
			CompileConfigRecursive(node.nodes[j]);
		}
	}
}
