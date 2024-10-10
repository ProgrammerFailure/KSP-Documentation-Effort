using System;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[Serializable]
public class CameraOrbit : CameraMode
{
	public float distance = 10f;

	[Space(5f)]
	public float minVerticalAngle = -20f;

	public float maxVerticalAngle = 80f;

	public float horizontalSpeed = 5f;

	public float verticalSpeed = 2.5f;

	public float orbitDamping = 4f;

	[Space(5f)]
	public float minDistance = 5f;

	public float maxDistance = 50f;

	public float distanceSpeed = 10f;

	public float distanceDamping = 4f;

	[Space(5f)]
	public string horizontalAxis = "Mouse X";

	public string verticalAxis = "Mouse Y";

	public string distanceAxis = "Mouse ScrollWheel";

	public float m_orbitX;

	public float m_orbitY;

	public float m_orbitDistance;

	public override void SetTargetConfig(VPCameraTarget targetConfig)
	{
		distance = targetConfig.viewDistance;
		minDistance = targetConfig.viewMinDistance;
		minVerticalAngle = targetConfig.viewMinAngle;
	}

	public override void Initialize(Transform self)
	{
		m_orbitDistance = distance;
		Vector3 eulerAngles = self.eulerAngles;
		m_orbitX = eulerAngles.y;
		m_orbitY = MathUtility.ClampAngle(eulerAngles.x);
	}

	public override void Update(Transform self, Transform target, float deltaTime)
	{
		if (!(target == null))
		{
			m_orbitX += CameraMode.GetInputForAxis(horizontalAxis) * horizontalSpeed;
			m_orbitY -= CameraMode.GetInputForAxis(verticalAxis) * verticalSpeed;
			distance -= CameraMode.GetInputForAxis(distanceAxis) * distanceSpeed;
			m_orbitY = Mathf.Clamp(m_orbitY, minVerticalAngle, maxVerticalAngle);
			distance = Mathf.Clamp(distance, minDistance, maxDistance);
			m_orbitDistance = Mathf.Lerp(m_orbitDistance, distance, distanceDamping * deltaTime);
			self.rotation = Quaternion.Slerp(self.rotation, Quaternion.Euler(m_orbitY, m_orbitX, 0f), orbitDamping * deltaTime);
			self.position = target.position + self.rotation * new Vector3(0f, 0f, 0f - m_orbitDistance);
		}
	}
}
