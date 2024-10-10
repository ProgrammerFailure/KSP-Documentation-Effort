using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class EnginesJettisonedBeforeUse : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_252125;

	public static string cacheAutoLOC_252126;

	public static string cacheAutoLOC_252132;

	public static string cacheAutoLOC_252133;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if ((part.isEngine() || part.FindModuleImplementing<ModuleEnginesFX>() != null) && RecurseHierarchy(part, part) != null)
			{
				failedParts.Add(part);
			}
		}
		return failedParts.Count == 0;
	}

	public Part RecurseHierarchy(Part p, Part orig)
	{
		if (p.parent == null)
		{
			return null;
		}
		if (p.parent.HasModuleImplementing<ModuleDecouple>() && p.parent.inStageIndex != -1 && p.parent.inverseStage >= orig.inverseStage)
		{
			return p;
		}
		return RecurseHierarchy(p.parent, orig);
	}

	public override string GetConcernTitle()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_252125;
		}
		return cacheAutoLOC_252126;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_252132;
		}
		return cacheAutoLOC_252133;
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
		cacheAutoLOC_252125 = Localizer.Format("#autoLOC_252125");
		cacheAutoLOC_252126 = Localizer.Format("#autoLOC_252126");
		cacheAutoLOC_252132 = Localizer.Format("#autoLOC_252132");
		cacheAutoLOC_252133 = Localizer.Format("#autoLOC_252133");
	}
}
