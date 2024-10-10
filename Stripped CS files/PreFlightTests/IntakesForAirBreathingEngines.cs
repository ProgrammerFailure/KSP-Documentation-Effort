using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class IntakesForAirBreathingEngines : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_252317;

	public static string cacheAutoLOC_252322;

	public IntakesForAirBreathingEngines(ShipConstruct ship, EditorFacility facility)
	{
		this.ship = ship;
	}

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if (RUIutils.Any(part.FindModulesImplementing<ModuleEngines>(), (ModuleEngines meg) => RUIutils.Any(meg.propellants, (Propellant prop) => prop.name == "IntakeAir")))
			{
				failedParts.Add(part);
			}
		}
		if (failedParts.Count > 0)
		{
			for (int j = 0; j < count; j++)
			{
				Part part = ship[j];
				if (RUIutils.Any(part.FindModulesImplementing<ModuleResourceIntake>(), (ModuleResourceIntake x) => x.resourceName == "IntakeAir"))
				{
					failedParts.Clear();
					return true;
				}
			}
		}
		return failedParts.Count == 0;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.WARNING;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public override string GetConcernTitle()
	{
		return cacheAutoLOC_252317;
	}

	public override string GetConcernDescription()
	{
		return cacheAutoLOC_252322;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_252317 = Localizer.Format("#autoLOC_252317");
		cacheAutoLOC_252322 = Localizer.Format("#autoLOC_252322");
	}
}
