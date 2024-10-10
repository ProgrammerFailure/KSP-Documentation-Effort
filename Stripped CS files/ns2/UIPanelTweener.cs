using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class UIPanelTweener : UITransitionBase
{
	public delegate void OnTransitionComplete();

	public bool lockWhileTransitioning = true;

	public float transitionTime = 1f;

	public Vector2 panelPosition;

	public Coroutine panelCoroutine;

	public OnTransitionComplete onTransitionComplete;

	public OnTransitionComplete onTransitionCompleteTemporary;

	public Vector2 PanelPosition => panelPosition;

	public bool Transitioning { get; set; }

	public void Reset()
	{
		panelTransform = GetComponent<RectTransform>();
	}

	public void Awake()
	{
		panelPosition = panelTransform.anchoredPosition;
	}

	public void Transition(Vector2 tgtPos, bool inputLockAfterTransition)
	{
		if (panelCoroutine != null)
		{
			StopCoroutine(panelCoroutine);
		}
		SetMethod(method);
		panelCoroutine = StartCoroutine(TransitionState(tgtPos, inputLockAfterTransition));
	}

	public IEnumerator TransitionState(Vector2 newPosition, bool inputLock)
	{
		Transitioning = true;
		if (lockWhileTransitioning || inputLock)
		{
			SetInteractable(interactable: false);
		}
		float t = transitionTime;
		PrepMethod(t, panelPosition, newPosition);
		while (t > 0f)
		{
			yield return null;
			t -= Time.deltaTime;
			float arg = 1f - t / transitionTime;
			panelPosition = TransitionMethod(arg, panelPosition, newPosition);
			panelTransform.anchoredPosition = panelPosition;
		}
		panelPosition = newPosition;
		panelTransform.anchoredPosition = panelPosition;
		SetInteractable(!inputLock);
		if (onTransitionComplete != null)
		{
			onTransitionComplete();
		}
		if (onTransitionCompleteTemporary != null)
		{
			onTransitionCompleteTemporary();
			onTransitionCompleteTemporary = null;
		}
		Transitioning = false;
		panelCoroutine = null;
	}

	public new void SetInteractable(bool interactable)
	{
		Selectable[] componentsInChildren = panelTransform.GetComponentsInChildren<Selectable>();
		int num = componentsInChildren.Length;
		while (num-- > 0)
		{
			componentsInChildren[num].interactable = interactable;
		}
	}
}
