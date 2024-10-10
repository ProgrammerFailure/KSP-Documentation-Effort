using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Ground Materials/Ground Material Manager")]
public class VPGroundMaterialManager : GroundMaterialManagerBase
{
	public GroundMaterial[] groundMaterials = new GroundMaterial[0];

	public GroundMaterial fallback = new GroundMaterial();

	public override GroundMaterial GetGroundMaterial(VehicleBase vehicle, GroundMaterialHit groundHit)
	{
		int num = 0;
		int num2 = groundMaterials.Length;
		while (true)
		{
			if (num < num2)
			{
				if (groundMaterials[num].physicMaterial == groundHit.physicMaterial)
				{
					break;
				}
				num++;
				continue;
			}
			return fallback;
		}
		return groundMaterials[num];
	}
}
