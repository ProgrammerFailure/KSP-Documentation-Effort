using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Experience;
using KSPAssets.Loaders;
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
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextureInfo(UrlDir.UrlFile file, Texture2D texture, bool isNormalMap, bool isReadable, bool isCompressed)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCreateDatabase_003Ed__71 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameDatabase _003C_003E4__this;

		private float _003CstartTime_003E5__2;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CCreateDatabase_003Ed__71(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadObjects_003Ed__90 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameDatabase _003C_003E4__this;

		private float _003Cdelta_003E5__2;

		private float _003CnextFrameTime_003E5__3;

		private bool _003CloadTextures_003E5__4;

		private bool _003CloadAudio_003E5__5;

		private bool _003CloadParts_003E5__6;

		private IEnumerator<UrlDir.UrlFile> _003C_003E7__wrap6;

		private UrlDir.UrlFile _003Cfile_003E5__8;

		private List<DatabaseLoader<AudioClip>>.Enumerator _003C_003E7__wrap8;

		private DatabaseLoader<AudioClip> _003Cloader_003E5__10;

		private List<DatabaseLoader<TextureInfo>>.Enumerator _003C_003E7__wrap10;

		private DatabaseLoader<TextureInfo> _003Cloader_003E5__12;

		private List<DatabaseLoader<GameObject>>.Enumerator _003C_003E7__wrap12;

		private DatabaseLoader<GameObject> _003Cloader_003E5__14;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CLoadObjects_003Ed__90(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally2()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally3()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally4()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally5()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally6()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadAssetBundleObjects_003Ed__94 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameDatabase _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CLoadAssetBundleObjects_003Ed__94(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	private Dictionary<string, string> flagSwaps;

	[SerializeField]
	private string _settingsFileName;

	[SerializeField]
	private List<UrlDir.ConfigDirectory> urlConfig;

	[SerializeField]
	private string pluginDataFolder;

	[SerializeField]
	private List<string> _assemblyTypes;

	[SerializeField]
	public List<AudioClip> databaseAudio;

	[SerializeField]
	public List<UrlDir.UrlFile> databaseAudioFiles;

	[SerializeField]
	public List<TextureInfo> databaseTexture;

	[SerializeField]
	public List<GameObject> databaseModel;

	[SerializeField]
	private ExperienceSystemConfig experienceConfigs;

	[SerializeField]
	public List<UrlDir.UrlFile> databaseModelFiles;

	[SerializeField]
	public List<Shader> databaseShaders;

	[SerializeField]
	[HideInInspector]
	private UrlDir _root;

	private static GameDatabase _instance;

	private bool isReady;

	private string progressTitle;

	private float progressFraction;

	private List<DatabaseLoader<AudioClip>> loadersAudio;

	private List<DatabaseLoader<TextureInfo>> loadersTexture;

	private List<DatabaseLoader<GameObject>> loadersModel;

	internal static IntPtr intPtr;

	private static bool modded;

	private static string environemntInfo;

	private static HashSet<string> loaderInfo;

	internal static List<string> loadedModsInfo;

	private string[] assemblyWhitelist;

	private string[] folderWhitelist;

	private AssetLoader.Loader assetLoader;

	private bool _recompileModels;

	private bool _recompile;

	private DateTime lastLoadTime;

	public string settingsFileName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string PluginDataFolder
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ExperienceSystemConfig ExperienceConfigs
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UrlDir root
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static GameDatabase Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool Modded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string EnvironmentInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool RecompileModels
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool Recompile
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GameDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ExistsShader(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Shader GetShader(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveShader(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ExistsAudioClip(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AudioClip GetAudioClip(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<AudioClip> AudioClipNameContains(string nameContains)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlDir.UrlFile GetAudioFile(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveAudioClip(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ExistsTexture(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlDir.UrlFile GetTextureFile(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextureInfo GetTextureInfo(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextureInfo GetTextureInfoIn(string url, string textureName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Texture2D GetTexture(string url, bool asNormalMap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Texture2D GetTextureIn(string url, string textureName, bool asNormalMap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveTexture(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ReplaceTexture(string url, TextureInfo newTex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<TextureInfo> GetAllTexturesInFolderType(string folderName, bool caseInsensitive = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<TextureInfo> GetAllTexturesInFolder(string folderURL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D BitmapToUnityNormalMap(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ExistsModel(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject GetModel(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject GetModelPrefab(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlDir.UrlFile GetModelFile(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlDir.UrlFile GetModelFile(GameObject modelPrefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject GetModelPrefabIn(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject GetModelIn(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveModel(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ExistsConfigNode(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetConfigNode(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode[] GetConfigNodes(string typeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode[] GetConfigNodes(string baseUrl, string typeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetMergedConfigNodes(string nodeName, bool mergeChildren = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlDir.UrlConfig[] GetConfigs(string typeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
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
	public override bool IsReady()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ProgressTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override float ProgressFraction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void StartLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CCreateDatabase_003Ed__71))]
	private IEnumerator CreateDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<UrlDir.ConfigFileType> SetupAssemblyLoader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<UrlDir.ConfigFileType> SetupMainLoaders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadEmpty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UpdateLoaderInfo(Dictionary<string, bool> dest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SaveLoaderInfo(ConfigNode node, Dictionary<string, bool> dest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadLoaderInfo(ConfigNode node, Dictionary<string, bool> dest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void translateLoadedNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadObjects_003Ed__90))]
	private IEnumerator LoadObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CleanupLoaders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadAssetBlacklist()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadAssetBundleObjects_003Ed__94))]
	private IEnumerator LoadAssetBundleObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Write Debug Log")]
	private void WriteDebugLog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CompileConfig(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CompileConfigRecursive(ConfigNode node)
	{
		throw null;
	}
}
