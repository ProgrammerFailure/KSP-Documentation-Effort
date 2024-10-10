using ns9;
using PreFlightTests;

namespace Expansions.Missions;

public class MissionPreflightCheck : IPreFlightTest
{
	public VesselRestriction vesselRestriction;

	public MissionPreflightCheck(VesselRestriction vesselRestriction)
	{
		this.vesselRestriction = vesselRestriction;
	}

	public bool Test()
	{
		return vesselRestriction.IsComplete();
	}

	public string GetWarningTitle()
	{
		return Localizer.Format("#autoLOC_8100208");
	}

	public string GetWarningDescription()
	{
		return Localizer.Format("#autoLOC_8100209");
	}

	public string GetProceedOption()
	{
		return null;
	}

	public string GetAbortOption()
	{
		return Localizer.Format("#autoLOC_8100210");
	}
}
