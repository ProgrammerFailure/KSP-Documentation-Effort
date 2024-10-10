using System;
using ns9;
using UnityEngine;

public static class EditorEnumExtensions
{
	public static SpaceCenterFacility ToFacility(this EditorFacility editor)
	{
		return editor switch
		{
			EditorFacility.const_1 => SpaceCenterFacility.VehicleAssemblyBuilding, 
			_ => SpaceCenterFacility.SpaceplaneHangar, 
		};
	}

	public static EditorFacility ToEditor(this SpaceCenterFacility facility)
	{
		switch (facility)
		{
		default:
			return EditorFacility.None;
		case SpaceCenterFacility.Runway:
		case SpaceCenterFacility.SpaceplaneHangar:
			return EditorFacility.const_2;
		case SpaceCenterFacility.LaunchPad:
		case SpaceCenterFacility.VehicleAssemblyBuilding:
			return EditorFacility.const_1;
		}
	}

	public static SpaceCenterFacility GetEditorFacility(this SpaceCenterFacility launchSite)
	{
		switch (launchSite)
		{
		case SpaceCenterFacility.Runway:
		case SpaceCenterFacility.SpaceplaneHangar:
			return SpaceCenterFacility.SpaceplaneHangar;
		default:
			return SpaceCenterFacility.VehicleAssemblyBuilding;
		}
	}

	public static SpaceCenterFacility GetFacility(this PSystemSetup.SpaceCenterFacility pSystemFacility)
	{
		switch (pSystemFacility.name)
		{
		case "LaunchPad":
			return SpaceCenterFacility.LaunchPad;
		case "ResearchAndDevelopment":
		case "RnD":
			return SpaceCenterFacility.ResearchAndDevelopment;
		case "TrackingStation":
			return SpaceCenterFacility.TrackingStation;
		case "Runway":
			return SpaceCenterFacility.Runway;
		case "VAB":
		case "VehicleAssemblyBuilding":
			return SpaceCenterFacility.VehicleAssemblyBuilding;
		case "Administration":
			return SpaceCenterFacility.Administration;
		case "FlagPole":
			Debug.LogError("FlagPole is not an actual Space Center Facility!");
			return SpaceCenterFacility.AstronautComplex;
		case "SPH":
		case "SpaceplaneHangar":
			return SpaceCenterFacility.SpaceplaneHangar;
		default:
			throw new ArgumentNullException("pSystemFacility", pSystemFacility.name + " is not a valid Space Center Facility name!");
		case "MissionControl":
			return SpaceCenterFacility.MissionControl;
		}
	}

	public static string GetSymmetryMethodName(SymmetryMethod method)
	{
		return method switch
		{
			SymmetryMethod.Mirror => Localizer.Format("#autoLOC_6004040"), 
			SymmetryMethod.Radial => Localizer.Format("#autoLOC_6004039"), 
			_ => "Unlocalised symmetry method!", 
		};
	}
}
