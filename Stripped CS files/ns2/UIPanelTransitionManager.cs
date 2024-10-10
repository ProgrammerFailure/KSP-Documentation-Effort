using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns2;

public class UIPanelTransitionManager : MonoBehaviour
{
	public UIPanelTransition[] panels;

	public void BringIn(int panel, Action onFinished = null)
	{
		int num = panels.Length;
		do
		{
			if (num-- <= 0)
			{
				return;
			}
		}
		while (num != panel);
		PanelTransitionIn(panels[num], onFinished);
	}

	public void BringIn(UIPanelTransition panel, Action onFinished = null)
	{
		StartCoroutine(PanelTransitionIn(panel, onFinished));
	}

	public void BringIn(List<UIPanelTransition> panelsToBringIn, Action onFinished = null)
	{
		StartCoroutine(PanelTransitionIn(panelsToBringIn, onFinished));
	}

	public void Dismiss(UIPanelTransition panel)
	{
		panel.Transition("Out");
	}

	public IEnumerator PanelTransitionIn(UIPanelTransition panel, Action onFinished = null)
	{
		if (panel.State == "In")
		{
			yield break;
		}
		int num = panels.Length;
		while (num-- > 0)
		{
			if (panels[num] != panel)
			{
				panels[num].Transition("Out");
			}
		}
		while (AnyPanelsTransitioning())
		{
			yield return null;
		}
		panel.Transition("In", onFinished);
	}

	public IEnumerator PanelTransitionIn(List<UIPanelTransition> panelsToBringIn, Action onFinished = null)
	{
		List<UIPanelTransition> panelsToAction = new List<UIPanelTransition>();
		for (int i = 0; i < panelsToBringIn.Count; i++)
		{
			if (panelsToBringIn[i].State != "In")
			{
				panelsToAction.Add(panelsToBringIn[i]);
			}
		}
		if (panelsToAction.Count < 1)
		{
			yield break;
		}
		int num = panels.Length;
		while (num-- > 0)
		{
			bool flag = false;
			for (int j = 0; j < panelsToBringIn.Count; j++)
			{
				if (panels[num] != panelsToBringIn[j])
				{
					flag = true;
				}
			}
			if (!flag)
			{
				panels[num].Transition("Out");
			}
		}
		while (AnyPanelsTransitioning())
		{
			yield return null;
		}
		for (int k = 0; k < panelsToAction.Count; k++)
		{
			panelsToAction[k].Transition("In", onFinished);
		}
	}

	public bool AnyPanelsTransitioning()
	{
		int num = panels.Length;
		do
		{
			if (num-- <= 0)
			{
				return false;
			}
		}
		while (!panels[num].Transitioning);
		return true;
	}
}
