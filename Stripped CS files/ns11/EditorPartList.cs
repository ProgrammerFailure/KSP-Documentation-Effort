using System.Collections;
using System.Collections.Generic;
using System.IO;
using ns13;
using ns2;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class EditorPartList : MonoBehaviour
{
	public enum State
	{
		PartsList,
		SubassemblyList,
		CustomPartList,
		PartSearch,
		Nothing,
		VariantsList
	}

	public EditorPartIcon partPrefab;

	public RectTransform partGrid;

	public EditorSubassemblyItem subassemblyPrefab;

	public RectTransform subassemblyGrid;

	public EditorVariantItem variantPrefab;

	public RectTransform variantGrid;

	public UIListSorter partListSorter;

	public UIScrollRectState scrollRectState;

	public ScrollRect partListScrollRect;

	public float iconOverSpin;

	public float iconOverScale;

	public float iconSize;

	public Button customPartButtonTransform;

	public Button subassemblyButtonTransform;

	public EditorPartListFilter<AvailablePart> AmountAvailableFilter = new EditorPartListFilter<AvailablePart>("filterAmountAvailable", (AvailablePart p) => p.amountAvailable > 0);

	public EditorPartListFilter<AvailablePart> SearchFilterParts;

	public EditorPartListFilterList<AvailablePart> GreyoutFilters;

	public EditorPartListFilterList<AvailablePart> ExcludeFilters;

	public EditorPartListFilterList<AvailablePart> CategorizerFilters;

	public EditorPartListFilterList<ShipTemplate> ExcludeFiltersSubassembly;

	public bool allowTabChange;

	public IComparer<AvailablePart> currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc: true, r1.title.CompareTo(r2.title), (r1.cost + r1.partPrefab.GetModuleCosts(r1.cost)).CompareTo(r2.cost + r2.partPrefab.GetModuleCosts(r2.cost))));

	public IComparer<ShipTemplate> currentSubassemblySorting = new RUIutils.FuncComparer<ShipTemplate>((ShipTemplate r1, ShipTemplate r2) => RUIutils.SortAscDescPrimarySecondary(asc: true, r1.shipName.CompareTo(r2.shipName), r1.totalCost.CompareTo(r2.totalCost)));

	public List<AvailablePart> categoryList = new List<AvailablePart>();

	public List<EditorPartIcon> icons;

	public State state;

	public GameObject partIconStorage;

	public Dictionary<string, float> scrollbarPositions = new Dictionary<string, float>();

	public string lastFilterID = "";

	public Dictionary<string, EditorPartIcon> iconCache = new Dictionary<string, EditorPartIcon>();

	public List<EditorSubassemblyItem> saItems;

	public List<ShipTemplate> saList = new List<ShipTemplate>();

	public List<EditorVariantItem> vItems;

	public static EditorPartList Instance { get; set; }

	public void Awake()
	{
		Instance = this;
		allowTabChange = false;
		icons = new List<EditorPartIcon>();
		saItems = new List<EditorSubassemblyItem>();
		vItems = new List<EditorVariantItem>();
		GreyoutFilters = new EditorPartListFilterList<AvailablePart>();
		ExcludeFilters = new EditorPartListFilterList<AvailablePart>();
		CategorizerFilters = new EditorPartListFilterList<AvailablePart>();
		ExcludeFiltersSubassembly = new EditorPartListFilterList<ShipTemplate>();
		partListSorter.AddOnSortCallback(SortingCallback);
		partIconStorage = new GameObject("partIconStorage");
		partIconStorage.transform.SetParent(base.transform);
		partIconStorage.SetActive(value: false);
		partListScrollRect.onValueChanged.AddListener(OnPartListUpdate);
	}

	public void Start()
	{
		GameEvents.onEditorRestart.Add(OnEditorRestart);
		OnPartListUpdate(Vector2.zero);
	}

	public void OnDestroy()
	{
		if (partIconStorage != null)
		{
			Object.Destroy(partIconStorage);
		}
		GameEvents.onEditorRestart.Remove(OnEditorRestart);
		partListScrollRect.onValueChanged.RemoveListener(OnPartListUpdate);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnPartListUpdate(Vector2 vector)
	{
		PartListTooltipController.SetupScreenSpaceMask((RectTransform)scrollRectState.transform);
		int count = icons.Count;
		while (count-- > 0)
		{
			PartListTooltipController.SetScreenSpaceMaskMaterials(icons[count].materials);
		}
	}

	public void OnEditorRestart()
	{
	}

	public void SortingCallback(int button, bool asc)
	{
		switch (button)
		{
		case 0:
			currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.title.CompareTo(r2.title), (r1.cost + r1.partPrefab.GetModuleCosts(r1.cost)).CompareTo(r2.cost + r2.partPrefab.GetModuleCosts(r2.cost))));
			currentSubassemblySorting = new RUIutils.FuncComparer<ShipTemplate>((ShipTemplate r1, ShipTemplate r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.shipName.CompareTo(r2.shipName), r1.totalCost.CompareTo(r2.totalCost)));
			break;
		case 1:
			currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.partPrefab.mass.CompareTo(r2.partPrefab.mass), r1.title.CompareTo(r2.title)));
			currentSubassemblySorting = new RUIutils.FuncComparer<ShipTemplate>((ShipTemplate r1, ShipTemplate r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.totalMass.CompareTo(r2.totalMass), r1.shipName.CompareTo(r2.shipName)));
			break;
		case 2:
			currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc, (r1.cost + r1.partPrefab.GetModuleCosts(r1.cost)).CompareTo(r2.cost + r2.partPrefab.GetModuleCosts(r2.cost)), r1.title.CompareTo(r2.title)));
			currentSubassemblySorting = new RUIutils.FuncComparer<ShipTemplate>((ShipTemplate r1, ShipTemplate r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.totalCost.CompareTo(r2.totalCost), r1.shipName.CompareTo(r2.shipName)));
			break;
		case 3:
			currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.partSize.CompareTo(r2.partSize), r1.title.CompareTo(r2.title)));
			currentSubassemblySorting = new RUIutils.FuncComparer<ShipTemplate>((ShipTemplate r1, ShipTemplate r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.shipSize.magnitude.CompareTo(r2.shipSize.magnitude), r1.partCount.CompareTo(r2.partCount), r1.shipName.CompareTo(r2.shipName)));
			break;
		}
		Refresh();
	}

	public void RememberScrollbarPosition(State currentState)
	{
		UpdateScrollbarPositionDictionary(lastFilterID);
		switch (currentState)
		{
		case State.PartsList:
			StartCoroutine(ScrollBarFix(CategorizerFilters.GetFilterKeySingleOrNothing()));
			lastFilterID = CategorizerFilters.GetFilterKeySingleOrNothing();
			break;
		case State.SubassemblyList:
			StartCoroutine(ScrollBarFix(ExcludeFiltersSubassembly.GetFilterKeySingleOrNothing()));
			lastFilterID = ExcludeFiltersSubassembly.GetFilterKeySingleOrNothing();
			break;
		case State.CustomPartList:
			StartCoroutine(ScrollBarFix(CategorizerFilters.GetFilterKeySingleOrNothing()));
			lastFilterID = CategorizerFilters.GetFilterKeySingleOrNothing();
			break;
		case State.PartSearch:
			StartCoroutine(AutoScrollBarFix_routine());
			lastFilterID = "";
			break;
		case State.Nothing:
			StartCoroutine(AutoScrollBarFix_routine());
			lastFilterID = "";
			break;
		case State.VariantsList:
			StartCoroutine(AutoScrollBarFix_routine());
			lastFilterID = "";
			break;
		}
	}

	public void UpdateScrollbarPositionDictionary(string key)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (scrollbarPositions.ContainsKey(key))
			{
				scrollbarPositions[key] = partListScrollRect.verticalNormalizedPosition;
			}
			else
			{
				scrollbarPositions.Add(key, partListScrollRect.verticalNormalizedPosition);
			}
		}
	}

	public IEnumerator AutoScrollBarFix_routine()
	{
		yield return null;
		yield return null;
		AutoScrollBarFix();
	}

	public void AutoScrollBarFix()
	{
		partListScrollRect.verticalNormalizedPosition = Mathf.Clamp01(partListScrollRect.verticalNormalizedPosition);
		partListScrollRect.Rebuild(CanvasUpdate.PostLayout);
	}

	public IEnumerator ScrollBarFix(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			AutoScrollBarFix();
			yield break;
		}
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		if (scrollbarPositions.ContainsKey(key))
		{
			partListScrollRect.verticalNormalizedPosition = scrollbarPositions[key];
			partListScrollRect.Rebuild(CanvasUpdate.PostLayout);
		}
		else
		{
			partListScrollRect.verticalNormalizedPosition = 1f;
			partListScrollRect.Rebuild(CanvasUpdate.PostLayout);
		}
	}

	public void Refresh(State state)
	{
		RememberScrollbarPosition(state);
		this.state = state;
		Refresh();
	}

	public void Refresh()
	{
		switch (state)
		{
		case State.PartsList:
			RefreshPartList();
			subassemblyButtonTransform.gameObject.SetActive(value: false);
			customPartButtonTransform.gameObject.SetActive(value: false);
			break;
		case State.SubassemblyList:
			RefreshSubassemblies();
			subassemblyButtonTransform.gameObject.SetActive(value: true);
			customPartButtonTransform.gameObject.SetActive(value: false);
			break;
		case State.CustomPartList:
			RefreshCustomPartList();
			subassemblyButtonTransform.gameObject.SetActive(value: false);
			customPartButtonTransform.gameObject.SetActive(value: true);
			break;
		case State.PartSearch:
			RefreshSearchList();
			subassemblyButtonTransform.gameObject.SetActive(value: false);
			customPartButtonTransform.gameObject.SetActive(value: false);
			break;
		case State.Nothing:
			ClearAllItems();
			subassemblyButtonTransform.gameObject.SetActive(value: false);
			customPartButtonTransform.gameObject.SetActive(value: false);
			break;
		case State.VariantsList:
			RefreshVariants();
			subassemblyButtonTransform.gameObject.SetActive(value: false);
			customPartButtonTransform.gameObject.SetActive(value: false);
			break;
		}
		OnPartListUpdate(Vector2.zero);
	}

	public void ClearAllItems()
	{
		int i = 0;
		for (int count = icons.Count; i < count; i++)
		{
			EditorPartIcon editorPartIcon = icons[i];
			editorPartIcon.UnsetGrey();
			editorPartIcon.gameObject.SetActive(value: false);
			editorPartIcon.transform.SetParent(partIconStorage.transform, worldPositionStays: false);
		}
		int j = 0;
		for (int count2 = saItems.Count; j < count2; j++)
		{
			Object.Destroy(saItems[j].gameObject);
		}
		int k = 0;
		for (int count3 = vItems.Count; k < count3; k++)
		{
			Object.Destroy(vItems[k].gameObject);
		}
		icons.Clear();
		saItems.Clear();
		vItems.Clear();
	}

	public void RefreshSearchList()
	{
		categoryList = ExcludeFilters.GetFilteredList(PartLoader.LoadedPartsList);
		EditorPartListFilter<AvailablePart>.FilterList(categoryList, AmountAvailableFilter.FilterCriteria);
		EditorPartListFilter<AvailablePart>.FilterList(categoryList, SearchFilterParts.FilterCriteria);
		categoryList.Sort(currentPartSorting);
		UpdatePartIcons();
	}

	public void RefreshPartList()
	{
		categoryList = ExcludeFilters.GetFilteredList(PartLoader.LoadedPartsList);
		EditorPartListFilter<AvailablePart>.FilterList(categoryList, AmountAvailableFilter.FilterCriteria);
		CategorizerFilters.FilterList(categoryList);
		categoryList.Sort(currentPartSorting);
		UpdatePartIcons();
	}

	public void RefreshCustomPartList()
	{
		categoryList = ExcludeFilters.GetFilteredList(PartLoader.LoadedPartsList);
		EditorPartListFilter<AvailablePart>.FilterList(categoryList, AmountAvailableFilter.FilterCriteria);
		CategorizerFilters.FilterList(categoryList);
		categoryList.Sort(currentPartSorting);
		UpdatePartIcons(customCategory: true);
	}

	public void UpdatePartIcons(bool customCategory = false)
	{
		ClearAllItems();
		scrollRectState.SetState("parts");
		int count = categoryList.Count;
		for (int i = 0; i < count; i++)
		{
			if (!iconCache.ContainsKey(categoryList[i].name))
			{
				EditorPartIcon editorPartIcon = Object.Instantiate(partPrefab);
				editorPartIcon.Create(this, categoryList[i], iconSize, iconOverScale, iconOverSpin);
				UpdatePartIcon(editorPartIcon, categoryList[i], customCategory);
				icons.Add(editorPartIcon);
				iconCache.Add(editorPartIcon.partInfo.name, editorPartIcon);
			}
			else
			{
				EditorPartIcon editorPartIcon2 = iconCache[categoryList[i].name];
				UpdatePartIcon(editorPartIcon2, categoryList[i], customCategory);
				icons.Add(editorPartIcon2);
			}
		}
	}

	public void UpdatePartIcon(EditorPartIcon newIcon, AvailablePart availablePart, bool customCategory = false)
	{
		if (customCategory)
		{
			newIcon.EnableDeleteButton();
			newIcon.DisableAddButton();
		}
		else
		{
			if (PartCategorizer.Instance.HasCustomCategories)
			{
				newIcon.EnableAddButton();
			}
			else
			{
				newIcon.DisableAddButton();
			}
			newIcon.DisableDeleteButton();
		}
		bool flag = false;
		string grey = "";
		int i = 0;
		for (int count = GreyoutFilters.Count; i < count; i++)
		{
			EditorPartListFilter<AvailablePart> editorPartListFilter = GreyoutFilters[i];
			if (!editorPartListFilter.FilterCriteria(availablePart))
			{
				flag = true;
				grey = editorPartListFilter.criteriaFailMessage;
				break;
			}
		}
		if (flag)
		{
			newIcon.SetGrey(grey);
		}
		else
		{
			newIcon.UnsetGrey();
		}
		newIcon.transform.SetParent(partGrid.transform, worldPositionStays: false);
		newIcon.gameObject.SetActive(value: true);
	}

	[ContextMenu("Refresh Subs")]
	public void RefreshSubassemblies()
	{
		saList = new List<ShipTemplate>();
		string path = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Subassemblies/";
		Directory.CreateDirectory(path);
		FileInfo[] files = new DirectoryInfo(path).GetFiles("*.craft");
		int num = files.Length;
		for (int i = 0; i < num; i++)
		{
			ShipTemplate shipTemplate = ShipConstruction.LoadTemplate(files[i].FullName);
			if (shipTemplate != null && shipTemplate.partCount != 0 && shipTemplate.shipPartsUnlocked)
			{
				saList.Add(shipTemplate);
			}
		}
		ExcludeFiltersSubassembly.FilterList(saList);
		saList.Sort(currentSubassemblySorting);
		if (state == State.SubassemblyList)
		{
			RefreshSubassemblyList();
		}
	}

	public void RefreshSubassemblyList()
	{
		ClearAllItems();
		scrollRectState.SetState("subassemblies");
		int count = saList.Count;
		for (int i = 0; i < count; i++)
		{
			EditorSubassemblyItem editorSubassemblyItem = Object.Instantiate(subassemblyPrefab);
			editorSubassemblyItem.transform.SetParent(subassemblyGrid.transform, worldPositionStays: false);
			editorSubassemblyItem.Create(this, saList[i]);
			editorSubassemblyItem.gameObject.SetActive(value: true);
			editorSubassemblyItem.transform.localPosition = new Vector3(editorSubassemblyItem.transform.localPosition.x, editorSubassemblyItem.transform.localPosition.y, -0.5f);
			saItems.Add(editorSubassemblyItem);
		}
	}

	[ContextMenu("Refresh Variants")]
	public void RefreshVariants()
	{
		if (state == State.VariantsList)
		{
			RefreshVariantsList();
		}
	}

	public void RefreshVariantsList()
	{
		ClearAllItems();
		scrollRectState.SetState("variants");
		int count = PartLoader.LoadedVariantThemesList.Count;
		for (int i = 0; i < count; i++)
		{
			EditorVariantItem editorVariantItem = Object.Instantiate(variantPrefab);
			editorVariantItem.transform.SetParent(variantGrid.transform, worldPositionStays: false);
			editorVariantItem.Create(this, PartLoader.LoadedVariantThemesList[i]);
			editorVariantItem.gameObject.SetActive(value: true);
			editorVariantItem.transform.localPosition = new Vector3(editorVariantItem.transform.localPosition.x, editorVariantItem.transform.localPosition.y, -0.5f);
			vItems.Add(editorVariantItem);
		}
	}

	public void TapIcon(AvailablePart part)
	{
		EditorLogic.fetch.OnPartListIconTap(part);
	}

	public void TapIcon(ShipTemplate st)
	{
		EditorLogic.fetch.OnPartListIconTap(st);
	}

	public void TapBackground()
	{
		EditorLogic.fetch.OnPartListBackgroundTap();
	}

	public void RevealPart(AvailablePart partToFind, bool revealToolip)
	{
	}
}
