using System.Collections.Generic;
using ns11;
using ns9;

namespace PreFlightTests;

public class DecouplersBeforeClamps : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_251251;

	public static string cacheAutoLOC_251252;

	public static string cacheAutoLOC_251258;

	public static string cacheAutoLOC_251259;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts = StageManager.FindPartsWithModuleSeparatingBeforeOtherPartsWithModule<ModuleDecouple, LaunchClamp>(ship.parts);
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251251;
		}
		return cacheAutoLOC_251252;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251258;
		}
		return cacheAutoLOC_251259;
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
		cacheAutoLOC_251251 = Localizer.Format("#autoLOC_251251");
		cacheAutoLOC_251252 = Localizer.Format("#autoLOC_251252");
		cacheAutoLOC_251258 = Localizer.Format("#autoLOC_251258");
		cacheAutoLOC_251259 = Localizer.Format("#autoLOC_251259");
	}
}
