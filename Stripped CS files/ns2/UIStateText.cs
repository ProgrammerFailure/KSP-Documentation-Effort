using System;
using TMPro;
using UnityEngine;

namespace ns2;

public class UIStateText : MonoBehaviour
{
	[Serializable]
	public class TextState
	{
		public string name;

		public string textMessage;

		public Color textColor;
	}

	public TextMeshProUGUI textLabel;

	public TextState[] states = new TextState[0];

	public string startState = "";

	public void Reset()
	{
		textLabel = GetComponent<TextMeshProUGUI>();
	}

	public void Awake()
	{
		SetState(startState);
	}

	public void SetState(int index)
	{
		if (index >= 0 && index < states.Length && textLabel != null)
		{
			textLabel.text = states[index].textMessage;
			textLabel.color = states[index].textColor;
		}
	}

	public void SetState(string name)
	{
		int num = states.Length;
		while (num-- > 0)
		{
			if (states[num].name == name)
			{
				SetState(num);
			}
		}
	}
}
