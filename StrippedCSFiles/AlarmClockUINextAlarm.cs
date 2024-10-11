using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmClockUINextAlarm : MonoBehaviour
{
	[SerializeField]
	private Button deleteButton;

	public Image alarmType;

	public TextMeshProUGUI nameLabel;

	public TextMeshProUGUI timeLabel;

	public Color futureAlarmTextColor;

	public Color pastAlarmTextColor;

	private Color currentTextColor;

	[SerializeField]
	private RectTransform commNetRect;

	[SerializeField]
	private RectTransform commNetBackgroundRect;

	[SerializeField]
	private RectTransform nextAlarmRect;

	private AlarmTypeBase alarm;

	private double currentUT;

	private double lastUIUpdateUT;

	private Type currentAlarmType;

	private Vector2 nextAlarmSizeDelta;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockUINextAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UIUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UIUpdate_NoAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeleteAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeleteAlarmDialogOutput(bool dontShowAgain, bool deleteAlarm)
	{
		throw null;
	}
}
