using System.Collections.Generic;
using Expansions.Missions.Editor;
using Expansions.Missions.Runtime;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

public class AwardModule : DynamicModule
{
	public bool enabled;

	public float score;

	public AwardDefinition _definition;

	public string awardID;

	public AwardDefinition Definition
	{
		get
		{
			if (_definition == null && !string.IsNullOrEmpty(awardID) && (MissionEditorLogic.Instance != null || MissionSystem.Instance != null))
			{
				Awards awards = (HighLogic.LoadedSceneIsMissionBuilder ? MissionEditorLogic.Instance.awardDefinitions : MissionSystem.awardDefinitions);
				if (awards != null)
				{
					_definition = awards.GetAwardDefinition(awardID);
				}
			}
			return _definition;
		}
	}

	public AwardModule(MENode node)
		: base(node)
	{
		enabled = false;
	}

	public AwardModule(MENode node, AwardDefinition definition)
		: this(node)
	{
		awardID = definition.id;
	}

	public override string GetDisplayName()
	{
		if (Definition != null)
		{
			return Definition.displayName;
		}
		return base.GetDisplayName();
	}

	public virtual bool EvaluateCondition(Mission mission)
	{
		return false;
	}

	public void EvaluateAward(Mission mission, ref List<string> awards)
	{
		if (awards == null)
		{
			awards = new List<string>();
		}
		if (EvaluateCondition(mission) && Definition != null)
		{
			awards.Add(Definition.id);
			Debug.Log("[MissionAwards]: <b><color=green> Award earned " + Localizer.Format(Definition.displayName) + ". </color></b>");
		}
	}

	public virtual void StartTracking()
	{
	}

	public virtual void StopTracking()
	{
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("enabled", ref enabled);
		node.TryGetValue("score", ref score);
		node.TryGetValue("awardID", ref awardID);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("awardID", awardID);
		node.AddValue("enabled", enabled);
		node.AddValue("score", score);
	}
}
