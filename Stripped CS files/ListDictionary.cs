using System;
using System.Collections.Generic;

[Serializable]
public class ListDictionary<TKey, TValue>
{
	public Dictionary<TKey, List<TValue>> dict;

	public Dictionary<TKey, List<TValue>>.KeyCollection Keys => dict.Keys;

	public Dictionary<TKey, List<TValue>>.ValueCollection Values => dict.Values;

	public List<TValue> this[TKey key]
	{
		get
		{
			if (dict.ContainsKey(key))
			{
				return dict[key];
			}
			return new List<TValue>();
		}
		set
		{
			dict[key] = value;
		}
	}

	public ListDictionary()
	{
		dict = new Dictionary<TKey, List<TValue>>();
	}

	public ListDictionary(ListDictionary<TKey, TValue> old, bool shallow = true)
	{
		if (shallow)
		{
			dict = new Dictionary<TKey, List<TValue>>(old.dict);
			return;
		}
		dict = new Dictionary<TKey, List<TValue>>();
		Dictionary<TKey, List<TValue>>.Enumerator enumerator = old.dict.GetEnumerator();
		while (enumerator.MoveNext())
		{
			dict.Add(enumerator.Current.Key, new List<TValue>(enumerator.Current.Value));
		}
	}

	public void Clear()
	{
		dict.Clear();
	}

	public bool Add(TKey key, TValue val)
	{
		if (dict.TryGetValue(key, out var value))
		{
			if (value.Contains(val))
			{
				return true;
			}
			value.Add(val);
			return false;
		}
		value = new List<TValue>();
		value.Add(val);
		dict.Add(key, value);
		return false;
	}

	public bool Remove(TKey key, TValue val)
	{
		if (dict.TryGetValue(key, out var value))
		{
			int count = value.Count;
			while (count-- > 0)
			{
				if (value[count].Equals(val))
				{
					value.RemoveAt(count);
					return true;
				}
			}
		}
		return false;
	}

	public bool Remove(TKey key)
	{
		return dict.Remove(key);
	}

	public bool TryGetValue(TKey key, out List<TValue> val)
	{
		return dict.TryGetValue(key, out val);
	}

	public Dictionary<TKey, List<TValue>>.Enumerator GetEnumerator()
	{
		return dict.GetEnumerator();
	}
}
