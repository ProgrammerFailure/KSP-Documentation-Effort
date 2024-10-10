using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class DecouplerFacing : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_251200;

	public static string cacheAutoLOC_251201;

	public static string cacheAutoLOC_251207;

	public static string cacheAutoLOC_251208;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		List<Part> list = new List<Part>(ship.parts);
		RUIutils.WhereMutating(ref list, (Part a) => a.FindModuleImplementing<ModuleDecouple>() != null);
		Part part = null;
		for (int i = 0; i < list.Count; i++)
		{
			part = list[i];
			part.isDecoupler(out var moduleDecoupler);
			AttachNode attachNode = part.FindAttachNode(moduleDecoupler.explosiveNodeID);
			if (!moduleDecoupler.isOmniDecoupler && attachNode != null && attachNode.attachedPart != part.parent)
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
			return cacheAutoLOC_251200;
		}
		return cacheAutoLOC_251201;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251207;
		}
		return cacheAutoLOC_251208;
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
		cacheAutoLOC_251200 = Localizer.Format("#autoLOC_251200");
		cacheAutoLOC_251201 = Localizer.Format("#autoLOC_251201");
		cacheAutoLOC_251207 = Localizer.Format("#autoLOC_251207");
		cacheAutoLOC_251208 = Localizer.Format("#autoLOC_251208");
	}
}
