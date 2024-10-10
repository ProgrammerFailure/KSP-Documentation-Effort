using ns9;
using UnityEngine;

public class TutorialAsteroid1 : TutorialScenario
{
	public Texture2D navBallVectors;

	public TutorialPage welcome;

	public TutorialPage rendezvous;

	public TutorialPage mapView;

	public TutorialPage attitude1;

	public TutorialPage attitude2;

	public TutorialPage precision;

	public TutorialPage attitude3;

	public TutorialPage lowerPe3;

	public TutorialPage closeToAsteroid;

	public TutorialPage conclusion;

	public TutorialPage deathBonusPage;

	public KFSMEvent fsmEvent;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
		navBallVectors = Object.FindObjectOfType<MapView>().orbitIconsMap;
	}

	public override void OnTutorialSetup()
	{
		welcome = new TutorialPage("welcome");
		welcome.windowTitle = Localizer.Format("#autoLOC_308648");
		welcome.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(ControlTypes.THROTTLE | ControlTypes.STAGING | ControlTypes.THROTTLE_CUT_MAX, "Asteroid1TutorialLock");
		};
		welcome.dialog = new MultiOptionDialog(welcome.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308656"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308657"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(welcome);
		rendezvous = new TutorialPage("rendezvous");
		rendezvous.windowTitle = Localizer.Format("#autoLOC_308667");
		rendezvous.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_smileA, 5f);
		};
		rendezvous.dialog = new MultiOptionDialog(rendezvous.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308674"), expandW: false, expandH: true));
		rendezvous.SetAdvanceCondition((KFSMState st) => MapView.MapIsEnabled);
		Tutorial.AddPage(rendezvous);
		mapView = new TutorialPage("mapView");
		mapView.windowTitle = Localizer.Format("#autoLOC_308682");
		mapView.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_lookAround, 5f);
		};
		mapView.dialog = new MultiOptionDialog(mapView.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308689"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308690"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(mapView);
		attitude1 = new TutorialPage("attitude1");
		attitude1.windowTitle = Localizer.Format("#autoLOC_308701");
		attitude1.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_nodB, 0f);
		};
		attitude1.dialog = new MultiOptionDialog(attitude1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308708"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308709"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(attitude1);
		attitude2 = new TutorialPage("attitude2");
		attitude2.windowTitle = Localizer.Format("#autoLOC_308719");
		attitude2.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
			InputLockManager.RemoveControlLock("Asteroid1TutorialLock");
		};
		attitude2.dialog = new MultiOptionDialog(attitude2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308728"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(32f, 32f), Vector2.zero, XKCDColors.BrownishOrange, navBallVectors, new Rect(0.039f, 0.29f, 0.12f, 0.12f)), new DialogGUILabel(Localizer.Format("#autoLOC_308731")), new DialogGUILayoutEnd(), new DialogGUIButton(Localizer.Format("#autoLOC_308733"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(attitude2);
		precision = new TutorialPage("precision");
		precision.windowTitle = Localizer.Format("#autoLOC_308743");
		precision.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
		};
		precision.dialog = new MultiOptionDialog(precision.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308751"), expandW: false, expandH: true));
		precision.SetAdvanceCondition(delegate
		{
			Vector2d separations = FlightGlobals.ActiveVessel.orbitTargeter.GetSeparations();
			return ((separations.x <= 100.0 || separations.y <= 100.0) && FlightGlobals.ActiveVessel.orbit.ApA > 3200000.0) ? true : false;
		});
		Tutorial.AddPage(precision);
		attitude3 = new TutorialPage("attitude3");
		attitude3.windowTitle = Localizer.Format("#autoLOC_308769");
		attitude3.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbsUp, 0f);
		};
		attitude3.dialog = new MultiOptionDialog(attitude3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308776", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308777"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(attitude3);
		lowerPe3 = new TutorialPage("lowerPe3");
		lowerPe3.windowTitle = Localizer.Format("#autoLOC_308787");
		lowerPe3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		lowerPe3.dialog = new MultiOptionDialog(lowerPe3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308794", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308795"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => FlightGlobals.ship_tgtSpeed < 1.0, dismissOnSelect: true));
		Tutorial.AddPage(lowerPe3);
		closeToAsteroid = new TutorialPage("closeToAsteroid");
		closeToAsteroid.windowTitle = Localizer.Format("#autoLOC_308811");
		closeToAsteroid.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_smileB);
		};
		closeToAsteroid.dialog = new MultiOptionDialog(closeToAsteroid.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308819", tutorialControlColorString, GameSettings.TRANSLATE_FWD.name, tutorialControlColorString, GameSettings.TRANSLATE_BACK.name, tutorialControlColorString, GameSettings.TRANSLATE_UP.name, tutorialControlColorString, GameSettings.TRANSLATE_DOWN.name, tutorialControlColorString, GameSettings.TRANSLATE_RIGHT.name, tutorialControlColorString, GameSettings.TRANSLATE_LEFT.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308820"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			Vessel vessel = ((FlightGlobals.fetch.VesselTarget != null) ? FlightGlobals.fetch.VesselTarget.GetVessel() : null);
			return !(vessel == null) && Vector3.Distance(FlightGlobals.ActiveVessel.transform.position, vessel.transform.position) < 10000f;
		}, dismissOnSelect: true));
		Tutorial.AddPage(closeToAsteroid);
		conclusion = new TutorialPage("conclusion");
		conclusion.windowTitle = Localizer.Format("#autoLOC_308840");
		conclusion.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
		};
		conclusion.dialog = new MultiOptionDialog(conclusion.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308848"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308849"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(conclusion);
		deathBonusPage = new TutorialPage("deathBonusPage");
		deathBonusPage.windowTitle = Localizer.Format("#autoLOC_308860");
		deathBonusPage.OnEnter = delegate
		{
			deathBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_false_disappointed);
		};
		deathBonusPage.dialog = new MultiOptionDialog(deathBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308868"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308869"), delegate
		{
			CloseTutorialWindow();
		}, dismissOnSelect: true));
		deathBonusPage.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.state != Vessel.State.DEAD);
		Tutorial.AddState(deathBonusPage);
		fsmEvent = new KFSMEvent(Localizer.Format("#autoLOC_308884"));
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
