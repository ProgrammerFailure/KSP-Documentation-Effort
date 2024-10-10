using UnityEngine;

public class TutorialDemo : TutorialScenario
{
	public TutorialPage defaultPage1;

	public TutorialPage defaultPage2;

	public TutorialPage defaultPage3;

	public TutorialPage specialPage1;

	public KFSMEvent onSomethingUnplanned;

	public KFSMEvent onTutorialRestart;

	public KFSMTimedEvent onStayTooLongOnPage1;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
	}

	public override void OnTutorialSetup()
	{
		defaultPage1 = new TutorialPage("default page 1");
		defaultPage1.windowTitle = "Tutorial Window";
		defaultPage1.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		defaultPage1.OnDrawContent = delegate
		{
			GUILayout.Label("This is a demo tutorial to test out the tutorial scenario features. Press Next to go to the next page, or wait " + (10.0 - Tutorial.TimeAtCurrentState).ToString("0") + " seconds.", GUILayout.ExpandHeight(expand: true));
			if (GUILayout.Button("Next"))
			{
				Tutorial.GoToNextPage();
			}
		};
		Tutorial.AddPage(defaultPage1);
		defaultPage2 = new TutorialPage("default page 2");
		defaultPage2.windowTitle = "Tutorial Window (continued)";
		defaultPage2.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_lookAround, 5f);
		};
		defaultPage2.OnDrawContent = delegate
		{
			GUILayout.Label("This second page is only here to test the state progression system. Tutorial pages can be stepped forward, and also stepped back.", GUILayout.ExpandHeight(expand: true));
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Back"))
			{
				Tutorial.GoToLastPage();
			}
			if (GUILayout.Button("Next"))
			{
				Tutorial.GoToNextPage();
			}
			GUILayout.EndHorizontal();
		};
		Tutorial.AddPage(defaultPage2);
		defaultPage3 = new TutorialPage("default page 3");
		defaultPage3.windowTitle = "Tutorial Window (last one)";
		defaultPage3.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_nodA, 5f);
		};
		defaultPage3.OnDrawContent = delegate
		{
			GUILayout.Label("This third page is also only here to test the state progression system. It's very much like the previous one, but it has a button to restart the tutorial.", GUILayout.ExpandHeight(expand: true));
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Back"))
			{
				Tutorial.GoToLastPage();
			}
			if (GUILayout.Button("Restart"))
			{
				Tutorial.RunEvent(onTutorialRestart);
			}
			GUILayout.EndHorizontal();
		};
		Tutorial.AddPage(defaultPage3);
		specialPage1 = new TutorialPage("special page 1");
		specialPage1.OnEnter = delegate(KFSMState lastSt)
		{
			specialPage1.windowTitle = "Tutorial Window (from " + lastSt.name + ")";
			specialPage1.onAdvanceConditionMet.GoToStateOnEvent = lastSt;
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
		};
		specialPage1.OnDrawContent = delegate
		{
			GUILayout.Label("This Page shows that it's possible to use external events to send the tutorial to any arbitrary state, even ones not in the default sequence. Use this to handle cases where the player strays off the plan.\n\nNote that this page is added with AddState instead of AddPage, because we don't want this page to be part of the normal tutorial sequence.", GUILayout.ExpandHeight(expand: true));
			if (GUILayout.Button("Yep"))
			{
				Tutorial.RunEvent(specialPage1.onAdvanceConditionMet);
			}
		};
		specialPage1.OnLeave = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_sigh);
		};
		Tutorial.AddState(specialPage1);
		onTutorialRestart = new KFSMEvent("Tutorial Restarted");
		onTutorialRestart.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		onTutorialRestart.GoToStateOnEvent = defaultPage1;
		Tutorial.AddEvent(onTutorialRestart, defaultPage3);
		onSomethingUnplanned = new KFSMEvent("Something Unplanned");
		onSomethingUnplanned.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		onSomethingUnplanned.GoToStateOnEvent = specialPage1;
		Tutorial.AddEventExcluding(onSomethingUnplanned, specialPage1);
		onStayTooLongOnPage1 = new KFSMTimedEvent("Too Long at Page 1", 10.0);
		onStayTooLongOnPage1.GoToStateOnEvent = specialPage1;
		Tutorial.AddEvent(onStayTooLongOnPage1, defaultPage1);
		Tutorial.StartTutorial(defaultPage1);
	}

	[ContextMenu("Send Unplanned Event to FSM")]
	public void SomethingUnplanned()
	{
		if (Tutorial.Started)
		{
			Tutorial.RunEvent(onSomethingUnplanned);
		}
	}
}
