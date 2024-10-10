using UnityEngine;

public class MonoUtilities : MonoBehaviour
{
	public static void RefreshContextWindows(Part part)
	{
		Object[] array = Object.FindObjectsOfType(typeof(UIPartActionWindow));
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			UIPartActionWindow uIPartActionWindow = (UIPartActionWindow)array[i];
			if (uIPartActionWindow.part == part)
			{
				uIPartActionWindow.displayDirty = true;
			}
		}
	}

	public static void RefreshPartContextWindow(Part part)
	{
		if (UIPartActionController.Instance != null)
		{
			UIPartActionWindow item = UIPartActionController.Instance.GetItem(part);
			if (item != null)
			{
				item.displayDirty = true;
			}
		}
	}
}
