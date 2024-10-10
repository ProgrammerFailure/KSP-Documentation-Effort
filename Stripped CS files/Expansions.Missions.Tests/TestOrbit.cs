using System;
using System.Collections;
using Expansions.Missions.Editor;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time),
	typeof(ScoreModule_Accuracy)
})]
public class TestOrbit : TestVessel, IScoreableObjective, INodeOrbit
{
	[MEGUI_NumberRange(maxValue = 100f, roundToPlaces = 0, displayUnits = "%", minValue = 0f, resetValue = "90", displayFormat = "0.##", onValueChange = "OnOrbitAccuracyChanged", order = 20, guiName = "#autoLOC_8000159", Tooltip = "#autoLOC_8000160")]
	public double orbitAccuracy;

	[MEGUI_Time(order = 100, resetValue = "5", guiName = "#autoLOC_8003019", Tooltip = "#autoLOC_8003061")]
	public double stabilizationTime = 5.0;

	[MEGUI_Checkbox(order = 110, resetValue = "False", guiName = "#autoLOC_8003062", Tooltip = "#autoLOC_8003063")]
	public bool underThrust;

	[MEGUI_CelestialBody_Orbit(displayMeanAnomaly = false, order = 10, gapDisplay = true, guiName = "#autoLOC_8000163")]
	public MissionOrbit missionOrbit;

	public Orbit relativeOrbit;

	public MissionOrbitRenderer orbitRenderer;

	public Coroutine initializeorbitRenderer;

	public bool situationSuccess;

	public bool orbitSuccess;

	public double successStartTime = -1.0;

	public double OrbitDeviation => Mathf.Clamp(100f - (float)orbitAccuracy, 0f, 100f);

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000158");
		orbitAccuracy = 90.0;
		missionOrbit = new MissionOrbit(FlightGlobals.GetHomeBody());
		useActiveVessel = true;
	}

	public override void Initialized()
	{
		base.Initialized();
		relativeOrbit = missionOrbit.RelativeOrbit(base.node.mission);
		initializeorbitRenderer = StartCoroutine(initializeDrawOrbit());
	}

	public void OnOrbitAccuracyChanged(double newValue)
	{
		orbitAccuracy = Mathf.Clamp((float)newValue, 0f, 100f);
		UpdateNodeBodyUI();
	}

	public IEnumerator initializeDrawOrbit()
	{
		DrawOrbit();
		if (orbitRenderer == null)
		{
			yield return null;
		}
	}

	public void DrawOrbit()
	{
		if (HighLogic.CurrentGame != null && HighLogic.LoadedScene != GameScenes.SPACECENTER && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER))
		{
			orbitRenderer = CreateOrbitRenderer();
			relativeOrbit = missionOrbit.RelativeOrbit(base.node.mission);
		}
	}

	public override void Cleared()
	{
		base.Cleared();
		if (orbitRenderer != null)
		{
			orbitRenderer.Cleanup();
		}
		if (initializeorbitRenderer != null)
		{
			StopCoroutine(initializeorbitRenderer);
		}
	}

	public MissionOrbitRenderer CreateOrbitRenderer()
	{
		Vessel vessel = null;
		if (vesselID == 0)
		{
			if (vessel == null && FlightGlobals.Vessels.Count > 0)
			{
				for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
				{
					if (FlightGlobals.Vessels[i] != null)
					{
						vessel = FlightGlobals.Vessels[i];
						break;
					}
				}
			}
		}
		else
		{
			uint key = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(key))
			{
				vessel = FlightGlobals.PersistentVesselIds[key];
			}
		}
		if (vessel != null)
		{
			relativeOrbit = missionOrbit.RelativeOrbit(base.node.mission);
			return MissionOrbitRenderer.Setup(base.node, vessel, relativeOrbit);
		}
		return null;
	}

	public override bool Test()
	{
		if (orbitRenderer == null)
		{
			DrawOrbit();
		}
		base.Test();
		situationSuccess = vessel != null && vessel.mainBody == missionOrbit.Body;
		if (!situationSuccess)
		{
			successStartTime = -1.0;
			return false;
		}
		if (!underThrust && vessel.geeForce > 0.10000000149011612)
		{
			successStartTime = -1.0;
			return false;
		}
		orbitSuccess = false;
		if (relativeOrbit != null)
		{
			orbitSuccess = VesselUtilities.VesselAtOrbit(relativeOrbit, OrbitDeviation, vessel);
		}
		if (!orbitSuccess)
		{
			successStartTime = -1.0;
			return false;
		}
		if (successStartTime < 0.0)
		{
			successStartTime = Planetarium.GetUniversalTime();
		}
		return successStartTime + stabilizationTime < Planetarium.GetUniversalTime();
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.FieldType == typeof(MissionOrbit))
		{
			return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(Localizer.Format("#autoLOC_8100316", field.guiName, missionOrbit.Body.displayName.LocalizeRemoveGender()) + "\n", Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100058"), missionOrbit.SemiMajorAxis.ToString("N0")), "\n"), Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100059"), missionOrbit.Apoapsis.ToString("N0")), "\n"), Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100060"), missionOrbit.Periapsis.ToString("N0")), "\n"), Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100061"), missionOrbit.Eccentricity.ToString("0.##")), "\n"), Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100062"), missionOrbit.Inclination.ToString("0.##°")), "\n"), Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100063"), missionOrbit.double_0.ToString("0.##")), "\n"), Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100064"), missionOrbit.ArgumentOfPeriapsis.ToString("N0")), "\n"), Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100065"), missionOrbit.MeanAnomalyAtEpoch.ToString("N0")));
		}
		if (field.name == "orbitAccuracy")
		{
			return Localizer.Format("#autoLOC_8004190", field.guiName, orbitAccuracy.ToString("0.##") + "%");
		}
		return base.GetNodeBodyParameterString(field);
	}

	public bool HasNodeOrbit()
	{
		return true;
	}

	public Orbit GetNodeOrbit()
	{
		return missionOrbit.Orbit;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004023");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("orbitAccuracy", orbitAccuracy);
		node.AddValue("stabilizationTime", stabilizationTime);
		node.AddValue("underThrust", underThrust);
		missionOrbit.Save(node.AddNode("ORBIT"));
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (!node.TryGetValue("orbitAccuracy", ref orbitAccuracy))
		{
			float value = 0.1f;
			node.TryGetValue("orbitTolerance", ref value);
			orbitAccuracy = 100.0 - (double)value * 100.0;
		}
		node.TryGetValue("stabilizationTime", ref stabilizationTime);
		node.TryGetValue("underThrust", ref underThrust);
		ConfigNode configNode = new ConfigNode();
		if (node.TryGetNode("ORBIT", ref configNode))
		{
			missionOrbit = new MissionOrbit(configNode);
			if (missionOrbit.Orbit != null)
			{
				missionOrbit.Orbit.Init();
			}
		}
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			if (vesselID != 0)
			{
				return FlightGlobals.PersistentVesselIds[vesselID];
			}
			return FlightGlobals.ActiveVessel;
		}
		if (scoreModule == typeof(ScoreModule_Accuracy) && relativeOrbit != null)
		{
			return VesselUtilities.VesselAtOrbitAccuracy(relativeOrbit, OrbitDeviation, vessel);
		}
		return null;
	}
}
