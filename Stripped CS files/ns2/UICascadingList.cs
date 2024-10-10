using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class UICascadingList : MonoBehaviour
{
	public enum UpdateScrollAction
	{
		SCROLL_TO_TOP = 1,
		SCROLL_TO_UPDATED,
		SCROLL_TO_PREVIOUS_POSITION
	}

	public class CascadingListItem
	{
		public bool showing;

		public UIListItem header;

		public UIListItem footer;

		public UIList list { get; set; }

		public List<UIListItem> items { get; set; }

		public CascadingListItem(UIList list, bool showing, List<UIListItem> items)
		{
			this.list = list;
			this.showing = showing;
			this.items = items;
		}

		public void Add(UIListItem item)
		{
			items.Add(item);
		}
	}

	public delegate List<UIListItem> UpdateBodiesCallback();

	public UIList cascadingList;

	public bool DeleteHeaderOnUpdate = true;

	public bool DeleteBodyOnUpdate = true;

	public bool DeleteFooterOnUpdate = true;

	public void SegmentHeaderButtonInput(PointerEventData eventData)
	{
		Button component = eventData.pointerPress.GetComponent<Button>();
		UIListItem component2 = component.GetComponent<UIListItem>();
		CascadingListItem cascadingListItem = component.GetComponent<PointerClickHandler>().Data as CascadingListItem;
		int num = cascadingList.GetIndex(component2);
		int count = cascadingListItem.items.Count;
		for (int i = 0; i < count; i++)
		{
			UIListItem item = cascadingListItem.items[i];
			if (cascadingListItem.showing)
			{
				cascadingListItem.list.RemoveItemAndMove(item, base.transform);
			}
			else
			{
				cascadingListItem.list.InsertItem(item, ++num);
			}
		}
		cascadingListItem.showing = !cascadingListItem.showing;
	}

	public void AddSegment(UIListItem item, ref int idx)
	{
		if (idx == -1)
		{
			cascadingList.AddItem(item);
			return;
		}
		cascadingList.InsertItem(item, idx);
		idx++;
	}

	public CascadingListItem AddSegmentHeader(UIListItem item, Button button, ref int idx)
	{
		AddSegment(item, ref idx);
		List<UIListItem> items = new List<UIListItem>();
		CascadingListItem cascadingListItem = new CascadingListItem(cascadingList, showing: true, items);
		cascadingListItem.header = item;
		PointerClickHandler component = button.GetComponent<PointerClickHandler>();
		component.Data = cascadingListItem;
		component.onPointerClick.AddListener(SegmentHeaderButtonInput);
		return cascadingListItem;
	}

	public UIListItem AddSegmentBody(UIListItem item, ref int idx)
	{
		AddSegment(item, ref idx);
		return item;
	}

	public UIListItem AddSegmentFooter(UIListItem item, ref int idx)
	{
		AddSegment(item, ref idx);
		return item;
	}

	public void ClearList(bool destroy)
	{
		cascadingList.Clear(destroy);
	}

	public CascadingListItem AddCascadingItem(UIListItem header, UIListItem footer, List<UIListItem> bodies, Button button, int index = -1)
	{
		int idx = index;
		CascadingListItem cascadingListItem = AddSegmentHeader(header, button, ref idx);
		int i = 0;
		for (int count = bodies.Count; i < count; i++)
		{
			cascadingListItem.Add(AddSegmentBody(bodies[i], ref idx));
		}
		cascadingListItem.footer = AddSegmentFooter(footer, ref idx);
		return cascadingListItem;
	}

	public int RemoveCascadingItem(CascadingListItem item)
	{
		int index = cascadingList.GetIndex(item.header);
		cascadingList.RemoveItem(item.header, DeleteHeaderOnUpdate);
		int count = item.items.Count;
		for (int i = 0; i < count; i++)
		{
			UIListItem uIListItem = item.items[i];
			if (item.showing)
			{
				item.list.RemoveItem(uIListItem, DeleteBodyOnUpdate);
				if (!DeleteBodyOnUpdate)
				{
					uIListItem.transform.SetParent(base.transform, worldPositionStays: false);
				}
			}
		}
		cascadingList.RemoveItem(item.footer, DeleteFooterOnUpdate);
		return index;
	}

	public void UpdateCascadingItem(ref CascadingListItem item, UIListItem header, UIListItem footer, UpdateBodiesCallback callback, Button button, UpdateScrollAction action = UpdateScrollAction.SCROLL_TO_PREVIOUS_POSITION)
	{
		int index = cascadingList.GetIndex(item.header);
		index++;
		if (DeleteHeaderOnUpdate)
		{
			cascadingList.RemoveItem(item.header, deleteItem: true);
			AddSegment(header, ref index);
			item.header = header;
		}
		int count = item.items.Count;
		for (int i = 0; i < count; i++)
		{
			UIListItem item2 = item.items[i];
			if (item.showing)
			{
				if (DeleteBodyOnUpdate)
				{
					item.list.RemoveItem(item2, deleteItem: true);
				}
				else
				{
					item.list.RemoveItemAndMove(item2, base.transform);
				}
			}
		}
		List<UIListItem> list = callback();
		item.items.Clear();
		count = list.Count;
		for (int j = 0; j < count; j++)
		{
			UIListItem item2 = list[j];
			item.Add(item2);
			if (item.showing)
			{
				AddSegmentBody(item2, ref index);
			}
		}
		if (DeleteFooterOnUpdate)
		{
			cascadingList.RemoveItem(item.footer, deleteItem: true);
			AddSegment(footer, ref index);
			item.footer = footer;
		}
		else
		{
			cascadingList.RemoveItem(item.footer);
			AddSegment(footer, ref index);
		}
	}

	public CascadingListItem UpdateCascadingItem(CascadingListItem item, UIListItem header, UIListItem footer, List<UIListItem> bodies, Button button, UpdateScrollAction action = UpdateScrollAction.SCROLL_TO_PREVIOUS_POSITION)
	{
		int index = RemoveCascadingItem(item);
		return AddCascadingItem(header, footer, bodies, button, index);
	}
}
