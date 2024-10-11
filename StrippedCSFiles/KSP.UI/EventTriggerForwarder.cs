using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI;

[RequireComponent(typeof(EventTrigger))]
public class EventTriggerForwarder : MonoBehaviour
{
	public bool forwardDrop;

	public bool forwardPointerEnter;

	public bool forwardPointerExit;

	public bool forwardScroll;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EventTriggerForwarder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Drop(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Scroll(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PointerEnter(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PointerExit(BaseEventData data)
	{
		throw null;
	}
}
