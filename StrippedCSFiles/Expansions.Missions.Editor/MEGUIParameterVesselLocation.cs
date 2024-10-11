using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_VesselLocation]
public class MEGUIParameterVesselLocation : MEGUICompoundParameter
{
	public RectTransform containerHeader;

	public Image imageHeader;

	public Image imageBackGround;

	public MEGUIParameterDropdownList situationDropDown;

	public MEGUIParameterDropdownList facilityDropDown;

	public MEGUIParameterDropdownList launchsiteDropDown;

	public MEGUIParameterCheckbox brakesOn;

	public MEGUIParameterCheckbox splashed;

	public VerticalLayoutGroup dropDownLayoutGroup;

	private VesselLocation vesselLocation;

	private MEGUIParameterCelestialBodyOrbit vesselOrbitLocation;

	private MEGUIParameterCelestialBody_VesselGroundLocation vesselGroundLocation;

	public EditorFacility selectedFacility;

	public MissionSituation.VesselStartSituations selectedStartSituation;

	public VesselLocation FieldValue
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
	public MEGUIParameterVesselLocation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEGUIParameterVesselLocation Create(VesselLocation vesselLocation, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Display()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DropDownAction_Situation(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DropDownAction_Facility(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DropDownAction_LaunchSite(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setSituationType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override ConfigNode GetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnHistoryUpdateDropdowns(ConfigNode data, HistoryType type)
	{
		throw null;
	}
}
