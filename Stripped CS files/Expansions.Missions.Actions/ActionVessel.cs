using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Actions;

public class ActionVessel : ActionModule
{
	[MEGUI_VesselSelect(order = 0, onControlCreated = "vesselSelectorCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001")]
	public uint vesselID;

	public bool useActiveVessel;

	public Vessel _vessel;

	public uint oldRootPart;

	public Vessel vessel
	{
		get
		{
			if (_vessel == null)
			{
				if (vesselID == 0)
				{
					if (FlightGlobals.ActiveVessel != null)
					{
						_vessel = FlightGlobals.ActiveVessel;
					}
				}
				else
				{
					uint key = node.mission.CurrentVesselID(node, vesselID);
					if (FlightGlobals.PersistentVesselIds.ContainsKey(key))
					{
						_vessel = FlightGlobals.PersistentVesselIds[key];
					}
				}
			}
			return _vessel;
		}
	}

	public override void Awake()
	{
		base.Awake();
	}

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (vesselID == oldId)
		{
			vesselID = newId;
		}
	}

	public override void OnVesselsDocking(uint oldId, uint newId)
	{
		if (oldId == vesselID)
		{
			Vessel vessel = null;
			FlightGlobals.FindVessel(oldId, out vessel);
			if (vessel != null)
			{
				oldRootPart = vessel.rootPart.persistentId;
			}
		}
	}

	public override void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		if (oldVessel.persistentId == vesselID || (newVessel.persistentId == vesselID && oldRootPart != 0))
		{
			Part partout = null;
			ProtoPartSnapshot partout2 = null;
			if (FlightGlobals.FindLoadedPart(oldRootPart, out partout))
			{
				vesselID = partout.vessel.persistentId;
			}
			else if (FlightGlobals.FindUnloadedPart(oldRootPart, out partout2))
			{
				vesselID = partout2.pVesselRef.persistentId;
			}
		}
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "vesselID")
		{
			VesselSituation vesselSituationByVesselID = node.mission.GetVesselSituationByVesselID(vesselID);
			if (vesselSituationByVesselID == null)
			{
				return Localizer.Format("#autoLOC_8000069", useActiveVessel ? Localizer.Format("#autoLOC_8004217") : Localizer.Format("#autoLOC_8001004"));
			}
			string text = vesselSituationByVesselID.vesselName;
			if (HighLogic.LoadedSceneIsMissionBuilder)
			{
				MissionCraft craftBySituationsVesselID = MissionEditorLogic.Instance.EditorMission.GetCraftBySituationsVesselID(vesselSituationByVesselID.persistentId);
				if (craftBySituationsVesselID != null && MissionEditorLogic.Instance.incompatibleCraft.Contains(craftBySituationsVesselID.craftFile))
				{
					text = Localizer.Format("#autoLOC_8004245", text);
				}
			}
			return Localizer.Format("#autoLOC_8000069", text);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public void vesselSelectorCreated(MEGUIParameter parameter)
	{
		(parameter as MEGUIParameterVesselDropdownList).OverrideDefaultValue(useActiveVessel);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("vesselID", vesselID);
		if (oldRootPart != 0)
		{
			node.AddValue("oldRootPart", oldRootPart);
		}
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("vesselID", ref vesselID);
		node.TryGetValue("oldRootPart", ref oldRootPart);
	}
}
