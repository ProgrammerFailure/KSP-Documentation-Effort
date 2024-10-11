using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class PartGeometryUtil
{
	public class PosRot
	{
		public Vector3 vector;

		public Quaternion quaternion;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PosRot(Vector3 v, Quaternion q)
		{
			throw null;
		}
	}

	private static List<string> disabledVariantGOs;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds GetRendererBounds(this GameObject p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds GetRendererBoundsWithoutParticles(this GameObject p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds GetColliderBounds(this GameObject p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds GetPartRendererBound(this Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds[] GetRendererBounds(this Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds[] GetColliderBounds(this Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds[] GetPartRendererBounds(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds[] GetPartColliderBounds(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds MergeBounds(Bounds[] bounds, Transform relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static Bounds MergeBounds(Bounds[] bounds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds MergeBounds(Dictionary<Bounds[], PosRot> centeredBounds, PosRot relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 FindBoundsCentroid(Bounds[] bounds, Transform localTo)
	{
		throw null;
	}
}
