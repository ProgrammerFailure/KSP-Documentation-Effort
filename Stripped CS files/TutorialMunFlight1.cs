using ns9;

public class TutorialMunFlight1 : TutorialScenario
{
	public TutorialPage welcome;

	public TutorialPage explainTMI;

	public TutorialPage mapOpen1;

	public TutorialPage mapOpen2;

	public TutorialPage maneuver1;

	public TutorialPage maneuver2;

	public TutorialPage maneuversuccess;

	public TutorialPage maneuverexecute;

	public TutorialPage executionsuccess;

	public TutorialPage transfersuccess;

	public TutorialPage transfersuccess2;

	public TutorialPage transfersuccess2b;

	public TutorialPage transfersuccess3;

	public TutorialPage conclusion;

	[KSPField(isPersistant = true)]
	public string StateName;

	public ManeuverNode mNode;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
	}

	public override void OnOnDestroy()
	{
		InputLockManager.RemoveControlLock("munflight1lock");
	}

	public override void OnTutorialSetup()
	{
		welcome = new TutorialPage("welcome");
		welcome.windowTitle = Localizer.Format("#autoLOC_317041");
		welcome.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(ControlTypes.flag_68 | ControlTypes.THROTTLE | ControlTypes.STAGING | ControlTypes.TIMEWARP | ControlTypes.THROTTLE_CUT_MAX, "munflight1lock");
		};
		welcome.dialog = new MultiOptionDialog(welcome.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317049", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317050"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(welcome);
		explainTMI = new TutorialPage("explainTMI");
		explainTMI.windowTitle = Localizer.Format("#autoLOC_317061");
		explainTMI.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
			InputLockManager.SetControlLock(ControlTypes.THROTTLE | ControlTypes.STAGING | ControlTypes.TIMEWARP | ControlTypes.THROTTLE_CUT_MAX, "munflight1lock");
		};
		explainTMI.dialog = new MultiOptionDialog(explainTMI.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317070", tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name), expandW: false, expandH: true));
		explainTMI.SetAdvanceCondition((KFSMState st) => MapView.MapIsEnabled);
		Tutorial.AddPage(explainTMI);
		mapOpen1 = new TutorialPage("mapOpen1");
		mapOpen1.windowTitle = Localizer.Format("#autoLOC_317079");
		mapOpen1.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(ControlTypes.THROTTLE | ControlTypes.STAGING | ControlTypes.TIMEWARP | ControlTypes.THROTTLE_CUT_MAX, "munflight1lock");
		};
		mapOpen1.dialog = new MultiOptionDialog(mapOpen1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317087", tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true));
		mapOpen1.SetAdvanceCondition((KFSMState KFSMEvent) => OrbitTargeter.HasManeuverNode);
		Tutorial.AddPage(mapOpen1);
		mapOpen2 = new TutorialPage("mapOpen2");
		mapOpen2.windowTitle = Localizer.Format("#autoLOC_317098");
		mapOpen2.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
			StateName = "mapOpen2";
			InputLockManager.SetControlLock(ControlTypes.STAGING, "munflight1lock");
		};
		mapOpen2.dialog = new MultiOptionDialog(mapOpen2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317109"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317110"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(mapOpen2);
		maneuver1 = new TutorialPage("maneuver1");
		maneuver1.windowTitle = Localizer.Format("#autoLOC_317121");
		maneuver1.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_idle_sigh);
			StateName = "maneuver1";
			InputLockManager.SetControlLock(ControlTypes.STAGING, "munflight1lock");
		};
		maneuver1.dialog = new MultiOptionDialog(maneuver1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317132"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317133"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(maneuver1);
		maneuver2 = new TutorialPage("maneuver2");
		maneuver2.windowTitle = Localizer.Format("#autoLOC_317144");
		maneuver2.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			StateName = "maneuver2";
			InputLockManager.SetControlLock(ControlTypes.STAGING, "munflight1lock");
		};
		maneuver2.dialog = new MultiOptionDialog(maneuver2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317154"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317160"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			bool flag2 = false;
			if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
			{
				int count2 = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count;
				while (count2-- > 0)
				{
					mNode = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[count2];
					if (!(mNode.double_0 < Planetarium.GetUniversalTime()) && ((mNode.patch != null && mNode.patch.referenceBody != null && mNode.patch.referenceBody.name == "Mun") || (mNode.nextPatch != null && mNode.nextPatch.referenceBody != null && mNode.nextPatch.referenceBody.name == "Mun") || (mNode.nextPatch.nextPatch != null && mNode.nextPatch.nextPatch.referenceBody != null && mNode.nextPatch.nextPatch.referenceBody.name == "Mun")))
					{
						flag2 = true;
						break;
					}
				}
			}
			if (FlightGlobals.ActiveVessel.orbit.referenceBody.name == "Mun")
			{
				Tutorial.GoToNextPage();
			}
			else if (flag2)
			{
				return true;
			}
			return false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(maneuver2);
		maneuversuccess = new TutorialPage("maneuversuccess");
		maneuversuccess.windowTitle = Localizer.Format("#autoLOC_317203");
		maneuversuccess.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		maneuversuccess.dialog = new MultiOptionDialog(maneuversuccess.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317210"), expandW: false, expandH: true));
		maneuversuccess.SetAdvanceCondition(delegate
		{
			if (mNode == null)
			{
				bool flag = false;
				if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
				{
					int count = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count;
					while (count-- > 0)
					{
						mNode = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[count];
						if (!(mNode.double_0 < Planetarium.GetUniversalTime()) && ((mNode.patch != null && mNode.patch.referenceBody != null && mNode.patch.referenceBody.name == "Mun") || (mNode.nextPatch != null && mNode.nextPatch.referenceBody != null && mNode.nextPatch.referenceBody.name == "Mun") || (mNode.nextPatch.nextPatch != null && mNode.nextPatch.nextPatch.referenceBody != null && mNode.nextPatch.nextPatch.referenceBody.name == "Mun")))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return mNode.double_0 - Planetarium.GetUniversalTime() < 58.0;
		});
		Tutorial.AddPage(maneuversuccess);
		maneuverexecute = new TutorialPage("maneuverexecute");
		maneuverexecute.windowTitle = Localizer.Format("#autoLOC_317246");
		maneuverexecute.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
			StateName = "maneuverexecute";
			InputLockManager.SetControlLock(ControlTypes.STAGING, "munflight1lock");
		};
		maneuverexecute.dialog = new MultiOptionDialog(maneuverexecute.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317257"), expandW: false, expandH: true));
		maneuverexecute.SetAdvanceCondition((KFSMState _003Cp0_003E) => (FlightGlobals.ActiveVessel.orbit.closestEncounterLevel != 0 && FlightGlobals.ActiveVessel.orbit.nextPatch != null && ((FlightGlobals.ActiveVessel.orbit.nextPatch.referenceBody != null && FlightGlobals.ActiveVessel.orbit.nextPatch.referenceBody.name == "Mun") || (FlightGlobals.ActiveVessel.orbit.nextPatch.nextPatch != null && FlightGlobals.ActiveVessel.orbit.nextPatch.nextPatch.referenceBody != null && FlightGlobals.ActiveVessel.orbit.nextPatch.nextPatch.referenceBody.name == "Mun"))) ? true : false);
		Tutorial.AddPage(maneuverexecute);
		executionsuccess = new TutorialPage("executionsuccess");
		executionsuccess.windowTitle = Localizer.Format("#autoLOC_317276");
		executionsuccess.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
			MapView.EnterMapView();
		};
		executionsuccess.dialog = new MultiOptionDialog(executionsuccess.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317284"), expandW: false, expandH: true));
		executionsuccess.SetAdvanceCondition(delegate
		{
			if (FlightGlobals.ActiveVessel.orbit.referenceBody.name == "Mun")
			{
				TimeWarp.SetRate(0, instant: false);
				return true;
			}
			return false;
		});
		Tutorial.AddPage(executionsuccess);
		transfersuccess2 = new TutorialPage("transfersuccess2");
		transfersuccess2.windowTitle = Localizer.Format("#autoLOC_317299");
		transfersuccess2.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			TimeWarp.SetRate(0, instant: false);
			StateName = "transfersuccess2";
			InputLockManager.SetControlLock(ControlTypes.STAGING, "munflight1lock");
		};
		transfersuccess2.dialog = new MultiOptionDialog(transfersuccess2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(() => (!(FlightGlobals.ActiveVessel.orbit.referenceBody.name == "Mun")) ? Localizer.Format("#autoLOC_5030006") : Localizer.Format("#autoLOC_317313"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317316"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => FlightGlobals.ActiveVessel.orbit.PeA > 10000.0, dismissOnSelect: true));
		Tutorial.AddPage(transfersuccess2);
		transfersuccess2b = new TutorialPage("transfersuccess2b");
		transfersuccess2b.windowTitle = Localizer.Format("#autoLOC_317332");
		transfersuccess2b.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodA);
			TimeWarp.SetRate(0, instant: false);
			StateName = "transfersuccess2b";
			InputLockManager.SetControlLock(ControlTypes.STAGING, "munflight1lock");
		};
		transfersuccess2b.dialog = new MultiOptionDialog(transfersuccess2b.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(() => (!(FlightGlobals.ActiveVessel.orbit.referenceBody.name == "Mun")) ? Localizer.Format("#autoLOC_5030007") : Localizer.Format("#autoLOC_317347"), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_317351", FlightGlobals.ActiveVessel.orbit.eccentricity.ToString("N2")), skin.customStyles[1], expandW: false, expandH: true), new DialogGUILabel(() => (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count < 1) ? Localizer.Format("#autoLOC_317355") : Localizer.Format("#autoLOC_317357", FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[0].nextPatch.eccentricity.ToString("N2")), skin.customStyles[1], expandW: false, expandH: true));
		transfersuccess2b.SetAdvanceCondition((KFSMState _003Cp0_003E) => FlightGlobals.ActiveVessel.orbit.referenceBody.name == "Mun" && FlightGlobals.ActiveVessel.orbit.eccentricity < 0.8);
		Tutorial.AddPage(transfersuccess2b);
		transfersuccess3 = new TutorialPage("transfersuccess3");
		transfersuccess3.windowTitle = Localizer.Format("#autoLOC_317372");
		transfersuccess3.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_smileA);
			InputLockManager.SetControlLock(ControlTypes.STAGING, "munflight1lock");
		};
		transfersuccess3.dialog = new MultiOptionDialog(transfersuccess3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317381", 200000.ToString("N0"), 200000.ToString("N0")), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_317383", FlightGlobals.ActiveVessel.orbit.PeA.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_317385", FlightGlobals.ActiveVessel.orbit.ApA.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_317387", FlightGlobals.ActiveVessel.orbit.eccentricity.ToString("N2")), skin.customStyles[1], expandW: false, expandH: true));
		transfersuccess3.SetAdvanceCondition(delegate
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			return activeVessel.orbit.PeA < 200000.0 && activeVessel.orbit.PeA > 10000.0 && activeVessel.orbit.ApA < 200000.0 && activeVessel.orbit.eccentricity < 0.05;
		});
		Tutorial.AddPage(transfersuccess3);
		conclusion = new TutorialPage("conclusion");
		conclusion.windowTitle = Localizer.Format("#autoLOC_317400");
		conclusion.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			InputLockManager.RemoveControlLock("munflight1lock");
		};
		conclusion.dialog = new MultiOptionDialog(conclusion.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317408"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317409"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(conclusion);
		Tutorial.StartTutorial(StateName);
	}
}
