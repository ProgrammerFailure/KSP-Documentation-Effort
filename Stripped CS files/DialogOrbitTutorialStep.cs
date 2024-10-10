using System;
using Expansions.Missions;
using Expansions.Missions.Editor;

public class DialogOrbitTutorialStep : IntermediateTutorialPageStep
{
	public const string intermediateTitleLoc = "#autoLOC_9990010";

	public const int dialogCountAttended = 1;

	public DialogOrbitTutorialStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorialFsm)
		: base(tutorialScenario, tutorialFsm)
	{
	}

	public override void AddTutorialStepConfig()
	{
		AddTutorialStepConfig("dialogueNodeExplanation", "#autoLOC_9990010", "#autoLOC_9990026", base.OnEnterEmpty);
		AddTutorialStepConfig("dragDialogueNode", "#autoLOC_9990010", "#autoLOC_9990027", OnEnterDragDialogueNode, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("dragDialogueNodeHelper", "#autoLOC_9990010", "#autoLOC_9990028", OnEnterDragDialogueNode, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("setDialogueText", "#autoLOC_9990010", "#autoLOC_9990029", OnEnterSetDialogueTextHelper);
	}

	public void OnEnterDragDialogueNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateDragDialogueNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveOnEnterDragDialogueNode));
		GameEvents.Mission.onBuilderNodeConnection.Add(OnNodeEventDialogueTuto);
		GameEvents.Mission.onBuilderNodeDisconnection.Add(OnNodeEventDialogueTuto);
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
		if (tutorialScenario.GetMEGUINodes("DialogMessage").Count == 1)
		{
			tutorialScenario.RemoveDragHelper();
			tutorialScenario.LockCanvas(locked: false);
		}
	}

	public void OnNodeEventDialogueTuto(GameEvents.FromToAction<MENode, MENode> connection)
	{
		if (IsDialogLinked())
		{
			tutorial.GoToNextPage();
		}
	}

	public void OnUpdateDragDialogueNode()
	{
		if (IsDialogLinked())
		{
			tutorial.GoToNextPage();
		}
	}

	public bool IsDialogLinked()
	{
		if (tutorialScenario.IsAllNodeLinked(METutorialScenario.NodeDefinition.Orbit, METutorialScenario.NodeDefinition.DialogMessage))
		{
			return tutorialScenario.IsAllNodeLinked(METutorialScenario.NodeDefinition.DialogMessage, METutorialScenario.NodeDefinition.ScienceExperiment);
		}
		return false;
	}

	public void OnLeaveOnEnterDragDialogueNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateDragDialogueNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveOnEnterDragDialogueNode));
		GameEvents.Mission.onBuilderNodeConnection.Remove(OnNodeEventDialogueTuto);
		GameEvents.Mission.onBuilderNodeDisconnection.Remove(OnNodeEventDialogueTuto);
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
		if (tutorialScenario.GetSelectedNodeTemplateName() != "DialogMessage".ToString())
		{
			return false;
		}
		tutorialScenario.LockSapParameters(METutorialScenario.NodeDefinition.DialogMessage, "Expansions.Missions.Actions.ActionDialogMessage.message");
		MEGUIParameterTextArea mEGUIParameterTextArea = tutorialScenario.GetDialogMessageParam() as MEGUIParameterTextArea;
		if (!(mEGUIParameterTextArea == null) && !(mEGUIParameterTextArea.inputField == null))
		{
			return !string.IsNullOrEmpty(mEGUIParameterTextArea.inputField.text);
		}
		return false;
	}

	public void HighlightDialogueMessage()
	{
		if (tutorialScenario.GetSelectedNodeTemplateName() != "DialogMessage".ToString())
		{
			tutorialScenario.HideAllTutorialSelectors();
		}
		else
		{
			tutorialScenario.ShowTutorialSelection(tutorialScenario.GetDialogMessageParam());
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
