using System;
using Expansions.Missions.Editor;
using UnityEngine;

public class EventSelectionTutorialStep : IntermediateTutorialPageStep
{
	public const string intermediateTitleLoc = "#autoLOC_9990010";

	public EventSelectionTutorialStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorialFsm)
		: base(tutorialScenario, tutorialFsm)
	{
	}

	public override void AddTutorialStepConfig()
	{
		AddTutorialStepConfig("tickEventNode", "#autoLOC_9990010", "#autoLOC_9990024", OnEnterTickEventNode, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("tickEventNodeHelper", "#autoLOC_9990010", "#autoLOC_9990025", OnEnterTickEventNodeHelper, METutorialScenario.TutorialButtonType.NoButton);
	}

	public void OnEnterTickEventNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Combine(currentState.OnLeave, new KFSMStateChange(OnLeaveOnEnterTickEventNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnUpdate = (KFSMCallback)Delegate.Combine(currentState2.OnUpdate, new KFSMCallback(OnUpdateTickEventNode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectDuringTickEvent));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateTickEventNode()
	{
		tutorialScenario.LockSapParameters(METutorialScenario.NodeDefinition.ScienceExperiment, "Expansions.Missions.MENode.isEvent");
		ShowEventBoxSelector(IsEventBoxSelected());
	}

	public bool IsEventBoxSelected()
	{
		if (!IsScienceNodeSelected())
		{
			return false;
		}
		MEGUIParameterCheckbox mEGUIParameterCheckbox = tutorialScenario.GetScienceIsEventNodeParam() as MEGUIParameterCheckbox;
		if (mEGUIParameterCheckbox != null)
		{
			return mEGUIParameterCheckbox.FieldValue;
		}
		return false;
	}

	public void ShowEventBoxSelector(bool isEventNodeTicked)
	{
		if (!IsScienceNodeSelected())
		{
			tutorialScenario.HideAllTutorialSelectors();
			return;
		}
		MEGUIParameter scienceIsEventNodeParam = tutorialScenario.GetScienceIsEventNodeParam();
		if (isEventNodeTicked)
		{
			tutorialScenario.HideTutorialSelection(scienceIsEventNodeParam);
		}
		else
		{
			tutorialScenario.ShowTutorialSelection(scienceIsEventNodeParam);
		}
	}

	public bool IsScienceNodeSelected()
	{
		return tutorialScenario.GetSelectedNodeTemplateName() == "ScienceExperiment".ToString();
	}

	public void OnSelectDuringTickEvent(GameObject gameObject)
	{
		tutorial.GoToNextPage();
	}

	public void OnLeaveOnEnterTickEventNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Remove(currentState.OnLeave, new KFSMStateChange(OnLeaveOnEnterTickEventNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnUpdate = (KFSMCallback)Delegate.Remove(currentState2.OnUpdate, new KFSMCallback(OnUpdateTickEventNode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectDuringTickEvent));
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}

	public void OnEnterTickEventNodeHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateTickEventNodeHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveOnEnterTickEventNodeHelper));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateTickEventNodeHelper()
	{
		OnUpdateTickEventNode();
		if (IsEventBoxSelected())
		{
			tutorial.GoToNextPage();
		}
	}

	public void OnLeaveOnEnterTickEventNodeHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateTickEventNodeHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveOnEnterTickEventNodeHelper));
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}
}
