using System;
using Expansions.Missions;
using Expansions.Missions.Editor;
using UnityEngine;

public class ScienceNodeTutorialStep : IntermediateTutorialPageStep
{
	public const string intermediateTitleLoc = "#autoLOC_9990010";

	public int nodeCount;

	public ScienceNodeTutorialStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorialFsm)
		: base(tutorialScenario, tutorialFsm)
	{
	}

	public override void AddTutorialStepConfig()
	{
		AddTutorialStepConfig("dragScienceEventNode", "#autoLOC_9990010", "#autoLOC_9990019", OnEnterDragScienceEventNode, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("dragScienceEventNodeHelper", "#autoLOC_9990010", "#autoLOC_9990020", OnEnterDragScienceEventNodeHelper, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("linkScienceEventNode", "#autoLOC_9990010", "#autoLOC_9990021", OnEnterLinkScienceEventNode, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("linkScienceEventNodeHelper", "#autoLOC_9990010", "#autoLOC_9990022", OnEnterLinkScienceEventNode, METutorialScenario.TutorialButtonType.NoButton);
	}

	public void OnEnterDragScienceEventNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Combine(currentState.OnLeave, new KFSMStateChange(OnLeaveOnEnterDragScienceEventNode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectDuringDragScienceTuto));
		tutorialScenario.DragNodeHelper(FilterScienceNodeCriteria, "ResourcesAndScience");
	}

	public bool FilterScienceNodeCriteria(MEGUINodeIcon meguiNodeIcon)
	{
		if (meguiNodeIcon.basicNode != null)
		{
			return meguiNodeIcon.basicNode.name == "ScienceExperiment".ToString();
		}
		return false;
	}

	public void OnSelectDuringDragScienceTuto(GameObject gameObject)
	{
		tutorial.GoToNextPage();
	}

	public void OnLeaveOnEnterDragScienceEventNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Remove(currentState.OnLeave, new KFSMStateChange(OnLeaveOnEnterDragScienceEventNode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectDuringDragScienceTuto));
		tutorialScenario.RemoveDragHelper();
	}

	public void OnEnterDragScienceEventNodeHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateDragScienceEventNodeHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveDragScienceEventNodeHelper));
		nodeCount = MissionEditorLogic.Instance.GetNodeListCout();
		tutorialScenario.DragNodeHelper(FilterScienceNodeCriteria, "ResourcesAndScience");
	}

	public void OnUpdateDragScienceEventNodeHelper()
	{
		if (tutorialScenario.CanSkipCurrentTutorialOnNodeDragEnd(nodeCount))
		{
			if (tutorialScenario.GetSelectedNodeTemplateName() == METutorialScenario.NodeDefinition.ScienceExperiment.ToString())
			{
				tutorial.GoToNextPage();
			}
			else
			{
				nodeCount = MissionEditorLogic.Instance.GetNodeListCout();
			}
		}
	}

	public void OnLeaveDragScienceEventNodeHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateDragScienceEventNodeHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveDragScienceEventNodeHelper));
		tutorialScenario.RemoveDragHelper();
	}

	public void OnEnterLinkScienceEventNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkScienceEventNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveOnEnterLinkScienceEventNode));
		GameEvents.Mission.onBuilderNodeConnection.Add(OnNodeEventScienceTuto);
		GameEvents.Mission.onBuilderNodeDisconnection.Add(OnNodeEventScienceTuto);
		tutorialScenario.LockCanvas(locked: false);
	}

	public void OnNodeEventScienceTuto(GameEvents.FromToAction<MENode, MENode> connection)
	{
		if (IsScienceLinked())
		{
			tutorial.GoToNextPage();
		}
	}

	public void OnUpdateLinkScienceEventNode()
	{
		if (IsScienceLinked())
		{
			tutorial.GoToNextPage();
		}
	}

	public bool IsScienceLinked()
	{
		if (tutorialScenario.IsAllNodeLinked(METutorialScenario.NodeDefinition.Orbit, METutorialScenario.NodeDefinition.ScienceExperiment))
		{
			return tutorialScenario.IsAllNodeLinked(METutorialScenario.NodeDefinition.ScienceExperiment, METutorialScenario.NodeDefinition.VesselLanded);
		}
		return false;
	}

	public void OnLeaveOnEnterLinkScienceEventNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkScienceEventNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveOnEnterLinkScienceEventNode));
		GameEvents.Mission.onBuilderNodeConnection.Remove(OnNodeEventScienceTuto);
		GameEvents.Mission.onBuilderNodeDisconnection.Remove(OnNodeEventScienceTuto);
		tutorialScenario.ResetHighLightLink();
		tutorialScenario.LockCanvas(locked: true);
	}
}
