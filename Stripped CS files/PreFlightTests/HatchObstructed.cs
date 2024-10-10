using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class HatchObstructed : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_252190;

	public static string cacheAutoLOC_252195;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if (part.CrewCapacity <= 0 || !(part.airlock != null) || !FlightEVA.HatchIsObstructedMore(part, part.airlock))
			{
				continue;
			}
			if (part.partInfo.name == "MK1CrewCabin")
			{
				AttachNode attachNode = part.FindAttachNode("top");
				if (attachNode != null && attachNode.attachedPart != null && attachNode.attachedPart.CrewCapacity > 0)
				{
					continue;
				}
			}
			failedParts.Add(part);
		}
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		return cacheAutoLOC_252190;
	}

	public override string GetConcernDescription()
	{
		return cacheAutoLOC_252195;
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
		cacheAutoLOC_252190 = Localizer.Format("#autoLOC_252190");
		cacheAutoLOC_252195 = Localizer.Format("#autoLOC_252195");
	}
}
