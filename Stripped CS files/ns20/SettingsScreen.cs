using System.Collections;
using System.Collections.Generic;
using ns2;
using ns21;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns20;

public class SettingsScreen : MonoBehaviour
{
	public ConfigNode keysMapConfig;

	public SettingsScreenSetup setupPrefab;

	public SettingsSetup setup;

	public Button buttonApply;

	public Button buttonAccept;

	public Button buttonCancel;

	public Button buttonRevert;

	public Button buttonReset;

	public GameObject popUpReset;

	public SettingsScreenTab mainTabPrefab;

	public LayoutGroup mainTabLayout;

	public RectTransform mainScreenParent;

	public List<Selectable> selectablesCached = new List<Selectable>();

	public SettingsScreenTab subTabPrefab;

	public LayoutGroup subTabLayout;

	public RectTransform subTabScreenParent;

	public SettingsTemplate basicTemplate;

	public MenuNavigation menuNav;

	public bool firstTimeLoadingMenuNav;

	public int currentSelectionColumn;

	public int currentSelectableItemIndex;

	public int newSelectableIndex;

	public Transform contentContainer;

	public Transform menuNavCachedTransform;

	public List<SettingsScreenTab> tabsMain = new List<SettingsScreenTab>();

	public static SettingsScreen Instance { get; set; }

	public ConfigNode KeysMapConfig
	{
		set
		{
			keysMapConfig = value;
		}
	}

	public List<SettingsScreenTab> Tabs => tabsMain;

	public void Awake()
	{
		Instance = this;
		buttonApply.onClick.AddListener(OnApply);
		buttonAccept.onClick.AddListener(OnAccept);
		buttonCancel.onClick.AddListener(OnCancel);
		buttonReset.onClick.AddListener(OnReset);
		if (buttonRevert != null)
		{
			buttonRevert.onClick.AddListener(OnRevert);
		}
		setup = setupPrefab.setup;
	}

	public void Start()
	{
		Setup();
		string key = (string.IsNullOrEmpty(GameSettings.CURRENT_LAYOUT_SETTINGS) ? GameSettings.DetectKeyboardLayout() : GameSettings.CURRENT_LAYOUT_SETTINGS);
		if (GameSettings.KeyboardLayouts.ContainsKey(key))
		{
			keysMapConfig = GameSettings.KeyboardLayouts[key].GetNode("KEY_MAP");
		}
		OnOpenWindow();
		menuNav = base.gameObject.AddComponent(typeof(MenuNavigation)) as MenuNavigation;
		LoadSelectables();
		CacheCurrentSelectedItemColumn();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			LoadSelectables();
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
		{
			CacheCurrentSelectedItemColumn();
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (menuNavCachedTransform == null && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
			{
				CacheCurrentSelectedItemColumn();
			}
			NavigateBetweenColumns();
		}
	}

	public void LoadSelectables()
	{
		StartCoroutine(LoadSelectablesCo());
	}

	public IEnumerator LoadSelectablesCo()
	{
		selectablesCached.Clear();
		yield return null;
		for (int i = 0; i < mainTabLayout.transform.childCount; i++)
		{
			if (mainTabLayout.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				selectablesCached.Add(mainTabLayout.transform.GetChild(i).gameObject.GetComponentInChildren<Selectable>());
			}
		}
		for (int j = 0; j < mainScreenParent.transform.childCount; j++)
		{
			if (mainScreenParent.transform.GetChild(j).gameObject.activeInHierarchy)
			{
				SetExplicitUINavigation(mainScreenParent.transform.GetChild(j).transform);
			}
		}
		for (int k = 0; k < subTabLayout.transform.childCount; k++)
		{
			if (subTabLayout.transform.GetChild(k).gameObject.activeInHierarchy)
			{
				Selectable[] componentsInChildren = subTabLayout.transform.GetChild(k).GetComponentsInChildren<Selectable>();
				for (int l = 0; l < componentsInChildren.Length; l++)
				{
					selectablesCached.Add(componentsInChildren[l]);
				}
			}
		}
		for (int m = 0; m < subTabScreenParent.transform.childCount; m++)
		{
			if (subTabScreenParent.transform.GetChild(m).gameObject.activeInHierarchy)
			{
				Selectable[] componentsInChildren2 = subTabScreenParent.transform.GetChild(m).GetComponentsInChildren<Selectable>();
				for (int n = 0; n < componentsInChildren2.Length; n++)
				{
					selectablesCached.Add(componentsInChildren2[n]);
				}
			}
		}
		Transform transform = TransformExtension.FindChild(base.transform, "ActionButtons");
		for (int num = 0; num < transform.childCount; num++)
		{
			Selectable[] componentsInChildren3 = transform.GetComponentsInChildren<Selectable>();
			for (int num2 = 0; num2 < componentsInChildren3.Length; num2++)
			{
				selectablesCached.Add(componentsInChildren3[num2]);
			}
		}
		Selectable[] items = selectablesCached.ToArray();
		menuNav.ResetAll();
		menuNav.SetSelectableItems(items, Navigation.Mode.Automatic, hasText: true, resetNavMode: true);
	}

	public void SetExplicitUINavigation(Transform t)
	{
		Selectable selectable = null;
		if (!firstTimeLoadingMenuNav)
		{
			if (mainTabLayout.transform.childCount > 0)
			{
				selectable = mainTabLayout.transform.GetChild(0).GetComponent<Selectable>();
			}
			firstTimeLoadingMenuNav = true;
		}
		else
		{
			for (int i = 0; i < menuNav.selectableItems.Count; i++)
			{
				UIRadioButton component = menuNav.selectableItems[i].GetComponent<UIRadioButton>();
				if (component != null && component.StartState == UIRadioButton.State.True)
				{
					selectable = menuNav.selectableItems[i];
					break;
				}
			}
		}
		List<Selectable> list = new List<Selectable>(TransformExtension.FindChild(t, "ContentA").GetComponentsInChildren<Selectable>());
		for (int j = 0; j < list.Count; j++)
		{
			if (!list[j].IsInteractable())
			{
				list.Remove(list[j]);
			}
		}
		List<Selectable> list2 = new List<Selectable>(TransformExtension.FindChild(t, "ContentB").GetComponentsInChildren<Selectable>());
		for (int k = 0; k < list2.Count; k++)
		{
			if (!list2[k].IsInteractable())
			{
				list2.Remove(list2[k]);
			}
		}
		Navigation navigation = default(Navigation);
		navigation.mode = Navigation.Mode.Explicit;
		if (list.Count > 1)
		{
			navigation.selectOnDown = list[1];
		}
		if (selectable != null)
		{
			if (list.Count > 0 && !(list[0] is Slider))
			{
				navigation.selectOnLeft = selectable;
			}
			navigation.selectOnUp = selectable;
		}
		if (list.Count > 0)
		{
			list[0].navigation = navigation;
		}
		if (list.Count > 1)
		{
			for (int l = 1; l < list.Count - 1; l++)
			{
				Navigation navigation2 = default(Navigation);
				navigation2.mode = Navigation.Mode.Explicit;
				navigation2.selectOnDown = list[l + 1];
				navigation2.selectOnUp = list[l - 1];
				if (selectable != null && !(list[l] is Slider))
				{
					navigation2.selectOnLeft = selectable;
				}
				list[l].navigation = navigation2;
			}
		}
		if (list2.Count > 1)
		{
			for (int m = 1; m < list2.Count - 1; m++)
			{
				Navigation navigation3 = default(Navigation);
				navigation3.mode = Navigation.Mode.Explicit;
				navigation3.selectOnDown = list2[m + 1];
				navigation3.selectOnUp = list2[m - 1];
				list2[m].navigation = navigation3;
			}
		}
		Navigation navigation4 = default(Navigation);
		navigation4.mode = Navigation.Mode.Explicit;
		if (list2.Count > 0)
		{
			navigation4.selectOnDown = list2[0];
			if (!(list2[1] is Slider) && list2.Count > 1)
			{
				navigation4.selectOnRight = list2[1];
			}
		}
		if (list.Count > 1)
		{
			navigation4.selectOnUp = list[list.Count - 2];
		}
		if (list.Count > 0)
		{
			list[list.Count - 1].navigation = navigation4;
		}
		if (list2.Count > 0 && list.Count > 0)
		{
			Navigation navigation5 = default(Navigation);
			navigation5.mode = Navigation.Mode.Explicit;
			if (!(list2[0] is Slider))
			{
				navigation5.selectOnLeft = list[list.Count - 1];
			}
			navigation5.selectOnUp = list[list.Count - 1];
			navigation5.selectOnDown = list2[1];
			list2[0].navigation = navigation5;
		}
		for (int n = 0; n < list.Count; n++)
		{
			selectablesCached.Add(list[n]);
		}
		for (int num = 0; num < list2.Count; num++)
		{
			selectablesCached.Add(list2[num]);
		}
	}

	public IEnumerator LoadResetPopupSelectablesCo()
	{
		selectablesCached.Clear();
		yield return null;
		Selectable[] componentsInChildren = popUpReset.transform.GetComponentsInChildren<Selectable>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			selectablesCached.Add(componentsInChildren[i]);
		}
		if (componentsInChildren.Length > 1)
		{
			Navigation navigation = default(Navigation);
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnRight = componentsInChildren[1];
			navigation.selectOnDown = componentsInChildren[1];
			navigation.selectOnLeft = componentsInChildren[0];
			navigation.selectOnUp = componentsInChildren[1];
			componentsInChildren[0].navigation = navigation;
			Navigation navigation2 = default(Navigation);
			navigation2.mode = Navigation.Mode.Explicit;
			navigation2.selectOnRight = componentsInChildren[1];
			navigation.selectOnDown = componentsInChildren[0];
			navigation.selectOnLeft = componentsInChildren[0];
			navigation.selectOnUp = componentsInChildren[0];
			componentsInChildren[1].navigation = navigation2;
		}
		Selectable[] items = selectablesCached.ToArray();
		menuNav.ResetAll();
		menuNav.SetSelectableItems(items, Navigation.Mode.Automatic, hasText: true, resetNavMode: true);
	}

	public void CacheCurrentSelectedItemColumn()
	{
		if (!(EventSystem.current != null) || !(EventSystem.current.currentSelectedGameObject != null))
		{
			return;
		}
		bool flag = false;
		menuNavCachedTransform = EventSystem.current.currentSelectedGameObject.transform;
		if (menuNavCachedTransform.parent != null)
		{
			currentSelectableItemIndex = menuNavCachedTransform.transform.parent.GetSiblingIndex();
		}
		while (!flag && !(menuNavCachedTransform.parent == null))
		{
			switch (menuNavCachedTransform.parent.name)
			{
			case "ContentB":
				flag = true;
				currentSelectionColumn = 3;
				break;
			case "ContentA":
				flag = true;
				currentSelectionColumn = 2;
				break;
			case "MainTabButtons":
				flag = true;
				currentSelectionColumn = 1;
				break;
			}
			menuNavCachedTransform = menuNavCachedTransform.parent;
		}
		if (!flag)
		{
			currentSelectionColumn = 0;
		}
	}

	public void NavigateBetweenColumns()
	{
		switch (currentSelectionColumn)
		{
		case 1:
			TryNavigatingNextContentColumn("ContentA", currentSelectableItemIndex);
			break;
		case 2:
			TryNavigatingNextContentColumn("ContentB", currentSelectableItemIndex);
			break;
		case 3:
			FocusSelectableWithinRange(mainTabLayout.transform);
			break;
		}
	}

	public void TryNavigatingNextContentColumn(string contentColumn, int itemIndex)
	{
		contentContainer = null;
		if (contentContainer == null)
		{
			contentContainer = mainScreenParent.FindChild(contentColumn, findActiveChild: true);
			if (contentContainer != null)
			{
				contentContainer = contentContainer.FindChild("Layout", findActiveChild: true);
			}
			else
			{
				contentContainer = subTabLayout.transform.FindChild(contentColumn, findActiveChild: true);
				if (contentContainer != null)
				{
					contentContainer = contentContainer.FindChild("Layout", findActiveChild: true);
				}
				else
				{
					contentContainer = subTabScreenParent.transform.FindChild(contentColumn, findActiveChild: true);
					if (contentContainer != null)
					{
					}
				}
			}
		}
		if (contentContainer != null)
		{
			FocusSelectableWithinRange(contentContainer);
		}
	}

	public void FocusSelectableWithinRange(Transform parent)
	{
		if (parent.childCount <= 0)
		{
			return;
		}
		newSelectableIndex = Mathf.Clamp(currentSelectableItemIndex, 0, parent.childCount - 1);
		if (parent.childCount > newSelectableIndex)
		{
			Selectable selectable = parent.GetChild(newSelectableIndex).gameObject.GetComponent<Selectable>();
			if (selectable == null)
			{
				selectable = parent.GetChild(newSelectableIndex).gameObject.GetComponentInChildren<Selectable>();
			}
			if (selectable != null)
			{
				menuNav.SetItemAsFirstSelected(selectable.gameObject);
				CacheCurrentSelectedItemColumn();
			}
		}
	}

	public void OnOpenWindow()
	{
	}

	public void OnCloseWindow()
	{
		if (HighLogic.fetch != null)
		{
			HighLogic.LoadScene(GameScenes.MAINMENU);
		}
	}

	public void OnApply()
	{
		SettingsLayoutConfig.SaveChanges();
		SettingsControlBase[] componentsInChildren = GetComponentsInChildren<SettingsControlBase>(includeInactive: true);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].OnApply();
		}
		GameSettings.SaveSettings();
		GameSettings.ApplySettings();
		GameEvents.OnGameSettingsApplied.Fire();
	}

	public void OnAccept()
	{
		OnApply();
		OnCloseWindow();
	}

	public void OnCancel()
	{
		OnCloseWindow();
	}

	public void OnReset()
	{
		MonoBehaviour.print("Reset Settings!");
		popUpReset.SetActive(value: true);
		StartCoroutine(LoadResetPopupSelectablesCo());
	}

	public void CloseResetPopUp(bool resetStatus)
	{
		popUpReset.SetActive(value: false);
		if (resetStatus)
		{
			GameSettings.ResetSettings();
			OnCloseWindow();
		}
		LoadSelectables();
	}

	public void OnRevert()
	{
		SettingsLayoutConfig.OnRevert();
		SettingsControlBase[] componentsInChildren = GetComponentsInChildren<SettingsControlBase>(includeInactive: true);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].OnRevert();
		}
	}

	public string GetKeyLayoutMap(string key)
	{
		if (keysMapConfig != null && keysMapConfig.HasValue(key))
		{
			return keysMapConfig.GetValue(key.ToString());
		}
		return key.ToString();
	}

	public void Clear()
	{
		int i = 0;
		for (int count = Tabs.Count; i < count; i++)
		{
			SettingsScreenTab settingsScreenTab = Tabs[i];
			int j = 0;
			for (int count2 = settingsScreenTab.SpawnedTabs.Count; j < count2; j++)
			{
				SettingsScreenTab settingsScreenTab2 = Tabs[i];
				if (settingsScreenTab2.WindowTransform != null)
				{
					Object.DestroyImmediate(settingsScreenTab2.WindowTransform.gameObject);
				}
				Object.DestroyImmediate(settingsScreenTab2.gameObject);
			}
			if (settingsScreenTab.WindowTransform != null)
			{
				Object.DestroyImmediate(settingsScreenTab.WindowTransform.gameObject);
			}
		}
	}

	public void Setup()
	{
		int num = 99383;
		for (int i = 0; i < setup.tabs.Length; i++)
		{
			SettingsSetup.MainTab mainTab = setup.tabs[i];
			if (mainTab != null && mainTab.IsValid())
			{
				SettingsScreenTab settingsScreenTab = Object.Instantiate(mainTabPrefab);
				settingsScreenTab.transform.SetParent(mainTabLayout.transform, worldPositionStays: false);
				settingsScreenTab.titleText.text = mainTab.name;
				settingsScreenTab.btn.SetGroup(num, pop: false);
				settingsScreenTab.StartTrue = tabsMain.Count == 0;
				tabsMain.Add(settingsScreenTab);
				SetupTab(settingsScreenTab, mainTab, num + i);
			}
		}
	}

	public void SetupTab(SettingsScreenTab tabInstance, SettingsSetup.MainTab tab, int radioGroup)
	{
		if (tab.subTabs.Length != 0)
		{
			for (int i = 0; i < tab.subTabs.Length; i++)
			{
				SettingsSetup.SubTab subTab = tab.subTabs[i];
				if (subTab.IsValid())
				{
					SettingsScreenTab settingsScreenTab = Object.Instantiate(subTabPrefab);
					settingsScreenTab.transform.SetParent(subTabLayout.transform, worldPositionStays: false);
					settingsScreenTab.titleText.text = subTab.name;
					settingsScreenTab.btn.SetGroup(radioGroup, pop: false);
					settingsScreenTab.StartTrue = tabInstance.SpawnedTabs.Count == 0;
					tabInstance.SpawnedTabs.Add(settingsScreenTab);
					SetupWindow(settingsScreenTab, subTab.window, subTabScreenParent);
				}
			}
		}
		else
		{
			SetupWindow(tabInstance, tab.window, mainScreenParent);
		}
	}

	public void SetupWindow(SettingsScreenTab tabInstance, SettingsSetup.Window window, RectTransform screenWindow)
	{
		if (window.prefab == null)
		{
			Debug.LogError("If a tab has no subtabs, it needs to have a window assigned");
		}
		else if (window.prefab.IsValid())
		{
			RectTransform rectTransform = screenWindow;
			SettingsTemplate settingsTemplate = null;
			if (window.templatePrefab != null)
			{
				settingsTemplate = Object.Instantiate(window.templatePrefab);
				settingsTemplate.transform.SetParent(screenWindow, worldPositionStays: false);
				rectTransform = (RectTransform)settingsTemplate.transform;
			}
			else if (window.prefab.templatePrefab != null)
			{
				settingsTemplate = Object.Instantiate(window.prefab.templatePrefab);
				settingsTemplate.transform.SetParent(screenWindow, worldPositionStays: false);
				rectTransform = (RectTransform)settingsTemplate.transform;
			}
			else
			{
				settingsTemplate = Object.Instantiate(basicTemplate);
				settingsTemplate.transform.SetParent(screenWindow, worldPositionStays: false);
				rectTransform = (RectTransform)settingsTemplate.transform;
			}
			SettingsWindow settingsWindow = Object.Instantiate(window.prefab);
			settingsWindow.transform.SetParent(rectTransform, worldPositionStays: false);
			settingsWindow.Template = settingsTemplate;
			SettingsSetup.SetupReflectionValues(settingsWindow, window.values);
			tabInstance.SpawnedWindow = settingsWindow;
			tabInstance.WindowTransform = (RectTransform)settingsTemplate.transform;
		}
	}

	[ContextMenu("Recreate")]
	public void Recreate()
	{
		setup = setupPrefab.setup;
		Clear();
		Setup();
		OnOpenWindow();
	}
}
