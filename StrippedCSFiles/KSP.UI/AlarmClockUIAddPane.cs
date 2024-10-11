using System;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class AlarmClockUIAddPane : MonoBehaviour
{
	internal AlarmClockUIFrame appFrame;

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
	private TextMeshProUGUI addAlarmButtonText;

	[SerializeField]
	private string addText;

	[SerializeField]
	private string editText;

	[SerializeField]
	private string closeText;

	private Type currentAlarmType;

	private AlarmUIDisplayMode displayMode;

	private AlarmTypeBase currentAlarmObject;

	private bool isClosing;

	private AppUIMemberDropdown.AppUIDropdownItemDictionary itemList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockUIAddPane()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupAddAlarm(Type alarmType, bool forceNew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupEditAlarm(AlarmTypeBase alarm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CancelAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClosePane()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetCurrentAlarmValuesFromUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetCurrentBaseUIValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddAlarmTypeChanged(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Rebuild Dropdown from Types")]
	private void BuildDropdownList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildDropdownList(bool showAll)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RebuildUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputDataChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAvailableVesselChanged(Vessel currentVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyTextFieldHasFocus()
	{
		throw null;
	}
}
