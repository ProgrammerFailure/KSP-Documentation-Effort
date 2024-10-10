using System.Collections.Generic;
using Expansions.Missions.Runtime;
using UnityEngine;

public class ResourceGameSettings
{
	public static int defaultMaxDeltaTime = 21600;

	public List<DepletionData> _DepletionInfo;

	public List<BiomeLockData> _BiomeLockInfo;

	public List<PlanetScanData> _PlanetScanInfo;

	public int Seed { get; set; }

	public int MaxDeltaTime { get; set; }

	public ConfigNode SettingsNode { get; set; }

	public int ROCMissionSeed { get; set; }

	public void Load(ConfigNode node)
	{
		if (node.HasNode("RESOURCE_SETTINGS"))
		{
			SettingsNode = node.GetNode("RESOURCE_SETTINGS");
			Seed = GetValue(SettingsNode, "GameSeed", Seed);
			ROCMissionSeed = GetValue(SettingsNode, "ROCMissionSeed", ROCMissionSeed);
			MaxDeltaTime = GetValue(SettingsNode, "MaxDeltaTime", MaxDeltaTime);
			_DepletionInfo = SetupDepletionInfo();
			_BiomeLockInfo = SetupBiomeLockInfo();
			_PlanetScanInfo = SetupPlanetScanInfo();
			if (ResourceMap.Instance != null)
			{
				ResourceMap.Instance.ResetCache();
			}
		}
		else
		{
			if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionSystem.missions.Count > 0)
			{
				Seed = MissionSystem.missions[0].situation.resourceSeed;
				ROCMissionSeed = MissionSystem.missions[0].situation.rocMissionSeed;
			}
			else
			{
				GenerateNewSeed();
				GenerateNewROCMissionSeed();
			}
			MaxDeltaTime = defaultMaxDeltaTime;
			_DepletionInfo = new List<DepletionData>();
			_BiomeLockInfo = new List<BiomeLockData>();
			_PlanetScanInfo = new List<PlanetScanData>();
		}
	}

	public void GenerateNewSeed()
	{
		KSPRandom kSPRandom = new KSPRandom();
		Seed = kSPRandom.Next(1, int.MaxValue);
	}

	public void GenerateNewROCMissionSeed()
	{
		KSPRandom kSPRandom = new KSPRandom();
		ROCMissionSeed = kSPRandom.Next(1, int.MaxValue);
	}

	public List<DepletionData> SetupDepletionInfo()
	{
		Debug.Log("Loading Depletion Nodes");
		ConfigNode[] nodes = SettingsNode.GetNodes("DEPLETION_DATA");
		Debug.Log("DepNodeCount:  " + nodes.Length);
		return ResourceUtilities.ImportDepletionNodeList(nodes);
	}

	public List<BiomeLockData> SetupBiomeLockInfo()
	{
		Debug.Log("Loading Biome Nodes");
		ConfigNode[] nodes = SettingsNode.GetNodes("BIOME_LOCK_DATA");
		Debug.Log("BiomeNodeCount:  " + nodes.Length);
		return ResourceUtilities.ImportBiomeLockNodeList(nodes);
	}

	public List<PlanetScanData> SetupPlanetScanInfo()
	{
		Debug.Log("Loading Planet Nodes");
		ConfigNode[] nodes = SettingsNode.GetNodes("PLANET_SCAN_DATA");
		Debug.Log("PlanetNodeCount:  " + nodes.Length);
		return ResourceUtilities.ImportPlanetScanNodeList(nodes);
	}

	public List<DepletionData> GetDepletionInfo()
	{
		return _DepletionInfo ?? (_DepletionInfo = new List<DepletionData>());
	}

	public List<BiomeLockData> GetBiomeLockInfo()
	{
		return _BiomeLockInfo ?? (_BiomeLockInfo = new List<BiomeLockData>());
	}

	public List<PlanetScanData> GetPlanetScanInfo()
	{
		return _PlanetScanInfo ?? (_PlanetScanInfo = new List<PlanetScanData>());
	}

	public void Save(ConfigNode node)
	{
		if (node.HasNode("RESOURCE_SETTINGS"))
		{
			SettingsNode = node.GetNode("RESOURCE_SETTINGS");
		}
		else
		{
			SettingsNode = node.AddNode("RESOURCE_SETTINGS");
		}
		SettingsNode.AddValue("GameSeed", Seed);
		SettingsNode.AddValue("ROCMissionSeed", ROCMissionSeed);
		SettingsNode.AddValue("MaxDeltaTime", MaxDeltaTime);
		int count = _DepletionInfo.Count;
		for (int i = 0; i < count; i++)
		{
			DepletionData depletionData = _DepletionInfo[i];
			ConfigNode configNode = new ConfigNode("DEPLETION_DATA");
			configNode.AddValue("PlanetId", depletionData.PlanetId);
			configNode.AddValue("ResourceName", depletionData.ResourceName);
			int count2 = depletionData.DepletionNodes.Count;
			for (int j = 0; j < count2; j++)
			{
				DepletionNode depletionNode = depletionData.DepletionNodes[j];
				ConfigNode configNode2 = new ConfigNode("DEPLETION_NODE");
				configNode2.AddValue("X", depletionNode.Int32_0);
				configNode2.AddValue("Y", depletionNode.Int32_1);
				configNode2.AddValue("Value", depletionNode.Value);
				configNode2.AddValue("LastUpdate", depletionNode.LastUpdate);
				configNode.AddNode(configNode2);
			}
			SettingsNode.AddNode(configNode);
		}
		int count3 = _BiomeLockInfo.Count;
		for (int k = 0; k < count3; k++)
		{
			BiomeLockData biomeLockData = _BiomeLockInfo[k];
			ConfigNode configNode3 = new ConfigNode("BIOME_LOCK_DATA");
			configNode3.AddValue("PlanetId", biomeLockData.PlanetId);
			configNode3.AddValue("BiomeName", biomeLockData.BiomeName);
			SettingsNode.AddNode(configNode3);
		}
		int count4 = _PlanetScanInfo.Count;
		for (int l = 0; l < count4; l++)
		{
			PlanetScanData planetScanData = _PlanetScanInfo[l];
			ConfigNode configNode4 = new ConfigNode("PLANET_SCAN_DATA");
			configNode4.AddValue("PlanetId", planetScanData.PlanetId);
			SettingsNode.AddNode(configNode4);
		}
		if (ResourceMap.Instance != null)
		{
			ResourceMap.Instance.ResetCache();
		}
	}

	public static int GetValue(ConfigNode config, string name, int currentValue)
	{
		if (config.HasValue(name) && int.TryParse(config.GetValue(name), out var result))
		{
			return result;
		}
		return currentValue;
	}

	public static float GetValue(ConfigNode config, string name, float currentValue)
	{
		if (config.HasValue(name) && float.TryParse(config.GetValue(name), out var result))
		{
			return result;
		}
		return currentValue;
	}

	public void SaveDepletionNodeValue(int planetId, string resource, int x, int y, float value)
	{
		DepletionData depletionData = null;
		int count = _DepletionInfo.Count;
		for (int i = 0; i < count; i++)
		{
			DepletionData depletionData2 = _DepletionInfo[i];
			if (depletionData2.PlanetId == planetId && depletionData2.ResourceName == resource)
			{
				depletionData = depletionData2;
				break;
			}
		}
		if (depletionData == null)
		{
			depletionData = new DepletionData();
			depletionData.PlanetId = planetId;
			depletionData.ResourceName = resource;
			_DepletionInfo.Add(depletionData);
		}
		DepletionNode depletionNode = CheckDepletionNode(planetId, resource, x, y);
		if (depletionNode == null)
		{
			depletionNode = new DepletionNode();
			depletionNode.Int32_0 = x;
			depletionNode.Int32_1 = y;
			depletionData.DepletionNodes.Add(depletionNode);
		}
		depletionNode.Value = value;
		depletionNode.LastUpdate = Planetarium.GetUniversalTime();
	}

	public void SaveBiomeUnlockNode(int planetId, string biomeName)
	{
		int count = _BiomeLockInfo.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				BiomeLockData biomeLockData = _BiomeLockInfo[num];
				if (biomeLockData.PlanetId != planetId || !(biomeLockData.BiomeName == biomeName))
				{
					num++;
					continue;
				}
				break;
			}
			BiomeLockData item = new BiomeLockData
			{
				PlanetId = planetId,
				BiomeName = biomeName
			};
			_BiomeLockInfo.Add(item);
			break;
		}
	}

	public void SavePlanetUnlockNode(int planetId)
	{
		int count = _PlanetScanInfo.Count;
		for (int i = 0; i < count; i++)
		{
			if (_PlanetScanInfo[i].PlanetId == planetId)
			{
				return;
			}
		}
		PlanetScanData item = new PlanetScanData
		{
			PlanetId = planetId
		};
		_PlanetScanInfo.Add(item);
	}

	public DepletionNode CheckDepletionNode(int planetId, string resource, int x, int y)
	{
		DepletionData depletionData = null;
		int count = _DepletionInfo.Count;
		for (int i = 0; i < count; i++)
		{
			DepletionData depletionData2 = _DepletionInfo[i];
			if (depletionData2.PlanetId == planetId && depletionData2.ResourceName == resource)
			{
				depletionData = depletionData2;
				break;
			}
		}
		if (depletionData == null)
		{
			return null;
		}
		DepletionNode result = null;
		int count2 = depletionData.DepletionNodes.Count;
		for (int j = 0; j < count2; j++)
		{
			DepletionNode depletionNode = depletionData.DepletionNodes[j];
			if (depletionNode.Int32_0 == x && depletionNode.Int32_1 == y)
			{
				result = depletionNode;
				break;
			}
		}
		return result;
	}
}
