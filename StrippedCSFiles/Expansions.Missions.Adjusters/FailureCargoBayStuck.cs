using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Adjusters;

public class FailureCargoBayStuck : AdjusterCargoBayBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureOpenState stuckState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureCargoBayStuck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureCargoBayStuck(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Deactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsCargoBayStuck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}
}
