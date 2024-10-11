using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

internal static class SetPropertyUtility
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SetColor(ref Color currentValue, Color newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SetEquatableStruct<T>(ref T currentValue, T newValue) where T : IEquatable<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
	{
		throw null;
	}
}
