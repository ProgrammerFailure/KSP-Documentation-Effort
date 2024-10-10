using ns9;

namespace PreFlightTests;

public class WrongVesselTypeForLaunchSite : IPreFlightTest
{
	public EditorFacility correctType;

	public string craftFile;

	public string vesselType;

	public string launchSiteName;

	public WrongVesselTypeForLaunchSite(EditorFacility CorrectType, string CraftFileName, string VesselTypeName, string LaunchSiteName)
	{
		correctType = CorrectType;
		craftFile = CraftFileName;
		vesselType = VesselTypeName;
		launchSiteName = LaunchSiteName;
	}

	public bool Test()
	{
		return ShipConstruction.CheckCraftFileType(craftFile) == correctType;
	}

	public string GetWarningTitle()
	{
		return Localizer.Format("#autoLOC_253804");
	}

	public string GetWarningDescription()
	{
		return Localizer.Format("#autoLOC_253809", vesselType, launchSiteName);
	}

	public string GetProceedOption()
	{
		return Localizer.Format("#autoLOC_253814");
	}

	public string GetAbortOption()
	{
		return Localizer.Format("#autoLOC_253819");
	}
}
