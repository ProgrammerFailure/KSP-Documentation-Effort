using System;
using System.Runtime.CompilerServices;
using Smooth.Collections;

namespace Smooth.Comparisons;

public class IEquatableEqualityComparer<T> : EqualityComparer<T> where T : IEquatable<T>
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public IEquatableEqualityComparer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(T l, T r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode(T t)
	{
		throw null;
	}
}
