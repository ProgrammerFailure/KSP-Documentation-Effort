using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

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

	private struct Typekey
	{
		public Type type;

		public string value;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Typekey(Type typekey, string valuekey)
		{
			throw null;
		}
	}

	private const string lockIdMissionBuilder = "missionBuilder_start";

	private List<Typekey> typekeys;

	private int nodeCount;

	private const int HEADER_PARAMETER_GROUP = 0;

	private const int DEFAULT_PARAMETER_GROUP = 1;

	private const int MESSAGE_PARAMETER_INDEX = 1;

	private const int END_CHECKBOX_PARAMETER = 4;

	private TutorialStep currentstep;

	private string validationLockid;

	private List<NodeDefinition> dialogNodes;

	private Dictionary<NodeDefinition, string> textNodes;

	private int m_TutorialShown;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdvancedTutorial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMissionBriefingShow(MissionBriefingDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CreateTutorialPages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitMissionEditorScreen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitCancelSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LockBar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ArrangeGraph()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDoneButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterCrew(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInitCreateVesel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectedNodeChange(GameObject objectchange)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateCrew(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateSelectedCrewNodeChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitCrew(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveCrew(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateCrewErrors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdatePart(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdatePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdatePartError()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeavePart(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterVessel(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselError()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveVessel(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnterVesselDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnterVesselName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnterVesselWriteMain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnterVesselWriteError()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnterVesselLink()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnterVesselMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterDragStep(string filterednode, string highlightCategory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselSelectName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselLink()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsDragMainStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsDragErrorStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsLinkMainStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsLinkErrorStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsWriteMainStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsWriteErrorStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSideBarNodeFiltered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEGUIParameterVesselDropdownList GetVesselDropDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEGUIParameterTextArea GetMessageParameter(NodeDefinition node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTextValueChanged(string newname)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<NodeDefinition> GetTextNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetLinkNodes(ref List<int> start, ref List<int> end)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsAllTextValueValid(List<NodeDefinition> nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsTextValueValid(NodeDefinition node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterValidation(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterValidation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateValidation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateValidationError()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveValidation(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateValidationClickReport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddEndNodeListener()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCheckboxValueChanged(bool newvalue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsAllDialogNodeEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEGUIParameterCheckbox GetNodeEndParameter(NodeDefinition node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsValidationDialogOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterExport(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterExport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeaveExport(KFSMState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateExport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightTitleBarExportButton(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsExportDialogOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTestButtonInteractibility(bool interactable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterConclusion(KFSMState aState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEGUIParameter[] GetCurrentNodeParameters(int parametergroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsParametersCorrect(MEGUIParameter[] baseParameters, List<Typekey> parameters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsParameterValid(MEGUIParameter parameter, Type type, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private TutorialStep GetCurrentTutorialStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPagePositionTopLeft()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveDrag()
	{
		throw null;
	}
}
