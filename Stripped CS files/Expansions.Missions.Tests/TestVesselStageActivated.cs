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
public class TestVesselStageActivated : TestVessel, IScoreableObjective
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8000130", Tooltip = "#autoLOC_8000131")]
	public int activation;

	public bool eventFound;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000129");
		useActiveVessel = true;
	}

	public override bool Test()
	{
		return eventFound;
	}

	public override void Initialized()
	{
		eventFound = false;
		GameEvents.onStageActivate.Add(StageActivated);
	}

	public override void Cleared()
	{
		GameEvents.onStageActivate.Remove(StageActivated);
	}

	public void StageActivated(int act)
	{
		base.Test();
		if (vessel != null && vessel == FlightGlobals.ActiveVessel && act == activation)
		{
			eventFound = true;
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004032");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("activation", activation);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("activation", ref activation);
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
			Debug.LogErrorFormat("[TestVesselStageActivated] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
