using System;
using System.Collections;
using UnityEngine;

namespace ns2;

public class UIPanelTransitionRTScaler : UITransitionBase
{
	[Serializable]
	public class RTRectBounds
	{
		public string name;

		public Vector2 topBottom;
	}

	[SerializeField]
	public UIPanelTransition panelTransition;

	[SerializeField]
	public float transitionTime = 1f;

	[SerializeField]
	public RTRectBounds[] states;

	public Coroutine panelCoroutine;

	public Vector2 panelPosition;

	public string State { get; set; }

	public bool Transitioning { get; set; }

	public void Awake()
	{
		panelTransition.onTransitionStart.AddListener(Transition);
	}

	public void Transition()
	{
		if (!(State == panelTransition.State))
		{
			int index;
			RTRectBounds state = GetState(panelTransition.State, out index);
			if (panelCoroutine != null)
			{
				StopCoroutine(panelCoroutine);
			}
			SetMethod(method);
			panelCoroutine = StartCoroutine(TransitionState(state));
		}
	}

	public IEnumerator TransitionState(RTRectBounds newPosition)
	{
		Transitioning = true;
		State = newPosition.name;
		float t = transitionTime;
		PrepMethod(t, panelPosition, newPosition.topBottom);
		while (t > 0f)
		{
			yield return null;
			t -= Time.deltaTime;
			float arg = 1f - t / transitionTime;
			panelPosition = TransitionMethod(arg, panelPosition, newPosition.topBottom);
			panelTransform.anchoredPosition = new Vector2(0f, panelPosition.y);
			panelTransform.sizeDelta = new Vector2(panelTransform.sizeDelta.x, 0f - panelPosition.y - panelPosition.x);
		}
		panelPosition = newPosition.topBottom;
		panelTransform.anchoredPosition = new Vector2(0f, panelPosition.y);
		panelTransform.sizeDelta = new Vector2(panelTransform.sizeDelta.x, 0f - panelPosition.y - panelPosition.x);
		Transitioning = false;
		State = newPosition.name;
		panelCoroutine = null;
	}

	public RTRectBounds GetState(string stateName, out int index)
	{
		int num = states.Length;
		do
		{
			if (num-- <= 0)
			{
				index = 0;
				return null;
			}
		}
		while (!(stateName == states[num].name));
		index = num;
		return states[num];
	}
}
