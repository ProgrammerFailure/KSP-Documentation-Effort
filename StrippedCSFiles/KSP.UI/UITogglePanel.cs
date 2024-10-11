using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI;

public class UITogglePanel : MonoBehaviour
{
	public List<EventTrigger> interactableA;

	public List<EventTrigger> interactableB;

	public GameObject panelA;

	public GameObject panelB;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UITogglePanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Hover(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HoverOut(BaseEventData data)
	{
		throw null;
	}
}
