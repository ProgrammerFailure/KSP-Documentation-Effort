using ns9;

public class ModuleBiomeScanner : PartModule
{
	[KSPEvent(unfocusedRange = 3f, active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001474")]
	public virtual void RunAnalysis()
	{
		if (base.vessel.CurrentControlLevel <= Vessel.ControlLevel.NONE)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_258793"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return;
		}
		CelestialBody mainBody = base.vessel.mainBody;
		double lat = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLat(base.vessel.latitude));
		double lon = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLon(base.vessel.longitude));
		CBAttributeMapSO.MapAttribute biome = ResourceUtilities.GetBiome(lat, lon, FlightGlobals.currentMainBody);
		string situationString = base.vessel.SituationString;
		string text = situationString;
		if (biome != null)
		{
			situationString = biome.name;
			text = biome.displayname;
		}
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001050", mainBody.displayName, text), 5f, ScreenMessageStyle.UPPER_CENTER);
		ResourceMap.Instance.UnlockBiome(mainBody.flightGlobalsIndex, situationString);
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || base.vessel == null)
		{
			return;
		}
		bool flag;
		if (flag = base.vessel.LandedOrSplashed)
		{
			CelestialBody mainBody = base.vessel.mainBody;
			double lat = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLat(base.vessel.latitude));
			double lon = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLon(base.vessel.longitude));
			CBAttributeMapSO.MapAttribute biome = ResourceUtilities.GetBiome(lat, lon, FlightGlobals.currentMainBody);
			string situationString = base.vessel.SituationString;
			if (biome != null)
			{
				situationString = biome.name;
			}
			if (ResourceMap.Instance.IsBiomeUnlocked(mainBody.flightGlobalsIndex, situationString))
			{
				flag = false;
			}
		}
		if (base.Events["RunAnalysis"].active != flag)
		{
			base.Events["RunAnalysis"].active = flag;
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
	}
}
