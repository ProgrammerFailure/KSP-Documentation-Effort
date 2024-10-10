using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[Serializable]
public class CameraSmoothFollow : CameraMode
{
	[Space(5f)]
	public float distance = 10f;

	public float height = 5f;

	[FormerlySerializedAs("viewHeightRatio")]
	public float heightMultiplier = 0.5f;

	[Space(5f)]
	public float heightDamping = 2f;

	public float rotationDamping = 3f;

	[Space(5f)]
	public bool followVelocity = true;

	public float velocityDamping = 5f;

	public Vector3 m_smoothLastPos = Vector3.zero;

	public Vector3 m_smoothVelocity = Vector3.zero;

	public float m_smoothTargetAngle;

	public override void SetTargetConfig(VPCameraTarget targetConfig)
	{
		distance = targetConfig.viewDistance;
		height = targetConfig.viewHeight;
		rotationDamping = targetConfig.viewDamping;
	}

	public override void Reset(Transform self, Transform target)
	{
		if (!(target == null))
		{
			m_smoothLastPos = target.position;
			m_smoothVelocity = target.forward * 2f;
			m_smoothTargetAngle = target.eulerAngles.y;
		}
	}

	public override void Update(Transform self, Transform target, float deltaTime)
	{
		if (!(target == null))
		{
			Vector3 b = (target.position - m_smoothLastPos) / deltaTime;
			m_smoothLastPos = target.position;
			b.y = 0f;
			if (b.magnitude > 1f)
			{
				m_smoothVelocity = Vector3.Lerp(m_smoothVelocity, b, velocityDamping * deltaTime);
				m_smoothTargetAngle = Mathf.Atan2(m_smoothVelocity.x, m_smoothVelocity.z) * 57.29578f;
			}
			if (!followVelocity)
			{
				m_smoothTargetAngle = target.eulerAngles.y;
			}
			float b2 = target.position.y + height;
			float y = self.eulerAngles.y;
			float y2 = self.position.y;
			y = Mathf.LerpAngle(y, m_smoothTargetAngle, rotationDamping * deltaTime);
			y2 = Mathf.Lerp(y2, b2, heightDamping * deltaTime);
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			self.position = target.position;
			self.position -= quaternion * Vector3.forward * distance;
			Vector3 position = self.position;
			position.y = y2;
			self.position = position;
			self.LookAt(target.position + Vector3.up * height * heightMultiplier);
		}
	}
}
