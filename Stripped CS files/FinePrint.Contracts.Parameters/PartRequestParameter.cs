using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class PartRequestParameter : ContractParameter
{
	public int successCounter;

	public string article;

	public string partDescription;

	public string vesselDescription;

	public List<string> partNames;

	public List<string> moduleNames;

	public List<uint> existingParts;

	public bool validVessel;

	public bool dirtyVessel = true;

	public int activePartCount;

	public bool eventsAdded;

	public PartRequestParameter()
	{
		partNames = new List<string>();
		moduleNames = new List<string>();
		existingParts = new List<uint>();
		successCounter = 0;
		article = "a";
		partDescription = "part";
		vesselDescription = "vessel";
		partNames.Add("part");
		moduleNames.Add("module");
	}

	public PartRequestParameter(ConfigNode requestNode, Vessel v = null)
	{
		partNames = new List<string>();
		moduleNames = new List<string>();
		existingParts = new List<uint>();
		successCounter = 0;
		article = "a";
		partDescription = "part";
		vesselDescription = "vessel";
		SystemUtilities.LoadNode(requestNode, "PartRequestParameter", "Article", ref article, "a", logging: false);
		SystemUtilities.LoadNode(requestNode, "PartRequestParameter", "PartDescription", ref partDescription, "part", logging: false);
		SystemUtilities.LoadNode(requestNode, "PartRequestParameter", "VesselDescription", ref vesselDescription, "vessel", logging: false);
		partNames = new List<string>(requestNode.GetValues("Part"));
		moduleNames = new List<string>(requestNode.GetValues("Module"));
		if (v != null)
		{
			ScanVesselForExistingParts(v);
		}
		CheckLists();
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		if (existingParts.Count > 0)
		{
			return Localizer.Format("#autoLOC_7001087", partDescription, vesselDescription);
		}
		return Localizer.Format("#autoLOC_7001088", partDescription, vesselDescription);
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
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
		node.AddValue("article", article);
		node.AddValue("partDescription", partDescription);
		node.AddValue("vesselDescription", vesselDescription);
		SystemUtilities.SaveNodeList(node, "partNames", partNames);
		SystemUtilities.SaveNodeList(node, "moduleNames", moduleNames);
		SystemUtilities.SaveNodeList(node, "existingParts", existingParts);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "PartRequestParameter", "article", ref article, "a");
		SystemUtilities.LoadNode(node, "PartRequestParameter", "partDescription", ref partDescription, "part");
		SystemUtilities.LoadNode(node, "PartRequestParameter", "vesselDescription", ref vesselDescription, "vessel");
		SystemUtilities.LoadNodeList(node, "PartRequestParameter", "partNames", ref partNames);
		SystemUtilities.LoadNodeList(node, "PartRequestParameter", "moduleNames", ref moduleNames);
		SystemUtilities.LoadNodeList(node, "PartRequestParameter", "existingParts", ref existingParts);
		CheckLists();
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
			activePartCount = num;
			validVessel = VesselHasPartRequest();
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

	public void CheckLists()
	{
		if (partNames.Count == 0 && moduleNames.Count == 0)
		{
			base.Root.RemoveParameter(this);
			Unregister();
		}
	}

	public bool VesselHasPartRequest(Vessel v = null)
	{
		if (!VesselUtilities.ActiveVesselFallback(ref v, logging: false))
		{
			return false;
		}
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				Part part = v.Parts[count];
				if (existingParts.Contains(part.flightID))
				{
					continue;
				}
				int count2 = partNames.Count;
				while (count2-- > 0)
				{
					if (VesselUtilities.GetPartName(part) == partNames[count2].Replace('_', '.'))
					{
						return true;
					}
				}
				int count3 = part.Modules.Count;
				while (count3-- > 0)
				{
					PartModule partModule = part.Modules[count3];
					int count4 = moduleNames.Count;
					while (count4-- > 0)
					{
						if (partModule.moduleName == moduleNames[count4])
						{
							return true;
						}
					}
				}
			}
		}
		else
		{
			int count5 = v.protoVessel.protoPartSnapshots.Count;
			while (count5-- > 0)
			{
				ProtoPartSnapshot protoPartSnapshot = v.protoVessel.protoPartSnapshots[count5];
				if (existingParts.Contains(protoPartSnapshot.flightID))
				{
					continue;
				}
				int count6 = partNames.Count;
				while (count6-- > 0)
				{
					if (protoPartSnapshot.partName == partNames[count6].Replace('_', '.'))
					{
						return true;
					}
				}
				int count7 = protoPartSnapshot.modules.Count;
				while (count7-- > 0)
				{
					ProtoPartModuleSnapshot protoPartModuleSnapshot = protoPartSnapshot.modules[count7];
					int count8 = moduleNames.Count;
					while (count8-- > 0)
					{
						if (protoPartModuleSnapshot.moduleName == moduleNames[count8])
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public void ScanVesselForExistingParts(Vessel v)
	{
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				Part part = v.Parts[count];
				int count2 = partNames.Count;
				while (count2-- > 0)
				{
					if (VesselUtilities.GetPartName(part) == partNames[count2].Replace('_', '.') && !existingParts.Contains(part.flightID))
					{
						existingParts.Add(part.flightID);
						break;
					}
				}
				int count3 = part.Modules.Count;
				while (count3-- > 0 && !existingParts.Contains(part.flightID))
				{
					PartModule partModule = part.Modules[count3];
					int count4 = moduleNames.Count;
					while (count4-- > 0)
					{
						if (partModule.moduleName == moduleNames[count4])
						{
							existingParts.Add(part.flightID);
							break;
						}
					}
				}
			}
			return;
		}
		int count5 = v.protoVessel.protoPartSnapshots.Count;
		while (count5-- > 0)
		{
			ProtoPartSnapshot protoPartSnapshot = v.protoVessel.protoPartSnapshots[count5];
			int count6 = partNames.Count;
			while (count6-- > 0)
			{
				if (protoPartSnapshot.partName == partNames[count6].Replace('_', '.') && !existingParts.Contains(protoPartSnapshot.flightID))
				{
					existingParts.Add(protoPartSnapshot.flightID);
					break;
				}
			}
			int count7 = protoPartSnapshot.modules.Count;
			while (count7-- > 0 && !existingParts.Contains(protoPartSnapshot.flightID))
			{
				ProtoPartModuleSnapshot protoPartModuleSnapshot = protoPartSnapshot.modules[count7];
				int count8 = moduleNames.Count;
				while (count8-- > 0)
				{
					if (protoPartModuleSnapshot.moduleName == moduleNames[count8])
					{
						existingParts.Add(protoPartSnapshot.flightID);
						break;
					}
				}
			}
		}
	}
}
