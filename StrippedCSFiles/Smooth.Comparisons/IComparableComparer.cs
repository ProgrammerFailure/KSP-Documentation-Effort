using System;
using System.Runtime.CompilerServices;
using Smooth.Collections;

namespace Smooth.Comparisons;

public class IComparableComparer<T> : Comparer<T> where T : IComparable<T>
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public IComparableComparer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int Compare(T l, T r)
	{
		throw null;
	}
}
