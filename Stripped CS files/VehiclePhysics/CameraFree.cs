using System;
using EdyCommonTools;
using UnityEngine;
using UnityEngine.XR;

namespace VehiclePhysics;

[Serializable]
public class CameraFree : CameraMode
{
	public float minVerticalAngle = -60f;

	public float maxVerticalAngle = 60f;

	public float horizontalSpeed = 4f;

	public float verticalSpeed = 2f;

	public float damping = 4f;

	[Space(5f)]
	public bool adjustFov = true;

	public float minFov = 10f;

	public float maxFov = 60f;

	public float fovSpeed = 20f;

	public float fovDamping = 4f;

	[Space(5f)]
	public bool adjustNearPlane;

	public float nearPlaneAtMinFov = 1.5f;

	[Space(5f)]
	public string horizontalAxis = "Mouse X";

	public string verticalAxis = "Mouse Y";

	public string fovAxis = "Mouse ScrollWheel";

	[Space(5f)]
	public bool enableMovement;

	public float movementSpeed = 2f;

	public float movementDamping = 5f;

	public string forwardAxis = "";

	public string sidewaysAxis = "";

	public string upwardsAxis = "";

	public Camera m_camera;

	public Vector3 m_position;

	public float m_fov;

	public float m_savedFov;

	public float m_savedNearPlane;

	public float m_horizontal;

	public float m_vertical;

	public bool m_vrPresent;

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
		Vector3 eulerAngles = self.eulerAngles;
		m_horizontal = eulerAngles.y;
		m_vertical = MathUtility.ClampAngle(0f - eulerAngles.x);
		if (m_camera != null)
		{
			m_fov = m_camera.fieldOfView;
			m_savedFov = m_camera.fieldOfView;
			m_savedNearPlane = m_camera.nearClipPlane;
		}
	}

	public override void Update(Transform self, Transform target, float deltaTime)
	{
		m_horizontal += CameraMode.GetInputForAxis(horizontalAxis) * horizontalSpeed;
		m_vertical += CameraMode.GetInputForAxis(verticalAxis) * verticalSpeed;
		m_vertical = Mathf.Clamp(m_vertical, minVerticalAngle, maxVerticalAngle);
		if (enableMovement)
		{
			float num = movementSpeed * deltaTime;
			m_position += CameraMode.GetInputForAxis(forwardAxis) * num * new Vector3(self.forward.x, 0f, self.forward.z).normalized;
			m_position += CameraMode.GetInputForAxis(sidewaysAxis) * num * self.right;
			m_position += CameraMode.GetInputForAxis(upwardsAxis) * num * self.up;
		}
		self.position = Vector3.Lerp(self.position, m_position, movementDamping * deltaTime);
		self.rotation = Quaternion.Slerp(self.rotation, Quaternion.Euler(0f - m_vertical, m_horizontal, 0f), damping * deltaTime);
		if (!(m_camera != null))
		{
			return;
		}
		m_fov -= CameraMode.GetInputForAxis(fovAxis) * fovSpeed;
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
		self.rotation = rotation;
		Vector3 eulerAngles = self.eulerAngles;
		m_horizontal = eulerAngles.y;
		m_vertical = MathUtility.ClampAngle(0f - eulerAngles.x);
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
