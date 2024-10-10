using System;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class UIScrollRectState : MonoBehaviour
{
	[Serializable]
	public class PanelState
	{
		public ScrollRect scrollRect;

		public string name;

		public RectTransform rectTransform;

		public void Enable()
		{
			rectTransform.gameObject.SetActive(value: true);
			scrollRect.content = rectTransform;
		}

		public void Disable()
		{
			rectTransform.gameObject.SetActive(value: false);
		}
	}

	public PanelState[] panelList;

	[NonSerialized]
	public PanelState CurrentState;

	public string StartState;

	public void Awake()
	{
		int num = panelList.Length;
		while (num-- > 0)
		{
			panelList[num].Disable();
		}
		SetState(StartState);
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
