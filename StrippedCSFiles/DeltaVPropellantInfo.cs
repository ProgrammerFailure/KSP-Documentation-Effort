using System;
using System.Runtime.CompilerServices;

[Serializable]
public class DeltaVPropellantInfo
{
	public Propellant propellant;

	public double amountAvailable;

	public double amountBurnt;

	public double maxBurnTime;

	public double setThrottleBurnTime;

	public double currentBurnTime;

	public double amountPerSecondMaxThrottle;

	public double amountPerSecondSetThrottle;

	public double amountPerSecondCurrentThrottle;

	public double pendingDemand;

	public float timeLeftSetThrottle;

	public float timeLeftCurrentThrottle;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVPropellantInfo(Propellant propellant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVPropellantInfo Copy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float MassBurnt()
	{
		throw null;
	}
}
