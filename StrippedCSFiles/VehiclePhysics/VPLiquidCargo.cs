using System.Runtime.CompilerServices;
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

	public TankerShape tankerShape;

	public Vector3 tankerSize;

	public float maxCargoMass;

	public float viscosityFactor;

	[Range(0f, 1f)]
	public float fillLevel;

	private Rigidbody m_rigidbody;

	private Rigidbody m_parentRigidbody;

	private Vector3 m_anchorPosition;

	private Vector3 m_originalPosition;

	private Quaternion m_originalRotation;

	private ConfigurableJoint m_joint;

	private SoftJointLimit m_limit;

	private JointDrive m_drive;

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPLiquidCargo()
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
	public static float ContainerRadius(TankerShape tankerShape, Vector3 semiAxes, Vector3 direction, float fillLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float EllipseRadius(float a, float b, float x, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float EllipseHorizontalSemiChord(float a, float b, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float RectangleRadius(float a, float b, float x, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float EllipsoidRadius(Vector3 semiAxes, Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float BoxRadius(Vector3 semiAxes, Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float CylinderRadius(Vector3 semiAxes, Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}
}
