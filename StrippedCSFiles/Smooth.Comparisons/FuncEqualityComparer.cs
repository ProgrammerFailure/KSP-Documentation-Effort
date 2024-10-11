using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Collections;
using Smooth.Delegates;

namespace Smooth.Comparisons;

public class FuncEqualityComparer<T> : Smooth.Collections.EqualityComparer<T>
{
	private readonly DelegateFunc<T, T, bool> equals;

	private readonly DelegateFunc<T, int> hashCode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FuncEqualityComparer(DelegateFunc<T, T, bool> equals)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FuncEqualityComparer(DelegateFunc<T, T, bool> equals, DelegateFunc<T, int> hashCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FuncEqualityComparer(IEqualityComparer<T> equalityComparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(T t1, T t2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode(T t)
	{
		throw null;
	}
}
