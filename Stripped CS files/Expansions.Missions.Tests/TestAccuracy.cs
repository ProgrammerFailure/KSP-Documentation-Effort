using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

public class TestAccuracy : TestModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.Percentage, resetValue = "0", guiName = "#autoLOC_8001010", Tooltip = "#autoLOC_8001011")]
	public double accuracy;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator = TestComparisonLessGreaterOnly.GreaterThan;

	public IScoreableObjective scoreableObjective;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8001008");
		accuracy = 0.0;
	}

	public override bool Test()
	{
		if (scoreableObjective == null && base.node.dockParentNode != null && base.node.dockParentNode.HasTestModules)
		{
			scoreableObjective = base.node.dockParentNode.testGroups[0].testModules[0] as IScoreableObjective;
		}
		if (scoreableObjective != null)
		{
			object scoreModifier = scoreableObjective.GetScoreModifier(typeof(ScoreModule_Accuracy));
			double num = ((scoreModifier != null) ? ((double)scoreModifier * 100.0) : 100.0);
			Debug.LogFormat("[TestAccuracy]: Node:{0} ({1}) Accuracy test must be {2} than {3:0.00}%. Accuracy was {4:0.00}%", base.node.Title, base.node.id, (comparisonOperator == TestComparisonLessGreaterOnly.LessThan) ? "<" : ">", accuracy, num);
			return comparisonOperator switch
			{
				TestComparisonLessGreaterOnly.GreaterThan => num > accuracy, 
				TestComparisonLessGreaterOnly.LessThan => num < accuracy, 
				_ => false, 
			};
		}
		Debug.LogFormat("[TestAccuracy]: {0} ({1}) was unable to find valid scoreable test on docked parent node {2} ({3})", base.node.Title, base.node.id, (base.node.dockParentNode != null) ? base.node.dockParentNode.Title : "Not Docked to any node", (base.node.dockParentNode != null) ? base.node.dockParentNode.id.ToString() : "N/A");
		return false;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "accuracy")
		{
			return Localizer.Format("#autoLOC_8001012", (comparisonOperator == TestComparisonLessGreaterOnly.LessThan) ? "<" : ">", accuracy.ToString());
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8001009");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("accuracy", accuracy);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("accuracy", ref accuracy);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterOnly.GreaterThan);
	}
}
