using System;
using System.Collections.Generic;
using Smooth.Collections;

namespace Smooth.Comparisons;

public class FuncComparer<T> : Smooth.Collections.Comparer<T>
{
	public readonly Comparison<T> comparison;

	public FuncComparer(Comparison<T> comparison)
	{
		this.comparison = comparison;
	}

	public FuncComparer(IComparer<T> comparer)
	{
		comparison = comparer.Compare;
	}

	public override int Compare(T t1, T t2)
	{
		return comparison(t1, t2);
	}
}
