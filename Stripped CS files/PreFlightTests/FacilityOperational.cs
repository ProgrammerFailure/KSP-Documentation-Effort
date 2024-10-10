using ns9;
using UnityEngine;

namespace PreFlightTests;

public class FacilityOperational : IPreFlightTest
{
	public string facilityName;

	public string facilityTitle;

	public bool operational;

	public FacilityOperational(string FacilityName, string FacilityTitle)
	{
		facilityName = FacilityName;
		facilityTitle = FacilityTitle;
		PSystemSetup.SpaceCenterFacility spaceCenterFacility = PSystemSetup.Instance.GetSpaceCenterFacility(facilityName);
		if (spaceCenterFacility != null)
		{
			operational = spaceCenterFacility.GetFacilityDamage() < 100f;
			return;
		}
		if (PSystemSetup.Instance.GetLaunchSite(FacilityName) != null)
		{
			operational = true;
			return;
		}
		Debug.LogError("[ConstructionFacilityOperational]: Invalid Test Parameters! No Facility called " + facilityName + " exists");
		operational = false;
	}

	public bool Test()
	{
		return operational;
	}

	public string GetWarningTitle()
	{
		return Localizer.Format("#autoLOC_253284", facilityTitle);
	}

	public string GetWarningDescription()
	{
		return Localizer.Format("#autoLOC_253289", facilityTitle);
	}

	public string GetProceedOption()
	{
		return null;
	}

	public string GetAbortOption()
	{
		return Localizer.Format("#autoLOC_253299");
	}
}
