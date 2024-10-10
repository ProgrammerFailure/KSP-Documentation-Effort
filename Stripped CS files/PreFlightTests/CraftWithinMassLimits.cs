using ns9;

namespace PreFlightTests;

public class CraftWithinMassLimits : IPreFlightTest
{
	public double craftMass;

	public double maxMass;

	public string facilityName;

	public string shipName;

	public static string cacheAutoLOC_250677;

	public static string cacheAutoLOC_250692;

	public CraftWithinMassLimits(ShipConstruct ship, SpaceCenterFacility facility, double maxMass)
	{
		facilityName = Localizer.Format(KSPUtil.PrintLocalizedModuleName(facility.ToString()));
		craftMass = ship.GetTotalMass();
		this.maxMass = maxMass;
		shipName = ship.shipName;
	}

	public CraftWithinMassLimits(ShipTemplate template, SpaceCenterFacility facility, double maxMass)
	{
		facilityName = Localizer.Format(KSPUtil.PrintLocalizedModuleName(facility.ToString()));
		craftMass = template.totalMass;
		this.maxMass = maxMass;
		shipName = template.shipName;
	}

	public CraftWithinMassLimits(float mass, string name, SpaceCenterFacility facility, double maxMass)
	{
		facilityName = Localizer.Format(KSPUtil.PrintLocalizedModuleName(facility.ToString()));
		craftMass = mass;
		this.maxMass = maxMass;
		shipName = name;
	}

	public CraftWithinMassLimits(ShipConstruct ship, LaunchSite facility, double maxMass)
	{
		facilityName = KSPUtil.PrintModuleName(facility.ToString());
		craftMass = ship.GetTotalMass();
		this.maxMass = maxMass;
		shipName = ship.shipName;
	}

	public CraftWithinMassLimits(ShipTemplate template, LaunchSite facility, double maxMass)
	{
		facilityName = KSPUtil.PrintModuleName(facility.ToString());
		craftMass = template.totalMass;
		this.maxMass = maxMass;
		shipName = template.shipName;
	}

	public bool Test()
	{
		return craftMass <= maxMass;
	}

	public string GetWarningTitle()
	{
		return cacheAutoLOC_250677;
	}

	public string GetWarningDescription()
	{
		return Localizer.Format("#autoLOC_250682", facilityName, maxMass.ToString("0.##"), shipName, craftMass.ToString("0.##"));
	}

	public string GetProceedOption()
	{
		return null;
	}

	public string GetAbortOption()
	{
		return cacheAutoLOC_250692;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_250677 = Localizer.Format("#autoLOC_250677");
		cacheAutoLOC_250692 = Localizer.Format("#autoLOC_250692");
	}
}
