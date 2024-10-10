using System;
using Expansions.Missions.Editor;
using ns11;
using ns9;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
public class TestFundsRecovery : TestModule, IScoreableObjective
{
	[MEGUI_InputField(canBePinned = false, resetValue = "1000", guiName = "#autoLOC_8000141", Tooltip = "#autoLOC_8000142")]
	public double fundsToRecover;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	public static double epsilon = 1E-15;

	public bool eventFound;

	public double fundsCollected;

	public string operatorString;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000233");
		eventFound = false;
	}

	public override void Initialized()
	{
		GameEvents.onVesselRecoveryProcessingComplete.Add(Recovered);
	}

	public override void Cleared()
	{
		GameEvents.onVesselRecoveryProcessingComplete.Remove(Recovered);
	}

	public void Recovered(ProtoVessel v, MissionRecoveryDialog mrd, float x)
	{
		if (mrd == null)
		{
			float f = 0f;
			float s = 0f;
			float r = 0f;
			f = CurrencyModifierQuery.RunQuery(TransactionReasons.VesselRecovery, f, s, r).GetEffectDelta(Currency.Funds);
			fundsCollected = f;
		}
		else
		{
			fundsCollected = mrd.fundsEarned;
		}
		switch (comparisonOperator)
		{
		case TestComparisonOperator.LessThan:
			eventFound = fundsCollected < fundsToRecover;
			break;
		case TestComparisonOperator.LessThanorEqual:
			eventFound = fundsCollected < fundsToRecover;
			break;
		case TestComparisonOperator.Equal:
			if (Math.Abs(fundsCollected - fundsToRecover) < epsilon)
			{
				eventFound = true;
			}
			break;
		case TestComparisonOperator.GreaterThanorEqual:
			eventFound = fundsCollected >= fundsToRecover;
			break;
		case TestComparisonOperator.GreaterThan:
			eventFound = fundsCollected > fundsToRecover;
			break;
		}
	}

	public override bool Test()
	{
		return eventFound;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004016");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "fundsToRecover")
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
			return Localizer.Format("#autoLOC_8100154", field.guiName, operatorString, fundsToRecover.ToString("0"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("comparisonOperator", comparisonOperator);
		node.AddValue("fundsRecovered", fundsToRecover);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonOperator.GreaterThan);
		node.TryGetValue("fundsRecovered", ref fundsToRecover);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		return null;
	}
}
