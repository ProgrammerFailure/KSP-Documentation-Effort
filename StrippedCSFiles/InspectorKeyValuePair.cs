using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public abstract class InspectorKeyValuePair<K, V>
{
	public K key;

	public V value;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InspectorKeyValuePair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InspectorKeyValuePair(K key, V value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyValuePair<K, V> ToKeyValuePair()
	{
		throw null;
	}
}
