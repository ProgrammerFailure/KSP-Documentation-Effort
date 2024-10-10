using System;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Effects/Head Motion", 20)]
public class VPHeadMotion : MonoBehaviour
{
	public enum UpdateMode
	{
		OnEnable,
		OnFixedUpdate,
		OnFixedUpdateInEditorOnly
	}

	[Serializable]
	public class HorizontalMotion
	{
		public enum Mode
		{
			Disabled,
			Tilt,
			Slide
		}

		public Mode mode;

		public float springRate = 50f;

		public float damperRate = 10f;

		public ConfigurableJointMotion GetLinealMotion()
		{
			if (mode == Mode.Slide)
			{
				return ConfigurableJointMotion.Limited;
			}
			return ConfigurableJointMotion.Locked;
		}

		public ConfigurableJointMotion GetAngularMotion()
		{
			if (mode == Mode.Tilt)
			{
				return ConfigurableJointMotion.Limited;
			}
			return ConfigurableJointMotion.Locked;
		}

		public JointDrive GetLinealDrive()
		{
			JointDrive result = default(JointDrive);
			if (mode == Mode.Slide)
			{
				result.positionSpring = springRate;
				result.positionDamper = damperRate;
				result.maximumForce = float.PositiveInfinity;
			}
			else
			{
				result.positionSpring = 0f;
				result.positionDamper = 0f;
			}
			return result;
		}

		public JointDrive GetAngularDrive()
		{
			JointDrive result = default(JointDrive);
			if (mode == Mode.Tilt)
			{
				result.positionSpring = springRate;
				result.positionDamper = damperRate;
				result.maximumForce = float.PositiveInfinity;
			}
			else
			{
				result.positionSpring = 0f;
				result.positionDamper = 0f;
			}
			return result;
		}
	}

	[Serializable]
	public class VerticalMotion
	{
		public enum Mode
		{
			Disabled,
			Slide
		}

		public Mode mode;

		public float springRate = 200f;

		public float damperRate = 10f;

		public ConfigurableJointMotion GetLinealMotion()
		{
			if (mode != 0)
			{
				return ConfigurableJointMotion.Limited;
			}
			return ConfigurableJointMotion.Locked;
		}

		public JointDrive GetLinealDrive()
		{
			JointDrive result = default(JointDrive);
			if (mode != 0)
			{
				result.positionSpring = springRate;
				result.positionDamper = damperRate;
				result.maximumForce = float.PositiveInfinity;
			}
			else
			{
				result.positionSpring = 0f;
				result.positionDamper = 0f;
			}
			return result;
		}
	}

	public UpdateMode updateMode;

	[Header("Inertial motion")]
	public HorizontalMotion longitudinal;

	public HorizontalMotion lateral;

	public VerticalMotion vertical;

	[Space(5f)]
	public float tiltRadius = 0.5f;

	[Space(5f)]
	public float maxDistance = 0.2f;

	public float maxTiltAngle = 10f;

	[Space(5f)]
	public float inertialMass = 0.5f;

	public bool useGravity = true;

	public Rigidbody m_rigidbody;

	public Rigidbody m_parentRigidbody;

	public Vector3 m_anchorPosition;

	public Vector3 m_originalPosition;

	public Quaternion m_originalRotation;

	public ConfigurableJoint m_joint;

	public SoftJointLimit m_limit;

	public Rigidbody motionBody => m_rigidbody;

	public Vector3 localPosition
	{
		get
		{
			return m_anchorPosition;
		}
		set
		{
			m_anchorPosition = value;
			if (m_rigidbody != null)
			{
				ConfigureJointAndBody();
			}
		}
	}

	public void Awake()
	{
		if (GetComponent<Rigidbody>() != null)
		{
			Debug.LogWarning("A Rigidbody is located in this GameObject. This component must be placed at a child GameObject. Component disabled.", this);
			base.enabled = false;
			return;
		}
		m_parentRigidbody = GetComponentInParent<Rigidbody>();
		if (m_parentRigidbody == null)
		{
			Debug.LogWarning("Parent rigidbody not found. This component requires a parent rigidbody to be attached to. Component disabled.", this);
			base.enabled = false;
		}
	}

	public void OnEnable()
	{
		m_originalPosition = base.transform.localPosition;
		m_originalRotation = base.transform.localRotation;
		m_anchorPosition = m_parentRigidbody.transform.InverseTransformPoint(base.transform.position);
	}

	public void OnDisable()
	{
		if (m_rigidbody != null)
		{
			m_rigidbody.isKinematic = true;
		}
		if (m_joint != null)
		{
			m_joint.connectedBody = null;
		}
		UnityEngine.Object.Destroy(m_joint);
		UnityEngine.Object.Destroy(m_rigidbody);
		m_rigidbody = null;
		base.transform.localPosition = m_originalPosition;
		base.transform.localRotation = m_originalRotation;
	}

	public void FixedUpdate()
	{
		if (m_rigidbody == null)
		{
			if (base.gameObject.GetComponent<Rigidbody>() == null)
			{
				m_rigidbody = base.gameObject.AddComponent<Rigidbody>();
				m_rigidbody.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable;
				m_rigidbody.angularDrag = 0f;
				m_rigidbody.velocity = m_parentRigidbody.velocity;
				m_joint = base.gameObject.AddComponent<ConfigurableJoint>();
				m_joint.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable;
				m_joint.connectedBody = m_parentRigidbody;
				m_joint.autoConfigureConnectedAnchor = false;
				ConfigureJointAndBody();
			}
		}
		else
		{
			if (updateMode != 0 && (updateMode != UpdateMode.OnFixedUpdateInEditorOnly || Application.isEditor))
			{
				ConfigureJointAndBody();
			}
			m_rigidbody.interpolation = m_parentRigidbody.interpolation;
		}
	}

	public void ConfigureJointAndBody()
	{
		Vector3 vector = new Vector3(0f, 0f - tiltRadius, 0f);
		Vector3 vector2 = m_anchorPosition + vector;
		if (m_joint.anchor != vector || m_joint.connectedAnchor != vector2)
		{
			m_joint.anchor = vector;
			m_joint.connectedAnchor = vector2;
		}
		m_joint.angularXMotion = longitudinal.GetAngularMotion();
		m_joint.angularXDrive = longitudinal.GetAngularDrive();
		m_joint.zMotion = longitudinal.GetLinealMotion();
		m_joint.zDrive = longitudinal.GetLinealDrive();
		m_joint.angularZMotion = lateral.GetAngularMotion();
		m_joint.angularYZDrive = lateral.GetAngularDrive();
		m_joint.xMotion = lateral.GetLinealMotion();
		m_joint.xDrive = lateral.GetLinealDrive();
		m_joint.yMotion = vertical.GetLinealMotion();
		m_joint.yDrive = vertical.GetLinealDrive();
		m_joint.angularYMotion = ConfigurableJointMotion.Locked;
		m_limit.limit = maxDistance;
		m_joint.linearLimit = m_limit;
		m_limit.limit = maxTiltAngle;
		m_joint.angularZLimit = m_limit;
		m_joint.highAngularXLimit = m_limit;
		m_limit.limit = 0f - maxTiltAngle;
		m_joint.lowAngularXLimit = m_limit;
		m_rigidbody.mass = inertialMass;
		m_rigidbody.useGravity = useGravity;
	}
}
