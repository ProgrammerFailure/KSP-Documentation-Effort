using System;
using System.Globalization;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;

namespace SentinelMission;

public class SentinelParameter : ContractParameter
{
	public CelestialBody FocusBody;

	public int TotalDiscoveries;

	public int RemainingDiscoveries;

	public SentinelScanType ScanType;

	public UntrackedObjectClass TargetSize;

	public double MinimumEccentricity;

	public double MinimumInclination;

	public SentinelParameter()
	{
		FocusBody = Planetarium.fetch.Home;
		ScanType = SentinelScanType.NONE;
		if (ScenarioDiscoverableObjects.Instance != null)
		{
			TargetSize = ScenarioDiscoverableObjects.Instance.maxAsteroidClass;
		}
		else
		{
			TargetSize = (UntrackedObjectClass)(Enum.GetNames(typeof(UntrackedObjectClass)).Length - 1);
		}
		MinimumEccentricity = 0.0;
		MinimumInclination = 0.0;
		TotalDiscoveries = 3;
		RemainingDiscoveries = 3;
	}

	public SentinelParameter(CelestialBody focusBody, SentinelScanType scanType, UntrackedObjectClass targetSize, double minimumEccentricity, double minimumInclination, int discoveryCount)
	{
		FocusBody = focusBody;
		ScanType = scanType;
		TargetSize = targetSize;
		MinimumEccentricity = minimumEccentricity;
		MinimumInclination = minimumInclination;
		TotalDiscoveries = discoveryCount;
		RemainingDiscoveries = discoveryCount;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		string text = ((FocusBody == Planetarium.fetch.Home) ? Localizer.Format("#autoLOC_6002303") : Localizer.Format("#autoLOC_6002304"));
		return ScanType switch
		{
			SentinelScanType.CLASS => Localizer.Format("#autoLOC_6002305", TotalDiscoveries, TargetSize, text, FocusBody.displayName), 
			SentinelScanType.ECCENTRICITY => Localizer.Format("#autoLOC_6002306", TotalDiscoveries, text, FocusBody.displayName, Math.Round(MinimumEccentricity, 2)), 
			SentinelScanType.INCLINATION => Localizer.Format("#autoLOC_6002307", TotalDiscoveries, text, FocusBody.displayName, Math.Round(MinimumInclination)), 
			_ => Localizer.Format("#autoLOC_6002308", TotalDiscoveries, text, FocusBody.displayName), 
		};
	}

	public override string GetNotes()
	{
		return ((HighLogic.LoadedScene == GameScenes.SPACECENTER) ? "\n" : "") + Localizer.Format("#autoLOC_6002309");
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = true;
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("FocusBody", FocusBody.flightGlobalsIndex);
		node.AddValue("ScanType", ScanType);
		node.AddValue("TargetSize", TargetSize);
		node.AddValue("MinimumEccentricity", MinimumEccentricity);
		node.AddValue("MinimumInclination", MinimumInclination);
		node.AddValue("TotalDiscoveries", TotalDiscoveries);
		node.AddValue("RemainingDiscoveries", RemainingDiscoveries);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "SentinelParameter", "FocusBody", ref FocusBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "SentinelParameter", "ScanType", ref ScanType, SentinelScanType.NONE);
		SystemUtilities.LoadNode(node, "SentinelParameter", "TargetSize", ref TargetSize, (UntrackedObjectClass)(Enum.GetNames(typeof(UntrackedObjectClass)).Length - 1));
		SystemUtilities.LoadNode(node, "SentinelParameter", "MinimumEccentricity", ref MinimumEccentricity, 0.0);
		SystemUtilities.LoadNode(node, "SentinelParameter", "MinimumInclination", ref MinimumInclination, 0.0);
		SystemUtilities.LoadNode(node, "SentinelParameter", "TotalDiscoveries", ref TotalDiscoveries, 3);
		SystemUtilities.LoadNode(node, "SentinelParameter", "RemainingDiscoveries", ref RemainingDiscoveries, 3);
	}

	public void DiscoverAsteroid(UntrackedObjectClass size, double eccentricity, double inclination, CelestialBody body)
	{
		if (base.Root.ContractState != Contract.State.Active || body != FocusBody || (ScanType == SentinelScanType.CLASS && size != TargetSize) || (ScanType == SentinelScanType.ECCENTRICITY && eccentricity < MinimumEccentricity) || (ScanType == SentinelScanType.INCLINATION && inclination < MinimumInclination))
		{
			return;
		}
		SpecificOrbitParameter parameter = base.Root.GetParameter<SpecificOrbitParameter>();
		if (parameter != null && parameter.State != ParameterState.Complete && parameter.Orbit != null)
		{
			Orbit orbit = parameter.Orbit;
			if (!SentinelScenario.Instance.ActiveSentinelMatchingOrbit(orbit, parameter.deviationWindow))
			{
				return;
			}
		}
		RemainingDiscoveries--;
		if (RemainingDiscoveries > 0)
		{
			string text = Localizer.Format("#autoLOC_6002310", TotalDiscoveries - RemainingDiscoveries, TotalDiscoveries, body.displayName, base.Root.Agent.Title);
			ScreenMessages.PostScreenMessage(text, SentinelUtilities.CalculateReadDuration(text), ScreenMessageStyle.UPPER_CENTER);
		}
		else
		{
			string text2 = Localizer.Format("#autoLOC_6002311", FocusBody.displayName, base.Root.Agent.Title);
			ScreenMessages.PostScreenMessage(text2, SentinelUtilities.CalculateReadDuration(text2), ScreenMessageStyle.UPPER_CENTER);
			SetComplete();
			base.Root.Complete();
		}
	}
}
