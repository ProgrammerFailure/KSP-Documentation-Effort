using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[Serializable]
public class CameraSmoothFollow : CameraMode
{
	[Space(5f)]
	public float distance;

	public float height;

	[FormerlySerializedAs("viewHeightRatio")]
	public float heightMultiplier;

	[Space(5f)]
	public float heightDamping;

	public float rotationDamping;

	[Space(5f)]
	public bool followVelocity;

	public float velocityDamping;

	private Vector3 m_smoothLastPos;

	private Vector3 m_smoothVelocity;

	private float m_smoothTargetAngle;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraSmoothFollow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetTargetConfig(VPCameraTarget targetConfig)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Reset(Transform self, Transform target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update(Transform self, Transform target, float deltaTime)
	{
		throw null;
	}
}
