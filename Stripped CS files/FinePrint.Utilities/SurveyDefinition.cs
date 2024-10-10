using System.Collections.Generic;

namespace FinePrint.Utilities;

public class SurveyDefinition
{
	public string DataName = "generic";

	public string AnomalyName = "anomalies";

	public string ResultName = "report";

	public float FundsAdvance = ContractDefs.Survey.Funds.DefaultAdvance;

	public float FundsReward = ContractDefs.Survey.Funds.DefaultReward;

	public float FundsFailure = ContractDefs.Survey.Funds.DefaultFailure;

	public float ScienceReward = ContractDefs.Survey.Science.DefaultReward;

	public float ReputationReward = ContractDefs.Survey.Reputation.DefaultReward;

	public float ReputationFailure = ContractDefs.Survey.Reputation.DefaultFailure;

	public List<SurveyDefinitionParameter> Parameters;

	public ConfigNode Node;

	public SurveyDefinition()
	{
		Node = new ConfigNode("SURVEY_DEFINITION");
		Parameters = new List<SurveyDefinitionParameter>();
	}

	public SurveyDefinition(SurveyDefinition that)
	{
		DataName = that.DataName;
		AnomalyName = that.AnomalyName;
		ResultName = that.ResultName;
		FundsAdvance = that.FundsAdvance;
		FundsReward = that.FundsReward;
		FundsFailure = that.FundsFailure;
		ScienceReward = that.ScienceReward;
		ReputationReward = that.ReputationReward;
		ReputationFailure = that.ReputationFailure;
		Parameters = new List<SurveyDefinitionParameter>();
		int count = that.Parameters.Count;
		while (count-- > 0)
		{
			Parameters.Add(new SurveyDefinitionParameter(that.Parameters[count]));
		}
		Node = that.Node;
	}

	public SurveyDefinition(ConfigNode node)
	{
		if (node != null && !(node.name != "SURVEY_DEFINITION"))
		{
			Node = node;
			SystemUtilities.LoadNode(node, "SurveyContract", "DataName", ref DataName, "generic", logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "AnomalyName", ref AnomalyName, "anomaly", logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "ResultName", ref ResultName, "report", logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "FundsAdvance", ref FundsAdvance, ContractDefs.Survey.Funds.DefaultAdvance, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "FundsReward", ref FundsReward, ContractDefs.Survey.Funds.DefaultReward, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "FundsFailure", ref FundsFailure, ContractDefs.Survey.Funds.DefaultFailure, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "ScienceReward", ref ScienceReward, ContractDefs.Survey.Science.DefaultReward, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "ReputationReward", ref ReputationReward, ContractDefs.Survey.Reputation.DefaultReward, logging: false);
			SystemUtilities.LoadNode(node, "SurveyContract", "ReputationFailure", ref ReputationFailure, ContractDefs.Survey.Reputation.DefaultFailure, logging: false);
			ConfigNode[] nodes = node.GetNodes("PARAM");
			Parameters = new List<SurveyDefinitionParameter>();
			int num = nodes.Length;
			while (num-- > 0)
			{
				Parameters.Add(new SurveyDefinitionParameter(this, nodes[num]));
			}
		}
	}
}
