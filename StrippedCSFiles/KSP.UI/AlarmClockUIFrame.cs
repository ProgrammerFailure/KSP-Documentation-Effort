using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public class AlarmClockUIFrame : GenericAppFrame
{
	[CompilerGenerated]
	private sealed class _003CDelayedOpenAddPane_003Ed__39 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AlarmClockUIFrame _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CDelayedOpenAddPane_003Ed__39(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

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

	private List<AlarmClockUIAlarmRow> alarmRows;

	public UIPanelTransitionToggle addAlarmTransition;

	public TextMeshProUGUI addAlarmHeading;

	[SerializeField]
	private string addText;

	[SerializeField]
	private string editText;

	public ToggleGroup alarmSelectionGroup;

	[SerializeField]
	private AlarmClockUIAddPane addPane;

	[SerializeField]
	private RectTransform addPanePositionRect;

	private Vector2 addPaneLocalPos;

	internal AlarmTypeBase selectedAlarm;

	private bool showActiveAlarmsOnly;

	private bool showActiveVesselOnly;

	[SerializeField]
	private PointerEnterExitHandler enterExitHandler;

	private SpaceCenterBuilding[] sceneComponents;

	private double lastTime;

	private double currentTime;

	private bool timeNotSet;

	private bool hover;

	private bool timeAsDateTime;

	public bool IsDetailsShowing
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal bool MouseOver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool TimeAsDateTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockUIFrame()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableActionButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableActionButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool EditAlarm(AlarmTypeBase alarm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEdit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CDelayedOpenAddPane_003Ed__39))]
	private IEnumerator DelayedOpenAddPane()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSwitchTo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWarpTo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAvailableVesselChanged(Vessel newVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnActiveAlarmsToggleChanged(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnActiveVesselToggleChanged(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAlarmAdded(AlarmTypeBase newAlarm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAlarmRemoved(uint alarmId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AddAlarmPaneOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AddAlarmPaneClose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AddAlarmPaneCloseImmediate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RebuildList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAlarmRowSelected(AlarmTypeBase alarm, bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyTextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateInputLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddInputLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveInputLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleTimeDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTimeDisplay(bool asDateTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintDate(double ut, bool includeTime, bool includeSeconds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string PrintDate(double ut, bool includeTime, bool includeSeconds, bool timeAsDateTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintTime(double ut, int valuesOfInterest, bool explicitPositive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintTimeCompact(double ut, bool explicitPositive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintTimeStampCompact(double ut, bool days, bool years)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string PrintTimeStampCompact(double ut, bool days, bool years, bool timeAsDateTime)
	{
		throw null;
	}
}
