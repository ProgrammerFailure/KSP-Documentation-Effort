using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestVesselVelocity : TestVessel
{
	public enum SpeedType
	{
		[Description("#autoLOC_8004167")]
		SurfaceVelocity,
		[Description("#autoLOC_8004168")]
		OrbitalVelocity
	}

	public enum VelocityReferenceFrame
	{
		[Description("#autoLOC_8005455")]
		Default,
		[Description("#autoLOC_8000046")]
		Asteroid,
		[Description("#autoLOC_6005065")]
		Comet,
		[Description("#autoLOC_7000020")]
		Kerbal,
		[Description("#autoLOC_8000001")]
		Vessel
	}

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8004169", Tooltip = "#autoLOC_8004170")]
	public float velocity;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator = TestComparisonLessGreaterOnly.GreaterThan;

	[MEGUI_Dropdown(onDropDownValueChange = "OnVelocityWRTBodyValueChange", onControlSetupComplete = "OnVelocityWRTBodyControlSetup", onControlCreated = "OnVelocityWRTBodyControlCreated", gapDisplay = false, guiName = "#autoLOC_8005449", Tooltip = "#autoLOC_8005450")]
	public VelocityReferenceFrame referenceBody;

	public MEGUIParameterDropdownList velocityWRTBodyParameterReference;

	[MEGUI_Dropdown(onControlSetupComplete = "OnSpeedTypeControlSetup", canBePinned = true, onControlCreated = "OnSpeedTypeControlCreated", resetValue = "SurfaceVelocity", canBeReset = true, guiName = "#autoLOC_8004171", Tooltip = "#autoLOC_8004172")]
	public SpeedType speedType;

	public MEGUIParameterDropdownList speedTypeParameterReference;

	[MEGUI_MissionKerbal(onControlCreated = "OnVelocityWRTKerbalControlCreated", statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, canBePinned = false, showAllRosterStatus = true, hideOnSetup = true, guiName = "#autoLOC_7000020")]
	public MissionKerbal velocityKerbal;

	public MEGUIParameterMissionKerbal kerbalVelocityReference;

	[MEGUI_VesselSelect(resetValue = "0", addDefaultOption = false, canBePinned = false, onControlCreated = "OnVelocityWRTVesselControlCreated", hideOnSetup = true, gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8005451")]
	public uint velocityVesselID;

	public MEGUIParameterVesselDropdownList vesselVelocityReference;

	[MEGUI_AsteroidSelect(canBePinned = false, onControlCreated = "OnVelocityWRTAsteroidControlCreated", resetValue = "0", guiName = "#autoLOC_8000046", Tooltip = "#autoLOC_8005452")]
	public uint velocityAsteroidID;

	[MEGUI_CometSelect(canBePinned = false, onControlCreated = "OnVelocityWRTCometControlCreated", resetValue = "0", guiName = "#autoLOC_6005065", Tooltip = "#autoLOC_8005453")]
	public uint velocityCometID;

	public MEGUIParameterAsteroidDropdownList velocityAsteroidParameterReference;

	public MEGUIParameterCometDropdownList velocityCometParameterReference;

	public Vessel vesselRefBody;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8004164");
		velocityKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI, ProtoCrewMember.KerbalType.Crew, showAnyKerbal: false);
		useActiveVessel = true;
	}

	public void OnVelocityWRTBodyControlCreated(MEGUIParameterDropdownList parameter)
	{
		velocityWRTBodyParameterReference = parameter;
	}

	public void OnVelocityWRTBodyControlSetup(MEGUIParameterDropdownList parameter)
	{
	}

	public void OnSpeedTypeControlCreated(MEGUIParameterDropdownList parameter)
	{
		speedTypeParameterReference = parameter;
	}

	public void OnSpeedTypeControlSetup(MEGUIParameterDropdownList parameter)
	{
	}

	public void OnVelocityWRTKerbalControlCreated(MEGUIParameterMissionKerbal parameter)
	{
		kerbalVelocityReference = parameter;
	}

	public void OnVelocityWRTVesselControlCreated(MEGUIParameterVesselDropdownList parameter)
	{
		vesselVelocityReference = parameter;
	}

	public void OnVelocityWRTAsteroidControlCreated(MEGUIParameterAsteroidDropdownList parameter)
	{
		velocityAsteroidParameterReference = parameter;
	}

	public void OnVelocityWRTCometControlCreated(MEGUIParameterCometDropdownList parameter)
	{
		velocityCometParameterReference = parameter;
	}

	public void OnVelocityWRTBodyValueChange(MEGUIParameterDropdownList sender, int newIndex)
	{
		if (kerbalVelocityReference != null)
		{
			kerbalVelocityReference.gameObject.SetActive(value: false);
		}
		if (vesselVelocityReference != null)
		{
			vesselVelocityReference.gameObject.SetActive(value: false);
		}
		if (speedTypeParameterReference != null)
		{
			speedTypeParameterReference.gameObject.SetActive(value: false);
		}
		if (velocityAsteroidParameterReference != null)
		{
			velocityAsteroidParameterReference.gameObject.SetActive(value: false);
		}
		if (velocityCometParameterReference != null)
		{
			velocityCometParameterReference.gameObject.SetActive(value: false);
		}
		if (sender.SelectedValue != null && base.node != null)
		{
			switch ((VelocityReferenceFrame)sender.SelectedValue)
			{
			default:
				if ((bool)speedTypeParameterReference)
				{
					speedTypeParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case VelocityReferenceFrame.Asteroid:
				if (velocityAsteroidParameterReference != null)
				{
					velocityAsteroidParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case VelocityReferenceFrame.Comet:
				if (velocityCometParameterReference != null)
				{
					velocityCometParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case VelocityReferenceFrame.Kerbal:
				if (kerbalVelocityReference != null)
				{
					kerbalVelocityReference.gameObject.SetActive(value: true);
				}
				break;
			case VelocityReferenceFrame.Vessel:
				if (vesselVelocityReference != null)
				{
					vesselVelocityReference.gameObject.SetActive(value: true);
				}
				break;
			}
		}
		UpdateNodeBodyUI();
	}

	public override void Initialize(TestGroup testGroup, string title)
	{
		base.Initialize(testGroup, title);
		velocityKerbal.Initialize(null, testGroup.node);
	}

	public override void ParameterSetupComplete()
	{
		OnVelocityWRTBodyValueChange(velocityWRTBodyParameterReference, velocityWRTBodyParameterReference.FieldValue);
	}

	public override bool Test()
	{
		base.Test();
		if (vessel == null)
		{
			return true;
		}
		double num = 0.0;
		Vector3 zero = Vector3.zero;
		switch (referenceBody)
		{
		default:
			if (speedType == SpeedType.SurfaceVelocity)
			{
				num = vessel.srfSpeed;
			}
			else if (speedType == SpeedType.OrbitalVelocity)
			{
				num = vessel.obt_speed;
			}
			break;
		case VelocityReferenceFrame.Asteroid:
			vesselRefBody = FlightGlobals.PersistentVesselIds[velocityAsteroidID];
			if (vesselRefBody != null)
			{
				num = ((Vector3)(FlightGlobals.ship_obtVelocity - vesselRefBody.GetObtVelocity())).magnitude;
			}
			break;
		case VelocityReferenceFrame.Comet:
			vesselRefBody = FlightGlobals.PersistentVesselIds[velocityCometID];
			if (vesselRefBody != null)
			{
				num = ((Vector3)(FlightGlobals.ship_obtVelocity - vesselRefBody.GetObtVelocity())).magnitude;
			}
			break;
		case VelocityReferenceFrame.Kerbal:
		{
			for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
			{
				if (CheckCrew(FlightGlobals.Vessels[i], velocityKerbal))
				{
					vesselRefBody = FlightGlobals.Vessels[i];
					break;
				}
			}
			if (vesselRefBody != null)
			{
				num = ((Vector3)(FlightGlobals.ship_obtVelocity - vesselRefBody.GetObtVelocity())).magnitude;
			}
			break;
		}
		case VelocityReferenceFrame.Vessel:
		{
			uint key = base.node.mission.CurrentVesselID(base.node, velocityVesselID);
			vesselRefBody = FlightGlobals.PersistentVesselIds[key];
			if (vesselRefBody != null)
			{
				num = ((Vector3)(FlightGlobals.ship_obtVelocity - vesselRefBody.GetObtVelocity())).magnitude;
			}
			break;
		}
		}
		if (comparisonOperator == TestComparisonLessGreaterOnly.GreaterThan && !(num < (double)velocity))
		{
			return true;
		}
		if (comparisonOperator == TestComparisonLessGreaterOnly.LessThan)
		{
			return num <= (double)velocity;
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

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004165");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "velocity")
		{
			return Localizer.Format("#autoLOC_8100154", field.guiName, (comparisonOperator == TestComparisonLessGreaterOnly.LessThan) ? "<" : ">", velocity.ToString("0"));
		}
		if (field.name == "referenceBody")
		{
			string text = base.GetNodeBodyParameterString(field) + "\n";
			switch (referenceBody)
			{
			case VelocityReferenceFrame.Asteroid:
			{
				string text3 = "";
				Asteroid asteroidByPersistentID = base.node.mission.GetAsteroidByPersistentID(velocityAsteroidID);
				text3 = ((asteroidByPersistentID != null) ? asteroidByPersistentID.name : Localizer.Format("#autoLOC_6003000"));
				return text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000046"), text3);
			}
			case VelocityReferenceFrame.Comet:
			{
				string text2 = "";
				Comet cometByPersistentID = base.node.mission.GetCometByPersistentID(velocityCometID);
				text2 = ((cometByPersistentID != null) ? Localizer.Format(cometByPersistentID.name) : Localizer.Format("#autoLOC_6003000"));
				return text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_6005065"), text2);
			}
			case VelocityReferenceFrame.Kerbal:
				return text + velocityKerbal.GetNodeBodyParameterString();
			case VelocityReferenceFrame.Default:
			case VelocityReferenceFrame.Vessel:
				return text;
			}
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("velocity", velocity);
		node.AddValue("comparisonOperator", comparisonOperator);
		node.AddValue("speedType", speedType);
		node.AddValue("referenceBody", referenceBody);
		node.AddValue("velocityVesselID", velocityVesselID);
		node.AddValue("velocityAsteroidID", velocityAsteroidID);
		node.AddValue("velocityCometID", velocityCometID);
		ConfigNode configNode = new ConfigNode("VELOCITYWRTKERBAL");
		velocityKerbal.Save(configNode);
		node.AddNode(configNode);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("velocity", ref velocity);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterOnly.GreaterThan);
		node.TryGetEnum("speedType", ref speedType, SpeedType.SurfaceVelocity);
		node.TryGetEnum("referenceBody", ref referenceBody, VelocityReferenceFrame.Default);
		node.TryGetValue("velocityVesselID", ref velocityVesselID);
		node.TryGetValue("velocityAsteroidID", ref velocityAsteroidID);
		node.TryGetValue("velocityCometID", ref velocityCometID);
		ConfigNode configNode = null;
		node.TryGetNode("VELOCITYWRTKERBAL", ref configNode);
		if (configNode != null)
		{
			velocityKerbal.Load(configNode);
		}
	}
}
