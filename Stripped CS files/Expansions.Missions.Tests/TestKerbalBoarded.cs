using System;
using Expansions.Missions.Editor;
using FinePrint;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestKerbalBoarded : TestModule, IScoreableObjective, INodeWaypoint, ITestNodeLabel
{
	[MEGUI_MissionKerbal(statusToShow = ProtoCrewMember.RosterStatus.Available, order = 10, showStranded = false, showAllRosterStatus = true, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	[MEGUI_ParameterSwitchCompound(order = 20, excludeParamFields = "launchsiteName", guiName = "#autoLOC_8000149")]
	public ParamChoices_CelestialBodySurface situation;

	[MEGUI_VesselSelect(order = 30, resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8000074")]
	public uint vesselID;

	public bool eventFound;

	public Vessel vessel;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000148");
		missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
		situation = new ParamChoices_CelestialBodySurface();
		situation.bodyData = new MissionCelestialBody(FlightGlobals.GetHomeBody());
		situation.biomeData = new MissionBiome(FlightGlobals.GetHomeBody(), "");
		situation.areaData = new SurfaceArea(FlightGlobals.GetHomeBody(), 0.0, 0.0, 150000f);
		situation.launchSiteName = "";
	}

	public override void Initialize(TestGroup testGroup, string title)
	{
		base.Initialize(testGroup, title);
		missionKerbal.Initialize(null, testGroup.node);
	}

	public override void Initialized()
	{
		eventFound = false;
		GameEvents.onCrewTransferred.Add(OnBoarded);
		GameEvents.onCommandSeatInteraction.Add(OnSeated);
	}

	public override void Cleared()
	{
		GameEvents.onCrewTransferred.Remove(OnBoarded);
		GameEvents.onCommandSeatInteraction.Remove(OnSeated);
	}

	public void OnBoarded(GameEvents.HostedFromToAction<ProtoCrewMember, Part> hostedFromTo)
	{
		if (hostedFromTo.from.vessel.isEVA && missionKerbal.IsValid(hostedFromTo.host))
		{
			vessel = hostedFromTo.to.vessel;
			CheckSituation();
		}
	}

	public void OnSeated(KerbalEVA eva, bool entered)
	{
		int num = 0;
		while (true)
		{
			if (num < eva.part.protoModuleCrew.Count)
			{
				if (missionKerbal.IsValid(eva.part.protoModuleCrew[num]))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		vessel = eva.vessel;
		CheckSituation();
	}

	public void CheckSituation()
	{
		if (vesselID != 0)
		{
			uint num = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (vessel.persistentId != num)
			{
				return;
			}
		}
		switch (situation.locationChoice)
		{
		default:
			eventFound = true;
			break;
		case ParamChoices_CelestialBodySurface.Choices.bodyData:
			if (situation.bodyData.IsValid(FlightGlobals.currentMainBody))
			{
				eventFound = true;
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.biomeData:
			if (!(FlightGlobals.currentMainBody != situation.biomeData.body))
			{
				string experimentBiome = ScienceUtil.GetExperimentBiome(FlightGlobals.currentMainBody, vessel.latitude, vessel.longitude);
				if (string.IsNullOrEmpty(situation.biomeData.biomeName) || experimentBiome.Equals(situation.biomeData.biomeName, StringComparison.InvariantCultureIgnoreCase))
				{
					eventFound = true;
				}
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.areaData:
			if (!(FlightGlobals.currentMainBody != situation.areaData.body))
			{
				eventFound = situation.areaData.IsPointInCircle(vessel.latitude, vessel.longitude);
			}
			break;
		}
	}

	public override bool Test()
	{
		return eventFound;
	}

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (vesselID == oldId)
		{
			vesselID = newId;
		}
	}

	public bool HasNodeWaypoint()
	{
		return situation.HasWaypoint();
	}

	public Waypoint GetNodeWaypoint()
	{
		Waypoint waypoint = situation.GetWaypoint();
		if (waypoint == null)
		{
			return null;
		}
		waypoint.name = title;
		return waypoint;
	}

	public bool HasNodeLabel()
	{
		return situation.HasNodeLabel();
	}

	public bool HasWorldPosition()
	{
		return situation.HasWorldPosition();
	}

	public Vector3 GetWorldPosition()
	{
		return situation.GetWorldPosition();
	}

	public string GetExtraText()
	{
		return situation.GetExtraText();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004017");
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
			VesselSituation vesselSituation = base.node.mission.GetVesselSituationByVesselID(vesselID);
			string vesselName = "";
			bool flag = false;
			if (vesselSituation != null)
			{
				vesselName = vesselSituation.vesselName;
			}
			else if (base.node.mission.UpdateFromMappedVesselIDs(vesselID, ref vesselName, ref vesselSituation))
			{
				flag = true;
			}
			if (vesselSituation == null && !flag)
			{
				return Localizer.Format("#autoLOC_8000069", Localizer.Format("#autoLOC_8001004"));
			}
			string text = Localizer.Format("#autoLOC_8000069", vesselName);
			if (flag && vesselSituation != null)
			{
				text = text + Localizer.Format("#autoLOC_6002491", vesselSituation.vesselName) + "\n";
			}
			return text;
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (missionKerbal.Kerbal != null && base.node.mission.situation.crewRoster[missionKerbal.Kerbal.name].rosterStatus != ProtoCrewMember.RosterStatus.Assigned)
		{
			validator.AddNodeWarn(base.node, Localizer.Format("#autoLOC_8002033", missionKerbal.Name));
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		missionKerbal.Save(node);
		situation.Save(node);
		node.AddValue("vesselID", vesselID);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (missionKerbal == null)
		{
			missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
		}
		missionKerbal.Load(node);
		situation.Load(node);
		node.TryGetValue("vesselID", ref vesselID);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			if (!(vessel == null))
			{
				return vessel;
			}
			return FlightGlobals.ActiveVessel;
		}
		return null;
	}
}
