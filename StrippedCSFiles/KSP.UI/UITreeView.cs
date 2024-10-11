using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

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

		public List<Item> subItems;

		public bool expandable
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Item(Item parent, string name, string text, Action onExpand, Action onSelect)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CScrollRectToLevelSelection_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ScrollRect scrollRect;

		public RectTransform layoutGroup;

		public UITreeView _003C_003E4__this;

		public float scrollSpeed;

		private RectTransform _003CscrollRectRT_003E5__2;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CScrollRectToLevelSelection_003Ed__21(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public UITreeViewItem itemPrefab;

	public ScrollRect scrollRect;

	private List<Item> items;

	private bool isDirty;

	private Item selectedItem;

	private Coroutine scrollRectRoutine;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UITreeView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Item AddItem(Item parent, string name, string text, Action onExpand, Action onSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Item AddItem(Item newItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Refresh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupItemList(int level, List<Item> items)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearItems(bool clearInternalList = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearItemsRecursive(Item item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggleExpand(Item item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSetExpand(Item item, bool expanded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectFirstItem(bool fireCallback = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectItem(string url, bool fireCallback = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectItem(Item itemToSelect, bool fireCallback = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnselectAllRecursive(Item item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Item FindItemByURL(string url, Item item = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CScrollRectToLevelSelection_003Ed__21))]
	private IEnumerator ScrollRectToLevelSelection(ScrollRect scrollRect, RectTransform layoutGroup, float scrollSpeed = 50f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Item GetItem(string name)
	{
		throw null;
	}
}
