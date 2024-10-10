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
public class TestVesselState : TestVessel, IScoreableObjective
{
	[MEGUI_Dropdown(resetValue = "INACTIVE", guiName = "#autoLOC_8000122", Tooltip = "#autoLOC_8000123")]
	public Vessel.State vesselState;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000121");
		vesselState = Vessel.State.INACTIVE;
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			return vessel.state == vesselState;
		}
		return false;
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("state", vesselState);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (!node.TryGetEnum("state", ref vesselState, Vessel.State.INACTIVE))
		{
			Debug.LogError("Failed to parse vesselstate from " + title);
		}
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
			Debug.LogErrorFormat("[TestVesselState] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
