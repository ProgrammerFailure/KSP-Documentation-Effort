using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(EventTrigger))]
public class UIInputExtraMouseButtons : MonoBehaviour
{
	private bool mouseEntered;

	public Button.ButtonClickedEvent leftClick;

	public Button.ButtonClickedEvent rightClick;

	public Button.ButtonClickedEvent middleClick;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIInputExtraMouseButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseEnter(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseExit(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
