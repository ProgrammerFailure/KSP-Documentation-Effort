using System.Collections;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionCreateComet : ActionModule, INodeOrbit
{
	[MEGUI_Comet(order = 1, gapDisplay = true, guiName = "#autoLOC_6005065")]
	public Comet comet;

	[MEGUI_ParameterSwitchCompound(order = 2, guiName = "#autoLOC_8000027", Tooltip = "#autoLOC_8005444")]
	public ParamChoices_VesselSimpleLocation location;

	public uint PersistentID => comet.persistentId;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8005445");
		comet = new Comet();
		location = new ParamChoices_VesselSimpleLocation();
		location.orbit = new MissionOrbit(FlightGlobals.GetHomeBody());
		location.landed = new VesselGroundLocation(node, VesselGroundLocation.GizmoIcon.Comet);
		location.locationChoice = ParamChoices_VesselSimpleLocation.Choices.orbit;
		location.landed.splashed = true;
	}

	public override IEnumerator Fire()
	{
		SpawnComet();
		yield break;
	}

	public override Vector3 ActionLocation()
	{
		Vector3 zero = Vector3.zero;
		return location.locationChoice switch
		{
			ParamChoices_VesselSimpleLocation.Choices.orbit => location.orbit.Orbit.getPositionAtUT((HighLogic.CurrentGame != null) ? HighLogic.CurrentGame.UniversalTime : 0.0), 
			ParamChoices_VesselSimpleLocation.Choices.landed => location.landed.GetWorldPosition(), 
			_ => zero, 
		};
	}

	public override void OnCloned(ref ActionModule actionModuleBase)
	{
		base.OnCloned(ref actionModuleBase);
		comet.seed = Comet.GetRandomCometSeed();
		comet.persistentId = FlightGlobals.GetUniquepersistentId();
	}

	public bool HasNodeOrbit()
	{
		return true;
	}

	public Orbit GetNodeOrbit()
	{
		Orbit orbit = null;
		orbit = location.orbit.Orbit;
		switch (location.locationChoice)
		{
		case ParamChoices_VesselSimpleLocation.Choices.landed:
			orbit.referenceBody = location.landed.targetBody;
			break;
		}
		return orbit;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.FieldType == typeof(Comet))
		{
			return Localizer.Format("#autoLOC_8000139", field.guiName, comet.cometDisplayType, comet.cometClass.displayDescription());
		}
		if (field.name == "location")
		{
			return location.GetNodeBodyParameterString();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8005446");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		comet.Save(node);
		location.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		comet.Load(node);
		location.Load(node);
	}

	public void SpawnComet()
	{
		ConfigNode protoVesselNode = GetProtoVesselNode();
		ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);
		GameEvents.onCometSpawned.Fire(protoVessel.vesselRef);
	}

	public ConfigNode GetProtoVesselNode()
	{
		ConfigNode configNode = ProtoVessel.CreatePartNode("PotatoComet", comet.seed);
		uint value = uint.Parse(configNode.GetValue("persistentId"));
		ConfigNode configNode2 = new ConfigNode("MODULE");
		configNode2.AddValue("name", "ModuleComet");
		configNode2.AddValue("seed", comet.seed);
		configNode2.AddValue("type", comet.cometType);
		if (comet.seed == 1633779195)
		{
			configNode2.AddValue("prefabBaseURL", "Comets/Rosetta");
		}
		configNode.AddNode(configNode2);
		ConfigNode configNode3 = new ConfigNode("DISCOVERY");
		configNode3.AddValue("size", (int)comet.cometClass);
		bool flag;
		bool flag2 = (flag = location.locationChoice == ParamChoices_VesselSimpleLocation.Choices.landed) && location.landed.splashed;
		Vessel.Situations situations = (flag ? Vessel.Situations.LANDED : Vessel.Situations.ORBITING);
		Orbit orbit = (flag ? Orbit.CreateRandomOrbitAround(location.landed.targetBody) : location.orbit.RelativeOrbit(node.mission));
		ConfigNode configNode4 = ProtoVessel.CreateVesselNode(comet.name, VesselType.SpaceObject, orbit, 0, new ConfigNode[2] { configNode, configNode3 });
		double num = location.landed.altitude;
		Vector3d relSurfaceNVector = location.landed.targetBody.GetRelSurfaceNVector(location.landed.latitude, location.landed.longitude);
		if (flag2)
		{
			num = location.landed.targetBody.pqsController.GetSurfaceHeight(relSurfaceNVector, overrideQuadBuildCheck: true) - location.landed.targetBody.Radius;
			if (num < 0.0)
			{
				num = 0.0;
				flag = false;
				situations = Vessel.Situations.SPLASHED;
			}
			else
			{
				flag2 = false;
			}
		}
		configNode4.SetValue("persistentId", PersistentID);
		configNode4.SetValue("sit", situations.ToString());
		configNode4.SetValue("landedAt", location.landed.targetBody.name);
		configNode4.SetValue("lat", location.landed.latitude);
		configNode4.SetValue("lon", location.landed.longitude);
		DiscoveryInfo.GetClassRadius(comet.cometClass, (int)comet.seed);
		configNode4.SetValue("alt", num + (double)comet.radius);
		configNode4.SetValue("landed", flag);
		configNode4.SetValue("splashed", flag2);
		configNode4.AddValue("rot", Quaternion.FromToRotation(Vector3.up, relSurfaceNVector) * Quaternion.AngleAxis(location.landed.rotation.eulerZ, Vector3.up));
		configNode4.SetValue("vesselSpawning", newValue: true);
		if (flag)
		{
			configNode4.SetValue("skipGroundPositioning", newValue: false);
			if (location.landed.targetBody.pqsController != null)
			{
				configNode4.SetValue("PQSMin", 0);
				configNode4.SetValue("PQSMax", 0);
			}
		}
		else
		{
			configNode4.SetValue("skipGroundPositioning", newValue: true);
		}
		CometDefinition cometDefinition = CometManager.GenerateDefinition(CometManager.GetCometOrbitType(comet.cometType), comet.cometClass, (int)comet.seed);
		ConfigNode configNode5 = new ConfigNode("VESSELMODULES");
		ConfigNode configNode6 = cometDefinition.CreateVesselNode(optimizedCollider: false, 0f, hasName: true);
		configNode6.AddValue("cometPartId", value);
		configNode5.AddNode(configNode6);
		configNode4.AddNode(configNode5);
		return configNode4;
	}
}
