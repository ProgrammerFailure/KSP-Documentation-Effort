using System;
using System.Collections;
using System.Collections.Generic;
using CommNet;
using Experience.Effects;
using ns11;
using ns19;
using ns9;
using UnityEngine;

public class ModuleScienceLab : PartModule, IResourceConsumer, IScienceDataContainer
{
	public List<string> ExperimentData;

	[KSPField]
	public float crewsRequired;

	[KSPField]
	public float SurfaceBonus = 0.1f;

	[KSPField]
	public float ContextBonus = 0.25f;

	[KSPField]
	public float homeworldMultiplier = 0.5f;

	[KSPField]
	public int containerModuleIndex;

	[KSPField]
	public bool canResetConnectedModules;

	[KSPField]
	public bool canResetNearbyModules;

	[KSPField]
	public float interactionRange = 5f;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001440")]
	public string statusText = "";

	public ModuleScienceConverter _converter;

	[KSPField(isPersistant = true)]
	public float dataStored;

	[KSPField]
	public float dataStorage = 500f;

	[KSPField(isPersistant = true)]
	public float storedScience;

	public bool processingData;

	public bool transmittingData;

	public List<ModuleResource> processResources;

	public ModuleScienceContainer container;

	public BaseEvent cleanModulesEvent;

	public BaseEvent transmitScienceEvent;

	public ScreenMessage progressMessage;

	public bool allResourcesAvailable;

	public List<PartResourceDefinition> consumedResources;

	public ScienceData[] emptyData = new ScienceData[0];

	public ModuleScienceConverter Converter => _converter ?? (_converter = base.part.FindModuleImplementing<ModuleScienceConverter>());

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001441")]
	public void TransmitScience()
	{
		sendDataToComms();
	}

	public void OnTransmissionComplete(ScienceData data, Vessel origin, bool xmitAborted)
	{
		if (data.container != base.part.flightID || !data.subjectID.StartsWith("sciencelab@"))
		{
			return;
		}
		transmittingData = false;
		updateModuleUI();
		if (!xmitAborted)
		{
			float dataAmount = data.dataAmount;
			progressMessage.message = Localizer.Format("#autoLOC_238985", base.vessel.GetDisplayName(), ResearchAndDevelopment.ScienceTransmissionRewardString(dataAmount));
			ScreenMessages.PostScreenMessage(progressMessage);
			if (ResearchAndDevelopment.Instance != null)
			{
				ResearchAndDevelopment.Instance.AddScience(storedScience, TransactionReasons.ScienceTransmission);
			}
			storedScience -= dataAmount;
		}
	}

	public override void OnAwake()
	{
		if (processResources == null)
		{
			processResources = new List<ModuleResource>();
		}
		GameEvents.onPartActionUICreate.Add(onPartActionUI);
		GameEvents.OnTriggeredDataTransmission.Add(OnTransmissionComplete);
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = processResources.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(processResources[i].name));
		}
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionUICreate.Remove(onPartActionUI);
		GameEvents.OnTriggeredDataTransmission.Remove(OnTransmissionComplete);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("RESOURCE_PROCESS"))
		{
			processResources.Clear();
			ConfigNode[] nodes = node.GetNodes("RESOURCE_PROCESS");
			int i = 0;
			for (int num = nodes.Length; i < num; i++)
			{
				ConfigNode node2 = nodes[i];
				ModuleResource moduleResource = new ModuleResource();
				moduleResource.Load(node2);
				processResources.Add(moduleResource);
			}
		}
		if (node.HasNode("EXPERIMENT_DATA"))
		{
			ExperimentData.Clear();
			ConfigNode node3 = node.GetNode("EXPERIMENT_DATA");
			int j = 0;
			for (int count = node3.values.Count; j < count; j++)
			{
				string value = node3.values[j].value;
				MonoBehaviour.print("Loading: " + value);
				ExperimentData.Add(value);
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		ConfigNode configNode = new ConfigNode("EXPERIMENT_DATA");
		int i = 0;
		for (int count = ExperimentData.Count; i < count; i++)
		{
			string value = ExperimentData[i];
			configNode.AddValue("experiment", value);
		}
		node.AddNode(configNode);
	}

	public override void OnStart(StartState state)
	{
		container = base.part.Modules.GetModule(containerModuleIndex) as ModuleScienceContainer;
		if (container == null)
		{
			Debug.LogError("[ModuleScienceLab]: No Container Module found at index " + containerModuleIndex, base.gameObject);
			return;
		}
		progressMessage = new ScreenMessage("", 8f, ScreenMessageStyle.UPPER_LEFT);
		cleanModulesEvent = base.Events["CleanModulesEvent"];
		transmitScienceEvent = base.Events["TransmitScience"];
		updateModuleUI();
	}

	public void onPartActionUI(Part p)
	{
		if (p == base.part)
		{
			updateModuleUI();
		}
	}

	public void updateModuleUI()
	{
		transmitScienceEvent.active = !transmittingData;
		int crewCountOfExperienceEffect = base.part.GetCrewCountOfExperienceEffect<ScienceSkill>();
		if ((float)crewCountOfExperienceEffect < crewsRequired)
		{
			statusText = Localizer.Format("#autoLOC_6001028", crewCountOfExperienceEffect, crewsRequired);
			cleanModulesEvent.active = false;
			return;
		}
		statusText = Localizer.Format("#autoLOC_239109");
		if (!canResetConnectedModules && !canResetNearbyModules)
		{
			cleanModulesEvent.active = false;
			return;
		}
		int num = 0;
		int count = FlightGlobals.ActiveVessel.Parts.Count;
		while (count-- > 0)
		{
			Part part = FlightGlobals.ActiveVessel.Parts[count];
			int count2 = part.Modules.Count;
			while (count2-- > 0)
			{
				PartModule partModule = part.Modules[count2];
				if (partModule is ModuleScienceExperiment && (partModule as ModuleScienceExperiment).Inoperable)
				{
					num++;
				}
			}
		}
		cleanModulesEvent.active = num > 0;
		cleanModulesEvent.guiActiveUnfocused = canResetNearbyModules;
		cleanModulesEvent.unfocusedRange = interactionRange;
		cleanModulesEvent.guiName = Localizer.Format("#autoLOC_6001863", num);
	}

	public bool IsStorable(ScienceData item)
	{
		if (item.labValue + dataStored > dataStorage)
		{
			progressMessage.message = Localizer.Format("#autoLOC_239144", base.part.partInfo.title, item.title);
			ScreenMessages.PostScreenMessage(progressMessage);
			return false;
		}
		if (ExperimentData.Contains(item.subjectID))
		{
			progressMessage.message = Localizer.Format("#autoLOC_239150", base.part.partInfo.title, item.title);
			ScreenMessages.PostScreenMessage(progressMessage);
			return false;
		}
		return true;
	}

	public IEnumerator ProcessData(ScienceData item, Callback<ScienceData> onComplete = null)
	{
		if (IsStorable(item))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_239163", item.title, item.labValue.ToString()), 5f, ScreenMessageStyle.UPPER_CENTER);
			StoreData(item);
			onComplete?.Invoke(item);
		}
		yield break;
	}

	public void StoreData(ScienceData item)
	{
		if (IsStorable(item))
		{
			ExperimentData.Add(item.subjectID);
			dataStored += item.labValue;
		}
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001864")]
	public void CleanModulesEvent()
	{
		StartCoroutine(CleanUpVesselExperiments(FlightGlobals.ActiveVessel));
	}

	public IEnumerator CleanUpVesselExperiments(Vessel v)
	{
		int i = v.Parts.Count;
		while (i-- > 0)
		{
			Part p = v.Parts[i];
			int j = p.Modules.Count;
			while (j-- > 0)
			{
				PartModule partModule = p.Modules[j];
				if (partModule is ModuleScienceExperiment)
				{
					ModuleScienceExperiment moduleScienceExperiment = partModule as ModuleScienceExperiment;
					if (moduleScienceExperiment.Inoperable)
					{
						yield return StartCoroutine(CleanUpExperiment(moduleScienceExperiment));
						yield return new WaitForSeconds(1f);
					}
				}
			}
		}
	}

	public IEnumerator CleanUpExperiment(ModuleScienceExperiment exp)
	{
		if (!exp.Inoperable)
		{
			Debug.LogError("[Lab Module]: " + exp.part.partInfo.title + " does not require cleanup", exp.gameObject);
			yield break;
		}
		if (exp.experiment == null)
		{
			Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. ScienceExperiment reference is Null.", exp.experimentID);
			yield break;
		}
		yield return StartCoroutine(gatherProcessResources(exp.resourceResetCost * exp.experiment.baseValue, exp.part.partInfo.title, "#autoLOC_6001029", "#autoLOC_6001030"));
		if (allResourcesAvailable)
		{
			int i = 0;
			for (int count = processResources.Count; i < count; i++)
			{
				ModuleResource moduleResource = processResources[i];
				moduleResource.currentAmount -= moduleResource.amount * (double)exp.resourceResetCost;
			}
			exp.Inoperable = false;
			exp.ResetExperiment();
			progressMessage.message = Localizer.Format("#autoLOC_239242", base.part.partInfo.title, exp.part.partInfo.title);
			ScreenMessages.PostScreenMessage(progressMessage);
		}
	}

	public IEnumerator lockdownAndGatherProcessResources(float amount, string expParttitle, string progressMessageText, string shortageMessageText)
	{
		yield return StartCoroutine(gatherProcessResources(amount, expParttitle, progressMessageText, shortageMessageText));
	}

	public IEnumerator gatherProcessResources(float amount, string expParttitle, string progressMessageText, string shortageMessageText)
	{
		allResourcesAvailable = true;
		int i = 0;
		int iC = processResources.Count;
		while (i < iC)
		{
			ModuleResource mr = processResources[i];
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(mr.id);
			string resourceName = ((definition == null) ? mr.title : definition.displayName);
			double required = mr.amount * (double)amount;
			mr.rate = mr.amount * (double)Time.deltaTime;
			while (Math.Ceiling(mr.currentAmount) < Math.Floor(required))
			{
				mr.currentRequest = Math.Min(mr.rate, required - mr.currentAmount);
				double num = base.part.RequestResource(mr.id, mr.currentRequest);
				mr.currentAmount += num;
				string text;
				if (num <= 0.0)
				{
					text = "(" + mr.currentAmount.ToString("0") + "/" + (mr.amount * (double)amount).ToString("0") + ")";
					progressMessage.message = "<color=orange>" + Localizer.Format(shortageMessageText, base.part.partInfo.title, resourceName, expParttitle, text) + "</color>";
					ScreenMessages.PostScreenMessage(progressMessage);
					allResourcesAvailable = false;
					yield break;
				}
				text = "[" + (mr.currentAmount / required * 100.0).ToString("0") + "%]";
				progressMessage.message = "<color=#99ff0099>" + Localizer.Format(progressMessageText, base.part.partInfo.title, expParttitle, text) + "</color>";
				ScreenMessages.PostScreenMessage(progressMessage);
				yield return null;
			}
			int num2 = i + 1;
			i = num2;
		}
	}

	public bool IsOperational()
	{
		if ((float)base.part.GetCrewCountOfExperienceEffect<ScienceSkill>() < crewsRequired)
		{
			return false;
		}
		return true;
	}

	public override string GetInfo()
	{
		return string.Concat("" + Localizer.Format("#autoLOC_239317", crewsRequired.ToString("0.0###")), Localizer.Format("#autoLOC_239318", dataStorage.ToString("0.0###")));
	}

	public void TransmissionErrorScreenMessage(string reason)
	{
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003223", reason), 3f, ScreenMessageStyle.UPPER_CENTER);
	}

	public void sendDataToComms()
	{
		if (transmittingData)
		{
			TransmissionErrorScreenMessage(Localizer.Format("#autoLOC_239331"));
			return;
		}
		if (storedScience < 1f)
		{
			TransmissionErrorScreenMessage(Localizer.Format("#autoLOC_239337"));
			return;
		}
		IScienceDataTransmitter bestTransmitter = ScienceUtil.GetBestTransmitter(base.vessel);
		if (bestTransmitter != null)
		{
			ScienceData item = new ScienceData((float)Math.Floor(storedScience), 1f, 0f, "sciencelab@" + base.vessel.mainBody.bodyName, Localizer.Format("#autoLOC_239346"), triggered: true, base.part.flightID);
			List<ScienceData> list = new List<ScienceData>();
			list.Add(item);
			bestTransmitter.TransmitData(list);
			transmittingData = true;
		}
		else
		{
			if (CommNetScenario.CommNetEnabled)
			{
				TransmissionErrorScreenMessage(Localizer.Format("#autoLOC_239357"));
			}
			else
			{
				TransmissionErrorScreenMessage(Localizer.Format("#autoLOC_239359"));
			}
			updateModuleUI();
		}
	}

	public static void RecoverScienceLabs(ProtoVessel protoVessel, MissionRecoveryDialog dialog)
	{
		if (ResearchAndDevelopment.Instance == null)
		{
			return;
		}
		float num = 0f;
		int count = protoVessel.protoPartSnapshots.Count;
		while (count-- > 0)
		{
			float num2 = 0f;
			ProtoPartSnapshot protoPartSnapshot = protoVessel.protoPartSnapshots[count];
			int count2 = protoPartSnapshot.modules.Count;
			while (count2-- > 0)
			{
				ProtoPartModuleSnapshot protoPartModuleSnapshot = protoPartSnapshot.modules[count2];
				if (protoPartModuleSnapshot.moduleName != "ModuleScienceLab")
				{
					continue;
				}
				string[] values = protoPartModuleSnapshot.moduleValues.GetValues("storedScience");
				int num3 = values.Length;
				while (num3-- > 0)
				{
					float result = 0f;
					if (float.TryParse(values[num3], out result) && result > Mathf.Epsilon)
					{
						num2 += result;
					}
				}
			}
			if (!(num2 <= 0f))
			{
				num += num2;
				if (!(dialog == null))
				{
					ScienceSubjectWidget widget = ScienceSubjectWidget.Create(new ScienceSubject(string.Empty, protoPartSnapshot.partInfo.title, 0f, 0f, 0f), num2, num2, dialog);
					dialog.AddDataWidget(widget);
					dialog.scienceEarned += num2;
				}
			}
		}
		if (num > 0f)
		{
			ResearchAndDevelopment.Instance.AddScience(num, TransactionReasons.VesselRecovery);
		}
	}

	ScienceData[] IScienceDataContainer.GetData()
	{
		return emptyData;
	}

	void IScienceDataContainer.ReturnData(ScienceData data)
	{
	}

	void IScienceDataContainer.DumpData(ScienceData data)
	{
	}

	void IScienceDataContainer.ReviewData()
	{
	}

	void IScienceDataContainer.ReviewDataItem(ScienceData data)
	{
	}

	int IScienceDataContainer.GetScienceCount()
	{
		return 0;
	}

	bool IScienceDataContainer.IsRerunnable()
	{
		return false;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003060");
	}
}
