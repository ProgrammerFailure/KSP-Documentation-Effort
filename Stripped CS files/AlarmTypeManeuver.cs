using System.Collections.Generic;
using ns11;
using ns2;
using ns9;
using UnityEngine;

public class AlarmTypeManeuver : AlarmTypeBase
{
	[AppUI_Dropdown(guiName = "#autoLOC_8003555", dropdownItemsFieldName = "maneuverList")]
	public int maneuverNumberUI;

	[AppUI_Label(guiName = "#autoLOC_8003555")]
	public string maneuverNumberLabelUI = "#autoLOC_8003615";

	[AppUI_RadioBool(guiName = "#autoLOC_8003556", valueText = "", hoverText = "#autoLOC_8003556", guiNameVertAlignment = AppUI_Control.VerticalAlignment.Capline)]
	public bool useBurnTimeMargin = true;

	[AppUI_InputDateTime(guiName = "#autoLOC_8003554", datetimeMode = AppUIMemberDateTime.DateTimeModes.timespan)]
	public double marginEntry = 60.0;

	public List<AppUIMemberDropdown.AppUIDropdownItem> maneuverList;

	public ManeuverNode maneuver;

	public double maneuverUT;

	public double maneuverStartBurnUT;

	public string cantSetAlarmText = "";

	public List<ManeuverNode> nodes;

	public int maneuverIndex;

	public bool futureManeuvers;

	public List<ManeuverNode> uiNodes;

	public bool maenueverDropDownInteractible;

	public AppUIMemberDropdown maneuverDropDown;

	public AppUIMemberLabel maneuverLabel;

	public ManeuverNode Maneuver
	{
		get
		{
			return maneuver;
		}
		set
		{
			maneuver = value;
			UpdateAlarmUT();
		}
	}

	public AlarmTypeManeuver()
	{
		iconURL = "maneuver";
		maneuverList = new List<AppUIMemberDropdown.AppUIDropdownItem>();
		nodes = new List<ManeuverNode>();
		uiNodes = new List<ManeuverNode>();
		GameEvents.onManeuverAdded.Add(OnManeuverNodeChanged);
		GameEvents.onManeuverRemoved.Add(OnManeuverNodeChanged);
		UpdateManeuverList();
	}

	public override string GetDefaultTitle()
	{
		return Localizer.Format("#autoLOC_8003537");
	}

	public override bool RequiresVessel()
	{
		return true;
	}

	public override bool CanSetAlarm(AlarmUIDisplayMode displayMode)
	{
		if (base.IsAlarmVesselTheAvailableVessel)
		{
			return maneuver != null;
		}
		return true;
	}

	public override string CannotSetAlarmText()
	{
		return Localizer.Format("#autoLOC_8003612", cantSetAlarmText);
	}

	public override MapObject.ObjectType MapNodeType()
	{
		return MapObject.ObjectType.ManeuverNode;
	}

	public override bool ShowAlarmMapObject(MapObject mapObject)
	{
		return mapObject.maneuverNode.double_0 > Planetarium.GetUniversalTime() + AlarmClockScenario.Instance.settings.defaultMapNodeMargin;
	}

	public override bool InitializeFromMapObject(MapObject mapObject)
	{
		if (mapObject.vesselRef == null)
		{
			Debug.LogError("[AlarmTypeManeuver]: Unable to create alarm - there is no vessel reference");
			return false;
		}
		maneuver = mapObject.maneuverNode;
		UpdateManeuverList();
		return true;
	}

	public override void OnManeuversLoaded(Vessel vessel, PatchedConicSolver solver)
	{
		if (base.Vessel == vessel && solver != null && solver.maneuverNodes != null)
		{
			for (int i = 0; i < solver.maneuverNodes.Count; i++)
			{
				if (solver.maneuverNodes[i].double_0 == ut + eventOffset)
				{
					maneuver = solver.maneuverNodes[i];
				}
			}
		}
		UpdateManeuverList();
	}

	public override void OnScenarioUpdate()
	{
		UpdateAlarmUT();
	}

	public void UpdateAlarmUT()
	{
		if (maneuver != null)
		{
			maneuverUT = maneuver.double_0;
			if (maneuver.startBurnIn > 0.0)
			{
				maneuverStartBurnUT = Planetarium.GetUniversalTime() + maneuver.startBurnIn;
			}
		}
		eventOffset = marginEntry;
		if (useBurnTimeMargin && maneuverStartBurnUT > 0.0)
		{
			eventOffset += maneuverUT - maneuverStartBurnUT;
		}
		ut = maneuverUT - eventOffset;
	}

	public void OnManeuverNodeChanged(Vessel v, PatchedConicSolver s)
	{
		if (v.persistentId == vesselId)
		{
			UpdateManeuverList();
			if (base.IsEditing)
			{
				uiPanel.RefreshUI();
			}
		}
	}

	public void UpdateManeuverList()
	{
		ManeuverNode maneuverNode = maneuver;
		nodes.Clear();
		if (!AlarmClockScenario.IsVesselAvailable)
		{
			maneuver = null;
			maneuverIndex = -1;
			return;
		}
		Vessel availableVessel = AlarmClockScenario.AvailableVessel;
		int num = -1;
		int num2 = 0;
		if (availableVessel.patchedConicSolver != null && availableVessel.patchedConicSolver.maneuverNodes.Count > 0)
		{
			for (int i = 0; i < availableVessel.patchedConicSolver.maneuverNodes.Count; i++)
			{
				nodes.Add(availableVessel.patchedConicSolver.maneuverNodes[i]);
				if (availableVessel.patchedConicSolver.maneuverNodes[i] == maneuverNode)
				{
					num = num2;
				}
				num2++;
			}
		}
		if (num > -1 && num != maneuverIndex)
		{
			Debug.Log($"[AlarmTypeManeuver]: Flight Plan changed, we found the maneuverIndex {maneuverIndex}->{num}");
		}
		else if (maneuverNode != null && num < 0 && maneuverIndex > -1)
		{
			Debug.Log("[AlarmTypeManeuver]: Flight Plan changed, the maneuver no longer exists, dettaching the alarm from the maneuver");
		}
		maneuverIndex = num;
		if (num > -1)
		{
			maneuver = nodes[maneuverIndex];
		}
		else
		{
			maneuver = null;
		}
	}

	public void BuildManeuverListUI(AlarmUIDisplayMode displayMode)
	{
		maneuverList.Clear();
		uiNodes.Clear();
		maenueverDropDownInteractible = true;
		if (displayMode == AlarmUIDisplayMode.Add && !AlarmClockScenario.IsVesselAvailable)
		{
			cantSetAlarmText = "No Available Vessel";
			maneuverList.Add(new AppUIMemberDropdown.AppUIDropdownItem
			{
				text = cantSetAlarmText,
				key = 0
			});
			maneuver = null;
			maneuverNumberUI = 0;
			maenueverDropDownInteractible = false;
			return;
		}
		futureManeuvers = false;
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < nodes.Count; i++)
		{
			if (displayMode != 0 || nodes[i].double_0 > Planetarium.GetUniversalTime())
			{
				maneuverList.Add(new AppUIMemberDropdown.AppUIDropdownItem
				{
					text = Localizer.Format("#autoLOC_8003613", num2 + 1),
					key = num2
				});
				uiNodes.Add(nodes[i]);
				if (nodes[i] == maneuver)
				{
					num = num2;
				}
				futureManeuvers = true;
				num2++;
			}
		}
		if (displayMode == AlarmUIDisplayMode.Add && !futureManeuvers)
		{
			cantSetAlarmText = Localizer.Format("#autoLOC_8003618");
			maneuverList.Add(new AppUIMemberDropdown.AppUIDropdownItem
			{
				text = cantSetAlarmText,
				key = 0
			});
			maneuver = null;
			maenueverDropDownInteractible = false;
			maneuverNumberUI = 0;
		}
		else if (base.IsAlarmVesselTheAvailableVessel)
		{
			if (num > -1)
			{
				maneuverNumberUI = num;
			}
			else
			{
				maneuverNumberUI = 0;
			}
			maneuver = uiNodes[maneuverNumberUI];
			maneuverIndex = nodes.IndexOf(maneuver);
		}
	}

	public override void OnUIInitialization(AlarmUIDisplayMode displayMode)
	{
		if (displayMode == AlarmUIDisplayMode.Add)
		{
			if (!AlarmClockScenario.IsVesselAvailable)
			{
				return;
			}
			if (base.IsMapNodeDefined)
			{
				marginEntry = AlarmClockScenario.Instance.settings.defaultMapNodeMargin;
			}
			UpdateManeuverList();
		}
		BuildManeuverListUI(displayMode);
	}

	public override void OnUIEndInitialization(AlarmUIDisplayMode displayMode)
	{
		if (displayMode != AlarmUIDisplayMode.Edit || !(base.TimeToAlarm < 0.0))
		{
			maneuverDropDown = (AppUIMemberDropdown)uiPanel.GetControl("maneuverNumberUI");
			maneuverLabel = (AppUIMemberLabel)uiPanel.GetControl("maneuverNumberLabelUI");
			maneuverDropDown.dropdown.interactable = maenueverDropDownInteractible;
			maneuverDropDown.dropdown.RefreshShownValue();
		}
	}

	public override void OnInputPanelUpdate(AlarmUIDisplayMode displayMode)
	{
		if (maneuver != null && base.IsAlarmVesselTheAvailableVessel)
		{
			maneuver = uiNodes[maneuverNumberUI];
			maneuverIndex = nodes.IndexOf(maneuver);
		}
		maneuverDropDown.gameObject.SetActive(base.IsAlarmVesselTheAvailableVessel);
		maneuverLabel.gameObject.SetActive(!base.IsAlarmVesselTheAvailableVessel);
		UpdateAlarmUT();
	}

	public override void OnAlarmSave(ConfigNode node)
	{
		node.AddValue("maneuverUT", maneuverUT);
		node.AddValue("maneuverStartBurnUT", maneuverStartBurnUT);
		node.AddValue("marginEntry", marginEntry);
		node.AddValue("useBurnTimeMargin", useBurnTimeMargin);
	}

	public override void OnAlarmLoad(ConfigNode node)
	{
		maneuverUT = 0.0;
		maneuverStartBurnUT = -1.0;
		node.TryGetValue("maneuverUT", ref maneuverUT);
		node.TryGetValue("maneuverStartBurnUT", ref maneuverStartBurnUT);
		node.TryGetValue("marginEntry", ref marginEntry);
		node.TryGetValue("useBurnTimeMargin", ref useBurnTimeMargin);
	}
}
