using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public class UIHoverText : XHoverable
{
	[SerializeField]
	public TextMeshProUGUI textTargetForHover;

	[SerializeField]
	public Selectable interactibleTargetControl;

	[SerializeField]
	public bool RequireInteractable;

	[SerializeField]
	public string hoverText;

	[SerializeField]
	public bool clearTextOnHoverExit;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIHoverText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTargetControl(TextMeshProUGUI textLabel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTargetControl(Selectable interactibleTarget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected new void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected new void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnHover(XHoverable arg1, PointerEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnUnhover(XHoverable arg1, PointerEventData arg2)
	{
		throw null;
	}
}
