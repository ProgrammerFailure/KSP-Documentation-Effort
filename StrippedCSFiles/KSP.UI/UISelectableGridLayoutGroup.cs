using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(LayoutGroup))]
public class UISelectableGridLayoutGroup : MonoBehaviour
{
	public delegate void OnGridSelectItem();

	private List<UISelectableGridLayoutGroupItem> objects;

	private LayoutGroup grid;

	public OnGridSelectItem onSelectItem;

	public List<UISelectableGridLayoutGroupItem> Objects
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UISelectableGridLayoutGroupItem SelectedItem
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
	public UISelectableGridLayoutGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableGridLayoutGroupItem AddItem(UISelectableGridLayoutGroupItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableGridLayoutGroupItem AddItem(UISelectableGridLayoutGroupItem item, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableGridLayoutGroupItem CreateItem(UISelectableGridLayoutGroupItem itemPrefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableGridLayoutGroupItem InsertItem(UISelectableGridLayoutGroupItem item, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveItem(UISelectableGridLayoutGroupItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Select(UISelectableGridLayoutGroupItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Select(int itemIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectSet(int itemIndex, bool isheader = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClickItem(int index)
	{
		throw null;
	}
}
