using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace VehiclePhysics;

[Serializable]
public class CameraLookAt : CameraMode
{
	public float damping = 6f;

	[Space(5f)]
	public bool adjustFov = true;

	public float minFov = 10f;

	public float maxFov = 60f;

	public float fovSpeed = 20f;

	public float fovDamping = 4f;

	[Space(5f)]
	public bool autoFov;

	public float targetRadius = 5f;

	public float targetRadiusSpeed = 5f;

	[Space(5f)]
	public bool adjustNearPlane;

	public float nearPlaneAtMinFov = 1.5f;

	[Space(5f)]
	public string fovAxis = "Mouse ScrollWheel";

	[Space(5f)]
	public bool enableMovement;

	public float movementSpeed = 2f;

	public float movementDamping = 5f;

	public string forwardAxis = "";

	public string sidewaysAxis = "";

	[FormerlySerializedAs("verticalAxis")]
	public string upwardsAxis = "";

	public Camera m_camera;

	public Vector3 m_position;

	public float m_fov;

	public float m_savedFov;

	public float m_savedNearPlane;

	public bool m_vrPresent;

	public override void SetTargetConfig(VPCameraTarget targetConfig)
	{
		targetRadius = targetConfig.targetRadius;
	}

	public override void Initialize(Transform self)
	{
		m_camera = self.GetComponent<Camera>();
		if (m_camera == null)
		{
			m_camera = self.GetComponentInChildren<Camera>();
		}
		m_vrPresent = XRDevice.isPresent;
	}

	public override void OnEnable(Transform self)
	{
		m_position = self.position;
		if (m_camera != null)
		{
			m_fov = m_camera.fieldOfView;
			m_savedFov = m_camera.fieldOfView;
			m_savedNearPlane = m_camera.nearClipPlane;
		}
	}

	public override void Update(Transform self, Transform target, float deltaTime)
	{
		if (enableMovement)
		{
			float num = movementSpeed * deltaTime;
			m_position += CameraMode.GetInputForAxis(forwardAxis) * num * new Vector3(self.forward.x, 0f, self.forward.z).normalized;
			m_position += CameraMode.GetInputForAxis(sidewaysAxis) * num * self.right;
			m_position += CameraMode.GetInputForAxis(upwardsAxis) * num * self.up;
		}
		self.position = Vector3.Lerp(self.position, m_position, movementDamping * deltaTime);
		if (target != null)
		{
			Quaternion b = Quaternion.LookRotation(target.position - self.position);
			self.rotation = Quaternion.Slerp(self.rotation, b, damping * deltaTime);
		}
		if (!(m_camera != null))
		{
			return;
		}
		if (autoFov && target != null)
		{
			targetRadius -= CameraMode.GetInputForAxis(fovAxis) * targetRadiusSpeed;
			if (targetRadius < 0f)
			{
				targetRadius = 0f;
			}
			float x = Vector3.Distance(target.position, self.position);
			m_fov = Mathf.Atan2(targetRadius, x) * 57.29578f;
		}
		else
		{
			m_fov -= CameraMode.GetInputForAxis(fovAxis) * fovSpeed;
		}
		m_fov = Mathf.Clamp(m_fov, minFov, maxFov);
		if (!m_vrPresent)
		{
			if (adjustFov)
			{
				m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_fov, fovDamping * deltaTime);
			}
			if (adjustNearPlane)
			{
				m_camera.nearClipPlane = Mathf.Lerp(m_savedNearPlane, nearPlaneAtMinFov, Mathf.InverseLerp(maxFov, minFov, m_camera.fieldOfView));
			}
		}
	}

	public override void SetPose(Transform self, Vector3 position, Quaternion rotation)
	{
		m_position = position;
		self.position = position;
	}

	public override void OnDisable(Transform self)
	{
		if (m_camera != null && !m_vrPresent)
		{
			m_camera.fieldOfView = m_savedFov;
			m_camera.nearClipPlane = m_savedNearPlane;
		}
	}
}
