using System.Collections.Generic;
using Smooth.Compare;

namespace Smooth.Collections;

public abstract class EqualityComparer<T> : IEqualityComparer<T>
{
	public static IEqualityComparer<T> _default;

	public static IEqualityComparer<T> Default
	{
		get
		{
			if (_default == null)
			{
				_default = Finder.EqualityComparer<T>();
			}
			return _default;
		}
		set
		{
			_default = value;
		}
	}

	public EqualityComparer()
	{
	}

	public abstract bool Equals(T lhs, T rhs);

	public abstract int GetHashCode(T t);
}
