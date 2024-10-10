using System.Collections.Generic;

public class EditorPartListFilterList<T>
{
	public List<EditorPartListFilter<T>> filters;

	public int Count => filters.Count;

	public EditorPartListFilter<T> this[int index] => filters[index];

	public EditorPartListFilter<T> this[string id]
	{
		get
		{
			int num = 0;
			int count = filters.Count;
			EditorPartListFilter<T> editorPartListFilter;
			while (true)
			{
				if (num < count)
				{
					editorPartListFilter = filters[num];
					if (editorPartListFilter.String_0 == id)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return editorPartListFilter;
		}
	}

	public EditorPartListFilterList()
	{
		filters = new List<EditorPartListFilter<T>>();
	}

	public void AddFilter(EditorPartListFilter<T> filter)
	{
		if (!filters.Contains(filter))
		{
			filters.Add(filter);
		}
	}

	public void RemoveFilter(EditorPartListFilter<T> filter)
	{
		if (filters.Contains(filter))
		{
			filters.Remove(filter);
		}
	}

	public void RemoveFilter(string filterID)
	{
		RemoveFilter(this[filterID]);
	}

	public List<EditorPartListFilter<T>>.Enumerator GetEnumerator()
	{
		return filters.GetEnumerator();
	}

	public List<T> GetFilteredList(List<T> list)
	{
		List<T> list2 = new List<T>(list);
		int i = 0;
		for (int count = filters.Count; i < count; i++)
		{
			EditorPartListFilter<T>.FilterList(list2, filters[i].FilterCriteria);
		}
		return list2;
	}

	public void FilterList(List<T> list)
	{
		int i = 0;
		for (int count = filters.Count; i < count; i++)
		{
			EditorPartListFilter<T>.FilterList(list, filters[i].FilterCriteria);
		}
	}

	public string GetFilterKey()
	{
		string text = "";
		int i = 0;
		for (int count = filters.Count; i < count; i++)
		{
			EditorPartListFilter<T> editorPartListFilter = filters[i];
			text += editorPartListFilter.String_0;
		}
		return text;
	}

	public string GetFilterKeySingleOrNothing()
	{
		if (filters.Count == 1)
		{
			return filters[0].String_0;
		}
		return "";
	}

	public override string ToString()
	{
		string text = "EditorPartListFilterList(" + typeof(T).Name + "): " + filters.Count;
		int count = filters.Count;
		for (int i = 0; i < count; i++)
		{
			text = text + "\n  " + filters[i].ToString();
		}
		return text;
	}
}
