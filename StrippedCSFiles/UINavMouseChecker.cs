using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class UINavMouseChecker : MonoBehaviour, ISelectHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	public MenuNavigation menuNav;

	public int index;

	private bool mouseOver;

	private bool isSearchField;

	public bool IsSearchField
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UINavMouseChecker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMenuNavReference(MenuNavigation nav)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseIsHovering()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseHasExited()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSelect(BaseEventData eventData)
	{
		throw null;
	}
}
