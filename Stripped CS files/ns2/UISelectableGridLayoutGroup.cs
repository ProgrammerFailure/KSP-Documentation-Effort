using System.Collections.Generic;
using ns11;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(LayoutGroup))]
public class UISelectableGridLayoutGroup : MonoBehaviour
{
	public delegate void OnGridSelectItem();

	public List<UISelectableGridLayoutGroupItem> objects = new List<UISelectableGridLayoutGroupItem>();

	public LayoutGroup grid;

	public OnGridSelectItem onSelectItem;

	public List<UISelectableGridLayoutGroupItem> Objects => objects;

	public UISelectableGridLayoutGroupItem SelectedItem { get; set; }

	public int SelectedIndex { get; set; }

	public int Count => objects.Count;

	public void Awake()
	{
		grid = GetComponent<LayoutGroup>();
	}

	public UISelectableGridLayoutGroupItem AddItem(UISelectableGridLayoutGroupItem item)
	{
		int count = objects.Count;
		item.transform.SetParent(grid.transform, worldPositionStays: false);
		item.Setup(this, count);
		objects.Add(item);
		if (count == 0)
		{
			SelectedIndex = count;
			SelectedItem = objects[count];
			item.Select();
		}
		else
		{
			item.Deselect();
		}
		return item;
	}

	public UISelectableGridLayoutGroupItem AddItem(UISelectableGridLayoutGroupItem item, Transform parent)
	{
		int count = objects.Count;
		item.itemParent = parent;
		parent.transform.SetParent(grid.transform, worldPositionStays: false);
		item.Setup(this, count);
		objects.Add(item);
		if (count == 0)
		{
			SelectedIndex = count;
			SelectedItem = objects[count];
			item.Select();
		}
		else
		{
			item.Deselect();
		}
		return item;
	}

	public UISelectableGridLayoutGroupItem CreateItem(UISelectableGridLayoutGroupItem itemPrefab)
	{
		UISelectableGridLayoutGroupItem item = Object.Instantiate(itemPrefab);
		return AddItem(item);
	}

	public UISelectableGridLayoutGroupItem InsertItem(UISelectableGridLayoutGroupItem item, int index)
	{
		objects.Insert(index, item);
		int count = Count;
		for (int i = 0; i < count; i++)
		{
			objects[i].transform.parent = null;
		}
		for (int j = 0; j < count; j++)
		{
			objects[j].transform.parent = grid.transform;
		}
		return item;
	}

	public void RemoveItem(UISelectableGridLayoutGroupItem item)
	{
		int count = objects.Count;
		while (count-- > 0)
		{
			if (objects[count] == item)
			{
				if (item.itemParent != null)
				{
					Object.Destroy(objects[count].itemParent.gameObject);
				}
				else
				{
					Object.Destroy(objects[count].gameObject);
				}
				objects.RemoveAt(count);
			}
		}
	}

	public void Clear()
	{
		SelectedIndex = -1;
		SelectedItem = null;
		int count = objects.Count;
		while (count-- > 0)
		{
			if (objects[count].itemParent != null)
			{
				Object.Destroy(objects[count].itemParent.gameObject);
			}
			else
			{
				Object.Destroy(objects[count].gameObject);
			}
		}
		objects.Clear();
	}

	public void Select(UISelectableGridLayoutGroupItem item)
	{
		if (item == SelectedItem)
		{
			return;
		}
		int count = objects.Count;
		while (count-- > 0)
		{
			if (objects[count] == item)
			{
				objects[count].Select();
			}
			else
			{
				objects[count].Deselect();
			}
		}
	}

	public void Select(int itemIndex)
	{
		if (itemIndex == SelectedIndex)
		{
			return;
		}
		SelectedIndex = itemIndex;
		int count = objects.Count;
		while (count-- > 0)
		{
			if (count == itemIndex)
			{
				SelectedItem = objects[count];
				objects[count].Select();
			}
			else
			{
				objects[count].Deselect();
			}
		}
		if (objects.Count > 0 && objects[itemIndex] != null && objects[itemIndex].groupAction)
		{
			SelectSet(objects[itemIndex].mySetIndex, isheader: false);
		}
	}

	public void SelectSet(int itemIndex, bool isheader = true)
	{
		if (!isheader)
		{
			EditorActionGroups.Instance.currentSelectedIndex = itemIndex;
		}
		bool flag = false;
		int num = 0;
		while (true)
		{
			if (num >= objects.Count)
			{
				return;
			}
			if (num == itemIndex)
			{
				SelectedItem = objects[num];
				objects[num].Select();
				flag = true;
			}
			else if (!flag)
			{
				objects[num].Deselect();
			}
			if (flag && num != itemIndex)
			{
				if (objects[num].GetComponent<EditorActionOverrideGroup>() != null)
				{
					break;
				}
				objects[num].Select();
			}
			num++;
		}
		flag = false;
		objects[num].Deselect();
	}

	public void ClickItem(int index)
	{
		Select(index);
		if (onSelectItem != null)
		{
			onSelectItem();
		}
	}
}
