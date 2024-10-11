using System;
using System.Runtime.CompilerServices;
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

		public Mode mode;

		public float spring;

		public float damper;

		public float maxForce;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public JointMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigurableJointMotion GetJointMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public JointDrive GetJointDrive()
		{
			throw null;
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

		public Mode mode;

		public float spring;

		public float damper;

		public float maxForce;

		public float maxAngle;

		public float limit;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AngularJointMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigurableJointMotion GetJointMotion()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SoftJointLimit GetJointLimit()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public JointDrive GetJointDrive()
		{
			throw null;
		}
	}

	public Transform thisAnchor;

	public Transform otherAnchor;

	public UpdateMode updateMode;

	public bool enableCollision;

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
	public Vector3 targetPosition;

	public Quaternion targetRotation;

	public bool resetFrameOnEnable;

	[Header("Advanced")]
	public bool propagateIsKinematic;

	public MatchInertiaMode matchInertiaMode;

	[Range(0.1f, 3f)]
	public float matchInertiaFactor;

	public bool resetInertiaOnDisable;

	[Space(5f)]
	public DebugLabel debugLabel;

	private Rigidbody m_thisRigidbody;

	private Rigidbody m_otherRigidbody;

	private bool m_isChildRigidbody;

	private ConfigurableJoint m_joint;

	private Vector3 m_thisInertiaTensor;

	private Vector3 m_otherInertiaTensor;

	private MatchInertiaMode m_prevInertiaMode;

	private float m_prevInertiaFactor;

	private Vector3 m_referencePosition;

	private Quaternion m_referenceRotation;

	private VehicleBase m_vehicle;

	public Vector3 referencePosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Quaternion referenceRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 thisAnchorPosition
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

	public Vector3 otherAnchorPosition
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
	public VPVehicleJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfigureJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckForDeprecations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MatchInertia(bool firstRun = false)
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
}
