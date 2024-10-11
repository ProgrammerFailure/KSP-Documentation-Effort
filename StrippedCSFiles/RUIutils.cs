using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class RUIutils
{
	public class FuncComparer<T> : IComparer<T>
	{
		private readonly Comparison<T> comparison;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FuncComparer(Comparison<T> comparison)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int Compare(T x, T y)
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string CutString(string s, int l, string add)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string DecToHex(int n, bool doubleDigit = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static char HexToChar(string hex, bool doubleDigit = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ColorToHex(Color32 c, bool alpha = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string Repeat(string repeat, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int SortAscDescPrimarySecondary(bool asc, int comp1, int comp2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int SortAscDescPrimarySecondary(bool asc, params int[] comp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 CamToCamReposition(Vector3 originalPos, Camera fromCam, Camera toCam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string StringListToString(List<string> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetYesNoUIString(bool yes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Func<T, bool> And<T>(params Func<T, bool>[] predicates)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool All<T>(List<T> list, params Func<T, bool>[] predicates)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool All<T>(List<T> list, Func<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool All<T>(T[] list, Func<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool Any<T>(List<T> list, Func<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool Any<T>(T[] list, Func<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int Count<T>(T[] list, Func<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<T> Where<T>(List<T> list, Func<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<T> WhereMutating<T>(ref List<T> list, Func<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int Sum<T>(List<T> list, Func<T, int> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Sum<T>(List<T> list, Func<T, double> predicate)
	{
		throw null;
	}
}
