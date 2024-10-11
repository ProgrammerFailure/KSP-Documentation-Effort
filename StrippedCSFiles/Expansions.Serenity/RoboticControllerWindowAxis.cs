using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Serenity;

public class RoboticControllerWindowAxis : RoboticControllerWindowBaseRow
{
	public RectTransform axisLimits;

	private bool showLimitsEditorWidget;

	private IAxisFieldLimits fieldLimits;

	[SerializeField]
	private CurvePanel curvePanel;

	private UIHoverText curvePanelTextHover;

	[SerializeField]
	private AxisLimitLine axisMinLimit;

	private UIHoverText axisMinLimitTextHover;

	[SerializeField]
	private AxisLimitLine axisMaxLimit;

	private UIHoverText axisMaxLimitTextHover;

	[SerializeField]
	private float axisMinMaxDeadzone;

	[SerializeField]
	private float clampSpace;

	private bool normalized;

	private float headerWidth;

	private float axisWidth;

	private Vector2 headerSize;

	private Vector2 moveAxisCache;

	public ControlledAxis Axis
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
	public RoboticControllerWindowAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RoboticControllerWindowAxis Spawn(RoboticControllerWindow window, ControlledAxis axis, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(RoboticControllerWindow window, ControlledAxis axis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRowStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRowDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRowCollapsed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRowExpanded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdateUILayout(bool recreateLine = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void RedrawCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void ReloadCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLayoutElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowPoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HidePoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void InsertPoint(float timeValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ReverseCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InvertCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AlignCurveEnds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClampAllPointValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClampPointValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetFlat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetSine(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetSquare(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetTriangle(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetSaw(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetRevSaw(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePreset(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ClearPresetRefs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SelectAllPoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SelectPointAtTime(float timeValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnPointSelectionChanged(CurvePanel panel, List<CurvePanelPoint> points)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnPointDragging(List<CurvePanelPoint> points)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPanelLeftClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLimitsChanged(AxisFieldLimit newLimits)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void BeginDragAxisLimit(AxisLimitLine line, PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void EndDragAxisLimit(AxisLimitLine line, PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnDragAxisLimit(AxisLimitLine line, PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MoveAxisLimits(AxisLimitLine.LimitOptions limitType, float newValue)
	{
		throw null;
	}
}
