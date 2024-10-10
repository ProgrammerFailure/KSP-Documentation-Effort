using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UrlDir
{
	[Flags]
	public enum DirectoryType
	{
		Parts = 1,
		Internals = 2,
		GameData = 3
	}

	public enum FileType
	{
		Unknown,
		Config,
		Texture,
		Model,
		Audio,
		Assembly,
		AssetBundle
	}

	[Serializable]
	public class ConfigDirectory
	{
		public string directory;

		public string urlRoot;

		public DirectoryType type;

		public ConfigDirectory()
		{
			urlRoot = "";
			directory = ".";
			type = DirectoryType.Parts;
		}

		public ConfigDirectory(string urlRoot, string directory, DirectoryType type)
		{
			this.urlRoot = urlRoot;
			this.directory = directory;
			this.type = type;
		}
	}

	public class ConfigFileType : IEnumerable
	{
		public FileType type;

		public List<string> extensions;

		public int Count => extensions.Count;

		public ConfigFileType()
		{
			type = FileType.Unknown;
			extensions = new List<string>();
		}

		public ConfigFileType(FileType fileType)
		{
			type = fileType;
			extensions = new List<string>();
		}

		public ConfigFileType(FileType fileType, string[] extensions)
		{
			type = fileType;
			this.extensions = new List<string>(extensions);
		}

		public IEnumerator GetEnumerator()
		{
			return extensions.GetEnumerator();
		}
	}

	public class UrlIdentifier
	{
		[HideInInspector]
		[SerializeField]
		public string _url;

		[SerializeField]
		[HideInInspector]
		public string[] _urlSplit;

		[SerializeField]
		[HideInInspector]
		public int _urlDepth;

		public string url
		{
			get
			{
				return _url;
			}
			set
			{
				ConstructURL(value);
			}
		}

		public string[] urlSplit => _urlSplit;

		public int urlDepth => _urlDepth;

		public string this[int index] => urlSplit[index];

		public UrlIdentifier()
		{
			_url = "";
			_urlSplit = new string[0];
			_urlDepth = -1;
		}

		public UrlIdentifier(string url)
		{
			ConstructURL(url);
		}

		public void ConstructURL(string url)
		{
			_url = url;
			_urlSplit = UrlSplit(url);
			_urlDepth = _urlSplit.Length - 1;
		}

		public static string[] UrlSplit(string url)
		{
			url = url.Trim('/', ' ', '\n', '\r', '\t');
			return url.Split(new char[3] { '/', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}

	public class UrlConfig
	{
		[SerializeField]
		public string _name;

		[SerializeField]
		public string _type;

		[SerializeField]
		public ConfigNode _config;

		[HideInInspector]
		[SerializeField]
		public UrlFile _parent;

		public string name => _name;

		public string type => _type;

		public ConfigNode config
		{
			get
			{
				return _config;
			}
			set
			{
				_config = value;
			}
		}

		public UrlFile parent => _parent;

		public string url => parent.url + "/" + name;

		public UrlConfig(UrlFile parent, ConfigNode node)
		{
			_type = node.name;
			_parent = parent;
			if (node.name == string.Empty)
			{
				node.name = parent.name;
			}
			if (node.HasValue("name"))
			{
				_name = node.GetValue("name");
			}
			else
			{
				_name = node.name;
			}
			_config = node;
		}

		public static List<UrlConfig> CreateNodeList(UrlDir parentDir, UrlFile parent)
		{
			ConfigNode configNode = ConfigNode.Load(parent.fullPath);
			if (configNode == null)
			{
				Debug.LogWarning("Cannot create config from file '" + parent.fullPath + "'.");
				return new List<UrlConfig>();
			}
			if (!configNode.HasData)
			{
				Debug.LogWarning("Config in file '" + parent.fullPath + "' contains no data.");
				return new List<UrlConfig>();
			}
			List<UrlConfig> list = new List<UrlConfig>();
			if (configNode.HasValue() && parentDir.type == DirectoryType.Parts)
			{
				configNode.name = "PART";
				list.Add(new UrlConfig(parent, configNode));
			}
			else
			{
				int i = 0;
				for (int count = configNode.nodes.Count; i < count; i++)
				{
					ConfigNode configNode2 = configNode.nodes[i];
					if (configNode2.name == string.Empty)
					{
						if (parentDir.type != DirectoryType.Parts)
						{
							Debug.LogWarning("Config in file '" + parent.fullPath + "' contains an unnamed node. Skipping.");
							continue;
						}
						configNode2.name = "PART";
					}
					list.Add(new UrlConfig(parent, configNode2));
				}
			}
			return list;
		}
	}

	public class UrlFile
	{
		[SerializeField]
		public string _name;

		[SerializeField]
		public FileType _fileType;

		[SerializeField]
		public string _path;

		[SerializeField]
		public string _fileExtension;

		[SerializeField]
		public DateTime _fileTime;

		[SerializeField]
		[HideInInspector]
		public UrlDir _root;

		[HideInInspector]
		[SerializeField]
		public UrlDir _parent;

		[SerializeField]
		public List<UrlConfig> _configs;

		public string name => _name;

		public FileType fileType => _fileType;

		public string fullPath => _path;

		public string fileExtension => _fileExtension;

		public DateTime fileTime => _fileTime;

		public UrlDir root => _root;

		public UrlDir parent => _parent;

		public List<UrlConfig> configs => _configs;

		public string url => parent.url + "/" + name;

		public UrlFile(UrlDir parent, FileInfo info)
		{
			_name = Path.GetFileNameWithoutExtension(info.Name);
			_path = info.FullName;
			_fileExtension = Path.GetExtension(info.Name);
			try
			{
				_fileTime = info.LastWriteTime;
			}
			catch (Exception)
			{
				_fileTime = new DateTime(1, 1, 1);
			}
			if (_fileExtension.Length > 1)
			{
				_fileExtension = _fileExtension.Substring(1);
			}
			_parent = parent;
			_root = parent.root;
			_configs = new List<UrlConfig>();
			if (_fileExtension == "cfg")
			{
				_fileType = FileType.Config;
				_configs = UrlConfig.CreateNodeList(parent, this);
			}
			else
			{
				_fileType = FileType.Unknown;
			}
		}

		public bool Exists(string url)
		{
			return Exists(new UrlIdentifier(url), 0);
		}

		public bool Exists(UrlIdentifier url, int index)
		{
			if (url.urlDepth == -1)
			{
				return false;
			}
			if (index == url.urlDepth)
			{
				if (GetConfig(url[index]) == null)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public UrlConfig GetConfig(string name)
		{
			int num = 0;
			int count = configs.Count;
			while (true)
			{
				if (num < count)
				{
					if (configs[num].name == name)
					{
						break;
					}
					num++;
					continue;
				}
				Debug.LogError("Cannot find config in file : " + name);
				return null;
			}
			return configs[num];
		}

		public bool ContainsConfig(string name)
		{
			return GetConfig(name) != null;
		}

		public void ConfigureFile(ConfigFileType[] fileConfig)
		{
			if (_fileType != 0)
			{
				return;
			}
			int i = 0;
			for (int num = fileConfig.Length; i < num; i++)
			{
				ConfigFileType configFileType = fileConfig[i];
				int j = 0;
				for (int count = configFileType.extensions.Count; j < count; j++)
				{
					if (configFileType.extensions[j] == fileExtension)
					{
						_fileType = configFileType.type;
						return;
					}
				}
			}
		}

		public void SaveConfigs()
		{
			if (_fileType != FileType.Config)
			{
				Debug.Log("Cannot save as file is not of type Config");
				return;
			}
			ConfigNode configNode = new ConfigNode();
			int i = 0;
			for (int count = configs.Count; i < count; i++)
			{
				configNode.AddNode(configs[i].config);
			}
			configNode.Save(fullPath);
		}

		public void AddConfig(ConfigNode newConfig)
		{
			if (_fileType != FileType.Config)
			{
				Debug.Log("Cannot add config as file is not of type Config");
			}
			else
			{
				configs.Add(new UrlConfig(this, newConfig));
			}
		}
	}

	public const string configExtension = "cfg";

	[SerializeField]
	public string _name;

	[SerializeField]
	[HideInInspector]
	public UrlDir _root;

	[SerializeField]
	[HideInInspector]
	public UrlDir _parent;

	[SerializeField]
	public List<UrlDir> _children;

	[SerializeField]
	public List<UrlFile> _files;

	[SerializeField]
	public string _path;

	[SerializeField]
	public DirectoryType _type = DirectoryType.GameData;

	public static string selectiveExpansionstring;

	public static bool selectiveExpansionLoad = false;

	public static string selectedExpansionFolders;

	public static string[] selectedExpansions;

	public static string limitedPartsLoadString;

	public static bool limitedPartsLoad = false;

	public static string disabledPartFoldersString;

	public static string[] disabledPartsFolders;

	public static bool logSkippedGameDataLoading = true;

	public static bool setupSelectiveLoadingVars = false;

	public string name => _name;

	public UrlDir root => _root;

	public UrlDir parent => _parent;

	public List<UrlDir> children => _children;

	public List<UrlFile> files => _files;

	public string path => _path;

	public DirectoryType type => _type;

	public string url
	{
		get
		{
			if (parent != null && parent != root && parent.name != string.Empty)
			{
				return parent.url + "/" + name;
			}
			return name;
		}
	}

	public IEnumerable<UrlDir> AllDirectories
	{
		get
		{
			int i = 0;
			int iC = children.Count;
			while (i < iC)
			{
				UrlDir child = children[i];
				yield return child;
				foreach (UrlDir allDirectory in child.AllDirectories)
				{
					yield return allDirectory;
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	public IEnumerable<UrlFile> AllFiles
	{
		get
		{
			foreach (UrlDir child in AllDirectories)
			{
				int i = 0;
				int jC = child.files.Count;
				while (i < jC)
				{
					if (child.files[i].fileType != FileType.Config)
					{
						yield return child.files[i];
					}
					int num = i + 1;
					i = num;
				}
			}
		}
	}

	public IEnumerable<UrlFile> AllConfigFiles
	{
		get
		{
			foreach (UrlDir child in AllDirectories)
			{
				int i = 0;
				int jC = child.files.Count;
				while (i < jC)
				{
					if (child.files[i].fileType == FileType.Config)
					{
						yield return child.files[i];
					}
					int num = i + 1;
					i = num;
				}
			}
		}
	}

	public IEnumerable<UrlConfig> AllConfigs
	{
		get
		{
			foreach (UrlFile file in AllConfigFiles)
			{
				int i = 0;
				int jC = file.configs.Count;
				while (i < jC)
				{
					yield return file.configs[i];
					int num = i + 1;
					i = num;
				}
			}
		}
	}

	public static string ApplicationRootPath
	{
		get
		{
			if (Application.platform != RuntimePlatform.OSXPlayer)
			{
				return Application.dataPath + "/../";
			}
			return Application.dataPath + "/../../";
		}
	}

	public UrlDir(ConfigDirectory[] dirConfig, ConfigFileType[] fileConfig)
	{
		_files = new List<UrlFile>();
		_children = new List<UrlDir>();
		_parent = null;
		_root = this;
		_name = "root";
		int i = 0;
		for (int num = dirConfig.Length; i < num; i++)
		{
			_children.Add(new UrlDir(this, dirConfig[i]));
		}
		foreach (UrlFile allFile in AllFiles)
		{
			allFile.ConfigureFile(fileConfig);
		}
	}

	public UrlDir(UrlDir root, ConfigDirectory rootInfo)
	{
		DirectoryInfo info = Directory.CreateDirectory(CreateApplicationPath(rootInfo.directory));
		_name = rootInfo.urlRoot;
		_type = rootInfo.type;
		Create(root, info);
	}

	public UrlDir(UrlDir parent, DirectoryInfo info)
	{
		_name = info.Name;
		_type = parent.type;
		Create(parent, info);
	}

	public void Create(UrlDir parent, DirectoryInfo info)
	{
		_path = info.FullName;
		_parent = parent;
		_root = parent.root;
		_files = new List<UrlFile>();
		_children = new List<UrlDir>();
		FileInfo[] array = info.GetFiles();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			_files.Add(new UrlFile(this, array[i]));
		}
		DirectoryInfo[] directories = info.GetDirectories();
		int j = 0;
		for (int num2 = directories.Length; j < num2; j++)
		{
			DirectoryInfo directoryInfo = directories[j];
			if (!(directoryInfo.Name == ".svn") && !(directoryInfo.Name == "PluginData") && !(directoryInfo.Name == "zDeprecated"))
			{
				_children.Add(new UrlDir(this, directoryInfo));
			}
		}
	}

	public bool DirectoryExists(string url)
	{
		return GetDirectory(url) != null;
	}

	public UrlDir GetDirectory(string url)
	{
		if (new UrlIdentifier(url).urlDepth == -1)
		{
			return null;
		}
		return GetDirectory(new UrlIdentifier(url), 0);
	}

	public UrlDir GetDirectory(UrlIdentifier id, int index)
	{
		if (index == id.urlDepth)
		{
			int i = 0;
			for (int count = children.Count; i < count; i++)
			{
				if (id[index] == children[i].name)
				{
					return children[i];
				}
			}
		}
		else
		{
			int j = 0;
			for (int count2 = children.Count; j < count2; j++)
			{
				if (id[index] == children[j].name)
				{
					return children[j].GetDirectory(id, index + 1);
				}
			}
		}
		return null;
	}

	public UrlDir CreateDirectory(string urlDir)
	{
		if (new UrlIdentifier(url).urlDepth == -1)
		{
			return null;
		}
		return CreateDirectory(new UrlIdentifier(url), 0);
	}

	public UrlDir CreateDirectory(UrlIdentifier id, int index)
	{
		if (index == id.urlDepth)
		{
			int num = 0;
			int count = children.Count;
			while (true)
			{
				if (num < count)
				{
					if (id[index] == children[num].name)
					{
						break;
					}
					num++;
					continue;
				}
				DirectoryInfo info = Directory.CreateDirectory(Path.Combine(path, id[index]));
				UrlDir urlDir = new UrlDir(this, info);
				children.Add(urlDir);
				return urlDir;
			}
			return children[num];
		}
		int num2 = 0;
		int count2 = children.Count;
		while (true)
		{
			if (num2 < count2)
			{
				if (id[index] == children[num2].name)
				{
					break;
				}
				num2++;
				continue;
			}
			return null;
		}
		return children[num2].CreateDirectory(id, index + 1);
	}

	public bool FileExists(string url)
	{
		return GetFile(url) != null;
	}

	public UrlFile GetFile(string url)
	{
		if (new UrlIdentifier(url).urlDepth == -1)
		{
			return null;
		}
		return GetFile(new UrlIdentifier(url), 0);
	}

	public UrlFile GetFile(UrlIdentifier id, int index)
	{
		if (index == id.urlDepth - 1)
		{
			int i = 0;
			for (int count = files.Count; i < count; i++)
			{
				if (id[index] == files[i].name)
				{
					return files[i];
				}
			}
		}
		else
		{
			int j = 0;
			for (int count2 = children.Count; j < count2; j++)
			{
				if (id[index] == children[j].name)
				{
					return children[j].GetFile(id, index + 1);
				}
			}
		}
		return null;
	}

	public bool ConfigExists(string url)
	{
		return GetConfig(url) != null;
	}

	public UrlConfig GetConfig(string url)
	{
		UrlIdentifier urlIdentifier = new UrlIdentifier(url);
		if (urlIdentifier.urlDepth == -1)
		{
			Debug.LogError("Invalid url: '" + url + "'");
			return null;
		}
		return GetConfig(urlIdentifier, 0);
	}

	public UrlConfig GetConfig(UrlIdentifier id, int index)
	{
		if (index == id.urlDepth)
		{
			int i = 0;
			for (int count = files.Count; i < count; i++)
			{
				UrlFile urlFile = files[i];
				if (urlFile.fileType == FileType.Config && urlFile.ContainsConfig(id[index]))
				{
					return urlFile.GetConfig(id[index]);
				}
			}
		}
		else
		{
			int j = 0;
			for (int count2 = children.Count; j < count2; j++)
			{
				UrlDir urlDir = children[j];
				if (urlDir.name != string.Empty)
				{
					if (id[index] == urlDir.name)
					{
						return urlDir.GetConfig(id, index + 1);
					}
					continue;
				}
				UrlConfig config = urlDir.GetConfig(id, index);
				if (config != null)
				{
					return config;
				}
			}
			int k = 0;
			for (int count3 = files.Count; k < count3; k++)
			{
				UrlFile urlFile2 = files[k];
				if (urlFile2.fileType == FileType.Config && id[index] == urlFile2.name)
				{
					return urlFile2.GetConfig(id[index + 1]);
				}
			}
		}
		return null;
	}

	public IEnumerable<UrlFile> GetFiles(FileType type)
	{
		foreach (UrlFile allFile in AllFiles)
		{
			if (allFile.fileType == type)
			{
				yield return allFile;
			}
		}
	}

	public IEnumerable<UrlConfig> GetConfigs(string typeName, bool recursive = true)
	{
		if (recursive)
		{
			foreach (UrlConfig allConfig in AllConfigs)
			{
				if (allConfig.type == typeName)
				{
					yield return allConfig;
				}
			}
			yield break;
		}
		int i = 0;
		int iC = files.Count;
		while (i < iC)
		{
			UrlFile file = files[i];
			int j = 0;
			int jC = file.configs.Count;
			int num;
			while (j < jC)
			{
				UrlConfig urlConfig = file.configs[j];
				if (urlConfig.type == typeName)
				{
					yield return urlConfig;
				}
				num = j + 1;
				j = num;
			}
			num = i + 1;
			i = num;
		}
	}

	public IEnumerable<UrlConfig> GetConfigs(string typeName, string name, bool recursive = true)
	{
		if (recursive)
		{
			foreach (UrlConfig allConfig in AllConfigs)
			{
				if (allConfig.type == typeName && allConfig.name == name)
				{
					yield return allConfig;
				}
			}
			yield break;
		}
		int i = 0;
		int iC = files.Count;
		while (i < iC)
		{
			UrlFile file = files[i];
			int j = 0;
			int jC = file.configs.Count;
			int num;
			while (j < jC)
			{
				UrlConfig urlConfig = file.configs[j];
				if (urlConfig.type == typeName && urlConfig.name == name)
				{
					yield return urlConfig;
				}
				num = j + 1;
				j = num;
			}
			num = i + 1;
			i = num;
		}
	}

	public static string CreateApplicationPath(string relativePath)
	{
		return Path.Combine(ApplicationRootPath, relativePath);
	}

	public static string StripExtension(string filename, string extension)
	{
		return filename.Remove(filename.Length - extension.Length);
	}

	public static string PathCombine(string a, string b)
	{
		return Path.Combine(a, b);
	}
}
