using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public class FailureEnginesDeadEngine : AdjusterEnginesBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureEnginesDeadEngine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureEnginesDeadEngine(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsEngineDead()
	{
		throw null;
	}
}
