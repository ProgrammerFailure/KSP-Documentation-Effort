using System;
using System.Runtime.CompilerServices;

public static class DelegateExtensions
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Clear<T>(this Func<T> f, T defaultValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Clear<T, R>(this Func<T, R> f, R defaultValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Clear<T, U, R>(this Func<T, U, R> f, R defaultValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Clear<T, U, V, R>(this Func<T, U, V, R> f, R defaultValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Clear<T, U, V, W, R>(this Func<T, U, V, W, R> f, R defaultValue)
	{
		throw null;
	}
}
