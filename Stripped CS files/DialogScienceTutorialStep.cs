using System;
using Expansions.Missions;
using Expansions.Missions.Editor;

public class DialogScienceTutorialStep : IntermediateTutorialPageStep
{
	public const string intermediateTitleLoc = "#autoLOC_9990010";

	public const int nodeCountRequired = 1;

	public const int dialogNodeCountRequired = 2;

	public DialogScienceTutorialStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorialFsm)
		: base(tutorialScenario, tutorialFsm)
	{
	}

	public override void AddTutorialStepConfig()
	{
		AddTutorialStepConfig("secondDialogueIntro", "#autoLOC_9990010", "#autoLOC_9990034", base.OnEnterEmpty);
		AddTutorialStepConfig("secondDialogueExplanation", "#autoLOC_9990010", "#autoLOC_9990035", base.OnEnterEmpty);
		AddTutorialStepConfig("dragSecondDialogueNode", "#autoLOC_9990010", "#autoLOC_9990036", OnEnterDragSecondDialogue, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("dragSecondDialogueNodeHelper", "#autoLOC_9990010", "#autoLOC_9990037", OnEnterDragSecondDialogue, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("setSecondDialogueText", "#autoLOC_9990010", "#autoLOC_9990038", OnEnterSetDialogueTextHelper);
	}

	public void OnEnterDragSecondDialogue(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkScienceEventNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveLinkScienceEventNode));
		GameEvents.Mission.onBuilderNodeConnection.Add(OnNodeEventScienceTuto);
		GameEvents.Mission.onBuilderNodeDisconnection.Add(OnNodeEventScienceTuto);
		tutorialScenario.DragNodeHelper(FilterDialogNodeCriteria, "Utility");
		GameEvents.Mission.onBuilderNodeFocused.Add(OnNodeFocused);
	}

	public bool FilterDialogNodeCriteria(MEGUINodeIcon meguiNodeIcon)
	{
		if (meguiNodeIcon.basicNode != null)
		{
			return meguiNodeIcon.basicNode.name == "DialogMessage".ToString();
		}
		return false;
	}

	public void OnNodeFocused(MENode data)
	{
		if (tutorialScenario.GetMEGUINodes("DialogMessage").Count == 2)
		{
			tutorialScenario.RemoveDragHelper();
			tutorialScenario.LockCanvas(locked: false);
		}
	}

	public void OnUpdateLinkScienceEventNode()
	{
		if (IsDialogLinked())
		{
			tutorial.GoToNextPage();
		}
	}

	public void OnNodeEventScienceTuto(GameEvents.FromToAction<MENode, MENode> connection)
	{
		if (IsDialogLinked())
		{
			tutorial.GoToNextPage();
		}
	}

	public bool IsDialogLinked()
	{
		if (tutorialScenario.IsAllNodeLinked(METutorialScenario.NodeDefinition.ScienceExperiment, METutorialScenario.NodeDefinition.DialogMessage2))
		{
			return tutorialScenario.IsAllNodeLinked(METutorialScenario.NodeDefinition.DialogMessage2, METutorialScenario.NodeDefinition.VesselLanded);
		}
		return false;
	}

	public void OnLeaveLinkScienceEventNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkScienceEventNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveLinkScienceEventNode));
		GameEvents.Mission.onBuilderNodeConnection.Remove(OnNodeEventScienceTuto);
		GameEvents.Mission.onBuilderNodeDisconnection.Remove(OnNodeEventScienceTuto);
		tutorialScenario.ResetHighLightLink();
		tutorialScenario.RemoveDragHelper();
		tutorialScenario.LockCanvas(locked: true);
		GameEvents.Mission.onBuilderNodeFocused.Remove(OnNodeFocused);
	}

	public void OnEnterSetDialogueTextHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateSetDialogueTextHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveSetDialogueTextHelper));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateSetDialogueTextHelper()
	{
		bool enable;
		if (!(enable = IsDialogueMessageSetted()))
		{
			HighlightDialogueMessage();
		}
		tutorialScenario.EnableAllTutorialPageButtons(enable);
	}

	public bool IsDialogueMessageSetted()
	{
		if (!(tutorialScenario.GetSelectedNodeTemplateName() != "DialogMessage".ToString()) && tutorialScenario.GetSelectedNodeIndex() == 6)
		{
			tutorialScenario.LockSapParameters(METutorialScenario.NodeDefinition.DialogMessage2, "Expansions.Missions.Actions.ActionDialogMessage.message");
			MEGUIParameterTextArea mEGUIParameterTextArea = tutorialScenario.GetDialogMessageParam() as MEGUIParameterTextArea;
			if (!(mEGUIParameterTextArea == null) && !(mEGUIParameterTextArea.inputField == null))
			{
				return !string.IsNullOrEmpty(mEGUIParameterTextArea.inputField.text);
			}
			return false;
		}
		return false;
	}

	public void HighlightDialogueMessage()
	{
		if (!(tutorialScenario.GetSelectedNodeTemplateName() != "DialogMessage".ToString()) && tutorialScenario.GetSelectedNodeIndex() == 6)
		{
			tutorialScenario.ShowTutorialSelection(tutorialScenario.GetDialogMessageParam());
		}
		else
		{
			tutorialScenario.HideAllTutorialSelectors();
		}
	}

	public void OnLeaveSetDialogueTextHelper(KFSMState kfsmState)
	{
		tutorialScenario.HideAllTutorialSelectors();
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateSetDialogueTextHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveSetDialogueTextHelper));
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}
}
