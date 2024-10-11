using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class CelestialBodySplashdown : ProgressNode
{
	private CelestialBody body;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodySplashdown(CelestialBody cb)
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
}
