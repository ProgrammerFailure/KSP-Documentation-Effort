using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;

public class AlarmTypeTransferWindow : AlarmTypeBase
{
	[AppUI_Dropdown(guiName = "#autoLOC_8003562", dropdownItemsFieldName = "sourceList")]
	public string sourceBody;

	[AppUI_Dropdown(guiName = "#autoLOC_8003563", dropdownItemsFieldName = "destList")]
	public string destBody;

	private List<AppUIMemberDropdown.AppUIDropdownItem> sourceList;

	private List<AppUIMemberDropdown.AppUIDropdownItem> destList;

	public CelestialBody source;

	public CelestialBody dest;

	private bool canSetAlarm;

	private string cantSetAlarmText;

	private AppUIMemberDropdown sourceDropDown;

	private AppUIMemberDropdown destDropDown;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmTypeTransferWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetDefaultTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool RequiresVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanSetAlarm(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string CannotSetAlarmText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnScenarioUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSourceList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDestList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUIInitialization(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUIInputPanelDataChanged(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetNextTransferWindow(out double startTime, out double endTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUIEndInitialization(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInputPanelUpdate(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAlarmSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAlarmLoad(ConfigNode node)
	{
		throw null;
	}
}
