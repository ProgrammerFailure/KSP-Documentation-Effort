using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestTimeSinceNode : TestModule, IScoreableObjective
{
	[MEGUI_NodeSelect(guiName = "#autoLOC_8000100", Tooltip = "#autoLOC_8000101")]
	public Guid nodeID;

	[MEGUI_Time(resetValue = "300", guiName = "#autoLOC_8000102", Tooltip = "#autoLOC_8000103")]
	public double time = 300.0;

	public double nodeCompareTime;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator = TestComparisonLessGreaterOnly.GreaterThan;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000099");
		time = 300.0;
	}

	public override void Initialize(TestGroup testGroup, string title)
	{
		base.Initialize(testGroup, string.IsNullOrEmpty(title) ? base.title : title);
		if (testGroup.node != null && testGroup.node.mission != null && testGroup.node.mission.startNode != null)
		{
			nodeID = testGroup.node.mission.startNode.id;
		}
	}

	public override bool Test()
	{
		if (!base.node.mission.nodes.ContainsKey(nodeID))
		{
			return false;
		}
		nodeCompareTime = base.node.mission.nodes[nodeID].activatedUT + time;
		if (base.node != null && base.node.mission.nodes[nodeID].HasBeenActivated)
		{
			return comparisonOperator switch
			{
				TestComparisonLessGreaterOnly.GreaterThan => Planetarium.GetUniversalTime() > nodeCompareTime, 
				TestComparisonLessGreaterOnly.LessThan => Planetarium.GetUniversalTime() < nodeCompareTime, 
				_ => false, 
			};
		}
		return false;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "nodeID")
		{
			if (base.node.mission.nodes.ContainsKey(nodeID))
			{
				string objectiveString = base.node.mission.GetNodeById(nodeID).ObjectiveString;
				string text = Localizer.Format(objectiveString);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
				return objectiveString;
			}
			return Localizer.Format("#autoLOC_8007310");
		}
		if (field.name == "time")
		{
			return ((comparisonOperator == TestComparisonLessGreaterOnly.LessThan) ? "< " : "> ") + KSPUtil.PrintDateDeltaCompact(time, includeTime: true, includeSeconds: true);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004038");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("nodeID", nodeID.ToString());
		node.AddValue("time", time);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (nodeID == Guid.Empty && base.testGroup.node != null && base.testGroup.node.mission != null && base.testGroup.node.mission.startNode != null)
		{
			nodeID = base.testGroup.node.mission.startNode.id;
		}
		node.TryGetValue("nodeID", ref nodeID);
		node.TryGetValue("time", ref time);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterOnly.LessThan);
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (!MissionEditorLogic.Instance.EditorMission.nodes.ContainsKey(nodeID))
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8007311"));
		}
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			return FlightGlobals.ActiveVessel;
		}
		return null;
	}
}
