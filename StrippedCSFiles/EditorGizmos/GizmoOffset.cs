using System.Runtime.CompilerServices;
using UnityEngine;

namespace EditorGizmos;

public class GizmoOffset : MonoBehaviour
{
	[SerializeField]
	private GizmoOffsetHandle[] handles;

	[SerializeField]
	private LookAtObject centerPivotController;

	[SerializeField]
	private ScreenSpaceObjectScaling ssScaling;

	private float gridSnapInterval;

	private float gridSnapIntervalFine;

	public bool useGrid;

	private Camera refCamera;

	private Transform host;

	private Quaternion rotOffset;

	private Callback<Vector3> onGizmoMove;

	private Callback<Vector3> onGizmoMoved;

	private bool isDragging;

	private Vector3 trfPos0;

	private Vector3 offset0;

	private Vector3 offset;

	private static Space coordSpace;

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

	public float SnapInterval
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

	public Space CoordSpace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GizmoOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GizmoOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GizmoOffset Attach(Transform host, Quaternion rotOffset, Callback<Vector3> onMove, Callback<Vector3> onMoved, Camera refCamera = null)
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
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHandleMoveStart(GizmoOffsetHandle h, Vector3 axis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHandleMove(GizmoOffsetHandle h, Vector3 axis, float distance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHandleMoveEnd(GizmoOffsetHandle h, Vector3 axis, float deltaAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorSnapChanged(bool useSnap)
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
