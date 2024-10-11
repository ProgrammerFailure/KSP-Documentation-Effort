using System.Runtime.CompilerServices;

public class TutorialMunFlight1 : TutorialScenario
{
	private TutorialPage welcome;

	private TutorialPage explainTMI;

	private TutorialPage mapOpen1;

	private TutorialPage mapOpen2;

	private TutorialPage maneuver1;

	private TutorialPage maneuver2;

	private TutorialPage maneuversuccess;

	private TutorialPage maneuverexecute;

	private TutorialPage executionsuccess;

	private TutorialPage transfersuccess;

	private TutorialPage transfersuccess2;

	private TutorialPage transfersuccess2b;

	private TutorialPage transfersuccess3;

	private TutorialPage conclusion;

	[KSPField(isPersistant = true)]
	public string StateName;

	private ManeuverNode mNode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialMunFlight1()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAssetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}
}
