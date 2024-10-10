using ns11;
using ns9;
using UnityEngine;

public class TutorialFlightSuborbital : TutorialScenario
{
	public TutorialPage page;

	public TutorialPage welcome;

	public TutorialPage pitchProgram1;

	public TutorialPage holdMid;

	public TutorialPage pitchProgram2;

	public TutorialPage holdSteady;

	public TutorialPage holdSteady2;

	public TutorialPage ivaBonusPage;

	public TutorialPage deathBonusPage;

	public TutorialPage dockingCtrlBonusPage;

	public TutorialPage chuteBurnBonusPage;

	public TutorialPage flipOutPage;

	public KFSMEvent onFlipOut;

	public KFSMEvent onDead;

	public KFSMEvent onFoundDockingMode;

	public KFSMEvent fsmEvent;

	public FloatCurve velocityPitch;

	public DirectionTarget pitchTarget;

	public bool willClearAtmo;

	public ModuleParachute chute;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
	}

	public override void OnTutorialSetup()
	{
		ConfigNode configNode = null;
		velocityPitch = new FloatCurve();
		configNode = TutorialScenario.GetTutorialNode("FlightSuborbital");
		if (configNode != null && configNode.GetNode("velocityPitch") != null)
		{
			velocityPitch.Load(configNode.GetNode("velocityPitch"));
		}
		else
		{
			Debug.LogError("[Tutorial]: Could not find config node for FlightSuborbital tutorial!");
		}
		pitchTarget = new DirectionTarget("pitchTarget", "#autoLOC_1900853");
		welcome = (page = new TutorialPage("welcome"));
		page.windowTitle = Localizer.Format("#autoLOC_315017");
		page.OnEnter = delegate
		{
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.PAUSE | ControlTypes.CAMERAMODES | ControlTypes.CAMERACONTROLS | ControlTypes.TARGETING), "flightTutorial");
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315026", tutorialControlColorString, GameSettings.PAUSE.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315027"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(page);
		page = new TutorialPage("flightPlan");
		page.windowTitle = Localizer.Format("#autoLOC_315041");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			UpdateDirectionTarget();
			FlightGlobals.fetch.SetVesselTarget(pitchTarget);
			InputLockManager.SetControlLock(ControlTypes.TARGETING, "TutTargeting");
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315054"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315055"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(page);
		page = new TutorialPage("throttle");
		page.windowTitle = Localizer.Format("#autoLOC_315069");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.THROTTLE | ControlTypes.PAUSE | ControlTypes.CAMERAMODES | ControlTypes.CAMERACONTROLS | ControlTypes.THROTTLE_CUT_MAX), "flightTutorial");
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315079", tutorialControlColorString, GameSettings.THROTTLE_UP.name, tutorialControlColorString, GameSettings.THROTTLE_DOWN.name, tutorialControlColorString, GameSettings.THROTTLE_FULL.name, tutorialControlColorString, GameSettings.THROTTLE_CUTOFF.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315080"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(page);
		page = new TutorialPage("SAS");
		page.windowTitle = Localizer.Format("#autoLOC_315094");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.flag_8 | ControlTypes.PAUSE | ControlTypes.CAMERAMODES | ControlTypes.CAMERACONTROLS), "flightTutorial");
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315103", tutorialControlColorString, GameSettings.SAS_HOLD.name, tutorialControlColorString, GameSettings.SAS_TOGGLE.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315104"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(page);
		page = new TutorialPage("prepForLaunch");
		page.windowTitle = Localizer.Format("#autoLOC_315118");
		page.OnEnter = delegate
		{
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.THROTTLE | ControlTypes.flag_8 | ControlTypes.PAUSE | ControlTypes.CAMERAMODES | ControlTypes.CAMERACONTROLS | ControlTypes.THROTTLE_CUT_MAX), "flightTutorial");
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315125", tutorialHighlightColorString, tutorialControlColorString, GameSettings.SAS_TOGGLE.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315126"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			Vessel activeVessel14 = FlightGlobals.ActiveVessel;
			return activeVessel14.ctrlState.mainThrottle >= 0.62f && activeVessel14.ctrlState.mainThrottle <= 0.75f && activeVessel14.ActionGroups[KSPActionGroup.flag_6];
		}, dismissOnSelect: true));
		Tutorial.AddPage(page);
		page = new TutorialPage("readingwarning");
		page.windowTitle = Localizer.Format("#autoLOC_315144");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315153", tutorialHighlightColorString, tutorialControlColorString, GameSettings.PAUSE.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315154"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(page);
		page = new TutorialPage("readyToLaunch");
		page.windowTitle = Localizer.Format("#autoLOC_315168");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
			InputLockManager.RemoveControlLock("flightTutorial");
			chute = null;
			Vessel activeVessel13 = FlightGlobals.ActiveVessel;
			int count3 = activeVessel13.Parts.Count;
			Part part3;
			do
			{
				if (count3-- <= 0)
				{
					return;
				}
				part3 = activeVessel13.Parts[count3];
			}
			while (!(part3.partInfo.name == "parachuteSingle"));
			chute = part3.Modules.GetModule<ModuleParachute>();
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315190", tutorialControlColorString, GameSettings.PAUSE.name, tutorialHighlightColorString, tutorialControlColorString, GameSettings.LAUNCH_STAGES.name), expandW: false, expandH: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.situation == Vessel.Situations.FLYING);
		Tutorial.AddPage(page);
		page = new TutorialPage("pitchProgramWait");
		page.windowTitle = Localizer.Format("#autoLOC_315202");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315210"), expandW: false, expandH: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.srfSpeed >= 49.0);
		page.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(page);
		pitchProgram1 = (page = new TutorialPage("pitchProgram1"));
		page.windowTitle = Localizer.Format("#autoLOC_315227");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315235"), expandW: false, expandH: true));
		page.SetAdvanceCondition(delegate
		{
			Vessel activeVessel12 = FlightGlobals.ActiveVessel;
			if (activeVessel12.srfSpeed >= 230.0)
			{
				pitchProgram1.onAdvanceConditionMet.GoToStateOnEvent = pitchProgram2;
				return true;
			}
			return activeVessel12.srfSpeed >= 150.0 && Vector3.Dot(pitchTarget.direction.normalized, activeVessel12.ReferenceTransform.up) >= 0.9994f;
		});
		page.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(page);
		holdMid = (page = new TutorialPage("holdMid"));
		page.windowTitle = Localizer.Format("#autoLOC_315260");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315268"), expandW: false, expandH: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.srfSpeed >= 249.0);
		page.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(page);
		pitchProgram2 = (page = new TutorialPage("pitchProgram2"));
		page.windowTitle = Localizer.Format("#autoLOC_315286");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315294"), expandW: false, expandH: true));
		page.SetAdvanceCondition(delegate
		{
			Vessel activeVessel11 = FlightGlobals.ActiveVessel;
			return activeVessel11.srfSpeed >= 550.0 || (activeVessel11.srfSpeed >= 400.0 && Vector3.Dot(pitchTarget.direction.normalized, activeVessel11.ReferenceTransform.up) >= 0.9994f);
		});
		page.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(page);
		holdSteady = (page = new TutorialPage("holdSteady"));
		page.windowTitle = Localizer.Format("#autoLOC_315313");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodA);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315321"), expandW: false, expandH: true));
		page.SetAdvanceCondition(delegate
		{
			Vessel activeVessel10 = FlightGlobals.ActiveVessel;
			if (activeVessel10.ctrlState.mainThrottle >= 0.99f)
			{
				return true;
			}
			ModuleEngines moduleEngines2 = null;
			int count2 = activeVessel10.parts.Count;
			while (count2-- > 0)
			{
				Part part2 = activeVessel10.parts[count2];
				if (part2.partInfo.name == "liquidEngine2")
				{
					moduleEngines2 = part2.Modules.GetModule<ModuleEngines>();
				}
			}
			if (moduleEngines2 == null)
			{
				return true;
			}
			return moduleEngines2.flameout ? true : false;
		});
		page.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(page);
		holdSteady2 = (page = new TutorialPage("holdSteady2"));
		page.windowTitle = Localizer.Format("#autoLOC_315358");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315366"), expandW: false, expandH: true));
		page.SetAdvanceCondition(delegate
		{
			Vessel activeVessel9 = FlightGlobals.ActiveVessel;
			ModuleEngines moduleEngines = null;
			int count = activeVessel9.parts.Count;
			while (count-- > 0)
			{
				Part part = activeVessel9.parts[count];
				if (part.partInfo.name == "liquidEngine2")
				{
					moduleEngines = part.Modules.GetModule<ModuleEngines>();
				}
			}
			if (moduleEngines == null)
			{
				return true;
			}
			return moduleEngines.flameout ? true : false;
		});
		page.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(page);
		page = new TutorialPage("burnout");
		page.windowTitle = Localizer.Format("#autoLOC_315403");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			if (FlightGlobals.ActiveVessel.orbit.ApA >= 78000.0)
			{
				willClearAtmo = true;
				instructor.PlayEmote(instructor.anim_true_thumbsUp);
			}
			else
			{
				instructor.PlayEmote(instructor.anim_false_disappointed);
			}
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(delegate
		{
			Vessel activeVessel8 = FlightGlobals.ActiveVessel;
			return willClearAtmo ? Localizer.Format("#autoLOC_315422", tutorialHighlightColorString, tutorialControlColorString, GameSettings.PAUSE.name, tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name) : Localizer.Format("#autoLOC_315424", (activeVessel8.orbit.ApA > 70000.0) ? "" : Localizer.Format("#autoLOC_5030003"), tutorialControlColorString, GameSettings.PAUSE.name, tutorialHighlightColorString);
		}, expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315426"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => willClearAtmo, dismissOnSelect: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.altitude > 50000.0 && willClearAtmo);
		page.OnFixedUpdate = delegate
		{
			pitchTarget.Update(FlightGlobals.ActiveVessel.srf_vel_direction);
		};
		Tutorial.AddPage(page);
		page = new TutorialPage("coast");
		page.windowTitle = Localizer.Format("#autoLOC_315452");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315460"), expandW: false, expandH: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.altitude > 70000.0);
		page.OnFixedUpdate = delegate
		{
			pitchTarget.Update(FlightGlobals.ActiveVessel.srf_vel_direction);
		};
		Tutorial.AddPage(page);
		page = new TutorialPage("science");
		page.windowTitle = Localizer.Format("#autoLOC_315477");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_idle_wonder);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315485"), expandW: false, expandH: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.verticalSpeed < -1.0);
		page.OnFixedUpdate = delegate
		{
			pitchTarget.Update(FlightGlobals.ActiveVessel.srf_vel_direction);
		};
		Tutorial.AddPage(page);
		page = new TutorialPage("reorient");
		page.windowTitle = Localizer.Format("#autoLOC_315502");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_smileB);
			pitchTarget.Update(FlightGlobals.ActiveVessel, 70.0, 270.0, degrees: true);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315512", tutorialHighlightColorString, tutorialControlColorString, GameSettings.PAUSE.name), expandW: false, expandH: true));
		page.SetAdvanceCondition(delegate
		{
			Vessel activeVessel7 = FlightGlobals.ActiveVessel;
			return activeVessel7.altitude < 75000.0 || Vector3.Dot(pitchTarget.direction.normalized, activeVessel7.ReferenceTransform.up) >= 0.9994f;
		});
		page.OnFixedUpdate = delegate
		{
			pitchTarget.Update(FlightGlobals.ActiveVessel, 70.0, 270.0, degrees: true);
		};
		Tutorial.AddPage(page);
		page = new TutorialPage("stabilize");
		page.windowTitle = Localizer.Format("#autoLOC_315531");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315539"), expandW: false, expandH: true));
		page.SetAdvanceCondition(delegate
		{
			Vessel activeVessel6 = FlightGlobals.ActiveVessel;
			return (activeVessel6.altitude < 75000.0 && Vector3.Dot(pitchTarget.direction.normalized, activeVessel6.ReferenceTransform.up) >= 0.9994f && !(activeVessel6.angularVelocity.sqrMagnitude >= 0.0001f)) || activeVessel6.altitude < 72000.0;
		});
		page.OnFixedUpdate = delegate
		{
			pitchTarget.Update(FlightGlobals.ActiveVessel, 70.0, 270.0, degrees: true);
		};
		Tutorial.AddPage(page);
		page = new TutorialPage("decouple");
		page.windowTitle = Localizer.Format("#autoLOC_315558");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_smileA);
			InputLockManager.RemoveControlLock("TutTargeting");
			FlightGlobals.fetch.SetVesselTarget(null);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315568"), expandW: false, expandH: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => StageManager.CurrentStage <= 1);
		Tutorial.AddPage(page);
		page = new TutorialPage("sasOff");
		page.windowTitle = Localizer.Format("#autoLOC_315581");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("TutTargeting");
			FlightGlobals.fetch.SetVesselTarget(null);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315591"), expandW: false, expandH: true));
		page.SetAdvanceCondition(delegate
		{
			Vessel activeVessel5 = FlightGlobals.ActiveVessel;
			if (!activeVessel5.ActionGroups[KSPActionGroup.flag_6] && activeVessel5.altitude < 70000.0)
			{
				return true;
			}
			if (activeVessel5.altitude < 68000.0)
			{
				activeVessel5.ActionGroups[KSPActionGroup.flag_6] = false;
				return true;
			}
			return false;
		});
		Tutorial.AddPage(page);
		page = new TutorialPage("reentry");
		page.windowTitle = Localizer.Format("#autoLOC_315613");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315621", tutorialHighlightColorString, tutorialControlColorString, GameSettings.PAUSE.name, tutorialControlColorString, GameSettings.LAUNCH_STAGES.name), expandW: false, expandH: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => !(chute == null) && chute.deploymentState != ModuleParachute.deploymentStates.STOWED);
		Tutorial.AddPage(page);
		page = new TutorialPage("chuteWait");
		page.windowTitle = Localizer.Format("#autoLOC_315636");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315644", tutorialHighlightColorString, tutorialControlColorString, GameSettings.PAUSE.name), expandW: false, expandH: true));
		page.SetAdvanceCondition((KFSMState KFSMEvent) => !(chute == null) && chute.deploymentState != ModuleParachute.deploymentStates.ACTIVE);
		Tutorial.AddPage(page);
		page = new TutorialPage("chuteDeployed");
		page.windowTitle = Localizer.Format("#autoLOC_315659");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodA);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315668"), expandW: false, expandH: true));
		page.SetAdvanceCondition(delegate
		{
			Vessel activeVessel4 = FlightGlobals.ActiveVessel;
			return activeVessel4.state == Vessel.State.DEAD || activeVessel4.situation == Vessel.Situations.LANDED || activeVessel4.situation == Vessel.Situations.SPLASHED;
		});
		Tutorial.AddPage(page);
		page = new TutorialPage("splashdown");
		page.windowTitle = Localizer.Format("#autoLOC_315681");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
		};
		page.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315689"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315690"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(page);
		ivaBonusPage = new TutorialPage("ivaBonusPage");
		ivaBonusPage.windowTitle = Localizer.Format("#autoLOC_315708");
		ivaBonusPage.OnEnter = delegate
		{
			ivaBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		ivaBonusPage.dialog = new MultiOptionDialog(ivaBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315720", tutorialControlColorString, GameSettings.CAMERA_NEXT.name, tutorialControlColorString, GameSettings.CAMERA_MODE.name), expandW: false, expandH: true));
		ivaBonusPage.SetAdvanceCondition(delegate
		{
			if (CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.Internal && CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.const_3)
			{
				Object.DestroyImmediate(dialogDisplay.gameObject);
				dialogDisplay = null;
				return true;
			}
			return false;
		});
		Tutorial.AddState(ivaBonusPage);
		deathBonusPage = new TutorialPage("deathBonusPage");
		deathBonusPage.windowTitle = Localizer.Format("#autoLOC_315740");
		deathBonusPage.OnEnter = delegate
		{
			InputLockManager.SetControlLock(ControlTypes.STAGING, "flightTutorial");
			deathBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_false_disappointed);
		};
		deathBonusPage.dialog = new MultiOptionDialog(page.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315749"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315750"), delegate
		{
			CloseTutorialWindow();
		}, dismissOnSelect: true));
		deathBonusPage.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.state != Vessel.State.DEAD);
		Tutorial.AddState(deathBonusPage);
		chuteBurnBonusPage = new TutorialPage("chuteBurnBonusPage");
		chuteBurnBonusPage.windowTitle = Localizer.Format("#autoLOC_315766");
		chuteBurnBonusPage.onAdvanceConditionMet.GoToStateOnEvent = deathBonusPage;
		chuteBurnBonusPage.OnEnter = delegate
		{
			InputLockManager.SetControlLock(ControlTypes.STAGING, "flightTutorial");
			chuteBurnBonusPage.onAdvanceConditionMet.GoToStateOnEvent = deathBonusPage;
			instructor.PlayEmote(instructor.anim_false_disappointed);
		};
		chuteBurnBonusPage.dialog = new MultiOptionDialog(chuteBurnBonusPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315776"), expandW: false, expandH: true));
		chuteBurnBonusPage.SetAdvanceCondition(delegate
		{
			if (FlightGlobals.ActiveVessel.state == Vessel.State.DEAD)
			{
				Object.DestroyImmediate(dialogDisplay.gameObject);
				dialogDisplay = null;
				return true;
			}
			return false;
		});
		Tutorial.AddState(chuteBurnBonusPage);
		dockingCtrlBonusPage = new TutorialPage("dockingCtrlBonusPage");
		dockingCtrlBonusPage.windowTitle = Localizer.Format("#autoLOC_315795");
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
				Object.DestroyImmediate(dialogDisplay.gameObject);
				dialogDisplay = null;
				return true;
			}
			return false;
		});
		Tutorial.AddState(dockingCtrlBonusPage);
		flipOutPage = new TutorialPage("flipOutPage");
		flipOutPage.windowTitle = Localizer.Format("#autoLOC_315826");
		flipOutPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_false_disappointed);
			flipOutPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
		};
		flipOutPage.dialog = new MultiOptionDialog(flipOutPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315836", tutorialControlColorString, GameSettings.PAUSE.name), expandW: false, expandH: true));
		flipOutPage.SetAdvanceCondition(delegate
		{
			Vessel activeVessel3 = FlightGlobals.ActiveVessel;
			return (Vector3.Dot(activeVessel3.ReferenceTransform.up, activeVessel3.srf_vel_direction) >= 0.98f || !(Vector3.Dot(activeVessel3.ReferenceTransform.up, pitchTarget.direction.normalized) < 0.98f)) ? true : false;
		});
		Tutorial.AddState(flipOutPage);
		fsmEvent = new KFSMEvent("Found IVA Mode");
		fsmEvent.GoToStateOnEvent = ivaBonusPage;
		fsmEvent.OnCheckCondition = (KFSMState st) => CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.const_3;
		Tutorial.AddEventExcluding(fsmEvent, ivaBonusPage);
		fsmEvent = new KFSMEvent("Died");
		fsmEvent.GoToStateOnEvent = deathBonusPage;
		fsmEvent.OnCheckCondition = (KFSMState st) => FlightGlobals.ActiveVessel.state == Vessel.State.DEAD;
		Tutorial.AddEventExcluding(fsmEvent, deathBonusPage, chuteBurnBonusPage);
		fsmEvent = new KFSMEvent("ChuteBurn");
		fsmEvent.GoToStateOnEvent = chuteBurnBonusPage;
		fsmEvent.OnCheckCondition = delegate
		{
			Vessel activeVessel2 = FlightGlobals.ActiveVessel;
			if (activeVessel2.state == Vessel.State.DEAD)
			{
				return false;
			}
			return activeVessel2.situation == Vessel.Situations.FLYING && activeVessel2.srfSpeed >= 100.0 && !(chute == null) && chute.deploymentState == ModuleParachute.deploymentStates.const_4;
		};
		Tutorial.AddEventExcluding(fsmEvent, chuteBurnBonusPage, deathBonusPage);
		fsmEvent = new KFSMEvent("Found Docking Mode");
		fsmEvent.GoToStateOnEvent = dockingCtrlBonusPage;
		fsmEvent.OnCheckCondition = (KFSMState st) => FlightUIModeController.Instance.Mode == FlightUIMode.DOCKING;
		Tutorial.AddEventExcluding(fsmEvent, dockingCtrlBonusPage, ivaBonusPage);
		fsmEvent = new KFSMEvent("onFlipOut");
		fsmEvent.GoToStateOnEvent = flipOutPage;
		fsmEvent.OnCheckCondition = delegate
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			return Vector3.Dot(activeVessel.ReferenceTransform.up, activeVessel.srf_vel_direction) < 0.8f;
		};
		Tutorial.AddEvent(fsmEvent, pitchProgram1, holdMid, pitchProgram2, holdSteady, holdSteady2);
		Tutorial.StartTutorial(welcome);
	}

	public override void OnOnDestroy()
	{
		HighLogic.CurrentGame.Parameters.Flight.CanEVA = true;
		InputLockManager.RemoveControlLock("TutTargeting");
		InputLockManager.RemoveControlLock("flightTutorial");
	}

	public void UpdateDirectionTarget()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		pitchTarget.Update(activeVessel, velocityPitch.Evaluate((float)activeVessel.srfSpeed), 90.0, degrees: true);
	}
}
