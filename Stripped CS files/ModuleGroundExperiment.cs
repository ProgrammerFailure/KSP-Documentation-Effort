using Expansions;
using Expansions.Serenity.DeployedScience.Runtime;
using ns9;
using UnityEngine;

public class ModuleGroundExperiment : ModuleGroundSciencePart
{
	[KSPField]
	public string experimentId;

	[KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false)]
	[SerializeField]
	public float scienceLimit;

	[SerializeField]
	public float scienceValue;

	[KSPField(unfocusedRange = 20f, guiActiveUnfocused = true, isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8002248", guiUnits = "#autoLOC_8002249")]
	[SerializeField]
	public float scienceValueDisplay;

	[KSPField(guiActiveUnfocused = true, guiFormat = "F2", isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002250", guiUnits = "%")]
	public float ScienceCompletedPercentage;

	[KSPField(guiActiveUnfocused = true, guiFormat = "F2", isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002257", guiUnits = "%")]
	public float ScienceTransmittedPercentage;

	[KSPField(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002245", guiUnits = "%")]
	public float ScienceModifierRate = 5f;

	[KSPField]
	public FloatCurve distanceCurve = new FloatCurve();

	public DeployedScienceExperiment scienceExperiment;

	public DeployedSciencePart deployedSciencePart;

	public ScienceExperiment experiment;

	public static string cacheAutoLOC_8002251;

	public float ScienceLimit
	{
		get
		{
			return scienceLimit;
		}
		set
		{
			if (value != scienceLimit)
			{
				scienceLimit = value;
				GameEvents.onGroundScienceExperimentScienceLimitChanged.Fire(this);
			}
		}
	}

	public float ScienceValue
	{
		get
		{
			return scienceValue;
		}
		set
		{
			if (value != scienceValue)
			{
				scienceValue = value;
				GameEvents.onGroundScienceExperimentScienceValueChanged.Fire(this);
			}
		}
	}

	public new void Start()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity") && HighLogic.LoadedSceneIsGame)
		{
			base.enabled = false;
			Object.Destroy(this);
		}
		experiment = ResearchAndDevelopment.GetExperiment(experimentId);
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		GameEvents.onGroundScienceGenerated.Add(OnGroundScienceGenerated);
		GameEvents.onGroundScienceTransmitted.Add(OnGroundScienceTransmitted);
		if (experimentId == DeployedScience.SeismicExperimentId)
		{
			base.Fields["scienceValueDisplay"].guiActive = false;
		}
		scienceValueDisplay = scienceValue;
	}

	public override void OnDestroy()
	{
		GameEvents.onGroundScienceGenerated.Remove(OnGroundScienceGenerated);
		GameEvents.onGroundScienceTransmitted.Remove(OnGroundScienceTransmitted);
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		if ((bool)DeployedScience.Instance && DeployedScience.IsActive && ExpansionsLoader.IsExpansionInstalled("Serenity") && base.ScienceClusterData != null)
		{
			deployedSciencePart = base.ScienceClusterData.DeployedScienceParts.Get(base.part.persistentId);
			if (deployedSciencePart != null && deployedSciencePart.Experiment != null)
			{
				scienceLimit = deployedSciencePart.Experiment.ScienceLimit;
				scienceValue = deployedSciencePart.Experiment.ScienceValue;
				ScienceModifierRate = deployedSciencePart.Experiment.ScienceModifierRate;
			}
		}
	}

	public void OnGroundScienceGenerated(DeployedScienceExperiment scienceExperiment, DeployedSciencePart sciencePart, DeployedScienceCluster cluster, float scienceGenerated)
	{
		if (sciencePart.PartId == base.part.persistentId)
		{
			UpdateModuleUI();
		}
	}

	public void OnGroundScienceTransmitted(DeployedScienceExperiment scienceExperiment, DeployedSciencePart sciencePart, DeployedScienceCluster cluster, float scienceTransmitted)
	{
		if (sciencePart.PartId == base.part.persistentId)
		{
			UpdateModuleUI();
		}
	}

	public override void UpdateModuleUI()
	{
		base.UpdateModuleUI();
		if (scienceExperiment == null && base.ScienceClusterData != null)
		{
			DeployedSciencePart deployedSciencePart = base.ScienceClusterData.DeployedScienceParts.GetExperiment(experimentId);
			if (deployedSciencePart != null)
			{
				scienceExperiment = deployedSciencePart.Experiment;
			}
		}
		if (scienceExperiment != null)
		{
			ScienceCompletedPercentage = ((scienceLimit > 0f) ? (scienceExperiment.TotalScienceGenerated / scienceExperiment.ScienceLimit * 100f) : 0f);
			ScienceTransmittedPercentage = ((scienceExperiment.TransmittedScienceData > 0f) ? (scienceExperiment.TransmittedScienceData / scienceExperiment.ScienceLimit * 100f) : 0f);
		}
		else
		{
			ScienceCompletedPercentage = 0f;
			ScienceTransmittedPercentage = 0f;
		}
		if ((!base.Enabled || !base.DeployedOnGround) && scienceExperiment != null && !scienceExperiment.ExperimentSituationValid)
		{
			PowerState = cacheAutoLOC_8002251;
		}
		if (base.Enabled && base.DeployedOnGround && !(scienceExperiment == null) && scienceExperiment.ExperimentSituationValid && (!(base.ScienceClusterData != null) || base.ScienceClusterData.IsPowered))
		{
			scienceValueDisplay = scienceValue;
		}
		else
		{
			scienceValueDisplay = 0f;
		}
	}

	public override void RetrievePart()
	{
		if (!CanBeStored())
		{
			return;
		}
		AvailablePart availablePart = ((base.part != null) ? base.part.partInfo : null);
		if (availablePart != null)
		{
			ModuleGroundExperiment moduleGroundExperiment = availablePart.partPrefab.FindModuleImplementing<ModuleGroundExperiment>();
			if (moduleGroundExperiment != null)
			{
				ScienceModifierRate = moduleGroundExperiment.ScienceModifierRate;
			}
		}
		base.RetrievePart();
	}

	public override string GetInfo()
	{
		string text = "";
		text += base.GetInfo();
		if (experiment == null)
		{
			experiment = ResearchAndDevelopment.GetExperiment(experimentId);
		}
		if (experiment != null)
		{
			text = text + "<color=" + XKCDColors.HexFormat.Cyan + ">" + experiment.experimentTitle + "</color>\n";
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8002224"), RUIutils.GetYesNoUIString(experiment.requireAtmosphere)) + "\n";
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8002226"), RUIutils.GetYesNoUIString(experiment.requireNoAtmosphere)) + "\n";
		}
		return text;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8002225");
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_8002251 = Localizer.Format("#autoLOC_8002251");
	}
}
