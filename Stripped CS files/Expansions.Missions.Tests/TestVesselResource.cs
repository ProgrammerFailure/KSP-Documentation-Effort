using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
public class TestVesselResource : TestVessel, IScoreableObjective
{
	[MEGUI_Dropdown(order = 10, SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014")]
	public string resourceName = "LiquidFuel";

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 30, resetValue = "0", guiName = "#autoLOC_8000088", Tooltip = "#autoLOC_8000089")]
	public double resourceAmount;

	[MEGUI_Dropdown(order = 20, canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	public static double epsilon = 1E-15;

	public double currentResourceAmount;

	public double maxResourceAmount;

	public string resourceDisplay;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000085");
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
			if (definition.id != -1)
			{
				if (vessel.loaded)
				{
					vessel.GetConnectedResourceTotals(definition.id, out currentResourceAmount, out maxResourceAmount);
				}
				else
				{
					currentResourceAmount = 0.0;
					maxResourceAmount = 0.0;
					for (int i = 0; i < vessel.protoVessel.protoPartSnapshots.Count; i++)
					{
						ProtoPartSnapshot protoPartSnapshot = vessel.protoVessel.protoPartSnapshots[i];
						for (int j = 0; j < protoPartSnapshot.resources.Count; j++)
						{
							if (protoPartSnapshot.resources[j].resourceName == resourceName)
							{
								currentResourceAmount += protoPartSnapshot.resources[j].amount;
								maxResourceAmount += protoPartSnapshot.resources[j].maxAmount;
							}
						}
					}
				}
				return comparisonOperator switch
				{
					TestComparisonOperator.LessThan => currentResourceAmount < resourceAmount, 
					TestComparisonOperator.LessThanorEqual => currentResourceAmount <= resourceAmount, 
					TestComparisonOperator.Equal => Math.Abs(currentResourceAmount - resourceAmount) < epsilon, 
					TestComparisonOperator.GreaterThanorEqual => currentResourceAmount >= resourceAmount, 
					TestComparisonOperator.GreaterThan => currentResourceAmount > resourceAmount, 
					_ => false, 
				};
			}
			return false;
		}
		return false;
	}

	public List<MEGUIDropDownItem> SetResourceNames()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		IEnumerator<PartResourceDefinition> enumerator = PartResourceLibrary.Instance.resourceDefinitions.GetEnumerator();
		while (enumerator.MoveNext())
		{
			PartResourceDefinition current = enumerator.Current;
			list.Add(new MEGUIDropDownItem(current.name, current.name, current.displayName));
		}
		return list;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "resourceAmount")
		{
			switch (comparisonOperator)
			{
			case TestComparisonOperator.LessThan:
				resourceDisplay = "< ";
				break;
			case TestComparisonOperator.LessThanorEqual:
				resourceDisplay = "<= ";
				break;
			case TestComparisonOperator.Equal:
				resourceDisplay = "= ";
				break;
			case TestComparisonOperator.GreaterThanorEqual:
				resourceDisplay = ">= ";
				break;
			default:
				resourceDisplay = "> ";
				break;
			}
			if (resourceAmount < 1000.0)
			{
				return resourceDisplay + resourceAmount.ToString("F1");
			}
			return resourceDisplay + resourceAmount.ToString("F0");
		}
		if (field.name == "resourceName")
		{
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
			if (definition.id != -1)
			{
				return definition.displayName;
			}
			return MissionsUtils.GetFieldValueForDisplay(field);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004028");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("resourceName", resourceName);
		node.AddValue("resourceAmount", resourceAmount);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("resourceName", ref resourceName);
		node.TryGetValue("resourceAmount", ref resourceAmount);
		if (!node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonOperator.Equal))
		{
			Debug.LogError("Failed to parse comparisonOperator from " + title);
		}
	}

	public object GetScoreModifier(Type scoreModule)
	{
		return null;
	}
}
