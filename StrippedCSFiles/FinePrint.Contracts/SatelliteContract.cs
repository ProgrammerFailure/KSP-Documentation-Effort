using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using FinePrint.Utilities;

namespace FinePrint.Contracts;

public class SatelliteContract : Contract, IUpdateWaypoints, ICheckSpecificVessels
{
	private enum SatellitePlacementMode
	{
		NEW,
		ADJUST,
		NETWORK,
		NETWORKADJUST
	}

	private CelestialBody targetBody;

	private double deviation;

	private OrbitType orbitType;

	private KSPRandom generator;

	private double altitudeFactor;

	private double inclinationFactor;

	private SatellitePlacementMode placementMode;

	private static List<string> objectiveTypes;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SatelliteContract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool Generate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeDeclined()
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
	protected override string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetSynopsys()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string MessageCompleted()
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
	public override bool MeetRequirements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override List<CelestialBody> GetWeightBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAccepted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool AreSatellitesUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool ProbeCoresUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void pickEasy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void pickMedium()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void pickHard()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setOrbitType(OrbitType targetType, double altitudeFactor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PrestigeAppropriateOrbitType(OrbitType ot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int NetworkSatelliteCount(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckSatelliteNetwork(int networkCount, List<Vessel> possibleSatellites, OrbitType orbitType, ref Orbit orbit, ref double stationaryLongitude, ref Vessel adjustVessel, Vessel startVessel = null)
	{
		throw null;
	}
}
