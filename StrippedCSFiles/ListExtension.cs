using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class ListExtension
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Shuffle<T>(this IList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T AddUnique<T>(this IList<T> list, T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddUniqueRange<T>(this IList<T> list, IEnumerable<T> items)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddUniqueRange<T>(this IList<T> list, IList<T> items)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MaxAt<T, TKey>(this IList<T> list, Func<T, TKey> sortBy)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MinAt<T, TKey>(this IList<T> list, Func<T, TKey> sortBy)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetRange<T>(this List<T> list, T[] range, int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetRange<T>(this List<T> list, List<T> range, int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetRange<T>(this T[] list, List<T> range, int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetRange<T>(this T[] list, T[] range, int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ContainsId(this List<Part> list, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ContainsId(this List<Part> list, uint id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ContainsId(this HashSet<Part> list, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<T> OrderByAlphaNumeric<T>(this List<T> source, Func<T, string> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T[] OrderByAlphanumeric<T>(this T[] source, Func<T, string> selector)
	{
		throw null;
	}
}
