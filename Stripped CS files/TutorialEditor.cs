using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;

public class TutorialEditor : TutorialScenario
{
	public string stateName = "welcome";

	public bool complete;

	public string vesselName;

	public Texture2D stagingStackIcon;

	public EditorPartListFilter<AvailablePart> tutorialFilter_none = new EditorPartListFilter<AvailablePart>("TutorialEditor_none", (AvailablePart a) => false);

	public EditorPartListFilter<AvailablePart> tutorialFilter_pod = new EditorPartListFilter<AvailablePart>("TutorialEditor_pod", (AvailablePart a) => a.name == "mk1pod" || a.name == "mk1pod.v2");

	public EditorPartListFilter<AvailablePart> tutorialFilter_chute = new EditorPartListFilter<AvailablePart>("TutorialEditor_chute", (AvailablePart a) => a.name == "parachuteSingle");

	public EditorPartListFilter<AvailablePart> tutorialFilter_bacc = new EditorPartListFilter<AvailablePart>("TutorialEditor_bacc", (AvailablePart a) => a.name == "solidBooster1-1");

	public EditorPartListFilter<AvailablePart> tutorialFilter_flea = new EditorPartListFilter<AvailablePart>("TutorialEditor_flea", (AvailablePart a) => a.name == "solidBooster_sm" || a.name == "solidBooster.sm" || a.name == "solidBooster.sm.v2");

	public EditorPartListFilter<AvailablePart> tutorialFilter_final = new EditorPartListFilter<AvailablePart>("TutorialEditor_final", (AvailablePart a) => a.name == "parachuteSingle" || a.name == "mk1pod" || a.name == "mk1pod.v2" || a.name == "solidBooster_sm" || a.name == "solidBooster.sm" || a.name == "solidBooster.sm.v2" || a.name == "solidBooster1-1");

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Wernher";
		SetDialogRect(new Rect(CalcDialogXRatio(), 0.85f, 420f, 190f));
		stagingStackIcon = GameDatabase.Instance.GetTexture("Squad/Tutorials/StagingStack", asNormalMap: false);
		base.OnAssetSetup();
	}

	public override void OnTutorialSetup()
	{
		if (complete)
		{
			CloseTutorialWindow();
			return;
		}
		TutorialPage tutorialPage = new TutorialPage("welcome");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310122");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310129"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310130"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("overview");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310141");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_smileB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310149", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310150"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("spaceCenter");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310160");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
			HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInVAB = true;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310168"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState _003Cp0_003E) => HighLogic.LoadedScene == GameScenes.EDITOR);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("insideVAB");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310181");
		tutorialPage.OnEnter = delegate
		{
			EditorDriver.fetch.SetInputLockFromGameParameters();
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_none);
			EditorPartList.Instance.Refresh();
			InputLockManager.SetControlLock(ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_NEW, "EditorTutorial");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_EDIT_STAGES, "tutorialEditor_Stages");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_EDIT_NAME_FIELDS, "tutorialEditor_Name");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_GIZMO_TOOLS, "tutorialEditor_Gizmos");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ROOT_REFLOW, "tutorialEditor_Reroot");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_MODE_SWITCH, "tutorialEditor_Modes");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_PAD_PICK_COPY, "tutorialEditor_Copy");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_SAVE, "tutorialEditor_Save");
			if (EditorLogic.fetch != null)
			{
				EditorLogic.fetch.disallowSave = true;
			}
			InputLockManager.SetControlLock(ControlTypes.EDITOR_LAUNCH, "tutorialEditor_Launch");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310206"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310207"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("selectPod");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310218");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_smileA);
			InputLockManager.RemoveControlLock("tutorialEditor_PickPart");
			InputLockManager.RemoveControlLock("tutorialEditor_PickIcon");
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_pod);
			EditorPartList.Instance.Refresh();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310243", tutorialHighlightColorString), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState st) => EditorLogic.fetch.ship.Count == 1 && EditorLogic.RootPart != null && (EditorLogic.RootPart.partInfo.name == "mk1pod" || EditorLogic.RootPart.partInfo.name == "mk1pod.v2"));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("camControls");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310254");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
			InputLockManager.SetControlLock(ControlTypes.EDITOR_PAD_PICK_PLACE, "tutorialEditor_PickPart");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_PICK, "tutorialEditor_PickIcon");
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_none);
			EditorPartList.Instance.Refresh();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310279", tutorialControlColorString, tutorialControlColorString, tutorialControlColorString, GameSettings.SCROLL_VIEW_UP.name, tutorialControlColorString, GameSettings.SCROLL_VIEW_DOWN.name, tutorialControlColorString, GameSettings.AXIS_MOUSEWHEEL.primary.name, tutorialControlColorString, GameSettings.ZOOM_IN.name, tutorialControlColorString, GameSettings.ZOOM_OUT.name, tutorialControlColorString, tutorialControlColorString, GameSettings.AXIS_MOUSEWHEEL.primary.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310280"), delegate
		{
			EditorPartList.Instance.Refresh();
			Tutorial.GoToLastPage();
		}, () => EditorLogic.RootPart == null || (EditorLogic.RootPart.partInfo.name != "mk1pod" && EditorLogic.RootPart.partInfo.name != "mk1pod.v2"), dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_310289"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			List<Part> sortedShipList13 = EditorLogic.fetch.getSortedShipList();
			return (sortedShipList13 != null && sortedShipList13.Count == 1 && (!(sortedShipList13[0].partInfo.name != "mk1pod") || !(sortedShipList13[0].partInfo.name != "mk1pod.v2"))) ? true : false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addParachute");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310307");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_nodA);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_chute);
			EditorPartList.Instance.Refresh();
			InputLockManager.RemoveControlLock("tutorialEditor_PickPart");
			InputLockManager.RemoveControlLock("tutorialEditor_PickIcon");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310327", tutorialHighlightColorString, tutorialHighlightColorString, tutorialControlColorString, tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310328"), delegate
		{
			EditorPartList.Instance.Refresh();
			Tutorial.GoToLastPage();
		}, () => EditorLogic.RootPart == null || (EditorLogic.RootPart.partInfo.name != "mk1pod" && EditorLogic.RootPart.partInfo.name != "mk1pod.v2"), dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_310337"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			List<Part> sortedShipList12 = EditorLogic.fetch.getSortedShipList();
			return (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) == 2 && sortedShipList12 != null && sortedShipList12.Count == 2 && (!(sortedShipList12[0].partInfo.name != "mk1pod") || !(sortedShipList12[0].partInfo.name != "mk1pod.v2")) && !(sortedShipList12[0].FindAttachNode("top").attachedPart == null) && !(sortedShipList12[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle") && !(EditorLogic.SelectedPart != null)) ? true : false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("parachuteParams");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310356");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(ControlTypes.EDITOR_PAD_PICK_PLACE, "tutorialEditor_PickPart");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_PICK, "tutorialEditor_PickIcon");
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_none);
			EditorPartList.Instance.Refresh();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310376", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310377"), delegate
		{
			InputLockManager.RemoveControlLock("tutorialEditor_PickPart");
			InputLockManager.RemoveControlLock("tutorialEditor_PickIcon");
			Tutorial.GoToLastPage();
		}, delegate
		{
			List<Part> sortedShipList11 = EditorLogic.fetch.getSortedShipList();
			return (sortedShipList11 == null || sortedShipList11.Count != 2 || (sortedShipList11[0].partInfo.name != "mk1pod" && sortedShipList11[0].partInfo.name != "mk1pod.v2") || sortedShipList11[0].FindAttachNode("top").attachedPart == null || sortedShipList11[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle") ? true : false;
		}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_310392"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			List<Part> sortedShipList10 = EditorLogic.fetch.getSortedShipList();
			if (sortedShipList10 != null && sortedShipList10.Count == 2 && (!(sortedShipList10[0].partInfo.name != "mk1pod") || !(sortedShipList10[0].partInfo.name != "mk1pod.v2")) && !(sortedShipList10[0].FindAttachNode("top").attachedPart == null) && !(sortedShipList10[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle"))
			{
				ModuleParachute moduleParachute = sortedShipList10[1].Modules["ModuleParachute"] as ModuleParachute;
				if (moduleParachute.minAirPressureToOpen > 0.18f && moduleParachute.minAirPressureToOpen < 0.22f && moduleParachute.deployAltitude >= 999f)
				{
					return true;
				}
				return false;
			}
			return false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addBacc");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310414");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_PickPart");
			InputLockManager.RemoveControlLock("tutorialEditor_PickIcon");
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_bacc);
			EditorPartList.Instance.Refresh();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310434", tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310435"), delegate
		{
			Tutorial.GoToLastPage();
		}, delegate
		{
			List<Part> sortedShipList9 = EditorLogic.fetch.getSortedShipList();
			return (sortedShipList9 == null || sortedShipList9.Count < 2 || (sortedShipList9[0].partInfo.name != "mk1pod" && sortedShipList9[0].partInfo.name != "mk1pod.v2") || sortedShipList9[0].FindAttachNode("top").attachedPart == null || sortedShipList9[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle") ? true : false;
		}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_310446"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			List<Part> sortedShipList8 = EditorLogic.fetch.getSortedShipList();
			return (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) == 3 && sortedShipList8 != null && sortedShipList8.Count == 3 && (!(sortedShipList8[0].partInfo.name != "mk1pod") || !(sortedShipList8[0].partInfo.name != "mk1pod.v2")) && !(sortedShipList8[0].FindAttachNode("top").attachedPart == null) && !(sortedShipList8[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle") && !(sortedShipList8[0].FindAttachNode("bottom").attachedPart == null) && !(sortedShipList8[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster1-1") && !(EditorLogic.SelectedPart != null)) ? true : false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("removeBACC");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310465");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_false_disappointed);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_none);
			EditorPartList.Instance.Refresh();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310482", tutorialHighlightColorString, tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310483"), delegate
		{
			Tutorial.GoToLastPage();
		}, delegate
		{
			List<Part> sortedShipList7 = EditorLogic.fetch.getSortedShipList();
			return (sortedShipList7 == null || sortedShipList7.Count < 2 || (sortedShipList7[0].partInfo.name != "mk1pod" && sortedShipList7[0].partInfo.name != "mk1pod.v2") || sortedShipList7[0].FindAttachNode("top").attachedPart == null || sortedShipList7[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle") ? true : false;
		}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_310494"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			List<Part> sortedShipList6 = EditorLogic.fetch.getSortedShipList();
			return (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) == 2 && sortedShipList6 != null && sortedShipList6.Count == 2 && (!(sortedShipList6[0].partInfo.name != "mk1pod") || !(sortedShipList6[0].partInfo.name != "mk1pod.v2")) && !(sortedShipList6[0].FindAttachNode("top").attachedPart == null) && !(sortedShipList6[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle") && !(sortedShipList6[0].FindAttachNode("bottom").attachedPart != null) && !(EditorLogic.SelectedPart != null)) ? true : false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addFlea");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310513");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_flea);
			EditorPartList.Instance.Refresh();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310530", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310531"), delegate
		{
			Tutorial.GoToLastPage();
		}, delegate
		{
			List<Part> sortedShipList5 = EditorLogic.fetch.getSortedShipList();
			return (sortedShipList5 == null || sortedShipList5.Count < 2 || (sortedShipList5[0].partInfo.name != "mk1pod" && sortedShipList5[0].partInfo.name != "mk1pod.v2") || sortedShipList5[0].FindAttachNode("top").attachedPart == null || sortedShipList5[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle") ? true : false;
		}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_310542"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			List<Part> sortedShipList4 = EditorLogic.fetch.getSortedShipList();
			return (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) == 3 && sortedShipList4 != null && sortedShipList4.Count == 3 && (!(sortedShipList4[0].partInfo.name != "mk1pod") || !(sortedShipList4[0].partInfo.name != "mk1pod.v2")) && !(sortedShipList4[0].FindAttachNode("top").attachedPart == null) && !(sortedShipList4[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle") && !(sortedShipList4[0].FindAttachNode("bottom").attachedPart == null) && (!(sortedShipList4[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster_sm") || !(sortedShipList4[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster.sm") || !(sortedShipList4[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster.sm.v2")) && !(EditorLogic.SelectedPart != null)) ? true : false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("staging");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310561");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_none);
			EditorPartList.Instance.Refresh();
			InputLockManager.RemoveControlLock("tutorialEditor_Stages");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_PAD_PICK_PLACE, "tutorialEditor_PickPart");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310582"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(64f, 128f), Vector2.zero, XKCDColors.White, stagingStackIcon), new DialogGUILabel(Localizer.Format("#autoLOC_310585"), expandW: false, expandH: true), new DialogGUILayoutEnd(), new DialogGUILabel(Localizer.Format("#autoLOC_310587", tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310588"), delegate
		{
			InputLockManager.SetControlLock(ControlTypes.EDITOR_EDIT_STAGES, "tutorialEditor_Stages");
			Tutorial.GoToLastPage();
		}, delegate
		{
			List<Part> sortedShipList3 = EditorLogic.fetch.getSortedShipList();
			return (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 3 || sortedShipList3 == null || sortedShipList3.Count != 3 || (sortedShipList3[0].partInfo.name != "mk1pod" && sortedShipList3[0].partInfo.name != "mk1pod.v2") || sortedShipList3[0].FindAttachNode("top").attachedPart == null || sortedShipList3[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle" || sortedShipList3[0].FindAttachNode("bottom").attachedPart == null || (sortedShipList3[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster_sm" && sortedShipList3[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster.sm" && sortedShipList3[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster.sm.v2")) ? true : false;
		}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_310603"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => StageManager.StageCount == 2, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("staging2");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310617");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310625", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310626"), delegate
		{
			Tutorial.GoToLastPage();
		}, delegate
		{
			List<Part> sortedShipList2 = EditorLogic.fetch.getSortedShipList();
			return (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 3 || sortedShipList2 == null || sortedShipList2.Count != 3 || (sortedShipList2[0].partInfo.name != "mk1pod" && sortedShipList2[0].partInfo.name != "mk1pod.v2") || sortedShipList2[0].FindAttachNode("top").attachedPart == null || sortedShipList2[0].FindAttachNode("top").attachedPart.partInfo.name != "parachuteSingle" || sortedShipList2[0].FindAttachNode("bottom").attachedPart == null || (sortedShipList2[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster_sm" && sortedShipList2[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster.sm" && sortedShipList2[0].FindAttachNode("bottom").attachedPart.partInfo.name != "solidBooster.sm.v2")) ? true : false;
		}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_310640"), delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			if (StageManager.StageCount != 2)
			{
				return false;
			}
			List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
			Part part = null;
			Part part2 = null;
			int count = sortedShipList.Count;
			while (count-- > 0)
			{
				Part part3 = sortedShipList[count];
				if (part3.partInfo.name == "parachuteSingle")
				{
					part = part3;
				}
				else if (part3.partInfo.name == "solidBooster_sm" || part3.partInfo.name == "solidBooster.sm" || part3.partInfo.name == "solidBooster.sm.v2")
				{
					part2 = part3;
				}
			}
			return (object)part != null && (object)part2 != null && ((part.inverseStage == 0 && part2.inverseStage == 1) ? true : false);
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("nameSave1");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310672");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Name");
			InputLockManager.RemoveControlLock("tutorialEditor_Save");
			vesselName = EditorLogic.fetch.shipNameField.text;
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310686", tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310687"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => EditorLogic.fetch.shipNameField.text != vesselName, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("conclusion");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_310701");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			EditorLogic.fetch.allowSrfAttachment = true;
			EditorLogic.fetch.allowNodeAttachment = true;
			InputLockManager.RemoveControlLock("tutorialEditor_Stages");
			InputLockManager.RemoveControlLock("tutorialEditor_Name");
			InputLockManager.RemoveControlLock("tutorialEditor_Gizmos");
			InputLockManager.RemoveControlLock("tutorialEditor_Reroot");
			InputLockManager.RemoveControlLock("tutorialEditor_Modes");
			InputLockManager.RemoveControlLock("tutorialEditor_Copy");
			InputLockManager.RemoveControlLock("tutorialEditor_Save");
			if (EditorLogic.fetch != null)
			{
				EditorLogic.fetch.disallowSave = false;
			}
			InputLockManager.RemoveControlLock("tutorialEditor_PickPart");
			InputLockManager.RemoveControlLock("tutorialEditor_PickIcon");
			InputLockManager.RemoveControlLock("EditorTutorial");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_310726", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_310727"), delegate
		{
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_final);
			EditorPartList.Instance.Refresh();
			complete = true;
			CompleteTutorial(destroySelf: false);
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		if (!HighLogic.LoadedSceneIsEditor)
		{
			stateName = "welcome";
		}
		else
		{
			stateName = "insideVAB";
		}
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
		if (EditorPartList.Instance != null)
		{
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_pod);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_chute);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_bacc);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_flea);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_final);
			try
			{
				EditorPartList.Instance.Refresh();
			}
			catch
			{
			}
		}
		InputLockManager.RemoveControlLock("tutorialEditor_Stages");
		InputLockManager.RemoveControlLock("tutorialEditor_Name");
		InputLockManager.RemoveControlLock("tutorialEditor_Gizmos");
		InputLockManager.RemoveControlLock("tutorialEditor_Reroot");
		InputLockManager.RemoveControlLock("tutorialEditor_Modes");
		InputLockManager.RemoveControlLock("tutorialEditor_Copy");
		InputLockManager.RemoveControlLock("tutorialEditor_Save");
		if (EditorLogic.fetch != null)
		{
			EditorLogic.fetch.disallowSave = false;
		}
		InputLockManager.RemoveControlLock("tutorialEditor_PickPart");
		InputLockManager.RemoveControlLock("tutorialEditor_PickIcon");
		InputLockManager.RemoveControlLock("tutorialEditor_Launch");
		InputLockManager.RemoveControlLock("EditorTutorial");
	}
}
