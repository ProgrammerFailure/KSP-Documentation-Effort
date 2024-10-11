using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class ReachSpace : ProgressNode
{
	public VesselRef firstVessel;

	public CrewRef firstCrew;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachSpace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
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
