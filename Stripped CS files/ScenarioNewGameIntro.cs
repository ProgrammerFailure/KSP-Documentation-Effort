using ns9;
using UnityEngine;

[KSPScenario(ScenarioCreationOptions.AddToNewGames, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.EDITOR,
	GameScenes.TRACKSTATION
})]
public class ScenarioNewGameIntro : TutorialScenario
{
	public bool kscComplete;

	public bool editorComplete;

	public bool tsComplete;

	public override void OnAwake()
	{
		ExclusiveTutorial = false;
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("kscComplete"))
		{
			kscComplete = bool.Parse(node.GetValue("kscComplete"));
		}
		if (node.HasValue("editorComplete"))
		{
			editorComplete = bool.Parse(node.GetValue("editorComplete"));
		}
		if (node.HasValue("tsComplete"))
		{
			tsComplete = bool.Parse(node.GetValue("tsComplete"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("kscComplete", kscComplete);
		node.AddValue("editorComplete", editorComplete);
		node.AddValue("tsComplete", tsComplete);
	}

	public override void OnOnDestroy()
	{
		InputLockManager.RemoveControlLock("intro_KSC");
		InputLockManager.RemoveControlLock("intro_Editor");
		InputLockManager.RemoveControlLock("intro_TS");
		GameEvents.onGameSceneLoadRequested.Remove(onLeavingScene);
	}

	public override void OnAssetSetup()
	{
		switch (HighLogic.LoadedScene)
		{
		case GameScenes.SPACECENTER:
			instructorPrefabName = "Instructor_Gene";
			SetDialogRect(new Rect(0.15f, 0.85f, 350f, 250f));
			break;
		case GameScenes.EDITOR:
			instructorPrefabName = "Instructor_Wernher";
			SetDialogRect(new Rect(0.25f, 0.85f, 350f, 110f));
			break;
		case GameScenes.TRACKSTATION:
			instructorPrefabName = "Instructor_Gene";
			SetDialogRect(new Rect(0.25f, 0.85f, 350f, 110f));
			break;
		}
		GameEvents.onGameSceneLoadRequested.Add(onLeavingScene);
	}

	public override void OnTutorialSetup()
	{
		if (kscComplete && editorComplete && tsComplete)
		{
			HighLogic.CurrentGame.RemoveProtoScenarioModule(typeof(ScenarioNewGameIntro));
			CloseTutorialWindow();
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			return;
		}
		switch (HighLogic.LoadedScene)
		{
		case GameScenes.SPACECENTER:
			KSCtutorialSetup();
			break;
		case GameScenes.EDITOR:
			EditorTutorialSetup();
			break;
		case GameScenes.TRACKSTATION:
			TSTutorialSetup();
			break;
		case GameScenes.FLIGHT:
			break;
		}
	}

	public void onLeavingScene(GameScenes scn)
	{
		CloseTutorialWindow();
	}

	public void KSCtutorialSetup()
	{
		if (kscComplete)
		{
			CloseTutorialWindow();
			return;
		}
		TutorialPage page = new TutorialPage("welcome");
		page.windowTitle = Localizer.Format("#autoLOC_294977");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		page.OnDrawContent = delegate
		{
			page.dialog = new MultiOptionDialog("IntroKSC", "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_294987") + Localizer.Format("#autoLOC_294988") + "\n\n" + Localizer.Format("#autoLOC_294989") + Localizer.Format("#autoLOC_294990", tutorialControlColorString, tutorialControlColorString) + Localizer.Format("#autoLOC_294991", tutorialControlColorString, tutorialControlColorString) + Localizer.Format("#autoLOC_294992")), new DialogGUIButton(Localizer.Format("#autoLOC_294994"), delegate
			{
				kscComplete = true;
				CloseTutorialWindow();
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			}, dismissOnSelect: true));
		};
		Tutorial.AddPage(page);
		Tutorial.StartTutorial("welcome");
	}

	public void EditorTutorialSetup()
	{
		if (editorComplete)
		{
			CloseTutorialWindow();
			return;
		}
		TutorialPage page = new TutorialPage("welcome");
		page.windowTitle = Localizer.Format("#autoLOC_295026", KSPUtil.PrintModuleName(EditorDriver.editorFacility.ToFacility().displayDescription()));
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(ControlTypes.EDITOR_LOCK, "intro_Editor");
		};
		page.OnLeave = delegate
		{
			Debug.LogError("OnLeave");
			InputLockManager.RemoveControlLock("intro_Editor");
		};
		page.OnDrawContent = delegate
		{
			switch (EditorDriver.editorFacility)
			{
			default:
				page.dialog = new MultiOptionDialog("IntroDefault", "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_295080") + " " + Localizer.Format("#autoLOC_295081") + Localizer.Format("#autoLOC_295082"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_295084"), delegate
				{
					editorComplete = true;
					CloseTutorialWindow();
					GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
				}));
				break;
			case EditorFacility.None:
				break;
			case EditorFacility.const_1:
				page.dialog = new MultiOptionDialog("IntroVAB", "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_295046") + " " + Localizer.Format("#autoLOC_295047") + Localizer.Format("#autoLOC_295048") + Localizer.Format("#autoLOC_295049") + Localizer.Format("#autoLOC_295050", tutorialControlColorString, tutorialControlColorString) + Localizer.Format("#autoLOC_295051"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_295053"), delegate
				{
					editorComplete = true;
					CloseTutorialWindow();
					GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
				}));
				break;
			case EditorFacility.const_2:
				page.dialog = new MultiOptionDialog("IntroSPH", "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_295063") + " " + Localizer.Format("#autoLOC_295064") + Localizer.Format("#autoLOC_295065") + Localizer.Format("#autoLOC_295066") + Localizer.Format("#autoLOC_295067", tutorialControlColorString, tutorialControlColorString) + Localizer.Format("#autoLOC_295068"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_295070"), delegate
				{
					editorComplete = true;
					CloseTutorialWindow();
					GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
				}));
				break;
			}
		};
		Tutorial.AddPage(page);
		Tutorial.StartTutorial("welcome");
	}

	public void TSTutorialSetup()
	{
		if (tsComplete)
		{
			CloseTutorialWindow();
			return;
		}
		TutorialPage page = new TutorialPage("welcome");
		page.windowTitle = Localizer.Format("#autoLOC_295116");
		page.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(ControlTypes.TRACKINGSTATION_ALL, "intro_TS");
		};
		page.OnDrawContent = delegate
		{
			page.dialog = new MultiOptionDialog("IntroTS", "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_295126") + Localizer.Format("#autoLOC_295127") + Localizer.Format("#autoLOC_295128") + Localizer.Format("#autoLOC_295129") + Localizer.Format("#autoLOC_295130", tutorialControlColorString, GameSettings.MAP_VIEW_TOGGLE.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_295133"), delegate
			{
				tsComplete = true;
				CloseTutorialWindow();
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			}));
		};
		Tutorial.AddPage(page);
		Tutorial.StartTutorial("welcome");
	}
}
