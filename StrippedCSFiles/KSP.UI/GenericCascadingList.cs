using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public class GenericCascadingList : MonoBehaviour
{
	private class CollapsableBody
	{
		public TextMeshProUGUI text;

		public string label;

		public string label2;

		public bool collapsed;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CollapsableBody(TextMeshProUGUI text, string label, string label2)
		{
			throw null;
		}
	}

	public UIListItem cascadeHeader;

	public UIListItem cascadeFooter;

	public UIListItem cascadeBody;

	public UIListItem cascadeBody_keyValue;

	public UICascadingList ruiList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GenericCascadingList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(UIList scrollList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem CreateHeader(string label, out Button button, bool scaleBg = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem_spacer CreateBody_spacer(UIListItem_spacer prefab, string label, int space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem_spacer CreateBodyCollapsable_spacer(UIListItem_spacer prefab, string label, string label2, int maxWidth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInputCollapsableBody_spacer(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem CreateBodyKeyValueAutofit(string key, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem CreateBody(string label)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem CreateBody(UIListItem prefab, string label)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem CreateBody(string key, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem CreateBodyCollapsable(string label, string label2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem CreateBodyCollapsable(UIListItem prefab, string label, string label2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateBodyCollapsable(UIListItem lic, string label, string label2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInputCollapsableBody(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem CreateFooter()
	{
		throw null;
	}
}
