using UnityEngine;
using UnityEngine.UI;

public class UINavExplicit : MonoBehaviour
{
	public Selectable selectable;

	public UINavMouseChecker uiNavMouse;

	public int lastItemSelected;

	public void Start()
	{
		selectable = GetComponent<Selectable>();
		uiNavMouse = GetComponent<UINavMouseChecker>();
		GameEvents.onMenuNavGetInput.Add(CheckForNonInteractable);
	}

	public void OnDestroy()
	{
		GameEvents.onMenuNavGetInput.Remove(CheckForNonInteractable);
	}

	public void CheckForNonInteractable(MenuNavInput input)
	{
		if (uiNavMouse.index == uiNavMouse.menuNav.lastItemSelected)
		{
			if (!selectable.IsInteractable())
			{
				uiNavMouse.menuNav.lastItemSelected = lastItemSelected;
				uiNavMouse.menuNav.SelectLastArrowSelected();
			}
		}
		else
		{
			lastItemSelected = uiNavMouse.menuNav.lastItemSelected;
		}
	}
}
