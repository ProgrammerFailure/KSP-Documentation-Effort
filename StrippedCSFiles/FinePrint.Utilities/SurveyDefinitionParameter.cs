using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FinePrint.Utilities;

public class SurveyDefinitionParameter
{
	public string Experiment;

	public string Description;

	public string Texture;

	public bool AllowGround;

	public bool AllowLow;

	public bool AllowHigh;

	public bool AllowWater;

	public bool AllowVacuum;

	public bool CrewRequired;

	public bool EVARequired;

	public float FundsMultiplier;

	public float ScienceMultiplier;

	public float ReputationMultiplier;

	public List<string> Tech;

	public ConfigNode Node;

	public SurveyDefinition Definition;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyDefinitionParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyDefinitionParameter(SurveyDefinitionParameter that)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyDefinitionParameter(SurveyDefinition definition, ConfigNode node)
	{
		throw null;
	}
}
