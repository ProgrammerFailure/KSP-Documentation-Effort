using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestCrewAssignment : TestModule, IScoreableObjective
{
	[MEGUI_VesselPartSelect(resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000012", Tooltip = "#autoLOC_8000072")]
	public VesselPartIDPair vesselPartIDs;

	[MEGUI_MissionKerbal(statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, showAllRosterStatus = true, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	public Vessel vessel;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000070");
		vesselPartIDs = new VesselPartIDPair();
		vesselPartIDs.partID = 0u;
		missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
	}

	public override void Initialize(TestGroup testGroup, string title)
	{
		base.Initialize(testGroup, title);
		missionKerbal.Initialize(null, testGroup.node);
	}

	public override bool Test()
	{
		if (vesselPartIDs.partID == 0)
		{
			uint key = base.node.mission.CurrentVesselID(base.node, vesselPartIDs.VesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(key))
			{
				vessel = FlightGlobals.PersistentVesselIds[key];
			}
			if (vessel == null)
			{
				Debug.LogWarning("[TestCrewAssignment]: Vessel Not found for Test. Skipping..");
				return true;
			}
			int num = 0;
			while (true)
			{
				if (num < vessel.parts.Count)
				{
					if (vessel.parts[num].CrewCapacity > 0 && partContainsKerbal(vessel.parts[num].protoModuleCrew))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}
		if (FlightGlobals.PersistentLoadedPartIds.ContainsKey(vesselPartIDs.partID))
		{
			Part part = FlightGlobals.PersistentLoadedPartIds[vesselPartIDs.partID];
			if (part != null)
			{
				return partContainsKerbal(part.protoModuleCrew);
			}
			return false;
		}
		if (FlightGlobals.PersistentUnloadedPartIds.ContainsKey(vesselPartIDs.partID))
		{
			ProtoPartSnapshot protoPartSnapshot = FlightGlobals.PersistentUnloadedPartIds[vesselPartIDs.partID];
			if (protoPartSnapshot != null)
			{
				return partContainsKerbal(protoPartSnapshot.protoModuleCrew);
			}
			return false;
		}
		Debug.LogWarningFormat("[TestCrewAssignment]: Unable to find Vessel PartID ({0}). Skipping..", vesselPartIDs.partID);
		return true;
	}

	public bool partContainsKerbal(List<ProtoCrewMember> crew)
	{
		int num = 0;
		while (true)
		{
			if (num < crew.Count)
			{
				if (crew[num] != null && missionKerbal.IsValid(crew[num]))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public override void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		if (vesselPartIDs.VesselID == vesselID && vesselPartIDs.partID == oldId)
		{
			vesselPartIDs.partID = newId;
			Debug.LogFormat("[TestCrewAssignment]: Node ({0}) PartId changed from {1} to {2}", base.node.id, oldId, newId);
		}
	}

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (vesselPartIDs.VesselID == oldId)
		{
			vesselPartIDs.VesselID = newId;
			Debug.LogFormat("[TestCrewAssignment]: Node ({0}) VesselId changed from {1} to {2}", base.node.id, oldId, newId);
		}
	}

	public override void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		if (vesselPartIDs.VesselID != oldVessel.persistentId && vesselPartIDs.VesselID != newVessel.persistentId)
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < newVessel.parts.Count)
			{
				if (vesselPartIDs.partID == newVessel.parts[num].persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		vesselPartIDs.VesselID = newVessel.persistentId;
		Debug.LogFormat("[TestCrewAssignment]: Node ({0}) VesselId changed on undocking from {1} to {2}", base.node.id, oldVessel.persistentId, newVessel.persistentId);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004011");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (base.node != null)
		{
			base.node.UpdateInfoOnRefresh = true;
		}
		if (field.name == "missionKerbal")
		{
			return missionKerbal.GetNodeBodyParameterString();
		}
		if (field.name == "vesselID" && base.node.mission != null)
		{
			VesselSituation vesselSituation = new VesselSituation();
			string vesselName = "";
			bool flag = false;
			if (vesselPartIDs != null)
			{
				vesselSituation = base.node.mission.GetVesselSituationByVesselID(vesselPartIDs.VesselID);
			}
			if (vesselSituation != null)
			{
				vesselName = vesselSituation.vesselName;
			}
			else if (base.node.mission.UpdateFromMappedVesselIDs(vesselPartIDs.VesselID, ref vesselName, ref vesselSituation))
			{
				flag = true;
			}
			if ((vesselSituation == null || vesselSituation.persistentId == 0) && !flag)
			{
				return Localizer.Format("#autoLOC_8000068");
			}
			string text = Localizer.Format("#autoLOC_8000069", Localizer.Format(vesselName));
			if (flag && vesselSituation != null)
			{
				text = text + Localizer.Format("#autoLOC_6002491", vesselSituation.vesselName) + "\n";
			}
			return text;
		}
		if (field.name == "vesselPartIDs")
		{
			return Localizer.Format("#autoLOC_8006061", Localizer.Format(vesselPartIDs.partName));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (missionKerbal.Kerbal != null && base.node.mission.situation.crewRoster[missionKerbal.Kerbal.name].rosterStatus != ProtoCrewMember.RosterStatus.Assigned)
		{
			validator.AddNodeWarn(base.node, Localizer.Format("#autoLOC_8002033", missionKerbal.Name));
		}
		if (vesselPartIDs != null)
		{
			vesselPartIDs.ValidatePartAgainstCraft(base.node, validator);
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		vesselPartIDs.Save(node);
		missionKerbal.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		vesselPartIDs.Load(node);
		if (missionKerbal == null)
		{
			missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
		}
		missionKerbal.Load(node);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			uint key = base.node.mission.CurrentVesselID(base.node, vesselPartIDs.VesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(key))
			{
				return FlightGlobals.PersistentVesselIds[key];
			}
			return FlightGlobals.ActiveVessel;
		}
		return null;
	}
}
