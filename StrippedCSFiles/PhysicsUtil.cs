using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhysicsUtil
{
	public class SphereHit
	{
		public Vector3 position;

		public float distance;

		public Collider collider;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SphereHit(Collider col, Vector3 position, float distance)
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PhysicsUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool HasAncestorTransform(Transform src, Transform ancestor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RaycastHit[] CapsuleCastAllIgnoreSelf(Vector3 p1, Vector3 p2, float capsuleRadius, Vector3 direction, float distance, int layerMask, Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int RaycastHitDistCompare(RaycastHit a, RaycastHit b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RaycastHit[] SphereCastAllIgnoreSelf(Vector3 p1, float radius, Vector3 direction, float distance, int layerMask, Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RaycastHit[] RaycastAllIgnoreSelf(Vector3 origin, Vector3 direction, float distance, int layerMask, Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<SphereHit> SphereSweepTest(Vector3 start, Vector3 forward, float distance, float sweepInterval, float radius, int layerMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<SphereHit> SphereSweepTestWhere(Vector3 start, Vector3 forward, float distance, float sweepInterval, float radius, int layerMask, Func<SphereHit, bool> criteria)
	{
		throw null;
	}
}
