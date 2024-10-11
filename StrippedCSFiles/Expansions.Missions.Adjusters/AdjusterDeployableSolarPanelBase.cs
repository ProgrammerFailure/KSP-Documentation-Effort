using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterDeployableSolarPanelBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterDeployableSolarPanelBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterDeployableSolarPanelBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float ApplyEfficiencyAdjustment(float efficiency)
	{
		throw null;
	}
}
