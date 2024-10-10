using System;
using TMPro;
using UnityEngine;

namespace ns2;

public class UIStatePanel : MonoBehaviour
{
	[Serializable]
	public class PanelState
	{
		public string name;

		public RectTransform[] rectTransforms;

		public void Enable()
		{
			int i = 0;
			for (int num = rectTransforms.Length; i < num; i++)
			{
				rectTransforms[i].gameObject.SetActive(value: true);
			}
		}

		public void Disable()
		{
			int i = 0;
			for (int num = rectTransforms.Length; i < num; i++)
			{
				rectTransforms[i].gameObject.SetActive(value: false);
			}
		}
	}

	[Serializable]
	public class TextState
	{
		public string name;

		public TextMeshProUGUI text;
	}

	public PanelState[] panelList;

	[NonSerialized]
	public PanelState CurrentState;

	public string StartState;

	public TextState[] textList;

	public void Awake()
	{
		int num = panelList.Length;
		while (num-- > 0)
		{
			panelList[num].Disable();
		}
		SetState(StartState);
	}

	public void SetText(string key, string value, Color color)
	{
		int num = textList.Length;
		while (num-- > 0)
		{
			if (textList[num].name == key)
			{
				textList[num].text.text = value;
				textList[num].text.color = color;
			}
		}
	}

	public void SetTextOverflowMode(string key, TextOverflowModes overflowMode, bool wordWrap = true)
	{
		int num = textList.Length;
		while (num-- > 0)
		{
			if (textList[num].name == key)
			{
				textList[num].text.overflowMode = overflowMode;
				textList[num].text.enableWordWrapping = wordWrap;
			}
		}
	}

	public void SetState(int index)
	{
		if (index >= 0 && index < panelList.Length)
		{
			if (CurrentState != null)
			{
				CurrentState.Disable();
			}
			panelList[index].Enable();
			CurrentState = panelList[index];
		}
	}

	public void SetState(string name)
	{
		int num = panelList.Length;
		while (num-- > 0)
		{
			if (panelList[num].name == name)
			{
				SetState(num);
			}
		}
	}
}
