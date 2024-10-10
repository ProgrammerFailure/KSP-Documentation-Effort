using System;
using System.ComponentModel;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[DisallowMultipleComponent]
[AddComponentMenu("Vehicle Physics/Wheel Collider", -20)]
public class VPWheelCollider : VehicleBehaviour
{
	public float mass = 20f;

	public float radius = 0.3f;

	public Vector3 center;

	[Range(0.01f, 2f)]
	public float suspensionDistance = 0.25f;

	[Range(0f, 1f)]
	public float suspensionAnchor = 0.5f;

	public float springRate = 30000f;

	public float damperRate = 1500f;

	public Transform suspensionTransform;

	public Transform caliperTransform;

	public Transform wheelTransform;

	[Range(0f, 0.2f)]
	public float groundPenetration = 0.02f;

	public bool disableSuspensionMovement;

	public bool hideWheelOnDisable = true;

	public static bool disableSteerAngleFix = false;

	public static bool disableWheelReferenceFrameFix = false;

	public static float minSuspensionDistance = 0.01f;

	public static float scaleFactor = 1f;

	public int layerMask = -1;

	public bool updateSuspension;

	public bool updateCaliper;

	public bool updateWheel;

	public float suspensionOffset;

	public Transform m_transform;

	public Rigidbody m_rigidbody;

	public Transform m_rigidbodyTransform;

	public RaycastHit m_visualHit;

	public ColliderUtility.LayerCollisionMatrix m_collisionMatrix;

	public WheelCollider m_wheelCollider;

	public float m_steerAngle;

	public float m_angularPosition;

	public float m_contactDistance;

	public Vector3 m_suspensionPosition = Vector3.zero;

	public InterpolatedFloat m_visualSteerAngle = new InterpolatedFloat();

	public bool m_isCaliperChildOfSuspension;

	public bool m_isWheelChildOfSuspension;

	public bool m_isWheelChildOfCaliper;

	public float m_2PI = (float)Math.PI * 2f;

	public bool debugForces;

	public float springSlerpRate = 0.02f;

	public bool hidden { get; set; }

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

	[DefaultValue(false)]
	public bool visualGrounded { get; set; }

	public RaycastHit visualHit => m_visualHit;

	public float steerAngle
	{
		get
		{
			return m_steerAngle;
		}
		set
		{
			if (base.isActiveAndEnabled)
			{
				m_wheelCollider.steerAngle = (disableSteerAngleFix ? value : FixSteerAngle(value));
				m_steerAngle = value;
			}
		}
	}

	public float angularVelocity { get; set; }

	public float angularPosition
	{
		get
		{
			return m_angularPosition;
		}
		set
		{
			m_angularPosition = value;
		}
	}

	public bool canSleep { get; set; }

	public float effectiveSpringRate => m_wheelCollider.suspensionSpring.spring;

	public float runtimeSpringRate { get; set; }

	public float runtimeDamperRate { get; set; }

	public float runtimeSuspensionTravel { get; set; }

	public float lastRuntimeSpringRate { get; set; }

	public float lastRuntimeDamperRate { get; set; }

	public float lastRuntimeSuspensionTravel { get; set; }

	public float runtimeExtraDownforce { get; set; }

	public void OnValidate()
	{
		if (radius < 0.01f)
		{
			radius = 0.01f;
		}
		if (mass < 0.1f)
		{
			mass = 0.1f;
		}
		if (suspensionDistance < minSuspensionDistance)
		{
			suspensionDistance = minSuspensionDistance;
		}
		if (springRate < 1f)
		{
			springRate = 1f;
		}
		if (damperRate < 0f)
		{
			damperRate = 0f;
		}
		suspensionAnchor = Mathf.Clamp01(suspensionAnchor);
	}

	public void Awake()
	{
		m_wheelCollider = SetupWheelCollider();
	}

	public override void OnEnableVehicle()
	{
		m_wheelCollider.enabled = true;
		m_transform = base.transform;
		m_rigidbody = m_wheelCollider.attachedRigidbody;
		m_rigidbodyTransform = m_rigidbody.transform;
		m_collisionMatrix = new ColliderUtility.LayerCollisionMatrix();
		m_wheelCollider.ConfigureVehicleSubsteps(1000f, 1, 1);
		m_isCaliperChildOfSuspension = caliperTransform != null && suspensionTransform != null && caliperTransform.IsChildOf(suspensionTransform);
		m_isWheelChildOfSuspension = wheelTransform != null && suspensionTransform != null && wheelTransform.IsChildOf(suspensionTransform);
		m_isWheelChildOfCaliper = wheelTransform != null && caliperTransform != null && wheelTransform.IsChildOf(caliperTransform);
		m_suspensionPosition = m_transform.position;
		if ((bool)wheelTransform)
		{
			m_angularPosition = wheelTransform.localEulerAngles.x;
		}
		m_visualSteerAngle.Reset(m_steerAngle);
		runtimeSpringRate = springRate;
		runtimeDamperRate = damperRate;
		runtimeSuspensionTravel = suspensionDistance;
		runtimeExtraDownforce = 0f;
	}

	public override void OnDisableVehicle()
	{
		m_wheelCollider.enabled = false;
	}

	public override void OnDisableComponent()
	{
		m_wheelCollider.enabled = false;
	}

	public override int GetUpdateOrder()
	{
		return 100;
	}

	public override void FixedUpdateVehicle()
	{
		UpdateWheelCollider(m_wheelCollider);
		lastRuntimeSpringRate = runtimeSpringRate;
		lastRuntimeDamperRate = runtimeDamperRate;
		lastRuntimeSuspensionTravel = runtimeSuspensionTravel;
		runtimeSpringRate = springRate;
		runtimeDamperRate = damperRate;
		runtimeSuspensionTravel = suspensionDistance;
		runtimeExtraDownforce = 0f;
		m_visualSteerAngle.Set(m_steerAngle);
	}

	public void UpdateVisualWheel(float deltaTime)
	{
		if (!base.isActiveAndEnabled)
		{
			if (hideWheelOnDisable && wheelTransform != null && wheelTransform.gameObject.activeInHierarchy)
			{
				wheelTransform.gameObject.SetActive(value: false);
			}
			return;
		}
		if (hideWheelOnDisable && wheelTransform != null && !wheelTransform.gameObject.activeInHierarchy)
		{
			wheelTransform.gameObject.SetActive(value: true);
		}
		Vector3 origin = m_transform.TransformPoint(m_wheelCollider.center) + m_transform.up * radius;
		Vector3 vector = -m_transform.up;
		visualGrounded = Physics.Raycast(origin, vector, out m_visualHit, lastRuntimeSuspensionTravel + radius * 2f, layerMask, QueryTriggerInteraction.Ignore);
		if (visualGrounded)
		{
			m_contactDistance = Mathf.Clamp(m_visualHit.distance - radius * 2f, 0f, lastRuntimeSuspensionTravel);
		}
		else
		{
			m_contactDistance = lastRuntimeSuspensionTravel;
		}
		m_angularPosition = (m_angularPosition + angularVelocity * deltaTime) % m_2PI;
		if (!disableSuspensionMovement)
		{
			m_suspensionPosition = m_transform.position + vector * (m_contactDistance + groundPenetration - suspensionDistance * (1f - suspensionAnchor) + suspensionOffset);
		}
		else
		{
			m_suspensionPosition = m_transform.position;
		}
		if (suspensionTransform != null && updateSuspension)
		{
			suspensionTransform.position = m_suspensionPosition;
		}
		if (caliperTransform != null && updateCaliper)
		{
			if (suspensionTransform == null || !m_isCaliperChildOfSuspension)
			{
				caliperTransform.position = m_suspensionPosition;
			}
			caliperTransform.localRotation = Quaternion.Euler(0f, m_visualSteerAngle.GetInterpolated(), 0f);
		}
		if (wheelTransform != null && updateWheel)
		{
			if ((suspensionTransform == null && caliperTransform == null) || (!m_isWheelChildOfSuspension && !m_isWheelChildOfCaliper))
			{
				wheelTransform.position = m_suspensionPosition;
			}
			if (caliperTransform != null && m_isWheelChildOfCaliper)
			{
				wheelTransform.localRotation = Quaternion.Euler(m_angularPosition * 57.29578f, 0f, 0f);
			}
			else
			{
				wheelTransform.localRotation = Quaternion.Euler(m_angularPosition * 57.29578f, m_visualSteerAngle.GetInterpolated(), 0f);
			}
		}
	}

	public void SetSuspensionForceOffset(float offset)
	{
		if (!(m_wheelCollider == null))
		{
			float forceAppPointDistance = offset - lastRuntimeSuspensionTravel * m_wheelCollider.suspensionSpring.targetPosition;
			m_wheelCollider.forceAppPointDistance = forceAppPointDistance;
		}
	}

	public bool GetGroundHit(out WheelHit hit)
	{
		if (!(m_wheelCollider == null) && !(m_rigidbody == null))
		{
			bool flag = m_wheelCollider.GetGroundHit(out hit);
			if (!base.isActiveAndEnabled)
			{
				flag = false;
			}
			if (flag)
			{
				if (m_rigidbody != null)
				{
					hit.point += m_rigidbody.velocity * Time.fixedDeltaTime;
				}
				if (!disableWheelReferenceFrameFix)
				{
					Vector3 vector = Quaternion.AngleAxis(m_steerAngle, m_transform.up) * m_transform.forward;
					float num = 0f - Vector3.Dot(vector, hit.normal);
					float num2 = Vector3.Dot(m_transform.up, hit.normal);
					if (num2 > 1E-06f)
					{
						Vector3 vector2 = m_transform.up * (num / num2);
						hit.forwardDir = (vector + vector2).normalized;
					}
					else
					{
						hit.forwardDir = ((num >= 0f) ? m_transform.up : (-m_transform.up));
					}
					hit.sidewaysDir = Vector3.Cross(hit.normal, hit.forwardDir);
				}
			}
			return flag;
		}
		hit = default(WheelHit);
		return false;
	}

	public float GetContactDepth(Vector3 contactPoint, float suspensionTravel)
	{
		if (suspensionDistance < minSuspensionDistance)
		{
			suspensionDistance = minSuspensionDistance;
		}
		float num = 0f - m_transform.InverseTransformPoint(contactPoint).y;
		return suspensionTravel + radius - (num + suspensionDistance * (1f - suspensionAnchor));
	}

	public Vector3 GetTangentVelocity(Vector3 contactPoint, Vector3 surfaceNormal, Rigidbody surfaceRigidbody)
	{
		Vector3 pointVelocity = m_rigidbody.GetPointVelocity(contactPoint);
		if (surfaceRigidbody != null)
		{
			pointVelocity -= surfaceRigidbody.GetPointVelocity(contactPoint);
		}
		return pointVelocity - Vector3.Project(pointVelocity, surfaceNormal);
	}

	public void ApplyForce(Vector3 force, Vector3 position, Rigidbody otherRb)
	{
		m_rigidbody.AddForceAtPosition(force, position);
		if (otherRb != null && !otherRb.isKinematic)
		{
			float num = Mathf.Clamp01(otherRb.mass / m_rigidbody.mass);
			otherRb.AddForceAtPosition(-force * num, position);
		}
	}

	public WheelCollider GetWheelCollider()
	{
		WheelCollider wheelCollider = GetComponent<WheelCollider>();
		if (wheelCollider == null)
		{
			wheelCollider = SetupWheelCollider();
		}
		return wheelCollider;
	}

	public WheelCollider ResetWheelCollider()
	{
		m_wheelCollider = SetupWheelCollider();
		return m_wheelCollider;
	}

	public WheelCollider SetupWheelCollider()
	{
		WheelCollider wheelCollider = GetComponent<WheelCollider>();
		if (wheelCollider == null)
		{
			wheelCollider = base.gameObject.AddComponent<WheelCollider>();
			wheelCollider.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable;
		}
		WheelFrictionCurve wheelFrictionCurve = default(WheelFrictionCurve);
		wheelFrictionCurve.stiffness = 0f;
		wheelCollider.sidewaysFriction = wheelFrictionCurve;
		wheelCollider.forwardFriction = wheelFrictionCurve;
		wheelCollider.mass = 1f;
		wheelCollider.enabled = base.enabled;
		return wheelCollider;
	}

	public void UpdateWheelCollider(WheelCollider wheelCol)
	{
		wheelCol.motorTorque = (canSleep ? 0f : 1E-05f);
		if (suspensionDistance < minSuspensionDistance)
		{
			suspensionDistance = minSuspensionDistance;
		}
		Vector3 b = center + Vector3.up * suspensionDistance * (1f - suspensionAnchor);
		if (!MathUtility.Vector3Equals(wheelCol.center, b))
		{
			wheelCol.center = b;
		}
		if (runtimeSuspensionTravel < minSuspensionDistance)
		{
			runtimeSuspensionTravel = minSuspensionDistance;
		}
		if (wheelCol.suspensionDistance != runtimeSuspensionTravel)
		{
			wheelCol.suspensionDistance = runtimeSuspensionTravel;
		}
		if (wheelCol.radius != radius)
		{
			wheelCol.radius = radius;
		}
		float num = wheelCol.sprungMass * ((base.vehicle == null) ? 9.80665f : base.vehicle.gravity.Magnitude);
		float num2 = Mathf.Max(num / wheelCol.suspensionDistance, 0.1f);
		JointSpring suspensionSpring = default(JointSpring);
		suspensionSpring.spring = Mathf.Clamp(runtimeSpringRate * scaleFactor, num2, float.PositiveInfinity);
		suspensionSpring.damper = (base.vehicle.advancedSuspensionDamper ? 0f : Mathf.Max(runtimeDamperRate * scaleFactor, 0.1f));
		suspensionSpring.targetPosition = Mathf.Clamp(num / suspensionSpring.spring / wheelCol.suspensionDistance, 0.1f, 1f);
		if (float.IsNaN(suspensionSpring.targetPosition))
		{
			return;
		}
		JointSpring suspensionSpring2 = wheelCol.suspensionSpring;
		if (Mathf.Abs(suspensionSpring.targetPosition - suspensionSpring2.targetPosition) > springSlerpRate)
		{
			if (debugForces)
			{
				Debug.LogFormat("TargetPosition change > " + springSlerpRate);
			}
			_ = num / suspensionSpring.spring / wheelCol.suspensionDistance;
			if (suspensionSpring2.targetPosition > suspensionSpring.targetPosition)
			{
				suspensionSpring.targetPosition = suspensionSpring2.targetPosition - springSlerpRate;
			}
			else
			{
				suspensionSpring.targetPosition = suspensionSpring2.targetPosition + springSlerpRate;
			}
		}
		if (suspensionSpring2.spring != suspensionSpring.spring || suspensionSpring2.damper != suspensionSpring.damper || suspensionSpring2.targetPosition != suspensionSpring.targetPosition)
		{
			if (debugForces)
			{
				Debug.LogFormat("Spring changed. MinSpring:{0} CurrentSpring:{1} NewSpring:{2} CurrentDamper:{3} NewDamper:{4} CurrentTargetPos:{5} NewTargetPos:{6} SprungForce:{7} SuspensionDistance:{8} SprungMass:{9}", num2, suspensionSpring2.spring, suspensionSpring.spring, suspensionSpring2.damper, suspensionSpring.damper, suspensionSpring2.targetPosition, suspensionSpring.targetPosition, num, wheelCol.suspensionDistance, wheelCol.sprungMass);
			}
			wheelCol.suspensionSpring = suspensionSpring;
		}
	}

	public float FixSteerAngle(float inputSteerAngle)
	{
		if (!base.isActiveAndEnabled)
		{
			return inputSteerAngle;
		}
		Vector3 vector = Quaternion.AngleAxis(inputSteerAngle, m_transform.up) * m_transform.forward;
		Vector3 vector2 = vector - Vector3.Project(vector, m_rigidbodyTransform.up);
		return Vector3.Angle(m_rigidbodyTransform.forward, vector2) * Mathf.Sign(Vector3.Dot(m_rigidbodyTransform.right, vector2));
	}

	[ContextMenu("Adjust position and radius to the Wheel mesh")]
	public void AdjustToWheelMesh()
	{
		if (wheelTransform == null)
		{
			DebugLogError("A Wheel transform is required");
			return;
		}
		base.transform.position = wheelTransform.position;
		base.transform.localPosition = MathUtility.RoundDecimals(base.transform.localPosition, 3);
		base.transform.localRotation = wheelTransform.localRotation;
		MeshFilter[] componentsInChildren = wheelTransform.GetComponentsInChildren<MeshFilter>();
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			Bounds scaledBounds = GetScaledBounds(componentsInChildren[0]);
			int i = 1;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				Bounds scaledBounds2 = GetScaledBounds(componentsInChildren[i]);
				scaledBounds.Encapsulate(scaledBounds2.min);
				scaledBounds.Encapsulate(scaledBounds2.max);
			}
			if (Mathf.Abs(scaledBounds.extents.y - scaledBounds.extents.z) > 0.05f)
			{
				DebugLogWarning("The Wheel mesh might not be correct. The calculated radius is different along z and y axis.");
			}
			radius = MathUtility.FloorDecimals(scaledBounds.extents.y, 2);
			if (base.transform.localPosition.x > 0f)
			{
				center.x = MathUtility.RoundDecimals(scaledBounds.center.x + scaledBounds.extents.x * 0.85f, 3);
			}
			else
			{
				center.x = MathUtility.RoundDecimals(scaledBounds.center.x - scaledBounds.extents.x * 0.85f, 3);
			}
		}
		else
		{
			DebugLogWarning("Couldn't calculate radius. There are no meshes in the Wheel transform or its children");
		}
	}

	public Bounds GetScaledBounds(MeshFilter meshFilter)
	{
		Bounds bounds = meshFilter.sharedMesh.bounds;
		Vector3 lossyScale = meshFilter.transform.lossyScale;
		lossyScale.x = Mathf.Abs(lossyScale.x);
		lossyScale.y = Mathf.Abs(lossyScale.y);
		lossyScale.z = Mathf.Abs(lossyScale.z);
		bounds.max = Vector3.Scale(bounds.max, lossyScale);
		bounds.min = Vector3.Scale(bounds.min, lossyScale);
		return bounds;
	}

	public void OnDrawGizmosSelected()
	{
	}
}
