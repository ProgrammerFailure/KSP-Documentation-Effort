using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_TraveledMoreThan : AwardModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, guiName = "#autoLOC_8100017", Tooltip = "#autoLOC_8004206")]
	public double distance;

	public double distanceTraveled;

	public AwardModule_TraveledMoreThan(MENode node)
		: base(node)
	{
	}

	public AwardModule_TraveledMoreThan(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
	}

	public override bool EvaluateCondition(Mission mission)
	{
		int i = 0;
		for (int count = FlightGlobals.Vessels.Count; i < count; i++)
		{
			distanceTraveled += FlightGlobals.Vessels[i].distanceTraveled;
		}
		return distanceTraveled / 1000.0 > distance;
	}

	public override void StartTracking()
	{
		GameEvents.onVesselWillDestroy.Add(OnVesselWillDestroy);
	}

	public override void StopTracking()
	{
		GameEvents.onVesselWillDestroy.Remove(OnVesselWillDestroy);
	}

	public void OnVesselWillDestroy(Vessel data)
	{
		distanceTraveled += data.distanceTraveled;
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("distance", ref distance);
		ConfigNode configNode = new ConfigNode();
		if (node.TryGetNode("PROGRESS", ref configNode))
		{
			configNode.TryGetValue("distanceTraveled", ref distanceTraveled);
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("distance", distance);
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			node.AddNode("PROGRESS").AddValue("distanceTraveled", distanceTraveled);
		}
	}
}
