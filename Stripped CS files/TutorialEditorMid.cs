using System.Collections.Generic;
using System.IO;
using ns11;
using ns9;
using UnityEngine;

public class TutorialEditorMid : TutorialScenario
{
	public string stateName = "welcome";

	public bool complete;

	public string oldVesselName = "Hopper";

	public Texture2D symmIcon;

	public Texture2D snap4xIcon;

	public EditorPartListFilter<AvailablePart> tutorialFilter_none = new EditorPartListFilter<AvailablePart>("TutorialEditor_none", (AvailablePart a) => false);

	public EditorPartListFilter<AvailablePart> tutorialFilterMid = new EditorPartListFilter<AvailablePart>("TutorialEditorMid", (AvailablePart a) => a.name == "Decoupler.1" || a.name == "GooExperiment" || a.name == "fuelTankSmallFlat" || a.name == "mk1pod" || a.name == "mk1pod.v2" || a.name == "liquidEngine2" || a.name == "liquidEngine2.v2" || a.name == "parachuteSingle" || a.name == "basicFin");

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Wernher";
		SetDialogRect(new Rect(CalcDialogXRatio(), 0.85f, 420f, 190f));
		symmIcon = GameDatabase.Instance.GetTexture("Squad/Tutorials/EditorSymm", asNormalMap: false);
		snap4xIcon = GameDatabase.Instance.GetTexture("Squad/Tutorials/EditorSnap4x", asNormalMap: false);
		base.OnAssetSetup();
	}

	public bool Check_BaseVesselNoStats()
	{
		int num = 2;
		if (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 2)
		{
			return false;
		}
		List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
		if (sortedShipList != null && sortedShipList.Count == num && (!(sortedShipList[0].partInfo.name != "mk1pod") || !(sortedShipList[0].partInfo.name != "mk1pod.v2")))
		{
			Part attachedPart = sortedShipList[0].FindAttachNode("top").attachedPart;
			if (!(attachedPart == null) && !(attachedPart.partInfo.name != "parachuteSingle"))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public bool Check_BaseVessel()
	{
		int num = 2;
		if (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 2)
		{
			return false;
		}
		List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
		if (sortedShipList != null && sortedShipList.Count == num && (!(sortedShipList[0].partInfo.name != "mk1pod") || !(sortedShipList[0].partInfo.name != "mk1pod.v2")))
		{
			Part attachedPart = sortedShipList[0].FindAttachNode("top").attachedPart;
			if (!(attachedPart == null) && !(attachedPart.partInfo.name != "parachuteSingle"))
			{
				ModuleParachute moduleParachute = attachedPart.Modules["ModuleParachute"] as ModuleParachute;
				if (!(moduleParachute.minAirPressureToOpen < 0.74f) && moduleParachute.deployAltitude >= 999f)
				{
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool Check_Name()
	{
		int num = 2;
		if (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 2)
		{
			return false;
		}
		List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
		if (sortedShipList != null && sortedShipList.Count == num && (!(sortedShipList[0].partInfo.name != "mk1pod") || !(sortedShipList[0].partInfo.name != "mk1pod.v2")))
		{
			Part attachedPart = sortedShipList[0].FindAttachNode("top").attachedPart;
			if (!(attachedPart == null) && !(attachedPart.partInfo.name != "parachuteSingle"))
			{
				ModuleParachute moduleParachute = attachedPart.Modules["ModuleParachute"] as ModuleParachute;
				if (!(moduleParachute.minAirPressureToOpen < 0.74f) && moduleParachute.deployAltitude >= 999f)
				{
					if (EditorLogic.fetch.shipNameField.text == oldVesselName)
					{
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool Check_Goo()
	{
		int num = 4;
		if (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 4)
		{
			return false;
		}
		List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
		if (sortedShipList != null && sortedShipList.Count == num && (!(sortedShipList[0].partInfo.name != "mk1pod") || !(sortedShipList[0].partInfo.name != "mk1pod.v2")))
		{
			Part attachedPart = sortedShipList[0].FindAttachNode("top").attachedPart;
			if (!(attachedPart == null) && !(attachedPart.partInfo.name != "parachuteSingle"))
			{
				ModuleParachute moduleParachute = attachedPart.Modules["ModuleParachute"] as ModuleParachute;
				if (!(moduleParachute.minAirPressureToOpen < 0.74f) && moduleParachute.deployAltitude >= 999f)
				{
					if (EditorLogic.fetch.shipNameField.text == oldVesselName)
					{
						return false;
					}
					List<Part> list = sortedShipList[0].children.FindAll((Part p) => p.partInfo.name == "GooExperiment");
					if (list != null && list.Count == 2)
					{
						if (list[0].transform.position.y < sortedShipList[0].transform.position.y)
						{
							return false;
						}
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool Check_Dec()
	{
		int num = 5;
		if (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 5)
		{
			return false;
		}
		List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
		if (sortedShipList != null && sortedShipList.Count == num && (!(sortedShipList[0].partInfo.name != "mk1pod") || !(sortedShipList[0].partInfo.name != "mk1pod.v2")))
		{
			Part attachedPart = sortedShipList[0].FindAttachNode("top").attachedPart;
			if (!(attachedPart == null) && !(attachedPart.partInfo.name != "parachuteSingle"))
			{
				ModuleParachute moduleParachute = attachedPart.Modules["ModuleParachute"] as ModuleParachute;
				if (!(moduleParachute.minAirPressureToOpen < 0.74f) && moduleParachute.deployAltitude >= 999f)
				{
					if (EditorLogic.fetch.shipNameField.text == oldVesselName)
					{
						return false;
					}
					List<Part> list = sortedShipList[0].children.FindAll((Part p) => p.partInfo.name == "GooExperiment");
					if (list != null && list.Count == 2)
					{
						if (list[0].transform.position.y < sortedShipList[0].transform.position.y)
						{
							return false;
						}
						Part attachedPart2 = sortedShipList[0].FindAttachNode("bottom").attachedPart;
						if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "Decoupler.1"))
						{
							Part attachedPart3 = attachedPart2.FindAttachNode("top").attachedPart;
							if (!(attachedPart3 == null) && !(attachedPart3 != sortedShipList[0]))
							{
								return true;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool Check_Tanks()
	{
		int num = 10;
		if (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 10)
		{
			return false;
		}
		List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
		if (sortedShipList != null && sortedShipList.Count == num && (!(sortedShipList[0].partInfo.name != "mk1pod") || !(sortedShipList[0].partInfo.name != "mk1pod.v2")))
		{
			Part attachedPart = sortedShipList[0].FindAttachNode("top").attachedPart;
			if (!(attachedPart == null) && !(attachedPart.partInfo.name != "parachuteSingle"))
			{
				ModuleParachute moduleParachute = attachedPart.Modules["ModuleParachute"] as ModuleParachute;
				if (!(moduleParachute.minAirPressureToOpen < 0.74f) && moduleParachute.deployAltitude >= 999f)
				{
					if (EditorLogic.fetch.shipNameField.text == oldVesselName)
					{
						return false;
					}
					List<Part> list = sortedShipList[0].children.FindAll((Part p) => p.partInfo.name == "GooExperiment");
					if (list != null && list.Count == 2)
					{
						if (list[0].transform.position.y < sortedShipList[0].transform.position.y)
						{
							return false;
						}
						Part attachedPart2 = sortedShipList[0].FindAttachNode("bottom").attachedPart;
						if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "Decoupler.1"))
						{
							Part attachedPart3 = attachedPart2.FindAttachNode("top").attachedPart;
							if (!(attachedPart3 == null) && !(attachedPart3 != sortedShipList[0]))
							{
								Part part = attachedPart2.FindAttachNode("bottom").attachedPart;
								if (!(part == null) && !(part.partInfo.name != "fuelTankSmallFlat"))
								{
									int num2 = 3;
									while (true)
									{
										if (num2 >= 0)
										{
											if (part.children.Count == 1)
											{
												if (!(part.FindAttachNode("bottom").attachedPart == null))
												{
													part = part.children[0];
													if (part.partInfo.name != "fuelTankSmallFlat")
													{
														break;
													}
													num2--;
													continue;
												}
												return false;
											}
											return false;
										}
										if (part.children.Count != 0)
										{
											return false;
										}
										return true;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool Check_Engine()
	{
		int num = 11;
		if (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 11)
		{
			return false;
		}
		List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
		if (sortedShipList != null && sortedShipList.Count == num && (!(sortedShipList[0].partInfo.name != "mk1pod") || !(sortedShipList[0].partInfo.name != "mk1pod.v2")))
		{
			Part attachedPart = sortedShipList[0].FindAttachNode("top").attachedPart;
			if (!(attachedPart == null) && !(attachedPart.partInfo.name != "parachuteSingle"))
			{
				ModuleParachute moduleParachute = attachedPart.Modules["ModuleParachute"] as ModuleParachute;
				if (!(moduleParachute.minAirPressureToOpen < 0.74f) && moduleParachute.deployAltitude >= 999f)
				{
					if (EditorLogic.fetch.shipNameField.text == oldVesselName)
					{
						return false;
					}
					List<Part> list = sortedShipList[0].children.FindAll((Part p) => p.partInfo.name == "GooExperiment");
					if (list != null && list.Count == 2)
					{
						if (list[0].transform.position.y < sortedShipList[0].transform.position.y)
						{
							return false;
						}
						Part attachedPart2 = sortedShipList[0].FindAttachNode("bottom").attachedPart;
						if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "Decoupler.1"))
						{
							Part attachedPart3 = attachedPart2.FindAttachNode("top").attachedPart;
							if (!(attachedPart3 == null) && !(attachedPart3 != sortedShipList[0]))
							{
								Part part = attachedPart2.FindAttachNode("bottom").attachedPart;
								if (!(part == null) && !(part.partInfo.name != "fuelTankSmallFlat"))
								{
									int num2 = 3;
									while (true)
									{
										if (num2 >= 0)
										{
											if (part.children.Count == 1)
											{
												if (!(part.FindAttachNode("bottom").attachedPart == null))
												{
													part = part.children[0];
													if (part.partInfo.name != "fuelTankSmallFlat")
													{
														break;
													}
													num2--;
													continue;
												}
												return false;
											}
											return false;
										}
										if (part.children.Count != 1)
										{
											return false;
										}
										Part part2 = part.children[0];
										if (part2.partInfo.name != "liquidEngine2" && part2.partInfo.name != "liquidEngine2.v2")
										{
											return false;
										}
										if (part2.FindAttachNodeByPart(part).id != "top")
										{
											return false;
										}
										return true;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool Check_Fins()
	{
		int num = 15;
		if (EditorLogic.fetch.CountAllSceneParts(includeSelected: false) != 15)
		{
			return false;
		}
		List<Part> sortedShipList = EditorLogic.fetch.getSortedShipList();
		if (sortedShipList != null && sortedShipList.Count == num && (!(sortedShipList[0].partInfo.name != "mk1pod") || !(sortedShipList[0].partInfo.name != "mk1pod.v2")))
		{
			Part attachedPart = sortedShipList[0].FindAttachNode("top").attachedPart;
			if (!(attachedPart == null) && !(attachedPart.partInfo.name != "parachuteSingle"))
			{
				ModuleParachute moduleParachute = attachedPart.Modules["ModuleParachute"] as ModuleParachute;
				if (!(moduleParachute.minAirPressureToOpen < 0.74f) && moduleParachute.deployAltitude >= 999f)
				{
					if (EditorLogic.fetch.shipNameField.text == oldVesselName)
					{
						return false;
					}
					List<Part> list = sortedShipList[0].children.FindAll((Part p) => p.partInfo.name == "GooExperiment");
					if (list != null && list.Count == 2)
					{
						if (list[0].transform.position.y < sortedShipList[0].transform.position.y)
						{
							return false;
						}
						Part attachedPart2 = sortedShipList[0].FindAttachNode("bottom").attachedPart;
						if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "Decoupler.1"))
						{
							Part attachedPart3 = attachedPart2.FindAttachNode("top").attachedPart;
							if (!(attachedPart3 == null) && !(attachedPart3 != sortedShipList[0]))
							{
								Part part = attachedPart2.FindAttachNode("bottom").attachedPart;
								if (!(part == null) && !(part.partInfo.name != "fuelTankSmallFlat"))
								{
									int num2 = 3;
									while (true)
									{
										if (num2 >= 0)
										{
											if (part.children.Count == 1)
											{
												if (!(part.FindAttachNode("bottom").attachedPart == null))
												{
													part = part.children[0];
													if (part.partInfo.name != "fuelTankSmallFlat")
													{
														break;
													}
													num2--;
													continue;
												}
												return false;
											}
											return false;
										}
										if (part.children.Count != 5)
										{
											return false;
										}
										Part part2 = part.children[0];
										if (part2.partInfo.name != "liquidEngine2" && part2.partInfo.name != "liquidEngine2.v2")
										{
											return false;
										}
										if (part2.FindAttachNodeByPart(part).id != "top")
										{
											return false;
										}
										List<Part> list2 = new List<Part>(part.children);
										list2.Remove(part2);
										int num3 = 3;
										while (true)
										{
											if (num3 >= 0)
											{
												if (!(list2[num3].partInfo.name != "basicFin"))
												{
													if (list2[num3].partTransform.up != sortedShipList[0].partTransform.up)
													{
														break;
													}
													num3--;
													continue;
												}
												return false;
											}
											return true;
										}
										return false;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckCraftFile()
	{
		if (!Directory.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB"))
		{
			return true;
		}
		if (!File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB/Hopper.craft"))
		{
			return true;
		}
		return false;
	}

	public void CopyCraftFile()
	{
		if (!Directory.Exists(KSPUtil.ApplicationRootPath + "GameData/Squad/Tutorials") || !File.Exists(KSPUtil.ApplicationRootPath + "GameData/Squad/Tutorials/Hopper.craft"))
		{
			Debug.LogError("[Tutorial]: Intermediate Construction: Hopper.craft does not exist!");
			CloseTutorialWindow();
		}
		if (!Directory.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB"))
		{
			Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB");
		}
		File.Copy(KSPUtil.ApplicationRootPath + "GameData/Squad/Tutorials/Hopper.craft", KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB/Hopper.craft", overwrite: true);
	}

	public override void OnTutorialSetup()
	{
		if (complete)
		{
			CloseTutorialWindow();
			return;
		}
		TutorialPage tutorialPage = new TutorialPage("welcome");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314034");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInVAB = true;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314043"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState _003Cp0_003E) => HighLogic.LoadedScene == GameScenes.EDITOR);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("overview");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314055");
		tutorialPage.OnEnter = delegate
		{
			EditorDriver.fetch.SetInputLockFromGameParameters();
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilter_none);
			EditorPartList.Instance.Refresh();
			InputLockManager.SetControlLock(ControlTypes.EDITOR_GIZMO_TOOLS, "tutorialEditor_Gizmos");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ROOT_REFLOW, "tutorialEditor_Reroot");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_MODE_SWITCH, "tutorialEditor_Modes");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_LAUNCH, "tutorialEditor_Launch");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_SAVE, "tutorialEditor_Save");
			if (EditorLogic.fetch != null)
			{
				EditorLogic.fetch.disallowSave = true;
			}
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_NEW | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_EDIT_STAGES | ControlTypes.EDITOR_UNDO_REDO, "tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314077"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314078"), delegate
		{
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			CopyCraftFile();
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("newLoad");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314093");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Most");
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilterMid);
			EditorPartList.Instance.Refresh();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314106", tutorialControlColorString, tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314107"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_BaseVesselNoStats() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		tutorialPage.OnUpdate = delegate
		{
			if (CheckCraftFile())
			{
				CopyCraftFile();
			}
		};
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("fixChute");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314126");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Most");
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilterMid);
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilterMid);
			EditorPartList.Instance.Refresh();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314140"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314141"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_BaseVessel() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("rename");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314156");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314166"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314167"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_Name() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addGoo");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314181");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314189", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314190"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_Name() && (EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE || PartCategorizer.Instance.filterFunction.button.activeButton.Value) && PartCategorizer.Instance.subcategoryFunctionScience.button.activeButton.Value && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("gooTest");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314205");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314213"), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(92f, 92f), Vector2.zero, XKCDColors.White, symmIcon), new DialogGUILabel(Localizer.Format("#autoLOC_314216", tutorialHighlightColorString, tutorialControlColorString, GameSettings.Editor_toggleSymMode.name), expandW: false, expandH: true), new DialogGUILayoutEnd(), new DialogGUILabel(Localizer.Format("#autoLOC_314218", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314219"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_Goo() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addDecoupler");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314233");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodA);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314242", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314243"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_Dec() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("autostageExplain");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314257");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314266", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314267"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_Dec() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("placeTanks");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314281");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314289", tutorialControlColorString, GameSettings.MODIFIER_KEY.name, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314290"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_Tanks() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("placeEngine");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314304");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314312", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314313"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_Engine() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("placeFins");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314327");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314335", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(92f, 92f), Vector2.zero, XKCDColors.White, snap4xIcon), new DialogGUILabel(Localizer.Format("#autoLOC_314338", tutorialControlColorString, GameSettings.Editor_toggleAngleSnap.name), expandW: false, expandH: true), new DialogGUILayoutEnd(), new DialogGUILabel(Localizer.Format("#autoLOC_314340", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314341"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => Check_Fins() && EditorLogic.SelectedPart == null, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("conclusion");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_314355");
		tutorialPage.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			InputLockManager.RemoveControlLock("tutorialEditor_Gizmos");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_314364", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_314365"), delegate
		{
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilterMid);
			EditorPartList.Instance.Refresh();
			complete = true;
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		if (!HighLogic.LoadedSceneIsEditor)
		{
			stateName = "welcome";
		}
		else
		{
			stateName = "overview";
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
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilterMid);
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
		InputLockManager.RemoveControlLock("tutorialEditor_Most");
		InputLockManager.RemoveControlLock("tutorialEditor_Launch");
	}
}
