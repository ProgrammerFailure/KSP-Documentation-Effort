using UnityEngine;

namespace EdyCommonTools;

public static class RigidbodyUtility
{
	public static float GetKineticEnergy(Rigidbody rb)
	{
		return GetLinearKineticEnergy(rb) + GetAngularKineticEnergy(rb);
	}

	public static float GetNormalizedKineticEnergy(Rigidbody rb)
	{
		return GetNormalizedLinearKineticEnergy(rb) + GetNormalizedAngularKineticEnergy(rb);
	}

	public static float GetLinearKineticEnergy(Rigidbody rb)
	{
		float magnitude = rb.velocity.magnitude;
		float mass = rb.mass;
		return 0.5f * mass * magnitude * magnitude;
	}

	public static float GetAngularKineticEnergy(Rigidbody rb)
	{
		Vector3 inertiaTensor = rb.inertiaTensor;
		Vector3 angularVelocity = rb.angularVelocity;
		return 0.5f * (inertiaTensor.x * angularVelocity.x * angularVelocity.x + inertiaTensor.y * angularVelocity.y * angularVelocity.y + inertiaTensor.z * angularVelocity.z * angularVelocity.z);
	}

	public static float GetNormalizedLinearKineticEnergy(Rigidbody rb)
	{
		float magnitude = rb.velocity.magnitude;
		return 0.5f * magnitude * magnitude;
	}

	public static float GetNormalizedAngularKineticEnergy(Rigidbody rb)
	{
		Vector3 angularVelocity = rb.angularVelocity;
		return 0.5f * (angularVelocity.x * angularVelocity.x + angularVelocity.y * angularVelocity.y + angularVelocity.z * angularVelocity.z);
	}
}
