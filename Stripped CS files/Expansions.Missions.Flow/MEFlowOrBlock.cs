namespace Expansions.Missions.Flow;

public class MEFlowOrBlock : MEFlowBlock
{
	public Callback<MEFlowParser> OnUpdateFlowUI;

	public MEFlowOrBlock(Mission mission)
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
