using System;
using System.Collections.Generic;
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
public class TestTakeKerbal : TestModule, IScoreableObjective, INodeWaypoint, ITestNodeLabel
{
	public enum TestType
	{
		AnyVesselAnyKerbal,
		AnyVesselSpecificKerbal,
		SpecificVesselAnyKerbal,
		SpecificVesselSpecificKerbal
	}

	[MEGUI_MissionKerbal(statusToShow = ProtoCrewMember.RosterStatus.Available, order = 10, showStranded = false, showAllRosterStatus = true, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	[MEGUI_ParameterSwitchCompound(order = 20, excludeParamFields = "anyData", guiName = "#autoLOC_8000149")]
	public ParamChoices_CelestialBodySurface locationSituation;

	[MEGUI_VesselSelect(order = 30, resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8000074")]
	public uint vesselID;

	public List<ProtoCrewMember> pcm;

	public bool testPassed;

	public Vessel targetVessel;

	public string currentBiome;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000174");
		missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI, ProtoCrewMember.KerbalType.Tourist);
		locationSituation = new ParamChoices_CelestialBodySurface();
		locationSituation.bodyData = new MissionCelestialBody(FlightGlobals.GetHomeBody());
		locationSituation.biomeData = new MissionBiome(FlightGlobals.GetHomeBody(), "");
		locationSituation.areaData = new SurfaceArea(FlightGlobals.GetHomeBody(), 0.0, 0.0, 150000f);
		locationSituation.launchSiteName = "";
		pcm = new List<ProtoCrewMember>();
		targetVessel = null;
	}

	public override void Initialize(TestGroup testGroup, string title)
	{
		base.Initialize(testGroup, title);
		missionKerbal.Initialize(null, testGroup.node);
	}

	public override void Initialized()
	{
		testPassed = false;
		if (vesselID != 0)
		{
			if (!missionKerbal.AnyValid)
			{
				GameEvents.onVesselCrewWasModified.Add(onVesselCrewWasModified);
			}
			uint key = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(key))
			{
				CheckCrew(FlightGlobals.PersistentVesselIds[key]);
			}
			return;
		}
		if (!missionKerbal.AnyValid)
		{
			GameEvents.onVesselCrewWasModified.Add(onVesselCrewWasModified);
			GameEvents.onVesselChange.Add(onVesselChange);
			scanVessels();
			return;
		}
		GameEvents.onVesselChange.Add(onVesselChange);
		List<Vessel> vessels = FlightGlobals.Vessels;
		for (int i = 0; i < vessels.Count; i++)
		{
			testLocation(vessels[i]);
			if (testPassed)
			{
				CheckCrew(vessels[i]);
				if (targetVessel != null && targetVessel == vessels[i])
				{
					testPassed = true;
				}
				else
				{
					testPassed = false;
				}
			}
		}
		targetVessel = FlightGlobals.ActiveVessel;
	}

	public override void Cleared()
	{
		GameEvents.onVesselCrewWasModified.Remove(onVesselCrewWasModified);
		GameEvents.onVesselChange.Remove(onVesselChange);
	}

	public void onVesselChange(Vessel v)
	{
		targetVessel = FlightGlobals.ActiveVessel;
	}

	public void scanVessels()
	{
		targetVessel = null;
		List<Vessel> vessels = FlightGlobals.Vessels;
		for (int i = 0; i < vessels.Count; i++)
		{
			CheckCrew(vessels[i]);
		}
	}

	public void onVesselCrewWasModified(Vessel v)
	{
		CheckCrew(v);
	}

	public bool CheckCrew(Vessel v)
	{
		if (v != null)
		{
			pcm = v.GetVesselCrew();
			int count = pcm.Count;
			while (count-- > 0)
			{
				if (missionKerbal.IsValid(pcm[count]))
				{
					targetVessel = v;
					return true;
				}
			}
		}
		return false;
	}

	public bool targetVesselStillValid(Vessel vessel)
	{
		return CheckCrew(vessel);
	}

	public void testLocation(Vessel vessel)
	{
		switch (locationSituation.locationChoice)
		{
		case ParamChoices_CelestialBodySurface.Choices.bodyData:
			if (locationSituation.bodyData.IsValid(vessel.mainBody) && targetVesselStillValid(vessel))
			{
				testPassed = true;
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.biomeData:
			currentBiome = ScienceUtil.GetExperimentBiome(vessel.mainBody, vessel.latitude, vessel.longitude);
			if ((vessel.Landed || vessel.Splashed) && (string.IsNullOrEmpty(locationSituation.biomeData.biomeName) || currentBiome.Equals(locationSituation.biomeData.biomeName, StringComparison.InvariantCultureIgnoreCase)) && targetVesselStillValid(vessel))
			{
				testPassed = true;
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.areaData:
			if (locationSituation.areaData.body == vessel.mainBody && (vessel.Landed || vessel.Splashed) && targetVesselStillValid(vessel))
			{
				testPassed = locationSituation.areaData.IsPointInCircle(vessel.latitude, vessel.longitude);
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.launchSiteName:
			testPassed = locationSituation.launchSiteName == vessel.landedAt;
			break;
		}
	}

	public override bool Test()
	{
		if (targetVessel != null)
		{
			if (vesselID != 0 && targetVessel.persistentId != vesselID)
			{
				return testPassed;
			}
			testLocation(targetVessel);
		}
		return testPassed;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004033");
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

	public bool HasNodeWaypoint()
	{
		return locationSituation.HasWaypoint();
	}

	public Waypoint GetNodeWaypoint()
	{
		Waypoint waypoint = locationSituation.GetWaypoint();
		if (waypoint == null)
		{
			return null;
		}
		waypoint.name = title;
		return waypoint;
	}

	public bool HasNodeLabel()
	{
		return locationSituation.HasNodeLabel();
	}

	public bool HasWorldPosition()
	{
		return locationSituation.HasWorldPosition();
	}

	public Vector3 GetWorldPosition()
	{
		return locationSituation.GetWorldPosition();
	}

	public string GetExtraText()
	{
		return locationSituation.GetExtraText();
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		missionKerbal.Save(node);
		locationSituation.Save(node);
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
		locationSituation.Load(node);
		node.TryGetValue("vesselID", ref vesselID);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			if (!(targetVessel == null))
			{
				return targetVessel;
			}
			return FlightGlobals.ActiveVessel;
		}
		return null;
	}
}
