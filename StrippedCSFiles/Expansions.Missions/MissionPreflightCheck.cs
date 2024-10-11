using System.Runtime.CompilerServices;
using PreFlightTests;

namespace Expansions.Missions;

public class MissionPreflightCheck : IPreFlightTest
{
	private VesselRestriction vesselRestriction;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionPreflightCheck(VesselRestriction vesselRestriction)
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
