using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

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

		public UIList list
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

		public List<UIListItem> items
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
		public CascadingListItem(UIList list, bool showing, List<UIListItem> items)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(UIListItem item)
		{
			throw null;
		}
	}

	public delegate List<UIListItem> UpdateBodiesCallback();

	public UIList cascadingList;

	public bool DeleteHeaderOnUpdate;

	public bool DeleteBodyOnUpdate;

	public bool DeleteFooterOnUpdate;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UICascadingList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SegmentHeaderButtonInput(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddSegment(UIListItem item, ref int idx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private CascadingListItem AddSegmentHeader(UIListItem item, Button button, ref int idx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UIListItem AddSegmentBody(UIListItem item, ref int idx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UIListItem AddSegmentFooter(UIListItem item, ref int idx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearList(bool destroy)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CascadingListItem AddCascadingItem(UIListItem header, UIListItem footer, List<UIListItem> bodies, Button button, int index = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int RemoveCascadingItem(CascadingListItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCascadingItem(ref CascadingListItem item, UIListItem header, UIListItem footer, UpdateBodiesCallback callback, Button button, UpdateScrollAction action = UpdateScrollAction.SCROLL_TO_PREVIOUS_POSITION)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CascadingListItem UpdateCascadingItem(CascadingListItem item, UIListItem header, UIListItem footer, List<UIListItem> bodies, Button button, UpdateScrollAction action = UpdateScrollAction.SCROLL_TO_PREVIOUS_POSITION)
	{
		throw null;
	}
}
