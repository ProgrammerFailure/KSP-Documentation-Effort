using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class VesselSystemsParameter : ContractParameter
{
	public bool requireNew;

	public List<string> checkModuleTypes;

	public List<string> checkModuleDescriptions;

	public MannedStatus mannedStatus;

	public string vesselDescription;

	public uint launchID;

	public int successCounter;

	public bool validVessel;

	public bool dirtyVessel = true;

	public int activePartCount;

	public bool eventsAdded;

	public string mannedString => mannedStatus switch
	{
		MannedStatus.UNMANNED => Localizer.Format("#autoLOC_286382"), 
		MannedStatus.MANNED => Localizer.Format("#autoLOC_286380"), 
		_ => "", 
	};

	public VesselSystemsParameter()
	{
		checkModuleTypes = new List<string>();
		checkModuleDescriptions = new List<string>();
		requireNew = false;
		vesselDescription = "potato";
	}

	public VesselSystemsParameter(List<string> checkModuleTypes, List<string> checkModuleDescriptions, string vesselDescription, MannedStatus mannedStatus = MannedStatus.const_2, bool requireNew = true)
	{
		this.checkModuleTypes = checkModuleTypes;
		this.checkModuleDescriptions = checkModuleDescriptions;
		this.requireNew = requireNew;
		this.vesselDescription = vesselDescription;
		this.mannedStatus = mannedStatus;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		if (checkModuleDescriptions != null && checkModuleDescriptions.Count > 0)
		{
			if (requireNew)
			{
				return Localizer.Format("#autoLOC_7001085", (mannedString == "") ? "" : mannedString, vesselDescription, StringUtilities.ThisThisAndThat(checkModuleDescriptions)) + "\n" + Localizer.Format("#autoLOC_6002645", vesselDescription);
			}
			return Localizer.Format("#autoLOC_7001086", vesselDescription, StringUtilities.ThisThisAndThat(checkModuleDescriptions));
		}
		if (requireNew)
		{
			return Localizer.Format("#autoLOC_286401", mannedString, vesselDescription) + "\n" + Localizer.Format("#autoLOC_6002645", vesselDescription);
		}
		if (mannedStatus != MannedStatus.const_2)
		{
			return Localizer.Format("#autoLOC_286404", vesselDescription, mannedString);
		}
		return Localizer.Format("#autoLOC_286407", vesselDescription);
	}

	public override string GetNotes()
	{
		if (requireNew)
		{
			return Localizer.Format("#autoLOC_7000009", mannedString, vesselDescription, base.Root.Agent.Title);
		}
		return null;
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
		eventsAdded = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.onPartCouple.Add(OnDock);
			GameEvents.onVesselWasModified.Add(VesselModified);
			GameEvents.onPartDie.Add(PartModified);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.onPartCouple.Remove(OnDock);
			GameEvents.onVesselWasModified.Remove(VesselModified);
			GameEvents.onPartDie.Remove(PartModified);
		}
	}

	public override void OnReset()
	{
		SetIncomplete();
		dirtyVessel = true;
	}

	public void VesselModified(Vessel v)
	{
		if (v == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public void PartModified(Part p)
	{
		if (p.vessel == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public void OnDock(GameEvents.FromToAction<Part, Part> action)
	{
		if (action.from.vessel == FlightGlobals.ActiveVessel || action.to.vessel == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("typeString", vesselDescription);
		node.AddValue("mannedStatus", (int)mannedStatus);
		node.AddValue("requireNew", requireNew);
		SystemUtilities.SaveNodeList(node, "checkModuleTypes", checkModuleTypes);
		SystemUtilities.SaveNodeList(node, "checkModuleDescriptions", checkModuleDescriptions);
		if (requireNew)
		{
			node.AddValue("launchID", launchID);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "VesselSystemsParameter", "typeString", ref vesselDescription, "potato");
		SystemUtilities.LoadNode(node, "VesselSystemsParameter", "mannedStatus", ref mannedStatus, MannedStatus.const_2);
		SystemUtilities.LoadNode(node, "VesselSystemsParameter", "requireNew", ref requireNew, defaultValue: false);
		SystemUtilities.LoadNodeList(node, "VesselSystemsParameter", "checkModuleTypes", ref checkModuleTypes);
		SystemUtilities.LoadNodeList(node, "VesselSystemsParameter", "checkModuleDescriptions", ref checkModuleDescriptions);
		if (requireNew)
		{
			SystemUtilities.LoadNode(node, "VesselSystemsParameter", "launchID", ref launchID, 0u);
		}
	}

	public override void OnUpdate()
	{
		if (!SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active, checkVessel: true))
		{
			return;
		}
		int num = (FlightGlobals.ActiveVessel.loaded ? FlightGlobals.ActiveVessel.Parts.Count : FlightGlobals.ActiveVessel.protoVessel.protoPartSnapshots.Count);
		if (activePartCount != num)
		{
			dirtyVessel = true;
		}
		if (dirtyVessel)
		{
			dirtyVessel = false;
			activePartCount = (FlightGlobals.ActiveVessel.loaded ? FlightGlobals.ActiveVessel.Parts.Count : FlightGlobals.ActiveVessel.protoVessel.protoPartSnapshots.Count);
			validVessel = isVesselValid(FlightGlobals.ActiveVessel);
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

	public bool isVesselValid(Vessel v)
	{
		if (v == null)
		{
			return false;
		}
		if (!v.IsControllable)
		{
			return false;
		}
		int num = (v.loaded ? v.GetCrewCount() : v.protoVessel.GetVesselCrew().Count);
		if (mannedStatus == MannedStatus.UNMANNED && num > 0)
		{
			return false;
		}
		if (mannedStatus == MannedStatus.MANNED && num < 1)
		{
			return false;
		}
		if (requireNew && !VesselUtilities.VesselLaunchedAfterID(launchID, v, "PotatoRoid"))
		{
			return false;
		}
		if (checkModuleTypes != null && checkModuleTypes.Count > 0 && !v.HasValidContractObjectives(checkModuleTypes))
		{
			return false;
		}
		return true;
	}
}
