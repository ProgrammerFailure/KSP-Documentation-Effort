using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Collections;

public abstract class EqualityComparer<T> : IEqualityComparer<T>
{
	private static IEqualityComparer<T> _default;

	public static IEqualityComparer<T> Default
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
	protected EqualityComparer()
	{
		throw null;
	}

	public abstract bool Equals(T lhs, T rhs);

	public abstract int GetHashCode(T t);
}
