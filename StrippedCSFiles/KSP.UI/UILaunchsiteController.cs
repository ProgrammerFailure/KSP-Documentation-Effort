using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public class UILaunchsiteController : UIHoverSlidePanel
{
	private bool groupSet;

	[SerializeField]
	private XSelectable launchSitesSelectable;

	[SerializeField]
	private EditorLaunchPadItem subassemblyPrefab;

	private List<EditorLaunchPadItem> launchPadItems;

	[SerializeField]
	private ToggleGroup selectedToggleGroup;

	public RectTransform launchsiteGrid;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UILaunchsiteController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected new void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setupItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void resetItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setSelectedItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void addlaunchPadItem(string siteName, string displayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setupToggleGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void launchSitesSelectable_onPointerExit(XSelectable arg1, PointerEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void launchSitesSelectable_onPointerEnter(XSelectable arg1, PointerEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}
}
