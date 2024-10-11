using System;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmClockUIAlarmRow : MonoBehaviour
{
	private AlarmClockUIFrame appFrame;

	[SerializeField]
	public Button deleteButton;

	[SerializeField]
	public Toggle toggle;

	public Image alarmType;

	public TextMeshProUGUI nameLabel;

	public TextMeshProUGUI timeLabel;

	public Color futureAlarmTextColor;

	public Color pastAlarmTextColor;

	private Color currentTextColor;

	private AlarmTypeBase alarm;

	private Action<AlarmTypeBase, bool> onToggleChanged;

	private double currentUT;

	private double lastUIUpdateUT;

	internal uint AlarmId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockUIAlarmRow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(AlarmClockUIFrame appFrame, AlarmTypeBase alarm, bool selected, Transform parent, ToggleGroup toggleGroup, Action<AlarmTypeBase, bool> onToggleChanged)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggleChanged(bool value)
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
	private void UpdateRowUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTextColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UIUpdate()
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
