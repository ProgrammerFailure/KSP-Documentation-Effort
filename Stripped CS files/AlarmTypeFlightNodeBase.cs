using ns11;
using ns2;

public abstract class AlarmTypeFlightNodeBase : AlarmTypeBase
{
	[AppUI_InputDateTime(guiName = "#autoLOC_8003554", datetimeMode = AppUIMemberDateTime.DateTimeModes.timespan)]
	public double marginEntry = 60.0;

	public Orbit orbitCache;

	public double nodeUT;

	public AlarmTypeFlightNodeBase()
	{
	}

	public override bool RequiresVessel()
	{
		return true;
	}

	public override void OnScenarioUpdate()
	{
		if (!base.Triggered)
		{
			UpdateAlarmUT();
		}
	}

	public void UpdateAlarmUT()
	{
		if (base.IsAlarmVesselTheAvailableVessel)
		{
			orbitCache = base.Vessel.orbit;
			nodeUT = GetNodeUT();
		}
		eventOffset = marginEntry;
		ut = nodeUT - eventOffset;
	}

	public abstract double GetNodeUT();

	public override void OnUIInitialization(AlarmUIDisplayMode displayMode)
	{
		if (displayMode == AlarmUIDisplayMode.Add && AlarmClockScenario.IsVesselAvailable)
		{
			if (base.Vessel != null)
			{
				orbitCache = base.Vessel.orbit;
			}
			if (base.IsMapNodeDefined)
			{
				marginEntry = AlarmClockScenario.Instance.settings.defaultMapNodeMargin;
			}
		}
	}

	public override void OnUIEndInitialization(AlarmUIDisplayMode displayMode)
	{
	}

	public override void OnInputPanelUpdate(AlarmUIDisplayMode displayMode)
	{
		UpdateAlarmUT();
	}

	public override void OnAlarmSave(ConfigNode node)
	{
		node.AddValue("nodeUT", nodeUT);
		node.AddValue("marginEntry", marginEntry);
	}

	public override void OnAlarmLoad(ConfigNode node)
	{
		node.TryGetValue("nodeUT", ref nodeUT);
		node.TryGetValue("marginEntry", ref marginEntry);
	}
}
