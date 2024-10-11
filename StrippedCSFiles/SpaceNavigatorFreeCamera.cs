using System.Runtime.CompilerServices;
using UnityEngine;

public class SpaceNavigatorFreeCamera : SpaceNavigatorCamera
{
	private Transform pivot;

	private Transform cam;

	public bool HorizonLock;

	private float pitch;

	private float roll;

	private float yaw;

	private float rollDominance;

	private Quaternion rotation;

	private Vector3 offset;

	private Vector3 translation;

	private Vector3 nextStep;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceNavigatorFreeCamera()
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
}
