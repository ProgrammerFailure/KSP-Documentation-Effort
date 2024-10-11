using System.ComponentModel;
using System.Runtime.CompilerServices;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[DisallowMultipleComponent]
[AddComponentMenu("Vehicle Physics/Wheel Collider", -20)]
public class VPWheelCollider : VehicleBehaviour
{
	public float mass;

	public float radius;

	public Vector3 center;

	[Range(0.01f, 2f)]
	public float suspensionDistance;

	[Range(0f, 1f)]
	public float suspensionAnchor;

	public float springRate;

	public float damperRate;

	public Transform suspensionTransform;

	public Transform caliperTransform;

	public Transform wheelTransform;

	[Range(0f, 0.2f)]
	public float groundPenetration;

	public bool disableSuspensionMovement;

	public bool hideWheelOnDisable;

	public static bool disableSteerAngleFix;

	public static bool disableWheelReferenceFrameFix;

	public static float minSuspensionDistance;

	public static float scaleFactor;

	public int layerMask;

	public bool updateSuspension;

	public bool updateCaliper;

	public bool updateWheel;

	public float suspensionOffset;

	private Transform m_transform;

	private Rigidbody m_rigidbody;

	private Transform m_rigidbodyTransform;

	private RaycastHit m_visualHit;

	private ColliderUtility.LayerCollisionMatrix m_collisionMatrix;

	private WheelCollider m_wheelCollider;

	private float m_steerAngle;

	private float m_angularPosition;

	private float m_contactDistance;

	private Vector3 m_suspensionPosition;

	private InterpolatedFloat m_visualSteerAngle;

	private bool m_isCaliperChildOfSuspension;

	private bool m_isWheelChildOfSuspension;

	private bool m_isWheelChildOfCaliper;

	private float m_2PI;

	internal bool debugForces;

	internal float springSlerpRate;

	public bool hidden
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public Transform cachedTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[DefaultValue(false)]
	public bool visualGrounded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public RaycastHit visualHit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float steerAngle
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

	public float angularVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public float angularPosition
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

	public bool canSleep
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public float effectiveSpringRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float runtimeSpringRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public float runtimeDamperRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public float runtimeSuspensionTravel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public float lastRuntimeSpringRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public float lastRuntimeDamperRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public float lastRuntimeSuspensionTravel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public float runtimeExtraDownforce
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPWheelCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VPWheelCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisableComponent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetUpdateOrder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateVisualWheel(float deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSuspensionForceOffset(float offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetGroundHit(out WheelHit hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetContactDepth(Vector3 contactPoint, float suspensionTravel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetTangentVelocity(Vector3 contactPoint, Vector3 surfaceNormal, Rigidbody surfaceRigidbody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyForce(Vector3 force, Vector3 position, Rigidbody otherRb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelCollider GetWheelCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelCollider ResetWheelCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private WheelCollider SetupWheelCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateWheelCollider(WheelCollider wheelCol)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float FixSteerAngle(float inputSteerAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Adjust position and radius to the Wheel mesh")]
	public void AdjustToWheelMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Bounds GetScaledBounds(MeshFilter meshFilter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmosSelected()
	{
		throw null;
	}
}
