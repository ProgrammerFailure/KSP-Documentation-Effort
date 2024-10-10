using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

public class MissionCelestialBody
{
	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024", Tooltip = "#autoLOC_8006029")]
	public CelestialBody body;

	public const string AnyValidString = "AnyValid";

	public string DisplayName
	{
		get
		{
			if (!(body == null))
			{
				return body.displayName.LocalizeRemoveGender();
			}
			return Localizer.Format("#autoLOC_8000273");
		}
	}

	public string Name
	{
		get
		{
			if (!(body == null))
			{
				return body.name;
			}
			return "AnyValid";
		}
	}

	public CelestialBody Body
	{
		get
		{
			return body;
		}
		set
		{
			body = value;
		}
	}

	public bool AnyValid
	{
		get
		{
			return body == null;
		}
		set
		{
			body = null;
		}
	}

	public MissionCelestialBody()
	{
		body = FlightGlobals.GetHomeBody();
	}

	public MissionCelestialBody(CelestialBody body)
	{
		this.body = body;
	}

	public bool IsValid(CelestialBody targetbody)
	{
		bool result = true;
		if (!AnyValid)
		{
			result = body == targetbody;
		}
		return result;
	}

	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		list.Add(new MEGUIDropDownItem("AnyValid", null, Localizer.Format("#autoLOC_8000273")));
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			list.Add(new MEGUIDropDownItem(FlightGlobals.Bodies[i].name, FlightGlobals.Bodies[i], FlightGlobals.Bodies[i].displayName.LocalizeRemoveGender()));
		}
		return list;
	}

	public string GetNodeBodyParameterString()
	{
		return Localizer.Format("#autoLOC_8000267", DisplayName);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("bodyName", Name);
	}

	public void Load(ConfigNode node)
	{
		string value = string.Empty;
		node.TryGetValue("bodyName", ref value);
		if (value != string.Empty)
		{
			if (value == "AnyValid")
			{
				body = null;
			}
			else
			{
				body = FlightGlobals.GetBodyByName(value);
			}
		}
	}
}
