using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class ParachutePresent : DesignConcernBase
{
	public ShipConstruct ship;

	public VesselCrewManifest manifest;

	public bool chutePresentInSameStage;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_252764;

	public static string cacheAutoLOC_252765;

	public static string cacheAutoLOC_252773;

	public static string cacheAutoLOC_252774;

	public static string cacheAutoLOC_252779;

	public static string cacheAutoLOC_252780;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		manifest = ShipConstruction.ShipManifest;
		chutePresentInSameStage = false;
		if (manifest == null)
		{
			return false;
		}
		int count = ship.Count;
		Part p;
		for (int i = 0; i < count; i++)
		{
			p = ship[i];
			PartCrewManifest partCrewManifest = manifest.GetPartCrewManifest(p.craftID);
			if (partCrewManifest != null && partCrewManifest.PartInfo.partPrefab.isControlSource > Vessel.ControlLevel.NONE && p != null && p.CrewCapacity > 0 && (bool)p.FindModuleImplementing<ModuleCommand>() && partCrewManifest.AnySeatTaken() && (!(p.parent != null) || !p.parent.isParachute()) && !RUIutils.Any(p.children, (Part a) => a.FindModuleImplementing<ModuleParachute>()))
			{
				if (RUIutils.Any(ship.parts, (Part a) => a.separationIndex == p.separationIndex && (bool)a.FindModuleImplementing<ModuleParachute>()))
				{
					chutePresentInSameStage = true;
				}
				failedParts.Add(p);
			}
		}
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		if (chutePresentInSameStage)
		{
			return cacheAutoLOC_252764;
		}
		return cacheAutoLOC_252765;
	}

	public override string GetConcernDescription()
	{
		if (chutePresentInSameStage)
		{
			if (failedParts.Count == 1)
			{
				return cacheAutoLOC_252773;
			}
			return cacheAutoLOC_252774;
		}
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_252779;
		}
		return cacheAutoLOC_252780;
	}

	public override EditorFacilities GetEditorFacilities()
	{
		return EditorFacilities.flag_2;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.WARNING;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_252764 = Localizer.Format("#autoLOC_252764");
		cacheAutoLOC_252765 = Localizer.Format("#autoLOC_252765");
		cacheAutoLOC_252773 = Localizer.Format("#autoLOC_252773");
		cacheAutoLOC_252774 = Localizer.Format("#autoLOC_252774");
		cacheAutoLOC_252779 = Localizer.Format("#autoLOC_252779");
		cacheAutoLOC_252780 = Localizer.Format("#autoLOC_252780");
	}
}
