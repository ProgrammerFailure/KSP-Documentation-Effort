using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class CelestialBodyFlyby : ProgressNode
{
	private CelestialBody body;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodyFlyby(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> vcb)
	{
		throw null;
	}
}
