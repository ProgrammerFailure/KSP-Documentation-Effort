using System;
using ns11;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class AlarmClockUIAddPane : MonoBehaviour
{
	public AlarmClockUIFrame appFrame;

	public TMP_Dropdown alarmTypeDropdown;

	public TMP_InputField alarmName;

	public TMP_InputField alarmDescription;

	public GameObject actionsHolder;

	public Toggle warpActionNone;

	public Toggle warpActionKill;

	public Toggle warpActionPause;

	public Toggle messageActionNo;

	public Toggle messageActionYes;

	public Toggle messageActionYesIfOtherVessel;

	public Toggle actionPlaySound;

	public Toggle actionDeleteWhenDone;

	public Button addAlarmButton;

	public Button cancelAlarmButton;

	public GameObject alarmTypeHolder;

	public TextMeshProUGUI alarmTimeOfHeading;

	public TextMeshProUGUI alarmTimeUntilHeading;

	public TextMeshProUGUI alarmTimeOf;

	public TextMeshProUGUI alarmTimeUntil;

	public TextMeshProUGUI alarmAddWarning;

	public AppUIInputPanel inputPanel;

	[SerializeField]
	public TextMeshProUGUI addAlarmButtonText;

	[SerializeField]
	public string addText = "#autoLOC_8003519";

	[SerializeField]
	public string editText = "#autoLOC_8003517";

	[SerializeField]
	public string closeText = "#autoLOC_8003529";

	public Type currentAlarmType;

	public AlarmUIDisplayMode displayMode;

	public AlarmTypeBase currentAlarmObject = new AlarmTypeRaw();

	public bool isClosing;

	public AppUIMemberDropdown.AppUIDropdownItemDictionary itemList;

	public void Awake()
	{
		currentAlarmType = typeof(AlarmTypeRaw);
		BuildDropdownList();
		RebuildUI();
	}

	public void Start()
	{
		alarmTypeDropdown.onValueChanged.AddListener(AddAlarmTypeChanged);
		addAlarmButton.onClick.AddListener(AddAlarm);
		cancelAlarmButton.onClick.AddListener(CancelAlarm);
		GameEvents.onAlarmAvailableVesselChanged.Add(OnAvailableVesselChanged);
	}

	public void OnDestroy()
	{
		alarmTypeDropdown.onValueChanged.RemoveListener(AddAlarmTypeChanged);
		addAlarmButton.onClick.RemoveListener(AddAlarm);
		cancelAlarmButton.onClick.RemoveListener(CancelAlarm);
		GameEvents.onAlarmAvailableVesselChanged.Remove(OnAvailableVesselChanged);
	}

	public void LateUpdate()
	{
		if (currentAlarmObject != null)
		{
			SetCurrentAlarmValuesFromUI();
			addAlarmButton.interactable = currentAlarmObject.CanSetAlarm(displayMode);
			alarmTimeOf.gameObject.SetActive(currentAlarmObject.CanSetAlarm(displayMode));
			alarmTimeOfHeading.gameObject.SetActive(currentAlarmObject.CanSetAlarm(displayMode));
			alarmTimeUntil.gameObject.SetActive(currentAlarmObject.CanSetAlarm(displayMode) && !currentAlarmObject.Triggered);
			alarmTimeUntilHeading.gameObject.SetActive(currentAlarmObject.CanSetAlarm(displayMode) && !currentAlarmObject.Triggered);
			alarmAddWarning.gameObject.SetActive(!currentAlarmObject.CanSetAlarm(displayMode));
			alarmTimeOf.text = appFrame.PrintDate(currentAlarmObject.ut, includeTime: true, includeSeconds: true);
			alarmTimeUntil.text = appFrame.PrintTime(currentAlarmObject.TimeToAlarm, 5, explicitPositive: false);
			if (!currentAlarmObject.CanSetAlarm(displayMode))
			{
				alarmAddWarning.text = currentAlarmObject.CannotSetAlarmText();
			}
			alarmTypeDropdown.interactable = displayMode == AlarmUIDisplayMode.Add;
			warpActionPause.interactable = currentAlarmObject.actions.message != AlarmActions.MessageEnum.No;
			if (currentAlarmObject.actions.message == AlarmActions.MessageEnum.No && currentAlarmObject.actions.warp == AlarmActions.WarpEnum.PauseGame)
			{
				currentAlarmObject.actions.warp = AlarmActions.WarpEnum.KillWarp;
				warpActionKill.isOn = true;
			}
		}
		else if (!isClosing)
		{
			addAlarmButton.interactable = false;
			alarmTimeOf.gameObject.SetActive(value: false);
			alarmTimeUntil.gameObject.SetActive(value: false);
			alarmTimeOfHeading.gameObject.SetActive(value: false);
			alarmTimeUntilHeading.gameObject.SetActive(value: false);
			alarmAddWarning.gameObject.SetActive(value: true);
			alarmAddWarning.text = Localizer.Format("#autoLOC_8003551");
		}
	}

	public void SetupAddAlarm(Type alarmType, bool forceNew)
	{
		isClosing = false;
		Type type = currentAlarmType;
		AlarmTypeBase alarmTypeBase = currentAlarmObject;
		if (currentAlarmType != alarmType || forceNew)
		{
			currentAlarmType = alarmType;
			currentAlarmObject = AlarmClockScenario.CreateAlarmByType(currentAlarmType);
			if (currentAlarmObject == null)
			{
				Debug.LogError("[AlarmClockUIAddPane]: Unable to get new type, resetting to previous type: " + type.Name);
				currentAlarmObject = alarmTypeBase;
				currentAlarmType = type;
				return;
			}
			if (!forceNew)
			{
				if (currentAlarmObject.title == null || alarmName.text == alarmTypeBase.GetDefaultTitle())
				{
					currentAlarmObject.title = currentAlarmObject.GetDefaultTitle();
					alarmName.text = currentAlarmObject.title;
				}
				SetCurrentAlarmValuesFromUI();
			}
			else
			{
				currentAlarmObject.title = currentAlarmObject.GetDefaultTitle();
			}
		}
		displayMode = AlarmUIDisplayMode.Add;
		if (AlarmClockScenario.IsVesselAvailable)
		{
			currentAlarmObject.vesselId = AlarmClockScenario.AvailableVessel.persistentId;
		}
		OnAvailableVesselChanged((AlarmClockScenario.Instance == null) ? null : AlarmClockScenario.AvailableVessel);
		currentAlarmObject.UIInitialization(displayMode);
		BuildDropdownList(showAll: false);
		RebuildUI();
		currentAlarmObject.UIEndInitialization(displayMode);
	}

	public void SetupEditAlarm(AlarmTypeBase alarm)
	{
		isClosing = false;
		currentAlarmType = alarm.GetType();
		currentAlarmObject = alarm.CloneAlarm();
		displayMode = AlarmUIDisplayMode.Edit;
		currentAlarmObject.UIInitialization(displayMode);
		BuildDropdownList(showAll: true);
		RebuildUI();
		currentAlarmObject.UIEndInitialization(displayMode);
	}

	public void AddAlarm()
	{
		if (!currentAlarmObject.Triggered)
		{
			SetCurrentAlarmValuesFromUI();
			AlarmUIDisplayMode alarmUIDisplayMode = displayMode;
			if (alarmUIDisplayMode != 0 && alarmUIDisplayMode == AlarmUIDisplayMode.Edit)
			{
				if (AlarmClockScenario.TryGetAlarm(currentAlarmObject.Id, out var alarm))
				{
					AlarmClockScenario.DeleteAlarm(alarm.Id);
					AlarmClockScenario.AddAlarm(currentAlarmObject);
					appFrame.selectedAlarm = currentAlarmObject;
				}
			}
			else
			{
				AlarmClockScenario.AddAlarm(currentAlarmObject);
			}
		}
		ClosePane();
	}

	public void CancelAlarm()
	{
		ClosePane();
	}

	public void ClosePane()
	{
		isClosing = true;
		if (appFrame != null)
		{
			appFrame.AddAlarmPaneClose();
		}
		inputPanel.ReleaseData();
		currentAlarmObject = null;
	}

	public void SetCurrentAlarmValuesFromUI()
	{
		currentAlarmObject.title = alarmName.text;
		currentAlarmObject.description = alarmDescription.text;
		if (warpActionPause.isOn)
		{
			currentAlarmObject.actions.warp = AlarmActions.WarpEnum.PauseGame;
		}
		else if (warpActionKill.isOn)
		{
			currentAlarmObject.actions.warp = AlarmActions.WarpEnum.KillWarp;
		}
		else
		{
			currentAlarmObject.actions.warp = AlarmActions.WarpEnum.DoNothing;
		}
		if (messageActionNo.isOn)
		{
			currentAlarmObject.actions.message = AlarmActions.MessageEnum.No;
		}
		else if (messageActionYesIfOtherVessel.isOn)
		{
			currentAlarmObject.actions.message = AlarmActions.MessageEnum.YesIfOtherVessel;
		}
		else
		{
			currentAlarmObject.actions.message = AlarmActions.MessageEnum.Yes;
		}
		currentAlarmObject.actions.playSound = actionPlaySound.isOn;
		currentAlarmObject.actions.deleteWhenDone = actionDeleteWhenDone.isOn;
	}

	public void SetCurrentBaseUIValues()
	{
		alarmName.text = currentAlarmObject.title;
		alarmDescription.text = currentAlarmObject.description;
		switch (currentAlarmObject.actions.warp)
		{
		case AlarmActions.WarpEnum.DoNothing:
			warpActionNone.isOn = true;
			break;
		default:
			warpActionKill.isOn = true;
			break;
		case AlarmActions.WarpEnum.PauseGame:
			warpActionPause.isOn = true;
			break;
		}
		switch (currentAlarmObject.actions.message)
		{
		case AlarmActions.MessageEnum.No:
			messageActionNo.isOn = true;
			break;
		default:
			messageActionYes.isOn = true;
			break;
		case AlarmActions.MessageEnum.YesIfOtherVessel:
			messageActionYesIfOtherVessel.isOn = true;
			break;
		}
		actionPlaySound.isOn = currentAlarmObject.actions.playSound;
		actionDeleteWhenDone.isOn = currentAlarmObject.actions.deleteWhenDone;
	}

	public void AddAlarmTypeChanged(int value)
	{
		Type alarmType = (Type)itemList.KeyAt(value);
		if (displayMode == AlarmUIDisplayMode.Add)
		{
			SetupAddAlarm(alarmType, forceNew: false);
		}
	}

	[ContextMenu("Rebuild Dropdown from Types")]
	public void BuildDropdownList()
	{
		BuildDropdownList(showAll: false);
	}

	public void BuildDropdownList(bool showAll)
	{
		if (AlarmClockScenario.AlarmTypes == null)
		{
			Debug.LogError("[AlarmClockUIAddPane]: There are no alarm types loaded");
			return;
		}
		itemList = new AppUIMemberDropdown.AppUIDropdownItemDictionary();
		alarmTypeDropdown.ClearOptions();
		for (int i = 0; i < AlarmClockScenario.AlarmTypes.Count; i++)
		{
			AlarmTypeBase alarmTypeBase = AlarmClockScenario.CreateAlarmByType(AlarmClockScenario.AlarmTypes[i]);
			if (alarmTypeBase != null && (showAll || !alarmTypeBase.RequiresVessel() || AlarmClockScenario.IsVesselAvailable))
			{
				itemList.Add(alarmTypeBase.GetType(), new AppUIMemberDropdown.AppUIDropdownItem
				{
					key = alarmTypeBase.GetType(),
					text = alarmTypeBase.GetDefaultTitle(),
					image = AlarmClockScenario.AlarmSprites[alarmTypeBase.iconURL]
				});
			}
		}
		alarmTypeDropdown.ClearOptions();
		for (int j = 0; j < itemList.Count; j++)
		{
			alarmTypeDropdown.options.Add(itemList.ValuesList[j].TMP_OptionData);
		}
	}

	public void RebuildUI()
	{
		if (currentAlarmObject == null)
		{
			return;
		}
		actionsHolder.SetActive(!currentAlarmObject.Triggered);
		alarmTypeHolder.SetActive(!currentAlarmObject.Triggered);
		alarmName.interactable = !currentAlarmObject.Triggered;
		alarmDescription.interactable = !currentAlarmObject.Triggered;
		cancelAlarmButton.gameObject.SetActive(!currentAlarmObject.Triggered);
		if (!currentAlarmObject.Triggered)
		{
			inputPanel.Setup(currentAlarmObject, InputDataChanged);
		}
		if (itemList.GetIndexOfKey(currentAlarmType) > -1)
		{
			alarmTypeDropdown.value = itemList.GetIndexOfKey(currentAlarmType);
		}
		else
		{
			alarmTypeDropdown.value = 0;
		}
		alarmTypeDropdown.RefreshShownValue();
		if (currentAlarmObject.Triggered)
		{
			addAlarmButtonText.text = Localizer.Format(closeText);
			alarmTypeDropdown.interactable = false;
		}
		else
		{
			AlarmUIDisplayMode alarmUIDisplayMode = displayMode;
			if (alarmUIDisplayMode != 0 && alarmUIDisplayMode == AlarmUIDisplayMode.Edit)
			{
				addAlarmButtonText.text = Localizer.Format(editText);
				alarmTypeDropdown.interactable = false;
			}
			else
			{
				addAlarmButtonText.text = Localizer.Format(addText);
				alarmTypeDropdown.interactable = true;
			}
		}
		SetCurrentBaseUIValues();
	}

	public void InputDataChanged()
	{
		SetCurrentBaseUIValues();
	}

	public void OnAvailableVesselChanged(Vessel currentVessel)
	{
		BuildDropdownList(displayMode != AlarmUIDisplayMode.Add);
		if (currentAlarmObject != null && displayMode == AlarmUIDisplayMode.Add && currentVessel != null)
		{
			currentAlarmObject.vesselId = currentVessel.persistentId;
		}
		RebuildUI();
	}

	public bool AnyTextFieldHasFocus()
	{
		if (alarmName.isFocused)
		{
			return true;
		}
		if (alarmDescription.isFocused)
		{
			return true;
		}
		if (inputPanel != null)
		{
			return inputPanel.AnyTextFieldHasFocus();
		}
		return false;
	}
}
