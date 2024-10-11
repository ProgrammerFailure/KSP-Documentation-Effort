using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions;

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

	public Dictionary<string, bool> loaderInfo;

	public DictionaryValueList<string, int> cometNames;

	public Modes Mode;

	public GameStatus Status;

	public int Seed;

	public int ROCSeed;

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

	public string versionFull;

	public string versionCreated;

	public string flagURL;

	public uint launchID;

	public string defaultVABLaunchSite;

	public string defaultSPHLaunchSite;

	public bool IsMissionMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double UniversalTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CurrenciesAvailable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Game()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Game(ConfigNode root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Game Updated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Game Updated(GameScenes startSceneOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode rootNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MergeLoaderInfo(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsResumable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Game GetCloneOf(Game g)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoScenarioModule AddProtoScenarioModule(Type typeOfScnModule, params GameScenes[] scenes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ProtoScenarioModule AddProtoScenarioModule(List<ProtoScenarioModule> scenarioList, bool returnNullIfUnchanged, bool addScenarioToScenarioListIfNew, Type typeOfScnModule, params GameScenes[] scenes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveProtoScenarioModule(Type typeOfScnModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoVessel AddVessel(ConfigNode protoVesselNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool DestroyVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PopulateGameSeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PopulateROCGameSeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetCometNumberedName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveCometNames(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadCometNames(ConfigNode node)
	{
		throw null;
	}
}
