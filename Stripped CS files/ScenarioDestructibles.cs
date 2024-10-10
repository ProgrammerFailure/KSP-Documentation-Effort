using System.Collections.Generic;
using UnityEngine;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.FLIGHT,
	GameScenes.EDITOR,
	GameScenes.TRACKSTATION
})]
public class ScenarioDestructibles : ScenarioModule
{
	public class ProtoDestructible
	{
		public ConfigNode configNode;

		public List<DestructibleBuilding> dBuildingRefs;

		public bool isIntact;

		public float FacilityDamageFraction;

		public ProtoDestructible(DestructibleBuilding dBuilding)
		{
			dBuildingRefs = new List<DestructibleBuilding>();
			dBuildingRefs.Add(dBuilding);
			configNode = new ConfigNode(dBuilding.id);
			dBuilding.Save(configNode);
			FacilityDamageFraction = dBuilding.FacilityDamageFraction;
		}

		public ProtoDestructible(ConfigNode node)
		{
			dBuildingRefs = new List<DestructibleBuilding>();
			configNode = node;
		}

		public void AddRef(DestructibleBuilding dBuilding)
		{
			if (dBuildingRefs.Count == 0)
			{
				FacilityDamageFraction = dBuilding.FacilityDamageFraction;
			}
			dBuildingRefs.Add(dBuilding);
		}

		public void Load(ConfigNode node)
		{
			configNode = node;
			int count = dBuildingRefs.Count;
			while (count-- > 0)
			{
				dBuildingRefs[count].Load(configNode);
			}
			GetDamage();
		}

		public void Save(ConfigNode node)
		{
			if (dBuildingRefs.Count > 0)
			{
				dBuildingRefs[0].Save(node);
				configNode = node;
				isIntact = dBuildingRefs[0].IsIntact;
			}
			else
			{
				configNode.CopyTo(node);
			}
		}

		public float GetDamage()
		{
			if (dBuildingRefs.Count > 0)
			{
				isIntact = dBuildingRefs[0].IsIntact;
			}
			else
			{
				if (configNode == null)
				{
					return 0f;
				}
				if (configNode.HasValue("intact"))
				{
					isIntact = bool.Parse(configNode.GetValue("intact"));
				}
			}
			if (!isIntact)
			{
				return FacilityDamageFraction;
			}
			return 0f;
		}
	}

	public static ScenarioDestructibles Instance;

	public static Dictionary<string, ProtoDestructible> protoDestructibles = new Dictionary<string, ProtoDestructible>();

	public static Dictionary<string, List<ProtoDestructible>> facilityToDestructibles = new Dictionary<string, List<ProtoDestructible>>();

	public override void OnAwake()
	{
		if (Instance != null)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		protoDestructibles.Clear();
		facilityToDestructibles.Clear();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override void OnSave(ConfigNode node)
	{
		Dictionary<string, ProtoDestructible>.KeyCollection.Enumerator enumerator = protoDestructibles.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string current = enumerator.Current;
			protoDestructibles[current].Save(node.AddNode(current));
		}
	}

	public static string GetFacility(string dName)
	{
		int num = 0;
		int num2 = 0;
		int length = dName.Length;
		for (int i = 0; i < length; i++)
		{
			if (dName[i] == '/')
			{
				num2++;
				if (num2 == 2)
				{
					num = i;
					break;
				}
			}
		}
		if (num > 0)
		{
			return dName.Substring(0, num);
		}
		return dName;
	}

	public static bool AddTo(Dictionary<string, List<ProtoDestructible>> dict, string key, ProtoDestructible pD)
	{
		if (dict.TryGetValue(key, out var value))
		{
			value.AddUnique(pD);
			return false;
		}
		value = new List<ProtoDestructible>();
		value.Add(pD);
		dict[key] = value;
		return true;
	}

	public static void RemoveFrom(Dictionary<string, List<ProtoDestructible>> dict, string key, ProtoDestructible pD)
	{
		if (dict.TryGetValue(key, out var value))
		{
			value.Remove(pD);
			if (value.Count == 0)
			{
				dict.Remove(key);
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		Debug.Log("[ScenarioDestructibles]: Loading... " + protoDestructibles.Keys.Count + " objects registered");
		int count = node.nodes.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			if (protoDestructibles.ContainsKey(configNode.name))
			{
				protoDestructibles[configNode.name].Load(configNode);
				continue;
			}
			ProtoDestructible protoDestructible = new ProtoDestructible(configNode);
			protoDestructibles.Add(configNode.name, protoDestructible);
			AddTo(facilityToDestructibles, GetFacility(configNode.name), protoDestructible);
		}
	}

	public static bool RegisterDestructible(DestructibleBuilding dBuilding, string id)
	{
		if (!protoDestructibles.ContainsKey(id))
		{
			ProtoDestructible protoDestructible = new ProtoDestructible(dBuilding);
			protoDestructibles.Add(id, protoDestructible);
			AddTo(facilityToDestructibles, GetFacility(id), protoDestructible);
		}
		else
		{
			if (!protoDestructibles[id].dBuildingRefs.Contains(dBuilding))
			{
				protoDestructibles[id].AddRef(dBuilding);
			}
			if (Instance != null && protoDestructibles[id].configNode != null)
			{
				dBuilding.Load(protoDestructibles[id].configNode);
				return true;
			}
		}
		return false;
	}

	public static void UnregisterDestructible(DestructibleBuilding dBuilding, string id, bool saveState)
	{
		if (protoDestructibles.ContainsKey(id) && protoDestructibles[id].dBuildingRefs.Contains(dBuilding))
		{
			if (saveState)
			{
				protoDestructibles[id].Save(new ConfigNode(id));
			}
			protoDestructibles[id].dBuildingRefs.Remove(dBuilding);
		}
	}

	public static float GetFacilityDamage(string facilityName)
	{
		if (Instance != null)
		{
			float num = 0f;
			facilityName = ScenarioUpgradeableFacilities.SlashSanitize(facilityName);
			if (facilityToDestructibles.TryGetValue(facilityName, out var value))
			{
				int count = value.Count;
				while (count-- > 0)
				{
					num += value[count].GetDamage();
				}
			}
			return num;
		}
		return 0f;
	}
}
