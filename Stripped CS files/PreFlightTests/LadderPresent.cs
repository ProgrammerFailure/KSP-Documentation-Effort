using ns9;

namespace PreFlightTests;

public class LadderPresent : DesignConcernBase
{
	public ShipConstruct ship;

	public static string cacheAutoLOC_252377;

	public static string cacheAutoLOC_252382;

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		int count = ship.Count;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if (part.FindModuleImplementing<ModuleWheelBase>() != null)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					Part part = ship[num];
					if ((bool)part.FindModuleImplementing<RetractableLadder>() || part.name.ToLower().Contains("ladder"))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}
		return true;
	}

	public override string GetConcernTitle()
	{
		return cacheAutoLOC_252377;
	}

	public override string GetConcernDescription()
	{
		return cacheAutoLOC_252382;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.NOTICE;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_252377 = Localizer.Format("#autoLOC_252377");
		cacheAutoLOC_252382 = Localizer.Format("#autoLOC_252382");
	}
}
