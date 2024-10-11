using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using VehiclePhysics;

public class KSPWheelController : VehicleBase
{
	[Header("KSP Wheel Controller")]
	public VPWheelCollider wheelCollider;

	public TireFriction tireFriction;

	public float maxDriveTorque;

	public float maxBrakeTorque;

	public float maxBrakeTorqueRest;

	public float maxSteerAngle;

	public float maxRpm;

	public float wheelDamping;

	private new Rigidbody cachedRigidbody;

	public Rigidbody tgtRb;

	public bool keyboardSteering;

	public bool keyboardDrive;

	public bool keyboardBrakes;

	[Range(-1f, 1f)]
	public float driveInput;

	[Range(0f, 1f)]
	public float brakeInput;

	[Range(-1f, 1f)]
	public float steerInput;

	[Range(0.1f, 100f)]
	public float steeringResponse;

	[Range(0.1f, 100f)]
	public float driveResponse;

	[Range(0.1f, 100f)]
	public float brakeResponse;

	public float driveState;

	public float steerState;

	public float brakeState;

	public Transform wcTransform;

	protected DirectDrive m_directDrive;

	private bool init;

	public Func<WheelState, bool> OnShouldIgnoreForces;

	public Rigidbody rb
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsGrounded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public Vector3 SuspensionAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Rigidbody RbTgt
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public WheelState currentState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float LegacyWheelLoad
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 WheelCenter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float WheelRadius
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 WheelDown
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 WheelBase
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPWheelController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KSPWheelController Create(Rigidbody rb, GameObject host, GameObject wheelColliderHost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetRigidbodyTarget(Rigidbody tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInitialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DoUpdateBlocks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetWheelTransform(Vector3 position, Quaternion rotation)
	{
		throw null;
	}
}
