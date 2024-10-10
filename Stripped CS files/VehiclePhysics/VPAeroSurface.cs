using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Aerodynamic Surface", 40)]
public class VPAeroSurface : VehicleBehaviour
{
	public float dragCoefficient = 1f;

	public float downforceCoefficient = 1f;

	public bool showDebugLabel;

	public override void FixedUpdateVehicle()
	{
		Rigidbody cachedRigidbody = base.vehicle.cachedRigidbody;
		Vector3 vector = (0f - dragCoefficient) * cachedRigidbody.velocity * cachedRigidbody.velocity.magnitude;
		Vector3 vector2 = (0f - downforceCoefficient) * cachedRigidbody.velocity.sqrMagnitude * base.transform.up;
		cachedRigidbody.AddForceAtPosition(vector * base.vehicle.scaleFactor, base.transform.position);
		cachedRigidbody.AddForceAtPosition(vector2 * base.vehicle.scaleFactor, base.transform.position);
	}
}
