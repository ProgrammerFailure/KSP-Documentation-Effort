using Expansions.Missions.Editor;
using FinePrint.Utilities;
using UnityEngine;

namespace Expansions.Missions;

public class AwardModule_LandedOn : AwardModule
{
	[MEGUI_VesselSelect(gapDisplay = false, guiName = "#autoLOC_8000001")]
	public uint vesselID;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, guiName = "#autoLOC_8100017")]
	public double distance;

	public AwardModule_LandedOn(MENode node)
		: base(node)
	{
	}

	public AwardModule_LandedOn(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
	}

	public override bool EvaluateCondition(Mission mission)
	{
		Vessel vessel = GetVessel();
		if (vessel != null && vessel.situation == Vessel.Situations.LANDED)
		{
			PSystemSetup.SpaceCenterFacility spaceCenterFacility = PSystemSetup.Instance.GetSpaceCenterFacility("LaunchPad");
			if (spaceCenterFacility != null && !string.IsNullOrEmpty(spaceCenterFacility.pqsName) && spaceCenterFacility.facilityPQS != null)
			{
				CelestialBody bodyByName = FlightGlobals.GetBodyByName(spaceCenterFacility.pqsName);
				if (bodyByName != null)
				{
					SurfaceLocation surfaceLocation = default(SurfaceLocation);
					bodyByName.GetLatLonAlt(spaceCenterFacility.facilityTransform.parent.position, out surfaceLocation.latitude, out surfaceLocation.longitude, out surfaceLocation.altitude);
					return CelestialUtilities.GreatCircleDistance(bodyByName, vessel.latitude, vessel.longitude, surfaceLocation.latitude, surfaceLocation.longitude) <= distance;
				}
			}
		}
		return false;
	}

	public Vessel GetVessel()
	{
		Vessel vessel = null;
		if (vesselID == 0)
		{
			return FlightGlobals.ActiveVessel;
		}
		uint key = node.mission.CurrentVesselID(node, vesselID);
		if (FlightGlobals.PersistentVesselIds.ContainsKey(key))
		{
			return FlightGlobals.PersistentVesselIds[key];
		}
		Debug.LogErrorFormat("[AwardModule] Unable to find VesselID ({0}) from award {1} in FlightGlobals.", vesselID, GetDisplayName());
		return null;
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("vesselID", ref vesselID);
		node.TryGetValue("distance", ref distance);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("vesselID", vesselID);
		node.AddValue("distance", distance);
	}
}
