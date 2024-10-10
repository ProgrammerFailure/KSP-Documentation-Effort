using System;
using ns36;
using ns9;
using UnityEngine;

public class TutorialOrbit101 : TutorialScenario
{
	public Texture2D maneuverVectors;

	public const int WarpRateIndex = 1;

	public TutorialPage welcome;

	public TutorialPage whatsAnOrbit;

	public TutorialPage mapView;

	public TutorialPage attitude1;

	public TutorialPage attitude2;

	public TutorialPage attitude3;

	public TutorialPage navball;

	public TutorialPage waitForMap1;

	public TutorialPage raiseAp1;

	public TutorialPage waitForPeriapsis1;

	public TutorialPage raiseAp2;

	public TutorialPage raiseAp3a;

	public TutorialPage raiseAp3b;

	public TutorialPage raiseAp4;

	public TutorialPage raiseAp5;

	public TutorialPage raiseAp6;

	public TutorialPage waitForApoapsis1;

	public TutorialPage raisePe1;

	public TutorialPage raisePe2;

	public TutorialPage raisePe3;

	public TutorialPage incChange1;

	public TutorialPage incChange2;

	public TutorialPage incChange3;

	public TutorialPage lowerPe1;

	public TutorialPage lowerPe2;

	public TutorialPage lowerPe3;

	public TutorialPage NormalVectors;

	public TutorialPage conclusion;

	public Vector3 nudge = Vector3.zero;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
		maneuverVectors = AssetBase.GetTexture("ManeuverNode_vectors");
	}

	public override void OnTutorialSetup()
	{
		welcome = new TutorialPage("welcome");
		welcome.windowTitle = Localizer.Format("#autoLOC_317452");
		welcome.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(ControlTypes.THROTTLE | ControlTypes.STAGING | ControlTypes.THROTTLE_CUT_MAX, "Orbit101TutorialLock");
		};
		welcome.dialog = new MultiOptionDialog(welcome.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317460", instructor.CharacterName), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317461"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(welcome);
		whatsAnOrbit = new TutorialPage("whatsAnOrbit");
		whatsAnOrbit.windowTitle = Localizer.Format("#autoLOC_317472");
		whatsAnOrbit.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_smileA, 5f);
		};
		whatsAnOrbit.dialog = new MultiOptionDialog(whatsAnOrbit.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317479", tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name), expandW: false, expandH: true));
		whatsAnOrbit.SetAdvanceCondition((KFSMState st) => MapView.MapIsEnabled);
		Tutorial.AddPage(whatsAnOrbit);
		mapView = new TutorialPage("mapView");
		mapView.windowTitle = Localizer.Format("#autoLOC_317487");
		mapView.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_lookAround, 5f);
		};
		mapView.dialog = new MultiOptionDialog(mapView.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317494", tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name), expandW: false, expandH: true));
		mapView.SetAdvanceCondition((KFSMState st) => !MapView.MapIsEnabled);
		Tutorial.AddPage(mapView);
		attitude1 = new TutorialPage("attitude1");
		attitude1.windowTitle = Localizer.Format("#autoLOC_317502");
		attitude1.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_nodB, 0f);
			FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.flag_6] = false;
		};
		attitude1.dialog = new MultiOptionDialog(attitude1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317510", tutorialControlColorString, GameSettings.SAS_TOGGLE.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317511"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(attitude1);
		attitude2 = new TutorialPage("attitude2");
		attitude2.windowTitle = Localizer.Format("#autoLOC_317522");
		attitude2.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_wonder, 3f);
			nudge = UnityEngine.Random.onUnitSphere;
			Vessel activeVessel2 = FlightGlobals.ActiveVessel;
			activeVessel2.OnPostAutopilotUpdate = (FlightInputCallback)Delegate.Combine(activeVessel2.OnPostAutopilotUpdate, new FlightInputCallback(remoteNudge));
		};
		attitude2.dialog = new MultiOptionDialog(attitude2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(() => (Tutorial.TimeAtCurrentState < 3.0) ? Localizer.Format("#autoLOC_317538") : Localizer.Format("#autoLOC_317542", (FlightGlobals.ActiveVessel.angularVelocity.magnitude * 57.29578f).ToString("0.0")), expandW: false, expandH: true));
		attitude2.OnLeave = delegate
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			activeVessel.OnPostAutopilotUpdate = (FlightInputCallback)Delegate.Remove(activeVessel.OnPostAutopilotUpdate, new FlightInputCallback(remoteNudge));
		};
		attitude2.SetAdvanceCondition((KFSMState st) => Tutorial.TimeAtCurrentState > 4.0 && (double)FlightGlobals.ActiveVessel.angularVelocity.magnitude < 0.05);
		Tutorial.AddPage(attitude2);
		attitude3 = new TutorialPage("attitude3");
		attitude3.windowTitle = Localizer.Format("#autoLOC_317563");
		attitude3.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbsUp, 0f);
		};
		attitude3.dialog = new MultiOptionDialog(attitude3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317570"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317571"), delegate
		{
			Tutorial.GoToLastPage();
		}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_317576"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(attitude3);
		navball = new TutorialPage("navball");
		navball.windowTitle = Localizer.Format("#autoLOC_317587");
		navball.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		navball.dialog = new MultiOptionDialog(navball.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317594"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(64f, 32f), Vector2.zero, XKCDColors.ElectricLime, maneuverVectors, new Rect(0f, 0.666f, 0.666f, 0.333f)), new DialogGUILabel(Localizer.Format("#autoLOC_317598"), expandW: false, expandH: true), new DialogGUILayoutEnd(), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(64f, 32f), Vector2.zero, XKCDColors.TiffanyBlue, maneuverVectors, new Rect(0f, 0.333f, 0.666f, 0.333f)), new DialogGUILabel(Localizer.Format("#autoLOC_317603"), expandW: false, expandH: true), new DialogGUILayoutEnd(), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(64f, 32f), Vector2.zero, XKCDColors.StrongPink, maneuverVectors, new Rect(0f, 0f, 0.666f, 0.333f)), new DialogGUILabel(Localizer.Format("#autoLOC_317608"), expandW: false, expandH: true), new DialogGUILayoutEnd(), new DialogGUIButton(Localizer.Format("#autoLOC_317611"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(navball);
		waitForMap1 = new TutorialPage("waitForMap1");
		waitForMap1.windowTitle = Localizer.Format("#autoLOC_317622");
		waitForMap1.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_sigh, 0f);
		};
		waitForMap1.dialog = new MultiOptionDialog(waitForMap1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317629"), expandW: false, expandH: true));
		waitForMap1.SetAdvanceCondition((KFSMState st) => MapView.MapIsEnabled);
		Tutorial.AddPage(waitForMap1);
		raiseAp1 = new TutorialPage("raiseAp1");
		raiseAp1.windowTitle = Localizer.Format("#autoLOC_317638");
		raiseAp1.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_nodA, 0f);
		};
		raiseAp1.dialog = new MultiOptionDialog(raiseAp1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317645", tutorialControlColorString, GameSettings.TIME_WARP_INCREASE.name), expandW: false, expandH: true));
		raiseAp1.SetAdvanceCondition((KFSMState st) => TimeWarp.CurrentRate > 1f);
		Tutorial.AddPage(raiseAp1);
		waitForPeriapsis1 = new TutorialPage("waitForPeriapsis1");
		waitForPeriapsis1.windowTitle = Localizer.Format("#autoLOC_317653");
		waitForPeriapsis1.OnEnter = delegate
		{
			TimeWarp.SetRate(1, instant: false);
		};
		waitForPeriapsis1.dialog = new MultiOptionDialog(waitForPeriapsis1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317660", TimeWarp.fetch.warpRates[1].ToString("0"), tutorialControlColorString, GameSettings.TIME_WARP_INCREASE.name, tutorialControlColorString, GameSettings.TIME_WARP_DECREASE.name), expandW: false, expandH: true));
		waitForPeriapsis1.SetAdvanceCondition(delegate
		{
			double timeToPe = FlightGlobals.ActiveVessel.orbit.timeToPe;
			if ((double)TimeWarp.CurrentRateIndex * 50.0 > timeToPe)
			{
				TimeWarp.SetRate(TimeWarp.CurrentRateIndex - 1, instant: false);
			}
			return timeToPe < 60.0;
		});
		waitForPeriapsis1.onAdvanceConditionMet.OnEvent = delegate
		{
			TimeWarp.SetRate(0, instant: false);
			TimeWarp.fetch.CancelAutoWarp(0);
		};
		Tutorial.AddPage(waitForPeriapsis1);
		raiseAp2 = new TutorialPage("raiseAp2");
		raiseAp2.windowTitle = Localizer.Format("#autoLOC_317679");
		raiseAp2.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_sigh, 0f);
		};
		raiseAp2.dialog = new MultiOptionDialog(raiseAp2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317686", tutorialControlColorString, GameSettings.NAVBALL_TOGGLE.name, tutorialControlColorString, GameSettings.SAS_TOGGLE.name, tutorialHighlightColorString), expandW: false, expandH: true));
		raiseAp2.SetAdvanceCondition((KFSMState st) => NavBallToggle.Instance.ManeuverModeActive);
		Tutorial.AddPage(raiseAp2);
		raiseAp3a = new TutorialPage("raiseAp3a");
		raiseAp3a.windowTitle = Localizer.Format("#autoLOC_317694");
		raiseAp3a.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_nodA, 0f);
		};
		raiseAp3a.dialog = new MultiOptionDialog(raiseAp3a.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317701", tutorialControlColorString, GameSettings.SAS_TOGGLE.name), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUILabel(Localizer.Format("#autoLOC_317703"), expandW: false, expandH: true), new DialogGUIImage(new Vector2(32f, 32f), Vector2.zero, XKCDColors.ElectricLime, maneuverVectors, new Rect(0f, 0.666f, 0.333f, 0.333f)), new DialogGUILayoutEnd(), new DialogGUILabel(Localizer.Format("#autoLOC_317706"), expandW: false, expandH: true));
		raiseAp3a.SetAdvanceCondition((KFSMState st) => Mathf.Acos(Vector3.Dot(FlightGlobals.ActiveVessel.ReferenceTransform.up, FlightGlobals.ActiveVessel.obt_velocity.normalized)) * 57.29578f < 5f);
		Tutorial.AddPage(raiseAp3a);
		raiseAp3b = new TutorialPage("raiseAp3b");
		raiseAp3b.windowTitle = Localizer.Format("#autoLOC_317715");
		raiseAp3b.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_nodA, 0f);
			FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.flag_6] = true;
			InputLockManager.SetControlLock(ControlTypes.STAGING, "Orbit101TutorialLock");
		};
		raiseAp3b.dialog = new MultiOptionDialog(raiseAp3b.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317725", tutorialControlColorString, GameSettings.THROTTLE_FULL.name), expandW: false, expandH: true));
		raiseAp3b.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0f);
		Tutorial.AddPage(raiseAp3b);
		raiseAp4 = new TutorialPage("raiseAp4");
		raiseAp4.windowTitle = Localizer.Format("#autoLOC_317733");
		raiseAp4.OnEnter = delegate
		{
		};
		raiseAp4.dialog = new MultiOptionDialog(raiseAp4.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317740"), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_317743", FlightGlobals.ActiveVessel.orbit.ApA.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true));
		raiseAp4.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.orbit.ApA >= 800000.0);
		Tutorial.AddPage(raiseAp4);
		raiseAp5 = new TutorialPage("raiseAp5");
		raiseAp5.windowTitle = Localizer.Format("#autoLOC_317753");
		raiseAp5.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbUp, 0f);
		};
		raiseAp5.OnUpdate = delegate
		{
			if (Tutorial.TimeAtCurrentState > 6.0 && FlightGlobals.ActiveVessel.ctrlState.mainThrottle != 0f)
			{
				FlightInputHandler.SetNeutralControls();
			}
		};
		raiseAp5.dialog = new MultiOptionDialog(raiseAp5.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317767", tutorialControlColorString, GameSettings.THROTTLE_DOWN.name, tutorialControlColorString, GameSettings.THROTTLE_CUTOFF.name), expandW: false, expandH: true));
		raiseAp5.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.ctrlState.mainThrottle == 0f);
		Tutorial.AddPage(raiseAp5);
		raiseAp6 = new TutorialPage("raiseAp6");
		raiseAp6.windowTitle = Localizer.Format("#autoLOC_317775");
		raiseAp6.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_true_thumbsUp, 0f);
		};
		raiseAp6.dialog = new MultiOptionDialog(raiseAp6.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317782", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317783"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		raiseAp6.SetAdvanceCondition((KFSMState st) => TimeWarp.CurrentRate > 1f);
		Tutorial.AddPage(raiseAp6);
		waitForApoapsis1 = new TutorialPage("waitForApoapsis1");
		waitForApoapsis1.windowTitle = Localizer.Format("#autoLOC_317795");
		waitForApoapsis1.OnEnter = delegate
		{
			TimeWarp.SetRate(3, instant: false);
			instructor.PlayEmoteRepeating(instructor.anim_true_nodB, 0f);
		};
		waitForApoapsis1.dialog = new MultiOptionDialog(waitForApoapsis1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317803"), expandW: false, expandH: true));
		waitForApoapsis1.SetAdvanceCondition(delegate
		{
			double timeToAp = FlightGlobals.ActiveVessel.orbit.timeToAp;
			if ((double)TimeWarp.CurrentRateIndex * 50.0 > timeToAp)
			{
				TimeWarp.SetRate(TimeWarp.CurrentRateIndex - 1, instant: false);
			}
			return timeToAp < 60.0;
		});
		waitForApoapsis1.onAdvanceConditionMet.OnEvent = delegate
		{
			TimeWarp.SetRate(0, instant: false);
			TimeWarp.fetch.CancelAutoWarp(0);
		};
		Tutorial.AddPage(waitForApoapsis1);
		raisePe1 = new TutorialPage("raisePe1");
		raisePe1.windowTitle = Localizer.Format("#autoLOC_317822");
		raisePe1.OnEnter = delegate
		{
		};
		raisePe1.dialog = new MultiOptionDialog(raisePe1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317829"), expandW: false, expandH: true));
		raisePe1.SetAdvanceCondition((KFSMState st) => FlightInputHandler.state.mainThrottle > 0f);
		Tutorial.AddPage(raisePe1);
		raisePe2 = new TutorialPage("raisePe2");
		raisePe2.windowTitle = Localizer.Format("#autoLOC_317837");
		raisePe2.OnEnter = delegate
		{
		};
		raisePe2.dialog = new MultiOptionDialog(raisePe2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317844"), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_317847", FlightGlobals.ActiveVessel.orbit.PeA.ToString("N0"), FlightGlobals.ActiveVessel.orbit.eccentricity.ToString("0.000")), skin.customStyles[1], expandW: false, expandH: true));
		raisePe2.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.orbit.PeA >= 750000.0);
		Tutorial.AddPage(raisePe2);
		raisePe3 = new TutorialPage("raisePe3");
		raisePe3.windowTitle = Localizer.Format("#autoLOC_317856");
		raisePe3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		raisePe3.OnUpdate = delegate
		{
			if (Tutorial.TimeAtCurrentState > 5.0 && FlightInputHandler.state.mainThrottle != 0f)
			{
				FlightInputHandler.SetNeutralControls();
			}
		};
		raisePe3.dialog = new MultiOptionDialog(raisePe3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317870", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317871"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(raisePe3);
		incChange1 = new TutorialPage("incChange1");
		incChange1.windowTitle = Localizer.Format("#autoLOC_317882");
		incChange1.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_sigh);
		};
		incChange1.dialog = new MultiOptionDialog(incChange1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317889"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317890"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(incChange1);
		NormalVectors = new TutorialPage("NormalVectors");
		NormalVectors.windowTitle = Localizer.Format("#autoLOC_317901");
		NormalVectors.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle);
		};
		NormalVectors.dialog = new MultiOptionDialog(NormalVectors.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317908"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUILabel(Localizer.Format("#autoLOC_317910"), expandW: false, expandH: true), new DialogGUIImage(new Vector2(32f, 32f), Vector2.zero, XKCDColors.StrongPink, maneuverVectors, new Rect(0f, 0f, 0.333f, 0.333f)), new DialogGUILayoutEnd(), new DialogGUILabel(""));
		NormalVectors.SetAdvanceCondition((KFSMState st) => FlightInputHandler.state.mainThrottle > 0f);
		Tutorial.AddPage(NormalVectors);
		incChange2 = new TutorialPage("incChange2");
		incChange2.windowTitle = Localizer.Format("#autoLOC_317921");
		incChange2.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_sigh);
		};
		incChange2.dialog = new MultiOptionDialog(incChange2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317928"), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_317931", FlightGlobals.ActiveVessel.orbit.inclination.ToString("0.0")), skin.customStyles[1], expandW: false, expandH: true));
		incChange2.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.orbit.inclination >= 9.9);
		Tutorial.AddPage(incChange2);
		incChange3 = new TutorialPage("incChange3");
		incChange3.windowTitle = Localizer.Format("#autoLOC_317940");
		incChange3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_sigh);
		};
		incChange3.OnUpdate = delegate
		{
			if (Tutorial.TimeAtCurrentState > 5.0 && FlightInputHandler.state.mainThrottle != 0f)
			{
				FlightInputHandler.SetNeutralControls();
			}
		};
		incChange3.dialog = new MultiOptionDialog(incChange3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317954"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_317955"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(incChange3);
		lowerPe1 = new TutorialPage("lowerPe1");
		lowerPe1.windowTitle = Localizer.Format("#autoLOC_317966");
		lowerPe1.OnEnter = delegate
		{
		};
		lowerPe1.dialog = new MultiOptionDialog(lowerPe1.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317973"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUILabel(Localizer.Format("#autoLOC_317975"), expandW: false, expandH: true), new DialogGUIImage(new Vector2(32f, 32f), Vector2.zero, XKCDColors.ElectricLime, maneuverVectors, new Rect(0.333f, 0.666f, 0.333f, 0.333f)), new DialogGUILayoutEnd());
		lowerPe1.SetAdvanceCondition((KFSMState st) => FlightInputHandler.state.mainThrottle > 0f);
		Tutorial.AddPage(lowerPe1);
		lowerPe2 = new TutorialPage("lowerPe2");
		lowerPe2.windowTitle = Localizer.Format("#autoLOC_317985");
		lowerPe2.OnEnter = delegate
		{
		};
		lowerPe2.dialog = new MultiOptionDialog(lowerPe2.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_317992", tutorialControlColorString, GameSettings.THROTTLE_DOWN.name, tutorialControlColorString, GameSettings.THROTTLE_UP.name), expandW: false, expandH: true), new DialogGUILabel(() => Localizer.Format("#autoLOC_317995", FlightGlobals.ActiveVessel.orbit.PeA.ToString("N0")), skin.customStyles[1], expandW: false, expandH: true));
		lowerPe2.SetAdvanceCondition((KFSMState st) => FlightGlobals.ActiveVessel.orbit.PeA <= 20000.0);
		Tutorial.AddPage(lowerPe2);
		lowerPe3 = new TutorialPage("lowerPe3");
		lowerPe3.windowTitle = Localizer.Format("#autoLOC_318004");
		lowerPe3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		lowerPe3.dialog = new MultiOptionDialog(lowerPe3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318011"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318012"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(lowerPe3);
		conclusion = new TutorialPage("conclusion");
		conclusion.windowTitle = Localizer.Format("#autoLOC_318023");
		conclusion.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_smileB);
		};
		conclusion.dialog = new MultiOptionDialog(conclusion.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318031"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318032"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(conclusion);
		Tutorial.StartTutorial(welcome);
	}

	public override void OnOnDestroy()
	{
		InputLockManager.RemoveControlLock("Orbit101TutorialLock");
	}

	public void remoteNudge(FlightCtrlState fcst)
	{
		if (Tutorial.TimeAtCurrentState < 3.0)
		{
			fcst.pitch = nudge.x;
			fcst.yaw = nudge.y;
			fcst.roll = nudge.z;
		}
	}
}
