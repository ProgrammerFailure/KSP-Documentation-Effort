using System;
using System.Collections.Generic;
using Expansions.Missions;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;
using UnityEngine.UI;

public class BasicTutorial : METutorialScenario
{
	public int nodeCount;

	public const string paramValueId = "value";

	public const string orbitValueId = "ORBIT";

	public const string attendedSituationValue = "PRELAUNCH";

	public const string attendedFacilityValue = "VAB";

	public const string attendedLaunchSiteValue = "LaunchPad";

	public const string attendedOrbitValue = "Mun";

	public new const float centerPosition = 0.35f;

	public override void OnTutorialSetup()
	{
		base.OnTutorialSetup();
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnLeave = (Action)Delegate.Combine(instance.OnLeave, new Action(OnExitMissionEditorScreen));
		MissionEditorLogic instance2 = MissionEditorLogic.Instance;
		instance2.OnExitEditor = (Action)Delegate.Combine(instance2.OnExitEditor, new Action(OnExitCancelSave));
		MissionEditorLogic.Instance.PreventNodeDestruction = true;
		SaveOnClickOnSearchListener();
		RemoveOnClickOnSearchListener();
	}

	public override void CreateTutorialPages()
	{
		tutorialPages = new List<TutorialPage>
		{
			AddTutorialPage("welcome", "#autoLOC_8300010", "#autoLOC_8300011", OnEnterWelcomeTutorialPage),
			AddTutorialPage("creatingMission", "#autoLOC_8300010", "#autoLOC_8300041", OnEnterEmpty),
			AddTutorialPage("nodeDragging", "#autoLOC_8300010", "#autoLOC_8300012", OnEnterNodeDraggingTutorialPage),
			AddTutorialPage("setting", "#autoLOC_8300010", "#autoLOC_8300013", OnEnterSettingsTutorialPage),
			AddTutorialPage("startNode", "#autoLOC_8300010", "#autoLOC_8300014", OnEnterEmpty),
			AddTutorialPage("vessel", "#autoLOC_8300010", "#autoLOC_8300015", OnEnterChangeVesselName),
			AddTutorialPage("createVesselNode", "#autoLOC_8300010", "#autoLOC_8300016", OnEnterCreateVesselNode),
			AddTutorialPage("launchSite", "#autoLOC_8300010", "#autoLOC_8300017", OnEnterShowSituationParameter),
			AddTutorialPage("facility", "#autoLOC_8300010", "#autoLOC_8300018", OnEnterShowFacilityParameter),
			AddTutorialPage("selectLaunchSite", "#autoLOC_8300019", OnEnterSelectLaunchSite),
			AddTutorialPage("confirmLaunchSite", "#autoLOC_8300020", OnEnterConfirmLaunchSite),
			AddTutorialPage("launchSiteExplanation", "#autoLOC_8300021", OnEnterEmpty),
			AddTutorialPage("dragOrbitNode", "#autoLOC_8300022", OnEnterDragOrbitNode, TutorialButtonType.NoButton),
			AddTutorialPage("findOrbitNode", "#autoLOC_8300023", OnEnterFindOrbitNode, TutorialButtonType.NoButton),
			AddTutorialPage("showOrbitPlanetSetting", "#autoLOC_8300024", OnEnterShowOrbitPlanetSettings),
			AddTutorialPage("selectOrbitPlanet", "#autoLOC_8300025", OnEnterSelectOrbitPlanet, TutorialButtonType.NoButton),
			AddTutorialPage("confirmOrbitPlanet", "#autoLOC_8300026", OnEnterConfirmOrbitPlanet),
			AddTutorialPage("orbitNodeExplanation", "#autoLOC_8300027", OnEnterOrbitNodeExplanation),
			AddTutorialPage("linkOrbitNode", "#autoLOC_8300028", OnEnterLinkOrbitNode),
			AddTutorialPage("dragVesselNode", "#autoLOC_8300029", OnEnterDragVesselNode, TutorialButtonType.NoButton),
			AddTutorialPage("findVesselNode", "#autoLOC_8300030", OnEnterFindVesselNode, TutorialButtonType.NoButton),
			AddTutorialPage("setMunInVesselNode", "#autoLOC_8300031", OnEnterSetMunInVesselNode, TutorialButtonType.NoButton),
			AddTutorialPage("trySetMunInVesselNode", "#autoLOC_8300032", OnEnterTrySetMunInVesselNode),
			AddTutorialPage("vesselNodeExplanation", "#autoLOC_8300033", OnEnterEmpty),
			AddTutorialPage("linkVesselNode", "#autoLOC_8300034", OnEnterLinkVesselNode, TutorialButtonType.NoButton),
			AddTutorialPage("save", "#autoLOC_8300035", OnEnterSave),
			AddTutorialPage("clickSave", "#autoLOC_8300036", OnEnterClickSaveButton, TutorialButtonType.NoButton),
			AddTutorialPage("trySaving", "#autoLOC_8300037", OnEnterTrySaving),
			AddTutorialPage("missionBriefing", "#autoLOC_8300038", OnEnterEmpty, TutorialButtonType.NoButton),
			AddTutorialPage("nameMission", "#autoLOC_8300039", OnEnterNameMission, TutorialButtonType.NoButton),
			AddTutorialPage("savingDone", "#autoLOC_8300040", OnSavingDone, (TutorialButtonType)12)
		};
	}

	public void OnEnterSettingsTutorialPage(KFSMState state)
	{
		instructor.PlayEmote(instructor.anim_idle_lookAround);
	}

	public void OnEnterNodeDraggingTutorialPage(KFSMState state)
	{
		instructor.StopRepeatingEmote();
		instructor.PlayEmote(instructor.anim_true_smileB);
	}

	public void OnEnterWelcomeTutorialPage(KFSMState state)
	{
		SetEditorLock();
		MENodeCategorizer.Instance.EnableDrag(enable: false);
		instructor.StopRepeatingEmote();
	}

	public void OnEnterNodeDragging(KFSMState state)
	{
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_GIZMO_TOOLS, add: false, lockId);
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_EDIT_NAME_FIELDS, add: false, lockId);
	}

	public void OnEnterChangeVesselName(KFSMState state)
	{
		EnableAllTutorialPageButtons(enable: false);
		SetEditorLock();
		LockNodeSettings(locked: false);
		OnInitChangeVesselName();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateVesselName));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveVesselName));
	}

	public void OnUpdateVesselName()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			HideAllTutorialSelectors();
			return;
		}
		MEGUIParameter vesselSettings = GetVesselSettings();
		ShowTutorialSelection(vesselSettings);
		LockSapParameters(NodeDefinition.CreateVessel);
	}

	public void OnLeaveVesselName(KFSMState state)
	{
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateVesselName));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveVesselName));
		HighlightNode(1, active: false);
		LockNodeSettings(locked: true);
		ResetNodeSettingsMask();
	}

	public void OnInitChangeVesselName()
	{
		if (Tutorial.CurrentState is TutorialPage)
		{
			HighlightNode(1, active: true);
			MissionEditorLogic instance = MissionEditorLogic.Instance;
			instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnVesselNameSelectionChange));
			KFSMState currentState = Tutorial.CurrentState;
			currentState.OnLeave = (KFSMStateChange)Delegate.Combine(currentState.OnLeave, new KFSMStateChange(OnLeaveChangeVesselName));
		}
	}

	public void OnVesselNameSelectionChange(GameObject objectchange)
	{
		if (GetSelectedNodeIndex() == 1)
		{
			MEGUIParameter vesselSettings = GetVesselSettings();
			if (vesselSettings != null && vesselSettings is MEGUIParameterVesselSituation && (vesselSettings as MEGUIParameterVesselSituation).inputVesselName.onValueChanged.GetPersistentEventCount() == 0)
			{
				(vesselSettings as MEGUIParameterVesselSituation).inputVesselName.onValueChanged.AddListener(OnVesselValueChanged);
				MissionEditorLogic instance = MissionEditorLogic.Instance;
				instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnVesselNameSelectionChange));
			}
		}
		else
		{
			HighlightNode(1, active: true);
		}
	}

	public void OnVesselValueChanged(string newname)
	{
		MEGUIParameter vesselSettings = GetVesselSettings();
		if (vesselSettings != null && vesselSettings is MEGUIParameterVesselSituation)
		{
			(vesselSettings as MEGUIParameterVesselSituation).inputVesselName.onValueChanged.RemoveListener(OnVesselValueChanged);
		}
		HideTutorialSelection(vesselSettings);
		EnableAllTutorialPageButtons(enable: true);
	}

	public void OnLeaveChangeVesselName(KFSMState state)
	{
		HideAllTutorialSelectors();
		EnableAllTutorialPageButtons(enable: true);
		Tutorial.CurrentState.OnUpdate = null;
		Tutorial.CurrentState.OnLeave = null;
	}

	public void OnEnterCreateVesselNode(KFSMState state)
	{
		MissionEditorLogic.Instance.SimulateOnNodeClick(1);
		MEGUIParameter vesselToggleTypeParam = GetVesselToggleTypeParam();
		if ((bool)vesselToggleTypeParam)
		{
			ShowTutorialSelection(vesselToggleTypeParam);
		}
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Combine(currentState.OnLeave, new KFSMStateChange(OnLeaveCreateVesselNode));
	}

	public MEGUIParameter GetVesselToggleTypeParam()
	{
		MEGUIParameterVesselSituation mEGUIParameterVesselSituation = GetVesselSettings() as MEGUIParameterVesselSituation;
		if (mEGUIParameterVesselSituation == null)
		{
			return null;
		}
		return mEGUIParameterVesselSituation.toggleGroupVesselType.GetComponent<MEGUIParameter>();
	}

	public void OnLeaveCreateVesselNode(KFSMState kfsmState)
	{
		MEGUIParameter vesselToggleTypeParam = GetVesselToggleTypeParam();
		if ((bool)vesselToggleTypeParam)
		{
			HideTutorialSelection(vesselToggleTypeParam);
		}
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Remove(currentState.OnLeave, new KFSMStateChange(OnLeaveCreateVesselNode));
	}

	public void OnEnterSave(KFSMState state)
	{
		SetEditorLock();
	}

	public void OnEnterClickSaveButton(KFSMState state)
	{
		SetEditorLock();
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_SAVE, add: false, lockId);
		EnableAllTutorialPageButtons(enable: false);
		HighlightTitleBarSaveButton(enable: true);
		ModifySaveButtonForTutorial();
		if (Tutorial.CurrentState is TutorialPage)
		{
			MissionEditorLogic.Instance.buttonSave.onClick.AddListener(OnSave);
			MissionEditorLogic instance = MissionEditorLogic.Instance;
			instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnGameObjectClick));
		}
	}

	public void ModifySaveButtonForTutorial()
	{
		MissionEditorLogic.Instance.buttonSave.onClick.RemoveAllListeners();
		MissionEditorLogic.Instance.buttonSave.onClick.AddListener(OnTutorialSave);
	}

	public void OnTutorialSave()
	{
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_SAVE, add: true, lockId);
		Tutorial.GoToNextPage();
	}

	public void OnGameObjectClick(GameObject objectSelected)
	{
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnGameObjectClick));
		Tutorial.GoToNextPage();
	}

	public void OnSave()
	{
		HighlightTitleBarSaveButton(enable: false);
		MissionEditorLogic.Instance.buttonSave.onClick.RemoveListener(OnSave);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("NeedABriefing", "#autoLOC_8100001", "#autoLOC_8100002", UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton("#autoLOC_226975", delegate
		{
			OpeningBriefingOnOK();
		})), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = OpeningBriefingOnOK;
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.dialog.dialogRect.x = 0.1f;
		}
	}

	public void OpeningBriefingOnOK()
	{
		Tutorial.GoToNextPage();
		Localizer.OverrideMELock = true;
		MissionBriefingDialog missionBriefingDialog = MissionBriefingDialog.Display(MissionEditorLogic.Instance.EditorMission, OnBriefingOkay, OnBriefingCancel);
		Localizer.OverrideMELock = false;
		missionBriefingDialog.DisableExportButon();
	}

	public void OnBriefingOkay()
	{
		Tutorial.GoToNextPage();
	}

	public void OnBriefingCancel()
	{
		Tutorial.GoToNextPage();
	}

	public void OnEnterTrySaving(KFSMState state)
	{
		EnableAllTutorialPageButtons(enable: false);
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.dialog.dialogRect.x = 0.1f;
		}
	}

	public void HighlightTitleBarSaveButton(bool enable)
	{
		Button buttonSave = MissionEditorLogic.Instance.buttonSave;
		if ((bool)buttonSave && (bool)buttonSave.GetComponent<ButtonHighlighter>())
		{
			buttonSave.GetComponentInChildren<ButtonHighlighter>().Enable(enable);
		}
	}

	public void OnEnterNameMission(KFSMState state)
	{
		EnableAllTutorialPageButtons(enable: false);
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.dialog.dialogRect.x = 0f;
			tutorialPage.dialog.dialogRect.y = 1f;
		}
	}

	public void OnSavingDone(KFSMState state)
	{
		SetEditorLock();
	}

	public TutorialPage AddTutorialPage(string pageId, string loc, KFSMStateChange onEnterCallback, TutorialButtonType pageTypes = TutorialButtonType.Next)
	{
		TutorialPage tutorialPage = AddTutorialPage(pageId, onEnterCallback);
		tutorialPage.dialog = CreateDialog(pageTypes, pageId, loc);
		tutorialPage.dialog.dialogRect.x = 0.35f;
		return tutorialPage;
	}

	public TutorialPage AddTutorialPage(string pageId, KFSMStateChange onEnterCallback)
	{
		return CreateTutorialPage(pageId, "#autoLOC_8300010", onEnterCallback);
	}

	public new MultiOptionDialog CreateDialog(TutorialButtonType pageTypes, string pageId, string loc)
	{
		return CreateMultiButtonDialog(pageId, loc, pageTypes);
	}

	public new void OnEnterEmpty(KFSMState state)
	{
	}

	public void OnExitCancelSave()
	{
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnExitEditor = (Action)Delegate.Remove(instance.OnExitEditor, new Action(OnExitCancelSave));
		MissionEditorHistory.Clear();
	}

	public void OnExitMissionEditorScreen()
	{
		if (Tutorial.CurrentState != null)
		{
			Tutorial.CurrentState.OnLeave(null);
			Tutorial.CurrentState.OnLeave = null;
			Tutorial.CurrentState.OnUpdate = null;
		}
		MissionEditorLogic.Instance.SetLock(ControlTypes.All, add: false, lockId);
		MissionEditorLogic.Instance.PreventNodeDestruction = false;
		RestoreOnClickOnSearchListener();
		HideAllTutorialSelectors();
		EnableAllTutorialPageButtons(enable: false);
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnLeave = (Action)Delegate.Remove(instance.OnLeave, new Action(OnExitMissionEditorScreen));
		MissionEditorLogic.Instance.Unlock(lockId);
		METutorialScenario.ShowMissionPlayDialog();
	}

	public void OnEnterShowSituationParameter(KFSMState state)
	{
		HideAllTutorialSelectors();
		EnableAllTutorialPageButtons(enable: false);
		LockNodeSettings(locked: false);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateShowSituationParameter));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveShowSituationParameter));
	}

	public void OnUpdateShowSituationParameter()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			HideAllTutorialSelectors();
			return;
		}
		MEGUIParameter situationParam = GetSituationParam();
		ShowTutorialSelection(situationParam);
		EnableAllTutorialPageButtons(IsSituationSetted());
	}

	public bool IsSituationSetted()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			return false;
		}
		return GetSituationParam().GetState().GetValue("value") == "PRELAUNCH";
	}

	public void OnLeaveShowSituationParameter(KFSMState state)
	{
		EnableAllTutorialPageButtons(enable: true);
		LockNodeSettings(locked: true);
		ResetNodeSettingsMask();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateShowFacilityParameter));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveShowFacilityParameter));
		HideAllTutorialSelectors();
	}

	public void OnEnterShowFacilityParameter(KFSMState state)
	{
		HideAllTutorialSelectors();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateShowFacilityParameter));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveShowFacilityParameter));
		LockNodeSettings(locked: false);
	}

	public void OnUpdateShowFacilityParameter()
	{
		if (GetSelectedNodeIndex() == 1 && IsSituationSetted() && IsPlayerBuiltCreation())
		{
			MEGUIParameter facilityParam = GetFacilityParam();
			ShowTutorialSelection(facilityParam);
			EnableAllTutorialPageButtons(enable: true);
		}
		else
		{
			HideAllTutorialSelectors();
			EnableAllTutorialPageButtons(enable: false);
		}
	}

	public void OnLeaveShowFacilityParameter(KFSMState state)
	{
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateShowFacilityParameter));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveShowFacilityParameter));
		HideAllTutorialSelectors();
		EnableAllTutorialPageButtons(enable: true);
		LockNodeSettings(locked: true);
		ResetNodeSettingsMask();
	}

	public void OnEnterSelectLaunchSite(KFSMState state)
	{
		SetEditorLock();
		LockNodeSettings(locked: false);
		MissionEditorLogic.Instance.SimulateOnNodeClick(1);
		HideAllTutorialSelectors();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateSelectLaunchSite));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveSelectLaunchSite));
	}

	public void OnUpdateSelectLaunchSite()
	{
		if (GetSelectedNodeIndex() == 1 && IsSituationSetted() && IsPlayerBuiltCreation())
		{
			MEGUIParameter launchSiteParam = GetLaunchSiteParam();
			ShowTutorialSelection(launchSiteParam);
			EnableAllTutorialPageButtons(enable: true);
		}
		else
		{
			HideAllTutorialSelectors();
			EnableAllTutorialPageButtons(enable: false);
		}
	}

	public void OnLeaveSelectLaunchSite(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		LockNodeFilter(locked: false);
		ResetNodeSettingsMask();
		ResetNodeFilterMask();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateSelectLaunchSite));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveSelectLaunchSite));
		HideAllTutorialSelectors();
		EnableAllTutorialPageButtons(enable: true);
	}

	public void OnEnterConfirmLaunchSite(KFSMState state)
	{
		LockNodeSettings(locked: false);
		EnableAllTutorialPageButtons(enable: false);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateConfirmLaunchSite));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveConfirmLaunchSite));
		if (FacilityIsSetted())
		{
			Tutorial.GoToNextPage();
		}
	}

	public bool FacilityIsSetted()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			return false;
		}
		return GetFacilityParam().GetState().GetValue("value") == "VAB";
	}

	public bool LaunchSiteIsSetted()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			return false;
		}
		return GetLaunchSiteParam().GetState().GetValue("value") == "LaunchPad";
	}

	public void OnUpdateConfirmLaunchSite()
	{
		bool flag = FacilityIsSetted();
		bool flag2 = LaunchSiteIsSetted();
		bool flag3 = IsPlayerBuiltCreation();
		if (GetSelectedNodeIndex() != 1)
		{
			HideAllTutorialSelectors();
		}
		else if (!flag3)
		{
			HideAllTutorialSelectors();
		}
		else
		{
			ShowConfirmLaunchSiteSelector(flag, flag2);
		}
		bool enable = flag && flag2 && flag3;
		EnableAllTutorialPageButtons(enable);
	}

	public bool IsPlayerBuiltCreation()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			return false;
		}
		MEGUIParameter vesselSettings = GetVesselSettings();
		if (vesselSettings != null && vesselSettings is MEGUIParameterVesselSituation)
		{
			return (vesselSettings as MEGUIParameterVesselSituation).FieldValue.playerCreated;
		}
		return false;
	}

	public void ShowConfirmLaunchSiteSelector(bool facilityIsSetted, bool launchSiteIsSetted)
	{
		MEGUIParameter facilityParam = GetFacilityParam();
		MEGUIParameter launchSiteParam = GetLaunchSiteParam();
		if (!facilityIsSetted)
		{
			ShowTutorialSelection(facilityParam);
		}
		else
		{
			HideTutorialSelection(facilityParam);
		}
		if (!launchSiteIsSetted)
		{
			ShowTutorialSelection(launchSiteParam);
		}
		else
		{
			HideTutorialSelection(launchSiteParam);
		}
	}

	public void OnLeaveConfirmLaunchSite(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		ResetNodeSettingsMask();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateConfirmLaunchSite));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveConfirmLaunchSite));
		HideAllTutorialSelectors();
	}

	public void OnEnterDragOrbitNode(KFSMState state)
	{
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectChangeTuto12));
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateFindOrbitNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveDragOrbitNode));
	}

	public void OnSelectedGameObjectChangeTuto12(GameObject gameObjectSelected)
	{
		if (gameObjectSelected == null)
		{
			Tutorial.GoToNextPage();
			return;
		}
		MEGUINodeIcon component = gameObjectSelected.GetComponent<MEGUINodeIcon>();
		if (component == null || !CurrentNodeFilteredAreSelected(component))
		{
			Tutorial.GoToNextPage();
		}
	}

	public void OnLeaveDragOrbitNode(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		LockNodeFilter(locked: false);
		LockNodeList(locked: false);
		ResetNodeSettingsMask();
		ResetNodeFilterMask();
		ResetNodeListMask();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateFindOrbitNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveDragOrbitNode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectChangeTuto12));
	}

	public void OnEnterFindOrbitNode(KFSMState state)
	{
		DragNodeHelper(OrbitDragCriteria, "Location");
		nodeCount = MissionEditorLogic.Instance.GetNodeListCout();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateFindOrbitNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveFindOrbitNode));
	}

	public bool OrbitDragCriteria(MEGUINodeIcon meguiNodeIcon)
	{
		if (meguiNodeIcon.basicNode != null)
		{
			return meguiNodeIcon.basicNode.name == "Orbit";
		}
		return false;
	}

	public void OnUpdateFindOrbitNode()
	{
		if (CanSkipCurrentTutorialOnNodeDragEnd(nodeCount))
		{
			MENodeCategorizer.Instance.EnableDrag(enable: false);
			Tutorial.GoToNextPage();
		}
	}

	public void OnLeaveFindOrbitNode(KFSMState state)
	{
		ResetSideBarFilter();
		RemoveDragHelper();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateFindOrbitNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveFindOrbitNode));
	}

	public bool CurrentNodeFilteredAreSelected(MEGUINodeIcon nodeIconSelected)
	{
		List<string> displayedNodeNames = MissionEditorLogic.Instance.GetDisplayedNodeNames();
		int num = 0;
		while (true)
		{
			if (num < displayedNodeNames.Count)
			{
				if (displayedNodeNames[num] == nodeIconSelected.nodeText.text)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void OnEnterShowOrbitPlanetSettings(KFSMState state)
	{
		MissionEditorLogic.Instance.SimulateOnNodeClick(2);
		LockNodeSettings(locked: false);
		LockNodeList(locked: true);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateShowOrbitPlanetSettings));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveShowOrbitPlanetSettings));
	}

	public void OnUpdateShowOrbitPlanetSettings()
	{
		if (GetSelectedNodeIndex() != 2)
		{
			HideAllTutorialSelectors();
			return;
		}
		MEGUIParameter orbitCelestialBody = GetOrbitCelestialBody();
		ShowTutorialSelection(orbitCelestialBody);
	}

	public void OnLeaveShowOrbitPlanetSettings(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		LockNodeList(locked: false);
		ResetNodeSettingsMask();
		ResetNodeListMask();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateShowOrbitPlanetSettings));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveShowOrbitPlanetSettings));
		HideAllTutorialSelectors();
	}

	public void OnEnterSelectOrbitPlanet(KFSMState state)
	{
		SetEditorLock();
		LockNodeSettings(locked: false);
		ResetSideBarFilter();
		MissionEditorLogic.Instance.SimulateOnNodeClick(2);
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectChangeTutoOrbit));
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Combine(currentState.OnLeave, new KFSMStateChange(OnLeaveSelectOrbitPlanet));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnUpdate = (KFSMCallback)Delegate.Combine(currentState2.OnUpdate, new KFSMCallback(OnUpdateSelectOrbitPlanet));
		LockSapParameters(NodeDefinition.Orbit);
	}

	public void ResetSideBarFilter()
	{
		MissionEditorLogic.Instance.FilterSideBarNode(string.Empty, null);
	}

	public void OnUpdateSelectOrbitPlanet()
	{
		if (OrbitIsSetted())
		{
			Tutorial.GoToNextPage();
			return;
		}
		if (GetSelectedNodeIndex() != 2)
		{
			HideAllTutorialSelectors();
			return;
		}
		MEGUIParameter orbitCelestialBody = GetOrbitCelestialBody();
		ShowTutorialSelection(orbitCelestialBody);
	}

	public void OnSelectedGameObjectChangeTutoOrbit(GameObject gameObjectSelected)
	{
		Tutorial.GoToNextPage();
	}

	public void OnLeaveSelectOrbitPlanet(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		LockNodeList(locked: false);
		ResetNodeSettingsMask();
		ResetNodeListMask();
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectChangeTutoOrbit));
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Remove(currentState.OnLeave, new KFSMStateChange(OnLeaveSelectOrbitPlanet));
	}

	public void OnEnterConfirmOrbitPlanet(KFSMState state)
	{
		LockNodeSettings(locked: false);
		LockNodeList(locked: false);
		EnableAllTutorialPageButtons(enable: false);
		MissionEditorLogic.Instance.SimulateOnNodeClick(2);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateConfirmOrbitPlanet));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveConfirmOrbitPlanet));
	}

	public void OnUpdateConfirmOrbitPlanet()
	{
		bool flag = OrbitIsSetted();
		if (GetSelectedNodeIndex() != 2)
		{
			HideAllTutorialSelectors();
		}
		else
		{
			ShowOrbitPlanetSelector(flag);
		}
		EnableAllTutorialPageButtons(flag);
	}

	public bool OrbitIsSetted()
	{
		if (GetSelectedNodeIndex() != 2)
		{
			return false;
		}
		MEGUIParameter orbitCelestialBody = GetOrbitCelestialBody();
		if (orbitCelestialBody == null)
		{
			return false;
		}
		ConfigNode state = orbitCelestialBody.GetState();
		string text = string.Empty;
		ConfigNode node = new ConfigNode();
		if (state.TryGetNode("ORBIT", ref node))
		{
			text = new MissionOrbit(node).Body.bodyName;
		}
		return text == "Mun";
	}

	public void ShowOrbitPlanetSelector(bool orbitIsSetted)
	{
		if (GetSelectedNodeIndex() == 2)
		{
			MEGUIParameter orbitCelestialBody = GetOrbitCelestialBody();
			if (orbitIsSetted)
			{
				HideTutorialSelection(orbitCelestialBody);
			}
			else
			{
				ShowTutorialSelection(orbitCelestialBody);
			}
		}
	}

	public void OnLeaveConfirmOrbitPlanet(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		LockNodeList(locked: false);
		ResetNodeSettingsMask();
		ResetNodeListMask();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateConfirmOrbitPlanet));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveConfirmOrbitPlanet));
		HideAllTutorialSelectors();
	}

	public void OnEnterOrbitNodeExplanation(KFSMState kfsmState)
	{
		MissionEditorLogic.Instance.ScrollToParameterGroup(1);
	}

	public void OnEnterLinkOrbitNode(KFSMState state)
	{
		LockNodeList(locked: true);
		EnableAllTutorialPageButtons(enable: false);
		GetMEGUINode(0).ToggleOutputHolderHighlighter(state: true);
		GetMEGUINode(2).ToggleInputHolderHighlighter(state: true);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkOrbitNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveLinkOrbitNode));
	}

	public void OnUpdateLinkOrbitNode()
	{
		MENode mENode = GetMENode(0);
		MENode mENode2 = GetMENode(2);
		bool enable = mENode.toNodes.Contains(mENode2) && mENode2.fromNodes.Contains(mENode);
		EnableAllTutorialPageButtons(enable);
	}

	public void OnLeaveLinkOrbitNode(KFSMState kfsmState)
	{
		LockNodeList(locked: false);
		ResetNodeListMask();
		GetMEGUINode(0).ToggleOutputHolderHighlighter(state: false);
		GetMEGUINode(2).ToggleInputHolderHighlighter(state: false);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkOrbitNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveLinkOrbitNode));
	}

	public void OnEnterDragVesselNode(KFSMState state)
	{
		DragNodeHelper(VesselLandedDragCriteria, "Location");
		nodeCount = MissionEditorLogic.Instance.GetNodeListCout();
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectDuringTuto19));
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateDragVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveDragVesselNode));
	}

	public bool VesselLandedDragCriteria(MEGUINodeIcon meguiNodeIcon)
	{
		if (meguiNodeIcon.basicNode != null)
		{
			return meguiNodeIcon.basicNode.name == "Landed";
		}
		return false;
	}

	public void OnSelectedGameObjectDuringTuto19(GameObject gameObjectSelected)
	{
		if (gameObjectSelected == null)
		{
			Tutorial.GoToNextPage();
			return;
		}
		MEGUINodeIcon component = gameObjectSelected.GetComponent<MEGUINodeIcon>();
		if (component == null || !CurrentNodeFilteredAreSelected(component))
		{
			Tutorial.GoToNextPage();
		}
	}

	public void OnUpdateDragVesselNode()
	{
		if (CanSkipCurrentTutorialOnNodeDragEnd(nodeCount))
		{
			Tutorial.GoToNextPage();
		}
	}

	public void OnLeaveDragVesselNode(KFSMState kfsmState)
	{
		RemoveDragHelper();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateDragVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveDragVesselNode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectDuringTuto19));
	}

	public void OnEnterFindVesselNode(KFSMState state)
	{
		DragNodeHelper(VesselLandedDragCriteria, "Location");
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateDragVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveFindVesselNode));
	}

	public void OnLeaveFindVesselNode(KFSMState kfsmState)
	{
		ResetSideBarFilter();
		RemoveDragHelper();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateDragVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveFindVesselNode));
	}

	public void OnEnterSetMunInVesselNode(KFSMState state)
	{
		SetEditorLock();
		LockNodeSettings(locked: false);
		ResetSideBarFilter();
		MissionEditorLogic.Instance.SimulateOnNodeClick(3);
		LockSapParameters(NodeDefinition.VesselLanded);
		MissionEditorLogic.Instance.SetLock(ControlTypes.All, add: false, lockId);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateSetMunInVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveSetMunInVesselMode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectChangeTuto20));
	}

	public void OnSelectChangeTuto20(GameObject gameObjectSelected)
	{
		Tutorial.GoToNextPage();
	}

	public void OnUpdateSetMunInVesselNode()
	{
		if (VesselOrbitIsSetted())
		{
			Tutorial.GoToNextPage();
		}
		else
		{
			UpdateVesselMunParameterEnable();
		}
	}

	public void OnLeaveSetMunInVesselMode(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		LockNodeList(locked: false);
		ResetNodeSettingsMask();
		ResetNodeListMask();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateSetMunInVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveSetMunInVesselMode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectChangeTuto20));
	}

	public void OnEnterTrySetMunInVesselNode(KFSMState state)
	{
		SetEditorLock();
		LockNodeSettings(locked: false);
		ResetSideBarFilter();
		EnableAllTutorialPageButtons(enable: false);
		MissionEditorLogic.Instance.SimulateOnNodeClick(3);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateTrySetMunInVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnTryLeaveSetMunInVesselMode));
	}

	public void OnUpdateTrySetMunInVesselNode()
	{
		UpdateVesselMunParameterEnable();
	}

	public void UpdateVesselMunParameterEnable()
	{
		bool flag = VesselOrbitIsSetted();
		if (GetSelectedNodeIndex() != 3)
		{
			HideAllTutorialSelectors();
		}
		else
		{
			ShowVesselSettingTutorialSelector(flag);
		}
		EnableAllTutorialPageButtons(flag);
	}

	public bool VesselOrbitIsSetted()
	{
		if (GetSelectedNodeIndex() != 3)
		{
			return false;
		}
		MEGUIParameterCelestialBody mEGUIParameterCelestialBody = GetVesselLandedCelestialBody() as MEGUIParameterCelestialBody;
		if (mEGUIParameterCelestialBody == null)
		{
			return false;
		}
		return ((mEGUIParameterCelestialBody.FieldValue.Body == null) ? string.Empty : mEGUIParameterCelestialBody.FieldValue.Body.bodyName) == "Mun";
	}

	public void ShowVesselSettingTutorialSelector(bool orbitIsSetted)
	{
		if (GetSelectedNodeIndex() != 3)
		{
			HideAllTutorialSelectors();
			return;
		}
		MEGUIParameter vesselLandedCelestialBody = GetVesselLandedCelestialBody();
		if (orbitIsSetted)
		{
			HideTutorialSelection(vesselLandedCelestialBody);
		}
		else
		{
			ShowTutorialSelection(vesselLandedCelestialBody);
		}
	}

	public void OnTryLeaveSetMunInVesselMode(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		LockNodeList(locked: false);
		ResetNodeSettingsMask();
		ResetNodeListMask();
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateTrySetMunInVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnTryLeaveSetMunInVesselMode));
	}

	public void OnEnterLinkVesselNode(KFSMState state)
	{
		LockNodeSettings(locked: false);
		LockNodeList(locked: true);
		ResetNodeSettingsMask();
		ResetNodeListMask();
		EnableAllTutorialPageButtons(enable: false);
		GetMEGUINode(2).ToggleOutputHolderHighlighter(state: true);
		GetMEGUINode(3).ToggleInputHolderHighlighter(state: true);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveLinkVesselNode));
		LockSapParameters(NodeDefinition.Orbit);
	}

	public void OnUpdateLinkVesselNode()
	{
		MENode mENode = GetMENode(2);
		MENode mENode2 = GetMENode(3);
		if (mENode.toNodes.Contains(mENode2) && mENode2.fromNodes.Contains(mENode))
		{
			Tutorial.GoToNextPage();
		}
	}

	public void OnLeaveLinkVesselNode(KFSMState kfsmState)
	{
		LockNodeSettings(locked: false);
		LockNodeList(locked: false);
		ResetNodeSettingsMask();
		ResetNodeListMask();
		GetMEGUINode(2).ToggleOutputHolderHighlighter(state: false);
		GetMEGUINode(3).ToggleInputHolderHighlighter(state: false);
		KFSMState currentState = Tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkVesselNode));
		KFSMState currentState2 = Tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveLinkVesselNode));
	}

	public override void OnDoneButtonClick()
	{
		complete = true;
		MissionEditorLogic.Instance.OnExit();
		base.OnDoneButtonClick();
	}

	public override void OnContinueButtonClick()
	{
		MissionEditorLogic.Instance.Unlock(lockId);
		MissionEditorLogic.Instance.SetLock(ControlTypes.All, add: false, lockId);
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnLeave = (Action)Delegate.Remove(instance.OnLeave, new Action(OnExitMissionEditorScreen));
		base.OnContinueButtonClick();
		GoToNextTutorial();
	}

	public override string GetNextTutorialName()
	{
		return "/IntermediateTutorial/";
	}

	public override void OnSave(ConfigNode node)
	{
		stateName = GetCurrentStateName();
		if (stateName != null)
		{
			node.AddValue("statename", stateName);
		}
		node.AddValue("complete", complete);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("statename"))
		{
			stateName = node.GetValue("statename");
		}
		if (node.HasValue("complete"))
		{
			complete = bool.Parse(node.GetValue("complete"));
		}
	}
}
