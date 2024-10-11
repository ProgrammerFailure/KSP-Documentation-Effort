using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Collections;

namespace Smooth.Compare.Comparers;

public class KeyValuePairComparer<K, V> : Smooth.Collections.Comparer<KeyValuePair<K, V>>
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyValuePairComparer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int Compare(KeyValuePair<K, V> l, KeyValuePair<K, V> r)
	{
		throw null;
	}
}
