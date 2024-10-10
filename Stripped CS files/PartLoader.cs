using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Expansions.Serenity;
using ns11;
using ns9;
using UnityEngine;
using UnityEngine.Rendering;

public class PartLoader : LoadingSystem
{
	public float progressDelta;

	public bool isReady;

	public string progressTitle = "";

	public float progressFraction;

	public ConfigNode partDatabase;

	public static string partDatabaseFileName = "PartDatabase.cfg";

	[SerializeField]
	public bool debugAlwaysRecreateDatabase;

	public List<AvailablePart> loadedParts;

	public List<AvailablePart> parts;

	public List<AvailableVariantTheme> loadedVariantThemes;

	public float shaderMultiplier = 2.25f;

	public int initialPartsLength;

	public int initialPropsLength;

	public int initialInternalPartsLength;

	public Dictionary<GameObject, AvailablePart> APFinderByIcon;

	public Dictionary<string, AvailablePart> APFinderByName;

	public List<AvailablePart> cargoParts;

	public List<AvailablePart> cargoInventoryParts;

	public List<AvailablePart> deployedScienceExperimentParts;

	public Dictionary<string, string> PartReplacements;

	public List<AvailablePart> robotArmScannerParts;

	public List<string> missingVariantThemes;

	public List<InternalProp> internalProps = new List<InternalProp>();

	public List<InternalModel> internalParts;

	public bool _recompile;

	public static PartLoader Instance { get; set; }

	public bool CargoPartsLoaded => cargoParts.Count > 0;

	public static List<AvailablePart> LoadedPartsList
	{
		get
		{
			if (!Instance)
			{
				return RDTestSceneLoader.Instance.partsList;
			}
			return Instance.loadedParts;
		}
	}

	public static List<AvailableVariantTheme> LoadedVariantThemesList
	{
		get
		{
			if (!Instance)
			{
				return null;
			}
			return Instance.loadedVariantThemes;
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

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		initialPartsLength = parts.Count;
		initialPropsLength = internalProps.Count;
		initialInternalPartsLength = internalParts.Count;
		APFinderByIcon = new Dictionary<GameObject, AvailablePart>();
		APFinderByName = new Dictionary<string, AvailablePart>();
		loadedVariantThemes = new List<AvailableVariantTheme>();
		cargoParts = new List<AvailablePart>();
		cargoInventoryParts = new List<AvailablePart>();
		deployedScienceExperimentParts = new List<AvailablePart>();
		PartReplacements = new Dictionary<string, string>();
		robotArmScannerParts = new List<AvailablePart>();
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
			Debug.LogError("PartLoader: Already started and finished. Aborting.");
		}
		else
		{
			StartCoroutine(CompileAll());
		}
	}

	public IEnumerator CompileAll()
	{
		if (_recompile)
		{
			ClearAll();
		}
		progressTitle = "";
		progressFraction = 0f;
		for (int i = 0; i < initialPartsLength; i++)
		{
			AvailablePart availablePart = new AvailablePart(parts[i]);
			availablePart.partPrefab.gameObject.SetActive(value: false);
			availablePart.partPrefab = UnityEngine.Object.Instantiate(availablePart.partPrefab);
			availablePart.partPrefab.transform.parent = base.transform;
			availablePart.partPrefab.gameObject.SetActive(value: false);
			if (availablePart.partPrefab.fxGroups != null)
			{
				for (int j = 0; j < availablePart.partPrefab.fxGroups.Count; j++)
				{
					if (availablePart.partPrefab.fxGroups[j].maxVisualPower == 0f)
					{
						availablePart.partPrefab.fxGroups[j].maxVisualPower = 1f;
					}
				}
			}
			if ((bool)FlightGlobals.fetch)
			{
				FlightGlobals.PersistentLoadedPartIds.Remove(availablePart.partPrefab.persistentId);
			}
			if (availablePart.iconPrefab != null)
			{
				availablePart.iconPrefab = UnityEngine.Object.Instantiate(availablePart.iconPrefab);
				availablePart.iconPrefab.transform.parent = base.transform;
				availablePart.iconPrefab.name = availablePart.partPrefab.name + " icon";
				availablePart.iconPrefab.gameObject.SetActive(value: false);
			}
			loadedParts.Add(availablePart);
		}
		UrlDir.UrlConfig[] configs = GameDatabase.Instance.GetConfigs("PART");
		UrlDir.UrlConfig[] allPropNodes = GameDatabase.Instance.GetConfigs("PROP");
		UrlDir.UrlConfig[] allSpaceNodes = GameDatabase.Instance.GetConfigs("INTERNAL");
		UrlDir.UrlConfig[] configs2 = GameDatabase.Instance.GetConfigs("VARIANTTHEME");
		int num = configs.Length + allPropNodes.Length + allSpaceNodes.Length;
		progressDelta = 1f / (float)num;
		InitializePartDatabase();
		APFinderByIcon.Clear();
		APFinderByName.Clear();
		CompileVariantThemes(configs2);
		yield return StartCoroutine(CompileParts(configs));
		yield return StartCoroutine(CompileInternalProps(allPropNodes));
		yield return StartCoroutine(CompileInternalSpaces(allSpaceNodes));
		SavePartDatabase();
		_recompile = false;
		PartUpgradeManager.Handler.LinkUpgrades();
		GameEvents.OnUpgradesLinked.Fire();
		isReady = true;
		GameEvents.OnPartLoaderLoaded.Fire();
	}

	public void InitializePartDatabase()
	{
		if (File.Exists(KSPUtil.ApplicationRootPath + partDatabaseFileName) && !debugAlwaysRecreateDatabase)
		{
			partDatabase = ConfigNode.Load(KSPUtil.ApplicationRootPath + partDatabaseFileName);
			if (partDatabase != null)
			{
				if (CheckPartDatabaseVersion())
				{
					Debug.Log("PartLoader: Loading part database");
					return;
				}
				Debug.Log("PartLoader: Version strings do not match. Creating part database.");
				CreatePartDatabase();
			}
			else
			{
				Debug.Log("PartLoader: Creating part database");
				CreatePartDatabase();
			}
		}
		else
		{
			Debug.Log("PartLoader: Creating part database");
			CreatePartDatabase();
		}
	}

	public void CreatePartDatabase()
	{
		partDatabase = new ConfigNode();
		ConfigNode configNode = partDatabase.AddNode("PART_DATABASE");
		if (!Application.isEditor)
		{
			configNode.AddValue("version", Versioning.VersionString);
		}
	}

	public bool CheckPartDatabaseVersion()
	{
		ConfigNode node = partDatabase.GetNode("PART_DATABASE");
		if (node == null)
		{
			return false;
		}
		string value = node.GetValue("version");
		if (value == null)
		{
			if (!Application.isEditor)
			{
				node.AddValue("version", Versioning.VersionString);
			}
			return true;
		}
		return value == Versioning.VersionString;
	}

	public void SavePartDatabase()
	{
		if (partDatabase != null)
		{
			partDatabase.Save(KSPUtil.ApplicationRootPath + partDatabaseFileName);
		}
	}

	public void OnDestroy()
	{
		SavePartDatabase();
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public ConfigNode GetDatabaseConfig(Part p)
	{
		ConfigNode configNode = partDatabase.GetNode("PART", "url", p.partInfo.partUrl);
		if (configNode == null)
		{
			Debug.Log("PartLoader: Part '" + p.partInfo.partUrl + "' has no database record. Creating.");
			configNode = new ConfigNode("PART");
			configNode.AddValue("url", p.partInfo.partUrl);
			SetDatabaseFilestamps(configNode, p);
			partDatabase.AddNode(configNode);
		}
		else if (!CheckPartDatabaseTimestamps(configNode, p.partInfo))
		{
			Debug.Log("PartLoader: Timestamps for part '" + p.partInfo.partUrl + "' do not match. Resetting.");
			SetDatabaseFilestamps(configNode, p);
			configNode.nodes.Clear();
		}
		return configNode;
	}

	public void SetDatabaseFilestamps(ConfigNode node, Part p)
	{
		string text = "";
		int i = 0;
		for (int count = p.partInfo.fileTimes.Count; i < count; i++)
		{
			if (text.Length != 0)
			{
				text += ", ";
			}
			text += p.partInfo.fileTimes[i].ToString();
		}
		node.SetValue("timestamps", text);
	}

	public ConfigNode GetDatabaseConfig(Part p, string nodeName)
	{
		return GetDatabaseConfig(p)?.GetNode(nodeName);
	}

	public void SetDatabaseConfig(Part p, ConfigNode newNode)
	{
		ConfigNode databaseConfig = GetDatabaseConfig(p);
		if (databaseConfig.HasNode(newNode.name))
		{
			databaseConfig.RemoveNode(newNode.name);
		}
		databaseConfig.AddNode(newNode);
	}

	public bool CheckPartDatabaseTimestamps(ConfigNode config, AvailablePart partInfo)
	{
		string value = config.GetValue("timestamps");
		if (value != null)
		{
			string[] array = value.Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (partInfo.fileTimes.Count != array.Length)
			{
				return false;
			}
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				if (array[i] != partInfo.fileTimes[i])
				{
					return false;
				}
			}
		}
		return true;
	}

	public void AddPartReplacement(string oldName, string newName)
	{
		if (!Instance.PartReplacements.ContainsKey(oldName))
		{
			Instance.PartReplacements.Add(oldName, newName);
		}
	}

	public static string GetPartReplacementName(string oldName)
	{
		string result = string.Empty;
		if (Instance.PartReplacements.ContainsKey(oldName))
		{
			result = Instance.PartReplacements[oldName];
		}
		return result;
	}

	public static AvailablePart getPartInfoByName(string name)
	{
		string key = name;
		if (DoesPartHaveReplacement(name) && !DoesPartExist(name))
		{
			key = GetPartReplacementName(name);
		}
		if (Instance.APFinderByName.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}

	public static bool DoesPartExist(string name)
	{
		return Instance.APFinderByName.ContainsKey(name);
	}

	public static bool DoesPartHaveReplacement(string name)
	{
		return Instance.PartReplacements.ContainsKey(name);
	}

	public static AvailablePart getPartInfoByPartPrefab(GameObject partPrefab)
	{
		int count = Instance.loadedParts.Count;
		AvailablePart availablePart;
		do
		{
			if (count-- > 0)
			{
				availablePart = Instance.loadedParts[count];
				continue;
			}
			return null;
		}
		while (!(availablePart.partPrefab == partPrefab));
		return availablePart;
	}

	public static AvailablePart getPartInfoByIconPrefab(GameObject iconPrefab)
	{
		if (Instance.APFinderByIcon.TryGetValue(iconPrefab, out var value))
		{
			return value;
		}
		return null;
	}

	public static AvailableVariantTheme GetVariantInfoByName(string variantName)
	{
		int num = 0;
		while (true)
		{
			if (num < Instance.loadedVariantThemes.Count)
			{
				if (Instance.loadedVariantThemes[num].name == variantName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return Instance.loadedVariantThemes[num];
	}

	public void CompileVariantThemes(UrlDir.UrlConfig[] allThemeNodes)
	{
		loadedVariantThemes = new List<AvailableVariantTheme>();
		missingVariantThemes = new List<string>();
		AvailableVariantTheme availableVariantTheme = null;
		int i = 0;
		for (int num = allThemeNodes.Length; i < num; i++)
		{
			availableVariantTheme = AvailableVariantTheme.CreateVariantTheme();
			availableVariantTheme.Load(allThemeNodes[i].config);
			loadedVariantThemes.Add(availableVariantTheme);
		}
	}

	public IEnumerator CompileParts(UrlDir.UrlConfig[] allPartNodes)
	{
		float nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
		int partNodesIdx = 0;
		int partNodesIdxC = allPartNodes.Length;
		while (partNodesIdx < partNodesIdxC)
		{
			UrlDir.UrlConfig urlConfig = allPartNodes[partNodesIdx];
			if (urlConfig != null)
			{
				Debug.Log("PartLoader: Compiling Part '" + urlConfig.url + "'");
				progressTitle = urlConfig.url;
				progressFraction += progressDelta;
				int num = -1;
				switch (urlConfig.config.GetValue("name"))
				{
				case "kerbalEVA":
					num = 0;
					break;
				case "kerbalEVASlimSuitFemale":
					num = 8;
					break;
				case "kerbalEVAfemaleFuture":
					num = 6;
					break;
				case "kerbalEVAFuture":
					num = 5;
					break;
				case "kerbalEVAfemale":
					num = 1;
					break;
				case "kerbalEVASlimSuit":
					num = 7;
					break;
				case "kerbalEVAfemaleVintage":
					num = 4;
					break;
				case "kerbalEVAVintage":
					num = 3;
					break;
				case "flag":
					num = 2;
					break;
				}
				if (num >= 0)
				{
					AvailablePart part = loadedParts[num];
					Part partPrefab = part.partPrefab;
					partPrefab.Awake();
					if ((bool)FlightGlobals.fetch)
					{
						FlightGlobals.PersistentLoadedPartIds.Remove(partPrefab.persistentId);
					}
					int i = 0;
					for (int count = urlConfig.config.values.Count; i < count; i++)
					{
						ApplyPartValue(partPrefab, urlConfig.config.values[i]);
					}
					partPrefab.Fields.SetOriginalValue();
					partPrefab.Modules[0].Awake();
					int j = 0;
					for (int count2 = urlConfig.config.nodes.Count; j < count2; j++)
					{
						ConfigNode configNode = urlConfig.config.nodes[j];
						switch (configNode.name)
						{
						case "EFFECTS":
							partPrefab.LoadEffects(configNode);
							break;
						case "DRAG_CUBE":
							partPrefab.DragCubes.LoadCubes(configNode);
							break;
						case "RESOURCE":
							partPrefab.AddResource(configNode);
							break;
						case "MODULE":
						{
							string value = configNode.GetValue("__OVERLOAD");
							if (!string.IsNullOrEmpty(value) && bool.Parse(value))
							{
								partPrefab.Modules[configNode.GetValue("name")].Load(configNode);
							}
							else
							{
								partPrefab.AddModule(configNode, forceAwake: true);
							}
							break;
						}
						}
					}
					int k = 0;
					for (int count3 = urlConfig.config.values.Count; k < count3; k++)
					{
						ConfigNode.Value value2 = urlConfig.config.values[k];
						if (value2.name.StartsWith("sound"))
						{
							LoadSound(urlConfig, value2.name, value2.value, partPrefab);
						}
					}
					partPrefab.gameObject.SetActive(value: false);
					if (partPrefab.gameObject.activeInHierarchy || partPrefab.gameObject.activeSelf)
					{
						Debug.LogError("[PartLoader]: Part " + part.name + " is still active!");
					}
				}
				else
				{
					AvailablePart part;
					try
					{
						part = ParsePart(urlConfig, urlConfig.config);
					}
					catch (Exception ex)
					{
						Debug.LogError("PartLoader: Encountered exception during compilation. " + ex);
						part = null;
					}
					if (part == null)
					{
						Debug.LogError("PartCompiler: Cannot compile part");
					}
					else
					{
						if ((bool)FlightGlobals.fetch)
						{
							FlightGlobals.PersistentLoadedPartIds.Remove(part.partPrefab.persistentId);
						}
						if (part.partPrefab.DragCubes.Cubes.Count != 0)
						{
							Instance.SetDatabaseConfig(part.partPrefab, part.partPrefab.DragCubes.SaveCubes());
						}
						else
						{
							yield return StartCoroutine(DragCubeSystem.Instance.SetupDragCubeCoroutine(part.partPrefab));
						}
						CompilePartInfo(part, part.partPrefab);
						if ((bool)FlightGlobals.fetch)
						{
							FlightGlobals.PersistentLoadedPartIds.Remove(part.partPrefab.persistentId);
						}
						loadedParts.Add(part);
						if (part.Variants != null && part.Variants.Count > 0)
						{
							AddVariants(part.Variants, part);
						}
						if (Time.realtimeSinceStartup > nextFrameTime)
						{
							nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
							yield return null;
						}
					}
				}
			}
			int num2 = partNodesIdx + 1;
			partNodesIdx = num2;
		}
		int l = 0;
		for (int count4 = loadedParts.Count; l < count4; l++)
		{
			AvailablePart part = loadedParts[l];
			if (part.partPrefab.gameObject.activeInHierarchy || part.partPrefab.gameObject.activeSelf)
			{
				Debug.LogError("[PartLoader]: Part " + part.name + " is still active!");
			}
			if (!APFinderByName.ContainsKey(part.name))
			{
				APFinderByName[part.name] = part;
			}
			if (!APFinderByIcon.ContainsKey(part.iconPrefab))
			{
				APFinderByIcon[part.iconPrefab] = part;
			}
		}
	}

	public AvailablePart ParsePart(UrlDir.UrlConfig urlConfig, ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogError("PartCompiler: Part config requires value 'name' is defined");
			return null;
		}
		if (!node.HasValue("module"))
		{
			Debug.LogError("PartCompiler: Part config requires value 'module' is defined");
			return null;
		}
		string partName = node.GetValue("name").Replace('_', '.');
		string value = node.GetValue("module");
		AvailablePart availablePart = new AvailablePart();
		availablePart.name = partName;
		availablePart.partUrl = urlConfig.url;
		availablePart.partUrlConfig = urlConfig;
		availablePart.partConfig = node;
		availablePart.configFileFullName = urlConfig.parent.fullPath;
		availablePart.AddFileTime(urlConfig.parent);
		float num = 1.25f;
		if (node.HasValue("rescaleFactor"))
		{
			num = float.Parse(node.GetValue("rescaleFactor"));
		}
		float num2 = 1f * num;
		GameObject gameObject = CompileModel(urlConfig, node, num, availablePart);
		if (gameObject == null)
		{
			Debug.LogError("PartCompiler: Cannot compile model");
			return null;
		}
		SetupSkinnedMeshes(gameObject);
		SetFogFactor(gameObject, 1f);
		Part part = CreatePart(partName, value, gameObject, num);
		if (part == null)
		{
			Debug.LogError("PartCompiler: Error parsing config");
			return null;
		}
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.PersistentLoadedPartIds.Remove(part.persistentId);
		}
		ConfigNode.LoadObjectFromConfig(availablePart, node, 0, removeAfterUse: true);
		string value2 = node.GetValue("entryCost");
		if (!string.IsNullOrEmpty(value2))
		{
			availablePart.SetEntryCost(int.Parse(value2));
			node.RemoveValues("entryCost");
		}
		availablePart.name = partName;
		if (!string.IsNullOrEmpty(availablePart.description))
		{
			availablePart.description = availablePart.description.Replace("\\n", "\n");
		}
		ShipConstruction.SanitizePartCosts(availablePart, node);
		int i = 0;
		for (int count = node.values.Count; i < count; i++)
		{
			ConfigNode.Value value3 = node.values[i];
			if (ApplyPartValue(part, value3))
			{
				continue;
			}
			switch (value3.name)
			{
			case "texture":
				ReplacePartTexture(urlConfig, gameObject, value3.value, normalMap: false);
				continue;
			case "alphaCutoff":
				ReplacePartMaterialValue(gameObject, "_Cutoff", float.Parse(value3.value));
				continue;
			case "normalmap":
				ReplacePartTexture(urlConfig, gameObject, value3.value, normalMap: true);
				continue;
			case "specPower":
				ReplacePartMaterialValue(gameObject, "_SpecPower", float.Parse(value3.value));
				continue;
			case "rimFalloff":
				ReplacePartMaterialValue(gameObject, "_RimFalloff", float.Parse(value3.value));
				continue;
			case "vesselType":
				part.vesselType = (VesselType)Enum.Parse(typeof(VesselType), value3.value);
				continue;
			case "scale":
			case "exportScale":
				num2 = float.Parse(value3.value) * num;
				part.scaleFactor = float.Parse(value3.value) / num;
				continue;
			case "iconCenter":
			case "name":
			case "mesh":
			case "subcategory":
			case "module":
				continue;
			}
			if (value3.name.StartsWith("node"))
			{
				string[] array = value3.value.Split(',');
				string[] array2 = value3.name.Split('_');
				if (array.Length < 6)
				{
					PDebug.Warning("ERROR: Bad node definition at " + value3.name);
					continue;
				}
				AttachNode attachNode = new AttachNode();
				attachNode.position = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2])) * num2;
				attachNode.orientation = new Vector3(float.Parse(array[3]), float.Parse(array[4]), float.Parse(array[5])) * num2;
				attachNode.originalPosition = attachNode.position;
				attachNode.originalOrientation = attachNode.orientation;
				if (array.Length >= 7)
				{
					attachNode.size = int.Parse(array[6]);
				}
				else
				{
					attachNode.size = 1;
				}
				if (array.Length >= 8)
				{
					attachNode.attachMethod = (AttachNodeMethod)int.Parse(array[7]);
				}
				else
				{
					if (array2[1] == "stack")
					{
						attachNode.attachMethod = AttachNodeMethod.FIXED_JOINT;
					}
					if (array2[1] == "dock")
					{
						attachNode.attachMethod = AttachNodeMethod.FIXED_JOINT;
					}
					if (array2[1] == "attach")
					{
						attachNode.attachMethod = AttachNodeMethod.HINGE_JOINT;
					}
				}
				if (array.Length >= 9)
				{
					attachNode.ResourceXFeed = int.Parse(array[8]) > 0;
				}
				if (array.Length >= 10)
				{
					attachNode.rigid = int.Parse(array[9]) > 0;
				}
				if (array.Length >= 11)
				{
					attachNode.AllowOneWayXFeed = int.Parse(array[10]) > 0;
				}
				PDebug.Log("Added " + value3.name + " at " + value3.value, PDebug.DebugLevel.PartLoader);
				if (array2[1] == "stack")
				{
					if (array2.Length > 2)
					{
						attachNode.id = array2[2];
					}
					attachNode.nodeType = AttachNode.NodeType.Stack;
					part.attachNodes.Add(attachNode);
					attachNode.owner = part;
				}
				if (array2[1] == "dock")
				{
					if (array2.Length > 2)
					{
						attachNode.id = array2[2];
					}
					attachNode.nodeType = AttachNode.NodeType.Dock;
					part.attachNodes.Add(attachNode);
					attachNode.owner = part;
				}
				if (array2[1] == "attach")
				{
					attachNode.id = "srfAttach";
					if (part.srfAttachNode != null)
					{
						PDebug.Warning("PartLoader WARNING: surface attach node is defined more than once!");
					}
					attachNode.nodeType = AttachNode.NodeType.Surface;
					part.srfAttachNode = attachNode;
					attachNode.owner = part;
				}
			}
			else if (value3.name.StartsWith("fx"))
			{
				string[] array3 = value3.value.Split(',');
				if (array3.Length < 7)
				{
					PDebug.Warning("PartLoader ERROR: bad effect node definition at " + value3.name);
					continue;
				}
				Vector3 localPosition = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2])) * num2;
				Vector3 up = new Vector3(float.Parse(array3[3]), float.Parse(array3[4]), float.Parse(array3[5])) * num2;
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Effects/" + value3.name)) as GameObject;
				if (array3.Length > 7 && array3[7].Trim() == "true")
				{
					gameObject2.name += "(Keep Pos)";
				}
				gameObject2.transform.position = Vector3.zero;
				gameObject2.transform.parent = part.transform;
				gameObject2.transform.localPosition = localPosition;
				gameObject2.transform.up = up;
				bool flag = false;
				int j = 6;
				for (int num3 = array3.Length; j < num3; j++)
				{
					string text = array3[j].Trim();
					if (text == "true")
					{
						continue;
					}
					if (part.findFxGroup(text) == null)
					{
						FXGroup item = new FXGroup(text);
						part.fxGroups.Add(item);
					}
					flag = true;
					if (gameObject2.GetComponent<ParticleSystem>() != null)
					{
						part.findFxGroup(text).fxEmittersNewSystem.Add(gameObject2.GetComponent<ParticleSystem>());
					}
					else if (gameObject2.GetComponentInChildren<ParticleSystem>() != null)
					{
						ParticleSystem[] componentsInChildren = gameObject2.GetComponentsInChildren<ParticleSystem>();
						for (int k = 0; k < componentsInChildren.Length; k++)
						{
							part.findFxGroup(text).fxEmittersNewSystem.Add(componentsInChildren[k]);
						}
					}
					else
					{
						if (!(gameObject2.GetComponent<Light>() != null))
						{
							PDebug.Warning("ERROR: fx object " + value3.name + " doesn't have any required components");
							UnityEngine.Object.Destroy(gameObject2);
							break;
						}
						part.findFxGroup(text).lights.Add(gameObject2.GetComponent<Light>());
					}
					PDebug.Log("Added " + value3.name + " at " + value3.value, PDebug.DebugLevel.PartLoader);
				}
				if (!flag)
				{
					PDebug.Warning("PartLoader ERROR: No FXGroups found from " + part.GetType().Name);
					UnityEngine.Object.Destroy(gameObject2);
				}
			}
			else if (value3.name.StartsWith("sound"))
			{
				LoadSound(urlConfig, value3.name, value3.value, part);
			}
		}
		if (part.skinMaxTemp <= 0.0)
		{
			part.skinMaxTemp = part.maxTemp;
		}
		part.Fields.SetOriginalValue();
		int l = 0;
		for (int count2 = node.nodes.Count; l < count2; l++)
		{
			ConfigNode configNode = node.nodes[l];
			switch (configNode.name)
			{
			case "DRAG_CUBE":
				if (!part.DragCubes.LoadCubes(configNode))
				{
					Debug.LogError("DragCube: Invalid cubes in part.cfg for part " + part.name);
				}
				break;
			case "EFFECTS":
				part.LoadEffects(configNode);
				break;
			case "NODE":
				part.AddAttachNode(configNode);
				break;
			case "RESOURCE":
				part.AddResource(configNode);
				break;
			case "MODULE":
				part.AddModule(configNode);
				break;
			}
		}
		if (part.vesselType > VesselType.Unknown)
		{
			part.AddModule("ModuleTripLogger");
		}
		if (node.HasNode("INTERNAL"))
		{
			node.GetNode("INTERNAL").CopyTo(availablePart.internalConfig);
		}
		availablePart.iconPrefab = CreatePartIcon(part.gameObject, out availablePart.iconScale);
		StripTaggedTransforms(part.gameObject.transform, "Icon_Only");
		availablePart.partPrefab = part;
		availablePart.partSize = PartGeometryUtil.MergeBounds(availablePart.partPrefab.GetRendererBounds(), availablePart.partPrefab.transform).size.magnitude;
		part.collider = GetPartCollider(part.gameObject);
		part.transform.parent = base.transform;
		part.gameObject.SetActive(value: false);
		if (!node.TryGetValue("showVesselNaming", ref availablePart.showVesselNaming))
		{
			if (part.FindModuleImplementing<ModuleCommand>() != null)
			{
				availablePart.showVesselNaming = true;
			}
			else if (part.FindModuleImplementing<KerbalSeat>() != null)
			{
				availablePart.showVesselNaming = true;
			}
		}
		float value4 = PhysicsGlobals.PartMassMin;
		node.TryGetValue("minimumMass", ref value4);
		availablePart.MinimumMass = value4;
		float value5 = PhysicsGlobals.PartRBMassMin;
		node.TryGetValue("minimumRBMass", ref value5);
		availablePart.MinimumRBMass = value5;
		node.TryGetValue("minimumCost", ref availablePart.minimumCost);
		part.partInfo = availablePart;
		availablePart.tags = availablePart.tags.ToLower() + " " + BasePartCategorizer.GeneratePartAutoTags(availablePart);
		if (part.FindModuleImplementing<ModuleCargoPart>() != null)
		{
			cargoParts.Add(availablePart);
		}
		if (part.FindModuleImplementing<ModuleInventoryPart>() != null)
		{
			cargoInventoryParts.Add(availablePart);
		}
		if (part.FindModuleImplementing<ModuleGroundExperiment>() != null)
		{
			deployedScienceExperimentParts.Add(availablePart);
		}
		if (part.FindModuleImplementing<ModuleRobotArmScanner>() != null)
		{
			robotArmScannerParts.Add(availablePart);
		}
		List<MeshRenderer> list = part.FindModelMeshRenderersCached();
		for (int m = 0; m < list.Count; m++)
		{
			list[m].reflectionProbeUsage = ReflectionProbeUsage.BlendProbesAndSkybox;
		}
		return availablePart;
	}

	public static bool ApplyPartValue(Part part, ConfigNode.Value nodeValue)
	{
		switch (nodeValue.name)
		{
		case "dragModelType":
			switch (nodeValue.value.ToUpper())
			{
			case "SPHERICAL":
				part.dragModel = Part.DragModel.SPHERICAL;
				break;
			case "NONE":
			case "OVERRIDE":
				part.dragModel = Part.DragModel.NONE;
				break;
			case "CYLINDRICAL":
				part.dragModel = Part.DragModel.CYLINDRICAL;
				break;
			case "CONIC":
				part.dragModel = Part.DragModel.CONIC;
				break;
			default:
				part.dragModel = Part.DragModel.CUBE;
				break;
			}
			return true;
		case "partRendererBoundsIgnore":
		{
			string[] array5 = nodeValue.value.Split(',');
			part.partRendererBoundsIgnore = new List<string>();
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i] = array5[i].Trim();
				part.partRendererBoundsIgnore.Add(array5[i]);
			}
			return true;
		}
		case "iconCenter":
		case "alphaCutoff":
		case "scale":
		case "texture":
		case "normalmap":
		case "name":
		case "specPower":
		case "rimFalloff":
		case "vesselType":
		case "mesh":
		case "subcategory":
		case "module":
		case "exportScale":
			return false;
		default:
		{
			if (nodeValue.name.StartsWith("node"))
			{
				return false;
			}
			if (nodeValue.name.StartsWith("fx"))
			{
				return false;
			}
			if (nodeValue.name.StartsWith("sound"))
			{
				return false;
			}
			Type type = part.GetType();
			FieldInfo field = type.GetField(nodeValue.name);
			if (field == null)
			{
				PDebug.Warning("PartLoader Warning: Variable " + nodeValue.name + " not found in " + type.Name);
				return true;
			}
			if (field.FieldType == typeof(string))
			{
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1] { nodeValue.value });
				PDebug.Log(type.Name + " " + nodeValue.name + ": " + nodeValue.value, PDebug.DebugLevel.PartLoader);
				return true;
			}
			if (field.FieldType == typeof(Vector2))
			{
				string[] array = nodeValue.value.Split(',');
				if (array.Length < 2)
				{
					PDebug.Log("WARNING: " + nodeValue.name + " is nor formatted properly! proper format for Vector2s is x,y");
					return true;
				}
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1]
				{
					new Vector2(float.Parse(array[0]), float.Parse(array[1]))
				});
				PDebug.Log(type.Name + " " + nodeValue.name + ": " + nodeValue.value, PDebug.DebugLevel.PartLoader);
				return true;
			}
			if (field.FieldType == typeof(Vector3))
			{
				string[] array2 = nodeValue.value.Split(',');
				if (array2.Length < 3)
				{
					PDebug.Log("WARNING: " + nodeValue.name + " is nor formatted properly! proper format for Vector3s is x,y,z");
					return true;
				}
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1]
				{
					new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]))
				});
				PDebug.Log(type.Name + " " + nodeValue.name + ": " + nodeValue.value, PDebug.DebugLevel.PartLoader);
				return true;
			}
			if (field.FieldType == typeof(Vector4))
			{
				string[] array3 = nodeValue.value.Split(',');
				if (array3.Length < 4)
				{
					PDebug.Log("WARNING: " + nodeValue.name + " is nor formatted properly! proper format for Vector4s is x,y,z,w");
					return true;
				}
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1]
				{
					new Vector4(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]), float.Parse(array3[3]))
				});
				PDebug.Log(type.Name + " " + nodeValue.name + ": " + nodeValue.value, PDebug.DebugLevel.PartLoader);
				return true;
			}
			if (field.FieldType == typeof(Quaternion))
			{
				string[] array4 = nodeValue.value.Split(',');
				if (array4.Length < 4)
				{
					PDebug.Warning("WARNING: " + nodeValue.name + " is nor formatted properly! proper format for Quaternions is angle(deg),x,y,z");
					return true;
				}
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1] { Quaternion.AngleAxis(float.Parse(array4[0]), new Vector3(float.Parse(array4[1]), float.Parse(array4[2]), float.Parse(array4[3]))) });
				PDebug.Log(type.Name + " " + nodeValue.name + ": " + nodeValue.value, PDebug.DebugLevel.PartLoader);
				return true;
			}
			if (field.FieldType == typeof(int))
			{
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1] { int.Parse(nodeValue.value) });
				return true;
			}
			if (field.FieldType == typeof(double))
			{
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1] { float.Parse(nodeValue.value) });
				return true;
			}
			if (field.FieldType == typeof(double))
			{
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1] { double.Parse(nodeValue.value) });
				return true;
			}
			if (field.FieldType == typeof(bool))
			{
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1] { bool.Parse(nodeValue.value) });
				return true;
			}
			if (field.FieldType.IsEnum)
			{
				type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1] { Enum.Parse(field.FieldType, nodeValue.value) });
				return true;
			}
			object obj = ((field.FieldType.GetMethod("Parse", new Type[1] { typeof(string) }) != null) ? field.FieldType.GetMethod("Parse", new Type[1] { typeof(string) }).Invoke(null, new object[1] { nodeValue.value }) : null);
			if (obj == null)
			{
				return false;
			}
			type.InvokeMember(nodeValue.name, BindingFlags.SetField, null, part, new object[1] { obj });
			PDebug.Log(type.Name + " " + nodeValue.name + ": " + obj.ToString(), PDebug.DebugLevel.PartLoader);
			return true;
		}
		case "dragModel":
			part.dragModel = (Part.DragModel)Enum.Parse(typeof(Part.DragModel), nodeValue.value);
			return true;
		}
	}

	public void SetupSkinnedMeshes(GameObject part)
	{
		SkinnedMeshRenderer[] componentsInChildren = part.GetComponentsInChildren<SkinnedMeshRenderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].updateWhenOffscreen = true;
		}
	}

	public void CompilePartInfo(AvailablePart newPartInfo, Part part)
	{
		bool flag = false;
		if (part.GetType().IsSubclassOf(typeof(Part)))
		{
			AvailablePart.ModuleInfo moduleInfo = new AvailablePart.ModuleInfo();
			if (part is IModuleInfo)
			{
				IModuleInfo moduleInfo2 = part as IModuleInfo;
				moduleInfo.moduleName = moduleInfo2.GetModuleTitle();
				moduleInfo.onDrawWidget = moduleInfo2.GetDrawModulePanelCallback();
				moduleInfo.info = moduleInfo2.GetInfo().Trim();
				moduleInfo.primaryInfo = moduleInfo2.GetPrimaryField();
			}
			else
			{
				moduleInfo.moduleName = KSPUtil.PrintModuleName(part.ClassName);
				moduleInfo.info = part.drawStats().Trim();
			}
			if (!string.IsNullOrEmpty(moduleInfo.info) || moduleInfo.onDrawWidget != null)
			{
				newPartInfo.moduleInfos.Add(moduleInfo);
			}
		}
		string text = "";
		PartModule partModule = null;
		int i = 0;
		for (int count = part.Modules.Count; i < count; i++)
		{
			PartModule partModule2 = part.Modules[i];
			AvailablePart.ModuleInfo moduleInfo3 = new AvailablePart.ModuleInfo();
			if (partModule2 is IModuleInfo)
			{
				IModuleInfo moduleInfo4 = partModule2 as IModuleInfo;
				moduleInfo3.moduleName = moduleInfo4.GetModuleTitle();
				moduleInfo3.onDrawWidget = moduleInfo4.GetDrawModulePanelCallback();
				moduleInfo3.info = moduleInfo4.GetInfo().Trim();
				if (partModule2.showUpgradesInModuleInfo && partModule2.HasUpgrades())
				{
					moduleInfo3.info = moduleInfo3.info + "\n" + partModule2.PrintUpgrades();
				}
				moduleInfo3.primaryInfo = moduleInfo4.GetPrimaryField();
				if (moduleInfo3.moduleName == ModulePartVariants.GetTitle())
				{
					partModule = partModule2;
				}
			}
			else
			{
				moduleInfo3.moduleName = partModule2.GUIName ?? KSPUtil.PrintModuleName(partModule2.moduleName);
				moduleInfo3.moduleDisplayName = partModule2.GetModuleDisplayName();
				moduleInfo3.info = partModule2.GetInfo().Trim();
				if (partModule2.showUpgradesInModuleInfo && partModule2.HasUpgrades())
				{
					moduleInfo3.info = moduleInfo3.info + "\n" + partModule2.PrintUpgrades();
				}
			}
			if (partModule2 is ModuleCommand)
			{
				flag = true;
			}
			moduleInfo3.moduleDisplayName = partModule2.GetModuleDisplayName();
			if (moduleInfo3.moduleDisplayName == "")
			{
				moduleInfo3.moduleDisplayName = moduleInfo3.moduleName;
			}
			if (!string.IsNullOrEmpty(moduleInfo3.info) || moduleInfo3.onDrawWidget != null)
			{
				newPartInfo.moduleInfos.Add(moduleInfo3);
				if (text != string.Empty)
				{
					text += "\n";
				}
				text += moduleInfo3.info;
			}
		}
		newPartInfo.moduleInfo = text;
		newPartInfo.moduleInfos.Sort((AvailablePart.ModuleInfo ap1, AvailablePart.ModuleInfo ap2) => ap1.moduleName.CompareTo(ap2.moduleName));
		if (partModule != null && partModule is ModulePartVariants)
		{
			ModulePartVariants variants = partModule as ModulePartVariants;
			part.variants = variants;
			newPartInfo.variant = part.baseVariant;
		}
		string text2 = "";
		int j = 0;
		for (int count2 = part.Resources.Count; j < count2; j++)
		{
			PartResource partResource = part.Resources[j];
			AvailablePart.ResourceInfo resourceInfo = new AvailablePart.ResourceInfo();
			resourceInfo.resourceName = partResource.resourceName;
			resourceInfo.displayName = partResource.info.displayName.LocalizeRemoveGender();
			resourceInfo.info = Localizer.Format("#autoLOC_166269", partResource.amount.ToString("F1")) + ((partResource.amount != partResource.maxAmount) ? (" " + Localizer.Format("#autoLOC_6004042", partResource.maxAmount.ToString("F1"))) : "") + Localizer.Format("#autoLOC_166270", (partResource.amount * (double)partResource.info.density).ToString("F2")) + Localizer.Format("#autoLOC_7001407") + ((partResource.info.unitCost > 0f) ? Localizer.Format("#autoLOC_166271", (partResource.amount * (double)partResource.info.unitCost).ToString("F2")) : "");
			if (partResource.maxAmount > 0.0)
			{
				resourceInfo.primaryInfo = "<b>" + resourceInfo.displayName + ": </b>" + KSPUtil.LocalizeNumber(partResource.maxAmount, "F1");
			}
			if (!string.IsNullOrEmpty(resourceInfo.info))
			{
				newPartInfo.resourceInfos.Add(resourceInfo);
				if (text2 != string.Empty)
				{
					text2 += "\n";
				}
				text2 += resourceInfo.info;
			}
		}
		if (part.Resources.Count > 0)
		{
			text2 = text2 + "\nDry Mass: " + part.mass;
		}
		newPartInfo.resourceInfo = text2;
		newPartInfo.resourceInfos.Sort((AvailablePart.ResourceInfo rp1, AvailablePart.ResourceInfo rp2) => rp1.resourceName.CompareTo(rp2.resourceName));
		if (part.applyKerbalMassModification && part.CrewCapacity > 0)
		{
			if (flag)
			{
				part.partSeatMassReduction = PhysicsGlobals.PerCommandSeatReduction * (float)part.CrewCapacity;
			}
			else
			{
				part.partSeatMassReduction = PhysicsGlobals.PerSeatReduction * (float)part.CrewCapacity;
			}
			part.mass -= part.partSeatMassReduction;
		}
	}

	public void AddVariants(List<PartVariant> variants, AvailablePart part)
	{
		for (int i = 0; i < variants.Count; i++)
		{
			if (!string.IsNullOrEmpty(variants[i].themeName))
			{
				AvailableVariantTheme availableVariantTheme = GetVariantInfoByName(variants[i].themeName);
				if (availableVariantTheme == null && !missingVariantThemes.Contains(variants[i].themeName))
				{
					Debug.LogErrorFormat("Unable to find theme named:'{0}' in GameDB", variants[i].themeName);
					missingVariantThemes.Add(variants[i].themeName);
					availableVariantTheme = AvailableVariantTheme.CreateVariantTheme();
					availableVariantTheme.name = (availableVariantTheme.displayName = variants[i].themeName);
					availableVariantTheme.description = "No Definition in GameDB";
					loadedVariantThemes.Add(availableVariantTheme);
				}
				availableVariantTheme.parts.Add(part);
			}
		}
	}

	public Collider GetPartCollider(GameObject part)
	{
		Collider[] componentsInChildren = part.GetComponentsInChildren<Collider>();
		int num = 0;
		int num2 = componentsInChildren.Length;
		Collider collider;
		while (true)
		{
			if (num < num2)
			{
				collider = componentsInChildren[num];
				if (!collider.isTrigger && !collider.gameObject.CompareTag("Airlock") && !collider.gameObject.CompareTag("Ladder") && !(collider is WheelCollider))
				{
					break;
				}
				num++;
				continue;
			}
			return part.GetComponentInChildren<Collider>();
		}
		return collider;
	}

	public Part CreatePart(string partName, string moduleName, GameObject partModel, float rescaleFactor)
	{
		Type classByName = AssemblyLoader.GetClassByName(typeof(Part), moduleName);
		if (classByName == null)
		{
			Debug.LogError("PartCompiler: Cannot find Part of type '" + moduleName + "'");
			return null;
		}
		partModel.name = partName;
		Part obj = (Part)partModel.AddComponent(classByName);
		obj.OnLoad();
		obj.hasHeiarchyModel = true;
		obj.rescaleFactor = rescaleFactor;
		return obj;
	}

	public void ReplacePartTexture(UrlDir.UrlConfig urlConfig, GameObject partModel, string textureName, bool normalMap)
	{
		string text = urlConfig.parent.parent.url + "/textures/" + Path.GetFileNameWithoutExtension(textureName);
		Texture2D texture = GameDatabase.Instance.GetTexture(text, normalMap);
		if (texture == null)
		{
			Debug.LogWarning("PartLoader: Cannot replace model texture with '" + text + "'");
		}
		MeshRenderer[] componentsInChildren = partModel.GetComponentsInChildren<MeshRenderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			MeshRenderer meshRenderer = componentsInChildren[i];
			if (meshRenderer.material == null)
			{
				continue;
			}
			if (normalMap)
			{
				if (meshRenderer.material.HasProperty("_NormalMap"))
				{
					meshRenderer.material.SetTexture("_NormalMap", texture);
				}
			}
			else
			{
				meshRenderer.material.mainTexture = texture;
			}
		}
	}

	public void ReplacePartMaterialValue(GameObject partModel, string valueName, float value)
	{
		MeshRenderer[] componentsInChildren = partModel.GetComponentsInChildren<MeshRenderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			MeshRenderer meshRenderer = componentsInChildren[i];
			if (!(meshRenderer.material == null) && meshRenderer.material.HasProperty(valueName))
			{
				meshRenderer.material.SetFloat(valueName, value);
			}
		}
	}

	public GameObject CreatePartIcon(GameObject newPart, out float iconScale)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(newPart);
		gameObject.SetActive(value: true);
		Part component = gameObject.GetComponent<Part>();
		if (component != null)
		{
			int i = 0;
			for (int count = component.Modules.Count; i < count; i++)
			{
				component.Modules[i].OnIconCreate();
			}
		}
		StripComponent<Part>(gameObject);
		StripComponent<PartModule>(gameObject);
		StripComponent<EffectBehaviour>(gameObject);
		StripGameObject<Collider>(gameObject, "collider");
		StripComponent<Collider>(gameObject);
		StripComponent<WheelCollider>(gameObject);
		StripComponent<SmokeTrailControl>(gameObject);
		StripComponent<FXPrefab>(gameObject);
		StripComponent<ParticleSystem>(gameObject);
		StripComponent<Light>(gameObject);
		StripComponent<Animation>(gameObject);
		StripComponent<DAE>(gameObject);
		StripComponent<MeshRenderer>(gameObject, "Icon_Hidden", parseChildren: true);
		StripComponent<MeshFilter>(gameObject, "Icon_Hidden", parseChildren: true);
		StripComponent<SkinnedMeshRenderer>(gameObject, "Icon_Hidden", parseChildren: true);
		Bounds partBounds = GetPartBounds(gameObject);
		SetPartIconMaterials(gameObject);
		float num = (iconScale = 1f / Mathf.Max(Mathf.Abs(partBounds.size.x), Mathf.Max(Mathf.Abs(partBounds.size.y), Mathf.Abs(partBounds.size.z))));
		GameObject gameObject2 = new GameObject();
		gameObject2.name = gameObject.name + " icon";
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localScale = Vector3.one * num;
		gameObject.transform.localPosition = partBounds.center * (0f - num);
		gameObject2.transform.parent = base.transform;
		gameObject2.SetActive(value: false);
		return gameObject2;
	}

	public void SetPartIconMaterials(GameObject part)
	{
		Renderer[] componentsInChildren = part.GetComponentsInChildren<Renderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			Material[] sharedMaterials = componentsInChildren[i].sharedMaterials;
			int j = 0;
			for (int num2 = sharedMaterials.Length; j < num2; j++)
			{
				Material material = sharedMaterials[j];
				if (!(material == null))
				{
					Material material2 = null;
					material2 = ((material.shader.name == "KSP/Bumped Specular (Mapped)") ? new Material(Shader.Find("KSP/ScreenSpaceMaskSpecular")) : ((material.shader.name == "KSP/Bumped Specular (Transparent)") ? new Material(Shader.Find("KSP/ScreenSpaceMaskBumpedSpecular(Transparent)")) : (material.shader.name.Contains("Bumped") ? new Material(Shader.Find("KSP/ScreenSpaceMaskBumped")) : (material.shader.name.Contains("KSP/Alpha/CutoffBackground") ? new Material(Shader.Find("KSP/ScreenSpaceMaskAlphaCutoffBackground")) : ((!(material.shader.name == "KSP/Unlit")) ? new Material(Shader.Find("KSP/ScreenSpaceMask")) : new Material(Shader.Find("KSP/ScreenSpaceMaskUnlit")))))));
					material2.name = material.name;
					material2.CopyPropertiesFromMaterial(material);
					if (!material2.HasProperty("_Color"))
					{
						material2.SetColor(PropertyIDs._Color, Color.white);
					}
					else
					{
						material2.SetColor(PropertyIDs._Color, new Color(Mathf.Clamp(material2.color.r, 0.5f, 1f), Mathf.Clamp(material2.color.g, 0.5f, 1f), Mathf.Clamp(material2.color.b, 0.5f, 1f)));
					}
					material2.SetFloat(PropertyIDs._Multiplier, shaderMultiplier);
					material2.SetFloat(PropertyIDs._MinX, 0f);
					material2.SetFloat(PropertyIDs._MaxX, 1f);
					material2.SetFloat(PropertyIDs._MinY, 0f);
					material2.SetFloat(PropertyIDs._MaxY, 1f);
					sharedMaterials[j] = material2;
				}
			}
			componentsInChildren[i].sharedMaterials = sharedMaterials;
		}
	}

	public Bounds GetPartBounds(GameObject part)
	{
		Bounds result = default(Bounds);
		bool flag = false;
		part.transform.position = Vector3.zero;
		part.transform.rotation = Quaternion.identity;
		Renderer[] componentsInChildren = part.GetComponentsInChildren<Renderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			Renderer renderer = componentsInChildren[i];
			if (!(renderer as ParticleSystemRenderer != null))
			{
				if (flag)
				{
					result.Encapsulate(renderer.bounds.min);
					result.Encapsulate(renderer.bounds.max);
				}
				else
				{
					result = new Bounds(renderer.bounds.center, renderer.bounds.size);
					flag = true;
				}
			}
		}
		return result;
	}

	public void LoadSound(UrlDir.UrlConfig urlConfig, string filename, string sfxRawData, Part part)
	{
		bool flag = false;
		AudioClip audioClip = null;
		if (filename.Contains("\\"))
		{
			audioClip = GameDatabase.Instance.GetAudioClip(filename);
			if (audioClip != null)
			{
				ConstructSoundFXGroup(part, filename, sfxRawData, audioClip);
				return;
			}
		}
		audioClip = Resources.Load("Sounds/" + filename) as AudioClip;
		if (audioClip != null)
		{
			ConstructSoundFXGroup(part, filename, sfxRawData, audioClip);
			return;
		}
		if (!flag)
		{
			audioClip = GameDatabase.Instance.GetAudioClip(urlConfig.parent.url + "/Sounds/" + filename);
			if (audioClip != null)
			{
				ConstructSoundFXGroup(part, filename, sfxRawData, audioClip);
				return;
			}
		}
		if (!flag)
		{
			audioClip = GameDatabase.Instance.GetAudioClip("/GameData/Squad/Sounds/" + filename);
			if (audioClip != null)
			{
				ConstructSoundFXGroup(part, filename, sfxRawData, audioClip);
				return;
			}
		}
		if (!flag)
		{
			audioClip = GameDatabase.Instance.GetAudioClip("/Sounds/" + filename);
			if (audioClip != null)
			{
				ConstructSoundFXGroup(part, filename, sfxRawData, audioClip);
				return;
			}
		}
		Debug.LogWarning("ERROR: no sound file found for " + filename);
	}

	public void ConstructSoundFXGroup(Part part, string filename, string sfxRawData, AudioClip newSfx)
	{
		string[] array = sfxRawData.Split(',');
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			string text = array[i];
			string groupID = text.Trim();
			FXGroup fXGroup = part.findFxGroup(groupID);
			if (fXGroup == null)
			{
				fXGroup = new FXGroup(text);
				part.fxGroups.Add(fXGroup);
				if (fXGroup != null)
				{
					fXGroup.sfx = newSfx;
				}
			}
			else
			{
				fXGroup.sfx = newSfx;
			}
		}
	}

	public string InferBulkheadProfiles(ConfigNode n)
	{
		string text = "";
		string[] valuesStartsWith = n.GetValuesStartsWith("node_stack");
		int i = 0;
		for (int num = valuesStartsWith.Length; i < num; i++)
		{
			string text2 = valuesStartsWith[i];
			int num2 = 0;
			int length = text2.Length;
			while (length-- > 0)
			{
				if (text2[length] == ',')
				{
					num2++;
				}
			}
			if (num2 >= 6)
			{
				int num3 = int.Parse(text2.Split(',')[6]);
				text = KSPUtil.AppendValueToString(text, "size" + num3, ',');
			}
			else
			{
				text = KSPUtil.AppendValueToString(text, "size1", ',');
			}
		}
		int num4 = n.GetValuesStartsWith("node_attach").Length;
		for (int j = 0; j < num4; j++)
		{
			text = KSPUtil.AppendValueToString(text, "srf", ',');
		}
		return text;
	}

	public static InternalProp GetInternalProp(string name)
	{
		int num = 0;
		int count = Instance.internalProps.Count;
		InternalProp internalProp;
		while (true)
		{
			if (num < count)
			{
				internalProp = Instance.internalProps[num];
				if (internalProp.propName == name)
				{
					break;
				}
				num++;
				continue;
			}
			Debug.Log("Cannot find InternalProp '" + name + "'");
			return null;
		}
		InternalProp internalProp2 = UnityEngine.Object.Instantiate(internalProp);
		internalProp2.gameObject.name = internalProp.propName;
		internalProp2.gameObject.SetActive(value: true);
		return internalProp2;
	}

	public IEnumerator CompileInternalProps(UrlDir.UrlConfig[] allPropNodes)
	{
		internalProps = new List<InternalProp>();
		float nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
		int i = 0;
		int iC = allPropNodes.Length;
		while (i < iC)
		{
			UrlDir.UrlConfig urlConfig = allPropNodes[i];
			Debug.Log("PartLoader: Compiling Internal Prop '" + urlConfig.url + "'");
			progressTitle = urlConfig.url;
			progressFraction += progressDelta;
			InternalProp internalProp = LoadInternalProp(urlConfig);
			if (internalProp != null)
			{
				internalProps.Add(internalProp);
			}
			if (!(Time.realtimeSinceStartup <= nextFrameTime))
			{
				nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
				yield return null;
			}
			int num = i + 1;
			i = num;
		}
	}

	public InternalProp LoadInternalProp(UrlDir.UrlConfig urlConf)
	{
		string value = urlConf.config.GetValue("name");
		if (value == null)
		{
			Debug.Log("LoadInternalProp '" + urlConf.url + "' FAILED: Config invalid");
			return null;
		}
		GameObject gameObject = CompileModel(urlConf, urlConf.config, 1f);
		if (gameObject == null)
		{
			Debug.Log("LoadInternalProp '" + urlConf.url + "' FAILED: Cannot find model");
			return null;
		}
		gameObject.name = value;
		gameObject.transform.parent = base.transform;
		InternalProp internalProp = gameObject.AddComponent<InternalProp>();
		internalProp.propName = value;
		internalProp.gameObject.name = value;
		internalProp.hasModel = true;
		internalProp.Load(urlConf.config);
		internalProp.transform.parent = base.transform;
		internalProp.gameObject.SetActive(value: false);
		return internalProp;
	}

	public static InternalModel GetInternalPart(string name)
	{
		int count = Instance.internalParts.Count;
		InternalModel internalModel;
		do
		{
			if (count-- > 0)
			{
				internalModel = Instance.internalParts[count];
				continue;
			}
			Debug.Log("Cannot find InternalPart '" + name + "'");
			return null;
		}
		while (!(internalModel.internalName == name));
		return internalModel;
	}

	public IEnumerator CompileInternalSpaces(UrlDir.UrlConfig[] allSpaceNodes)
	{
		internalParts = new List<InternalModel>();
		float nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
		int i = 0;
		int iC = allSpaceNodes.Length;
		while (i < iC)
		{
			UrlDir.UrlConfig urlConfig = allSpaceNodes[i];
			Debug.Log("PartLoader: Compiling Internal Space '" + urlConfig.url + "'");
			progressTitle = urlConfig.url;
			progressFraction += progressDelta;
			InternalModel internalModel = LoadInternalSpace(urlConfig);
			if (internalModel != null)
			{
				internalParts.Add(internalModel);
			}
			if (!(Time.realtimeSinceStartup <= nextFrameTime))
			{
				nextFrameTime = Time.realtimeSinceStartup + LoadingScreen.minFrameTime;
				yield return null;
			}
			int num = i + 1;
			i = num;
		}
	}

	public InternalModel LoadInternalSpace(UrlDir.UrlConfig urlConf)
	{
		string value = urlConf.config.GetValue("name");
		if (value == null)
		{
			Debug.Log("LoadInternalPart '" + urlConf.url + "' FAILED: Config invalid");
			return null;
		}
		GameObject gameObject = CompileModel(urlConf, urlConf.config, 1f);
		if (gameObject == null)
		{
			Debug.Log("LoadInternalPart '" + urlConf.url + "' FAILED: Cannot find model");
			return null;
		}
		gameObject.name = value;
		gameObject.transform.parent = base.transform;
		SetTwoSidedShadows(gameObject);
		InternalModel internalModel = gameObject.AddComponent<InternalModel>();
		internalModel.internalName = value;
		internalModel.Load(urlConf.config);
		internalModel.transform.parent = base.transform;
		internalModel.gameObject.SetActive(value: false);
		return internalModel;
	}

	public GameObject CompileModel(UrlDir.UrlConfig cfg, ConfigNode partCfg, float scaleFactor, AvailablePart partInfo = null)
	{
		GameObject gameObject = null;
		ConfigNode[] nodes = partCfg.GetNodes("MODEL");
		if (nodes != null && nodes.Length != 0)
		{
			GameObject gameObject2 = null;
			int num = nodes.Length;
			for (int i = 0; i < num; i++)
			{
				ConfigNode configNode = nodes[i];
				string value = configNode.GetValue("model");
				GameObject modelPrefab = GameDatabase.Instance.GetModelPrefab(value);
				if (modelPrefab == null)
				{
					Debug.LogError("PartCompiler: Cannot clone model '" + value + "' as model does not exist");
					continue;
				}
				GameObject gameObject3 = UnityEngine.Object.Instantiate(modelPrefab);
				if (gameObject == null)
				{
					gameObject = new GameObject("part");
					gameObject2 = new GameObject("model");
					gameObject2.transform.parent = gameObject.transform;
					gameObject2.transform.localScale = Vector3.one * scaleFactor;
				}
				if (partInfo != null)
				{
					UrlDir.UrlFile modelFile = GameDatabase.Instance.GetModelFile(modelPrefab);
					if (modelFile == null)
					{
						Debug.Log("AddFileTime: File '" + value + "' is null!");
					}
					else
					{
						partInfo.AddFileTime(modelFile);
					}
				}
				gameObject3.SetActive(value: true);
				gameObject3.transform.parent = gameObject2.transform;
				string value2 = configNode.GetValue("parent");
				if (value2 != null)
				{
					Transform transform = gameObject2.transform.Find(value2);
					if (transform != null)
					{
						gameObject3.transform.NestToParent(transform);
					}
					else
					{
						gameObject3.transform.NestToParent(gameObject2.transform);
					}
				}
				else
				{
					gameObject3.transform.NestToParent(gameObject2.transform);
				}
				string value3 = configNode.GetValue("name");
				if (value3 != null)
				{
					gameObject3.name = value3;
				}
				string value4 = configNode.GetValue("scale");
				if (value4 != null)
				{
					gameObject3.transform.localScale = ConfigNode.ParseVector3(value4);
				}
				else
				{
					gameObject3.transform.localScale = Vector3.one;
				}
				string value5 = configNode.GetValue("position");
				if (value5 != null)
				{
					gameObject3.transform.localPosition = ConfigNode.ParseVector3(value5);
				}
				string value6 = configNode.GetValue("rotation");
				if (value6 != null)
				{
					gameObject3.transform.localRotation = Quaternion.Euler(ConfigNode.ParseVector3(value6));
				}
				string value7 = string.Empty;
				if (configNode.TryGetValue("iconHidden", ref value7) && bool.Parse(value7))
				{
					gameObject3.tag = "Icon_Hidden";
					for (int j = 0; j < gameObject3.transform.childCount; j++)
					{
						gameObject3.transform.GetChild(j).tag = "Icon_Hidden";
					}
				}
				string[] values = configNode.GetValues("texture");
				if (values.Length == 0)
				{
					continue;
				}
				List<string> list = new List<string>();
				List<GameDatabase.TextureInfo> list2 = new List<GameDatabase.TextureInfo>();
				int k = 0;
				for (int num2 = values.Length; k < num2; k++)
				{
					string[] array = values[k].Split(new char[3] { ':', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length != 2)
					{
						Debug.LogError("PartCompiler: Cannot replace texture as texture replacement string is invalid. Syntax is 'origTextureName newTextureURL'");
						continue;
					}
					string text = array[0].Trim();
					string text2 = array[1].Trim();
					if (GameDatabase.Instance.GetTextureInfoIn(value, text) == null)
					{
						Debug.LogError("PartCompiler: Cannot replace texture as cannot find texture '" + text + "' to replace");
						continue;
					}
					GameDatabase.TextureInfo textureInfo = GameDatabase.Instance.GetTextureInfo(text2);
					if (textureInfo == null)
					{
						Debug.LogError("PartCompiler: Cannot replace texture '" + text + "' as cannot find texture '" + text2 + "' to replace with");
					}
					else
					{
						list.Add(text);
						list2.Add(textureInfo);
					}
				}
				if (list.Count > 0)
				{
					ReplaceTextures(gameObject3, list, list2);
				}
			}
			if (gameObject == null)
			{
				Debug.LogError("PartCompiler: Model was not compiled correctly");
				return null;
			}
		}
		else
		{
			GameObject modelPrefabIn = GameDatabase.Instance.GetModelPrefabIn(cfg.parent.parent.url);
			if (modelPrefabIn == null)
			{
				Debug.LogError("PartCompiler: Cannot clone model from '" + cfg.parent.parent.url + "' directory as model does not exist");
				return null;
			}
			GameObject obj = UnityEngine.Object.Instantiate(modelPrefabIn);
			if (partInfo != null)
			{
				UrlDir.UrlFile modelFile2 = GameDatabase.Instance.GetModelFile(modelPrefabIn);
				if (modelFile2 == null)
				{
					Debug.Log("AddFileTime: File '" + cfg.parent.parent.url + "' is null!");
				}
				else
				{
					partInfo.AddFileTime(modelFile2);
				}
			}
			gameObject = new GameObject("part");
			obj.SetActive(value: true);
			obj.name = "model";
			obj.transform.parent = gameObject.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localScale = Vector3.one * scaleFactor;
		}
		return gameObject;
	}

	public void ReplaceTextures(GameObject model, List<string> textureNames, List<GameDatabase.TextureInfo> newTextures)
	{
		Renderer[] componentsInChildren = model.GetComponentsInChildren<Renderer>();
		List<Material> list = new List<Material>();
		List<Material> list2 = new List<Material>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			Renderer renderer = componentsInChildren[i];
			if (renderer is ParticleSystemRenderer || renderer.sharedMaterial == null)
			{
				continue;
			}
			int num2 = list.IndexOf(renderer.sharedMaterial);
			if (num2 != -1)
			{
				renderer.sharedMaterial = list2[num2];
				continue;
			}
			Material material = null;
			int num3 = -1;
			if (renderer.sharedMaterial.HasProperty("_MainTex"))
			{
				Texture texture = renderer.sharedMaterial.GetTexture("_MainTex");
				num3 = textureNames.IndexOf(texture.name.Substring(texture.name.LastIndexOf('/') + 1));
				if (num3 != -1)
				{
					if (material == null)
					{
						material = new Material(renderer.sharedMaterial);
					}
					material.SetTexture("_MainTex", newTextures[num3].texture);
				}
			}
			if (renderer.sharedMaterial.HasProperty("_BumpMap"))
			{
				Texture texture2 = renderer.sharedMaterial.GetTexture("_BumpMap");
				num3 = textureNames.IndexOf(texture2.name.Substring(texture2.name.LastIndexOf('/') + 1));
				if (num3 != -1)
				{
					if (material == null)
					{
						material = new Material(renderer.sharedMaterial);
					}
					material.SetTexture("_BumpMap", newTextures[num3].normalMap);
				}
			}
			if (renderer.sharedMaterial.HasProperty("_Emissive"))
			{
				Texture texture3 = renderer.sharedMaterial.GetTexture("_Emissive");
				num3 = textureNames.IndexOf(texture3.name.Substring(texture3.name.LastIndexOf('/') + 1));
				if (num3 != -1)
				{
					if (material == null)
					{
						material = new Material(renderer.sharedMaterial);
					}
					material.SetTexture("_Emissive", newTextures[num3].texture);
				}
			}
			if (renderer.sharedMaterial.HasProperty("_SpecMap"))
			{
				Texture texture4 = renderer.sharedMaterial.GetTexture("_SpecMap");
				num3 = textureNames.IndexOf(texture4.name.Substring(texture4.name.LastIndexOf('/') + 1));
				if (num3 != -1)
				{
					if (material == null)
					{
						material = new Material(renderer.sharedMaterial);
					}
					material.SetTexture("_SpecMap", newTextures[num3].texture);
				}
			}
			if (material != null)
			{
				list.Add(renderer.sharedMaterial);
				list2.Add(material);
				renderer.sharedMaterial = material;
			}
		}
	}

	public void DebugHeirarchy(GameObject obj)
	{
		Debug.Log(obj.name + " " + obj.activeSelf + " " + obj.activeInHierarchy);
		foreach (Transform item in obj.transform)
		{
			DebugHeirarchy(item.gameObject);
		}
	}

	public void SetFogFactor(GameObject model, float fogFactor)
	{
		int nameID = Shader.PropertyToID("_UnderwaterFogFactor");
		Renderer[] componentsInChildren = model.GetComponentsInChildren<Renderer>();
		int num = componentsInChildren.Length;
		while (num-- > 0)
		{
			Renderer renderer = componentsInChildren[num];
			int num2 = renderer.sharedMaterials.Length;
			while (num2-- > 0)
			{
				Material material = renderer.sharedMaterials[num2];
				if (material != null && material.HasProperty(nameID))
				{
					material.SetFloat(nameID, fogFactor);
				}
			}
		}
	}

	public void SetTwoSidedShadows(GameObject model)
	{
		Renderer[] componentsInChildren = model.GetComponentsInChildren<Renderer>();
		int num = componentsInChildren.Length;
		while (num-- > 0)
		{
			Renderer renderer = componentsInChildren[num];
			if (renderer.shadowCastingMode == ShadowCastingMode.On)
			{
				renderer.shadowCastingMode = ShadowCastingMode.TwoSided;
			}
		}
	}

	public static void StripGameObject<T>(GameObject part, string name) where T : Component
	{
		T[] componentsInChildren = part.GetComponentsInChildren<T>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (componentsInChildren[i].gameObject.name == name)
			{
				UnityEngine.Object.DestroyImmediate(componentsInChildren[i].gameObject);
			}
		}
	}

	public static void StripTaggedTransforms(Transform root, string tag)
	{
		int childCount = root.childCount;
		while (childCount-- > 0)
		{
			Transform child = root.GetChild(childCount);
			if (child.gameObject.CompareTag(tag))
			{
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
			else
			{
				StripTaggedTransforms(child, tag);
			}
		}
	}

	public static void StripComponent<T>(GameObject part) where T : Component
	{
		T[] componentsInChildren = part.GetComponentsInChildren<T>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			UnityEngine.Object.DestroyImmediate(componentsInChildren[i]);
		}
	}

	public static void StripComponent<T>(GameObject part, string tag, bool parseChildren = false) where T : Component
	{
		T[] componentsInChildren = part.GetComponentsInChildren<T>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (!(componentsInChildren[i] == null) && componentsInChildren[i].gameObject.CompareTag(tag))
			{
				if (parseChildren)
				{
					StripComponent<T>(componentsInChildren[i].gameObject);
				}
				UnityEngine.Object.DestroyImmediate(componentsInChildren[i]);
			}
		}
	}

	public List<string> GetUnlockedCargoPartNames(List<AvailablePart> inputPartList)
	{
		List<string> list = new List<string>();
		bool flag = HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION;
		for (int i = 0; i < inputPartList.Count; i++)
		{
			if (flag || ResearchAndDevelopment.PartModelPurchased(inputPartList[i]))
			{
				list.Add(inputPartList[i].name);
			}
		}
		return list;
	}

	public List<AvailablePart> GetUnlockedCargoParts(List<AvailablePart> inputPartList)
	{
		return GetUnlockedCargoParts(inputPartList, includeNotPurchased: false);
	}

	public List<AvailablePart> GetUnlockedCargoParts(List<AvailablePart> inputPartList, bool includeNotPurchased)
	{
		List<AvailablePart> list = new List<AvailablePart>();
		bool flag = HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION;
		for (int i = 0; i < inputPartList.Count; i++)
		{
			if (flag || (includeNotPurchased && ResearchAndDevelopment.PartTechAvailable(inputPartList[i])) || (!includeNotPurchased && ResearchAndDevelopment.PartModelPurchased(inputPartList[i])))
			{
				list.Add(inputPartList[i]);
			}
		}
		return list;
	}

	public List<AvailablePart> GetAvailableDeployedScienceExpParts()
	{
		return GetUnlockedCargoParts(deployedScienceExperimentParts);
	}

	public List<string> GetAvailableCargoPartNames()
	{
		return GetUnlockedCargoPartNames(cargoParts);
	}

	public List<string> GetAvailableCargoInventoryPartNames()
	{
		return GetUnlockedCargoPartNames(cargoInventoryParts);
	}

	public List<AvailablePart> GetAvailableCargoParts()
	{
		return GetUnlockedCargoParts(cargoParts);
	}

	public List<AvailablePart> GetAvailableAndPurchaseableCargoParts()
	{
		return GetUnlockedCargoParts(cargoParts, includeNotPurchased: true);
	}

	public List<AvailablePart> GetAvailableCargoInventoryParts()
	{
		return GetUnlockedCargoParts(cargoInventoryParts);
	}

	public List<AvailablePart> GetAvailableRobotArmScannerParts()
	{
		return GetUnlockedCargoParts(robotArmScannerParts);
	}

	public void ClearAll()
	{
		int count = loadedParts.Count;
		while (count-- > 0)
		{
			UnityEngine.Object.Destroy(loadedParts[count].iconPrefab.gameObject);
			UnityEngine.Object.Destroy(loadedParts[count].partPrefab.gameObject);
		}
		loadedParts.Clear();
		loadedVariantThemes.Clear();
		int i = initialPropsLength;
		for (int count2 = internalProps.Count; i < count2; i++)
		{
			UnityEngine.Object.Destroy(internalProps[i].gameObject);
		}
		internalProps.RemoveRange(initialPropsLength, internalProps.Count - initialPropsLength);
		int j = initialInternalPartsLength;
		for (int count3 = internalParts.Count; j < count3; j++)
		{
			UnityEngine.Object.Destroy(internalParts[j].gameObject);
		}
		internalParts.RemoveRange(initialInternalPartsLength, internalParts.Count - initialInternalPartsLength);
	}
}
