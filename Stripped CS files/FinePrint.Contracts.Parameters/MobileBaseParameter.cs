using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class MobileBaseParameter : ContractParameter
{
	public int successCounter;

	public bool validVessel;

	public bool dirtyVessel = true;

	public int activePartCount;

	public bool eventsAdded;

	public MobileBaseParameter()
	{
		successCounter = 0;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_7001018");
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.onPartCouple.Add(OnDock);
			GameEvents.onVesselWasModified.Add(VesselModified);
			GameEvents.onPartDie.Add(PartModified);
			GameEvents.onVesselSituationChange.Add(ChangeSituation);
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
			GameEvents.onVesselSituationChange.Remove(ChangeSituation);
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

	public void ChangeSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> action)
	{
		if (action.host == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
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
			validVessel = VesselUtilities.VesselHasWheelsOnGround(FlightGlobals.ActiveVessel, WheelType.MOTORIZED);
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
