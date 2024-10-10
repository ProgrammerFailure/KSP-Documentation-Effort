using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using UnityEngine;

public class Game
{
	public enum Modes
	{
		[Description("#autoLOC_190706")]
		SANDBOX,
		[Description("#autoLOC_190722")]
		CAREER,
		[Description("#autoLOC_6003000")]
		SCENARIO,
		[Description("#autoLOC_6003000")]
		SCENARIO_NON_RESUMABLE,
		[Description("#autoLOC_190714")]
		SCIENCE_SANDBOX,
		MISSION,
		MISSION_BUILDER
	}

	public enum GameStatus
	{
		UNSTARTED,
		ONGOING,
		FAILED_OR_ABORTED,
		COMPLETED
	}

	public string Title;

	public string Description;

	public string linkURL;

	public string linkCaption;

	public bool modded;

	public Dictionary<string, bool> loaderInfo = new Dictionary<string, bool>();

	public DictionaryValueList<string, int> cometNames = new DictionaryValueList<string, int>();

	public Modes Mode;

	public GameStatus Status;

	public int Seed = -1;

	public int ROCSeed = -1;

	public GameScenes startScene;

	public EditorFacility editorFacility;

	public FlightState flightState;

	public GameParameters Parameters;

	public KerbalRoster CrewRoster;

	public Mission missionToStart;

	public List<ProtoScenarioModule> scenarios;

	public ConfigNode additionalSystems;

	public ConfigNode config;

	public const int lastCompatibleMajor = 0;

	public const int lastCompatibleMinor = 21;

	public const int lastCompatibleRev = 0;

	public bool compatible;

	public int file_version_major;

	public int file_version_minor;

	public int file_version_revision;

	public string versionFull = "";

	public string versionCreated = "unknown";

	public string flagURL;

	public uint launchID = 1u;

	public string defaultVABLaunchSite = "LaunchPad";

	public string defaultSPHLaunchSite = "Runway";

	public bool IsMissionMode
	{
		get
		{
			if (Mode != Modes.MISSION)
			{
				return Mode == Modes.MISSION_BUILDER;
			}
			return true;
		}
	}

	public double UniversalTime
	{
		get
		{
			if (flightState == null)
			{
				return 0.0;
			}
			return flightState.universalTime;
		}
	}

	public bool CurrenciesAvailable
	{
		get
		{
			switch (Mode)
			{
			default:
				return true;
			case Modes.SCIENCE_SANDBOX:
				return ResearchAndDevelopment.Instance != null;
			case Modes.CAREER:
				if (Funding.Instance != null && ResearchAndDevelopment.Instance != null)
				{
					return Reputation.Instance != null;
				}
				return false;
			}
		}
	}

	public Game()
	{
		Title = "";
		Description = "";
		linkURL = "";
		linkCaption = "";
		modded = GameDatabase.Modded;
		GameDatabase.UpdateLoaderInfo(loaderInfo);
		flagURL = "";
		Mode = Modes.SANDBOX;
		Status = GameStatus.ONGOING;
		PopulateGameSeed();
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			PopulateROCGameSeed();
		}
		startScene = GameScenes.SPACECENTER;
		editorFacility = EditorFacility.None;
		Parameters = new GameParameters();
		scenarios = new List<ProtoScenarioModule>();
		CrewRoster = new KerbalRoster(Modes.SANDBOX);
		file_version_major = Versioning.version_major;
		file_version_minor = Versioning.version_minor;
		file_version_revision = Versioning.Revision;
		compatible = true;
		versionCreated = (versionFull = Versioning.GetVersionStringFull());
		versionFull = Versioning.GetVersionStringFull();
		Debug.Log("Game State Created.");
		GameEvents.onGameStateCreated.Fire(this);
	}

	public Game(ConfigNode root)
	{
		if (root != null && root.HasNode("GAME"))
		{
			linkCaption = "";
			linkURL = "";
			ConfigNode node = root.GetNode("GAME");
			foreach (ConfigNode.Value value in node.values)
			{
				switch (value.name)
				{
				case "launchID":
					launchID = uint.Parse(value.value);
					break;
				case "Status":
					Status = (GameStatus)int.Parse(value.value);
					break;
				case "scene":
					startScene = (GameScenes)int.Parse(value.value);
					break;
				case "ROCSeed":
					if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
					{
						ROCSeed = int.Parse(value.value);
					}
					else
					{
						ROCSeed = -1;
					}
					break;
				case "version":
				{
					string[] array = value.value.Split('.');
					file_version_major = int.Parse(array[0]);
					file_version_minor = int.Parse(array[1]);
					file_version_revision = int.Parse(array[2]);
					VersionCompareResult versionCompareResult = KSPUtil.CheckVersion(value.value, 0, 21, 0);
					compatible = versionCompareResult == VersionCompareResult.COMPATIBLE;
					if (!compatible)
					{
						Debug.Log("Save compatibility check failed: " + value.value + " vs " + 0 + "." + 21 + "." + 0 + ": " + versionCompareResult);
						return;
					}
					break;
				}
				case "Title":
					Title = value.value;
					break;
				case "modded":
				{
					if (bool.TryParse(value.value, out var result2))
					{
						modded |= result2;
					}
					break;
				}
				case "editor":
					editorFacility = (EditorFacility)Enum.Parse(typeof(EditorFacility), value.value);
					break;
				case "Mode":
				{
					if (int.TryParse(value.value, out var result))
					{
						switch (result)
						{
						default:
							Mode = Modes.SANDBOX;
							break;
						case 1:
							Mode = Modes.CAREER;
							break;
						case 2:
							Mode = Modes.SCENARIO;
							break;
						case 3:
							Mode = Modes.SCENARIO_NON_RESUMABLE;
							break;
						}
					}
					else
					{
						Mode = (Modes)Enum.Parse(typeof(Modes), value.value);
					}
					break;
				}
				case "versionCreated":
					versionCreated = value.value;
					break;
				case "Description":
					Description = value.value;
					Description = Description.Replace("^", "\n");
					break;
				case "flag":
					flagURL = value.value;
					break;
				case "linkURL":
					linkURL = value.value;
					break;
				case "defaultSPHLaunchSite":
					defaultSPHLaunchSite = value.value;
					break;
				case "Seed":
					Seed = int.Parse(value.value);
					break;
				case "versionFull":
					versionFull = value.value;
					break;
				case "linkCaption":
					linkCaption = value.value;
					break;
				case "defaultVABLaunchSite":
					defaultVABLaunchSite = value.value;
					break;
				}
			}
			if (Mode != 0 && Mode != Modes.SCIENCE_SANDBOX)
			{
				defaultVABLaunchSite = "LaunchPad";
				defaultSPHLaunchSite = "Runway";
			}
			PopulateGameSeed();
			if (!node.HasNode("ROSTER"))
			{
				Debug.LogError("[GamePersistence Error]: No ROSTER node found in save state: " + Title);
				return;
			}
			CrewRoster = new KerbalRoster(node.GetNode("ROSTER"), Mode);
			scenarios = new List<ProtoScenarioModule>();
			foreach (ConfigNode node2 in node.nodes)
			{
				switch (node2.name)
				{
				case "SCENARIO":
				{
					ProtoScenarioModule protoScenarioModule = new ProtoScenarioModule(node2);
					if (protoScenarioModule.moduleName == "DeployedScience")
					{
						if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
						{
							scenarios.Add(protoScenarioModule);
						}
						else
						{
							Debug.LogWarning("[GamePersistence]: Game contains DeployedScience Data but Breaking Ground DLC is not installed, skipping.");
						}
					}
					else
					{
						scenarios.Add(protoScenarioModule);
					}
					break;
				}
				case "CometNames":
					LoadCometNames(node2);
					break;
				case "LoaderInfo":
					loaderInfo.Clear();
					GameDatabase.LoadLoaderInfo(node2, loaderInfo);
					break;
				case "PARAMETERS":
					Parameters = new GameParameters(node2);
					break;
				case "FLIGHTSTATE":
					flightState = new FlightState(node2, this);
					break;
				case "RemovedROCs":
					if (ExpansionsLoader.IsExpansionInstalled("Serenity") && (bool)ROCManager.Instance)
					{
						ROCManager.Instance.LoadRemovedROCs(node2);
					}
					break;
				case "MISSIONTOSTART":
					MissionSystem.RemoveMissionObjects(removeAll: true);
					missionToStart = Mission.Spawn();
					missionToStart.Load(node2);
					break;
				}
			}
			if (Parameters == null)
			{
				Parameters = new GameParameters();
			}
			config = node;
		}
		else
		{
			Title = "";
			Mode = Modes.SANDBOX;
			Status = GameStatus.ONGOING;
			PopulateGameSeed();
			startScene = GameScenes.SPACECENTER;
			editorFacility = EditorFacility.None;
			flagURL = "";
			linkCaption = "";
			linkURL = "";
			defaultVABLaunchSite = "LaunchPad";
			defaultSPHLaunchSite = "Runway";
			modded = GameDatabase.Modded;
			file_version_major = Versioning.version_major;
			file_version_minor = Versioning.version_minor;
			file_version_revision = Versioning.Revision;
			if (root == null)
			{
				Debug.LogError("[GamePersistence]: Game state root config node is null, this should never happen. Initializing with default game and flight states.");
				Description = "Defaulted from corrupt or missing SFS file";
				flightState = new FlightState();
			}
			else
			{
				Debug.LogError("[GamePersistence]: Game state from older save file was missing. Initializing with default game states.");
				Description = "Upgraded from old SFS file";
				flightState = new FlightState(root, this);
			}
			compatible = flightState.compatible;
			scenarios = new List<ProtoScenarioModule>();
			Parameters = new GameParameters();
			CrewRoster = new KerbalRoster(Mode);
		}
		GameEvents.onGameStateCreated.Fire(this);
	}

	public Game Updated()
	{
		if (HighLogic.CurrentGame.Mode != Modes.MISSION && HighLogic.CurrentGame.Mode != Modes.MISSION_BUILDER)
		{
			return Updated(GameScenes.SPACECENTER);
		}
		return Updated(GameScenes.LOADING);
	}

	public Game Updated(GameScenes startSceneOverride)
	{
		if (HighLogic.LoadedSceneHasPlanetarium)
		{
			flightState = new FlightState();
		}
		bool flag = false;
		if (HighLogic.CurrentGame != null)
		{
			Parameters = HighLogic.CurrentGame.Parameters;
			CrewRoster = HighLogic.CurrentGame.CrewRoster;
			flag = HighLogic.CurrentGame.Mode == Modes.MISSION || HighLogic.CurrentGame.Mode == Modes.MISSION_BUILDER;
			CrewRoster.ValidateAssignments(this);
		}
		if (HighLogic.LoadedSceneIsGame)
		{
			scenarios = ScenarioRunner.GetUpdatedProtoModules();
		}
		file_version_major = Versioning.version_major;
		file_version_minor = Versioning.version_minor;
		file_version_revision = Versioning.Revision;
		compatible = true;
		versionFull = Versioning.GetVersionStringFull();
		if (Parameters != null && Parameters.Flight.CanLeaveToSpaceCenter && !flag)
		{
			startScene = GameScenes.SPACECENTER;
		}
		else if (flag)
		{
			startScene = ((startSceneOverride == GameScenes.LOADING) ? HighLogic.LoadedScene : startSceneOverride);
		}
		additionalSystems = new ConfigNode();
		GameEvents.onGameStateSave.Fire(additionalSystems);
		return this;
	}

	public void Save(ConfigNode rootNode)
	{
		ConfigNode configNode = rootNode.AddNode("GAME");
		configNode.AddValue("version", file_version_major + "." + file_version_minor + "." + file_version_revision);
		configNode.AddValue("Title", Title);
		configNode.AddValue("Description", Description);
		configNode.AddValue("linkURL", linkURL);
		configNode.AddValue("linkCaption", linkCaption);
		configNode.AddValue("Mode", Mode.ToString());
		configNode.AddValue("Status", (int)Status);
		configNode.AddValue("Seed", Seed);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			configNode.AddValue("ROCSeed", ROCSeed);
		}
		if (ExpansionsLoader.IsExpansionInstalled("Serenity") && (bool)ROCManager.Instance)
		{
			ROCManager.Instance.SaveRemoveROCs(configNode.AddNode("RemovedROCs"));
		}
		SaveCometNames(configNode.AddNode("CometNames"));
		configNode.AddValue("scene", (int)startScene);
		configNode.AddValue("editor", editorFacility.ToString());
		configNode.AddValue("flag", flagURL);
		configNode.AddValue("launchID", launchID);
		configNode.AddValue("defaultVABLaunchSite", defaultVABLaunchSite);
		configNode.AddValue("defaultSPHLaunchSite", defaultSPHLaunchSite);
		Parameters.Save(configNode.AddNode("PARAMETERS"));
		foreach (ProtoScenarioModule scenario in scenarios)
		{
			scenario.Save(configNode.AddNode("SCENARIO"));
		}
		if (flightState != null)
		{
			flightState.Save(configNode.AddNode("FLIGHTSTATE"));
		}
		if (missionToStart != null)
		{
			missionToStart.Save(configNode.AddNode("MISSIONTOSTART"));
		}
		configNode.AddValue("modded", (modded || GameDatabase.Modded).ToString());
		configNode.AddValue("envInfo", GameDatabase.EnvironmentInfo);
		configNode.AddValue("versionFull", versionFull);
		configNode.AddValue("versionCreated", versionCreated);
		GameDatabase.SaveLoaderInfo(configNode.AddNode("LoaderInfo"), loaderInfo);
		CrewRoster.Save(configNode.AddNode("ROSTER"));
		if (additionalSystems == null)
		{
			additionalSystems = new ConfigNode();
			GameEvents.onGameStateSave.Fire(additionalSystems);
		}
		configNode.AddData(additionalSystems);
		GameEvents.onGameStateSaved.Fire(this);
	}

	public void MergeLoaderInfo(ConfigNode node)
	{
	}

	public void Start()
	{
		if ((HighLogic.CurrentGame.Mode == Modes.MISSION_BUILDER || HighLogic.CurrentGame.Mode == Modes.MISSION) && !ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			string signature = "";
			byte[] array = null;
			if (missionToStart != null)
			{
				signature = missionToStart.Signature;
				array = missionToStart.HashBytes;
			}
			else
			{
				array = ExpansionsLoader.GetMissionVerificationStrings(config, out signature);
			}
			if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", array, signature))
			{
				return;
			}
		}
		Status = GameStatus.ONGOING;
		ShipConstruction.ShipConfig = null;
		CrewRoster.Init(this);
		switch (startScene)
		{
		case GameScenes.FLIGHT:
			FlightDriver.StartAndFocusVessel(this, flightState.activeVesselIdx);
			break;
		default:
			HighLogic.LoadScene(startScene);
			break;
		case GameScenes.EDITOR:
			EditorDriver.StartupBehaviour = (EditorDriver.StartupBehaviours)Parameters.Editor.startUpMode;
			EditorDriver.filePathToLoad = KSPUtil.ApplicationRootPath + Parameters.Editor.craftFileToLoad;
			EditorDriver.StartEditor(editorFacility);
			break;
		}
	}

	public void Load()
	{
		if ((HighLogic.CurrentGame.Mode == Modes.MISSION_BUILDER || HighLogic.CurrentGame.Mode == Modes.MISSION) && !ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			string signature = "";
			byte[] array = null;
			if (missionToStart != null)
			{
				signature = missionToStart.Signature;
				array = missionToStart.HashBytes;
			}
			else
			{
				array = ExpansionsLoader.GetMissionVerificationStrings(config, out signature);
			}
			if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", array, signature))
			{
				return;
			}
		}
		HighLogic.CurrentGame = this;
		ScenarioRunner.SetProtoModules(scenarios);
		CrewRoster.Init(this);
		if (HighLogic.LoadedSceneHasPlanetarium && flightState != null)
		{
			flightState.Load();
		}
		if (config != null)
		{
			GameEvents.onGameStateLoad.Fire(config);
		}
	}

	public bool IsResumable()
	{
		if (Mode == Modes.SCENARIO_NON_RESUMABLE)
		{
			Debug.Log("Scenario " + Title + " is non-resumable.");
			return false;
		}
		if (Status == GameStatus.FAILED_OR_ABORTED)
		{
			Debug.Log("Scenario " + Title + " is not resumable because it was failed or aborted.");
			return false;
		}
		if (Status == GameStatus.COMPLETED)
		{
			Debug.Log("Scenario " + Title + " is not resumable because it was already completed.");
			return false;
		}
		if (startScene == GameScenes.FLIGHT)
		{
			if (flightState == null)
			{
				Debug.Log("Scenario " + Title + " cannot be resumed because it has a null flight state and starts at the flight scene.");
				return false;
			}
			if (flightState.protoVessels.Count == 0)
			{
				Debug.Log("Scenario " + Title + " cannot be resumed because it starts at the flight scene, but there are no live vessels.");
				return false;
			}
		}
		return true;
	}

	public static Game GetCloneOf(Game g)
	{
		ConfigNode configNode = new ConfigNode();
		g.Save(configNode);
		return new Game(configNode);
	}

	public ProtoScenarioModule AddProtoScenarioModule(Type typeOfScnModule, params GameScenes[] scenes)
	{
		return AddProtoScenarioModule(scenarios, returnNullIfUnchanged: true, addScenarioToScenarioListIfNew: true, typeOfScnModule, scenes);
	}

	public static ProtoScenarioModule AddProtoScenarioModule(List<ProtoScenarioModule> scenarioList, bool returnNullIfUnchanged, bool addScenarioToScenarioListIfNew, Type typeOfScnModule, params GameScenes[] scenes)
	{
		if (!typeOfScnModule.IsSubclassOf(typeof(ScenarioModule)))
		{
			throw new ArgumentException("Given ScenarioModule parameter must be a subclass of ScenarioModule", "typeOfScnModule");
		}
		if (typeOfScnModule.Name == "DeployedScience" && !ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Debug.LogWarning("[GamePersistence]: Cannot add DeployedScience Scenario, Breaking Ground DLC is not installed, skipping.");
			return null;
		}
		ProtoScenarioModule protoScenarioModule = scenarioList.Find((ProtoScenarioModule s) => s.moduleName == typeOfScnModule.Name);
		if (protoScenarioModule != null)
		{
			int count = protoScenarioModule.targetScenes.Count;
			bool flag;
			if (flag = count == scenes.Length)
			{
				int num = count;
				while (num-- > 0)
				{
					if (protoScenarioModule.targetScenes[num] != scenes[num])
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				if (returnNullIfUnchanged)
				{
					return null;
				}
				return protoScenarioModule;
			}
			Debug.Log("[Game]: ScenarioModule " + typeOfScnModule.Name + " already exists in save, but saved target scenes do not match module definition. Updating...");
			protoScenarioModule.SetTargetScenes(scenes);
			return protoScenarioModule;
		}
		ConfigNode configNode = new ConfigNode("SCENARIO");
		configNode.AddValue("name", typeOfScnModule.Name);
		string text = "";
		int i = 0;
		for (int num2 = scenes.Length; i < num2; i++)
		{
			string text2 = text;
			int num3 = (int)scenes[i];
			text = text2 + num3;
			if (i < scenes.Length - 1)
			{
				text += ", ";
			}
		}
		configNode.AddValue("scene", text);
		ProtoScenarioModule protoScenarioModule2 = new ProtoScenarioModule(configNode);
		if (addScenarioToScenarioListIfNew)
		{
			scenarioList.Add(protoScenarioModule2);
		}
		return protoScenarioModule2;
	}

	public bool RemoveProtoScenarioModule(Type typeOfScnModule)
	{
		if (!typeOfScnModule.IsSubclassOf(typeof(ScenarioModule)))
		{
			throw new ArgumentException("Given ScenarioModule parameter must be a subclass of ScenarioModule", "typeOfScnModule");
		}
		ProtoScenarioModule protoScenarioModule = scenarios.Find((ProtoScenarioModule s) => s.moduleName == typeOfScnModule.Name);
		if (protoScenarioModule != null)
		{
			scenarios.Remove(protoScenarioModule);
			return true;
		}
		return false;
	}

	public ProtoVessel AddVessel(ConfigNode protoVesselNode)
	{
		ProtoVessel protoVessel = new ProtoVessel(protoVesselNode, this);
		protoVessel.Load(flightState);
		GameEvents.onNewVesselCreated.Fire(protoVessel.vesselRef);
		return protoVessel;
	}

	public bool DestroyVessel(Vessel v)
	{
		if (!(v == null) && FlightGlobals.Vessels.Contains(v))
		{
			for (int i = 0; i < v.Parts.Count; i++)
			{
				if (v.Parts[i] != null)
				{
					UnityEngine.Object.Destroy(v.Parts[i]);
				}
			}
			FlightGlobals.RemoveVessel(v);
			return true;
		}
		return false;
	}

	public void PopulateGameSeed()
	{
		if (Seed == -1)
		{
			KSPRandom kSPRandom = new KSPRandom(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
			Seed = kSPRandom.Next();
		}
	}

	public void PopulateROCGameSeed()
	{
		if (ROCSeed <= -1)
		{
			KSPRandom kSPRandom = new KSPRandom(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
			ROCSeed = kSPRandom.Next();
		}
	}

	public override string ToString()
	{
		ConfigNode configNode = new ConfigNode();
		Save(configNode);
		return configNode.ToString();
	}

	public string GetCometNumberedName(string name)
	{
		if (HighLogic.CurrentGame.cometNames.ContainsKey(name))
		{
			HighLogic.CurrentGame.cometNames[name] = HighLogic.CurrentGame.cometNames[name] + 1;
		}
		else
		{
			HighLogic.CurrentGame.cometNames.Add(name, 1);
		}
		return name + " " + HighLogic.CurrentGame.cometNames[name];
	}

	public void SaveCometNames(ConfigNode node)
	{
		if (cometNames != null)
		{
			for (int i = 0; i < cometNames.Keys.Count; i++)
			{
				node.AddValue("CometName", cometNames.KeysList[i] + "," + cometNames[cometNames.KeysList[i]]);
			}
		}
	}

	public void LoadCometNames(ConfigNode node)
	{
		cometNames = new DictionaryValueList<string, int>();
		List<string> valuesList = node.GetValuesList("CometName");
		for (int i = 0; i < valuesList.Count; i++)
		{
			if (string.IsNullOrWhiteSpace(valuesList[i]))
			{
				continue;
			}
			string[] array = valuesList[i].Split(',');
			if (array.Length == 0 || string.IsNullOrWhiteSpace(array[0]) || cometNames.ContainsKey(array[0]))
			{
				continue;
			}
			if (array.Length < 2)
			{
				array[0] = array[0].Replace("0", "").Replace("1", "").Replace("2", "");
				cometNames.Add(array[0], 1);
				continue;
			}
			try
			{
				cometNames.Add(array[0], Convert.ToInt32(array[1]));
			}
			catch
			{
				Debug.LogWarning("Unable to restore comet name. Skipping line " + valuesList[i]);
			}
		}
	}
}
