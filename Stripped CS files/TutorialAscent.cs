using System;
using System.Collections.Generic;
using ns36;
using ns9;
using UnityEngine;

public class TutorialAscent : TutorialScenario
{
	public TutorialPage welcome;

	public TutorialPage ivaBonusPage;

	public TutorialPage deathBonusPage;

	public TutorialPage dockingCtrlBonusPage;

	public TutorialPage failBonusPage;

	public TutorialPage orbitPage;

	public KFSMEvent onFoundIVAMode;

	public KFSMEvent onVesselDied;

	public KFSMEvent onFoundDockingMode;

	public KFSMEvent onOffCourse;

	public KFSMEvent onOrbit;

	public FloatCurve velocityPitch;

	public FloatCurve velocityAlt;

	public bool checkAlt;

	public bool offCourse;

	public bool careOffCourse = true;

	public double srfVelMax = -1.0;

	public double altDesired;

	public double pitchDesired = 90.0;

	public double margin = 1000.0;

	public double nodeDvMag;

	public DirectionTarget pitchTarget;

	public ManeuverNode mNode;

	public Part solidBooster;

	public ModuleEngines engine;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
	}

	public override void OnTutorialSetup()
	{
		ConfigNode configNode = null;
		velocityPitch = new FloatCurve();
		velocityAlt = new FloatCurve();
		configNode = TutorialScenario.GetTutorialNode("GoForOrbit");
		if (configNode != null && configNode.GetNode("velocityPitch") != null && configNode.GetNode("velocityAlt") != null)
		{
			velocityPitch.Load(configNode.GetNode("velocityPitch"));
			velocityAlt.Load(configNode.GetNode("velocityAlt"));
		}
		else
		{
			Debug.LogError("[Tutorial]: Could not find config node for GoForOrbit tutorial!");
		}
		pitchTarget = new DirectionTarget("pitchTarget", "#autoLOC_1900853");
		TutorialPage tutorialPage = (welcome = new TutorialPage("welcome"));
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_307856");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.PAUSE | ControlTypes.CAMERAMODES | ControlTypes.CAMERACONTROLS), "flightTutorial");
		};
		tutorialPage.dialog = new MultiOptionDialog("welcome", "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_307865", tutorialControlColorString, GameSettings.PAUSE.name, tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_307866"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("flightPlan");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_307880");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_smileA);
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.THROTTLE | ControlTypes.PAUSE | ControlTypes.CAMERAMODES | ControlTypes.CAMERACONTROLS | ControlTypes.THROTTLE_CUT_MAX), "flightTutorial");
		};
		tutorialPage.dialog = new MultiOptionDialog("flightPlan", "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_307889", tutorialControlColorString, GameSettings.THROTTLE_FULL.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_307890"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0.98f, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("readyToLaunch");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_307907");
		tutorialPage.OnEnter = delegate
		{
			HighLogic.CurrentGame.Parameters.Flight.CanIVA = true;
			InputLockManager.RemoveControlLock("flightTutorial");
			UpdateDirectionTarget();
			FlightGlobals.fetch.SetVesselTarget(pitchTarget, overrideInputLock: true);
			InputLockManager.SetControlLock(ControlTypes.TARGETING, "TutTargeting");
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_307924", tutorialControlColorString, GameSettings.LAUNCH_STAGES.name, tutorialHighlightColorString), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.situation == Vessel.Situations.FLYING);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("liftoff");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_307936");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_smileB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_307943"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.verticalSpeed >= 60.0);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("turnStart");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_307959");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
			solidBooster = FlightGlobals.ActiveVessel.parts.Find((Part p) => p.partInfo.name == "solidBooster" || p.partInfo.name == "solidBooster.v2");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_307968"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => solidBooster == null || solidBooster.Resources["SolidFuel"].amount < 50.0);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("solidsNear");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_307984");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_307991", tutorialControlColorString, GameSettings.LAUNCH_STAGES.name), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => solidBooster == null || solidBooster.Resources["SolidFuel"].amount < 0.5);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("solidsDone");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308007");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308014", tutorialControlColorString, GameSettings.LAUNCH_STAGES.name), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => solidBooster == null || solidBooster.vessel != FlightGlobals.ActiveVessel);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("solidsGone");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308030");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			Part part2 = FlightGlobals.ActiveVessel.parts.Find((Part p) => p.partInfo.name == "liquidEngine2" || p.partInfo.name == "liquidEngine2.v2");
			if (part2 != null)
			{
				engine = part2.Modules.GetModule<ModuleEngines>();
			}
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308041"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => engine == null || engine.part == null || engine.vessel != FlightGlobals.ActiveVessel || engine.flameout || engine.propellantReqMet < 10f);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("lowerNear");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308057");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308064", tutorialControlColorString, GameSettings.LAUNCH_STAGES.name), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => engine == null || engine.vessel != FlightGlobals.ActiveVessel);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("lowerGone");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308080");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_nodB);
			engine = null;
			Part part = FlightGlobals.ActiveVessel.parts.Find((Part p) => p.partInfo.name == "liquidEngine3" || p.partInfo.name == "liquidEngine3.v2");
			if (part != null)
			{
				engine = part.Modules.GetModule<ModuleEngines>();
			}
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308091"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => engine == null || engine.EngineIgnited);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("upperLit");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308107");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308114", tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => MapView.MapIsEnabled || FlightGlobals.ActiveVessel.orbit.ApA > 60000.0);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("openNavball");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308162");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			if (!MapView.MapIsEnabled)
			{
				MapView.EnterMapView();
			}
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308139", tutorialControlColorString, GameSettings.NAVBALL_TOGGLE.name), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_308142", FlightGlobals.ActiveVessel.altitude.ToString("N0"), FlightGlobals.ActiveVessel.orbit.ApA.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			Vessel activeVessel7 = FlightGlobals.ActiveVessel;
			return activeVessel7.orbit.ApA > 80000.0 || activeVessel7.altitude > 70000.0 || NavBallToggle.Instance == null || NavBallToggle.Instance.panel.expanded;
		});
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("seco");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308130");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308169", tutorialControlColorString, tutorialControlColorString, GameSettings.THROTTLE_CUTOFF.name), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_308172", FlightGlobals.ActiveVessel.altitude.ToString("N0"), FlightGlobals.ActiveVessel.orbit.ApA.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			Vessel activeVessel6 = FlightGlobals.ActiveVessel;
			double apA = activeVessel6.orbit.ApA;
			return apA > 80000.0 && (activeVessel6.ctrlState.mainThrottle == 0f || apA > 81000.0);
		});
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("coast");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308193");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
			if (FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0f)
			{
				FlightGlobals.ActiveVessel.ctrlState.mainThrottle = 0f;
				FlightInputHandler.SetNeutralControls();
			}
			InputLockManager.RemoveControlLock("TutTargeting");
			FlightGlobals.fetch.SetVesselTarget(null);
			careOffCourse = false;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_308212", FlightGlobals.ActiveVessel.altitude.ToString("N0"), FlightGlobals.ActiveVessel.orbit.ApA.ToString("N0")), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.altitude > 70000.0);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("outOfAtmo");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308226");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			Vessel activeVessel5 = FlightGlobals.ActiveVessel;
			double double_ = activeVessel5.orbit.timeToAp + Planetarium.GetUniversalTime();
			double num3 = Math.Sqrt(Planetarium.fetch.Home.gravParameter / activeVessel5.orbit.ApR);
			nodeDvMag = new Vector3d(0.0, 0.0, num3 - activeVessel5.orbit.getOrbitalVelocityAtUT(double_).magnitude).magnitude;
			careOffCourse = false;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308241", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308242"), delegate
		{
			TimeWarp.fetch.Mode = TimeWarp.Modes.HIGH;
			TimeWarp.SetRate(2, instant: false);
		}, () => TimeWarp.CurrentRate == 1f, dismissOnSelect: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			if (FlightGlobals.ActiveVessel.orbit.timeToAp < Math.Max(90.0, nodeDvMag * 0.5))
			{
				TimeWarp.SetRate(0, instant: false);
				return true;
			}
			return false;
		});
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("nearAp");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308271");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_sigh);
			Vessel activeVessel4 = FlightGlobals.ActiveVessel;
			double num = activeVessel4.orbit.timeToAp + Planetarium.GetUniversalTime();
			double num2 = Math.Sqrt(Planetarium.fetch.Home.gravParameter / activeVessel4.orbit.ApR);
			Vector3d orbitalVelocityAtUT = activeVessel4.orbit.getOrbitalVelocityAtUT(num);
			mNode = activeVessel4.patchedConicSolver.AddManeuverNode(num);
			mNode.DeltaV = new Vector3d(0.0, 0.0, num2 - orbitalVelocityAtUT.magnitude);
			activeVessel4.patchedConicSolver.UpdateFlightPlan();
			careOffCourse = false;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308288", tutorialHighlightColorString), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			Vessel activeVessel3 = FlightGlobals.ActiveVessel;
			return activeVessel3.orbit.timeToAp < mNode.DeltaV.magnitude / activeVessel3.specificAcceleration * 0.75;
		});
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("circularize");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308302");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_nodB);
			careOffCourse = false;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308311"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308312"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => FlightGlobals.ActiveVessel.orbit.PeA > 75000.0, dismissOnSelect: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			Vessel activeVessel2 = FlightGlobals.ActiveVessel;
			if (mNode != null && engine != null && !engine.flameout && engine.propellantReqMet > 0f)
			{
				return mNode.GetBurnVector(FlightGlobals.ActiveVessel.orbit).sqrMagnitude <= 1.0;
			}
			return (activeVessel2.orbit.PeA >= activeVessel2.altitude - 1000.0 && activeVessel2.orbit.PeA > 70000.0) || activeVessel2.orbit.PeA >= 85000.0 || (activeVessel2.orbit.PeA > 71000.0 && activeVessel2.orbit.ApA > 120000.0);
		});
		Tutorial.AddPage(tutorialPage);
		tutorialPage = (orbitPage = new TutorialPage("inOrbit"));
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308340");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			if (activeVessel.ctrlState.mainThrottle > 0f)
			{
				activeVessel.ctrlState.mainThrottle = 0f;
				FlightInputHandler.SetNeutralControls();
			}
			List<ManeuverNode> list = new List<ManeuverNode>(activeVessel.patchedConicSolver.maneuverNodes);
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				list[i].RemoveSelf();
			}
			careOffCourse = false;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308362"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308363"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("summary");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_308378");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			careOffCourse = false;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308387", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308388"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		ivaBonusPage = new TutorialPage("ivaBonusPage");
		ivaBonusPage.windowTitle = Localizer.Format("#autoLOC_308406");
		ivaBonusPage.OnEnter = delegate
		{
			ivaBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		ivaBonusPage.dialog = new MultiOptionDialog(ivaBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308418", GameSettings.CAMERA_NEXT.name, GameSettings.CAMERA_MODE.name), expandW: false, expandH: true));
		ivaBonusPage.SetAdvanceCondition(delegate
		{
			if (CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.Internal && CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.const_3)
			{
				UnityEngine.Object.DestroyImmediate(dialogDisplay.gameObject);
				dialogDisplay = null;
				return true;
			}
			return false;
		});
		Tutorial.AddState(ivaBonusPage);
		deathBonusPage = new TutorialPage("deathBonusPage");
		deathBonusPage.windowTitle = Localizer.Format("#autoLOC_308438");
		deathBonusPage.OnEnter = delegate
		{
			InputLockManager.RemoveControlLock("TutTargeting");
			deathBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_false_disappointed);
		};
		deathBonusPage.dialog = new MultiOptionDialog(deathBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308451"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308452"), delegate
		{
			CloseTutorialWindow();
		}, dismissOnSelect: true));
		deathBonusPage.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.state != Vessel.State.DEAD);
		Tutorial.AddState(deathBonusPage);
		dockingCtrlBonusPage = new TutorialPage("dockingCtrlBonusPage");
		dockingCtrlBonusPage.windowTitle = Localizer.Format("#autoLOC_308470");
		dockingCtrlBonusPage.OnEnter = delegate
		{
			InputLockManager.SetControlLock(ControlTypes.STAGING, "flightTutorial");
			dockingCtrlBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		dockingCtrlBonusPage.dialog = new MultiOptionDialog(dockingCtrlBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_6005086"), expandW: false, expandH: true));
		dockingCtrlBonusPage.SetAdvanceCondition(delegate
		{
			if (FlightUIModeController.Instance.Mode != FlightUIMode.DOCKING)
			{
				UnityEngine.Object.DestroyImmediate(dialogDisplay.gameObject);
				dialogDisplay = null;
				return true;
			}
			return false;
		});
		Tutorial.AddState(dockingCtrlBonusPage);
		failBonusPage = new TutorialPage("failBonusPage");
		failBonusPage.windowTitle = Localizer.Format("#autoLOC_308501");
		failBonusPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_false_disappointed);
			InputLockManager.RemoveControlLock("TutTargeting");
		};
		failBonusPage.dialog = new MultiOptionDialog(failBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_308512"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_308513"), delegate
		{
			CloseTutorialWindow();
		}, dismissOnSelect: true));
		failBonusPage.onAdvanceConditionMet.GoToStateOnEvent = orbitPage;
		failBonusPage.SetAdvanceCondition(delegate
		{
			if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
			{
				UnityEngine.Object.DestroyImmediate(dialogDisplay.gameObject);
				dialogDisplay = null;
				careOffCourse = false;
				return true;
			}
			return false;
		});
		Tutorial.AddState(failBonusPage);
		onFoundIVAMode = new KFSMEvent("Found IVA Mode");
		onFoundIVAMode.GoToStateOnEvent = ivaBonusPage;
		onFoundIVAMode.OnCheckCondition = (KFSMState st) => CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.const_3;
		Tutorial.AddEventExcluding(onFoundIVAMode, ivaBonusPage);
		onVesselDied = new KFSMEvent("Vessel Died");
		onVesselDied.GoToStateOnEvent = deathBonusPage;
		onVesselDied.OnCheckCondition = (KFSMState st) => FlightGlobals.ActiveVessel.state == Vessel.State.DEAD;
		Tutorial.AddEventExcluding(onVesselDied, deathBonusPage);
		onFoundDockingMode = new KFSMEvent("Found Docking Mode");
		onFoundDockingMode.GoToStateOnEvent = dockingCtrlBonusPage;
		onFoundDockingMode.OnCheckCondition = (KFSMState st) => FlightUIModeController.Instance.Mode == FlightUIMode.DOCKING;
		Tutorial.AddEventExcluding(onFoundDockingMode, dockingCtrlBonusPage, ivaBonusPage);
		onOffCourse = new KFSMEvent("Found IVA Mode");
		onOffCourse.GoToStateOnEvent = failBonusPage;
		onOffCourse.OnCheckCondition = (KFSMState st) => offCourse && careOffCourse;
		Tutorial.AddEventExcluding(onOffCourse, deathBonusPage, failBonusPage);
		Tutorial.StartTutorial(welcome);
	}

	public override void OnOnDestroy()
	{
		HighLogic.CurrentGame.Parameters.Flight.CanEVA = true;
		InputLockManager.RemoveControlLock("flightTutorial");
		InputLockManager.RemoveControlLock("TutTargeting");
	}

	public void UpdateDirectionTarget()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		float time = (float)activeVessel.srfSpeed;
		srfVelMax = Math.Max(activeVessel.srfSpeed, srfVelMax);
		pitchDesired = velocityPitch.Evaluate(time);
		checkAlt = activeVessel.srfSpeed > 100.0 && pitchDesired < 85.0 && pitchDesired > 5.0 && activeVessel.altitude < 50000.0;
		altDesired = velocityAlt.Evaluate(time);
		margin = activeVessel.srfSpeed * 1.0;
		if (checkAlt)
		{
			double num = 1.0 / margin;
			double num2 = activeVessel.altitude - altDesired;
			double num3 = num2 * num;
			if (num3 < -1.0)
			{
				pitchDesired -= 3.0 * (num3 + 0.5);
				num2 = 0.0 - num2;
			}
			else if (num3 > 1.0)
			{
				pitchDesired -= 3.0 * (num3 - 0.5);
			}
			Vector3d lhs = Vector3d.Exclude(activeVessel.upAxis, activeVessel.srf_velocity);
			if (num2 > 1000.0 * Math.Max(1.0, activeVessel.srfSpeed * 0.01) || activeVessel.srfSpeed + 50.0 < srfVelMax || (lhs.sqrMagnitude > 50.0 && Vector3d.Dot(lhs, activeVessel.east) < 0.95))
			{
				offCourse = true;
			}
		}
		pitchTarget.Update(activeVessel, pitchDesired, 90.0, degrees: true);
	}
}
