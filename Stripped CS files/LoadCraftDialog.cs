using System;
using System.Collections.Generic;
using System.IO;
using Expansions;
using ns9;
using UnityEngine;
using UnityEngine.UI;

public class LoadCraftDialog : MonoBehaviour
{
	public delegate void SelectFileCallback(string fullPath, LoadType t);

	public delegate void CancelCallback();

	public enum LoadType
	{
		Normal,
		Merge
	}

	public class CraftEntry
	{
		public string name;

		public string fullFilePath;

		public int partCount;

		public int stageCount;

		public bool isStock;

		public bool isValid;

		public ShipTemplate template;

		public VersionCompareResult compatibility;

		public Texture2D thumbnail;

		public string thumbURL;

		public CraftEntry(FileInfo fInfo, bool stock)
		{
			name = fInfo.Name.Replace(fInfo.Extension, "");
			fullFilePath = fInfo.FullName;
			ConfigNode configNode = ConfigNode.Load(fullFilePath);
			template = new ShipTemplate();
			template.LoadShip(configNode);
			isStock = stock;
			isValid = template.shipPartsUnlocked;
			partCount = template.partCount;
			string text = new FileInfo(fullFilePath).Directory.Name;
			if (isStock)
			{
				thumbURL = "Ships/@thumbs/" + text + "/" + Path.GetFileNameWithoutExtension(fullFilePath);
			}
			else
			{
				thumbURL = "thumbs/" + HighLogic.SaveFolder + "_" + text + "_" + Path.GetFileNameWithoutExtension(fullFilePath);
			}
			if (fullFilePath.Contains("SquadExpansion"))
			{
				List<ExpansionsLoader.ExpansionInfo> installedExpansions = ExpansionsLoader.GetInstalledExpansions();
				string text2 = "";
				for (int i = 0; i < installedExpansions.Count; i++)
				{
					text2 = KSPExpansionsUtils.ExpansionsGameDataPath + installedExpansions[i].FolderName + "/" + thumbURL;
					if (fullFilePath.Contains(installedExpansions[i].FolderName))
					{
						thumbnail = ShipConstruction.GetThumbnail(text2, fullPath: true, addFileExt: true);
					}
				}
			}
			else
			{
				thumbnail = ShipConstruction.GetThumbnail(thumbURL);
			}
			stageCount = template.stageCount;
			if (configNode.HasValue("version"))
			{
				compatibility = KSPUtil.CheckVersion(configNode.GetValue("version"), ShipConstruct.lastCompatibleMajor, ShipConstruct.lastCompatibleMinor, ShipConstruct.lastCompatibleRev);
			}
		}
	}

	public string windowTitle = "Select a craft to load";

	public Texture2D FileIcon;

	public string profileName = "";

	public SelectFileCallback OnFileSelected;

	public CancelCallback OnBrowseCancelled;

	public EditorFacility facility;

	public List<CraftEntry> craftList;

	public bool promptFileCleanup;

	public UISkinDef skin;

	public PopupDialog window;

	public Rect windowRect;

	public CraftEntry selectedEntry;

	public float previousSelectionTime = -1f;

	public static LoadCraftDialog Create(EditorFacility facility, string profile, SelectFileCallback onFileSelected, CancelCallback onCancel, bool showMergeOption)
	{
		LoadCraftDialog loadCraftDialog = new GameObject("Craft Browser").AddComponent<LoadCraftDialog>();
		loadCraftDialog.facility = facility;
		loadCraftDialog.profileName = profile;
		loadCraftDialog.OnFileSelected = onFileSelected;
		loadCraftDialog.OnBrowseCancelled = onCancel;
		loadCraftDialog.windowRect = new Rect(0.5f, 0.5f, 350f, 500f);
		loadCraftDialog.skin = UISkinManager.GetSkin("KSP window 7");
		loadCraftDialog.BuildCraftList();
		loadCraftDialog.ShowWindowBrowser();
		return loadCraftDialog;
	}

	public void ShowWindowBrowser()
	{
		if (window != null)
		{
			window.Dismiss();
		}
		if (promptFileCleanup)
		{
			ShowWindowCleanupPrompt();
			return;
		}
		selectedEntry = null;
		window = PopupDialog.SpawnPopupDialog(new MultiOptionDialog("LoadCraft", "", windowTitle, skin, windowRect, CreateBrowserDialog()), persistAcrossScenes: false, skin);
	}

	public DialogGUIBase CreateBrowserDialog()
	{
		DialogGUIToggleButton dialogGUIToggleButton = new DialogGUIToggleButton(() => facility == EditorFacility.const_1, "VAB", delegate
		{
			facility = EditorFacility.const_1;
			ShowWindowBrowser();
		}, 60f, 24f);
		DialogGUIToggleButton dialogGUIToggleButton2 = new DialogGUIToggleButton(() => facility == EditorFacility.const_2, "SPH", delegate
		{
			facility = EditorFacility.const_2;
			ShowWindowBrowser();
		}, 60f, 24f);
		DialogGUIHorizontalLayout child = new DialogGUIHorizontalLayout(60f, 24f, dialogGUIToggleButton, dialogGUIToggleButton2);
		Vector2 size = new Vector2(344f, 425f);
		RectOffset padding = new RectOffset(0, 4, 4, 4);
		Vector2 cellSize = new Vector2(320f, 64f);
		Vector2 spacing = new Vector2(0f, 0f);
		DialogGUIBase[] obj = new DialogGUIBase[2]
		{
			new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, useParentSize: true),
			null
		};
		DialogGUIToggle[] toggles = CreateBrowserItems();
		obj[1] = new DialogGUIToggleGroup(toggles);
		DialogGUIScrollList child2 = new DialogGUIScrollList(size, hScroll: false, vScroll: true, new DialogGUIGridLayout(padding, cellSize, spacing, GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal, TextAnchor.UpperLeft, GridLayoutGroup.Constraint.FixedColumnCount, 1, obj));
		DialogGUIButton dialogGUIButton = new DialogGUIButton("Delete", delegate
		{
			ShowDeleteFileConfirm();
		});
		dialogGUIButton.OptionInteractableCondition = () => selectedEntry != null && !selectedEntry.isStock;
		DialogGUIButton dialogGUIButton2 = new DialogGUIButton("Cancel", delegate
		{
			if (OnBrowseCancelled != null)
			{
				OnBrowseCancelled();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		});
		DialogGUIButton dialogGUIButton3 = new DialogGUIButton("Load", delegate
		{
			OnFileSelected(selectedEntry.fullFilePath, LoadType.Normal);
			UnityEngine.Object.Destroy(base.gameObject);
		});
		dialogGUIButton3.OptionInteractableCondition = () => selectedEntry != null;
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(sw: true, sh: true);
		dialogGUIVerticalLayout.AddChild(child);
		dialogGUIVerticalLayout.AddChild(child2);
		dialogGUIVerticalLayout.AddChild(new DialogGUIHorizontalLayout(dialogGUIButton, dialogGUIButton2, dialogGUIButton3));
		return dialogGUIVerticalLayout;
	}

	public DialogGUIToggleButton[] CreateBrowserItems()
	{
		BuildCraftList();
		List<DialogGUIToggleButton> list = new List<DialogGUIToggleButton>();
		int i = 0;
		for (int count = craftList.Count; i < count; i++)
		{
			CraftEntry entry = craftList[i];
			int craftIndex = i;
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(4, 4, 4, 4), TextAnchor.UpperLeft);
			dialogGUIVerticalLayout.AddChild(new DialogGUILabel(entry.name, skin.customStyles[0], expandW: true));
			if (entry.compatibility == VersionCompareResult.COMPATIBLE && entry.isValid)
			{
				dialogGUIVerticalLayout.AddChild(new DialogGUILabel("<color=#ffffff>" + entry.partCount + ((entry.partCount == 1) ? " part" : " parts") + " in " + entry.stageCount + ((entry.stageCount == 1) ? " stage." : " stages.") + "</color>", skin.customStyles[0], expandW: true));
				dialogGUIVerticalLayout.AddChild(new DialogGUILabel("<color=#ffffff>Cost: " + entry.template.totalCost + "</color>", skin.customStyles[0], expandW: true));
				if (entry.template.shipPartsExperimental)
				{
					dialogGUIVerticalLayout.AddChild(new DialogGUILabel("<color=#8dffec>  *** " + Localizer.Format("#autoLOC_6003094") + " ***</color>", skin.customStyles[0], expandW: true));
				}
			}
			else if (!entry.isValid)
			{
				dialogGUIVerticalLayout.AddChild(new DialogGUILabel("<color=#ffffff>" + entry.partCount + ((entry.partCount == 1) ? " part" : " parts") + " in " + entry.stageCount + ((entry.stageCount == 1) ? " stage." : " stages.") + "</color>", skin.customStyles[0], expandW: true));
				dialogGUIVerticalLayout.AddChild(new DialogGUILabel("<color=#ffffff>" + Localizer.Format("#autoLOC_6003100", entry.template.totalCost) + "</color> ", skin.customStyles[0], expandW: true));
				dialogGUIVerticalLayout.AddChild(new DialogGUILabel("<color=#db6227>  *** " + Localizer.Format("#autoLOC_6003097") + " ***</color>", skin.customStyles[0], expandW: true));
			}
			else
			{
				dialogGUIVerticalLayout.AddChild(new DialogGUILabel("<color=#db6227>  *** " + Localizer.Format("#autoLOC_6003098") + " ***</color>", skin.customStyles[0], expandW: true));
			}
			DialogGUIToggleButton dialogGUIToggleButton = new DialogGUIToggleButton(() => selectedEntry == craftList[craftIndex], "", delegate
			{
				SelectItem(craftIndex);
			});
			dialogGUIToggleButton.AddChild(dialogGUIVerticalLayout);
			dialogGUIToggleButton.OptionInteractableCondition = () => entry.compatibility == VersionCompareResult.COMPATIBLE;
			list.Add(dialogGUIToggleButton);
		}
		return list.ToArray();
	}

	public void SelectItem(int craftIndex)
	{
		if (selectedEntry == craftList[craftIndex] && previousSelectionTime + 0.2f >= Time.realtimeSinceStartup)
		{
			OnFileSelected(selectedEntry.fullFilePath, LoadType.Normal);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			previousSelectionTime = Time.realtimeSinceStartup;
			selectedEntry = craftList[craftIndex];
		}
	}

	public void BuildCraftList()
	{
		string shipsSubfolderFor = ShipConstruction.GetShipsSubfolderFor(facility);
		craftList = new List<CraftEntry>();
		if (HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels)
		{
			FileInfo[] files = new DirectoryInfo(KSPUtil.ApplicationRootPath + "Ships/" + shipsSubfolderFor).GetFiles("*.craft");
			int num = files.Length;
			for (int i = 0; i < num; i++)
			{
				CraftEntry craftEntry = new CraftEntry(files[i], stock: true);
				craftEntry.name += Localizer.Format("#autoLOC_482705");
				craftList.Add(craftEntry);
				if (craftEntry.compatibility != VersionCompareResult.COMPATIBLE)
				{
					promptFileCleanup = true;
				}
			}
			if (ExpansionsLoader.IsAnyExpansionInstalled())
			{
				List<ExpansionsLoader.ExpansionInfo> installedExpansions = ExpansionsLoader.GetInstalledExpansions();
				for (int j = 0; j < installedExpansions.Count; j++)
				{
					FileInfo[] files2 = new DirectoryInfo(KSPExpansionsUtils.ExpansionsGameDataPath + installedExpansions[j].FolderName + "/Ships/" + shipsSubfolderFor).GetFiles("*.craft");
					num = files.Length;
					for (int k = 0; k < num; k++)
					{
						CraftEntry craftEntry2 = new CraftEntry(files2[k], stock: true);
						craftEntry2.name += Localizer.Format("#autoLOC_482705");
						craftList.Add(craftEntry2);
						if (craftEntry2.compatibility != VersionCompareResult.COMPATIBLE)
						{
							promptFileCleanup = true;
						}
					}
				}
			}
		}
		if (profileName != null || profileName != string.Empty)
		{
			FileInfo[] files3 = new DirectoryInfo(KSPUtil.ApplicationRootPath + "saves/" + profileName + "/Ships/" + shipsSubfolderFor).GetFiles("*.craft");
			int num2 = files3.Length;
			for (int l = 0; l < num2; l++)
			{
				CraftEntry craftEntry3 = new CraftEntry(files3[l], stock: false);
				craftList.Add(craftEntry3);
				if (craftEntry3.compatibility != VersionCompareResult.COMPATIBLE)
				{
					promptFileCleanup = true;
				}
			}
		}
		craftList.Sort((CraftEntry a, CraftEntry b) => string.Compare(a.name, b.name));
	}

	public void ShowWindowCleanupPrompt()
	{
		if (window != null)
		{
			window.Dismiss();
		}
		window = PopupDialog.SpawnPopupDialog(new MultiOptionDialog("CraftCleanup", "", "Cleanup", skin, CreateWindowCleanupPrompt()), persistAcrossScenes: false, skin);
	}

	public DialogGUIBase CreateWindowCleanupPrompt()
	{
		DialogGUIHorizontalLayout dialogGUIHorizontalLayout = new DialogGUIHorizontalLayout();
		dialogGUIHorizontalLayout.AddChild(new DialogGUIButton("Delete", delegate
		{
			DeleteInvalidSaves();
			promptFileCleanup = false;
			ShowWindowBrowser();
		}));
		dialogGUIHorizontalLayout.AddChild(new DialogGUIButton("Not Now", delegate
		{
			promptFileCleanup = false;
			ShowWindowBrowser();
		}));
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout();
		dialogGUIVerticalLayout.AddChild(new DialogGUILabel("There are incompatible craft files in the folder.\n\nWould you like to delete them?", expandW: true));
		dialogGUIVerticalLayout.AddChild(dialogGUIHorizontalLayout);
		return dialogGUIVerticalLayout;
	}

	public void DeleteInvalidSaves()
	{
		int i = 0;
		for (int count = craftList.Count; i < count; i++)
		{
			CraftEntry craftEntry = craftList[i];
			if (craftEntry.compatibility != VersionCompareResult.COMPATIBLE)
			{
				Debug.Log("Deleting craft " + craftEntry.name);
				try
				{
					File.Delete(craftEntry.fullFilePath);
				}
				catch (Exception ex)
				{
					PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Delete Failed", "Cannot delete " + craftEntry.name, ex.Message, "Ok", persistAcrossScenes: false, UISkinManager.GetSkin("MainMenuSkin"));
				}
			}
		}
		BuildCraftList();
	}

	public void ShowDeleteFileConfirm()
	{
		if (window != null)
		{
			window.Dismiss();
		}
		window = PopupDialog.SpawnPopupDialog(new MultiOptionDialog("ConfirmFileDelete", "", "Delete File", skin, CreateDeleteFileConfirm()), persistAcrossScenes: false, skin);
	}

	public DialogGUIBase CreateDeleteFileConfirm()
	{
		DialogGUIHorizontalLayout dialogGUIHorizontalLayout = new DialogGUIHorizontalLayout();
		dialogGUIHorizontalLayout.AddChild(new DialogGUIButton("Delete", delegate
		{
			OnDeleteConfirm();
			ShowWindowBrowser();
		}));
		dialogGUIHorizontalLayout.AddChild(new DialogGUIButton("No", delegate
		{
			ShowWindowBrowser();
		}));
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout();
		dialogGUIVerticalLayout.AddChild(new DialogGUILabel("Do you want to delete this craft?", expandW: true));
		dialogGUIVerticalLayout.AddChild(dialogGUIHorizontalLayout);
		return dialogGUIVerticalLayout;
	}

	public void OnDeleteConfirm()
	{
		if (File.Exists(KSPUtil.ApplicationRootPath + selectedEntry.thumbURL + ".png"))
		{
			File.Delete(KSPUtil.ApplicationRootPath + selectedEntry.thumbURL + ".png");
		}
		File.Delete(selectedEntry.fullFilePath);
	}

	public void OnDestroy()
	{
		if (window != null)
		{
			window.Dismiss();
		}
	}
}
