using System;
using System.Collections.Generic;
using System.IO;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class DirectoryController : MonoBehaviour
{
	[Header("Inspector Assigned")]
	[Tooltip("The GameObject within the ScrollView that displays the directory tree")]
	public GameObject DirectoryStructureContent;

	[Tooltip("The ScrollView object that contains the Content area")]
	public ScrollRect DirectoryStructureScrollView;

	[Tooltip("The prefab object that defines a \"directory\" group")]
	public GameObject DirectoryGroupPrefab;

	[Tooltip("The prefab object that defines a not-displayed directory entry")]
	public GameObject HiddenDirectoryPrefab;

	[Tooltip("The GameObject button control for creating a new directory folder")]
	public GameObject CreateDirectoryButton;

	[Tooltip("The GameObject button control for deleting directory folders")]
	public GameObject DeleteDirectoryButton;

	[SerializeField]
	[Tooltip("The color of the top-level game and mission directory text")]
	public Color gameNameDirectoryColor = Color.white;

	[Tooltip("The color of the subfolder name text within a game directory")]
	[SerializeField]
	public Color subdirectoryColor = new Color(0.8f, 0.8f, 0.8f, 1f);

	public const string saveGameFile = "persistent.sfs";

	public const string stockFolder = "Stock";

	public const string missionsFolder = "missions";

	public const string scenariosFolder = "scenarios";

	public const string trainingFolder = "training";

	public const string testMissionsFolder = "test_missions";

	public const string savesFolder = "saves";

	public const string shipsFolder = "Ships";

	public const string folderSeparator = "/";

	public const string vabFolder = "VAB";

	public const string sphFolder = "SPH";

	public readonly string[] hiddenFolders = new string[7]
	{
		"saves".ToUpper(),
		"VAB".ToUpper(),
		"SPH".ToUpper(),
		"missions".ToUpper(),
		"scenarios".ToUpper(),
		"training".ToUpper(),
		"test_missions".ToUpper()
	};

	public bool isEnabledThisFrame;

	public GameObject contentArea;

	public string rootFolderPath;

	public string currentGameDirectoryFolder;

	public DirectoryActionGroup currentSelectedDirectory;

	public DirectoryActionGroup previousSelectedDirectory;

	public List<DirectoryActionGroup> directoryActionGroups;

	public CraftBrowserDialog craftBrowserDialog;

	public DirectoryActionGroup overriddenDisplay;

	public string CurrentSelectedDirectoryPath { get; set; }

	public bool IsPlayerCraftDirectorySelected
	{
		get
		{
			try
			{
				return !IsStockDirectorySelected && !IsSteamDirectorySelected;
			}
			catch (Exception)
			{
			}
			return false;
		}
	}

	public bool IsStockDirectorySelected
	{
		get
		{
			if (currentSelectedDirectory != null)
			{
				return currentSelectedDirectory.IsStockDirectory;
			}
			return false;
		}
	}

	public bool IsSteamDirectorySelected
	{
		get
		{
			if (currentSelectedDirectory != null)
			{
				return currentSelectedDirectory.IsSteamDirectory;
			}
			return false;
		}
	}

	public EditorFacility DisplayedFacility
	{
		get
		{
			Transform obj = contentArea.transform.Find(ShipConstruction.GetShipsSubfolderFor(EditorFacility.const_1));
			Transform transform = contentArea.transform.Find(ShipConstruction.GetShipsSubfolderFor(EditorFacility.const_2));
			if (obj.gameObject.activeInHierarchy)
			{
				return EditorFacility.const_1;
			}
			if (transform.gameObject.activeInHierarchy)
			{
				return EditorFacility.const_2;
			}
			return EditorFacility.None;
		}
	}

	public string StockDirectoryPath => rootFolderPath + "Ships/";

	public string MissionCraftPath
	{
		get
		{
			string result = string.Empty;
			if (MissionSystem.HasMissions)
			{
				MissionFileInfo missionInfo = MissionSystem.missions[0].MissionInfo;
				switch (HighLogic.CurrentGame.Mode)
				{
				case Game.Modes.MISSION_BUILDER:
					result = missionInfo.ShipFolderPath;
					break;
				case Game.Modes.MISSION:
					result = missionInfo.SaveShipFolderPath;
					break;
				}
			}
			return result;
		}
	}

	public static bool IsStockCraftAvailable
	{
		get
		{
			bool allowStockVessels = HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels;
			bool flag = HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX || HighLogic.CurrentGame.Mode == Game.Modes.CAREER;
			if (!((HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX) | (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)))
			{
				return flag && allowStockVessels;
			}
			return true;
		}
	}

	public static bool IsTrainingScenario
	{
		get
		{
			if (HighLogic.SaveFolder == null)
			{
				return false;
			}
			return HighLogic.SaveFolder.StartsWith("training");
		}
	}

	public void Awake()
	{
		directoryActionGroups = new List<DirectoryActionGroup>();
		craftBrowserDialog = GetComponent<CraftBrowserDialog>();
		contentArea = DirectoryStructureContent;
		rootFolderPath = KSPUtil.ApplicationRootPath;
		CurrentSelectedDirectoryPath = rootFolderPath + "/saves/" + HighLogic.SaveFolder + "/Ships/";
		if (HighLogic.CurrentGame.IsMissionMode)
		{
			BuildMissionDirectoryUI(EditorFacility.const_1);
			BuildMissionDirectoryUI(EditorFacility.const_2);
		}
		else
		{
			BuildDirectoryUI(EditorFacility.const_1);
			BuildDirectoryUI(EditorFacility.const_2);
		}
		if (!IsTrainingScenario)
		{
			BuildStockDirectoryUI(EditorFacility.const_1);
			BuildStockDirectoryUI(EditorFacility.const_2);
			BuildSteamDirectoryUI(EditorFacility.const_1);
			BuildSteamDirectoryUI(EditorFacility.const_2);
		}
		UpdateAllDirectoryDisplays();
		if (CreateDirectoryButton != null)
		{
			Button component = CreateDirectoryButton.GetComponent<Button>();
			if (component != null)
			{
				component.interactable = !IsTrainingScenario;
				component.onClick.AddListener(delegate
				{
					UIFolderManagementDialog.Spawn(UIFolderManagementDialog.FolderAction.Create, CurrentSelectedDirectoryPath, DirectoryCreatedCallback);
				});
			}
		}
		if (!(DeleteDirectoryButton != null))
		{
			return;
		}
		Button component2 = DeleteDirectoryButton.GetComponent<Button>();
		if (component2 != null)
		{
			component2.interactable = !IsTrainingScenario;
			component2.onClick.AddListener(delegate
			{
				UIFolderManagementDialog.Spawn(UIFolderManagementDialog.FolderAction.Delete, CurrentSelectedDirectoryPath, DirectoryDeletedCallback);
			});
		}
	}

	public void OnEnable()
	{
		UpdateAllDirectoryDisplays();
		isEnabledThisFrame = true;
	}

	public void Update()
	{
		if (isEnabledThisFrame)
		{
			isEnabledThisFrame = false;
			CraftListUpdate();
		}
	}

	public void CraftListUpdate()
	{
		if (!(currentSelectedDirectory == null) && !(craftBrowserDialog == null))
		{
			DirectoryActionGroup selectedDirectory = currentSelectedDirectory;
			currentSelectedDirectory = null;
			SelectDirectory(selectedDirectory);
		}
	}

	public void OverrideDisplay(string path)
	{
		if (craftBrowserDialog.IsSubdirectorySearch || craftBrowserDialog.IsAllGameSearch)
		{
			RestoreDisplay();
			DirectoryActionGroup directoryActionGroup = (overriddenDisplay = FindDirectory(path));
			while (directoryActionGroup != null)
			{
				directoryActionGroup.ExpandOverride(overrideOn: true);
				directoryActionGroup = directoryActionGroup.Parent;
			}
		}
	}

	public DirectoryActionGroup FindDirectory(string path)
	{
		DirectoryActionGroup directoryActionGroup = null;
		int i = 0;
		for (int count = directoryActionGroups.Count; i < count; i++)
		{
			DirectoryActionGroup directoryActionGroup2 = directoryActionGroups[i];
			if (path.IndexOf(directoryActionGroup2.Path) == 0 && (directoryActionGroup == null || directoryActionGroup2.Path.Length > directoryActionGroup.Path.Length))
			{
				directoryActionGroup = directoryActionGroup2;
			}
		}
		return directoryActionGroup;
	}

	public void RestoreDisplay()
	{
		while (overriddenDisplay != null)
		{
			overriddenDisplay.ExpandOverride(overrideOn: false);
			overriddenDisplay = overriddenDisplay.Parent;
		}
	}

	public void UpdateAllDirectoryDisplays()
	{
		List<int> list = new List<int>();
		int i = 0;
		for (int count = directoryActionGroups.Count; i < count; i++)
		{
			if (directoryActionGroups[i] == null)
			{
				list.Add(i);
			}
			else
			{
				directoryActionGroups[i].UpdateFileCount();
			}
		}
		int num = list.Count;
		int num2 = 0;
		while (num > num2)
		{
			int index = list[num - 1];
			directoryActionGroups.RemoveAt(index);
			num--;
		}
	}

	public void UpdateDirectoryDisplay(string path)
	{
		DirectoryActionGroup directoryActionGroup = FindDirectory(path);
		if (!(directoryActionGroup == null))
		{
			directoryActionGroup.UpdateFileCount();
		}
	}

	public void UpdateDirectoryLayout(GameObject directory)
	{
		Transform parent = directory.transform;
		while (parent.gameObject != contentArea)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(parent as RectTransform);
			parent = parent.parent;
		}
	}

	public void SelectDirectory(DirectoryActionGroup selectedDirectory)
	{
		if (selectedDirectory == currentSelectedDirectory)
		{
			return;
		}
		DeselectCurrentDirectory();
		CurrentSelectedDirectoryPath = selectedDirectory.Path;
		previousSelectedDirectory = currentSelectedDirectory;
		currentSelectedDirectory = selectedDirectory;
		currentSelectedDirectory.Select();
		if (!(craftBrowserDialog == null))
		{
			if (currentSelectedDirectory.IsSteamDirectory)
			{
				craftBrowserDialog.BuildSteamSubscribedCraftList();
			}
			else if (currentSelectedDirectory.IsStockDirectory)
			{
				craftBrowserDialog.BuildStockCraftList();
			}
			else
			{
				craftBrowserDialog.BuildPlayerCraftList();
			}
			UpdateDirectoryControlsBySelected();
			CraftSearch.Instance.StopSearch();
		}
	}

	public void DeselectCurrentDirectory()
	{
		if (!(currentSelectedDirectory == null))
		{
			bool num = currentSelectedDirectory.IsStockDirectory || currentSelectedDirectory.IsSteamDirectory;
			currentSelectedDirectory.Deselect();
			currentSelectedDirectory = null;
			if (num)
			{
				SelectDirectory(previousSelectedDirectory);
			}
		}
	}

	public List<string> ExpansionDirectories()
	{
		List<string> list = new List<string>();
		if (ExpansionsLoader.IsAnyExpansionInstalled())
		{
			List<ExpansionsLoader.ExpansionInfo> installedExpansions = ExpansionsLoader.GetInstalledExpansions();
			for (int i = 0; i < installedExpansions.Count; i++)
			{
				string item = KSPExpansionsUtils.ExpansionsGameDataPath + installedExpansions[i].FolderName + "/Ships/";
				list.Add(item);
			}
		}
		return list;
	}

	public void BuildMissionDirectoryUI(EditorFacility editorFacility)
	{
		GameObject gameObject = CreateEditorDirectory(editorFacility);
		gameObject.transform.SetParent(contentArea.transform, worldPositionStays: false);
		BuildMissionDirectory(gameObject.transform);
		DirectoryActionGroup component = gameObject.transform.GetChild(0).GetComponent<DirectoryActionGroup>();
		directoryActionGroups.Add(component);
		int i = 0;
		for (int count = directoryActionGroups.Count; i < count; i++)
		{
			directoryActionGroups[i].UpdateExpansionIcon();
		}
	}

	public void BuildDirectoryUI(EditorFacility editorFacility)
	{
		GameObject gameObject = CreateEditorDirectory(editorFacility);
		gameObject.transform.SetParent(contentArea.transform, worldPositionStays: false);
		BuildGameNameDirectories(gameObject.transform);
		int i = 0;
		for (int childCount = gameObject.transform.childCount; i < childCount; i++)
		{
			Transform child = gameObject.transform.GetChild(i);
			DirectoryActionGroup component = child.GetComponent<DirectoryActionGroup>();
			directoryActionGroups.Add(component);
			if (!(component == null))
			{
				BuildSubdirectories(child);
				component.Collapse(collapseSubdirectories: true);
			}
		}
		int j = 0;
		for (int count = directoryActionGroups.Count; j < count; j++)
		{
			directoryActionGroups[j].UpdateExpansionIcon();
		}
		PrioritizeCurrentGameDirectory(gameObject.transform);
	}

	public void ShowDirectoryTreeForEditor(EditorFacility facility)
	{
		int i = 0;
		for (int childCount = contentArea.transform.childCount; i < childCount; i++)
		{
			Transform child = contentArea.transform.GetChild(i);
			bool active = child.name.Equals(facility.ToString(), StringComparison.OrdinalIgnoreCase);
			child.gameObject.SetActive(active);
			if (child.gameObject.activeInHierarchy && child.childCount > 0)
			{
				UpdateCurrentlySelectedByEditor(child);
			}
		}
	}

	public void UpdateCurrentlySelectedByEditor(Transform editorFolder)
	{
		currentSelectedDirectory = null;
		int i = 0;
		for (int childCount = editorFolder.childCount; i < childCount; i++)
		{
			DirectoryActionGroup component = editorFolder.GetChild(i).GetComponent<DirectoryActionGroup>();
			if (!(component == null))
			{
				component = component.GetSelected;
				if (!(component == null))
				{
					SelectDirectory(component);
					break;
				}
			}
		}
		if (!(currentSelectedDirectory == null))
		{
			return;
		}
		int num = 0;
		int childCount2 = editorFolder.childCount;
		DirectoryActionGroup component2;
		while (true)
		{
			if (num < childCount2)
			{
				component2 = editorFolder.GetChild(0).GetComponent<DirectoryActionGroup>();
				if (!(component2 == null))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		SelectDirectory(component2);
	}

	public void OnScroll(PointerEventData data)
	{
		DirectoryStructureScrollView.OnScroll(data);
	}

	public void InitForCraftSave()
	{
		ShowDirectoryTreeForEditor(EditorDriver.editorFacility);
		Transform obj = contentArea.transform.Find(EditorDriver.editorFacility.ToString());
		obj.GetChild(obj.childCount - 1).gameObject.SetActive(value: false);
		Button component = CreateDirectoryButton.GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
		Button component2 = DeleteDirectoryButton.GetComponent<Button>();
		component2.onClick.RemoveAllListeners();
		component2.onClick.AddListener(delegate
		{
			DirectoryActionGroup directoryActionGroup = currentSelectedDirectory;
			if (!(directoryActionGroup == null))
			{
				DirectoryActionGroup directoryActionGroup2 = directoryActionGroup;
				while (directoryActionGroup2.Parent != null)
				{
					directoryActionGroup2 = directoryActionGroup2.Parent;
				}
				ShipConstruction.SaveShipToPath(directoryActionGroup2.IsCurrentGame ? HighLogic.SaveFolder : directoryActionGroup2.name, localPath: directoryActionGroup.Path.Substring(directoryActionGroup2.Path.Length), shipName: EditorLogic.fetch.shipNameField.text, editorFacility: EditorDriver.editorFacility);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		});
	}

	public void UpdateDirectoryControlsBySelected()
	{
		if (HighLogic.CurrentGame.IsMissionMode)
		{
			CreateDirectoryButton.GetComponent<Button>().interactable = false;
			DeleteDirectoryButton.GetComponent<Button>().interactable = false;
		}
		else
		{
			bool flag = currentSelectedDirectory.IsStockDirectory || currentSelectedDirectory.IsSteamDirectory;
			CreateDirectoryButton.GetComponent<Button>().interactable = !flag && !IsTrainingScenario;
			DeleteDirectoryButton.GetComponent<Button>().interactable = !currentSelectedDirectory.IsTopLevelDirectory;
		}
	}

	public void DirectoryCreatedCallback(string createdPath)
	{
		string[] array = createdPath.Split("/"[0]);
		string text = array[array.Length - 1];
		GameObject gameObject = CreateDirectory(text, createdPath);
		DirectoryActionGroup component = gameObject.GetComponent<DirectoryActionGroup>();
		component.SetLabelColor(subdirectoryColor);
		DirectoryActionGroup directoryActionGroup = currentSelectedDirectory;
		directoryActionGroup.AddItemToContent(gameObject.transform);
		directoryActionGroup.UpdateExpansionIcon();
		directoryActionGroups.Add(component);
		UpdateDirectoryLayout(gameObject);
		UnityEngine.Object.FindObjectOfType<UICraftSaveFlyoutController>().ResetItems();
		CraftSearch.Instance.StopSearch();
	}

	public void DirectoryDeletedCallback(string deletePath)
	{
		GameObject gameObject = currentSelectedDirectory.gameObject;
		DirectoryActionGroup component = gameObject.GetComponent<DirectoryActionGroup>();
		if (directoryActionGroups.Contains(component))
		{
			directoryActionGroups.Remove(component);
		}
		DirectoryActionGroup parent = currentSelectedDirectory.GetComponent<DirectoryActionGroup>().Parent;
		SelectDirectory(parent);
		parent.RemoveItemFromContent(gameObject.transform);
		parent.UpdateExpansionIcon();
		UnityEngine.Object.Destroy(gameObject);
		UpdateDirectoryLayout(parent.gameObject);
		UnityEngine.Object.FindObjectOfType<UICraftSaveFlyoutController>().ResetItems();
	}

	public void BuildMissionDirectory(Transform parent)
	{
		string directoryPath = MissionCraftPath + parent.name;
		string text = Localizer.Format("#autoLOC_6009023");
		GameObject obj = CreateDirectory(text, directoryPath);
		obj.transform.SetParent(parent, worldPositionStays: false);
		obj.GetComponent<DirectoryActionGroup>().SetLabelColor(gameNameDirectoryColor);
	}

	public void BuildGameNameDirectories(Transform parent)
	{
		DirectoryInfo[] array = null;
		string text = rootFolderPath + "saves";
		if (IsTrainingScenario)
		{
			text += "/training";
		}
		try
		{
			array = new DirectoryInfo(text).GetDirectories();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
			return;
		}
		array = array.OrderByAlphanumeric((DirectoryInfo d) => d.Name);
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			DirectoryInfo directoryInfo = array[i];
			if (Array.IndexOf(hiddenFolders, directoryInfo.Name.ToUpper()) <= -1 && directoryInfo.GetFiles("persistent.sfs").Length != 0 && (!IsTrainingScenario || !(directoryInfo.FullName != Path.GetFullPath(rootFolderPath + "saves/" + HighLogic.SaveFolder))))
			{
				string directoryPath = directoryInfo.FullName + "/Ships/" + parent.name;
				GameObject obj = CreateDirectory(IsTrainingScenario ? Localizer.Format("#autoLOC_1900256") : directoryInfo.Name, directoryPath);
				obj.transform.SetParent(parent, worldPositionStays: false);
				obj.GetComponent<DirectoryActionGroup>().SetLabelColor(gameNameDirectoryColor);
			}
		}
	}

	public GameObject CreateDirectory(string name, string directoryPath, bool isStockDirectory = false, bool isSteamDirectory = false)
	{
		GameObject obj = UnityEngine.Object.Instantiate(DirectoryGroupPrefab);
		obj.GetComponent<DirectoryActionGroup>().Initialize(name, directoryPath, startCollapsed: true, this, isStockDirectory, isSteamDirectory);
		return obj;
	}

	public GameObject CreateEditorDirectory(EditorFacility editorType)
	{
		GameObject obj = UnityEngine.Object.Instantiate(HiddenDirectoryPrefab);
		obj.name = editorType.ToString();
		return obj;
	}

	public void BuildSubdirectories(Transform parentDirectoryTransform)
	{
		DirectoryActionGroup component = parentDirectoryTransform.GetComponent<DirectoryActionGroup>();
		string path = component.Path;
		DirectoryInfo[] array = null;
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			array = directoryInfo.GetDirectories();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
			return;
		}
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			DirectoryInfo directoryInfo2 = array[i];
			GameObject gameObject = CreateDirectory(directoryInfo2.Name, directoryInfo2.FullName);
			DirectoryActionGroup component2 = gameObject.GetComponent<DirectoryActionGroup>();
			component2.SetLabelColor(subdirectoryColor);
			component.AddItemToContent(gameObject.transform);
			directoryActionGroups.Add(component2);
			BuildSubdirectories(gameObject.transform);
		}
	}

	public void BuildStockDirectoryUI(EditorFacility editorFacility)
	{
		if (IsStockCraftAvailable)
		{
			string shipsSubfolderFor = ShipConstruction.GetShipsSubfolderFor(editorFacility);
			Transform parent = contentArea.transform.Find(shipsSubfolderFor);
			string directoryPath = StockDirectoryPath + shipsSubfolderFor;
			GameObject obj = CreateDirectory(Localizer.Format("#autoLOC_6009025"), directoryPath, isStockDirectory: true);
			obj.transform.SetParent(parent, worldPositionStays: false);
			obj.GetComponent<DirectoryActionGroup>().SetLabelColor(gameNameDirectoryColor);
			obj.transform.SetSiblingIndex(1);
		}
	}

	public void BuildSteamDirectoryUI(EditorFacility editorFacility)
	{
		string shipsSubfolderFor = ShipConstruction.GetShipsSubfolderFor(editorFacility);
		Transform parent = contentArea.transform.Find(shipsSubfolderFor);
		GameObject obj = CreateDirectory(Localizer.Format("#autoLOC_8002139"), string.Empty, isStockDirectory: false, isSteamDirectory: true);
		obj.transform.SetParent(parent, worldPositionStays: false);
		obj.GetComponent<DirectoryActionGroup>().SetLabelColor(gameNameDirectoryColor);
		obj.transform.SetSiblingIndex(2);
	}

	public void PrioritizeCurrentGameDirectory(Transform editorFolder)
	{
		currentGameDirectoryFolder = HighLogic.SaveFolder;
		int num = 0;
		int childCount = editorFolder.childCount;
		Transform child;
		while (true)
		{
			if (num < childCount)
			{
				child = editorFolder.GetChild(num);
				if (string.Equals(child.name, currentGameDirectoryFolder, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		child.SetAsFirstSibling();
		DirectoryActionGroup component = child.GetComponent<DirectoryActionGroup>();
		if (!(component == null))
		{
			string text = Localizer.Format("#autoLOC_6009023") + " - " + currentGameDirectoryFolder;
			component.SetName(text);
			component.IsCurrentGame = true;
			component.AddExtraSpacing();
			bool flag = false;
			try
			{
				flag = new DirectoryInfo(CurrentSelectedDirectoryPath + "/" + editorFolder.name).GetDirectories().Length != 0;
			}
			catch (Exception)
			{
			}
			if (flag)
			{
				component.Expand(expandSubdirectories: true);
			}
			else
			{
				component.Collapse(collapseSubdirectories: true);
			}
		}
	}
}
