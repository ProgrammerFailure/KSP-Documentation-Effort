using System;
using System.Runtime.CompilerServices;
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

		public float springRate;

		public float damperRate;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public HorizontalMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigurableJointMotion GetLinealMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigurableJointMotion GetAngularMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public JointDrive GetLinealDrive()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public JointDrive GetAngularDrive()
		{
			throw null;
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

		public float springRate;

		public float damperRate;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VerticalMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigurableJointMotion GetLinealMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public JointDrive GetLinealDrive()
		{
			throw null;
		}
	}

	public UpdateMode updateMode;

	[Header("Inertial motion")]
	public HorizontalMotion longitudinal;

	public HorizontalMotion lateral;

	public VerticalMotion vertical;

	[Space(5f)]
	public float tiltRadius;

	[Space(5f)]
	public float maxDistance;

	public float maxTiltAngle;

	[Space(5f)]
	public float inertialMass;

	public bool useGravity;

	private Rigidbody m_rigidbody;

	private Rigidbody m_parentRigidbody;

	private Vector3 m_anchorPosition;

	private Vector3 m_originalPosition;

	private Quaternion m_originalRotation;

	private ConfigurableJoint m_joint;

	private SoftJointLimit m_limit;

	public Rigidbody motionBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPHeadMotion()
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
}
