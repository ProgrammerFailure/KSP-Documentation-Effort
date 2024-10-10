using UnityEngine;

namespace VehiclePhysics;

public class CameraMode
{
	public KeyCode hotKey;

	public virtual void SetTargetConfig(VPCameraTarget targetConfig)
	{
	}

	public virtual void Initialize(Transform self)
	{
	}

	public virtual void OnEnable(Transform self)
	{
	}

	public virtual void Reset(Transform self, Transform target)
	{
	}

	public virtual void Update(Transform self, Transform target, float deltaTime)
	{
	}

	public virtual void SetPose(Transform self, Vector3 position, Quaternion rotation)
	{
	}

	public virtual void OnDisable(Transform self)
	{
	}

	public static float GetInputForAxis(string axisName)
	{
		if (!string.IsNullOrEmpty(axisName))
		{
			return Input.GetAxis(axisName);
		}
		return 0f;
	}
}
