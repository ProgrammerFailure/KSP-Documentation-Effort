using System.Runtime.CompilerServices;
using UnityEngine;

public static class TransformExtension
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void NestToParent(this Transform t, Transform newParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void NestToParent(this Transform t, Transform newParent, bool resetParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindChild(this Transform t, string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindChild(this Transform t, string childName, bool findActiveChild)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Transform FindChildRecursive(Transform parent, string childName, bool findActiveChild)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindParent(this Transform t, string parentName, bool findActiveParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Transform FindParentRecursive(Transform parent, string parentName, bool findActiveParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetLayerRecursive(this Transform root, int layer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetShader(this Transform root, string shader)
	{
		throw null;
	}
}
