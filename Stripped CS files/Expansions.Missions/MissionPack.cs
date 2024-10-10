using System;
using ns9;

namespace Expansions.Missions;

[Serializable]
public class MissionPack : IConfigNode
{
	[Persistent]
	public string name;

	[Persistent]
	public string displayName;

	[Persistent]
	public string description;

	[Persistent]
	public int order;

	[Persistent]
	public string color;

	public MissionPack()
	{
		name = (displayName = (description = ""));
		order = int.MaxValue;
		color = "#5576AEFF";
	}

	public static int CompareOrder(MissionPack a, MissionPack b)
	{
		int num = a.order.CompareTo(b.order);
		if (num == 0)
		{
			num = CompareDisplayName(a, b);
		}
		return num;
	}

	public static int CompareDisplayName(MissionPack a, MissionPack b)
	{
		return a.displayName.CompareTo(b.displayName);
	}

	public void Load(ConfigNode node)
	{
		if (!node.TryGetValue("name", ref name))
		{
			name = Localizer.Format("#autoLOC_168872");
		}
		if (!node.TryGetValue("displayName", ref displayName))
		{
			displayName = Localizer.Format("#autoLOC_168872");
		}
		if (!node.TryGetValue("description", ref description))
		{
			description = Localizer.Format("#autoLOC_168872");
		}
		node.TryGetValue("order", ref order);
		node.TryGetValue("color", ref color);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("displayName", displayName);
		node.AddValue("description", description);
		node.AddValue("order", order);
		node.AddValue("color", color);
	}
}
