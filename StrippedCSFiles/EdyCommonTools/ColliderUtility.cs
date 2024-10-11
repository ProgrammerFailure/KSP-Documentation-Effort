using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

public static class ColliderUtility
{
	public class LayerCollisionMatrix
	{
		private Dictionary<int, int> m_masksByLayer;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LayerCollisionMatrix()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Refresh()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetLayerCollisionMask(int layer)
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Collider[] GetSolidColliders(Transform transform, bool includeInactive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ComputeCentroid(MeshCollider meshCollider, out Vector3 centroid, out float volume)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ComputeCentroid(MeshCollider[] meshColliders, out Vector3 centroid, out float volume)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ComputeCentroidAdditive(MeshCollider meshCollider, ref Vector3 centroid, ref float volume)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DrawColliderGizmos(Collider[] colliders, Color color, bool includeInactiveInHierarchy, bool includeNonConvex)
	{
		throw null;
	}
}
