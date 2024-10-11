using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Serenity;

public class RoboticControllerWindow : MonoBehaviour
{
	private enum presetButtonMode
	{
		None,
		Sine,
		Square,
		Triangle,
		Saw,
		RevSaw,
		Flat
	}

	public TMP_InputField nameInput;

	public TMP_InputField lengthInput;

	[SerializeField]
	private Button playButton;

	private UIStateImage playButtonImageState;

	[SerializeField]
	private Button directionButton;

	private UIStateImage directionButtonImageState;

	[SerializeField]
	private Button loopButton;

	private UIStateImage loopButtonImageState;

	private UIHoverText loopButtonTextHover;

	[SerializeField]
	private Button resizeLengthModeButton;

	private UIStateImage resizeLengthButtonImageState;

	private UIHoverText resizeLengthModeTextHover;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private Button skipStartButton;

	[SerializeField]
	private Button skipPrevButton;

	[SerializeField]
	private Button skipNextButton;

	[SerializeField]
	private Button skipEndButton;

	[SerializeField]
	private Button openActionGroupsButton;

	[SerializeField]
	private RectTransform openActionGroupsRow;

	[SerializeField]
	private Transform pointInfoFooter;

	[SerializeField]
	private bool showPointInfo;

	[SerializeField]
	private TextMeshProUGUI pointInfoIndexLabel;

	[SerializeField]
	private TMP_InputField pointInfoTimeField;

	[SerializeField]
	private TMP_InputField pointInfoValueField;

	[SerializeField]
	private TextMeshProUGUI pointInfoValueLabel;

	[SerializeField]
	private RectTransform pointValuesHolder;

	[SerializeField]
	private RectTransform pointMultiHolder;

	[SerializeField]
	private TextMeshProUGUI pointMultiLabel;

	[SerializeField]
	private Button axisUp;

	[SerializeField]
	private Button axisDown;

	[SerializeField]
	private Button axisCopy;

	[SerializeField]
	private Button axisPaste;

	[SerializeField]
	private Button axisFlipHorizontal;

	[SerializeField]
	private Button axisFlipVertical;

	[SerializeField]
	private Button axisAlignEnds;

	[SerializeField]
	private Button axisSelectAll;

	[SerializeField]
	private Button axisClampValues;

	[SerializeField]
	private RectTransform presetPanel;

	[SerializeField]
	private Button axisPreset;

	[SerializeField]
	private ToggleGroup presetsGroup;

	[SerializeField]
	private Toggle axisPresetFlat;

	[SerializeField]
	private Toggle axisPresetSine;

	[SerializeField]
	private Toggle axisPresetSquare;

	[SerializeField]
	private Toggle axisPresetTriangle;

	[SerializeField]
	private Toggle axisPresetSaw;

	[SerializeField]
	private Toggle axisPresetRevSaw;

	[SerializeField]
	private Slider axisPresetCycles;

	[SerializeField]
	private Slider axisPresetPhase;

	[SerializeField]
	private TextMeshProUGUI axisPresetCyclesLabel;

	[SerializeField]
	private TextMeshProUGUI axisPresetPhaseLabel;

	[SerializeField]
	private Button pointAdd;

	[SerializeField]
	private Button pointDelete;

	[SerializeField]
	private Button pointValueSharp;

	[SerializeField]
	private Button pointValueSmooth;

	[SerializeField]
	private Button pointClampValue;

	[SerializeField]
	private Slider sequencePlaySpeed;

	[SerializeField]
	private TextMeshProUGUI sequencePlaySpeedLabel;

	[SerializeField]
	private TextMeshProUGUI statusHelpLabel;

	public Transform axesParent;

	public Transform sequencePlayPositionRow;

	[SerializeField]
	private Slider sequencePlayPosition;

	[SerializeField]
	private RectTransform sequencePlayLine;

	[SerializeField]
	private float sequencePlayLineOffsetFlight;

	[SerializeField]
	private float sequencePlayLineOffsetEditor;

	[SerializeField]
	private UIWindow uiWindow;

	private List<RoboticControllerWindowBaseRow> rowControls;

	private Vector2 sequencePlayLineDimensions;

	private CurvePanelPoint selectedPoint;

	private List<CurvePanelPoint> selectedPoints;

	private bool sliderChanging;

	private bool speedSliderChanging;

	private float newLength;

	private bool resizeLengthMaintainTimes;

	private float tempPointInfoTime;

	private float tempPointInfoValue;

	private presetButtonMode selectedPreset;

	public TextMeshProUGUI StatusHelpLabel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsRowExpanded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public RoboticControllerWindowBaseRow ExpandedRow
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[Obsolete("Use ExpandedRow.IsAxis/IsAction - this will be removed in a future release")]
	public bool IsAxisExpanded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete("Use ExpandedRow - this will be removed in a future release")]
	public RoboticControllerWindowAxis ExpandedAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Ready
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public ModuleRoboticController Controller
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RoboticControllerWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RoboticControllerWindow Spawn(ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CollapseOtherRows(uint partId, string fieldName, uint partModuleId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void AddInputFieldLock(string val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RemoveInputfieldLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnControllerChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnWindowAxisChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RebuildAxesControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SaveRowIndexes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddRow(ControlledAxis axis, bool updateUIElements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddRow(ControlledAction action, bool updateUIElements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddRow(RoboticControllerWindowBaseRow newBaseRow, bool updateUIElements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RemoveRow(ControlledBase baseRow, bool updateUIElements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateUIFromController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSequencePlayLineDimensions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnControllerItemsChanged(ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePointInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateExpandedWindowAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnPointSelected(RoboticControllerWindowBaseRow row, List<CurvePanelPoint> points)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnPointDragging(RoboticControllerWindowBaseRow row, List<CurvePanelPoint> points)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearSelectedPresetMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPositionSliderChanged(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPlaySpeedSliderChanged(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCloseClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorStarted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CloseWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnVesselChanged(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWindowMove(RectTransform windowRect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPlayPauseClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDirectionClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLoopClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSkipStartClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSkipEndClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSkipPrevClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSkipNextClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectPointAtTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNameEdited(string newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLengthEdited(string newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnResizeLengthModeClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateResizeLengthStateImage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointInfoEdited(string newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWindowResize(RectTransform rect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisUpClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisDownClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisCopyClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisPasteClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisFlipHorizontalClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisFlipVerticalClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisAlignEndsClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisClampValuesClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointClampValuesClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisSelectAllClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisPresetClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PresetToggleChanged(bool isOn, presetButtonMode mode, float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisPresetFlatClick(bool isOn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisPresetSineClick(bool isOn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisPresetSquareClick(bool isOn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisPresetTriangleClick(bool isOn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisPresetSawClick(bool isOn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAxisPresetRevSawClick(bool isOn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPresetCyclesSliderChanged(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPresetPhaseSliderChanged(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointAddClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointDeleteClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointValueSharpClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointValueSmoothClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOpenActionGroupsClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyTextFieldHasFocus()
	{
		throw null;
	}
}
