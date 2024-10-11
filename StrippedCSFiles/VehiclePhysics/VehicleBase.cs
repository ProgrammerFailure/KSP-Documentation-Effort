using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[DisallowMultipleComponent]
public abstract class VehicleBase : MonoBehaviour
{
	public enum VehicleSleepCriteria
	{
		Strict,
		Relaxed
	}

	public class WheelState
	{
		public VPWheelCollider wheelCol;

		public bool steerable;

		public float steerAngle;

		public bool grounded;

		public WheelHit hit;

		public GroundMaterial groundMaterial;

		public GroundMaterialHit lastGroundHit;

		public float contactDepth;

		public float suspensionCompression;

		public float downforce;

		public float normalizedLoad;

		public float contactAngle;

		public float contactSpeed;

		public float damperForce;

		public float lastContactDepth;

		public Vector3 wheelVelocity;

		public Vector3 surfaceForce;

		public Vector2 localWheelVelocity;

		public Vector2 localSurfaceForce;

		public Vector2 externalTireForce;

		public float angularVelocity;

		public Vector2 tireForce;

		public float reactionTorque;

		public Vector2 tireSlip;

		public float combinedTireSlip;

		public Vector2 lastTireForce;

		public float driveTorque;

		public float brakeTorque;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public WheelState()
		{
			throw null;
		}
	}

	[Serializable]
	public struct VehicleStateVars
	{
		public float time;

		public Vector3 lastVelocity;

		public float lastImpactTime;
	}

	[Serializable]
	public struct WheelStateVars
	{
		public float L;

		public float Tr;

		public float contactDepth;

		public Vector2 lastTireForce;
	}

	public struct BlockState
	{
		public float L;

		public float I;

		public float Tr;

		public float Td;
	}

	public struct SolverState
	{
		public int step;

		public float time;

		public BlockState[] blockStates;
	}

	public enum WheelPos
	{
		Default = 0,
		Left = 0,
		Right = 99
	}

	public bool tireSideDeflection;

	public float tireSideDeflectionRate;

	[Range(0f, 1f)]
	public float tireImpulseRatio;

	[Range(0f, 1f)]
	public float wheelSleepVelocity;

	public bool advancedSuspensionDamper;

	public float suspensionDamperLimitFactor;

	public bool contactAngleAffectsTireForce;

	[Range(1f, 20f)]
	public int integrationSteps;

	public bool integrationUseRK4;

	public Transform centerOfMass;

	[HideInInspector]
	public bool accurateSuspensionForces;

	[HideInInspector]
	public float scaleFactor;

	[HideInInspector]
	public VehicleSleepCriteria vehicleSleepCriteria;

	[NonSerialized]
	public bool inhibitWheelSleep;

	[NonSerialized]
	public bool invertVisualWheelSpinDirection;

	public Action onPreDynamicsStep;

	public Action onBeforeUpdateBlocks;

	public Action onBeforeIntegrationStep;

	public Action onPreVisualUpdate;

	public float minimumdownForce;

	private Transform m_transform;

	private Rigidbody m_rigidbody;

	public KSPWheelGravity gravity;

	public bool tireForceSmoothing;

	public bool suspensionForceSmoothing;

	public float tireForceSharpness;

	public float suspensionForceSharpness;

	private float m_time;

	private Solver m_solver;

	private Wheel[] m_wheels;

	private WheelState[] m_wheelState;

	private Vector3 m_localAcceleration;

	private Vector3 m_lastVelocity;

	private float m_speed;

	private float m_speedAngle;

	public DataBus data;

	private Collider[] m_colliders;

	private int[] m_colLayers;

	private bool m_paused;

	private bool m_singleFixedStep;

	private bool m_singleUpdateStep;

	private List<VehicleBehaviour> m_activeBehaviours;

	public bool showContactGizmos;

	[HideInInspector]
	public bool disableContactProcessing;

	public Action onImpact;

	public Action onRawCollision;

	public static VehicleBase vehicle;

	public static Collision currentCollision;

	public static bool isCollisionEnter;

	[NonSerialized]
	public float impactThreeshold;

	[NonSerialized]
	public float impactInterval;

	[NonSerialized]
	public float impactIntervalRandom;

	[NonSerialized]
	public float impactMinSpeed;

	private int m_sumImpactCount;

	private Vector3 m_sumImpactPosition;

	private Vector3 m_sumImpactVelocity;

	private int m_sumImpactHardness;

	private float m_lastImpactTime;

	private Vector3 m_localDragPosition;

	private Vector3 m_localDragVelocity;

	private int m_localDragHardness;

	private GroundMaterial m_impactedGroundMaterial;

	private GroundMaterialHit m_lastImpactedMaterial;

	public Transform cachedTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Rigidbody cachedRigidbody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[DefaultValue(false)]
	public bool initialized
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public GroundMaterialManagerBase groundMaterialManager
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public int wheelCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public WheelState[] wheelState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float speed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float speedAngle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 localAcceleration
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float time
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool paused
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

	protected Wheel[] wheels
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 localImpactPosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 localImpactVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isHardImpact
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 localDragPosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 localDragVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isHardDrag
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Collider lastContactedCollider
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected VehicleBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VehicleBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetWheelLocalPosition(VPWheelCollider wheelCol)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NotifyCollidersChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SingleStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reposition(Vector3 position, Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetWheelRadius(int wheelIndex, float radius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetWheelRadius(int wheelIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetWheelTireFriction(int wheelIndex, TireFriction friction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetWheelTireFrictionMultiplier(int wheelIndex, float frictionMultiplier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TireFriction GetWheelTireFriction(int wheelIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetWheelAngularVelocityForSlip(int wheelIndex, float slip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetWheelPeakSlip(int wheelIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetWheelAdherentSlip(int wheelIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddWheelBrakeTorque(int wheelIndex, float torque)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VehicleStateVars GetVehicleStateVars()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVehicleStateVars(VehicleStateVars stateVars)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelStateVars[] GetWheelStateVars()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetWheelStateVars(WheelStateVars[] stateVars)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SolverState GetSolverState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Type[] GetSolverBlockTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableCollidersRaycast()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableCollidersRaycast()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ComputeVehicleSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ComputeLocalAcceleration()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfigureCenterOfMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float ComputeCombinedSlip(Vector2 localVelocity, Vector2 tireSlip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetNumberOfWheels(int numberOfWheels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnInitialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnFinalize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DoUpdateBlocks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DoUpdateData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual object GetInternalObject(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetWheelIndex(int axle, WheelPos position = WheelPos.Default)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetAxleCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetWheelsInAxle(int axle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateSuspensionTravel(WheelState wheel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateSuspensionForces(WheelState wheel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateDownforce(WheelState wheel, float referenceDownforce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateLocalFrame(WheelState wheel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyTireForce(WheelState wheel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableVehicleBehaviours()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableVehicleBehaviours()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdateVehicleBehaviours()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVehicleBehaviours()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSuspensionBehaviours()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddVehicleBehaviour(VehicleBehaviour vb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveVehicleBehaviour(VehicleBehaviour vb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RegisterVehicleBehaviour(VehicleBehaviour vb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnregisterVehicleBehaviour(VehicleBehaviour vb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NotifyReposition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NotifyEnterPause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NotifyLeavePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DebugLogWarning(string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DebugLogError(string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearCollisionData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessImpacts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCollisionEnter(Collision collision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCollisionStay(Collision collision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCollision(Collision collision, bool isCollisionEnter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDragState(Vector3 dragPosition, Vector3 dragVelocity, int dragHardness)
	{
		throw null;
	}
}
