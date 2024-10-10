using System;
using System.Collections.Generic;
using System.ComponentModel;
using EdyCommonTools;
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
		public float float_0;

		public float Tr;

		public float contactDepth;

		public Vector2 lastTireForce;
	}

	public struct BlockState
	{
		public float float_0;

		public float float_1;

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

	public float tireSideDeflectionRate = 10f;

	[Range(0f, 1f)]
	public float tireImpulseRatio = 0.5f;

	[Range(0f, 1f)]
	public float wheelSleepVelocity = 0.2f;

	public bool advancedSuspensionDamper;

	public float suspensionDamperLimitFactor = 2f;

	public bool contactAngleAffectsTireForce = true;

	[Range(1f, 20f)]
	public int integrationSteps = 4;

	public bool integrationUseRK4;

	public Transform centerOfMass;

	[HideInInspector]
	public bool accurateSuspensionForces = true;

	[HideInInspector]
	public float scaleFactor = 1f;

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

	public Transform m_transform;

	public Rigidbody m_rigidbody;

	public KSPWheelGravity gravity = new KSPWheelGravity();

	public bool tireForceSmoothing = true;

	public bool suspensionForceSmoothing = true;

	public float tireForceSharpness = 5f;

	public float suspensionForceSharpness = 5f;

	public float m_time;

	public Solver m_solver;

	public Wheel[] m_wheels = new Wheel[0];

	public WheelState[] m_wheelState = new WheelState[0];

	public Vector3 m_localAcceleration;

	public Vector3 m_lastVelocity;

	public float m_speed;

	public float m_speedAngle;

	public DataBus data = new DataBus();

	public Collider[] m_colliders = new Collider[0];

	public int[] m_colLayers = new int[0];

	public bool m_paused;

	public bool m_singleFixedStep;

	public bool m_singleUpdateStep;

	public List<VehicleBehaviour> m_activeBehaviours = new List<VehicleBehaviour>();

	public bool showContactGizmos = true;

	[HideInInspector]
	public bool disableContactProcessing;

	public Action onImpact;

	public Action onRawCollision;

	public static VehicleBase vehicle;

	public static Collision currentCollision;

	public static bool isCollisionEnter;

	[NonSerialized]
	public float impactThreeshold = 0.6f;

	[NonSerialized]
	public float impactInterval = 0.2f;

	[NonSerialized]
	public float impactIntervalRandom = 0.4f;

	[NonSerialized]
	public float impactMinSpeed = 2f;

	public int m_sumImpactCount;

	public Vector3 m_sumImpactPosition = Vector3.zero;

	public Vector3 m_sumImpactVelocity = Vector3.zero;

	public int m_sumImpactHardness;

	public float m_lastImpactTime;

	public Vector3 m_localDragPosition = Vector3.zero;

	public Vector3 m_localDragVelocity = Vector3.zero;

	public int m_localDragHardness;

	public GroundMaterial m_impactedGroundMaterial;

	public GroundMaterialHit m_lastImpactedMaterial;

	public Transform cachedTransform
	{
		get
		{
			if (!(m_transform != null))
			{
				return base.transform;
			}
			return m_transform;
		}
	}

	public Rigidbody cachedRigidbody
	{
		get
		{
			if (!(m_rigidbody != null))
			{
				return GetComponent<Rigidbody>();
			}
			return m_rigidbody;
		}
	}

	[DefaultValue(false)]
	public bool initialized { get; set; }

	public GroundMaterialManagerBase groundMaterialManager { get; set; }

	public int wheelCount => m_wheels.Length;

	public WheelState[] wheelState => m_wheelState;

	public float speed => m_speed;

	public float speedAngle => m_speedAngle;

	public Vector3 localAcceleration => m_localAcceleration;

	public float time => m_time;

	public bool paused
	{
		get
		{
			return m_paused;
		}
		set
		{
			if (m_paused != value)
			{
				m_singleFixedStep = false;
				m_singleUpdateStep = false;
				m_paused = value;
				if (m_paused)
				{
					NotifyEnterPause();
				}
				else
				{
					NotifyLeavePause();
				}
			}
		}
	}

	public Wheel[] wheels => m_wheels;

	public Vector3 localImpactPosition => m_sumImpactPosition;

	public Vector3 localImpactVelocity => m_sumImpactVelocity;

	public bool isHardImpact => m_sumImpactHardness >= 0;

	public Vector3 localDragPosition => m_localDragPosition;

	public Vector3 localDragVelocity => m_localDragVelocity;

	public bool isHardDrag => m_localDragHardness >= 0;

	public Collider lastContactedCollider { get; set; }

	public VehicleBase()
	{
	}

	public Vector3 GetWheelLocalPosition(VPWheelCollider wheelCol)
	{
		return m_transform.InverseTransformPoint(wheelCol.cachedTransform.TransformPoint(wheelCol.center));
	}

	public void NotifyCollidersChanged()
	{
		m_colliders = ColliderUtility.GetSolidColliders(base.transform, includeInactive: true);
		m_colLayers = new int[m_colliders.Length];
	}

	public void SingleStep()
	{
		if (paused && !m_singleFixedStep && !m_singleUpdateStep)
		{
			m_singleFixedStep = true;
			m_singleUpdateStep = true;
		}
	}

	public void Reposition(Vector3 position, Quaternion rotation)
	{
		m_transform.position = position;
		m_transform.rotation = rotation;
		NotifyReposition();
	}

	public void SetWheelRadius(int wheelIndex, float radius)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			radius = Mathf.Max(radius, 0.01f);
			m_wheels[wheelIndex].radius = radius;
			m_wheelState[wheelIndex].wheelCol.radius = radius;
		}
	}

	public float GetWheelRadius(int wheelIndex)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			return m_wheels[wheelIndex].radius;
		}
		return 0f;
	}

	public void SetWheelTireFriction(int wheelIndex, TireFriction friction)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length && friction != null)
		{
			m_wheels[wheelIndex].tireFriction = friction;
		}
	}

	public void SetWheelTireFrictionMultiplier(int wheelIndex, float frictionMultiplier)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			m_wheels[wheelIndex].tireFriction.frictionMultiplier = Mathf.Max(0f, frictionMultiplier);
		}
	}

	public TireFriction GetWheelTireFriction(int wheelIndex)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			return m_wheels[wheelIndex].tireFriction;
		}
		return null;
	}

	public float GetWheelAngularVelocityForSlip(int wheelIndex, float slip)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			WheelState wheelState = m_wheelState[wheelIndex];
			Wheel wheel = m_wheels[wheelIndex];
			if (!wheelState.grounded)
			{
				return 0f;
			}
			return (wheelState.localWheelVelocity.y + slip) / wheel.radius;
		}
		return 0f;
	}

	public Vector2 GetWheelPeakSlip(int wheelIndex)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			Wheel wheel = m_wheels[wheelIndex];
			return wheel.tireFriction.GetPeakSlipBounds(wheel.contactPatch);
		}
		return Vector2.zero;
	}

	public Vector2 GetWheelAdherentSlip(int wheelIndex)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			Wheel wheel = m_wheels[wheelIndex];
			return wheel.tireFriction.GetAdherentSlipBounds(wheel.contactPatch);
		}
		return Vector2.zero;
	}

	public void AddWheelBrakeTorque(int wheelIndex, float torque)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			m_wheels[wheelIndex].AddBrakeTorque(torque);
		}
	}

	public VehicleStateVars GetVehicleStateVars()
	{
		VehicleStateVars result = default(VehicleStateVars);
		result.time = m_time;
		result.lastVelocity = m_lastVelocity;
		result.lastImpactTime = m_lastImpactTime;
		return result;
	}

	public void SetVehicleStateVars(VehicleStateVars stateVars)
	{
		m_time = stateVars.time;
		m_lastVelocity = stateVars.lastVelocity;
		m_lastImpactTime = stateVars.lastImpactTime;
	}

	public WheelStateVars[] GetWheelStateVars()
	{
		WheelStateVars[] array = new WheelStateVars[m_wheels.Length];
		for (int i = 0; i < m_wheels.Length; i++)
		{
			array[i] = new WheelStateVars
			{
				float_0 = m_wheels[i].float_1,
				Tr = m_wheels[i].Tr,
				contactDepth = m_wheelState[i].contactDepth,
				lastTireForce = m_wheelState[i].lastTireForce
			};
		}
		return array;
	}

	public void SetWheelStateVars(WheelStateVars[] stateVars)
	{
		if (stateVars.Length == m_wheels.Length)
		{
			for (int i = 0; i < m_wheels.Length; i++)
			{
				m_wheels[i].float_1 = stateVars[i].float_0;
				m_wheels[i].Tr = stateVars[i].Tr;
				m_wheelState[i].reactionTorque = stateVars[i].Tr;
				m_wheelState[i].contactDepth = stateVars[i].contactDepth;
				m_wheelState[i].lastTireForce = stateVars[i].lastTireForce;
			}
		}
	}

	public SolverState GetSolverState()
	{
		Block[] blockArray = m_solver.GetBlockArray();
		SolverState result = default(SolverState);
		result.step = 0;
		result.time = Solver.time;
		result.blockStates = new BlockState[blockArray.Length];
		for (int i = 0; i < blockArray.Length; i++)
		{
			Block block = blockArray[i];
			if (block.connectedInputs > 0)
			{
				Block.Connection connection = null;
				int num = 0;
				while (num < block.inputs.Length)
				{
					connection = block.inputs[num];
					if (connection != null)
					{
						break;
					}
					i++;
				}
				result.blockStates[i] = new BlockState
				{
					float_0 = connection.float_0,
					float_1 = connection.float_1,
					Tr = connection.Tr,
					Td = connection.outTd
				};
			}
			else
			{
				result.blockStates[i] = new BlockState
				{
					float_0 = 0f,
					float_1 = 0f,
					Tr = 0f,
					Td = 0f
				};
			}
		}
		return result;
	}

	public Type[] GetSolverBlockTypes()
	{
		Block[] blockArray = m_solver.GetBlockArray();
		Type[] array = new Type[blockArray.Length];
		for (int i = 0; i < blockArray.Length; i++)
		{
			array[i] = blockArray[i].GetType();
		}
		return array;
	}

	public void DisableCollidersRaycast()
	{
		int i = 0;
		for (int num = m_colliders.Length; i < num; i++)
		{
			GameObject gameObject = m_colliders[i].gameObject;
			m_colLayers[i] = gameObject.layer;
			gameObject.layer = 2;
		}
	}

	public void EnableCollidersRaycast()
	{
		int i = 0;
		for (int num = m_colliders.Length; i < num; i++)
		{
			m_colliders[i].gameObject.layer = m_colLayers[i];
		}
	}

	public void ComputeVehicleSpeed()
	{
		Vector3 velocity = m_rigidbody.velocity;
		Vector3 forward = m_transform.forward;
		m_speed = Vector3.Dot(velocity, forward);
		velocity.y = 0f;
		forward.y = 0f;
		m_speedAngle = Vector3.Angle(velocity, forward) * Mathf.Sign(Vector3.Dot(velocity, m_transform.right));
	}

	public void ComputeLocalAcceleration()
	{
		Vector3 velocity = m_rigidbody.velocity;
		Vector3 direction = (velocity - m_lastVelocity) / Time.deltaTime;
		m_localAcceleration = m_transform.InverseTransformDirection(direction);
		m_lastVelocity = velocity;
	}

	public void ConfigureCenterOfMass()
	{
		if (centerOfMass != null)
		{
			Vector3 vector = m_transform.InverseTransformPoint(centerOfMass.position);
			if ((m_rigidbody.centerOfMass - vector).sqrMagnitude > 1E-07f)
			{
				m_rigidbody.centerOfMass = vector;
			}
		}
	}

	public static float ComputeCombinedSlip(Vector2 localVelocity, Vector2 tireSlip)
	{
		float magnitude = localVelocity.magnitude;
		if (magnitude > 0.01f)
		{
			float num = tireSlip.x * localVelocity.x / magnitude;
			float y = tireSlip.y;
			return Mathf.Sqrt(num * num + y * y);
		}
		return tireSlip.magnitude;
	}

	public void SetNumberOfWheels(int numberOfWheels)
	{
		m_wheels = new Wheel[numberOfWheels];
		m_wheelState = new WheelState[numberOfWheels];
		for (int i = 0; i < numberOfWheels; i++)
		{
			m_wheels[i] = new Wheel();
			m_wheelState[i] = new WheelState();
			m_wheelState[i].lastGroundHit.physicMaterial = new PhysicMaterial();
		}
	}

	public virtual void OnInitialize()
	{
		SetNumberOfWheels(4);
	}

	public virtual void OnFinalize()
	{
	}

	public virtual void DoUpdateBlocks()
	{
	}

	public virtual void DoUpdateData()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual object GetInternalObject(Type type)
	{
		return null;
	}

	public virtual int GetWheelIndex(int axle, WheelPos position = WheelPos.Default)
	{
		int num = (int)position;
		if (num < 0)
		{
			num = 0;
		}
		else if (num >= 1)
		{
			num = 0;
		}
		int num2 = axle + num;
		if (num2 >= 0 && num2 < wheels.Length)
		{
			return num2;
		}
		return -1;
	}

	public virtual int GetAxleCount()
	{
		return wheels.Length / 1;
	}

	public virtual int GetWheelsInAxle(int axle)
	{
		if (axle >= 0 && axle < GetAxleCount())
		{
			return 1;
		}
		return 0;
	}

	public void CalculateSuspensionTravel(WheelState wheel)
	{
		VPWheelCollider wheelCol = wheel.wheelCol;
		wheel.grounded = wheelCol.GetGroundHit(out wheel.hit);
		wheel.lastContactDepth = wheel.contactDepth;
		if (wheel.grounded)
		{
			wheel.contactDepth = wheelCol.GetContactDepth(wheel.hit.point, wheelCol.lastRuntimeSuspensionTravel);
			wheel.suspensionCompression = wheel.contactDepth / wheelCol.lastRuntimeSuspensionTravel;
		}
		else
		{
			wheel.contactDepth = 0f;
			wheel.suspensionCompression = 0f;
			wheel.hit.force = 0f;
		}
	}

	public void CalculateSuspensionForces(WheelState wheel)
	{
		VPWheelCollider wheelCol = wheel.wheelCol;
		if (wheel.grounded)
		{
			wheel.contactDepth += wheelCol.runtimeSuspensionTravel - wheelCol.lastRuntimeSuspensionTravel;
			wheel.suspensionCompression = wheel.contactDepth / wheelCol.runtimeSuspensionTravel;
			wheel.contactSpeed = (wheel.contactDepth - wheel.lastContactDepth) / Time.deltaTime;
			if (advancedSuspensionDamper)
			{
				wheel.damperForce = wheel.contactSpeed * wheelCol.runtimeDamperRate * scaleFactor;
				float max = suspensionDamperLimitFactor * m_rigidbody.mass * vehicle.gravity.Magnitude;
				wheel.damperForce = Mathf.Clamp(wheel.damperForce, 0f - wheel.hit.force, max);
				wheel.hit.force += wheel.damperForce;
			}
			else
			{
				wheel.damperForce = wheel.hit.force - wheel.contactDepth * wheelCol.runtimeSpringRate * scaleFactor;
			}
		}
		else
		{
			wheel.contactSpeed = 0f;
			wheel.damperForce = 0f;
		}
		if (accurateSuspensionForces)
		{
			wheelCol.SetSuspensionForceOffset(wheel.contactDepth);
		}
	}

	public void CalculateDownforce(WheelState wheel, float referenceDownforce)
	{
		if (wheel.grounded)
		{
			wheel.downforce = wheel.hit.force + wheel.wheelCol.runtimeExtraDownforce;
			if (contactAngleAffectsTireForce)
			{
				Vector3 up = wheel.wheelCol.cachedTransform.up;
				float y = Vector3.Dot(up, wheel.hit.sidewaysDir);
				float x = Vector3.Dot(up, wheel.hit.normal);
				wheel.contactAngle = Mathf.Atan2(y, x);
				wheel.downforce *= Mathf.Cos(wheel.contactAngle);
			}
			if (wheel.downforce < minimumdownForce)
			{
				wheel.downforce = minimumdownForce;
			}
			wheel.normalizedLoad = wheel.hit.force / referenceDownforce;
		}
		else
		{
			wheel.contactAngle = 0f;
			wheel.downforce = 0f;
			wheel.normalizedLoad = 0f;
		}
	}

	public void CalculateLocalFrame(WheelState wheel)
	{
		if (!wheel.grounded)
		{
			wheel.wheelVelocity = Vector3.zero;
			wheel.localWheelVelocity = Vector2.zero;
			wheel.surfaceForce = Vector3.zero;
			wheel.localSurfaceForce = Vector2.zero;
			wheel.externalTireForce = Vector2.zero;
			return;
		}
		wheel.wheelVelocity = wheel.wheelCol.GetTangentVelocity(wheel.hit.point, wheel.hit.normal, wheel.hit.collider.attachedRigidbody);
		wheel.localWheelVelocity.y = Vector3.Dot(wheel.hit.forwardDir, wheel.wheelVelocity);
		wheel.localWheelVelocity.x = Vector3.Dot(wheel.hit.sidewaysDir, wheel.wheelVelocity);
		float num = Mathf.InverseLerp(1f, 0.25f, wheel.wheelVelocity.sqrMagnitude);
		if (num > 0f)
		{
			float num2 = Vector3.Dot(gravity.Up, wheel.hit.normal);
			if (num2 > 1E-06f)
			{
				Vector3 vector = gravity.Up * wheel.hit.force / num2;
				wheel.surfaceForce = vector - Vector3.Project(vector, wheel.hit.normal);
			}
			else
			{
				wheel.surfaceForce = gravity.Up * 100000f;
			}
			wheel.localSurfaceForce.y = Vector3.Dot(wheel.hit.forwardDir, wheel.surfaceForce);
			wheel.localSurfaceForce.x = Vector3.Dot(wheel.hit.sidewaysDir, wheel.surfaceForce);
			wheel.localSurfaceForce *= num;
		}
		else
		{
			wheel.surfaceForce = Vector3.zero;
			wheel.localSurfaceForce = Vector2.zero;
		}
		float num3 = (wheel.hit.force + wheel.wheelCol.runtimeExtraDownforce) * (1f / gravity.Magnitude);
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		Vector2 vector2 = (0f - tireImpulseRatio) * num3 * wheel.localWheelVelocity / Time.deltaTime;
		wheel.externalTireForce = vector2 + wheel.localSurfaceForce;
		if (groundMaterialManager != null)
		{
			GroundMaterialHit groundMaterialHit = default(GroundMaterialHit);
			groundMaterialHit.physicMaterial = wheel.hit.collider.sharedMaterial;
			groundMaterialHit.collider = wheel.hit.collider;
			groundMaterialHit.hitPoint = wheel.hit.point;
			GroundMaterialHit groundHit = groundMaterialHit;
			groundMaterialManager.GetGroundMaterialCached(this, groundHit, ref wheel.lastGroundHit, ref wheel.groundMaterial);
		}
	}

	public void ApplyTireForce(WheelState wheel)
	{
		if (!wheel.grounded)
		{
			wheel.lastTireForce = Vector2.zero;
			return;
		}
		float num = ((wheel.groundMaterial != null) ? wheel.groundMaterial.drag : 0f);
		Vector2 vector = (0f - wheel.hit.force * wheel.localWheelVelocity.magnitude * num * 0.001f) * wheel.localWheelVelocity;
		if (tireSideDeflection)
		{
			wheel.tireForce.x = Mathf.Lerp(wheel.lastTireForce.x, wheel.tireForce.x, tireSideDeflectionRate * Time.deltaTime);
		}
		if (tireForceSmoothing)
		{
			wheel.tireForce.x = Mathf.Lerp(wheel.lastTireForce.x, wheel.tireForce.x, tireForceSharpness * Time.deltaTime);
			wheel.tireForce.y = Mathf.Lerp(wheel.lastTireForce.y, wheel.tireForce.y, tireForceSharpness * Time.deltaTime);
		}
		wheel.lastTireForce = wheel.tireForce;
		Vector3 vector2 = wheel.hit.forwardDir * (wheel.tireForce.y + vector.y) + wheel.hit.sidewaysDir * (wheel.tireForce.x + vector.x);
		Vector3 vector3 = (advancedSuspensionDamper ? (wheel.wheelCol.transform.up * wheel.damperForce) : Vector3.zero);
		wheel.wheelCol.ApplyForce(vector2 + vector3, wheel.hit.point, wheel.hit.collider.attachedRigidbody);
	}

	public void OnEnable()
	{
		m_transform = GetComponent<Transform>();
		m_rigidbody = GetComponentInParent<Rigidbody>();
		if (m_rigidbody == null)
		{
			DebugLogError("Rigidbody not found in this GameObject nor any of its parents. Vehicle controller disabled.");
			base.enabled = false;
			return;
		}
		Vector3 vector = m_rigidbody.centerOfMass;
		m_time = 0f;
		VPWheelCollider.scaleFactor = scaleFactor;
		OnInitialize();
		if (groundMaterialManager == null)
		{
			groundMaterialManager = UnityEngine.Object.FindObjectOfType<GroundMaterialManagerBase>();
		}
		int num = 0;
		int num2 = wheels.Length;
		while (true)
		{
			if (num < num2)
			{
				WheelState obj = m_wheelState[num];
				Wheel wheel = m_wheels[num];
				if (!(obj.wheelCol == null))
				{
					if (wheel.tireFriction != null)
					{
						if (wheel.radius < 0.01f || !(wheel.mass >= 0.01f))
						{
							break;
						}
						wheel.RecalculateConstants();
						wheel.RecalculateVars();
						num++;
						continue;
					}
					DebugLogError("Missing tire friction data for wheel[" + num + "]");
					base.enabled = false;
					return;
				}
				DebugLogError("Missing WheelCollider for wheelsState[" + num + "]");
				base.enabled = false;
				return;
			}
			if (centerOfMass != null)
			{
				m_rigidbody.centerOfMass = m_transform.InverseTransformPoint(centerOfMass.position);
			}
			else
			{
				m_rigidbody.centerOfMass = vector;
			}
			m_rigidbody.maxAngularVelocity = 14f;
			NotifyCollidersChanged();
			m_solver = new Solver();
			if (!m_solver.Initialize(m_wheels, enableRK4: true))
			{
				DebugLogWarning("Block chain initialization failed. Ensure all blocks are properly connected.\n" + m_solver.resultMessage);
			}
			m_solver.Integrate(m_time, Time.fixedDeltaTime, 1, useRK4: false);
			EnableVehicleBehaviours();
			initialized = true;
			return;
		}
		DebugLogError("Invalid wheel properties (radius and/or mass) for wheel[" + num + "]");
		base.enabled = false;
	}

	public void OnDisable()
	{
		if (!(m_rigidbody == null))
		{
			ClearCollisionData();
			initialized = false;
			DisableVehicleBehaviours();
			OnFinalize();
			m_solver = null;
		}
	}

	public void FixedUpdate()
	{
		float magnitude = m_rigidbody.velocity.magnitude;
		if (magnitude > 1f)
		{
			m_rigidbody.maxDepenetrationVelocity = 1f;
		}
		else
		{
			m_rigidbody.maxDepenetrationVelocity = Mathf.Lerp(8f, 1f, magnitude);
		}
		if (paused)
		{
			if (!m_singleFixedStep)
			{
				return;
			}
			m_singleFixedStep = false;
		}
		if (onPreDynamicsStep != null)
		{
			onPreDynamicsStep();
		}
		VPWheelCollider.scaleFactor = scaleFactor;
		float num = 1f / scaleFactor;
		ComputeVehicleSpeed();
		ComputeLocalAcceleration();
		WheelState[] array = m_wheelState;
		foreach (WheelState wheel in array)
		{
			CalculateSuspensionTravel(wheel);
		}
		UpdateSuspensionBehaviours();
		float referenceDownforce = m_rigidbody.mass * gravity.Magnitude / (float)m_wheelState.Length;
		array = m_wheelState;
		foreach (WheelState wheel2 in array)
		{
			CalculateSuspensionForces(wheel2);
			CalculateDownforce(wheel2, referenceDownforce);
			CalculateLocalFrame(wheel2);
		}
		int j = 0;
		for (int num2 = m_wheelState.Length; j < num2; j++)
		{
			Wheel wheel3 = m_wheels[j];
			WheelState wheelState = m_wheelState[j];
			wheel3.load = wheelState.downforce * num;
			wheel3.grip = ((wheelState.groundMaterial != null) ? wheelState.groundMaterial.grip : 1f);
			wheel3.vector2_0 = wheelState.localWheelVelocity;
			wheel3.Fext = wheelState.externalTireForce * num;
			float num3 = Mathf.Max(wheelState.brakeTorque, wheelState.downforce * num * wheel3.radius);
			wheel3.Tr = Mathf.Clamp(wheelState.reactionTorque, 0f - num3, num3);
		}
		if (onBeforeUpdateBlocks != null)
		{
			onBeforeUpdateBlocks();
		}
		DoUpdateBlocks();
		if (onBeforeIntegrationStep != null)
		{
			onBeforeIntegrationStep();
		}
		m_solver.Integrate(m_time, Time.deltaTime, integrationSteps, integrationUseRK4);
		int num4 = 0;
		int k = 0;
		for (int num5 = m_wheelState.Length; k < num5; k++)
		{
			if (m_wheels[k].isResting)
			{
				num4++;
			}
		}
		bool flag = ((vehicleSleepCriteria == VehicleSleepCriteria.Strict) ? (num4 >= m_wheelState.Length / 2) : (num4 > m_wheelState.Length / 2));
		int l = 0;
		for (int num6 = m_wheelState.Length; l < num6; l++)
		{
			Wheel wheel4 = m_wheels[l];
			WheelState wheelState2 = m_wheelState[l];
			wheelState2.angularVelocity = wheel4.angularVelocity;
			wheelState2.tireForce = wheel4.vector2_2 * scaleFactor;
			wheelState2.reactionTorque = wheel4.Tr;
			wheelState2.tireSlip.x = wheelState2.localWheelVelocity.x;
			wheelState2.tireSlip.y = wheelState2.localWheelVelocity.y - wheelState2.angularVelocity * wheel4.radius;
			wheelState2.combinedTireSlip = ComputeCombinedSlip(wheelState2.localWheelVelocity, wheelState2.tireSlip);
			wheelState2.driveTorque = wheel4.driveTorque;
			wheelState2.brakeTorque = wheel4.brakeTorque;
			ApplyTireForce(wheelState2);
			bool flag2 = wheelState2.localWheelVelocity.magnitude < wheelSleepVelocity;
			wheelState2.wheelCol.canSleep = !inhibitWheelSleep && (flag ? flag2 : (flag2 && wheel4.isResting));
		}
		inhibitWheelSleep = false;
		DoUpdateData();
		if (!disableContactProcessing)
		{
			ProcessImpacts();
		}
		ConfigureCenterOfMass();
		FixedUpdateVehicleBehaviours();
		m_time += Time.deltaTime;
	}

	public void Update()
	{
		if (paused)
		{
			if (m_singleFixedStep || !m_singleUpdateStep)
			{
				return;
			}
			m_singleUpdateStep = false;
		}
		if (onPreVisualUpdate != null)
		{
			onPreVisualUpdate();
		}
		bool flag;
		if (flag = m_rigidbody.interpolation != RigidbodyInterpolation.None)
		{
			DisableCollidersRaycast();
		}
		int i = 0;
		for (int num = wheelCount; i < num; i++)
		{
			WheelState wheelState = m_wheelState[i];
			wheelState.wheelCol.steerAngle = wheelState.steerAngle;
			wheelState.wheelCol.angularVelocity = (invertVisualWheelSpinDirection ? (0f - wheelState.angularVelocity) : wheelState.angularVelocity);
			wheelState.wheelCol.UpdateVisualWheel(paused ? Time.fixedDeltaTime : Time.deltaTime);
		}
		invertVisualWheelSpinDirection = false;
		if (flag)
		{
			EnableCollidersRaycast();
		}
		if (!disableContactProcessing)
		{
			UpdateDragState(Vector3.zero, Vector3.zero, m_localDragHardness);
		}
		OnUpdate();
		UpdateVehicleBehaviours();
	}

	public void EnableVehicleBehaviours()
	{
		VehicleBehaviour[] componentsInChildren = GetComponentsInChildren<VehicleBehaviour>(includeInactive: true);
		foreach (VehicleBehaviour vehicleBehaviour in componentsInChildren)
		{
			if (vehicleBehaviour.enabled && vehicleBehaviour.gameObject.activeInHierarchy)
			{
				AddVehicleBehaviour(vehicleBehaviour);
			}
			else
			{
				RemoveVehicleBehaviour(vehicleBehaviour);
			}
		}
	}

	public void DisableVehicleBehaviours()
	{
		while (m_activeBehaviours.Count > 0)
		{
			RemoveVehicleBehaviour(m_activeBehaviours[0]);
		}
	}

	public void FixedUpdateVehicleBehaviours()
	{
		for (int i = 0; i < m_activeBehaviours.Count; i++)
		{
			m_activeBehaviours[i].FixedUpdateVehicle();
		}
	}

	public void UpdateVehicleBehaviours()
	{
		for (int i = 0; i < m_activeBehaviours.Count; i++)
		{
			m_activeBehaviours[i].UpdateVehicle();
		}
	}

	public void UpdateSuspensionBehaviours()
	{
		for (int i = 0; i < m_activeBehaviours.Count; i++)
		{
			m_activeBehaviours[i].UpdateVehicleSuspension();
		}
	}

	public void AddVehicleBehaviour(VehicleBehaviour vb)
	{
		if (m_activeBehaviours.Contains(vb) || vb.vehicle == null)
		{
			return;
		}
		vb.OnEnableVehicle();
		if (vb.enabled && vb.gameObject.activeInHierarchy)
		{
			int i;
			for (i = 0; i < m_activeBehaviours.Count && vb.GetUpdateOrder() >= m_activeBehaviours[i].GetUpdateOrder(); i++)
			{
			}
			m_activeBehaviours.Insert(i, vb);
			if (paused)
			{
				vb.OnEnterPause();
			}
		}
	}

	public void RemoveVehicleBehaviour(VehicleBehaviour vb)
	{
		if (m_activeBehaviours.Contains(vb))
		{
			if (paused)
			{
				vb.OnLeavePause();
			}
			vb.OnDisableVehicle();
			m_activeBehaviours.Remove(vb);
		}
	}

	public void RegisterVehicleBehaviour(VehicleBehaviour vb)
	{
		if (initialized)
		{
			AddVehicleBehaviour(vb);
		}
	}

	public void UnregisterVehicleBehaviour(VehicleBehaviour vb)
	{
		if (initialized)
		{
			RemoveVehicleBehaviour(vb);
		}
	}

	public void NotifyReposition()
	{
		if (initialized)
		{
			for (int i = 0; i < m_activeBehaviours.Count; i++)
			{
				m_activeBehaviours[i].OnReposition();
			}
		}
	}

	public void NotifyEnterPause()
	{
		if (initialized)
		{
			for (int i = 0; i < m_activeBehaviours.Count; i++)
			{
				m_activeBehaviours[i].OnEnterPause();
			}
		}
	}

	public void NotifyLeavePause()
	{
		if (initialized)
		{
			for (int i = 0; i < m_activeBehaviours.Count; i++)
			{
				m_activeBehaviours[i].OnLeavePause();
			}
		}
	}

	public void DebugLogWarning(string message)
	{
		Debug.LogWarning(ToString() + ": " + message + "\n", this);
	}

	public void DebugLogError(string message)
	{
		Debug.LogError(ToString() + ": " + message + "\n", this);
	}

	public void ClearCollisionData()
	{
		m_sumImpactCount = 0;
		m_sumImpactPosition = Vector3.zero;
		m_sumImpactVelocity = Vector3.zero;
		m_sumImpactHardness = 0;
		m_lastImpactTime = 0f;
		m_localDragPosition = Vector3.zero;
		m_localDragVelocity = Vector3.zero;
		m_localDragHardness = 0;
		m_lastImpactedMaterial.physicMaterial = null;
		m_impactedGroundMaterial = null;
	}

	public void ProcessImpacts()
	{
		if (m_time - m_lastImpactTime >= impactInterval && m_sumImpactCount > 0)
		{
			float num = 1f / (float)m_sumImpactCount;
			m_sumImpactPosition *= num;
			m_sumImpactVelocity *= num;
			if (onImpact != null)
			{
				vehicle = this;
				onImpact();
				vehicle = null;
			}
			if (showContactGizmos && localImpactVelocity.sqrMagnitude > 0.0001f)
			{
				Debug.DrawLine(base.transform.TransformPoint(localImpactPosition), base.transform.TransformPoint(localImpactPosition) + MathUtility.Lin2Log(base.transform.TransformDirection(localImpactVelocity)), Color.red, 0.2f, depthTest: false);
			}
			m_sumImpactCount = 0;
			m_sumImpactPosition = Vector3.zero;
			m_sumImpactVelocity = Vector3.zero;
			m_sumImpactHardness = 0;
			m_lastImpactTime = m_time + impactInterval * UnityEngine.Random.Range(0f - impactIntervalRandom, impactIntervalRandom);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		OnCollision(collision, isCollisionEnter: true);
	}

	public void OnCollisionStay(Collision collision)
	{
		OnCollision(collision, isCollisionEnter: false);
	}

	public void OnCollision(Collision collision, bool isCollisionEnter)
	{
		if (!initialized || disableContactProcessing)
		{
			return;
		}
		int num = 0;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		int num2 = 0;
		int num3 = 0;
		Vector3 zero3 = Vector3.zero;
		Vector3 zero4 = Vector3.zero;
		int num4 = 0;
		float num5 = impactMinSpeed * impactMinSpeed;
		ContactPoint[] contacts = collision.contacts;
		for (int i = 0; i < contacts.Length; i++)
		{
			ContactPoint contactPoint = contacts[i];
			lastContactedCollider = contactPoint.otherCollider;
			int num6 = 0;
			if (groundMaterialManager != null)
			{
				GroundMaterialHit groundMaterialHit = default(GroundMaterialHit);
				groundMaterialHit.physicMaterial = contactPoint.otherCollider.sharedMaterial;
				groundMaterialHit.collider = contactPoint.otherCollider;
				groundMaterialHit.hitPoint = contactPoint.point;
				GroundMaterialHit groundHit = groundMaterialHit;
				groundMaterialManager.GetGroundMaterialCached(this, groundHit, ref m_lastImpactedMaterial, ref m_impactedGroundMaterial);
				if (m_impactedGroundMaterial != null)
				{
					num6 = ((m_impactedGroundMaterial.surfaceType == GroundMaterial.SurfaceType.Hard) ? 1 : (-1));
				}
			}
			Vector3 pointVelocity = contactPoint.thisCollider.attachedRigidbody.GetPointVelocity(contactPoint.point);
			if (contactPoint.otherCollider.attachedRigidbody != null)
			{
				pointVelocity -= contactPoint.otherCollider.attachedRigidbody.GetPointVelocity(contactPoint.point);
			}
			float num7 = Vector3.Dot(pointVelocity, contactPoint.normal);
			if (!(num7 < 0f - impactThreeshold) && (!isCollisionEnter || collision.relativeVelocity.sqrMagnitude <= num5))
			{
				if (num7 < impactThreeshold)
				{
					num3++;
					zero3 += contactPoint.point;
					zero4 += pointVelocity;
					num4 += num6;
					if (showContactGizmos)
					{
						Debug.DrawLine(contactPoint.point, contactPoint.point + MathUtility.Lin2Log(pointVelocity), Color.cyan);
					}
				}
			}
			else
			{
				num++;
				zero += contactPoint.point;
				zero2 += collision.relativeVelocity;
				num2 += num6;
				if (showContactGizmos)
				{
					Debug.DrawLine(contactPoint.point, contactPoint.point + MathUtility.Lin2Log(pointVelocity), Color.red);
				}
			}
			if (showContactGizmos)
			{
				Debug.DrawLine(contactPoint.point, contactPoint.point + contactPoint.normal * 0.25f, Color.yellow);
			}
		}
		if (num > 0)
		{
			float num8 = 1f / (float)num;
			zero *= num8;
			zero2 *= num8;
			m_sumImpactCount++;
			m_sumImpactPosition += m_transform.InverseTransformPoint(zero);
			m_sumImpactVelocity += m_transform.InverseTransformDirection(zero2);
			m_sumImpactHardness += num2;
		}
		if (num3 > 0)
		{
			float num9 = 1f / (float)num3;
			zero3 *= num9;
			zero4 *= num9;
			UpdateDragState(m_transform.InverseTransformPoint(zero3), m_transform.InverseTransformDirection(zero4), num4);
		}
		if (onRawCollision != null)
		{
			vehicle = this;
			currentCollision = collision;
			VehicleBase.isCollisionEnter = isCollisionEnter;
			onRawCollision();
			currentCollision = null;
			vehicle = null;
		}
	}

	public void UpdateDragState(Vector3 dragPosition, Vector3 dragVelocity, int dragHardness)
	{
		if (dragVelocity.sqrMagnitude > 0.0001f)
		{
			m_localDragPosition = Vector3.Lerp(m_localDragPosition, dragPosition, 10f * Time.deltaTime);
			m_localDragVelocity = Vector3.Lerp(m_localDragVelocity, dragVelocity, 20f * Time.deltaTime);
			m_localDragHardness = dragHardness;
		}
		else
		{
			m_localDragVelocity = Vector3.Lerp(m_localDragVelocity, Vector3.zero, 10f * Time.deltaTime);
		}
		if (showContactGizmos && localDragVelocity.sqrMagnitude > 0.0001f)
		{
			Debug.DrawLine(base.transform.TransformPoint(localDragPosition), base.transform.TransformPoint(localDragPosition) + MathUtility.Lin2Log(base.transform.TransformDirection(localDragVelocity)), Color.cyan, 0.05f, depthTest: false);
		}
	}
}
