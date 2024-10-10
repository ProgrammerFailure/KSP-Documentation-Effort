using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class ElectricChargeAndNoBattery : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_251890;

	public static string cacheAutoLOC_251891;

	public static string cacheAutoLOC_251897;

	public static string cacheAutoLOC_251898;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if (part.HasModuleImplementing<ModuleDeployableSolarPanel>() || (part.name != "launchClamp1" && part.isGenerator(out var generator) && RUIutils.Any(generator.resHandler.outputResources, (ModuleResource a) => a.name == "ElectricCharge")))
			{
				failedParts.Add(part);
			}
		}
		if (failedParts.Count > 0)
		{
			for (int j = 0; j < count; j++)
			{
				Part part = ship[j];
				if (part.Resources.Contains("ElectricCharge") && part.FindModuleImplementing<ModuleCommand>() == null && part.FindModuleImplementing<ModuleAlternator>() == null)
				{
					failedParts.Clear();
					return true;
				}
			}
		}
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251890;
		}
		return cacheAutoLOC_251891;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251897;
		}
		return cacheAutoLOC_251898;
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
		cacheAutoLOC_251890 = Localizer.Format("#autoLOC_251890");
		cacheAutoLOC_251891 = Localizer.Format("#autoLOC_251891");
		cacheAutoLOC_251897 = Localizer.Format("#autoLOC_251897");
		cacheAutoLOC_251898 = Localizer.Format("#autoLOC_251898");
	}
}
