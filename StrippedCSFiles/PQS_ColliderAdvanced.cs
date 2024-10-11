using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PQS_ColliderAdvanced : MonoBehaviour
{
	private enum NormSet
	{
		Grav,
		Velocity
	}

	public PQS sphere;

	public Vector2 colliderSize;

	public bool useGravityCollider;

	public Collider gravityCollider;

	private bool isGravityColliderActive;

	public bool useVelocityCollider;

	public Collider velocityCollider;

	public Collider velocityCliffCollider;

	private bool isVelocityColliderActive;

	public bool collidersVisibleDEBUG;

	public bool showNormalsDEBUG;

	private static int maxVelocityIteration;

	private static float velocityThinkAhead;

	private static float halfColliderHeight;

	private static float halfCliffHeight;

	private static double sampleAngle;

	private static QuaternionD[] sampleRotationsD;

	private static List<PQS_ColliderAdvanced> colliderList;

	private Vector3d[] samplesD;

	private Vector3d edgeABd;

	private Vector3d edgeACd;

	private Vector3d normal;

	private Rigidbody _rigidbody;

	private Vector3 relPos;

	private Vector3 relVel;

	private Vector3d relDir;

	private double radDist;

	private double surfaceHeight;

	private double altitude;

	private double radSpeed;

	private Vector3d planeVelocity;

	private double verticalSpeed;

	private Vector3d vel;

	private double speed;

	private Vector3d hitPoint;

	private double surfHeight;

	private GameObject[,] cubes;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQS_ColliderAdvanced()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PQS_ColliderAdvanced()
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
	private bool IterateVelocity(Vector3d start, Vector3d vel, double length, int itr, int maxItr)
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
	private void ShowDebugNormals(int set, Vector3d radPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3d GetNormalAtPoint(Vector3d radian)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnReferenceBodyChange(CelestialBody body)
	{
		throw null;
	}
}
