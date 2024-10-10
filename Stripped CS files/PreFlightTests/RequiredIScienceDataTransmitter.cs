using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class RequiredIScienceDataTransmitter : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_252827;

	public static string cacheAutoLOC_252834;

	public static string cacheAutoLOC_252835;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		if (!RUIutils.Any(ship.parts, (Part a) => a.FindModuleImplementing<IScienceDataTransmitter>() != null))
		{
			failedParts = RUIutils.Where(ship.parts, (Part a) => a.FindModuleImplementing<ModuleOrbitalSurveyor>() != null);
		}
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		_ = failedParts.Count;
		return cacheAutoLOC_252827;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_252834;
		}
		return cacheAutoLOC_252835;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.CRITICAL;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_252827 = Localizer.Format("#autoLOC_252827");
		cacheAutoLOC_252834 = Localizer.Format("#autoLOC_252834");
		cacheAutoLOC_252835 = Localizer.Format("#autoLOC_252835");
	}
}
