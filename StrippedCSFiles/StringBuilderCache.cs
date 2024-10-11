using System;
using System.Runtime.CompilerServices;
using System.Text;

public static class StringBuilderCache
{
	private const int MAX_BUILDER_SIZE = 360;

	[ThreadStatic]
	private static StringBuilder CachedInstance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static StringBuilder Acquire(int capacity = 256)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Release(this StringBuilder sb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ToStringAndRelease(this StringBuilder sb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string Format(string format, params object[] args)
	{
		throw null;
	}
}
