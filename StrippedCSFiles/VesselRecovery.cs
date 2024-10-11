using System.Runtime.CompilerServices;
using KSP.UI.Screens;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.EDITOR
})]
public class VesselRecovery : ScenarioModule
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRecovery()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselRecovered(ProtoVessel pv, bool quick)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool checkLaunchSites(ProtoVessel pv, out float recoveryFactor, out float vanillaFactor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void recoverVesselCrew(ProtoVessel pv, MissionRecoveryDialog mrDialog)
	{
		throw null;
	}
}
