using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class ElectricBatteryAndNoCharge : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public bool alternatorException;

	public static string cacheAutoLOC_251805;

	public static string cacheAutoLOC_251806;

	public static string cacheAutoLOC_251814;

	public static string cacheAutoLOC_251815;

	public static string cacheAutoLOC_251820;

	public static string cacheAutoLOC_251821;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		alternatorException = false;
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if (part.Resources.Contains("ElectricCharge") && part.FindModuleImplementing<ModuleCommand>() == null && part.FindModuleImplementing<ModuleAlternator>() == null)
			{
				failedParts.Add(part);
			}
		}
		if (failedParts.Count > 0)
		{
			for (int j = 0; j < count; j++)
			{
				Part part = ship[j];
				if (!part.HasModuleImplementing<ModuleDeployableSolarPanel>() && (!(part.name != "launchClamp1") || !part.isGenerator(out var generator) || !RUIutils.Any(generator.resHandler.outputResources, (ModuleResource a) => a.name == "ElectricCharge")))
				{
					if (!alternatorException && part.FindModuleImplementing<ModuleAlternator>() != null)
					{
						alternatorException = true;
					}
					continue;
				}
				failedParts.Clear();
				return true;
			}
		}
		return failedParts.Count == 0;
	}

	public override string GetConcernTitle()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251805;
		}
		return cacheAutoLOC_251806;
	}

	public override string GetConcernDescription()
	{
		if (alternatorException)
		{
			if (failedParts.Count == 1)
			{
				return cacheAutoLOC_251814;
			}
			return cacheAutoLOC_251815;
		}
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251820;
		}
		return cacheAutoLOC_251821;
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
		cacheAutoLOC_251805 = Localizer.Format("#autoLOC_251805");
		cacheAutoLOC_251806 = Localizer.Format("#autoLOC_251806");
		cacheAutoLOC_251814 = Localizer.Format("#autoLOC_251814");
		cacheAutoLOC_251815 = Localizer.Format("#autoLOC_251815");
		cacheAutoLOC_251820 = Localizer.Format("#autoLOC_251820");
		cacheAutoLOC_251821 = Localizer.Format("#autoLOC_251821");
	}
}
