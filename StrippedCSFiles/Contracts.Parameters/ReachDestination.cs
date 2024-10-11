using System;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class ReachDestination : ContractParameter
{
	public CelestialBody Destination;

	protected string title;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachDestination()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachDestination(CelestialBody destination, string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetTitleStringShort(CelestialBody dest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
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
	private void OnVesselSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TrackVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool checkVesselDestination(Vessel v)
	{
		throw null;
	}
}
