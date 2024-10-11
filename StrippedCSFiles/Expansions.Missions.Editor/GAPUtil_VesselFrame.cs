using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class GAPUtil_VesselFrame : GAPUtil_BiSelector
{
	public delegate void categoryValueChange(PartCategories category, bool state);

	public categoryValueChange onCategoryValueChange;

	public float scrollStep;

	[SerializeField]
	private ScrollRect partCategoryScroll;

	[SerializeField]
	private PointerClickAndHoldHandler GetScrollBtnDown;

	[SerializeField]
	private PointerClickAndHoldHandler GetScrollBtnUp;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPUtil_VesselFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartTypes(ShipConstruct vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTitleText(string newText, bool playerCreated)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCategoryValueChange(PartCategories category, MEPartCategoryButton control)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ScrollWithButtons(float direction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerDown(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerUp(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
