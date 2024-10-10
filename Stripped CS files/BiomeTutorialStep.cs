using System;
using Expansions.Missions.Editor;
using UnityEngine;

public class BiomeTutorialStep : IntermediateTutorialPageStep
{
	public const string intermediateTitleLoc = "#autoLOC_9990010";

	public const string biomeSettingExpectedValue = "biomeData";

	public const string biomeCelestialBodyExpected = "Mun";

	public BiomeTutorialStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorialFsm)
		: base(tutorialScenario, tutorialFsm)
	{
	}

	public override void AddTutorialStepConfig()
	{
		AddTutorialStepConfig("findBiomeSetting", "#autoLOC_9990010", "#autoLOC_9990012", OnEnterFindBiomeSetting, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("findBiomeSettingHelper", "#autoLOC_9990010", "#autoLOC_9990013", OnEnterFindBiomeSettingHelper);
		AddTutorialStepConfig("biomeExplanation", "#autoLOC_9990010", "#autoLOC_9990014", base.OnEnterEmpty);
		AddTutorialStepConfig("selectBiome", "#autoLOC_9990010", "#autoLOC_9990015", OnEnterSelectBiome, METutorialScenario.TutorialButtonType.NoButton);
		AddTutorialStepConfig("selectBiomeHelper", "#autoLOC_9990010", "#autoLOC_9990016", OnEnterSelectBiomeHelper);
	}

	public void OnEnterFindBiomeSetting(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateFindBiomeSetting));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveFindBiomeSetting));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnFindBiomeSettingHelper));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateFindBiomeSetting()
	{
		bool flag = IsVesselLandedSelected();
		bool flag2 = IsBiomeSettingSetted(flag);
		ShowBiomeSetttingSelector(flag, flag2);
		if (flag && flag2)
		{
			tutorial.GoToNextPage();
		}
	}

	public void OnFindBiomeSettingHelper(GameObject gameObject)
	{
		tutorial.GoToNextPage();
	}

	public void OnLeaveFindBiomeSetting(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateFindBiomeSetting));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveFindBiomeSetting));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnFindBiomeSettingHelper));
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}

	public void OnEnterFindBiomeSettingHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateFindBiomeSettingHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveFindBiomeSettingHelper));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateFindBiomeSettingHelper()
	{
		bool flag = IsVesselLandedSelected();
		bool flag2 = IsBiomeSettingSetted(flag);
		ShowBiomeSetttingSelector(flag, flag2);
		tutorialScenario.EnableAllTutorialPageButtons(flag2);
	}

	public bool IsVesselLandedSelected()
	{
		return tutorialScenario.GetSelectedNodeTemplateName() == "Landed".ToString();
	}

	public bool IsBiomeSettingSetted(bool isVesselLandedSelected)
	{
		if (!isVesselLandedSelected)
		{
			return false;
		}
		MEGUIParameterDropdownList mEGUIParameterDropdownList = tutorialScenario.GetVesselLandedLocationType() as MEGUIParameterDropdownList;
		if (mEGUIParameterDropdownList != null)
		{
			return mEGUIParameterDropdownList.SelectedValue.ToString() == "biomeData";
		}
		return false;
	}

	public void ShowBiomeSetttingSelector(bool isInLandedNode, bool isBiomeSettingSelected)
	{
		if (!isInLandedNode)
		{
			tutorialScenario.HideAllTutorialSelectors();
			return;
		}
		MEGUIParameter vesselLandedLocationType = tutorialScenario.GetVesselLandedLocationType();
		if (isBiomeSettingSelected)
		{
			tutorialScenario.HideTutorialSelection(vesselLandedLocationType);
		}
		else
		{
			tutorialScenario.ShowTutorialSelection(vesselLandedLocationType);
		}
	}

	public void OnLeaveFindBiomeSettingHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateFindBiomeSettingHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveFindBiomeSettingHelper));
		tutorialScenario.EnableAllTutorialPageButtons(enable: true);
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}

	public void OnEnterSelectBiome(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnLeave = (KFSMStateChange)Delegate.Combine(currentState.OnLeave, new KFSMStateChange(OnLeaveSelectBiome));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnUpdate = (KFSMCallback)Delegate.Combine(currentState2.OnUpdate, new KFSMCallback(OnUpdateSelectBiome));
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Combine(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectBiome));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateSelectBiome()
	{
		if (MunBiomeSelected())
		{
			tutorial.GoToNextPage();
		}
	}

	public bool MunBiomeSelected()
	{
		return IsMunSelected() & HasBiomeSelected();
	}

	public bool IsMunSelected()
	{
		if (tutorialScenario.GetSelectedNodeTemplateName() != "Landed".ToString())
		{
			tutorialScenario.HideAllTutorialSelectors();
			return false;
		}
		MEGUIParameterCelestialBody_Biomes mEGUIParameterCelestialBody_Biomes = tutorialScenario.GetVesselLandedBiomes() as MEGUIParameterCelestialBody_Biomes;
		bool num = ((mEGUIParameterCelestialBody_Biomes.FieldValue.body == null) ? string.Empty : mEGUIParameterCelestialBody_Biomes.FieldValue.body.bodyName) == "Mun";
		if (!num)
		{
			tutorialScenario.ShowTutorialSelection(mEGUIParameterCelestialBody_Biomes);
		}
		return num;
	}

	public bool HasBiomeSelected()
	{
		if (tutorialScenario.GetSelectedNodeTemplateName() != "Landed".ToString())
		{
			tutorialScenario.HideAllTutorialSelectors();
			return false;
		}
		GAPCelestialBody gAPCelestialBody = MissionEditorLogic.Instance.actionPane.CurrentGapDisplay as GAPCelestialBody;
		if (!(gAPCelestialBody == null) && gAPCelestialBody.Biomes != null)
		{
			MEGUIParameter vesselLandedBiomes = tutorialScenario.GetVesselLandedBiomes();
			if (!gAPCelestialBody.Biomes.HasSelectedBiome())
			{
				tutorialScenario.ShowTutorialSelection(vesselLandedBiomes);
			}
			return gAPCelestialBody.Biomes.HasSelectedBiome();
		}
		return false;
	}

	public void OnSelectBiome(GameObject gameObject)
	{
		tutorial.GoToNextPage();
	}

	public void OnLeaveSelectBiome(KFSMState kfsmState)
	{
		MissionEditorLogic instance = MissionEditorLogic.Instance;
		instance.OnSelectedGameObjectChange = (Action<GameObject>)Delegate.Remove(instance.OnSelectedGameObjectChange, new Action<GameObject>(OnSelectBiome));
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateSelectBiome));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveSelectBiome));
		tutorialScenario.HideAllTutorialSelectors();
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}

	public void OnEnterSelectBiomeHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Combine(currentState.OnUpdate, new KFSMCallback(OnUpdateSelectHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Combine(currentState2.OnLeave, new KFSMStateChange(OnLeaveSelectBiomeHelper));
		tutorialScenario.LockNodeSettings(locked: false);
	}

	public void OnUpdateSelectHelper()
	{
		bool enable;
		if (enable = MunBiomeSelected())
		{
			tutorialScenario.HideAllTutorialSelectors();
		}
		tutorialScenario.EnableAllTutorialPageButtons(enable);
	}

	public void OnLeaveSelectBiomeHelper(KFSMState kfsmState)
	{
		KFSMState currentState = tutorial.CurrentState;
		currentState.OnUpdate = (KFSMCallback)Delegate.Remove(currentState.OnUpdate, new KFSMCallback(OnUpdateSelectHelper));
		KFSMState currentState2 = tutorial.CurrentState;
		currentState2.OnLeave = (KFSMStateChange)Delegate.Remove(currentState2.OnLeave, new KFSMStateChange(OnLeaveSelectBiomeHelper));
		tutorialScenario.HideAllTutorialSelectors();
		tutorialScenario.EnableAllTutorialPageButtons(enable: true);
		tutorialScenario.LockNodeSettings(locked: true);
		tutorialScenario.ResetNodeSettingsMask();
	}
}
