using System.Runtime.CompilerServices;
using Contracts;

namespace FinePrint.Contracts.Parameters;

public class VesselDestinationParameter : ContractParameter
{
	public CelestialBody targetBody;

	public FlightLog.EntryType targetType;

	private bool eventsAdded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselDestinationParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselDestinationParameter(CelestialBody targetBody, FlightLog.EntryType targetType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetHashString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUnregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckFlightLog(Vessel v)
	{
		throw null;
	}
}
