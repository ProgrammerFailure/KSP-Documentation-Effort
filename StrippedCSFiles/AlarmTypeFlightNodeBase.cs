using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;

public abstract class AlarmTypeFlightNodeBase : AlarmTypeBase
{
	[AppUI_InputDateTime(guiName = "#autoLOC_8003554", datetimeMode = AppUIMemberDateTime.DateTimeModes.timespan)]
	public double marginEntry;

	protected Orbit orbitCache;

	private double nodeUT;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmTypeFlightNodeBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool RequiresVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnScenarioUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateAlarmUT()
	{
		throw null;
	}

	protected abstract double GetNodeUT();

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
