using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class CelestialBodyFlight : ProgressNode
{
	public CelestialBody body;

	public VesselRef firstVessel;

	public CrewRef firstCrew;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodyFlight(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselSitChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
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
}
