using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class CelestialBodyReturn : ProgressNode
{
	private CelestialBody body;

	public ReturnFrom Level;

	public VesselRef firstVessel;

	public CrewRef firstCrew;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodyReturn(CelestialBody cb, ReturnFrom level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
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
