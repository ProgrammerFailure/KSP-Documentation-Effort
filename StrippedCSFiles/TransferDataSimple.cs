using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;

[Serializable]
public class TransferDataSimple : TransferDataBase
{
	[AppUI_Heading(hideOnError = true, guiName = "#autoLOC_6002666", order = 2, hoverText = "#autoLOC_6002660", textAlignment = AppUI_Control.HorizontalAlignment.Left)]
	public string UICircularizeHeader;

	[AppUI_Label(hideOnError = true, guiName = "", order = 3, hoverText = "#autoLOC_6002661", showGuiName = false, textAlignment = AppUI_Control.HorizontalAlignment.Left)]
	public string lowCircularizationDv;

	[AppUI_Label(hideOnError = true, guiName = "", order = 4, hoverText = "#autoLOC_6002661", showGuiName = false, textAlignment = AppUI_Control.HorizontalAlignment.Left)]
	public string highCircularizationDv;

	private bool _transferCircularize;

	[SerializeField]
	private CelestialBody sourceBody;

	[SerializeField]
	private CelestialBody targetBody;

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

	public Vector3d transferdV;

	public double circularizedV;

	public double vesselOriginOrbitalSpeed;

	public double ejectionInclination;

	public Vector3d sourceBodyVelocity;

	internal bool correctionBurnRequired;

	internal double correctionBurnTime;

	internal Vector3d correctiondV;

	internal double otherNodeTime;

	public List<AppUIMemberDropdown.AppUIDropdownItem> cbItems;

	[AppUI_DropdownMultiLine(dropdownItemsFieldName = "cbItems", order = 1, hoverText = "#autoLOC_6002659", guiName = "#autoLOC_6002665")]
	public string targetBodyName
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

	public bool transferCircularize
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

	public CelestialBody SourceBody
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

	public CelestialBody TargetBody
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

	public double PhaseAngleTarget360
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double PhaseAngleCurrent360
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TransferDataSimple(Callback dataChangedCallback, Callback<ManeuverCalculationState, int> calculationStateChangedCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetAlarmTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetAlarmDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsAnyDropdownOpen(AppUIInputPanel panel)
	{
		throw null;
	}
}
