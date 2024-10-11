using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;

public class AlarmTypeManeuver : AlarmTypeBase
{
	[AppUI_Dropdown(guiName = "#autoLOC_8003555", dropdownItemsFieldName = "maneuverList")]
	public int maneuverNumberUI;

	[AppUI_Label(guiName = "#autoLOC_8003555")]
	public string maneuverNumberLabelUI;

	[AppUI_RadioBool(guiName = "#autoLOC_8003556", valueText = "", hoverText = "#autoLOC_8003556", guiNameVertAlignment = AppUI_Control.VerticalAlignment.Capline)]
	public bool useBurnTimeMargin;

	[AppUI_InputDateTime(guiName = "#autoLOC_8003554", datetimeMode = AppUIMemberDateTime.DateTimeModes.timespan)]
	public double marginEntry;

	private List<AppUIMemberDropdown.AppUIDropdownItem> maneuverList;

	private ManeuverNode maneuver;

	private double maneuverUT;

	private double maneuverStartBurnUT;

	private string cantSetAlarmText;

	private List<ManeuverNode> nodes;

	private int maneuverIndex;

	private bool futureManeuvers;

	private List<ManeuverNode> uiNodes;

	private bool maenueverDropDownInteractible;

	private AppUIMemberDropdown maneuverDropDown;

	private AppUIMemberLabel maneuverLabel;

	public ManeuverNode Maneuver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmTypeManeuver()
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
	public override MapObject.ObjectType MapNodeType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool ShowAlarmMapObject(MapObject mapObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool InitializeFromMapObject(MapObject mapObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnManeuversLoaded(Vessel vessel, PatchedConicSolver solver)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnManeuverNodeChanged(Vessel v, PatchedConicSolver s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateManeuverList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildManeuverListUI(AlarmUIDisplayMode displayMode)
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
