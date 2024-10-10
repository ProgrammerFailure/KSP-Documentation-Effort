using System.Collections.Generic;
using UnityEngine;

public class MissionScreenTutorial : TutorialScenario
{
	public string stateName = "missionScreen";

	public bool complete;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Wernher";
		SetDialogRect(new Rect(CalcDialogXRatio(), 0.85f, 420f, 190f));
		base.OnAssetSetup();
	}

	public override void OnTutorialSetup()
	{
		if (complete)
		{
			CloseTutorialWindow();
			return;
		}
		List<TutorialPage> st = CreateMissionScreenTutorialPages();
		Tutorial.AddPages(st);
		Tutorial.StartTutorial(stateName);
	}

	public List<TutorialPage> CreateMissionScreenTutorialPages()
	{
		return new List<TutorialPage>
		{
			CreateMissionScreenTutorialPage(),
			CreateMissionBriefingTutorialPage(),
			CreateMissionTutorialPage()
		};
	}

	public TutorialPage CreateMissionScreenTutorialPage()
	{
		TutorialPage tutorialPage = CreateTutorialPage("missionScreen", "#autoLOC_8300000", OnEnterMissionScreenTutorialPage);
		tutorialPage.dialog = CreateNextDialog(tutorialPage.name, "#autoLOC_8300001");
		return tutorialPage;
	}

	public void OnEnterMissionScreenTutorialPage(KFSMState aState)
	{
		instructor.StopRepeatingEmote();
	}

	public TutorialPage CreateMissionBriefingTutorialPage()
	{
		TutorialPage tutorialPage = CreateTutorialPage("missionBriefing", "#autoLOC_8300000", OnEnterCreateMissionBriefingTutorialPage);
		tutorialPage.dialog = CreateNextDialog(tutorialPage.name, "#autoLOC_8300002");
		return tutorialPage;
	}

	public void OnEnterCreateMissionBriefingTutorialPage(KFSMState aState)
	{
		instructor.StopRepeatingEmote();
		instructor.PlayEmote(instructor.anim_true_smileB);
	}

	public TutorialPage CreateMissionTutorialPage()
	{
		TutorialPage tutorialPage = CreateTutorialPage("tutorialMissions", "#autoLOC_8300000", OnEnterMissionTutorialPage);
		tutorialPage.dialog = CreateDoneDialog(tutorialPage.name, "#autoLOC_8300003");
		return tutorialPage;
	}

	public void OnEnterMissionTutorialPage(KFSMState aState)
	{
		instructor.PlayEmote(instructor.anim_idle_lookAround);
	}

	public override void OnDoneButtonClick()
	{
		GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED = true;
		GameSettings.SaveSettings();
		complete = true;
		base.OnDoneButtonClick();
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
