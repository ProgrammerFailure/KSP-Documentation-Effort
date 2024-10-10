using System.Collections.Generic;
using ns11;
using ns2;
using ns9;

public class AlarmTypeTransferWindow : AlarmTypeBase
{
	[AppUI_Dropdown(guiName = "#autoLOC_8003562", dropdownItemsFieldName = "sourceList")]
	public string sourceBody;

	[AppUI_Dropdown(guiName = "#autoLOC_8003563", dropdownItemsFieldName = "destList")]
	public string destBody;

	public List<AppUIMemberDropdown.AppUIDropdownItem> sourceList;

	public List<AppUIMemberDropdown.AppUIDropdownItem> destList;

	public CelestialBody source;

	public CelestialBody dest;

	public bool canSetAlarm;

	public string cantSetAlarmText = "";

	public AppUIMemberDropdown sourceDropDown;

	public AppUIMemberDropdown destDropDown;

	public AlarmTypeTransferWindow()
	{
		iconURL = "xfer";
		sourceList = new List<AppUIMemberDropdown.AppUIDropdownItem>();
		destList = new List<AppUIMemberDropdown.AppUIDropdownItem>();
	}

	public override string GetDefaultTitle()
	{
		return Localizer.Format("#autoLOC_8003540");
	}

	public override bool RequiresVessel()
	{
		return false;
	}

	public override bool CanSetAlarm(AlarmUIDisplayMode displayMode)
	{
		return canSetAlarm;
	}

	public override string CannotSetAlarmText()
	{
		return cantSetAlarmText;
	}

	public override void OnScenarioUpdate()
	{
	}

	public void UpdateSourceList()
	{
		sourceDropDown = (AppUIMemberDropdown)uiPanel.GetControl("sourceBody");
		sourceList.Clear();
		string text = "";
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			if (!celestialBody.isStar && celestialBody.referenceBody != null && celestialBody.referenceBody.orbitingBodies.Count > 1)
			{
				sourceList.Add(new AppUIMemberDropdown.AppUIDropdownItem
				{
					key = celestialBody.name,
					text = celestialBody.displayName.LocalizeRemoveGender()
				});
				if (sourceBody == celestialBody.name || text == "")
				{
					text = celestialBody.name;
				}
			}
		}
		sourceBody = text;
		sourceDropDown.RefreshUI();
	}

	public void UpdateDestList()
	{
		destDropDown = (AppUIMemberDropdown)uiPanel.GetControl("destBody");
		destList.Clear();
		if (!(base.Vessel == null))
		{
			_ = base.Vessel.orbit;
		}
		CelestialBody bodyByName = FlightGlobals.GetBodyByName(sourceBody);
		string text = "";
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			if (!celestialBody.isStar && celestialBody != bodyByName && bodyByName.referenceBody == celestialBody.referenceBody)
			{
				destList.Add(new AppUIMemberDropdown.AppUIDropdownItem
				{
					key = celestialBody.name,
					text = celestialBody.displayName.LocalizeRemoveGender()
				});
				if (destBody == celestialBody.name || text == "")
				{
					text = celestialBody.name;
				}
			}
		}
		destBody = text;
		destDropDown.RefreshUI();
	}

	public override void OnUIInitialization(AlarmUIDisplayMode displayMode)
	{
		if (displayMode == AlarmUIDisplayMode.Add)
		{
			sourceBody = FlightGlobals.GetHomeBodyName();
			if (base.Vessel != null)
			{
				sourceBody = base.Vessel.orbit.referenceBody.name;
			}
			OnUIInputPanelDataChanged(displayMode);
		}
	}

	public override void OnUIInputPanelDataChanged(AlarmUIDisplayMode displayMode)
	{
		UpdateSourceList();
		UpdateDestList();
		source = FlightGlobals.GetBodyByName(sourceBody);
		dest = FlightGlobals.GetBodyByName(destBody);
		if (!(source == null) && !(dest == null))
		{
			canSetAlarm = true;
			GetNextTransferWindow(out var startTime, out var endTime);
			ut = startTime;
			title = Localizer.Format("#autoLOC_8003565", source.GetDisplayName().LocalizeRemoveGender(), dest.GetDisplayName().LocalizeRemoveGender());
			description = Localizer.Format("#autoLOC_8003565", AlarmClockUIFrame.PrintDate(ut, includeTime: true, includeSeconds: true, timeAsDateTime: true), AlarmClockUIFrame.PrintTimeStampCompact(endTime - startTime, days: true, years: true, timeAsDateTime: true), AlarmClockUIFrame.PrintDate(ut + endTime - startTime, includeTime: true, includeSeconds: true, timeAsDateTime: true));
		}
		else
		{
			canSetAlarm = false;
			cantSetAlarmText = Localizer.Format("#autoLOC_8003564");
		}
	}

	public void GetNextTransferWindow(out double startTime, out double endTime)
	{
		new TransferDataSimple(null, null)
		{
			SourceBody = source,
			TargetBody = dest
		};
		TransferMath.CalculateStartEndXferTimes(source, dest, GameSettings.MANEUVER_TOOL_TRANSFER_DEGREES, out startTime, out endTime);
		startTime += Planetarium.GetUniversalTime();
		endTime += Planetarium.GetUniversalTime();
		if (startTime < Planetarium.GetUniversalTime())
		{
			TransferMath.CalculateStartEndXferTimes(source, dest, endTime + 300.0, GameSettings.MANEUVER_TOOL_TRANSFER_DEGREES, out startTime, out endTime);
			startTime += Planetarium.GetUniversalTime();
			endTime += Planetarium.GetUniversalTime();
		}
	}

	public override void OnUIEndInitialization(AlarmUIDisplayMode displayMode)
	{
		OnUIInputPanelDataChanged(displayMode);
	}

	public override void OnInputPanelUpdate(AlarmUIDisplayMode displayMode)
	{
	}

	public override void OnAlarmSave(ConfigNode node)
	{
	}

	public override void OnAlarmLoad(ConfigNode node)
	{
	}
}
