using System.Collections.Generic;
using Expansions.Missions;
using ns9;
using UnityEngine;
using Upgradeables;

[KSPScenario((ScenarioCreationOptions)1056, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.EDITOR,
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION
})]
public class ScenarioUpgradeableFacilities : ScenarioModule
{
	public class ProtoUpgradeable
	{
		public ConfigNode configNode;

		public List<UpgradeableFacility> facilityRefs;

		public float level;

		public ProtoUpgradeable(UpgradeableFacility facility)
		{
			facilityRefs = new List<UpgradeableFacility>();
			facilityRefs.Add(facility);
			level = facility.GetNormLevel();
		}

		public ProtoUpgradeable(ConfigNode node)
		{
			facilityRefs = new List<UpgradeableFacility>();
			configNode = node;
		}

		public void AddRef(UpgradeableFacility facility)
		{
			if (facilityRefs.Count == 0)
			{
				level = facility.GetNormLevel();
			}
			facilityRefs.Add(facility);
		}

		public void Load(ConfigNode node)
		{
			configNode = node;
			int count = facilityRefs.Count;
			while (count-- > 0)
			{
				facilityRefs[count].Load(configNode);
			}
			GetLevel();
		}

		public void Save(ConfigNode node)
		{
			if (facilityRefs.Count > 0)
			{
				facilityRefs[0].Save(node);
				configNode = node;
				level = facilityRefs[0].FacilityLevel;
			}
			else
			{
				configNode.CopyTo(node);
			}
		}

		public float GetLevel()
		{
			if (facilityRefs.Count > 0)
			{
				level = facilityRefs[0].GetNormLevel();
			}
			else if (configNode != null && configNode.HasValue("lvl"))
			{
				level = float.Parse(configNode.GetValue("lvl"));
			}
			else
			{
				level = 1f;
			}
			return level;
		}

		public int GetLevelCount()
		{
			if (facilityRefs.Count > 0)
			{
				return facilityRefs[0].MaxLevel;
			}
			return -1;
		}
	}

	public static ScenarioUpgradeableFacilities Instance;

	public static Dictionary<string, ProtoUpgradeable> protoUpgradeables = new Dictionary<string, ProtoUpgradeable>();

	public static Dictionary<string, string> slashSanitizedStrings = new Dictionary<string, string>();

	public static Dictionary<SpaceCenterFacility, string> facilityStrings = new Dictionary<SpaceCenterFacility, string>();

	public override void OnAwake()
	{
		if (Instance != null)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		protoUpgradeables.Clear();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		if (Application.isPlaying)
		{
			GameEvents.onLevelWasLoaded.Add(CleanUpUnmanagedData);
		}
	}

	public void CleanUpUnmanagedData(GameScenes scn)
	{
		GameEvents.onLevelWasLoaded.Remove(CleanUpUnmanagedData);
		if (scn == GameScenes.MAINMENU)
		{
			Debug.Log("[ScenarioUpgradeables]: Back to Main Menu. Clearing persistent data.");
			protoUpgradeables.Clear();
			return;
		}
		bool flag = true;
		int count = HighLogic.CurrentGame.scenarios.Count;
		while (count-- > 0)
		{
			if (HighLogic.CurrentGame.scenarios[count].moduleName == "ScenarioUpgradeableFacilities")
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			Debug.Log("[ScenarioUpgradeables]: Loaded Game has no Upgradeables Scenario Module. Clearing persistent data.");
			protoUpgradeables.Clear();
		}
	}

	public static bool RegisterUpgradeable(UpgradeableFacility facilityRef, string id)
	{
		if (!protoUpgradeables.ContainsKey(id))
		{
			protoUpgradeables.Add(id, new ProtoUpgradeable(facilityRef));
		}
		else
		{
			if (!protoUpgradeables[id].facilityRefs.Contains(facilityRef))
			{
				protoUpgradeables[id].AddRef(facilityRef);
			}
			if (Instance != null)
			{
				facilityRef.Load(protoUpgradeables[id].configNode);
				return true;
			}
		}
		return false;
	}

	public static void UnregisterUpgradeable(UpgradeableFacility facilityRef, string id)
	{
		if (protoUpgradeables.ContainsKey(id) && protoUpgradeables[id].facilityRefs.Contains(facilityRef))
		{
			protoUpgradeables[id].facilityRefs.Remove(facilityRef);
		}
	}

	public static string GetFacilityName(SpaceCenterFacility facility)
	{
		return facility switch
		{
			SpaceCenterFacility.Administration => Localizer.Format("#autoLOC_6001644"), 
			SpaceCenterFacility.AstronautComplex => Localizer.Format("#autoLOC_6001643"), 
			SpaceCenterFacility.LaunchPad => Localizer.Format("#autoLOC_6001663"), 
			SpaceCenterFacility.MissionControl => Localizer.Format("#autoLOC_6001645"), 
			SpaceCenterFacility.ResearchAndDevelopment => Localizer.Format("#autoLOC_6001646"), 
			SpaceCenterFacility.Runway => Localizer.Format("#autoLOC_6001662"), 
			SpaceCenterFacility.TrackingStation => Localizer.Format("#autoLOC_6001648"), 
			SpaceCenterFacility.SpaceplaneHangar => Localizer.Format("#autoLOC_6001661"), 
			SpaceCenterFacility.VehicleAssemblyBuilding => Localizer.Format("#autoLOC_6001660"), 
			_ => "Unlocalised facility name!", 
		};
	}

	public static float GetFacilityLevel(SpaceCenterFacility facility)
	{
		if (!facilityStrings.ContainsKey(facility))
		{
			facilityStrings.Add(facility, facility.ToString());
		}
		return GetFacilityLevel(facilityStrings[facility]);
	}

	public static float GetFacilityLevel(string facilityId)
	{
		facilityId = SlashSanitize(facilityId);
		float result = 1f;
		if (protoUpgradeables.Count > 0 && protoUpgradeables.TryGetValue(facilityId, out var value))
		{
			result = value.GetLevel();
		}
		return result;
	}

	public static string SlashSanitize(string instr)
	{
		if (slashSanitizedStrings.ContainsKey(instr))
		{
			return slashSanitizedStrings[instr];
		}
		bool flag = false;
		int length = instr.Length;
		for (int i = 0; i < length; i++)
		{
			if (instr[i] == '/')
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			slashSanitizedStrings.Add(instr, instr);
		}
		else
		{
			slashSanitizedStrings.Add(instr, "SpaceCenter/" + instr);
		}
		return slashSanitizedStrings[instr];
	}

	public static int GetFacilityLevelCount(SpaceCenterFacility facility)
	{
		if (!facilityStrings.ContainsKey(facility))
		{
			facilityStrings.Add(facility, facility.ToString());
		}
		return GetFacilityLevelCount(facilityStrings[facility]);
	}

	public static int GetFacilityLevelCount(string facilityId)
	{
		facilityId = SlashSanitize(facilityId);
		int result = 1;
		if (protoUpgradeables.Count > 0)
		{
			if (protoUpgradeables.TryGetValue(facilityId, out var value))
			{
				result = value.GetLevelCount();
			}
			else
			{
				Debug.LogError("[ScenarioUpgradeableFacilities]: No facility exists with id " + facilityId);
			}
		}
		return result;
	}

	public static bool IsRunway(string launchLocation)
	{
		if (!(launchLocation == "Runway") && !(launchLocation == "Island_Airfield"))
		{
			return launchLocation == "Desert_Airfield";
		}
		return true;
	}

	public static bool IsLaunchPad(string launchLocation)
	{
		if (!(launchLocation == "LaunchPad") && !(launchLocation == "Woomerang_Launch_Site"))
		{
			return launchLocation == "Desert_Launch_Site";
		}
		return true;
	}

	public override void OnSave(ConfigNode node)
	{
		Dictionary<string, ProtoUpgradeable>.KeyCollection.Enumerator enumerator = protoUpgradeables.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string current = enumerator.Current;
			protoUpgradeables[current].Save(node.AddNode(current));
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.CountNodes == 0)
		{
			Debug.Log("[ScenarioUpgradeableFacilities]: Loading in initial state... ");
			if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
			{
				getInitialState(node).CopyTo(node);
			}
			else
			{
				getInitialMissionState(node).CopyTo(node);
			}
		}
		Debug.Log("[ScenarioUpgradeableFacilities]: Loading... " + protoUpgradeables.Keys.Count + " objects registered");
		int count = node.nodes.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			if (protoUpgradeables.ContainsKey(configNode.name))
			{
				protoUpgradeables[configNode.name].Load(configNode);
			}
			else
			{
				protoUpgradeables.Add(configNode.name, new ProtoUpgradeable(configNode));
			}
		}
	}

	public ConfigNode getInitialState(ConfigNode node)
	{
		ConfigNode configNode = new ConfigNode();
		node.CopyTo(configNode);
		PSystemSetup.SpaceCenterFacility[] spaceCenterFacilities = PSystemSetup.Instance.SpaceCenterFacilities;
		int num = spaceCenterFacilities.Length;
		for (int i = 0; i < num; i++)
		{
			UpgradeableObject componentInParent = spaceCenterFacilities[i].facilityTransform.GetComponentInParent<UpgradeableObject>();
			if (componentInParent != null)
			{
				ConfigNode configNode2 = new ConfigNode(HierarchyUtil.CompileID(componentInParent.transform, "SpaceCenter"));
				configNode2.AddValue("lvl", 0f);
				configNode.AddNode(configNode2);
			}
		}
		return configNode;
	}

	public ConfigNode getInitialMissionState(ConfigNode node)
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
		{
			Debug.LogError("[ScenarioUpgradeableFacilities]: Ended up in InitialMissionState by mistake...\n\tGetting normal InitialState...");
			getInitialState(node).CopyTo(node);
			return node;
		}
		ConfigNode configNode = new ConfigNode();
		node.CopyTo(configNode);
		PSystemSetup.SpaceCenterFacility[] spaceCenterFacilities = PSystemSetup.Instance.SpaceCenterFacilities;
		int num = spaceCenterFacilities.Length;
		for (int i = 0; i < num; i++)
		{
			UpgradeableFacility component = spaceCenterFacilities[i].facilityTransform.GetComponent<UpgradeableFacility>();
			if (component != null)
			{
				ConfigNode configNode2 = new ConfigNode(HierarchyUtil.CompileID(component.transform, "SpaceCenter"));
				float num2 = MissionsUtils.GetFacilityLimit(spaceCenterFacilities[i].facilityName, component.MaxLevel);
				configNode2.AddValue("lvl", (num2 - 1f) / (float)component.MaxLevel);
				configNode.AddNode(configNode2);
			}
		}
		return configNode;
	}

	public void CheatFacilities()
	{
		Dictionary<string, ProtoUpgradeable>.ValueCollection.Enumerator enumerator = protoUpgradeables.Values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ProtoUpgradeable current = enumerator.Current;
			int count = current.facilityRefs.Count;
			while (count-- > 0)
			{
				UpgradeableFacility upgradeableFacility = current.facilityRefs[count];
				upgradeableFacility.SetLevel(upgradeableFacility.MaxLevel);
			}
		}
	}
}
