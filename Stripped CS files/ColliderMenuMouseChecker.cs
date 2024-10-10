using UnityEngine;

public class ColliderMenuMouseChecker : MonoBehaviour
{
	public MainMenu mainMenu;

	public bool mouseOver;

	public void OnMouseEnter()
	{
		mouseOver = true;
		MouseIsHovering();
	}

	public void OnMouseExit()
	{
		mouseOver = false;
		MouseHasExited();
	}

	public void MouseIsHovering()
	{
		if ((bool)mainMenu)
		{
			mainMenu.MouseIsHovering(mouseOver);
		}
	}

	public void MouseHasExited()
	{
		if ((bool)mainMenu)
		{
			mainMenu.MouseIsHovering(mouseOver);
		}
	}
}
