using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using FinePrint;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class SurfaceArea : IConfigNode
{
	[MEGUI_Dropdown(addDefaultOption = false, order = 0, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody body;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 90f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -90f, resetValue = "0.0", displayFormat = "F3", group = "Translation", order = 2, guiName = "#autoLOC_8200027")]
	public double latitude;

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8200025", canBeReset = true, maxValue = 180f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -180f, resetValue = "0.0", displayFormat = "F3", group = "Translation", order = 1, guiName = "#autoLOC_8200026")]
	public double longitude;

	public double altitude;

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8200040", canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "m", minValue = 100f, resetValue = "0.0", displayFormat = "F3", group = "Area", order = 3, guiName = "#autoLOC_8200035")]
	public float radius;

	public SurfaceArea()
	{
		body = FlightGlobals.GetHomeBody();
	}

	public SurfaceArea(CelestialBody targetBody, double longitude, double latitude, float radius)
	{
		body = targetBody;
		this.longitude = longitude;
		this.latitude = latitude;
		this.radius = radius;
	}

	public bool HasWaypoint()
	{
		return true;
	}

	public Waypoint GetWaypoint()
	{
		Waypoint waypoint = new Waypoint();
		waypoint.celestialName = body.name;
		waypoint.latitude = latitude;
		waypoint.longitude = longitude;
		waypoint.altitude = altitude;
		waypoint.radius = radius;
		waypoint.nodeCaption1 = Localizer.Format("#autoLOC_8000274", radius);
		return waypoint;
	}

	public bool HasNodeLabel()
	{
		return HasWorldPosition();
	}

	public bool HasWorldPosition()
	{
		return true;
	}

	public Vector3 GetWorldPosition()
	{
		return body.GetWorldSurfacePosition(latitude, longitude, altitude);
	}

	public string GetExtraText()
	{
		return Localizer.Format("#autoLOC_8000274", radius);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is SurfaceArea surfaceArea))
		{
			return false;
		}
		if (body.name.Equals(surfaceArea.body.name) && UtilMath.Approximately(latitude, surfaceArea.latitude) && UtilMath.Approximately(longitude, surfaceArea.longitude))
		{
			return Mathf.Approximately(radius, surfaceArea.radius);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return body.name.GetHashCode_Net35() ^ latitude.GetHashCode() ^ longitude.GetHashCode() ^ radius.GetHashCode();
	}

	public void Load(ConfigNode node)
	{
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "radius":
				radius = float.Parse(value.value);
				break;
			case "alt":
				altitude = double.Parse(value.value);
				break;
			case "lon":
				longitude = double.Parse(value.value);
				break;
			case "lat":
				latitude = double.Parse(value.value);
				break;
			case "bodyName":
				if (!string.IsNullOrEmpty(value.value))
				{
					body = FlightGlobals.GetBodyByName(value.value);
				}
				break;
			}
		}
		if (body == null)
		{
			body = FlightGlobals.GetHomeBody();
		}
	}

	public void Save(ConfigNode node)
	{
		if (body == null)
		{
			node.AddValue("bodyName", "");
		}
		else
		{
			node.AddValue("bodyName", body.name);
		}
		node.AddValue("lat", latitude);
		node.AddValue("lon", longitude);
		node.AddValue("alt", altitude);
		node.AddValue("radius", radius);
	}

	public bool IsPointInCircle(double lat, double lon)
	{
		return CelestialUtilities.GreatCircleDistance(body, lat, lon, latitude, longitude) <= (double)radius;
	}

	public double PointInCircleAccuracy(double lat, double lon)
	{
		double num = CelestialUtilities.GreatCircleDistance(body, lat, lon, latitude, longitude);
		return 1.0 - UtilMath.Clamp(num / (double)radius, 0.0, 1.0);
	}

	public string GetNodeBodyParameterString()
	{
		string text = longitude.ToString("0.##");
		string text2 = latitude.ToString("0.##");
		string text3 = (radius / 1000f).ToString("0.##");
		return Localizer.Format("#autoLOC_8007312", body.displayName, text, text2, text3);
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
}
