using System.Collections.Generic;

namespace FinePrint.Utilities;

public class SurveyDefinitionParameter
{
	public string Experiment = "experiment";

	public string Description = "Perform a survey";

	public string Texture = "default";

	public bool AllowGround = true;

	public bool AllowLow = true;

	public bool AllowHigh = true;

	public bool AllowWater = true;

	public bool AllowVacuum = true;

	public bool CrewRequired;

	public bool EVARequired;

	public float FundsMultiplier = 1f;

	public float ScienceMultiplier = 1f;

	public float ReputationMultiplier = 1f;

	public List<string> Tech;

	public ConfigNode Node;

	public SurveyDefinition Definition;

	public SurveyDefinitionParameter()
	{
		Node = new ConfigNode("PARAM");
		Tech = new List<string>();
		Definition = new SurveyDefinition();
	}

	public SurveyDefinitionParameter(SurveyDefinitionParameter that)
	{
		Experiment = that.Experiment;
		Description = that.Description;
		Texture = that.Texture;
		AllowGround = that.AllowGround;
		AllowLow = that.AllowLow;
		AllowHigh = that.AllowHigh;
		AllowWater = that.AllowWater;
		AllowVacuum = that.AllowVacuum;
		CrewRequired = that.CrewRequired;
		EVARequired = that.EVARequired;
		FundsMultiplier = that.FundsMultiplier;
		ScienceMultiplier = that.ScienceMultiplier;
		ReputationMultiplier = that.ReputationMultiplier;
		Tech = new List<string>();
		int count = that.Tech.Count;
		while (count-- > 0)
		{
			Tech.Add(that.Tech[count]);
		}
		Node = that.Node;
		Definition = that.Definition;
		if (EVARequired)
		{
			CrewRequired = true;
		}
	}

	public SurveyDefinitionParameter(SurveyDefinition definition, ConfigNode node)
	{
		if (definition != null && node != null && !(node.name != "PARAM"))
		{
			Node = node;
			Definition = definition;
			SystemUtilities.LoadNode(node, "SurveyContract", "Experiment", ref Experiment, "experiment", logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "Description", ref Description, "Perform a survey", logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "Texture", ref Texture, "default", logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "AllowGround", ref AllowGround, defaultValue: true, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "AllowLow", ref AllowLow, defaultValue: true, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "AllowHigh", ref AllowHigh, defaultValue: true, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "AllowWater", ref AllowWater, defaultValue: true, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "AllowVacuum", ref AllowVacuum, defaultValue: true, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "CrewRequired", ref CrewRequired, defaultValue: false, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "EVARequired", ref EVARequired, defaultValue: false, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "FundsMultiplier", ref FundsMultiplier, 1f, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "ScienceMultiplier", ref ScienceMultiplier, 1f, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "ReputationMultiplier", ref ReputationMultiplier, 1f, logging: false);
			Tech = new List<string>(node.GetValues("Tech"));
			if (EVARequired)
			{
				CrewRequired = true;
			}
		}
	}
}
