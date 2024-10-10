using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedTutorial : METutorialScenario
{
	public enum TutorialStep
	{
		none = -1,
		welcome,
		crewStep1_intro,
		crewStep2_clickCreateVesselNode,
		crewStep3a_clickAdd,
		crewStep3b_clickAdd,
		crewStep4a_crewEqual2,
		crewStep4b_crewEqual2,
		crewStep5_exit,
		partStep1_intro,
		partStep2a_clickRequiredPart,
		partStep2b_clickRequiredPart,
		partStep3a_selectMysteryGoo,
		partStep3b_selectMysteryGoo,
		vesselStep1_intro,
		vesselStep2a_dragVesselNode,
		vesselStep2b_dragVesselNode,
		vesselStep3a_selectVesselName,
		vesselStep3b_selectVesselName,
		vesselStep4a_dragDialogMessage,
		vesselStep4b_dragDialogMessage,
		vesselStep5a_writeMessage,
		vesselStep5b_writeMessage,
		vesselStep6a_linkVesselDestroyed,
		vesselStep6b_linkVesselDestroyed,
		vesselStep7a_drag2DialogMessage,
		vesselStep7b_drag2DialogMessage,
		vesselStep8a_linkDialogMessage,
		vesselStep8b_linkDialogMessage,
		vesselStep9a_writeMessage,
		vesselStep9b_writeMessage,
		validationStep1_intro,
		validationStep2_misisonError,
		validationStep3a_clickRedDot,
		validationStep3b_clickRedDot,
		validationStep4_markEndNode,
		validationStep5a_setAsEndNode,
		validationStep5b_setAsEndNode,
		exportingStep1_intro,
		exportingStep2a_clickExport,
		exportingStep2b_clickExport,
		exportingStep3_award,
		exportingStep4_mods,
		exportingStep5_banners,
		conclusion_exit
	}

	public new enum NodeDefinition
	{
		None = -1,
		StartNode = 0,
		CreateVessel = 1,
		VesselLandedKerbin = 8,
		VesselLandedMun = 9,
		VesselDestroyed = 10,
		DialogMessage1 = 11,
		DialogMessage2 = 12,
		DialogMessage3 = 13
	}

	public struct Typekey
	{
		public Type type;

		public string value;

		public Typekey(Type typekey, string valuekey)
		{
			type = typekey;
			value = valuekey;
		}
	}

	public const string lockIdMissionBuilder = "missionBuilder_start";

	public List<Typekey> typekeys = new List<Typekey>
	{
		new Typekey(typeof(MEGUIParameterDropdownList), "TotalCrew"),
		new Typekey(typeof(MEGUIParameterDropdownList), "Equal"),
		new Typekey(typeof(MEGUIParameterNumberRange), "2")
	};

	public int nodeCount;

	public const int HEADER_PARAMETER_GROUP = 0;

	public const int DEFAULT_PARAMETER_GROUP = 1;

	public const int MESSAGE_PARAMETER_INDEX = 1;

	public const int END_CHECKBOX_PARAMETER = 4;

	public TutorialStep currentstep;

	public string validationLockid = "missionBuilder_validation";

	public List<NodeDefinition> dialogNodes = new List<NodeDefinition>
	{
		NodeDefinition.DialogMessage1,
		NodeDefinition.DialogMessage2,
		NodeDefinition.DialogMessage3
	};

	public Dictionary<NodeDefinition, string> textNodes = new Dictionary<NodeDefinition, string>();

	public int m_TutorialShown = -1;

	public override void OnTutorialSetup()
	{
		base.OnTutorialSetup();
		MissionEditorLogic.Instance.PreventNodeDestruction = true;
		MissionEditorLogic.Instance.Unlock("missionBuilder_start");
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnLeave = (Action)Delegate.Combine(instance.OnLeave, new Action(OnExitMissionEditorScreen));
		MissionEditorLogic instance2 = MissionEditorLogic.Instance;
		instance2.OnExitEditor = (Action)Delegate.Combine(instance2.OnExitEditor, new Action(OnExitCancelSave));
		GameEvents.Mission.onMissionBriefingCreated.Add(OnMissionBriefingShow);
		SaveOnClickOnSearchListener();
		RemoveOnClickOnSearchListener();
		LockBar();
	}

	public void OnMissionBriefingShow(MissionBriefingDialog dialog)
	{
		GameEvents.Mission.onMissionBriefingCreated.Remove(OnMissionBriefingShow);
		dialog.DisableExportButon();
	}

	public override void CreateTutorialPages()
	{
		tutorialPages = new List<TutorialPage>
		{
			AddTutorialPage(TutorialStep.welcome.ToString(), "#autoLOC_8400509", "#autoLOC_8400510", base.OnEnterEmpty),
			AddTutorialPage(TutorialStep.crewStep1_intro.ToString(), "#autoLOC_8400509", "#autoLOC_8400511", base.OnEnterEmpty),
			AddTutorialPage(TutorialStep.crewStep2_clickCreateVesselNode.ToString(), "#autoLOC_8400509", "#autoLOC_8400512", OnEnterCrew),
			AddTutorialPage(TutorialStep.crewStep3a_clickAdd.ToString(), "#autoLOC_8400509", "#autoLOC_8400513", OnUpdateCrew),
			AddTutorialPage(TutorialStep.crewStep3b_clickAdd.ToString(), "#autoLOC_8400509", "#autoLOC_8400514", OnUpdateCrew),
			AddTutorialPage(TutorialStep.crewStep4a_crewEqual2.ToString(), "#autoLOC_8400509", "#autoLOC_8400515", OnUpdateCrew),
			AddTutorialPage(TutorialStep.crewStep4b_crewEqual2.ToString(), "#autoLOC_8400509", "#autoLOC_8400516", OnUpdateCrew),
			AddTutorialPage(TutorialStep.crewStep5_exit.ToString(), "#autoLOC_8400509", "#autoLOC_8400517", OnExitCrew),
			AddTutorialPage(TutorialStep.partStep1_intro.ToString(), "#autoLOC_8400509", "#autoLOC_8400518", base.OnEnterEmpty),
			AddTutorialPage(TutorialStep.partStep2a_clickRequiredPart.ToString(), "#autoLOC_8400509", "#autoLOC_8400519", OnUpdatePart),
			AddTutorialPage(TutorialStep.partStep2b_clickRequiredPart.ToString(), "#autoLOC_8400509", "#autoLOC_8400520", OnUpdatePart),
			AddTutorialPage(TutorialStep.partStep3a_selectMysteryGoo.ToString(), "#autoLOC_8400509", "#autoLOC_8400521", OnUpdatePart),
			AddTutorialPage(TutorialStep.partStep3b_selectMysteryGoo.ToString(), "#autoLOC_8400509", "#autoLOC_8400522", OnUpdatePart),
			AddTutorialPage(TutorialStep.vesselStep1_intro.ToString(), "#autoLOC_8400509", "#autoLOC_8400523", base.OnEnterEmpty),
			AddTutorialPage(TutorialStep.vesselStep2a_dragVesselNode.ToString(), "#autoLOC_8400509", "#autoLOC_8400524", OnEnterVessel, TutorialButtonType.NoButton),
			AddTutorialPage(TutorialStep.vesselStep2b_dragVesselNode.ToString(), "#autoLOC_8400509", "#autoLOC_8400525", OnEnterVessel, TutorialButtonType.NoButton),
			AddTutorialPage(TutorialStep.vesselStep3a_selectVesselName.ToString(), "#autoLOC_8400509", "#autoLOC_8400542", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep3b_selectVesselName.ToString(), "#autoLOC_8400509", "#autoLOC_8400541", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep4a_dragDialogMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400528", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep4b_dragDialogMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400529", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep5a_writeMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400543", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep5b_writeMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400544", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep6a_linkVesselDestroyed.ToString(), "#autoLOC_8400509", "#autoLOC_8400533", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep6b_linkVesselDestroyed.ToString(), "#autoLOC_8400509", "#autoLOC_8400534", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep7a_drag2DialogMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400535", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep7b_drag2DialogMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400536", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep8a_linkDialogMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400537", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep8b_linkDialogMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400538", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep9a_writeMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400539", OnEnterVessel),
			AddTutorialPage(TutorialStep.vesselStep9b_writeMessage.ToString(), "#autoLOC_8400509", "#autoLOC_8400540", OnEnterVessel),
			AddTutorialPage(TutorialStep.validationStep1_intro.ToString(), "#autoLOC_8400509", "#autoLOC_8400545", OnEnterValidation),
			AddTutorialPage(TutorialStep.validationStep2_misisonError.ToString(), "#autoLOC_8400509", "#autoLOC_8400546", base.OnEnterEmpty),
			AddTutorialPage(TutorialStep.validationStep3a_clickRedDot.ToString(), "#autoLOC_8400509", "#autoLOC_8400547", OnEnterValidation),
			AddTutorialPage(TutorialStep.validationStep3b_clickRedDot.ToString(), "#autoLOC_8400509", "#autoLOC_8400548", OnEnterValidation),
			AddTutorialPage(TutorialStep.validationStep4_markEndNode.ToString(), "#autoLOC_8400509", "#autoLOC_8400549", OnEnterValidation),
			AddTutorialPage(TutorialStep.validationStep5a_setAsEndNode.ToString(), "#autoLOC_8400509", "#autoLOC_8400550", OnEnterValidation),
			AddTutorialPage(TutorialStep.validationStep5b_setAsEndNode.ToString(), "#autoLOC_8400509", "#autoLOC_8400551", OnEnterValidation),
			AddTutorialPage(TutorialStep.exportingStep1_intro.ToString(), "#autoLOC_8400509", "#autoLOC_8400552", OnEnterExport),
			AddTutorialPage(TutorialStep.exportingStep2a_clickExport.ToString(), "#autoLOC_8400509", "#autoLOC_8400553", OnEnterExport),
			AddTutorialPage(TutorialStep.exportingStep2b_clickExport.ToString(), "#autoLOC_8400509", "#autoLOC_8400554", OnEnterExport),
			AddTutorialPage(TutorialStep.exportingStep3_award.ToString(), "#autoLOC_8400509", "#autoLOC_8400555", OnEnterExport),
			AddTutorialPage(TutorialStep.exportingStep4_mods.ToString(), "#autoLOC_8400509", "#autoLOC_8400556", OnEnterExport),
			AddTutorialPage(TutorialStep.exportingStep5_banners.ToString(), "#autoLOC_8400509", "#autoLOC_8400557", OnEnterExport),
			AddTutorialPage(TutorialStep.conclusion_exit.ToString(), "#autoLOC_8400509", "#autoLOC_8400532", OnEnterConclusion, TutorialButtonType.Done)
		};
	}

	public void OnExitMissionEditorScreen()
	{
		MissionEditorLogic.Instance.PreventNodeDestruction = false;
		if (Tutorial.CurrentState != null)
		{
			Tutorial.CurrentState.OnLeave(null);
			Tutorial.CurrentState.OnLeave = null;
			Tutorial.CurrentState.OnUpdate = null;
		}
		MissionEditorLogic.Instance.SetLock(ControlTypes.All, add: false, lockId);
		MissionEditorLogic.Instance.Unlock(lockId);
		RestoreOnClickOnSearchListener();
		EnableAllTutorialPageButtons(enable: false);
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnLeave = (Action)Delegate.Remove(instance.OnLeave, new Action(OnExitMissionEditorScreen));
		GameEvents.Mission.onMissionBriefingCreated.Remove(OnMissionBriefingShow);
		METutorialScenario.ShowMissionPlayDialog();
	}

	public void OnExitCancelSave()
	{
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnExitEditor = (Action)Delegate.Remove(instance.OnExitEditor, new Action(OnExitCancelSave));
		MissionEditorHistory.Clear();
	}

	public void LockBar()
	{
		MissionEditorLogic.Instance.SetLock(ControlTypes.All, add: true, lockId);
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_EXIT, add: false, lockId);
	}

	public void ArrangeGraph()
	{
		MissionEditorLogic.Instance.ArrangeGraphNodes();
	}

	public override void OnDoneButtonClick()
	{
		complete = true;
		MissionBriefingDialog.Hide(MissionEditorLogic.Instance.EditorMission);
		MissionEditorLogic.Instance.OnExit();
		base.OnDoneButtonClick();
	}

	public void OnEnterCrew(KFSMState state)
	{
		currentstep = GetCurrentTutorialStep();
		EnableAllTutorialPageButtons(enable: false);
		OnInitCreateVesel();
		SetEditorLock();
		LockNodeSettings(locked: false);
	}

	public void OnInitCreateVesel()
	{
		if (Tutorial.CurrentState is TutorialPage)
		{
			HighlightNode(1, active: true);
			MissionEditorLogic instance = MissionEditorLogic.Instance;
			instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedNodeChange));
		}
	}

	public void OnSelectedNodeChange(GameObject objectchange)
	{
		MEGUINode component = objectchange.GetComponent<MEGUINode>();
		if (component != null && component.Node != null)
		{
			if (component.Node.basicNodeSource == "CreateVessel")
			{
				HighlightNode(1, active: false);
				MissionEditorLogic instance = MissionEditorLogic.Instance;
				instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedNodeChange));
				Tutorial.GoToNextPage();
			}
			else
			{
				HighlightNode(1, active: true);
			}
		}
	}

	public void OnUpdateCrew(KFSMState state)
	{
		currentstep = GetCurrentTutorialStep();
		EnableAllTutorialPageButtons(enable: false);
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Combine(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateSelectedCrewNodeChange));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Combine(tutorialPage.OnLeave, new KFSMStateChange(OnLeaveCrew));
		}
	}

	public void OnUpdateSelectedCrewNodeChange()
	{
		if (GetSelectedNodeIndex() == 1 && Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Remove(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateSelectedCrewNodeChange));
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Combine(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateCrew));
		}
	}

	public void OnUpdateCrew()
	{
		if (GetSelectedNodeIndex() == 1)
		{
			int count = 1;
			if (currentstep == TutorialStep.crewStep4a_crewEqual2 || currentstep == TutorialStep.crewStep4b_crewEqual2)
			{
				count = typekeys.Count;
			}
			bool flag = IsParametersCorrect(GetCurrentNodeParameters(1), typekeys.GetRange(0, count));
			EnableAllTutorialPageButtons(flag);
			if ((flag && currentstep == TutorialStep.crewStep3b_clickAdd) || (flag && currentstep == TutorialStep.crewStep4b_crewEqual2))
			{
				Tutorial.GoToNextPage();
			}
		}
		OnUpdateCrewErrors();
	}

	public void OnExitCrew(KFSMState state)
	{
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Remove(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateCrew));
		}
	}

	public void OnLeaveCrew(KFSMState state)
	{
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Remove(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateSelectedCrewNodeChange));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Remove(tutorialPage.OnLeave, new KFSMStateChange(OnLeaveCrew));
		}
	}

	public void OnUpdateCrewErrors()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			MissionEditorLogic.Instance.SimulateOnNodeClick(1);
			if (currentstep == TutorialStep.crewStep3a_clickAdd || currentstep == TutorialStep.crewStep4a_crewEqual2)
			{
				Tutorial.GoToNextPage();
			}
		}
	}

	public void OnUpdatePart(KFSMState state)
	{
		currentstep = GetCurrentTutorialStep();
		EnableAllTutorialPageButtons(enable: false);
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Combine(tutorialPage.OnUpdate, new KFSMCallback(OnUpdatePart));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Combine(tutorialPage.OnLeave, new KFSMStateChange(OnLeavePart));
		}
		OnEnterPart();
	}

	public void OnEnterPart()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			return;
		}
		ShowTutorialSelection(GetPartPickerParam());
		if (currentstep == TutorialStep.partStep2b_clickRequiredPart && GetPartPickerParam().IsSelected)
		{
			Tutorial.GoToNextPage();
		}
		if (currentstep == TutorialStep.partStep3b_selectMysteryGoo)
		{
			MEGUIParameterPartPicker mEGUIParameterPartPicker = GetPartPickerParam() as MEGUIParameterPartPicker;
			bool flag = false;
			if (mEGUIParameterPartPicker != null)
			{
				flag = mEGUIParameterPartPicker.FieldValue.Count == 1 && mEGUIParameterPartPicker.FieldValue[0] == "GooExperiment";
			}
			if (flag)
			{
				Tutorial.GoToNextPage();
			}
		}
	}

	public void OnUpdatePart()
	{
		if (GetSelectedNodeIndex() == 1)
		{
			if (currentstep == TutorialStep.partStep2a_clickRequiredPart || currentstep == TutorialStep.partStep2b_clickRequiredPart)
			{
				EnableAllTutorialPageButtons(GetPartPickerParam().IsSelected);
			}
			if (currentstep == TutorialStep.partStep3a_selectMysteryGoo || currentstep == TutorialStep.partStep3b_selectMysteryGoo)
			{
				MEGUIParameterPartPicker mEGUIParameterPartPicker = GetPartPickerParam() as MEGUIParameterPartPicker;
				bool enable = false;
				if (mEGUIParameterPartPicker != null)
				{
					enable = mEGUIParameterPartPicker.FieldValue.Count == 1 && mEGUIParameterPartPicker.FieldValue[0] == "GooExperiment";
				}
				EnableAllTutorialPageButtons(enable);
			}
		}
		OnUpdatePartError();
	}

	public void OnUpdatePartError()
	{
		if (GetSelectedNodeIndex() != 1)
		{
			MissionEditorLogic.Instance.SimulateOnNodeClick(1);
			if (currentstep == TutorialStep.partStep2a_clickRequiredPart || currentstep == TutorialStep.partStep3a_selectMysteryGoo)
			{
				Tutorial.GoToNextPage();
			}
		}
		else
		{
			if (currentstep != TutorialStep.partStep3a_selectMysteryGoo && currentstep != TutorialStep.partStep3b_selectMysteryGoo)
			{
				return;
			}
			MEGUIParameterPartPicker mEGUIParameterPartPicker = GetPartPickerParam() as MEGUIParameterPartPicker;
			if (mEGUIParameterPartPicker != null && (mEGUIParameterPartPicker.FieldValue.Count > 1 || (mEGUIParameterPartPicker.FieldValue.Count == 1 && mEGUIParameterPartPicker.FieldValue[0] != "GooExperiment")))
			{
				if (currentstep == TutorialStep.partStep3a_selectMysteryGoo)
				{
					Tutorial.GoToNextPage();
				}
				mEGUIParameterPartPicker.FieldValue.Clear();
				mEGUIParameterPartPicker.RefreshUI();
				mEGUIParameterPartPicker.UpdateDisplayedParts();
			}
		}
	}

	public void OnLeavePart(KFSMState state)
	{
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Remove(tutorialPage.OnUpdate, new KFSMCallback(OnUpdatePart));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Remove(tutorialPage.OnLeave, new KFSMStateChange(OnLeavePart));
		}
	}

	public void OnEnterVessel(KFSMState state)
	{
		currentstep = GetCurrentTutorialStep();
		EnableAllTutorialPageButtons(enable: false);
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Combine(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateVessel));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Combine(tutorialPage.OnLeave, new KFSMStateChange(OnLeaveVessel));
		}
		OnEnterVessel();
	}

	public void OnEnterVessel()
	{
		EnterVesselDrag();
		EnterVesselName();
		EnterVesselWriteMain();
		EnterVesselWriteError();
		EnterVesselLink();
		EnterVesselMessage();
	}

	public void OnUpdateVessel()
	{
		UpdateVesselDrag();
		UpdateVesselSelectName();
		UpdateVesselLink();
		UpdateVesselError();
	}

	public void UpdateVesselError()
	{
		if (currentstep != TutorialStep.vesselStep3a_selectVesselName && currentstep != TutorialStep.vesselStep3b_selectVesselName)
		{
			if (currentstep != TutorialStep.vesselStep5a_writeMessage && currentstep != TutorialStep.vesselStep5b_writeMessage)
			{
				if (currentstep != TutorialStep.vesselStep9a_writeMessage && currentstep != TutorialStep.vesselStep9b_writeMessage)
				{
					return;
				}
				int selectedNodeIndex = GetSelectedNodeIndex();
				bool flag = IsTextValueValid(NodeDefinition.DialogMessage2);
				bool flag2 = IsTextValueValid(NodeDefinition.DialogMessage3);
				if (!flag || !flag2)
				{
					int num = -1;
					if (!flag && selectedNodeIndex == 12)
					{
						num = 12;
					}
					else if (!flag2 && selectedNodeIndex == 13)
					{
						num = 13;
					}
					if (num != -1 && m_TutorialShown != num)
					{
						m_TutorialShown = num;
						GetMessageParameter((NodeDefinition)num);
					}
				}
			}
			else if (GetSelectedNodeIndex() != 11)
			{
				MissionEditorLogic.Instance.SimulateOnNodeClick(11);
				MEGUIParameterTextArea messageParameter = GetMessageParameter((NodeDefinition)GetSelectedNodeIndex());
				if (messageParameter != null)
				{
					ShowTutorialSelection(messageParameter);
				}
				if (currentstep == TutorialStep.vesselStep5a_writeMessage)
				{
					Tutorial.GoToNextPage();
				}
			}
		}
		else if (GetSelectedNodeIndex() != 10)
		{
			MissionEditorLogic.Instance.SimulateOnNodeClick(10);
			ShowTutorialSelection(GetVesselDestroyedVesselParam());
			if (currentstep == TutorialStep.vesselStep3a_selectVesselName)
			{
				Tutorial.GoToNextPage();
			}
		}
	}

	public void OnLeaveVessel(KFSMState state)
	{
		if (currentstep == TutorialStep.vesselStep2b_dragVesselNode || currentstep == TutorialStep.vesselStep4b_dragDialogMessage || currentstep == TutorialStep.vesselStep7b_drag2DialogMessage)
		{
			RemoveDrag();
			if (currentstep == TutorialStep.vesselStep2b_dragVesselNode)
			{
				MissionEditorLogic.Instance.SimulateOnNodeClick(10);
				ShowTutorialSelection(GetVesselDestroyedVesselParam());
			}
			else if (currentstep == TutorialStep.vesselStep4b_dragDialogMessage)
			{
				MissionEditorLogic.Instance.SimulateOnNodeClick(11);
				MEGUIParameterTextArea messageParameter = GetMessageParameter((NodeDefinition)GetSelectedNodeIndex());
				if (messageParameter != null)
				{
					ShowTutorialSelection(messageParameter);
				}
			}
			else if (currentstep == TutorialStep.vesselStep7b_drag2DialogMessage)
			{
				MissionEditorLogic.Instance.SimulateOnNodeClick(12);
			}
		}
		if (currentstep == TutorialStep.vesselStep5b_writeMessage || currentstep == TutorialStep.vesselStep9b_writeMessage)
		{
			List<NodeDefinition> list = GetTextNodes();
			for (int i = 0; i < list.Count; i++)
			{
				MEGUIParameterTextArea messageParameter2 = GetMessageParameter(list[i]);
				if (messageParameter2 != null)
				{
					messageParameter2.inputField.onValueChanged.RemoveListener(OnTextValueChanged);
				}
				HideTutorialSelection(messageParameter2);
			}
			textNodes.Clear();
		}
		if (currentstep == TutorialStep.vesselStep8b_linkDialogMessage)
		{
			List<int> start = new List<int>();
			List<int> end = new List<int>();
			GetLinkNodes(ref start, ref end);
			for (int j = 0; j < start.Count; j++)
			{
				GetMEGUINode(start[j]).ToggleOutputHolderHighlighter(state: false);
				GetMEGUINode(end[j]).ToggleInputHolderHighlighter(state: false);
			}
		}
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Remove(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateVessel));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Remove(tutorialPage.OnLeave, new KFSMStateChange(OnLeaveVessel));
		}
	}

	public void EnterVesselDrag()
	{
		if (IsDragMainStep())
		{
			string filterednode = "";
			string highlightCategory = "";
			if (currentstep == TutorialStep.vesselStep2a_dragVesselNode)
			{
				highlightCategory = "Vessel";
				filterednode = "Crashed";
			}
			else if (currentstep == TutorialStep.vesselStep4a_dragDialogMessage || currentstep == TutorialStep.vesselStep7a_drag2DialogMessage)
			{
				highlightCategory = "Utility";
				filterednode = "DialogMessage";
			}
			OnEnterDragStep(filterednode, highlightCategory);
		}
	}

	public void EnterVesselName()
	{
		if (currentstep == TutorialStep.vesselStep3b_selectVesselName)
		{
			MEGUIParameterVesselDropdownList vesselDropDown = GetVesselDropDown();
			if (vesselDropDown != null && vesselDropDown.FieldValue > 0)
			{
				Tutorial.GoToNextPage();
			}
		}
	}

	public void EnterVesselWriteMain()
	{
		if (!IsWriteMainStep())
		{
			return;
		}
		List<NodeDefinition> list = GetTextNodes();
		for (int i = 0; i < list.Count; i++)
		{
			MEGUIParameterTextArea messageParameter = GetMessageParameter(list[i]);
			if (messageParameter != null)
			{
				messageParameter.inputField.onValueChanged.AddListener(OnTextValueChanged);
			}
		}
		textNodes.Clear();
	}

	public void EnterVesselWriteError()
	{
		if (!IsWriteErrorStep())
		{
			return;
		}
		List<NodeDefinition> list = GetTextNodes();
		if (IsAllTextValueValid(list))
		{
			Tutorial.GoToNextPage();
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			MEGUIParameterTextArea messageParameter = GetMessageParameter(list[i]);
			if (messageParameter != null)
			{
				messageParameter.inputField.onValueChanged.AddListener(OnTextValueChanged);
			}
		}
		textNodes.Clear();
	}

	public void EnterVesselLink()
	{
		if ((IsLinkMainStep() || IsLinkErrorStep()) && IsLinkErrorStep())
		{
			List<int> start = new List<int>();
			List<int> end = new List<int>();
			GetLinkNodes(ref start, ref end);
			if (IsAllNodeLinked(start, end, uniquestartlink: false))
			{
				Tutorial.GoToNextPage();
			}
		}
	}

	public void EnterVesselMessage()
	{
		if (currentstep != TutorialStep.vesselStep9a_writeMessage && currentstep != TutorialStep.vesselStep9b_writeMessage)
		{
			return;
		}
		List<NodeDefinition> list = GetTextNodes();
		foreach (NodeDefinition item in list)
		{
			MEGUIParameterTextArea messageParameter = GetMessageParameter(item);
			if (messageParameter != null)
			{
				textNodes[item] = messageParameter.inputField.text;
			}
		}
		if (IsAllTextValueValid(list))
		{
			EnableAllTutorialPageButtons(enable: true);
		}
	}

	public void OnEnterDragStep(string filterednode, string highlightCategory)
	{
		Func<MEGUINodeIcon, bool> filterCriteria = (MEGUINodeIcon icon) => icon.basicNode != null && icon.basicNode.name == filterednode;
		DragNodeHelper(filterCriteria, highlightCategory);
		EnableAllTutorialPageButtons(enable: false);
		OnInitCreateVesel();
		SetEditorLock();
		LockNodeSettings(locked: false);
		nodeCount = MissionEditorLogic.Instance.GetNodeListCout();
	}

	public void UpdateVesselDrag()
	{
		if (IsDragMainStep() || IsDragErrorStep())
		{
			int nodeListCout = MissionEditorLogic.Instance.GetNodeListCout();
			int num = 1;
			if (currentstep == TutorialStep.vesselStep7a_drag2DialogMessage || currentstep == TutorialStep.vesselStep7b_drag2DialogMessage)
			{
				num = 2;
			}
			if (nodeListCout == nodeCount + num && MissionEditorLogic.Instance.CurrentSelectedNode != null)
			{
				Tutorial.GoToNextPage();
			}
		}
	}

	public void UpdateVesselSelectName()
	{
		if (currentstep == TutorialStep.vesselStep3a_selectVesselName || currentstep == TutorialStep.vesselStep3b_selectVesselName)
		{
			MEGUIParameterVesselDropdownList vesselDropDown = GetVesselDropDown();
			EnableAllTutorialPageButtons(vesselDropDown != null && vesselDropDown.FieldValue > 0);
		}
	}

	public void UpdateVesselLink()
	{
		if (IsLinkMainStep() || IsLinkErrorStep())
		{
			List<int> start = new List<int>();
			List<int> end = new List<int>();
			GetLinkNodes(ref start, ref end);
			EnableAllTutorialPageButtons(IsAllNodeLinked(start, end, uniquestartlink: false));
		}
	}

	public bool IsDragMainStep()
	{
		if (currentstep != TutorialStep.vesselStep2a_dragVesselNode && currentstep != TutorialStep.vesselStep4a_dragDialogMessage)
		{
			return currentstep == TutorialStep.vesselStep7a_drag2DialogMessage;
		}
		return true;
	}

	public bool IsDragErrorStep()
	{
		if (currentstep != TutorialStep.vesselStep2b_dragVesselNode && currentstep != TutorialStep.vesselStep4b_dragDialogMessage)
		{
			return currentstep == TutorialStep.vesselStep7b_drag2DialogMessage;
		}
		return true;
	}

	public bool IsLinkMainStep()
	{
		if (currentstep != TutorialStep.vesselStep6a_linkVesselDestroyed)
		{
			return currentstep == TutorialStep.vesselStep8a_linkDialogMessage;
		}
		return true;
	}

	public bool IsLinkErrorStep()
	{
		if (currentstep != TutorialStep.vesselStep6b_linkVesselDestroyed)
		{
			return currentstep == TutorialStep.vesselStep8b_linkDialogMessage;
		}
		return true;
	}

	public bool IsWriteMainStep()
	{
		if (currentstep != TutorialStep.vesselStep5a_writeMessage)
		{
			return currentstep == TutorialStep.vesselStep9a_writeMessage;
		}
		return true;
	}

	public bool IsWriteErrorStep()
	{
		if (currentstep != TutorialStep.vesselStep5b_writeMessage)
		{
			return currentstep == TutorialStep.vesselStep9b_writeMessage;
		}
		return true;
	}

	public void OnSideBarNodeFiltered()
	{
		MissionEditorLogic.Instance.HighLightDisplayedNode(isActive: true);
	}

	public MEGUIParameterVesselDropdownList GetVesselDropDown()
	{
		return GetVesselDestroyedVesselParam() as MEGUIParameterVesselDropdownList;
	}

	public MEGUIParameterTextArea GetMessageParameter(NodeDefinition node)
	{
		int selectedNodeIndex = GetSelectedNodeIndex();
		MissionEditorLogic.Instance.SimulateOnNodeClick((int)node);
		MEGUIParameter[] currentNodeParameters = GetCurrentNodeParameters(1);
		MissionEditorLogic.Instance.SimulateOnNodeClick(selectedNodeIndex);
		if (1 >= currentNodeParameters.Length)
		{
			return null;
		}
		ShowTutorialSelection(currentNodeParameters[1]);
		return currentNodeParameters[1] as MEGUIParameterTextArea;
	}

	public void OnTextValueChanged(string newname)
	{
		NodeDefinition selectedNodeIndex = (NodeDefinition)GetSelectedNodeIndex();
		textNodes[selectedNodeIndex] = newname;
		EnableAllTutorialPageButtons(IsAllTextValueValid(GetTextNodes()));
	}

	public List<NodeDefinition> GetTextNodes()
	{
		List<NodeDefinition> list = new List<NodeDefinition>();
		if (currentstep != TutorialStep.vesselStep5a_writeMessage && currentstep != TutorialStep.vesselStep5b_writeMessage)
		{
			if (currentstep == TutorialStep.vesselStep9a_writeMessage || currentstep == TutorialStep.vesselStep9b_writeMessage)
			{
				list.Add(NodeDefinition.DialogMessage2);
				list.Add(NodeDefinition.DialogMessage3);
			}
		}
		else
		{
			list.Add(NodeDefinition.DialogMessage1);
		}
		return list;
	}

	public void GetLinkNodes(ref List<int> start, ref List<int> end)
	{
		if (currentstep != TutorialStep.vesselStep6a_linkVesselDestroyed && currentstep != TutorialStep.vesselStep6b_linkVesselDestroyed)
		{
			if ((currentstep == TutorialStep.vesselStep8a_linkDialogMessage) | (currentstep == TutorialStep.vesselStep8b_linkDialogMessage))
			{
				start.Add(8);
				start.Add(9);
				end.Add(12);
				end.Add(13);
			}
		}
		else
		{
			start.Add(10);
			end.Add(11);
		}
	}

	public bool IsAllTextValueValid(List<NodeDefinition> nodes)
	{
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < nodes.Count)
			{
				NodeDefinition key = nodes[num2];
				if (textNodes.ContainsKey(key) && textNodes[key].Length > 0)
				{
					num++;
					if (num == nodes.Count)
					{
						break;
					}
				}
				num2++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool IsTextValueValid(NodeDefinition node)
	{
		if (textNodes.ContainsKey(node))
		{
			return textNodes[node].Length > 0;
		}
		return false;
	}

	public void OnEnterValidation(KFSMState state)
	{
		currentstep = GetCurrentTutorialStep();
		EnableAllTutorialPageButtons(enable: false);
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Combine(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateValidation));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Combine(tutorialPage.OnLeave, new KFSMStateChange(OnLeaveValidation));
		}
		OnEnterValidation();
	}

	public void OnEnterValidation()
	{
		if (currentstep == TutorialStep.validationStep1_intro)
		{
			EnableAllTutorialPageButtons(enable: true);
			MissionEditorLogic.Instance.RunValidator();
		}
		if (currentstep == TutorialStep.validationStep3b_clickRedDot && IsValidationDialogOpen())
		{
			Tutorial.GoToNextPage();
		}
		if (currentstep == TutorialStep.validationStep4_markEndNode || currentstep == TutorialStep.validationStep5a_setAsEndNode || currentstep == TutorialStep.validationStep5b_setAsEndNode)
		{
			if (currentstep == TutorialStep.validationStep4_markEndNode)
			{
				EnableAllTutorialPageButtons(enable: true);
			}
			if (currentstep == TutorialStep.validationStep5a_setAsEndNode || currentstep == TutorialStep.validationStep5b_setAsEndNode)
			{
				AddEndNodeListener();
				MissionEditorLogic.Instance.SimulateOnNodeClick(11);
				if (IsAllDialogNodeEnd() && currentstep == TutorialStep.validationStep5b_setAsEndNode)
				{
					Tutorial.GoToNextPage();
				}
			}
			SetPagePositionTopLeft();
		}
		if (currentstep == TutorialStep.validationStep3a_clickRedDot || currentstep == TutorialStep.validationStep3b_clickRedDot)
		{
			RemoveDrag();
			MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_EDIT_NAME_FIELDS, add: false, lockId);
		}
		if (currentstep == TutorialStep.validationStep5a_setAsEndNode)
		{
			MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_EDIT_NAME_FIELDS, add: true, lockId);
		}
	}

	public void OnUpdateValidation()
	{
		UpdateValidationClickReport();
		UpdateValidationError();
	}

	public void UpdateValidationError()
	{
		if (currentstep == TutorialStep.validationStep5a_setAsEndNode)
		{
			NodeDefinition selectedNodeIndex = (NodeDefinition)GetSelectedNodeIndex();
			if (selectedNodeIndex != NodeDefinition.DialogMessage1 || selectedNodeIndex != NodeDefinition.DialogMessage2 || selectedNodeIndex != NodeDefinition.DialogMessage3)
			{
				Tutorial.GoToNextPage();
			}
		}
	}

	public void OnLeaveValidation(KFSMState state)
	{
		if (currentstep == TutorialStep.validationStep5b_setAsEndNode)
		{
			HighlightNode(11, active: false);
			HighlightNode(12, active: false);
			HighlightNode(13, active: false);
			for (int i = 0; i < dialogNodes.Count; i++)
			{
				MEGUIParameterCheckbox nodeEndParameter = GetNodeEndParameter(dialogNodes[i]);
				if (nodeEndParameter != null)
				{
					nodeEndParameter.toggle.onValueChanged.RemoveListener(OnCheckboxValueChanged);
				}
			}
		}
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Remove(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateValidation));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Remove(tutorialPage.OnLeave, new KFSMStateChange(OnLeaveValidation));
		}
	}

	public void UpdateValidationClickReport()
	{
		if (currentstep == TutorialStep.validationStep3a_clickRedDot || currentstep == TutorialStep.validationStep3b_clickRedDot)
		{
			MissionEditorLogic.Instance.buttonTS.interactable = false;
			if (IsValidationDialogOpen())
			{
				Tutorial.GoToNextPage();
			}
		}
	}

	public void AddEndNodeListener()
	{
		HighlightNode(11, active: true);
		HighlightNode(12, active: true);
		HighlightNode(13, active: true);
		for (int i = 0; i < dialogNodes.Count; i++)
		{
			MEGUIParameterCheckbox nodeEndParameter = GetNodeEndParameter(dialogNodes[i]);
			if (nodeEndParameter != null)
			{
				nodeEndParameter.toggle.onValueChanged.AddListener(OnCheckboxValueChanged);
			}
		}
	}

	public void OnCheckboxValueChanged(bool newvalue)
	{
		EnableAllTutorialPageButtons(IsAllDialogNodeEnd());
	}

	public bool IsAllDialogNodeEnd()
	{
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < dialogNodes.Count)
			{
				MEGUIParameterCheckbox nodeEndParameter = GetNodeEndParameter(dialogNodes[num2]);
				if (!(nodeEndParameter != null) || !nodeEndParameter.toggle.isOn)
				{
					break;
				}
				num++;
				if (num != dialogNodes.Count)
				{
					num2++;
					continue;
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public MEGUIParameterCheckbox GetNodeEndParameter(NodeDefinition node)
	{
		int selectedNodeIndex = GetSelectedNodeIndex();
		MissionEditorLogic.Instance.SimulateOnNodeClick((int)node);
		MEGUIParameter[] currentNodeParameters = GetCurrentNodeParameters(0);
		MissionEditorLogic.Instance.SimulateOnNodeClick(selectedNodeIndex);
		if (4 > currentNodeParameters.Length - 1)
		{
			return null;
		}
		ShowTutorialSelection(currentNodeParameters[4]);
		return currentNodeParameters[4] as MEGUIParameterCheckbox;
	}

	public bool IsValidationDialogOpen()
	{
		return (InputLockManager.GetControlLock(validationLockid) & ControlTypes.EDITOR_UI_TOPRIGHT) == ControlTypes.EDITOR_UI_TOPRIGHT;
	}

	public void OnEnterExport(KFSMState state)
	{
		currentstep = GetCurrentTutorialStep();
		EnableAllTutorialPageButtons(enable: false);
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Combine(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateExport));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Combine(tutorialPage.OnLeave, new KFSMStateChange(OnLeaveExport));
		}
		OnEnterExport();
	}

	public void OnEnterExport()
	{
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_LAUNCH, add: false, lockId);
		HighlightTitleBarExportButton(enable: true);
		SetPagePositionTopLeft();
		if (currentstep == TutorialStep.exportingStep3_award || currentstep == TutorialStep.exportingStep4_mods || currentstep == TutorialStep.exportingStep5_banners)
		{
			EnableAllTutorialPageButtons(enable: true);
		}
	}

	public void OnLeaveExport(KFSMState state)
	{
		HighlightTitleBarExportButton(enable: false);
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.OnUpdate = (KFSMCallback)Delegate.Remove(tutorialPage.OnUpdate, new KFSMCallback(OnUpdateExport));
			tutorialPage.OnLeave = (KFSMStateChange)Delegate.Remove(tutorialPage.OnLeave, new KFSMStateChange(OnLeaveExport));
		}
	}

	public void OnUpdateExport()
	{
		if ((currentstep == TutorialStep.exportingStep1_intro || currentstep == TutorialStep.exportingStep2a_clickExport || currentstep == TutorialStep.exportingStep2b_clickExport) && IsExportDialogOpen())
		{
			Tutorial.GoToNextPage();
		}
		SetTestButtonInteractibility(interactable: false);
	}

	public void HighlightTitleBarExportButton(bool enable)
	{
		Button buttonExport = MissionEditorLogic.Instance.buttonExport;
		if ((bool)buttonExport && (bool)buttonExport.GetComponent<ButtonHighlighter>())
		{
			buttonExport.GetComponentInChildren<ButtonHighlighter>().Enable(enable);
		}
	}

	public bool IsExportDialogOpen()
	{
		return (InputLockManager.GetControlLock("missionBuilder_export") & ControlTypes.EDITOR_UI_TOPRIGHT) == ControlTypes.EDITOR_UI_TOPRIGHT;
	}

	public void SetTestButtonInteractibility(bool interactable)
	{
		MissionEditorLogic.Instance.buttonTest.interactable = interactable;
	}

	public void OnEnterConclusion(KFSMState aState)
	{
		MissionEditorLogic.Instance.SetLock(ControlTypes.EDITOR_LAUNCH, add: true, lockId);
	}

	public MEGUIParameter[] GetCurrentNodeParameters(int parametergroup)
	{
		Transform contentRoot = MissionEditorLogic.Instance.actionPane.SAPPanel.ContentRoot;
		int childCount = contentRoot.childCount;
		List<MEGUIParameterGroup> list = new List<MEGUIParameterGroup>();
		for (int i = 0; i < childCount; i++)
		{
			Transform child = contentRoot.GetChild(i);
			if (child != null)
			{
				MEGUIParameterGroup component = child.GetComponent<MEGUIParameterGroup>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		if (parametergroup >= list.Count)
		{
			return null;
		}
		return list[parametergroup].containerChilden.GetComponentsInChildren<MEGUIParameter>();
	}

	public bool IsParametersCorrect(MEGUIParameter[] baseParameters, List<Typekey> parameters)
	{
		int num = 0;
		if (baseParameters == null)
		{
			return false;
		}
		for (int i = 0; i < baseParameters.Length; i++)
		{
			if (IsParameterValid(baseParameters[i], parameters[num].type, parameters[num].value))
			{
				num++;
				if (num == parameters.Count)
				{
					break;
				}
			}
			else if (num > 0)
			{
				num = 0;
			}
		}
		return num == parameters.Count;
	}

	public bool IsParameterValid(MEGUIParameter parameter, Type type, string value)
	{
		if (!(parameter == null) && !(type == null))
		{
			if (type.IsAssignableFrom(parameter.GetType()))
			{
				if (type == typeof(MEGUIParameterDropdownList))
				{
					return (parameter as MEGUIParameterDropdownList).SelectedValue.ToString().Equals(value);
				}
				if (type == typeof(MEGUIParameterNumberRange))
				{
					return (parameter as MEGUIParameterNumberRange).valueText.text.Equals(value);
				}
			}
			return false;
		}
		return false;
	}

	public TutorialStep GetCurrentTutorialStep()
	{
		TutorialPage tutorialPage = Tutorial.CurrentState as TutorialPage;
		int num = 0;
		while (true)
		{
			if (num < Tutorial.pages.Count)
			{
				if (tutorialPage == Tutorial.pages[num])
				{
					break;
				}
				num++;
				continue;
			}
			return TutorialStep.none;
		}
		return (TutorialStep)num;
	}

	public void SetPagePositionTopLeft()
	{
		if (Tutorial.CurrentState is TutorialPage tutorialPage)
		{
			tutorialPage.dialog.dialogRect.x = 0f;
			tutorialPage.dialog.dialogRect.y = 1f;
		}
	}

	public void RemoveDrag()
	{
		OnEnterDragStep("", "");
		RemoveDragHelper();
		LockNodeSettings(locked: false);
	}
}
