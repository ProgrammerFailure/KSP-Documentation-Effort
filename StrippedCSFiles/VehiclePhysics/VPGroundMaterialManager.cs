using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Ground Materials/Ground Material Manager")]
public class VPGroundMaterialManager : GroundMaterialManagerBase
{
	public GroundMaterial[] groundMaterials;

	public GroundMaterial fallback;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPGroundMaterialManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override GroundMaterial GetGroundMaterial(VehicleBase vehicle, GroundMaterialHit groundHit)
	{
		throw null;
	}
}
