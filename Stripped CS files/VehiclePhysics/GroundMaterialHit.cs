using UnityEngine;

namespace VehiclePhysics;

public struct GroundMaterialHit
{
	public PhysicMaterial physicMaterial;

	public Collider collider;

	public Vector3 hitPoint;
}
