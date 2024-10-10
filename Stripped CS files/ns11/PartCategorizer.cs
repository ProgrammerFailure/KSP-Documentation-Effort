using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Contracts.Agents;
using Expansions;
using ns2;
using ns24;
using ns5;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class PartCategorizer : BasePartCategorizer
{
	public enum ButtonType
	{
		FILTER,
		CATEGORY,
		SUBCATEGORY
	}

	public enum SelectionGroup
	{
		FILTER,
		CATEGORY
	}

	public class Category
	{
		public ButtonType buttonType;

		public EditorPartList.State displayType;

		public PartCategorizerButton button;

		public PartCategorizerButton buttonPlus;

		public List<Category> subcategories;

		public int subCategoryGroup;

		public static int subCategoryGroupIndex = 241;

		public Category parent;

		public EditorPartListFilter<AvailablePart> exclusionFilter;

		public EditorPartListFilter<ShipTemplate> exclusionFilterSubassembly;

		public List<string> availableParts;

		public List<string> shipTemplates;

		public Category(ButtonType buttonType, EditorPartList.State displayType, string categoryName, string categorydisplayName, Icon icon, Color color, Color colorIcon, EditorPartListFilter<AvailablePart> exclusionFilter, bool last = false)
		{
			button = InstantiatePartCategorizerButton(categoryName, categorydisplayName, icon, color, colorIcon, last);
			Setup(buttonType, displayType, button, addItem: true);
			this.exclusionFilter = exclusionFilter;
		}

		public Category(ButtonType buttonType, EditorPartList.State displayType, PartCategorizerButton button, EditorPartListFilter<AvailablePart> exclusionFilter, bool addItem)
		{
			Setup(buttonType, displayType, button, addItem);
			this.exclusionFilter = exclusionFilter;
		}

		public Category(ButtonType buttonType, EditorPartList.State displayType, string categoryName, string categorydisplayName, Icon icon, Color color, Color colorIcon, EditorPartListFilter<ShipTemplate> exclusionFilter, bool last = false)
		{
			button = InstantiatePartCategorizerButton(categoryName, categorydisplayName, icon, color, colorIcon, last);
			Setup(buttonType, displayType, button, addItem: true);
			exclusionFilterSubassembly = exclusionFilter;
		}

		public Category(ButtonType buttonType, EditorPartList.State displayType, PartCategorizerButton button, EditorPartListFilter<ShipTemplate> exclusionFilter, bool addItem)
		{
			Setup(buttonType, displayType, button, addItem);
			exclusionFilterSubassembly = exclusionFilter;
		}

		public Category(ButtonType buttonType, EditorPartList.State displayType, string categoryName, string categorydisplayName, Icon icon, Color color, Color colorIcon, bool last = false)
		{
			button = InstantiatePartCategorizerButton(categoryName, categorydisplayName, icon, color, colorIcon, last);
			Setup(buttonType, displayType, button, addItem: true);
		}

		public void Setup(ButtonType buttonType, EditorPartList.State displayType, PartCategorizerButton button, bool addItem)
		{
			this.button = button;
			this.buttonType = buttonType;
			this.displayType = displayType;
			button.activeButton.onTrueBtn.AddListener(OnTrue);
			button.activeButton.onFalseBtn.AddListener(OnFalse);
			button.activeButton.onClick.AddListener(OnClick);
			if (buttonType != ButtonType.SUBCATEGORY)
			{
				if (buttonType == ButtonType.FILTER || buttonType == ButtonType.CATEGORY)
				{
					subcategories = new List<Category>();
					subCategoryGroup = subCategoryGroupIndex++;
				}
				if (addItem)
				{
					Instance.scrollListMain.AddItem(button.container);
				}
			}
			switch (buttonType)
			{
			case ButtonType.FILTER:
				button.SetRadioGroup(0);
				Instance.filters.Add(this);
				break;
			case ButtonType.CATEGORY:
				button.SetRadioGroup(240);
				Instance.categories.Add(this);
				switch (displayType)
				{
				case EditorPartList.State.CustomPartList:
					buttonPlus = Instance.InstantiatePartCategorizerButtonPlus("Add custom subcategory", "#autoLOC_6004018", Instance.iconLoader.GetIcon("internalIcon_plus"), Instance.colorCategory, Color.white);
					buttonPlus.OnBtnTap = AddCustomCategory;
					buttonPlus.container.transform.SetParent(Instance.partCategorizerButtonRepository, worldPositionStays: false);
					break;
				case EditorPartList.State.SubassemblyList:
					buttonPlus = Instance.InstantiatePartCategorizerButtonPlus("Add custom subcategory", "#autoLOC_6004018", Instance.iconLoader.GetIcon("internalIcon_plus"), Instance.colorCategory, Color.white);
					buttonPlus.OnBtnTap = AddCustomCategorySubassembly;
					buttonPlus.container.transform.SetParent(Instance.partCategorizerButtonRepository, worldPositionStays: false);
					break;
				}
				break;
			case ButtonType.SUBCATEGORY:
				button.SetRadioGroup(0);
				switch (displayType)
				{
				case EditorPartList.State.CustomPartList:
					availableParts = new List<string>();
					break;
				case EditorPartList.State.SubassemblyList:
					shipTemplates = new List<string>();
					break;
				}
				button.container.transform.SetParent(Instance.partCategorizerButtonRepository, worldPositionStays: false);
				break;
			}
		}

		public void AddSubassembly(string template, bool compile, bool exclude, bool refresh)
		{
			if (!shipTemplates.Contains(template))
			{
				shipTemplates.Add(template);
			}
			if (compile)
			{
				CompileExclusionFilterSubassembly(exclude, refresh);
			}
		}

		public void RemoveSubassembly(string template, bool compile, bool exclude, bool refresh)
		{
			if (shipTemplates.Contains(template))
			{
				shipTemplates.Remove(template);
			}
			if (compile)
			{
				CompileExclusionFilterSubassembly(exclude, refresh);
			}
		}

		public void CompileExclusionFilterSubassembly(bool exclude = true, bool sendRefresh = true)
		{
			if (exclude)
			{
				Instance.editorPartList.ExcludeFiltersSubassembly.RemoveFilter(exclusionFilterSubassembly);
			}
			Func<ShipTemplate, bool> criteria = (ShipTemplate p) => shipTemplates.Contains(p.shipName);
			exclusionFilterSubassembly = new EditorPartListFilter<ShipTemplate>("CustomSubassembly_" + button.categoryName, criteria);
			if (exclude)
			{
				EditorPartList.Instance.ExcludeFiltersSubassembly.AddFilter(exclusionFilterSubassembly);
			}
			if (sendRefresh)
			{
				Instance.refreshRequested = true;
			}
		}

		public void AddPart(AvailablePart part)
		{
			if (!availableParts.Contains(part.name))
			{
				availableParts.Add(part.name);
			}
			CompileExclusionFilter();
		}

		public void AddPart(string partName, bool compile)
		{
			if (!availableParts.Contains(partName))
			{
				availableParts.Add(partName);
			}
			CompileExclusionFilter(compile);
		}

		public void RemovePart(AvailablePart part)
		{
			if (availableParts.Contains(part.name))
			{
				availableParts.Remove(part.name);
			}
			CompileExclusionFilter();
		}

		public void CompileExclusionFilter(bool sendRefresh = true)
		{
			if (sendRefresh)
			{
				Instance.editorPartList.CategorizerFilters.RemoveFilter(exclusionFilter);
			}
			Func<AvailablePart, bool> criteria = (AvailablePart p) => availableParts.Contains(p.name);
			exclusionFilter = new EditorPartListFilter<AvailablePart>("CustomSubCategory_" + button.categoryName, criteria);
			if (sendRefresh)
			{
				EditorPartList.Instance.CategorizerFilters.AddFilter(exclusionFilter);
				Instance.refreshRequested = true;
			}
		}

		public void AddCustomCategorySubassembly()
		{
			PopupData popupData = new PopupData("#autoLOC_6004023", "Unknown", Localizer.Format("#autoLOC_6004021"), Instance.iconLoader.GetIcon("stockIcon_fallback"));
			Instance.ShowPopup(popupData, AcceptNewSubcategorySubassembly, AcceptNewSubcategoryCriteria, null, null);
		}

		public void AcceptNewSubcategorySubassembly()
		{
			AddSubcategory(new Category(ButtonType.SUBCATEGORY, EditorPartList.State.SubassemblyList, Instance.currentPopupData.categoryName, Instance.currentPopupData.categorydisplayName, Instance.currentPopupData.icon, button.color, Instance.colorIcons, Instance.filterNothingSubassembly));
			Instance.scrollListSub.InsertItem(subcategories[subcategories.Count - 1].button.container, subcategories.Count - 1);
			Instance.SaveCustomSubassemblyCategories();
		}

		public void AddSubcategory(Category subcategory)
		{
			subcategory.parent = this;
			subcategory.button.SetRadioGroup(subCategoryGroup);
			subcategory.button.activeButton.onFalse.AddListener(OnFalseFromSubcategory);
			subcategory.button.activeButton.unselectable = false;
			subcategories.Add(subcategory);
		}

		public void AddCustomCategory()
		{
			PopupData popupData = new PopupData("#autoLOC_6004023", "Unknown", Localizer.Format("#autoLOC_6004021"), Instance.iconLoader.GetIcon("stockIcon_fallback"));
			Instance.ShowPopup(popupData, AcceptNewSubcategory, AcceptNewSubcategoryCriteria, null, null);
		}

		public void AcceptNewSubcategory()
		{
			AddSubcategory(new Category(ButtonType.SUBCATEGORY, EditorPartList.State.CustomPartList, Instance.currentPopupData.categoryName, Instance.currentPopupData.categorydisplayName, Instance.currentPopupData.icon, button.color, Instance.colorIcons, Instance.filterCustomCategory));
			Instance.scrollListSub.InsertItem(subcategories[subcategories.Count - 1].button.container, subcategories.Count - 1);
			Instance.SaveCustomPartCategories();
		}

		public void EditCategory()
		{
			button.categoryName = Instance.currentPopupData.categoryName;
			button.categorydisplayName = Instance.currentPopupData.categoryName;
			button.tooltipController.textString = Instance.currentPopupData.categoryName;
			button.SetIcon(Instance.currentPopupData.icon);
			if (displayType == EditorPartList.State.CustomPartList)
			{
				Instance.SaveCustomPartCategories();
			}
			else if (displayType == EditorPartList.State.SubassemblyList)
			{
				Instance.SaveCustomSubassemblyCategories();
			}
		}

		public void DeleteCategory()
		{
			if (button.activeButton.Value)
			{
				OnFalseFilterOrCategory(this);
			}
			RemoveSubcategoryButtons(this);
			Instance.categories.Remove(this);
			Instance.scrollListMain.RemoveItem(button.container, deleteItem: true);
			Instance.refreshRequested = true;
			Instance.SaveCustomPartCategories();
		}

		public void DeleteSubcategory()
		{
			parent.subcategories.Remove(this);
			if (button.activeButton.Value)
			{
				OnFalseSUB(this);
				UpdateSubcategoryStates(parent, 0, insertItems: false);
			}
			Instance.scrollListSub.RemoveItem(button.container, deleteItem: true);
			Instance.refreshRequested = true;
			if (displayType == EditorPartList.State.CustomPartList)
			{
				Instance.SaveCustomPartCategories();
			}
			else if (displayType == EditorPartList.State.SubassemblyList)
			{
				Instance.SaveCustomSubassemblyCategories();
			}
		}

		public bool AcceptNewSubcategoryCriteria(string categoryName, out string reason)
		{
			if (subcategories.Exists((Category a) => a.button.categoryName.Equals(categoryName)))
			{
				reason = "#autoLOC_6004024";
				return false;
			}
			reason = "Everything ok";
			return true;
		}

		public bool EditCategoryCriteria(string categoryName, out string reason)
		{
			if (Instance.categories.Exists((Category a) => a.button.categoryName.Equals(categoryName) && a != this))
			{
				reason = "#autoLOC_6004025";
				return false;
			}
			reason = "Everything ok";
			return true;
		}

		public bool EditSubCategoryCriteria(string categoryName, out string reason)
		{
			if (parent.subcategories.Exists((Category a) => a.button.categoryName.Equals(categoryName) && a != this))
			{
				reason = "#autoLOC_6004026";
				return false;
			}
			reason = "Everything ok";
			return true;
		}

		public bool DeleteCategoryCriteria(out string reason)
		{
			if (subcategories.Count > 0)
			{
				if (subcategories.Exists((Category a) => a.availableParts.Count > 0))
				{
					reason = Localizer.Format("#autoLOC_453138");
					return false;
				}
				reason = Localizer.Format("#autoLOC_453141");
				return false;
			}
			reason = "Everything ok";
			return true;
		}

		public bool DeleteSubcategoryCriteria(out string reason)
		{
			if (displayType == EditorPartList.State.CustomPartList && availableParts.Count > 0)
			{
				reason = Localizer.Format("#autoLOC_453153");
				return false;
			}
			if (displayType == EditorPartList.State.SubassemblyList && shipTemplates.Count > 0)
			{
				reason = Localizer.Format("#autoLOC_453158");
				return false;
			}
			reason = "Everything ok";
			return true;
		}

		public void OnClick(PointerEventData eventData, UIRadioButton.State state, UIRadioButton.CallType callType)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				UIRadioButton component = eventData.pointerPress.GetComponent<UIRadioButton>();
				if (buttonType == ButtonType.CATEGORY && displayType != EditorPartList.State.SubassemblyList && displayType != EditorPartList.State.VariantsList)
				{
					PopupData popupData = new PopupData("#autoLOC_6004031", button.categoryName, button.categorydisplayName, button.icon);
					Instance.ShowPopup(popupData, EditCategory, EditCategoryCriteria, DeleteCategory, DeleteCategoryCriteria);
				}
				else if (buttonType == ButtonType.SUBCATEGORY && parent.buttonType == ButtonType.CATEGORY && (parent.displayType != EditorPartList.State.SubassemblyList || !(component == parent.subcategories[0].button.activeButton)))
				{
					PopupData popupData2 = new PopupData("#autoLOC_6004032", button.categoryName, button.categorydisplayName, button.icon);
					if (component == parent.subcategories[0].button.activeButton)
					{
						Instance.ShowPopup(popupData2, EditCategory, EditSubCategoryCriteria, null, null);
					}
					else
					{
						Instance.ShowPopup(popupData2, EditCategory, EditSubCategoryCriteria, DeleteSubcategory, DeleteSubcategoryCriteria);
					}
				}
			}
			Instance.SearchStop();
		}

		public void OnTrue(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
		{
			Instance.displayType = displayType;
			Instance.refreshRequested = true;
			if (buttonType == ButtonType.SUBCATEGORY)
			{
				OnTrueSUB(this);
			}
			else if (buttonType == ButtonType.FILTER)
			{
				OnTrueFILTER(button, callType, data);
			}
			else if (buttonType == ButtonType.CATEGORY)
			{
				OnTrueCATEGORY();
			}
		}

		public void OnTrueFILTER(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
		{
			if (callType == UIRadioButton.CallType.APPLICATION && button.Value)
			{
				if (Instance.SelectedGroup != 0)
				{
					Instance.SelectedGroup = SelectionGroup.FILTER;
					FlipAllCategoryButtons(button);
				}
				FlipAllFilterButtons(button);
				if (Instance.scrollListSub.Count == 0)
				{
					InsertSubcategoryButtons();
					if (Instance.scrollListSub.Count != 0)
					{
						Instance.editorPartList.CategorizerFilters.RemoveFilter(Instance.filterGenericNothing);
					}
				}
				Instance.displayType = displayType;
			}
			else
			{
				InsertSubcategoryButtons();
				if (Instance.SelectedGroup != 0)
				{
					Instance.SelectedGroup = SelectionGroup.FILTER;
					FlipAllCategoryButtons(button);
				}
				if (Instance.scrollListSub.Count != 0)
				{
					Instance.editorPartList.CategorizerFilters.RemoveFilter(Instance.filterGenericNothing);
				}
				if (data.button == PointerEventData.InputButton.Left)
				{
					FlipAllFilterButtons(button);
				}
			}
		}

		public void OnTrueCATEGORY()
		{
			if (displayType != EditorPartList.State.VariantsList)
			{
				InsertSubcategoryButtons();
			}
			if (Instance.SelectedGroup != SelectionGroup.CATEGORY)
			{
				Instance.SelectedGroup = SelectionGroup.CATEGORY;
				FlipAllFilterButtons(button.activeButton);
			}
			if (Instance.scrollListSub.Count != 0)
			{
				Instance.editorPartList.CategorizerFilters.RemoveFilter(Instance.filterGenericNothing);
			}
		}

		public void OnTrueSUB(Category cat)
		{
			if (cat.displayType == EditorPartList.State.SubassemblyList)
			{
				EditorPartList.Instance.ExcludeFiltersSubassembly.AddFilter(cat.exclusionFilterSubassembly);
			}
			else
			{
				EditorPartList.Instance.CategorizerFilters.AddFilter(cat.exclusionFilter);
			}
			Instance.SelectedCategory = cat;
		}

		public void FlipAllFilterButtons(UIRadioButton exceptionBtn)
		{
			int i = 0;
			for (int count = Instance.filters.Count; i < count; i++)
			{
				if (Instance.filters[i].button.activeButton.Value && exceptionBtn != Instance.filters[i].button.activeButton)
				{
					Instance.filters[i].button.activeButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null);
				}
			}
		}

		public void FlipAllCategoryButtons(UIRadioButton exceptionBtn)
		{
			int i = 0;
			for (int count = Instance.categories.Count; i < count; i++)
			{
				if (Instance.categories[i].button.activeButton.Value && exceptionBtn != Instance.categories[i].button.activeButton)
				{
					Instance.categories[i].button.activeButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null);
				}
			}
		}

		public void RebuildSubcategoryButtons()
		{
			RemoveSubcategoryButtons(this);
			InsertSubcategoryButtons();
		}

		public void InsertSubcategoryButtons()
		{
			int startIndex = 0;
			if (buttonType == ButtonType.FILTER)
			{
				int num = Instance.filters.IndexOf(this) - 1;
				while (num >= 0)
				{
					if (num < 0 || !Instance.filters[num].button.activeButton.Value)
					{
						num--;
						continue;
					}
					startIndex = Instance.scrollListSub.GetIndex(Instance.filters[num].subcategories[Instance.filters[num].subcategories.Count - 1].button.container) + 1;
					break;
				}
			}
			int num2 = UpdateSubcategoryStates(this, startIndex, insertItems: true);
			if (buttonType == ButtonType.CATEGORY)
			{
				Instance.scrollListSub.InsertItem(buttonPlus.container, num2++);
			}
		}

		public int UpdateSubcategoryStates(Category cat, int startIndex, bool insertItems)
		{
			int result = 0;
			bool flag = !cat.subcategories.Exists((Category a) => a.button.activeButton.Value);
			int i = 0;
			for (int count = cat.subcategories.Count; i < count; i++)
			{
				if (insertItems)
				{
					Instance.scrollListSub.InsertItem(cat.subcategories[i].button.container, startIndex + result++);
				}
				if (flag)
				{
					if (i == 0)
					{
						cat.subcategories[i].button.activeButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null, popButtonsInGroup: false);
					}
					else
					{
						cat.subcategories[i].button.activeButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null);
					}
				}
				else if (cat.subcategories[i].button.activeButton.Value && insertItems)
				{
					OnTrueSUB(cat.subcategories[i]);
				}
			}
			return result;
		}

		public void OnFalse(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
		{
			if (callType == UIRadioButton.CallType.APPLICATIONSILENT)
			{
				return;
			}
			Instance.refreshRequested = true;
			if (buttonType == ButtonType.SUBCATEGORY)
			{
				OnFalseSUB(this);
			}
			if (buttonType == ButtonType.FILTER)
			{
				OnFalseFilterOrCategory(this);
				int num = 0;
				for (int num2 = Instance.filters.Count - 1; num2 >= 0; num2--)
				{
					if (Instance.filters[num2].button.activeButton.Value)
					{
						num++;
					}
				}
				if (data != null && data.button == PointerEventData.InputButton.Left && num > 0)
				{
					FlipAllFilterButtons(button);
					button.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
				}
			}
			else if (buttonType == ButtonType.CATEGORY)
			{
				OnFalseFilterOrCategory(this);
			}
		}

		public void OnFalseFilterOrCategory(Category cat)
		{
			RemoveSubcategoryButtons(this);
			if (Instance.scrollListSub.Count == 0 && Instance.displayType != EditorPartList.State.VariantsList)
			{
				EditorPartList.Instance.CategorizerFilters.AddFilter(Instance.filterGenericNothing);
				Instance.displayType = EditorPartList.State.Nothing;
			}
		}

		public void RemoveSubcategoryButtons(Category cat)
		{
			int i = 0;
			for (int count = cat.subcategories.Count; i < count; i++)
			{
				if (cat.subcategories[i].button.activeButton.Value)
				{
					OnFalseSUB(cat.subcategories[i]);
				}
				Instance.scrollListSub.RemoveItemAndMove(cat.subcategories[i].button.container, Instance.partCategorizerButtonRepository);
			}
			if (buttonType == ButtonType.CATEGORY && displayType != EditorPartList.State.VariantsList)
			{
				Instance.scrollListSub.RemoveItemAndMove(cat.buttonPlus.container, Instance.partCategorizerButtonRepository);
			}
		}

		public void OnFalseFromSubcategory(PointerEventData data, UIRadioButton.CallType callType)
		{
			if (!subcategories.Exists((Category a) => a.button.activeButton.Value))
			{
				button.activeButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null);
			}
		}

		public void OnFalseSUB(Category cat)
		{
			if (cat.displayType == EditorPartList.State.SubassemblyList)
			{
				Instance.editorPartList.ExcludeFiltersSubassembly.RemoveFilter(cat.exclusionFilterSubassembly);
			}
			else
			{
				Instance.editorPartList.CategorizerFilters.RemoveFilter(cat.exclusionFilter);
			}
		}
	}

	public class PopupData
	{
		public string popupName;

		public string categoryName;

		public string categorydisplayName;

		public Icon icon;

		public PopupData(string popupName, string categoryName, string categorydisplayName, Icon icon)
		{
			this.popupName = popupName;
			this.categoryName = categoryName;
			this.categorydisplayName = categorydisplayName;
			this.icon = icon;
		}
	}

	public static PartCategorizer Instance;

	public static bool Ready;

	public UIList scrollListMain;

	public PartCategorizerButton buttonPrefab;

	public EditorPartList editorPartList;

	public TextMeshProUGUI labelCategoryName;

	public Button arrowRight;

	public Button arrowLeft;

	public PartCategorizerPopup popupPrefab;

	public PartCategorizerPopup popup;

	public PartCategorizerPopupAddPart popupAddPartPrefab;

	public PartCategorizerPopupAddPart popupAddPart;

	public RectTransform popupArea;

	public TechTier[] TechTierCategories;

	public BulkheadProfile[] BulkheadProfiles;

	public UIRadioButton btnSearch;

	public Category SelectedCategory;

	public bool refreshRequested;

	public bool subcategoryButtonRebuildRequested;

	public EditorPartList.State displayType;

	public bool searching;

	public List<Category> filters = new List<Category>();

	public List<Category> categories = new List<Category>();

	public Category subassemblies;

	public Category subassembliesAll;

	public Category variants;

	public new EditorPartListFilter<AvailablePart> filterPods = new EditorPartListFilter<AvailablePart>("Function_Pods", (AvailablePart p) => p.category == PartCategories.Pods);

	public new EditorPartListFilter<AvailablePart> filterEngine = new EditorPartListFilter<AvailablePart>("Function_Engine", (AvailablePart p) => p.category == PartCategories.Engine || (p.category == PartCategories.Propulsion && p.moduleInfos.Exists((AvailablePart.ModuleInfo q) => q.moduleName == "Engine")));

	public new EditorPartListFilter<AvailablePart> filterFuelTank = new EditorPartListFilter<AvailablePart>("Function_FuelTank", (AvailablePart p) => p.category == PartCategories.FuelTank || (p.category == PartCategories.Propulsion && !p.moduleInfos.Exists((AvailablePart.ModuleInfo q) => q.moduleName == "Engine")));

	public new EditorPartListFilter<AvailablePart> filterControl = new EditorPartListFilter<AvailablePart>("Function_Control", (AvailablePart p) => p.category == PartCategories.Control);

	public new EditorPartListFilter<AvailablePart> filterStructural = new EditorPartListFilter<AvailablePart>("Function_Structural", (AvailablePart p) => p.category == PartCategories.Structural);

	public new EditorPartListFilter<AvailablePart> filterCoupling = new EditorPartListFilter<AvailablePart>("Function_Coupling", (AvailablePart p) => p.category == PartCategories.Coupling);

	public new EditorPartListFilter<AvailablePart> filterPayload = new EditorPartListFilter<AvailablePart>("Function_Payload", (AvailablePart p) => p.category == PartCategories.Payload);

	public new EditorPartListFilter<AvailablePart> filterAero = new EditorPartListFilter<AvailablePart>("Function_Aero", (AvailablePart p) => p.category == PartCategories.Aero);

	public new EditorPartListFilter<AvailablePart> filterGround = new EditorPartListFilter<AvailablePart>("Function_Ground", (AvailablePart p) => p.category == PartCategories.Ground);

	public new EditorPartListFilter<AvailablePart> filterThermal = new EditorPartListFilter<AvailablePart>("Function_Thermal", (AvailablePart p) => p.category == PartCategories.Thermal);

	public new EditorPartListFilter<AvailablePart> filterElectrical = new EditorPartListFilter<AvailablePart>("Function_Electrical", (AvailablePart p) => p.category == PartCategories.Electrical);

	public new EditorPartListFilter<AvailablePart> filterCommunication = new EditorPartListFilter<AvailablePart>("Function_Communication", (AvailablePart p) => p.category == PartCategories.Communication);

	public new EditorPartListFilter<AvailablePart> filterScience = new EditorPartListFilter<AvailablePart>("Function_Science", (AvailablePart p) => p.category == PartCategories.Science);

	public new EditorPartListFilter<AvailablePart> filterCargo = new EditorPartListFilter<AvailablePart>("Function_Cargo", (AvailablePart p) => p.category == PartCategories.Cargo);

	public new EditorPartListFilter<AvailablePart> filterRobotics = new EditorPartListFilter<AvailablePart>("Function_Robotics", (AvailablePart p) => p.category == PartCategories.Robotics);

	public new EditorPartListFilter<AvailablePart> filterUtility = new EditorPartListFilter<AvailablePart>("Function_Utility", (AvailablePart p) => p.category == PartCategories.Utility);

	public Category filterFunction;

	public Category subcategoryFunctionPods;

	public Category subcategoryFunctionEngine;

	public Category subcategoryFunctionFuelTank;

	public Category subcategoryFunctionControl;

	public Category subcategoryFunctionStructural;

	public Category subcategoryFunctionCoupling;

	public Category subcategoryFunctionPayload;

	public Category subcategoryFunctionAero;

	public Category subcategoryFunctionGround;

	public Category subcategoryFunctionThermal;

	public Category subcategoryFunctionElectrical;

	public Category subcategoryFunctionCommunication;

	public Category subcategoryFunctionScience;

	public Category subcategoryFunctionCargo;

	public Category subcategoryFunctionRobotics;

	public Category subcategoryFunctionUtility;

	public static bool alwaysShowCargoTab;

	public EditorPartListFilter<AvailablePart> filter_LeftBar = new EditorPartListFilter<AvailablePart>("FilterLeftBar", (AvailablePart p) => false);

	public EditorPartListFilter<ShipTemplate> filterSubassemblies = new EditorPartListFilter<ShipTemplate>("Subassembly_LeftBar", (ShipTemplate p) => false);

	public EditorPartListFilter<ShipTemplate> filterNothingSubassembly = new EditorPartListFilter<ShipTemplate>("Subassembly_Nothing", (ShipTemplate p) => false);

	public EditorPartListFilter<ShipTemplate> filterEverythingSubassembly = new EditorPartListFilter<ShipTemplate>("Subassembly_Everything", (ShipTemplate p) => true);

	public EditorPartListFilter<AvailablePart> filterCustomCategory = new EditorPartListFilter<AvailablePart>("Parts_CustomCategory", (AvailablePart p) => false);

	public EditorPartListFilter<AvailablePart> filterGenericNothing = new EditorPartListFilter<AvailablePart>("FilterGenericNothing", (AvailablePart p) => false);

	[NonSerialized]
	public Color colorFilterModule = new Color(0.76f, 0.682f, 0.922f, 1f);

	[NonSerialized]
	public Color colorFilterResource = new Color(1f, 0.859f, 0.506f, 1f);

	[NonSerialized]
	public Color colorFilterManufacturer = new Color(1f, 1f, 1f, 1f);

	[NonSerialized]
	public Color colorFilterTech = new Color(0.537f, 0.71f, 1f, 1f);

	[NonSerialized]
	public Color colorFilterProfile = new Color(0.537f, 0.71f, 1f, 1f);

	[NonSerialized]
	public Color colorSubassembly = new Color(0.553f, 1f, 0.737f, 1f);

	[NonSerialized]
	public Color colorVariants = new Color(0.551f, 0.923f, 1f, 1f);

	[NonSerialized]
	public Color colorCategory = new Color(0.878f, 0.831f, 0.792f, 1f);

	public SelectionGroup SelectedGroup;

	public Transform partCategorizerButtonRepository;

	public static ConfigNode[] configNodes;

	public static ConfigNode[] configNodesSubassembly;

	public PopupData currentPopupData;

	public bool updateDaemonRunning;

	public bool HasCustomCategories => categories.Count > 2;

	public bool IsTransitioning { get; set; }

	public new void Awake()
	{
		Instance = this;
		GameEvents.onEditorShowPartList.Add(Setup);
		arrowLeft.onClick.AddListener(MouseInputArrowLeft);
		arrowRight.onClick.AddListener(MouseInputArrowRight);
		searchField.onValueChanged.AddListener(SearchField_OnValueChange);
		searchField.onEndEdit.AddListener(SearchField_OnEndEdit);
		searchFieldBackground = searchField.GetComponent<Image>();
		searchFieldClickHandler = searchField.gameObject.AddComponent<PointerClickHandler>();
		searchFieldClickHandler.onPointerClick.AddListener(SearchField_OnClick);
		partCategorizerButtonRepository = new GameObject("partCategorizerButtonRepository").transform;
		partCategorizerButtonRepository.transform.SetParent(base.transform);
		partCategorizerButtonRepository.gameObject.SetActive(value: false);
	}

	public IEnumerator Start()
	{
		yield return null;
		searchField.gameObject.SetActive(value: false);
		searchField.gameObject.SetActive(value: true);
	}

	public void Update()
	{
		if ((!(CraftSearch.Instance != null) || !CraftSearch.Instance.gameObject.activeInHierarchy) && Input.GetKeyUp(KeyCode.Escape))
		{
			searchField.text = string.Empty;
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		if (popup != null && popup.gameObject != null)
		{
			UnityEngine.Object.Destroy(popup.gameObject);
		}
		if (popupAddPart != null && popupAddPart.gameObject != null)
		{
			UnityEngine.Object.Destroy(popupAddPart.gameObject);
		}
	}

	public void Setup()
	{
		GameEvents.onEditorShowPartList.Remove(Setup);
		iconLoader = UnityEngine.Object.Instantiate(iconLoaderPrefab).GetComponent<IconLoader>();
		updateArrowStates();
		filterFunction = new Category(ButtonType.FILTER, EditorPartList.State.PartsList, "Filter by Function", "#autoLOC_453547", iconLoader.GetIcon("stockIcon_function"), colorFilterFunction, colorIcons, filter_LeftBar);
		subcategoryFunctionPods = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Pods", "#autoLOC_453549", iconLoader.GetIcon("stockIcon_pods"), colorFilterFunction, colorIcons, filterPods);
		filterFunction.AddSubcategory(subcategoryFunctionPods);
		subcategoryFunctionFuelTank = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Fuel Tanks", "#autoLOC_453552", iconLoader.GetIcon("stockIcon_fueltank"), colorFilterFunction, colorIcons, filterFuelTank);
		filterFunction.AddSubcategory(subcategoryFunctionFuelTank);
		subcategoryFunctionEngine = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Engines", "#autoLOC_453555", iconLoader.GetIcon("stockIcon_engine"), colorFilterFunction, colorIcons, filterEngine);
		filterFunction.AddSubcategory(subcategoryFunctionEngine);
		subcategoryFunctionControl = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Command and Control", "#autoLOC_453558", iconLoader.GetIcon("stockIcon_cmdctrl"), colorFilterFunction, colorIcons, filterControl);
		filterFunction.AddSubcategory(subcategoryFunctionControl);
		subcategoryFunctionStructural = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Structural", "#autoLOC_453561", iconLoader.GetIcon("stockIcon_structural"), colorFilterFunction, colorIcons, filterStructural);
		filterFunction.AddSubcategory(subcategoryFunctionStructural);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			subcategoryFunctionRobotics = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Robotics", "#autoLOC_8003290", iconLoader.GetIcon("serenityIcon_robotics"), colorFilterFunction, colorIcons, filterRobotics);
			filterFunction.AddSubcategory(subcategoryFunctionRobotics);
		}
		subcategoryFunctionCoupling = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Coupling", "#autoLOC_453564", iconLoader.GetIcon("stockIcon_coupling"), colorFilterFunction, colorIcons, filterCoupling);
		filterFunction.AddSubcategory(subcategoryFunctionCoupling);
		subcategoryFunctionPayload = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Payload", "#autoLOC_453567", iconLoader.GetIcon("stockIcon_payload"), colorFilterFunction, colorIcons, filterPayload);
		filterFunction.AddSubcategory(subcategoryFunctionPayload);
		subcategoryFunctionAero = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Aerodynamics", "#autoLOC_453570", iconLoader.GetIcon("stockIcon_aerodynamics"), colorFilterFunction, colorIcons, filterAero);
		filterFunction.AddSubcategory(subcategoryFunctionAero);
		subcategoryFunctionGround = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Ground", "#autoLOC_453573", iconLoader.GetIcon("stockIcon_ground"), colorFilterFunction, colorIcons, filterGround);
		filterFunction.AddSubcategory(subcategoryFunctionGround);
		subcategoryFunctionThermal = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Thermal", "#autoLOC_453576", iconLoader.GetIcon("stockIcon_thermal"), colorFilterFunction, colorIcons, filterThermal);
		filterFunction.AddSubcategory(subcategoryFunctionThermal);
		subcategoryFunctionElectrical = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Electrical", "#autoLOC_453579", iconLoader.GetIcon("stockIcon_electrical"), colorFilterFunction, colorIcons, filterElectrical);
		filterFunction.AddSubcategory(subcategoryFunctionElectrical);
		subcategoryFunctionCommunication = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Communication", "#autoLOC_453582", iconLoader.GetIcon("stockIcon_communication"), colorFilterFunction, colorIcons, filterCommunication);
		filterFunction.AddSubcategory(subcategoryFunctionCommunication);
		subcategoryFunctionScience = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Science", "#autoLOC_453585", iconLoader.GetIcon("stockIcon_science"), colorFilterFunction, colorIcons, filterScience);
		filterFunction.AddSubcategory(subcategoryFunctionScience);
		if (PartLoader.Instance.CargoPartsLoaded || alwaysShowCargoTab)
		{
			subcategoryFunctionCargo = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Cargo", "#autoLOC_8320001", iconLoader.GetIcon("stockIcon_cargo"), colorFilterFunction, colorIcons, filterCargo);
			filterFunction.AddSubcategory(subcategoryFunctionCargo);
		}
		subcategoryFunctionUtility = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Utility", "#autoLOC_453588", iconLoader.GetIcon("stockIcon_utility"), colorFilterFunction, colorIcons, filterUtility);
		filterFunction.AddSubcategory(subcategoryFunctionUtility);
		CreateCrossSectionFilters();
		CreateModuleFilters();
		CreateResourceFilters();
		CreateManufacturerFilters();
		CreateTechTierFilters();
		Icon icon = iconLoader.GetIcon("stockIcon_subassemblies");
		subassemblies = new Category(ButtonType.CATEGORY, EditorPartList.State.SubassemblyList, "Subassemblies", "#autoLOC_453605", icon, colorSubassembly, colorIcons, filterSubassemblies);
		subassemblies.button.EnableDividerOverlay();
		subassembliesAll = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.SubassemblyList, "All Subassemblies", "#autoLOC_453607", icon, colorSubassembly, colorIcons, filterEverythingSubassembly);
		subassemblies.AddSubcategory(subassembliesAll);
		Icon icon2 = iconLoader.GetIcon("stockIcon_variants");
		variants = new Category(ButtonType.CATEGORY, EditorPartList.State.VariantsList, "VariantThemes", "#autoLOC_8003021", icon2, colorVariants, colorIcons);
		variants.button.EnableDividerSpaceUnder();
		PartCategorizerButton partCategorizerButton = InstantiatePartCategorizerButtonPlus("Add custom category", "#autoLOC_6004019", iconLoader.GetIcon("internalIcon_plus"), colorCategory, Color.white);
		partCategorizerButton.OnBtnTap = OnPlusButtonClick;
		scrollListMain.AddItem(partCategorizerButton.container);
		StartCoroutine(SetInitialState());
		StartCoroutine(UpdateDaemon());
		LoadCustomPartCategories();
		LoadCustomSubassemblyCategories();
	}

	public void CreateCrossSectionFilters()
	{
		Category category = new Category(ButtonType.FILTER, EditorPartList.State.PartsList, "Filter by Cross-Section Profile", "#autoLOC_453626", iconLoader.GetIcon("cs_main"), colorFilterProfile, colorIcons, filter_LeftBar);
		List<string> list = new List<string>();
		for (int num = PartLoader.LoadedPartsList.Count - 1; num >= 0; num--)
		{
			AvailablePart availablePart = PartLoader.LoadedPartsList[num];
			if (!string.IsNullOrEmpty(availablePart.bulkheadProfiles))
			{
				string[] array = availablePart.bulkheadProfiles.Split(',');
				int num2 = array.Length;
				for (int i = 0; i < num2; i++)
				{
					list.AddUnique(array[i].Trim());
				}
			}
		}
		list.Sort();
		int j = 0;
		for (int count = list.Count; j < count; j++)
		{
			string text = list[j];
			BulkheadProfile bulkheadProfile = new BulkheadProfile();
			bulkheadProfile.color = colorFilterProfile;
			bulkheadProfile.iconColor = colorIcons;
			bulkheadProfile.iconUrl = "stockIcon_fallback";
			switch (text)
			{
			case "mk2":
				bulkheadProfile.name = "Mk 2";
				bulkheadProfile.displayName = "#autoLOC_6004005";
				bulkheadProfile.iconUrl = "cs_mk2";
				break;
			case "mk3":
				bulkheadProfile.name = "Mk 3";
				bulkheadProfile.displayName = "#autoLOC_6004006";
				bulkheadProfile.iconUrl = "cs_mk3";
				break;
			case "size4":
				bulkheadProfile.name = "Size 4 [5m]";
				bulkheadProfile.displayName = "#autoLOC_8008001";
				bulkheadProfile.iconUrl = "cs_size4";
				break;
			case "size1p5":
				bulkheadProfile.name = "Size 1.5 [1.875m]";
				bulkheadProfile.displayName = "#autoLOC_8008000";
				bulkheadProfile.iconUrl = "cs_size1p5";
				break;
			case "size1":
				bulkheadProfile.name = "Size 1 [1.25m]";
				bulkheadProfile.displayName = "#autoLOC_6004002";
				bulkheadProfile.iconUrl = "cs_size1";
				break;
			case "size0":
				bulkheadProfile.name = "Size 0 [0.625m]";
				bulkheadProfile.displayName = "#autoLOC_6004001";
				bulkheadProfile.iconUrl = "cs_size0";
				break;
			case "srf":
				bulkheadProfile.name = "Surface Attach Only";
				bulkheadProfile.displayName = "#autoLOC_6004007";
				bulkheadProfile.iconUrl = "cs_surface";
				break;
			case "size3":
				bulkheadProfile.name = "Size 3 [3.75m]";
				bulkheadProfile.displayName = "#autoLOC_6004004";
				bulkheadProfile.iconUrl = "cs_size3";
				break;
			default:
				bulkheadProfile.name = KSPUtil.PrintModuleName(text);
				break;
			case "size2":
				bulkheadProfile.name = "Size 2 [2.5m]";
				bulkheadProfile.displayName = "#autoLOC_6004003";
				bulkheadProfile.iconUrl = "cs_size2";
				break;
			}
			bulkheadProfile.profileTag = text;
			category.AddSubcategory(bulkheadProfile.GetCategory());
		}
	}

	public void CreateModuleFilters()
	{
		Icon icon = iconLoader.GetIcon("stockIcon_module");
		Category category = new Category(ButtonType.FILTER, EditorPartList.State.PartsList, "Filter by Module", "#autoLOC_453705", icon, colorFilterModule, colorIcons, filter_LeftBar);
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		int count = PartLoader.LoadedPartsList.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = PartLoader.LoadedPartsList[i];
			int count2 = availablePart.moduleInfos.Count;
			for (int j = 0; j < count2; j++)
			{
				AvailablePart.ModuleInfo moduleInfo = availablePart.moduleInfos[j];
				if (!list.Contains(moduleInfo.moduleName))
				{
					list.Add(moduleInfo.moduleName);
					list2.Add(Localizer.Format(moduleInfo.moduleDisplayName));
				}
			}
		}
		List<string> list3 = new List<string>(list2);
		list2.Sort();
		int count3 = list.Count;
		for (int k = 0; k < count3; k++)
		{
			int index = list3.IndexOf(list2[k]);
			string x;
			string text = (x = list[index]);
			Func<AvailablePart, bool> criteria = (AvailablePart p) => p.moduleInfos.Exists((AvailablePart.ModuleInfo r) => r.moduleName == x);
			int num = 0;
			for (int num2 = PartLoader.LoadedPartsList.Count - 1; num2 >= 0; num2--)
			{
				AvailablePart availablePart = PartLoader.LoadedPartsList[num2];
				int num3 = availablePart.moduleInfos.Count - 1;
				while (num3 >= 0)
				{
					if (!(availablePart.moduleInfos[num3].moduleName == text))
					{
						num3--;
						continue;
					}
					num++;
					break;
				}
				if (num > 0)
				{
					break;
				}
			}
			if (num != 0)
			{
				category.AddSubcategory(new Category(icon: text switch
				{
					"Parachute" => iconLoader.GetIcon("R&D_node_icon_survivability"), 
					"Ground Comms Part" => iconLoader.GetIcon("deployable_comms_part"), 
					"Control Surface" => iconLoader.GetIcon("R&D_node_icon_supersonicflight"), 
					"SAS" => iconLoader.GetIcon("R&D_node_icon_advflightcontrol"), 
					"PartVariants" => iconLoader.GetIcon("variant"), 
					"Ablator" => iconLoader.GetIcon("ablator"), 
					"Asteroid Drill" => iconLoader.GetIcon("asteroid_drill"), 
					"Aero Surface" => iconLoader.GetIcon("R&D_node_icon_aerodynamicsystems"), 
					"Orbital Surveyor" => iconLoader.GetIcon("R&D_node_icon_largeprobes"), 
					"RCSFX" => iconLoader.GetIcon("rcsfx"), 
					"Grapple Node" => iconLoader.GetIcon("R&D_node_icon_specializedconstruction"), 
					"Ground Exp Control" => iconLoader.GetIcon("deployed_science_control_unit"), 
					"Experience Management" => iconLoader.GetIcon("experience_management"), 
					"Robotic Controller" => iconLoader.GetIcon("robotic_controller"), 
					"Separator" => iconLoader.GetIcon("R&D_node_icon_advconstruction"), 
					"Robot Arm Scanner" => iconLoader.GetIcon("scanner_arm"), 
					"Landing Gear Fixed" => iconLoader.GetIcon("R&D_node_icon_advlanding"), 
					"Robotic Servo Hinge" => iconLoader.GetIcon("robotic_servo_hinge"), 
					"Inventory Part" => iconLoader.GetIcon("inventory_part"), 
					"Robotic Servo Rotor" => iconLoader.GetIcon("robotic_servo_rotor"), 
					"Custom-Built Fairing" => iconLoader.GetIcon("custom_built_fairing"), 
					"RCS" => iconLoader.GetIcon("R&D_node_icon_specializedcontrol"), 
					"Resource Intake" => iconLoader.GetIcon("R&D_node_icon_experimentalaerodynamics"), 
					"Resource Harvester" => iconLoader.GetIcon("fuels_ore"), 
					"Deployable Part" => iconLoader.GetIcon("deployable_part"), 
					"Ground Science Part" => iconLoader.GetIcon("deployed_science_part"), 
					"Winglet" => iconLoader.GetIcon("R&D_node_icon_stability"), 
					"Deployable Antenna" => iconLoader.GetIcon("deployable_antenna"), 
					"Strut Connector" => iconLoader.GetIcon("R&D_node_icon_automation"), 
					"Light" => iconLoader.GetIcon("R&D_node_icon_sciencetech"), 
					"Generator" => iconLoader.GetIcon("R&D_node_icon_specializedelectrics"), 
					"Lifting Surface" => iconLoader.GetIcon("R&D_node_icon_aerospacetech"), 
					"Decoupler" => iconLoader.GetIcon("R&D_node_icon_advconstruction"), 
					"Reaction Wheel" => iconLoader.GetIcon("R&D_node_icon_largecontrol"), 
					"Ground Part" => iconLoader.GetIcon("deployable_ground_part"), 
					"Wheel" => iconLoader.GetIcon("R&D_node_icon_advancedmotors"), 
					"Landing Gear" => iconLoader.GetIcon("R&D_node_icon_advlanding"), 
					"Enviro Sensor" => iconLoader.GetIcon("enviro_sensor"), 
					"Comet Drill" => iconLoader.GetIcon("comet_drill"), 
					"Ground Experiment" => iconLoader.GetIcon("deployed_science_experiment"), 
					"KerbNet Access" => iconLoader.GetIcon("kerbnet_access"), 
					"Fuel Line" => iconLoader.GetIcon("R&D_node_icon_fuelsystems"), 
					"Active Radiator" => iconLoader.GetIcon("active_radiator"), 
					"Landing Leg" => iconLoader.GetIcon("R&D_node_icon_landing"), 
					"Alternator" => iconLoader.GetIcon("R&D_node_icon_highaltitudeflight"), 
					"Command" => iconLoader.GetIcon("stockIcon_pods"), 
					"Data Transmitter" => iconLoader.GetIcon("R&D_node_icon_advunmanned"), 
					"Gimbal" => iconLoader.GetIcon("R&D_node_icon_flightcontrol"), 
					"Docking Node" => iconLoader.GetIcon("R&D_node_icon_specializedconstruction"), 
					"Cargo Part" => iconLoader.GetIcon("cargo_part"), 
					"Deployable Solar Panel" => iconLoader.GetIcon("R&D_node_icon_largeelectrics"), 
					"Resource Scanner" => iconLoader.GetIcon("R&D_node_icon_unmannedtech"), 
					"Engine" => iconLoader.GetIcon("stockIcon_engine"), 
					"Deployable Radiator" => iconLoader.GetIcon("deployable_radiator"), 
					"Robotic Servo Piston" => iconLoader.GetIcon("robotic_servo_piston"), 
					"Robotic Rotation Servo" => iconLoader.GetIcon("robotic_rotation_servo"), 
					"Core Heat" => iconLoader.GetIcon("core_heat"), 
					"Probe Control Point" => iconLoader.GetIcon("probe_control_unit"), 
					"Resource Converter" => iconLoader.GetIcon("R&D_node_icon_specializedelectrics"), 
					"Science Experiment" => iconLoader.GetIcon("R&D_node_icon_experimentalscience"), 
					"Science Lab" => iconLoader.GetIcon("R&D_node_icon_advsciencetech"), 
					_ => iconLoader.GetIcon("stockIcon_fallback"), 
				}, buttonType: ButtonType.SUBCATEGORY, displayType: EditorPartList.State.PartsList, categoryName: list2[k], categorydisplayName: list2[k], color: colorFilterModule, colorIcon: colorIcons, exclusionFilter: new EditorPartListFilter<AvailablePart>("Module_" + x, criteria)));
			}
		}
		category.AddSubcategory(new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "Animate Generic", "#autoLOC_6004017", iconLoader.GetIcon("R&D_node_icon_robotics"), colorFilterModule, colorIcons, new EditorPartListFilter<AvailablePart>("Module_ModuleAnimateGeneric", (AvailablePart p) => p.partPrefab != null && p.partPrefab.Modules != null && p.partPrefab.Modules.Contains("ModuleAnimateGeneric"))));
	}

	public void CreateResourceFilters()
	{
		Icon icon = iconLoader.GetIcon("stockIcon_resource");
		Category category = new Category(ButtonType.FILTER, EditorPartList.State.PartsList, "Filter by Resource", "#autoLOC_453877", icon, colorFilterResource, colorIcons, filter_LeftBar);
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		int count = PartLoader.LoadedPartsList.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = PartLoader.LoadedPartsList[i];
			int count2 = availablePart.resourceInfos.Count;
			for (int j = 0; j < count2; j++)
			{
				AvailablePart.ResourceInfo resourceInfo = availablePart.resourceInfos[j];
				if (!list.Contains(resourceInfo.resourceName))
				{
					list.Add(resourceInfo.resourceName);
					list2.Add(resourceInfo.displayName.LocalizeRemoveGender());
				}
			}
		}
		List<string> list3 = new List<string>(list2);
		list2.Sort();
		int count3 = list.Count;
		for (int k = 0; k < count3; k++)
		{
			int index = list3.IndexOf(list2[k]);
			string x;
			string text = (x = list[index]);
			Func<AvailablePart, bool> criteria = (AvailablePart p) => p.resourceInfos.Exists((AvailablePart.ResourceInfo r) => r.resourceName == x);
			int num = 0;
			for (int num2 = PartLoader.LoadedPartsList.Count - 1; num2 >= 0; num2--)
			{
				AvailablePart availablePart = PartLoader.LoadedPartsList[num2];
				int num3 = availablePart.resourceInfos.Count - 1;
				while (num3 >= 0)
				{
					if (!(availablePart.resourceInfos[num3].resourceName == text))
					{
						num3--;
						continue;
					}
					num++;
					break;
				}
				if (num > 0)
				{
					break;
				}
			}
			if (num != 0)
			{
				category.AddSubcategory(new Category(icon: text switch
				{
					"IntakeAir" => iconLoader.GetIcon("R&D_node_icon_experimentalaerodynamics"), 
					"Oxidizer" => iconLoader.GetIcon("fuels_oxidizer"), 
					"ElectricCharge" => iconLoader.GetIcon("R&D_node_icon_advelectrics"), 
					"SolidFuel" => iconLoader.GetIcon("fuels_solidfuel"), 
					"XenonGas" => iconLoader.GetIcon("fuels_xenongas"), 
					"LiquidFuel" => iconLoader.GetIcon("R&D_node_icon_fuelsystems"), 
					"Ore" => iconLoader.GetIcon("fuels_ore"), 
					"MonoPropellant" => iconLoader.GetIcon("fuels_monopropellant"), 
					_ => iconLoader.GetIcon("stockIcon_resource"), 
				}, buttonType: ButtonType.SUBCATEGORY, displayType: EditorPartList.State.PartsList, categoryName: list2[k], categorydisplayName: list2[k], color: colorFilterResource, colorIcon: colorIcons, exclusionFilter: new EditorPartListFilter<AvailablePart>("Resource_" + x, criteria)));
			}
		}
	}

	public void CreateManufacturerFilters()
	{
		Icon icon = iconLoader.GetIcon("stockIcon_manufacturer");
		Category category = new Category(ButtonType.FILTER, EditorPartList.State.PartsList, "Filter by Manufacturer", "#autoLOC_453968", icon, colorFilterManufacturer, colorIcons, filter_LeftBar);
		int count = AgentList.Instance.Agencies.Count;
		for (int i = 0; i < count; i++)
		{
			string mName = AgentList.Instance.Agencies[i].Title;
			Func<AvailablePart, bool> criteria = (AvailablePart p) => p.manufacturer == mName;
			int num = 0;
			int num2 = PartLoader.LoadedPartsList.Count - 1;
			while (num2 >= 0)
			{
				if (!(PartLoader.LoadedPartsList[num2].manufacturer == mName))
				{
					num2--;
					continue;
				}
				num++;
				break;
			}
			if (num != 0)
			{
				icon = new Icon("test", AgentList.Instance.Agencies[i].LogoScaled, AgentList.Instance.Agencies[i].LogoScaled, simple: true);
				Category category2 = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, AgentList.Instance.Agencies[i].Name, AgentList.Instance.Agencies[i].Title, icon, colorFilterManufacturer, colorIcons, new EditorPartListFilter<AvailablePart>("Manufacturer_" + AgentList.Instance.Agencies[i].Name, criteria));
				category2.button.ForceAspect();
				category.AddSubcategory(category2);
			}
		}
		category.AddSubcategory(new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, "C7 Aerospace Division and Rockomax Conglomerate", Localizer.Format("#autoLOC_501640"), iconLoader.GetIcon("stockIcon_manufacturer"), colorFilterManufacturer, colorIcons, new EditorPartListFilter<AvailablePart>("Manufacturer_C7 Aerospace Division and Rockomax Conglomerate", (AvailablePart p) => p.manufacturer == Localizer.Format("#autoLOC_501640"))));
	}

	public void CreateTechTierFilters()
	{
		Category category = new Category(ButtonType.FILTER, EditorPartList.State.PartsList, "Filter by Tech Level", "#autoLOC_454017", iconLoader.GetIcon("stockIcon_techlevel"), colorFilterTech, colorIcons, filter_LeftBar);
		for (int num = TechTierCategories.Length - 1; num >= 0; num--)
		{
			int num2 = PartLoader.LoadedPartsList.Count - 1;
			while (num2 >= 0)
			{
				if (!TechTierCategories[num].ExclusionCriteria(PartLoader.LoadedPartsList[num2]))
				{
					num2--;
					continue;
				}
				TechTierCategories[num].color = colorFilterTech;
				category.AddSubcategory(TechTierCategories[num].GetCategory());
				break;
			}
		}
	}

	public IEnumerator SetInitialState()
	{
		filterFunction.button.activeButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
		int i = 0;
		while (i < 50)
		{
			if (PartDropZone.Instance != null)
			{
				PartDropZone instance = PartDropZone.Instance;
				instance.onAddPart = (Callback<AvailablePart>)Delegate.Combine(instance.onAddPart, new Callback<AvailablePart>(AddPartToCategory));
				break;
			}
			yield return null;
			int num = i + 1;
			i = num;
		}
		Ready = true;
		GameEvents.onGUIEditorToolbarReady.Fire();
	}

	public void FocusSearchField()
	{
		if (!(searchField == null) && !searchField.isFocused)
		{
			InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "SearchFieldTextInput");
			searchField.ActivateInputField();
		}
	}

	public override void SearchField_OnEndEdit(string s)
	{
		InputLockManager.RemoveControlLock("SearchFieldTextInput");
		base.SearchField_OnEndEdit(s);
	}

	public override void SearchField_OnClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "SearchFieldTextInput");
			base.SearchField_OnClick(eventData);
		}
	}

	public override IEnumerator SearchRoutine()
	{
		yield return StartCoroutine(base.SearchRoutine());
		searching = true;
		refreshRequested = true;
	}

	public override void SearchStop()
	{
		base.SearchStop();
		searching = false;
		refreshRequested = true;
	}

	public override void SearchFilterResult(EditorPartListFilter<AvailablePart> filter)
	{
		base.SearchFilterResult(filter);
		if (filter != null)
		{
			editorPartList.SearchFilterParts = filter;
		}
	}

	public static void SetPanel_FunctionPods()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionPods);
	}

	public static void SetPanel_FunctionFuelTank()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionFuelTank);
	}

	public static void SetPanel_FunctionEngine()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionEngine);
	}

	public static void SetPanel_FunctionControl()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionControl);
	}

	public static void SetPanel_FunctionStructural()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionStructural);
	}

	public static void SetPanel_FunctionCoupling()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionCoupling);
	}

	public static void SetPanel_FunctionPayload()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionPayload);
	}

	public static void SetPanel_FunctionAero()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionAero);
	}

	public static void SetPanel_FunctionGround()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionGround);
	}

	public static void SetPanel_FunctionThermal()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionThermal);
	}

	public static void SetPanel_FunctionElectrical()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionElectrical);
	}

	public static void SetPanel_FunctionCommunication()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionCommunication);
	}

	public static void SetPanel_FunctionScience()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionScience);
	}

	public static void SetPanel_FunctionCargo()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionCargo);
	}

	public static void SetPanel_FunctionRobotics()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Instance.SetSimpleMode();
			Instance.ForceShowPanel(Instance.subcategoryFunctionRobotics);
		}
	}

	public static void SetPanel_FunctionUtility()
	{
		Instance.SetSimpleMode();
		Instance.ForceShowPanel(Instance.subcategoryFunctionUtility);
	}

	public void ForceShowPanel(Category cat, bool ignoreActive = false)
	{
		if (ignoreActive)
		{
			cat.button.activeButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
		}
		else if (!cat.button.activeButton.Value)
		{
			cat.button.activeButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
		}
	}

	public static PartCategorizerButton InstantiatePartCategorizerButton(string categoryName, string categorydisplayName, Icon icon, Color colorButton, Color colorIcon, bool last = false)
	{
		PartCategorizerButton partCategorizerButton = UnityEngine.Object.Instantiate(Instance.buttonPrefab);
		partCategorizerButton.InitializeToggleBtn(categoryName, categorydisplayName, icon, colorButton, colorIcon, last);
		return partCategorizerButton;
	}

	public PartCategorizerButton InstantiatePartCategorizerButtonPlus(string categoryName, string categorydisplayName, Icon icon, Color colorButton, Color colorIcon)
	{
		PartCategorizerButton partCategorizerButton = UnityEngine.Object.Instantiate(Instance.buttonPrefab);
		partCategorizerButton.InitializeBtn(categoryName, categorydisplayName, icon, colorButton, colorIcon);
		return partCategorizerButton;
	}

	public static Category AddCustomFilter(string filterName, string filterdisplayName, Icon icon, Color colorButton)
	{
		PartCategorizerButton partCategorizerButton = InstantiatePartCategorizerButton(filterName, filterdisplayName, icon, colorButton, Instance.colorIcons);
		Category result = new Category(ButtonType.FILTER, EditorPartList.State.CustomPartList, partCategorizerButton, Instance.filter_LeftBar, addItem: false);
		Instance.scrollListMain.InsertItem(partCategorizerButton.container, Instance.filters.Count - 1);
		return result;
	}

	public static Category AddCustomSubcategoryFilter(Category mainFilter, string subFilterName, string subFilterdisplayName, Icon icon, Func<AvailablePart, bool> exclusionFilter)
	{
		Category category = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.PartsList, subFilterName, subFilterdisplayName, icon, mainFilter.button.color, Instance.colorIcons, new EditorPartListFilter<AvailablePart>(mainFilter.button.categoryName + "_" + subFilterName, exclusionFilter));
		mainFilter.AddSubcategory(category);
		if (mainFilter.button.activeButton.Value)
		{
			Instance.subcategoryButtonRebuildRequested = true;
		}
		return category;
	}

	public void UpdateCategoryNameLabel()
	{
		int num = 0;
		Category category = null;
		for (int num2 = filters.Count - 1; num2 >= 0; num2--)
		{
			if (filters[num2].button.activeButton.Value)
			{
				num++;
				category = filters[num2];
			}
		}
		int num3 = 0;
		Category category2 = null;
		for (int num4 = categories.Count - 1; num4 >= 0; num4--)
		{
			if (categories[num4].button.activeButton.Value)
			{
				num3++;
				category2 = categories[num4];
			}
		}
		bool flag = SelectedGroup == SelectionGroup.FILTER;
		bool flag2 = SelectedGroup == SelectionGroup.CATEGORY;
		if (num + num3 != 1)
		{
			if (flag2 && !flag)
			{
				labelCategoryName.text = num + num3 + " categories selected";
			}
			else
			{
				labelCategoryName.text = num + num3 + " filters selected";
			}
		}
		else if (num == 1)
		{
			Category category3 = null;
			int i = 0;
			for (int count = category.subcategories.Count; i < count; i++)
			{
				if (category.subcategories[i].button.activeButton.Value)
				{
					category3 = category.subcategories[i];
					break;
				}
			}
			if (category3 == null)
			{
				if (displayType != EditorPartList.State.VariantsList)
				{
					Debug.LogError("Category has no subcategory. This is not supported.");
				}
			}
			else
			{
				labelCategoryName.text = category3.button.displayCategoryName;
			}
		}
		else
		{
			if (num3 != 1)
			{
				return;
			}
			Category category4 = null;
			int j = 0;
			for (int count2 = category2.subcategories.Count; j < count2; j++)
			{
				if (category2.subcategories[j].button.activeButton.Value)
				{
					category4 = category2.subcategories[j];
					break;
				}
			}
			if (category4 == null)
			{
				if (displayType != EditorPartList.State.VariantsList)
				{
					Debug.LogError("Category has no subcategory. This is not supported.");
				}
			}
			else
			{
				labelCategoryName.text = category4.button.displayCategoryName;
			}
		}
	}

	public void updateArrowStates()
	{
		if (EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE)
		{
			arrowLeft.gameObject.SetActive(value: false);
			arrowRight.gameObject.SetActive(value: true);
		}
		else
		{
			arrowLeft.gameObject.SetActive(value: true);
			arrowRight.gameObject.SetActive(value: false);
		}
	}

	public void MouseInputArrowLeft()
	{
		SetSimpleMode();
	}

	public void MouseInputArrowRight()
	{
		SetAdvancedMode();
	}

	public void SetSimpleMode()
	{
		if (EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE || IsTransitioning)
		{
			return;
		}
		IsTransitioning = true;
		EditorLogic.Mode = EditorLogic.EditorModes.SIMPLE;
		EditorPanels.Instance.updatePartsListMode(delegate
		{
			StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				IsTransitioning = false;
			}));
		});
		updateArrowStates();
		ForceShowPanel(filterFunction, ignoreActive: true);
	}

	public void SetAdvancedMode()
	{
		if (EditorLogic.Mode == EditorLogic.EditorModes.ADVANCED || IsTransitioning)
		{
			return;
		}
		IsTransitioning = true;
		EditorLogic.Mode = EditorLogic.EditorModes.ADVANCED;
		EditorPanels.Instance.updatePartsListMode(delegate
		{
			StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				IsTransitioning = false;
			}));
		});
		updateArrowStates();
	}

	public void LoadCustomPartCategories()
	{
		if (configNodes == null)
		{
			configNodes = GameDatabase.Instance.GetConfigNodes("CUSTOM_PARTLIST_CATEGORY");
		}
		int i = 0;
		for (int num = configNodes.Length; i < num; i++)
		{
			ConfigNode configNode = configNodes[i];
			Category category = AddCustomCategory(configNode.GetValue("categoryName"), configNode.GetValue("categoryName"), iconLoader.GetIcon(configNode.GetValue("icon")), colorCategory, colorIcons, addStdSubcategory: false);
			for (int j = 0; j < configNode.GetNodes("SUBCATEGORY").Length; j++)
			{
				ConfigNode configNode2 = configNode.GetNodes("SUBCATEGORY")[j];
				Category category2 = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.CustomPartList, configNode2.GetValue("categoryName"), configNode2.GetValue("categoryName"), iconLoader.GetIcon(configNode2.GetValue("icon")), colorCategory, colorIcons, filterCustomCategory);
				category.AddSubcategory(category2);
				if (!configNode2.HasNode("PARTS"))
				{
					continue;
				}
				ConfigNode node = configNode2.GetNode("PARTS");
				if (node.HasValue("part"))
				{
					string[] values = node.GetValues("part");
					int k = 0;
					for (int num2 = values.Length; k < num2; k++)
					{
						category2.AddPart(values[k], compile: false);
					}
					if (values.Length != 0)
					{
						category2.CompileExclusionFilter(sendRefresh: false);
					}
				}
			}
		}
	}

	public void LoadCustomSubassemblyCategories()
	{
		if (configNodesSubassembly == null)
		{
			configNodesSubassembly = GameDatabase.Instance.GetConfigNodes("CUSTOM_SUBASSEMBLY_SUBCATEGORY");
		}
		Category category = categories[0];
		int i = 0;
		for (int num = configNodesSubassembly.Length; i < num; i++)
		{
			ConfigNode configNode = configNodesSubassembly[i];
			for (int j = 0; j < configNode.GetNodes("SUBCATEGORY").Length; j++)
			{
				ConfigNode configNode2 = configNode.GetNodes("SUBCATEGORY")[j];
				Category category2 = new Category(ButtonType.SUBCATEGORY, EditorPartList.State.SubassemblyList, configNode2.GetValue("categoryName"), configNode2.GetValue("categoryName"), iconLoader.GetIcon(configNode2.GetValue("icon")), colorSubassembly, colorIcons, filterNothingSubassembly);
				category.AddSubcategory(category2);
				if (!configNode2.HasNode("SUBASSEMBLIES"))
				{
					continue;
				}
				ConfigNode node = configNode2.GetNode("SUBASSEMBLIES");
				if (node.HasValue("subassembly"))
				{
					string[] values = node.GetValues("subassembly");
					int k = 0;
					for (int num2 = values.Length; k < num2; k++)
					{
						category2.AddSubassembly(values[k], compile: false, exclude: false, refresh: false);
					}
					if (values.Length != 0)
					{
						category2.CompileExclusionFilterSubassembly(exclude: false, sendRefresh: false);
					}
				}
			}
		}
	}

	public void SaveCustomPartCategories()
	{
		ConfigNode configNode = new ConfigNode();
		int count = categories.Count;
		for (int i = 2; i < count; i++)
		{
			Category category = categories[i];
			ConfigNode configNode2 = new ConfigNode("CUSTOM_PARTLIST_CATEGORY");
			configNode2.AddValue("categoryName", category.button.categoryName);
			configNode2.AddValue("icon", category.button.icon.name);
			int count2 = category.subcategories.Count;
			for (int j = 0; j < count2; j++)
			{
				Category category2 = categories[i].subcategories[j];
				ConfigNode configNode3 = new ConfigNode("SUBCATEGORY");
				configNode3.AddValue("categoryName", category2.button.categoryName);
				configNode3.AddValue("icon", category2.button.icon.name);
				ConfigNode configNode4 = new ConfigNode("PARTS");
				int count3 = category2.availableParts.Count;
				for (int k = 0; k < count3; k++)
				{
					configNode4.AddValue("part", category2.availableParts[k]);
				}
				if (count3 > 0)
				{
					configNode3.AddNode(configNode4);
				}
				configNode2.AddNode(configNode3);
			}
			configNode.AddNode(configNode2);
		}
		string text = KSPUtil.ApplicationRootPath + "GameData/Squad/PartList/PartCategories.cfg";
		if (configNode.HasNode("CUSTOM_PARTLIST_CATEGORY"))
		{
			configNode.Save(text);
			configNodes = configNode.GetNodes("CUSTOM_PARTLIST_CATEGORY");
		}
		else
		{
			File.Delete(text);
			configNodes = new List<ConfigNode>().ToArray();
		}
	}

	public void SaveCustomSubassemblyCategories()
	{
		ConfigNode configNode = new ConfigNode();
		Category category = categories[0];
		ConfigNode configNode2 = new ConfigNode("CUSTOM_SUBASSEMBLY_SUBCATEGORY");
		int count = category.subcategories.Count;
		for (int i = 1; i < count; i++)
		{
			Category category2 = category.subcategories[i];
			ConfigNode configNode3 = new ConfigNode("SUBCATEGORY");
			configNode3.AddValue("categoryName", category2.button.categoryName);
			configNode3.AddValue("icon", category2.button.icon.name);
			ConfigNode configNode4 = new ConfigNode("SUBASSEMBLIES");
			int count2 = category2.shipTemplates.Count;
			for (int j = 0; j < count2; j++)
			{
				configNode4.AddValue("subassembly", category2.shipTemplates[j]);
			}
			if (count2 > 0)
			{
				configNode3.AddNode(configNode4);
			}
			configNode2.AddNode(configNode3);
		}
		if (count > 1)
		{
			configNode.AddNode(configNode2);
		}
		string text = KSPUtil.ApplicationRootPath + "GameData/Squad/PartList/SubassemblyCategories.cfg";
		if (configNode.HasNode("CUSTOM_SUBASSEMBLY_SUBCATEGORY"))
		{
			configNode.Save(text);
			configNodesSubassembly = configNode.GetNodes("CUSTOM_SUBASSEMBLY_SUBCATEGORY");
		}
		else
		{
			File.Delete(text);
			configNodesSubassembly = new List<ConfigNode>().ToArray();
		}
	}

	public void AddPartToCategory(AvailablePart part)
	{
		SelectedCategory.AddPart(part);
		SaveCustomPartCategories();
	}

	public void RemovePartFromCategory(AvailablePart part)
	{
		SelectedCategory.RemovePart(part);
		SaveCustomPartCategories();
	}

	public void AddSubassemblyToSelectedCategory(string st)
	{
		if (SelectedSubassemblyIsCustom())
		{
			SelectedCategory.AddSubassembly(st, compile: true, exclude: true, refresh: true);
			SaveCustomSubassemblyCategories();
		}
	}

	public void RemoveSubassemblyFromSelectedCategory(string st, bool refresh)
	{
		if (SelectedSubassemblyIsCustom())
		{
			SelectedCategory.RemoveSubassembly(st, compile: true, exclude: true, refresh);
			SaveCustomSubassemblyCategories();
			return;
		}
		int count = subassemblies.subcategories.Count;
		bool flag = false;
		for (int i = 1; i < count; i++)
		{
			if (subassemblies.subcategories[i].shipTemplates.Contains(st))
			{
				subassemblies.subcategories[i].RemoveSubassembly(st, compile: false, exclude: false, refresh);
				flag = true;
			}
		}
		if (flag)
		{
			SaveCustomSubassemblyCategories();
		}
	}

	public bool SelectedSubassemblyIsCustom()
	{
		return SelectedCategory != subassembliesAll;
	}

	public void AddPartToCustomCategoryViaPopup(AvailablePart part)
	{
		ShowPopupAddPart(part.name, part.title, OnAcceptPopupAddPart, AcceptCriteriaPopupAddPart);
	}

	public void OnPlusButtonClick()
	{
		PopupData popupData = new PopupData("#autoLOC_6004020", "Unknown", Localizer.Format("#autoLOC_6004021"), iconLoader.GetIcon("stockIcon_fallback"));
		ShowPopup(popupData, AcceptNewCategory, AcceptCriteria, null, null);
	}

	public void AcceptNewCategory()
	{
		AddCustomCategory(currentPopupData.categoryName, currentPopupData.categorydisplayName, currentPopupData.icon, colorCategory, colorIcons, addStdSubcategory: true);
		SaveCustomPartCategories();
		refreshRequested = true;
	}

	public bool AcceptCriteria(string categoryName, out string reason)
	{
		if (categories.Exists((Category a) => a.button.categoryName.Equals(categoryName)))
		{
			reason = "#autoLOC_6004022";
			return false;
		}
		reason = "Everything ok";
		return true;
	}

	public Category AddCustomCategory(string categoryName, string categorydisplayName, Icon icon, Color color, Color colorIcon, bool addStdSubcategory)
	{
		PartCategorizerButton partCategorizerButton = InstantiatePartCategorizerButton(categoryName, categorydisplayName, icon, color, colorIcon);
		Category category = new Category(ButtonType.CATEGORY, EditorPartList.State.CustomPartList, partCategorizerButton, filter_LeftBar, addItem: false);
		if (addStdSubcategory)
		{
			category.AddSubcategory(new Category(ButtonType.SUBCATEGORY, EditorPartList.State.CustomPartList, "Unknown", Localizer.Format("#autoLOC_6004021"), iconLoader.GetIcon("stockIcon_fallback"), colorCategory, colorIcons, filterCustomCategory));
		}
		scrollListMain.InsertItem(partCategorizerButton.container, filters.Count + categories.Count - 1);
		return category;
	}

	public void ShowPopup(PopupData popupData, Callback onAccept, PartCategorizerPopup.CriteriaAccept onAcceptCriteria, Callback onDelete, PartCategorizerPopup.CriteriaDelete onDeleteCriteria)
	{
		currentPopupData = popupData;
		bool yield = false;
		if (popup == null)
		{
			InstantiatePopup();
			yield = true;
		}
		StartCoroutine(ShowPopup(popupData, onAccept, onAcceptCriteria, onDelete, onDeleteCriteria, yield));
	}

	public void InstantiatePopup()
	{
		popup = UnityEngine.Object.Instantiate(popupPrefab);
		popup.transform.SetParent(DialogCanvasUtil.DialogCanvas.transform, worldPositionStays: false);
	}

	public IEnumerator ShowPopup(PopupData popupData, Callback onAccept, PartCategorizerPopup.CriteriaAccept onAcceptCriteria, Callback onDelete, PartCategorizerPopup.CriteriaDelete onDeleteCriteria, bool yield)
	{
		if (yield)
		{
			yield return null;
			yield return null;
		}
		popup.Show(popupData, onAccept, onAcceptCriteria, onDelete, onDeleteCriteria);
	}

	public void ShowPopupAddPart(string partName, string partTitle, Callback<string, Category> onAccept, PartCategorizerPopupAddPart.CriteriaAccept onAcceptCriteria)
	{
		bool yield = false;
		if (popupAddPart == null)
		{
			InstantiatePopupAddPart();
			yield = true;
		}
		StartCoroutine(ShowPopupAddPart(partName, partTitle, onAccept, onAcceptCriteria, yield));
	}

	public void InstantiatePopupAddPart()
	{
		popupAddPart = UnityEngine.Object.Instantiate(popupAddPartPrefab);
		popupAddPart.transform.SetParent(DialogCanvasUtil.DialogCanvas.transform, worldPositionStays: false);
		popupAddPart.Show("", "", null, null, forceListRebuild: false);
	}

	public IEnumerator ShowPopupAddPart(string partName, string partTitle, Callback<string, Category> onAccept, PartCategorizerPopupAddPart.CriteriaAccept onAcceptCriteria, bool yield)
	{
		if (yield)
		{
			yield return null;
			yield return null;
		}
		popupAddPart.Show(partName, partTitle, onAccept, onAcceptCriteria, forceListRebuild: true);
	}

	public void OnAcceptPopupAddPart(string partName, Category category)
	{
		category.AddPart(partName, compile: false);
		SaveCustomPartCategories();
	}

	public bool AcceptCriteriaPopupAddPart(string partName, Category category, out string reason)
	{
		if (category == null)
		{
			reason = Localizer.Format("#autoLOC_6002481");
			return false;
		}
		if (category.availableParts.Exists((string a) => a.Equals(partName)))
		{
			reason = Localizer.Format("#autoLOC_6002482");
			return false;
		}
		reason = Localizer.Format("#autoLOC_6002483");
		return true;
	}

	public IEnumerator UpdateDaemon()
	{
		yield return null;
		yield return null;
		if (updateDaemonRunning)
		{
			yield break;
		}
		updateDaemonRunning = true;
		while ((bool)this)
		{
			if (!IsTransitioning)
			{
				if (subcategoryButtonRebuildRequested)
				{
					subcategoryButtonRebuildRequested = false;
					int i = 0;
					for (int count = filters.Count; i < count; i++)
					{
						if (filters[i].button.activeButton.Value)
						{
							filters[i].RebuildSubcategoryButtons();
						}
					}
				}
				if (refreshRequested)
				{
					editorPartList.Refresh(searching ? EditorPartList.State.PartSearch : displayType);
					refreshRequested = false;
					UpdateCategoryNameLabel();
				}
			}
			yield return null;
		}
	}

	public void PrintManufacturersUsed()
	{
		List<string> list = new List<string>();
		int count = PartLoader.LoadedPartsList.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = PartLoader.LoadedPartsList[i];
			if (!list.Contains(availablePart.manufacturer))
			{
				list.Add(availablePart.manufacturer);
			}
		}
		string text = "PARTS\n";
		int count2 = list.Count;
		for (int j = 0; j < count2; j++)
		{
			text = text + list[j] + "\n";
		}
		MonoBehaviour.print(text);
	}
}
