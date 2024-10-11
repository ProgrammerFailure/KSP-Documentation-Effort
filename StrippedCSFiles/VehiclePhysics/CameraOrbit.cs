using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[Serializable]
public class CameraOrbit : CameraMode
{
	public float distance;

	[Space(5f)]
	public float minVerticalAngle;

	public float maxVerticalAngle;

	public float horizontalSpeed;

	public float verticalSpeed;

	public float orbitDamping;

	[Space(5f)]
	public float minDistance;

	public float maxDistance;

	public float distanceSpeed;

	public float distanceDamping;

	[Space(5f)]
	public string horizontalAxis;

	public string verticalAxis;

	public string distanceAxis;

	private float m_orbitX;

	private float m_orbitY;

	private float m_orbitDistance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetTargetConfig(VPCameraTarget targetConfig)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(Transform self)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update(Transform self, Transform target, float deltaTime)
	{
		throw null;
	}
}
