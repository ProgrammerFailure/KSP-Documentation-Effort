using System;
using FlightUIModeControllerUtil;
using Highlighting;
using ns11;
using ns2;
using ns36;
using UnityEngine;
using UnityEngine.UI;

public class FlightUIModeController : MonoBehaviour
{
	public class elementScalingDetails
	{
		public bool isUIPanelTransition
		{
			get
			{
				if (prefabStates != null)
				{
					return prefabStates.Length != 0;
				}
				return false;
			}
		}

		public Vector3 prefabScale { get; set; }

		public Vector3 prefabPosition { get; set; }

		public UIPanelTransition.PanelPosition[] prefabStates { get; set; }

		public float currentScale { get; set; }

		public float currentPos { get; set; }

		public elementScalingDetails(UIPanelTransition panel)
		{
			prefabScale = panel.transform.localScale;
			prefabPosition = panel.transform.position;
			prefabStates = new UIPanelTransition.PanelPosition[panel.states.Length];
			for (int i = 0; i < panel.states.Length; i++)
			{
				prefabStates[i] = new UIPanelTransition.PanelPosition();
				prefabStates[i].name = panel.states[i].name;
				prefabStates[i].position = panel.states[i].position;
			}
			currentScale = 1f;
		}

		public elementScalingDetails(GameObject o)
		{
			prefabScale = o.transform.localScale;
			prefabPosition = o.transform.position;
			currentScale = 1f;
		}

		public elementScalingDetails()
		{
		}
	}

	public static FlightUIModeController Instance;

	public UIButtonToggle stagingButton;

	public UIButtonToggle dockingButton;

	public UIButtonToggle mapModeButton;

	public UIButtonToggle maneuverButton;

	public UIPanelTransition altimeterFrame;

	public UIPanelTransition timeFrame;

	public UIPanelTransition MapOptionsQuadrant;

	public UIPanelTransition stagingQuadrant;

	public UIPanelTransition dockingRotQuadrant;

	public UIPanelTransition dockingLinQuadrant;

	public UIPanelTransition manNodeHandleEditor;

	public UIPanelTransition manNodeEditor;

	public UIPanelTransition crew;

	public UIPanelTransition navBall;

	public UIPanelTransition uiModeFrame;

	public GameObject UIScaleStageManager;

	public GameObject UIScaleModeFrame;

	public GameObject UIFrameCrewPortaits;

	public Button alarmClockButton;

	public FlightUIMode _lastMode;

	public FlightUIMode _mode;

	[HideInInspector]
	public Action finishedAllTransitions;

	public bool disableManNodeEditor;

	public UIModePanelState[] UIPanelStates;

	public UIModeSwitchCallback modeCallback = delegate
	{
	};

	public UIModeCheckCallback modeCheckCallback = (FlightUIMode m) => true;

	public int crewPortraitsCount;

	public elementScalingDetails uiElementTime;

	public elementScalingDetails uiElementAltimeter;

	public elementScalingDetails uiElementMapOptions;

	public elementScalingDetails uiElementAppLauncher;

	public elementScalingDetails uiElementStaging;

	public elementScalingDetails uiElementModes;

	public elementScalingDetails uiElementNavBall;

	public elementScalingDetails uiElementCrew;

	public float NavBallHCenter;

	public float NavBallPaddingSASat100Pct = 60f;

	public float NavBallPaddingCrewat100Pct = 85f;

	public bool nextAlarmShowing;

	[SerializeField]
	public float withoutNextAlarm = -34f;

	[SerializeField]
	public float withNextAlarm = -66f;

	public Vector2 timeFrameInPosition;

	public FlightUIMode Mode => _mode;

	public bool NextAlarmShowing => nextAlarmShowing;

	public void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void AddModeSwitchCallback(UIModeSwitchCallback callback)
	{
		modeCallback = (UIModeSwitchCallback)Delegate.Combine(modeCallback, callback);
	}

	public void RemoveModeSwitchCallback(UIModeSwitchCallback callback)
	{
		modeCallback = (UIModeSwitchCallback)Delegate.Remove(modeCallback, callback);
	}

	public void AddModeCheckCallback(UIModeCheckCallback callback)
	{
		modeCheckCallback = (UIModeCheckCallback)Delegate.Combine(modeCheckCallback, callback);
	}

	public void RemoveModeCheckCallback(UIModeCheckCallback callback)
	{
		modeCheckCallback = (UIModeCheckCallback)Delegate.Remove(modeCheckCallback, callback);
	}

	public void FinishedAllTransitionsCallback()
	{
		GameEvents.OnFlightUIModeChanged.Fire(_mode);
	}

	public void Start()
	{
		if ((bool)stagingButton && (bool)dockingButton && (bool)mapModeButton && (bool)maneuverButton)
		{
			if ((bool)altimeterFrame && (bool)timeFrame && (bool)navBall && (bool)crew && (bool)MapOptionsQuadrant && (bool)stagingQuadrant && (bool)uiModeFrame)
			{
				int num = UIPanelStates.Length;
				while (num-- > 0)
				{
					UIPanelStates[num].controller = this;
				}
				stagingButton.onToggle.AddListener(OnStagingPress);
				dockingButton.onToggle.AddListener(OnDockingPress);
				mapModeButton.onToggle.AddListener(OnMapPress);
				maneuverButton.onToggle.AddListener(OnManeuverPress);
				alarmClockButton.onClick.AddListener(OnNextAlarmToggle);
				GameEvents.OnMapEntered.Add(OnMapViewEnter);
				GameEvents.OnMapExited.Add(OnMapViewExit);
				GameEvents.onManeuverNodeSelected.Add(OnManNodeSelected);
				finishedAllTransitions = (Action)Delegate.Combine(finishedAllTransitions, new Action(FinishedAllTransitionsCallback));
				if (GameSettings.UIELEMENTSCALINGENABLED)
				{
					uiElementTime = new elementScalingDetails(timeFrame);
					uiElementAltimeter = new elementScalingDetails(altimeterFrame);
					uiElementMapOptions = new elementScalingDetails(MapOptionsQuadrant);
					uiElementNavBall = new elementScalingDetails(navBall);
					uiElementCrew = new elementScalingDetails(crew);
					uiElementStaging = new elementScalingDetails(UIScaleStageManager);
					uiElementModes = new elementScalingDetails(UIScaleModeFrame);
					SetUIElementScale(FlightUIElements.TIME, GameSettings.UI_SCALE_TIME);
					SetUIElementScale(FlightUIElements.ALTIMITER, GameSettings.UI_SCALE_ALTIMETER);
					SetUIElementScale(FlightUIElements.MAPFILTER, GameSettings.UI_SCALE_MAPOPTIONS);
					if (!GameSettings.UI_SCALE_DISABLEDMODEANDSTAGE)
					{
						SetUIElementScale(FlightUIElements.STAGING, GameSettings.UI_SCALE_STAGINGSTACK);
						SetUIElementScale(FlightUIElements.MODE, GameSettings.UI_SCALE_MODE);
					}
					SetUIElementScale(FlightUIElements.NAVBALL, GameSettings.UI_SCALE_NAVBALL);
					SetUIElementScale(FlightUIElements.CREW, GameSettings.UI_SCALE_CREW);
					SetUIElementScale(FlightUIElements.APPS, GameSettings.UI_SCALE_APPS);
					StartCoroutine(CallbackUtil.DelayedCallback(5, delegate
					{
						SetNavBallHPos(GameSettings.UI_POS_NAVBALL);
					}));
				}
				Highlighter.HighlighterLimit = GameSettings.PART_HIGHLIGHTER_BRIGHTNESSFACTOR;
				_mode = FlightUIMode.MAPMODE;
				SetMode(FlightUIMode.STAGING);
				_lastMode = FlightUIMode.STAGING;
				UpdateHighlighting();
				nextAlarmShowing = !GameSettings.ALARM_ROW_DISPLAYED_FLIGHT;
				NextAlarmToggle(immediate: true);
			}
			else
			{
				Debug.LogError("[FlightUIModeController ERROR]: Cannot start without references to the UI Sliding Tabs", base.gameObject);
			}
		}
		else
		{
			Debug.LogError("[FlightUIModeController ERROR]: Cannot start without references to the mode buttons", base.gameObject);
		}
	}

	public void OnDestroy()
	{
		alarmClockButton.onClick.RemoveListener(OnNextAlarmToggle);
		GameEvents.OnMapEntered.Remove(OnMapViewEnter);
		GameEvents.OnMapExited.Remove(OnMapViewExit);
		GameEvents.onManeuverNodeSelected.Remove(OnManNodeSelected);
		finishedAllTransitions = (Action)Delegate.Remove(finishedAllTransitions, new Action(FinishedAllTransitionsCallback));
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void UpdateHighlighting()
	{
		if (StageManager.Instance.Visible)
		{
			stagingButton.SetState(on: true);
		}
		else
		{
			stagingButton.SetState(on: false);
		}
		if (_mode == FlightUIMode.DOCKING)
		{
			dockingButton.SetState(on: true);
		}
		else
		{
			dockingButton.SetState(on: false);
		}
		if (MapView.MapIsEnabled)
		{
			mapModeButton.SetState(on: true);
		}
		else
		{
			mapModeButton.SetState(on: false);
		}
		if (_mode != FlightUIMode.MANEUVER_INFO && _mode != FlightUIMode.MANEUVER_EDIT)
		{
			maneuverButton.SetState(on: false);
		}
		else
		{
			maneuverButton.SetState(on: true);
		}
	}

	public void OnStagingPress()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.FLIGHTUIMODE))
		{
			if (Mode == FlightUIMode.MAPMODE)
			{
				SetMode(FlightUIMode.STAGING);
			}
			else
			{
				StageManager.ToggleStageStack();
			}
		}
		else
		{
			SetMode(_mode);
		}
		UpdateHighlighting();
	}

	public void OnDockingPress()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.FLIGHTUIMODE))
		{
			if (Mode == FlightUIMode.DOCKING)
			{
				SetMode(FlightUIMode.STAGING);
			}
			else
			{
				SetMode(FlightUIMode.DOCKING);
			}
		}
		else if (_mode != FlightUIMode.DOCKING)
		{
			SetMode(_mode);
		}
		UpdateHighlighting();
	}

	public void OnMapPress()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.MAP_TOGGLE))
		{
			if (!MapView.MapIsEnabled)
			{
				MapView.EnterMapView();
			}
			else
			{
				MapView.ExitMapView();
			}
		}
		else
		{
			SetMode(_mode);
		}
		UpdateHighlighting();
	}

	public void OnManeuverPress()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.FLIGHTUIMODE))
		{
			if (Mode != FlightUIMode.MANEUVER_INFO && Mode != FlightUIMode.MANEUVER_EDIT)
			{
				SetMode(FlightUIMode.MANEUVER_INFO);
			}
			else
			{
				SetMode(FlightUIMode.STAGING);
			}
		}
		else
		{
			SetMode(_mode);
		}
		UpdateHighlighting();
	}

	public void OnMapViewEnter()
	{
		SetMode(FlightUIMode.MAPMODE);
		SetLastMode();
		ChangeTabState(crew, TabAction.COLLAPSE);
		ChangeTabState(MapOptionsQuadrant, TabAction.COLLAPSE);
		ChangeTabState(altimeterFrame, TabAction.COLLAPSE, useCallback: true);
		UpdateHighlighting();
	}

	public void OnMapViewExit()
	{
		if (_mode == FlightUIMode.MAPMODE)
		{
			if (_lastMode == FlightUIMode.MAPMODE)
			{
				SetMode(_mode);
			}
			else
			{
				SetLastMode();
			}
		}
		ChangeTabState(crew, TabAction.EXPAND);
		ChangeTabState(MapOptionsQuadrant, TabAction.COLLAPSE);
		ChangeTabState(altimeterFrame, TabAction.EXPAND, useCallback: true);
		UpdateHighlighting();
	}

	public void OnManNodeSelected()
	{
		if (!disableManNodeEditor)
		{
			SetMode(FlightUIMode.MANEUVER_EDIT);
		}
	}

	public void Update()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.FLIGHTUIMODE))
		{
			if (GameSettings.UIMODE_STAGING.GetKeyDown())
			{
				OnStagingPress();
			}
			if (GameSettings.UIMODE_DOCKING.GetKeyDown())
			{
				OnDockingPress();
			}
		}
		if (!(KerbalPortraitGallery.Instance != null))
		{
			return;
		}
		if (crewPortraitsCount != KerbalPortraitGallery.Instance.countPortraits)
		{
			StartCoroutine(CallbackUtil.DelayedCallback(2, delegate
			{
				SetNavBallHPos(GameSettings.UI_POS_NAVBALL);
			}));
		}
		crewPortraitsCount = KerbalPortraitGallery.Instance.countPortraits;
	}

	public void SetMode(FlightUIMode mode)
	{
		if (mode == _mode)
		{
			return;
		}
		if (modeCheckCallback.GetInvocationList() is UIModeCheckCallback[] array && array.Length != 0)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				if (!array[i](mode))
				{
					return;
				}
			}
		}
		_lastMode = _mode;
		_mode = mode;
		if (MapView.MapIsEnabled)
		{
			UIPanelStates[(int)mode].crew = TabAction.COLLAPSE;
			UIPanelStates[(int)mode].altimeter = TabAction.COLLAPSE;
			if (_mode != FlightUIMode.MANEUVER_EDIT && _mode != FlightUIMode.MANEUVER_INFO)
			{
				if (_mode == FlightUIMode.DOCKING)
				{
					InputLockManager.ClearControlLocks();
					if (StageManager.Instance.Visible)
					{
						InputLockManager.SetControlLock(ControlTypes.None, "stageDockInMap");
					}
				}
				else if (_mode == FlightUIMode.STAGING)
				{
					InputLockManager.SetControlLock(ControlTypes.None, "stageDockInMap");
				}
			}
			else
			{
				UIPanelStates[(int)mode].MapOptionsQuadrant = TabAction.NO_CHANGE;
			}
		}
		else if (_mode != FlightUIMode.MAPMODE)
		{
			UIPanelStates[(int)mode].crew = TabAction.EXPAND;
			UIPanelStates[(int)mode].MapOptionsQuadrant = TabAction.NO_CHANGE;
			UIPanelStates[(int)mode].altimeter = TabAction.EXPAND;
			InputLockManager.RemoveControlLock("stageDockInMap");
		}
		UIPanelStates[(int)mode].ApplyMode();
		modeCallback(mode);
		if (GameSettings.UIELEMENTSCALINGENABLED)
		{
			SetNavBallHPos();
		}
	}

	public void SetLastMode()
	{
		SetMode(_lastMode);
	}

	public void ChangeTabState(UIPanelTransition element, TabAction action, bool useCallback = false)
	{
		if (useCallback)
		{
			element.Transition(action.TransitionStateName(), Instance.finishedAllTransitions);
		}
		else
		{
			element.Transition(action.TransitionStateName());
		}
	}

	public void SetUIElementScale(FlightUIElements e, float scale)
	{
		if (!GameSettings.UIELEMENTSCALINGENABLED)
		{
			Debug.Log("Element Scaling is Globally Disabled");
			return;
		}
		if (scale > 5f)
		{
			scale = 5f;
		}
		if (scale <= 0f)
		{
			scale = 0f;
		}
		elementScalingDetails elementScalingDetails = null;
		UIPanelTransition uIPanelTransition = null;
		switch (e)
		{
		case FlightUIElements.TIME:
			elementScalingDetails = uiElementTime;
			uIPanelTransition = timeFrame;
			break;
		case FlightUIElements.ALTIMITER:
			elementScalingDetails = uiElementAltimeter;
			uIPanelTransition = altimeterFrame;
			break;
		case FlightUIElements.MAPFILTER:
			elementScalingDetails = uiElementMapOptions;
			uIPanelTransition = MapOptionsQuadrant;
			break;
		case FlightUIElements.APPS:
			if (UIMasterController.Instance != null)
			{
				UIMasterController.Instance.SetAppScale(scale);
			}
			return;
		case FlightUIElements.STAGING:
		case FlightUIElements.MODE:
			if (!GameSettings.UI_SCALE_DISABLEDMODEANDSTAGE)
			{
				SetUIElementScale_ModeControls(e, scale);
				SetNavBallHPos();
			}
			else
			{
				Debug.Log("Element Scale not supported for " + Enum.GetName(typeof(FlightUIElements), e) + " Scaling");
			}
			return;
		case FlightUIElements.NAVBALL:
			elementScalingDetails = uiElementNavBall;
			uIPanelTransition = navBall;
			break;
		case FlightUIElements.CREW:
			elementScalingDetails = uiElementCrew;
			uIPanelTransition = crew;
			break;
		}
		if (!(uIPanelTransition == null) && elementScalingDetails != null)
		{
			elementScalingDetails.currentScale = scale;
			uIPanelTransition.transform.localScale = elementScalingDetails.prefabScale * scale;
			for (int i = 0; i < uIPanelTransition.states.Length; i++)
			{
				uIPanelTransition.states[i].position = elementScalingDetails.prefabStates[i].position * scale;
			}
			uIPanelTransition.panelTransform.anchoredPosition = uIPanelTransition.states[uIPanelTransition.StateIndex].position;
			if (e == FlightUIElements.MODE || e == FlightUIElements.NAVBALL || e == FlightUIElements.CREW)
			{
				SetNavBallHPos();
			}
		}
		else
		{
			Debug.Log("Element Scale not supported for " + Enum.GetName(typeof(FlightUIElements), e) + " Scaling");
		}
	}

	public void SetUIElementScale_ModeControls(FlightUIElements e, float scale)
	{
		if (!GameSettings.UIELEMENTSCALINGENABLED)
		{
			Debug.Log("Element Scaling is Globally Disabled");
		}
		else if (!GameSettings.UI_SCALE_DISABLEDMODEANDSTAGE && !(UIScaleModeFrame == null) && !(UIScaleModeFrame == null) && uiElementModes != null && uiElementStaging != null)
		{
			switch (e)
			{
			case FlightUIElements.MODE:
				uiElementModes.currentScale = scale;
				break;
			case FlightUIElements.STAGING:
				uiElementStaging.currentScale = scale;
				break;
			}
			UIScaleModeFrame.transform.localScale = new Vector3(uiElementModes.prefabScale.x * uiElementModes.currentScale, uiElementModes.prefabScale.y * uiElementModes.currentScale, uiElementModes.prefabScale.z * Mathf.Max(uiElementModes.currentScale, 0.7f));
			UIScaleStageManager.transform.localScale = uiElementModes.prefabScale.x / uiElementModes.currentScale * uiElementStaging.prefabScale * uiElementStaging.currentScale;
		}
		else
		{
			Debug.Log("Element Scale not supported for Staging and Modes");
		}
	}

	public void SetNavBallHPos()
	{
		SetNavBallHPos(uiElementNavBall.currentPos);
	}

	public void SetNavBallHPos(float PosSlide)
	{
		if (!GameSettings.UIELEMENTSCALINGENABLED)
		{
			Debug.Log("Element Scaling is Globally Disabled");
			return;
		}
		float num = Screen.width;
		float value = num / 2f + num / 2f * PosSlide;
		float num2 = uiModeFrame.states[uiModeFrame.StateIndex].position.x * GameSettings.UI_SCALE * uiElementModes.currentScale + ((RectTransform)uiModeFrame.transform).rect.width * uiModeFrame.transform.localScale.x * GameSettings.UI_SCALE * uiElementModes.currentScale;
		float num3 = num;
		if (Mode != FlightUIMode.MAPMODE)
		{
			num3 = num - UIFrameCrewPortaits.transform.localPosition.x * GameSettings.UI_SCALE * uiElementModes.currentScale - ((RectTransform)UIFrameCrewPortaits.transform).rect.width * GameSettings.UI_SCALE * uiElementCrew.currentScale;
		}
		float num4 = navBall.panelTransform.rect.width * GameSettings.UI_SCALE * uiElementNavBall.currentScale / 2f;
		float num5 = NavBallPaddingSASat100Pct * GameSettings.UI_SCALE * uiElementNavBall.currentScale;
		float num6 = NavBallPaddingCrewat100Pct * GameSettings.UI_SCALE * uiElementNavBall.currentScale;
		if (Mode == FlightUIMode.MAPMODE)
		{
			num6 += 40f * GameSettings.UI_SCALE * uiElementNavBall.currentScale;
		}
		value = Mathf.Clamp(value, num2 + num4 + num5, num3 - num4 - num6);
		NavBallHCenter = value - num / 2f;
		uiElementNavBall.currentPos = PosSlide;
		navBall.transform.localPosition = new Vector3(NavBallHCenter / GameSettings.UI_SCALE, navBall.transform.localPosition.y, navBall.transform.localPosition.z);
		for (int i = 0; i < navBall.states.Length; i++)
		{
			navBall.states[i].position = new Vector2(NavBallHCenter / GameSettings.UI_SCALE, navBall.states[i].position.y);
		}
		navBall.panelTransform.anchoredPosition = navBall.states[navBall.StateIndex].position;
	}

	public void OnNextAlarmToggle()
	{
		NextAlarmToggle(immediate: false);
	}

	public void NextAlarmToggle(bool immediate)
	{
		nextAlarmShowing = !nextAlarmShowing;
		GameSettings.ALARM_ROW_DISPLAYED_FLIGHT = nextAlarmShowing;
		if (timeFrame.GetState("In", out var index) == null)
		{
			return;
		}
		timeFrameInPosition = timeFrame.states[index].position;
		if (nextAlarmShowing)
		{
			timeFrameInPosition.y = withNextAlarm;
		}
		else
		{
			timeFrameInPosition.y = withoutNextAlarm;
		}
		timeFrame.states[index].position = timeFrameInPosition;
		if (timeFrame.StateIndex == index)
		{
			if (immediate)
			{
				timeFrame.TransitionImmediate(index);
			}
			else
			{
				timeFrame.Transition(index);
			}
		}
	}
}
