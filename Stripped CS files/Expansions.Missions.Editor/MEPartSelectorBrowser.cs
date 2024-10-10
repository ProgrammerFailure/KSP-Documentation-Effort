using System.Collections;
using System.Collections.Generic;
using ns13;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEPartSelectorBrowser : MonoBehaviour
{
	public delegate void CancelledCallback();

	public delegate void SelectionCallback(List<string> selectedParts);

	public delegate void PartSelectionCallback(AvailablePart part, bool state);

	[SerializeField]
	public TextMeshProUGUI headerTitle;

	public MEPartSelectorEntry mePrefab;

	public string title;

	public UIListSorter partListSorter;

	public MEPartCategorizer PartCategorizer;

	public ScrollRect partListScrollRect;

	public float iconSize;

	public float iconOverScale;

	public float iconOverSpin;

	public Button selectButton;

	public Button cancelButton;

	public CancelledCallback OnBrowseCancelled;

	public SelectionCallback OnBrowseSelectedParts;

	public PartSelectionCallback OnPartSelectedChange;

	[SerializeField]
	public RectTransform listContainer;

	public List<MEPartSelectorEntry> partsList;

	public Dictionary<string, List<string>> excludedParts;

	public List<string> _selectedParts;

	public Color _selectionColor = Color.green;

	public Dictionary<string, MEPartSelectorEntry> iconCache = new Dictionary<string, MEPartSelectorEntry>();

	public EditorPartListFilterList<AvailablePart> partsFilter;

	public EditorPartListFilter<AvailablePart> searchFilter;

	public IComparer<AvailablePart> currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc: true, r1.title.CompareTo(r2.title), (r1.cost + r1.partPrefab.GetModuleCosts(r1.cost)).CompareTo(r2.cost + r2.partPrefab.GetModuleCosts(r2.cost))));

	public Transform partIconStorage;

	public List<string> SelectedParts => _selectedParts;

	public Color SelectionColor => _selectionColor;

	public void Awake()
	{
		_selectedParts = new List<string>();
		partListScrollRect.onValueChanged.AddListener(OnPartListUpdate);
		partListSorter.AddOnSortCallback(SortingCallback);
		selectButton.onClick.AddListener(onButtonSelect);
		cancelButton.onClick.AddListener(onButtonCancel);
		partIconStorage = new GameObject("partIconStorage").transform;
		partIconStorage.SetParent(base.transform);
		partIconStorage.gameObject.SetActive(value: false);
	}

	public IEnumerator Start()
	{
		headerTitle.text = title;
		partsFilter = new EditorPartListFilterList<AvailablePart>();
		PartCategorizer.Setup(this);
		BuildPartList();
		yield return null;
		OnPartListUpdate(Vector2.zero);
		selectButton.interactable = true;
	}

	public void OnDestroy()
	{
		partListScrollRect.onValueChanged.RemoveListener(OnPartListUpdate);
	}

	public MEPartSelectorBrowser Spawn(List<string> partList, Dictionary<string, List<string>> excludedParts, Color selectionColor, SelectionCallback onSelect, CancelledCallback onCancel, RectTransform parent = null)
	{
		MEPartSelectorBrowser component = Object.Instantiate(this).GetComponent<MEPartSelectorBrowser>();
		component.transform.SetParent((parent == null) ? DialogCanvasUtil.DialogCanvasRect : parent, worldPositionStays: false);
		component._selectedParts = partList;
		component.excludedParts = ((excludedParts != null) ? excludedParts : new Dictionary<string, List<string>>());
		component._selectionColor = selectionColor;
		component.OnBrowseCancelled = onCancel;
		component.OnBrowseSelectedParts = onSelect;
		component.title = Localizer.Format("#autoLOC_8006104");
		return component;
	}

	public MEPartSelectorBrowser Spawn(MEGUIParameterPartPicker parameter, PartSelectionCallback onPartSelect, RectTransform parent = null)
	{
		MEPartSelectorBrowser mEPartSelectorBrowser = Spawn(new List<string>(parameter.FieldValue), parameter.ExcludedParts, parameter.SelectedPartsColor, null, null, parent);
		mEPartSelectorBrowser.title = parameter.DialogTitle;
		mEPartSelectorBrowser.OnPartSelectedChange = onPartSelect;
		return mEPartSelectorBrowser;
	}

	public void Setup(MEGUIParameterPartPicker parameter, PartSelectionCallback onPartSelect)
	{
		_selectedParts = new List<string>(parameter.FieldValue);
		excludedParts = ((parameter.ExcludedParts != null) ? parameter.ExcludedParts : new Dictionary<string, List<string>>());
		_selectionColor = parameter.SelectedPartsColor;
		title = parameter.DialogTitle;
		OnPartSelectedChange = onPartSelect;
		headerTitle.text = title;
		PartCategorizer.Reset();
		BuildPartList();
	}

	public void onButtonCancel()
	{
		OnBrowseCancelled();
		Dismiss();
	}

	public void onButtonSelect()
	{
		OnBrowseSelectedParts(_selectedParts);
		Dismiss();
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void Dismiss()
	{
		Object.Destroy(base.gameObject);
	}

	public void BuildPartList()
	{
		ClearPartList();
		List<AvailablePart> filteredList = partsFilter.GetFilteredList(PartLoader.LoadedPartsList);
		filteredList.Sort(currentPartSorting);
		int i = 0;
		for (int count = filteredList.Count; i < count; i++)
		{
			bool selected = _selectedParts.Contains(filteredList[i].name);
			bool status = false;
			string excludeReason = "";
			foreach (KeyValuePair<string, List<string>> excludedPart in excludedParts)
			{
				if (excludedPart.Value.Contains(filteredList[i].name))
				{
					status = true;
					excludeReason = excludedPart.Key;
					break;
				}
			}
			if (!iconCache.ContainsKey(filteredList[i].name))
			{
				MEPartSelectorEntry mEPartSelectorEntry = Object.Instantiate(mePrefab);
				mEPartSelectorEntry.Create(this, filteredList[i], iconSize, iconOverScale, iconOverSpin, selected);
				DisablePart(mEPartSelectorEntry, status, excludeReason);
				partsList.Add(mEPartSelectorEntry);
				iconCache.Add(mEPartSelectorEntry.partInfo.name, mEPartSelectorEntry);
			}
			else
			{
				MEPartSelectorEntry mEPartSelectorEntry2 = iconCache[filteredList[i].name];
				mEPartSelectorEntry2.gameObject.SetActive(value: true);
				mEPartSelectorEntry2.UpdateSelection(selected);
				DisablePart(mEPartSelectorEntry2, status, excludeReason);
				partsList.Add(mEPartSelectorEntry2);
			}
			AddPartIcon(partsList[i], listContainer);
		}
		OnPartListUpdate(Vector2.zero);
	}

	public void DisablePart(MEPartSelectorEntry entry, bool status, string excludeReason)
	{
		if (entry.isGrey != status)
		{
			if (status)
			{
				entry.SetGrey(excludeReason);
			}
			else
			{
				entry.UnsetGrey();
			}
		}
	}

	public void AddPartIcon(MEPartSelectorEntry entry, RectTransform listParent)
	{
		entry.transform.SetParent(listParent, worldPositionStays: false);
	}

	public void OnPartListUpdate(Vector2 vector)
	{
		PartListTooltipController.SetupScreenSpaceMask((RectTransform)listContainer.parent);
		int count = partsList.Count;
		while (count-- > 0)
		{
			PartListTooltipController.SetScreenSpaceMaskMaterials(partsList[count].materials);
		}
	}

	public void ClearPartList()
	{
		if (partsList != null)
		{
			int count = partsList.Count;
			while (count-- > 0)
			{
				MEPartSelectorEntry mEPartSelectorEntry = partsList[count];
				mEPartSelectorEntry.gameObject.SetActive(value: false);
				mEPartSelectorEntry.transform.SetParent(partIconStorage, worldPositionStays: false);
			}
			partsList.Clear();
		}
		else
		{
			partsList = new List<MEPartSelectorEntry>();
		}
	}

	public void SetPartSelectionStatus(AvailablePart part, bool selected)
	{
		MEPartSelectorEntry mEPartSelectorEntry = iconCache[part.name];
		if (!mEPartSelectorEntry.isGrey && mEPartSelectorEntry.isSelected != selected)
		{
			iconCache[part.name].UpdateSelection(selected);
			OnPartSelectionChange(part, selected);
		}
	}

	public void OnPartSelectionChange(AvailablePart part, bool selected)
	{
		if (selected)
		{
			_selectedParts.Add(part.name);
		}
		else if (_selectedParts.Contains(part.name))
		{
			_selectedParts.Remove(part.name);
		}
		OnPartSelectedChange(part, selected);
	}

	public void OnFilterSelectionChange(EditorPartListFilter<AvailablePart> filter, bool status)
	{
		if (status)
		{
			partsFilter.AddFilter(filter);
		}
		else
		{
			partsFilter.RemoveFilter(filter);
		}
		BuildPartList();
	}

	public void OnSearchChange(EditorPartListFilter<AvailablePart> filter)
	{
		if (searchFilter != null)
		{
			partsFilter.RemoveFilter(searchFilter);
		}
		searchFilter = filter;
		if (filter != null)
		{
			partsFilter.AddFilter(searchFilter);
		}
		BuildPartList();
	}

	public void SortingCallback(int button, bool asc)
	{
		switch (button)
		{
		case 0:
			currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.title.CompareTo(r2.title), (r1.cost + r1.partPrefab.GetModuleCosts(r1.cost)).CompareTo(r2.cost + r2.partPrefab.GetModuleCosts(r2.cost))));
			break;
		case 1:
			currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.partPrefab.mass.CompareTo(r2.partPrefab.mass), r1.title.CompareTo(r2.title)));
			break;
		case 2:
			currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc, (r1.cost + r1.partPrefab.GetModuleCosts(r1.cost)).CompareTo(r2.cost + r2.partPrefab.GetModuleCosts(r2.cost)), r1.title.CompareTo(r2.title)));
			break;
		case 3:
			currentPartSorting = new RUIutils.FuncComparer<AvailablePart>((AvailablePart r1, AvailablePart r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.partSize.CompareTo(r2.partSize), r1.title.CompareTo(r2.title)));
			break;
		}
		BuildPartList();
	}
}
