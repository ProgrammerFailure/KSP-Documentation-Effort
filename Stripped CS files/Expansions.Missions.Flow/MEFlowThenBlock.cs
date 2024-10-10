namespace Expansions.Missions.Flow;

public class MEFlowThenBlock : MEFlowBlock
{
	public Callback<MEFlowParser> OnUpdateFlowUI;

	public MEFlowThenBlock(Mission mission)
		: base(mission)
	{
	}

	public new void UpdateMissionFlowUI(MEFlowParser parser)
	{
		if (OnUpdateFlowUI != null)
		{
			OnUpdateFlowUI(parser);
		}
	}
}
