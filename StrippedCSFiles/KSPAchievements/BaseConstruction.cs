using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class BaseConstruction : ProgressNode
{
	private CelestialBody body;

	private VesselRef firstBase;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseConstruction(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartsCoupled(GameEvents.FromToAction<Part, Part> pa)
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
