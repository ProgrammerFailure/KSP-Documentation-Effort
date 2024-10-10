using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionCreateKerbal : ActionModule, INodeOrbit, IMissionKerbal
{
	[MEGUI_MissionKerbal(tabStop = true, statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, showAllRosterStatus = false, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	[MEGUI_Checkbox(onValueChange = "OnHelmetValueChange", order = 8, guiName = "#autoLOC_6010016", Tooltip = "#autoLOC_6010017")]
	public bool isHelmetEnabled = GameSettings.EVA_DEFAULT_HELMET_ON;

	[MEGUI_Checkbox(order = 9, onControlCreated = "OnNeckRingControlCreated", guiName = "#autoLOC_6010018", Tooltip = "#autoLOC_6010019")]
	public bool isNeckRingEnabled = GameSettings.EVA_DEFAULT_NECKRING_ON;

	[MEGUI_Checkbox(order = 10, resetValue = "false", guiName = "#autoLOC_8000024", Tooltip = "#autoLOC_8000026")]
	public bool isStranded;

	[MEGUI_ParameterSwitchCompound(order = 20, guiName = "#autoLOC_8000027")]
	public ParamChoices_VesselSimpleLocation location;

	public uint persistentId;

	public string kerbalName;

	public double TimeDeadline = double.MaxValue;

	public float kerbalRotationOffset = -90f;

	public MEGUIParameterCheckbox neckRingCheckbox;

	public float defaultKerbalFeetToPivotDist = 0.25f;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000240");
		kerbalName = "";
		missionKerbal = new MissionKerbal(null, node, UpdateNodeBodyUI);
		location = new ParamChoices_VesselSimpleLocation();
		location.orbit = new MissionOrbit(FlightGlobals.GetHomeBody());
		location.landed = new VesselGroundLocation(node, VesselGroundLocation.GizmoIcon.Kerbal);
		location.landed.splashed = true;
		persistentId = FlightGlobals.GetUniquepersistentId();
		location.locationChoice = ParamChoices_VesselSimpleLocation.Choices.landed;
	}

	public override void Initialize(MENode node)
	{
		base.Initialize(node);
		missionKerbal.Initialize(null, node);
	}

	public override IEnumerator Fire()
	{
		if (!string.IsNullOrEmpty(kerbalName))
		{
			missionKerbal.Kerbal = null;
			IEnumerator<ProtoCrewMember> enumerator = node.mission.situation.crewRoster.Crew.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (kerbalName == enumerator.Current.name)
					{
						missionKerbal.Kerbal = enumerator.Current;
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}
		SpawnKerbal();
		yield break;
	}

	public ConfigNode SpawnKerbal(bool addtoCurrentGame = true)
	{
		if (missionKerbal.Kerbal == null || string.IsNullOrEmpty(missionKerbal.Kerbal.name))
		{
			Debug.LogFormat("Unable to find kerbal {0} in mission roster, generating a random Kerbal of type {1}", kerbalName, missionKerbal.TypeToShow);
			missionKerbal.Kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal(missionKerbal.TypeToShow);
			missionKerbal.Kerbal = missionKerbal.Kerbal;
			kerbalName = missionKerbal.Kerbal.name;
		}
		missionKerbal.Kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
		missionKerbal.Kerbal.seat = null;
		missionKerbal.Kerbal.seatIdx = -1;
		missionKerbal.Kerbal.hasHelmetOn = isHelmetEnabled;
		missionKerbal.Kerbal.hasNeckRingOn = isNeckRingEnabled;
		missionKerbal.Kerbal.completedFirstEVA = true;
		KerbalEVA kerbalEVA = null;
		AvailablePart partInfoByName = PartLoader.getPartInfoByName(missionKerbal.Kerbal.GetKerbalEVAPartName());
		if (partInfoByName != null && partInfoByName.partPrefab != null)
		{
			kerbalEVA = partInfoByName.partPrefab.GetComponent<KerbalEVA>();
		}
		if (!(kerbalEVA != null))
		{
		}
		ConfigNode protoVesselNode = GetProtoVesselNode();
		if (addtoCurrentGame)
		{
			HighLogic.CurrentGame.AddVessel(protoVesselNode);
		}
		return protoVesselNode;
	}

	public ConfigNode GetProtoVesselNode()
	{
		return GetProtoVesselNode(defaultKerbalFeetToPivotDist);
	}

	public ConfigNode GetProtoVesselNode(float feetToPivotDistance)
	{
		uint uniqueFlightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		bool flag;
		bool flag2 = (flag = location.locationChoice == ParamChoices_VesselSimpleLocation.Choices.landed) && location.landed.splashed;
		Orbit orbit = (flag ? Orbit.CreateRandomOrbitAround(location.landed.targetBody) : location.orbit.RelativeOrbit(node.mission));
		ConfigNode configNode = new ConfigNode();
		configNode = ((!isStranded) ? ProtoVessel.CreateVesselNode(missionKerbal.Kerbal.name, VesselType.const_11, orbit, 0, new ConfigNode[1] { CreateKerbalPartNode(uniqueFlightID, missionKerbal.Kerbal) }) : ProtoVessel.CreateVesselNode(missionKerbal.Kerbal.name, VesselType.const_11, orbit, 0, new ConfigNode[1] { CreateKerbalPartNode(uniqueFlightID, missionKerbal.Kerbal) }, ProtoVessel.CreateDiscoveryNode(DiscoveryLevels.Unowned, UntrackedObjectClass.const_0, TimeDeadline * 2.0, TimeDeadline * 2.0)));
		Vessel.Situations situations = (flag ? Vessel.Situations.LANDED : Vessel.Situations.ORBITING);
		configNode.SetValue("landedAt", location.landed.targetBody.name);
		configNode.SetValue("lat", location.landed.latitude);
		configNode.SetValue("lon", location.landed.longitude);
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
		if (flag && !flag2)
		{
			num += (double)feetToPivotDistance;
		}
		configNode.SetValue("sit", situations.ToString());
		configNode.SetValue("alt", num);
		configNode.SetValue("landed", flag);
		configNode.SetValue("splashed", flag2);
		configNode.SetValue("skipGroundPositioning", newValue: false);
		configNode.SetValue("vesselSpawning", newValue: true);
		configNode.AddValue("prst", value: true);
		Vector3 vector = location.landed.targetBody.GetRelSurfacePosition(location.landed.latitude, location.landed.longitude, num);
		Quaternion identity = Quaternion.identity;
		Vector3 normalized = vector.normalized;
		Vector3 normalized2;
		if (location.landed.longitude > 89.9000015258789)
		{
			normalized2 = Vector3.Cross(((Vector3)location.landed.targetBody.GetRelSurfacePosition(location.landed.latitude - 1E-05, location.landed.longitude, num) - vector).normalized, normalized).normalized;
		}
		else
		{
			Vector3 vector2 = location.landed.targetBody.GetRelSurfacePosition(location.landed.latitude + 1E-05, location.landed.longitude, num);
			normalized2 = Vector3.Cross(normalized, (vector2 - vector).normalized).normalized;
		}
		identity = Quaternion.LookRotation(normalized2, normalized);
		identity = Quaternion.AngleAxis(kerbalRotationOffset + location.landed.rotation.eulerZ, normalized) * identity;
		configNode.AddValue("rot", identity);
		if (flag)
		{
			configNode.SetValue("skipGroundPositioning", newValue: false);
		}
		else
		{
			configNode.SetValue("skipGroundPositioning", newValue: true);
		}
		if (location.landed.targetBody.pqsController != null)
		{
			configNode.SetValue("PQSMin", 0, createIfNotFound: true);
			configNode.SetValue("PQSMax", 0, createIfNotFound: true);
		}
		return configNode;
	}

	public ConfigNode CreateKerbalPartNode(uint id, params ProtoCrewMember[] crew)
	{
		ConfigNode configNode = ProtoVessel.CreatePartNode(crew[0].GetKerbalEVAPartName(), id, crew);
		configNode.SetValue("flag", node.mission.flagURL, createIfNotFound: true);
		return configNode;
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
		persistentId = FlightGlobals.GetUniquepersistentId();
		missionKerbal.Kerbal = null;
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

	public void OnNeckRingControlCreated(MEGUIParameterCheckbox control)
	{
		neckRingCheckbox = control;
		if (isHelmetEnabled)
		{
			neckRingCheckbox.FieldValue = true;
			neckRingCheckbox.IsInteractable = false;
			neckRingCheckbox.RefreshUI();
		}
	}

	public void OnHelmetValueChange(bool newValue)
	{
		if (newValue)
		{
			neckRingCheckbox.FieldValue = true;
			neckRingCheckbox.IsInteractable = false;
			neckRingCheckbox.RefreshUI();
		}
		else
		{
			neckRingCheckbox.IsInteractable = true;
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004008");
	}

	public void NodeDeleted()
	{
		if (missionKerbal != null)
		{
			missionKerbal.setcurrentKerbalAvailable();
		}
	}

	public void KerbalRosterStatusChange(ProtoCrewMember kerbal, ProtoCrewMember.RosterStatus oldStatus, ProtoCrewMember.RosterStatus newStatus)
	{
		if (missionKerbal != null)
		{
			missionKerbal.onKerbalStatusChange(kerbal, oldStatus, newStatus);
		}
	}

	public void KerbalAdded(ProtoCrewMember kerbal)
	{
		if (missionKerbal != null)
		{
			missionKerbal.onKerbalAdded(kerbal);
		}
	}

	public void KerbalTypeChange(ProtoCrewMember kerbal, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType)
	{
		if (missionKerbal != null)
		{
			missionKerbal.onKerbalTypeChange(kerbal, oldType, newType);
		}
	}

	public void KerbalNameChange(ProtoCrewMember kerbal, string oldName, string newName)
	{
		if (missionKerbal != null)
		{
			missionKerbal.onKerbalNameChange(kerbal, oldName, newName);
		}
	}

	public void KerbalRemoved(ProtoCrewMember kerbal)
	{
		if (missionKerbal != null)
		{
			missionKerbal.onKerbalRemoved(kerbal);
		}
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "location")
		{
			return location.GetNodeBodyParameterString();
		}
		if (field.name == "missionKerbal")
		{
			return missionKerbal.GetNodeBodyParameterString();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		missionKerbal.Save(node);
		node.AddValue("isStranded", isStranded);
		node.AddValue("isHelmetEnabled", isHelmetEnabled);
		node.AddValue("isNeckRingEnabled", isNeckRingEnabled);
		node.AddValue("persistentId", persistentId);
		location.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		location.Load(node);
		if (missionKerbal == null)
		{
			missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
		}
		missionKerbal.Load(node);
		node.TryGetValue("isStranded", ref isStranded);
		node.TryGetValue("isHelmetEnabled", ref isHelmetEnabled);
		node.TryGetValue("isNeckRingEnabled", ref isNeckRingEnabled);
		node.TryGetValue("persistentId", ref persistentId);
	}
}
