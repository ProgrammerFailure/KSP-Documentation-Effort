using EdyCommonTools;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

public class VPVehicleInertia : VehicleBehaviour
{
	public bool visualize = true;

	public bool showLabel;

	[FormerlySerializedAs("chassisColliders")]
	public Collider[] inertiaColliders = new Collider[0];

	public Vector3 m_labelPosition = Vector3.zero;

	public override void OnEnableVehicle()
	{
		Inertia.ApplyInertiaFromColliders(base.vehicle.cachedRigidbody, inertiaColliders);
		Inertia.VerifyInertiaAndShowWarning(base.vehicle.cachedRigidbody);
	}

	public override void OnDisableVehicle()
	{
		base.vehicle.cachedRigidbody.ResetInertiaTensor();
	}

	public override void UpdateVehicle()
	{
		m_labelPosition = base.vehicle.cachedTransform.TransformPoint(base.vehicle.cachedRigidbody.centerOfMass);
	}

	public void OnDrawGizmos()
	{
		if (base.isActiveAndEnabled && visualize)
		{
			ColliderUtility.DrawColliderGizmos(inertiaColliders, Inertia.inertiaGizmosColor, includeInactiveInHierarchy: false, includeNonConvex: false);
		}
	}
}
