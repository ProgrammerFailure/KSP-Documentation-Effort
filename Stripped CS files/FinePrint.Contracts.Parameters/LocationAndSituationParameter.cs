using System;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class LocationAndSituationParameter : ContractParameter
{
	public CelestialBody targetBody;

	public Vessel.Situations targetSituation;

	public string noun;

	public bool finalObjective;

	public bool validVessel;

	public bool dirtyVessel = true;

	public int successCounter;

	public bool eventsAdded;

	public LocationAndSituationParameter()
	{
		targetSituation = Vessel.Situations.ESCAPING;
		targetBody = null;
		noun = "potato";
		finalObjective = false;
		successCounter = 0;
	}

	public LocationAndSituationParameter(CelestialBody targetBody, Vessel.Situations targetSituation, string noun, bool finalObjective = false)
	{
		this.targetBody = targetBody;
		this.targetSituation = targetSituation;
		this.noun = noun;
		this.finalObjective = finalObjective;
		successCounter = 0;
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.onVesselSOIChanged.Add(ChangeBody);
			GameEvents.onVesselSituationChange.Add(ChangeSituation);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.onVesselSOIChanged.Remove(ChangeBody);
			GameEvents.onVesselSituationChange.Remove(ChangeSituation);
		}
	}

	public override void OnReset()
	{
		SetIncomplete();
		dirtyVessel = true;
	}

	public void ChangeBody(GameEvents.HostedFromToAction<Vessel, CelestialBody> action)
	{
		if (action.host == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public void ChangeSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> action)
	{
		if (action.host == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		if (targetBody == null)
		{
			return Localizer.Format("#autoLOC_283645", Planetarium.fetch.Sun.displayName);
		}
		return targetSituation switch
		{
			Vessel.Situations.SUB_ORBITAL => Localizer.Format("#autoLOC_7000017", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
			Vessel.Situations.FLYING => Localizer.Format("#autoLOC_7000012", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
			Vessel.Situations.LANDED => Localizer.Format("#autoLOC_7000013", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
			Vessel.Situations.SPLASHED => Localizer.Format("#autoLOC_7000016", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
			Vessel.Situations.PRELAUNCH => Localizer.Format("#autoLOC_7000015", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
			Vessel.Situations.DOCKED => Localizer.Format("#autoLOC_7000010", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
			Vessel.Situations.ESCAPING => Localizer.Format("#autoLOC_7000011", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
			Vessel.Situations.ORBITING => Localizer.Format("#autoLOC_7000014", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
			_ => Localizer.Format("#autoLOC_7000018", Convert.ToInt32(!finalObjective), noun, targetBody.displayName), 
		};
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("targetSituation", targetSituation);
		node.AddValue("noun", noun);
		node.AddValue("finalObjective", finalObjective);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "LocationAndSituationParameter", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "LocationAndSituationParameter", "targetSituation", ref targetSituation, Vessel.Situations.ORBITING);
		SystemUtilities.LoadNode(node, "LocationAndSituationParameter", "noun", ref noun, "potato");
		SystemUtilities.LoadNode(node, "LocationAndSituationParameter", "finalObjective", ref finalObjective, defaultValue: false);
	}

	public override void OnUpdate()
	{
		if (!SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active, checkVessel: true))
		{
			return;
		}
		if (dirtyVessel)
		{
			dirtyVessel = false;
			validVessel = FlightGlobals.ActiveVessel.situation == targetSituation && FlightGlobals.ActiveVessel.mainBody == targetBody && FlightGlobals.ActiveVessel.vesselType > VesselType.Unknown && FlightGlobals.ActiveVessel.vesselType != VesselType.const_11 && FlightGlobals.ActiveVessel.vesselType != VesselType.Flag && FlightGlobals.ActiveVessel.vesselType != VesselType.DeployedSciencePart && FlightGlobals.ActiveVessel.vesselType != VesselType.DeployedScienceController;
		}
		if (base.State == ParameterState.Incomplete)
		{
			if (validVessel)
			{
				successCounter++;
			}
			else
			{
				successCounter = 0;
			}
			if (successCounter >= 5)
			{
				SetComplete();
			}
		}
		if (base.State == ParameterState.Complete && !validVessel)
		{
			SetIncomplete();
		}
	}
}
