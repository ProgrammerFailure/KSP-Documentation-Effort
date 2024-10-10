using ns9;

namespace PreFlightTests;

public class CanAffordLaunchTest : IPreFlightTest
{
	public float launchCost;

	public double fundsAvailable;

	public static string cacheAutoLOC_250625;

	public static string cacheAutoLOC_250630;

	public static string cacheAutoLOC_250640;

	public CanAffordLaunchTest(ShipConstruct ship, Funding funding)
	{
		if (funding != null)
		{
			launchCost = ship.GetShipCosts(out var _, out var _, ShipConstruction.ShipManifest);
			fundsAvailable = funding.Funds;
		}
		else
		{
			launchCost = 0f;
			fundsAvailable = 3.4028234663852886E+38;
		}
	}

	public CanAffordLaunchTest(ShipTemplate template, Funding funding)
	{
		if (funding != null)
		{
			launchCost = template.totalCost;
			fundsAvailable = funding.Funds;
		}
		else
		{
			launchCost = 0f;
			fundsAvailable = 3.4028234663852886E+38;
		}
	}

	public bool Test()
	{
		return (double)launchCost <= fundsAvailable;
	}

	public string GetWarningTitle()
	{
		return cacheAutoLOC_250625;
	}

	public string GetWarningDescription()
	{
		return cacheAutoLOC_250630;
	}

	public string GetProceedOption()
	{
		return null;
	}

	public string GetAbortOption()
	{
		return cacheAutoLOC_250640;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_250625 = Localizer.Format("#autoLOC_250625");
		cacheAutoLOC_250630 = Localizer.Format("#autoLOC_250630");
		cacheAutoLOC_250640 = Localizer.Format("#autoLOC_250640");
	}
}
