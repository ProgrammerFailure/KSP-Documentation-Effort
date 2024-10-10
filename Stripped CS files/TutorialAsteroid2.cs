using ns9;
using UnityEngine;

public class TutorialAsteroid2 : TutorialScenario
{
	public TutorialPage welcome;

	public TutorialPage grappleDeploy;

	public TutorialPage BeginApproach;

	public TutorialPage asteroidCatched;

	public TutorialPage alignment;

	public TutorialPage attitude3;

	public TutorialPage lowerPe3;

	public TutorialPage closeToAsteroid;

	public TutorialPage conclusion;

	public TutorialPage deathBonusPage;

	public KFSMEvent fsmEvent;

	public ModuleGrappleNode grapple;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
	}

	public override void OnTutorialSetup()
	{
		welcome = new TutorialPage("welcome");
		welcome.windowTitle = Localizer.Format("#autoLOC_308926");
		welcome.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			Part part = FlightGlobals.ActiveVessel.Parts.Find((Part p) => p.partInfo.name == "GrapplingDevice");
			if (part != null)
			{
				grapple = part.Modules.GetModule<ModuleGrappleNode>();
			}
		};
		welcome.dialog = new MultiOptionDialog(welcome.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308937", tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308938"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => (grapple != null && grapple.state != "Disabled") ? true : false, dismissOnSelect: true));
		Tutorial.AddPage(welcome);
		grappleDeploy = new TutorialPage("grappleDeploy");
		grappleDeploy.windowTitle = Localizer.Format("#autoLOC_308955");
		grappleDeploy.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_nodB, 5f);
		};
		grappleDeploy.dialog = new MultiOptionDialog(grappleDeploy.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308962"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308963"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(grappleDeploy);
		BeginApproach = new TutorialPage("BeginApproach");
		BeginApproach.windowTitle = Localizer.Format("#autoLOC_308974");
		BeginApproach.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_lookAround, 5f);
		};
		BeginApproach.dialog = new MultiOptionDialog(BeginApproach.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308981"), expandW: false, expandH: true));
		BeginApproach.SetAdvanceCondition((KFSMState st) => (Object.FindObjectOfType<ModuleAsteroid>().GetComponent<Vessel>() == null) ? true : false);
		Tutorial.AddPage(BeginApproach);
		asteroidCatched = new TutorialPage("asteroidCatched");
		asteroidCatched.windowTitle = Localizer.Format("#autoLOC_308995");
		asteroidCatched.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbUp, 0f);
		};
		asteroidCatched.dialog = new MultiOptionDialog(asteroidCatched.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309002", tutorialControlColorString, tutorialHighlightColorString, tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309003"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(asteroidCatched);
		alignment = new TutorialPage("alignment");
		alignment.windowTitle = Localizer.Format("#autoLOC_309013");
		alignment.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
			FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.flag_6] = false;
			FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.flag_5] = false;
		};
		alignment.dialog = new MultiOptionDialog(alignment.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309024", tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309025"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(alignment);
		attitude3 = new TutorialPage("attitude3");
		attitude3.windowTitle = Localizer.Format("#autoLOC_309035");
		attitude3.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbsUp, 0f);
		};
		attitude3.dialog = new MultiOptionDialog(attitude3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309042"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309043"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(attitude3);
		lowerPe3 = new TutorialPage("lowerPe3");
		lowerPe3.windowTitle = Localizer.Format("#autoLOC_309053");
		lowerPe3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		lowerPe3.dialog = new MultiOptionDialog(lowerPe3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309060"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309061"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		lowerPe3.SetAdvanceCondition((KFSMState st) => (Object.FindObjectOfType<ManeuverGizmo>() != null) ? true : false);
		Tutorial.AddPage(lowerPe3);
		closeToAsteroid = new TutorialPage("closeToAsteroid");
		closeToAsteroid.windowTitle = Localizer.Format("#autoLOC_309076");
		closeToAsteroid.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_smileB);
		};
		closeToAsteroid.dialog = new MultiOptionDialog(closeToAsteroid.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309084"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309085"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			bool flag = false;
			if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
			{
				int count = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count;
				while (count-- > 0)
				{
					ManeuverNode maneuverNode = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[count];
					if (!(maneuverNode.double_0 < Planetarium.GetUniversalTime()) && ((maneuverNode.patch != null && maneuverNode.patch.referenceBody != null && maneuverNode.patch.referenceBody.isHomeWorld) || (maneuverNode.nextPatch != null && maneuverNode.nextPatch.referenceBody != null && maneuverNode.nextPatch.referenceBody.isHomeWorld) || (maneuverNode.nextPatch.nextPatch != null && maneuverNode.nextPatch.nextPatch.referenceBody != null && maneuverNode.nextPatch.nextPatch.referenceBody.isHomeWorld)))
					{
						flag = true;
						break;
					}
				}
			}
			if (FlightGlobals.ActiveVessel.orbit.referenceBody.isHomeWorld)
			{
				Tutorial.GoToNextPage();
			}
			else if (flag)
			{
				return true;
			}
			return false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(closeToAsteroid);
		conclusion = new TutorialPage("conclusion");
		conclusion.windowTitle = Localizer.Format("#autoLOC_309137");
		conclusion.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
			InputLockManager.RemoveControlLock("Asteroid1TutorialLock");
		};
		conclusion.dialog = new MultiOptionDialog(conclusion.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309146"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309147"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(conclusion);
		deathBonusPage = new TutorialPage("deathBonusPage");
		deathBonusPage.windowTitle = Localizer.Format("#autoLOC_309158");
		deathBonusPage.OnEnter = delegate
		{
			deathBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_false_disappointed);
		};
		deathBonusPage.dialog = new MultiOptionDialog(deathBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309166"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309167"), delegate
		{
			CloseTutorialWindow();
		}, dismissOnSelect: true));
		deathBonusPage.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.state != Vessel.State.DEAD);
		Tutorial.AddState(deathBonusPage);
		fsmEvent = new KFSMEvent("Died");
		fsmEvent.GoToStateOnEvent = deathBonusPage;
		fsmEvent.OnCheckCondition = (KFSMState st) => FlightGlobals.ActiveVessel.state == Vessel.State.DEAD;
		Tutorial.AddEventExcluding(fsmEvent, deathBonusPage);
		Tutorial.StartTutorial(welcome);
	}

	public override void OnOnDestroy()
	{
		InputLockManager.RemoveControlLock("Asteroid1TutorialLock");
	}
}
