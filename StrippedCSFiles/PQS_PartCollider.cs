using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Misc/Collider")]
public class PQS_PartCollider : MonoBehaviour
{
	public PQS sphere;

	public Vector2 colliderSize;

	public bool useGravityCollider;

	public bool useVelocityCollider;

	public int layerID;

	[HideInInspector]
	public Collider gravityCollider;

	[HideInInspector]
	public bool isGravityColliderActive;

	[HideInInspector]
	public Collider velocityCollider;

	[HideInInspector]
	public Collider velocityCliffCollider;

	[HideInInspector]
	public bool isVelocityColliderActive;

	[HideInInspector]
	public Part part;

	[HideInInspector]
	public Vector3d[] samplesD;

	[HideInInspector]
	public Collider[] colliders;

	private static Vector3 safeSpot;

	private static List<PQS_PartCollider> colliderList;

	private static Vector3d edgeABd;

	private static Vector3d edgeACd;

	private static Vector3d normal;

	public static int maxVelocityIteration;

	public static double velocityThinkAhead;

	public static double halfColliderHeight;

	public static double halfCliffHeight;

	public static double sampleAngle;

	public static QuaternionD[] sampleRotationsD;

	private Vector3d relPos;

	private Vector3d relVel;

	private Vector3d relDir;

	private double radDist;

	private double surfaceHeight;

	private double altitude;

	private double radSpeed;

	private Vector3d planeVelocity;

	private double verticalSpeed;

	private Vector3d relVelNorm;

	private double speed;

	private Vector3d hitPoint;

	private double surfHeight;

	private int itr;

	private Quaternion q;

	private Rigidbody _rigidbody;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQS_PartCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PQS_PartCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RegisterColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnregisterColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ActivateGravityCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeactivateGravityCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ActivateVelocityCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeactivateVelocityCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTargetSphere(PQS sphere)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGravityCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVelocityCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IterateVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVerticalCollider(Transform col, double surfaceHeight, Vector3d radianNormalized)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateNormalCollider(Transform col, double depth, Vector3d radianNormalized)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetNormalAtPoint(Vector3d radian)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnReferenceBodyChange(CelestialBody body)
	{
		throw null;
	}
}
