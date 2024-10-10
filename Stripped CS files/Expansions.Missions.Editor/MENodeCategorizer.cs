using System;
using System.Collections;
using System.Collections.Generic;
using ns11;
using ns2;
using ns5;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MENodeCategorizer : MonoBehaviour
{
	public Transform nodeCategoryButtonTransformParent;

	public Transform displayedNodeIconTransformParent;

	public MENodeCategoryButton nodeCategoryButtonPrefab;

	public MEGUINodeIcon NodeIconPrefab;

	public TMP_InputField searchField;

	public PointerClickHandler searchFieldClickHandler;

	public float searchKeystrokeDelay = 0.25f;

	[SerializeField]
	public TextMeshProUGUI NotFoundText;

	[SerializeField]
	public ToggleGroup toggleGroup;

	public List<MENodeCategoryButton> categoryButtons;

	public List<MEGUINodeIcon> displayedNodeIcons;

	public List<MEGUINodeIcon> storedNodeIcons;

	public MEGUINodeIconGroup _toolboxNodeIconGroup;

	public MEGUINodeIconGroup _canvasNodeIconGroup;

	public Transform nodeCategorizerButtonRepository;

	public string lastDisplayedCategory;

	[SerializeField]
	public GameObject categoryIconLoaderPrefab;

	[NonSerialized]
	public IconLoader categoryIconLoader;

	[SerializeField]
	public GameObject nodeIconLoaderPrefab;

	[NonSerialized]
	public IconLoader nodeIconLoader;

	public MEBasicNodeListFilterList<MEGUINodeIcon> ExcludeFilters;

	public string[] searchTerms;

	public bool showingSearchResults;

	public float searchTimer;

	public Coroutine searchRoutine;

	public Action onSearchRoutineFinish;

	public static MENodeCategorizer Instance { get; set; }

	public bool ShowingSearchResults => showingSearchResults;

	public void Awake()
	{
		Instance = this;
		searchField.onValueChanged.AddListener(SearchField_OnValueChange);
		searchField.onEndEdit.AddListener(SearchField_OnEndEdit);
		searchFieldClickHandler = searchField.gameObject.AddComponent<PointerClickHandler>();
		searchFieldClickHandler.onPointerClick.AddListener(SearchField_OnClick);
		displayedNodeIcons = new List<MEGUINodeIcon>();
		storedNodeIcons = new List<MEGUINodeIcon>();
		categoryButtons = new List<MENodeCategoryButton>();
		nodeCategorizerButtonRepository = new GameObject("nodeCategorizerButtonRepository").transform;
		nodeCategorizerButtonRepository.transform.SetParent(base.transform);
		nodeCategorizerButtonRepository.gameObject.SetActive(value: false);
		ExcludeFilters = new MEBasicNodeListFilterList<MEGUINodeIcon>();
		categoryIconLoader = UnityEngine.Object.Instantiate(categoryIconLoaderPrefab).GetComponent<IconLoader>();
		nodeIconLoader = UnityEngine.Object.Instantiate(nodeIconLoaderPrefab).GetComponent<IconLoader>();
	}

	public void Start()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		searchTerms = new string[0];
		for (int i = 0; i < MissionEditorLogic.Instance.basicNodes.Count; i++)
		{
			if (!list.Contains(MissionEditorLogic.Instance.basicNodes[i].category))
			{
				list.Add(MissionEditorLogic.Instance.basicNodes[i].category);
				list2.Add(Localizer.Format(MissionEditorLogic.Instance.basicNodes[i].categoryDisplayName));
			}
			MEGUINodeIcon mEGUINodeIcon = UnityEngine.Object.Instantiate(NodeIconPrefab);
			string value = ((MissionEditorLogic.Instance.basicNodes[i].iconURL == null) ? "" : MissionEditorLogic.Instance.basicNodes[i].iconURL);
			Icon icon = null;
			if (!string.IsNullOrEmpty(value))
			{
				icon = nodeIconLoader.GetIcon(value);
			}
			mEGUINodeIcon.SetUp(MissionEditorLogic.Instance.basicNodes[i], icon);
			mEGUINodeIcon.toggle.group = toggleGroup;
			mEGUINodeIcon.toggle.interactable = false;
			mEGUINodeIcon.transform.SetParent(nodeCategorizerButtonRepository);
			mEGUINodeIcon.transform.localPosition = Vector3.zero;
			mEGUINodeIcon.transform.localScale = Vector3.one;
			mEGUINodeIcon.gameObject.SetActive(value: false);
			storedNodeIcons.Add(mEGUINodeIcon);
		}
		List<string> list3 = new List<string>(list2);
		list2.Sort();
		storedNodeIcons.Sort();
		for (int j = 0; j < list2.Count; j++)
		{
			string text = list[list3.IndexOf(list2[j])];
			MENodeCategoryButton mENodeCategoryButton = UnityEngine.Object.Instantiate(nodeCategoryButtonPrefab);
			mENodeCategoryButton.Setup(text, list2[j], this, GetCategoryImage(text));
			mENodeCategoryButton.transform.SetParent(nodeCategoryButtonTransformParent.transform);
			mENodeCategoryButton.transform.localPosition = Vector3.zero;
			mENodeCategoryButton.transform.localScale = Vector3.one;
			categoryButtons.Add(mENodeCategoryButton);
		}
		DisplayNodesInCategory(list[list3.IndexOf(list2[0])]);
	}

	public void ResetStoredIcons()
	{
		for (int num = storedNodeIcons.Count - 1; num >= 0; num--)
		{
			storedNodeIcons[num].ResetCanvasSetting();
			if (storedNodeIcons[num].IsCanvasNode)
			{
				storedNodeIcons[num].gameObject.DestroyGameObject();
				storedNodeIcons.RemoveAt(num);
			}
		}
		showingSearchResults = false;
	}

	public void ClearNodesInCategory()
	{
		toggleGroup.SetAllTogglesOff();
		for (int num = displayedNodeIcons.Count - 1; num >= 0; num--)
		{
			displayedNodeIcons[num].gameObject.SetActive(value: false);
			displayedNodeIcons[num].transform.SetParent(nodeCategorizerButtonRepository);
			displayedNodeIcons[num].transform.localPosition = Vector3.zero;
			displayedNodeIcons[num].toggle.interactable = true;
			storedNodeIcons.Add(displayedNodeIcons[num]);
			displayedNodeIcons.RemoveAt(num);
		}
		RemoveSearchToolboxGroups();
		if (NotFoundText != null)
		{
			NotFoundText.gameObject.SetActive(value: true);
		}
		showingSearchResults = false;
	}

	public MEGUINodeIcon AddCanvasNodeButton(MEBasicNode basicNode, MEGUINode guiNode)
	{
		if (basicNode != null && !(guiNode == null))
		{
			MEGUINodeIcon mEGUINodeIcon = UnityEngine.Object.Instantiate(NodeIconPrefab);
			string value = ((basicNode.iconURL == null) ? "" : basicNode.iconURL);
			Icon icon = null;
			if (!string.IsNullOrEmpty(value))
			{
				icon = nodeIconLoader.GetIcon(value);
			}
			mEGUINodeIcon.SetUp(basicNode, icon, guiNode);
			mEGUINodeIcon.toggle.group = toggleGroup;
			mEGUINodeIcon.toggle.interactable = false;
			mEGUINodeIcon.transform.SetParent(nodeCategorizerButtonRepository);
			mEGUINodeIcon.transform.localPosition = Vector3.zero;
			mEGUINodeIcon.transform.localScale = Vector3.one;
			mEGUINodeIcon.gameObject.SetActive(value: false);
			storedNodeIcons.Add(mEGUINodeIcon);
			if (guiNode.Node.isStartNode)
			{
				NodeListTooltipController component = mEGUINodeIcon.gameObject.GetComponent<NodeListTooltipController>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			if (showingSearchResults && NodeMatchesSearch(mEGUINodeIcon))
			{
				DisplayStoredIcon(mEGUINodeIcon);
				if (NotFoundText != null)
				{
					NotFoundText.gameObject.SetActive(value: false);
				}
			}
			return mEGUINodeIcon;
		}
		return null;
	}

	public void RemoveCanvasNodeButton(MEGUINodeIcon nodeIcon)
	{
		storedNodeIcons.Remove(nodeIcon);
		nodeIcon.gameObject.SetActive(value: false);
	}

	public void RefreshCurrentDisplay()
	{
		DisplayNodesInCategory(string.Empty);
	}

	public void DisplayNodesInCategory(string newCategory)
	{
		DeselectLastCategoryButton();
		ClearNodesInCategory();
		int num = 0;
		List<MEGUINodeIcon> filteredList = ExcludeFilters.GetFilteredList(storedNodeIcons);
		if (newCategory != string.Empty)
		{
			for (int num2 = filteredList.Count - 1; num2 >= 0; num2--)
			{
				if (filteredList[num2].basicNode.category == newCategory && !filteredList[num2].IsCanvasNode)
				{
					DisplayStoredIcon(filteredList[num2]);
					num++;
				}
			}
			lastDisplayedCategory = newCategory;
			for (int i = 0; i < categoryButtons.Count; i++)
			{
				if (categoryButtons[i].categoryName == lastDisplayedCategory)
				{
					categoryButtons[i].SelectCategory();
					break;
				}
			}
		}
		else
		{
			for (int num3 = filteredList.Count - 1; num3 >= 0; num3--)
			{
				if (!filteredList[num3].IsCanvasNode)
				{
					DisplayStoredIcon(filteredList[num3]);
					num++;
				}
			}
			DeselectLastCategoryButton();
		}
		if (num > 0 && NotFoundText != null)
		{
			NotFoundText.gameObject.SetActive(value: false);
		}
	}

	public void DeselectLastCategoryButton()
	{
		int num = 0;
		while (true)
		{
			if (num < categoryButtons.Count)
			{
				if (categoryButtons[num].categoryName == lastDisplayedCategory)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		categoryButtons[num].DeselectCategory();
	}

	public Icon GetCategoryImage(string category)
	{
		Icon icon = categoryIconLoader.GetIcon("stockIcon_" + category);
		if (icon.name == "stockIcon_fallback")
		{
			ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("MEBASICCATEGORY");
			for (int i = 0; i < configNodes.Length; i++)
			{
				if (configNodes[i].GetValue("name") == category && configNodes[i].HasValue("icon"))
				{
					return categoryIconLoader.GetIcon(configNodes[i].GetValue("icon"));
				}
			}
		}
		return icon;
	}

	public void DisplayStoredIcon(int index)
	{
		storedNodeIcons[index].gameObject.SetActive(value: true);
		if (storedNodeIcons[index].IsCanvasNode && _canvasNodeIconGroup != null && _canvasNodeIconGroup.containerChildren != null)
		{
			storedNodeIcons[index].transform.SetParent(_canvasNodeIconGroup.containerChildren.transform);
		}
		else
		{
			storedNodeIcons[index].transform.SetParent(showingSearchResults ? _toolboxNodeIconGroup.containerChildren.transform : displayedNodeIconTransformParent.transform);
		}
		storedNodeIcons[index].toggle.interactable = showingSearchResults;
		if (showingSearchResults)
		{
			toggleGroup.RegisterToggle(storedNodeIcons[index].toggle);
		}
		storedNodeIcons[index].transform.localPosition = Vector3.zero;
		displayedNodeIcons.Add(storedNodeIcons[index]);
		storedNodeIcons.RemoveAt(index);
	}

	public void DisplayStoredIcon(MEGUINodeIcon icon)
	{
		icon.gameObject.SetActive(value: true);
		if (icon.IsCanvasNode && _canvasNodeIconGroup != null && _canvasNodeIconGroup.containerChildren != null)
		{
			icon.transform.SetParent(_canvasNodeIconGroup.containerChildren.transform);
		}
		else
		{
			icon.transform.SetParent(showingSearchResults ? _toolboxNodeIconGroup.containerChildren.transform : displayedNodeIconTransformParent.transform);
		}
		icon.toggle.interactable = showingSearchResults;
		if (showingSearchResults)
		{
			toggleGroup.RegisterToggle(icon.toggle);
		}
		icon.transform.localPosition = Vector3.zero;
		displayedNodeIcons.Add(icon);
		storedNodeIcons.Remove(icon);
	}

	public void HighLightDisplayedNodeIcon(bool isActive)
	{
		for (int i = 0; i < displayedNodeIcons.Count; i++)
		{
			HightLightNodeIcon(displayedNodeIcons[i], isActive);
		}
	}

	public void HightLightNodeIcon(MEGUINodeIcon nodeIcon, bool isActive)
	{
		nodeIcon.ToggleHighlighter(isActive);
	}

	public void HighlightNodes(bool highlight)
	{
		for (int i = 0; i < storedNodeIcons.Count; i++)
		{
			HightLightNodeIcon(storedNodeIcons[i], highlight);
		}
		HighLightDisplayedNodeIcon(highlight);
	}

	public void HighlightNodes(Func<MEGUINodeIcon, bool> criteria, bool highlight)
	{
		MEGUINodeIcon[] nodes = GetNodes(criteria);
		for (int i = 0; i < nodes.Length; i++)
		{
			HightLightNodeIcon(nodes[i], highlight);
		}
	}

	public void HightLightCategoryButton(bool isActive)
	{
		for (int i = 0; i < categoryButtons.Count; i++)
		{
			HightLightCategoryButton(categoryButtons[i], isActive);
		}
	}

	public void HightLightCategoryButton(string categoryName, bool isActive)
	{
		if (string.IsNullOrEmpty(categoryName))
		{
			return;
		}
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("MEBASICNODE");
		if (!ContainsCategoryConfig(categoryName, configNodes))
		{
			Debug.LogError("categoryName not found in config. Make sure of the categoryName is present in the config file");
			return;
		}
		for (int i = 0; i < categoryButtons.Count; i++)
		{
			if (categoryButtons[i].categoryName == categoryName)
			{
				HightLightCategoryButton(categoryButtons[i], isActive);
			}
		}
	}

	public bool ContainsCategoryConfig(string categoryName, ConfigNode[] nodes)
	{
		int num = 0;
		while (true)
		{
			if (num < nodes.Length)
			{
				ConfigNode configNode = nodes[num];
				if (configNode != null)
				{
					string value = string.Empty;
					if (configNode.TryGetValue("category", ref value) && !string.IsNullOrEmpty(value) && value.Equals(categoryName))
					{
						break;
					}
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void HightLightCategoryButton(MENodeCategoryButton categoryButton, bool isActive)
	{
		categoryButton.ToggleHighlighter(isActive);
	}

	public MEGUINodeIcon[] GetNodes(Func<MEGUINodeIcon, bool> criteria)
	{
		List<MEGUINodeIcon> list = new List<MEGUINodeIcon>();
		for (int i = 0; i < storedNodeIcons.Count; i++)
		{
			if (criteria(storedNodeIcons[i]))
			{
				list.Add(storedNodeIcons[i]);
			}
		}
		for (int j = 0; j < displayedNodeIcons.Count; j++)
		{
			if (criteria(displayedNodeIcons[j]))
			{
				list.Add(displayedNodeIcons[j]);
			}
		}
		return list.ToArray();
	}

	public void EnableDrag(bool enable)
	{
		for (int i = 0; i < storedNodeIcons.Count; i++)
		{
			storedNodeIcons[i].IsInteractable = enable;
		}
		for (int j = 0; j < displayedNodeIcons.Count; j++)
		{
			displayedNodeIcons[j].IsInteractable = enable;
		}
	}

	public void EnableDrag(Func<MEGUINodeIcon, bool> criteria, bool enable)
	{
		MEGUINodeIcon[] nodes = GetNodes(criteria);
		for (int i = 0; i < nodes.Length; i++)
		{
			nodes[i].IsInteractable = enable;
		}
	}

	public List<string> GetDisplayedNodeNames()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < displayedNodeIcons.Count; i++)
		{
			list.Add(displayedNodeIcons[i].nodeText.text);
		}
		return list;
	}

	public void ClearSearchField()
	{
		if (!(searchField == null))
		{
			searchField.text = string.Empty;
		}
	}

	public void FocusSearchField()
	{
		if (!(searchField == null) && !searchField.isFocused)
		{
			InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "SearchFieldTextInput");
			searchField.ActivateInputField();
		}
	}

	public void SearchField_OnEndEdit(string s)
	{
		InputLockManager.RemoveControlLock("SearchFieldTextInput");
	}

	public void SearchField_OnValueChange(string s)
	{
		if (searchField.text != string.Empty)
		{
			SearchStart();
		}
		else
		{
			SearchStop();
		}
	}

	public void SearchField_OnValueChange(string s, Action ballback)
	{
		onSearchRoutineFinish = ballback;
		SearchField_OnValueChange(s);
	}

	public void SearchField_OnClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "SearchFieldTextInput");
			SearchStart();
		}
	}

	public void SearchStart()
	{
		searchTimer = Time.realtimeSinceStartup;
		if (searchRoutine == null)
		{
			searchRoutine = StartCoroutine(SearchRoutine());
		}
	}

	public IEnumerator SearchRoutine()
	{
		while (searchTimer + searchKeystrokeDelay > Time.realtimeSinceStartup)
		{
			yield return null;
		}
		searchTerms = BasePartCategorizer.SearchTagSplit(searchField.text);
		if (searchTerms.Length == 0)
		{
			DisplayNodesInCategory(lastDisplayedCategory);
		}
		else
		{
			MissionEditorLogic.Instance.ClearSelectedNodesList();
			DeselectLastCategoryButton();
			ClearNodesInCategory();
			CreateSearchToolboxGroups();
			showingSearchResults = true;
			int num = 0;
			for (int num2 = storedNodeIcons.Count - 1; num2 >= 0; num2--)
			{
				if (NodeMatchesSearch(storedNodeIcons[num2]))
				{
					DisplayStoredIcon(num2);
					num++;
				}
			}
			if (num > 0)
			{
				if (NotFoundText != null)
				{
					NotFoundText.gameObject.SetActive(value: false);
				}
			}
			else
			{
				RemoveSearchToolboxGroups();
			}
		}
		SearchStop();
	}

	public void HighLightSelectedSearchResults()
	{
		MEGUINodeIcon mEGUINodeIcon = null;
		for (int i = 0; i < displayedNodeIcons.Count; i++)
		{
			if (displayedNodeIcons[i].toggle.isOn)
			{
				mEGUINodeIcon = displayedNodeIcons[i];
				break;
			}
		}
		if (mEGUINodeIcon != null)
		{
			mEGUINodeIcon.toggle.isOn = false;
			mEGUINodeIcon.toggle.isOn = true;
		}
	}

	public void CreateSearchToolboxGroups()
	{
		if (_toolboxNodeIconGroup == null)
		{
			_toolboxNodeIconGroup = MEGUINodeIconGroup.Create("Toolbox", Localizer.Format("#autoLOC_8002173"));
			_toolboxNodeIconGroup.transform.SetParent(displayedNodeIconTransformParent.transform);
		}
		else
		{
			_toolboxNodeIconGroup.gameObject.SetActive(value: true);
		}
		if (_canvasNodeIconGroup == null)
		{
			_canvasNodeIconGroup = MEGUINodeIconGroup.Create("Canvas", Localizer.Format("#autoLOC_8002174"));
			_canvasNodeIconGroup.transform.SetParent(displayedNodeIconTransformParent.transform);
		}
		else
		{
			_canvasNodeIconGroup.gameObject.SetActive(value: true);
		}
	}

	public void RemoveSearchToolboxGroups()
	{
		if (_canvasNodeIconGroup != null)
		{
			_canvasNodeIconGroup.gameObject.SetActive(value: false);
		}
		if (_toolboxNodeIconGroup != null)
		{
			_toolboxNodeIconGroup.gameObject.SetActive(value: false);
		}
	}

	public void SearchStop()
	{
		searchRoutine = null;
		if (searchField.text == string.Empty)
		{
			DisplayNodesInCategory(lastDisplayedCategory);
		}
		if (onSearchRoutineFinish != null)
		{
			onSearchRoutineFinish();
		}
	}

	public bool NodeMatchesSearch(MEGUINodeIcon node)
	{
		int num = searchTerms.Length - 1;
		while (true)
		{
			if (num >= 0)
			{
				if (node.lowerCaseCategoryName.Contains(searchTerms[num]) || node.lowerCaseDisplayName.Contains(searchTerms[num]) || (node.IsCanvasNode && Localizer.Format(node.nodeText.text).ToLowerInvariant().Contains(searchTerms[num])))
				{
					break;
				}
				num--;
				continue;
			}
			return false;
		}
		return true;
	}
}
