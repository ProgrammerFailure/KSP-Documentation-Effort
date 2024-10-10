using UnityEngine;

namespace VehiclePhysics;

public abstract class GroundMaterialManagerBase : MonoBehaviour
{
	public GroundMaterialManagerBase()
	{
	}

	public abstract GroundMaterial GetGroundMaterial(VehicleBase vehicle, GroundMaterialHit groundHit);

	public virtual void GetGroundMaterialCached(VehicleBase vehicle, GroundMaterialHit groundHit, ref GroundMaterialHit cachedGroundHit, ref GroundMaterial groundMaterial)
	{
		if (groundHit.physicMaterial != cachedGroundHit.physicMaterial)
		{
			cachedGroundHit = groundHit;
			groundMaterial = GetGroundMaterial(vehicle, groundHit);
		}
	}
}
