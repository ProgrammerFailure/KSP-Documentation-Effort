using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DictionaryValueList<TKey, TValue>
{
	public Dictionary<TKey, TValue> dict;

	[SerializeField]
	public List<TValue> list;

	[SerializeField]
	public List<TKey> listKeys;

	public TValue this[TKey key]
	{
		get
		{
			return dict[key];
		}
		set
		{
			if (dict.TryGetValue(key, out var value2))
			{
				list.Remove(value2);
				listKeys.Remove(key);
				dict.Remove(key);
			}
			dict.Add(key, value);
			list.Add(value);
			listKeys.Add(key);
		}
	}

	public int Count => list.Count;

	public Dictionary<TKey, TValue>.KeyCollection Keys => dict.Keys;

	public Dictionary<TKey, TValue>.ValueCollection Values => dict.Values;

	public List<TValue> ValuesList => list;

	public List<TKey> KeysList => listKeys;

	public DictionaryValueList()
	{
		dict = new Dictionary<TKey, TValue>();
		list = new List<TValue>();
		listKeys = new List<TKey>();
	}

	public DictionaryValueList(DictionaryValueList<TKey, TValue> old)
	{
		list = new List<TValue>(old.list);
		listKeys = new List<TKey>(old.listKeys);
		dict = new Dictionary<TKey, TValue>(old.dict);
	}

	public void Clear()
	{
		list.Clear();
		listKeys.Clear();
		dict.Clear();
	}

	public bool Add(TKey key, TValue val)
	{
		bool result = false;
		if (dict.TryGetValue(key, out var value))
		{
			list.Remove(value);
			listKeys.Remove(key);
			dict.Remove(key);
			result = true;
		}
		list.Add(val);
		listKeys.Add(key);
		dict.Add(key, val);
		return result;
	}

	public bool Remove(TKey key)
	{
		if (dict.TryGetValue(key, out var value))
		{
			list.Remove(value);
			listKeys.Remove(key);
			dict.Remove(key);
			return true;
		}
		return false;
	}

	public bool TryGetValue(TKey key, out TValue val)
	{
		return dict.TryGetValue(key, out val);
	}

	public TValue At(int index)
	{
		return list[index];
	}

	public TKey KeyAt(int index)
	{
		return listKeys[index];
	}

	public bool Contains(TKey key)
	{
		return dict.ContainsKey(key);
	}

	public bool ContainsKey(TKey key)
	{
		return dict.ContainsKey(key);
	}

	public int IndexOf(TValue val)
	{
		return list.IndexOf(val);
	}

	public void SortList(Comparison<TValue> comparison)
	{
		list.Sort(comparison);
	}

	public Dictionary<TKey, TValue>.Enumerator GetDictEnumerator()
	{
		return dict.GetEnumerator();
	}

	public List<TValue>.Enumerator GetListEnumerator()
	{
		return list.GetEnumerator();
	}
}
