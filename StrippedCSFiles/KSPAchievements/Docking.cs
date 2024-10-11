using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class Docking : ProgressNode
{
	private CelestialBody body;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Docking(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartsCoupled(GameEvents.FromToAction<Part, Part> pa)
	{
		throw null;
	}
}
