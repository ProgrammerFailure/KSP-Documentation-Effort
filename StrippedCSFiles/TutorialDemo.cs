using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialDemo : TutorialScenario
{
	private TutorialPage defaultPage1;

	private TutorialPage defaultPage2;

	private TutorialPage defaultPage3;

	private TutorialPage specialPage1;

	private KFSMEvent onSomethingUnplanned;

	private KFSMEvent onTutorialRestart;

	private KFSMTimedEvent onStayTooLongOnPage1;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialDemo()
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
	[ContextMenu("Send Unplanned Event to FSM")]
	public void SomethingUnplanned()
	{
		throw null;
	}
}
