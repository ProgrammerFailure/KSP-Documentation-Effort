using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions;
using Expansions.Missions.Editor;
using KSP.UI;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static int Count()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string At(int index)
		{
			throw null;
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

	private struct NodeParameter
	{
		public NodeDefinition node;

		public List<string> fields;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public NodeParameter(NodeDefinition nodekey, List<string> fieldkeys)
		{
			throw null;
		}
	}

	protected const string createVesselSettings = "Expansions.Missions.Actions.ActionCreateVessel.vesselSituation";

	protected const string createVesselLocationSettings = "location";

	protected const string createVesselSituationParam = "situation";

	protected const string createVesselFacilityParam = "facility";

	protected const string createVesselLaunchSiteParam = "launchSite";

	protected const string orbitCelestialBodyParam = "Expansions.Missions.Tests.TestOrbit.missionOrbit";

	protected const string vesselLandedSettings = "Expansions.Missions.Tests.TestVesselSituationLanded.locationSituation";

	protected const string vesselLandedCelestialBodyParam = "bodyData";

	protected const string vesselLandedCelestialBiomeParam = "biomeData";

	protected const string vesselLandedLocationTypeParam = "locationChoice";

	protected const string scienceExperimentIsEventNode = "Expansions.Missions.MENode.isEvent";

	public const string dialogMessageParam = "Expansions.Missions.Actions.ActionDialogMessage.message";

	protected const string scienceExperimentParam = "Expansions.Missions.Tests.TestScienceExperiment.experimentID";

	protected const string scienceSituationParam = "Expansions.Missions.Tests.TestScienceExperiment.experimentSituation";

	protected const string scienceCelestialBody = "Expansions.Missions.Tests.TestScienceExperiment.biomeData";

	protected const string timeNodeNodeParam = "Expansions.Missions.Tests.TestTimeSinceNode.nodeID";

	protected const string timeNodeTimeParam = "Expansions.Missions.Tests.TestTimeSinceNode.time";

	protected const string timeNodeOperatorParam = "Expansions.Missions.Tests.TestTimeSinceNode.comparisonOperator";

	protected const string applyScoreScoreParam = "Expansions.Missions.Actions.ActionMissionScore.score";

	protected const string createVesselPartPicker = "requiredParts";

	protected const string vesselDestroyedVesselParam = "Expansions.Missions.Tests.TestVessel.vesselID";

	public string stateName;

	public string lockId;

	public bool complete;

	protected List<TutorialPage> tutorialPages;

	protected MEBasicNodeListFilter<MEGUINodeIcon> tutorialFilter_none;

	private readonly List<MEGUIParameter> meguiParametersInTutorialMode;

	private const float centerPosition = 0.3f;

	private PointerClickHandler.PointerClickEvent<PointerEventData> clickOnSearchEvent;

	private List<NodeParameter> parameterfields;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public METutorialScenario()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAssetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CreateTutorialPages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyTutorialPage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DestroyTutorialPage(TutorialPage page)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableAllTutorialPageButtons(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void EnableTutorialPageButton(bool enable, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected DialogGUIButton[] FindButtons(ref DialogGUIBase[] buttons)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanSkipCurrentTutorialOnNodeDragEnd(int countBeforeDrag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void ShowMissionPlayDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected TutorialPage AddTutorialPage(string pageId, string pageTitleLocId, string dialogLocId, KFSMStateChange onEnterCallback, TutorialButtonType pageType = TutorialButtonType.Next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private TutorialPage AddTutorialPage(string pageId, string pageTitleLocId, KFSMStateChange onEnterCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected List<TutorialPage> AddTutorialPage(TutorialPageConfig config)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MultiOptionDialog CreateDialog(TutorialButtonType pageType, string pageId, string loc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEnterEmpty(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideAllTutorialSelectors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideTutorialSelection(MEGUIParameter parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowTutorialSelection(MEGUIParameter parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool IsTutorialParameterSelected(MEGUIParameter parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetSelectedNodeIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetMEGUINodeIndex(MEGUINode guiNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode GetSelectedMEGUINode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENode GetSelectedMENode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetSelectedNodeTemplateName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode GetMEGUINode(int nodeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MENode> GetMENodeByTemplateName(string nodeDefinition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUINode> GetMEGUINodes(string templateName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENode GetMENode(int nodeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void HighlightNode(int nodeIndex, bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetHighLightLink()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DragNodeHelper(Func<MEGUINodeIcon, bool> filterCriteria, string categoryName = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveDragHelper()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FilterNodes(bool add)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FilterNodes(bool add, Func<MEGUINodeIcon, bool> criteria)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FilterNodes(bool add, MEBasicNodeListFilter<MEGUINodeIcon> filterName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetEditorLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LockNodeSettings(bool locked)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LockCanvas(bool locked)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetNodeSettingsMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LockNodeList(bool locked)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetNodeListMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LockNodeFilter(bool locked)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetNodeFilterMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveOnClickOnSearchListener()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnClickOnSearchListener()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RestoreOnClickOnSearchListener()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HighlightNodeFiltered(bool highLight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshParameterList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LockSapParameters(NodeDefinition nodedefinition, params string[] excluded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableMeguiParameter(MEGUIParameter parameter, bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MEGUIParameter GetParameter(string fieldid)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MEGUIParameter GetSubParameter(List<string> fieldsId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetVesselSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetSituationParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetFacilityParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MEGUIParameter GetLaunchSiteParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MEGUIParameter GetPartPickerParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetOrbitCelestialBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetVesselLandedSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetVesselLandedCelestialBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetVesselLandedBiomes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetVesselLandedLocationType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetScienceIsEventNodeParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetDialogMessageParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetScienceExperimentParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetScienceSituationParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetScienceCelestialBodyParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetTimeNodeStartNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetTimeNodeTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetTimeNodeOperator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetApplyScoreScoreParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetVesselDestroyedVesselParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void GoToNextTutorial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetNextTutorialName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool IsAllNodeLinked(NodeDefinition start, NodeDefinition end, bool uniquestartlink = true, bool uniqueendlink = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool IsAllNodeLinked(List<int> start, List<int> end, bool uniquestartlink = true, bool uniqueendlink = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightRequiredNodes(int startIndex, int endIndex, bool highlight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightUnwantedConnector(MENode startNode, int start, MENode endNode, int end)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsAllTrue(bool[] array)
	{
		throw null;
	}
}
