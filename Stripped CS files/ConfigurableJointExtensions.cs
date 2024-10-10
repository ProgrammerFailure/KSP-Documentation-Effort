using UnityEngine;

public static class ConfigurableJointExtensions
{
	public static void SetTargetRotationLocal(this ConfigurableJoint joint, Quaternion targetLocalRotation, Quaternion startLocalRotation)
	{
		if (joint.configuredInWorldSpace)
		{
			Debug.LogError("SetTargetRotationLocal should not be used with joints that are configured in world space. For world space joints, use SetTargetRotation.", joint);
		}
		joint.SetTargetRotationInternal(targetLocalRotation, startLocalRotation, Space.Self);
	}

	public static void SetTargetRotation(this ConfigurableJoint joint, Quaternion targetWorldRotation, Quaternion startWorldRotation)
	{
		if (!joint.configuredInWorldSpace)
		{
			Debug.LogError("SetTargetRotation must be used with joints that are configured in world space. For local space joints, use SetTargetRotationLocal.", joint);
		}
		joint.SetTargetRotationInternal(targetWorldRotation, startWorldRotation, Space.World);
	}

	public static void SetTargetRotationInternal(this ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation, Space space)
	{
		Vector3 axis = joint.axis;
		Vector3 normalized = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, axis).normalized;
		Quaternion quaternion = Quaternion.LookRotation(normalized, normalized2);
		Quaternion targetRotation2 = Quaternion.Inverse(quaternion);
		if (space == Space.World)
		{
			targetRotation2 *= startRotation * Quaternion.Inverse(targetRotation);
		}
		else
		{
			targetRotation2 *= Quaternion.Inverse(targetRotation) * startRotation;
		}
		targetRotation2 *= quaternion;
		joint.targetRotation = targetRotation2;
	}

	public static Quaternion GetExternalToJointSpaceRotation(this ConfigurableJoint joint)
	{
		Vector3 axis = joint.axis;
		Vector3 normalized = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, axis).normalized;
		return Quaternion.LookRotation(normalized, normalized2);
	}
}
