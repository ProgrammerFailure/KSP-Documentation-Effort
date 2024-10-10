using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_MissionTime : AwardModule
{
	[MEGUI_Time(guiName = "#autoLOC_8000188")]
	public double missionTime;

	public AwardModule_MissionTime(MENode node)
		: base(node)
	{
	}

	public AwardModule_MissionTime(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
	}

	public override bool EvaluateCondition(Mission mission)
	{
		return HighLogic.CurrentGame.UniversalTime <= missionTime;
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("missionTime", ref missionTime);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("missionTime", missionTime);
	}
}
