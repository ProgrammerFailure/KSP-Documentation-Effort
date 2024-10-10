using System;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
public class TestMissionFunds : TestModule, IScoreableObjective
{
	[MEGUI_InputField(canBePinned = false, resetValue = "0", guiName = "#autoLOC_8003132", Tooltip = "#autoLOC_8003133")]
	public float funds;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	public static double epsilon = 1E-15;

	public string bodyFunds;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8003129");
	}

	public override bool Test()
	{
		if (Funding.Instance == null)
		{
			return true;
		}
		if (base.node != null)
		{
			return comparisonOperator switch
			{
				TestComparisonOperator.LessThan => Funding.Instance.Funds < (double)funds, 
				TestComparisonOperator.LessThanorEqual => Funding.Instance.Funds <= (double)funds, 
				TestComparisonOperator.Equal => Math.Abs((double)funds - Funding.Instance.Funds) < epsilon, 
				TestComparisonOperator.GreaterThanorEqual => Funding.Instance.Funds >= (double)funds, 
				TestComparisonOperator.GreaterThan => Funding.Instance.Funds > (double)funds, 
				_ => false, 
			};
		}
		return false;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "funds")
		{
			bodyFunds = "";
			switch (comparisonOperator)
			{
			case TestComparisonOperator.LessThan:
				bodyFunds = "<";
				break;
			case TestComparisonOperator.LessThanorEqual:
				bodyFunds = "<=";
				break;
			case TestComparisonOperator.Equal:
				bodyFunds = "=";
				break;
			case TestComparisonOperator.GreaterThanorEqual:
				bodyFunds = ">=";
				break;
			case TestComparisonOperator.GreaterThan:
				bodyFunds = ">";
				break;
			}
			bodyFunds = Localizer.Format("#autoLOC_8003131", bodyFunds, funds.ToString("0"));
			return bodyFunds;
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8003130");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("funds", funds);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("funds", ref funds);
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
