using System.Runtime.CompilerServices;
using Contracts;

namespace FinePrint.Contracts.Parameters;

public class StationaryPointParameter : ContractParameter
{
	private Waypoint wp;

	private CelestialBody targetBody;

	private bool submittedWaypoint;

	private double longitude;

	private double deviation;

	private int successCounter;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StationaryPointParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StationaryPointParameter(CelestialBody targetBody, double longitude, double deviation)
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
	protected override string GetMessageComplete()
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
	protected override void OnUpdate()
	{
		throw null;
	}
}
