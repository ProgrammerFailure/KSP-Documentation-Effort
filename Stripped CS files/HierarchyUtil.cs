using System;
using UnityEngine;

public static class HierarchyUtil
{
	public static int GetHierarchyDepth(Part p)
	{
		if (p.parent == null)
		{
			return 0;
		}
		Part parent = p.parent;
		int num = 0;
		while (parent != null)
		{
			num++;
			parent = parent.parent;
		}
		return num;
	}

	public static int GetHierarchyDepth(Transform t)
	{
		if (t.parent == null)
		{
			return 0;
		}
		Transform parent = t.parent;
		int num = 0;
		while (parent != null)
		{
			num++;
			parent = parent.parent;
		}
		return num;
	}

	public static int GetHierarchyDepth<T>(T o, Func<T, T> parentAccessor)
	{
		if (parentAccessor(o) == null)
		{
			return 0;
		}
		T val = parentAccessor(o);
		int num = 0;
		while (val != null)
		{
			num++;
			val = parentAccessor(val);
		}
		return num;
	}

	public static int CompareHierarchyDepths(int d1, int d2)
	{
		return d1.CompareTo(d2);
	}

	public static int CompareHierarchyDepths(Part p1, Part p2)
	{
		return GetHierarchyDepth(p1).CompareTo(GetHierarchyDepth(p2));
	}

	public static int CompareHierarchyDepths(Transform t1, Transform t2)
	{
		return GetHierarchyDepth(t1).CompareTo(GetHierarchyDepth(t2));
	}

	public static int CompareHierarchyDepths<T>(T t1, T t2, Func<T, T> parentAccessor)
	{
		return GetHierarchyDepth(t1, parentAccessor).CompareTo(GetHierarchyDepth(t2, parentAccessor));
	}

	public static bool isAncestor(Part p1, Part p2)
	{
		return CompareHierarchyDepths(p1, p2) < 0;
	}

	public static bool isSameLevel(Part p1, Part p2)
	{
		return CompareHierarchyDepths(p1, p2) == 0;
	}

	public static bool isDescendant(Part p1, Part p2)
	{
		return CompareHierarchyDepths(p1, p2) > 0;
	}

	public static bool isAncestor(Transform t1, Transform t2)
	{
		return CompareHierarchyDepths(t1, t2) < 0;
	}

	public static bool isSameLevel(Transform t1, Transform t2)
	{
		return CompareHierarchyDepths(t1, t2) == 0;
	}

	public static bool isDescendant(Transform t1, Transform t2)
	{
		return CompareHierarchyDepths(t1, t2) > 0;
	}

	public static bool isAncestor<T>(T t1, T t2, Func<T, T> parentAccessor)
	{
		return CompareHierarchyDepths(t1, t2, parentAccessor) < 0;
	}

	public static bool isSameLevel<T>(T t1, T t2, Func<T, T> parentAccessor)
	{
		return CompareHierarchyDepths(t1, t2, parentAccessor) == 0;
	}

	public static bool isDescendant<T>(T t1, T t2, Func<T, T> parentAccessor)
	{
		return CompareHierarchyDepths(t1, t2, parentAccessor) > 0;
	}

	public static string CompileID(Transform trf, string rootName)
	{
		return compileID(trf, "", rootName).Replace(" ", "");
	}

	public static string compileID(Transform trf, string tId, string rootName)
	{
		if (trf.name != rootName)
		{
			if (trf.parent != null)
			{
				tId = tId + compileID(trf.parent, tId, rootName) + "/";
			}
			else if (!string.IsNullOrEmpty(rootName))
			{
				Debug.LogError("[HierarchyUtil]: CompileID failed because it reached the scene root without finding a 'SpaceCenter' game object", trf);
			}
		}
		tId += trf.name;
		return tId;
	}
}
