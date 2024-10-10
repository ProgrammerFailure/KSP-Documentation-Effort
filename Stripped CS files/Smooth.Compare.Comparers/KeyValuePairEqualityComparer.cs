using System.Collections.Generic;
using Smooth.Collections;

namespace Smooth.Compare.Comparers;

public class KeyValuePairEqualityComparer<T, U> : Smooth.Collections.EqualityComparer<KeyValuePair<T, U>>
{
	public override bool Equals(KeyValuePair<T, U> l, KeyValuePair<T, U> r)
	{
		if (Smooth.Collections.EqualityComparer<T>.Default.Equals(l.Key, r.Key))
		{
			return Smooth.Collections.EqualityComparer<U>.Default.Equals(l.Value, r.Value);
		}
		return false;
	}

	public override int GetHashCode(KeyValuePair<T, U> kvp)
	{
		int num = 17;
		num = 493 + Smooth.Collections.EqualityComparer<T>.Default.GetHashCode(kvp.Key);
		return 29 * num + Smooth.Collections.EqualityComparer<U>.Default.GetHashCode(kvp.Value);
	}
}
