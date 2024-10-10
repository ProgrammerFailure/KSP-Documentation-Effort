using System;
using System.Collections.Generic;

public class MEBasicNodeListFilter<T>
{
	public Func<T, bool> filterCriteria;

	public string id;

	public Func<T, bool> FilterCriteria => filterCriteria;

	public string String_0 => id;

	public MEBasicNodeListFilter(string filterID, Func<T, bool> criteria)
	{
		id = filterID;
		filterCriteria = criteria;
	}

	public override string ToString()
	{
		return "MEBasicNodeListFilter(" + typeof(T).Name + "): " + id + " '";
	}

	public static void FilterList(List<T> nodes, Func<T, bool> filter)
	{
		int count = nodes.Count;
		while (count-- > 0)
		{
			if (!filter(nodes[count]))
			{
				nodes.RemoveAt(count);
			}
		}
	}
}
