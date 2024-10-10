using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Editor;
using FinePrint;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class VesselGroundLocation : IConfigNode
{
	public enum GizmoIcon
	{
		[Description("#autoLOC_8007216")]
		Rocket,
		[Description("#autoLOC_8007217")]
		AirCraft,
		[Description("#autoLOC_8007218")]
		Asteroid,
		[Description("#autoLOC_8007219")]
		Kerbal,
		[Description("#autoLOC_8000067")]
		LaunchSite,
		[Description("#autoLOC_8007221")]
		Flag,
		[Description("#autoLOC_8007221")]
		Comet
	}

	public delegate void OnGizmoIconChangeDelegate(GizmoIcon gizmoIcon);

	public OnGizmoIconChangeDelegate OnGizmoIconChange;

	[MEGUI_Dropdown(onValueChange = "OnVesselIconChange", addDefaultOption = false, order = 1, SetDropDownItems = "SetAvailableGizmoIcons", canBePinned = true, guiName = "#autoLOC_8007215")]
	public GizmoIcon gizmoIcon = GizmoIcon.Flag;

	[MEGUI_Dropdown(addDefaultOption = false, order = 0, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody targetBody;

	[MEGUI_Checkbox(order = 2, canBePinned = true, guiName = "#autoLOC_8002007", Tooltip = "#autoLOC_8006110")]
	public bool splashed;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 90f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -90f, resetValue = "0.0", displayFormat = "F5", group = "Translation", order = 4, guiName = "#autoLOC_8200027")]
	public double latitude;

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8200025", canBeReset = true, maxValue = 180f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -180f, resetValue = "0.0", displayFormat = "F5", group = "Translation", order = 3, guiName = "#autoLOC_8200026")]
	public double longitude;

	[MEGUI_InputField(group = "Translation", ContentType = MEGUI_Control.InputContentType.Alphanumeric, order = 5, guiName = "#autoLOC_8200028")]
	public double altitude;

	[MEGUI_Quaternion(group = "Angles", order = 6, groupDisplayName = "#autoLOC_8200029", guiName = "#autoLOC_8200029")]
	public MissionQuaternion rotation;

	public double startLatMin = -0.008999999612569809;

	public double startLatMax = 0.15530000627040863;

	public double startLonMin = -74.82420349121094;

	public double startLonMax = -74.51119995117188;

	public GizmoIcon GAPGizmoIcon
	{
		get
		{
			return gizmoIcon;
		}
		set
		{
			gizmoIcon = value;
			if (OnGizmoIconChange != null)
			{
				OnGizmoIconChange(value);
			}
		}
	}

	public VesselGroundLocation()
	{
		rotation = new MissionQuaternion();
	}

	public VesselGroundLocation(MENode node, GizmoIcon icon)
		: this()
	{
		targetBody = FlightGlobals.GetHomeBody();
		rotation = new MissionQuaternion();
		KSPRandom kSPRandom = new KSPRandom();
		latitude = startLatMin + kSPRandom.NextDouble() * (startLatMax - startLatMin);
		longitude = startLonMin + kSPRandom.NextDouble() * (startLonMax - startLonMin);
		gizmoIcon = icon;
	}

	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			if (FlightGlobals.Bodies[i].pqsController != null)
			{
				list.Add(new MEGUIDropDownItem(FlightGlobals.Bodies[i].name, FlightGlobals.Bodies[i], FlightGlobals.Bodies[i].displayName.LocalizeRemoveGender()));
			}
		}
		return list;
	}

	public bool HasWaypoint()
	{
		return true;
	}

	public Waypoint GetWaypoint()
	{
		return new Waypoint
		{
			celestialName = targetBody.name,
			latitude = latitude,
			longitude = longitude,
			nodeCaption1 = Localizer.Format("#autoLOC_8002000")
		};
	}

	public bool HasNodeLabel()
	{
		return true;
	}

	public Vector3 GetWorldPosition()
	{
		return targetBody.GetWorldSurfacePosition(latitude, longitude, 0.0);
	}

	public string GetExtraText()
	{
		return Localizer.Format("#autoLOC_8002000");
	}

	public string GetNodeBodyParameterString()
	{
		string text = longitude.ToString("0.##");
		string text2 = latitude.ToString("0.##");
		return Localizer.Format("#autoLOC_8004191", targetBody.displayName, text, text2);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is VesselGroundLocation vesselGroundLocation))
		{
			return false;
		}
		if (targetBody.Equals(vesselGroundLocation.targetBody) && altitude.Equals(vesselGroundLocation.altitude) && latitude.Equals(vesselGroundLocation.latitude) && longitude.Equals(vesselGroundLocation.longitude))
		{
			return rotation.Equals(vesselGroundLocation.rotation);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return targetBody.GetHashCode() ^ altitude.GetHashCode() ^ latitude.GetHashCode() ^ longitude.GetHashCode() ^ rotation.GetHashCode();
	}

	public void OnVesselIconChange(GizmoIcon newIcon)
	{
		GAPGizmoIcon = newIcon;
	}

	public List<MEGUIDropDownItem> SetAvailableGizmoIcons()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		if (gizmoIcon == GizmoIcon.AirCraft)
		{
			list.Add(new MEGUIDropDownItem(GizmoIcon.Rocket.ToString(), GizmoIcon.Rocket, GizmoIcon.Rocket.Description()));
		}
		list.Add(new MEGUIDropDownItem(gizmoIcon.ToString(), gizmoIcon, gizmoIcon.Description()));
		if (gizmoIcon == GizmoIcon.Rocket)
		{
			list.Add(new MEGUIDropDownItem(GizmoIcon.AirCraft.ToString(), GizmoIcon.AirCraft, GizmoIcon.AirCraft.Description()));
		}
		return list;
	}

	public void Load(ConfigNode node)
	{
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "lon":
				longitude = double.Parse(value.value);
				break;
			case "gizmoIcon":
				gizmoIcon = (GizmoIcon)Enum.Parse(typeof(GizmoIcon), value.value);
				break;
			case "alt":
				altitude = double.Parse(value.value);
				break;
			case "lat":
				latitude = double.Parse(value.value);
				break;
			case "rot":
				rotation = new MissionQuaternion(KSPUtil.ParseVector3(value.value));
				break;
			case "splashed":
				splashed = bool.Parse(value.value);
				break;
			case "bodyName":
				if (!string.IsNullOrEmpty(value.value))
				{
					targetBody = FlightGlobals.GetBodyByName(value.value);
				}
				break;
			}
		}
		if (targetBody == null)
		{
			targetBody = FlightGlobals.GetHomeBody();
		}
	}

	public void Save(ConfigNode node)
	{
		if (targetBody == null)
		{
			node.AddValue("bodyName", "");
		}
		else
		{
			node.AddValue("bodyName", targetBody.name);
		}
		node.AddValue("gizmoIcon", gizmoIcon.ToString());
		node.AddValue("lat", latitude);
		node.AddValue("lon", longitude);
		node.AddValue("alt", altitude);
		node.AddValue("rot", KSPUtil.WriteVector(rotation.eulerAngles));
		node.AddValue("splashed", splashed);
	}
}
