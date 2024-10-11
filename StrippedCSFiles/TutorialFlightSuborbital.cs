using System.Runtime.CompilerServices;

public class TutorialFlightSuborbital : TutorialScenario
{
	private TutorialPage page;

	private TutorialPage welcome;

	private TutorialPage pitchProgram1;

	private TutorialPage holdMid;

	private TutorialPage pitchProgram2;

	private TutorialPage holdSteady;

	private TutorialPage holdSteady2;

	private TutorialPage ivaBonusPage;

	private TutorialPage deathBonusPage;

	private TutorialPage dockingCtrlBonusPage;

	private TutorialPage chuteBurnBonusPage;

	private TutorialPage flipOutPage;

	private KFSMEvent onFlipOut;

	private KFSMEvent onDead;

	private KFSMEvent onFoundDockingMode;

	private KFSMEvent fsmEvent;

	private FloatCurve velocityPitch;

	private DirectionTarget pitchTarget;

	private bool willClearAtmo;

	private ModuleParachute chute;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialFlightSuborbital()
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
