using System.Runtime.CompilerServices;
using UnityEngine;

namespace EditorGizmos;

public class GizmoRotate : MonoBehaviour
{
	[SerializeField]
	private GizmoRotateHandle[] handles;

	private static Space coordSpace;

	private float gridSnapInterval;

	private float gridSnapIntervalFine;

	public bool useAngleSnap;

	private Camera refCamera;

	private Vector3 pivot;

	private Quaternion rot0;

	private Quaternion hostRot0;

	private Quaternion rotOffset;

	private Transform host;

	private Callback<Quaternion> onGizmoRotate;

	private Callback<Quaternion> onGizmoRotated;

	private bool isDragging;

	[SerializeField]
	private ScreenSpaceObjectScaling ssScaling;

	public Space CoordSpace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float SnapDegrees
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Quaternion HostRot0
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsDragging
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float GetScreenSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool GetMouseOverGizmo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GizmoRotate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GizmoRotate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GizmoRotate Attach(Transform host, Vector3 pivotPoint, Quaternion rotOffset, Callback<Quaternion> onRotate, Callback<Quaternion> onRotated, Camera refCamera = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Detach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHandleRotateStart(GizmoRotateHandle h, Vector3 axis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHandleRotate(GizmoRotateHandle h, Vector3 axis, float dAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHandleRotateEnd(GizmoRotateHandle h, Vector3 axis, float deltaAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onAngleSnapChanged(bool useSnap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCoordSystem(Space space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SnapToGrid()
	{
		throw null;
	}
}
