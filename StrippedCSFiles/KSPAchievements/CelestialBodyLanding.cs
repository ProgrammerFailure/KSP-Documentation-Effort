using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class CelestialBodyLanding : ProgressNode
{
	private CelestialBody body;

	private VesselRef firstVessel;

	private CrewRef firstCrew;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodyLanding(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsValidVessel(Vessel v)
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
}
