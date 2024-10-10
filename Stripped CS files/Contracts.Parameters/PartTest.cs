using System;
using System.Collections.Generic;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Contracts.Parameters;

[Serializable]
public class PartTest : ContractParameter
{
	public string partName;

	public AvailablePart tgtPartInfo;

	public List<uint> testedParts;

	public string testNotes;

	public string body;

	public string situation;

	public string uniqueID;

	public PartTestConstraint.TestRepeatability repeatability = PartTestConstraint.TestRepeatability.ONCEPERPART;

	public bool hauled;

	public bool eventsAdded;

	public bool validVessel;

	public bool dirtyVessel = true;

	public int successCounter;

	public int activePartCount;

	public string title;

	public PartTest()
	{
		testedParts = new List<uint>();
	}

	public PartTest(AvailablePart tgtPart, string testNotes, PartTestConstraint.TestRepeatability partRepeatability, CelestialBody tgtBody, Vessel.Situations tgtSit, string id, bool hauled)
	{
		testedParts = new List<uint>();
		tgtPartInfo = tgtPart;
		partName = tgtPartInfo.name;
		this.testNotes = testNotes;
		repeatability = partRepeatability;
		body = tgtBody.bodyName;
		situation = tgtSit.ToString();
		uniqueID = id;
		this.hauled = hauled;
	}

	public override string GetHashString()
	{
		string text = partName;
		switch (repeatability)
		{
		case PartTestConstraint.TestRepeatability.BODYANDSITUATION:
			text = text + body + situation;
			break;
		case PartTestConstraint.TestRepeatability.ALWAYS:
			text += uniqueID;
			break;
		}
		return text;
	}

	public override string GetTitle()
	{
		if (!string.IsNullOrEmpty(title))
		{
			return title;
		}
		string text = ((tgtPartInfo != null) ? tgtPartInfo.title : partName);
		if (hauled)
		{
			return Localizer.Format("#autoLOC_6100000", text);
		}
		return Localizer.Format("#autoLOC_6100001", text);
	}

	public override string GetNotes()
	{
		return testNotes;
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("part"))
		{
			partName = node.GetValue("part");
			tgtPartInfo = PartLoader.getPartInfoByName(partName);
		}
		if (node.HasValue("notes"))
		{
			testNotes = node.GetValue("notes");
		}
		if (node.HasValue("body"))
		{
			body = node.GetValue("body");
		}
		if (node.HasValue("situation"))
		{
			situation = node.GetValue("situation");
		}
		if (node.HasValue("uniqueID"))
		{
			uniqueID = node.GetValue("uniqueID");
		}
		if (node.HasValue("haul"))
		{
			hauled = bool.Parse(node.GetValue("haul"));
		}
		if (node.HasValue("title"))
		{
			body = node.GetValue("title");
		}
		if (node.HasValue("repeatability"))
		{
			try
			{
				repeatability = (PartTestConstraint.TestRepeatability)Enum.Parse(typeof(PartTestConstraint.TestRepeatability), node.GetValue("repeatability"));
			}
			catch
			{
				Debug.LogError("[PartTestConstraint]: Error parsing value of type repeatability, value =  " + node.GetValue("repeatability") + " in constraint for Part Test Contract.");
			}
		}
		string[] values = node.GetValues("tested");
		int num = values.Length;
		for (int i = 0; i < num; i++)
		{
			testedParts.Add(uint.Parse(values[i]));
		}
		if (tgtPartInfo == null || tgtPartInfo.TechHidden)
		{
			if (base.Root.ContractState == Contract.State.Active)
			{
				base.Root.Fail();
			}
			else if (base.Root.ContractState == Contract.State.Offered)
			{
				base.Root.Withdraw();
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("part", partName);
		node.AddValue("body", body);
		node.AddValue("situation", situation);
		node.AddValue("uniqueID", uniqueID);
		node.AddValue("haul", hauled);
		node.AddValue("repeatability", repeatability.ToString());
		if (!string.IsNullOrEmpty(title))
		{
			node.AddValue("title", title);
		}
		if (testNotes != null)
		{
			node.AddValue("notes", testNotes);
		}
		int i = 0;
		for (int count = testedParts.Count; i < count; i++)
		{
			node.AddValue("tested", testedParts[i]);
		}
	}

	public override void OnRegister()
	{
		if (base.Root.ContractState == Contract.State.Active)
		{
			if (hauled)
			{
				base.DisableOnStateChange = false;
				GameEvents.onPartCouple.Add(OnDock);
				GameEvents.onVesselWasModified.Add(VesselModified);
				GameEvents.onPartDie.Add(PartModified);
			}
			else
			{
				ModuleTestSubject.onTestRun.Add(OnPartRunTest);
				ModuleTestSubject.onTestSujectDestroyed.Add(OnTestPartDestroyed);
			}
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			if (hauled)
			{
				GameEvents.onPartCouple.Remove(OnDock);
				GameEvents.onVesselWasModified.Remove(VesselModified);
				GameEvents.onPartDie.Remove(PartModified);
			}
			else
			{
				ModuleTestSubject.onTestRun.Remove(OnPartRunTest);
				ModuleTestSubject.onTestSujectDestroyed.Remove(OnTestPartDestroyed);
			}
		}
	}

	public void OnPartRunTest(ModuleTestSubject p)
	{
		if (!hauled && !(p.part.partInfo.name != partName) && AllChildParametersComplete())
		{
			if (!testedParts.Contains(p.part.flightID))
			{
				testedParts.Add(p.part.flightID);
			}
			SetComplete();
		}
	}

	public void OnTestPartDestroyed(ModuleTestSubject p)
	{
		if (!hauled && !(p.part.partInfo.name != partName) && testedParts.Contains(p.part.flightID))
		{
			testedParts.Remove(p.part.flightID);
		}
	}

	public void OnVesselChanged(Vessel v)
	{
		if (!hauled)
		{
			if (state == ParameterState.Incomplete && hasTestedPart(v))
			{
				SetComplete();
			}
			if (state == ParameterState.Complete && !hasTestedPart(v))
			{
				SetIncomplete();
			}
		}
	}

	public bool hasTestedPart(Vessel v)
	{
		int count = v.parts.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!testedParts.Contains(v.parts[count].flightID));
		return true;
	}

	public override void OnReset()
	{
		if (hauled)
		{
			SetIncomplete();
			dirtyVessel = true;
		}
	}

	public void VesselModified(Vessel v)
	{
		if (hauled && v == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public void PartModified(Part p)
	{
		if (hauled && p.vessel == FlightGlobals.ActiveVessel)
		{
			dirtyVessel = true;
		}
	}

	public void OnDock(GameEvents.FromToAction<Part, Part> action)
	{
		if (hauled && (action.from.vessel == FlightGlobals.ActiveVessel || action.to.vessel == FlightGlobals.ActiveVessel))
		{
			dirtyVessel = true;
		}
	}

	public override void OnUpdate()
	{
		if (!hauled || !SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active, checkVessel: true))
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
			validVessel = VesselUtilities.VesselHasPartName(partName);
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
