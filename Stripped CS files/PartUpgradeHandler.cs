using System.Collections;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public class PartUpgradeHandler : IEnumerable
{
	public class Upgrade
	{
		public string name;

		public string partIcon;

		public string techRequired;

		public float entryCost;

		public float cost;

		public bool cumulativeCost;

		public string title;

		public string basicInfo;

		public string manufacturer;

		public string description;

		public ListDictionary<Part, PartModule> instances = new ListDictionary<Part, PartModule>();

		public virtual void SetFromInfo(ConfigNode node)
		{
			name = node.GetValue("name");
			partIcon = node.GetValue("partIcon").Replace("_", ".");
			techRequired = node.GetValue("techRequired");
			node.TryGetValue("entryCost", ref entryCost);
			node.TryGetValue("cost", ref cost);
			node.TryGetValue("cumulativeCost", ref cumulativeCost);
			title = node.GetValue("title");
			basicInfo = node.GetValue("basicInfo");
			if (!string.IsNullOrEmpty(basicInfo))
			{
				basicInfo = basicInfo.Replace("\\n", "\n");
			}
			manufacturer = node.GetValue("manufacturer");
			description = node.GetValue("description");
			if (!string.IsNullOrEmpty(description))
			{
				description = description.Replace("\\n", "\n");
			}
		}

		public virtual void SetFromUntrackedUpgrade(AvailablePart ap, ConfigNode node)
		{
			name = node.GetValue("name__");
			techRequired = node.GetValue("techRequired__");
			entryCost = 0f;
			cost = 0f;
			partIcon = ap.name;
			title = Localizer.Format("#autoLOC_6002276");
			basicInfo = string.Empty;
			manufacturer = string.Empty;
			description = string.Empty;
		}

		public virtual void AddUsedBy(Part p, PartModule m)
		{
			instances.Add(p, m);
		}

		public virtual ListDictionary<Part, PartModule> GetUsedBy()
		{
			return instances;
		}

		public virtual bool IsUsed()
		{
			return instances.Keys.Count > 0;
		}

		public virtual List<string[]> GetUsedByStrings()
		{
			List<string[]> list = new List<string[]>();
			Dictionary<Part, List<PartModule>>.Enumerator enumerator = instances.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string[] array = new string[2]
				{
					enumerator.Current.Key.partInfo.title,
					string.Empty
				};
				int i = 0;
				for (int count = enumerator.Current.Value.Count; i < count; i++)
				{
					PartModule partModule = enumerator.Current.Value[i];
					string text = partModule.GetModuleDisplayName();
					if (text == "")
					{
						text = ((partModule is IModuleInfo) ? (partModule as IModuleInfo).GetModuleTitle() : KSPUtil.PrintModuleName(partModule.moduleName));
					}
					array[1] += text;
					string text2 = string.Empty;
					bool flag = false;
					int j = 0;
					for (int count2 = partModule.upgrades.Count; j < count2; j++)
					{
						ConfigNode configNode = partModule.upgrades[j];
						string value = configNode.GetValue("name__");
						string value2;
						if (!string.IsNullOrEmpty(value) && value == name && !string.IsNullOrEmpty(value2 = configNode.GetValue("description__")))
						{
							if (flag)
							{
								text2 += " ";
							}
							text2 += value2;
							flag = true;
						}
					}
					if (flag)
					{
						ref string reference = ref array[1];
						reference = reference + ": " + text2;
					}
					array[1] += "\n";
				}
				list.Add(array);
			}
			return list;
		}
	}

	public Dictionary<string, Upgrade> upgrades = new Dictionary<string, Upgrade>();

	public ListDictionary<string, Upgrade> techToUpgrades = new ListDictionary<string, Upgrade>();

	public Dictionary<Upgrade, bool> unlocks = new Dictionary<Upgrade, bool>();

	public Dictionary<Upgrade, bool> enableds = new Dictionary<Upgrade, bool>();

	public static bool AllEnabled = true;

	public virtual void FillUpgrades()
	{
		upgrades.Clear();
		techToUpgrades.Clear();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("PARTUPGRADE");
		int i = 0;
		for (int num = configNodes.Length; i < num; i++)
		{
			ConfigNode configNode = configNodes[i];
			string value = configNode.GetValue("name");
			if (string.IsNullOrEmpty(value))
			{
				continue;
			}
			if (upgrades.ContainsKey(value))
			{
				Debug.LogError("[Upgrades]: Error! PARTUPGRADE named " + value + "already added! Skipping.");
				continue;
			}
			Upgrade upgrade = new Upgrade();
			upgrade.SetFromInfo(configNode);
			if (!string.IsNullOrEmpty(upgrade.name))
			{
				AddUpgrade(upgrade);
			}
		}
	}

	public virtual void LinkUpgrades()
	{
		int i = 0;
		for (int count = PartLoader.LoadedPartsList.Count; i < count; i++)
		{
			AvailablePart availablePart = PartLoader.LoadedPartsList[i];
			if (!(availablePart.partPrefab != null))
			{
				continue;
			}
			int j = 0;
			for (int count2 = availablePart.partPrefab.Modules.Count; j < count2; j++)
			{
				PartModule partModule = availablePart.partPrefab.Modules[j];
				int count3;
				if (partModule.upgrades == null || (count3 = partModule.upgrades.Count) == 0)
				{
					continue;
				}
				for (int k = 0; k < count3; k++)
				{
					ConfigNode configNode = partModule.upgrades[k];
					string value = configNode.GetValue("name__");
					if (!string.IsNullOrEmpty(value))
					{
						if (!upgrades.TryGetValue(value, out var value2))
						{
							value2 = new Upgrade();
							value2.SetFromUntrackedUpgrade(availablePart, configNode);
							AddUpgrade(value2);
						}
						value2.instances.Add(partModule.part, partModule);
					}
				}
			}
		}
	}

	public virtual bool AddUpgrade(Upgrade up)
	{
		bool result = RemoveUpgrade(up.name);
		upgrades.Add(up.name, up);
		if (!string.IsNullOrEmpty(up.techRequired))
		{
			techToUpgrades.Add(up.techRequired, up);
		}
		return result;
	}

	public virtual bool RemoveUpgrade(string name)
	{
		if (upgrades.TryGetValue(name, out var value))
		{
			if (!string.IsNullOrEmpty(value.techRequired))
			{
				techToUpgrades.Remove(value.techRequired, value);
			}
			upgrades.Remove(value.name);
			return true;
		}
		return false;
	}

	public virtual void OnLoad(ConfigNode node)
	{
		unlocks.Clear();
		enableds.Clear();
		if (node == null)
		{
			return;
		}
		ConfigNode node2 = node.GetNode("Unlocks");
		Upgrade value;
		if (node2 != null)
		{
			int i = 0;
			for (int count = node2.values.Count; i < count; i++)
			{
				if (upgrades.TryGetValue(node2.values[i].name, out value))
				{
					unlocks.Add(value, bool.Parse(node2.values[i].value));
				}
			}
		}
		node2 = node.GetNode("Enableds");
		if (node2 == null)
		{
			return;
		}
		int j = 0;
		for (int count2 = node2.values.Count; j < count2; j++)
		{
			if (upgrades.TryGetValue(node2.values[j].name, out value))
			{
				enableds.Add(value, bool.Parse(node2.values[j].value));
			}
		}
	}

	public virtual void OnSave(ConfigNode node)
	{
		ConfigNode configNode = node.AddNode("Unlocks");
		Dictionary<Upgrade, bool>.Enumerator enumerator = unlocks.GetEnumerator();
		while (enumerator.MoveNext())
		{
			configNode.AddValue(enumerator.Current.Key.name, enumerator.Current.Value);
		}
		configNode = node.AddNode("Enableds");
		enumerator = enableds.GetEnumerator();
		while (enumerator.MoveNext())
		{
			configNode.AddValue(enumerator.Current.Key.name, enumerator.Current.Value);
		}
	}

	public virtual bool CanHaveUpgrades()
	{
		if (HighLogic.CurrentGame != null)
		{
			if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION && HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER)
			{
				if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER && HighLogic.CurrentGame.Mode != Game.Modes.SCIENCE_SANDBOX)
				{
					return HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInSandbox;
				}
				return HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInCareer;
			}
			return HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInMission;
		}
		return true;
	}

	public virtual bool IsUnlocked(string name)
	{
		if (upgrades.TryGetValue(name, out var value))
		{
			if (HighLogic.CurrentGame != null)
			{
				if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION && HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER)
				{
					if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER && HighLogic.CurrentGame.Mode != Game.Modes.SCIENCE_SANDBOX)
					{
						if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInSandbox)
						{
							return false;
						}
					}
					else
					{
						if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInCareer)
						{
							return false;
						}
						if (!string.IsNullOrEmpty(value.techRequired) && ResearchAndDevelopment.GetTechnologyState(value.techRequired) != RDTech.State.Available)
						{
							return false;
						}
					}
				}
				else if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInMission)
				{
					return false;
				}
				if (HighLogic.CurrentGame.Parameters.Difficulty.BypassEntryPurchaseAfterResearch)
				{
					return true;
				}
			}
			if (unlocks.TryGetValue(value, out var value2))
			{
				return value2;
			}
		}
		return false;
	}

	public virtual bool IsAvailableToUnlock(string name)
	{
		if (upgrades.TryGetValue(name, out var value))
		{
			if (HighLogic.CurrentGame != null)
			{
				if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER && HighLogic.CurrentGame.Mode != Game.Modes.SCIENCE_SANDBOX)
				{
					return false;
				}
				if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInCareer)
				{
					return false;
				}
				if (!string.IsNullOrEmpty(value.techRequired) && ResearchAndDevelopment.GetTechnologyState(value.techRequired) != RDTech.State.Available)
				{
					return false;
				}
			}
			if (HighLogic.CurrentGame.Parameters.Difficulty.BypassEntryPurchaseAfterResearch)
			{
				return false;
			}
			if (unlocks.TryGetValue(value, out var value2))
			{
				return !value2;
			}
			return true;
		}
		return false;
	}

	public virtual bool IsEnabled(string name)
	{
		if (!IsUnlocked(name))
		{
			return false;
		}
		if (upgrades.TryGetValue(name, out var value))
		{
			if (AllEnabled)
			{
				return true;
			}
			if (enableds.TryGetValue(value, out var value2))
			{
				return value2;
			}
		}
		return false;
	}

	public virtual void SetUnlocked(string name, bool val)
	{
		if (upgrades.TryGetValue(name, out var value))
		{
			unlocks[value] = val;
		}
	}

	public virtual void SetEnabled(string name, bool val)
	{
		if (upgrades.TryGetValue(name, out var value))
		{
			enableds[value] = val;
		}
	}

	public virtual Upgrade GetUpgrade(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (upgrades.TryGetValue(name, out var value))
		{
			return value;
		}
		return null;
	}

	public virtual float GetUpgradeCost(string name)
	{
		if (upgrades.TryGetValue(name, out var value))
		{
			return value.entryCost;
		}
		return 0f;
	}

	public virtual bool UgpradesAllowed()
	{
		if (HighLogic.CurrentGame != null)
		{
			if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION && HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER)
			{
				if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER && HighLogic.CurrentGame.Mode != Game.Modes.SCIENCE_SANDBOX)
				{
					return HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInSandbox;
				}
				return HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInCareer;
			}
			return HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PartUpgradesInMission;
		}
		return true;
	}

	public virtual List<Upgrade> GetUpgradesForTech(string tech)
	{
		if (UgpradesAllowed() && techToUpgrades.TryGetValue(tech, out var val))
		{
			return val;
		}
		return new List<Upgrade>();
	}

	public virtual IEnumerator GetEnumerator()
	{
		return upgrades.Values.GetEnumerator();
	}
}
