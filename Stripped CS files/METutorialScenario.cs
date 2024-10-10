using System;
using System.Collections.Generic;
using Expansions.Missions;
using Expansions.Missions.Editor;
using Expansions.Missions.Runtime;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

public class METutorialScenario : TutorialScenario
{
	public enum NodeDefinition
	{
		None = -1,
		StartNode,
		CreateVessel,
		Orbit,
		VesselLanded,
		ScienceExperiment,
		DialogMessage,
		DialogMessage2,
		VesselLanded2,
		TimeSinceNode,
		ApplyScore,
		Count
	}

	public static class NodeTemplateName
	{
		public const string StartNode = "";

		public const string CreateVessel = "CreateVessel";

		public const string Orbit = "Orbit";

		public const string Landed = "Landed";

		public const string ScienceExperiment = "ScienceExperiment";

		public const string DialogMessage = "DialogMessage";

		public const string TestTimeSinceNode = "TestTimeSinceNode";

		public const string MissionScore = "MissionScore";

		public const string VesselDestroyed = "Crashed";

		public static int Count()
		{
			return typeof(NodeTemplateName).GetProperties().Length;
		}

		public static string At(int index)
		{
			if (index >= 0 && index <= Count())
			{
				return typeof(NodeTemplateName).GetProperties()[index].GetValue(null, null) as string;
			}
			return "";
		}
	}

	public enum TutorialButtonType
	{
		NoButton = 0,
		Next = 1,
		Ok = 2,
		Done = 4,
		Continue = 8
	}

	public struct NodeParameter
	{
		public NodeDefinition node;

		public List<string> fields;

		public NodeParameter(NodeDefinition nodekey, List<string> fieldkeys)
		{
			node = nodekey;
			fields = fieldkeys;
		}
	}

	public const string createVesselSettings = "Expansions.Missions.Actions.ActionCreateVessel.vesselSituation";

	public const string createVesselLocationSettings = "location";

	public const string createVesselSituationParam = "situation";

	public const string createVesselFacilityParam = "facility";

	public const string createVesselLaunchSiteParam = "launchSite";

	public const string orbitCelestialBodyParam = "Expansions.Missions.Tests.TestOrbit.missionOrbit";

	public const string vesselLandedSettings = "Expansions.Missions.Tests.TestVesselSituationLanded.locationSituation";

	public const string vesselLandedCelestialBodyParam = "bodyData";

	public const string vesselLandedCelestialBiomeParam = "biomeData";

	public const string vesselLandedLocationTypeParam = "locationChoice";

	public const string scienceExperimentIsEventNode = "Expansions.Missions.MENode.isEvent";

	public const string dialogMessageParam = "Expansions.Missions.Actions.ActionDialogMessage.message";

	public const string scienceExperimentParam = "Expansions.Missions.Tests.TestScienceExperiment.experimentID";

	public const string scienceSituationParam = "Expansions.Missions.Tests.TestScienceExperiment.experimentSituation";

	public const string scienceCelestialBody = "Expansions.Missions.Tests.TestScienceExperiment.biomeData";

	public const string timeNodeNodeParam = "Expansions.Missions.Tests.TestTimeSinceNode.nodeID";

	public const string timeNodeTimeParam = "Expansions.Missions.Tests.TestTimeSinceNode.time";

	public const string timeNodeOperatorParam = "Expansions.Missions.Tests.TestTimeSinceNode.comparisonOperator";

	public const string applyScoreScoreParam = "Expansions.Missions.Actions.ActionMissionScore.score";

	public const string createVesselPartPicker = "requiredParts";

	public const string vesselDestroyedVesselParam = "Expansions.Missions.Tests.TestVessel.vesselID";

	public string stateName = "welcome";

	public string lockId = "tutorial";

	public bool complete;

	public List<TutorialPage> tutorialPages;

	public MEBasicNodeListFilter<MEGUINodeIcon> tutorialFilter_none = new MEBasicNodeListFilter<MEGUINodeIcon>("tutorial_None", (MEGUINodeIcon a) => false);

	public readonly List<MEGUIParameter> meguiParametersInTutorialMode = new List<MEGUIParameter>();

	public const float centerPosition = 0.3f;

	public PointerClickHandler.PointerClickEvent<PointerEventData> clickOnSearchEvent;

	public List<NodeParameter> parameterfields = new List<NodeParameter>();

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Wernher";
		SetDialogRect(new Rect(CalcDialogXRatio(), 0.85f, 420f, 190f));
		base.OnAssetSetup();
	}

	public override void OnTutorialSetup()
	{
		if (complete)
		{
			CloseTutorialWindow();
			return;
		}
		CreateTutorialPages();
		Tutorial.AddPages(tutorialPages);
		Tutorial.StartTutorial(stateName);
	}

	public virtual void CreateTutorialPages()
	{
	}

	public override void OnOnDestroy()
	{
		DestroyTutorialPage();
		base.OnOnDestroy();
	}

	public void DestroyTutorialPage()
	{
		for (int i = 0; i < tutorialPages.Count; i++)
		{
			DestroyTutorialPage(tutorialPages[i]);
		}
		tutorialPages.Clear();
	}

	public void DestroyTutorialPage(TutorialPage page)
	{
		if (page.OnEnter != null && page.OnEnter.Target != null && page.OnEnter.Target is IDisposable disposable)
		{
			disposable.Dispose();
		}
		page.OnEnter = null;
		page.OnUpdate = null;
		page.OnFixedUpdate = null;
		page.OnLateUpdate = null;
		page.OnLeave = null;
		page.StateEvents.Clear();
	}

	public void EnableAllTutorialPageButtons(bool enable)
	{
		if (!(Tutorial.CurrentState is TutorialPage tutorialPage))
		{
			return;
		}
		DialogGUIButton[] array = FindButtons(ref tutorialPage.Dialog.Options);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OptionInteractableCondition = () => enable;
		}
	}

	public void EnableTutorialPageButton(bool enable, int index)
	{
		if (!(Tutorial.CurrentState is TutorialPage tutorialPage))
		{
			return;
		}
		DialogGUIButton[] array = FindButtons(ref tutorialPage.Dialog.Options);
		if (index < array.Length)
		{
			array[index].OptionInteractableCondition = () => enable;
		}
	}

	public DialogGUIButton[] FindButtons(ref DialogGUIBase[] buttons)
	{
		List<DialogGUIButton> list = new List<DialogGUIButton>();
		DialogGUIBase[] array = buttons;
		foreach (DialogGUIBase dialogGUIBase in array)
		{
			if (dialogGUIBase is DialogGUIButton)
			{
				list.Add(dialogGUIBase as DialogGUIButton);
			}
		}
		return list.ToArray();
	}

	public bool CanSkipCurrentTutorialOnNodeDragEnd(int countBeforeDrag)
	{
		if (MissionEditorLogic.Instance.GetNodeListCout() > countBeforeDrag)
		{
			return MissionEditorLogic.Instance.CurrentSelectedNode != null;
		}
		return false;
	}

	public static void ShowMissionPlayDialog()
	{
		HighLogic.CurrentGame = null;
		MissionPlayDialog.Create(null);
	}

	public TutorialPage AddTutorialPage(string pageId, string pageTitleLocId, string dialogLocId, KFSMStateChange onEnterCallback, TutorialButtonType pageType = TutorialButtonType.Next)
	{
		TutorialPage tutorialPage = AddTutorialPage(pageId, pageTitleLocId, onEnterCallback);
		tutorialPage.dialog = CreateDialog(pageType, pageId, dialogLocId);
		tutorialPage.dialog.dialogRect.x = 0.3f;
		return tutorialPage;
	}

	public TutorialPage AddTutorialPage(string pageId, string pageTitleLocId, KFSMStateChange onEnterCallback)
	{
		return CreateTutorialPage(pageId, pageTitleLocId, onEnterCallback);
	}

	public List<TutorialPage> AddTutorialPage(TutorialPageConfig config)
	{
		List<TutorialPage> list = new List<TutorialPage>();
		for (int i = 0; i < config.tutoStepsConfig.Count; i++)
		{
			TutorialPageConfig tutorialPageConfig = config.tutoStepsConfig[i];
			TutorialPage item = AddTutorialPage(tutorialPageConfig.pageId, tutorialPageConfig.pageTitleLocId, tutorialPageConfig.pageDialogLocId, tutorialPageConfig.onEnterCallback, tutorialPageConfig.pageButtonType);
			list.Add(item);
		}
		return list;
	}

	public MultiOptionDialog CreateDialog(TutorialButtonType pageType, string pageId, string loc)
	{
		return CreateMultiButtonDialog(pageId, loc, pageType);
	}

	public void OnEnterEmpty(KFSMState state)
	{
	}

	public void HideAllTutorialSelectors()
	{
		for (int num = meguiParametersInTutorialMode.Count - 1; num >= 0; num--)
		{
			MEGUIParameter parameter = meguiParametersInTutorialMode[num];
			HideTutorialSelection(parameter);
		}
		meguiParametersInTutorialMode.Clear();
	}

	public void HideTutorialSelection(MEGUIParameter parameter)
	{
		MissionEditorLogic.Instance.HideTutorialIndicator(parameter);
		if (IsTutorialParameterSelected(parameter))
		{
			meguiParametersInTutorialMode.Remove(parameter);
		}
	}

	public void ShowTutorialSelection(MEGUIParameter parameter)
	{
		MissionEditorLogic.Instance.ShowTutorialIndicator(parameter);
		if (!IsTutorialParameterSelected(parameter))
		{
			meguiParametersInTutorialMode.Add(parameter);
		}
	}

	public bool IsTutorialParameterSelected(MEGUIParameter parameter)
	{
		return meguiParametersInTutorialMode.Contains(parameter);
	}

	public int GetSelectedNodeIndex()
	{
		MEGUINode currentSelectedNode = MissionEditorLogic.Instance.CurrentSelectedNode;
		return GetMEGUINodeIndex(currentSelectedNode);
	}

	public int GetMEGUINodeIndex(MEGUINode guiNode)
	{
		List<MEGUINode> displayedNodes = MissionEditorLogic.Instance.GetDisplayedNodes();
		int num = 0;
		while (true)
		{
			if (num < displayedNodes.Count)
			{
				if (guiNode == displayedNodes[num])
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public MEGUINode GetSelectedMEGUINode()
	{
		int selectedNodeIndex = GetSelectedNodeIndex();
		if (selectedNodeIndex != -1)
		{
			return GetMEGUINode(selectedNodeIndex);
		}
		return null;
	}

	public MENode GetSelectedMENode()
	{
		MEGUINode selectedMEGUINode = GetSelectedMEGUINode();
		if (!(selectedMEGUINode == null))
		{
			return selectedMEGUINode.Node;
		}
		return null;
	}

	public string GetSelectedNodeTemplateName()
	{
		MENode selectedMENode = GetSelectedMENode();
		if (selectedMENode == null)
		{
			return string.Empty;
		}
		string text = selectedMENode.basicNodeSource;
		if (text == string.Empty)
		{
			text = "";
		}
		return text;
	}

	public MEGUINode GetMEGUINode(int nodeIndex)
	{
		List<MEGUINode> displayedNodes = MissionEditorLogic.Instance.GetDisplayedNodes();
		if (nodeIndex >= 0 && nodeIndex < displayedNodes.Count)
		{
			return displayedNodes[nodeIndex];
		}
		return null;
	}

	public List<MENode> GetMENodeByTemplateName(string nodeDefinition)
	{
		List<MEGUINode> mEGUINodes = GetMEGUINodes(nodeDefinition);
		List<MENode> list = new List<MENode>();
		for (int i = 0; i < mEGUINodes.Count; i++)
		{
			list.Add(mEGUINodes[i].Node);
		}
		return list;
	}

	public List<MEGUINode> GetMEGUINodes(string templateName)
	{
		return MissionEditorLogic.Instance.GetDisplayedNodes().FindAll((MEGUINode x) => x.Node != null && x.Node.basicNodeSource == templateName);
	}

	public MENode GetMENode(int nodeIndex)
	{
		List<MEGUINode> displayedNodes = MissionEditorLogic.Instance.GetDisplayedNodes();
		if (nodeIndex >= 0 && nodeIndex < displayedNodes.Count)
		{
			return displayedNodes[nodeIndex].Node;
		}
		return null;
	}

	public void HighlightNode(int nodeIndex, bool active)
	{
		MissionEditorLogic.Instance.ClearSelectedNodesList();
		MissionEditorLogic.Instance.ClearConnectorGroupSelection();
		MEGUINode mEGUINode = GetMEGUINode(nodeIndex);
		if (mEGUINode != null)
		{
			mEGUINode.SetHighlighter(MissionEditorLogic.Instance.TutorialHighlighterColor);
			mEGUINode.ToggleHighlighter(active);
		}
	}

	public void ResetHighLightLink()
	{
		for (int i = 0; i < NodeTemplateName.Count(); i++)
		{
			List<MEGUINode> mEGUINodes = GetMEGUINodes(NodeTemplateName.At(i));
			for (int j = 0; j < mEGUINodes.Count; j++)
			{
				MEGUINode mEGUINode = mEGUINodes[j];
				mEGUINode.ToggleInputHolderHighlighter(state: false);
				mEGUINode.ToggleOutputHolderHighlighter(state: false);
			}
		}
	}

	public void DragNodeHelper(Func<MEGUINodeIcon, bool> filterCriteria, string categoryName = "")
	{
		SetEditorLock();
		LockNodeSettings(locked: true);
		LockNodeFilter(locked: false);
		LockNodeList(locked: false);
		MENodeCategorizer.Instance.HighlightNodes(highlight: false);
		MENodeCategorizer.Instance.HighlightNodes(filterCriteria, highlight: true);
		MENodeCategorizer.Instance.HightLightCategoryButton(isActive: false);
		MENodeCategorizer.Instance.HightLightCategoryButton(categoryName, isActive: true);
		MENodeCategorizer.Instance.EnableDrag(enable: false);
		MENodeCategorizer.Instance.EnableDrag(filterCriteria, enable: true);
	}

	public void RemoveDragHelper()
	{
		LockNodeSettings(locked: true);
		LockNodeFilter(locked: true);
		LockNodeList(locked: true);
		MissionEditorLogic.Instance.HighLightDisplayedNode(isActive: false);
		MENodeCategorizer.Instance.HightLightCategoryButton(isActive: false);
		ResetNodeSettingsMask();
		ResetNodeFilterMask();
		ResetNodeListMask();
	}

	public void ClearFilter()
	{
		MENodeCategorizer.Instance.ExcludeFilters.ClearFilter();
		MENodeCategorizer.Instance.RefreshCurrentDisplay();
	}

	public void FilterNodes(bool add)
	{
		FilterNodes(add, tutorialFilter_none);
	}

	public void FilterNodes(bool add, Func<MEGUINodeIcon, bool> criteria)
	{
		FilterNodes(add, new MEBasicNodeListFilter<MEGUINodeIcon>("tutorial_byCriteria", criteria));
	}

	public void FilterNodes(bool add, MEBasicNodeListFilter<MEGUINodeIcon> filterName)
	{
		if (add)
		{
			MENodeCategorizer.Instance.ExcludeFilters.AddFilter(filterName);
			MENodeCategorizer.Instance.RefreshCurrentDisplay();
		}
		else
		{
			MENodeCategorizer.Instance.ExcludeFilters.RemoveFilter(filterName);
			MENodeCategorizer.Instance.RefreshCurrentDisplay();
		}
	}

	public void SetEditorLock()
	{
		MissionEditorLogic.Instance.Unlock(lockId);
		MissionEditorLogic.Instance.SetLock(ControlTypes.All, add: true, lockId);
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_EXIT, add: false, lockId);
	}

	public void LockNodeSettings(bool locked)
	{
		MissionEditorLogic.Instance.SetSettingsActionPaneInputMask(new List<string> { ControlTypes.EDITOR_EDIT_STAGES.ToString() });
		LockCanvas(locked);
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_EDIT_STAGES, locked, lockId);
	}

	public void LockCanvas(bool locked)
	{
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_LOCK, locked, lockId);
		MissionEditorHistory.Clear();
	}

	public void ResetNodeSettingsMask()
	{
		MissionEditorLogic.Instance.SetSettingsActionPaneInputMaskToDefault();
	}

	public void LockNodeList(bool locked)
	{
		if (UIMasterController.Instance.CurrentTooltip != null && UIMasterController.Instance.CurrentTooltip is NodeListTooltipController)
		{
			UIMasterController.Instance.DestroyCurrentTooltip();
		}
		MissionEditorLogic.Instance.SetNodeListInputMask(new List<string> { ControlTypes.EDITOR_ICON_PICK.ToString() });
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_ICON_PICK, locked, lockId);
	}

	public void ResetNodeListMask()
	{
		MissionEditorLogic.Instance.SetNodeListInputMaskToDefault();
	}

	public void LockNodeFilter(bool locked)
	{
		MissionEditorLogic.Instance.SetNodeFilterInputMask(new List<string> { ControlTypes.EDITOR_TAB_SWITCH.ToString() });
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_TAB_SWITCH, locked, lockId);
	}

	public void ResetNodeFilterMask()
	{
		MissionEditorLogic.Instance.SetNodeFilterInputMaskToDefault();
	}

	public void SaveOnClickOnSearchListener()
	{
		clickOnSearchEvent = MissionEditorLogic.Instance.GetOnClickOnSearchListener();
	}

	public void RemoveOnClickOnSearchListener()
	{
		MissionEditorLogic.Instance.SetOnClickOnSearchListener(null);
	}

	public void RestoreOnClickOnSearchListener()
	{
		MissionEditorLogic.Instance.SetOnClickOnSearchListener(clickOnSearchEvent);
	}

	public void HighlightNodeFiltered(bool highLight)
	{
		MissionEditorLogic.Instance.HighLightDisplayedNode(highLight);
	}

	public void RefreshParameterList()
	{
		List<NodeParameter> list = new List<NodeParameter>();
		for (NodeDefinition nodeDefinition = NodeDefinition.StartNode; nodeDefinition < NodeDefinition.Count; nodeDefinition++)
		{
			List<string> list2 = new List<string>();
			MENode mENode = GetMENode((int)nodeDefinition);
			if (mENode != null)
			{
				BaseAPFieldList baseAPFieldList = new BaseAPFieldList(mENode);
				int i;
				for (i = 0; i < baseAPFieldList.Count; i++)
				{
					list2.Add(baseAPFieldList[i].FieldID);
				}
				List<IActionModule> actionModules = mENode.actionModules;
				for (int j = 0; j < actionModules.Count; j++)
				{
					baseAPFieldList = new BaseAPFieldList(actionModules[j]);
					for (int k = 0; k < baseAPFieldList.Count; k++)
					{
						list2.Add(baseAPFieldList[k].FieldID);
						i++;
					}
				}
			}
			list.Add(new NodeParameter(nodeDefinition, list2));
		}
		parameterfields = list;
	}

	public void LockSapParameters(NodeDefinition nodedefinition, params string[] excluded)
	{
		RefreshParameterList();
		if (parameterfields.Count <= (int)nodedefinition || nodedefinition == NodeDefinition.None)
		{
			return;
		}
		List<string> list = new List<string>(excluded);
		for (int i = 0; i < parameterfields[(int)nodedefinition].fields.Count; i++)
		{
			bool enable = list.Contains(parameterfields[(int)nodedefinition].fields[i]);
			MEGUIParameter parameter = GetParameter(parameterfields[(int)nodedefinition].fields[i]);
			if (parameter != null)
			{
				EnableMeguiParameter(parameter, enable);
			}
		}
	}

	public void EnableMeguiParameter(MEGUIParameter parameter, bool enable)
	{
		parameter.isSelectable = enable;
		parameter.IsInteractable = enable;
	}

	public MEGUIParameter GetParameter(string fieldid)
	{
		return MissionEditorLogic.Instance.actionPane.GetParameterFromFieldID(fieldid);
	}

	public MEGUIParameter GetSubParameter(List<string> fieldsId)
	{
		MEGUIParameter mEGUIParameter = null;
		MEGUICompoundParameter mEGUICompoundParameter = null;
		int num = 0;
		while (true)
		{
			if (num >= fieldsId.Count)
			{
				return mEGUIParameter;
			}
			mEGUIParameter = ((!(mEGUIParameter == null)) ? mEGUICompoundParameter.GetSubParameter(fieldsId[num]) : GetParameter(fieldsId[num]));
			mEGUICompoundParameter = mEGUIParameter as MEGUICompoundParameter;
			if (mEGUICompoundParameter == null)
			{
				break;
			}
			num++;
		}
		return mEGUIParameter;
	}

	public MEGUIParameter GetVesselSettings()
	{
		return GetParameter("Expansions.Missions.Actions.ActionCreateVessel.vesselSituation");
	}

	public MEGUIParameter GetSituationParam()
	{
		List<string> fieldsId = new List<string> { "Expansions.Missions.Actions.ActionCreateVessel.vesselSituation", "location", "situation" };
		return GetSubParameter(fieldsId);
	}

	public MEGUIParameter GetFacilityParam()
	{
		List<string> fieldsId = new List<string> { "Expansions.Missions.Actions.ActionCreateVessel.vesselSituation", "location", "facility" };
		return GetSubParameter(fieldsId);
	}

	public MEGUIParameter GetLaunchSiteParam()
	{
		List<string> fieldsId = new List<string> { "Expansions.Missions.Actions.ActionCreateVessel.vesselSituation", "location", "launchSite" };
		return GetSubParameter(fieldsId);
	}

	public MEGUIParameter GetPartPickerParam()
	{
		List<string> fieldsId = new List<string> { "Expansions.Missions.Actions.ActionCreateVessel.vesselSituation", "requiredParts" };
		return GetSubParameter(fieldsId);
	}

	public MEGUIParameter GetOrbitCelestialBody()
	{
		return GetParameter("Expansions.Missions.Tests.TestOrbit.missionOrbit");
	}

	public MEGUIParameter GetVesselLandedSettings()
	{
		return GetParameter("Expansions.Missions.Tests.TestVesselSituationLanded.locationSituation");
	}

	public MEGUIParameter GetVesselLandedCelestialBody()
	{
		List<string> fieldsId = new List<string> { "Expansions.Missions.Tests.TestVesselSituationLanded.locationSituation", "bodyData" };
		return GetSubParameter(fieldsId);
	}

	public MEGUIParameter GetVesselLandedBiomes()
	{
		List<string> fieldsId = new List<string> { "Expansions.Missions.Tests.TestVesselSituationLanded.locationSituation", "biomeData" };
		return GetSubParameter(fieldsId);
	}

	public MEGUIParameter GetVesselLandedLocationType()
	{
		List<string> fieldsId = new List<string> { "Expansions.Missions.Tests.TestVesselSituationLanded.locationSituation", "locationChoice" };
		return GetSubParameter(fieldsId);
	}

	public MEGUIParameter GetScienceIsEventNodeParam()
	{
		return GetParameter("Expansions.Missions.MENode.isEvent");
	}

	public MEGUIParameter GetDialogMessageParam()
	{
		return GetParameter("Expansions.Missions.Actions.ActionDialogMessage.message");
	}

	public MEGUIParameter GetScienceExperimentParam()
	{
		return GetParameter("Expansions.Missions.Tests.TestScienceExperiment.experimentID");
	}

	public MEGUIParameter GetScienceSituationParam()
	{
		return GetParameter("Expansions.Missions.Tests.TestScienceExperiment.experimentSituation");
	}

	public MEGUIParameter GetScienceCelestialBodyParam()
	{
		return GetParameter("Expansions.Missions.Tests.TestScienceExperiment.biomeData");
	}

	public MEGUIParameter GetTimeNodeStartNode()
	{
		return GetParameter("Expansions.Missions.Tests.TestTimeSinceNode.nodeID");
	}

	public MEGUIParameter GetTimeNodeTime()
	{
		return GetParameter("Expansions.Missions.Tests.TestTimeSinceNode.time");
	}

	public MEGUIParameter GetTimeNodeOperator()
	{
		return GetParameter("Expansions.Missions.Tests.TestTimeSinceNode.comparisonOperator");
	}

	public MEGUIParameter GetApplyScoreScoreParam()
	{
		return GetParameter("Expansions.Missions.Actions.ActionMissionScore.score");
	}

	public MEGUIParameter GetVesselDestroyedVesselParam()
	{
		return GetParameter("Expansions.Missions.Tests.TestVessel.vesselID");
	}

	public virtual void GoToNextTutorial()
	{
		List<MissionFileInfo> list = MissionsUtils.GatherMissionFiles(MissionTypes.Stock);
		for (int i = 0; i < list.Count; i++)
		{
			MissionFileInfo missionFileInfo = list[i];
			if (missionFileInfo.FilePath.Contains(GetNextTutorialName()))
			{
				MissionEditorLogic.Instance.MissionToLoadSelected(missionFileInfo.FilePath);
			}
		}
	}

	public virtual string GetNextTutorialName()
	{
		return string.Empty;
	}

	public bool IsAllNodeLinked(NodeDefinition start, NodeDefinition end, bool uniquestartlink = true, bool uniqueendlink = true)
	{
		List<int> start2 = new List<int> { (int)start };
		List<int> end2 = new List<int> { (int)end };
		return IsAllNodeLinked(start2, end2, uniquestartlink, uniqueendlink);
	}

	public bool IsAllNodeLinked(List<int> start, List<int> end, bool uniquestartlink = true, bool uniqueendlink = true)
	{
		if (start.Count == end.Count)
		{
			bool[] array = new bool[end.Count];
			for (int i = 0; i < start.Count; i++)
			{
				for (int j = 0; j < end.Count; j++)
				{
					MENode mENode = GetMENode(start[i]);
					MENode mENode2 = GetMENode(end[j]);
					if ((bool)mENode && (bool)mENode2)
					{
						bool flag = (!uniquestartlink || (uniquestartlink && mENode.toNodes.Count == 1)) && (!uniqueendlink || (uniqueendlink && mENode2.fromNodes.Count == 1));
						array[j] = array[j] || (mENode.toNodes.Contains(mENode2) && mENode2.fromNodes.Contains(mENode) && flag);
						bool flag2 = IsAllTrue(array);
						HighlightRequiredNodes(start[i], end[j], !flag2);
						if (uniquestartlink && uniqueendlink)
						{
							HighlightUnwantedConnector(mENode, start[i], mENode2, end[j]);
						}
						if (flag2)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public void HighlightRequiredNodes(int startIndex, int endIndex, bool highlight)
	{
		GetMEGUINode(startIndex).ToggleOutputHolderHighlighter(highlight);
		GetMEGUINode(endIndex).ToggleInputHolderHighlighter(highlight);
	}

	public void HighlightUnwantedConnector(MENode startNode, int start, MENode endNode, int end)
	{
		if (!startNode.toNodes.Contains(endNode) || !endNode.fromNodes.Contains(startNode))
		{
			return;
		}
		MEGUINode mEGUINode = GetMEGUINode(start);
		for (int i = 0; i < mEGUINode.OutputConnectors.Count; i++)
		{
			if (startNode.toNodes[i] != endNode)
			{
				mEGUINode.OutputConnectors[i].LineColour = Color.red;
			}
		}
		MEGUINode mEGUINode2 = GetMEGUINode(end);
		for (int j = 0; j < mEGUINode2.InputConnectors.Count; j++)
		{
			if (endNode.fromNodes[j] != startNode)
			{
				mEGUINode2.InputConnectors[j].LineColour = Color.red;
			}
		}
	}

	public bool IsAllTrue(bool[] array)
	{
		int num = array.Length - 1;
		while (true)
		{
			if (num >= 0)
			{
				if (!array[num])
				{
					break;
				}
				num--;
				continue;
			}
			return true;
		}
		return false;
	}
}
