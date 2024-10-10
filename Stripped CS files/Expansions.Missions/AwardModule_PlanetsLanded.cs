using System.Collections.Generic;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_PlanetsLanded : AwardModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.IntegerNumber, guiName = "#autoLOC_8100022")]
	public int planetsLanded;

	public List<string> landedOn;

	public AwardModule_PlanetsLanded(MENode node)
		: base(node)
	{
		landedOn = new List<string>();
	}

	public AwardModule_PlanetsLanded(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
		landedOn = new List<string>();
	}

	public override bool EvaluateCondition(Mission mission)
	{
		return landedOn.Count >= planetsLanded;
	}

	public override void StartTracking()
	{
		GameEvents.onVesselSituationChange.Add(OnVesselSitutationChange);
	}

	public override void StopTracking()
	{
		GameEvents.onVesselSituationChange.Remove(OnVesselSitutationChange);
	}

	public void OnVesselSitutationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
	{
		if (data.to == Vessel.Situations.LANDED)
		{
			landedOn.AddUnique(data.host.lastBody.displayName);
		}
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("planetsLanded", ref planetsLanded);
		ConfigNode configNode = new ConfigNode();
		if (node.TryGetNode("PROGRESS", ref configNode))
		{
			landedOn = configNode.GetValuesList("landedOn");
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("planetsLanded", planetsLanded);
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			ConfigNode configNode = node.AddNode("PROGRESS");
			for (int i = 0; i < landedOn.Count; i++)
			{
				configNode.AddValue("landedOn", landedOn[i]);
			}
		}
	}
}
