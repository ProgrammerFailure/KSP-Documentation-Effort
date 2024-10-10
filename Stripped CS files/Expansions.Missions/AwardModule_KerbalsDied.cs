namespace Expansions.Missions;

public class AwardModule_KerbalsDied : AwardModule
{
	public AwardModule_KerbalsDied(MENode node)
		: base(node)
	{
	}

	public AwardModule_KerbalsDied(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
	}

	public override bool EvaluateCondition(Mission mission)
	{
		KerbalRoster crewRoster = HighLogic.CurrentGame.CrewRoster;
		int count = crewRoster.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (crewRoster[count].rosterStatus != ProtoCrewMember.RosterStatus.Assigned);
		return false;
	}
}
