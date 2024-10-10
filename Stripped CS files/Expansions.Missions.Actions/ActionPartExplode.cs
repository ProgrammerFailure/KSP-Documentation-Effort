using System.Collections;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionPartExplode : ActionModule
{
	[MEGUI_VesselPartSelect(resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000012", Tooltip = "#autoLOC_8000062")]
	public VesselPartIDPair vesselPartIDs;

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (vesselPartIDs.VesselID == oldId)
		{
			vesselPartIDs.VesselID = newId;
			Debug.LogFormat("[ActionPartExplode]: Node ({0}) VesselId changed from {1} to {2}", node.id, oldId, newId);
		}
	}

	public override void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		if (vesselPartIDs.VesselID == vesselID && vesselPartIDs.partID == oldId)
		{
			vesselPartIDs.partID = newId;
			Debug.LogFormat("[ActionPartExplode]: Node ({0}) PartId changed from {1} to {2}", node.id, oldId, newId);
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
		Debug.LogFormat("[ActionPartExplode]: Node ({0}) VesselId changed on undocking from {1} to {2}", node.id, oldVessel.persistentId, newVessel.persistentId);
	}

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000060");
		vesselPartIDs = new VesselPartIDPair();
		vesselPartIDs.partID = 0u;
	}

	public override IEnumerator Fire()
	{
		if (vesselPartIDs.partID != 0)
		{
			Part partout = null;
			if (FlightGlobals.FindLoadedPart(vesselPartIDs.partID, out partout))
			{
				partout.explode();
				if (partout == FlightGlobals.ActiveVessel.rootPart)
				{
					Vessel vessel = FlightGlobals.FindNearestControllableVessel(FlightGlobals.ActiveVessel);
					if (vessel != null)
					{
						FlightGlobals.ForceSetActiveVessel(vessel);
						FlightInputHandler.ResumeVesselCtrlState(vessel);
					}
				}
			}
			else
			{
				Debug.LogWarning($"[ActionPartExplode]: Unable to perform action, part {vesselPartIDs.partID} was not found on a loaded vessel.");
			}
		}
		else
		{
			uint key = node.mission.CurrentVesselID(node, vesselPartIDs.VesselID);
			Vessel vessel2 = FlightGlobals.PersistentVesselIds[key];
			if (vessel2 != null)
			{
				if (vessel2.loaded)
				{
					vessel2.Parts[Random.Range((1 < vessel2.Parts.Count) ? 1 : 0, vessel2.Parts.Count)].explode();
				}
				else
				{
					Debug.LogWarning($"[ActionPartExplode]: Unable to perform action, vessel {vesselPartIDs.VesselID} is not loaded.");
				}
			}
		}
		yield return null;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "vesselPartIDs")
		{
			return Localizer.Format("#autoLOC_8004190", field.guiName, Localizer.Format(vesselPartIDs.partName));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004003");
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (vesselPartIDs != null)
		{
			vesselPartIDs.ValidatePartAgainstCraft(node, validator);
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		vesselPartIDs.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		vesselPartIDs.Load(node);
	}
}
