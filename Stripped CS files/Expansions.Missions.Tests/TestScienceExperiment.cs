using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestScienceExperiment : TestModule, IScoreableObjective, INodeBody
{
	[MEGUI_Dropdown(addDefaultOption = true, SetDropDownItems = "TSEExpId_SetDropDownValues", defaultDisplayString = "#autoLOC_8002050", guiName = "#autoLOC_8000134")]
	public string experimentID = "";

	[MEGUI_Dropdown(addDefaultOption = true, SetDropDownItems = "TSEExpSit_SetDropDownValues", defaultDisplayString = "#autoLOC_8000258", guiName = "#autoLOC_8000135")]
	public string experimentSituation = "";

	[MEGUI_CelestialBody_Biomes(gapDisplay = true, guiName = "#autoLOC_8000136")]
	public MissionBiome biomeData;

	public bool testsSuccessful;

	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "TSEDoR_SetDropDownValues", guiName = "#autoLOC_8000137", Tooltip = "#autoLOC_8003055")]
	public string DeployorReceived = "";

	public bool passedTests;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000132");
		testsSuccessful = false;
		experimentID = "";
		experimentSituation = "";
		DeployorReceived = "Deployed";
		biomeData = new MissionBiome(FlightGlobals.GetHomeBody(), "");
	}

	public override void Initialized()
	{
		base.Initialized();
		GameEvents.OnScienceRecieved.Add(OnScienceReceived);
		GameEvents.OnExperimentStored.Add(OnExperimentDeployed);
	}

	public override void Cleared()
	{
		base.Cleared();
		GameEvents.OnScienceRecieved.Remove(OnScienceReceived);
		GameEvents.OnExperimentStored.Remove(OnExperimentDeployed);
	}

	public void OnScienceReceived(float scienceValue, ScienceSubject scienceSubjectIn, ProtoVessel vessel, bool reverseEngineered)
	{
		if (!(DeployorReceived != "Received"))
		{
			ConductScienceTest(scienceSubjectIn.id);
		}
	}

	public void OnExperimentDeployed(ScienceData scienceDataIn)
	{
		if (!(DeployorReceived != "Deployed"))
		{
			ConductScienceTest(scienceDataIn.subjectID);
		}
	}

	public override bool Test()
	{
		return testsSuccessful;
	}

	public void ConductScienceTest(string inputExperimentId)
	{
		passedTests = true;
		if (!string.IsNullOrEmpty(experimentID) && inputExperimentId.Split('@')[0] != experimentID)
		{
			passedTests = false;
		}
		if (!string.IsNullOrEmpty(experimentSituation) && !inputExperimentId.Contains(experimentSituation))
		{
			passedTests = false;
		}
		if (!string.IsNullOrEmpty(biomeData.biomeName))
		{
			if (biomeData.body != null && !inputExperimentId.Contains(biomeData.body.bodyName))
			{
				passedTests = false;
			}
			if (!inputExperimentId.Contains(biomeData.biomeName))
			{
				passedTests = false;
			}
		}
		testsSuccessful = passedTests;
	}

	public List<MEGUIDropDownItem> TSEExpId_SetDropDownValues()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		List<string> experimentIDs = ResearchAndDevelopment.GetExperimentIDs();
		for (int i = 0; i < experimentIDs.Count; i++)
		{
			ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(experimentIDs[i]);
			list.Add(new MEGUIDropDownItem(experiment.id, experiment.id, experiment.experimentTitle));
		}
		return list;
	}

	public List<MEGUIDropDownItem> TSEExpSit_SetDropDownValues()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		IEnumerator enumerator = Enum.GetValues(typeof(ExperimentSituations)).GetEnumerator();
		while (enumerator.MoveNext())
		{
			Enum @enum = (Enum)enumerator.Current;
			list.Add(new MEGUIDropDownItem(@enum.ToString(), @enum.ToString(), @enum.displayDescription()));
		}
		return list;
	}

	public List<MEGUIDropDownItem> TSEDoR_SetDropDownValues()
	{
		return new List<MEGUIDropDownItem>
		{
			new MEGUIDropDownItem("Deployed", "Deployed", Localizer.Format("#autoLOC_8001037")),
			new MEGUIDropDownItem("Received", "Received", Localizer.Format("#autoLOC_8000250"))
		};
	}

	public bool HasNodeBody()
	{
		if (biomeData != null)
		{
			return biomeData.body != null;
		}
		return false;
	}

	public CelestialBody GetNodeBody()
	{
		if (biomeData != null && biomeData.body != null)
		{
			return biomeData.body;
		}
		return null;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004030");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		if (!string.IsNullOrEmpty(experimentID))
		{
			node.AddValue("experimentID", experimentID);
		}
		if (!string.IsNullOrEmpty(experimentSituation))
		{
			node.AddValue("experimentSituation", experimentSituation);
		}
		node.AddValue("testsSuccessful", testsSuccessful);
		node.AddValue("DeployorReceived", DeployorReceived);
		if (biomeData != null)
		{
			biomeData.Save(node.AddNode("BIOMEDATA"));
		}
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		experimentID = "";
		experimentSituation = "";
		node.TryGetValue("experimentID", ref experimentID);
		node.TryGetValue("experimentSituation", ref experimentSituation);
		node.TryGetValue("testsSuccessful", ref testsSuccessful);
		node.TryGetValue("DeployorReceived", ref DeployorReceived);
		ConfigNode configNode = new ConfigNode();
		if (node.TryGetNode("BIOMEDATA", ref configNode))
		{
			biomeData.Load(configNode);
		}
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "biomeData")
		{
			if (!string.IsNullOrEmpty(biomeData.biomeName))
			{
				return Localizer.Format("#autoLOC_8000255", biomeData.body.bodyDisplayName, ScienceUtil.GetBiomedisplayName(biomeData.body, biomeData.biomeName));
			}
			return Localizer.Format("#autoLOC_8000269", biomeData.body.bodyDisplayName);
		}
		if (field.name == "experimentID")
		{
			if (!string.IsNullOrEmpty(experimentID))
			{
				ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(experimentID);
				if (experiment != null)
				{
					return Localizer.Format("#autoLOC_8000256", experiment.experimentTitle);
				}
			}
			return Localizer.Format("#autoLOC_8000256", Localizer.Format("#autoLOC_8002050"));
		}
		if (field.name == "experimentSituation")
		{
			if (!string.IsNullOrEmpty(experimentSituation))
			{
				ExperimentSituations experimentSituations = (ExperimentSituations)Enum.Parse(typeof(ExperimentSituations), experimentSituation);
				return Localizer.Format("#autoLOC_8000257", experimentSituations.displayDescription());
			}
			return Localizer.Format("#autoLOC_8000257", Localizer.Format("#autoLOC_8000258"));
		}
		if (field.name == "DeployorReceived")
		{
			return Localizer.Format("#autoLOC_8004190", field.guiName, (DeployorReceived == "Deployed") ? Localizer.Format("#autoLOC_8001037") : Localizer.Format("#autoLOC_8000250"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetAppObjectiveInfo()
	{
		if (biomeData != null && !string.IsNullOrEmpty(experimentID))
		{
			ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(experimentID);
			ExperimentSituations sit = (ExperimentSituations)((!string.IsNullOrEmpty(experimentSituation)) ? Enum.Parse(typeof(ExperimentSituations), experimentSituation) : ((object)(-1)));
			if (experiment != null)
			{
				string text = ScienceUtil.GenerateScienceSubjectTitle(experiment, sit, biomeData.body, biomeData.biomeName);
				text += "\n";
				if (DeployorReceived == "Deployed")
				{
					return text + Localizer.Format("#autoLOC_8002054");
				}
				return text + Localizer.Format("#autoLOC_8002055");
			}
		}
		return base.GetAppObjectiveInfo();
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (string.IsNullOrEmpty(experimentID))
		{
			return;
		}
		ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(experimentID);
		if (experiment != null && !string.IsNullOrEmpty(experimentSituation))
		{
			ExperimentSituations experimentSituations = (ExperimentSituations)Enum.Parse(typeof(ExperimentSituations), experimentSituation, ignoreCase: true);
			if (!experiment.IsAvailableWhile(experimentSituations, biomeData.body))
			{
				validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8002052", experimentSituations.displayDescription(), experiment.experimentTitle));
			}
			if (!string.IsNullOrEmpty(biomeData.biomeName) && !experiment.BiomeIsRelevantWhile(experimentSituations))
			{
				validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8002053", ScienceUtil.GetBiomedisplayName(biomeData.body, biomeData.biomeName), experiment.experimentTitle, biomeData.body.displayName));
			}
		}
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			return FlightGlobals.ActiveVessel;
		}
		return null;
	}
}
