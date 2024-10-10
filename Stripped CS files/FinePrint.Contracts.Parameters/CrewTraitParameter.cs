using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class CrewTraitParameter : ContractParameter
{
	public string vesselDescription;

	public string targetTrait;

	public int targetCount;

	public int successCounter;

	public bool validVessel;

	public bool dirtyVessel = true;

	public int activePartCount;

	public bool eventsAdded;

	public SpecificVesselParameter specificVesselSibling;

	public string targetTraitDisplayName
	{
		get
		{
			if (KerbalRoster.TryGetExperienceTraitConfig(targetTrait, out var traitConfig))
			{
				return traitConfig.Title;
			}
			return targetTrait;
		}
	}

	public SpecificVesselParameter SpecificVesselSibling => specificVesselSibling ?? (specificVesselSibling = base.Root.GetParameter<SpecificVesselParameter>());

	public CrewTraitParameter()
	{
		targetTrait = "Scientist";
		targetCount = 1;
	}

	public CrewTraitParameter(string targetTrait, int targetCount, string vesselDescription)
	{
		this.targetTrait = targetTrait;
		this.targetCount = targetCount;
		this.vesselDescription = vesselDescription;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		if (targetCount <= 1)
		{
			return Localizer.Format("#autoLOC_6001095", targetTraitDisplayName.ToLower(), vesselDescription);
		}
		return Localizer.Format("#autoLOC_283108", targetCount, targetTraitDisplayName.ToLower(), vesselDescription);
	}

	public override string GetNotes()
	{
		if (!base.Root.IsFinished() && SpecificVesselSibling != null && !(SpecificVesselSibling.targetVessel == null))
		{
			int num = VesselUtilities.VesselCrewWithTraitCount(targetTrait, SpecificVesselSibling.targetVessel);
			if (num < 0)
			{
				return null;
			}
			if (num == 0)
			{
				return Localizer.Format("#autoLOC_283122", SpecificVesselSibling.targetVesselName, targetTraitDisplayName.ToLower());
			}
			return Localizer.Format("#autoLOC_283124", SpecificVesselSibling.targetVesselName, num, targetTraitDisplayName.ToLower());
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
			GameEvents.onVesselCrewWasModified.Add(CrewModified);
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
			GameEvents.onVesselCrewWasModified.Remove(CrewModified);
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

	public void CrewModified(Vessel v)
	{
		if (v != null && v == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetTrait", targetTrait);
		node.AddValue("targetCount", targetCount);
		node.AddValue("vesselDescription", vesselDescription);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "CrewTraitParameter", "targetTrait", ref targetTrait, "Scientist");
		SystemUtilities.LoadNode(node, "CrewTraitParameter", "targetCount", ref targetCount, 1);
		SystemUtilities.LoadNode(node, "CrewTraitParameter", "vesselDescription", ref vesselDescription, "vessel");
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
			validVessel = VesselUtilities.VesselCrewWithTraitCount(targetTrait, FlightGlobals.ActiveVessel) >= targetCount;
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
