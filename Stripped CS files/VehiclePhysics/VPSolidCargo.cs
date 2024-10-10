using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Solid Cargo", 43)]
public class VPSolidCargo : MonoBehaviour, ISerializationCallbackReceiver
{
	public enum ShapeMode
	{
		PreserveAspect,
		PreserveWidth,
		PreserveHeight,
		PreserveLength
	}

	[Header("Container and limits")]
	public Vector3 containerSize = new Vector3(1.8f, 1.5f, 4f);

	[FormerlySerializedAs("cargoMass")]
	public float maxCargoMass = 1000f;

	[Range(0.001f, 1f)]
	[FormerlySerializedAs("cargoVolume")]
	public float maxCargoVolume = 0.5f;

	public ShapeMode shapeMode;

	[Range(0f, 1f)]
	public float shapeFactor;

	[Space(5f)]
	[Header("Cargo")]
	[Range(0f, 1f)]
	public float cargoLevel = 0.5f;

	[Range(-1f, 1f)]
	public float longitudinalPosition;

	[Range(-1f, 1f)]
	public float lateralPosition;

	[Range(-1f, 1f)]
	public float verticalPosition = -1f;

	[Header("Advanced & debug")]
	public bool useContainerInertia;

	public Rigidbody m_rigidbody;

	public Rigidbody m_parentRigidbody;

	public Vector3 m_anchorPosition;

	public Vector3 m_originalPosition;

	public Quaternion m_originalRotation;

	public FixedJoint m_joint;

	public Vector3 m_currentSize;

	public Vector3 localPosition
	{
		get
		{
			if (base.isActiveAndEnabled)
			{
				return m_originalPosition;
			}
			return base.transform.localPosition;
		}
		set
		{
			if (base.isActiveAndEnabled)
			{
				m_originalPosition = value;
			}
			else
			{
				base.transform.localPosition = value;
			}
		}
	}

	public Joint joint => m_joint;

	public Rigidbody cargoRigidbody => m_rigidbody;

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		if (containerSize.x < 0.1f)
		{
			containerSize.x = 0.1f;
		}
		if (containerSize.y < 0.1f)
		{
			containerSize.y = 0.1f;
		}
		if (containerSize.z < 0.1f)
		{
			containerSize.z = 0.1f;
		}
		if (maxCargoVolume < 0.001f)
		{
			maxCargoVolume = 0.001f;
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
		m_originalPosition = base.transform.localPosition;
		m_originalRotation = base.transform.localRotation;
		m_currentSize = -Vector3.one;
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
				m_joint = base.gameObject.AddComponent<FixedJoint>();
				m_joint.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable;
				m_joint.connectedBody = m_parentRigidbody;
				m_joint.autoConfigureConnectedAnchor = false;
				m_joint.anchor = Vector3.zero;
				ConfigureJointAndBody();
			}
		}
		else
		{
			ConfigureJointAndBody();
		}
	}

	public void ConfigureJointAndBody()
	{
		Vector3 vector = ScaleVolume(containerSize, maxCargoVolume * cargoLevel, shapeMode, shapeFactor);
		Vector3 vector2 = ComputeCoM(containerSize, vector, lateralPosition, verticalPosition, longitudinalPosition);
		m_anchorPosition = m_originalPosition + vector2;
		if (m_joint.connectedAnchor != m_anchorPosition)
		{
			m_joint.connectedAnchor = m_anchorPosition;
			m_rigidbody.position = m_joint.connectedBody.transform.TransformPoint(m_anchorPosition);
		}
		m_rigidbody.interpolation = m_parentRigidbody.interpolation;
		float num = maxCargoMass * cargoLevel;
		if (num < 0.001f)
		{
			num = 0.001f;
		}
		Vector3 vector3 = (useContainerInertia ? containerSize : vector);
		if (vector3 != m_currentSize || num != m_rigidbody.mass)
		{
			m_rigidbody.mass = num;
			m_currentSize = vector3;
			m_rigidbody.inertiaTensor = Inertia.ComputeBoxInertia(vector3, m_rigidbody.mass);
		}
	}

	public static Vector3 ScaleVolume(Vector3 volume, float scale, ShapeMode scaleMode, float shapeFactor)
	{
		if (scale < 0.0001f)
		{
			return Vector3.zero;
		}
		Vector3 result = default(Vector3);
		switch (scaleMode)
		{
		default:
		{
			float num4 = volume.x / volume.y;
			float num5 = volume.z / volume.y;
			float f4 = volume.x * volume.y * volume.z * scale / (num4 * num5);
			result.y = Mathf.Pow(Mathf.Abs(f4), 1f / 3f) * Mathf.Sign(f4);
			result.x = result.y * num4;
			result.z = result.y * num5;
			break;
		}
		case ShapeMode.PreserveWidth:
		{
			float num3 = Mathf.Lerp(volume.z / (volume.y * scale), volume.z * scale / volume.y, shapeFactor);
			float f3 = volume.y * volume.z * scale / num3;
			result.x = volume.x;
			result.y = Mathf.Sqrt(f3);
			result.z = result.y * num3;
			break;
		}
		case ShapeMode.PreserveHeight:
		{
			float num2 = Mathf.Lerp(volume.z / (volume.x * scale), volume.z * scale / volume.x, shapeFactor);
			float f2 = volume.x * volume.z * scale / num2;
			result.x = Mathf.Sqrt(f2);
			result.y = volume.y;
			result.z = result.x * num2;
			break;
		}
		case ShapeMode.PreserveLength:
		{
			float num = Mathf.Lerp(volume.x / (volume.y * scale), volume.x * scale / volume.y, shapeFactor);
			float f = volume.x * volume.y * scale / num;
			result.y = Mathf.Sqrt(f);
			result.x = result.y * num;
			result.z = volume.z;
			break;
		}
		}
		return result;
	}

	public static Vector3 ComputeCoM(Vector3 containerSize, Vector3 cargoSize, float lateralPosition, float verticalPosition, float longitudinalPosition)
	{
		Vector3 result = default(Vector3);
		result.x = lateralPosition * (containerSize.x - cargoSize.x) / 2f;
		result.y = verticalPosition * (containerSize.y - cargoSize.y) / 2f;
		result.z = longitudinalPosition * (containerSize.z - cargoSize.z) / 2f;
		return result;
	}

	public void OnDrawGizmos()
	{
	}
}
