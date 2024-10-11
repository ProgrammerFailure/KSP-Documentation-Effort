using System.Runtime.CompilerServices;
using UnityEngine;

public class SpaceNavigatorLocalCamera : SpaceNavigatorCamera
{
	private Transform cam;

	private Vector3 translation;

	private Vector3 upAxis;

	private Quaternion referenceFrame;

	private Quaternion rotation;

	private float pitch;

	private float roll;

	private float yaw;

	private float rollDominance;

	public bool LockRoll;

	public bool useBounds;

	public Bounds bounds;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceNavigatorLocalCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGetControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnCameraUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnCameraWantsControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 clampToBounds(Vector3 rPos)
	{
		throw null;
	}
}
