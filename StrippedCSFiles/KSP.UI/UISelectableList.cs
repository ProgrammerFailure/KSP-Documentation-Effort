using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(ScrollRect))]
public class UISelectableList : MonoBehaviour
{
	[Serializable]
	public class SelectableListEventInt : UnityEvent<int>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public SelectableListEventInt()
		{
			throw null;
		}
	}

	[Serializable]
	public class SelectableListEventItem : UnityEvent<UISelectableListItem>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public SelectableListEventItem()
		{
			throw null;
		}
	}

	public float seperation;

	public Scrollbar scrollBar;

	public Button.ButtonClickedEvent OnSelect;

	public SelectableListEventInt OnSelectInt;

	public SelectableListEventItem OnSelectItem;

	private List<UISelectableListItem> objects;

	[SerializeField]
	private RectTransform listTransform;

	[SerializeField]
	private ScrollRect scrollRect;

	public UISelectableListItem SelectedItem
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

	public int SelectedIndex
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

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableListItem AddItem(UISelectableListItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableListItem CreateItem(UISelectableListItem itemPrefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableListItem InsertItem(UISelectableListItem item, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableListItem CreateAndInsertItem(UISelectableListItem itemPrefab, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveItem(UISelectableListItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Select(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClickItem(int index)
	{
		throw null;
	}
}
