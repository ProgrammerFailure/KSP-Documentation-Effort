using System;
using System.Collections.Generic;
using KSPAchievements;
using ns9;
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

	public List<PointOfInterest> NonCheatablePOIs;

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

	public static ProgressTracking Instance { get; set; }

	public override void OnAwake()
	{
		Instance = this;
		achievementTree = generateAchievementsTree();
		GameEvents.OnProgressComplete.Add(OnAchievementComplete);
	}

	public ProgressTree generateAchievementsTree()
	{
		ProgressTree progressTree = new ProgressTree();
		firstLaunch = new FirstLaunch();
		progressTree.AddNode(firstLaunch);
		firstCrewToSurvive = new CrewRecovery();
		progressTree.AddNode(firstCrewToSurvive);
		towerBuzz = new TowerBuzz();
		progressTree.AddNode(towerBuzz);
		altitudeRecords = new RecordsAltitude();
		progressTree.AddNode(altitudeRecords);
		depthRecords = new RecordsDepth();
		progressTree.AddNode(depthRecords);
		speedRecords = new RecordsSpeed();
		progressTree.AddNode(speedRecords);
		distanceRecords = new RecordsDistance();
		progressTree.AddNode(distanceRecords);
		reachSpace = new ReachSpace();
		progressTree.AddNode(reachSpace);
		KSCLanding = new TargetedLanding("KSC", ReturnFrom.SubOrbit);
		progressTree.AddNode(KSCLanding);
		runwayLanding = new TargetedLanding("Runway", ReturnFrom.SubOrbit);
		progressTree.AddNode(runwayLanding);
		launchpadLanding = new TargetedLanding("LaunchPad", ReturnFrom.SubOrbit);
		progressTree.AddNode(launchpadLanding);
		POIKerbinKSC2 = new PointOfInterest("Kerbin", "KSC2", Localizer.Format("#autoLOC_297133"), uplifting: false);
		POIKerbinIslandAirfield = new PointOfInterest("Kerbin", "IslandAirfield", Localizer.Format("#autoLOC_297134"), uplifting: false, launchSite: true);
		POIKerbinDiscoverableLaunchsite01 = new PointOfInterest("Kerbin", "Crater_Launch_Site", Localizer.Format("#autoLOC_6006117"), uplifting: false, launchSite: true);
		POIKerbinDiscoverableLaunchsite02 = new PointOfInterest("Kerbin", "MahiMahi_Launch_Site", Localizer.Format("#autoLOC_6006118"), uplifting: false, launchSite: true);
		POIKerbinDiscoverableLaunchsite03 = new PointOfInterest("Kerbin", "Cove_Launch_Site", Localizer.Format("#autoLOC_6006119"), uplifting: false, launchSite: true);
		POIKerbinDiscoverableLaunchsite04 = new PointOfInterest("Kerbin", "GlacierLake_Launch_Site", Localizer.Format("#autoLOC_6006120"), uplifting: false, launchSite: true);
		POIKerbinUFO = new PointOfInterest("Kerbin", "UFO", Localizer.Format("#autoLOC_297135"), uplifting: false);
		POIKerbinPyramids = new PointOfInterest("Kerbin", "Pyramids", Localizer.Format("#autoLOC_297136"), uplifting: false);
		POIKerbinMonolith00 = new PointOfInterest("Kerbin", "Monolith00", Localizer.Format("#autoLOC_297137"), uplifting: false);
		POIKerbinMonolith01 = new PointOfInterest("Kerbin", "Monolith01", Localizer.Format("#autoLOC_297138"), uplifting: false);
		POIKerbinMonolith02 = new PointOfInterest("Kerbin", "Monolith02", Localizer.Format("#autoLOC_297139"), uplifting: false);
		POIKerbinDessertAirfield = new PointOfInterest("Kerbin", "Desert_Airfield", Localizer.Format("#autoLOC_8003184"), uplifting: false, launchSite: true);
		POIKerbinWoomerang = new PointOfInterest("Kerbin", "Woomerang_Launch_Site", Localizer.Format("#autoLOC_8003185"), uplifting: false, launchSite: true);
		POIMunArmstrongMemorial = new PointOfInterest("Mun", "ArmstrongMemorial", Localizer.Format("#autoLOC_297140"), uplifting: false);
		POIMunUFO = new PointOfInterest("Mun", "UFO", Localizer.Format("#autoLOC_297141"), uplifting: false);
		POIMunRockArch00 = new PointOfInterest("Mun", "RockArch00", Localizer.Format("#autoLOC_297142"), uplifting: false);
		POIMunRockArch01 = new PointOfInterest("Mun", "RockArch01", Localizer.Format("#autoLOC_297143"), uplifting: false);
		POIMunRockArch02 = new PointOfInterest("Mun", "RockArch02", Localizer.Format("#autoLOC_297144"), uplifting: false);
		POIMunMonolith00 = new PointOfInterest("Mun", "Monolith00", Localizer.Format("#autoLOC_297145"), uplifting: false);
		POIMunMonolith01 = new PointOfInterest("Mun", "Monolith01", Localizer.Format("#autoLOC_297146"), uplifting: false);
		POIMunMonolith02 = new PointOfInterest("Mun", "Monolith02", Localizer.Format("#autoLOC_297147"), uplifting: false);
		POIDunaPyramid = new PointOfInterest("Duna", "Pyramid", Localizer.Format("#autoLOC_297148"), uplifting: false);
		POIDunaMSL = new PointOfInterest("Duna", "MSL", Localizer.Format("#autoLOC_297149"), uplifting: false);
		POIDunaFace = new PointOfInterest("Duna", "Face", Localizer.Format("#autoLOC_297150"), uplifting: false);
		POIMinmusMonolith00 = new PointOfInterest("Minmus", "Monolith00", Localizer.Format("#autoLOC_297151"), uplifting: false);
		POITyloCave = new PointOfInterest("Tylo", "Cave", Localizer.Format("#autoLOC_297152"), uplifting: false);
		POIVallIcehenge = new PointOfInterest("Vall", "Icehenge", Localizer.Format("#autoLOC_297153"), uplifting: false);
		POIBopDeadKraken = new PointOfInterest("Bop", "DeadKraken", Localizer.Format("#autoLOC_297154"), uplifting: false);
		POIMohoRandolith = new PointOfInterest("Moho", "Randolith", Localizer.Format("#autoLOC_297156"), uplifting: true);
		POIEveRandolith = new PointOfInterest("Eve", "Randolith", Localizer.Format("#autoLOC_297157"), uplifting: true);
		POIGillyRandolith = new PointOfInterest("Gilly", "Randolith", Localizer.Format("#autoLOC_297158"), uplifting: true);
		POIKerbinRandolith = new PointOfInterest("Kerbin", "Randolith", Localizer.Format("#autoLOC_297159"), uplifting: true);
		POIMunRandolith = new PointOfInterest("Mun", "Randolith", Localizer.Format("#autoLOC_297160"), uplifting: true);
		POIMinmusRandolith = new PointOfInterest("Minmus", "Randolith", Localizer.Format("#autoLOC_297161"), uplifting: true);
		POIDunaRandolith = new PointOfInterest("Duna", "Randolith", Localizer.Format("#autoLOC_297162"), uplifting: true);
		POIIkeRandolith = new PointOfInterest("Ike", "Randolith", Localizer.Format("#autoLOC_297163"), uplifting: true);
		POIDresRandolith = new PointOfInterest("Dres", "Randolith", Localizer.Format("#autoLOC_297164"), uplifting: true);
		POILaytheRandolith = new PointOfInterest("Laythe", "Randolith", Localizer.Format("#autoLOC_297165"), uplifting: true);
		POIVallRandolith = new PointOfInterest("Vall", "Randolith", Localizer.Format("#autoLOC_297166"), uplifting: true);
		POITyloRandolith = new PointOfInterest("Tylo", "Randolith", Localizer.Format("#autoLOC_297167"), uplifting: true);
		POIBopRandolith = new PointOfInterest("Bop", "Randolith", Localizer.Format("#autoLOC_297168"), uplifting: true);
		POIPolRandolith = new PointOfInterest("Pol", "Randolith", Localizer.Format("#autoLOC_297169"), uplifting: true);
		POIEelooRandolith = new PointOfInterest("Eeloo", "Randolith", Localizer.Format("#autoLOC_297170"), uplifting: true);
		POIAnniversary1 = new PointOfInterest("Kerbin", "Anniversary1", Localizer.Format("#autoLOC_8003541"), uplifting: false);
		POIAnniversary2 = new PointOfInterest("Kerbin", "Anniversary2", Localizer.Format("#autoLOC_8003542"), uplifting: false);
		POIAnniversary3 = new PointOfInterest("Eeloo", "Anniversary3", Localizer.Format("#autoLOC_8003543"), uplifting: false);
		POIAnniversary4 = new PointOfInterest("Duna", "Anniversary4", Localizer.Format("#autoLOC_8003544"), uplifting: false);
		POIAnniversary5 = new PointOfInterest("Ike", "Anniversary5", Localizer.Format("#autoLOC_8003545"), uplifting: false);
		POIAnniversary6 = new PointOfInterest("Dres", "Anniversary6", Localizer.Format("#autoLOC_8003546"), uplifting: false);
		POIAnniversary7 = new PointOfInterest("Laythe", "Anniversary7", Localizer.Format("#autoLOC_8003547"), uplifting: false);
		NonCheatablePOIs = new List<PointOfInterest> { POIKerbinDiscoverableLaunchsite01, POIKerbinDiscoverableLaunchsite02, POIKerbinDiscoverableLaunchsite03, POIKerbinDiscoverableLaunchsite04 };
		progressTree.AddNode(POIKerbinKSC2);
		progressTree.AddNode(POIKerbinIslandAirfield);
		progressTree.AddNode(POIKerbinDiscoverableLaunchsite01);
		progressTree.AddNode(POIKerbinDiscoverableLaunchsite02);
		progressTree.AddNode(POIKerbinDiscoverableLaunchsite03);
		progressTree.AddNode(POIKerbinDiscoverableLaunchsite04);
		progressTree.AddNode(POIKerbinUFO);
		progressTree.AddNode(POIKerbinPyramids);
		progressTree.AddNode(POIKerbinMonolith00);
		progressTree.AddNode(POIKerbinMonolith01);
		progressTree.AddNode(POIKerbinMonolith02);
		progressTree.AddNode(POIKerbinDessertAirfield);
		progressTree.AddNode(POIKerbinWoomerang);
		progressTree.AddNode(POIMunArmstrongMemorial);
		progressTree.AddNode(POIMunUFO);
		progressTree.AddNode(POIMunRockArch00);
		progressTree.AddNode(POIMunRockArch01);
		progressTree.AddNode(POIMunRockArch02);
		progressTree.AddNode(POIMunMonolith00);
		progressTree.AddNode(POIMunMonolith01);
		progressTree.AddNode(POIMunMonolith02);
		progressTree.AddNode(POIDunaPyramid);
		progressTree.AddNode(POIDunaMSL);
		progressTree.AddNode(POIDunaFace);
		progressTree.AddNode(POIMinmusMonolith00);
		progressTree.AddNode(POITyloCave);
		progressTree.AddNode(POIVallIcehenge);
		progressTree.AddNode(POIBopDeadKraken);
		progressTree.AddNode(POIMohoRandolith);
		progressTree.AddNode(POIEveRandolith);
		progressTree.AddNode(POIGillyRandolith);
		progressTree.AddNode(POIKerbinRandolith);
		progressTree.AddNode(POIMunRandolith);
		progressTree.AddNode(POIMinmusRandolith);
		progressTree.AddNode(POIDunaRandolith);
		progressTree.AddNode(POIIkeRandolith);
		progressTree.AddNode(POIDresRandolith);
		progressTree.AddNode(POILaytheRandolith);
		progressTree.AddNode(POIVallRandolith);
		progressTree.AddNode(POITyloRandolith);
		progressTree.AddNode(POIBopRandolith);
		progressTree.AddNode(POIPolRandolith);
		progressTree.AddNode(POIEelooRandolith);
		progressTree.AddNode(POIAnniversary1);
		progressTree.AddNode(POIAnniversary2);
		progressTree.AddNode(POIAnniversary3);
		progressTree.AddNode(POIAnniversary4);
		progressTree.AddNode(POIAnniversary5);
		progressTree.AddNode(POIAnniversary6);
		progressTree.AddNode(POIAnniversary7);
		celestialBodyNodes = new CelestialBodySubtree[FlightGlobals.Bodies.Count];
		int i = 0;
		for (int num = celestialBodyNodes.Length; i < num; i++)
		{
			celestialBodyNodes[i] = new CelestialBodySubtree(FlightGlobals.Bodies[i]);
			progressTree.AddNode(celestialBodyNodes[i]);
		}
		int j = 0;
		for (int num2 = celestialBodyNodes.Length; j < num2; j++)
		{
			if (celestialBodyNodes[j].Body.isHomeWorld)
			{
				celestialBodyHome = celestialBodyNodes[j];
				celestialBodyNodes[j].LinkBodyHome(celestialBodyNodes);
			}
		}
		return progressTree;
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("Progress"))
		{
			achievementTree.Load(node.GetNode("Progress"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		achievementTree.Save(node.AddNode("Progress"));
	}

	public void Start()
	{
		achievementTree.Deploy();
	}

	public void Update()
	{
		if (!HighLogic.LoadedSceneIsEditor)
		{
			for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
			{
				achievementTree.IterateVessels(FlightGlobals.Vessels[i]);
			}
		}
	}

	public void OnDestroy()
	{
		achievementTree.Stow();
		GameEvents.OnProgressComplete.Remove(OnAchievementComplete);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnAchievementComplete(ProgressNode node)
	{
	}

	[ContextMenu("Debug Achievement Statuses")]
	public void DebugAchievements()
	{
		int count = achievementTree.Count;
		for (int i = 0; i < count; i++)
		{
			ProgressNode progressNode = achievementTree[i];
			Debug.Log(progressNode.Id + ": " + (progressNode.IsReached ? "Reached" : "Not Reached"));
		}
	}

	[ContextMenu("Debug Summary")]
	public void DebugSummary()
	{
		Debug.Log(GenerateSummary(), base.gameObject);
	}

	[ContextMenu("Test Posting Data")]
	public void TestProgressDataPost()
	{
	}

	public string GenerateSummary()
	{
		return achievementTree.GetTreeSummary("");
	}

	public bool NodeReached(params string[] search)
	{
		if (search.Length == 0)
		{
			return false;
		}
		return FindNode(search)?.IsReached ?? false;
	}

	public bool NodeComplete(params string[] search)
	{
		if (search.Length == 0)
		{
			return false;
		}
		return FindNode(search)?.IsComplete ?? false;
	}

	public bool NodeCompleteManned(params string[] search)
	{
		if (search.Length == 0)
		{
			return false;
		}
		return FindNode(search)?.IsCompleteManned ?? false;
	}

	public bool NodeCompleteUnmanned(params string[] search)
	{
		if (search.Length == 0)
		{
			return false;
		}
		return FindNode(search)?.IsCompleteUnmanned ?? false;
	}

	public ProgressNode FindNode(params string[] search)
	{
		if (search.Length == 0)
		{
			return null;
		}
		return FindNodeRecurse(achievementTree, 0, search);
	}

	public ProgressNode FindNodeRecurse(ProgressTree tree, int depth, string[] search)
	{
		ProgressNode progressNode = tree[search[depth]];
		if (progressNode == null)
		{
			return null;
		}
		if (depth + 1 < search.Length)
		{
			if (progressNode.Subtree.Count > 0)
			{
				return FindNodeRecurse(progressNode.Subtree, depth + 1, search);
			}
			return null;
		}
		return progressNode;
	}

	public CelestialBodySubtree GetBodyTree(string bodyName)
	{
		int num = 0;
		int num2 = celestialBodyNodes.Length;
		while (true)
		{
			if (num < num2)
			{
				if (celestialBodyNodes[num].Body.name == bodyName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return celestialBodyNodes[num];
	}

	public CelestialBodySubtree GetBodyTree(CelestialBody body)
	{
		int num = 0;
		int num2 = celestialBodyNodes.Length;
		while (true)
		{
			if (num < num2)
			{
				if (celestialBodyNodes[num].Body == body)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return celestialBodyNodes[num];
	}

	public CelestialBodySubtree GetBodyTree()
	{
		return celestialBodyHome;
	}

	public void RecurseCheatProgression(ProgressNode node)
	{
		if (node == null)
		{
			return;
		}
		bool flag = false;
		if (node is PointOfInterest)
		{
			PointOfInterest pointOfInterest = (PointOfInterest)node;
			for (int i = 0; i < NonCheatablePOIs.Count; i++)
			{
				if (pointOfInterest.name == NonCheatablePOIs[i].name && pointOfInterest.body == NonCheatablePOIs[i].body)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			return;
		}
		node.CheatComplete();
		if (node.Subtree != null)
		{
			int count = node.Subtree.Count;
			while (count-- > 0)
			{
				RecurseCheatProgression(node.Subtree[count]);
			}
		}
	}

	public void CheatProgression()
	{
		int count = achievementTree.Count;
		while (count-- > 0)
		{
			RecurseCheatProgression(achievementTree[count]);
		}
	}

	public void CheatEarlyProgression()
	{
		firstLaunch.CheatComplete();
		reachSpace.CheatComplete();
		CelestialBodySubtree bodyTree = GetBodyTree(FlightGlobals.GetHomeBody());
		ProgressNode progressNode = bodyTree?.orbit;
		ProgressNode progressNode2 = bodyTree?.science;
		progressNode?.CheatComplete();
		progressNode2?.CheatComplete();
	}
}
