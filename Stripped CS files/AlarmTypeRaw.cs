using ns11;
using ns2;
using ns9;

public class AlarmTypeRaw : AlarmTypeBase
{
	[AppUI_InputDateTime(guiName = "#autoLOC_8003557", datetimeMode = AppUIMemberDateTime.DateTimeModes.timespan)]
	public double timeEntry = 300.0;

	public string timeEntryTime = "#autoLOC_8003557";

	public string timeEntryDate = "#autoLOC_8003558";

	[AppUI_RadioBool(guiName = "#autoLOC_8003559", valueText = "", hoverText = "#autoLOC_8003560", guiNameVertAlignment = AppUI_Control.VerticalAlignment.Capline)]
	public bool linkToVessel;

	public ulong linkedVesselId;

	public double defaultTimeEntry = 300.0;

	public AppUIMemberDateTime timeUIField;

	public AppUIMemberRadioBool linkUIField;

	public AlarmTypeRaw()
	{
		iconURL = "raw";
	}

	public override string GetDefaultTitle()
	{
		return Localizer.Format("#autoLOC_8003536");
	}

	public override bool RequiresVessel()
	{
		return false;
	}

	public override bool CanSetAlarm(AlarmUIDisplayMode displayMode)
	{
		return true;
	}

	public override void OnUIInitialization(AlarmUIDisplayMode displayMode)
	{
		if (displayMode == AlarmUIDisplayMode.Add)
		{
			defaultTimeEntry = AlarmClockScenario.Instance.settings.defaultRawTime;
			timeEntry = defaultTimeEntry;
		}
		else
		{
			timeEntry = ut;
		}
	}

	public override void OnUIEndInitialization(AlarmUIDisplayMode displayMode)
	{
		if (displayMode == AlarmUIDisplayMode.Edit && base.TimeToAlarm < 0.0)
		{
			return;
		}
		timeUIField = (AppUIMemberDateTime)uiPanel.GetControl("timeEntry");
		if (timeUIField != null)
		{
			switch (displayMode)
			{
			default:
				timeUIField.DatetimeMode = AppUIMemberDateTime.DateTimeModes.date;
				timeUIField.guiName = timeEntryDate;
				break;
			case AlarmUIDisplayMode.Add:
				timeUIField.DatetimeMode = AppUIMemberDateTime.DateTimeModes.timespan;
				timeUIField.guiName = timeEntryTime;
				break;
			}
		}
		timeUIField.RefreshUI();
	}

	public override void OnInputPanelUpdate(AlarmUIDisplayMode displayMode)
	{
		switch (displayMode)
		{
		default:
			ut = timeEntry;
			break;
		case AlarmUIDisplayMode.Add:
			ut = Planetarium.GetUniversalTime() + timeEntry;
			break;
		}
		if (uiPanel != null)
		{
			AppUIMember control = uiPanel.GetControl("linkToVessel");
			if (control != null)
			{
				control.gameObject.SetActive(AlarmClockScenario.IsVesselAvailable && displayMode == AlarmUIDisplayMode.Add);
			}
		}
		if (displayMode == AlarmUIDisplayMode.Add)
		{
			if (linkToVessel && AlarmClockScenario.IsVesselAvailable)
			{
				vesselId = AlarmClockScenario.AvailableVessel.persistentId;
			}
			else
			{
				vesselId = 0u;
			}
		}
	}
}
