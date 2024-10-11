using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Delegates;
using Smooth.Events;

namespace Smooth.Compare;

public static class Finder
{
	private const string customConfigurationClassName = "Smooth.Compare.CustomConfiguration";

	public static GenericEvent<ComparerType, EventType, Type> OnEvent;

	private static readonly IEqualityComparer<Type> typeComparer;

	private static readonly Dictionary<Type, object> comparers;

	private static readonly Dictionary<Type, object> equalityComparers;

	private static readonly Configuration config;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Finder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RegisterKeyValuePair<K, V>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RegisterIComparable<T>() where T : IComparable<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RegisterIEquatable<T>() where T : IEquatable<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RegisterIComparableIEquatable<T>() where T : IComparable<T>, IEquatable<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Register<T>(Comparison<T> comparison, DelegateFunc<T, T, bool> equals)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Register<T>(Comparison<T> comparison, DelegateFunc<T, T, bool> equals, DelegateFunc<T, int> hashCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Register<T>(IComparer<T> comparer, IEqualityComparer<T> equalityComparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Register<T>(Comparison<T> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Register<T>(IComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IComparer<T> Comparer<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Register<T>(DelegateFunc<T, T, bool> equals)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Register<T>(DelegateFunc<T, T, bool> equals, DelegateFunc<T, int> hashCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Register<T>(IEqualityComparer<T> equalityComparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IEqualityComparer<T> EqualityComparer<T>()
	{
		throw null;
	}
}
