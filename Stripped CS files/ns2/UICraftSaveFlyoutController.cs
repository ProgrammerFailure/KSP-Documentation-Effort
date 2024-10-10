using System;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using ns11;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class UICraftSaveFlyoutController : UIHoverSlidePanel
{
	[SerializeField]
	[Tooltip("The object that holds the tab to open the flyout")]
	[Header("Inspector Assigned")]
	public XSelectable dropDownTag;

	[SerializeField]
	[Tooltip("Um?")]
	public XSelectable saveFolderSelectable;

	[SerializeField]
	[Tooltip("The prefab for the line item")]
	public EditorSaveFolderItem saveFolderPrefab;

	[SerializeField]
	[Tooltip("The object that contains the radio buttons for settting the default save location")]
	public ToggleGroup selectedToggleGroup;

	[Tooltip("The transform object that the save-folder items will parent to")]
	public RectTransform saveFolderContainer;

	public bool groupSet;

	public List<EditorSaveFolderItem> saveFolderItems;

	public GameObject craftSaveFolderBrowser;

	public DirectoryController craftSaveDirectoryController;

	public new void Start()
	{
		base.Start();
		saveFolderItems = new List<EditorSaveFolderItem>();
		locked = false;
		groupSet = false;
		saveFolderSelectable.onPointerEnter += SaveFolderSelectable_onPointerEnter;
		saveFolderSelectable.onPointerExit += SaveFolderSelectable_onPointerExit;
		SetupSaveFolderChoices();
		UpdateSelectionRadioButtons();
		GameEvents.onEditorRestart.Add(ResetItems);
	}

	public void OnDestroy()
	{
		GameEvents.onEditorRestart.Remove(ResetItems);
	}

	public void OnDisable()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
	}

	public void OnEnable()
	{
		if (!locked && !pointOver)
		{
			coroutine = StartCoroutine(MoveToState(0.1f, newState: false));
		}
	}

	public void Update()
	{
		if (UpdateSaveLocationForMissions())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		bool active = ShipConstruction.ShipConfig != null;
		saveFolderSelectable.gameObject.SetActive(active);
		dropDownTag.gameObject.SetActive(active);
	}

	public void SetupSaveFolderChoices()
	{
		EditorSaveFolderItem editorSaveFolderItem = null;
		string currentGameShipsPathFor = ShipConstruction.GetCurrentGameShipsPathFor(EditorDriver.editorFacility);
		int length = new DirectoryInfo(currentGameShipsPathFor).FullName.Length;
		List<string> subdirectoryList = GetSubdirectoryList(currentGameShipsPathFor);
		int i = 0;
		for (int count = subdirectoryList.Count; i < count; i++)
		{
			EditorSaveFolderItem editorSaveFolderItem2 = CreateFolderSaveItem(subdirectoryList[i], length);
			if (editorSaveFolderItem2.IsPath(currentGameShipsPathFor))
			{
				editorSaveFolderItem = editorSaveFolderItem2;
				string pathDisplay = Localizer.Format("#autoLOC_6009022");
				editorSaveFolderItem.SetPathDisplay(pathDisplay);
				try
				{
					string empty = string.Empty;
					empty = new DirectoryInfo(currentGameShipsPathFor).FullName;
					editorSaveFolderItem.SetPath(empty);
				}
				catch (Exception)
				{
				}
			}
		}
		if (editorSaveFolderItem != null)
		{
			editorSaveFolderItem.transform.SetAsFirstSibling();
			editorSaveFolderItem.MouseInput_SelectDefault(value: true);
		}
		string path = Localizer.Format("#autoLOC_6009021");
		EditorSaveFolderItem editorSaveFolderItem3 = CreateFolderSaveItem(path, 0);
		editorSaveFolderItem3.IsOpenFileBrowserRequest = true;
		Toggle componentInChildren = editorSaveFolderItem3.GetComponentInChildren<Toggle>();
		if (componentInChildren != null)
		{
			componentInChildren.gameObject.SetActive(value: false);
		}
	}

	public bool UpdateSaveLocationForMissions()
	{
		if (!HighLogic.CurrentGame.IsMissionMode)
		{
			return false;
		}
		if (!MissionSystem.HasMissions)
		{
			return false;
		}
		MissionFileInfo missionInfo = MissionSystem.missions[0].MissionInfo;
		string text = string.Empty;
		switch (HighLogic.CurrentGame.Mode)
		{
		case Game.Modes.MISSION_BUILDER:
			text = missionInfo.ShipFolderPath;
			break;
		case Game.Modes.MISSION:
			text = missionInfo.SaveShipFolderPath;
			break;
		}
		text += ShipConstruction.GetShipsSubfolderFor(EditorDriver.editorFacility);
		EditorDriver.SetDefaultSaveFolder(text);
		return true;
	}

	public List<string> GetSubdirectoryList(string parentPath, int depth = 0)
	{
		List<string> list = new List<string>();
		list.Add(parentPath);
		try
		{
			DirectoryInfo[] directories = new DirectoryInfo(parentPath).GetDirectories();
			foreach (DirectoryInfo directoryInfo in directories)
			{
				list.AddRange(GetSubdirectoryList(directoryInfo.FullName, depth + 1));
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
			return list;
		}
		if (depth == 0)
		{
			list.Sort();
		}
		return list;
	}

	public void ResetItems()
	{
		for (int i = 0; i < saveFolderItems.Count; i++)
		{
			selectedToggleGroup.UnregisterToggle(saveFolderItems[i].toggleSetDefault);
			saveFolderItems[i].gameObject.DestroyGameObject();
		}
		saveFolderItems.Clear();
		SetupSaveFolderChoices();
		groupSet = false;
		UpdateSelectionRadioButtons();
	}

	public void UpdateSelectionRadioButtons()
	{
		if (!groupSet)
		{
			SetupToggleGroup();
		}
		bool flag = false;
		int i = 0;
		for (int count = saveFolderItems.Count; i < count; i++)
		{
			if (saveFolderItems[i].saveFolderPath == EditorDriver.DefaultCraftSavePath)
			{
				saveFolderItems[i].toggleSetDefault.isOn = true;
				flag = true;
				break;
			}
		}
		if (!flag && saveFolderItems.Count > 0)
		{
			saveFolderItems[0].toggleSetDefault.isOn = true;
		}
	}

	public EditorSaveFolderItem CreateFolderSaveItem(string path, int leftTrimCount)
	{
		EditorSaveFolderItem editorSaveFolderItem = UnityEngine.Object.Instantiate(saveFolderPrefab);
		editorSaveFolderItem.transform.SetParent(saveFolderContainer.transform, worldPositionStays: false);
		editorSaveFolderItem.Create(this, path, path.Substring(leftTrimCount));
		editorSaveFolderItem.gameObject.SetActive(value: true);
		saveFolderItems.Add(editorSaveFolderItem);
		return editorSaveFolderItem;
	}

	public void SetupToggleGroup()
	{
		int i = 0;
		for (int count = saveFolderItems.Count; i < count; i++)
		{
			if (saveFolderItems[i].saveFolderPath == EditorDriver.DefaultCraftSavePath)
			{
				saveFolderItems[i].toggleSetDefault.isOn = true;
			}
			else
			{
				saveFolderItems[i].toggleSetDefault.isOn = false;
			}
		}
		int j = 0;
		for (int count2 = saveFolderItems.Count; j < count2; j++)
		{
			saveFolderItems[j].toggleSetDefault.group = selectedToggleGroup;
			selectedToggleGroup.RegisterToggle(saveFolderItems[j].toggleSetDefault);
		}
		groupSet = true;
	}

	public void SaveFolderSelectable_onPointerExit(XSelectable arg1, PointerEventData arg2)
	{
		pointOver = false;
		if (!locked && coroutine == null)
		{
			coroutine = StartCoroutine(MoveToState(0.1f, newState: false));
		}
	}

	public void SaveFolderSelectable_onPointerEnter(XSelectable arg1, PointerEventData arg2)
	{
		pointOver = true;
		if (!locked && coroutine == null)
		{
			coroutine = StartCoroutine(MoveToState(0f, newState: true));
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		pointOver = true;
		if (!locked && coroutine == null)
		{
			coroutine = StartCoroutine(MoveToState(0f, newState: true));
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		pointOver = false;
		if (!locked && coroutine == null)
		{
			coroutine = StartCoroutine(MoveToState(0.1f, newState: false));
		}
	}

	public void CloseFlyout()
	{
		OnPointerExit(null);
	}

	public void OpenCraftFolderBrowser()
	{
		GameObject prefab = AssetBase.GetPrefab("CraftSaveFolderBrowser");
		craftSaveFolderBrowser = UnityEngine.Object.Instantiate(prefab);
		craftSaveFolderBrowser.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		craftSaveDirectoryController = craftSaveFolderBrowser.GetComponent<DirectoryController>();
		craftSaveDirectoryController.InitForCraftSave();
	}

	public void UpdateFolderDisplay(string path)
	{
		if (!(craftSaveDirectoryController == null))
		{
			craftSaveDirectoryController.UpdateDirectoryDisplay(path);
		}
	}
}
