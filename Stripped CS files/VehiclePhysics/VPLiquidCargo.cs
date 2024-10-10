using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Liquid Cargo", 44)]
public class VPLiquidCargo : MonoBehaviour, ISerializationCallbackReceiver
{
	public enum TankerShape
	{
		Sphere,
		Cylinder,
		Box
	}

	public TankerShape tankerShape = TankerShape.Cylinder;

	public Vector3 tankerSize = new Vector3(1.8f, 1.5f, 4f);

	public float maxCargoMass = 1000f;

	public float viscosityFactor;

	[Range(0f, 1f)]
	public float fillLevel = 0.2f;

	public Rigidbody m_rigidbody;

	public Rigidbody m_parentRigidbody;

	public Vector3 m_anchorPosition;

	public Vector3 m_originalPosition;

	public Quaternion m_originalRotation;

	public ConfigurableJoint m_joint;

	public SoftJointLimit m_limit;

	public JointDrive m_drive;

	public Vector3 localPosition
	{
		get
		{
			if (m_parentRigidbody != null && m_rigidbody == null)
			{
				m_anchorPosition = m_parentRigidbody.transform.InverseTransformPoint(base.transform.position);
			}
			return m_anchorPosition;
		}
		set
		{
			if (m_parentRigidbody != null && m_rigidbody == null)
			{
				m_anchorPosition = m_parentRigidbody.transform.InverseTransformPoint(base.transform.position);
			}
			Vector3 direction = value - m_anchorPosition;
			m_anchorPosition = value;
			if (m_rigidbody != null)
			{
				m_rigidbody.position += base.transform.TransformDirection(direction);
				ConfigureJointAndBody();
			}
			else
			{
				base.transform.position += base.transform.TransformDirection(direction);
			}
		}
	}

	public Joint joint => m_joint;

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		if (tankerSize.x < 0.1f)
		{
			tankerSize.x = 0.1f;
		}
		if (tankerSize.y < 0.1f)
		{
			tankerSize.y = 0.1f;
		}
		if (tankerSize.z < 0.1f)
		{
			tankerSize.z = 0.1f;
		}
	}

	public void Awake()
	{
		if (GetComponent<Rigidbody>() != null)
		{
			Debug.LogWarning(ToString() + ": This GameObject already contains a Rigidbody. This component must be placed at a child GameObject at the center position of the cargo. Component disabled.", this);
			base.enabled = false;
			return;
		}
		m_parentRigidbody = GetComponentInParent<Rigidbody>();
		if (m_parentRigidbody == null)
		{
			Debug.LogWarning(ToString() + ": Parent rigidbody not found. This component requires a parent rigidbody to be attached to. Component disabled.", this);
			base.enabled = false;
		}
	}

	public void OnEnable()
	{
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
		Object.Destroy(m_joint);
		Object.Destroy(m_rigidbody);
		m_rigidbody = null;
		m_joint = null;
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
				m_joint.anchor = Vector3.zero;
				m_joint.xMotion = ConfigurableJointMotion.Limited;
				m_joint.yMotion = ConfigurableJointMotion.Limited;
				m_joint.zMotion = ConfigurableJointMotion.Limited;
				m_joint.angularXMotion = ConfigurableJointMotion.Locked;
				m_joint.angularYMotion = ConfigurableJointMotion.Locked;
				m_joint.angularZMotion = ConfigurableJointMotion.Locked;
				m_drive.maximumForce = m_joint.xDrive.maximumForce;
				m_originalPosition = base.transform.localPosition;
				m_originalRotation = base.transform.localRotation;
				m_anchorPosition = m_parentRigidbody.transform.InverseTransformPoint(base.transform.position);
				ConfigureJointAndBody();
				if (fillLevel < 1f)
				{
					m_limit.limit = ContainerRadius(tankerShape, tankerSize * 0.5f, -Vector3.up, fillLevel);
					m_joint.linearLimit = m_limit;
					m_rigidbody.position = m_joint.connectedBody.transform.TransformPoint(m_joint.connectedAnchor - Vector3.up * m_limit.limit);
				}
			}
		}
		else
		{
			ConfigureJointAndBody();
		}
	}

	public void ConfigureJointAndBody()
	{
		if (m_joint.connectedAnchor != m_anchorPosition)
		{
			m_joint.connectedAnchor = m_anchorPosition;
		}
		m_rigidbody.interpolation = m_parentRigidbody.interpolation;
		m_rigidbody.mass = Mathf.Max(0.0001f, maxCargoMass * fillLevel);
		m_rigidbody.inertiaTensor = Mathf.Max(m_rigidbody.mass * 0.001f * viscosityFactor, 0.001f) * Vector3.one;
		if (fillLevel < 1f)
		{
			Vector3 vector = m_rigidbody.velocity - m_parentRigidbody.GetPointVelocity(m_rigidbody.position);
			Vector3 direction = m_parentRigidbody.transform.InverseTransformPoint(m_rigidbody.position + vector * Time.deltaTime) - m_joint.connectedAnchor;
			Vector3 semiAxes = tankerSize * 0.5f;
			m_limit.limit = ContainerRadius(tankerShape, semiAxes, direction, fillLevel);
		}
		else
		{
			m_limit.limit = 0f;
		}
		m_joint.linearLimit = m_limit;
		m_drive.positionDamper = fillLevel * m_rigidbody.mass * viscosityFactor;
		m_joint.xDrive = m_drive;
		m_joint.yDrive = m_drive;
		m_joint.zDrive = m_drive;
	}

	public static float ContainerRadius(TankerShape tankerShape, Vector3 semiAxes, Vector3 direction, float fillLevel)
	{
		float num = 0f;
		float num2 = EllipsoidRadius(semiAxes, direction);
		switch (tankerShape)
		{
		case TankerShape.Sphere:
			num = num2;
			break;
		case TankerShape.Cylinder:
			num = Mathf.Lerp(CylinderRadius(semiAxes, direction), num2, fillLevel);
			break;
		case TankerShape.Box:
			num = Mathf.Lerp(BoxRadius(semiAxes, direction), num2, fillLevel);
			break;
		}
		return num * Mathf.Clamp01(1f - fillLevel);
	}

	public static float EllipseRadius(float a, float b, float x, float y)
	{
		float num = Mathf.Sqrt(x * x + y * y);
		float num2 = y / num;
		float num3 = x / num;
		float num4 = a * num2;
		float num5 = b * num3;
		return a * b / Mathf.Sqrt(num4 * num4 + num5 * num5);
	}

	public static float EllipseHorizontalSemiChord(float a, float b, float y)
	{
		float num = (b + y) * (b - y);
		if (num < 0f)
		{
			return 0f;
		}
		return a * Mathf.Sqrt(num) / b;
	}

	public static float RectangleRadius(float a, float b, float x, float y)
	{
		float num;
		float num2;
		if (MathUtility.FastAbs(y / x) > MathUtility.FastAbs(b / a))
		{
			num = b * Mathf.Sign(y);
			num2 = num * x / y;
		}
		else
		{
			num2 = a * Mathf.Sign(x);
			num = num2 * y / x;
		}
		return Mathf.Sqrt(num2 * num2 + num * num);
	}

	public static float EllipsoidRadius(Vector3 semiAxes, Vector3 dir)
	{
		float num = semiAxes.x * semiAxes.y;
		float num2 = semiAxes.x * semiAxes.z;
		float num3 = semiAxes.y * semiAxes.z;
		float num4 = num * semiAxes.z;
		float magnitude = dir.magnitude;
		float num5 = dir.z / magnitude;
		float num6 = dir.y / magnitude;
		float num7 = dir.x / magnitude;
		float num8 = num * num5;
		float num9 = num2 * num6;
		float num10 = num3 * num7;
		return num4 / Mathf.Sqrt(num8 * num8 + num9 * num9 + num10 * num10);
	}

	public static float BoxRadius(Vector3 semiAxes, Vector3 dir)
	{
		float b = RectangleRadius(semiAxes.x, semiAxes.y, dir.x, dir.y);
		float y = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);
		return RectangleRadius(semiAxes.z, b, dir.z, y);
	}

	public static float CylinderRadius(Vector3 semiAxes, Vector3 dir)
	{
		float b = EllipseRadius(semiAxes.x, semiAxes.y, dir.x, dir.y);
		float y = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);
		return RectangleRadius(semiAxes.z, b, dir.z, y);
	}

	public void OnDrawGizmos()
	{
	}
}
