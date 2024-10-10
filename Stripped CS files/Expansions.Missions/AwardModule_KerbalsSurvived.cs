namespace Expansions.Missions;

public class AwardModule_KerbalsSurvived : AwardModule
{
	public bool kerbalsAreAlive = true;

	public AwardModule_KerbalsSurvived(MENode node)
		: base(node)
	{
	}

	public AwardModule_KerbalsSurvived(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
	}

	public override bool EvaluateCondition(Mission mission)
	{
		return kerbalsAreAlive;
	}

	public override void StartTracking()
	{
		GameEvents.onCrewKilled.Add(OnCrewKilled);
	}

	public override void StopTracking()
	{
		GameEvents.onCrewKilled.Remove(OnCrewKilled);
	}

	public void OnCrewKilled(EventReport report)
	{
		kerbalsAreAlive = false;
		StopTracking();
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		ConfigNode configNode = new ConfigNode();
		if (node.TryGetNode("PROGRESS", ref configNode))
		{
			configNode.TryGetValue("kerbalsAreAlive", ref kerbalsAreAlive);
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			node.AddNode("PROGRESS").AddValue("kerbalsAreAlive", kerbalsAreAlive);
		}
	}
}
