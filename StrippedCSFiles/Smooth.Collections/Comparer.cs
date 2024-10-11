using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Collections;

public abstract class Comparer<T> : IComparer<T>
{
	private static IComparer<T> _default;

	public static IComparer<T> Default
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Comparer()
	{
		throw null;
	}

	public abstract int Compare(T lhs, T rhs);
}
