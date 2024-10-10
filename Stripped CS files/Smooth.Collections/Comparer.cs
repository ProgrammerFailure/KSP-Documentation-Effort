using System.Collections.Generic;
using Smooth.Compare;

namespace Smooth.Collections;

public abstract class Comparer<T> : IComparer<T>
{
	public static IComparer<T> _default;

	public static IComparer<T> Default
	{
		get
		{
			if (_default == null)
			{
				_default = Finder.Comparer<T>();
			}
			return _default;
		}
		set
		{
			_default = value;
		}
	}

	public Comparer()
	{
	}

	public abstract int Compare(T lhs, T rhs);
}
