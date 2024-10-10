using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ns2;

public class UIPanelTransition : UITransitionBase
{
	[Serializable]
	public class PanelPosition
	{
		public string name;

		public Vector2 position;

		public bool inputLock;

		public bool deactivateChildren;
	}

	public string startState = "";

	public bool lockWhileTransitioning = true;

	public float transitionTime = 1f;

	public PanelPosition[] states;

	public Vector2 panelPosition;

	public Coroutine panelCoroutine;

	public UnityEvent onTransitionStart;

	public UnityEvent onTransitionComplete;

	public Action onTransitionCompleteTemporary;

	public List<GameObject> childrenForDeactivate = new List<GameObject>();

	public int stateIndex;

	public string State { get; set; }

	public int StateIndex
	{
		get
		{
			return stateIndex;
		}
		set
		{
			stateIndex = value;
		}
	}

	public bool Transitioning { get; set; }

	public void Reset()
	{
		panelTransform = GetComponent<RectTransform>();
		PanelPosition panelPosition = new PanelPosition();
		panelPosition.name = "In";
		panelPosition.position = panelTransform.anchoredPosition;
		panelPosition.inputLock = false;
		states = new PanelPosition[1];
		states[0] = panelPosition;
	}

	public void Awake()
	{
		panelPosition = panelTransform.anchoredPosition;
	}

	public void Start()
	{
		for (int num = childrenForDeactivate.Count - 1; num >= 0; num--)
		{
			if (childrenForDeactivate[num] == null)
			{
				childrenForDeactivate.RemoveAt(num);
			}
		}
		if (!string.IsNullOrEmpty(startState))
		{
			Transition(startState);
		}
	}

	public void Transition(string stateName, Action onFinished = null)
	{
		int num = stateIndex;
		PanelPosition state = GetState(stateName, out stateIndex);
		if (state == null)
		{
			stateIndex = num;
			return;
		}
		if (panelCoroutine != null)
		{
			StopCoroutine(panelCoroutine);
		}
		SetMethod(method);
		panelCoroutine = StartCoroutine(TransitionState(state, onFinished));
		if (State == stateName)
		{
			onFinished?.Invoke();
		}
	}

	public void TransitionImmediate(string stateName, Action onFinished = null)
	{
		int num = stateIndex;
		PanelPosition state = GetState(stateName, out stateIndex);
		if (state == null)
		{
			stateIndex = num;
			return;
		}
		if (panelCoroutine != null)
		{
			StopCoroutine(panelCoroutine);
		}
		SetMethod(method);
		panelTransform.anchoredPosition = state.position;
		panelPosition = state.position;
		if (State == stateName)
		{
			onFinished?.Invoke();
		}
	}

	public void Transition(int stIndex)
	{
		stIndex = Mathf.Clamp(stIndex, 0, states.Length - 1);
		Transition(states[stIndex].name);
	}

	public void Transition(int stIndex, Action onFinished)
	{
		stIndex = Mathf.Clamp(stIndex, 0, states.Length - 1);
		Transition(states[stIndex].name, onFinished);
	}

	public void TransitionImmediate(int stIndex)
	{
		stIndex = Mathf.Clamp(stIndex, 0, states.Length - 1);
		TransitionImmediate(states[stIndex].name);
	}

	public void TransitionImmediate(int stIndex, Action onFinished)
	{
		stIndex = Mathf.Clamp(stIndex, 0, states.Length - 1);
		TransitionImmediate(states[stIndex].name, onFinished);
	}

	public IEnumerator TransitionState(PanelPosition newPosition, Action onFinished = null)
	{
		if (onFinished != null)
		{
			onTransitionCompleteTemporary = (Action)Delegate.Combine(onTransitionCompleteTemporary, onFinished);
		}
		if (!newPosition.deactivateChildren)
		{
			for (int i = 0; i < childrenForDeactivate.Count; i++)
			{
				if (childrenForDeactivate[i] != null && !childrenForDeactivate[i].activeSelf)
				{
					childrenForDeactivate[i].SetActive(value: true);
				}
			}
		}
		Transitioning = true;
		State = newPosition.name;
		if (onTransitionStart != null)
		{
			onTransitionStart.Invoke();
		}
		if (lockWhileTransitioning || newPosition.inputLock)
		{
			SetInteractable(interactable: false);
		}
		float t = transitionTime;
		PrepMethod(t, panelPosition, newPosition.position);
		while (t > 0f)
		{
			yield return null;
			t -= Time.deltaTime;
			float arg = 1f - t / transitionTime;
			panelPosition = TransitionMethod(arg, panelPosition, newPosition.position);
			panelTransform.anchoredPosition = panelPosition;
		}
		panelPosition = newPosition.position;
		panelTransform.anchoredPosition = panelPosition;
		SetInteractable(!newPosition.inputLock);
		if (onTransitionComplete != null)
		{
			onTransitionComplete.Invoke();
		}
		if (onTransitionCompleteTemporary != null)
		{
			onTransitionCompleteTemporary();
			onTransitionCompleteTemporary = null;
		}
		if (newPosition.deactivateChildren)
		{
			for (int j = 0; j < childrenForDeactivate.Count; j++)
			{
				if (childrenForDeactivate[j] != null && childrenForDeactivate[j].activeSelf)
				{
					childrenForDeactivate[j].SetActive(value: false);
				}
			}
		}
		Transitioning = false;
		State = newPosition.name;
		panelCoroutine = null;
	}

	public PanelPosition GetState(string stateName, out int index)
	{
		int num = states.Length;
		do
		{
			if (num-- <= 0)
			{
				index = 1;
				return null;
			}
		}
		while (!(stateName == states[num].name));
		index = num;
		return states[num];
	}
}
