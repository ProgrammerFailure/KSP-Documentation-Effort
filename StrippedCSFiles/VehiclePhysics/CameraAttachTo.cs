using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[Serializable]
public class CameraAttachTo : CameraMode
{
	public Transform attachTarget;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraAttachTo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetTargetConfig(VPCameraTarget targetConfig)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update(Transform self, Transform target, float deltaTime)
	{
		throw null;
	}
}
