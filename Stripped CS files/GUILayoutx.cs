using UnityEngine;

public class GUILayoutx
{
	public delegate void DoubleClickCallback(int index);

	public static int SelectionList(int selected, GUIContent[] list)
	{
		return SelectionList(selected, list, "List Item", null);
	}

	public static int SelectionList(int selected, GUIContent[] list, GUIStyle elementStyle)
	{
		return SelectionList(selected, list, elementStyle, null);
	}

	public static int SelectionList(int selected, GUIContent[] list, DoubleClickCallback callback)
	{
		return SelectionList(selected, list, "List Item", callback);
	}

	public static int SelectionList(int selected, GUIContent[] list, GUIStyle elementStyle, DoubleClickCallback callback)
	{
		int i = 0;
		for (int num = list.Length; i < num; i++)
		{
			Rect rect = GUILayoutUtility.GetRect(list[i], elementStyle);
			bool flag;
			if ((flag = rect.Contains(Event.current.mousePosition)) && callback != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
			{
				callback(i);
				Event.current.Use();
			}
			else if (flag && Event.current.type == EventType.MouseDown)
			{
				selected = i;
				Event.current.Use();
			}
			else if (Event.current.type == EventType.Repaint)
			{
				elementStyle.Draw(rect, list[i], flag, isActive: false, i == selected, hasKeyboardFocus: false);
			}
		}
		return selected;
	}

	public static int SelectionList(int selected, string[] list)
	{
		return SelectionList(selected, list, "List Item", null);
	}

	public static int SelectionList(int selected, string[] list, GUIStyle elementStyle)
	{
		return SelectionList(selected, list, elementStyle, null);
	}

	public static int SelectionList(int selected, string[] list, DoubleClickCallback callback)
	{
		return SelectionList(selected, list, "List Item", callback);
	}

	public static int SelectionList(int selected, string[] list, GUIStyle elementStyle, DoubleClickCallback callback)
	{
		int i = 0;
		for (int num = list.Length; i < num; i++)
		{
			Rect rect = GUILayoutUtility.GetRect(new GUIContent(list[i]), elementStyle);
			bool flag;
			if ((flag = rect.Contains(Event.current.mousePosition)) && Event.current.type == EventType.MouseDown)
			{
				selected = i;
				Event.current.Use();
			}
			else if (flag && callback != null && Event.current.type == EventType.MouseUp && Event.current.clickCount == 2)
			{
				callback(i);
				Event.current.Use();
			}
			else if (Event.current.type == EventType.Repaint)
			{
				elementStyle.Draw(rect, list[i], flag, isActive: false, i == selected, hasKeyboardFocus: false);
			}
		}
		return selected;
	}
}
