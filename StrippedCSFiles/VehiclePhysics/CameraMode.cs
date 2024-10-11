using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class CameraMode
{
	public KeyCode hotKey;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetTargetConfig(VPCameraTarget targetConfig)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Initialize(Transform self)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnEnable(Transform self)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Reset(Transform self, Transform target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update(Transform self, Transform target, float deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetPose(Transform self, Vector3 position, Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDisable(Transform self)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetInputForAxis(string axisName)
	{
		throw null;
	}
}
