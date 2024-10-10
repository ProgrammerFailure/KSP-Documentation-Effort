using System;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmClockUINextAlarm : MonoBehaviour
{
	[SerializeField]
	public Button deleteButton;

	public Image alarmType;

	public TextMeshProUGUI nameLabel;

	public TextMeshProUGUI timeLabel;

	public Color futureAlarmTextColor = Color.white;

	public Color pastAlarmTextColor = new Color32(103, 113, 125, byte.MaxValue);

	public Color currentTextColor;

	[SerializeField]
	public RectTransform commNetRect;

	[SerializeField]
	public RectTransform commNetBackgroundRect;

	[SerializeField]
	public RectTransform nextAlarmRect;

	public AlarmTypeBase alarm;

	public double currentUT;

	public double lastUIUpdateUT;

	public Type currentAlarmType;

	public Vector2 nextAlarmSizeDelta;

	public void Start()
	{
		deleteButton.onClick.AddListener(DeleteAlarm);
		UIUpdate_NoAlarm();
	}

	public void Update()
	{
		currentUT = Planetarium.GetUniversalTime();
		if (currentUT - lastUIUpdateUT > AlarmClockScenario.UIUpdatePeriod)
		{
			UIUpdate();
			lastUIUpdateUT = currentUT;
		}
	}

	public void UIUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && commNetRect != null)
		{
			nextAlarmSizeDelta = nextAlarmRect.sizeDelta;
			nextAlarmSizeDelta.x = Mathf.Max(240f, (commNetRect.anchoredPosition + commNetBackgroundRect.anchoredPosition + commNetBackgroundRect.sizeDelta).x);
			nextAlarmRect.sizeDelta = nextAlarmSizeDelta;
		}
		alarm = AlarmClockScenario.GetNextOrLastAlarm();
		if (alarm == null)
		{
			if (currentAlarmType != null)
			{
				UIUpdate_NoAlarm();
			}
			return;
		}
		if (currentAlarmType != alarm.GetType())
		{
			alarmType.gameObject.SetActive(value: true);
			deleteButton.gameObject.SetActive(value: true);
			timeLabel.gameObject.SetActive(value: true);
			alarmType.sprite = AlarmClockScenario.AlarmSprites[alarm.iconURL];
			currentAlarmType = alarm.GetType();
		}
		currentTextColor = ((currentUT > alarm.ut) ? pastAlarmTextColor : futureAlarmTextColor);
		nameLabel.text = alarm.title;
		nameLabel.color = currentTextColor;
		timeLabel.color = currentTextColor;
		timeLabel.text = AlarmClockUIFrame.PrintTimeStampCompact(alarm.TimeToAlarm, days: true, years: true, timeAsDateTime: true);
	}

	public void UIUpdate_NoAlarm()
	{
		alarmType.gameObject.SetActive(value: false);
		timeLabel.gameObject.SetActive(value: false);
		deleteButton.gameObject.SetActive(value: false);
		nameLabel.text = Localizer.Format("#autoLOC_8003535");
		nameLabel.color = pastAlarmTextColor;
		currentAlarmType = null;
	}

	public void OnDestroy()
	{
		deleteButton.onClick.AddListener(DeleteAlarm);
	}

	public void DeleteAlarm()
	{
		if (alarm == null)
		{
			return;
		}
		if (GameSettings.SHOW_DELETE_ALARM_CONFIRMATION)
		{
			UIConfirmDialog.Spawn(Localizer.Format("#autoLOC_8003552"), Localizer.Format("#autoLOC_8003553", alarm.title, AlarmClockUIFrame.PrintDate(alarm.ut, includeTime: true, includeSeconds: true, timeAsDateTime: true)), Localizer.Format("#autoLOC_226976"), Localizer.Format("#autoLOC_226975"), Localizer.Format("#autoLOC_360842"), delegate(bool dontShowAgain)
			{
				DeleteAlarmDialogOutput(dontShowAgain, deleteAlarm: true);
			}, delegate(bool dontShowAgain)
			{
				DeleteAlarmDialogOutput(dontShowAgain, deleteAlarm: false);
			});
		}
		else
		{
			DeleteAlarmDialogOutput(!GameSettings.SHOW_DELETE_ALARM_CONFIRMATION, deleteAlarm: true);
		}
	}

	public void DeleteAlarmDialogOutput(bool dontShowAgain, bool deleteAlarm)
	{
		GameSettings.SHOW_DELETE_ALARM_CONFIRMATION = !dontShowAgain;
		if (deleteAlarm)
		{
			AlarmClockScenario.DeleteAlarm(alarm.Id);
		}
	}
}
