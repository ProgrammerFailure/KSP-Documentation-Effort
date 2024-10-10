using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Editor;
using FinePrint;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class SurfaceVolume : IConfigNode
{
	public enum VolumeShape
	{
		[Description("#autoLOC_8100034")]
		Cone,
		[Description("#autoLOC_8100031")]
		Sphere
	}

	[MEGUI_Dropdown(addDefaultOption = false, order = 0, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody body;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 90f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -90f, resetValue = "0.0", displayFormat = "F3", group = "Translation", order = 2, guiName = "#autoLOC_8200027")]
	public double latitude;

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8200025", canBeReset = true, maxValue = 180f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -180f, resetValue = "0.0", displayFormat = "F3", group = "Translation", order = 1, guiName = "#autoLOC_8200026")]
	public double longitude;

	public double altitude;

	[MEGUI_Dropdown(group = "Shape", addDefaultOption = false, order = 3, groupDisplayName = "#autoLOC_8200033", guiName = "#autoLOC_8200034")]
	public VolumeShape shape;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "#autoLOC_289929", minValue = 10f, resetValue = "0.0", displayFormat = "F3", group = "Shape", order = 4, guiName = "#autoLOC_8200035")]
	public float radius;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "#autoLOC_289929", minValue = 0f, resetValue = "0.0", displayFormat = "F3", group = "Shape", order = 5, guiName = "#autoLOC_8200038")]
	public float heightSphere;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "#autoLOC_289929", minValue = 0f, resetValue = "0.0", displayFormat = "F3", group = "Shape", order = 6, guiName = "#autoLOC_8200036")]
	public float heightConeMin;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "#autoLOC_289929", minValue = 0f, resetValue = "0.0", displayFormat = "F3", group = "Shape", order = 7, guiName = "#autoLOC_8200037")]
	public float heightConeMax;

	public bool showNodeLabel;

	public SurfaceVolume(CelestialBody targetBody, double longitude, double latitude, float radius, VolumeShape shape, float heightSphere, float heightConeMin, float heightConeMax)
	{
		body = targetBody;
		this.longitude = longitude;
		this.latitude = latitude;
		this.radius = radius;
		this.shape = shape;
		this.heightSphere = heightSphere;
		this.heightConeMin = heightConeMin;
		this.heightConeMax = heightConeMax;
	}

	public SurfaceVolume()
	{
		CelestialBody homeBody = FlightGlobals.GetHomeBody();
		body = homeBody;
		longitude = 0.0;
		latitude = 0.0;
		shape = VolumeShape.Cone;
		ResetBodyToDefaultValues();
	}

	public void ResetBodyToDefaultValues()
	{
		heightSphere = 0f;
		radius = (float)body.Radius / 4f;
		heightConeMax = (float)body.Radius * 0.25f;
		heightConeMin = 0f;
	}

	public void AjustToBody(float radius, float heightSphere, float heigthConeMin, float heightConeMax, CelestialBody newBody, CelestialBody pastBody)
	{
		double num = (double)radius / pastBody.Radius;
		this.radius = (float)(newBody.Radius * num);
		double num2 = (double)heightSphere / pastBody.Radius;
		this.heightSphere = (float)(newBody.Radius * num2);
		double num3 = (double)heigthConeMin / pastBody.Radius;
		heightConeMin = (float)(newBody.Radius * num3);
		double num4 = (double)heightConeMax / pastBody.Radius;
		this.heightConeMax = (float)(newBody.Radius * num4);
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
		waypoint.nodeCaption1 = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8200035"), radius + Localizer.Format("#autoLOC_289929"));
		waypoint.nodeCaption2 = ((shape == VolumeShape.Cone) ? Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8200036"), heightConeMin) : Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8200038"), heightSphere)) + Localizer.Format("#autoLOC_289929");
		waypoint.nodeCaption3 = ((shape == VolumeShape.Cone) ? (Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8200037"), heightConeMax) + Localizer.Format("#autoLOC_289929")) : "");
		return waypoint;
	}

	public string GetNodeBodyParameterString()
	{
		string text = longitude.ToString("0.##");
		string text2 = latitude.ToString("0.##");
		string text3 = (radius / 1000f).ToString("0.##");
		if (shape == VolumeShape.Sphere)
		{
			return Localizer.Format("#autoLOC_8007313", body.displayName, text, text2, text3, (heightSphere / 1000f).ToString("0.##"));
		}
		return Localizer.Format("#autoLOC_8007314", body.displayName, text, text2, text3, (heightConeMin / 1000f).ToString("0.##"), (heightConeMax / 1000f).ToString("0.##"));
	}

	public bool HasNodeLabel()
	{
		if (showNodeLabel)
		{
			return HasWorldPosition();
		}
		return false;
	}

	public bool HasWorldPosition()
	{
		return true;
	}

	public Vector3 GetWorldPosition()
	{
		return shape switch
		{
			VolumeShape.Sphere => body.GetWorldSurfacePosition(latitude, longitude, altitude), 
			VolumeShape.Cone => body.GetWorldSurfacePosition(latitude, longitude, altitude), 
			_ => body.GetWorldSurfacePosition(latitude, longitude, altitude), 
		};
	}

	public string GetExtraText()
	{
		return Localizer.Format("#autoLOC_8000274", radius);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is SurfaceVolume surfaceVolume))
		{
			return false;
		}
		if (body.Equals(surfaceVolume.body) && UtilMath.Approximately(latitude, surfaceVolume.latitude) && UtilMath.Approximately(longitude, surfaceVolume.longitude) && Mathf.Approximately(radius, surfaceVolume.radius) && shape.Equals(surfaceVolume.shape) && Mathf.Approximately(heightSphere, surfaceVolume.heightSphere) && Mathf.Approximately(heightConeMin, surfaceVolume.heightConeMin))
		{
			return Mathf.Approximately(heightConeMax, surfaceVolume.heightConeMax);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return body.GetHashCode() ^ latitude.GetHashCode() ^ longitude.GetHashCode() ^ radius.GetHashCode() ^ shape.GetHashCode() ^ heightSphere.GetHashCode() ^ heightConeMin.GetHashCode() ^ heightConeMax.GetHashCode();
	}

	public void Load(ConfigNode node)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "hConeMax":
				flag3 = true;
				heightConeMax = float.Parse(value.value);
				break;
			case "alt":
				altitude = double.Parse(value.value);
				break;
			case "hConeMin":
				flag2 = true;
				heightConeMin = float.Parse(value.value);
				break;
			case "hSphere":
				flag = true;
				heightSphere = float.Parse(value.value);
				break;
			case "lat":
				latitude = double.Parse(value.value);
				break;
			case "lon":
				longitude = double.Parse(value.value);
				break;
			case "shape":
				shape = (VolumeShape)Enum.Parse(typeof(VolumeShape), value.value);
				break;
			case "targetBody":
				if (!string.IsNullOrEmpty(value.value))
				{
					body = FlightGlobals.GetBodyByName(value.value);
				}
				break;
			case "rad":
				radius = float.Parse(value.value);
				break;
			}
		}
		switch (shape)
		{
		case VolumeShape.Sphere:
			if (flag)
			{
				altitude = heightSphere;
			}
			break;
		case VolumeShape.Cone:
			if (flag2 && flag3)
			{
				altitude = (heightConeMin + heightConeMax) / 2f;
			}
			break;
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
			node.AddValue("targetBody", "");
		}
		else
		{
			node.AddValue("targetBody", body.name);
		}
		node.AddValue("lat", latitude);
		node.AddValue("lon", longitude);
		node.AddValue("alt", altitude);
		node.AddValue("rad", radius);
		node.AddValue("shape", shape);
		node.AddValue("hSphere", heightSphere);
		node.AddValue("hConeMin", heightConeMin);
		node.AddValue("hConeMax", heightConeMax);
	}

	public bool IsInsideVolume(float targetLatitude, float targetLongitude, float targetAltitude)
	{
		bool result = false;
		switch (shape)
		{
		case VolumeShape.Sphere:
		{
			Vector3d relSurfacePosition = body.GetRelSurfacePosition(targetLatitude, targetLongitude, targetAltitude);
			Vector3d relSurfacePosition2 = body.GetRelSurfacePosition(latitude, longitude, heightSphere);
			result = Math.Pow(relSurfacePosition.x - relSurfacePosition2.x, 2.0) + Math.Pow(relSurfacePosition.y - relSurfacePosition2.y, 2.0) + Math.Pow(relSurfacePosition.z - relSurfacePosition2.z, 2.0) < Math.Pow(radius, 2.0);
			break;
		}
		case VolumeShape.Cone:
			if (targetAltitude < heightConeMax && targetAltitude > heightConeMin)
			{
				result = CelestialUtilities.GreatCircleDistance(body, targetLatitude, targetLongitude, latitude, longitude) <= (double)radius;
			}
			break;
		}
		return result;
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
