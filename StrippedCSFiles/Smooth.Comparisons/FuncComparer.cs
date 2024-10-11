using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Collections;

namespace Smooth.Comparisons;

public class FuncComparer<T> : Smooth.Collections.Comparer<T>
{
	private readonly Comparison<T> comparison;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FuncComparer(Comparison<T> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FuncComparer(IComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int Compare(T t1, T t2)
	{
		throw null;
	}
}
