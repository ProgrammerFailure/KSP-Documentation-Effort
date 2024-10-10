using UnityEngine;

public class ModalGUI : MonoBehaviour
{
	public static void Window(Rect rect, string title, Callback windowFunction)
	{
		GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), rect, delegate(int id)
		{
			GUI.depth = 0;
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			if (GUIUtility.hotControl < controlID)
			{
				setHotControl(0);
			}
			windowFunction();
			int controlID2 = GUIUtility.GetControlID(FocusType.Passive);
			if (GUIUtility.hotControl < controlID || (GUIUtility.hotControl > controlID2 && controlID2 != -1))
			{
				setHotControl(-1);
			}
			GUI.FocusWindow(id);
			GUI.BringWindowToFront(id);
		}, title);
	}

	public static void setHotControl(int id)
	{
		if (new Rect(0f, 0f, Screen.width, Screen.height).Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
		{
			GUIUtility.hotControl = id;
		}
	}
}
