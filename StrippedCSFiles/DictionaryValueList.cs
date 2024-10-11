using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class DictionaryValueList<TKey, TValue>
{
	private Dictionary<TKey, TValue> dict;

	[SerializeField]
	private List<TValue> list;

	[SerializeField]
	private List<TKey> listKeys;

	public TValue this[TKey key]
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

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Dictionary<TKey, TValue>.KeyCollection Keys
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Dictionary<TKey, TValue>.ValueCollection Values
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal List<TValue> ValuesList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal List<TKey> KeysList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DictionaryValueList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DictionaryValueList(DictionaryValueList<TKey, TValue> old)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Add(TKey key, TValue val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Remove(TKey key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(TKey key, out TValue val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TValue At(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TKey KeyAt(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(TKey key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsKey(TKey key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int IndexOf(TValue val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SortList(Comparison<TValue> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Dictionary<TKey, TValue>.Enumerator GetDictEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<TValue>.Enumerator GetListEnumerator()
	{
		throw null;
	}
}
