using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ns2;

public class UIList : MonoBehaviour
{
	[Serializable]
	public class ListDropEvent<UIList, UIListItem, integer> : UnityEvent<UIList, UIListItem, integer>
	{
	}

	[SerializeField]
	public Transform customListAnchor;

	public UIList<UIListItem> list;

	[SerializeField]
	public UIListToggleController toggleController;

	public bool AddItemsOnStart;

	public UIListItem placeholder;

	public ListDropEvent<UIList, UIListItem, int> onDrop = new ListDropEvent<UIList, UIListItem, int>();

	public Transform ListAnchor
	{
		get
		{
			if (customListAnchor != null)
			{
				return customListAnchor;
			}
			return base.transform;
		}
	}

	public int Count => list.Count;

	public UIListToggleController Controller => toggleController;

	public void Awake()
	{
		list = new UIList<UIListItem>(ListAnchor, toggleController);
		if (!AddItemsOnStart)
		{
			return;
		}
		for (int i = 0; i < ListAnchor.childCount; i++)
		{
			UIListItem component = ListAnchor.GetChild(i).GetComponent<UIListItem>();
			if (component == null)
			{
				ListAnchor.gameObject.DestroyGameObject();
			}
			else
			{
				list.AddItem(new UIListData<UIListItem>(null, component), addToUi: false);
			}
		}
	}

	public void SetAnchor(RectTransform t)
	{
		list.SetAnchor(t);
	}

	public bool Contains(UIListItem item)
	{
		return list.Contains(item);
	}

	public UIListItem GetUilistItemAt(int index)
	{
		return list.GetUilistItemAt(index);
	}

	public int GetIndex(UIListItem item)
	{
		return list.GetIndex(item);
	}

	public int GetDropItemIndex(UIListItem item)
	{
		if (item == null)
		{
			return -1;
		}
		float num = float.MaxValue;
		int result = 0;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			UIListData<UIListItem> uIListData = list[i];
			if (uIListData != null)
			{
				float sqrMagnitude = (item.transform.position - uIListData.listItem.transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = GetIndex(uIListData.listItem);
				}
			}
		}
		return result;
	}

	public void AddItem(UIListItem item, bool forceZ = true)
	{
		list.AddItem(new UIListData<UIListItem>(null, item), addToUi: true, forceZ);
	}

	public void InsertItem(UIListItem item, int index, bool forceZ = true, bool worldPositionStays = false)
	{
		list.InsertItem(new UIListData<UIListItem>(null, item), index, forceZ, worldPositionStays);
	}

	public bool RemoveItem(UIListItem item, bool deleteItem = false)
	{
		return list.RemoveItem(item, deleteItem);
	}

	public bool RemoveItem(int index, bool deleteItem = false)
	{
		return list.RemoveItem(index, deleteItem);
	}

	public bool RemoveItemAndMove(UIListItem item, Transform newParent, bool forceZ = true, bool worldPositionStays = false)
	{
		return list.RemoveItemAndMove(item, newParent, forceZ, worldPositionStays);
	}

	public void SwapItems(UIListItem item1, UIListItem item2, bool forzeZ = true, bool worldPositionStays = false)
	{
		int index = GetIndex(item1);
		int index2 = GetIndex(item2);
		if (!(item1 == item2) && index != index2)
		{
			RemoveItem(item1);
			RemoveItem(item2);
			if (index > index2)
			{
				InsertItem(item1, index2, forzeZ, worldPositionStays);
				InsertItem(item2, index, forzeZ, worldPositionStays);
			}
			else
			{
				InsertItem(item2, index, forzeZ, worldPositionStays);
				InsertItem(item1, index2, forzeZ, worldPositionStays);
			}
		}
	}

	public void Refresh()
	{
		list.Refresh();
	}

	public void SetActive(bool active)
	{
		list.SetActive(active);
	}

	public void Clear(bool destroyElements)
	{
		list.Clear(destroyElements);
	}

	public void ClearUI(Transform tmpTransform)
	{
		list.ClearUI(tmpTransform);
	}

	public List<UIListData<UIListItem>>.Enumerator GetEnumerator()
	{
		return list.GetEnumerator();
	}

	public List<UIListItem> GetUiListItems()
	{
		List<UIListItem> list = new List<UIListItem>();
		int i = 0;
		for (int count = this.list.Count; i < count; i++)
		{
			list.Add(this.list.GetUilistItemAt(i));
		}
		return list;
	}
}
public class UIList<T>
{
	public RectTransform listAnchor;

	public List<UIListData<T>> items = new List<UIListData<T>>();

	public List<UIListData<T>> Items => items;

	public int Count => items.Count;

	public UIListData<T> this[int index]
	{
		get
		{
			return items[index];
		}
		set
		{
			items[index] = value;
		}
	}

	public UIListToggleController Controller { get; set; }

	public UIList(Transform listAnchor, UIListToggleController controller = null)
	{
		this.listAnchor = (RectTransform)listAnchor;
		Controller = controller;
	}

	public void SetAnchor(Transform t)
	{
		listAnchor = (RectTransform)t;
	}

	public void SetAnchor(RectTransform t)
	{
		listAnchor = t;
	}

	public bool Contains(UIListItem item)
	{
		return items.Find((UIListData<T> a) => a.listItem == item) != null;
	}

	public UIListItem GetUilistItemAt(int index)
	{
		if (items.Count <= index)
		{
			return null;
		}
		return items[index].listItem;
	}

	public int GetIndex(UIListItem item)
	{
		return items.FindIndex((UIListData<T> a) => a.listItem == item);
	}

	public void AddItem(UIListData<T> item, bool addToUi = true, bool forceZ = true)
	{
		items.Add(item);
		if (addToUi)
		{
			item.listItem.transform.SetParent(listAnchor, worldPositionStays: false);
			item.listItem.gameObject.SetActive(value: true);
			if (forceZ)
			{
				item.listItem.transform.localPosition = new Vector3(item.listItem.transform.localPosition.x, item.listItem.transform.localPosition.y, -1f);
			}
		}
	}

	public void InsertItem(UIListData<T> item, int index, bool forceZ = true, bool worldPositionStays = false)
	{
		items.Insert(index, item);
		item.listItem.transform.SetParent(listAnchor, worldPositionStays);
		item.listItem.gameObject.SetActive(value: true);
		if (forceZ)
		{
			item.listItem.transform.localPosition = new Vector3(item.listItem.transform.localPosition.x, item.listItem.transform.localPosition.y, -1f);
		}
		Refresh();
	}

	public bool RemoveItem(UIListItem item, bool deleteItem = false)
	{
		return RemoveItem(items.Find((UIListData<T> a) => a.listItem == item), deleteItem);
	}

	public bool RemoveItemAndMove(UIListItem item, Transform newParent, bool forceZ = true, bool worldPositionStays = false)
	{
		return RemoveItemAndMove(items.Find((UIListData<T> a) => a.listItem == item), newParent, forceZ, worldPositionStays);
	}

	public bool RemoveItem(int index, bool deleteItem = false)
	{
		return RemoveItem(items[index], deleteItem);
	}

	public bool RemoveItem(UIListData<T> item, bool deleteItem = false)
	{
		if (!items.Contains(item))
		{
			Debug.LogWarning("UIList: RemoveItem didn't find any item to remove.");
			return false;
		}
		items.Remove(item);
		if (deleteItem)
		{
			item.listItem.gameObject.DestroyGameObject();
		}
		else
		{
			item.listItem.transform.SetParent(null);
		}
		return true;
	}

	public bool RemoveItemAndMove(UIListData<T> item, Transform newParent, bool forceZ = true, bool worldPositionStays = false)
	{
		if (!items.Contains(item))
		{
			Debug.LogWarning("UIList: RemoveItem didn't find any item to remove.");
			return false;
		}
		items.Remove(item);
		item.listItem.transform.SetParent(newParent, worldPositionStays);
		if (forceZ)
		{
			item.listItem.transform.SetLocalPositionZ(-1f);
		}
		return true;
	}

	public void Refresh()
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			if (items[i].listItem == null && listAnchor == null)
			{
				Debug.LogError("Null!!! what the?");
			}
			if (items[i].listItem != null && listAnchor != null)
			{
				items[i].listItem.transform.SetParent(listAnchor, worldPositionStays: false);
			}
		}
		int j = 0;
		for (int count2 = items.Count; j < count2; j++)
		{
			if (items[j].listItem != null)
			{
				items[j].listItem.transform.SetSiblingIndex(j);
			}
		}
	}

	public void SetActive(bool active)
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			items[i].listItem.gameObject.SetActive(active);
		}
	}

	public void Clear(bool destroyElements)
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			if (destroyElements)
			{
				items[i].listItem.gameObject.DestroyGameObject();
			}
			else
			{
				items[i].listItem.gameObject.SetActive(value: false);
			}
		}
		items.Clear();
	}

	public void ClearUI(Transform tmpParent)
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			items[i].listItem.transform.SetParent(tmpParent, worldPositionStays: false);
		}
	}

	public void Sort(RUIutils.FuncComparer<UIListData<T>> comparer)
	{
		items.Sort(comparer);
		Refresh();
	}

	public List<UIListData<T>>.Enumerator GetEnumerator()
	{
		return items.GetEnumerator();
	}
}
