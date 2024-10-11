using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FinePrint.Utilities;

public class SurveyDefinition
{
	public string DataName;

	public string AnomalyName;

	public string ResultName;

	public float FundsAdvance;

	public float FundsReward;

	public float FundsFailure;

	public float ScienceReward;

	public float ReputationReward;

	public float ReputationFailure;

	public List<SurveyDefinitionParameter> Parameters;

	public ConfigNode Node;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyDefinition(SurveyDefinition that)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyDefinition(ConfigNode node)
	{
		throw null;
	}
}
