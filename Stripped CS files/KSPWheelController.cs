using System;
using UnityEngine;
using VehiclePhysics;

public class KSPWheelController : VehicleBase
{
	[Header("KSP Wheel Controller")]
	public VPWheelCollider wheelCollider;

	public TireFriction tireFriction = new TireFriction();

	public float maxDriveTorque;

	public float maxBrakeTorque;

	public float maxBrakeTorqueRest = 1000f;

	public float maxSteerAngle;

	public float maxRpm;

	public float wheelDamping = 0.95f;

	public new Rigidbody cachedRigidbody;

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

	public DirectDrive m_directDrive;

	public bool init;

	public Func<WheelState, bool> OnShouldIgnoreForces = (WheelState ws) => true;

	public Rigidbody rb => base.gameObject.GetComponentCached(ref cachedRigidbody);

	public bool IsGrounded
	{
		get
		{
			if (!init)
			{
				return false;
			}
			return base.wheelState[0].grounded;
		}
		set
		{
			if (init)
			{
				base.wheelState[0].grounded = value;
			}
		}
	}

	public Vector3 SuspensionAxis
	{
		get
		{
			if (!init)
			{
				return Vector3.up;
			}
			return base.cachedTransform.up;
		}
	}

	public Rigidbody RbTgt => tgtRb;

	public WheelState currentState
	{
		get
		{
			if (!init)
			{
				return null;
			}
			return base.wheelState[0];
		}
	}

	public float LegacyWheelLoad
	{
		get
		{
			if (!currentState.grounded)
			{
				return 0f;
			}
			return Mathf.Max(0f, currentState.contactDepth * currentState.wheelCol.effectiveSpringRate * (1f / gravity.Magnitude));
		}
	}

	public Vector3 WheelCenter
	{
		get
		{
			if ((bool)wheelCollider && (bool)wheelCollider.cachedTransform)
			{
				Vector3 position = wheelCollider.center + Vector3.up * wheelCollider.suspensionDistance * (1f - wheelCollider.suspensionAnchor);
				return wheelCollider.cachedTransform.TransformPoint(position) + WheelDown * currentState.contactDepth;
			}
			return Vector3.zero;
		}
	}

	public float WheelRadius
	{
		get
		{
			if (!wheelCollider)
			{
				return 0f;
			}
			return wheelCollider.radius;
		}
	}

	public Vector3 WheelDown
	{
		get
		{
			if ((bool)wheelCollider && (bool)wheelCollider.cachedTransform)
			{
				return -wheelCollider.cachedTransform.up;
			}
			return Vector3.zero;
		}
	}

	public Vector3 WheelBase
	{
		get
		{
			if ((bool)wheelCollider && (bool)wheelCollider.cachedTransform)
			{
				return WheelCenter + WheelDown * WheelRadius;
			}
			return Vector3.zero;
		}
	}

	public static KSPWheelController Create(Rigidbody rb, GameObject host, GameObject wheelColliderHost)
	{
		KSPWheelController obj = host.GetComponent<KSPWheelController>() ?? host.AddComponent<KSPWheelController>();
		obj.wheelCollider = wheelColliderHost.GetComponent<VPWheelCollider>() ?? wheelColliderHost.gameObject.AddComponent<VPWheelCollider>();
		obj.wcTransform = wheelColliderHost.transform;
		obj.maxRpm = float.MaxValue;
		obj.maxBrakeTorque = 0f;
		obj.maxDriveTorque = 0f;
		obj.maxSteerAngle = 0f;
		obj.steeringResponse = 0f;
		obj.driveResponse = 0f;
		obj.brakeResponse = 0f;
		return obj;
	}

	public void SetRigidbodyTarget(Rigidbody tgt)
	{
		tgtRb = tgt ?? rb;
	}

	public void Awake()
	{
		if (HighLogic.LoadedSceneIsGame)
		{
			base.enabled = false;
		}
	}

	public void Initialize()
	{
		if (!init)
		{
			OnInitialize();
		}
	}

	public override void OnInitialize()
	{
		tgtRb = rb;
		SetNumberOfWheels(1);
		if (wheelCollider == null)
		{
			Debug.LogError("Missing WheelCollider");
			return;
		}
		base.wheelState[0].wheelCol = wheelCollider;
		base.wheels[0].tireFriction = tireFriction;
		base.wheels[0].radius = wheelCollider.radius;
		base.wheels[0].mass = wheelCollider.mass;
		vehicleSleepCriteria = VehicleSleepCriteria.Relaxed;
		wheelSleepVelocity = 0f;
		m_directDrive = new DirectDrive();
		Block.Connect(base.wheels[0], 0, m_directDrive, 0);
		init = true;
	}

	public override void OnUpdate()
	{
		if (keyboardSteering)
		{
			steerInput = (Input.GetKey(KeyCode.A) ? (-1f) : (Input.GetKey(KeyCode.D) ? 1f : 0f));
		}
		steerState = Mathf.MoveTowards(steerState, steerInput, steeringResponse * Time.deltaTime);
		base.wheelState[0].steerAngle = steerState * maxSteerAngle;
	}

	public override void DoUpdateBlocks()
	{
		if (keyboardDrive)
		{
			driveInput = (Input.GetKey(KeyCode.S) ? (-1f) : (Input.GetKey(KeyCode.W) ? 1f : 0f));
		}
		if (keyboardBrakes)
		{
			brakeInput = (Input.GetKey(KeyCode.Space) ? 1f : 0f);
		}
		driveState = Mathf.MoveTowards(driveState, driveInput, driveResponse * Time.deltaTime);
		brakeState = Mathf.MoveTowards(brakeState, brakeInput, brakeResponse * Time.deltaTime);
		m_directDrive.motorInput = driveState;
		m_directDrive.maxMotorTorque = maxDriveTorque;
		m_directDrive.maxRpm = maxRpm;
		m_directDrive.damping = wheelDamping;
		if (brakeState >= 1f)
		{
			base.wheels[0].AddBrakeTorque(brakeState * maxBrakeTorqueRest);
		}
		else
		{
			base.wheels[0].AddBrakeTorque(brakeState * maxBrakeTorque);
		}
	}

	public void SetWheelTransform(Vector3 position, Quaternion rotation)
	{
		if (wcTransform != null)
		{
			wcTransform.position = position;
			wcTransform.rotation = rotation;
		}
	}
}
