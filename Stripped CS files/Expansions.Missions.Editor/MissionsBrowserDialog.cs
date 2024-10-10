using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions.Runtime;
using ns11;
using ns16;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MissionsBrowserDialog : MonoBehaviour
{
	public delegate void SelectFileCallback(string fullPath);

	public delegate void CancelledCallback();

	[SerializeField]
	public TextMeshProUGUI headerTitle;

	public DictionaryValueList<DialogGUIToggleButton, MissionFileInfo> items = new DictionaryValueList<DialogGUIToggleButton, MissionFileInfo>();

	public string title;

	public Texture2D currentIcon;

	public Texture2D steamIcon;

	public SelectFileCallback OnFileSelected;

	public CancelledCallback OnBrowseCancelled;

	[SerializeField]
	public Toggle tabUserCreated;

	[SerializeField]
	public Toggle tabStock;

	[SerializeField]
	public Button btnCancel;

	[SerializeField]
	public Button btnLoad;

	[SerializeField]
	public Button btnDelete;

	[SerializeField]
	public ToggleGroup listGroup;

	[SerializeField]
	public RectTransform listContainer;

	[SerializeField]
	public UISkinDefSO uiSkin;

	public UISkinDef skin;

	public PopupDialog window;

	public MissionFileInfo selectedMission;

	public MissionTypes selectedType;

	public Texture2D missionNormalIcon;

	public Texture2D missionHardIcon;

	[SerializeField]
	public List<Texture2D> missionDifficultyButtons;

	[SerializeField]
	public CanvasGroup canvasGroup;

	public DictionaryValueList<string, MissionListGroup> packGroups;

	public List<MissionPack> missionPacks;

	public Coroutine buildListCoroutine;

	public static float maxFrameTime = 0.25f;

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			onButtonCancel();
		}
	}

	public MissionsBrowserDialog Spawn(SelectFileCallback onFileSelected, CancelledCallback onCancel)
	{
		MissionImporting.ImportNewZips();
		MissionsBrowserDialog component = UnityEngine.Object.Instantiate(this).GetComponent<MissionsBrowserDialog>();
		component.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		component.OnBrowseCancelled = onCancel;
		component.OnFileSelected = onFileSelected;
		component.title = Localizer.Format("#autoLOC_8200122");
		return component;
	}

	public void Start()
	{
		skin = uiSkin.SkinDef;
		headerTitle.text = title;
		GameEvents.Mission.onMissionImported.Add(onMissionImported);
		btnDelete.onClick.AddListener(onButtonDelete);
		btnCancel.onClick.AddListener(onButtonCancel);
		btnLoad.onClick.AddListener(onButtonLoad);
		selectedType = MissionTypes.User;
		BuildMissionList();
		if (items.Count <= 0)
		{
			tabUserCreated.isOn = false;
			tabStock.isOn = true;
			OnTabStock(st: true);
		}
		else
		{
			tabUserCreated.isOn = true;
			tabStock.isOn = false;
		}
		tabUserCreated.onValueChanged.AddListener(OnTabUser);
		tabStock.onValueChanged.AddListener(OnTabStock);
	}

	public void onButtonLoad()
	{
		if (selectedMission == null)
		{
			return;
		}
		if (selectedMission.IsCompatible())
		{
			CraftProfileInfo.PrepareCraftMetaFileLoad();
			string errorString = string.Empty;
			HashSet<string> incompatibleCraftHashSet = new HashSet<string>();
			if (selectedMission.IsCraftCompatible(ref errorString, ref incompatibleCraftHashSet, checkSaveIfAvailable: false))
			{
				ConfirmLoadGame();
				return;
			}
			DialogGUILabel dialogGUILabel = new DialogGUILabel(errorString, skin.customStyles[0]);
			dialogGUILabel.bypassTextStyleColor = true;
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(4, 11, 4, 4), TextAnchor.UpperLeft, dialogGUILabel);
			dialogGUIVerticalLayout.AddChild(new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize));
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8004242"), Localizer.Format("#autoLOC_464288"), skin, 350f, new DialogGUISpace(6f), new DialogGUIScrollList(new Vector2(-1f, 80f), hScroll: false, vScroll: true, dialogGUIVerticalLayout), new DialogGUISpace(6f), new DialogGUIHorizontalLayout(TextAnchor.UpperLeft, new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_417274"), ConfirmLoadGame, 100f, -1f, true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), null, 150f, -1f, true))), persistAcrossScenes: false, skin);
		}
		else
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8004241"), Localizer.Format("#autoLOC_464288"), skin, 350f, new DialogGUIHorizontalLayout(TextAnchor.UpperLeft, new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_417274"), ConfirmLoadGame, 100f, -1f, true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), null, 150f, -1f, true))), persistAcrossScenes: false, skin);
		}
	}

	public void ConfirmLoadGame()
	{
		OnFileSelected(selectedMission.FilePath);
		Dismiss();
	}

	public void onButtonCancel()
	{
		OnBrowseCancelled();
		Dismiss();
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void Dismiss()
	{
		if (buildListCoroutine != null)
		{
			StopCoroutine(buildListCoroutine);
		}
		GameEvents.Mission.onMissionImported.Remove(onMissionImported);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void onMissionImported()
	{
		BuildMissionList();
	}

	public void BuildMissionList()
	{
		ClearListItems();
		if (buildListCoroutine != null)
		{
			StopCoroutine(buildListCoroutine);
		}
		buildListCoroutine = StartCoroutine(BuildMissionListRoutine());
	}

	public IEnumerator BuildMissionListRoutine()
	{
		List<MissionFileInfo> missionFiles2 = MissionsUtils.GatherMissionFiles(selectedType);
		float num = Time.realtimeSinceStartup + maxFrameTime;
		if (SteamManager.Initialized && selectedType == MissionTypes.User)
		{
			missionFiles2.AddRange(MissionsUtils.GatherMissionFiles(MissionTypes.Steam, excludeSteamUnsubscribed: true));
			try
			{
				missionFiles2.Sort(MissionsUtils.SortMissions);
			}
			catch (Exception)
			{
			}
		}
		if (selectedType == MissionTypes.Stock)
		{
			missionFiles2.AddRange(MissionsUtils.GatherMissionFiles(MissionTypes.Base, excludeSteamUnsubscribed: true));
		}
		missionPacks = MissionsUtils.GatherMissionPacks(selectedType);
		if (selectedType == MissionTypes.User && MissionEditorLogic.Instance != null)
		{
			MissionEditorLogic.Instance.userMissionPacks = missionPacks;
		}
		BuildPacksList();
		CraftProfileInfo.PrepareCraftMetaFileLoad();
		missionFiles2 = missionFiles2.OrderByAlphaNumeric((MissionFileInfo m) => m.title);
		int c = missionFiles2.Count;
		int i = 0;
		while (i < c)
		{
			MissionFileInfo mission = missionFiles2[i];
			DialogGUIToggleButton dialogGUIToggleButton = new DialogGUIToggleButton(set: false, string.Empty, delegate(bool b)
			{
				if (b)
				{
					selectedMission = mission;
					if (Mouse.Left.GetDoubleClick(isDelegate: true))
					{
						OnFileSelected(selectedMission.FilePath);
						Dismiss();
					}
					else
					{
						if (selectedMission != null && selectedMission.missionType == MissionTypes.Steam)
						{
							selectedMission.UpdateSteamState();
						}
						OnSelectionChanged(selectedMission != null);
					}
				}
			}, -1f, 62f);
			dialogGUIToggleButton.guiStyle = skin.customStyles[0];
			dialogGUIToggleButton.OptionInteractableCondition = () => selectedMission != null;
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(0, 0, 2, 2), TextAnchor.UpperLeft);
			CreateWidget(mission, dialogGUIVerticalLayout);
			if (mission.IsTutorialMission)
			{
				currentIcon = missionNormalIcon;
			}
			else
			{
				currentIcon = missionDifficultyButtons[(int)mission.difficulty];
			}
			if (mission.missionType == MissionTypes.Steam)
			{
				dialogGUIToggleButton.AddChild(new DialogGUIHorizontalLayout(false, false, 4f, new RectOffset(0, 8, 6, 7), TextAnchor.MiddleLeft, new DialogGUIImage(new Vector2(58f, 58f), Vector2.zero, Color.white, currentIcon), dialogGUIVerticalLayout, new DialogGUIImage(new Vector2(30f, 30f), Vector2.zero, Color.white, steamIcon)));
			}
			else
			{
				dialogGUIToggleButton.AddChild(new DialogGUIHorizontalLayout(false, false, 4f, new RectOffset(0, 8, 6, 7), TextAnchor.MiddleLeft, new DialogGUIImage(new Vector2(58f, 58f), Vector2.zero, Color.white, currentIcon), dialogGUIVerticalLayout));
			}
			items[dialogGUIToggleButton] = mission;
			if (!(Time.realtimeSinceStartup <= num))
			{
				DisplayAddedMissions();
				yield return null;
				num = Time.realtimeSinceStartup + maxFrameTime;
			}
			int num2 = i + 1;
			i = num2;
		}
		DisplayAddedMissions();
	}

	public void DisplayAddedMissions()
	{
		Dictionary<DialogGUIToggleButton, MissionFileInfo>.Enumerator dictEnumerator = items.GetDictEnumerator();
		while (dictEnumerator.MoveNext())
		{
			MissionFileInfo value = dictEnumerator.Current.Value;
			if ((value.packName == null || !packGroups.ContainsKey(value.packName) || !packGroups[value.packName].ContainsMission(value)) && !packGroups[MissionListGroup.defaultPackName].ContainsMission(value))
			{
				Stack<Transform> layouts = new Stack<Transform>();
				layouts.Push(base.transform);
				GameObject gameObject = dictEnumerator.Current.Key.Create(ref layouts, skin);
				if (value.packName != null && packGroups.ContainsKey(value.packName))
				{
					gameObject.transform.SetParent(packGroups[value.packName].containerChildren, worldPositionStays: false);
					packGroups[value.packName].AddMission(value);
				}
				else
				{
					gameObject.transform.SetParent(packGroups[MissionListGroup.defaultPackName].containerChildren, worldPositionStays: false);
					packGroups[MissionListGroup.defaultPackName].AddMission(value);
				}
				dictEnumerator.Current.Key.toggle.group = listGroup;
			}
		}
		for (int i = 0; i < packGroups.Count; i++)
		{
			if (packGroups.ValuesList[i].Missions.Count > 0 && packGroups.ValuesList[i].transform.parent == null)
			{
				packGroups.ValuesList[i].transform.SetParent(listContainer, worldPositionStays: false);
			}
		}
	}

	public void BuildPacksList()
	{
		if (packGroups == null)
		{
			packGroups = new DictionaryValueList<string, MissionListGroup>();
		}
		else
		{
			int count = packGroups.Count;
			while (count-- > 0)
			{
				UnityEngine.Object.Destroy(packGroups.ValuesList[count].gameObject);
			}
			packGroups.Clear();
		}
		if (selectedType == MissionTypes.User)
		{
			MissionListGroup missionListGroup = MissionListGroup.CreateDefault();
			packGroups.Add(missionListGroup.Pack.name, missionListGroup);
		}
		for (int i = 0; i < missionPacks.Count; i++)
		{
			MissionListGroup missionListGroup = MissionListGroup.Create(missionPacks[i]);
			if (missionListGroup != null)
			{
				packGroups.Add(missionListGroup.Pack.name, missionListGroup);
			}
		}
		if (selectedType == MissionTypes.Stock)
		{
			MissionListGroup missionListGroup = MissionListGroup.CreateDefault();
			packGroups.Add(missionListGroup.Pack.name, missionListGroup);
		}
	}

	public void CreateWidget(MissionFileInfo mission, DialogGUIVerticalLayout vLayout)
	{
		DialogGUILabel.TextLabelOptions textLabelOptions = new DialogGUILabel.TextLabelOptions
		{
			enableWordWrapping = false,
			OverflowMode = TextOverflowModes.Overflow,
			resizeBestFit = true,
			resizeMinFontSize = 11,
			resizeMaxFontSize = 12
		};
		string errorString = string.Empty;
		HashSet<string> incompatibleCraftHashSet = new HashSet<string>();
		if (mission == null)
		{
			DialogGUILabel dialogGUILabel = new DialogGUILabel(mission.title, skin.customStyles[0], expandW: true);
			dialogGUILabel.textLabelOptions = textLabelOptions;
			vLayout.AddChild(dialogGUILabel);
			vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8006019"), skin.customStyles[1], expandW: true));
		}
		else if (!mission.IsCompatible())
		{
			DialogGUILabel dialogGUILabel = new DialogGUILabel((!string.IsNullOrEmpty(mission.title)) ? mission.title : mission.folderName, skin.customStyles[0], expandW: true);
			dialogGUILabel.textLabelOptions = textLabelOptions;
			vLayout.AddChild(dialogGUILabel);
			vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8004248"), skin.customStyles[1], expandW: true)
			{
				textLabelOptions = textLabelOptions
			});
		}
		else if (!mission.IsCraftCompatible(ref errorString, ref incompatibleCraftHashSet, checkSaveIfAvailable: false))
		{
			DialogGUILabel dialogGUILabel = new DialogGUILabel((!string.IsNullOrEmpty(mission.title)) ? mission.title : mission.folderName, skin.customStyles[0], expandW: true);
			dialogGUILabel.textLabelOptions = textLabelOptions;
			vLayout.AddChild(dialogGUILabel);
			vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8004256"), skin.customStyles[1], expandW: true)
			{
				textLabelOptions = textLabelOptions
			});
		}
		else
		{
			DialogGUILabel dialogGUILabel = new DialogGUILabel((!string.IsNullOrEmpty(mission.title)) ? mission.title : mission.folderName, skin.customStyles[0], expandW: true);
			dialogGUILabel.textLabelOptions = textLabelOptions;
			vLayout.AddChild(dialogGUILabel);
			vLayout.AddChild(new DialogGUILabel(string.Format("<color=" + XKCDColors.HexFormat.ElectricLime + ">{0, -21}</color>", Localizer.Format("#autoLOC_8004152", mission.MetaMission.nodeCount)), skin.customStyles[1], expandW: true)
			{
				textLabelOptions = textLabelOptions
			});
			vLayout.AddChild(new DialogGUILabel(Localizer.Format(Localizer.Format("#autoLOC_8003014"), mission.MetaMission.vesselCount), skin.customStyles[2], expandW: true)
			{
				textLabelOptions = textLabelOptions
			});
		}
	}

	public void ClearListItems()
	{
		Dictionary<DialogGUIToggleButton, MissionFileInfo>.Enumerator dictEnumerator = items.GetDictEnumerator();
		while (dictEnumerator.MoveNext())
		{
			UnityEngine.Object.Destroy(dictEnumerator.Current.Key.uiItem);
		}
		items.Clear();
		selectedMission = null;
		OnSelectionChanged(haveSelection: false);
		MissionSystem.RemoveMissionObjects();
	}

	public void ClearSelection()
	{
		selectedMission = null;
		OnSelectionChanged(haveSelection: false);
		listGroup.SetAllTogglesOff();
	}

	public void OnSelectionChanged(bool haveSelection)
	{
		if (selectedType == MissionTypes.User)
		{
			btnDelete.gameObject.SetActive(value: true);
		}
		else
		{
			btnDelete.gameObject.SetActive(value: false);
		}
		if (haveSelection)
		{
			btnLoad.interactable = true;
			btnDelete.interactable = true;
		}
		else
		{
			btnLoad.interactable = false;
			btnDelete.interactable = false;
		}
	}

	public void onButtonDelete()
	{
		if (selectedMission != null)
		{
			PromptDeleteFileConfirm();
		}
	}

	public void PromptDeleteFileConfirm()
	{
		if (canvasGroup != null)
		{
			canvasGroup.interactable = false;
		}
		if (window != null)
		{
			window.Dismiss();
		}
		DialogGUIHorizontalLayout dialogGUIHorizontalLayout = new DialogGUIHorizontalLayout();
		dialogGUIHorizontalLayout.AddChild(new DialogGUIFlexibleSpace());
		dialogGUIHorizontalLayout.AddChild(new DialogGUIButton("<color=orange>" + Localizer.Format("#autoLOC_129950") + "</color>", delegate
		{
			if (canvasGroup != null)
			{
				canvasGroup.interactable = true;
			}
			window.Dismiss();
			OnDeleteConfirm();
		}, 80f, 30f, true));
		dialogGUIHorizontalLayout.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_464291"), delegate
		{
			if (canvasGroup != null)
			{
				canvasGroup.interactable = true;
			}
			window.Dismiss();
		}, null, 80f, 30f, dismissOnSelect: true));
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout();
		dialogGUIVerticalLayout.AddChild(new DialogGUISpace(0f));
		dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8007222", selectedMission.title), expandW: true));
		dialogGUIVerticalLayout.AddChild(new DialogGUISpace(0f));
		dialogGUIVerticalLayout.AddChild(new DialogGUIToggle(GameSettings.MISSION_DELETE_REMOVES_IN_PROGRESS_MISSIONS, Localizer.Format("#autoLOC_8003186"), delegate(bool b)
		{
			GameSettings.MISSION_DELETE_REMOVES_IN_PROGRESS_MISSIONS = b;
		}));
		dialogGUIVerticalLayout.AddChild(new DialogGUISpace(0f));
		dialogGUIVerticalLayout.AddChild(dialogGUIHorizontalLayout);
		window = PopupDialog.SpawnPopupDialog(new MultiOptionDialog("Delete Mission", "", Localizer.Format("#autoLOC_8007223"), skin, 350f, dialogGUIVerticalLayout), persistAcrossScenes: false, skin);
	}

	public void OnDeleteConfirm()
	{
		try
		{
			Directory.Delete(selectedMission.FolderPath, recursive: true);
			if (GameSettings.MISSION_DELETE_REMOVES_IN_PROGRESS_MISSIONS)
			{
				MissionsUtils.RemoveMissionSaves(selectedMission.folderName);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Unable to delete mission '{0}' - ERROR: {1}", selectedMission.title, ex.Message);
		}
		ClearSelection();
		BuildMissionList();
	}

	public void OnTabUser(bool st)
	{
		if (selectedType != MissionTypes.User)
		{
			selectedType = MissionTypes.User;
			BuildMissionList();
		}
	}

	public void OnTabStock(bool st)
	{
		if (selectedType != 0)
		{
			selectedType = MissionTypes.Stock;
			BuildMissionList();
		}
	}
}
