using ns9;
using UnityEngine;

namespace PreFlightTests;

public class CraftWithinSizeLimits : IPreFlightTest
{
	public Vector3 craftSize;

	public Vector3 maxSizeAllowed;

	public string facilityName;

	public string shipName;

	public SpaceCenterFacility facility;

	public LaunchSite launchsite;

	public bool isSCFacility = true;

	public static string cacheAutoLOC_250789;

	public static string cacheAutoLOC_250834;

	public CraftWithinSizeLimits(ShipConstruct ship, SpaceCenterFacility facility, Vector3 maxSize)
	{
		facilityName = Localizer.Format(KSPUtil.PrintLocalizedModuleName(facility.ToString()));
		craftSize = ShipConstruction.CalculateCraftSize(ship);
		maxSizeAllowed = maxSize;
		shipName = ship.shipName;
		this.facility = facility;
		isSCFacility = true;
	}

	public CraftWithinSizeLimits(ShipTemplate template, SpaceCenterFacility facility, Vector3 maxSize)
	{
		facilityName = Localizer.Format(KSPUtil.PrintLocalizedModuleName(facility.ToString()));
		craftSize = ShipConstruction.CalculateCraftSize(template);
		maxSizeAllowed = maxSize;
		shipName = template.shipName;
		this.facility = facility;
		isSCFacility = true;
	}

	public CraftWithinSizeLimits(Vector3 shipSize, string shipName, SpaceCenterFacility facility, Vector3 maxSize)
	{
		facilityName = Localizer.Format(KSPUtil.PrintLocalizedModuleName(facility.ToString()));
		craftSize = shipSize;
		maxSizeAllowed = maxSize;
		this.shipName = shipName;
		this.facility = facility;
		isSCFacility = true;
	}

	public CraftWithinSizeLimits(ShipConstruct ship, LaunchSite facility, Vector3 maxSize)
	{
		facilityName = KSPUtil.PrintModuleName(facility.ToString());
		craftSize = ShipConstruction.CalculateCraftSize(ship);
		maxSizeAllowed = maxSize;
		shipName = ship.shipName;
		launchsite = facility;
		isSCFacility = false;
	}

	public CraftWithinSizeLimits(ShipTemplate template, LaunchSite facility, Vector3 maxSize)
	{
		facilityName = KSPUtil.PrintModuleName(facility.ToString());
		craftSize = ShipConstruction.CalculateCraftSize(template);
		maxSizeAllowed = maxSize;
		shipName = template.shipName;
		launchsite = facility;
		isSCFacility = false;
	}

	public bool Test()
	{
		if (craftSize.x <= maxSizeAllowed.x && craftSize.y <= maxSizeAllowed.y && craftSize.z <= maxSizeAllowed.z)
		{
			return craftSize != Vector3.zero;
		}
		return false;
	}

	public string GetWarningTitle()
	{
		if (craftSize == Vector3.zero)
		{
			return cacheAutoLOC_250789;
		}
		return Localizer.Format("#autoLOC_250793", facilityName);
	}

	public string GetWarningDescription()
	{
		if (craftSize == Vector3.zero)
		{
			return Localizer.Format("#autoLOC_250801", shipName, KSPUtil.PrintModuleName(facility.GetEditorFacility().ToString()));
		}
		if (isSCFacility)
		{
			switch (facility)
			{
			default:
				return Localizer.Format("#autoLOC_250811", shipName) + "\n" + Localizer.Format("#autoLOC_250812", craftSize.y.ToString("0.##"), maxSizeAllowed.y.ToString("0.##")) + "\n" + Localizer.Format("#autoLOC_250813", craftSize.x.ToString("0.##"), maxSizeAllowed.x.ToString("0.##")) + "\n" + Localizer.Format("#autoLOC_250814", craftSize.z.ToString("0.##"), maxSizeAllowed.z.ToString("0.##")) + "\n";
			case SpaceCenterFacility.LaunchPad:
			case SpaceCenterFacility.VehicleAssemblyBuilding:
				return Localizer.Format("#autoLOC_250811", shipName) + "\n" + Localizer.Format("#autoLOC_250820", craftSize.y.ToString("0.##"), maxSizeAllowed.y.ToString("0.##")) + "\n" + Localizer.Format("#autoLOC_250821", craftSize.x.ToString("0.##"), maxSizeAllowed.x.ToString("0.##")) + "\n";
			}
		}
		EditorFacility editorFacility = launchsite.editorFacility;
		if (editorFacility != EditorFacility.const_1)
		{
			return Localizer.Format("#autoLOC_250811", shipName) + "\n" + Localizer.Format("#autoLOC_250812", craftSize.y.ToString("0.##"), maxSizeAllowed.y.ToString("0.##")) + "\n" + Localizer.Format("#autoLOC_250813", craftSize.x.ToString("0.##"), maxSizeAllowed.x.ToString("0.##")) + "\n" + Localizer.Format("#autoLOC_250814", craftSize.z.ToString("0.##"), maxSizeAllowed.z.ToString("0.##")) + "\n";
		}
		return Localizer.Format("#autoLOC_250811", shipName) + "\n" + Localizer.Format("#autoLOC_250820", craftSize.y.ToString("0.##"), maxSizeAllowed.y.ToString("0.##")) + "\n" + Localizer.Format("#autoLOC_250821", craftSize.x.ToString("0.##"), maxSizeAllowed.x.ToString("0.##")) + "\n";
	}

	public string GetProceedOption()
	{
		return null;
	}

	public string GetAbortOption()
	{
		return cacheAutoLOC_250834;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_250789 = Localizer.Format("#autoLOC_250789");
		cacheAutoLOC_250834 = Localizer.Format("#autoLOC_250834");
	}
}
