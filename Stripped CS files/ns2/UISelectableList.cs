using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(ScrollRect))]
public class UISelectableList : MonoBehaviour
{
	[Serializable]
	public class SelectableListEventInt : UnityEvent<int>
	{
	}

	[Serializable]
	public class SelectableListEventItem : UnityEvent<UISelectableListItem>
	{
	}

	public float seperation;

	public Scrollbar scrollBar;

	public Button.ButtonClickedEvent OnSelect = new Button.ButtonClickedEvent();

	public SelectableListEventInt OnSelectInt = new SelectableListEventInt();

	public SelectableListEventItem OnSelectItem = new SelectableListEventItem();

	public List<UISelectableListItem> objects = new List<UISelectableListItem>();

	[SerializeField]
	public RectTransform listTransform;

	[SerializeField]
	public ScrollRect scrollRect;

	public UISelectableListItem SelectedItem { get; set; }

	public int SelectedIndex { get; set; }

	public int Count => objects.Count;

	public void OnDestroy()
	{
		scrollRect.verticalScrollbar = null;
	}

	public void UpdateObjects()
	{
		int count = objects.Count;
		float num = 0f;
		for (int i = 0; i < count; i++)
		{
			UISelectableListItem uISelectableListItem = objects[i];
			uISelectableListItem.SetupListItem(this, i);
			RectTransform rectTransform = (RectTransform)uISelectableListItem.transform;
			rectTransform.SetParent(listTransform, worldPositionStays: false);
			num += rectTransform.rect.height;
			rectTransform.anchoredPosition = new Vector3(0f, 0f - num, 0f);
			num += seperation;
		}
		listTransform.sizeDelta = new Vector2(listTransform.sizeDelta.x, num);
	}

	public UISelectableListItem AddItem(UISelectableListItem item)
	{
		objects.Add(item);
		UpdateObjects();
		if (item.Index == 0)
		{
			item.Select();
		}
		return item;
	}

	public UISelectableListItem CreateItem(UISelectableListItem itemPrefab)
	{
		UISelectableListItem uISelectableListItem = UnityEngine.Object.Instantiate(itemPrefab);
		AddItem(uISelectableListItem);
		return uISelectableListItem;
	}

	public UISelectableListItem InsertItem(UISelectableListItem item, int index)
	{
		objects.Insert(index, item);
		UpdateObjects();
		return item;
	}

	public UISelectableListItem CreateAndInsertItem(UISelectableListItem itemPrefab, int index)
	{
		UISelectableListItem uISelectableListItem = UnityEngine.Object.Instantiate(itemPrefab);
		objects.Insert(index, uISelectableListItem);
		UpdateObjects();
		return uISelectableListItem;
	}

	public void RemoveItem(UISelectableListItem item)
	{
		int count = objects.Count;
		do
		{
			if (count-- <= 0)
			{
				return;
			}
		}
		while (!(objects[count] == item));
		UnityEngine.Object.Destroy(objects[count].gameObject);
		objects.RemoveAt(count);
		UpdateObjects();
	}

	public void Clear()
	{
		SelectedIndex = -1;
		SelectedItem = null;
		int count = objects.Count;
		while (count-- > 0)
		{
			UnityEngine.Object.Destroy(objects[count].gameObject);
		}
		objects.Clear();
		UpdateObjects();
	}

	public void Select(int index)
	{
		if (index == SelectedIndex)
		{
			return;
		}
		SelectedIndex = -1;
		SelectedItem = null;
		int count = objects.Count;
		while (count-- > 0)
		{
			if (count == index)
			{
				objects[count].Select();
				SelectedIndex = index;
				SelectedItem = objects[count];
			}
			else
			{
				objects[count].Deselect();
			}
		}
		if (SelectedIndex != -1)
		{
			OnSelect.Invoke();
			OnSelectInt.Invoke(SelectedIndex);
			OnSelectItem.Invoke(SelectedItem);
		}
	}

	public void ClickItem(int index)
	{
		Select(index);
	}
}
