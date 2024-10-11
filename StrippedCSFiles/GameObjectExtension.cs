using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameObjectExtension
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T GetComponentCached<T>(this GameObject gameobject, ref T cache) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T GetComponentCached<T>(this Component component, ref T cache) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T AddOrGetComponent<T>(this GameObject obj) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T GetComponentOnParent<T>(this GameObject obj) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Component GetComponentOnParent(this GameObject obj, string type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T GetComponentUpwards<T>(this GameObject obj) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Component GetComponentUpwards(this GameObject obj, string type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetComponentIndex<T>(this GameObject host, T tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameObject GetChild(this GameObject obj, string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetLayerRecursive(this GameObject obj, int layer, int ignoreLayersMask = 0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetLayerRecursive(this GameObject obj, int layer, bool filterTranslucent, int ignoreLayersMask = 0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetTagsRecursive(this GameObject obj, string tag, params string[] ignoreTags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DestroyGameObject(this GameObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DestroyGameObjectImmediate(this GameObject obj)
	{
		throw null;
	}
}
