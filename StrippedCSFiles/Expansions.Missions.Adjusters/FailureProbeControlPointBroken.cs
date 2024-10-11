using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public class FailureProbeControlPointBroken : AdjusterProbeControlPointBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureProbeControlPointBroken()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureProbeControlPointBroken(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsProbeControlPointBroken()
	{
		throw null;
	}
}
