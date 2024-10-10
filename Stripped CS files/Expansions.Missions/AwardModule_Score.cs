namespace Expansions.Missions;

public class AwardModule_Score : AwardModule
{
	public override bool canBeDisplayedInEditor => false;

	public AwardModule_Score(MENode node)
		: base(node)
	{
		enabled = true;
	}

	public AwardModule_Score(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
		enabled = true;
	}

	public override bool EvaluateCondition(Mission mission)
	{
		if (score > 0f)
		{
			return mission.currentScore >= score;
		}
		return false;
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
	}
}
