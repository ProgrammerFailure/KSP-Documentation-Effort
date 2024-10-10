using System.Collections.Generic;
using Expansions.Missions.Runtime;
using ns11;
using ns9;

namespace Expansions.Missions;

[VesselRestrictionOptions(listedInSAP = false)]
public class VesselRestriction_RequiredParts : VesselRestriction
{
	public List<string> requiredPartsInUse;

	public VesselRestriction_RequiredParts()
	{
	}

	public VesselRestriction_RequiredParts(MENode node)
		: base(node)
	{
	}

	public override void SuscribeToEvents()
	{
		GameEvents.onEditorPodPicked.Add(Callback_PlacePart);
		GameEvents.onEditorPartPlaced.Add(Callback_PlacePart);
		GameEvents.onEditorStarted.Add(UpdateAppEntry);
		GameEvents.onEditorLoad.Add(Callback_ShipLoad);
	}

	public override void ClearEvents()
	{
		GameEvents.onEditorPartPlaced.Remove(Callback_PlacePart);
		GameEvents.onEditorPodPicked.Remove(Callback_PlacePart);
		GameEvents.onEditorStarted.Remove(UpdateAppEntry);
		GameEvents.onEditorLoad.Remove(Callback_ShipLoad);
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("Required Parts");
	}

	public override void Load(ConfigNode node)
	{
	}

	public override void Save(ConfigNode node)
	{
	}

	public override bool IsComplete()
	{
		if (UIEntry.Data is MissionsAppVesselInfo missionsAppVesselInfo)
		{
			if (requiredPartsInUse == null)
			{
				requiredPartsInUse = new List<string>();
			}
			else
			{
				requiredPartsInUse.Clear();
			}
			ShipConstruct ship = EditorLogic.fetch.ship;
			for (int i = 0; i < ship.parts.Count; i++)
			{
				if (missionsAppVesselInfo.vesselSituation.requiredParts.Contains(ship.parts[i].name))
				{
					requiredPartsInUse.AddUnique(ship.parts[i].name);
				}
			}
			return requiredPartsInUse.Count == missionsAppVesselInfo.vesselSituation.requiredParts.Count;
		}
		return false;
	}

	public override string GetStateMessage()
	{
		if (!(MissionsApp.Instance == null) && MissionsApp.Instance.CurrentVessel != null && MissionsApp.Instance.CurrentVessel.vesselSituation != null)
		{
			string text = Localizer.Format("#autoLOC_8005322");
			for (int i = 0; i < MissionsApp.Instance.CurrentVessel.vesselSituation.requiredParts.Count; i++)
			{
				AvailablePart partInfoByName = PartLoader.getPartInfoByName(MissionsApp.Instance.CurrentVessel.vesselSituation.requiredParts[i]);
				text = text + "\n- " + ((partInfoByName == null) ? MissionsApp.Instance.CurrentVessel.vesselSituation.requiredParts[i] : partInfoByName.title);
			}
			return text;
		}
		return "[MissionsApp]: RequiredParts restriction-UIEntry not created yet";
	}

	public void Callback_PlacePart(Part part)
	{
		UpdateAppEntry();
	}

	public void Callback_ShipLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		UpdateAppEntry();
	}
}
