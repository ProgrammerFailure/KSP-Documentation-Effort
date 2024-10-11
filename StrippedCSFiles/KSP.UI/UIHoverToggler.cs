using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public class UIHoverToggler : XSelectable
{
	[SerializeField]
	private GameObject[] activeOnHover;

	[SerializeField]
	private Selectable tgtControl;

	[SerializeField]
	internal bool RequireInteractable;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIHoverToggler()
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
	private void OnDeselect(XSelectable arg1, BaseEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnHover(XSelectable arg1, PointerEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnUnhover(XSelectable arg1, PointerEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(string text)
	{
		throw null;
	}
}
