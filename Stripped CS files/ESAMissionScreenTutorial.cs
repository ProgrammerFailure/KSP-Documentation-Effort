using System.Collections.Generic;
using UnityEngine;

public class ESAMissionScreenTutorial : TutorialScenario
{
	public string stateName = "esaMissionScreen";

	public bool complete;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
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
		Tutorial.AddPages(CreateESAMissionScreenTutorialPages());
		Tutorial.StartTutorial(stateName);
	}

	public List<TutorialPage> CreateESAMissionScreenTutorialPages()
	{
		return new List<TutorialPage>
		{
			ESAMissionScreenTutorialPageOne(),
			ESAMissionScreenTutorialPageTwo(),
			ESAMissionScreenTutorialPageThree()
		};
	}

	public TutorialPage ESAMissionScreenTutorialPageOne()
	{
		TutorialPage tutorialPage = CreateTutorialPage("esaMissionScreen", "#autoLOC_6006011", OnEnterESAMissionScreenTutorialPageOne);
		tutorialPage.dialog = CreateNextDialog(tutorialPage.name, "#autoLOC_6006012");
		return tutorialPage;
	}

	public void OnEnterESAMissionScreenTutorialPageOne(KFSMState aState)
	{
		instructor.StopRepeatingEmote();
	}

	public TutorialPage ESAMissionScreenTutorialPageTwo()
	{
		TutorialPage tutorialPage = CreateTutorialPage("esaMissionScreenPage2", "#autoLOC_6006011", OnEnterESAMissionScreenTutorialPageTwo);
		tutorialPage.dialog = CreateNextDialog(tutorialPage.name, "#autoLOC_8300002");
		return tutorialPage;
	}

	public void OnEnterESAMissionScreenTutorialPageTwo(KFSMState aState)
	{
		instructor.StopRepeatingEmote();
		instructor.PlayEmote(instructor.anim_true_smileB);
	}

	public TutorialPage ESAMissionScreenTutorialPageThree()
	{
		TutorialPage tutorialPage = CreateTutorialPage("esaMissionScreenPage3", "#autoLOC_6006011", OnEnterESAMissionScreenTutorialPageThree);
		tutorialPage.dialog = CreateDoneDialog(tutorialPage.name, "#autoLOC_6006013");
		return tutorialPage;
	}

	public void OnEnterESAMissionScreenTutorialPageThree(KFSMState aState)
	{
		instructor.StopRepeatingEmote();
		instructor.PlayEmote(instructor.anim_true_thumbsUp);
	}

	public override void OnDoneButtonClick()
	{
		GameSettings.TUTORIALS_ESA_MISSION_SCREEN_TUTORIAL_COMPLETED = true;
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
