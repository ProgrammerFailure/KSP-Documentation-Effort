using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using UnityEngine;

namespace Expansions.Serenity;

public class RoboticControllerWindowAction : RoboticControllerWindowBaseRow
{
	[SerializeField]
	private CurvePanel curvePanel;

	private UIHoverText curvePanelTextHover;

	private FloatCurve timesCurve;

	private float floatCurveValue;

	private float headerWidth;

	private float axisWidth;

	private Vector2 headerSize;

	public ControlledAction Action
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
	public RoboticControllerWindowAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RoboticControllerWindowAction Spawn(RoboticControllerWindow window, ControlledAction action, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(RoboticControllerWindow window, ControlledAction action)
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
	private void LoadTimesCurve()
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
	private void OnPointAdded(CurvePanel.CurvePanelPointChangeInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointDeleted(int pointIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointMoved(CurvePanel.CurvePanelPointChangeInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPanelLeftClick()
	{
		throw null;
	}
}
