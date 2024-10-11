using System.Runtime.CompilerServices;
using Contracts;

namespace SentinelMission;

public class SentinelParameter : ContractParameter
{
	public CelestialBody FocusBody;

	public int TotalDiscoveries;

	public int RemainingDiscoveries;

	public SentinelScanType ScanType;

	public UntrackedObjectClass TargetSize;

	public double MinimumEccentricity;

	public double MinimumInclination;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SentinelParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SentinelParameter(CelestialBody focusBody, SentinelScanType scanType, UntrackedObjectClass targetSize, double minimumEccentricity, double minimumInclination, int discoveryCount)
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
	protected override string GetNotes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister()
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
	public void DiscoverAsteroid(UntrackedObjectClass size, double eccentricity, double inclination, CelestialBody body)
	{
		throw null;
	}
}
