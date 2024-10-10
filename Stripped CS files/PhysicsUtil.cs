using System;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsUtil
{
	public class SphereHit
	{
		public Vector3 position;

		public float distance;

		public Collider collider;

		public SphereHit(Collider col, Vector3 position, float distance)
		{
			collider = col;
			this.position = position;
			this.distance = distance;
		}
	}

	public static bool HasAncestorTransform(Transform src, Transform ancestor)
	{
		if (ancestor == src)
		{
			return true;
		}
		if (src.parent != null)
		{
			return HasAncestorTransform(src.parent, ancestor);
		}
		return false;
	}

	public static RaycastHit[] CapsuleCastAllIgnoreSelf(Vector3 p1, Vector3 p2, float capsuleRadius, Vector3 direction, float distance, int layerMask, Transform transform)
	{
		List<RaycastHit> list = new List<RaycastHit>(Physics.CapsuleCastAll(p1, p2, capsuleRadius, direction, distance, layerMask));
		int count = list.Count;
		while (count-- > 0)
		{
			if (!HasAncestorTransform(list[count].collider.transform, transform))
			{
				list.RemoveAt(count);
			}
		}
		RaycastHit[] array = list.ToArray();
		Array.Sort(array, RaycastHitDistCompare);
		return array;
	}

	public static int RaycastHitDistCompare(RaycastHit a, RaycastHit b)
	{
		return a.distance.CompareTo(b.distance);
	}

	public static RaycastHit[] SphereCastAllIgnoreSelf(Vector3 p1, float radius, Vector3 direction, float distance, int layerMask, Transform transform)
	{
		List<RaycastHit> list = new List<RaycastHit>(Physics.SphereCastAll(p1, radius, direction, distance, layerMask));
		int count = list.Count;
		while (count-- > 0)
		{
			if (!HasAncestorTransform(list[count].collider.transform, transform))
			{
				list.RemoveAt(count);
			}
		}
		RaycastHit[] array = list.ToArray();
		Array.Sort(array, RaycastHitDistCompare);
		return array;
	}

	public static RaycastHit[] RaycastAllIgnoreSelf(Vector3 origin, Vector3 direction, float distance, int layerMask, Transform transform)
	{
		List<RaycastHit> list = new List<RaycastHit>(Physics.RaycastAll(origin, direction, distance, layerMask));
		int count = list.Count;
		while (count-- > 0)
		{
			if (!HasAncestorTransform(list[count].collider.transform, transform))
			{
				list.RemoveAt(count);
			}
		}
		RaycastHit[] array = list.ToArray();
		Array.Sort(array, RaycastHitDistCompare);
		return array;
	}

	public static List<SphereHit> SphereSweepTest(Vector3 start, Vector3 forward, float distance, float sweepInterval, float radius, int layerMask)
	{
		List<SphereHit> list = new List<SphereHit>();
		for (float num = 0f; num <= distance; num += sweepInterval)
		{
			Vector3 position = start + forward * num;
			Collider[] array = Physics.OverlapSphere(position, radius, layerMask);
			foreach (Collider c in array)
			{
				if (list.Find((SphereHit hit) => hit.collider == c) == null)
				{
					list.Add(new SphereHit(c, position, num));
				}
			}
		}
		return list;
	}

	public static List<SphereHit> SphereSweepTestWhere(Vector3 start, Vector3 forward, float distance, float sweepInterval, float radius, int layerMask, Func<SphereHit, bool> criteria)
	{
		List<SphereHit> list = SphereSweepTest(start, forward, distance, sweepInterval, radius, layerMask);
		int count = list.Count;
		while (count-- > 0)
		{
			if (!criteria(list[count]))
			{
				list.RemoveAt(count);
			}
		}
		return list;
	}
}
