using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace FinePrint.Contracts.Parameters;

public class SurveyWaypointParameter : ContractParameter
{
	public Waypoint wp;

	public WaypointClusterState clusterState;

	public string experiment;

	public string actionDescription;

	public double thresholdAltitude;

	public FlightBand band;

	public CelestialBody targetBody;

	public bool submittedWaypoint;

	public bool outerWarning;

	public bool verified;

	public bool contextual;

	public int bouncesLeft;

	public double bounceOriginLatitude;

	public double bounceOriginLongitude;

	public bool eventsAdded;

	public string contextualAnomalyName;

	public string ContextualAnomalyName
	{
		get
		{
			if (!string.IsNullOrEmpty(contextualAnomalyName))
			{
				return contextualAnomalyName;
			}
			if (!(base.Root is SurveyContract surveyContract))
			{
				return Localizer.Format("#autoLOC_285665");
			}
			contextualAnomalyName = Localizer.Format("#autoLOC_8004457", surveyContract.dataName, surveyContract.anomalyName);
			return contextualAnomalyName;
		}
	}

	public SurveyWaypointParameter()
	{
		wp = new Waypoint();
		targetBody = Planetarium.fetch.Home;
		band = FlightBand.NONE;
		submittedWaypoint = false;
		outerWarning = false;
		verified = false;
		actionDescription = "Perform a survey";
		experiment = "crewReport";
		wp.id = "default";
		clusterState = WaypointClusterState.MIXED;
		thresholdAltitude = 10000.0;
	}

	public SurveyWaypointParameter(string experiment, string actionDescription, CelestialBody targetBody, Waypoint wp, FlightBand band)
	{
		this.wp = wp;
		this.targetBody = targetBody;
		this.band = band;
		submittedWaypoint = false;
		outerWarning = false;
		verified = false;
		clusterState = WaypointClusterState.MIXED;
		this.experiment = experiment;
		this.actionDescription = actionDescription;
		contextual = false;
		bouncesLeft = int.MaxValue;
		bounceOriginLatitude = 0.0;
		bounceOriginLongitude = 0.0;
		ProcessWaypoint();
	}

	public SurveyWaypointParameter(string experiment, string actionDescription, CelestialBody targetBody, Waypoint wp, FlightBand band, bool contextual, int bouncesLeft, double bounceOriginLatitude, double bounceOriginLongitude)
	{
		this.wp = wp;
		this.targetBody = targetBody;
		this.band = band;
		submittedWaypoint = false;
		outerWarning = false;
		verified = false;
		clusterState = WaypointClusterState.MIXED;
		this.experiment = experiment;
		this.actionDescription = actionDescription;
		this.contextual = contextual;
		this.bouncesLeft = bouncesLeft;
		this.bounceOriginLatitude = bounceOriginLatitude;
		this.bounceOriginLongitude = bounceOriginLongitude;
		ProcessWaypoint();
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		bool value;
		if ((value = CelestialUtilities.IsFlyablePlanet(targetBody)) && band == FlightBand.HIGH)
		{
			ScienceExperiment scienceExperiment = ResearchAndDevelopment.GetExperiment(experiment);
			if (scienceExperiment != null && !scienceExperiment.IsAvailableWhile(ExperimentSituations.FlyingHigh, targetBody))
			{
				value = false;
			}
		}
		return band switch
		{
			FlightBand.GROUND => Localizer.Format("#autoLOC_6001085", actionDescription, SiteString()), 
			FlightBand.const_2 => Localizer.Format("#autoLOC_6001086", actionDescription, Convert.ToInt32(value), Convert.ToDecimal(Math.Round(thresholdAltitude)).ToString("#,###"), SiteString()), 
			FlightBand.HIGH => Localizer.Format("#autoLOC_6001087", actionDescription, Convert.ToInt32(value), Convert.ToDecimal(Math.Round(thresholdAltitude)).ToString("#,###"), SiteString()), 
			_ => Localizer.Format("#autoLOC_6001088", actionDescription, SiteString()), 
		};
	}

	public override string GetMessageComplete()
	{
		if (clusterState == WaypointClusterState.FULL)
		{
			return Localizer.Format("#autoLOC_285754", StringUtilities.GenerateSiteName(wp.seed, targetBody, wp.landLocked, !contextual), StringUtilities.IntegerToGreek(wp.index), targetBody.displayName);
		}
		if (clusterState == WaypointClusterState.MIXED && band == FlightBand.GROUND)
		{
			return Localizer.Format("#autoLOC_285757", StringUtilities.GenerateSiteName(wp.seed, targetBody, wp.landLocked, !contextual), StringUtilities.IntegerToGreek(wp.index), targetBody.displayName);
		}
		return Localizer.Format("#autoLOC_285759", StringUtilities.GenerateSiteName(wp.uniqueSeed, targetBody, wp.landLocked, !contextual), targetBody.displayName);
	}

	public override void OnRegister()
	{
		if (base.Root.ContractState == Contract.State.Active && !eventsAdded)
		{
			GameEvents.OnExperimentDeployed.Add(CheckExperimentResults);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.OnExperimentDeployed.Remove(CheckExperimentResults);
		}
		if (submittedWaypoint)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				NavWaypoint.DeactivateIfWaypoint(wp);
			}
			WaypointManager.RemoveWaypoint(wp);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("actionDescription", actionDescription);
		node.AddValue("experiment", experiment);
		node.AddValue("flightThreshold", thresholdAltitude);
		node.AddValue("band", (int)band);
		node.AddValue("clusterState", clusterState);
		node.AddValue("verified", verified);
		node.AddValue("contextual", contextual);
		if (contextual)
		{
			node.AddValue("bouncesLeft", bouncesLeft);
			node.AddValue("bounceOriginLatitude", bounceOriginLatitude);
			node.AddValue("bounceOriginLongitude", bounceOriginLongitude);
		}
		node.AddValue("wpSeed", wp.seed);
		node.AddValue("wpIndex", wp.index);
		node.AddValue("wpTexture", wp.id);
		node.AddValue("wpLatitude", wp.latitude);
		node.AddValue("wpLongitude", wp.longitude);
		node.AddValue("wpLandlocked", wp.landLocked);
		node.AddValue("wpNavigationId", wp.navigationId);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "actionDescription", ref actionDescription, "Perform a survey");
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "experiment", ref experiment, "crewReport");
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "flightThreshold", ref thresholdAltitude, 10000.0);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "band", ref band, FlightBand.NONE);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "clusterState", ref clusterState, WaypointClusterState.MIXED);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "verified", ref verified, defaultValue: false);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "contextual", ref contextual, defaultValue: false);
		if (contextual)
		{
			SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "bouncesLeft", ref bouncesLeft, 0);
			SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "bounceOriginLatitude", ref bounceOriginLatitude, 0.0);
			SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "bounceOriginLongitude", ref bounceOriginLongitude, 0.0);
		}
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "wpSeed", ref wp.seed, 1337);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "wpIndex", ref wp.index, 0);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "wpTexture", ref wp.id, "default");
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "wpLatitude", ref wp.latitude, 0.0);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "wpLongitude", ref wp.longitude, 0.0);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "wpLandlocked", ref wp.landLocked, defaultValue: false);
		SystemUtilities.LoadNode(node, "SurveyWaypointParameter", "wpNavigationId", ref wp.navigationId, Guid.NewGuid());
		if (wp.navigationId == Guid.Empty)
		{
			Debug.LogWarningFormat("Stored navigationId was empty for {0} - Generating new id", wp.name);
			wp.navigationId = Guid.NewGuid();
		}
		ProcessWaypoint();
	}

	public override void OnUpdate()
	{
		if (!SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active, checkVessel: true))
		{
			return;
		}
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!submittedWaypoint || !(activeVessel.mainBody == targetBody) || !(WaypointManager.Instance() != null))
		{
			return;
		}
		float num = WaypointManager.Instance().LateralDistanceToVessel(wp);
		double num2 = TriggerRange(band);
		if ((double)num > num2 && outerWarning)
		{
			string text = wp.name;
			if (wp.isClustered)
			{
				text = text + " " + StringUtilities.IntegerToGreek(wp.index);
			}
			outerWarning = false;
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_285861", text), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
		if ((double)num <= num2 && !outerWarning)
		{
			string text2 = wp.name;
			if (wp.isClustered)
			{
				text2 = text2 + " " + StringUtilities.IntegerToGreek(wp.index);
			}
			outerWarning = true;
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_285872", text2), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
		NavWaypoint fetch = NavWaypoint.fetch;
		if (!(fetch != null) || !fetch.IsActive || !fetch.IsUsing(wp))
		{
			return;
		}
		if ((double)num <= num2)
		{
			if (!fetch.IsBlinking)
			{
				fetch.IsBlinking = true;
			}
		}
		else if (fetch.IsBlinking)
		{
			fetch.IsBlinking = false;
		}
	}

	public void CheckExperimentResults(ScienceData experimentData)
	{
		if (base.Root.ContractState != Contract.State.Active || !SystemUtilities.FlightIsReady(checkVessel: true, targetBody) || !submittedWaypoint || WaypointManager.Instance() == null || !experimentData.subjectID.StartsWith(experiment + "@") || VesselUtilities.GetFlightBand(thresholdAltitude) != band)
		{
			return;
		}
		float num = WaypointManager.Instance().LateralDistanceToVessel(wp);
		double num2 = TriggerRange(band);
		if (!((double)num < num2))
		{
			return;
		}
		if (contextual && bouncesLeft > 0)
		{
			Bounce();
			return;
		}
		string text = wp.name;
		if (wp.isClustered)
		{
			text = text + " " + StringUtilities.IntegerToGreek(wp.index);
		}
		if (!contextual)
		{
			UpdateNavWaypoint(NextWaypointInCluster());
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_285927", text), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
		else
		{
			NavWaypoint.DeactivateIfWaypoint(wp);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_285932", ContextualAnomalyName), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
		wp.isExplored = true;
		WaypointManager.RemoveWaypoint(wp);
		submittedWaypoint = false;
		SetComplete();
	}

	public void Bounce()
	{
		if (submittedWaypoint)
		{
			string text = wp.name;
			if (wp.isClustered)
			{
				text = text + " " + StringUtilities.IntegerToGreek(wp.index);
			}
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_285955", text, ContextualAnomalyName), 5f, ScreenMessageStyle.UPPER_LEFT);
			double num = Math.Max(SurveyContract.ParamRange(base.Root.Prestige, targetBody, band), ContractDefs.Survey.MinimumTriggerRange);
			Waypoint waypoint = new Waypoint(wp);
			waypoint.index++;
			waypoint.RandomizeAwayFrom(bounceOriginLatitude, bounceOriginLongitude, num, num * 2.0, 3, !waypoint.landLocked);
			wp.isExplored = true;
			WaypointManager.RemoveWaypoint(wp);
			submittedWaypoint = false;
			if (base.Root.AddParameter(new SurveyWaypointParameter(experiment, actionDescription, targetBody, waypoint, band, contextual, bouncesLeft - 1, bounceOriginLatitude, bounceOriginLongitude)) is SurveyWaypointParameter surveyWaypointParameter)
			{
				UpdateNavWaypoint(surveyWaypointParameter.wp);
				surveyWaypointParameter.clusterState = clusterState;
				surveyWaypointParameter.Register();
				surveyWaypointParameter.FundsCompletion = FundsCompletion;
				surveyWaypointParameter.ScienceCompletion = ScienceCompletion;
				surveyWaypointParameter.ReputationCompletion = ReputationCompletion;
				surveyWaypointParameter.ProcessWaypoint();
				SetComplete();
			}
		}
	}

	public static double TriggerRange(FlightBand band)
	{
		double minimumTriggerRange = ContractDefs.Survey.MinimumTriggerRange;
		double num = ContractDefs.Survey.MaximumTriggerRange;
		if (minimumTriggerRange == num)
		{
			num += 1.0;
		}
		double num2 = Math.Min(minimumTriggerRange, num);
		double num3 = Math.Max(minimumTriggerRange, num);
		return band switch
		{
			FlightBand.GROUND => num2, 
			FlightBand.const_2 => (num2 + num3) / 2.0, 
			FlightBand.HIGH => num3, 
			_ => 0.0, 
		};
	}

	public void ProcessWaypoint()
	{
		wp.celestialName = targetBody.GetName();
		wp.isOnSurface = true;
		wp.isNavigatable = true;
		wp.contractReference = base.Root;
		wp.SetFadeRange();
		if (band == FlightBand.GROUND && clusterState != WaypointClusterState.SINGLE)
		{
			wp.isClustered = true;
			wp.setName(uniqueSites: false, !contextual);
		}
		else
		{
			wp.setName(uniqueSites: true, !contextual);
		}
		double terrainAltitude = CelestialUtilities.TerrainAltitude(targetBody, wp.latitude, wp.longitude);
		CalculateAltitudes(terrainAltitude);
		if (!verified)
		{
			VerifyWaypoint(terrainAltitude);
		}
		wp.nodeCaption2 = Localizer.Format("#autoLOC_286029");
		switch (band)
		{
		default:
			wp.nodeCaption2 += KSPUtil.PrintSI(thresholdAltitude, Localizer.Format("#autoLOC_7001411"));
			break;
		case FlightBand.GROUND:
			wp.nodeCaption2 += Localizer.Format("#autoLOC_286034");
			break;
		case FlightBand.const_2:
			wp.nodeCaption2 += Localizer.Format("#autoLOC_7003013", KSPUtil.PrintSI(thresholdAltitude, Localizer.Format("#autoLOC_7001411")));
			break;
		case FlightBand.HIGH:
			wp.nodeCaption2 += Localizer.Format("#autoLOC_7003012", KSPUtil.PrintSI(thresholdAltitude, Localizer.Format("#autoLOC_7001411")));
			break;
		}
		if (!submittedWaypoint && base.Root != null)
		{
			if (HighLogic.LoadedSceneIsFlight && base.Root.ContractState == Contract.State.Active && base.State == ParameterState.Incomplete)
			{
				WaypointManager.AddWaypoint(wp);
				submittedWaypoint = true;
			}
			if (HighLogic.LoadedScene == GameScenes.TRACKSTATION && base.State == ParameterState.Incomplete && (base.Root.ContractState == Contract.State.Active || (!base.Root.IsFinished() && ContractDefs.DisplayOfferedWaypoints)))
			{
				WaypointManager.AddWaypoint(wp);
				submittedWaypoint = true;
			}
		}
	}

	public void CalculateAltitudes(double terrainAltitude)
	{
		if (targetBody == null || wp == null)
		{
			return;
		}
		KSPRandom kSPRandom = new KSPRandom(wp.uniqueSeed);
		double num = Math.Max(Math.Min(ContractDefs.Survey.ThresholdDeviancy, 100f), 0f);
		if (num < 1.0)
		{
			num *= 100.0;
		}
		num = (double)kSPRandom.Next((int)num) / 100.0;
		thresholdAltitude = ContractDefs.Survey.MinimumThreshold + targetBody.GeeASL * (ContractDefs.Survey.MaximumThreshold - ContractDefs.Survey.MinimumThreshold);
		if (targetBody.atmosphere)
		{
			if ((double)targetBody.scienceValues.flyingAltitudeThreshold > terrainAltitude)
			{
				thresholdAltitude = (double)targetBody.scienceValues.flyingAltitudeThreshold - terrainAltitude;
			}
			if (band == FlightBand.HIGH)
			{
				ScienceExperiment scienceExperiment = ResearchAndDevelopment.GetExperiment(experiment);
				if (scienceExperiment != null && !scienceExperiment.IsAvailableWhile(ExperimentSituations.FlyingHigh, targetBody))
				{
					thresholdAltitude = targetBody.atmosphereDepth;
				}
			}
		}
		switch (band)
		{
		default:
			wp.altitude = 0.0;
			break;
		case FlightBand.GROUND:
			wp.altitude = 0.0;
			break;
		case FlightBand.const_2:
			wp.altitude = thresholdAltitude - thresholdAltitude / 3.0;
			break;
		case FlightBand.HIGH:
			wp.altitude = thresholdAltitude + thresholdAltitude / 3.0;
			break;
		}
		num = thresholdAltitude * num;
		if (SystemUtilities.CoinFlip(kSPRandom))
		{
			thresholdAltitude -= num;
		}
		else
		{
			thresholdAltitude += num;
		}
		thresholdAltitude += terrainAltitude;
		thresholdAltitude = Math.Round(thresholdAltitude / 100.0) * 100.0;
	}

	public void VerifyWaypoint(double terrainAltitude)
	{
		KSPRandom generator = new KSPRandom(wp.uniqueSeed);
		int num = 0;
		do
		{
			if (!ProgressUtilities.ExperimentPossibleAt(experiment, targetBody, wp.latitude, wp.longitude, wp.altitude + terrainAltitude, terrainAltitude))
			{
				WaypointManager.ChooseRandomPosition(out wp.latitude, out wp.longitude, targetBody.name, waterAllowed: true, equatorial: false, generator);
				terrainAltitude = CelestialUtilities.TerrainAltitude(targetBody, wp.latitude, wp.longitude);
				CalculateAltitudes(terrainAltitude);
				num++;
				continue;
			}
			verified = true;
			return;
		}
		while (num <= 25);
		Debug.Log("Contract Log: Survey unable to place " + experiment + " waypoint - the survey definition is misconfigured.");
	}

	public string SiteString()
	{
		string text = "";
		text = ((clusterState == WaypointClusterState.FULL) ? (text + Localizer.Format("#autoLOC_285739", StringUtilities.IntegerToGreek(wp.index))) : ((clusterState != WaypointClusterState.MIXED || band != FlightBand.GROUND) ? (text + StringUtilities.GenerateSiteName(wp.uniqueSeed, targetBody, wp.landLocked, !contextual)) : (text + StringUtilities.GenerateSiteName(wp.seed, targetBody, wp.landLocked, !contextual) + " " + StringUtilities.IntegerToGreek(wp.index))));
		if (contextual)
		{
			text += Localizer.Format("#autoLOC_285746", ContextualAnomalyName);
		}
		return text;
	}

	public Waypoint NextWaypointInCluster()
	{
		Waypoint result = null;
		double num = double.MaxValue;
		IEnumerator<ContractParameter> enumerator = base.Root.AllParameters.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is SurveyWaypointParameter { State: ParameterState.Incomplete } surveyWaypointParameter && surveyWaypointParameter != this)
			{
				double num2 = CelestialUtilities.GreatCircleDistance(targetBody, wp.latitude, wp.longitude, surveyWaypointParameter.wp.latitude, surveyWaypointParameter.wp.longitude);
				if (num2 < num)
				{
					num = num2;
					result = surveyWaypointParameter.wp;
				}
			}
		}
		return result;
	}

	public void UpdateNavWaypoint(Waypoint newWaypoint)
	{
		NavWaypoint fetch = NavWaypoint.fetch;
		if (!(fetch == null) && fetch.IsActive && fetch.IsUsing(wp))
		{
			if (newWaypoint == null)
			{
				fetch.Deactivate();
				return;
			}
			string text = (newWaypoint.isClustered ? (newWaypoint.name + " " + StringUtilities.IntegerToGreek(newWaypoint.index)) : newWaypoint.name);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_286222", text), 2.5f, ScreenMessageStyle.UPPER_CENTER);
			fetch.Setup(newWaypoint);
		}
	}
}
