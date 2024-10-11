using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Collections;

public static class IListExtensions
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<T> Random<T>(this IList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Shuffle<T>(this IList<T> ts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void InsertionSort<T>(this IList<T> ts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void InsertionSort<T>(this IList<T> ts, IComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void InsertionSort<T>(this IList<T> ts, Comparison<T> comparison)
	{
		throw null;
	}
}
