using System;
using Expansions.Missions;
using Expansions.Missions.Editor;
using UnityEngine;

public class VesselTutorialStep : IntermediateTutorialPageStep
{
	public const string intermediateTitleLoc = "#autoLOC_9990010";

	public const string biomeSelectionExpectedValue = "Kerbin";

	public const int nodeCountRequired = 1;

	public const int dialogNodeCountRequired = 2;

	public const int vesselNodeCountRequired = 2;

	public VesselTutorialStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorialFsm)
		: base(tutorialScenario, tutorialFsm)
	{
	}

	public override void AddTutorialStepConfig()
	{
		AddTutorialStepConfig("dragAndLinkVesselNode", "#autoLOC_9990010", "#autoLOC_9990040", OnEnterDragAndLinkVesselNode, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("dragAndLinkVesselNodeHelper", "#autoLOC_9990010", "#autoLOC_9990041", OnEnterDragAndLinkVesselNode);
		AddTutorialStepConfig("selectKerbinPlanet", "#autoLOC_9990010", "#autoLOC_9990042", OnEnterSelectKerbinPlanet, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("selectKerbinPlanetHelper", "#autoLOC_9990010", "#autoLOC_9990043", OnEnterSelectKerbinPlanetHelper);
	}

	public void OnEnterDragAndLinkVesselNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkVesselNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveDragAndLinkVesselNode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectChange));
		tutorialScenario.DragNodeHelper(FilterVesselLandedNodeCriteria, "Location");
		GameEvents.Mission.onBuilderNodeFocused.Add(OnNodeFocused);
	}

	public bool FilterVesselLandedNodeCriteria(MEGUINodeIcon meguiNodeIcon)
	{
		if (meguiNodeIcon.basicNode != null)
		{
			return meguiNodeIcon.basicNode.name == "Landed".ToString();
		}
		return false;
	}

	public void OnNodeFocused(MENode data)
	{
		if (tutorialScenario.GetMEGUINodes("Landed").Count == 2)
		{
			tutorialScenario.RemoveDragHelper();
			tutorialScenario.LockCanvas(locked: false);
		}
	}

	public void OnUpdateLinkVesselNode()
	{
		if (IsDialogLinked())
		{
			tutorial.GoToNextPage();
		}
	}

	public void OnSelectedGameObjectChange(GameObject gameObject)
	{
		if (IsDialogLinked())
		{
			tutorial.GoToNextPage();
		}
	}

	public bool IsDialogLinked()
	{
		if (tutorialScenario.IsAllNodeLinked(METutorialScenario.NodeDefinition.DialogMessage2, METutorialScenario.NodeDefinition.VesselLanded2, uniquestartlink: false))
		{
			return tutorialScenario.IsAllNodeLinked(METutorialScenario.NodeDefinition.DialogMessage2, METutorialScenario.NodeDefinition.VesselLanded, uniquestartlink: false);
		}
		return false;
	}

	public void OnLeaveDragAndLinkVesselNode(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateLinkVesselNode));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveDragAndLinkVesselNode));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectChange));
		tutorialScenario.ResetHighLightLink();
		tutorialScenario.RemoveDragHelper();
		tutorialScenario.LockCanvas(locked: true);
		GameEvents.Mission.onBuilderNodeFocused.Remove(OnNodeFocused);
	}

	public void OnEnterSelectKerbinPlanet(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Combine(currentState.OnLeave, new KFSMStateChange(OnLeaveSelectKerbinPlanet));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnUpdate = (KFSMCallback)Delegate.Combine(currentState2.OnUpdate, new KFSMCallback(OnUpdateSelectKerbinPlanet));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectChangeSetPlanet));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateSelectKerbinPlanet()
	{
		CanHighlightVesselLanded();
	}

	public void OnSelectedGameObjectChangeSetPlanet(GameObject gameObject)
	{
		tutorial.GoToNextPage();
	}

	public void OnLeaveSelectKerbinPlanet(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Remove(currentState.OnLeave, new KFSMStateChange(OnLeaveSelectKerbinPlanet));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnUpdate = (KFSMCallback)Delegate.Remove(currentState2.OnUpdate, new KFSMCallback(OnUpdateSelectKerbinPlanet));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectedGameObjectChangeSetPlanet));
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}

	public void OnEnterSelectKerbinPlanetHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateSelectKerbinPlanetHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveSelectKerbinPlanetHelper));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateSelectKerbinPlanetHelper()
	{
		bool enable;
		if (enable = IsPlanetSetted())
		{
			tutorial.GoToNextPage();
		}
		else
		{
			CanHighlightVesselLanded();
		}
		tutorialScenario.EnableAllTutorialPageButtons(enable);
	}

	public bool IsPlanetSetted()
	{
		if (!(tutorialScenario.GetSelectedNodeTemplateName() != "Landed".ToString()) && tutorialScenario.GetSelectedNodeIndex() == 7)
		{
			MEGUIParameterCelestialBody mEGUIParameterCelestialBody = tutorialScenario.GetVesselLandedCelestialBody() as MEGUIParameterCelestialBody;
			string text = string.Empty;
			if (mEGUIParameterCelestialBody.FieldValue.Body != null && mEGUIParameterCelestialBody.FieldValue.Body.bodyName != null)
			{
				text = mEGUIParameterCelestialBody.FieldValue.Body.bodyName;
			}
			return text == "Kerbin";
		}
		return false;
	}

	public void CanHighlightVesselLanded()
	{
		if (!(tutorialScenario.GetSelectedNodeTemplateName() != "Landed".ToString()) && tutorialScenario.GetSelectedNodeIndex() == 7)
		{
			MEGUIParameterCelestialBody parameter = tutorialScenario.GetVesselLandedCelestialBody() as MEGUIParameterCelestialBody;
			tutorialScenario.ShowTutorialSelection(parameter);
		}
		else
		{
			tutorialScenario.HideAllTutorialSelectors();
		}
	}

	public void OnLeaveSelectKerbinPlanetHelper(KFSMState kfsmState)
	{
		tutorialScenario.HideAllTutorialSelectors();
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateSelectKerbinPlanetHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveSelectKerbinPlanetHelper));
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}
}
