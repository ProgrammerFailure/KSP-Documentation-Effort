using System;
using UnityEngine;

namespace EditorGizmos;

public class GizmoRotateHandle : GizmoHandle
{
	public Camera refCamera;

	public GizmoRotate host;

	[SerializeField]
	public Vector3 axis;

	public Plane plane;

	public Ray ray;

	public Vector3 v0;

	public Vector3 a0;

	public float d;

	public float dAngle;

	public Callback<GizmoRotateHandle, Vector3> onHandleRotateStart;

	public Callback<GizmoRotateHandle, Vector3, float> onHandleRotate;

	public Callback<GizmoRotateHandle, Vector3, float> onHandleRotateEnd;

	[SerializeField]
	public Transform spinner;

	public Quaternion spinnerRot0;

	public void Setup(GizmoRotate host, Callback<GizmoRotateHandle, Vector3> onHandleRotateStart, Callback<GizmoRotateHandle, Vector3, float> onHandleRotate, Callback<GizmoRotateHandle, Vector3, float> onHandleRotateEnd, Camera referenceCamera)
	{
		BaseSetup();
		this.host = host;
		this.onHandleRotateStart = onHandleRotateStart;
		this.onHandleRotate = onHandleRotate;
		this.onHandleRotate = (Callback<GizmoRotateHandle, Vector3, float>)Delegate.Combine(this.onHandleRotate, new Callback<GizmoRotateHandle, Vector3, float>(RotateSpinner));
		this.onHandleRotateEnd = onHandleRotateEnd;
		spinnerRot0 = spinner.localRotation;
		refCamera = referenceCamera;
		plane = default(Plane);
		a0 = axis;
	}

	public override bool CanHover()
	{
		return !host.IsDragging;
	}

	public override void On_MouseEnter()
	{
	}

	public override void On_MouseDown()
	{
		axis = host.transform.rotation * a0;
		plane = GetPlane(axis);
		v0 = GetMousePoint(plane);
		dAngle = 0f;
		onHandleRotateStart(this, axis);
	}

	public override void On_MouseDrag()
	{
		plane = GetPlane(axis);
		dAngle = GetAngleBetweenPoints(GetMousePoint(plane), v0, axis);
		if (host.useAngleSnap)
		{
			dAngle -= dAngle % host.SnapDegrees;
		}
		onHandleRotate(this, axis, dAngle);
	}

	public override void On_MouseUp()
	{
		onHandleRotateEnd(this, axis, dAngle);
		ResetSpinner();
	}

	public override void On_MouseExit()
	{
	}

	public Plane GetPlane(Vector3 normal)
	{
		plane.SetNormalAndPosition(normal * Mathf.Sign(0f - Vector3.Dot(normal, refCamera.transform.forward)), host.transform.position);
		Debug.DrawRay(host.transform.position, plane.normal);
		return plane;
	}

	public Vector3 GetMousePoint(Plane p)
	{
		ray = refCamera.ScreenPointToRay(Input.mousePosition);
		p.Raycast(ray, out d);
		return ray.GetPoint(d);
	}

	public float GetAngleBetweenPoints(Vector3 p, Vector3 p0, Vector3 axis)
	{
		Debug.DrawLine(host.transform.position, p0, Color.grey);
		Debug.DrawLine(host.transform.position, p, Color.green);
		p0 = (host.transform.position - p0).normalized;
		p = (host.transform.position - p).normalized;
		return UtilMath.WrapAround(KSPUtil.BearingDegrees(p0, p, axis), 0f, 360f);
	}

	public void RotateSpinner(GizmoRotateHandle instance, Vector3 axis, float angle)
	{
		if (host.CoordSpace == Space.World)
		{
			ResetSpinner();
			spinner.Rotate(axis, angle, Space.World);
		}
	}

	public void ResetSpinner()
	{
		spinner.localRotation = spinnerRot0;
	}
}
