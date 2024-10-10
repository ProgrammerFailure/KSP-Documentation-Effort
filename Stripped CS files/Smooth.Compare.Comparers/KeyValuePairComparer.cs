using System.Collections.Generic;
using Smooth.Collections;

namespace Smooth.Compare.Comparers;

public class KeyValuePairComparer<T, U> : Smooth.Collections.Comparer<KeyValuePair<T, U>>
{
	public override int Compare(KeyValuePair<T, U> l, KeyValuePair<T, U> r)
	{
		int num = Smooth.Collections.Comparer<T>.Default.Compare(l.Key, r.Key);
		if (num != 0)
		{
			return num;
		}
		return Smooth.Collections.Comparer<U>.Default.Compare(l.Value, r.Value);
	}
}
