using System;
using UnityEngine;

namespace VehiclePhysics;

[Serializable]
public class CameraAttachTo : CameraMode
{
	public Transform attachTarget;

	public override void SetTargetConfig(VPCameraTarget targetConfig)
	{
		if (targetConfig.attachToPoint != null && !targetConfig.useCustomCameras)
		{
			attachTarget = targetConfig.attachToPoint;
		}
		else
		{
			attachTarget = null;
		}
	}

	public override void Update(Transform self, Transform target, float deltaTime)
	{
		if (attachTarget != null)
		{
			target = attachTarget;
		}
		if (!(target == null))
		{
			self.position = target.position;
			self.rotation = target.rotation;
		}
	}
}
