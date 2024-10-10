using System.Collections;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionCreateFlag : ActionModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.Standard, tabStop = true, guiName = "#autoLOC_8004193")]
	public string siteName;

	[MEGUI_TextArea(tabStop = true, resetValue = "", guiName = "#autoLOC_8004194")]
	public string plaqueText;

	[MEGUI_VesselGroundLocation(DisableRotationY = true, DisableRotationX = true, gapDisplay = true, guiName = "#autoLOC_8000179")]
	public VesselGroundLocation location;

	public float flagRotationOffset = -30f;

	public uint persistentID;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8004174");
		siteName = Localizer.Format("#autoLOC_8100159");
		plaqueText = Localizer.Format("#autoLOC_8100159");
		location = new VesselGroundLocation(node, VesselGroundLocation.GizmoIcon.Flag);
		persistentID = FlightGlobals.GetUniquepersistentId();
	}

	public override IEnumerator Fire()
	{
		ConfigNode protoVesselNode = GetProtoVesselNode();
		HighLogic.CurrentGame.AddVessel(protoVesselNode);
		yield break;
	}

	public ConfigNode GetProtoVesselNode()
	{
		if (string.IsNullOrEmpty(siteName))
		{
			siteName = Localizer.Format("#autoLOC_8100159");
		}
		uint uniqueFlightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		Orbit orbit = Orbit.CreateRandomOrbitAround(location.targetBody);
		ConfigNode configNode = ProtoVessel.CreateVesselNode(siteName, VesselType.Flag, orbit, 0, new ConfigNode[1] { CreateFlagPartNode(uniqueFlightID) });
		configNode.SetValue("persistentId", persistentID);
		configNode.SetValue("landedAt", location.targetBody.name);
		configNode.SetValue("lat", location.latitude);
		configNode.SetValue("lon", location.longitude);
		configNode.SetValue("sit", "LANDED");
		configNode.SetValue("alt", location.altitude);
		configNode.SetValue("skipGroundPositioning", newValue: false);
		configNode.SetValue("landed", newValue: true);
		configNode.SetValue("splashed", newValue: false);
		configNode.AddValue("prst", value: false);
		Vector3d relSurfaceNVector = location.targetBody.GetRelSurfaceNVector(location.latitude, location.longitude);
		configNode.AddValue("nrm", Vector3.up);
		configNode.AddValue("rot", Quaternion.FromToRotation(Vector3.up, relSurfaceNVector) * Quaternion.AngleAxis(location.rotation.eulerZ + flagRotationOffset, Vector3.up));
		configNode.SetValue("vesselSpawning", newValue: true);
		if (location.targetBody.pqsController != null)
		{
			configNode.SetValue("PQSMin", 0);
			configNode.SetValue("PQSMax", 0);
		}
		return configNode;
	}

	public ConfigNode CreateFlagPartNode(uint id)
	{
		ConfigNode configNode = ProtoVessel.CreatePartNode("flag", id);
		configNode.SetValue("flag", node.mission.flagURL, createIfNotFound: true);
		ConfigNode configNode2 = new ConfigNode("MODULE");
		configNode2.AddValue("name", "FlagSite");
		configNode2.AddValue("state", "Placed");
		configNode2.AddValue("PlaqueText", plaqueText);
		configNode.AddNode(configNode2);
		return configNode;
	}

	public override Vector3 ActionLocation()
	{
		return location.GetWorldPosition();
	}

	public override void OnCloned(ref ActionModule actionModuleBase)
	{
		base.OnCloned(ref actionModuleBase);
		persistentID = FlightGlobals.GetUniquepersistentId();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004175");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "location")
		{
			return location.GetNodeBodyParameterString();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("siteName", siteName);
		node.AddValue("plaqueText", plaqueText);
		node.AddValue("persistentId", persistentID);
		location.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("siteName", ref siteName);
		node.TryGetValue("plaqueText", ref plaqueText);
		node.TryGetValue("persistentId", ref persistentID);
		location.Load(node);
	}
}
