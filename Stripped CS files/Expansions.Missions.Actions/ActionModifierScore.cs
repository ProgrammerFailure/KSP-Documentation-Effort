namespace Expansions.Missions.Actions;

public class ActionModifierScore : ActionMissionScore
{
	public override void Initialize(MENode node)
	{
		base.node = node;
		score = new MissionScore(node);
		if (score.activeModules.Count == 0)
		{
			score.activeModules.Add(new ScoreModule_Modifier(node));
		}
	}
}
