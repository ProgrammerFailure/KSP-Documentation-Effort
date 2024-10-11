using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PreFlightTests;

namespace Expansions.Missions;

public class MissionBlackListPreFlightCheck : IPreFlightTest
{
	private VesselSituation vesselSituation;

	private List<string> missionExcludedParts;

	private bool missionCheckReqd;

	private Dictionary<AvailablePart, int> blackListedPartsOnShip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionBlackListPreFlightCheck(VesselSituation situation, ShipConstruct ship)
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
