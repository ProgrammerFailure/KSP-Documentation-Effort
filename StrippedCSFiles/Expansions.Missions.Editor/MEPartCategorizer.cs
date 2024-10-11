using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;
using RUI.Icons.Selectable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEPartCategorizer : BasePartCategorizer
{
	public MEPartCategorizerButton buttonPrefab;

	protected Color selectionColor;

	private MEPartSelectorBrowser partSelector;

	private List<MEPartCategorizerButton> categoryButtons;

	private Dictionary<string, List<AvailablePart>> partsCategoryCache;

	private Dictionary<string, List<AvailablePart>> selectedParts;

	public float scrollStep;

	[SerializeField]
	private ScrollRect partCategoryScroll;

	[SerializeField]
	private PointerClickAndHoldHandler GetScrollBtnDown;

	[SerializeField]
	private PointerClickAndHoldHandler GetScrollBtnUp;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEPartCategorizer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(MEPartSelectorBrowser partSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MEPartCategorizerButton InstantiatePartCategorizerButton(string categoryName, string tooltip, Icon icon, Color colorButton, Color colorIcon, EditorPartListFilter<AvailablePart> filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTrue(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFalse(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClick(PointerEventData eventData, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SearchFilterResult(EditorPartListFilter<AvailablePart> filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected List<AvailablePart> SelectedPartsOfCategory(List<AvailablePart> categoryParts, List<string> selectedParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PartSelected(AvailablePart part, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
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
