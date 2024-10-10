using System.Collections.Generic;
using ns9;
using UnityEngine;

public class TutorialDocking : TutorialScenario
{
	public string stateName = "welcome";

	public bool complete;

	public Texture2D navBallVectors;

	public Texture2D maneuverVectors;

	public TutorialPage welcome;

	public TutorialPage grappleDeploy;

	public TutorialPage asteroidCatched;

	public TutorialPage alignment;

	public TutorialPage attitude3;

	public TutorialPage lowerPe3;

	public TutorialPage closeToAsteroid;

	public TutorialPage conclusion;

	public TutorialPage conclusion2;

	public TutorialPage conclusion3;

	public TutorialPage conclusion4;

	public TutorialPage precision;

	public TutorialPage beginApproach;

	public TutorialPage deathBonusPage;

	public KFSMEvent fsmEvent;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
		navBallVectors = Object.FindObjectOfType<MapView>().orbitIconsMap;
		maneuverVectors = AssetBase.GetTexture("ManeuverNode_vectors");
	}

	public override void OnTutorialSetup()
	{
		welcome = new TutorialPage("welcome");
		welcome.windowTitle = Localizer.Format("#autoLOC_309362");
		welcome.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		welcome.dialog = new MultiOptionDialog(welcome.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309369"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309370"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(welcome);
		grappleDeploy = new TutorialPage("grappleDeploy");
		grappleDeploy.windowTitle = Localizer.Format("#autoLOC_309381");
		grappleDeploy.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_nodB, 5f);
		};
		grappleDeploy.dialog = new MultiOptionDialog(grappleDeploy.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309388", tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name), expandW: false, expandH: true));
		grappleDeploy.SetAdvanceCondition((KFSMState KFSMEvent) => MapView.MapIsEnabled);
		Tutorial.AddPage(grappleDeploy);
		beginApproach = new TutorialPage("BeginApproach");
		beginApproach.windowTitle = Localizer.Format("#autoLOC_309397");
		beginApproach.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_lookAround, 5f);
		};
		beginApproach.dialog = new MultiOptionDialog(beginApproach.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309404", tutorialControlColorString), expandW: false, expandH: true));
		beginApproach.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.fetch.VesselTarget != null);
		Tutorial.AddPage(beginApproach);
		asteroidCatched = new TutorialPage("asteroidCatched");
		asteroidCatched.windowTitle = Localizer.Format("#autoLOC_309412");
		asteroidCatched.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbUp, 0f);
		};
		asteroidCatched.dialog = new MultiOptionDialog(asteroidCatched.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309419"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(48f, 48f), Vector2.zero, XKCDColors.ElectricLime, navBallVectors, new Rect(0.45f, 0.85f, 0.3f, 0.2f)), new DialogGUILabel(Localizer.Format("#autoLOC_309422")), new DialogGUILayoutEnd(), new DialogGUIButton(Localizer.Format("#autoLOC_309424"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(asteroidCatched);
		alignment = new TutorialPage("alignment");
		alignment.windowTitle = Localizer.Format("#autoLOC_309435");
		alignment.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
		};
		alignment.dialog = new MultiOptionDialog(alignment.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309444"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309445"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			if (!(FlightGlobals.fetch == null) && FlightGlobals.fetch.VesselTarget != null)
			{
				float num = (float)FlightGlobals.fetch.VesselTarget.GetOrbit().inclination * 0.99f;
				float num2 = (float)FlightGlobals.fetch.VesselTarget.GetOrbit().inclination * 1.01f;
				if (FlightGlobals.ActiveVessel.orbit.inclination > (double)num && FlightGlobals.ActiveVessel.orbit.inclination < (double)num2)
				{
					return true;
				}
				return false;
			}
			return false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(alignment);
		attitude3 = new TutorialPage("attitude3");
		attitude3.windowTitle = Localizer.Format("#autoLOC_309463");
		attitude3.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbsUp, 0f);
		};
		attitude3.dialog = new MultiOptionDialog(attitude3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309470"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(32f, 32f), Vector2.zero, XKCDColors.BrownishOrange, navBallVectors, new Rect(0.039f, 0.29f, 0.12f, 0.12f)), new DialogGUILabel(Localizer.Format("#autoLOC_309473"), expandW: false, expandH: true), new DialogGUILayoutEnd(), new DialogGUIButton(Localizer.Format("#autoLOC_309475"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => !(FlightGlobals.ActiveVessel.orbitTargeter == null) && !(FlightGlobals.ActiveVessel.orbitTargeter.GetSeparations() == Vector2d.zero) && ((FlightGlobals.ActiveVessel.orbitTargeter.GetSeparations().x <= 5.0 || !(FlightGlobals.ActiveVessel.orbitTargeter.GetSeparations().y > 5.0)) ? true : false), dismissOnSelect: true));
		Tutorial.AddPage(attitude3);
		lowerPe3 = new TutorialPage("lowerPe3");
		lowerPe3.windowTitle = Localizer.Format("#autoLOC_309493");
		lowerPe3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		lowerPe3.dialog = new MultiOptionDialog(lowerPe3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309500"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309501"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			Vessel activeVessel2 = FlightGlobals.ActiveVessel;
			List<ManeuverNode> list = new List<ManeuverNode>(activeVessel2.patchedConicSolver.maneuverNodes);
			return list.Count == 0 || list[0].GetBurnVector(activeVessel2.orbit).sqrMagnitude <= 1.0;
		}, dismissOnSelect: true));
		Tutorial.AddPage(lowerPe3);
		precision = new TutorialPage("lowerPe3");
		precision.windowTitle = Localizer.Format("#autoLOC_309521");
		precision.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_nodA);
		};
		precision.dialog = new MultiOptionDialog(precision.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309528"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309529"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => (FlightGlobals.GetDisplaySpeed() < 50.0 && FlightGlobals.speedDisplayMode == FlightGlobals.SpeedDisplayModes.Target) ? true : false, dismissOnSelect: true));
		Tutorial.AddPage(precision);
		closeToAsteroid = new TutorialPage("closeToAsteroid");
		closeToAsteroid.windowTitle = Localizer.Format("#autoLOC_309545");
		closeToAsteroid.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_smileB);
		};
		closeToAsteroid.dialog = new MultiOptionDialog(closeToAsteroid.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309553", tutorialControlColorString, GameSettings.RCS_TOGGLE.name, tutorialControlColorString, GameSettings.TRANSLATE_FWD.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309554"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => (FlightGlobals.GetDisplaySpeed() < 0.5 && FlightGlobals.speedDisplayMode == FlightGlobals.SpeedDisplayModes.Target) ? true : false, dismissOnSelect: true));
		Tutorial.AddPage(closeToAsteroid);
		beginApproach = new TutorialPage("beginApproach");
		beginApproach.windowTitle = Localizer.Format("#autoLOC_309570");
		beginApproach.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 5f);
		};
		beginApproach.dialog = new MultiOptionDialog(beginApproach.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309578"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(32f, 32f), Vector2.zero, XKCDColors.PastelPink, maneuverVectors, new Rect(0.666f, 0.666f, 0.333f, 0.333f)), new DialogGUILayoutEnd(), new DialogGUILabel(Localizer.Format("#autoLOC_309582")));
		beginApproach.SetAdvanceCondition((KFSMState KFSMEvent) => Vector3.Distance(FlightGlobals.fetch.VesselTarget.GetVessel().transform.position, FlightGlobals.ActiveVessel.transform.position) < 100f);
		Tutorial.AddPage(beginApproach);
		conclusion = new TutorialPage("conclusion");
		conclusion.windowTitle = Localizer.Format("#autoLOC_309596");
		conclusion.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
		};
		conclusion.dialog = new MultiOptionDialog(conclusion.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309605", tutorialControlColorString, tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309606"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(conclusion);
		conclusion2 = new TutorialPage("conclusion2");
		conclusion2.windowTitle = Localizer.Format("#autoLOC_309617");
		conclusion2.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
		};
		conclusion2.dialog = new MultiOptionDialog(conclusion2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309626", tutorialControlColorString, GameSettings.CAMERA_NEXT.name, tutorialControlColorString, GameSettings.PITCH_DOWN.name, tutorialControlColorString, GameSettings.YAW_LEFT.name, tutorialControlColorString, GameSettings.PITCH_UP.name, tutorialControlColorString, GameSettings.YAW_RIGHT.name, tutorialControlColorString, GameSettings.THROTTLE_UP.name, tutorialControlColorString, GameSettings.THROTTLE_DOWN.name, tutorialControlColorString, GameSettings.TRANSLATE_DOWN.name, tutorialControlColorString, GameSettings.TRANSLATE_UP.name, tutorialControlColorString, GameSettings.TRANSLATE_LEFT.name, tutorialControlColorString, GameSettings.TRANSLATE_RIGHT.name, tutorialControlColorString, GameSettings.TRANSLATE_FWD.name, tutorialControlColorString, GameSettings.TRANSLATE_BACK.name, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309627"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(conclusion2);
		conclusion3 = new TutorialPage("conclusion3");
		conclusion3.windowTitle = Localizer.Format("#autoLOC_309638");
		conclusion3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_wonder);
			InputLockManager.SetControlLock(ControlTypes.QUICKSAVE, "docking");
		};
		conclusion3.dialog = new MultiOptionDialog(conclusion3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309647", tutorialControlColorString, GameSettings.CAMERA_NEXT.name, tutorialControlColorString, GameSettings.CAMERA_MODE.name, tutorialControlColorString, GameSettings.QUICKSAVE.name, tutorialControlColorString, GameSettings.QUICKLOAD.name), expandW: false, expandH: true));
		conclusion3.SetAdvanceCondition(delegate
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			bool flag = false;
			bool flag2 = false;
			int count = activeVessel.Parts.Count;
			while (count-- > 0)
			{
				Part part = activeVessel.Parts[count];
				if (part.partInfo.name == "Size3to2Adapter")
				{
					flag = true;
				}
				else if (part.partInfo.name == "rcsTankRadialLong")
				{
					flag2 = true;
				}
				if (flag && flag2)
				{
					break;
				}
			}
			return flag && flag2;
		});
		Tutorial.AddPage(conclusion3);
		conclusion4 = new TutorialPage("conclusion4");
		conclusion4.windowTitle = Localizer.Format("#autoLOC_309671");
		conclusion4.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbsUp, 3f);
		};
		conclusion4.dialog = new MultiOptionDialog(conclusion4.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309679", tutorialControlColorString, GameSettings.MODIFIER_KEY.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309680"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(conclusion4);
		deathBonusPage = new TutorialPage("deathBonusPage");
		deathBonusPage.windowTitle = Localizer.Format("#autoLOC_309691");
		deathBonusPage.OnEnter = delegate
		{
			deathBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_false_disappointed);
		};
		deathBonusPage.dialog = new MultiOptionDialog(deathBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_309699"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_309700"), delegate
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
		InputLockManager.RemoveControlLock("docking");
	}
}
