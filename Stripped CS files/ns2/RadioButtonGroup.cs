using System.Collections.Generic;
using UnityEngine;

namespace ns2;

public class RadioButtonGroup
{
	public static List<RadioButtonGroup> groups = new List<RadioButtonGroup>();

	public int groupID;

	public List<IRadioButton> buttons = new List<IRadioButton>();

	public RadioButtonGroup(int id)
	{
		groupID = id;
		groups.Add(this);
	}

	~RadioButtonGroup()
	{
		groups.Remove(this);
	}

	public static IRadioButton GetSelected(GameObject go)
	{
		return GetSelected(go.transform.GetHashCode());
	}

	public static IRadioButton GetSelected(int id)
	{
		RadioButtonGroup radioButtonGroup = null;
		int i = 0;
		for (int count = groups.Count; i < count; i++)
		{
			if (groups[i].groupID == id)
			{
				radioButtonGroup = groups[i];
				break;
			}
		}
		if (radioButtonGroup == null)
		{
			return null;
		}
		int count2 = radioButtonGroup.buttons.Count;
		do
		{
			if (count2-- <= 0)
			{
				return null;
			}
		}
		while (!radioButtonGroup.buttons[count2].Value);
		return radioButtonGroup.buttons[count2];
	}

	public static RadioButtonGroup GetGroup(int id)
	{
		RadioButtonGroup radioButtonGroup = null;
		int i = 0;
		for (int count = groups.Count; i < count; i++)
		{
			if (groups[i].groupID == id)
			{
				radioButtonGroup = groups[i];
				break;
			}
		}
		if (radioButtonGroup == null)
		{
			radioButtonGroup = new RadioButtonGroup(id);
		}
		return radioButtonGroup;
	}
}
