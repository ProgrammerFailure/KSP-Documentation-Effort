using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TMPro;

internal static class TMP_ListPool<T>
{
	private static readonly TMP_ObjectPool<List<T>> s_ListPool;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_ListPool()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<T> Get()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Release(List<T> toRelease)
	{
		throw null;
	}
}
