using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PreFlightTests;

public class ExperimentalPartsAvailable : IPreFlightTest
{
	private Dictionary<AvailablePart, int> expPartsOnShip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperimentalPartsAvailable(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperimentalPartsAvailable(VesselCrewManifest manifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetWarningTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetWarningDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetProceedOption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetAbortOption()
	{
		throw null;
	}
}
