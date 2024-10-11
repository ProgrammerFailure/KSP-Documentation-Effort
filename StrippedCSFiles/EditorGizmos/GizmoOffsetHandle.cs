using System.Runtime.CompilerServices;
using UnityEngine;

namespace EditorGizmos;

public class GizmoOffsetHandle : GizmoHandle
{
	[SerializeField]
	private GizmoOffset host;

	[SerializeField]
	private Transform stretchSection;

	private Vector3 handlePos0;

	private Vector3 handleAxis0;

	private Camera refCamera;

	private Transform trf;

	private Vector2 mousePosAtGrabTime;

	private Vector2 mouseScreenAxis;

	private Vector2 screenAxis;

	private float pixel2WorldFactor;

	private float dOffset;

	private Callback<GizmoOffsetHandle, Vector3> onDragStart;

	private Callback<GizmoOffsetHandle, Vector3, float> onDrag;

	private Callback<GizmoOffsetHandle, Vector3, float> onDragEnded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GizmoOffsetHandle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(GizmoOffset host, Callback<GizmoOffsetHandle, Vector3> onHandleDragStart, Callback<GizmoOffsetHandle, Vector3, float> onHandleDrag, Callback<GizmoOffsetHandle, Vector3, float> onHandleDragEnd, Camera referenceCamera)
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
	public bool IsFacingDirection(Vector3 direction)
	{
		throw null;
	}
}
