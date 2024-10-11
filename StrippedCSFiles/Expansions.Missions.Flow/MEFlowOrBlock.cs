using System.Runtime.CompilerServices;

namespace Expansions.Missions.Flow;

public class MEFlowOrBlock : MEFlowBlock
{
	internal Callback<MEFlowParser> OnUpdateFlowUI;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEFlowOrBlock(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal new void UpdateMissionFlowUI(MEFlowParser parser)
	{
		throw null;
	}
}
