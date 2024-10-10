using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class StationHubAttachments : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_253109;

	public static string cacheAutoLOC_253115;

	public static string cacheAutoLOC_253116;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if (part.name == "stationHub" && RUIutils.Any(part.attachNodes, (AttachNode b) => b.attachedPart == null))
			{
				failedParts.Add(part);
			}
		}
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		return cacheAutoLOC_253109;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_253115;
		}
		return cacheAutoLOC_253116;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.NOTICE;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_253109 = Localizer.Format("#autoLOC_253109");
		cacheAutoLOC_253115 = Localizer.Format("#autoLOC_253115");
		cacheAutoLOC_253116 = Localizer.Format("#autoLOC_253116");
	}
}
