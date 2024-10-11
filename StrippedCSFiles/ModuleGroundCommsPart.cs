using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleGroundCommsPart : ModuleGroundSciencePart, ICommAntenna
{
	[KSPField]
	public bool antennaCombinable;

	[KSPField]
	public double antennaCombinableExponent;

	[KSPField]
	public AntennaType antennaType;

	[KSPField]
	public DoubleCurve rangeCurve;

	[KSPField]
	public DoubleCurve scienceCurve;

	[KSPField]
	public double antennaPower;

	private ModuleGroundExpControl controllerModule;

	private Part loadedPart;

	protected List<string> tempAppliedUpgrades;

	public bool CommCombinable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double CommCombinableExponent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public AntennaType CommType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DoubleCurve CommRangeCurve
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DoubleCurve CommScienceCurve
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual double CommPower
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleGroundCommsPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanScienceTo(bool combined, double bPower, double sqrDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanComm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanCommUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double CommPowerUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
