using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class CelestialBodyScience : ProgressNode
{
	private CelestialBody body;

	private VesselRef firstVessel;

	private CrewRef firstCrew;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodyScience(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnScience(float science, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTriggeredScience(ScienceData data, Vessel origin, bool xmitAborted)
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
