using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
	[Serializable]
	public class SameParentSelectables
	{
		public List<Selectable> selectables;

		public Transform sharedParent;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SameParentSelectables()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SameParentSelectables(List<Selectable> selectables, Transform sharedParent)
		{
			throw null;
		}
	}

	private MenuNavInput currentInput;

	private SliderFocusType sliderFocusType;

	public List<Selectable> selectableItems;

	private List<TextMeshProUGUI> textItems;

	public static Color inactiveTextColor;

	public static Color activeTextColor;

	public static Color inactiveTextMiniSettingsColor;

	public bool Initialized;

	private bool assignedOnce;

	private bool specialColor;

	private Color specialdefaultColor;

	private Color defaultTextColorCache;

	internal int lastItemSelected;

	private bool keysUsed;

	private bool needsHighlightColour;

	private bool directionalKeysHeldDown;

	public static bool blockPointerEnterExit;

	private bool hasScrollbar;

	private ScrollRect scrollRect;

	protected List<SameParentSelectables> sameParentSelectables;

	private bool dialogLimitCheck;

	private Selectable cachedCurrentSelectedSelectable;

	private int lastItemSubmited;

	private UINavMouseChecker navMouseCheckerCached;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MenuNavigation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckKeyboardInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectLastArrowSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseIsHovering()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsSearchFieldFocused()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleHighlightedElements(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLastItemSelected(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, bool limitCheck)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, bool hasText, bool limitCheck)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, SliderFocusType focusType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, SliderFocusType focusType, bool hasText, bool limitCheck)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, bool hasText, bool limitCheck, SliderFocusType focusType, bool includeDisabledChildren = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSelectableItems(Selectable[] items, Navigation.Mode navMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSelectableItems(Selectable[] items, Navigation.Mode navMode, bool hasText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSelectableItems(Selectable[] items, Navigation.Mode navMode, bool hasText, bool resetNavMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefocusOnInputDetect(MenuNavInput input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FocusNextItemFromTextInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FocusOnItemByScrollbar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FocusOnItemByAnchoredPos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnsureFocusIsWithinDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SumbmitOnSelectedToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetItemAsFirstSelected(GameObject go)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetAll()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetSelectablesOnly()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVerticalExplicitNavigation(List<Selectable> selectables)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSameParentExplicitNavigation(List<Selectable> selectables)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetSelectableParent(Selectable selectable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsSelectableValid(Selectable selectable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMenuNavInputLock(bool newState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSearchField()
	{
		throw null;
	}
}
