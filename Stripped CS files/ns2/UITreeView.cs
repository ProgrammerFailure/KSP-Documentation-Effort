using System;
using System.Collections;
using System.Collections.Generic;
using ns9;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class UITreeView : MonoBehaviour
{
	public class Item
	{
		public Item parent;

		public string name;

		public bool expanded;

		public bool selected;

		public string text;

		public Action onExpand;

		public Action onSelect;

		public UITreeViewItem item;

		public string url;

		public List<Item> subItems = new List<Item>();

		public bool expandable => subItems.Count > 0;

		public Item(Item parent, string name, string text, Action onExpand, Action onSelect)
		{
			this.parent = parent;
			this.name = name;
			if (string.IsNullOrEmpty(text))
			{
				this.text = Localizer.Format("<<1>>", Localizer.Format(name));
			}
			else
			{
				this.text = Localizer.Format("<<1>>", Localizer.Format(text));
			}
			this.onExpand = onExpand;
			this.onSelect = onSelect;
			url = ((parent != null) ? (parent.url + "/") : "") + name;
		}
	}

	public UITreeViewItem itemPrefab;

	public ScrollRect scrollRect;

	public List<Item> items = new List<Item>();

	public bool isDirty = true;

	public Item selectedItem;

	public Coroutine scrollRectRoutine;

	public void Update()
	{
		if (isDirty)
		{
			Refresh();
		}
	}

	public Item AddItem(Item parent, string name, string text, Action onExpand, Action onSelect)
	{
		Item item = new Item(parent, name, text, onExpand, onSelect);
		if (parent == null)
		{
			items.Add(item);
		}
		else
		{
			parent.subItems.Add(item);
		}
		isDirty = true;
		return item;
	}

	public Item AddItem(Item newItem)
	{
		items.Add(newItem);
		isDirty = true;
		return newItem;
	}

	public void Refresh()
	{
		ClearItems(clearInternalList: false);
		SetupItemList(0, items);
		Canvas.ForceUpdateCanvases();
		isDirty = false;
	}

	public void SetupItemList(int level, List<Item> items)
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			Item item = items[i];
			item.item = UnityEngine.Object.Instantiate(itemPrefab);
			item.item.transform.SetParent(base.transform, worldPositionStays: false);
			item.item.Setup(this, item, level, item.expandable, item.expanded, item.text, delegate
			{
				OnToggleExpand(item);
			}, item.onSelect);
			if (item.subItems.Count > 0 && item.expanded)
			{
				SetupItemList(level + 1, item.subItems);
			}
		}
	}

	public void ClearItems(bool clearInternalList = true)
	{
		ClearItemsRecursive(null);
		if (clearInternalList)
		{
			items.Clear();
		}
		isDirty = true;
	}

	public void ClearItemsRecursive(Item item)
	{
		if (item != null)
		{
			if (item.item != null)
			{
				UnityEngine.Object.Destroy(item.item.gameObject);
			}
			int i = 0;
			for (int count = item.subItems.Count; i < count; i++)
			{
				ClearItemsRecursive(item.subItems[i]);
			}
		}
		else
		{
			int j = 0;
			for (int count2 = items.Count; j < count2; j++)
			{
				ClearItemsRecursive(items[j]);
			}
		}
	}

	public void OnToggleExpand(Item item)
	{
		item.expanded = !item.expanded;
		if (item.onExpand != null)
		{
			item.onExpand();
		}
		isDirty = true;
	}

	public void OnSetExpand(Item item, bool expanded)
	{
		if (item.expanded != expanded)
		{
			item.expanded = expanded;
			if (item.onExpand != null)
			{
				item.onExpand();
			}
			if (item.parent != null && !item.parent.expanded)
			{
				OnSetExpand(item.parent, expanded: true);
			}
			isDirty = true;
		}
	}

	public void SelectFirstItem(bool fireCallback = false)
	{
		if (items.Count != 0)
		{
			SelectItem(items[0], fireCallback);
		}
	}

	public void SelectItem(string url, bool fireCallback = false)
	{
		Item item = FindItemByURL(url);
		if (item != null)
		{
			SelectItem(item, fireCallback);
		}
		else
		{
			Debug.Log("KSPedia: Cannot find item of url '" + url + "'");
		}
	}

	public void SelectItem(Item itemToSelect, bool fireCallback = false)
	{
		OnSetExpand(itemToSelect, expanded: true);
		if (isDirty)
		{
			Refresh();
		}
		UnselectAllRecursive(null);
		itemToSelect.selected = true;
		selectedItem = itemToSelect;
		if (itemToSelect.item != null)
		{
			itemToSelect.item.backgroundButton.image.color = itemToSelect.item.colorSelected;
		}
		if (fireCallback && itemToSelect.onSelect != null)
		{
			itemToSelect.onSelect();
		}
		if (selectedItem != null && selectedItem.item != null && scrollRectRoutine == null)
		{
			scrollRectRoutine = StartCoroutine(ScrollRectToLevelSelection(scrollRect, base.transform as RectTransform));
		}
	}

	public void UnselectAllRecursive(Item item)
	{
		if (item != null)
		{
			if (item.item != null)
			{
				item.item.backgroundButton.image.color = item.item.colorUnselected;
			}
			int i = 0;
			for (int count = item.subItems.Count; i < count; i++)
			{
				UnselectAllRecursive(item.subItems[i]);
			}
		}
		else
		{
			int j = 0;
			for (int count2 = items.Count; j < count2; j++)
			{
				UnselectAllRecursive(items[j]);
			}
		}
	}

	public Item FindItemByURL(string url, Item item = null)
	{
		if (item != null)
		{
			if (item.url == url)
			{
				return item;
			}
			int i = 0;
			for (int count = item.subItems.Count; i < count; i++)
			{
				Item item2 = FindItemByURL(url, item.subItems[i]);
				if (item2 != null)
				{
					return item2;
				}
			}
		}
		else
		{
			int j = 0;
			for (int count2 = items.Count; j < count2; j++)
			{
				Item item3 = FindItemByURL(url, items[j]);
				if (item3 != null)
				{
					return item3;
				}
			}
		}
		return null;
	}

	public IEnumerator ScrollRectToLevelSelection(ScrollRect scrollRect, RectTransform layoutGroup, float scrollSpeed = 50f)
	{
		if (!(scrollRect == null) && !(layoutGroup == null))
		{
			RectTransform scrollRectRT = scrollRect.GetComponent<RectTransform>();
			while (selectedItem != null && !(selectedItem.item == null))
			{
				float num = 0f - selectedItem.item.GetComponent<RectTransform>().anchoredPosition.y;
				float num2 = layoutGroup.rect.height / (float)layoutGroup.transform.childCount;
				float height = scrollRectRT.rect.height;
				float y = layoutGroup.anchoredPosition.y;
				float num3 = 0f;
				if (num < y)
				{
					num3 = y - num;
				}
				else if (num + num2 > y + height)
				{
					num3 = y + height - (num + num2);
				}
				float num4 = num3 / layoutGroup.rect.height;
				scrollRect.verticalNormalizedPosition += num4 * Time.deltaTime * scrollSpeed;
				if (Mathf.Abs(num4) < 0.001f)
				{
					scrollRectRoutine = null;
					yield break;
				}
				yield return null;
			}
			scrollRectRoutine = null;
		}
		else
		{
			scrollRectRoutine = null;
		}
	}

	public Item GetItem(string name)
	{
		int num = 0;
		int count = items.Count;
		while (true)
		{
			if (num < count)
			{
				if (items[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return items[num];
	}
}
