using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Util;

public static class RectUtil
{
	public enum ContainmentLevel
	{
		None,
		Partial,
		Full,
		Enclosing
	}

	private static Vector3[] rectCorners;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static RectUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ContainmentLevel GetRectContainment(RectTransform self, RectTransform ctr, Camera refCamera, bool testOppositeCase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 WorldToUISpacePos(Vector3 worldSpacePos, Camera refCamera, RectTransform canvasRect, ref bool zPositive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 WorldToUISpacePos(Vector3 worldSpacePos, Camera refCamera, RectTransform canvasRect, ref bool zPositive, float zFlattenEasing, float zFlattenMidPoint, float zUIstart, float zUIlength)
	{
		throw null;
	}
}
