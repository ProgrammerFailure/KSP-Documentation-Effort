using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class CelestialBodyEscape : ProgressNode
{
	private CelestialBody body;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodyEscape(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSOIEscaped(GameEvents.HostedFromToAction<Vessel, CelestialBody> vcs)
	{
		throw null;
	}
}
