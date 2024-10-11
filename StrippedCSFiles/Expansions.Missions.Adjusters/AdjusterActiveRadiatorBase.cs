using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterActiveRadiatorBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterActiveRadiatorBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterActiveRadiatorBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsBlockingCooling()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double ApplyMaxEnergyTransferAdjustment(double maxEnergyTransfer)
	{
		throw null;
	}
}
