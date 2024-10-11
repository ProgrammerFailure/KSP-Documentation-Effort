using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Adjusters;

public class FailureActiveRadiatorStuck : AdjusterActiveRadiatorBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureActivationState stuckState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureActiveRadiatorStuck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureActiveRadiatorStuck(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsBlockingCooling()
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
