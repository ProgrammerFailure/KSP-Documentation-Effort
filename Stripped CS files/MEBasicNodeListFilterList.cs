using System.Collections.Generic;

public class MEBasicNodeListFilterList<T>
{
	public List<MEBasicNodeListFilter<T>> filters;

	public int Count => filters.Count;

	public MEBasicNodeListFilterList()
	{
		filters = new List<MEBasicNodeListFilter<T>>();
	}

	public void AddFilter(MEBasicNodeListFilter<T> filter)
	{
		filters.Add(filter);
	}

	public void RemoveFilter(MEBasicNodeListFilter<T> filter)
	{
		filters.Remove(filter);
	}

	public void ClearFilter()
	{
		filters.Clear();
	}

	public List<T> GetFilteredList(List<T> list)
	{
		List<T> list2 = new List<T>(list);
		int i = 0;
		for (int count = filters.Count; i < count; i++)
		{
			MEBasicNodeListFilter<T>.FilterList(list2, filters[i].FilterCriteria);
		}
		return list2;
	}

	public void FilterList(List<T> list)
	{
		int i = 0;
		for (int count = filters.Count; i < count; i++)
		{
			MEBasicNodeListFilter<T>.FilterList(list, filters[i].FilterCriteria);
		}
	}

	public override string ToString()
	{
		string text = "MEBasicNodeListFilterList(" + typeof(T).Name + "): " + filters.Count;
		int count = filters.Count;
		for (int i = 0; i < count; i++)
		{
			text = text + "\n  " + filters[i].ToString();
		}
		return text;
	}
}
