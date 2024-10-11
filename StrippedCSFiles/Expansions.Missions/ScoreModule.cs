using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class ScoreModule : DynamicModule
{
	protected string awardedScoreDescription;

	protected IScoreableObjective scoreableObjective
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreModule(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float AwardScore(float currentScore)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string ScoreDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string AwarededScoreDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}
}
