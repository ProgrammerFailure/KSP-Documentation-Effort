using System;
using System.Collections;
using System.Collections.Generic;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class AlarmClockUIFrame : GenericAppFrame
{
	public Button editAlarmButton;

	public Button switchToAlarmButton;

	public Button warpToAlarmButton;

	public Toggle activeAlarmsOnlyToggle;

	public Toggle activeVesselOnlyToggle;

	public TextMeshProUGUI timeLabel;

	public Button timeDisplayTypeButton;

	public Image timeDisplayTypeDate;

	public TextMeshProUGUI timeDisplayTypeSeconds;

	public Button createAlarmButton;

	public Transform alarmsListParent;

	public AlarmClockUIAlarmRow alarmRowPrefab;

	public List<AlarmClockUIAlarmRow> alarmRows;

	public UIPanelTransitionToggle addAlarmTransition;

	public TextMeshProUGUI addAlarmHeading;

	[SerializeField]
	public string addText = "#autoLOC_8003507";

	[SerializeField]
	public string editText = "#autoLOC_8003508";

	public ToggleGroup alarmSelectionGroup;

	[SerializeField]
	public AlarmClockUIAddPane addPane;

	[SerializeField]
	public RectTransform addPanePositionRect;

	public Vector2 addPaneLocalPos;

	public AlarmTypeBase selectedAlarm;

	public bool showActiveAlarmsOnly;

	public bool showActiveVesselOnly;

	[SerializeField]
	public PointerEnterExitHandler enterExitHandler;

	public SpaceCenterBuilding[] sceneComponents;

	public double lastTime;

	public double currentTime;

	public bool timeNotSet;

	public bool hover;

	public bool timeAsDateTime = true;

	public bool IsDetailsShowing => addAlarmTransition.expanded;

	public bool MouseOver => hover;

	public bool TimeAsDateTime => timeAsDateTime;

	public new void Awake()
	{
		addPane.appFrame = this;
		addPaneLocalPos = addPanePositionRect.transform.localPosition;
		alarmSelectionGroup = GetComponent<ToggleGroup>();
		activeVesselOnlyToggle.isOn = showActiveVesselOnly;
		activeAlarmsOnlyToggle.isOn = showActiveVesselOnly;
		enterExitHandler.onPointerEnter.AddListener(OnPointerEnter);
		enterExitHandler.onPointerExit.AddListener(OnPointerExit);
		alarmRows = new List<AlarmClockUIAlarmRow>();
	}

	public void Start()
	{
		editAlarmButton.onClick.AddListener(OnEdit);
		switchToAlarmButton.onClick.AddListener(OnSwitchTo);
		warpToAlarmButton.onClick.AddListener(OnWarpTo);
		activeAlarmsOnlyToggle.onValueChanged.AddListener(OnActiveAlarmsToggleChanged);
		activeVesselOnlyToggle.onValueChanged.AddListener(OnActiveVesselToggleChanged);
		timeDisplayTypeButton.onClick.AddListener(ToggleTimeDisplay);
		createAlarmButton.onClick.AddListener(CreateAlarm);
		GameEvents.onAlarmAdded.Add(OnAlarmAdded);
		GameEvents.onAlarmRemoved.Add(OnAlarmRemoved);
		GameEvents.onAlarmAvailableVesselChanged.Add(OnAvailableVesselChanged);
		addAlarmTransition.CollapseImmediate();
		RebuildList();
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
		{
			sceneComponents = UnityEngine.Object.FindObjectsOfType<SpaceCenterBuilding>();
		}
	}

	public new void OnDestroy()
	{
		editAlarmButton.onClick.RemoveListener(OnEdit);
		switchToAlarmButton.onClick.RemoveListener(OnSwitchTo);
		warpToAlarmButton.onClick.RemoveListener(OnWarpTo);
		activeAlarmsOnlyToggle.onValueChanged.RemoveListener(OnActiveAlarmsToggleChanged);
		activeVesselOnlyToggle.onValueChanged.RemoveListener(OnActiveVesselToggleChanged);
		timeDisplayTypeButton.onClick.RemoveListener(ToggleTimeDisplay);
		createAlarmButton.onClick.RemoveListener(CreateAlarm);
		GameEvents.onAlarmAdded.Remove(OnAlarmAdded);
		GameEvents.onAlarmRemoved.Remove(OnAlarmRemoved);
		GameEvents.onAlarmAvailableVesselChanged.Remove(OnAvailableVesselChanged);
		enterExitHandler.onPointerEnter.RemoveListener(OnPointerEnter);
		enterExitHandler.onPointerExit.RemoveListener(OnPointerExit);
		RemoveInputLock();
	}

	public void Update()
	{
		currentTime = Planetarium.GetUniversalTime();
		if (Math.Floor(currentTime) != Math.Floor(lastTime) || timeNotSet)
		{
			timeLabel.text = PrintDate(currentTime, includeTime: true, includeSeconds: true);
			timeNotSet = true;
			lastTime = currentTime;
		}
		if (!alarmSelectionGroup.AnyTogglesOn())
		{
			selectedAlarm = null;
		}
		if (selectedAlarm == null)
		{
			DisableActionButtons();
		}
		else
		{
			EnableActionButtons();
		}
		warpToAlarmButton.interactable = AlarmClockScenario.GetNextAlarm() != null;
		UpdateInputLock();
	}

	public void DisableActionButtons()
	{
		editAlarmButton.interactable = false;
		switchToAlarmButton.interactable = false;
	}

	public void EnableActionButtons()
	{
		editAlarmButton.interactable = true;
		switchToAlarmButton.interactable = selectedAlarm.vesselId != 0;
	}

	public bool EditAlarm(AlarmTypeBase alarm)
	{
		if (AlarmClockScenario.AlarmExists(alarm.Id))
		{
			selectedAlarm = alarm;
			for (int i = 0; i < alarmRows.Count; i++)
			{
				if (alarmRows[i].AlarmId == alarm.Id)
				{
					alarmRows[i].toggle.isOn = true;
				}
			}
			OnEdit();
			return true;
		}
		return false;
	}

	public void OnEdit()
	{
		addAlarmHeading.text = Localizer.Format(editText);
		addPane.SetupEditAlarm(selectedAlarm);
		if (addAlarmTransition.collapsed)
		{
			StartCoroutine(DelayedOpenAddPane());
		}
	}

	public IEnumerator DelayedOpenAddPane()
	{
		yield return null;
		AddAlarmPaneOpen();
	}

	public void OnSwitchTo()
	{
		if (selectedAlarm.Vessel == null)
		{
			Debug.LogError("[AlarmClockApp]: Vessel reference is null, Unable to switch");
			return;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			FlightGlobals.SetActiveVessel(selectedAlarm.Vessel);
			return;
		}
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT);
		FlightDriver.StartAndFocusVessel("persistent", FlightGlobals.Vessels.IndexOf(selectedAlarm.Vessel));
	}

	public void OnWarpTo()
	{
		AlarmTypeBase nextAlarm = AlarmClockScenario.GetNextAlarm();
		if (nextAlarm.actions.warp == AlarmActions.WarpEnum.DoNothing)
		{
			nextAlarm.actions.warp = AlarmActions.WarpEnum.KillWarp;
		}
		int rate_index = WarpTransitionCalculator.SafeRateToUTPeriod(nextAlarm.TimeToAlarm);
		if (TimeWarp.fetch != null)
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(rate_index, instant: false);
		}
	}

	public void OnAvailableVesselChanged(Vessel newVessel)
	{
		RebuildList();
	}

	public void OnActiveAlarmsToggleChanged(bool value)
	{
		if (showActiveAlarmsOnly != value)
		{
			showActiveAlarmsOnly = value;
			RebuildList();
		}
	}

	public void OnActiveVesselToggleChanged(bool value)
	{
		if (showActiveVesselOnly != value)
		{
			showActiveVesselOnly = value;
			RebuildList();
		}
	}

	public void OnAlarmAdded(AlarmTypeBase newAlarm)
	{
		RebuildList();
	}

	public void OnAlarmRemoved(uint alarmId)
	{
		if (selectedAlarm != null && selectedAlarm.Id == alarmId)
		{
			AddAlarmPaneClose();
		}
		RebuildList();
	}

	public void CreateAlarm()
	{
		addAlarmHeading.text = Localizer.Format(addText);
		selectedAlarm = null;
		addPane.SetupAddAlarm(typeof(AlarmTypeRaw), forceNew: true);
		AddAlarmPaneOpen();
	}

	public void AddAlarmPaneOpen()
	{
		float num = addPanePositionRect.rect.height / 2f - UIMasterController.Instance.uiCamera.WorldToScreenPoint(addPane.transform.position).y / GameSettings.UI_SCALE;
		if (num > 0f)
		{
			addPaneLocalPos = addPanePositionRect.transform.localPosition;
			addPaneLocalPos.y += num;
			addPanePositionRect.transform.localPosition = addPaneLocalPos;
		}
		UIMasterController.ClampToScreen(addPane.transform as RectTransform, Vector2.zero);
		addAlarmTransition.Expand();
	}

	public void AddAlarmPaneClose()
	{
		addAlarmTransition.Collapse();
	}

	public void AddAlarmPaneCloseImmediate()
	{
		addAlarmTransition.CollapseImmediate();
	}

	public void RebuildList()
	{
		if (AlarmClockScenario.Instance == null)
		{
			return;
		}
		int childCount = alarmsListParent.childCount;
		while (childCount-- > 0)
		{
			UnityEngine.Object.Destroy(alarmsListParent.GetChild(childCount).gameObject);
		}
		alarmRows.Clear();
		uint num = ((selectedAlarm != null) ? selectedAlarm.Id : 0u);
		for (int i = 0; i < AlarmClockScenario.Instance.alarms.Count; i++)
		{
			AlarmTypeBase alarmTypeBase = AlarmClockScenario.Instance.alarms.ValuesList[i];
			if ((!showActiveVesselOnly || alarmTypeBase.IsAlarmVesselTheAvailableVessel) && (!showActiveAlarmsOnly || !alarmTypeBase.Actioned))
			{
				AlarmClockUIAlarmRow alarmClockUIAlarmRow = UnityEngine.Object.Instantiate(alarmRowPrefab);
				alarmClockUIAlarmRow.Setup(this, alarmTypeBase, alarmTypeBase.Id == num, alarmsListParent, alarmSelectionGroup, OnAlarmRowSelected);
				alarmRows.Add(alarmClockUIAlarmRow);
			}
		}
	}

	public void OnAlarmRowSelected(AlarmTypeBase alarm, bool value)
	{
		if (value)
		{
			selectedAlarm = alarm;
		}
	}

	public bool AnyTextFieldHasFocus()
	{
		bool flag = false;
		if (!addAlarmTransition.collapsed)
		{
			flag = flag || addPane.AnyTextFieldHasFocus();
		}
		return flag;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
		if (sceneComponents != null)
		{
			for (int i = 0; i < sceneComponents.Length; i++)
			{
				sceneComponents[i].HighLightBuilding(mouseOverIcon: false);
				sceneComponents[i].SetHighlighted(newValue: false);
			}
		}
		AddInputLock();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
		if (!AnyTextFieldHasFocus())
		{
			RemoveInputLock();
		}
	}

	public void UpdateInputLock()
	{
		if (!hover && InputLockManager.GetControlLock("UIAlarmClockApp") != ControlTypes.None && !AnyTextFieldHasFocus())
		{
			RemoveInputLock();
		}
	}

	public void AddInputLock()
	{
		InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "UIAlarmClockApp");
	}

	public void RemoveInputLock()
	{
		InputLockManager.RemoveControlLock("UIAlarmClockApp");
	}

	public void ToggleTimeDisplay()
	{
		SetTimeDisplay(!timeAsDateTime);
	}

	public void SetTimeDisplay(bool asDateTime)
	{
		if (timeAsDateTime != asDateTime)
		{
			timeAsDateTime = asDateTime;
			GameEvents.onAlarmAppTimeDisplayChanged.Fire(asDateTime);
		}
	}

	public string PrintDate(double ut, bool includeTime, bool includeSeconds)
	{
		if (timeAsDateTime)
		{
			return KSPUtil.PrintDate(ut, includeTime, includeSeconds);
		}
		return ut.ToString("0") + "s";
	}

	public static string PrintDate(double ut, bool includeTime, bool includeSeconds, bool timeAsDateTime)
	{
		if (timeAsDateTime)
		{
			return KSPUtil.PrintDate(ut, includeTime, includeSeconds);
		}
		return ut.ToString("0") + "s";
	}

	public string PrintTime(double ut, int valuesOfInterest, bool explicitPositive)
	{
		if (timeAsDateTime)
		{
			return KSPUtil.PrintTime(ut, valuesOfInterest, explicitPositive);
		}
		return ut.ToString("0") + "s";
	}

	public string PrintTimeCompact(double ut, bool explicitPositive)
	{
		if (timeAsDateTime)
		{
			return KSPUtil.PrintTimeCompact(ut, explicitPositive: false);
		}
		return ut.ToString("0") + "s";
	}

	public string PrintTimeStampCompact(double ut, bool days, bool years)
	{
		return PrintTimeStampCompact(ut, days, years, timeAsDateTime);
	}

	public static string PrintTimeStampCompact(double ut, bool days, bool years, bool timeAsDateTime)
	{
		double time = Math.Abs(ut);
		string text = ((!timeAsDateTime) ? (time.ToString("0") + "s") : KSPUtil.PrintTimeStampCompact(time, days, years));
		if (ut < 0.0)
		{
			text = "+ " + text;
		}
		return text;
	}
}
