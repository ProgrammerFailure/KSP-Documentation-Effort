using ns9;

namespace PreFlightTests;

public class InputAuthority : DesignConcernBase
{
	public ShipConstruct ship;

	public static string cacheAutoLOC_252231;

	public static string cacheAutoLOC_252236;

	public InputAuthority(ShipConstruct ship)
	{
		this.ship = ship;
	}

	public override bool TestCondition()
	{
		_ = ship.parts.Count;
		return true;
	}

	public override string GetConcernTitle()
	{
		return cacheAutoLOC_252231;
	}

	public override string GetConcernDescription()
	{
		return cacheAutoLOC_252236;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.WARNING;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_252231 = Localizer.Format("#autoLOC_252231");
		cacheAutoLOC_252236 = Localizer.Format("#autoLOC_252236");
	}
}
