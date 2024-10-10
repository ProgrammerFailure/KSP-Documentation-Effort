using ns9;

namespace SentinelMission;

public class SentinelModule : PartModule
{
	[KSPField(isPersistant = true)]
	public bool isTracking;

	[KSPField(guiFormat = "F3", guiActive = true, guiName = "#autoLOC_6002288", guiUnits = "")]
	public string status = "#autoLOC_6002289";

	public static string cacheAutoLOC_6002290;

	public static string cacheAutoLOC_6002296;

	public string FocusName
	{
		get
		{
			SentinelUtilities.FindInnerAndOuterBodies(base.vessel, out var _, out var outerBody);
			return Localizer.Format("#autoLOC_7001301", outerBody.displayName);
		}
	}

	public override void OnStart(StartState state)
	{
		if (isTracking)
		{
			base.Events["StartTracking"].active = false;
			base.Events["StopTracking"].active = true;
		}
		else
		{
			base.Events["StartTracking"].active = true;
			base.Events["StopTracking"].active = false;
		}
	}

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6002281")]
	public void StartTracking()
	{
		if (TelescopeCanActivate())
		{
			ShowMessage(Localizer.Format("#autoLOC_6002285", SentinelUtilities.SentinelPartTitle, FocusName));
			isTracking = true;
			base.Events["StartTracking"].active = false;
			base.Events["StopTracking"].active = true;
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
	}

	[KSPEvent(unfocusedRange = 3f, active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6002286")]
	public void StopTracking()
	{
		ShowMessage(Localizer.Format("#autoLOC_6002287", SentinelUtilities.SentinelPartTitle, FocusName));
		isTracking = false;
		base.Events["StartTracking"].active = true;
		base.Events["StopTracking"].active = false;
		MonoUtilities.RefreshPartContextWindow(base.part);
	}

	public void FixedUpdate()
	{
		if (isTracking)
		{
			if (SentinelUtilities.FindInnerAndOuterBodies(base.vessel, out var innerBody, out var outerBody))
			{
				status = (SentinelUtilities.SentinelCanScan(base.vessel, innerBody, outerBody) ? Localizer.Format("#autoLOC_6002291", outerBody.displayName) : Localizer.Format("#autoLOC_6002292", outerBody.displayName));
			}
			else
			{
				status = cacheAutoLOC_6002290;
			}
		}
		else
		{
			status = cacheAutoLOC_6002296;
		}
	}

	public bool TelescopeCanActivate()
	{
		bool flag = base.vessel.HasValidContractObjectives("Antenna");
		if (!base.vessel.HasValidContractObjectives("Generator"))
		{
			ShowMessage(Localizer.Format("#autoLOC_6002293", SentinelUtilities.SentinelPartTitle));
			return false;
		}
		if (!flag)
		{
			ShowMessage(Localizer.Format("#autoLOC_6002294", SentinelUtilities.SentinelPartTitle));
			return false;
		}
		if (base.vessel.orbit.referenceBody != Planetarium.fetch.Sun)
		{
			ShowMessage(Localizer.Format("#autoLOC_6002295", SentinelUtilities.SentinelPartTitle));
			return false;
		}
		return true;
	}

	public void ShowMessage(string s)
	{
		ScreenMessages.PostScreenMessage(s, SentinelUtilities.CalculateReadDuration(s), ScreenMessageStyle.UPPER_CENTER);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6002290 = Localizer.Format("#autoLOC_6002290");
		cacheAutoLOC_6002296 = Localizer.Format("#autoLOC_6002296");
	}
}
