using System;
using System.Collections.Generic;
using ns11;
using ns2;
using ns9;
using UnityEngine;

[Serializable]
public class TransferDataSimple : TransferDataBase
{
	[AppUI_Heading(hideOnError = true, guiName = "#autoLOC_6002666", order = 2, hoverText = "#autoLOC_6002660", textAlignment = AppUI_Control.HorizontalAlignment.Left)]
	public string UICircularizeHeader = "";

	[AppUI_Label(hideOnError = true, guiName = "", order = 3, hoverText = "#autoLOC_6002661", showGuiName = false, textAlignment = AppUI_Control.HorizontalAlignment.Left)]
	public string lowCircularizationDv = "";

	[AppUI_Label(hideOnError = true, guiName = "", order = 4, hoverText = "#autoLOC_6002661", showGuiName = false, textAlignment = AppUI_Control.HorizontalAlignment.Left)]
	public string highCircularizationDv = "";

	public bool _transferCircularize;

	[SerializeField]
	public CelestialBody sourceBody;

	[SerializeField]
	public CelestialBody targetBody;

	public double arrivalTime;

	public double optimalPhaseAngleTime;

	public double transferTime;

	public double altitudeAtTransferTime;

	public Vector3d ejectionVelocity;

	public Vector3d captureVelocity;

	public double phaseAngleTarget;

	public double phaseAngleCurrent;

	public double transferPe;

	public double ejectAngle;

	public bool ejectAngleRetrograde;

	public Vector3d transferdV = Vector3d.zero;

	public double circularizedV;

	public double vesselOriginOrbitalSpeed;

	public double ejectionInclination;

	public Vector3d sourceBodyVelocity;

	public bool correctionBurnRequired;

	public double correctionBurnTime;

	public Vector3d correctiondV;

	public double otherNodeTime;

	public List<AppUIMemberDropdown.AppUIDropdownItem> cbItems;

	[AppUI_DropdownMultiLine(dropdownItemsFieldName = "cbItems", order = 1, hoverText = "#autoLOC_6002659", guiName = "#autoLOC_6002665")]
	public string targetBodyName
	{
		get
		{
			if (targetBody != null)
			{
				return targetBody.name;
			}
			return "";
		}
		set
		{
			CelestialBody bodyByName = FlightGlobals.GetBodyByName(value);
			if (bodyByName != null)
			{
				targetBody = bodyByName;
				if (dataChangedCallback != null)
				{
					dataChangedCallback();
				}
			}
		}
	}

	public bool transferCircularize
	{
		get
		{
			return _transferCircularize;
		}
		set
		{
			_transferCircularize = value;
			if (dataChangedCallback != null)
			{
				dataChangedCallback();
			}
		}
	}

	public CelestialBody SourceBody
	{
		get
		{
			return sourceBody;
		}
		set
		{
			if (sourceBody == null || sourceBody != value)
			{
				calculationPercentage = 0;
				base.CalculationState = ManeuverCalculationState.waiting;
				sourceBody = value;
			}
		}
	}

	public CelestialBody TargetBody
	{
		get
		{
			return targetBody;
		}
		set
		{
			if (targetBody == null || targetBody != value)
			{
				calculationPercentage = 0;
				base.CalculationState = ManeuverCalculationState.waiting;
				targetBody = value;
			}
		}
	}

	public double PhaseAngleTarget360 => UtilMath.ClampDegrees360(360.0 * phaseAngleTarget);

	public double PhaseAngleCurrent360 => UtilMath.ClampDegrees360(360.0 * phaseAngleCurrent);

	public TransferDataSimple(Callback dataChangedCallback, Callback<ManeuverCalculationState, int> calculationStateChangedCallback)
		: base(dataChangedCallback, calculationStateChangedCallback)
	{
		cbItems = new List<AppUIMemberDropdown.AppUIDropdownItem>();
	}

	public override string GetAlarmTitle()
	{
		string text = "";
		if (vessel != null)
		{
			text = vessel.GetDisplayName() ?? "";
		}
		text += " -> ";
		if (TargetBody != null)
		{
			text += TargetBody.displayName.LocalizeRemoveGender();
		}
		return text;
	}

	public override string GetAlarmDescription()
	{
		string text = Localizer.Format("#autoLOC_8003574") + "\n";
		if (vessel != null)
		{
			text = text + "\t" + Localizer.Format("#autoLOC_8003575", vessel.GetDisplayName()) + "\n";
		}
		if (SourceBody != null)
		{
			text = text + "\t" + Localizer.Format("#autoLOC_8003576", SourceBody.displayName.LocalizeRemoveGender()) + "\n";
		}
		if (TargetBody != null)
		{
			text = text + "\t" + Localizer.Format("#autoLOC_8003577", TargetBody.displayName.LocalizeRemoveGender()) + "\n";
		}
		return text;
	}

	public override bool IsAnyDropdownOpen(AppUIInputPanel panel)
	{
		AppUIMember control = panel.GetControl("targetBodyName");
		if (control != null)
		{
			AppUIMemberDropdown appUIMemberDropdown = control as AppUIMemberDropdown;
			if (appUIMemberDropdown != null && appUIMemberDropdown.dropdown.IsExpanded)
			{
				return true;
			}
		}
		return false;
	}
}
