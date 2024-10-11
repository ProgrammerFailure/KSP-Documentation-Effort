using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EdyCommonTools;

public static class ObjectUtility
{
	public delegate TResult Func<T1, TResult>(T1 arg1);

	private static Dictionary<Type, Delegate> _cachedIL;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ObjectUtility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T CloneObject<T>(T source)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T CloneObjectFast<T>(T myObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CopyObjectOverwrite<T>(T source, ref T target)
	{
		throw null;
	}
}
