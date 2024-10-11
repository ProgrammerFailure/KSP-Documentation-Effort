using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Telemetry/Force Cones", 2)]
public class VPForceCones : VehicleBehaviour
{
	private struct Cones
	{
		public Transform red;

		public Transform green;

		public Transform blue;

		public Transform gray;
	}

	public float baseLength;

	public bool showDownforce;

	public bool showTireForce;

	public bool combinedTireForce;

	public bool useLogScale;

	private Cones[] m_wheelCones;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPForceCones()
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
	private Transform CreateConeObject(string name, Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 ScaledForce(float force)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVehicle()
	{
		throw null;
	}
}
