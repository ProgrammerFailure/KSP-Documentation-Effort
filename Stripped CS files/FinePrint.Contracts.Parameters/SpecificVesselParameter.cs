using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns11;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class SpecificVesselParameter : WaypointParameter
{
	public List<uint> partIDs;

	public Vessel targetVessel;

	public Guid targetVesselID;

	public int targetVesselPartCount;

	public string targetVesselName;

	public Vessel.Situations targetVesselSituation;

	public Waypoint wp;

	public bool tracked;

	public bool submittedWaypoint;

	public int successCounter;

	public SpecificVesselParameter()
	{
		partIDs = new List<uint>();
		targetVessel = null;
		targetVesselID = Guid.Empty;
		targetVesselPartCount = 1;
		targetVesselName = "Vessel";
		targetVesselSituation = Vessel.Situations.ORBITING;
		TargetBody = Planetarium.fetch.Home;
		wp = new Waypoint();
	}

	public SpecificVesselParameter(Vessel v)
	{
		partIDs = VesselUtilities.GetPartIDList(v);
		targetVessel = v;
		targetVesselID = (v.loaded ? v.id : v.protoVessel.vesselID);
		targetVesselPartCount = (v.loaded ? v.parts.Count : v.protoVessel.protoPartSnapshots.Count);
		targetVesselName = (targetVessel.loaded ? targetVessel.GetDisplayName() : targetVessel.protoVessel.GetDisplayName());
		targetVesselSituation = (targetVessel.loaded ? targetVessel.situation : targetVessel.protoVessel.situation);
		TargetBody = (v.loaded ? v.orbit.referenceBody : FlightGlobals.Bodies[v.protoVessel.orbitSnapShot.ReferenceBodyIndex]);
		wp = new Waypoint();
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_285073", targetVesselName);
	}

	public override string GetMessageComplete()
	{
		return Localizer.Format("#autoLOC_285078", targetVesselName);
	}

	public override string GetNotes()
	{
		if (!base.Root.IsFinished() && !(targetVessel == null))
		{
			return targetVesselSituation switch
			{
				Vessel.Situations.SUB_ORBITAL => Localizer.Format("#autoLOC_285099", targetVesselName, TargetBody.displayName), 
				Vessel.Situations.FLYING => Localizer.Format("#autoLOC_285091", targetVesselName, TargetBody.displayName), 
				Vessel.Situations.LANDED => Localizer.Format("#autoLOC_285093", targetVesselName, TargetBody.displayName), 
				Vessel.Situations.SPLASHED => Localizer.Format("#autoLOC_285097", targetVesselName, TargetBody.displayName), 
				Vessel.Situations.PRELAUNCH => Localizer.Format("#autoLOC_285089", targetVesselName, TargetBody.displayName), 
				Vessel.Situations.DOCKED => Localizer.Format("#autoLOC_285103", targetVesselName, TargetBody.displayName), 
				Vessel.Situations.ESCAPING => Localizer.Format("#autoLOC_285101", targetVesselName, TargetBody.displayName), 
				Vessel.Situations.ORBITING => Localizer.Format("#autoLOC_285095", targetVesselName, TargetBody.displayName), 
				_ => Localizer.Format("#autoLOC_285105", targetVesselName, TargetBody.displayName), 
			};
		}
		return null;
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
	}

	public override void OnUnregister()
	{
		CleanupWaypoints();
	}

	public override void OnReset()
	{
		SetIncomplete();
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("TargetBody", TargetBody.flightGlobalsIndex);
		node.AddValue("targetVesselID", targetVesselID.ToString("N"));
		node.AddValue("targetVesselPartCount", targetVesselPartCount);
		node.AddValue("targetVesselName", targetVesselName);
		node.AddValue("targetVesselSituation", targetVesselSituation);
		SystemUtilities.SaveNodeList(node, "partIDs", partIDs);
	}

	public override void OnLoad(ConfigNode node)
	{
		tracked = true;
		string value = "";
		SystemUtilities.LoadNode(node, "SpecificVesselParameter", "TargetBody", ref TargetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "SpecificVesselParameter", "targetVesselID", ref value, "");
		SystemUtilities.LoadNode(node, "SpecificVesselParameter", "targetVesselPartCount", ref targetVesselPartCount, 1);
		SystemUtilities.LoadNode(node, "SpecificVesselParameter", "targetVesselName", ref targetVesselName, "Vessel");
		SystemUtilities.LoadNode(node, "SpecificVesselParameter", "targetVesselSituation", ref targetVesselSituation, Vessel.Situations.ORBITING);
		SystemUtilities.LoadNodeList(node, "SpecificVesselParameter", "partIDs", ref partIDs);
		targetVesselID = new Guid(value);
		if (HighLogic.LoadedSceneHasPlanetarium && !base.Root.IsFinished())
		{
			TargetVesselSearch(cached: true);
		}
		if (tracked)
		{
			double fadeRange = double.MaxValue;
			if (targetVessel != null)
			{
				fadeRange = (targetVessel.loaded ? targetVessel.orbit : targetVessel.protoVessel.orbitSnapShot.Load()).ApR;
			}
			if (HighLogic.LoadedSceneIsFlight && base.Root.ContractState == Contract.State.Active)
			{
				wp.celestialName = TargetBody.GetName();
				wp.latitude = 0.0;
				wp.longitude = 0.0;
				wp.seed = SystemUtilities.SuperSeed(base.Root);
				wp.index = 0;
				wp.landLocked = false;
				wp.name = targetVesselName;
				wp.id = "vessel";
				wp.iconSize = 32;
				wp.altitude = 0.0;
				wp.isOnSurface = true;
				wp.isNavigatable = false;
				wp.enableTooltip = false;
				wp.enableMarker = false;
				wp.blocksInput = false;
				wp.contractReference = base.Root;
				wp.SetFadeRange(fadeRange);
				WaypointManager.AddWaypoint(wp);
				submittedWaypoint = true;
			}
			if (HighLogic.LoadedScene == GameScenes.TRACKSTATION && (base.Root.ContractState == Contract.State.Active || (!base.Root.IsFinished() && ContractDefs.DisplayOfferedWaypoints)))
			{
				wp.celestialName = TargetBody.GetName();
				wp.latitude = 0.0;
				wp.longitude = 0.0;
				wp.seed = SystemUtilities.SuperSeed(base.Root);
				wp.index = 0;
				wp.landLocked = false;
				wp.name = targetVesselName;
				wp.id = "vessel";
				wp.iconSize = 32;
				wp.altitude = 0.0;
				wp.isOnSurface = true;
				wp.isNavigatable = false;
				wp.enableTooltip = false;
				wp.enableMarker = false;
				wp.blocksInput = false;
				wp.contractReference = base.Root;
				wp.SetFadeRange(fadeRange);
				WaypointManager.AddWaypoint(wp);
				submittedWaypoint = true;
			}
		}
	}

	public override void OnUpdate()
	{
		if (!SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active, checkVessel: true) || !tracked)
		{
			return;
		}
		if (base.State == ParameterState.Incomplete)
		{
			if (FlightGlobals.ActiveVessel == targetVessel)
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
		if (base.State == ParameterState.Complete && FlightGlobals.ActiveVessel != targetVessel)
		{
			SetIncomplete();
		}
	}

	public override void UpdateWaypoints(bool focus)
	{
		if (!(targetVessel == null) && !base.Root.IsFinished() && tracked)
		{
			if (targetVesselPartCount != (targetVessel.loaded ? targetVessel.parts.Count : targetVessel.protoVessel.protoPartSnapshots.Count))
			{
				TargetVesselSearch(cached: false);
			}
			Vessel.Situations situations = (targetVessel.loaded ? targetVessel.situation : targetVessel.protoVessel.situation);
			TargetBody = (targetVessel.loaded ? targetVessel.orbit.referenceBody : FlightGlobals.Bodies[targetVessel.protoVessel.orbitSnapShot.ReferenceBodyIndex]);
			wp.celestialName = TargetBody.GetName();
			wp.name = (targetVessel.loaded ? targetVessel.GetDisplayName() : targetVessel.protoVessel.GetDisplayName());
			if ((uint)(situations - 1) > 1u && situations != Vessel.Situations.PRELAUNCH)
			{
				Orbit orbit = (targetVessel.loaded ? targetVessel.orbit : targetVessel.protoVessel.orbitSnapShot.Load());
				wp.orbitPosition = orbit.getPositionAtUT(Planetarium.GetUniversalTime());
				wp.isOnSurface = false;
			}
			else
			{
				wp.latitude = (targetVessel.loaded ? targetVessel.latitude : targetVessel.protoVessel.latitude);
				wp.longitude = (targetVessel.loaded ? targetVessel.longitude : targetVessel.protoVessel.longitude);
				wp.altitude = 0.0;
				wp.isOnSurface = true;
			}
		}
	}

	public void SpecificVesselRename(Guid id, string name)
	{
		if (!(targetVessel == null) && !base.Root.IsFinished() && tracked && ContractsApp.Instance != null && id == targetVesselID)
		{
			targetVesselName = name;
			ContractsApp.Instance.RefreshContractRequested(base.Root, changePositions: false);
		}
	}

	public void SpecificVesselScan(HashSet<Guid> ids)
	{
		if (!tracked)
		{
			return;
		}
		IEnumerator<Guid> enumerator = ids.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Guid current = enumerator.Current;
			if (!(current == Guid.Empty) && !(current != targetVesselID))
			{
				TargetVesselSearch(cached: false);
				if (base.Root.ContractState == Contract.State.Active && targetVesselID != current)
				{
					SetIncomplete();
				}
			}
		}
	}

	public void TargetVesselSearch(bool cached)
	{
		if (!tracked)
		{
			return;
		}
		targetVessel = null;
		if (targetVesselID == Guid.Empty)
		{
			cached = false;
		}
		if (cached)
		{
			int count = FlightGlobals.Vessels.Count;
			while (count-- > 0)
			{
				Vessel vessel = FlightGlobals.Vessels[count];
				if ((vessel.loaded ? vessel.id : vessel.protoVessel.vesselID) == targetVesselID)
				{
					targetVessel = vessel;
					break;
				}
			}
		}
		if (targetVessel == null)
		{
			targetVessel = VesselUtilities.FindVesselWithPartIDs(partIDs);
		}
		if (targetVessel == null)
		{
			ContractSystem.Instance.StartCoroutine(KillContract());
			return;
		}
		targetVesselPartCount = (targetVessel.loaded ? targetVessel.parts.Count : targetVessel.protoVessel.protoPartSnapshots.Count);
		if (targetVesselPartCount <= 0)
		{
			ContractSystem.Instance.StartCoroutine(KillContract());
			return;
		}
		targetVesselID = (targetVessel.loaded ? targetVessel.id : targetVessel.protoVessel.vesselID);
		targetVesselName = (targetVessel.loaded ? targetVessel.GetDisplayName() : targetVessel.protoVessel.GetDisplayName());
		targetVesselSituation = (targetVessel.loaded ? targetVessel.situation : targetVessel.protoVessel.situation);
		TargetBody = (targetVessel.loaded ? targetVessel.orbit.referenceBody : FlightGlobals.Bodies[targetVessel.protoVessel.orbitSnapShot.ReferenceBodyIndex]);
	}

	public override void CleanupWaypoints()
	{
		if (submittedWaypoint)
		{
			WaypointManager.RemoveWaypoint(wp);
			submittedWaypoint = false;
		}
	}

	public IEnumerator KillContract()
	{
		if (base.Root == null || base.Root.IsFinished())
		{
			yield break;
		}
		tracked = false;
		base.DisableOnStateChange = true;
		SetIncomplete();
		yield return null;
		switch (base.Root.ContractState)
		{
		case Contract.State.Active:
			SetFailed();
			yield break;
		case Contract.State.Offered:
			base.Root.Withdraw();
			yield break;
		}
		base.Root.Kill();
		if (ContractSystem.Instance != null)
		{
			ContractSystem.Instance.Contracts.Remove(base.Root);
		}
	}
}
