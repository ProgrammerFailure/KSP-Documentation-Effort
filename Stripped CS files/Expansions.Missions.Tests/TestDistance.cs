using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Actions;
using Expansions.Missions.Editor;
using Expansions.Missions.Runtime;
using FinePrint.Utilities;
using ns9;
using TMPro;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestDistance : TestModule, IScoreableObjective
{
	public enum DistanceFromChoices
	{
		[Description("#autoLOC_7000020")]
		Kerbal,
		[Description("#autoLOC_8000001")]
		Vessel
	}

	public enum DistanceToChoices
	{
		[Description("#autoLOC_8000046")]
		Asteroid,
		[Description("#autoLOC_6005065")]
		Comet,
		[Description("#autoLOC_8000263")]
		CelestialBody,
		[Description("#autoLoc_6002179")]
		Flag,
		[Description("#autoLOC_7000020")]
		Kerbal,
		[Description("#autoLOC_8000067")]
		LaunchSite,
		[Description("#autoLOC_8004145")]
		Location,
		[Description("#autoLOC_8004199")]
		NodeLabelNode,
		[Description("#autoLOC_8000001")]
		Vessel
	}

	public enum DistanceCalculationType
	{
		[Description("#autoLOC_8004207")]
		StraightLine,
		[Description("#autoLOC_8004208")]
		GreatCircle
	}

	[MEGUI_Dropdown(onDropDownValueChange = "OnDistanceFromValueChange", onControlCreated = "OnDistanceFromTargetControlCreated", gapDisplay = false, guiName = "#autoLOC_8004196", Tooltip = "#autoLOC_8004197")]
	public DistanceFromChoices distanceFromTarget = DistanceFromChoices.Vessel;

	public MEGUIParameterDropdownList distanceFromTargetParameterReference;

	[MEGUI_MissionKerbal(onControlCreated = "OnDistanceFromKerbalControlCreated", statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, canBePinned = false, showAllRosterStatus = true, hideOnSetup = true, guiName = "#autoLOC_7000020")]
	public MissionKerbal distanceFromKerbal;

	public MEGUIParameterMissionKerbal distanceFromKerbalParameterReference;

	public Vessel distanceFromKerbalVessel;

	[MEGUI_VesselSelect(onControlCreated = "OnDistanceFromVesselControlCreated", resetValue = "0", canBePinned = false, gapDisplay = true, hideOnSetup = true, addDefaultOption = true, defaultOptionIsActiveVessel = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8004198")]
	public uint distanceFromVesselID;

	public MEGUIParameterVesselDropdownList distanceFromVesselParameterReference;

	public Vessel distanceToKerbalVessel;

	[MEGUI_Dropdown(onDropDownValueChange = "OnDistanceToValueChange", onControlSetupComplete = "OnDistanceToTargetControlSetup", onControlCreated = "OnDistanceToTargetControlCreated", gapDisplay = false, guiName = "#autoLOC_8004200", Tooltip = "#autoLOC_8004201")]
	public DistanceToChoices distanceToTarget = DistanceToChoices.Vessel;

	public MEGUIParameterDropdownList distanceToTargetParameterReference;

	[MEGUI_MissionKerbal(onControlCreated = "OnDistanceToKerbalControlCreated", statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, canBePinned = false, showAllRosterStatus = true, hideOnSetup = true, guiName = "#autoLOC_7000020")]
	public MissionKerbal distanceToKerbal;

	public MEGUIParameterMissionKerbal distanceToKerbalParameterReference;

	[MEGUI_VesselSelect(resetValue = "0", addDefaultOption = false, canBePinned = false, onControlCreated = "OnDistanceToVesselControlCreated", hideOnSetup = true, gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8004211")]
	public uint distanceToVesselID;

	public MEGUIParameterVesselDropdownList distanceToVesselParameterReference;

	[MEGUI_VesselGroundLocation(DisableRotationX = true, onControlCreated = "OnDistanceToLocationControlCreated", DisableRotationZ = true, DisableRotationY = true, canBePinned = false, gapDisplay = true, hideOnSetup = true, guiName = "#autoLOC_8004145")]
	public VesselGroundLocation distanceToLocation;

	public MEGUIParameterCelestialBody_VesselGroundLocation distanceToLocationParameterReference;

	[MEGUI_CelestialBody(gapDisplay = true, canBePinned = false, onControlCreated = "OnDistanceToCelestialBodyControlCreated", resetValue = "0", showAnySOIoption = true, guiName = "#autoLOC_8000263")]
	public MissionCelestialBody distanceToCelestialBody;

	public MEGUIParameterCelestialBody distanceToCelestialBodyParameterReference;

	[MEGUI_AsteroidSelect(canBePinned = false, onControlCreated = "OnDistanceToAsteroidControlCreated", resetValue = "0", guiName = "#autoLOC_8000046", Tooltip = "#autoLOC_8004202")]
	public uint distanceToAsteroidID;

	[MEGUI_CometSelect(canBePinned = false, onControlCreated = "OnDistanceToCometControlCreated", resetValue = "0", guiName = "#autoLOC_6005065", Tooltip = "#autoLOC_8005447")]
	public uint distanceToCometID;

	public MEGUIParameterAsteroidDropdownList distanceToAsteroidParameterReference;

	public MEGUIParameterCometDropdownList distanceToCometParameterReference;

	[MEGUI_FlagSelect(canBePinned = false, onControlCreated = "OnDistanceToFlagControlCreated", resetValue = "0", guiName = "#autoLoc_6002179", Tooltip = "#autoLOC_8004203")]
	public uint distanceToFlagID;

	public MEGUIParameterFlagDropdownList distanceToFlagParameterReference;

	[MEGUI_LaunchSiteSelect(canBePinned = false, onControlCreated = "OnDistanceToLaunchSiteControlCreated", resetValue = "LaunchPad", guiName = "#autoLOC_8000067", Tooltip = "#autoLOC_8004204")]
	public string distanceToLaunchSiteName = "LaunchPad";

	public MEGUIParameterLaunchSiteDropdownList distanceToLaunchSiteParameterReference;

	public PSystemSetup.SpaceCenterFacility distanceToFacility;

	public LaunchSite distanceToLaunchSite;

	[MEGUI_NodeLabelNodeSelect(canBePinned = false, onControlCreated = "OnDistanceToNodeControlCreated", guiName = "#autoLOC_8004199", Tooltip = "#autoLOC_8004205")]
	public Guid distanceToNode = Guid.Empty;

	public MEGUIParameterNodeLabelNodeDropdownList distanceToNodeParameterReference;

	public ITestNodeLabel distanceToNodeLabelInterface;

	[MEGUI_Dropdown(canBePinned = true, resetValue = "StraightLine", canBeReset = true, guiName = "#autoLOC_8004209", Tooltip = "#autoLOC_8004210")]
	public DistanceCalculationType calculationType;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "LessThan", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8100017", Tooltip = "#autoLOC_8004206")]
	public double distance;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000176");
		distanceFromKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI, ProtoCrewMember.KerbalType.Crew, showAnyKerbal: false, anyTextIsActiveKerbal: true);
		distanceToKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI, ProtoCrewMember.KerbalType.Crew, showAnyKerbal: false);
		distanceToLocation = new VesselGroundLocation(base.node, VesselGroundLocation.GizmoIcon.Flag);
		distanceToCelestialBody = new MissionCelestialBody(FlightGlobals.GetHomeBody());
		if (base.node != null)
		{
			base.node.UpdateInfoOnRefresh = true;
		}
	}

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (distanceFromVesselID == oldId)
		{
			distanceFromVesselID = newId;
		}
		if (distanceToVesselID == oldId)
		{
			distanceToVesselID = newId;
		}
		if (distanceToAsteroidID == oldId)
		{
			distanceToAsteroidID = newId;
		}
		if (distanceToCometID == oldId)
		{
			distanceToCometID = newId;
		}
		if (distanceToFlagID == oldId)
		{
			distanceToFlagID = newId;
		}
	}

	public void OnDistanceFromTargetControlCreated(MEGUIParameterDropdownList parameter)
	{
		distanceFromTargetParameterReference = parameter;
	}

	public void OnDistanceFromKerbalControlCreated(MEGUIParameterMissionKerbal parameter)
	{
		distanceFromKerbalParameterReference = parameter;
	}

	public void OnDistanceFromVesselControlCreated(MEGUIParameterVesselDropdownList parameter)
	{
		distanceFromVesselParameterReference = parameter;
	}

	public void OnDistanceFromValueChange(MEGUIParameterDropdownList sender, int newIndex)
	{
		if (sender.SelectedValue != null && base.node != null)
		{
			switch ((DistanceFromChoices)sender.SelectedValue)
			{
			case DistanceFromChoices.Vessel:
				if (distanceFromKerbalParameterReference != null)
				{
					distanceFromKerbalParameterReference.gameObject.SetActive(value: false);
				}
				if (distanceFromVesselParameterReference != null)
				{
					distanceFromVesselParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceFromChoices.Kerbal:
				if (distanceFromKerbalParameterReference != null)
				{
					distanceFromKerbalParameterReference.gameObject.SetActive(value: true);
				}
				if (distanceFromVesselParameterReference != null)
				{
					distanceFromVesselParameterReference.gameObject.SetActive(value: false);
				}
				break;
			}
		}
		UpdateNodeBodyUI();
	}

	public void OnDistanceToTargetControlCreated(MEGUIParameterDropdownList parameter)
	{
		distanceToTargetParameterReference = parameter;
	}

	public void OnDistanceToTargetControlSetup(MEGUIParameterDropdownList parameter)
	{
	}

	public int SortDropDownItemByText(TMP_Dropdown.OptionData a, TMP_Dropdown.OptionData b)
	{
		return a.text.CompareTo(b.text);
	}

	public void OnDistanceToKerbalControlCreated(MEGUIParameterMissionKerbal parameter)
	{
		distanceToKerbalParameterReference = parameter;
	}

	public void OnDistanceToVesselControlCreated(MEGUIParameterVesselDropdownList parameter)
	{
		distanceToVesselParameterReference = parameter;
	}

	public void OnDistanceToLocationControlCreated(MEGUIParameterCelestialBody_VesselGroundLocation parameter)
	{
		distanceToLocationParameterReference = parameter;
	}

	public void OnDistanceToCelestialBodyControlCreated(MEGUIParameterCelestialBody parameter)
	{
		distanceToCelestialBodyParameterReference = parameter;
	}

	public void OnDistanceToAsteroidControlCreated(MEGUIParameterAsteroidDropdownList parameter)
	{
		distanceToAsteroidParameterReference = parameter;
	}

	public void OnDistanceToCometControlCreated(MEGUIParameterCometDropdownList parameter)
	{
		distanceToCometParameterReference = parameter;
	}

	public void OnDistanceToFlagControlCreated(MEGUIParameterFlagDropdownList parameter)
	{
		distanceToFlagParameterReference = parameter;
	}

	public void OnDistanceToLaunchSiteControlCreated(MEGUIParameterLaunchSiteDropdownList parameter)
	{
		distanceToLaunchSiteParameterReference = parameter;
	}

	public void OnDistanceToNodeControlCreated(MEGUIParameterNodeLabelNodeDropdownList parameter)
	{
		distanceToNodeParameterReference = parameter;
	}

	public void OnDistanceToValueChange(MEGUIParameterDropdownList sender, int newIndex)
	{
		if (distanceToKerbalParameterReference != null)
		{
			distanceToKerbalParameterReference.gameObject.SetActive(value: false);
		}
		if (distanceToVesselParameterReference != null)
		{
			distanceToVesselParameterReference.gameObject.SetActive(value: false);
		}
		if (distanceToLocationParameterReference != null)
		{
			distanceToLocationParameterReference.gameObject.SetActive(value: false);
		}
		if (distanceToCelestialBodyParameterReference != null)
		{
			distanceToCelestialBodyParameterReference.gameObject.SetActive(value: false);
		}
		if (distanceToAsteroidParameterReference != null)
		{
			distanceToAsteroidParameterReference.gameObject.SetActive(value: false);
		}
		if (distanceToCometParameterReference != null)
		{
			distanceToCometParameterReference.gameObject.SetActive(value: false);
		}
		if (distanceToFlagParameterReference != null)
		{
			distanceToFlagParameterReference.gameObject.SetActive(value: false);
		}
		if (distanceToLaunchSiteParameterReference != null)
		{
			distanceToLaunchSiteParameterReference.gameObject.SetActive(value: false);
		}
		if (distanceToNodeParameterReference != null)
		{
			distanceToNodeParameterReference.gameObject.SetActive(value: false);
		}
		if (sender.SelectedValue != null && base.node != null)
		{
			switch ((DistanceToChoices)sender.SelectedValue)
			{
			case DistanceToChoices.Asteroid:
				if (distanceToAsteroidParameterReference != null)
				{
					distanceToAsteroidParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceToChoices.Comet:
				if (distanceToCometParameterReference != null)
				{
					distanceToCometParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceToChoices.CelestialBody:
				if (distanceToCelestialBodyParameterReference != null)
				{
					distanceToCelestialBodyParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceToChoices.Flag:
				if (distanceToFlagParameterReference != null)
				{
					distanceToFlagParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceToChoices.Kerbal:
				if (distanceToKerbalParameterReference != null)
				{
					distanceToKerbalParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceToChoices.LaunchSite:
				if (distanceToLaunchSiteParameterReference != null)
				{
					distanceToLaunchSiteParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceToChoices.Location:
				if (distanceToLocationParameterReference != null)
				{
					distanceToLocationParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceToChoices.NodeLabelNode:
				if (distanceToNodeParameterReference != null)
				{
					distanceToNodeParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case DistanceToChoices.Vessel:
				if (distanceToVesselParameterReference != null)
				{
					distanceToVesselParameterReference.gameObject.SetActive(value: true);
				}
				break;
			}
		}
		UpdateNodeBodyUI();
	}

	public override void Initialize(TestGroup testGroup, string title)
	{
		base.Initialize(testGroup, title);
		distanceFromKerbal.Initialize(null, testGroup.node);
		distanceToKerbal.Initialize(null, testGroup.node);
	}

	public override void ParameterSetupComplete()
	{
		OnDistanceFromValueChange(distanceFromTargetParameterReference, distanceFromTargetParameterReference.FieldValue);
		OnDistanceToValueChange(distanceToTargetParameterReference, distanceToTargetParameterReference.FieldValue);
	}

	public override bool Test()
	{
		bool result = false;
		if (FlightGlobals.ActiveVessel != null)
		{
			Vector3d location = Vector3d.zero;
			bool flag = false;
			CelestialBody celestialBody = null;
			switch (distanceFromTarget)
			{
			case DistanceFromChoices.Vessel:
				flag = GetVesselLocation(distanceFromVesselID, ref location, ref celestialBody);
				break;
			case DistanceFromChoices.Kerbal:
				flag = GetKerbalLocation(distanceFromKerbal, ref distanceFromKerbalVessel, ref location, ref celestialBody);
				break;
			}
			Vector3d location2 = Vector3d.zero;
			bool flag2 = false;
			CelestialBody celestialBody2 = null;
			switch (distanceToTarget)
			{
			case DistanceToChoices.Asteroid:
				if (distanceToAsteroidID != 0)
				{
					flag2 = GetVesselLocation(distanceToAsteroidID, ref location2, ref celestialBody2);
				}
				break;
			case DistanceToChoices.Comet:
				if (distanceToCometID != 0)
				{
					flag2 = GetVesselLocation(distanceToCometID, ref location2, ref celestialBody2);
				}
				break;
			case DistanceToChoices.CelestialBody:
			{
				CelestialBody celestialBody3 = distanceToCelestialBody.Body;
				if (celestialBody3 == null)
				{
					celestialBody3 = celestialBody;
				}
				location2 = celestialBody3.position + (location - celestialBody3.position).normalized * celestialBody3.Radius;
				flag2 = true;
				break;
			}
			case DistanceToChoices.Flag:
				if (distanceToFlagID != 0)
				{
					flag2 = GetVesselLocation(distanceToFlagID, ref location2, ref celestialBody2);
				}
				break;
			case DistanceToChoices.Kerbal:
				flag2 = GetKerbalLocation(distanceToKerbal, ref distanceToKerbalVessel, ref location2, ref celestialBody2);
				break;
			case DistanceToChoices.LaunchSite:
				if (distanceToFacility == null && distanceToLaunchSite == null)
				{
					distanceToFacility = PSystemSetup.Instance.GetSpaceCenterFacility(distanceToLaunchSiteName);
				}
				if (distanceToFacility != null)
				{
					location2 = distanceToFacility.facilityTransform.position;
					flag2 = true;
				}
				if (!flag2)
				{
					if (distanceToFacility == null && distanceToLaunchSite == null)
					{
						distanceToLaunchSite = PSystemSetup.Instance.GetLaunchSite(distanceToLaunchSiteName);
					}
					if (distanceToLaunchSite != null)
					{
						location2 = distanceToLaunchSite.GetWorldPos().position;
						flag2 = true;
					}
				}
				break;
			case DistanceToChoices.Location:
				location2 = distanceToLocation.GetWorldPosition();
				flag2 = true;
				break;
			case DistanceToChoices.NodeLabelNode:
				if (distanceToNodeLabelInterface == null && distanceToNode != Guid.Empty && MissionSystem.missions[0].nodes.Contains(distanceToNode))
				{
					MENode mENode = MissionSystem.missions[0].nodes[distanceToNode];
					for (int i = 0; i < mENode.testGroups.Count; i++)
					{
						if (distanceToNodeLabelInterface != null)
						{
							break;
						}
						for (int j = 0; j < mENode.testGroups[i].testModules.Count; j++)
						{
							if (distanceToNodeLabelInterface != null)
							{
								break;
							}
							distanceToNodeLabelInterface = mENode.testGroups[i].testModules[j] as ITestNodeLabel;
						}
					}
				}
				if (distanceToNodeLabelInterface != null && distanceToNodeLabelInterface.HasWorldPosition())
				{
					location2 = distanceToNodeLabelInterface.GetWorldPosition();
					flag2 = true;
				}
				break;
			case DistanceToChoices.Vessel:
				flag2 = GetVesselLocation(distanceToVesselID, ref location2, ref celestialBody2);
				break;
			}
			if (!flag || !flag2)
			{
				return false;
			}
			result = TestPointsDistance(location, location2, celestialBody);
		}
		return result;
	}

	public bool TestPointsDistance(Vector3d pointA, Vector3d pointB, CelestialBody body)
	{
		double num = 0.0;
		switch (calculationType)
		{
		case DistanceCalculationType.GreatCircle:
			if (body == null)
			{
				return false;
			}
			num = CelestialUtilities.GreatCircleDistance(body, pointA, pointB);
			break;
		case DistanceCalculationType.StraightLine:
			num = Vector3d.Distance(pointA, pointB);
			break;
		}
		bool result = false;
		switch (comparisonOperator)
		{
		case TestComparisonLessGreaterOnly.GreaterThan:
			result = num >= distance;
			break;
		case TestComparisonLessGreaterOnly.LessThan:
			result = num <= distance;
			break;
		}
		return result;
	}

	public bool GetVesselLocation(uint vesselID, ref Vector3d location, ref CelestialBody celestialBody)
	{
		if (vesselID != 0)
		{
			vesselID = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (!FlightGlobals.PersistentVesselIds.ContainsKey(vesselID))
			{
				return false;
			}
		}
		Vessel vessel = ((vesselID == 0) ? FlightGlobals.ActiveVessel : FlightGlobals.PersistentVesselIds[vesselID]);
		if (vessel != null)
		{
			location = vessel.GetWorldPos3D();
			celestialBody = vessel.mainBody;
			return true;
		}
		return false;
	}

	public bool GetKerbalLocation(MissionKerbal missionKerbal, ref Vessel currentVessel, ref Vector3d location, ref CelestialBody celestialBody)
	{
		if (!CheckCrew(currentVessel, missionKerbal) || (missionKerbal.Kerbal == null && currentVessel != FlightGlobals.ActiveVessel))
		{
			currentVessel = null;
		}
		if (currentVessel == null)
		{
			if (missionKerbal.Kerbal == null)
			{
				Vessel activeVessel = FlightGlobals.ActiveVessel;
				if (CheckCrew(activeVessel, missionKerbal))
				{
					currentVessel = activeVessel;
				}
			}
			else
			{
				List<Vessel> vessels = FlightGlobals.Vessels;
				for (int i = 0; i < vessels.Count; i++)
				{
					if (!(currentVessel == null))
					{
						break;
					}
					if (CheckCrew(vessels[i], missionKerbal))
					{
						currentVessel = vessels[i];
					}
				}
			}
		}
		if (currentVessel != null)
		{
			location = currentVessel.GetWorldPos3D();
			celestialBody = currentVessel.mainBody;
			return true;
		}
		return false;
	}

	public bool CheckCrew(Vessel v, MissionKerbal missionKerbal)
	{
		if (v == null)
		{
			return false;
		}
		List<ProtoCrewMember> vesselCrew = v.GetVesselCrew();
		int count = vesselCrew.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!missionKerbal.IsValid(vesselCrew[count]));
		return true;
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (distanceFromTarget == DistanceFromChoices.Kerbal && distanceToTarget == DistanceToChoices.Kerbal && distanceFromKerbal.Name == distanceToKerbal.Name)
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8004219"));
		}
		if (distanceFromTarget == DistanceFromChoices.Vessel && distanceToTarget == DistanceToChoices.Vessel && distanceFromVesselID == distanceToVesselID)
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8004220"));
		}
		if (distanceFromTarget == DistanceFromChoices.Kerbal)
		{
			if (distanceFromKerbal.Kerbal != null && base.node.mission.situation.crewRoster[distanceFromKerbal.Kerbal.name].rosterStatus != ProtoCrewMember.RosterStatus.Assigned)
			{
				validator.AddNodeWarn(base.node, Localizer.Format("#autoLOC_8002033", distanceFromKerbal.Name));
			}
			if (distanceFromKerbal == null && distanceFromKerbal.TypeToShow == ProtoCrewMember.KerbalType.Tourist)
			{
				validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8004221"));
			}
		}
		if (distanceToTarget == DistanceToChoices.Kerbal)
		{
			if (distanceToKerbal.Kerbal != null && base.node.mission.situation.crewRoster[distanceToKerbal.Kerbal.name].rosterStatus != ProtoCrewMember.RosterStatus.Assigned)
			{
				validator.AddNodeWarn(base.node, Localizer.Format("#autoLOC_8002033", distanceToKerbal.Name));
			}
			if (distanceToKerbal == null)
			{
				validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8004222"));
			}
		}
		if (distanceToTarget == DistanceToChoices.Asteroid && distanceToAsteroidID == 0)
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8004223"));
		}
		if (distanceToTarget == DistanceToChoices.Comet && distanceToCometID == 0)
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8005448"));
		}
		if (distanceToTarget == DistanceToChoices.Flag && distanceToFlagID == 0)
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8004224"));
		}
		if (distanceToTarget == DistanceToChoices.NodeLabelNode && !MissionEditorLogic.Instance.EditorMission.nodes.ContainsKey(distanceToNode))
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8004225"));
		}
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (base.node != null)
		{
			base.node.UpdateInfoOnRefresh = true;
		}
		if (field.name == "distanceFromTarget" && base.node.mission != null)
		{
			string text = base.GetNodeBodyParameterString(field) + "\n";
			switch (distanceFromTarget)
			{
			case DistanceFromChoices.Vessel:
			{
				string text2 = "";
				string vesselName = "";
				bool flag = false;
				VesselSituation vesselSituation = base.node.mission.GetVesselSituationByVesselID(distanceFromVesselID);
				if (vesselSituation != null)
				{
					vesselName = vesselSituation.vesselName;
				}
				else if (base.node.mission.UpdateFromMappedVesselIDs(distanceFromVesselID, ref vesselName, ref vesselSituation))
				{
					flag = true;
				}
				text2 = ((vesselSituation != null || flag) ? vesselName : ((distanceFromVesselID != 0) ? Localizer.Format("#autoLOC_8100159") : Localizer.Format("#autoLOC_8004217")));
				string text3 = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000001"), text2);
				if (flag && vesselSituation != null)
				{
					text3 = text3 + "\n" + Localizer.Format("#autoLOC_6002491", vesselSituation.vesselName) + "\n";
				}
				return text3;
			}
			case DistanceFromChoices.Kerbal:
				return text + distanceFromKerbal.GetNodeBodyParameterString();
			}
		}
		if (field.name == "distanceToTarget")
		{
			string text4 = base.GetNodeBodyParameterString(field) + "\n";
			switch (distanceToTarget)
			{
			case DistanceToChoices.Asteroid:
			{
				string text10 = "";
				Asteroid asteroidByPersistentID = base.node.mission.GetAsteroidByPersistentID(distanceToAsteroidID);
				text10 = ((asteroidByPersistentID != null) ? asteroidByPersistentID.name : Localizer.Format("#autoLOC_6003000"));
				return text4 + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000046"), text10);
			}
			case DistanceToChoices.Comet:
			{
				string text9 = "";
				Comet cometByPersistentID = base.node.mission.GetCometByPersistentID(distanceToCometID);
				text9 = ((cometByPersistentID != null) ? Localizer.Format(cometByPersistentID.name) : Localizer.Format("#autoLOC_6003000"));
				return text4 + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_6005065"), text9);
			}
			case DistanceToChoices.CelestialBody:
				return text4 + distanceToCelestialBody.GetNodeBodyParameterString();
			case DistanceToChoices.Flag:
			{
				string text7 = "";
				ActionCreateFlag actionCreateFlagByPersistentID = base.node.mission.GetActionCreateFlagByPersistentID(distanceToFlagID);
				text7 = ((!(actionCreateFlagByPersistentID == null)) ? actionCreateFlagByPersistentID.siteName : Localizer.Format("#autoLOC_6003000"));
				return text4 + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLoc_6002179"), text7);
			}
			case DistanceToChoices.Kerbal:
				return text4 + distanceToKerbal.GetNodeBodyParameterString();
			case DistanceToChoices.LaunchSite:
				return text4 + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000067"), PSystemSetup.Instance.GetLaunchSiteDisplayName(distanceToLaunchSiteName));
			case DistanceToChoices.Location:
				return text4 + distanceToLocation.GetNodeBodyParameterString();
			case DistanceToChoices.NodeLabelNode:
			{
				string text8 = "";
				text8 = ((!base.node.mission.nodes.ContainsKey(distanceToNode)) ? Localizer.Format("#autoLOC_6003000") : base.node.mission.nodes[distanceToNode].Title);
				return text4 + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8004199"), text8);
			}
			case DistanceToChoices.Vessel:
			{
				string text5 = "";
				string vesselName2 = "";
				bool flag2 = false;
				VesselSituation vesselSituation2 = base.node.mission.GetVesselSituationByVesselID(distanceToVesselID);
				if (vesselSituation2 != null)
				{
					vesselName2 = vesselSituation2.vesselName;
				}
				else if (base.node.mission.UpdateFromMappedVesselIDs(distanceFromVesselID, ref vesselName2, ref vesselSituation2))
				{
					flag2 = true;
				}
				text5 = ((vesselSituation2 != null || flag2) ? vesselName2 : Localizer.Format("#autoLOC_8100159"));
				string text6 = text4 + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000001"), text5);
				if (flag2 && vesselSituation2 != null)
				{
					text6 = text6 + Localizer.Format("#autoLOC_6002491", vesselSituation2.vesselName) + "\n";
				}
				return text6;
			}
			}
		}
		if (field.name == "distance")
		{
			return Localizer.Format("#autoLOC_8004226", (comparisonOperator == TestComparisonLessGreaterOnly.LessThan) ? "<" : ">", distance.ToString());
		}
		if (field.name == "calculationType")
		{
			return base.GetNodeBodyParameterString(field);
		}
		return "";
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004141");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("distanceFromTarget", distanceFromTarget);
		node.AddValue("distanceFromVesselID", distanceFromVesselID);
		ConfigNode configNode = new ConfigNode("DISTANCEFROMKERBAL");
		distanceFromKerbal.Save(configNode);
		node.AddNode(configNode);
		node.AddValue("distanceToTarget", distanceToTarget);
		node.AddValue("distanceToVesselID", distanceToVesselID);
		distanceToKerbal.Save(node);
		ConfigNode configNode2 = new ConfigNode("DISTANCETOKERBAL");
		distanceToKerbal.Save(configNode2);
		node.AddNode(configNode2);
		ConfigNode configNode3 = new ConfigNode("DISTANCETOLOCATION");
		distanceToLocation.Save(configNode3);
		node.AddNode(configNode3);
		distanceToCelestialBody.Save(node);
		node.AddValue("distanceToAsteroidID", distanceToAsteroidID);
		node.AddValue("distanceToCometID", distanceToCometID);
		node.AddValue("distanceToFlagID", distanceToFlagID);
		node.AddValue("distanceToLaunchSiteName", distanceToLaunchSiteName);
		node.AddValue("distanceToNode", distanceToNode.ToString());
		node.AddValue("calculationType", calculationType);
		node.AddValue("comparisonOperator", comparisonOperator);
		node.AddValue("distance", distance);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (!node.TryGetEnum("distanceFromTarget", ref distanceFromTarget, DistanceFromChoices.Vessel))
		{
			Debug.LogError("Failed to parse distanceFromTarget from " + base.name);
		}
		node.TryGetValue("distanceFromVesselID", ref distanceFromVesselID);
		ConfigNode configNode = null;
		node.TryGetNode("DISTANCEFROMKERBAL", ref configNode);
		if (configNode != null)
		{
			distanceFromKerbal.Load(configNode);
		}
		if (!node.TryGetEnum("distanceToTarget", ref distanceToTarget, DistanceToChoices.Vessel))
		{
			Debug.LogError("Failed to parse distanceFromTarget from " + base.name);
		}
		node.TryGetValue("distanceToVesselID", ref distanceToVesselID);
		node.TryGetValue("distanceToLaunchSiteName", ref distanceToLaunchSiteName);
		ConfigNode configNode2 = null;
		node.TryGetNode("DISTANCETOKERBAL", ref configNode2);
		if (configNode2 != null)
		{
			distanceToKerbal.Load(configNode2);
		}
		ConfigNode configNode3 = null;
		node.TryGetNode("DISTANCETOLOCATION", ref configNode3);
		if (configNode2 != null)
		{
			distanceToLocation.Load(configNode3);
		}
		distanceToCelestialBody.Load(node);
		node.TryGetValue("distanceToAsteroidID", ref distanceToAsteroidID);
		node.TryGetValue("distanceToCometID", ref distanceToCometID);
		node.TryGetValue("distanceToFlagID", ref distanceToFlagID);
		node.TryGetValue("distanceToNode", ref distanceToNode);
		node.TryGetEnum("calculationType", ref calculationType, DistanceCalculationType.StraightLine);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterOnly.GreaterThan);
		node.TryGetValue("distance", ref distance);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			return FlightGlobals.ActiveVessel;
		}
		return null;
	}
}
