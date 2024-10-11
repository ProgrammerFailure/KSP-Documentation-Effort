using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Collections;

namespace Smooth.Compare.Comparers;

public class KeyValuePairEqualityComparer<K, V> : Smooth.Collections.EqualityComparer<KeyValuePair<K, V>>
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyValuePairEqualityComparer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(KeyValuePair<K, V> l, KeyValuePair<K, V> r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode(KeyValuePair<K, V> kvp)
	{
		throw null;
	}
}
