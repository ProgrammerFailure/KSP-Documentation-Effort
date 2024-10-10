using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class AsteroidParameter : ContractParameter
{
	public bool forStation;

	public List<int> asteroidSeeds;

	public string asteroidClass;

	public int successCounter;

	public bool validVessel;

	public bool dirtyVessel = true;

	public int activePartCount;

	public bool eventsAdded;

	public AsteroidParameter()
	{
		asteroidSeeds = new List<int>();
		forStation = false;
		asteroidClass = "A";
	}

	public AsteroidParameter(string size, bool forStation)
	{
		asteroidSeeds = new List<int>();
		this.forStation = forStation;
		asteroidClass = size.ToUpper();
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		if (forStation)
		{
			return Localizer.Format("#autoLOC_282737", asteroidClass);
		}
		return Localizer.Format("#autoLOC_282739", asteroidClass);
	}

	public override string GetNotes()
	{
		return Localizer.Format("#autoLOC_282744", base.Root.Agent.Title, asteroidClass);
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
		if (!SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active, checkVessel: true))
		{
			return;
		}
		if (asteroidSeeds == null)
		{
			asteroidSeeds = new List<int>();
		}
		List<ModuleAsteroid> list = action.from.vessel.FindPartModulesImplementing<ModuleAsteroid>();
		int count = list.Count;
		while (count-- > 0)
		{
			ModuleAsteroid moduleAsteroid = list[count];
			if (moduleAsteroid.vessel.DiscoveryInfo.Level.ToString() != "Owned")
			{
				if (moduleAsteroid.vessel.DiscoveryInfo.size.Value == asteroidClass)
				{
					asteroidSeeds.Add(moduleAsteroid.seed);
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_282809", base.Root.Agent.Title), 5f, ScreenMessageStyle.UPPER_LEFT);
				}
				else
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_282812", base.Root.Agent.Title), 5f, ScreenMessageStyle.UPPER_LEFT);
				}
				continue;
			}
			bool flag = false;
			int count2 = asteroidSeeds.Count;
			while (count2-- > 0)
			{
				if (moduleAsteroid.seed == asteroidSeeds[count2])
				{
					flag = true;
				}
			}
			if (!flag)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_282825", base.Root.Agent.Title), 5f, ScreenMessageStyle.UPPER_LEFT);
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_282827", base.Root.Agent.Title), 5f, ScreenMessageStyle.UPPER_LEFT);
			}
		}
		if (action.from.vessel == FlightGlobals.ActiveVessel || action.to.vessel == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("forStation", forStation);
		node.AddValue("asteroidClass", asteroidClass);
		SystemUtilities.SaveNodeList(node, "seedList", asteroidSeeds);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "AsteroidParameter", "forStation", ref forStation, defaultValue: false);
		SystemUtilities.LoadNode(node, "AsteroidParameter", "asteroidClass", ref asteroidClass, "A");
		SystemUtilities.LoadNodeList(node, "AsteroidParameter", "seedList", ref asteroidSeeds);
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
			validVessel = VesselTowingAsteroid(FlightGlobals.ActiveVessel);
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

	public bool VesselTowingAsteroid(Vessel v)
	{
		dirtyVessel = false;
		activePartCount = (v.loaded ? v.Parts.Count : v.protoVessel.protoPartSnapshots.Count);
		if (asteroidSeeds == null)
		{
			return false;
		}
		List<ModuleAsteroid> list = v.FindPartModulesImplementing<ModuleAsteroid>();
		int count = list.Count;
		while (count-- > 0)
		{
			int count2 = asteroidSeeds.Count;
			while (count2-- > 0)
			{
				if (list[count].seed == asteroidSeeds[count2])
				{
					return true;
				}
			}
		}
		return false;
	}
}
