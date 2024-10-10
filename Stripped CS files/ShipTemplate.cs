using System;
using System.Collections.Generic;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using UnityEngine;

[Serializable]
public class ShipTemplate
{
	public string shipName = "";

	public string shipDescription = "";

	public string filename = "";

	public int shipType;

	public int partCount;

	public bool isControllable;

	public ConfigNode config;

	public ConfigNode rootPartNode;

	public bool shipPartsUnlocked = true;

	public bool shipPartsExperimental;

	public bool duplicatedParts;

	public float totalCost;

	public float fuelCost;

	public float dryCost;

	public float totalMass;

	public int stageCount;

	public Vector3 shipSize;

	public Vector3 GetShipSize()
	{
		return ShipConstruction.CalculateCraftSize(this);
	}

	public void LoadShip(string filename, ConfigNode root)
	{
		this.filename = filename;
		LoadShip(root);
	}

	public void LoadShip(ConfigNode root)
	{
		config = root;
		partCount = root.CountNodes;
		shipName = root.GetValue("ship");
		if (root.HasValue("size"))
		{
			shipSize = KSPUtil.ParseVector3(root.GetValue("size"));
		}
		int count = root.nodes.Count;
		Dictionary<string, ConfigNode> dictionary = new Dictionary<string, ConfigNode>();
		int index = count;
		while (true)
		{
			if (index-- > 0)
			{
				if (root.nodes[index].name == "PART")
				{
					if (dictionary.ContainsKey(root.nodes[index].GetValue("part")))
					{
						break;
					}
					dictionary.Add(root.nodes[index].GetValue("part"), root.nodes[index]);
				}
				continue;
			}
			bool flag = false;
			List<string> list = new List<string>();
			if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER) && MissionSystem.missions.Count > 0)
			{
				if (MissionSystem.missions[0].situation.partFilter != null)
				{
					list = MissionSystem.missions[0].situation.partFilter.GetExcludedParts();
					if (list.Count > 0)
					{
						flag = true;
					}
				}
				if (MissionSystem.missions[0].situation.VesselsArePending)
				{
					for (int i = 0; i < MissionSystem.missions[0].situation.vesselSituationList.KeysList.Count; i++)
					{
						VesselSituation vesselSituation = MissionSystem.missions[0].situation.vesselSituationList.KeysList[i];
						if (!vesselSituation.launched && vesselSituation.partFilter != null && vesselSituation.partFilter.GetExcludedParts().Count > 0)
						{
							list.AddUniqueRange(vesselSituation.partFilter.GetExcludedParts());
							flag = true;
						}
					}
				}
			}
			for (int j = 0; j < count; j++)
			{
				ConfigNode configNode = root.nodes[j];
				string value = configNode.GetValue("part");
				if (value == null)
				{
					partCount = 0;
					continue;
				}
				value = value.Substring(0, value.IndexOf('_'));
				AvailablePart partInfoByName = PartLoader.getPartInfoByName(value);
				if (partInfoByName == null)
				{
					Debug.LogError("[ShipConstruct]: Trying to load " + shipName + " - No AvailablePart found for " + value);
					shipPartsUnlocked = false;
					continue;
				}
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				float dryMass = 0f;
				float fuelMass = 0f;
				num = ShipConstruction.GetPartCostsAndMass(configNode, partInfoByName, out num3, out num2, out dryMass, out fuelMass);
				totalCost += num;
				fuelCost += num2;
				dryCost += num3;
				totalMass += dryMass + fuelMass;
				int count2 = configNode.values.Count;
				while (count2-- > 0)
				{
					if (configNode.values[count2].name == "link")
					{
						dictionary.Remove(configNode.values[count2].value);
					}
					if (configNode.values[count2].name == "istg")
					{
						stageCount = Mathf.Max(stageCount, int.Parse(configNode.values[count2].value));
					}
				}
				if (ResearchAndDevelopment.IsExperimentalPart(partInfoByName))
				{
					shipPartsExperimental = true;
				}
				else if (!ResearchAndDevelopment.PartTechAvailable(partInfoByName))
				{
					shipPartsUnlocked = false;
					continue;
				}
				if (!flag)
				{
					continue;
				}
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] == value)
					{
						shipPartsUnlocked = false;
						break;
					}
				}
			}
			int count3 = root.values.Count;
			while (count3-- > 0)
			{
				ConfigNode.Value value2 = root.values[count3];
				switch (value2.name)
				{
				case "description":
					shipDescription = value2.value.Replace('\u00a8', '\n');
					break;
				case "type":
					if (value2.value == "SPH")
					{
						shipType = 1;
					}
					if (value2.value == "VAB")
					{
						shipType = 0;
					}
					break;
				case "ship":
					shipName = value2.value;
					break;
				}
			}
			count = dictionary.Count;
			if (count != 1)
			{
				Debug.LogError("[ShipTemplate]: Could not locate root part in " + shipName + " as " + count + " entries remain after eliminating all parts listed as children. This is probably wrong.");
			}
			else
			{
				Dictionary<string, ConfigNode>.Enumerator enumerator = dictionary.GetEnumerator();
				enumerator.MoveNext();
				rootPartNode = enumerator.Current.Value;
			}
			stageCount++;
			SetIfControllable();
			return;
		}
		duplicatedParts = true;
		shipPartsUnlocked = false;
	}

	public static string GetPartName(ConfigNode partNode)
	{
		string value = partNode.GetValue("part");
		return value.Substring(0, value.IndexOf('_'));
	}

	public static string GetPartId(ConfigNode partNode)
	{
		string value = partNode.GetValue("part");
		int num = value.IndexOf('_') + 1;
		return value.Substring(num, value.Length - num);
	}

	public void SetIfControllable()
	{
		ConfigNode node = config.GetNode("PART");
		if (node == null)
		{
			Debug.LogError(shipName + " has no root part defined, aborting");
			isControllable = false;
			return;
		}
		ConfigNode[] nodes = node.GetNodes("MODULE");
		int num = nodes.Length;
		int num2 = 0;
		while (true)
		{
			if (num2 < num)
			{
				if (nodes[num2].GetValue("name") == "ModuleCommand")
				{
					break;
				}
				num2++;
				continue;
			}
			return;
		}
		isControllable = true;
	}
}
