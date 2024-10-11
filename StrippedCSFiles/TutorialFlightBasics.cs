using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialFlightBasics : TutorialScenario
{
	private Texture2D YPRDiagram;

	private Texture2D chuteDiagram;

	private TutorialPage welcome;

	private TutorialPage basicControls;

	private TutorialPage pitch;

	private TutorialPage yaw;

	private TutorialPage roll;

	private TutorialPage PYRsummary;

	private TutorialPage throttle;

	private TutorialPage basicControlsSummary;

	private TutorialPage SAS;

	private TutorialPage SAS2;

	private TutorialPage RCS;

	private TutorialPage staging;

	private TutorialPage navball;

	private TutorialPage altimeter;

	private TutorialPage chuteColors;

	private TutorialPage readyToLaunch;

	private TutorialPage ivaBonusPage;

	private TutorialPage deathBonusPage;

	private TutorialPage dockingCtrlBonusPage;

	private TutorialPage shipLaunched;

	private TutorialPage readyChute;

	private TutorialPage chuteBurnBonusPage;

	private TutorialPage coastDown;

	private TutorialPage waitToSlow;

	private TutorialPage landed;

	private KFSMEvent fsmEvent;

	private bool deadPage;

	private ModuleEngines srb;

	private ModuleParachute chute;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialFlightBasics()
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
}
