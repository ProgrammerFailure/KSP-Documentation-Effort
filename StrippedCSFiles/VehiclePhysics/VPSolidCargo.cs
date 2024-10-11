using System.Runtime.CompilerServices;
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
	public Vector3 containerSize;

	[FormerlySerializedAs("cargoMass")]
	public float maxCargoMass;

	[Range(0.001f, 1f)]
	[FormerlySerializedAs("cargoVolume")]
	public float maxCargoVolume;

	public ShapeMode shapeMode;

	[Range(0f, 1f)]
	public float shapeFactor;

	[Space(5f)]
	[Header("Cargo")]
	[Range(0f, 1f)]
	public float cargoLevel;

	[Range(-1f, 1f)]
	public float longitudinalPosition;

	[Range(-1f, 1f)]
	public float lateralPosition;

	[Range(-1f, 1f)]
	public float verticalPosition;

	[Header("Advanced & debug")]
	public bool useContainerInertia;

	private Rigidbody m_rigidbody;

	private Rigidbody m_parentRigidbody;

	private Vector3 m_anchorPosition;

	private Vector3 m_originalPosition;

	private Quaternion m_originalRotation;

	private FixedJoint m_joint;

	private Vector3 m_currentSize;

	public Vector3 localPosition
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

	public Joint joint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Rigidbody cargoRigidbody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPSolidCargo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeforeSerialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnAfterDeserialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
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
	private void ConfigureJointAndBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ScaleVolume(Vector3 volume, float scale, ShapeMode scaleMode, float shapeFactor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ComputeCoM(Vector3 containerSize, Vector3 cargoSize, float lateralPosition, float verticalPosition, float longitudinalPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}
}
