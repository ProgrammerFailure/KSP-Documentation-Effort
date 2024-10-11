using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestVehicleController : MonoBehaviour
{
	[Serializable]
	public class Wheel
	{
		public KSPWheelController kwc;

		public float sprungMass;

		public float spring;

		public float damper;

		public float boost;

		public float loadScalarX;

		public float loadScalarY;

		public float Wx;

		public float Wy;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Wheel()
		{
			throw null;
		}
	}

	[SerializeField]
	private Wheel[] wheels;

	[SerializeField]
	private ConfigurableJoint[] joints;

	[SerializeField]
	private Rigidbody payload;

	private Transform Tref;

	[SerializeField]
	[Range(0.001f, 0.999999f)]
	private float massDistribution;

	[SerializeField]
	private float totalMass;

	[SerializeField]
	private float distributedMassSum;

	[SerializeField]
	private float TWLSum;

	[SerializeField]
	private float suspensionSpringRatio;

	[SerializeField]
	private float suspensionDampingRatio;

	[SerializeField]
	[Range(0.001f, 2f)]
	private float suspensionBoostRatio;

	private float WxSum;

	private float WySum;

	private float dMassError;

	private Vector3 CoM;

	[SerializeField]
	private float WheelMass;

	[SerializeField]
	private float PayloadMass;

	[SerializeField]
	private bool ApplyForcesOnParent;

	[SerializeField]
	private Vector3 GravityDirection;

	[SerializeField]
	private float GravityMagnitude;

	private KSPWheelGravity gravity;

	public float XYZspring;

	public float XYZmaxForce;

	public float angularSpring;

	public float angularMaxForce;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVehicleController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateMassDistribution(float dist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SuspensionLoadUpdate(Wheel[] wheels, float totalMass, Vector3 CoM, float Rmin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float DistanceFromCoM(Wheel wheel, Vector3 CoM, Vector3 alongAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SuspensionSpringUpdate(Wheel wheel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float BoostCurve(float b, float f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfigurableJointUpdate(ConfigurableJoint j)
	{
		throw null;
	}
}
