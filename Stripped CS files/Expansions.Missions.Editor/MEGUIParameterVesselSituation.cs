using System;
using System.Collections.Generic;
using Expansions.Missions.Actions;
using Expansions.Missions.Runtime;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_VesselSituation]
public class MEGUIParameterVesselSituation : MEGUICompoundParameter
{
	public delegate void DelegateCallback(MEGUIParameterVesselSituation parameterReference);

	public RectTransform containerHeader;

	public RectTransform containerDropDowns;

	public RectTransform containerButtons;

	public Image imageHeader;

	public Image imageBackGround;

	public TMP_InputField inputVesselName;

	public Toggle togglePlayerBuilt;

	public Toggle toggleCustomDefined;

	public TMP_Dropdown dropDownVessels;

	public Button buttonSetCrew;

	public Button buttonReset;

	public Button buttonRemove;

	public Button buttonCreateVessel;

	public ToggleGroup toggleGroupVesselType;

	public Button buttonExpand;

	[SerializeField]
	public GameObject CollapsibleGroup;

	[SerializeField]
	public GameObject CollapsibleButtonIcon;

	public VesselSituation vesselSituation;

	public MissionSituation missionSituation;

	public MEGUIParameterCheckbox autoPopulateCrew;

	public MEGUIParameterTextArea vesselDescription;

	public MEGUIParameterCheckbox focusOnSpawn;

	public MEGUIParameterVesselLocation vesselGUILocation;

	public MEGUIParameterDynamicModuleList vesselRestrictionsListGUI;

	public MEGUIParameterSwitchCompound vesselpartFilter;

	public int dropdownVesselsIndex;

	public List<string> dropdownCraftLookup;

	public TextMeshProUGUI btnCreateVesselText;

	public bool btnCreateVesseltoEdit;

	public bool goToVAB = true;

	public VesselSituation FieldValue
	{
		get
		{
			return vesselSituation;
		}
		set
		{
			vesselSituation = value;
			if (field != null)
			{
				field.SetValue(value);
			}
		}
	}

	public override void Setup(string name, object value)
	{
		base.Setup(name, value);
		missionSituation = MissionEditorLogic.Instance.EditorMission.situation;
		vesselSituation = value as VesselSituation;
		if (subParameters.ContainsKey("location"))
		{
			vesselGUILocation = subParameters["location"] as MEGUIParameterVesselLocation;
			if (containerDropDowns != null)
			{
				vesselGUILocation.transform.SetParent(containerDropDowns);
			}
		}
		if (subParameters.ContainsKey("vesselRestrictionList"))
		{
			vesselRestrictionsListGUI = subParameters["vesselRestrictionList"] as MEGUIParameterDynamicModuleList;
			vesselRestrictionsListGUI.transform.SetParent(containerButtons);
			vesselRestrictionsListGUI.GetComponent<RectTransform>().SetAsFirstSibling();
		}
		if (subParameters.ContainsKey("focusonSpawn"))
		{
			focusOnSpawn = subParameters["focusonSpawn"] as MEGUIParameterCheckbox;
			if (focusOnSpawn != null)
			{
				ActionCreateVessel actionCreateVessel = field.host as ActionCreateVessel;
				if (actionCreateVessel != null && actionCreateVessel.node.IsDockedToStartNode)
				{
					focusOnSpawn.gameObject.SetActive(value: false);
				}
			}
		}
		if (subParameters.ContainsKey("autoGenerateCrew"))
		{
			autoPopulateCrew = subParameters["autoGenerateCrew"] as MEGUIParameterCheckbox;
			if (autoPopulateCrew != null)
			{
				autoPopulateCrew.transform.SetParent(containerDropDowns);
				buttonSetCrew.gameObject.SetActive(!autoPopulateCrew.FieldValue);
				autoPopulateCrew.toggle.onValueChanged.AddListener(ToggleAction_AutoPopulateCrew);
			}
		}
		if (subParameters.ContainsKey("vesselDescription"))
		{
			vesselDescription = subParameters["vesselDescription"] as MEGUIParameterTextArea;
			if (vesselDescription != null)
			{
				vesselDescription.transform.SetParent(CollapsibleGroup.transform);
				vesselDescription.transform.SetSiblingIndex(toggleGroupVesselType.gameObject.transform.GetSiblingIndex());
			}
		}
		setupVessels();
		btnCreateVesselText = buttonCreateVessel.GetComponentInChildren<TextMeshProUGUI>();
		buttonCreateVessel.gameObject.SetActive(value: true);
		buttonCreateVessel.onClick.AddListener(ButtonAction_CreateVessel);
		if (dropdownCraftLookup.Contains(vesselSituation.craftFile))
		{
			dropDownVessels.value = dropdownCraftLookup.IndexOf(vesselSituation.craftFile) + 1;
			dropDownVessels.Select();
			dropDownVessels.RefreshShownValue();
			dropdownVesselsIndex = dropDownVessels.value;
			if (btnCreateVesselText != null)
			{
				btnCreateVesselText.text = Localizer.Format("#autoLOC_8100284");
				btnCreateVesseltoEdit = true;
			}
		}
		inputVesselName.text = Localizer.Format(vesselSituation.vesselName);
		if (vesselSituation.playerCreated)
		{
			ToggleAction_PlayerBuilt(vesselSituation.playerCreated);
			togglePlayerBuilt.isOn = true;
			toggleCustomDefined.isOn = false;
		}
		else
		{
			ToggleAction_PlayerBuilt(vesselSituation.playerCreated);
			toggleCustomDefined.isOn = true;
			togglePlayerBuilt.isOn = false;
		}
		title.text = name;
		buttonExpand.onClick.AddListener(toggleExpand);
		buttonSetCrew.onClick.AddListener(ButtonAction_SetCrew);
		buttonReset.onClick.AddListener(ButtonAction_ResetVessel);
		toggleGroupVesselType.RegisterToggle(togglePlayerBuilt);
		toggleGroupVesselType.RegisterToggle(toggleCustomDefined);
		togglePlayerBuilt.onValueChanged.AddListener(ToggleAction_PlayerBuilt);
		dropDownVessels.onValueChanged.AddListener(DropDownAction_Vessels);
		inputVesselName.onEndEdit.AddListener(InputTextAction_SetVesselName);
		GameEvents.Mission.onBuilderMissionDifficultyChanged.Add(setupVessels);
		base.TabStop = true;
	}

	public new void OnDestroy()
	{
		GameEvents.Mission.onBuilderMissionDifficultyChanged.Remove(setupVessels);
	}

	public override void Display()
	{
		base.Display();
		vesselGUILocation.Display();
	}

	public void setupVessels()
	{
		if (dropdownCraftLookup == null)
		{
			dropdownCraftLookup = new List<string>();
		}
		dropDownVessels.options.Clear();
		dropdownCraftLookup.Clear();
		dropDownVessels.options.Add(new TMP_Dropdown.OptionData("#autoLOC_8100038"));
		Mission editorMission = MissionEditorLogic.Instance.EditorMission;
		for (int i = 0; i < editorMission.craftFileList.Count; i++)
		{
			string text = editorMission.craftFileList.At(i).VesselName;
			if (!string.IsNullOrEmpty(text))
			{
				if (MissionEditorLogic.Instance.incompatibleCraft.Contains(editorMission.craftFileList.At(i).craftFile))
				{
					text = Localizer.Format("#autoLOC_8004245", text);
				}
				dropDownVessels.options.Add(new TMP_Dropdown.OptionData(text, null));
				dropdownCraftLookup.Add(editorMission.craftFileList.At(i).craftFile);
			}
		}
	}

	public void InputTextAction_SetVesselName(string newTitle)
	{
		MissionEditorHistory.PushUndoAction(this, onHistorysetVesselName);
		FieldValue.vesselName = newTitle;
		UpdateNodeBodyUI();
		for (int i = 0; i < vesselSituation.mission.mappedVessels.Count; i++)
		{
			if (vesselSituation.mission.mappedVessels[i].currentVesselPersistentId == vesselSituation.persistentId)
			{
				vesselSituation.mission.mappedVessels[i].situationVesselName = newTitle;
			}
		}
		GameEvents.Mission.onVesselSituationChanged.Fire();
	}

	public void ToggleAction_PlayerBuilt(bool value)
	{
		MissionEditorHistory.PushUndoAction(this, onHistoryTogglePlayerBuilt);
		if (value)
		{
			vesselSituation.mission.RemapVesselId(vesselSituation.persistentId);
			FieldValue.craftFile = string.Empty;
			for (int i = 0; i < vesselSituation.vesselCrew.Count; i++)
			{
				for (int j = 0; j < vesselSituation.vesselCrew[i].crewNames.Count; j++)
				{
					ProtoCrewMember protoCrewMember = missionSituation.crewRoster[vesselSituation.vesselCrew[i].crewNames[j]];
					if (protoCrewMember != null)
					{
						protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Available;
					}
				}
			}
			vesselSituation.vesselCrew = new List<Crew>();
			if (vesselGUILocation != null)
			{
				if (vesselGUILocation.selectedStartSituation == MissionSituation.VesselStartSituations.PRELAUNCH)
				{
					vesselGUILocation.facilityDropDown.gameObject.SetActive(value: true);
					vesselGUILocation.launchsiteDropDown.gameObject.SetActive(value: true);
				}
				else
				{
					vesselGUILocation.facilityDropDown.gameObject.SetActive(value: true);
					vesselGUILocation.launchsiteDropDown.gameObject.SetActive(value: false);
				}
			}
			if (dropDownVessels != null)
			{
				dropDownVessels.value = 0;
				dropDownVessels.gameObject.SetActive(value: false);
			}
			vesselRestrictionsListGUI.gameObject.SetActive(value: true);
			autoPopulateCrew.gameObject.SetActive(value: false);
		}
		else
		{
			if (dropDownVessels != null)
			{
				dropDownVessels.gameObject.SetActive(value: true);
			}
			if (vesselGUILocation != null)
			{
				if (vesselGUILocation.selectedStartSituation == MissionSituation.VesselStartSituations.PRELAUNCH)
				{
					vesselGUILocation.facilityDropDown.gameObject.SetActive(value: false);
					vesselGUILocation.launchsiteDropDown.gameObject.SetActive(value: true);
				}
				else
				{
					vesselGUILocation.facilityDropDown.gameObject.SetActive(value: false);
					vesselGUILocation.launchsiteDropDown.gameObject.SetActive(value: false);
				}
			}
			vesselRestrictionsListGUI.gameObject.SetActive(value: false);
			autoPopulateCrew.gameObject.SetActive(value: true);
		}
		FieldValue.playerCreated = value;
		buttonSetCrew.gameObject.SetActive(!value && !autoPopulateCrew.FieldValue);
		GameEvents.Mission.onVesselSituationChanged.Fire();
	}

	public void DropDownAction_Vessels(int index)
	{
		MissionEditorHistory.PushUndoAction(this, onHistoryDropDownVessels);
		bool flag = index != 0;
		if (index == 0)
		{
			btnCreateVesseltoEdit = false;
		}
		MissionCraft missionCraft = null;
		if (dropDownVessels.options.Count - 1 >= index)
		{
			missionCraft = MissionEditorLogic.Instance.EditorMission.GetMissionCraftByName(dropDownVessels.options[index].text);
			if (missionCraft == null)
			{
				FieldValue.craftFile = string.Empty;
				btnCreateVesseltoEdit = false;
			}
			else
			{
				FieldValue.craftFile = missionCraft.craftFile;
				btnCreateVesseltoEdit = true;
			}
			dropdownVesselsIndex = index;
		}
		if (btnCreateVesseltoEdit)
		{
			btnCreateVesselText.text = "#autoLOC_8100284";
		}
		else
		{
			btnCreateVesselText.text = "#autoLOC_8100285";
		}
		buttonSetCrew.gameObject.SetActive(flag && !autoPopulateCrew.FieldValue);
		vesselSituation.mission.RebuildCraftFileList();
		vesselSituation.mission.ProcessMappedVesselNodeMap();
		if (missionCraft != null && missionCraft.rootPartPersistentId != 0 && !vesselSituation.mission.mappedVessels.Exists(vesselSituation.persistentId, missionCraft.rootPartPersistentId, missionCraft.VesselName, missionCraft.craftFile))
		{
			MissionMappedVessel item = new MissionMappedVessel(missionCraft.rootPartPersistentId, vesselSituation.persistentId, vesselSituation.persistentId, missionCraft.VesselName, missionCraft.craftFile, vesselSituation.vesselName);
			vesselSituation.mission.mappedVessels.Add(item);
		}
		GameEvents.Mission.onVesselSituationChanged.Fire();
	}

	public void ButtonAction_SetCrew()
	{
		MissionCraft missionCraftByName = MissionEditorLogic.Instance.EditorMission.GetMissionCraftByName(dropDownVessels.options[dropdownVesselsIndex].text);
		if (missionCraftByName == null)
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8100286", 5f);
			return;
		}
		MissionEditorLogic.Instance.Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: true, "missionBuilder_SetVesselCrew");
		MissionEditorHistory.PushUndoAction(this, OnHistorySetCrew);
		MissionEditorLogic.Instance.crewAssignmentDialog.Spawn(missionCraftByName, vesselSituation.vesselCrew, vesselSituation.SetVesselCrew, delegate
		{
			MissionEditorLogic.Instance.Unlock("missionBuilder_SetVesselCrew");
		});
	}

	public void ButtonAction_ResetVessel()
	{
		MissionEditorHistory.PushUndoAction(this, onHistoryResetVessel);
		FieldValue.location.vesselGroundLocation.targetBody = FlightGlobals.GetHomeBody();
		FieldValue.location.orbitSnapShot.ReferenceBodyIndex = FlightGlobals.GetHomeBodyIndex();
		togglePlayerBuilt.isOn = true;
		if (vesselGUILocation != null)
		{
			vesselGUILocation.facilityDropDown.OnParameterReset();
			vesselGUILocation.launchsiteDropDown.OnParameterReset();
			vesselGUILocation.situationDropDown.OnParameterReset();
		}
		dropDownVessels.value = 0;
		dropdownVesselsIndex = 0;
		inputVesselName.text = Localizer.Format("#autoLOC_8100086");
		vesselSituation.vesselName = Localizer.Format(inputVesselName.text);
		GameEvents.Mission.onVesselSituationChanged.Fire();
	}

	public void toggleExpand()
	{
		bool flag = !CollapsibleGroup.activeSelf;
		CollapsibleGroup.SetActive(flag);
		CollapsibleButtonIcon.transform.eulerAngles = (flag ? new Vector3(0f, 0f, 180f) : Vector3.zero);
	}

	public void ButtonAction_CreateVessel()
	{
		if (MissionEditorLogic.Instance.EditorMission.IsTutorialMission)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003182"));
			return;
		}
		MissionEditorLogic.Instance.dialogSelectEditor = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ConfirmEditorSelection", Localizer.Format("#autoLOC_8100279"), Localizer.Format("#autoLOC_8100280"), null, 350f, new DialogGUISpace(5f), new DialogGUIButton(Localizer.Format("#autoLOC_8100281"), delegate
		{
			goToVAB = true;
			MissionEditorLogic.Instance.SaveMission(CreateVessel_SaveCompleted);
		}, 240f, 30f, true), new DialogGUIButton(Localizer.Format("#autoLOC_8100282"), delegate
		{
			goToVAB = false;
			MissionEditorLogic.Instance.SaveMission(CreateVessel_SaveCompleted);
		}, 240f, 30f, true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
		{
		}, 240f, 30f, true)), persistAcrossScenes: false, null);
	}

	public void CreateVessel_SaveCompleted()
	{
		if (MissionEditorLogic.Instance.EditorMission.MissionInfo != null)
		{
			StartCoroutine(MissionSystem.Instance.SetupMissionGame(MissionEditorLogic.Instance.EditorMission.MissionInfo, playMission: false, testMode: false, onCreateVesselMissionSetupSuccess));
		}
	}

	public void onCreateVesselMissionSetupSuccess()
	{
		if (btnCreateVesseltoEdit)
		{
			MissionSystem.missionToEdit = MissionEditorLogic.Instance.EditorMission.MissionInfo;
			EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_FILE;
			MissionCraft missionCraftByFileName = MissionEditorLogic.Instance.EditorMission.GetMissionCraftByFileName(FieldValue.craftFile);
			if (missionCraftByFileName != null)
			{
				EditorDriver.StartAndLoadVessel(missionCraftByFileName.craftFolder + missionCraftByFileName.craftFile, goToVAB ? EditorFacility.const_1 : EditorFacility.const_2);
				return;
			}
		}
		MissionSystem.missionToEdit = MissionEditorLogic.Instance.EditorMission.MissionInfo;
		EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.START_CLEAN;
		EditorDriver.StartEditor(goToVAB ? EditorFacility.const_1 : EditorFacility.const_2);
	}

	public void ToggleAction_AutoPopulateCrew(bool selected)
	{
		buttonSetCrew.gameObject.SetActive(!selected);
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("craftFile", FieldValue.craftFile);
		configNode.AddValue("persistentId", FieldValue.persistentId);
		configNode.AddValue("vesseldropvalue", dropdownVesselsIndex);
		configNode.AddValue("playerCreated", FieldValue.playerCreated);
		configNode.AddValue("vesselName", FieldValue.vesselName);
		if (missionSituation != null)
		{
			if (missionSituation.startVesselID == 0)
			{
				missionSituation.startVesselID = FlightGlobals.GetUniquepersistentId();
			}
			configNode.AddValue("startVessel", missionSituation.startVesselID);
			configNode.AddValue("startType", missionSituation.startType);
		}
		configNode.AddValue("targetBody", FieldValue.location.vesselGroundLocation.targetBody);
		configNode.AddValue("targetBodyIndex", FieldValue.location.orbitSnapShot.ReferenceBodyIndex);
		for (int i = 0; i < FieldValue.vesselCrew.Count; i++)
		{
			FieldValue.vesselCrew[i].Save(configNode.AddNode("PARTCREW"));
		}
		MissionEditorLogic.Instance.EditorMission.situation.crewRoster.Save(configNode);
		return configNode;
	}

	public void onHistorysetVesselName(ConfigNode data, HistoryType type)
	{
		data.TryGetValue("vesselName", ref FieldValue.vesselName);
		data.TryGetValue("startVessel", ref missionSituation.startVesselID);
		inputVesselName.text = Localizer.Format(FieldValue.vesselName);
	}

	public void onHistoryDropDownVessels(ConfigNode data, HistoryType type)
	{
		int value = 0;
		data.TryGetValue("vesseldropvalue", ref value);
		dropDownVessels.value = value;
		dropDownVessels.Select();
		dropDownVessels.RefreshShownValue();
	}

	public void onHistoryResetVessel(ConfigNode data, HistoryType type)
	{
		data.TryGetValue("craftFile", ref FieldValue.craftFile);
		data.TryGetValue("persistentId", ref FieldValue.persistentId);
		int value = 0;
		data.TryGetValue("vesseldropvalue", ref value);
		dropDownVessels.value = value;
		dropDownVessels.Select();
		dropDownVessels.RefreshShownValue();
		data.TryGetValue("playerCreated", ref FieldValue.playerCreated);
		if (FieldValue.playerCreated)
		{
			ToggleAction_PlayerBuilt(FieldValue.playerCreated);
		}
		else
		{
			ToggleAction_PlayerBuilt(FieldValue.playerCreated);
		}
		data.TryGetValue("vesselName", ref FieldValue.vesselName);
		inputVesselName.text = Localizer.Format(FieldValue.vesselName);
		data.TryGetValue("startVessel", ref missionSituation.startVesselID);
		string value2 = "";
		data.TryGetValue("targetBody", ref value2);
		FieldValue.location.vesselGroundLocation.targetBody = FlightGlobals.GetBodyByName(value2);
		data.TryGetValue("targetBodyIndex", ref FieldValue.location.orbitSnapShot.ReferenceBodyIndex);
		string value3 = "";
		data.TryGetValue("startType", ref value3);
		if (!string.IsNullOrEmpty(value3))
		{
			missionSituation.startType = (MissionSituation.StartTypeEnum)Enum.Parse(typeof(MissionSituation.StartTypeEnum), value3);
		}
	}

	public void onHistoryTogglePlayerBuilt(ConfigNode data, HistoryType type)
	{
		bool value = false;
		data.TryGetValue("playerCreated", ref value);
		ToggleAction_PlayerBuilt(value);
		togglePlayerBuilt.isOn = value;
		toggleCustomDefined.isOn = !value;
		string value2 = "";
		data.TryGetValue("startType", ref value2);
		if (!string.IsNullOrEmpty(value2))
		{
			missionSituation.startType = (MissionSituation.StartTypeEnum)Enum.Parse(typeof(MissionSituation.StartTypeEnum), value2);
		}
	}

	public void OnHistorySetCrew(ConfigNode data, HistoryType type)
	{
		List<Crew> list = new List<Crew>();
		for (int i = 0; i < data.nodes.Count; i++)
		{
			ConfigNode configNode = data.nodes[i];
			string text = configNode.name;
			if (text == "PARTCREW")
			{
				Crew crew = new Crew();
				crew.Load(configNode);
				list.Add(crew);
			}
		}
		FieldValue.vesselCrew = list;
		MissionEditorLogic.Instance.EditorMission.situation.crewRoster = new KerbalRoster(data, Game.Modes.MISSION);
	}
}
