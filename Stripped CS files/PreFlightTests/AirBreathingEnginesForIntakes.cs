using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class AirBreathingEnginesForIntakes : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_250906;

	public static string cacheAutoLOC_250911;

	public AirBreathingEnginesForIntakes(ShipConstruct ship, EditorFacility facility)
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
			if (RUIutils.Any(part.FindModulesImplementing<ModuleResourceIntake>(), (ModuleResourceIntake x) => x.resourceName == "IntakeAir"))
			{
				failedParts.Add(part);
			}
		}
		if (failedParts.Count > 0)
		{
			for (int j = 0; j < count; j++)
			{
				Part part = ship[j];
				if (RUIutils.Any(part.FindModulesImplementing<ModuleEngines>(), (ModuleEngines meg) => RUIutils.Any(meg.propellants, (Propellant prop) => prop.name == "IntakeAir")))
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
		return cacheAutoLOC_250906;
	}

	public override string GetConcernDescription()
	{
		return cacheAutoLOC_250911;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_250906 = Localizer.Format("#autoLOC_250906");
		cacheAutoLOC_250911 = Localizer.Format("#autoLOC_250911");
	}
}
