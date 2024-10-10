using UnityEngine;
using UnityEngine.EventSystems;

public class UINavMouseChecker : MonoBehaviour, ISelectHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	public MenuNavigation menuNav;

	public int index;

	public bool mouseOver;

	public bool isSearchField;

	public bool IsSearchField
	{
		get
		{
			return isSearchField;
		}
		set
		{
			isSearchField = value;
		}
	}

	public void SetMenuNavReference(MenuNavigation nav)
	{
		if (nav != null)
		{
			menuNav = nav;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!MenuNavigation.blockPointerEnterExit)
		{
			mouseOver = true;
			MouseIsHovering();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!MenuNavigation.blockPointerEnterExit)
		{
			mouseOver = false;
			MouseHasExited();
		}
	}

	public void MouseIsHovering()
	{
		if (menuNav != null)
		{
			menuNav.MouseIsHovering();
		}
	}

	public void MouseHasExited()
	{
		if (menuNav != null)
		{
			menuNav.SelectLastArrowSelected();
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		if ((bool)menuNav && !mouseOver)
		{
			menuNav.SetLastItemSelected(index);
		}
	}
}
