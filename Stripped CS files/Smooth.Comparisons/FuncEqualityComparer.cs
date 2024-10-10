using System.Collections.Generic;
using Smooth.Collections;
using Smooth.Delegates;

namespace Smooth.Comparisons;

public class FuncEqualityComparer<T> : Smooth.Collections.EqualityComparer<T>
{
	public readonly DelegateFunc<T, T, bool> equals;

	public readonly DelegateFunc<T, int> hashCode;

	public FuncEqualityComparer(DelegateFunc<T, T, bool> equals)
	{
		this.equals = equals;
		hashCode = (typeof(T).IsClass ? ((DelegateFunc<T, int>)((T t) => t?.GetHashCode() ?? 0)) : ((DelegateFunc<T, int>)((T t) => t.GetHashCode())));
	}

	public FuncEqualityComparer(DelegateFunc<T, T, bool> equals, DelegateFunc<T, int> hashCode)
	{
		this.equals = equals;
		this.hashCode = hashCode;
	}

	public FuncEqualityComparer(IEqualityComparer<T> equalityComparer)
	{
		equals = equalityComparer.Equals;
		hashCode = equalityComparer.GetHashCode;
	}

	public override bool Equals(T t1, T t2)
	{
		return equals(t1, t2);
	}

	public override int GetHashCode(T t)
	{
		return hashCode(t);
	}
}
