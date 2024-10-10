using System.Collections.Generic;
using ns9;

namespace Expansions.Missions.Editor;

public class MissionBiome
{
	[MEGUI_Dropdown(addDefaultOption = false, order = 1, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody body;

	[MEGUI_Dropdown(addDefaultOption = false, order = 2, SetDropDownItems = "SetBiomes", gapDisplay = true, guiName = "#autoLOC_8000136")]
	public string biomeName;

	public MissionBiome(CelestialBody celestialBody, string biomeName)
	{
		body = celestialBody;
		this.biomeName = biomeName;
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("bodyName", body.name);
		if (!string.IsNullOrEmpty(biomeName))
		{
			node.AddValue("biomeName", biomeName);
		}
	}

	public void Load(ConfigNode node)
	{
		string value = string.Empty;
		string value2 = string.Empty;
		node.TryGetValue("bodyName", ref value);
		node.TryGetValue("biomeName", ref value2);
		if (value != string.Empty)
		{
			body = FlightGlobals.GetBodyByName(value);
		}
		if (value2 != string.Empty)
		{
			biomeName = value2;
		}
	}

	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			list.Add(new MEGUIDropDownItem(FlightGlobals.Bodies[i].name, FlightGlobals.Bodies[i], FlightGlobals.Bodies[i].displayName.LocalizeRemoveGender()));
		}
		return list;
	}

	public List<MEGUIDropDownItem> SetBiomes()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		list.Add(new MEGUIDropDownItem("None", string.Empty, Localizer.Format("#autoLOC_8002051")));
		if (body.BiomeMap != null)
		{
			for (int i = 0; i < body.BiomeMap.Attributes.Length; i++)
			{
				MEGUIDropDownItem item = new MEGUIDropDownItem(body.BiomeMap.Attributes[i].name, body.BiomeMap.Attributes[i].name, body.BiomeMap.Attributes[i].displayname.LocalizeRemoveGender());
				list.Add(item);
			}
		}
		return list;
	}

	public override bool Equals(object other)
	{
		if (!(other is MissionBiome missionBiome))
		{
			return false;
		}
		if (biomeName.Equals(missionBiome.biomeName))
		{
			return body.Equals(missionBiome.body);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return biomeName.GetHashCode_Net35() ^ body.GetHashCode();
	}

	public string GetNodeBodyParameterString()
	{
		string text = (string.IsNullOrEmpty(biomeName) ? Localizer.Format("#autoLOC_8002051") : ScienceUtil.GetBiomedisplayName(body, biomeName));
		return Localizer.Format("#autoLOC_8000268", body.displayName, text);
	}
}
