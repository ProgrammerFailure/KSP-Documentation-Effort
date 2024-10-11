using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public abstract class GroundMaterialManagerBase : MonoBehaviour
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	protected GroundMaterialManagerBase()
	{
		throw null;
	}

	public abstract GroundMaterial GetGroundMaterial(VehicleBase vehicle, GroundMaterialHit groundHit);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetGroundMaterialCached(VehicleBase vehicle, GroundMaterialHit groundHit, ref GroundMaterialHit cachedGroundHit, ref GroundMaterial groundMaterial)
	{
		throw null;
	}
}
