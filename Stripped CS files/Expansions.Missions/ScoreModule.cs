using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class ScoreModule : DynamicModule
{
	public string awardedScoreDescription;

	public IScoreableObjective scoreableObjective
	{
		get
		{
			if (node.dockParentNode != null)
			{
				return node.dockParentNode.testGroups[0].testModules[0] as IScoreableObjective;
			}
			return null;
		}
	}

	public ScoreModule()
	{
	}

	public ScoreModule(MENode node)
		: base(node)
	{
	}

	public virtual float AwardScore(float currentScore)
	{
		return currentScore;
	}

	public virtual string ScoreDescription()
	{
		return "";
	}

	public string AwarededScoreDescription()
	{
		return awardedScoreDescription;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		return base.GetNodeBodyParameterString(field);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("awardedScoreDescription", ref awardedScoreDescription);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		if (!string.IsNullOrEmpty(awardedScoreDescription))
		{
			node.AddValue("awardedScoreDescription", awardedScoreDescription);
		}
	}
}
