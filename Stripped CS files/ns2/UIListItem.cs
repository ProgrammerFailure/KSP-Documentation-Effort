using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class UIListItem : MonoBehaviour
{
	public object Data;

	public TextMeshProUGUI GetTextElement(string elementName)
	{
		TextMeshProUGUI[] componentsInChildren = GetComponentsInChildren<TextMeshProUGUI>();
		int num = 0;
		int num2 = componentsInChildren.Length;
		while (true)
		{
			if (num < num2)
			{
				if (componentsInChildren[num].name == elementName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return componentsInChildren[num];
	}

	public Button GetButtonElement(string elementName)
	{
		Button[] componentsInChildren = GetComponentsInChildren<Button>();
		int num = 0;
		int num2 = componentsInChildren.Length;
		while (true)
		{
			if (num < num2)
			{
				if (componentsInChildren[num].name == elementName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return componentsInChildren[num];
	}

	public UIStateButton GetStateButtonElement(string elementName)
	{
		UIStateButton[] componentsInChildren = GetComponentsInChildren<UIStateButton>();
		int num = 0;
		int num2 = componentsInChildren.Length;
		while (true)
		{
			if (num < num2)
			{
				if (componentsInChildren[num].name == elementName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return componentsInChildren[num];
	}
}
