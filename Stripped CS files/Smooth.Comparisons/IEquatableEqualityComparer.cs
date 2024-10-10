using System;
using Smooth.Collections;

namespace Smooth.Comparisons;

public class IEquatableEqualityComparer<T> : EqualityComparer<T> where T : IEquatable<T>
{
	public override bool Equals(T l, T r)
	{
		return l.Equals(r);
	}

	public override int GetHashCode(T t)
	{
		return t.GetHashCode();
	}
}
