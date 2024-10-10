using ns11;
using ns2;
using ns9;
using UnityEngine;

public class TutorialScience : TutorialScenario
{
	public string stateName = "welcome";

	public bool complete;

	public KerbalInstructor otherWernher;

	public bool enteredRDComplex;

	public void EnterRD()
	{
		enteredRDComplex = true;
	}

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Wernher";
		SetDialogRect(new Rect(dialogPadSides / (float)Screen.width, 0.9f, 400f, 250f));
	}

	public override void OnTutorialSetup()
	{
		if (complete)
		{
			CloseTutorialWindow();
			return;
		}
		GameEvents.onGUIRnDComplexSpawn.Add(EnterRD);
		TutorialPage tutorialPage = new TutorialPage("welcome");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318504");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318512", instructor.CharacterName), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318513"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("rndBuilding");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318525");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoToRnD = true;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318533"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState _003Cp0_003E) => enteredRDComplex);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("insideRnD");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318545");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318553"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318554"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("unlockNextNode");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318566");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_smileA);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318575", tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState _003Cp0_003E) => ResearchAndDevelopment.PartTechAvailable(PartLoader.getPartInfoByName("stackDecoupler")) || ResearchAndDevelopment.PartTechAvailable(PartLoader.getPartInfoByName("fuelTankSmallFlat")));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("nodeUnlocked");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318587");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318595"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318596"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("whereDoesIt");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318608");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318615"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318616"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("situations");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318628");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318636", tutorialControlColorString), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition(delegate
		{
			UIStatePanel uIStatePanel = ((RDController.Instance != null) ? RDController.Instance.GetComponentInChildren<UIStatePanel>() : null);
			return (uIStatePanel != null && uIStatePanel.CurrentState.name == "archives") ? true : false;
		});
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("scienceArchives");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318654");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
			GameObject gameObject2 = GameObject.Find("Archives_Wernher(Clone)");
			if (gameObject2 != null)
			{
				otherWernher = gameObject2.GetComponent<KerbalInstructor>();
				if (otherWernher != null)
				{
					Light[] componentsInChildren2 = otherWernher.gameObject.GetComponentsInChildren<Light>();
					int num2 = componentsInChildren2.Length;
					while (num2-- > 0)
					{
						componentsInChildren2[num2].intensity = 0f;
					}
					otherWernher.transform.position = new Vector3(-4f, 0f, 0f);
				}
			}
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318678"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318679"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		tutorialPage.dialog.OnUpdate = delegate
		{
			GameObject gameObject = GameObject.Find("Archives_Wernher(Clone)");
			if (gameObject != null)
			{
				otherWernher = gameObject.GetComponent<KerbalInstructor>();
				if (otherWernher != null)
				{
					Light[] componentsInChildren = otherWernher.gameObject.GetComponentsInChildren<Light>();
					int num = componentsInChildren.Length;
					while (num-- > 0)
					{
						componentsInChildren[num].intensity = 0f;
					}
					otherWernher.transform.position = new Vector3(-4f, 0f, 0f);
				}
			}
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("scienceArchivsPlanets");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318711");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318719"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318720"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("conclusion");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_318732");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_318740", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_318741"), delegate
		{
			complete = true;
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		Tutorial.StartTutorial(stateName);
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

	public override void OnOnDestroy()
	{
		if (otherWernher != null)
		{
			Light[] componentsInChildren = otherWernher.gameObject.GetComponentsInChildren<Light>();
			int num = componentsInChildren.Length;
			while (num-- > 0)
			{
				componentsInChildren[num].intensity = 0.38f;
			}
		}
		GameEvents.onGUIRnDComplexSpawn.Remove(EnterRD);
	}
}
