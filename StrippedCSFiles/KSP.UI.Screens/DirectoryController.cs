using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

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
	private Color gameNameDirectoryColor;

	[Tooltip("The color of the subfolder name text within a game directory")]
	[SerializeField]
	private Color subdirectoryColor;

	private const string saveGameFile = "persistent.sfs";

	private const string stockFolder = "Stock";

	private const string missionsFolder = "missions";

	private const string scenariosFolder = "scenarios";

	private const string trainingFolder = "training";

	private const string testMissionsFolder = "test_missions";

	private const string savesFolder = "saves";

	private const string shipsFolder = "Ships";

	private const string folderSeparator = "/";

	private const string vabFolder = "VAB";

	private const string sphFolder = "SPH";

	private readonly string[] hiddenFolders;

	private bool isEnabledThisFrame;

	private GameObject contentArea;

	private string rootFolderPath;

	private string currentGameDirectoryFolder;

	private DirectoryActionGroup currentSelectedDirectory;

	private DirectoryActionGroup previousSelectedDirectory;

	private List<DirectoryActionGroup> directoryActionGroups;

	private CraftBrowserDialog craftBrowserDialog;

	private DirectoryActionGroup overriddenDisplay;

	public string CurrentSelectedDirectoryPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool IsPlayerCraftDirectorySelected
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsStockDirectorySelected
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsSteamDirectorySelected
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EditorFacility DisplayedFacility
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string StockDirectoryPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string MissionCraftPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool IsStockCraftAvailable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool IsTrainingScenario
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DirectoryController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CraftListUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OverrideDisplay(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DirectoryActionGroup FindDirectory(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RestoreDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAllDirectoryDisplays()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDirectoryDisplay(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDirectoryLayout(GameObject directory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectDirectory(DirectoryActionGroup selectedDirectory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeselectCurrentDirectory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> ExpansionDirectories()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildMissionDirectoryUI(EditorFacility editorFacility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildDirectoryUI(EditorFacility editorFacility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowDirectoryTreeForEditor(EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCurrentlySelectedByEditor(Transform editorFolder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnScroll(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitForCraftSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDirectoryControlsBySelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DirectoryCreatedCallback(string createdPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DirectoryDeletedCallback(string deletePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMissionDirectory(Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildGameNameDirectories(Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GameObject CreateDirectory(string name, string directoryPath, bool isStockDirectory = false, bool isSteamDirectory = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GameObject CreateEditorDirectory(EditorFacility editorType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildSubdirectories(Transform parentDirectoryTransform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildStockDirectoryUI(EditorFacility editorFacility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildSteamDirectoryUI(EditorFacility editorFacility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PrioritizeCurrentGameDirectory(Transform editorFolder)
	{
		throw null;
	}
}
