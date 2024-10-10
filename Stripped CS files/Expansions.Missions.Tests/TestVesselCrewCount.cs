using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestVesselCrewCount : TestVessel, IScoreableObjective
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 10, resetValue = "0", guiName = "#autoLOC_8000107", Tooltip = "#autoLOC_8000108")]
	public int crewAmount;

	[MEGUI_Dropdown(order = 20, canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	[MEGUI_Dropdown(order = 30, SetDropDownItems = "SetKerbalRoles", resetValue = "", guiName = "#autoLOC_8000111", Tooltip = "#autoLOC_8000112")]
	public string traitName;

	public int currentCrewCounter;

	public List<ProtoCrewMember> pcmList;

	public string operatorString = "";

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000106");
		traitName = "";
		pcmList = new List<ProtoCrewMember>();
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			currentCrewCounter = 0;
			if (traitName == "")
			{
				currentCrewCounter = vessel.GetVesselCrew().Count;
			}
			else
			{
				pcmList = vessel.GetVesselCrew();
				for (int i = 0; i < pcmList.Count; i++)
				{
					if (pcmList[i].trait == traitName || traitName == "Any Role")
					{
						currentCrewCounter++;
					}
				}
			}
			return comparisonOperator switch
			{
				TestComparisonOperator.LessThan => currentCrewCounter < crewAmount, 
				TestComparisonOperator.LessThanorEqual => currentCrewCounter <= crewAmount, 
				TestComparisonOperator.Equal => currentCrewCounter == crewAmount, 
				TestComparisonOperator.GreaterThanorEqual => currentCrewCounter >= crewAmount, 
				TestComparisonOperator.GreaterThan => currentCrewCounter > crewAmount, 
				_ => false, 
			};
		}
		return false;
	}

	public List<MEGUIDropDownItem> SetKerbalRoles()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		list.Add(new MEGUIDropDownItem("Any Role", "Any Role", Localizer.Format("#autoLOC_8002020")));
		for (int i = 0; i < GameDatabase.Instance.ExperienceConfigs.TraitNames.Count; i++)
		{
			list.Add(new MEGUIDropDownItem(GameDatabase.Instance.ExperienceConfigs.TraitNames[i], GameDatabase.Instance.ExperienceConfigs.TraitNames[i], GameDatabase.Instance.ExperienceConfigs.GetExperienceTraitConfig(GameDatabase.Instance.ExperienceConfigs.TraitNames[i]).Title));
		}
		return list;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004041");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "traitName")
		{
			if (!(field.GetValue().ToString() == "") && !(field.GetValue().ToString() == "Any Role"))
			{
				return Localizer.Format("#autoLOC_8004190", field.guiName, KerbalRoster.GetLocalizedExperienceTraitName(field.GetValue().ToString()));
			}
			return Localizer.Format("#autoLOC_8004190", field.guiName, "#autoLOC_8002020");
		}
		if (field.name == "crewAmount")
		{
			operatorString = "";
			switch (comparisonOperator)
			{
			case TestComparisonOperator.LessThan:
				operatorString = "<";
				break;
			case TestComparisonOperator.LessThanorEqual:
				operatorString = "<=";
				break;
			case TestComparisonOperator.Equal:
				operatorString = "=";
				break;
			case TestComparisonOperator.GreaterThanorEqual:
				operatorString = ">=";
				break;
			case TestComparisonOperator.GreaterThan:
				operatorString = ">";
				break;
			}
			return Localizer.Format("#autoLOC_8100154", field.guiName, operatorString, crewAmount.ToString("0"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("crewAmount", crewAmount);
		node.AddValue("traitName", traitName);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("crewAmount", ref crewAmount);
		node.TryGetValue("traitName", ref traitName);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonOperator.Equal);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			if (vesselID == 0)
			{
				return FlightGlobals.ActiveVessel;
			}
			uint num = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(num))
			{
				return FlightGlobals.PersistentVesselIds[num];
			}
			Debug.LogErrorFormat("[TestVesselCrewCount] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
