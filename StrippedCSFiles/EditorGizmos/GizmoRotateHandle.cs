using System.Runtime.CompilerServices;
using UnityEngine;

namespace EditorGizmos;

public class GizmoRotateHandle : GizmoHandle
{
	private Camera refCamera;

	private GizmoRotate host;

	[SerializeField]
	private Vector3 axis;

	private Plane plane;

	private Ray ray;

	private Vector3 v0;

	private Vector3 a0;

	private float d;

	private float dAngle;

	private Callback<GizmoRotateHandle, Vector3> onHandleRotateStart;

	private Callback<GizmoRotateHandle, Vector3, float> onHandleRotate;

	private Callback<GizmoRotateHandle, Vector3, float> onHandleRotateEnd;

	[SerializeField]
	private Transform spinner;

	private Quaternion spinnerRot0;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GizmoRotateHandle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(GizmoRotate host, Callback<GizmoRotateHandle, Vector3> onHandleRotateStart, Callback<GizmoRotateHandle, Vector3, float> onHandleRotate, Callback<GizmoRotateHandle, Vector3, float> onHandleRotateEnd, Camera referenceCamera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool CanHover()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void On_MouseEnter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void On_MouseDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void On_MouseDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void On_MouseUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void On_MouseExit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Plane GetPlane(Vector3 normal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetMousePoint(Plane p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetAngleBetweenPoints(Vector3 p, Vector3 p0, Vector3 axis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RotateSpinner(GizmoRotateHandle instance, Vector3 axis, float angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetSpinner()
	{
		throw null;
	}
}
