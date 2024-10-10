using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class MissionOrbit : OrbitSnapshot
{
	public double smaCache;

	public double degreesMNA;

	public double bodyRotationEditor;

	public Orbit Orbit { get; set; }

	[MEGUI_Dropdown(addDefaultOption = false, order = 0, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody Body
	{
		get
		{
			return Orbit.referenceBody;
		}
		set
		{
			Orbit.referenceBody = value;
			ReferenceBodyIndex = FlightGlobals.Bodies.IndexOf(value);
		}
	}

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100290", maxValue = 1200000f, clampTextInput = false, displayUnits = "", minValue = 0f, resetValue = "100000", group = "Base Parameters", order = 1, guiName = "#autoLOC_8100058")]
	public double SemiMajorAxis
	{
		get
		{
			return semiMajorAxis;
		}
		set
		{
			if ((1.0 + eccentricity) * value - Body.Radius <= Body.sphereOfInfluence && value >= Body.Radius / 2.0)
			{
				semiMajorAxis = value;
				Orbit.semiMajorAxis = value;
			}
		}
	}

	[MEGUI_InputField(group = "Base Parameters", ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 2, groupDisplayName = "#autoLOC_8100290", resetValue = "50000", guiName = "#autoLOC_8100059")]
	public double Apoapsis
	{
		get
		{
			return (1.0 + eccentricity) * semiMajorAxis - Body.Radius;
		}
		set
		{
			if (value < 0.0)
			{
				value = 0.0;
			}
			double editorMaxOrbitRadius = GetEditorMaxOrbitRadius(Body);
			double periapsis = Periapsis;
			if (value < periapsis)
			{
				Apoapsis = periapsis;
				return;
			}
			if (value > editorMaxOrbitRadius)
			{
				Eccentricity = 0.9999;
				return;
			}
			SemiMajorAxis = (value + Periapsis) / 2.0 + Body.Radius;
			Eccentricity = (value - semiMajorAxis + Body.Radius) / semiMajorAxis;
		}
	}

	[MEGUI_InputField(group = "Base Parameters", ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 3, groupDisplayName = "#autoLOC_8100290", resetValue = "50000", guiName = "#autoLOC_8100060")]
	public double Periapsis
	{
		get
		{
			return (1.0 - eccentricity) * semiMajorAxis - Body.Radius;
		}
		set
		{
			double num = 0.0 - Body.Radius;
			double apoapsis = Apoapsis;
			if (value > apoapsis)
			{
				Periapsis = apoapsis;
				return;
			}
			if (value <= num)
			{
				Eccentricity = 0.9999;
				return;
			}
			SemiMajorAxis = (Apoapsis + value) / 2.0 + Body.Radius;
			Eccentricity = (semiMajorAxis - Body.Radius - value) / semiMajorAxis;
		}
	}

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100301", canBeReset = true, maxValue = 0.9999f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = 0f, resetValue = "0.000", displayFormat = "F4", group = "Additional Parameters", order = 4, guiName = "#autoLOC_8100061")]
	public double Eccentricity
	{
		get
		{
			return eccentricity;
		}
		set
		{
			if ((1.0 + value) * semiMajorAxis - Body.Radius <= Body.sphereOfInfluence)
			{
				eccentricity = value;
				Orbit.eccentricity = value;
			}
		}
	}

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100301", canBeReset = true, maxValue = 180f, clampTextInput = true, roundToPlaces = 4, displayUnits = "°", minValue = 0f, resetValue = "0", displayFormat = "F2", group = "Additional Parameters", order = 5, guiName = "#autoLOC_8100062")]
	public double Inclination
	{
		get
		{
			return inclination;
		}
		set
		{
			inclination = value;
			Orbit.inclination = value;
		}
	}

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100301", canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 4, displayUnits = "°", minValue = 0f, resetValue = "0.0", displayFormat = "F2", group = "Additional Parameters", order = 6, guiName = "#autoLOC_8100063")]
	public double Lan
	{
		get
		{
			return double_0;
		}
		set
		{
			double_0 = value;
			Orbit.double_0 = value;
		}
	}

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100301", canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 4, displayUnits = "°", minValue = 0f, resetValue = "0.0", displayFormat = "F2", group = "Additional Parameters", order = 7, guiName = "#autoLOC_8100064")]
	public double ArgumentOfPeriapsis
	{
		get
		{
			return argOfPeriapsis;
		}
		set
		{
			argOfPeriapsis = value;
			Orbit.argumentOfPeriapsis = value;
		}
	}

	public double Epoch
	{
		get
		{
			return epoch;
		}
		set
		{
			epoch = value;
			Orbit.epoch = value;
		}
	}

	[MEGUI_NumberRange(canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 4, displayUnits = "°", minValue = 0f, resetValue = "0.0", displayFormat = "F2", group = "Additional Parameters", order = 8, guiName = "#autoLOC_8100065")]
	public double MeanAnomalyAtEpoch
	{
		get
		{
			return degreesMNA;
		}
		set
		{
			degreesMNA = value;
			meanAnomalyAtEpoch = value * (Math.PI / 180.0);
			Orbit.meanAnomalyAtEpoch = value * (Math.PI / 180.0);
		}
	}

	public MissionOrbit(CelestialBody bodyRef)
		: base(bodyRef)
	{
		Orbit = Load();
		degreesMNA = meanAnomalyAtEpoch * (180.0 / Math.PI);
		bodyRotationEditor = GAPCelestialBodyState_Orbit.CalculateBodyEditorAngle(Orbit.referenceBody);
	}

	public MissionOrbit(Orbit orbit)
		: base(orbit)
	{
		Orbit = Load();
		degreesMNA = meanAnomalyAtEpoch * (180.0 / Math.PI);
		bodyRotationEditor = GAPCelestialBodyState_Orbit.CalculateBodyEditorAngle(Orbit.referenceBody);
	}

	public MissionOrbit(ConfigNode node)
		: base(node)
	{
		Orbit = Load();
		degreesMNA = meanAnomalyAtEpoch * (180.0 / Math.PI);
		bodyRotationEditor = GAPCelestialBodyState_Orbit.CalculateBodyEditorAngle(Orbit.referenceBody);
	}

	public Orbit RelativeOrbit(Mission mission)
	{
		Orbit orbit = new Orbit(Orbit);
		double num = double_0;
		orbit.double_0 = num;
		orbit.meanAnomalyAtEpoch -= GetUTOrbitRotation(Orbit, (mission != null) ? mission.situation.startUT : Planetarium.GetUniversalTime()) * (Math.PI / 180.0);
		return orbit;
	}

	public double GetUTOrbitRotation(Orbit orbit, double double_1)
	{
		return double_1 * 360.0 / orbit.period;
	}

	public void Reset()
	{
		Eccentricity = 0.0;
		SemiMajorAxis = Body.Radius + 100000.0;
		Inclination = 0.001;
		Lan = 0.0;
		ArgumentOfPeriapsis = 0.0;
		MeanAnomalyAtEpoch = 0.0;
		Epoch = 0.0;
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("editorRotation", bodyRotationEditor);
	}

	public void Load(ConfigNode node)
	{
		foreach (ConfigNode.Value value in node.values)
		{
			switch (value.name)
			{
			case "MNA":
				MeanAnomalyAtEpoch = double.Parse(value.value) * (180.0 / Math.PI);
				break;
			case "ECC":
				Eccentricity = double.Parse(value.value);
				break;
			case "editorRotation":
				bodyRotationEditor = double.Parse(value.value);
				break;
			case "SMA":
				smaCache = double.Parse(value.value);
				break;
			case "LAN":
				Lan = double.Parse(value.value);
				break;
			case "REF":
				Body = FlightGlobals.Bodies[int.Parse(value.value)];
				SemiMajorAxis = smaCache;
				break;
			case "EPH":
				Epoch = double.Parse(value.value);
				break;
			case "INC":
				Inclination = double.Parse(value.value);
				break;
			case "LPE":
				ArgumentOfPeriapsis = double.Parse(value.value);
				break;
			}
		}
		Orbit.Init();
	}

	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			list.Add(new MEGUIDropDownItem(FlightGlobals.Bodies[i].name, FlightGlobals.Bodies[i], FlightGlobals.Bodies[i].displayName.LocalizeRemoveGender()));
		}
		return list;
	}

	public override bool Equals(object obj)
	{
		MissionOrbit missionOrbit = obj as MissionOrbit;
		if (ReferenceBodyIndex.Equals(missionOrbit.ReferenceBodyIndex) && UtilMath.Approximately(semiMajorAxis, missionOrbit.semiMajorAxis) && UtilMath.Approximately(eccentricity, missionOrbit.eccentricity) && UtilMath.Approximately(inclination, missionOrbit.inclination) && UtilMath.Approximately(double_0, missionOrbit.double_0) && UtilMath.Approximately(argOfPeriapsis, missionOrbit.argOfPeriapsis) && UtilMath.Approximately(epoch, missionOrbit.epoch))
		{
			return UtilMath.Approximately(meanAnomalyAtEpoch, missionOrbit.meanAnomalyAtEpoch);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ReferenceBodyIndex.GetHashCode() ^ semiMajorAxis.GetHashCode() ^ eccentricity.GetHashCode() ^ inclination.GetHashCode() ^ double_0.GetHashCode() ^ argOfPeriapsis.GetHashCode() ^ epoch.GetHashCode() ^ meanAnomalyAtEpoch.GetHashCode();
	}

	public string GetNodeBodyParameterString()
	{
		return Localizer.Format("#autoLOC_8004192", Body.displayName.LocalizeRemoveGender());
	}

	public static double GetEditorMaxOrbitRadius(CelestialBody body)
	{
		if (double.IsInfinity(body.sphereOfInfluence))
		{
			double num = 0.0;
			int count = body.orbitingBodies.Count;
			for (int i = 0; i < count; i++)
			{
				if (body.orbitingBodies[i].orbit.radius > num)
				{
					num = body.orbitingBodies[i].orbit.radius;
				}
			}
			if (num == 0.0)
			{
				Debug.LogWarning("Unable to find a maxorbit radius - using body radius times 10");
				return body.Radius * 10.0;
			}
			return num * 1.2;
		}
		return body.sphereOfInfluence - 1.0;
	}
}
