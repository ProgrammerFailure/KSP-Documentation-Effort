using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[Serializable]
public class CameraLookAt : CameraMode
{
	public float damping;

	[Space(5f)]
	public bool adjustFov;

	public float minFov;

	public float maxFov;

	public float fovSpeed;

	public float fovDamping;

	[Space(5f)]
	public bool autoFov;

	public float targetRadius;

	public float targetRadiusSpeed;

	[Space(5f)]
	public bool adjustNearPlane;

	public float nearPlaneAtMinFov;

	[Space(5f)]
	public string fovAxis;

	[Space(5f)]
	public bool enableMovement;

	public float movementSpeed;

	public float movementDamping;

	public string forwardAxis;

	public string sidewaysAxis;

	[FormerlySerializedAs("verticalAxis")]
	public string upwardsAxis;

	private Camera m_camera;

	private Vector3 m_position;

	private float m_fov;

	private float m_savedFov;

	private float m_savedNearPlane;

	private bool m_vrPresent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraLookAt()
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
	public override void OnEnable(Transform self)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update(Transform self, Transform target, float deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetPose(Transform self, Vector3 position, Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisable(Transform self)
	{
		throw null;
	}
}
