using System;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestSOIReached : TestVessel, IScoreableObjective, INodeBody
{
	[MEGUI_CelestialBody(order = 10, showAnySOIoption = true, gapDisplay = true, guiName = "#autoLOC_8000263", Tooltip = "#autoLOC_8000157")]
	public MissionCelestialBody missionBody;

	public bool eventFound;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000164");
		missionBody = new MissionCelestialBody(FlightGlobals.GetHomeBody());
	}

	public override void Initialized()
	{
		eventFound = false;
		GameEvents.onVesselSOIChanged.Add(OnReachedSOI);
	}

	public override void Cleared()
	{
		GameEvents.onVesselSOIChanged.Remove(OnReachedSOI);
	}

	public void OnReachedSOI(GameEvents.HostedFromToAction<Vessel, CelestialBody> fromTo)
	{
		base.Test();
		if (vessel == fromTo.host && missionBody.IsValid(fromTo.to))
		{
			eventFound = true;
		}
	}

	public override bool Test()
	{
		return eventFound;
	}

	public bool HasNodeBody()
	{
		return true;
	}

	public CelestialBody GetNodeBody()
	{
		return missionBody.Body;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004027");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		missionBody.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		missionBody.Load(node);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			if (vesselID == 0)
			{
				return FlightGlobals.ActiveVessel;
			}
			uint num = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(num))
			{
				return FlightGlobals.PersistentVesselIds[num];
			}
			Debug.LogErrorFormat("[TestSOIReached] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
