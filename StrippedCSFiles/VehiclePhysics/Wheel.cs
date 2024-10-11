using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class Wheel : Block
{
	public TireFriction tireFriction;

	public float radius;

	public float mass;

	private float I;

	private float invI;

	public float load;

	public float grip;

	public Vector2 V;

	public Vector2 Fext;

	private float m_brakeTorque;

	public float L;

	public Vector2 P;

	private float w;

	public float T;

	public Vector2 F;

	public float Tr;

	public bool isResting;

	public float m_forwardSlip;

	public Vector2 m_tireForce;

	public float m_Ty;

	public bool m_isAdherent;

	public Vector2 m_adherentSlip;

	public Vector2 m_adherentForce;

	private float m_inTd;

	private float m_inTb;

	private TireFriction.ContactPatch m_contact;

	public float debug1;

	public float debug2;

	public float debug3;

	public float inertia
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float angularVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float driveTorque
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float brakeTorque
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public TireFriction.ContactPatch contactPatch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Wheel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RecalculateConstants()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float AddBrakeTorque(float brakeTorque)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetBrakeTorque()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RecalculateVars()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void GetState(ref State S)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetSubstepState(State S)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ComputeStateUpstream()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void EvaluateTorqueDownstream()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void GetSubstepDerivative(ref Derivative D)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetState(State S)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float SolveSymmetricConstraint(float Tsum, float T, float Tmax)
	{
		throw null;
	}
}
