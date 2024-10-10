using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns11;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestResourcesRecovery : TestVessel, IScoreableObjective
{
	[MEGUI_Dropdown(order = 10, SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014")]
	public string resourceName = "LiquidFuel";

	[MEGUI_InputField(order = 30, resetValue = "1000", guiName = "#autoLOC_8000170", Tooltip = "#autoLOC_8000171")]
	public double resourcesToRecover;

	[MEGUI_Dropdown(order = 20, canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	public static double epsilon = 1E-15;

	public bool eventFound;

	public double resourcesRecovered;

	public double maxResourceAmount;

	public string operatorString;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000167");
		useActiveVessel = true;
	}

	public override void Initialized()
	{
		GameEvents.onVesselRecoveryProcessing.Add(Recovered);
	}

	public override void Cleared()
	{
		GameEvents.onVesselRecoveryProcessing.Remove(Recovered);
	}

	public void Recovered(ProtoVessel pv, MissionRecoveryDialog mrd, float x)
	{
		base.Test();
		if ((!(vessel != null) || !(vessel.id == pv.vesselID)) && vesselID != 0)
		{
			return;
		}
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
		resourcesRecovered = 0.0;
		if (definition.id == -1)
		{
			return;
		}
		List<ProtoPartSnapshot> allProtoPartsIncludingCargo = pv.GetAllProtoPartsIncludingCargo();
		for (int i = 0; i < allProtoPartsIncludingCargo.Count; i++)
		{
			ProtoPartSnapshot protoPartSnapshot = allProtoPartsIncludingCargo[i];
			for (int j = 0; j < protoPartSnapshot.resources.Count; j++)
			{
				ProtoPartResourceSnapshot protoPartResourceSnapshot = protoPartSnapshot.resources[j];
				PartResourceDefinition definition2 = PartResourceLibrary.Instance.GetDefinition(protoPartResourceSnapshot.resourceName);
				if (definition2 != null && definition2.id == definition.id)
				{
					resourcesRecovered += protoPartResourceSnapshot.amount;
				}
			}
		}
		eventFound = CompareResource(resourcesRecovered, resourcesToRecover, comparisonOperator);
	}

	public bool CompareResource(double resourcesRecovered, double resourcesToRecover, TestComparisonOperator comparisonOperator)
	{
		bool result = false;
		switch (comparisonOperator)
		{
		default:
			result = false;
			break;
		case TestComparisonOperator.LessThan:
			result = resourcesRecovered < resourcesToRecover;
			break;
		case TestComparisonOperator.LessThanorEqual:
			result = resourcesRecovered <= resourcesToRecover;
			break;
		case TestComparisonOperator.Equal:
			if (Math.Abs(resourcesRecovered - resourcesToRecover) < epsilon)
			{
				result = true;
			}
			break;
		case TestComparisonOperator.GreaterThanorEqual:
			result = resourcesRecovered >= resourcesToRecover;
			break;
		case TestComparisonOperator.GreaterThan:
			result = resourcesRecovered > resourcesToRecover;
			break;
		}
		return result;
	}

	public List<MEGUIDropDownItem> SetResourceNames()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		IEnumerator<PartResourceDefinition> enumerator = PartResourceLibrary.Instance.resourceDefinitions.GetEnumerator();
		while (enumerator.MoveNext())
		{
			PartResourceDefinition current = enumerator.Current;
			list.Add(new MEGUIDropDownItem(current.name, current.name, Localizer.Format("#autoLOC_7001301", current.displayName)));
		}
		return list;
	}

	public override bool Test()
	{
		return eventFound;
	}

	public override void ParameterSetupComplete()
	{
		base.ParameterSetupComplete();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004029");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "resourcesToRecover")
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
			return Localizer.Format("#autoLOC_8100154", field.guiName, operatorString, resourcesToRecover.ToString("0"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("comparisonOperator", comparisonOperator);
		node.AddValue("resourcesToRecover", resourcesToRecover);
		node.AddValue("resourceName", resourceName);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonOperator.GreaterThan);
		node.TryGetValue("resourcesToRecover", ref resourcesToRecover);
		node.TryGetValue("resourceName", ref resourceName);
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
			Debug.LogErrorFormat("[TestResourceRecovery] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
