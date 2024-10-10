using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class ElectricConsumerAndNoCharge : DesignConcernBase
{
	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public bool alternatorException;

	public PartResourceDefinition resourceDefinition;

	public static string cacheAutoLOC_252048;

	public static string cacheAutoLOC_252049;

	public static string cacheAutoLOC_252057;

	public static string cacheAutoLOC_252058;

	public static string cacheAutoLOC_252063;

	public ElectricConsumerAndNoCharge()
	{
		resourceDefinition = PartResourceLibrary.Instance.GetDefinition(Localizer.Format("#autoLOC_252004"));
	}

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		alternatorException = false;
		int count = ship.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				Part part = ship[num];
				if (part.HasModuleImplementing<ModuleDeployableSolarPanel>() || (part.name != "launchClamp1" && part.isGenerator(out var generator) && RUIutils.Any(generator.resHandler.outputResources, (ModuleResource a) => a.name == "ElectricCharge")))
				{
					break;
				}
				num++;
				continue;
			}
			for (int i = 0; i < count; i++)
			{
				Part part = ship[i];
				if (!(part.FindModuleImplementing<ModuleCommand>() != null))
				{
					if (RUIutils.Any(part.Modules.GetModules<IResourceConsumer>(), (IResourceConsumer a) => a.GetConsumedResources().Contains(resourceDefinition)))
					{
						failedParts.Add(part);
					}
					if (!alternatorException && part.FindModuleImplementing<ModuleAlternator>() != null)
					{
						alternatorException = true;
					}
				}
			}
			return failedParts.Count == 0;
		}
		return true;
	}

	public override string GetConcernTitle()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_252048;
		}
		return cacheAutoLOC_252049;
	}

	public override string GetConcernDescription()
	{
		if (alternatorException)
		{
			if (failedParts.Count == 1)
			{
				return cacheAutoLOC_252057;
			}
			return cacheAutoLOC_252058;
		}
		_ = failedParts.Count;
		return cacheAutoLOC_252063;
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
		cacheAutoLOC_252048 = Localizer.Format("#autoLOC_252048");
		cacheAutoLOC_252049 = Localizer.Format("#autoLOC_252049");
		cacheAutoLOC_252057 = Localizer.Format("#autoLOC_252057");
		cacheAutoLOC_252058 = Localizer.Format("#autoLOC_252058");
		cacheAutoLOC_252063 = Localizer.Format("#autoLOC_252063");
	}
}
