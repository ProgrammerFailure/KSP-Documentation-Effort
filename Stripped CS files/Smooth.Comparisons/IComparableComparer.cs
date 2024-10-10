using System;
using Smooth.Collections;

namespace Smooth.Comparisons;

public class IComparableComparer<T> : Comparer<T> where T : IComparable<T>
{
	public override int Compare(T l, T r)
	{
		return l.CompareTo(r);
	}
}
