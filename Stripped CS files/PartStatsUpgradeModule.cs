using System.Collections.Generic;
using ns13;
using ns9;
using UnityEngine;

public class PartStatsUpgradeModule : PartModule, IModuleInfo, IPartMassModifier, IPartCostModifier
{
	public ConfigNode upgradeNode;

	public bool isUpgraded;

	public float massOffset;

	public float costOffset;

	public bool cumulativeCost;

	public override void OnLoad(ConfigNode node)
	{
		List<ConfigNode> list = new List<ConfigNode>(node.GetNodes("PartStats"));
		if (list.Count == 0)
		{
			ConfigNode node2 = node.GetNode("UPGRADES");
			if (node2 != null)
			{
				ConfigNode[] nodes = node2.GetNodes("UPGRADE");
				for (int i = 0; i < nodes.Length; i++)
				{
					if (!nodes[i].HasValue("name__") || !PartUpgradeManager.Handler.IsEnabled(nodes[i].GetValue("name__")))
					{
						continue;
					}
					for (int j = 0; j < nodes[i].nodes.Count; j++)
					{
						ConfigNode[] nodes2 = nodes[i].nodes.GetNodes("PartStats");
						for (int k = 0; k < nodes2.Length; k++)
						{
							list.Add(nodes2[k]);
						}
					}
				}
			}
		}
		massOffset = 0f;
		costOffset = 0f;
		isUpgraded = false;
		cumulativeCost = false;
		int count;
		if (list == null || (count = list.Count) <= 0)
		{
			return;
		}
		isUpgraded = true;
		upgradeNode = new ConfigNode("PartStats");
		for (int l = 0; l < count; l++)
		{
			ConfigNode configNode = list[l];
			int m = 0;
			for (int count2 = configNode.values.Count; m < count2; m++)
			{
				if (configNode.values[m].name == "mass")
				{
					float.TryParse(configNode.values[m].value, out massOffset);
				}
				else if (configNode.values[m].name == "cost")
				{
					float.TryParse(configNode.values[m].value, out costOffset);
				}
				else if (configNode.values[m].name == "massAdd")
				{
					if (float.TryParse(configNode.values[m].value, out var result))
					{
						massOffset += result;
					}
				}
				else if (configNode.values[m].name == "costAdd")
				{
					cumulativeCost = true;
					if (float.TryParse(configNode.values[m].value, out var result2))
					{
						costOffset += result2;
					}
				}
				else if (!PartLoader.ApplyPartValue(base.part, configNode.values[m]))
				{
					Debug.LogError("[PartStatsUpgradeModule]: Could not parse value. " + configNode.values[m].name + " = " + configNode.values[m].value);
				}
				upgradeNode.SetValue(configNode.values[m].name, configNode.values[m].value, createIfNotFound: true);
			}
		}
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		return massOffset;
	}

	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		return costOffset;
	}

	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		return ModifierChangeWhen.FIXED;
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.FIXED;
	}

	public string GetModuleTitle()
	{
		return Localizer.Format("#autoLOC_6002277");
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_6002275");
	}

	public override string GetInfo()
	{
		if (isUpgraded)
		{
			string text = "<b>" + Localizer.Format("#autoLOC_234049", PartListTooltip.GetPartStats(base.part, showUpgradesAvail: false)) + ":</b>\n";
			if (costOffset != 0f)
			{
				text += Localizer.Format("#autoLOC_234052", costOffset.ToString("F0"));
			}
			return text;
		}
		return Localizer.Format("#autoLOC_234057");
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return null;
	}
}
