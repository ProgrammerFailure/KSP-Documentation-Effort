using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Delegates;

namespace Smooth.Comparisons;

public static class Comparisons
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Comparison<T> Reverse<T>(Comparison<T> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Comparison<T> NullsFirst<T>(Comparison<T> comparison) where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Comparison<T> NullsLast<T>(Comparison<T> comparison) where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Comparison<T?> NullableNullsFirst<T>(Comparison<T> comparison) where T : struct
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Comparison<T?> NullableNullsLast<T>(Comparison<T> comparison) where T : struct
	{
		throw null;
	}
}
public static class Comparisons<T>
{
	private static Dictionary<IComparer<T>, Comparison<T>> toComparison;

	private static Dictionary<IEqualityComparer<T>, DelegateFunc<T, T, bool>> toPredicate;

	public static Comparison<T> Default
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static DelegateFunc<T, T, bool> DefaultPredicate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Comparisons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Comparison<T> ToComparison(IComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DelegateFunc<T, T, bool> ToPredicate(IEqualityComparer<T> equalityComparer)
	{
		throw null;
	}
}
