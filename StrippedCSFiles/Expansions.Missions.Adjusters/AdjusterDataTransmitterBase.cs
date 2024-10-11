using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterDataTransmitterBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterDataTransmitterBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterDataTransmitterBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double ApplyPowerAdjustment(double power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsDataTransmitterBroken()
	{
		throw null;
	}
}
