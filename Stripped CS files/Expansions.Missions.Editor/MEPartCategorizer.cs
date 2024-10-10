using System.Collections.Generic;
using ns11;
using ns2;
using ns5;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEPartCategorizer : BasePartCategorizer
{
	public MEPartCategorizerButton buttonPrefab;

	public Color selectionColor;

	public MEPartSelectorBrowser partSelector;

	public List<MEPartCategorizerButton> categoryButtons;

	public Dictionary<string, List<AvailablePart>> partsCategoryCache;

	public Dictionary<string, List<AvailablePart>> selectedParts;

	public float scrollStep = 0.1f;

	[SerializeField]
	public ScrollRect partCategoryScroll;

	[SerializeField]
	public PointerClickAndHoldHandler GetScrollBtnDown;

	[SerializeField]
	public PointerClickAndHoldHandler GetScrollBtnUp;

	public new void Awake()
	{
		partsCategoryCache = new Dictionary<string, List<AvailablePart>>();
		selectedParts = new Dictionary<string, List<AvailablePart>>();
		categoryButtons = new List<MEPartCategorizerButton>();
		searchField.onValueChanged.AddListener(SearchField_OnValueChange);
		searchField.onEndEdit.AddListener(SearchField_OnEndEdit);
		searchFieldBackground = searchField.GetComponent<Image>();
		searchFieldClickHandler = searchField.gameObject.AddComponent<PointerClickHandler>();
		searchFieldClickHandler.onPointerClick.AddListener(SearchField_OnClick);
		GetScrollBtnDown.onPointerDownHold.AddListener(OnPointerDown);
		GetScrollBtnUp.onPointerDownHold.AddListener(OnPointerUp);
	}

	public void Setup(MEPartSelectorBrowser partSelector)
	{
		this.partSelector = partSelector;
		selectionColor = partSelector.SelectionColor;
		iconLoader = Object.Instantiate(iconLoaderPrefab).GetComponent<IconLoader>();
		MEPartCategorizerButton mEPartCategorizerButton = InstantiatePartCategorizerButton("Pods", "#autoLOC_453549", iconLoader.GetIcon("stockIcon_pods"), colorFilterFunction, colorIcons, filterPods);
		InstantiatePartCategorizerButton("Fuel Tanks", "#autoLOC_453552", iconLoader.GetIcon("stockIcon_fueltank"), colorFilterFunction, colorIcons, filterFuelTank);
		InstantiatePartCategorizerButton("Engines", "#autoLOC_453555", iconLoader.GetIcon("stockIcon_engine"), colorFilterFunction, colorIcons, filterEngine);
		InstantiatePartCategorizerButton("Command and Control", "#autoLOC_453558", iconLoader.GetIcon("stockIcon_cmdctrl"), colorFilterFunction, colorIcons, filterControl);
		InstantiatePartCategorizerButton("Structural", "#autoLOC_453561", iconLoader.GetIcon("stockIcon_structural"), colorFilterFunction, colorIcons, filterStructural);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			InstantiatePartCategorizerButton("Robotics", "#autoLOC_8003290", iconLoader.GetIcon("serenityIcon_robotics"), colorFilterFunction, colorIcons, filterRobotics);
		}
		InstantiatePartCategorizerButton("Coupling", "#autoLOC_453564", iconLoader.GetIcon("stockIcon_coupling"), colorFilterFunction, colorIcons, filterCoupling);
		InstantiatePartCategorizerButton("Payload", "#autoLOC_453567", iconLoader.GetIcon("stockIcon_payload"), colorFilterFunction, colorIcons, filterPayload);
		InstantiatePartCategorizerButton("Aerodynamics", "#autoLOC_453570", iconLoader.GetIcon("stockIcon_aerodynamics"), colorFilterFunction, colorIcons, filterAero);
		InstantiatePartCategorizerButton("Ground", "#autoLOC_453573", iconLoader.GetIcon("stockIcon_ground"), colorFilterFunction, colorIcons, filterGround);
		InstantiatePartCategorizerButton("Thermal", "#autoLOC_453576", iconLoader.GetIcon("stockIcon_thermal"), colorFilterFunction, colorIcons, filterThermal);
		InstantiatePartCategorizerButton("Electrical", "#autoLOC_453579", iconLoader.GetIcon("stockIcon_electrical"), colorFilterFunction, colorIcons, filterElectrical);
		InstantiatePartCategorizerButton("Communication", "#autoLOC_453582", iconLoader.GetIcon("stockIcon_communication"), colorFilterFunction, colorIcons, filterCommunication);
		InstantiatePartCategorizerButton("Science", "#autoLOC_453585", iconLoader.GetIcon("stockIcon_science"), colorFilterFunction, colorIcons, filterScience);
		InstantiatePartCategorizerButton("Cargo", "#autoLOC_8320001", iconLoader.GetIcon("stockIcon_cargo"), colorFilterFunction, colorIcons, filterCargo);
		InstantiatePartCategorizerButton("Utility", "#autoLOC_453588", iconLoader.GetIcon("stockIcon_utility"), colorFilterFunction, colorIcons, filterUtility);
		mEPartCategorizerButton.activeButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
	}

	public MEPartCategorizerButton InstantiatePartCategorizerButton(string categoryName, string tooltip, Icon icon, Color colorButton, Color colorIcon, EditorPartListFilter<AvailablePart> filter)
	{
		MEPartCategorizerButton mEPartCategorizerButton = Object.Instantiate(buttonPrefab);
		mEPartCategorizerButton.InitializeToggleBtn(categoryName, tooltip, icon, colorButton, colorIcon);
		mEPartCategorizerButton.transform.SetParent(scrollListSub.ListAnchor, worldPositionStays: false);
		mEPartCategorizerButton.activeButton.onClick.AddListener(OnClick);
		mEPartCategorizerButton.activeButton.onTrueBtn.AddListener(OnTrue);
		mEPartCategorizerButton.activeButton.onFalseBtn.AddListener(OnFalse);
		mEPartCategorizerButton.activeButton.Data = filter;
		mEPartCategorizerButton.activeButton.unselectable = true;
		mEPartCategorizerButton.partsSelected.gameObject.SetActive(value: false);
		categoryButtons.Add(mEPartCategorizerButton);
		List<AvailablePart> list = new List<AvailablePart>(PartLoader.LoadedPartsList);
		EditorPartListFilter<AvailablePart>.FilterList(list, filter.FilterCriteria);
		partsCategoryCache.Add(categoryName, list);
		List<AvailablePart> list2 = SelectedPartsOfCategory(list, partSelector.SelectedParts);
		selectedParts.Add(categoryName, list2);
		if (list2.Count > 0)
		{
			mEPartCategorizerButton.partsSelected.gameObject.SetActive(value: true);
			mEPartCategorizerButton.partsSelected.color = selectionColor;
		}
		return mEPartCategorizerButton;
	}

	public void OnTrue(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		partSelector.OnFilterSelectionChange((EditorPartListFilter<AvailablePart>)button.Data, status: true);
		if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
		{
			if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			{
				List<AvailablePart> list = partsCategoryCache[button.GetComponentInParent<MEPartCategorizerButton>().categoryName];
				int count = list.Count;
				while (count-- > 0)
				{
					partSelector.SetPartSelectionStatus(list[count], selected: false);
				}
			}
		}
		else
		{
			List<AvailablePart> list2 = partsCategoryCache[button.GetComponentInParent<MEPartCategorizerButton>().categoryName];
			int count2 = list2.Count;
			while (count2-- > 0)
			{
				partSelector.SetPartSelectionStatus(list2[count2], selected: true);
			}
		}
	}

	public void OnFalse(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		partSelector.OnFilterSelectionChange((EditorPartListFilter<AvailablePart>)button.Data, status: false);
	}

	public void OnClick(PointerEventData eventData, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		if (state == UIRadioButton.State.False)
		{
			eventData.pointerPress.GetComponent<UIRadioButton>().SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
		}
	}

	public override void SearchFilterResult(EditorPartListFilter<AvailablePart> filter)
	{
		partSelector.OnSearchChange(filter);
	}

	public List<AvailablePart> SelectedPartsOfCategory(List<AvailablePart> categoryParts, List<string> selectedParts)
	{
		List<AvailablePart> list = new List<AvailablePart>();
		if (selectedParts.Count > 0)
		{
			int count = categoryParts.Count;
			while (count-- > 0)
			{
				if (selectedParts.Contains(categoryParts[count].name))
				{
					list.Add(categoryParts[count]);
				}
			}
		}
		return list;
	}

	public void PartSelected(AvailablePart part, bool status)
	{
		int count = categoryButtons.Count;
		while (count-- > 0)
		{
			if (partsCategoryCache[categoryButtons[count].categoryName].Contains(part))
			{
				if (status)
				{
					categoryButtons[count].partsSelected.gameObject.SetActive(value: true);
					categoryButtons[count].partsSelected.color = selectionColor;
					selectedParts[categoryButtons[count].categoryName].AddUnique(part);
				}
				else
				{
					List<AvailablePart> list = selectedParts[categoryButtons[count].categoryName];
					list.Remove(part);
					categoryButtons[count].partsSelected.gameObject.SetActive(list.Count > 0);
				}
			}
		}
	}

	public void Reset()
	{
		selectionColor = partSelector.SelectionColor;
		int count = categoryButtons.Count;
		while (count-- > 0)
		{
			List<AvailablePart> list = SelectedPartsOfCategory(partsCategoryCache[categoryButtons[count].categoryName], partSelector.SelectedParts);
			if (list.Count > 0)
			{
				categoryButtons[count].partsSelected.gameObject.SetActive(value: true);
				categoryButtons[count].partsSelected.color = selectionColor;
			}
			else
			{
				categoryButtons[count].partsSelected.gameObject.SetActive(value: false);
				categoryButtons[count].partsSelected.color = Color.white;
			}
			selectedParts[categoryButtons[count].categoryName] = list;
		}
	}

	public void ScrollWithButtons(float direction)
	{
		partCategoryScroll.verticalNormalizedPosition = Mathf.Clamp(partCategoryScroll.verticalNormalizedPosition + scrollStep * direction, 0f, 1f);
	}

	public void OnPointerDown(PointerEventData data)
	{
		ScrollWithButtons(-1f);
	}

	public void OnPointerUp(PointerEventData data)
	{
		ScrollWithButtons(1f);
	}

	public void OnDestroy()
	{
		GetScrollBtnDown.onPointerDownHold.RemoveListener(OnPointerDown);
		GetScrollBtnUp.onPointerDownHold.RemoveListener(OnPointerDown);
	}
}
