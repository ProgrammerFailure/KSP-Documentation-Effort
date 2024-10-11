using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public class FailureDecoupleCantDecouple : AdjusterDecoupleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureDecoupleCantDecouple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureDecoupleCantDecouple(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsBlockingDecouple()
	{
		throw null;
	}
}
