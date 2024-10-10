using System;
using System.Collections.Generic;
using System.Linq;
using ns5;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEActionPane : MonoBehaviour
{
	public static MEActionPane fetch;

	public MEGUIPanel SAPPanel;

	[SerializeField]
	public ScrollRect sapScrollRect;

	public bool sapNeedsRefocus;

	public Bounds sapRectTransformBounds;

	public float sapFocusCalc;

	public MEGUIPanel GAPPanel;

	[SerializeField]
	public LayoutElement gapLayoutElement;

	[SerializeField]
	public int minGapPanelHeight = 100;

	public MEGUIParameter lockedSAPParameter;

	public MENode currentLockedNode;

	public MEGUIFooterAdditionalButton FooterAdditionalButtonPrefab;

	public MEGUIParameterGroup ParameterGroupPrefab;

	public Toggle gapLockToggle;

	[SerializeField]
	public int parameterGroupOffset = 6;

	[SerializeField]
	public GameObject sapFixedHeader;

	[SerializeField]
	public RectTransform sapHeaderParamHolder;

	public Image sapScrollRectBackgroundImage;

	public List<string> sapFrozenParamIDs;

	[SerializeField]
	public GameObject gapIconLoaderPrefab;

	[NonSerialized]
	public IconLoader gapIconLoader;

	public Dictionary<MonoBehaviour, Dictionary<string, MEGUIParameter>> parameterCache;

	public Dictionary<string, MEGUIParameter> currentParameterCache;

	public Transform cacheTmpLocation;

	public List<TMP_InputField> inputTabStops = new List<TMP_InputField>();

	public int currentInputField;

	public string currentInputFieldText = "";

	public ActionPaneDisplay CurrentGapDisplay { get; set; }

	public MEGUIParameter SelectedSAPParameter { get; set; }

	public bool IsGAPLocked { get; set; }

	public void Awake()
	{
		fetch = this;
		parameterCache = new Dictionary<MonoBehaviour, Dictionary<string, MEGUIParameter>>();
		gapIconLoader = UnityEngine.Object.Instantiate(gapIconLoaderPrefab).GetComponent<IconLoader>();
		sapScrollRectBackgroundImage = sapScrollRect.GetComponent<Image>();
		sapFrozenParamIDs = new List<string> { "Expansions.Missions.MENode.Title" };
	}

	public void Start()
	{
		cacheTmpLocation = new GameObject("APCache").transform;
		gapLockToggle.onValueChanged.AddListener(OnGAPLockValueChange);
		sapScrollRect.verticalScrollbar.onValueChanged.AddListener(delegate
		{
			OnSapScrollbarValueChange();
		});
		SetMaxPanelWidth();
		SetLockInteractivity();
		GAPPanel.minHeight = minGapPanelHeight;
		InitializeGapMinHeight();
	}

	public void OnDestroy()
	{
		Clean();
	}

	public void OnParameterClick(MEGUIParameter paramClicked)
	{
		if (!(SelectedSAPParameter != paramClicked))
		{
			return;
		}
		if (SelectedSAPParameter != null)
		{
			SelectedSAPParameter.UnSelect();
		}
		SelectedSAPParameter = paramClicked;
		if (SelectedSAPParameter != null)
		{
			if (CurrentGapDisplay != null && !IsGAPLocked)
			{
				if (SelectedSAPParameter.HasGAP)
				{
					CurrentGapDisplay.Clean();
				}
				else
				{
					CurrentGapDisplay.Destroy();
					CurrentGapDisplay = null;
				}
			}
			SelectedSAPParameter.Select();
			if (!IsGAPLocked && SelectedSAPParameter.HasGAP)
			{
				SelectedSAPParameter.DisplayGAP();
				sapNeedsRefocus = true;
			}
		}
		else if (CurrentGapDisplay != null && !IsGAPLocked)
		{
			CurrentGapDisplay.Destroy();
			CurrentGapDisplay = null;
		}
		SetLockInteractivity();
	}

	public void SAPRefreshNodeParameters()
	{
		if (MissionEditorLogic.Instance.CurrentSelectedNode != null)
		{
			MissionEditorLogic.Instance.CurrentSelectedNode.Select(deselectOtherNodes: false, bypassNodeSelection: true);
			SAPDisplayNodeParameters(MissionEditorLogic.Instance.CurrentSelectedNode);
		}
	}

	public void SAPDisplayNodeParameters(MEGUINode node)
	{
		Clean();
		if (node == null || node.Node == null)
		{
			return;
		}
		if (!parameterCache.ContainsKey(node))
		{
			parameterCache.Add(node, new Dictionary<string, MEGUIParameter>());
		}
		currentParameterCache = parameterCache[node];
		DisplayNodeSettingsSection(node.Node);
		if (node.Node.isStartNode)
		{
			DisplayNodeModuleSection(node.Node.mission.situation);
		}
		int i = 0;
		for (int count = node.Node.testGroups.Count; i < count; i++)
		{
			int j = 0;
			for (int count2 = node.Node.testGroups[i].testModules.Count; j < count2; j++)
			{
				DisplayNodeModuleSection(node.Node.testGroups[i].testModules[j]);
			}
		}
		int k = 0;
		for (int count3 = node.Node.actionModules.Count; k < count3; k++)
		{
			DisplayNodeModuleSection(node.Node.actionModules[k]);
		}
		GameEvents.Mission.onBuilderNodeFocused.Fire(node.Node);
		GetInputFieldsList(currentParameterCache);
	}

	public void SAPDisplayConnectorParameters(MEGUIConnector connector)
	{
		Clean();
		if (!(connector == null))
		{
			if (!parameterCache.ContainsKey(connector))
			{
				parameterCache.Add(connector, new Dictionary<string, MEGUIParameter>());
			}
			currentParameterCache = parameterCache[connector];
			BaseAPFieldList fields = new BaseAPFieldList(connector);
			AddParameterBlock("Connector Settings", "#autoLOC_8100305", fields, SAPPanel.ContentRoot);
		}
	}

	public void SAPDisplayMultipleSelectedItemsMessage(int nodesSelected, string itemName)
	{
		Clean();
		(MEGUIParametersController.Instance.GetControl(typeof(MEGUI_Label)).Create(null, SAPPanel.ContentRoot) as MEGUIParameterLabel).title.text = Localizer.Format("#autoLOC_8006022", itemName, nodesSelected);
	}

	public void DisplayNodeSettingsSection(MENode node)
	{
		sapFixedHeader.gameObject.SetActive(value: false);
		sapScrollRectBackgroundImage.enabled = false;
		BaseAPFieldList fields = new BaseAPFieldList(node);
		List<string> list = new List<string>();
		if (node.isStartNode || node.guiNode.HasOutputConnections())
		{
			list.Add("MissionEnd");
		}
		AddParameterBlock("Node Settings", "#autoLOC_8100306", fields, SAPPanel.ContentRoot, list);
	}

	public void DisplayNodeModuleSection(IMENodeDisplay module)
	{
		BaseAPFieldList fields = new BaseAPFieldList(module);
		AddParameterBlock(module.GetName(), module.GetDisplayName(), fields, SAPPanel.ContentRoot);
		module.ParameterSetupComplete();
	}

	public MEGUIParameterGroup DisplayModuleHeader(string name, string displayName, BaseAPFieldList fields, Transform parent)
	{
		if (currentParameterCache.ContainsKey("header_" + name))
		{
			currentParameterCache["header_" + name].transform.SetParent(parent);
			currentParameterCache["header_" + name].gameObject.SetActive(value: true);
		}
		else
		{
			MEGUIParameterGroup value = ParameterGroupPrefab.Create(fields, parent, displayName) as MEGUIParameterGroup;
			currentParameterCache.Add("header_" + name, value);
		}
		currentParameterCache["header_" + name].transform.SetAsLastSibling();
		return currentParameterCache["header_" + name] as MEGUIParameterGroup;
	}

	public void DisplayModuleFooter(string name, BaseAPFieldList fields)
	{
		if (currentParameterCache.ContainsKey("footer_" + name))
		{
			currentParameterCache["footer_" + name].transform.SetParent(SAPPanel.ContentRoot);
			currentParameterCache["footer_" + name].gameObject.SetActive(value: true);
		}
		else
		{
			currentParameterCache.Add("footer_" + name, FooterAdditionalButtonPrefab.Create(fields, SAPPanel.ContentRoot));
		}
		currentParameterCache["footer_" + name].transform.SetAsLastSibling();
	}

	public MEGUIParameter DisplayParameter(BaseAPField field, Transform parent, bool isSelectable)
	{
		MEGUIParameter mEGUIParameter = null;
		if (currentParameterCache.ContainsKey(field.FieldID))
		{
			mEGUIParameter = currentParameterCache[field.FieldID];
			mEGUIParameter.transform.SetParent(parent);
			mEGUIParameter.Display();
		}
		else
		{
			MEGUIParameter control = MEGUIParametersController.Instance.GetControl(field.Attribute.GetType());
			if (control != null)
			{
				mEGUIParameter = control.Create(field, parent);
				currentParameterCache.Add(field.FieldID, mEGUIParameter);
				currentParameterCache[field.FieldID].Display();
			}
		}
		mEGUIParameter.isSelectable = isSelectable;
		mEGUIParameter.transform.SetAsLastSibling();
		return mEGUIParameter;
	}

	public MEGUIParameterGroup AddParameterBlock(string headerName, string displayName, BaseAPFieldList fields, Transform parent, List<string> invalidGroups = null, string currentGroup = "", bool parametersSelectable = true, int offset = 0, int depth = 0)
	{
		MEGUIParameterGroup mEGUIParameterGroup = DisplayModuleHeader(headerName, displayName, fields, parent);
		mEGUIParameterGroup.Depth = depth;
		DictionaryValueList<string, BaseAPFieldList> dictionaryValueList = new DictionaryValueList<string, BaseAPFieldList>();
		bool flag = false;
		for (int i = 0; i < fields.Count; i++)
		{
			if (fields[i].Group == currentGroup)
			{
				if (invalidGroups == null || !invalidGroups.Contains(fields[i].Group))
				{
					if (sapFrozenParamIDs.Contains(fields[i].FieldID))
					{
						DisplayParameter(fields[i], sapHeaderParamHolder, isSelectable: false);
						sapFixedHeader.gameObject.SetActive(value: true);
						sapScrollRectBackgroundImage.enabled = true;
					}
					else
					{
						DisplayParameter(fields[i], mEGUIParameterGroup.containerChilden, parametersSelectable).parentGroup = mEGUIParameterGroup;
						flag |= fields[i].GroupStartCollapsed;
					}
				}
			}
			else if (dictionaryValueList.ContainsKey(fields[i].Group))
			{
				dictionaryValueList[fields[i].Group].Add(fields[i]);
			}
			else
			{
				BaseAPFieldList baseAPFieldList = new BaseAPFieldList();
				baseAPFieldList.Add(fields[i]);
				dictionaryValueList.Add(fields[i].Group, baseAPFieldList);
			}
		}
		for (int j = 0; j < dictionaryValueList.Count; j++)
		{
			string text = dictionaryValueList.KeyAt(j);
			string displayName2 = text;
			BaseAPFieldList baseAPFieldList2 = dictionaryValueList[text];
			for (int k = 0; k < baseAPFieldList2.Count; k++)
			{
				if (!string.IsNullOrEmpty(baseAPFieldList2[k].GroupDisplayName))
				{
					displayName2 = baseAPFieldList2[k].GroupDisplayName;
					break;
				}
			}
			AddParameterBlock(text, displayName2, baseAPFieldList2, mEGUIParameterGroup.containerChilden, invalidGroups, text, parametersSelectable, parameterGroupOffset, depth + 1);
		}
		mEGUIParameterGroup.Display();
		if (flag)
		{
			mEGUIParameterGroup.CollapseGroup();
		}
		if (fields.Count == 0)
		{
			mEGUIParameterGroup.gameObject.SetActive(value: false);
		}
		return mEGUIParameterGroup;
	}

	public void RemoveParameterGroup(string group)
	{
		if (currentParameterCache == null)
		{
			return;
		}
		List<MEGUIParameter> list = new List<MEGUIParameter>(currentParameterCache.Values);
		List<MEGUIParameter> list2 = new List<MEGUIParameter>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].GetGroupName() == group)
			{
				list2.Add(list[i]);
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			list2[j].RemoveParameterByGroup(group);
		}
	}

	public MEGUIParameter GetParameterFromFieldID(string fieldID)
	{
		if (currentParameterCache.ContainsKey(fieldID))
		{
			return currentParameterCache[fieldID];
		}
		return null;
	}

	public void OnSapScrollbarValueChange()
	{
		if (sapNeedsRefocus)
		{
			sapNeedsRefocus = false;
			sapRectTransformBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(sapScrollRect.transform);
			sapFocusCalc = sapRectTransformBounds.size.y / sapRectTransformBounds.center.y / 100f;
			if (sapFocusCalc < 0.2f)
			{
				sapScrollRect.verticalNormalizedPosition = 0f;
			}
			else
			{
				sapScrollRect.verticalNormalizedPosition = sapFocusCalc;
			}
		}
	}

	public T GAPInitialize<T>() where T : ActionPaneDisplay
	{
		if (!IsGAPLocked)
		{
			if (CurrentGapDisplay != null)
			{
				if (!(CurrentGapDisplay.GetType() != typeof(T)))
				{
					return CurrentGapDisplay as T;
				}
				CurrentGapDisplay.Destroy();
				UnityEngine.Object.DestroyImmediate(CurrentGapDisplay);
				CurrentGapDisplay = null;
			}
			T val = GAPPanel.ContentRoot.GetComponent<T>();
			if (val == null)
			{
				val = GAPPanel.ContentRoot.gameObject.AddComponent<T>();
			}
			CurrentGapDisplay = val;
			SetLockInteractivity();
			return val;
		}
		return null;
	}

	public void Clean()
	{
		if (SelectedSAPParameter != null)
		{
			SelectedSAPParameter.UnSelect();
			SelectedSAPParameter = null;
		}
		if (currentParameterCache != null)
		{
			foreach (KeyValuePair<string, MEGUIParameter> item in currentParameterCache)
			{
				item.Value.gameObject.SetActive(value: false);
				item.Value.transform.SetParent(cacheTmpLocation);
			}
		}
		if (CurrentGapDisplay != null && !IsGAPLocked)
		{
			CurrentGapDisplay.Destroy();
			CurrentGapDisplay = null;
		}
		for (int i = 0; i < SAPPanel.ContentRoot.childCount; i++)
		{
			UnityEngine.Object.Destroy(SAPPanel.ContentRoot.GetChild(i).gameObject);
		}
		SetLockInteractivity();
		inputTabStops.Clear();
	}

	public void ClearCache()
	{
		foreach (KeyValuePair<MonoBehaviour, Dictionary<string, MEGUIParameter>> item in parameterCache)
		{
			foreach (KeyValuePair<string, MEGUIParameter> item2 in item.Value)
			{
				UnityEngine.Object.Destroy(item2.Value.gameObject);
			}
		}
		currentParameterCache = null;
		parameterCache.Clear();
		InitializeGapMinHeight();
	}

	public void OnGAPLockValueChange(bool value)
	{
		IsGAPLocked = value;
		if (!IsGAPLocked)
		{
			if (SelectedSAPParameter != null && SelectedSAPParameter.HasGAP)
			{
				if (lockedSAPParameter != SelectedSAPParameter)
				{
					if (CurrentGapDisplay != null)
					{
						CurrentGapDisplay.Clean();
					}
					SelectedSAPParameter.DisplayGAP();
				}
			}
			else if (CurrentGapDisplay != null)
			{
				CurrentGapDisplay.Destroy();
				CurrentGapDisplay = null;
			}
		}
		else
		{
			lockedSAPParameter = SelectedSAPParameter;
			if (MissionEditorLogic.Instance.CurrentSelectedNode.Node != null)
			{
				CurrentLockdeNode(MissionEditorLogic.Instance.CurrentSelectedNode.Node);
			}
		}
	}

	public void ScrollBar(float value)
	{
		if (sapScrollRect.verticalScrollbar.onValueChanged != null)
		{
			sapScrollRect.verticalScrollbar.onValueChanged.Invoke(value);
		}
	}

	public void UpdateSAPScroll(PointerEventData pointerData)
	{
		sapScrollRect.OnScroll(pointerData);
	}

	public void ScrollPanel(float height)
	{
		RectTransform content = sapScrollRect.content;
		content.anchoredPosition = new Vector2(content.anchoredPosition.x, height);
		Canvas.ForceUpdateCanvases();
		VerticalLayoutGroup component = content.GetComponent<VerticalLayoutGroup>();
		if (component != null)
		{
			component.SetLayoutVertical();
		}
	}

	public void SetMaxPanelWidth()
	{
		GetComponent<MEGUIPanel>().maxWidth = Screen.width / 2;
	}

	public void SavePreferedGapSize()
	{
		GameSettings.MISSION_BUILDER_GAPHEIGHT = gapLayoutElement.minHeight;
	}

	public void InitializeGapMinHeight()
	{
		gapLayoutElement.minHeight = GameSettings.MISSION_BUILDER_GAPHEIGHT;
		GAPPanel.CurrentHeight = GameSettings.MISSION_BUILDER_GAPHEIGHT;
	}

	public void CheckDeletedNodeFromGap(MENode node)
	{
		if (IsGAPLocked && node == currentLockedNode)
		{
			gapLockToggle.isOn = false;
			Clean();
			ClearCache();
			CurrentGapDisplay = null;
			currentLockedNode = null;
			IsGAPLocked = false;
			SetLockInteractivity();
		}
	}

	public void CurrentLockdeNode(MENode node)
	{
		currentLockedNode = node;
	}

	public void SetLockInteractivity()
	{
		if (CurrentGapDisplay == null)
		{
			gapLockToggle.interactable = false;
		}
		else
		{
			gapLockToggle.interactable = true;
		}
	}

	public void GetInputFieldsList(Dictionary<string, MEGUIParameter> currentParam)
	{
		for (int i = 0; i < inputTabStops.Count; i++)
		{
			inputTabStops[i].onSelect.RemoveListener(InputfieldStringCatcher);
			inputTabStops[i].onValueChanged.RemoveListener(InputfieldStringCatcher);
		}
		inputTabStops.Clear();
		for (int j = 0; j < currentParam.Count; j++)
		{
			if (sapFrozenParamIDs.Contains(currentParam.ElementAt(j).Key) && currentParam.ElementAt(j).Value.TabStop)
			{
				inputTabStops.Add(currentParam.ElementAt(j).Value.GetComponentInChildren<TMP_InputField>());
			}
		}
		for (int k = 0; k < currentParam.Count; k++)
		{
			if (currentParam.ElementAt(k).Value.TabStop && !sapFrozenParamIDs.Contains(currentParam.ElementAt(k).Key))
			{
				TMP_InputField[] componentsInChildren = currentParam.ElementAt(k).Value.GetComponentsInChildren<TMP_InputField>();
				for (int l = 0; l < componentsInChildren.Length; l++)
				{
					inputTabStops.Add(componentsInChildren[l]);
				}
			}
		}
		for (int m = 0; m < inputTabStops.Count; m++)
		{
			inputTabStops[m].onSelect.AddListener(InputfieldStringCatcher);
			inputTabStops[m].onValueChanged.AddListener(InputfieldStringCatcher);
		}
	}

	public TMP_InputField GetNextInputTabStop(bool direction)
	{
		currentInputField = -1;
		for (int i = 0; i < inputTabStops.Count; i++)
		{
			if (inputTabStops[i].isFocused)
			{
				currentInputField = i;
				break;
			}
		}
		if (currentInputField == -1)
		{
			return null;
		}
		string text = currentInputFieldText.Replace("\t", "");
		inputTabStops[currentInputField].text = text;
		bool flag = false;
		int num = currentInputField;
		while (!flag)
		{
			num += ((!direction) ? 1 : (-1));
			if (num < 0)
			{
				num = inputTabStops.Count - 1;
			}
			if (num >= inputTabStops.Count)
			{
				num = 0;
			}
			if (num == currentInputField)
			{
				num = -1;
				flag = true;
			}
			if (inputTabStops[num].gameObject.activeInHierarchy)
			{
				flag = true;
			}
		}
		if (num < 0)
		{
			return inputTabStops[currentInputField];
		}
		return inputTabStops[num];
	}

	public void InputfieldStringCatcher(string text)
	{
		if (!(text == "\t"))
		{
			currentInputFieldText = text;
		}
	}
}
