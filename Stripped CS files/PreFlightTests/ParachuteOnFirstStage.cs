using System.Collections.Generic;
using ns11;
using ns9;

namespace PreFlightTests;

public class ParachuteOnFirstStage : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_252675;

	public static string cacheAutoLOC_252676;

	public static string cacheAutoLOC_252682;

	public static string cacheAutoLOC_252683;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if (part.isParachute() && part.inverseStage == StageManager.LastStage)
			{
				failedParts.Add(part);
			}
		}
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_252675;
		}
		return cacheAutoLOC_252676;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_252682;
		}
		return cacheAutoLOC_252683;
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
		cacheAutoLOC_252675 = Localizer.Format("#autoLOC_252675");
		cacheAutoLOC_252676 = Localizer.Format("#autoLOC_252676");
		cacheAutoLOC_252682 = Localizer.Format("#autoLOC_252682");
		cacheAutoLOC_252683 = Localizer.Format("#autoLOC_252683");
	}
}
