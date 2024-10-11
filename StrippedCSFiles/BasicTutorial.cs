using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

public class BasicTutorial : METutorialScenario
{
	private int nodeCount;

	private const string paramValueId = "value";

	private const string orbitValueId = "ORBIT";

	private const string attendedSituationValue = "PRELAUNCH";

	private const string attendedFacilityValue = "VAB";

	private const string attendedLaunchSiteValue = "LaunchPad";

	private const string attendedOrbitValue = "Mun";

	private const float centerPosition = 0.35f;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BasicTutorial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CreateTutorialPages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterSettingsTutorialPage(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterNodeDraggingTutorialPage(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterWelcomeTutorialPage(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterNodeDragging(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterChangeVesselName(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateVesselName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveVesselName(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInitChangeVesselName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselNameSelectionChange(GameObject objectchange)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselValueChanged(string newname)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveChangeVesselName(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterCreateVesselNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEGUIParameter GetVesselToggleTypeParam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveCreateVesselNode(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterSave(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterClickSaveButton(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ModifySaveButtonForTutorial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTutorialSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameObjectClick(GameObject objectSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpeningBriefingOnOK()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBriefingOkay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBriefingCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterTrySaving(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightTitleBarSaveButton(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterNameMission(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSavingDone(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private TutorialPage AddTutorialPage(string pageId, string loc, KFSMStateChange onEnterCallback, TutorialButtonType pageTypes = TutorialButtonType.Next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private TutorialPage AddTutorialPage(string pageId, KFSMStateChange onEnterCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private new MultiOptionDialog CreateDialog(TutorialButtonType pageTypes, string pageId, string loc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private new void OnEnterEmpty(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitCancelSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitMissionEditorScreen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterShowSituationParameter(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateShowSituationParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsSituationSetted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveShowSituationParameter(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterShowFacilityParameter(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateShowFacilityParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveShowFacilityParameter(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterSelectLaunchSite(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateSelectLaunchSite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveSelectLaunchSite(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterConfirmLaunchSite(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool FacilityIsSetted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool LaunchSiteIsSetted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateConfirmLaunchSite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsPlayerBuiltCreation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowConfirmLaunchSiteSelector(bool facilityIsSetted, bool launchSiteIsSetted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveConfirmLaunchSite(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterDragOrbitNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectedGameObjectChangeTuto12(GameObject gameObjectSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveDragOrbitNode(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterFindOrbitNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OrbitDragCriteria(MEGUINodeIcon meguiNodeIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateFindOrbitNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveFindOrbitNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CurrentNodeFilteredAreSelected(MEGUINodeIcon nodeIconSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterShowOrbitPlanetSettings(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateShowOrbitPlanetSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveShowOrbitPlanetSettings(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterSelectOrbitPlanet(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetSideBarFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateSelectOrbitPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectedGameObjectChangeTutoOrbit(GameObject gameObjectSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveSelectOrbitPlanet(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterConfirmOrbitPlanet(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateConfirmOrbitPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OrbitIsSetted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowOrbitPlanetSelector(bool orbitIsSetted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveConfirmOrbitPlanet(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterOrbitNodeExplanation(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterLinkOrbitNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateLinkOrbitNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveLinkOrbitNode(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterDragVesselNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool VesselLandedDragCriteria(MEGUINodeIcon meguiNodeIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectedGameObjectDuringTuto19(GameObject gameObjectSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateDragVesselNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveDragVesselNode(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterFindVesselNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveFindVesselNode(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterSetMunInVesselNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectChangeTuto20(GameObject gameObjectSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateSetMunInVesselNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveSetMunInVesselMode(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterTrySetMunInVesselNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateTrySetMunInVesselNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselMunParameterEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool VesselOrbitIsSetted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowVesselSettingTutorialSelector(bool orbitIsSetted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTryLeaveSetMunInVesselMode(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterLinkVesselNode(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateLinkVesselNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveLinkVesselNode(KFSMState kfsmState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDoneButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnContinueButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetNextTutorialName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}
}
