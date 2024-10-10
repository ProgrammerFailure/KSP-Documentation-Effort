using ns9;
using UnityEngine;

namespace ns11;

public class EditorActionGroup : EditorActionGroup_Base
{
	public int Group { get; set; }

	public void Setup(string groupName, bool contains)
	{
		base.groupName.text = Localizer.Format(groupName);
		if (contains)
		{
			base.groupName.color = Color.yellow;
		}
		else
		{
			base.groupName.color = Color.white;
		}
	}

	public void Setup(KSPActionGroup group, bool contains)
	{
		Group = (int)group;
		_type = EditorActionGroupType.Action;
		Setup(group.displayDescription(), contains);
	}

	public void Setup(KSPAxisGroup group, bool contains)
	{
		Group = (int)group;
		_type = EditorActionGroupType.Axis;
		Setup(group.displayDescription(), contains);
	}
}
