using System.Collections.Generic;

namespace Expansions.Missions.Editor;

public static class MEGUIDropDownItemListExtensions
{
	public static List<string> GetKeys(this List<MEGUIDropDownItem> list)
	{
		List<string> list2 = new List<string>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].key);
		}
		return list2;
	}

	public static List<object> GetValues(this List<MEGUIDropDownItem> list)
	{
		List<object> list2 = new List<object>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].value);
		}
		return list2;
	}

	public static List<string> GetDisplayStrings(this List<MEGUIDropDownItem> list)
	{
		List<string> list2 = new List<string>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].displayString);
		}
		return list2;
	}
}
