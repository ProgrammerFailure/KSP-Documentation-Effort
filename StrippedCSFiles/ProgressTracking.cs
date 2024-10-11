using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSPAchievements;
using UnityEngine;

[KSPScenario(ScenarioCreationOptions.AddToNewGames, new GameScenes[]
{
	GameScenes.EDITOR,
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER
})]
public class ProgressTracking : ScenarioModule
{
	public ProgressTree achievementTree;

	public FirstLaunch firstLaunch;

	public RecordsAltitude altitudeRecords;

	public RecordsDepth depthRecords;

	public RecordsSpeed speedRecords;

	public RecordsDistance distanceRecords;

	public CrewRecovery firstCrewToSurvive;

	public TowerBuzz towerBuzz;

	public TargetedLanding KSCLanding;

	public TargetedLanding runwayLanding;

	public TargetedLanding launchpadLanding;

	public ReachSpace reachSpace;

	public PointOfInterest POIKerbinKSC2;

	public PointOfInterest POIKerbinIslandAirfield;

	public PointOfInterest POIKerbinUFO;

	public PointOfInterest POIKerbinPyramids;

	public PointOfInterest POIKerbinMonolith00;

	public PointOfInterest POIKerbinMonolith01;

	public PointOfInterest POIKerbinMonolith02;

	public PointOfInterest POIKerbinDessertAirfield;

	public PointOfInterest POIKerbinWoomerang;

	public PointOfInterest POIKerbinDiscoverableLaunchsite01;

	public PointOfInterest POIKerbinDiscoverableLaunchsite02;

	public PointOfInterest POIKerbinDiscoverableLaunchsite03;

	public PointOfInterest POIKerbinDiscoverableLaunchsite04;

	private List<PointOfInterest> NonCheatablePOIs;

	public PointOfInterest POIMunArmstrongMemorial;

	public PointOfInterest POIMunUFO;

	public PointOfInterest POIMunRockArch00;

	public PointOfInterest POIMunRockArch01;

	public PointOfInterest POIMunRockArch02;

	public PointOfInterest POIMunMonolith00;

	public PointOfInterest POIMunMonolith01;

	public PointOfInterest POIMunMonolith02;

	public PointOfInterest POIDunaPyramid;

	public PointOfInterest POIDunaMSL;

	public PointOfInterest POIDunaFace;

	public PointOfInterest POIMinmusMonolith00;

	public PointOfInterest POITyloCave;

	public PointOfInterest POIVallIcehenge;

	public PointOfInterest POIBopDeadKraken;

	public PointOfInterest POIMohoRandolith;

	public PointOfInterest POIEveRandolith;

	public PointOfInterest POIGillyRandolith;

	public PointOfInterest POIKerbinRandolith;

	public PointOfInterest POIMunRandolith;

	public PointOfInterest POIMinmusRandolith;

	public PointOfInterest POIDunaRandolith;

	public PointOfInterest POIIkeRandolith;

	public PointOfInterest POIDresRandolith;

	public PointOfInterest POILaytheRandolith;

	public PointOfInterest POIVallRandolith;

	public PointOfInterest POITyloRandolith;

	public PointOfInterest POIBopRandolith;

	public PointOfInterest POIPolRandolith;

	public PointOfInterest POIEelooRandolith;

	public PointOfInterest POIAnniversary1;

	public PointOfInterest POIAnniversary2;

	public PointOfInterest POIAnniversary3;

	public PointOfInterest POIAnniversary4;

	public PointOfInterest POIAnniversary5;

	public PointOfInterest POIAnniversary6;

	public PointOfInterest POIAnniversary7;

	[NonSerialized]
	public CelestialBodySubtree[] celestialBodyNodes;

	[NonSerialized]
	public CelestialBodySubtree celestialBodyHome;

	public static ProgressTracking Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProgressTracking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ProgressTree generateAchievementsTree()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAchievementComplete(ProgressNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Debug Achievement Statuses")]
	private void DebugAchievements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Debug Summary")]
	private void DebugSummary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Test Posting Data")]
	private void TestProgressDataPost()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GenerateSummary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NodeReached(params string[] search)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NodeComplete(params string[] search)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NodeCompleteManned(params string[] search)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NodeCompleteUnmanned(params string[] search)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProgressNode FindNode(params string[] search)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ProgressNode FindNodeRecurse(ProgressTree tree, int depth, string[] search)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodySubtree GetBodyTree(string bodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodySubtree GetBodyTree(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodySubtree GetBodyTree()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RecurseCheatProgression(ProgressNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheatProgression()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheatEarlyProgression()
	{
		throw null;
	}
}
