using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public abstract class UITransitionBase : MonoBehaviour
{
	public enum Method
	{
		Linear,
		Lerp
	}

	public RectTransform panelTransform;

	public Method method;

	protected Callback<float, Vector2, Vector2> PrepMethod;

	protected Func<float, Vector2, Vector2, Vector2> TransitionMethod;

	[SerializeField]
	private CanvasGroup ctrlGroup;

	private Vector2 vStep;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected UITransitionBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Linear_Pre(float transitionTime, Vector2 vCrr, Vector2 vTgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector2 Linear_Transition(float t, Vector2 vCrr, Vector2 vTgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Lerp_Pre(float transitionTime, Vector2 vCrr, Vector2 vTgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector2 Lerp_Transition(float t, Vector2 vCrr, Vector2 vTgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetMethod(Method m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetInteractable(bool interactable)
	{
		throw null;
	}
}
