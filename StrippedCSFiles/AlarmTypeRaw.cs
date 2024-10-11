using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;

public class AlarmTypeRaw : AlarmTypeBase
{
	[AppUI_InputDateTime(guiName = "#autoLOC_8003557", datetimeMode = AppUIMemberDateTime.DateTimeModes.timespan)]
	public double timeEntry;

	private string timeEntryTime;

	private string timeEntryDate;

	[AppUI_RadioBool(guiName = "#autoLOC_8003559", valueText = "", hoverText = "#autoLOC_8003560", guiNameVertAlignment = AppUI_Control.VerticalAlignment.Capline)]
	public bool linkToVessel;

	private ulong linkedVesselId;

	public double defaultTimeEntry;

	private AppUIMemberDateTime timeUIField;

	private AppUIMemberRadioBool linkUIField;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmTypeRaw()
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
	public override void OnUIInitialization(AlarmUIDisplayMode displayMode)
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
}
