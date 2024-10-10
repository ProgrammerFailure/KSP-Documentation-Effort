using System;
using ns9;
using UnityEngine;

public class TutorialMunDescent : TutorialScenario
{
	public Texture2D navBallVectors;

	public TutorialPage welcome;

	public TutorialPage deathBonusPage;

	public KFSMEvent onDeath;

	public DirectionTarget pitchTarget;

	public double vSpeed;

	public double tgtSpeed;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
	}

	public override void OnTutorialSetup()
	{
		pitchTarget = new DirectionTarget("pitchTarget", "#autoLOC_1900853");
		TutorialPage tutorialPage = (welcome = new TutorialPage("welcome"));
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316653");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.PAUSE | ControlTypes.CAMERACONTROLS), "flightTutorial");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316662"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316663"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("overview");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316678");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316686"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316687"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("burnDOI");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316702");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
			InputLockManager.RemoveControlLock("flightTutorial");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316712", tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316713"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count == 0, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("refineDO");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316731");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316738"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316739"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			double peA = FlightGlobals.ActiveVessel.orbit.PeA;
			return peA < 6250.0 && peA > 5750.0;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("warpPe");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316760");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316768", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316769"), delegate
		{
			if (!MapView.MapIsEnabled)
			{
				MapView.EnterMapView();
			}
			TimeWarp.fetch.WarpTo(Planetarium.GetUniversalTime() + FlightGlobals.ActiveVessel.orbit.timeToPe - 15.0);
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("warpingPe");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316787");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316795"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			Orbit orbit = FlightGlobals.ActiveVessel.orbit;
			return orbit.timeToPe < 10.0 || orbit.timeToPe > orbit.timeToAp || orbit.PeA < 0.0;
		});
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("retroBraking");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316809");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			TimeWarp.SetRate(0, instant: false);
			instructor.PlayEmote(instructor.anim_true_thumbUp);
			Vessel activeVessel6 = FlightGlobals.ActiveVessel;
			pitchTarget.Update(-activeVessel6.srf_vel_direction);
			FlightGlobals.fetch.SetVesselTarget(pitchTarget);
			InputLockManager.SetControlLock(ControlTypes.TARGETING, "TutTargeting");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316824", tutorialControlColorString, GameSettings.LANDING_GEAR.name, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUILabel(delegate
		{
			Vessel activeVessel5 = FlightGlobals.ActiveVessel;
			return Localizer.Format("#autoLOC_316828", activeVessel5.heightFromTerrain.ToString("N0"), activeVessel5.verticalSpeed.ToString("N1"), activeVessel5.srfSpeed.ToString("N1")) + (((double)activeVessel5.ctrlState.mainThrottle > 0.35) ? Localizer.Format("#autoLOC_316831") : (((double)activeVessel5.ctrlState.mainThrottle < 0.31) ? Localizer.Format("#autoLOC_316832") : ""));
		}, skin.customStyles[1], expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			Vessel activeVessel4 = FlightGlobals.ActiveVessel;
			double num2 = activeVessel4.heightFromTerrain;
			if (num2 > 2500.0 && activeVessel4.verticalSpeed > -75.0)
			{
				return false;
			}
			return num2 > 0.0 && (num2 < 1500.0 || num2 < (0.0 - activeVessel4.verticalSpeed) * 25.0 || activeVessel4.srfSpeed > num2 / (0.0 - activeVessel4.verticalSpeed) * activeVessel4.specificAcceleration * 0.8 || activeVessel4.srfSpeed <= 100.0);
		});
		tutorialPage.OnFixedUpdate = delegate
		{
			Vessel activeVessel3 = FlightGlobals.ActiveVessel;
			double num = 0.0 - (activeVessel3.verticalSpeed + 50.0);
			if (num < 0.0)
			{
				pitchTarget.Update(-activeVessel3.srf_vel_direction);
			}
			else
			{
				num *= 0.01;
				pitchTarget.Update(Vector3d.Lerp(-activeVessel3.srf_vel_direction, activeVessel3.upAxis, (float)num));
			}
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("finalDescent");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316863");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316871"), expandW: false, expandH: true), new DialogGUILabel(delegate
		{
			Vessel activeVessel2 = FlightGlobals.ActiveVessel;
			tgtSpeed = (double)activeVessel2.heightFromTerrain * 0.1;
			return Localizer.Format("#autoLOC_316876", activeVessel2.heightFromTerrain.ToString("N0"), activeVessel2.verticalSpeed.ToString("N1"), activeVessel2.srfSpeed.ToString("N1"), tgtSpeed.ToString("N1")) + ((tgtSpeed - Math.Max(1.0, tgtSpeed * 0.05) > activeVessel2.srfSpeed) ? Localizer.Format("#autoLOC_316880") : ((tgtSpeed + Math.Max(1.0, tgtSpeed * 0.05) < activeVessel2.srfSpeed) ? Localizer.Format("#autoLOC_316881") : ""));
		}, skin.customStyles[1], expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => (double)FlightGlobals.ActiveVessel.heightFromTerrain < 100.0);
		tutorialPage.OnFixedUpdate = delegate
		{
			pitchTarget.Update(-FlightGlobals.ActiveVessel.srf_vel_direction);
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("softLanding");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316899");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316907"), expandW: false, expandH: true), new DialogGUILabel(delegate
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			tgtSpeed = Math.Max(1.0, (double)activeVessel.heightFromTerrain * 0.05);
			return Localizer.Format("#autoLOC_316912", activeVessel.heightFromTerrain.ToString("N0"), activeVessel.verticalSpeed.ToString("N1"), activeVessel.srfSpeed.ToString("N1"), tgtSpeed.ToString("N1")) + ((tgtSpeed - Math.Max(0.2, tgtSpeed * 0.1) > activeVessel.srfSpeed) ? Localizer.Format("#autoLOC_316916") : ((tgtSpeed + Math.Max(0.2, tgtSpeed * 0.1) < activeVessel.srfSpeed) ? Localizer.Format("#autoLOC_316917") : ""));
		}, skin.customStyles[1], expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED);
		tutorialPage.OnFixedUpdate = delegate
		{
			pitchTarget.Update(-FlightGlobals.ActiveVessel.srf_vel_direction);
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("summary");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316939");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			InputLockManager.RemoveControlLock("TutTargeting");
			FlightGlobals.fetch.SetVesselTarget(null);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316949"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316950"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		deathBonusPage = new TutorialPage("deathBonusPage");
		deathBonusPage.windowTitle = Localizer.Format("#autoLOC_316966");
		deathBonusPage.OnEnter = delegate
		{
			InputLockManager.SetControlLock(ControlTypes.STAGING, "flightTutorial");
			deathBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_false_disappointed);
		};
		deathBonusPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316977"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316978"), delegate
		{
			CloseTutorialWindow();
		}, dismissOnSelect: true));
		deathBonusPage.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.state != Vessel.State.DEAD);
		Tutorial.AddState(deathBonusPage);
		onDeath = new KFSMEvent("onDeath");
		onDeath.GoToStateOnEvent = deathBonusPage;
		onDeath.OnCheckCondition = (KFSMState st) => FlightGlobals.ActiveVessel.state == Vessel.State.DEAD;
		Tutorial.AddEventExcluding(onDeath, deathBonusPage);
		Tutorial.StartTutorial(welcome);
	}

	public override void OnOnDestroy()
	{
		HighLogic.CurrentGame.Parameters.Flight.CanEVA = true;
		InputLockManager.RemoveControlLock("TutTargeting");
		InputLockManager.RemoveControlLock("flightTutorial");
	}
}
