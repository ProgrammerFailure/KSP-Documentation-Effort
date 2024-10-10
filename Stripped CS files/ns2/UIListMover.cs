using System;
using System.Collections.Generic;
using UnityEngine;

namespace ns2;

public class UIListMover : MonoBehaviour
{
	[Serializable]
	public class ListAnchor
	{
		public string anchorName;

		public bool reversedOrder;

		public RectTransform anchor;
	}

	public UIList list;

	public ListAnchor[] anchors;

	[NonSerialized]
	public ListAnchor CurrentAnchor;

	public void SetAnchor(int index)
	{
		if (index < 0 || index >= anchors.Length || (CurrentAnchor != null && CurrentAnchor.anchorName == anchors[index].anchorName))
		{
			return;
		}
		List<UIListItem> list = new List<UIListItem>();
		int i = 0;
		for (int count = this.list.Count; i < count; i++)
		{
			UIListItem uilistItemAt = this.list.GetUilistItemAt(i);
			list.Add(uilistItemAt);
			this.list.RemoveItem(uilistItemAt);
		}
		this.list.SetAnchor(anchors[index].anchor);
		if (anchors[index].reversedOrder)
		{
			int j = 0;
			for (int count2 = list.Count; j < count2; j++)
			{
				this.list.InsertItem(list[j], 0);
			}
		}
		else
		{
			int k = 0;
			for (int count3 = list.Count; k < count3; k++)
			{
				this.list.AddItem(list[k]);
			}
		}
	}

	public void SetAnchor(string name)
	{
		int num = anchors.Length;
		while (num-- > 0)
		{
			if (anchors[num].anchorName == name)
			{
				SetAnchor(num);
			}
		}
	}
}
