using System;
using System.Runtime.CompilerServices;
using FlightUIModeControllerUtil;
using KSP.UI;
using UnityEngine;
using UnityEngine.UI;

public class FlightUIModeController : MonoBehaviour
{
	public class elementScalingDetails
	{
		public bool isUIPanelTransition
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public Vector3 prefabScale
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public Vector3 prefabPosition
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public UIPanelTransition.PanelPosition[] prefabStates
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public float currentScale
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public float currentPos
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public elementScalingDetails(UIPanelTransition panel)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public elementScalingDetails(GameObject o)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public elementScalingDetails()
		{
			throw null;
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

	private FlightUIMode _lastMode;

	private FlightUIMode _mode;

	[HideInInspector]
	public Action finishedAllTransitions;

	public bool disableManNodeEditor;

	public UIModePanelState[] UIPanelStates;

	private UIModeSwitchCallback modeCallback;

	private UIModeCheckCallback modeCheckCallback;

	private int crewPortraitsCount;

	private elementScalingDetails uiElementTime;

	private elementScalingDetails uiElementAltimeter;

	private elementScalingDetails uiElementMapOptions;

	private elementScalingDetails uiElementAppLauncher;

	private elementScalingDetails uiElementStaging;

	private elementScalingDetails uiElementModes;

	private elementScalingDetails uiElementNavBall;

	private elementScalingDetails uiElementCrew;

	private float NavBallHCenter;

	public float NavBallPaddingSASat100Pct;

	public float NavBallPaddingCrewat100Pct;

	private bool nextAlarmShowing;

	[SerializeField]
	private float withoutNextAlarm;

	[SerializeField]
	private float withNextAlarm;

	private Vector2 timeFrameInPosition;

	public FlightUIMode Mode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool NextAlarmShowing
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightUIModeController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddModeSwitchCallback(UIModeSwitchCallback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveModeSwitchCallback(UIModeSwitchCallback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddModeCheckCallback(UIModeCheckCallback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveModeCheckCallback(UIModeCheckCallback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FinishedAllTransitionsCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateHighlighting()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnStagingPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDockingPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnManeuverPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnMapViewEnter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnMapViewExit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnManNodeSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMode(FlightUIMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLastMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeTabState(UIPanelTransition element, TabAction action, bool useCallback = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIElementScale(FlightUIElements e, float scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetUIElementScale_ModeControls(FlightUIElements e, float scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNavBallHPos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNavBallHPos(float PosSlide)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNextAlarmToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NextAlarmToggle(bool immediate)
	{
		throw null;
	}
}
