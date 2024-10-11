using System.Runtime.CompilerServices;

public class TutorialAscent : TutorialScenario
{
	private TutorialPage welcome;

	private TutorialPage ivaBonusPage;

	private TutorialPage deathBonusPage;

	private TutorialPage dockingCtrlBonusPage;

	private TutorialPage failBonusPage;

	private TutorialPage orbitPage;

	private KFSMEvent onFoundIVAMode;

	private KFSMEvent onVesselDied;

	private KFSMEvent onFoundDockingMode;

	private KFSMEvent onOffCourse;

	private KFSMEvent onOrbit;

	private FloatCurve velocityPitch;

	private FloatCurve velocityAlt;

	private bool checkAlt;

	private bool offCourse;

	private bool careOffCourse;

	private double srfVelMax;

	private double altDesired;

	private double pitchDesired;

	private double margin;

	private double nodeDvMag;

	private DirectionTarget pitchTarget;

	private ManeuverNode mNode;

	private Part solidBooster;

	private ModuleEngines engine;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialAscent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAssetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDirectionTarget()
	{
		throw null;
	}
}
