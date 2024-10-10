using System;
using ns9;
using UnityEngine;

public class TutorialFromMun : TutorialScenario
{
	public Texture2D navBallVectors;

	public TutorialPage welcome;

	public TutorialPage deathBonusPage;

	public KFSMEvent onDeath;

	public FloatCurve velocityPitch;

	public double pitchDesired = 90.0;

	public DirectionTarget pitchTarget;

	public ManeuverNode mNode;

	public Part decoupler;

	public ModuleParachute chute;

	public double terrainHeightLast;

	public double vSpeed;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
	}

	public override void OnTutorialSetup()
	{
		ConfigNode configNode = null;
		velocityPitch = new FloatCurve();
		configNode = TutorialScenario.GetTutorialNode("FromMun");
		if (configNode != null && configNode.GetNode("velocityPitch") != null)
		{
			velocityPitch.Load(configNode.GetNode("velocityPitch"));
		}
		else
		{
			Debug.LogError("[Tutorial]: Could not find config node for FromMun tutorial!");
		}
		pitchTarget = new DirectionTarget("pitchTarget", "#autoLOC_1900853");
		TutorialPage tutorialPage = (welcome = new TutorialPage("welcome"));
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_315980");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.PAUSE | ControlTypes.CAMERACONTROLS), "flightTutorial");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_315989"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_315990"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("eva");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316004");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_idle_lookAround);
			InputLockManager.SetControlLock(ControlTypes.THROTTLE | ControlTypes.STAGING | ControlTypes.TIMEWARP | ControlTypes.THROTTLE_CUT_MAX, "flightTutorial");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316014", tutorialHighlightColorString, tutorialControlColorString, GameSettings.EVA_forward.name, tutorialControlColorString, GameSettings.EVA_left.name, tutorialControlColorString, GameSettings.EVA_back.name, tutorialControlColorString, GameSettings.EVA_right.name, tutorialControlColorString, GameSettings.EVA_TogglePack.name, tutorialControlColorString, GameSettings.EVA_Pack_up.name, tutorialControlColorString, GameSettings.EVA_Pack_down.name, tutorialControlColorString, GameSettings.EVA_Pack_forward.name, tutorialControlColorString, GameSettings.EVA_Pack_left.name, tutorialControlColorString, GameSettings.EVA_Pack_back.name, tutorialControlColorString, GameSettings.EVA_Pack_right.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316015"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => !FlightGlobals.ActiveVessel.isEVA, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("flightPlan");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316033");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.PAUSE | ControlTypes.CAMERACONTROLS), "flightTutorial");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316043"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316044"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("readyToLaunch");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316058");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			InputLockManager.RemoveControlLock("flightTutorial");
			terrainHeightLast = FlightGlobals.ActiveVessel.heightFromTerrain;
			UpdateDirectionTarget();
			FlightGlobals.fetch.SetVesselTarget(pitchTarget);
			InputLockManager.SetControlLock(ControlTypes.TARGETING, "TutTargeting");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316074", tutorialControlColorString, GameSettings.THROTTLE_FULL.name), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0.1f);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("liftoff");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316086");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316093", 50000.0.ToString("N0")), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_316096", FlightGlobals.ActiveVessel.orbit.ApA.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_316100", FlightGlobals.ActiveVessel.heightFromTerrain.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_316104", vSpeed.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316106"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => FlightGlobals.ActiveVessel.orbit.ApA >= 50000.0, dismissOnSelect: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.orbit.ApA >= 100000.0);
		tutorialPage.OnFixedUpdate = delegate
		{
			UpdateDirectionTarget();
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("coast");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316132");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
			Vessel activeVessel8 = FlightGlobals.ActiveVessel;
			if (activeVessel8.ctrlState.mainThrottle > 0f)
			{
				activeVessel8.ctrlState.mainThrottle = 0f;
				FlightInputHandler.SetNeutralControls();
			}
			InputLockManager.RemoveControlLock("TutTargeting");
			FlightGlobals.fetch.SetVesselTarget(null);
			double num = activeVessel8.orbit.timeToAp + Planetarium.GetUniversalTime();
			mNode = activeVessel8.patchedConicSolver.AddManeuverNode(num);
			double num2 = Math.Sqrt(activeVessel8.mainBody.gravParameter / activeVessel8.orbit.ApR);
			Vector3d orbitalVelocityAtUT = activeVessel8.orbit.getOrbitalVelocityAtUT(num);
			mNode.DeltaV = new Vector3d(0.0, 0.0, num2 - orbitalVelocityAtUT.magnitude);
			activeVessel8.patchedConicSolver.UpdateFlightPlan();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316157", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316158"), delegate
		{
			if (!MapView.MapIsEnabled)
			{
				MapView.EnterMapView();
			}
			TimeWarp.fetch.WarpTo(FlightGlobals.ActiveVessel.orbit.timeToAp + Planetarium.GetUniversalTime() - 45.0 - mNode.DeltaV.magnitude * 0.1);
		}, () => TimeWarp.CurrentRate == 1f && FlightGlobals.ActiveVessel.orbit.timeToAp > 60.0 + mNode.DeltaV.magnitude * 0.1, dismissOnSelect: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.orbit.timeToAp < 42.0 + mNode.DeltaV.magnitude * 0.1);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("circularize");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316183");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodA);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316191", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316192"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			Vessel activeVessel7 = FlightGlobals.ActiveVessel;
			return activeVessel7.orbit.PeA > activeVessel7.altitude - 5000.0 && (mNode == null || mNode.GetBurnVector(activeVessel7.orbit).sqrMagnitude <= 1.0);
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("howToEject");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316211");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316218", tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316219"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => MapView.MapIsEnabled, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("createReturn");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316237");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_idle_sigh);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316245"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316246"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			Vessel activeVessel6 = FlightGlobals.ActiveVessel;
			if (activeVessel6.orbit.referenceBody.isHomeWorld)
			{
				Tutorial.GoToNextPage();
				return true;
			}
			double universalTime = Planetarium.GetUniversalTime();
			if (activeVessel6.patchedConicSolver != null)
			{
				int count3 = activeVessel6.patchedConicSolver.maneuverNodes.Count;
				while (count3-- > 0)
				{
					mNode = activeVessel6.patchedConicSolver.maneuverNodes[count3];
					if (!(mNode.double_0 < universalTime))
					{
						Orbit orbit2 = null;
						if (mNode.patch != null && mNode.patch.referenceBody != null && mNode.patch.referenceBody.isHomeWorld)
						{
							orbit2 = mNode.patch;
						}
						else if (mNode.nextPatch != null && mNode.nextPatch.referenceBody != null && mNode.nextPatch.referenceBody.isHomeWorld)
						{
							orbit2 = mNode.nextPatch;
						}
						else if (mNode.nextPatch != null && mNode.nextPatch.nextPatch != null && mNode.nextPatch.nextPatch.referenceBody != null && mNode.nextPatch.nextPatch.referenceBody.isHomeWorld)
						{
							orbit2 = mNode.nextPatch.nextPatch;
						}
						if (orbit2 != null && orbit2.PeA <= 32000.0 && !(orbit2.PeA < 5000.0))
						{
							return true;
						}
					}
				}
			}
			Orbit orbit3 = activeVessel6.orbit;
			if (orbit3.nextPatch != null && ((orbit3.nextPatch.referenceBody != null && orbit3.nextPatch.referenceBody.isHomeWorld && orbit3.nextPatch.PeA >= 5000.0 && orbit3.nextPatch.PeA <= 32000.0) || (orbit3.nextPatch.nextPatch != null && orbit3.nextPatch.nextPatch.referenceBody != null && orbit3.nextPatch.nextPatch.referenceBody.isHomeWorld && orbit3.nextPatch.nextPatch.PeA >= 5000.0 && orbit3.nextPatch.nextPatch.PeA <= 32000.0)))
			{
				return true;
			}
			return (orbit3.referenceBody.isHomeWorld && orbit3.PeA >= 5000.0 && orbit3.PeA <= 32000.0) ? true : false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("burnReturn");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316312");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316319"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316320"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			Vessel activeVessel5 = FlightGlobals.ActiveVessel;
			if (!(activeVessel5.patchedConicSolver == null) && activeVessel5.patchedConicSolver.maneuverNodes != null)
			{
				if (activeVessel5.patchedConicSolver.maneuverNodes.Count != 0)
				{
					return false;
				}
				Orbit orbit = activeVessel5.orbit;
				if (orbit.nextPatch != null && ((orbit.nextPatch.referenceBody != null && orbit.nextPatch.referenceBody.isHomeWorld && orbit.nextPatch.PeA >= 5000.0 && orbit.nextPatch.PeA <= 32000.0) || (orbit.nextPatch.nextPatch != null && orbit.nextPatch.nextPatch.referenceBody != null && orbit.nextPatch.nextPatch.referenceBody.isHomeWorld && orbit.nextPatch.nextPatch.PeA >= 5000.0 && orbit.nextPatch.nextPatch.PeA <= 32000.0)))
				{
					return true;
				}
				if (orbit.referenceBody.isHomeWorld && orbit.PeA >= 5000.0 && orbit.PeA <= 32000.0)
				{
					return true;
				}
				return false;
			}
			return false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("warpToKerbin");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316354");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316362", tutorialControlColorString, tutorialHighlightColorString, tutorialControlColorString, GameSettings.TIME_WARP_INCREASE.name), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			Vessel activeVessel4 = FlightGlobals.ActiveVessel;
			return activeVessel4.mainBody.isHomeWorld && activeVessel4.altitude <= 120000.0;
		});
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("prepForReentry");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316376");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
			TimeWarp.SetRate(0, instant: false);
			decoupler = FlightGlobals.ActiveVessel.Parts.Find((Part p) => p.partInfo.name == "stackDecoupler");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316388"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => decoupler == null || decoupler.vessel != FlightGlobals.ActiveVessel);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("reentry");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316401");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			Vessel activeVessel3 = FlightGlobals.ActiveVessel;
			chute = null;
			int count2 = activeVessel3.Parts.Count;
			Part part2;
			do
			{
				if (count2-- <= 0)
				{
					return;
				}
				part2 = activeVessel3.Parts[count2];
			}
			while (!(part2.partInfo.name == "radialDrogue"));
			chute = part2.Modules.GetModule<ModuleParachute>();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316420"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => chute == null || chute.deploymentState != ModuleParachute.deploymentStates.STOWED);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("drogueDep");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316432");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
			Vessel activeVessel2 = FlightGlobals.ActiveVessel;
			chute = null;
			int count = activeVessel2.Parts.Count;
			Part part;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
				part = activeVessel2.Parts[count];
			}
			while (!(part.partInfo.name == "parachuteSingle"));
			chute = part.Modules.GetModule<ModuleParachute>();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316452"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState KFSMEvent) => chute == null || chute.deploymentState != ModuleParachute.deploymentStates.STOWED);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("mainDep");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316464");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_smileA);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316472"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			return activeVessel.situation == Vessel.Situations.LANDED || activeVessel.situation == Vessel.Situations.SPLASHED;
		});
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("splashdown");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316485");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316493"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316494"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("summary");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_316509");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_smileB);
			TimeWarp.SetRate(0, instant: false);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316518"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316519"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		deathBonusPage = new TutorialPage("deathBonusPage");
		deathBonusPage.windowTitle = Localizer.Format("#autoLOC_316537");
		deathBonusPage.OnEnter = delegate
		{
			InputLockManager.SetControlLock(ControlTypes.STAGING, "flightTutorial");
			deathBonusPage.onAdvanceConditionMet.GoToStateOnEvent = Tutorial.lastPage;
			instructor.PlayEmote(instructor.anim_false_disappointed);
		};
		deathBonusPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_316548"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_316549"), delegate
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

	public void UpdateDirectionTarget()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		float num = (float)activeVessel.srfSpeed;
		pitchDesired = velocityPitch.Evaluate(num);
		double num2 = TimeWarp.fixedDeltaTime;
		if (num2 > 0.0)
		{
			vSpeed = ((double)activeVessel.heightFromTerrain - terrainHeightLast) / num2;
			vSpeed -= 3.0;
			vSpeed = 0.0 - vSpeed;
			if (vSpeed >= 0.0 && num > 15f && (double)activeVessel.heightFromTerrain < vSpeed * 20.0)
			{
				pitchDesired += Math.Min(vSpeed * 0.25, 30.0);
			}
			vSpeed = 0.0 - vSpeed;
		}
		terrainHeightLast = activeVessel.heightFromTerrain;
		pitchTarget.Update(activeVessel, pitchDesired, 90.0, degrees: true);
	}
}
