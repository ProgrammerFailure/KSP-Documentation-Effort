using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace KSP.UI;

public class UIList : MonoBehaviour
{
	[Serializable]
	public class ListDropEvent<UIList, UIListItem, integer> : UnityEvent<UIList, UIListItem, integer>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ListDropEvent()
		{
			throw null;
		}
	}

	[SerializeField]
	private Transform customListAnchor;

	private UIList<UIListItem> list;

	[SerializeField]
	private UIListToggleController toggleController;

	public bool AddItemsOnStart;

	public UIListItem placeholder;

	public ListDropEvent<UIList, UIListItem, int> onDrop;

	public Transform ListAnchor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIListToggleController Controller
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAnchor(RectTransform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(UIListItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem GetUilistItemAt(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetIndex(UIListItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetDropItemIndex(UIListItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddItem(UIListItem item, bool forceZ = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InsertItem(UIListItem item, int index, bool forceZ = true, bool worldPositionStays = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveItem(UIListItem item, bool deleteItem = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveItem(int index, bool deleteItem = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveItemAndMove(UIListItem item, Transform newParent, bool forceZ = true, bool worldPositionStays = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SwapItems(UIListItem item1, UIListItem item2, bool forzeZ = true, bool worldPositionStays = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Refresh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetActive(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear(bool destroyElements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearUI(Transform tmpTransform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<UIListData<UIListItem>>.Enumerator GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<UIListItem> GetUiListItems()
	{
		throw null;
	}
}
public class UIList<T>
{
	public RectTransform listAnchor;

	private List<UIListData<T>> items;

	public List<UIListData<T>> Items
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIListData<T> this[int index]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public UIListToggleController Controller
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIList(Transform listAnchor, UIListToggleController controller = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAnchor(Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAnchor(RectTransform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(UIListItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem GetUilistItemAt(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetIndex(UIListItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddItem(UIListData<T> item, bool addToUi = true, bool forceZ = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InsertItem(UIListData<T> item, int index, bool forceZ = true, bool worldPositionStays = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveItem(UIListItem item, bool deleteItem = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveItemAndMove(UIListItem item, Transform newParent, bool forceZ = true, bool worldPositionStays = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveItem(int index, bool deleteItem = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveItem(UIListData<T> item, bool deleteItem = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveItemAndMove(UIListData<T> item, Transform newParent, bool forceZ = true, bool worldPositionStays = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Refresh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetActive(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear(bool destroyElements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearUI(Transform tmpParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Sort(RUIutils.FuncComparer<UIListData<T>> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<UIListData<T>>.Enumerator GetEnumerator()
	{
		throw null;
	}
}
