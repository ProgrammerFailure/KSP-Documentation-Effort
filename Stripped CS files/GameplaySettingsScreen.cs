using System;
using System.Collections.Generic;
using ns2;
using ns23;
using ns9;
using UnityEngine;

public class GameplaySettingsScreen : DialogGUIVerticalLayout, ISettings
{
	public bool checkForUpdates;

	public bool simInBackground;

	public float maxDtPerFrame;

	public bool enableTemperatureGauges;

	public bool enableThermalHighlights;

	public bool advancedTweakables;

	public bool advancedMessagesApp;

	public bool confirmMessageDeletion;

	public bool extendedBurnIndicator;

	public bool ghostedNavMarkers;

	public bool evaDefaultHelmetOn;

	public bool evaDefaultNeckRingOn;

	public bool evaCheckLadderend;

	public bool doubleClickMouseLook;

	public float camWobbleGainExt;

	public float camWobbleGainInt;

	public int maximumVesselBudget;

	public int[] VesselBudgetOptions = new int[18]
	{
		-1, 0, 25, 50, 100, 150, 250, 400, 600, 800,
		1000, 1500, 2500, 4000, 5000, 6000, 8000, 10000
	};

	public bool enableSpaceCenterCrew;

	public bool dontShowLauncher;

	public bool showVersionNumer;

	public bool showVesselLabels;

	public bool useKerbinTime;

	public float uiScale;

	public float uiScale_Time;

	public float uiScale_Altimeter;

	public float uiScale_MapOptions;

	public float uiScale_StagingStack;

	public float uiScale_Apps;

	public float uiScale_Mode;

	public float uiScale_NavBall;

	public float uiScale_Crew;

	public float uiPos_NavBall;

	public void GetSettings()
	{
		showVersionNumer = GameSettings.SHOW_VERSION_WATERMARK;
		showVesselLabels = GameSettings.FLT_VESSEL_LABELS;
		enableSpaceCenterCrew = GameSettings.SHOW_SPACE_CENTER_CREW;
		useKerbinTime = GameSettings.KERBIN_TIME;
		simInBackground = GameSettings.SIMULATE_IN_BACKGROUND;
		maxDtPerFrame = GameSettings.PHYSICS_FRAME_DT_LIMIT;
		checkForUpdates = GameSettings.CHECK_FOR_UPDATES;
		dontShowLauncher = GameSettings.dontShowLauncher;
		Debug.Log("Launcher start settings screen: " + dontShowLauncher);
		uiScale = GameSettings.UI_SCALE;
		uiScale_Time = GameSettings.UI_SCALE_TIME;
		uiScale_Altimeter = GameSettings.UI_SCALE_ALTIMETER;
		uiScale_MapOptions = GameSettings.UI_SCALE_MAPOPTIONS;
		uiScale_Apps = GameSettings.UI_SCALE_APPS;
		uiScale_StagingStack = GameSettings.UI_SCALE_STAGINGSTACK;
		uiScale_Mode = GameSettings.UI_SCALE_MODE;
		uiScale_NavBall = GameSettings.UI_SCALE_NAVBALL;
		uiScale_Crew = GameSettings.UI_SCALE_CREW;
		uiPos_NavBall = GameSettings.UI_POS_NAVBALL;
		maximumVesselBudget = Array.IndexOf(VesselBudgetOptions, GameSettings.MAX_VESSELS_BUDGET);
		if (maximumVesselBudget == -1)
		{
			maximumVesselBudget = 5;
		}
		enableTemperatureGauges = (GameSettings.TEMPERATURE_GAUGES_MODE & 1) > 0;
		enableThermalHighlights = (GameSettings.TEMPERATURE_GAUGES_MODE & 2) > 0;
		advancedTweakables = GameSettings.ADVANCED_TWEAKABLES;
		advancedMessagesApp = GameSettings.ADVANCED_MESSAGESAPP;
		confirmMessageDeletion = GameSettings.CONFIRM_MESSAGE_DELETION;
		extendedBurnIndicator = GameSettings.EXTENDED_BURNTIME;
		ghostedNavMarkers = GameSettings.NAVIGATION_GHOSTING;
		evaDefaultHelmetOn = GameSettings.EVA_DEFAULT_HELMET_ON;
		evaDefaultNeckRingOn = GameSettings.EVA_DEFAULT_NECKRING_ON;
		evaCheckLadderend = GameSettings.EVA_LADDER_CHECK_END;
		doubleClickMouseLook = GameSettings.CAMERA_DOUBLECLICK_MOUSELOOK;
		camWobbleGainExt = GameSettings.CAMERA_FX_EXTERNAL;
		camWobbleGainInt = GameSettings.CAMERA_FX_INTERNAL;
	}

	public void DrawSettings()
	{
	}

	public DialogGUIBase[] DrawMiniSettings()
	{
		TextAnchor achr = TextAnchor.MiddleLeft;
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		list.Add(new DialogGUIBox(Localizer.Format("#autoLOC_146476"), -1f, 18f, null));
		list.Add(new DialogGUIHorizontalLayout(TextAnchor.MiddleLeft, new DialogGUILabel(() => Localizer.Format("#autoLOC_146478"), 150f), new DialogGUIToggle(enableTemperatureGauges, () => (!enableTemperatureGauges) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			enableTemperatureGauges = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146483"), 150f), new DialogGUIToggle(enableThermalHighlights, () => (!enableThermalHighlights) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			enableThermalHighlights = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146488"), 150f), new DialogGUIToggle(doubleClickMouseLook, () => (!doubleClickMouseLook) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			doubleClickMouseLook = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146493", camWobbleGainExt.ToString("0.0")), 150f), new DialogGUISlider(() => camWobbleGainExt, 0f, 2f, wholeNumbers: false, 200f, 20f, delegate(float f)
		{
			camWobbleGainExt = f;
		}), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146498", camWobbleGainInt.ToString("0.0")), 150f), new DialogGUISlider(() => camWobbleGainInt, 0f, 2f, wholeNumbers: false, 200f, 20f, delegate(float f)
		{
			camWobbleGainInt = f;
		}), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146503"), 150f), new DialogGUIToggle(advancedTweakables, () => (!advancedTweakables) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			advancedTweakables = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8012007"), 150f), new DialogGUIToggle(confirmMessageDeletion, () => (!confirmMessageDeletion) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			confirmMessageDeletion = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_6010000"), 150f), new DialogGUIToggle(advancedMessagesApp, () => (!advancedMessagesApp) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			advancedMessagesApp = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8002213"), 150f), new DialogGUIToggle(extendedBurnIndicator, () => (!extendedBurnIndicator) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			extendedBurnIndicator = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8003408"), 150f), new DialogGUIToggle(ghostedNavMarkers, () => (!ghostedNavMarkers) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			ghostedNavMarkers = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8012053"), 150f), new DialogGUIToggle(evaCheckLadderend, () => (!evaCheckLadderend) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			evaCheckLadderend = b;
		}, 150f), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIBox(Localizer.Format("#autoLOC_146507"), -1f, 18f, null));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => (!(uiScale <= UIMasterController.Instance.GetMaxSuggestedUIScale())) ? ("<color=#FF0000>" + Localizer.Format("#autoLOC_146519", (Mathf.Round(uiScale * 10f) / 10f * 100f).ToString("F0")) + "</color>") : Localizer.Format("#autoLOC_146519", (Mathf.Round(uiScale * 10f) / 10f * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale * 10f, 8f, 20f, wholeNumbers: true, 200f, 20f, delegate(float f)
		{
			uiScale = f / 10f;
		}), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUILabel(() => Localizer.Format("#autoLOC_146522"), expandW: true));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146525", Mathf.Round(uiScale_Apps * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale_Apps, 0.5f, 1.5f, wholeNumbers: false, 200f, 20f, delegate(float f)
		{
			uiScale_Apps = f;
		}), new DialogGUIFlexibleSpace()));
		if (HighLogic.LoadedScene == GameScenes.FLIGHT)
		{
			list.Add(new DialogGUIBox(Localizer.Format("#autoLOC_146532"), -1f, 18f, null));
			list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146534", Mathf.Round(uiScale_Time * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale_Time, 0.5f, 1.5f, wholeNumbers: false, 200f, 20f, delegate(float f)
			{
				uiScale_Time = ApplyUIElementScale(FlightUIElements.TIME, f);
			}), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146538", Mathf.Round(uiScale_Altimeter * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale_Altimeter, 0.5f, 1.5f, wholeNumbers: false, 200f, 20f, delegate(float f)
			{
				uiScale_Altimeter = ApplyUIElementScale(FlightUIElements.ALTIMITER, f);
			}), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146542", Mathf.Round(uiScale_MapOptions * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale_MapOptions, 0.5f, 1.5f, wholeNumbers: false, 200f, 20f, delegate(float f)
			{
				uiScale_MapOptions = ApplyUIElementScale(FlightUIElements.MAPFILTER, f);
			}), new DialogGUIFlexibleSpace()));
			if (!GameSettings.UI_SCALE_DISABLEDMODEANDSTAGE)
			{
				list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146548", Mathf.Round(uiScale_StagingStack * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale_StagingStack, 0.5f, 1.5f, wholeNumbers: false, 200f, 20f, delegate(float f)
				{
					uiScale_StagingStack = ApplyUIElementScale(FlightUIElements.STAGING, f);
				}), new DialogGUIFlexibleSpace()));
				list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146552", Mathf.Round(uiScale_Mode * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale_Mode, 0.5f, 1.5f, wholeNumbers: false, 200f, 20f, delegate(float f)
				{
					uiScale_Mode = ApplyUIElementScale(FlightUIElements.MODE, f);
				}), new DialogGUIFlexibleSpace()));
			}
			list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146557", Mathf.Round(uiScale_Crew * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale_Crew, 0.5f, 1.5f, wholeNumbers: false, 200f, 20f, delegate(float f)
			{
				uiScale_Crew = ApplyUIElementScale(FlightUIElements.CREW, f);
			}), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146562", Mathf.Round(uiScale_NavBall * 100f).ToString("F0")), 150f), new DialogGUISlider(() => uiScale_NavBall, 0.5f, 1.5f, wholeNumbers: false, 200f, 20f, delegate(float f)
			{
				uiScale_NavBall = ApplyUIElementScale(FlightUIElements.NAVBALL, f);
			}), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_146566"), 150f), new DialogGUISlider(() => uiPos_NavBall, -1f, 1f, wholeNumbers: false, 200f, 20f, delegate(float f)
			{
				uiPos_NavBall = ApplyNavBallHPos(f);
			}), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(true, false, new DialogGUIButton(Localizer.Format("#autoLOC_146573"), ResetUIScaling, dismissOnSelect: false)));
			list.Add(new DialogGUIHorizontalLayout(true, false, new DialogGUIButton(Localizer.Format("#autoLOC_146574"), ResetUIAdjustments, dismissOnSelect: false)));
		}
		return list.ToArray();
	}

	public void ResetUIScaling()
	{
		uiScale_Time = 1f;
		uiScale_Altimeter = 1f;
		uiScale_MapOptions = 1f;
		uiScale_Apps = 1f;
		uiScale_StagingStack = 1f;
		uiScale_Mode = 1f;
		uiScale_NavBall = 1f;
		uiScale_Crew = 1f;
	}

	public void ResetUIAdjustments()
	{
		uiPos_NavBall = 0f;
	}

	public void ApplySettings()
	{
		GameSettings.SIMULATE_IN_BACKGROUND = simInBackground;
		GameSettings.PHYSICS_FRAME_DT_LIMIT = maxDtPerFrame;
		GameSettings.CHECK_FOR_UPDATES = checkForUpdates;
		GameSettings.FLT_VESSEL_LABELS = showVesselLabels;
		GameSettings.SHOW_SPACE_CENTER_CREW = enableSpaceCenterCrew;
		GameSettings.SHOW_VERSION_WATERMARK = showVersionNumer;
		GameSettings.MAX_VESSELS_BUDGET = VesselBudgetOptions[maximumVesselBudget];
		GameSettings.UI_SCALE = Mathf.Round(uiScale * 10f) / 10f;
		if (UIMasterController.Instance != null)
		{
			UIMasterController.Instance.SetScale(GameSettings.UI_SCALE);
		}
		GameSettings.UI_SCALE_TIME = ApplyUIElementScale(FlightUIElements.TIME, uiScale_Time);
		GameSettings.UI_SCALE_ALTIMETER = ApplyUIElementScale(FlightUIElements.ALTIMITER, uiScale_Altimeter);
		GameSettings.UI_SCALE_MAPOPTIONS = ApplyUIElementScale(FlightUIElements.MAPFILTER, uiScale_MapOptions);
		GameSettings.UI_SCALE_APPS = Mathf.Round(uiScale_Apps * 100f) / 100f;
		UIMasterController.Instance.SetAppScale(GameSettings.UI_SCALE_APPS * uiScale);
		if (!GameSettings.UI_SCALE_DISABLEDMODEANDSTAGE)
		{
			GameSettings.UI_SCALE_STAGINGSTACK = ApplyUIElementScale(FlightUIElements.STAGING, uiScale_StagingStack);
			GameSettings.UI_SCALE_MODE = ApplyUIElementScale(FlightUIElements.MODE, uiScale_Mode);
		}
		GameSettings.UI_SCALE_NAVBALL = ApplyUIElementScale(FlightUIElements.NAVBALL, uiScale_NavBall);
		GameSettings.UI_SCALE_CREW = ApplyUIElementScale(FlightUIElements.CREW, uiScale_Crew);
		GameSettings.UI_POS_NAVBALL = ApplyNavBallHPos(uiPos_NavBall);
		GameSettings.KERBIN_TIME = useKerbinTime;
		if (UIMasterController.Instance.mainCanvas.renderMode != RenderMode.WorldSpace)
		{
			GameEvents.onUIScaleChange.Fire();
			MapViewCanvasUtil.ResetNodeCanvasContainerScale(uiScale);
		}
		GameSettings.dontShowLauncher = dontShowLauncher;
		Application.runInBackground = GameSettings.SIMULATE_IN_BACKGROUND;
		Time.maximumDeltaTime = maxDtPerFrame;
		GameSettings.TEMPERATURE_GAUGES_MODE = Convert.ToInt32(enableTemperatureGauges) | (Convert.ToInt32(enableThermalHighlights) << 1);
		GameSettings.ADVANCED_TWEAKABLES = advancedTweakables;
		GameSettings.ADVANCED_MESSAGESAPP = advancedMessagesApp;
		GameSettings.CONFIRM_MESSAGE_DELETION = confirmMessageDeletion;
		GameSettings.EXTENDED_BURNTIME = extendedBurnIndicator;
		GameSettings.NAVIGATION_GHOSTING = ghostedNavMarkers;
		GameSettings.EVA_DEFAULT_HELMET_ON = evaDefaultHelmetOn;
		GameSettings.EVA_DEFAULT_NECKRING_ON = evaDefaultNeckRingOn;
		GameSettings.EVA_LADDER_CHECK_END = evaCheckLadderend;
		GameSettings.CAMERA_DOUBLECLICK_MOUSELOOK = doubleClickMouseLook;
		GameSettings.CAMERA_FX_EXTERNAL = camWobbleGainExt;
		GameSettings.CAMERA_FX_INTERNAL = camWobbleGainInt;
	}

	public void ApplyUIScalingAndAdjustments()
	{
		ApplyUIElementScale(FlightUIElements.TIME, GameSettings.UI_SCALE_TIME);
		ApplyUIElementScale(FlightUIElements.ALTIMITER, GameSettings.UI_SCALE_ALTIMETER);
		ApplyUIElementScale(FlightUIElements.MAPFILTER, GameSettings.UI_SCALE_MAPOPTIONS);
		ApplyUIElementScale(FlightUIElements.APPS, GameSettings.UI_SCALE_APPS);
		if (!GameSettings.UI_SCALE_DISABLEDMODEANDSTAGE)
		{
			ApplyUIElementScale(FlightUIElements.STAGING, GameSettings.UI_SCALE_STAGINGSTACK);
			ApplyUIElementScale(FlightUIElements.MODE, GameSettings.UI_SCALE_MODE);
		}
		ApplyUIElementScale(FlightUIElements.NAVBALL, GameSettings.UI_SCALE_NAVBALL);
		ApplyUIElementScale(FlightUIElements.CREW, GameSettings.UI_SCALE_CREW);
		ApplyNavBallHPos(GameSettings.UI_POS_NAVBALL);
	}

	public float ApplyUIElementScale(FlightUIElements e, float scale)
	{
		float num = Mathf.Round(scale * 100f) / 100f;
		if (FlightUIModeController.Instance != null)
		{
			FlightUIModeController.Instance.SetUIElementScale(e, num);
		}
		return num;
	}

	public float ApplyNavBallHPos(float pos)
	{
		float num = Mathf.Round(pos * 100f) / 100f;
		if (FlightUIModeController.Instance != null)
		{
			FlightUIModeController.Instance.SetNavBallHPos(num);
		}
		return num;
	}

	public string GetName()
	{
		return "General";
	}

	public new void OnUpdate()
	{
		Update();
	}
}
