using System.Runtime.CompilerServices;
using UnityEngine;

public class CollisionEnhancer : MonoBehaviour
{
	private Vector3 lastPos;

	public static bool bypass;

	public static double UnderTerrainTolerance;

	public static float upFactor;

	public static float minDistSqr;

	public Part part;

	public bool wasPacked;

	public int framesToSkip;

	public float instanceMinDistSqr;

	private RaycastHit hit;

	public CollisionEnhancerBehaviour OnTerrainPunchThrough;

	public float translateBackVelocityFactor;

	private Rigidbody rb;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CollisionEnhancer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CollisionEnhancer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}
}
