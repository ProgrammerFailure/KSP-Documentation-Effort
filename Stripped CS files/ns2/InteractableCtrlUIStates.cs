using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[ExecuteInEditMode]
public class InteractableCtrlUIStates : MonoBehaviour
{
	[Serializable]
	public class State
	{
		[SerializeField]
		public Graphic[] tgtGraphics;

		[SerializeField]
		public Color color = Color.white;

		[SerializeField]
		public bool toggleEnabled;

		[SerializeField]
		public TextMeshProUGUI tgtText;

		[SerializeField]
		public bool changeColorOnly;

		[SerializeField]
		public string text;

		public void SetActive()
		{
			int num = tgtGraphics.Length;
			while (num-- > 0)
			{
				tgtGraphics[num].color = color;
				if (toggleEnabled)
				{
					tgtGraphics[num].enabled = true;
				}
			}
			if (tgtText != null)
			{
				tgtText.color = color;
				if (!changeColorOnly)
				{
					tgtText.text = text;
				}
			}
		}
	}

	[SerializeField]
	public Selectable tgtCtrl;

	[SerializeField]
	public State stInteractable;

	[SerializeField]
	public State stNonInteractable;

	public bool ctrlFlagLast;

	public bool firstRun = true;

	public void Awake()
	{
	}

	public void LateUpdate()
	{
		if (ctrlFlagLast != tgtCtrl.interactable || firstRun)
		{
			ctrlFlagLast = tgtCtrl.interactable;
			if (ctrlFlagLast)
			{
				stInteractable.SetActive();
			}
			else
			{
				stNonInteractable.SetActive();
			}
		}
		if (firstRun)
		{
			firstRun = false;
		}
	}
}
