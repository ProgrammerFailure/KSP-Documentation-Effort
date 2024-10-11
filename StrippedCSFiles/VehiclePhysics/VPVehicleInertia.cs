using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

public class VPVehicleInertia : VehicleBehaviour
{
	public bool visualize;

	public bool showLabel;

	[FormerlySerializedAs("chassisColliders")]
	public Collider[] inertiaColliders;

	private Vector3 m_labelPosition;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPVehicleInertia()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}
}
