using System;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Utility/Vehicle Joint", 0)]
public class VPVehicleJoint : MonoBehaviour
{
	public enum UpdateMode
	{
		OnEnable,
		OnFixedUpdate,
		OnFixedUpdateInEditorOnly
	}

	public enum MatchInertiaMode
	{
		None,
		ConnectedMasses,
		ConnectedInertia
	}

	public enum DebugLabel
	{
		None,
		ForceAndTorque,
		MassAndInertia
	}

	[Serializable]
	public class JointMotion
	{
		public enum Mode
		{
			Locked,
			Free,
			DampedSpring
		}

		public Mode mode = Mode.Free;

		public float spring;

		public float damper;

		public float maxForce = float.PositiveInfinity;

		public ConfigurableJointMotion GetJointMotion()
		{
			if (mode == Mode.Locked)
			{
				return ConfigurableJointMotion.Locked;
			}
			return ConfigurableJointMotion.Free;
		}

		public JointDrive GetJointDrive()
		{
			JointDrive result = default(JointDrive);
			if (mode == Mode.DampedSpring)
			{
				result.positionSpring = spring;
				result.positionDamper = damper;
				result.maximumForce = maxForce;
			}
			else
			{
				result.maximumForce = float.PositiveInfinity;
			}
			return result;
		}
	}

	[Serializable]
	public class AngularJointMotion
	{
		public enum Mode
		{
			Locked,
			Free,
			DampedSpring,
			Limited
		}

		public Mode mode = Mode.Free;

		public float spring;

		public float damper;

		public float maxForce = float.PositiveInfinity;

		public float maxAngle;

		public float limit;

		public ConfigurableJointMotion GetJointMotion()
		{
			if (mode == Mode.Limited)
			{
				mode = Mode.DampedSpring;
				spring = 0f;
				damper = 0f;
				maxForce = float.PositiveInfinity;
				maxAngle = limit;
			}
			if (mode == Mode.Locked)
			{
				return ConfigurableJointMotion.Locked;
			}
			if (mode == Mode.DampedSpring && maxAngle > 0f)
			{
				return ConfigurableJointMotion.Limited;
			}
			return ConfigurableJointMotion.Free;
		}

		public SoftJointLimit GetJointLimit()
		{
			SoftJointLimit result = default(SoftJointLimit);
			if (mode == Mode.DampedSpring && maxAngle > 0f)
			{
				result.limit = maxAngle;
			}
			return result;
		}

		public JointDrive GetJointDrive()
		{
			JointDrive result = default(JointDrive);
			if (mode == Mode.DampedSpring)
			{
				result.positionSpring = spring;
				result.positionDamper = damper;
				result.maximumForce = maxForce;
			}
			else
			{
				result.maximumForce = float.PositiveInfinity;
			}
			return result;
		}
	}

	public Transform thisAnchor;

	public Transform otherAnchor;

	public UpdateMode updateMode;

	public bool enableCollision = true;

	public bool restorePoseOnDisable;

	[Header("Linear Constraints")]
	public JointMotion xMotion;

	public JointMotion yMotion;

	public JointMotion zMotion;

	[Header("Angular Constraints")]
	public AngularJointMotion angularXMotion;

	public AngularJointMotion angularYMotion;

	public AngularJointMotion angularZMotion;

	[Header("Damped Spring")]
	public Vector3 targetPosition = Vector3.zero;

	public Quaternion targetRotation = Quaternion.identity;

	public bool resetFrameOnEnable;

	[Header("Advanced")]
	public bool propagateIsKinematic;

	public MatchInertiaMode matchInertiaMode;

	[Range(0.1f, 3f)]
	public float matchInertiaFactor = 1f;

	public bool resetInertiaOnDisable;

	[Space(5f)]
	public DebugLabel debugLabel;

	public Rigidbody m_thisRigidbody;

	public Rigidbody m_otherRigidbody;

	public bool m_isChildRigidbody;

	public ConfigurableJoint m_joint;

	public Vector3 m_thisInertiaTensor;

	public Vector3 m_otherInertiaTensor;

	public MatchInertiaMode m_prevInertiaMode;

	public float m_prevInertiaFactor;

	public Vector3 m_referencePosition;

	public Quaternion m_referenceRotation;

	public VehicleBase m_vehicle;

	public Vector3 referencePosition => m_referencePosition;

	public Quaternion referenceRotation => m_referenceRotation;

	public Vector3 thisAnchorPosition
	{
		get
		{
			if (base.enabled)
			{
				return m_joint.anchor;
			}
			if (!(thisAnchor != null))
			{
				return base.transform.localPosition;
			}
			return thisAnchor.localPosition;
		}
		set
		{
			Transform transform = ((thisAnchor != null) ? thisAnchor : base.transform);
			if (base.enabled)
			{
				m_joint.anchor = value;
				transform.position = m_thisRigidbody.transform.TransformPoint(value);
			}
			else
			{
				transform.localPosition = value;
			}
		}
	}

	public Vector3 otherAnchorPosition
	{
		get
		{
			if (base.enabled)
			{
				return m_joint.connectedAnchor;
			}
			if (!(otherAnchor != null))
			{
				return Vector3.zero;
			}
			return otherAnchor.localPosition;
		}
		set
		{
			if (base.enabled)
			{
				m_joint.connectedAnchor = value;
				otherAnchor.position = m_otherRigidbody.transform.TransformPoint(value);
			}
			else if (otherAnchor != null)
			{
				otherAnchor.localPosition = value;
			}
		}
	}

	public void OnEnable()
	{
		if (otherAnchor == null)
		{
			Debug.LogWarning(ToString() + " Other Anchor cannot be null. It's required to attach the joint to. Component disabled.", this);
			base.enabled = false;
			return;
		}
		if (thisAnchor == null)
		{
			thisAnchor = base.transform;
		}
		m_thisRigidbody = GetComponentInParent<Rigidbody>();
		m_otherRigidbody = otherAnchor.GetComponentInParent<Rigidbody>();
		if (m_thisRigidbody == m_otherRigidbody)
		{
			Debug.LogError(ToString() + " Connected bodies are the same rigidbody. Cannot connect a body with itself. Component disabled.", this);
			base.enabled = false;
		}
		else if (!(m_thisRigidbody == null) && !(m_otherRigidbody == null))
		{
			m_isChildRigidbody = m_thisRigidbody.transform.IsChildOf(m_otherRigidbody.transform);
			if (resetFrameOnEnable)
			{
				if (xMotion.mode == JointMotion.Mode.DampedSpring || yMotion.mode == JointMotion.Mode.DampedSpring || zMotion.mode == JointMotion.Mode.DampedSpring)
				{
					base.transform.localPosition = Vector3.zero;
				}
				if (angularXMotion.mode == AngularJointMotion.Mode.DampedSpring || angularYMotion.mode == AngularJointMotion.Mode.DampedSpring || angularYMotion.mode == AngularJointMotion.Mode.DampedSpring)
				{
					base.transform.localRotation = Quaternion.identity;
				}
			}
			m_vehicle = GetComponentInParent<VehicleBase>();
			if (m_vehicle != null && m_vehicle.cachedRigidbody == m_thisRigidbody)
			{
				m_vehicle = null;
			}
			m_referencePosition = base.transform.localPosition;
			m_referenceRotation = base.transform.localRotation;
			m_joint = m_thisRigidbody.gameObject.AddComponent<ConfigurableJoint>();
			m_joint.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable;
			m_joint.autoConfigureConnectedAnchor = false;
			m_joint.connectedBody = m_otherRigidbody;
			m_joint.anchor = m_thisRigidbody.transform.InverseTransformPoint(thisAnchor.position);
			m_joint.connectedAnchor = m_otherRigidbody.transform.InverseTransformPoint(otherAnchor.position);
			ConfigureJoint();
			if (m_isChildRigidbody)
			{
				m_thisRigidbody.interpolation = RigidbodyInterpolation.None;
				if (propagateIsKinematic)
				{
					m_thisRigidbody.isKinematic = m_otherRigidbody.isKinematic;
				}
			}
			MatchInertia(firstRun: true);
			m_thisInertiaTensor = m_thisRigidbody.inertiaTensor;
			m_otherInertiaTensor = m_otherRigidbody.inertiaTensor;
			m_prevInertiaMode = matchInertiaMode;
			m_prevInertiaFactor = matchInertiaFactor;
		}
		else
		{
			Debug.LogWarning(ToString() + " Components to be joined must be active and belong to rigidbody each. Component disabled.", this);
			base.enabled = false;
		}
	}

	public void FixedUpdate()
	{
		if (updateMode != 0 && (updateMode != UpdateMode.OnFixedUpdateInEditorOnly || Application.isEditor))
		{
			ConfigureJoint();
		}
		if (m_isChildRigidbody)
		{
			m_thisRigidbody.interpolation = m_otherRigidbody.interpolation;
			if (propagateIsKinematic)
			{
				m_thisRigidbody.isKinematic = m_otherRigidbody.isKinematic;
			}
		}
		if ((matchInertiaMode != m_prevInertiaMode || matchInertiaMode != 0) && (matchInertiaMode != m_prevInertiaMode || matchInertiaFactor != m_prevInertiaFactor || m_thisRigidbody.inertiaTensor != m_thisInertiaTensor || m_otherRigidbody.inertiaTensor != m_otherInertiaTensor))
		{
			MatchInertia();
			m_thisInertiaTensor = m_thisRigidbody.inertiaTensor;
			m_otherInertiaTensor = m_otherRigidbody.inertiaTensor;
			m_prevInertiaMode = matchInertiaMode;
			m_prevInertiaFactor = matchInertiaFactor;
		}
	}

	public void OnDisable()
	{
		if (resetInertiaOnDisable && m_thisRigidbody != null)
		{
			m_thisRigidbody.ResetInertiaTensor();
		}
		UnityEngine.Object.Destroy(m_joint);
		m_joint = null;
		m_thisRigidbody = null;
		if (restorePoseOnDisable)
		{
			base.transform.localPosition = m_referencePosition;
			base.transform.localRotation = m_referenceRotation;
		}
	}

	public void ConfigureJoint()
	{
		CheckForDeprecations();
		m_joint.xMotion = xMotion.GetJointMotion();
		m_joint.yMotion = yMotion.GetJointMotion();
		m_joint.zMotion = zMotion.GetJointMotion();
		m_joint.angularXMotion = angularXMotion.GetJointMotion();
		m_joint.angularYMotion = angularYMotion.GetJointMotion();
		m_joint.angularZMotion = angularZMotion.GetJointMotion();
		m_joint.xDrive = xMotion.GetJointDrive();
		m_joint.yDrive = yMotion.GetJointDrive();
		m_joint.zDrive = zMotion.GetJointDrive();
		m_joint.angularXDrive = angularXMotion.GetJointDrive();
		if (angularYMotion.mode == AngularJointMotion.Mode.DampedSpring)
		{
			m_joint.angularYZDrive = angularYMotion.GetJointDrive();
		}
		else if (angularZMotion.mode == AngularJointMotion.Mode.DampedSpring)
		{
			m_joint.angularYZDrive = angularZMotion.GetJointDrive();
		}
		else
		{
			m_joint.angularYZDrive = angularYMotion.GetJointDrive();
		}
		SoftJointLimit jointLimit = angularXMotion.GetJointLimit();
		m_joint.highAngularXLimit = jointLimit;
		jointLimit.limit = 0f - jointLimit.limit;
		m_joint.lowAngularXLimit = jointLimit;
		m_joint.angularYLimit = angularYMotion.GetJointLimit();
		m_joint.angularZLimit = angularZMotion.GetJointLimit();
		m_joint.targetPosition = targetPosition;
		m_joint.targetRotation = targetRotation;
		m_joint.enableCollision = enableCollision;
	}

	public void CheckForDeprecations()
	{
		if (angularXMotion.mode == AngularJointMotion.Mode.Limited || angularYMotion.mode == AngularJointMotion.Mode.Limited || angularZMotion.mode == AngularJointMotion.Mode.Limited)
		{
			Debug.LogWarning(ToString() + ": Limited mode is deprecated. Use maxAngle in DampedSpring mode.", base.gameObject);
		}
	}

	public void MatchInertia(bool firstRun = false)
	{
		switch (matchInertiaMode)
		{
		default:
			if (!firstRun)
			{
				m_thisRigidbody.ResetInertiaTensor();
			}
			break;
		case MatchInertiaMode.ConnectedInertia:
		{
			float a2 = m_otherRigidbody.inertiaTensor.magnitude * matchInertiaFactor;
			m_thisRigidbody.inertiaTensor = m_thisRigidbody.inertiaTensor.normalized * Mathf.Max(a2, Vector3.one.magnitude);
			break;
		}
		case MatchInertiaMode.ConnectedMasses:
		{
			float num = m_thisRigidbody.mass / m_otherRigidbody.mass;
			float a = m_otherRigidbody.inertiaTensor.magnitude * num * matchInertiaFactor;
			m_thisRigidbody.inertiaTensor = m_thisRigidbody.inertiaTensor.normalized * Mathf.Max(a, Vector3.one.magnitude);
			break;
		}
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (m_vehicle != null)
		{
			m_vehicle.OnCollision(collision, isCollisionEnter: true);
		}
	}

	public void OnCollisionStay(Collision collision)
	{
		if (m_vehicle != null)
		{
			m_vehicle.OnCollision(collision, isCollisionEnter: false);
		}
	}
}
