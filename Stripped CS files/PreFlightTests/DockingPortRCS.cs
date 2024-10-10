using System.Collections.Generic;
using ns11;
using ns9;

namespace PreFlightTests;

public class DockingPortRCS : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_251727;

	public static string cacheAutoLOC_251732;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts = StageManager.FindPartsWithModuleSeparatingBeforeOtherPartsWithModule<ModuleDockingNode, ModuleRCS>(ship.parts);
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		return cacheAutoLOC_251727;
	}

	public override string GetConcernDescription()
	{
		return cacheAutoLOC_251732;
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
		cacheAutoLOC_251727 = Localizer.Format("#autoLOC_251727");
		cacheAutoLOC_251732 = Localizer.Format("#autoLOC_251732");
	}
}
