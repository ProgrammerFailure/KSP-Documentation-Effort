using System;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class StationaryPointParameter : ContractParameter
{
	public Waypoint wp;

	public CelestialBody targetBody;

	public bool submittedWaypoint;

	public double longitude;

	public double deviation;

	public int successCounter;

	public StationaryPointParameter()
	{
		targetBody = Planetarium.fetch.Home;
		longitude = 0.0;
		wp = new Waypoint();
		submittedWaypoint = false;
		successCounter = 0;
		deviation = ContractDefs.Satellite.TrivialDeviation;
	}

	public StationaryPointParameter(CelestialBody targetBody, double longitude, double deviation)
	{
		this.targetBody = targetBody;
		wp = new Waypoint();
		submittedWaypoint = false;
		successCounter = 0;
		this.longitude = longitude;
		this.deviation = deviation;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_285514", StringUtilities.GenerateSiteName(SystemUtilities.SuperSeed(base.Root), targetBody, landLocked: true));
	}

	public override string GetMessageComplete()
	{
		return Localizer.Format("#autoLOC_285519", StringUtilities.GenerateSiteName(SystemUtilities.SuperSeed(base.Root), targetBody, landLocked: true));
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
	}

	public override void OnUnregister()
	{
		if (submittedWaypoint)
		{
			WaypointManager.RemoveWaypoint(wp);
		}
	}

	public override void OnReset()
	{
		SetIncomplete();
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("longitude", longitude);
		node.AddValue("deviation", deviation);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "StationaryPointParameter", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "StationaryPointParameter", "longitude", ref longitude, 0.0);
		SystemUtilities.LoadNode(node, "StationaryPointParameter", "deviation", ref deviation, ContractDefs.Satellite.TrivialDeviation);
		if (HighLogic.LoadedSceneIsFlight && base.Root.ContractState == Contract.State.Active)
		{
			wp.celestialName = targetBody.GetName();
			wp.latitude = 0.0;
			wp.longitude = longitude;
			wp.seed = SystemUtilities.SuperSeed(base.Root);
			wp.index = 0;
			wp.landLocked = true;
			wp.setName(uniqueSites: false);
			wp.id = "dish";
			wp.altitude = 0.0;
			wp.isOnSurface = true;
			wp.isNavigatable = true;
			wp.contractReference = base.Root;
			wp.SetFadeRange();
			WaypointManager.AddWaypoint(wp);
			submittedWaypoint = true;
		}
		if (HighLogic.LoadedScene == GameScenes.TRACKSTATION && base.State == ParameterState.Incomplete && (base.Root.ContractState == Contract.State.Active || (!base.Root.IsFinished() && ContractDefs.DisplayOfferedOrbits)))
		{
			wp.celestialName = targetBody.GetName();
			wp.latitude = 0.0;
			wp.longitude = longitude;
			wp.seed = SystemUtilities.SuperSeed(base.Root);
			wp.index = 0;
			wp.landLocked = true;
			wp.setName(uniqueSites: false);
			wp.id = "dish";
			wp.altitude = 0.0;
			wp.isOnSurface = true;
			wp.isNavigatable = true;
			wp.contractReference = base.Root;
			wp.SetFadeRange();
			WaypointManager.AddWaypoint(wp);
			submittedWaypoint = true;
		}
	}

	public override void OnUpdate()
	{
		if (!SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active, checkVessel: true))
		{
			return;
		}
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!submittedWaypoint || activeVessel.mainBody != targetBody)
		{
			return;
		}
		bool flag = 180.0 - Math.Abs(Math.Abs(activeVessel.longitude - longitude) - 180.0) <= deviation / 100.0 * 360.0;
		if (base.State == ParameterState.Incomplete)
		{
			if (flag)
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
		if (base.State == ParameterState.Complete && !flag)
		{
			SetIncomplete();
		}
	}
}
