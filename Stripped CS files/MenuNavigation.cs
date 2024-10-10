using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
	[Serializable]
	public class SameParentSelectables
	{
		public List<Selectable> selectables = new List<Selectable>();

		public Transform sharedParent;

		public SameParentSelectables()
		{
		}

		public SameParentSelectables(List<Selectable> selectables, Transform sharedParent)
		{
			this.selectables.AddRange(selectables);
			this.sharedParent = sharedParent;
		}
	}

	public MenuNavInput currentInput;

	public SliderFocusType sliderFocusType;

	public List<Selectable> selectableItems = new List<Selectable>();

	public List<TextMeshProUGUI> textItems = new List<TextMeshProUGUI>();

	public static Color inactiveTextColor;

	public static Color activeTextColor;

	public static Color inactiveTextMiniSettingsColor;

	public bool Initialized;

	public bool assignedOnce;

	public bool specialColor;

	public Color specialdefaultColor;

	public Color defaultTextColorCache;

	public int lastItemSelected;

	public bool keysUsed;

	public bool needsHighlightColour;

	public bool directionalKeysHeldDown;

	public static bool blockPointerEnterExit;

	public bool hasScrollbar;

	public ScrollRect scrollRect;

	public List<SameParentSelectables> sameParentSelectables = new List<SameParentSelectables>();

	public bool dialogLimitCheck;

	public Selectable cachedCurrentSelectedSelectable;

	public int lastItemSubmited;

	public UINavMouseChecker navMouseCheckerCached;

	public void Start()
	{
		EventSystem.current.SetSelectedGameObject(null);
		SetMenuNavInputLock(newState: true);
		lastItemSubmited = -1;
		defaultTextColorCache = inactiveTextColor;
		Initialized = true;
	}

	public void OnDestroy()
	{
		SetMenuNavInputLock(newState: false);
	}

	public void Update()
	{
		CheckKeyboardInput();
		if (currentInput != MenuNavInput.None)
		{
			keysUsed = true;
			GameEvents.onMenuNavGetInput.Fire(currentInput);
			RefocusOnInputDetect(currentInput);
			if (currentInput == MenuNavInput.Tab && selectableItems.Count > 0)
			{
				FocusNextItemFromTextInput();
			}
			if (currentInput == MenuNavInput.Accept)
			{
				lastItemSubmited = lastItemSelected;
			}
		}
		if (dialogLimitCheck && directionalKeysHeldDown)
		{
			EnsureFocusIsWithinDialog();
		}
	}

	public void CheckKeyboardInput()
	{
		currentInput = MenuNavInput.None;
		if (dialogLimitCheck)
		{
			directionalKeysHeldDown = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			currentInput = MenuNavInput.Up;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			currentInput = MenuNavInput.Down;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			currentInput = MenuNavInput.Left;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			currentInput = MenuNavInput.Right;
		}
		else if (Input.GetKeyDown(KeyCode.Return))
		{
			currentInput = MenuNavInput.Accept;
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			currentInput = MenuNavInput.Accept;
		}
		else if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			currentInput = MenuNavInput.Accept;
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			currentInput = MenuNavInput.Cancel;
		}
		else if (Input.GetKeyDown(KeyCode.Tab))
		{
			currentInput = MenuNavInput.Tab;
		}
		else
		{
			currentInput = MenuNavInput.None;
		}
	}

	public void SelectLastArrowSelected()
	{
		if (keysUsed && !IsSearchFieldFocused())
		{
			if (selectableItems.Count > 0 && lastItemSelected < selectableItems.Count)
			{
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(selectableItems[lastItemSelected].gameObject);
				EventSystem.current.firstSelectedGameObject = selectableItems[lastItemSelected].gameObject;
			}
			if (needsHighlightColour)
			{
				ToggleHighlightedElements(toggle: true);
			}
		}
	}

	public void MouseIsHovering()
	{
		if (!IsSearchFieldFocused())
		{
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.firstSelectedGameObject = null;
			if (needsHighlightColour)
			{
				ToggleHighlightedElements(toggle: false);
			}
		}
	}

	public bool IsSearchFieldFocused()
	{
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
		{
			navMouseCheckerCached = EventSystem.current.currentSelectedGameObject.GetComponent<UINavMouseChecker>();
			if (navMouseCheckerCached != null && navMouseCheckerCached.IsSearchField)
			{
				return true;
			}
		}
		return false;
	}

	public void ToggleHighlightedElements(bool toggle)
	{
		bool flag = false;
		if ((selectableItems[lastItemSelected].GetType() == typeof(Slider) || selectableItems[lastItemSelected].GetType() == typeof(Button) || selectableItems[lastItemSelected].GetType() == typeof(Toggle)) && (selectableItems[lastItemSelected].gameObject.name.Contains("Toggle") || selectableItems[lastItemSelected].gameObject.name.Contains("Slider")))
		{
			flag = true;
		}
		if (toggle)
		{
			if (!flag || !(textItems[lastItemSelected] != null))
			{
				return;
			}
			if (textItems[lastItemSelected].color == Color.red)
			{
				specialdefaultColor = textItems[lastItemSelected].color;
				specialColor = true;
			}
			if (!assignedOnce)
			{
				if (textItems[lastItemSelected].color == inactiveTextColor)
				{
					defaultTextColorCache = Color.white;
				}
				else
				{
					defaultTextColorCache = textItems[lastItemSelected].color;
				}
				assignedOnce = true;
			}
			if (defaultTextColorCache != Color.white && defaultTextColorCache != activeTextColor)
			{
				textItems[lastItemSelected].color = Color.white;
			}
			else
			{
				textItems[lastItemSelected].color = inactiveTextMiniSettingsColor;
			}
		}
		else if (flag && textItems[lastItemSelected] != null)
		{
			if (specialColor)
			{
				textItems[lastItemSelected].color = specialdefaultColor;
				specialColor = false;
			}
			else
			{
				textItems[lastItemSelected].color = defaultTextColorCache;
			}
		}
	}

	public void SetLastItemSelected(int index)
	{
		if (needsHighlightColour)
		{
			ToggleHighlightedElements(toggle: false);
			lastItemSelected = index;
			ToggleHighlightedElements(toggle: true);
		}
		else
		{
			lastItemSelected = index;
		}
		if (hasScrollbar)
		{
			if (sliderFocusType == SliderFocusType.Scrollbar)
			{
				FocusOnItemByScrollbar();
			}
			else if (sliderFocusType == SliderFocusType.AnchoredPos)
			{
				FocusOnItemByAnchoredPos();
			}
		}
	}

	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode)
	{
		return SpawnMenuNavigation(go, navMode, hasText: false, limitCheck: false, SliderFocusType.None);
	}

	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, bool limitCheck)
	{
		return SpawnMenuNavigation(go, navMode, hasText: false, limitCheck, SliderFocusType.None, includeDisabledChildren: true);
	}

	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, bool hasText, bool limitCheck)
	{
		return SpawnMenuNavigation(go, navMode, hasText, limitCheck, SliderFocusType.None);
	}

	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, SliderFocusType focusType)
	{
		return SpawnMenuNavigation(go, navMode, hasText: false, limitCheck: false, focusType);
	}

	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, SliderFocusType focusType, bool hasText, bool limitCheck)
	{
		return SpawnMenuNavigation(go, navMode, hasText, limitCheck, focusType);
	}

	public static MenuNavigation SpawnMenuNavigation(GameObject go, Navigation.Mode navMode, bool hasText, bool limitCheck, SliderFocusType focusType, bool includeDisabledChildren = false)
	{
		MenuNavigation menuNavigation = go.AddComponent(typeof(MenuNavigation)) as MenuNavigation;
		menuNavigation.dialogLimitCheck = limitCheck;
		Selectable[] componentsInChildren = go.transform.GetComponentsInChildren<Selectable>(includeDisabledChildren);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].GetType() != typeof(Scrollbar))
			{
				menuNavigation.selectableItems.Add(componentsInChildren[i]);
			}
		}
		if (hasText)
		{
			menuNavigation.needsHighlightColour = true;
			for (int j = 0; j < menuNavigation.selectableItems.Count; j++)
			{
				menuNavigation.textItems.Add(menuNavigation.selectableItems[j].transform.parent.GetComponentInChildren<TextMeshProUGUI>());
			}
		}
		for (int k = 0; k < menuNavigation.selectableItems.Count; k++)
		{
			if (menuNavigation.selectableItems[k] != null)
			{
				if (menuNavigation.selectableItems[k].navigation.mode == Navigation.Mode.None && menuNavigation.selectableItems[k].GetType() != typeof(Scrollbar))
				{
					Navigation navigation = default(Navigation);
					navigation.mode = navMode;
					menuNavigation.selectableItems[k].navigation = navigation;
				}
				UINavMouseChecker uINavMouseChecker = null;
				uINavMouseChecker = menuNavigation.selectableItems[k].gameObject.GetComponent<UINavMouseChecker>();
				if (uINavMouseChecker == null)
				{
					uINavMouseChecker = menuNavigation.selectableItems[k].gameObject.AddComponent(typeof(UINavMouseChecker)) as UINavMouseChecker;
				}
				uINavMouseChecker.index = k;
				uINavMouseChecker.menuNav = menuNavigation;
			}
		}
		menuNavigation.hasScrollbar = true;
		menuNavigation.sliderFocusType = focusType;
		if (focusType != 0)
		{
			menuNavigation.scrollRect = go.GetComponentInChildren<ScrollRect>();
		}
		else
		{
			menuNavigation.scrollRect = null;
		}
		return menuNavigation;
	}

	public void SetSelectableItems(Selectable[] items, Navigation.Mode navMode)
	{
		SetSelectableItems(items, navMode, hasText: false, resetNavMode: false);
	}

	public void SetSelectableItems(Selectable[] items, Navigation.Mode navMode, bool hasText)
	{
		SetSelectableItems(items, navMode, hasText, resetNavMode: false);
	}

	public void SetSelectableItems(Selectable[] items, Navigation.Mode navMode, bool hasText, bool resetNavMode)
	{
		for (int i = 0; i < items.Length; i++)
		{
			selectableItems.Add(items[i]);
		}
		for (int j = 0; j < selectableItems.Count; j++)
		{
			if (selectableItems[j] != null)
			{
				if (resetNavMode)
				{
					Navigation navigation = default(Navigation);
					navigation.mode = navMode;
					selectableItems[j].navigation = navigation;
				}
				UINavMouseChecker uINavMouseChecker = null;
				uINavMouseChecker = selectableItems[j].gameObject.GetComponent<UINavMouseChecker>();
				if (uINavMouseChecker == null)
				{
					uINavMouseChecker = selectableItems[j].gameObject.AddComponent(typeof(UINavMouseChecker)) as UINavMouseChecker;
				}
				uINavMouseChecker.index = j;
				uINavMouseChecker.menuNav = this;
			}
		}
		if (hasText)
		{
			needsHighlightColour = true;
			for (int k = 0; k < selectableItems.Count; k++)
			{
				textItems.Add(selectableItems[k].transform.parent.GetComponentInChildren<TextMeshProUGUI>());
			}
		}
	}

	public void RefocusOnInputDetect(MenuNavInput input)
	{
		if (keysUsed && EventSystem.current.currentSelectedGameObject == null && selectableItems.Count > 0 && input != MenuNavInput.Accept && input != MenuNavInput.Cancel)
		{
			SelectLastArrowSelected();
		}
	}

	public void FocusNextItemFromTextInput()
	{
		if (!(selectableItems[lastItemSelected].GetType() == typeof(TMP_InputField)) && !(selectableItems[lastItemSelected].GetType() == typeof(Scrollbar)))
		{
			if (selectableItems[lastItemSelected].GetType() == typeof(Scrollbar))
			{
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.firstSelectedGameObject = null;
				lastItemSelected++;
			}
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.firstSelectedGameObject = null;
			lastItemSelected++;
			if (selectableItems[lastItemSelected].GetType() == typeof(Scrollbar))
			{
				lastItemSelected++;
			}
		}
		SelectLastArrowSelected();
	}

	public void FocusOnItemByScrollbar()
	{
		if (scrollRect != null && scrollRect.content != null && lastItemSelected < selectableItems.Count)
		{
			float num = 0f;
			if (selectableItems[lastItemSelected].transform.parent != null && selectableItems[lastItemSelected].transform.IsChildOf(scrollRect.content))
			{
				num = Mathf.InverseLerp(scrollRect.content.GetChild(scrollRect.content.childCount - 1).localPosition.y, scrollRect.content.GetChild(0).localPosition.y, selectableItems[lastItemSelected].transform.localPosition.y);
				scrollRect.verticalNormalizedPosition = num;
			}
		}
	}

	public void FocusOnItemByAnchoredPos()
	{
		if (scrollRect != null && scrollRect.content != null)
		{
			Vector2 anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, scrollRect.transform.InverseTransformPoint(scrollRect.content.position).y) - new Vector2(scrollRect.content.anchoredPosition.x, scrollRect.transform.InverseTransformPoint(selectableItems[lastItemSelected].transform.position).y);
			if (Mathf.Abs(anchoredPosition.y) > scrollRect.content.sizeDelta.y / 2f)
			{
				anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, Mathf.Clamp(anchoredPosition.y, (0f - scrollRect.content.sizeDelta.y) / 2f, scrollRect.content.sizeDelta.y / 2f));
			}
			scrollRect.content.anchoredPosition = anchoredPosition;
		}
	}

	public void EnsureFocusIsWithinDialog()
	{
		if (!(EventSystem.current == null) && !InputLockManager.IsLocked(ControlTypes.KEYBOARDINPUT))
		{
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				cachedCurrentSelectedSelectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
			}
			if (cachedCurrentSelectedSelectable != null && !selectableItems.Contains(cachedCurrentSelectedSelectable))
			{
				SelectLastArrowSelected();
			}
		}
	}

	public bool SumbmitOnSelectedToggle()
	{
		if (selectableItems.Count > 0 && !(selectableItems[lastItemSelected] == null) && Initialized)
		{
			Toggle component = selectableItems[lastItemSelected].GetComponent<Toggle>();
			if (component == null)
			{
				return false;
			}
			if (component.isOn)
			{
				return lastItemSubmited == lastItemSelected;
			}
			return false;
		}
		return false;
	}

	public void SetItemAsFirstSelected(GameObject go)
	{
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(go);
		EventSystem.current.firstSelectedGameObject = go;
	}

	public void ResetAll()
	{
		selectableItems.Clear();
		textItems.Clear();
		assignedOnce = false;
		lastItemSelected = 0;
		hasScrollbar = false;
		keysUsed = false;
		needsHighlightColour = false;
		scrollRect = null;
		sliderFocusType = SliderFocusType.None;
		lastItemSubmited = -1;
	}

	public void ResetSelectablesOnly()
	{
		selectableItems.Clear();
		textItems.Clear();
		assignedOnce = false;
		lastItemSelected = 0;
		lastItemSubmited = -1;
	}

	public void SetVerticalExplicitNavigation(List<Selectable> selectables)
	{
		Navigation navigation = default(Navigation);
		navigation.mode = Navigation.Mode.Explicit;
		for (int i = 0; i < selectables.Count; i++)
		{
			navigation.selectOnUp = ((i - 1 >= 0) ? selectables[i - 1] : null);
			navigation.selectOnDown = ((i + 1 < selectables.Count) ? selectables[i + 1] : null);
			selectables[i].navigation = navigation;
		}
	}

	public void SetSameParentExplicitNavigation(List<Selectable> selectables)
	{
		if (selectables.Count == 0)
		{
			return;
		}
		this.sameParentSelectables.Clear();
		SameParentSelectables sameParentSelectables = new SameParentSelectables();
		List<Selectable> list = new List<Selectable>();
		for (int i = 0; i < selectables.Count; i++)
		{
			if (this.sameParentSelectables.Count == 0)
			{
				if (IsSelectableValid(selectables[i]))
				{
					list.Add(selectables[i]);
					sameParentSelectables = new SameParentSelectables(list, selectables[i].transform.parent);
					this.sameParentSelectables.Add(sameParentSelectables);
				}
				continue;
			}
			for (int j = 0; j < this.sameParentSelectables.Count; j++)
			{
				if (IsSelectableValid(selectables[i]))
				{
					if (this.sameParentSelectables[j].sharedParent == GetSelectableParent(selectables[i]))
					{
						this.sameParentSelectables[j].selectables.Add(selectables[i]);
						break;
					}
					if (j == this.sameParentSelectables.Count - 1)
					{
						sameParentSelectables = new SameParentSelectables();
						list = new List<Selectable>();
						list.Add(selectables[i]);
						sameParentSelectables = new SameParentSelectables(list, selectables[i].transform.parent);
						this.sameParentSelectables.Add(sameParentSelectables);
						break;
					}
				}
			}
		}
		Navigation navigation = default(Navigation);
		navigation.mode = Navigation.Mode.Explicit;
		int num = 0;
		int num2 = 0;
		for (int k = 0; k < this.sameParentSelectables.Count; k++)
		{
			if (this.sameParentSelectables[k].selectables.Count <= 1)
			{
				continue;
			}
			if (k > 0)
			{
				num = k - 1;
				if (this.sameParentSelectables.Count >= num)
				{
					navigation.selectOnUp = this.sameParentSelectables[num].selectables[this.sameParentSelectables[num].selectables.Count - 1];
				}
			}
			else
			{
				navigation.selectOnUp = null;
			}
			if (k + 1 < this.sameParentSelectables.Count)
			{
				num2 = k + 1;
				if (this.sameParentSelectables.Count >= num2 && this.sameParentSelectables[num2].selectables.Count > 0)
				{
					navigation.selectOnDown = this.sameParentSelectables[num2].selectables[0];
				}
			}
			else
			{
				navigation.selectOnDown = null;
			}
			for (int l = 0; l < this.sameParentSelectables[k].selectables.Count; l++)
			{
				object selectOnLeft;
				if (l <= 0)
				{
					selectOnLeft = null;
				}
				else
				{
					Selectable selectable2 = (navigation.selectOnLeft = this.sameParentSelectables[k].selectables[l - 1]);
					selectOnLeft = selectable2;
				}
				navigation.selectOnLeft = (Selectable)selectOnLeft;
				navigation.selectOnRight = ((l + 1 < this.sameParentSelectables[k].selectables.Count) ? this.sameParentSelectables[k].selectables[l + 1] : null);
				this.sameParentSelectables[k].selectables[l].navigation = navigation;
			}
			num = 0;
			num2 = 0;
		}
	}

	public Transform GetSelectableParent(Selectable selectable)
	{
		if (IsSelectableValid(selectable))
		{
			return selectable.transform.parent;
		}
		return null;
	}

	public bool IsSelectableValid(Selectable selectable)
	{
		if (selectable != null && selectable.transform.parent != null)
		{
			return true;
		}
		return false;
	}

	public void SetMenuNavInputLock(bool newState)
	{
		if (newState)
		{
			InputLockManager.SetControlLock(ControlTypes.CAMERACONTROLS, "MenuNavigation");
		}
		else
		{
			InputLockManager.RemoveControlLock("MenuNavigation");
		}
	}

	public void SetSearchField()
	{
		int num = 0;
		UINavMouseChecker component;
		while (true)
		{
			if (num >= selectableItems.Count)
			{
				return;
			}
			if (string.Equals(selectableItems[num].gameObject.name, "SearchInput"))
			{
				component = selectableItems[num].gameObject.GetComponent<UINavMouseChecker>();
				if (component != null)
				{
					break;
				}
			}
			num++;
		}
		component.IsSearchField = true;
	}
}
