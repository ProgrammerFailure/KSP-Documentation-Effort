using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Adjusters;

public class FailureDeployablePartStuck : AdjusterDeployablePartBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureDeploymentState stuckState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureDeployablePartStuck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureDeployablePartStuck(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsDeployablePartStuck()
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
