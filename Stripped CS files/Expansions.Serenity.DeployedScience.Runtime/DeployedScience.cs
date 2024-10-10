using System.Collections.Generic;
using ns11;
using UnityEngine;

namespace Expansions.Serenity.DeployedScience.Runtime;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class DeployedScience : ScenarioModule
{
	public DictionaryValueList<uint, DeployedScienceCluster> deployedScienceClusters;

	public static GameObject deployedScienceGameObject;

	public DictionaryValueList<int, float> diminishingReturns;

	public static string SeismicExperimentId = "deployedSeismicSensor";

	public DictionaryValueList<string, float> seismicEnergyRates;

	public float MinimumSeismicEnergyRequired = 3000f;

	public int SeismicScienceProcessingDelay = 3;

	public double ScienceTimeDelay = 60.0;

	public double DataSendFailedTimeDelay = 600.0;

	public static DeployedScience Instance { get; set; }

	public static bool IsActive { get; set; }

	public DictionaryValueList<uint, DeployedScienceCluster> DeployedScienceClusters => deployedScienceClusters;

	public static GameObject DeployedScienceGameObject
	{
		get
		{
			if (deployedScienceGameObject == null)
			{
				deployedScienceGameObject = new GameObject("DeployedScienceGameObject");
				Object.DontDestroyOnLoad(deployedScienceGameObject);
			}
			return deployedScienceGameObject;
		}
	}

	public DictionaryValueList<int, float> DiminishingReturns => diminishingReturns;

	public DictionaryValueList<string, float> SeismicEnergyRates => seismicEnergyRates;

	public override void OnAwake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			if (Instance != null)
			{
				Object.Destroy(Instance);
			}
			Object.Destroy(this);
			return;
		}
		if (Instance != null && Instance != this)
		{
			Debug.LogError("[DeployedScience Module]: Instance already exists!", Instance.gameObject);
			Object.Destroy(this);
			return;
		}
		Instance = this;
		IsActive = false;
		deployedScienceClusters = new DictionaryValueList<uint, DeployedScienceCluster>();
		GameEvents.onGroundScienceRegisterCluster.Add(RegisterCluster);
		GameEvents.onGroundScienceDeregisterCluster.Add(DeRegisterCluster);
		GameEvents.onGroundScienceControllerChanged.Add(UpdateCluster);
		GameEvents.onVesselRecovered.Add(OnVesselRecovered);
		GameEvents.onVesselTerminated.Add(OnVesselTerminated);
		GameEvents.onVesselWillDestroy.Add(OnVesselWillDestroy);
		GameEvents.onPartExplodeGroundCollision.Add(OnPartExplodeGroundCollision);
		GameEvents.onVesselExplodeGroundCollision.Add(OnVesselExplodeGroundCollision);
		GameEvents.onGameStatePostLoad.Add(OnGameStateLoad);
		GameEvents.onGameStateLoad.Add(OnGameStateLoad);
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		GameEvents.onGroundScienceRegisterCluster.Remove(RegisterCluster);
		GameEvents.onGroundScienceDeregisterCluster.Remove(DeRegisterCluster);
		GameEvents.onGroundScienceControllerChanged.Remove(UpdateCluster);
		GameEvents.onVesselRecovered.Remove(OnVesselRecovered);
		GameEvents.onVesselTerminated.Remove(OnVesselTerminated);
		GameEvents.onVesselWillDestroy.Remove(OnVesselWillDestroy);
		GameEvents.onVesselExplodeGroundCollision.Remove(OnVesselExplodeGroundCollision);
		GameEvents.onGameStatePostLoad.Remove(OnGameStateLoad);
		GameEvents.onGameStateLoad.Remove(OnGameStateLoad);
		GameEvents.onPartExplodeGroundCollision.Remove(OnPartExplodeGroundCollision);
		RemoveMannedScienceObjects();
	}

	public void FixedUpdate()
	{
		for (int i = 0; i < deployedScienceClusters.ValuesList.Count; i++)
		{
			DeployedScienceCluster deployedScienceCluster = deployedScienceClusters.ValuesList[i];
			if (!(deployedScienceCluster != null))
			{
				continue;
			}
			bool flag = false;
			if (deployedScienceCluster.UpdatingScience)
			{
				continue;
			}
			if (deployedScienceCluster.DataSendFailed)
			{
				if ((Planetarium.GetUniversalTime() - deployedScienceCluster.LastScienceTransmittedUT) * (double)TimeWarp.CurrentRate > DataSendFailedTimeDelay * (double)TimeWarp.CurrentRate)
				{
					flag = true;
				}
			}
			else if ((Planetarium.GetUniversalTime() - deployedScienceCluster.LastScienceGeneratedUT) * (double)TimeWarp.CurrentRate > ScienceTimeDelay * (double)TimeWarp.CurrentRate)
			{
				flag = true;
			}
			if (flag && !deployedScienceCluster.UpdatingScience)
			{
				deployedScienceCluster.UpdatingScience = true;
				StartCoroutine(deployedScienceCluster.UpdateScience());
			}
		}
	}

	public void UpdateSciencePart(ModuleGroundSciencePart sciencePart)
	{
		ModuleGroundExperiment component = sciencePart.gameObject.GetComponent<ModuleGroundExperiment>();
		if (component != null)
		{
			SetDiminishingScienceRate(component.experimentId, sciencePart.vessel.mainBody.name);
		}
	}

	public void UpdateCluster(ModuleGroundExpControl controlUnit, bool replaceList, List<ModuleGroundSciencePart> deployableScienceParts)
	{
		if (!deployedScienceClusters.ContainsKey(controlUnit.part.persistentId))
		{
			RegisterCluster(controlUnit, deployableScienceParts);
		}
	}

	public void OnVesselRecovered(ProtoVessel vessel, bool quick)
	{
		for (int i = 0; i < vessel.protoPartSnapshots.Count; i++)
		{
			if (deployedScienceClusters.ContainsKey(vessel.protoPartSnapshots[i].persistentId))
			{
				DeRegisterCluster(vessel.protoPartSnapshots[i].persistentId);
			}
		}
	}

	public void OnVesselTerminated(ProtoVessel vessel)
	{
		OnVesselRecovered(vessel, quick: false);
	}

	public void OnVesselWillDestroy(Vessel vessel)
	{
		for (int i = 0; i < vessel.parts.Count; i++)
		{
			if (deployedScienceClusters.ContainsKey(vessel.parts[i].persistentId))
			{
				DeRegisterCluster(vessel.parts[i].persistentId);
			}
		}
	}

	public void OnPartExplodeGroundCollision(Part part)
	{
		if (part.vessel != null)
		{
			if (part.vessel.parts.Count == 0)
			{
				part.vessel.parts.Add(part);
			}
			ScanAndProcessSeismicEvent(part.vessel, part);
		}
		else
		{
			Debug.LogFormat("[DeployedScience]: Part {0} {1} Exploded on Ground collision, but Vessel was null. Unable to process.", part.partInfo.title, part.persistentId);
		}
	}

	public void OnVesselExplodeGroundCollision(Vessel vessel)
	{
		if (!(vessel == null) && !(vessel.mainBody == null))
		{
			ScanAndProcessSeismicEvent(vessel, null);
		}
		else
		{
			Debug.Log("[DeployedScience]: Seismic Event Cannot be processed - no Vessel data.");
		}
	}

	public void ScanAndProcessSeismicEvent(Vessel vessel, Part part)
	{
		if (!(vessel == null) && !(vessel.mainBody == null))
		{
			for (int i = 0; i < deployedScienceClusters.ValuesList.Count; i++)
			{
				if (deployedScienceClusters.ValuesList[i].DeployedBody.name == vessel.mainBody.name)
				{
					DeployedSciencePart experiment = deployedScienceClusters.ValuesList[i].DeployedScienceParts.GetExperiment(SeismicExperimentId);
					if (experiment != null && experiment.Experiment != null)
					{
						experiment.Experiment.CalcSendSeismicScience(vessel, part, deployedScienceClusters.ValuesList[i].PartialPowerMultiplier);
					}
					else if (experiment != null)
					{
						Debug.LogWarning("[DeployedScience]: CalcSeismicScience - unable to calculate science. DeployedSciencePart Experiment is null.");
					}
				}
			}
		}
		else
		{
			Debug.Log("[DeployedScience]: Seismic Event Cannot be processed - no Vessel data.");
		}
	}

	public void OnGameStateLoad(ConfigNode node)
	{
		for (int num = deployedScienceClusters.ValuesList.Count - 1; num >= 0; num--)
		{
			DeployedScienceCluster deployedScienceCluster = deployedScienceClusters.ValuesList[num];
			if (deployedScienceCluster != null)
			{
				if (!FlightGlobals.FindLoadedPart(deployedScienceCluster.ControlModulePartId, out var partout) && !FlightGlobals.FindUnloadedPart(deployedScienceCluster.ControlModulePartId, out var partout2))
				{
					Debug.LogFormat("[DeployedScience]: Unable to find Manned Science controller with Id {0} in current Game. Removed.", deployedScienceCluster.ControlModulePartId);
					deployedScienceCluster.gameObject.DestroyGameObject();
					deployedScienceClusters.Remove(deployedScienceClusters.KeysList[num]);
				}
				else
				{
					for (int num2 = deployedScienceCluster.DeployedScienceParts.Count - 1; num2 >= 0; num2--)
					{
						if (!FlightGlobals.FindLoadedPart(deployedScienceCluster.DeployedScienceParts[num2].PartId, out partout) && !FlightGlobals.FindUnloadedPart(deployedScienceCluster.DeployedScienceParts[num2].PartId, out partout2))
						{
							Debug.LogFormat("[DeployedScience]: Cluster with Controller Id {0}, Unable to find Manned Science Part with Id {1} in current Game. Removed Science Part.", deployedScienceCluster.ControlModulePartId, deployedScienceCluster.DeployedScienceParts[num2].PartId);
							if (deployedScienceCluster.SolarPanelParts.Contains(deployedScienceCluster.DeployedScienceParts[num2]))
							{
								deployedScienceCluster.solarPanelParts.Remove(deployedScienceCluster.DeployedScienceParts[num2]);
							}
							if (deployedScienceCluster.antennaParts.Contains(deployedScienceCluster.DeployedScienceParts[num2]))
							{
								deployedScienceCluster.antennaParts.Remove(deployedScienceCluster.DeployedScienceParts[num2]);
							}
							deployedScienceCluster.DeployedScienceParts[num2].gameObject.DestroyGameObject();
							deployedScienceCluster.DeployedScienceParts.RemoveAt(num2);
						}
					}
				}
			}
		}
		IsActive = true;
	}

	public float ExpDimReturnsRate(string experimentId, string celestialBodyName)
	{
		int num = 0;
		for (int i = 0; i < DeployedScienceClusters.ValuesList.Count; i++)
		{
			DeployedScienceCluster deployedScienceCluster = DeployedScienceClusters.ValuesList[i];
			if (!deployedScienceCluster.IsPowered)
			{
				continue;
			}
			for (int j = 0; j < deployedScienceCluster.DeployedScienceParts.Count; j++)
			{
				DeployedSciencePart deployedSciencePart = deployedScienceCluster.DeployedScienceParts[j];
				if (deployedSciencePart.Experiment != null && deployedSciencePart.Experiment.ExperimentVessel != null && deployedSciencePart.Experiment.ExperimentVessel.mainBody.name == celestialBodyName && deployedSciencePart.Experiment.ExperimentId == experimentId)
				{
					num++;
				}
			}
		}
		return GetDiminishingReturnsMultiplier(num);
	}

	public float GetDiminishingReturnsMultiplier(int numExperiments)
	{
		float result = 1f;
		bool flag = false;
		for (int i = 0; i < diminishingReturns.KeysList.Count; i++)
		{
			if (diminishingReturns.KeysList[i] <= numExperiments)
			{
				if (diminishingReturns.KeysList[i] == numExperiments)
				{
					flag = true;
					result = diminishingReturns.ValuesList[i];
				}
				else if (!flag)
				{
					result = diminishingReturns.ValuesList[i];
				}
			}
		}
		return result;
	}

	public DeployedSciencePart ExpMaxScienceMultiplier(string experimentId, string celestialBodyName)
	{
		float num = 0f;
		DeployedSciencePart result = null;
		for (int i = 0; i < DeployedScienceClusters.ValuesList.Count; i++)
		{
			DeployedScienceCluster deployedScienceCluster = DeployedScienceClusters.ValuesList[i];
			if (!deployedScienceCluster.IsPowered)
			{
				continue;
			}
			for (int j = 0; j < deployedScienceCluster.DeployedScienceParts.Count; j++)
			{
				DeployedSciencePart deployedSciencePart = deployedScienceCluster.DeployedScienceParts[j];
				if (deployedSciencePart.Experiment != null && deployedSciencePart.Experiment.ExperimentVessel != null && deployedSciencePart.Experiment.ExperimentVessel.mainBody.name == celestialBodyName && deployedSciencePart.Experiment.ExperimentId == experimentId && deployedSciencePart.Experiment.ScienceModifierRate > num)
				{
					num = deployedSciencePart.Experiment.ScienceModifierRate;
					result = deployedSciencePart;
				}
			}
		}
		return result;
	}

	public void SetDiminishingScienceRate(string experimentId, string celestialBodyName)
	{
		float scienceDiminishingModifierRate = ExpDimReturnsRate(experimentId, celestialBodyName);
		DeployedSciencePart deployedSciencePart = ExpMaxScienceMultiplier(experimentId, celestialBodyName);
		if (!(deployedSciencePart != null))
		{
			return;
		}
		for (int i = 0; i < DeployedScienceClusters.ValuesList.Count; i++)
		{
			DeployedScienceCluster deployedScienceCluster = DeployedScienceClusters.ValuesList[i];
			if (!deployedScienceCluster.IsPowered)
			{
				continue;
			}
			for (int j = 0; j < deployedScienceCluster.DeployedScienceParts.Count; j++)
			{
				DeployedSciencePart deployedSciencePart2 = deployedScienceCluster.DeployedScienceParts[j];
				if (deployedSciencePart2.PartId == deployedSciencePart.PartId)
				{
					if (deployedSciencePart2.Experiment != null)
					{
						deployedSciencePart2.Experiment.ScienceDiminishingModifierRate = 1f;
					}
				}
				else if (deployedSciencePart2.Experiment != null && deployedSciencePart2.Experiment.ExperimentVessel != null && deployedSciencePart2.Experiment.ExperimentVessel.mainBody.name == celestialBodyName && deployedSciencePart2.Experiment.ExperimentId == experimentId)
				{
					deployedSciencePart2.Experiment.ScienceDiminishingModifierRate = scienceDiminishingModifierRate;
				}
			}
		}
	}

	public void RegisterCluster(ModuleGroundExpControl controlUnit, List<ModuleGroundSciencePart> deployableScienceParts)
	{
		if (deployedScienceClusters.ContainsKey(controlUnit.part.persistentId))
		{
			Debug.LogFormat("[DeployedScience]: Already contains Science Cluster {0}", controlUnit.part.persistentId);
			return;
		}
		DeployedScienceCluster deployedScienceCluster = DeployedScienceCluster.Spawn(controlUnit);
		deployedScienceClusters.Add(controlUnit.part.persistentId, deployedScienceCluster);
		for (int i = 0; i < deployableScienceParts.Count; i++)
		{
			deployedScienceCluster.DeployedScienceParts.Add(DeployedSciencePart.Spawn(deployableScienceParts[i], controlUnit, deployedScienceCluster));
		}
		deployedScienceCluster.UpdatePowerState();
		deployedScienceCluster.UpdateExperimentDiminishReturns();
		GameEvents.onGroundScienceClusterRegistered.Fire(controlUnit, deployedScienceCluster);
		GameEvents.onGroundScienceClusterUpdated.Fire(controlUnit, deployedScienceCluster);
	}

	public void DeRegisterCluster(uint controlUnitId)
	{
		if (!deployedScienceClusters.TryGetValue(controlUnitId, out var val))
		{
			return;
		}
		List<string> experiments = val.DeployedScienceParts.GetExperiments();
		string celestialBodyName = val.DeployedBody.name;
		for (int i = 0; i < val.DeployedScienceParts.Count; i++)
		{
			if (val.DeployedScienceParts[i].PartId == val.ControlModulePartId)
			{
				continue;
			}
			Part partout = null;
			ProtoPartSnapshot partout2 = null;
			if (val.DeployedScienceParts[i].PartId == 0)
			{
				continue;
			}
			if (FlightGlobals.FindLoadedPart(val.DeployedScienceParts[i].PartId, out partout))
			{
				if (partout.vessel != null)
				{
					SetDeployedSciencePartVesselType(partout.vessel);
				}
			}
			else if (FlightGlobals.FindUnloadedPart(val.DeployedScienceParts[i].PartId, out partout2) && partout2.pVesselRef != null && partout2.pVesselRef.vesselRef != null)
			{
				SetDeployedSciencePartVesselType(partout2.pVesselRef.vesselRef);
			}
		}
		val.gameObject.DestroyGameObject();
		deployedScienceClusters.Remove(controlUnitId);
		for (int j = 0; j < experiments.Count; j++)
		{
			SetDiminishingScienceRate(experiments[j], celestialBodyName);
		}
	}

	public void SetDeployedSciencePartVesselType(Vessel vessel)
	{
		vessel.vesselType = VesselType.DeployedScienceController;
		GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(vessel, vessel.vesselName, vessel.vesselName));
		if ((bool)KSCVesselMarkers.fetch)
		{
			KSCVesselMarkers.fetch.RefreshMarkers();
		}
		if (vessel.orbitRenderer != null)
		{
			vessel.orbitRenderer.RefreshMapObject();
		}
	}

	public void RemoveMannedScienceObjects()
	{
		if (DeployedScienceClusters == null || DeployedScienceClusters.Count <= 0 || !(deployedScienceGameObject != null))
		{
			return;
		}
		for (int num = deployedScienceGameObject.transform.childCount - 1; num >= 0; num--)
		{
			GameObject gameObject = deployedScienceGameObject.transform.GetChild(num).gameObject;
			if (gameObject != null && gameObject.GetComponent<DeployedScienceCluster>() != null)
			{
				Object.Destroy(gameObject);
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		ConfigNode node2 = new ConfigNode();
		node.TryGetValue("ScienceTimeDelay", ref ScienceTimeDelay);
		node.TryGetValue("DataSendFailedTimeDelay", ref DataSendFailedTimeDelay);
		if (node.TryGetNode("SCIENCECLUSTERS", ref node2))
		{
			ConfigNode[] nodes = node2.GetNodes("SCIENCECLUSTER");
			for (int i = 0; i < nodes.Length; i++)
			{
				DeployedScienceCluster deployedScienceCluster = DeployedScienceCluster.SpawnandLoad(nodes[i]);
				deployedScienceClusters.Add(deployedScienceCluster.ControlModulePartId, deployedScienceCluster);
			}
		}
		ConfigNode node3 = new ConfigNode();
		if (node.TryGetNode("DIMINISHINGRETURNS", ref node3))
		{
			SetupDiminishingReturns(node3);
		}
		else
		{
			ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("DEPLOYEDSCIENCE");
			if (configNodes.Length != 0)
			{
				if (configNodes[0].TryGetNode("DIMINISHINGRETURNS", ref node3))
				{
					SetupDiminishingReturns(node3);
				}
				else
				{
					SetupDiminishingReturns(null);
				}
			}
			else
			{
				SetupDiminishingReturns(null);
			}
		}
		ConfigNode node4 = new ConfigNode();
		if (node.TryGetNode("SEISMICENERGY", ref node4))
		{
			SetupSeismicEnergy(node4);
			return;
		}
		ConfigNode[] configNodes2 = GameDatabase.Instance.GetConfigNodes("DEPLOYEDSCIENCE");
		if (configNodes2.Length != 0)
		{
			if (configNodes2[0].TryGetNode("SEISMICENERGY", ref node4))
			{
				SetupSeismicEnergy(node4);
			}
			else
			{
				SetupSeismicEnergy(null);
			}
		}
		else
		{
			SetupSeismicEnergy(null);
		}
	}

	public void SetupDiminishingReturns(ConfigNode node)
	{
		diminishingReturns = new DictionaryValueList<int, float>();
		if (node != null)
		{
			ConfigNode[] nodes = node.GetNodes("ENTRY");
			if (nodes == null)
			{
				return;
			}
			for (int i = 0; i < nodes.Length; i++)
			{
				int value = 0;
				float value2 = 1f;
				nodes[i].TryGetValue("ExpCount", ref value);
				nodes[i].TryGetValue("Rate", ref value2);
				if (!diminishingReturns.ContainsKey(value))
				{
					diminishingReturns.Add(value, value2);
				}
			}
		}
		else
		{
			diminishingReturns.Add(1, 1f);
			diminishingReturns.Add(2, 0.2f);
			diminishingReturns.Add(3, 0.35f);
			diminishingReturns.Add(4, 0.45f);
			diminishingReturns.Add(5, 0.5f);
		}
	}

	public void SetupSeismicEnergy(ConfigNode node)
	{
		seismicEnergyRates = new DictionaryValueList<string, float>();
		if (node != null)
		{
			node.TryGetValue("MinimumEnergyRequired", ref MinimumSeismicEnergyRequired);
			node.TryGetValue("SeismicScienceProcessingDelay", ref SeismicScienceProcessingDelay);
			ConfigNode[] nodes = node.GetNodes("ENTRY");
			if (nodes == null)
			{
				return;
			}
			for (int i = 0; i < nodes.Length; i++)
			{
				string value = "";
				float value2 = 1f;
				nodes[i].TryGetValue("BodyName", ref value);
				nodes[i].TryGetValue("Energy", ref value2);
				if (!seismicEnergyRates.ContainsKey(value))
				{
					seismicEnergyRates.Add(value, value2);
				}
			}
		}
		else
		{
			seismicEnergyRates.Add("Moho", 2.02E+09f);
			seismicEnergyRates.Add("Eve", 9.78E+10f);
			seismicEnergyRates.Add("Gilly", 1000000f);
			seismicEnergyRates.Add("Kerbin", 4.184E+10f);
			seismicEnergyRates.Add("Mun", 782000000f);
			seismicEnergyRates.Add("Minmus", 21200000f);
			seismicEnergyRates.Add("Duna", 3.62E+09f);
			seismicEnergyRates.Add("Ike", 223000000f);
			seismicEnergyRates.Add("Dres", 258000000f);
			seismicEnergyRates.Add("Laythe", 2.036E+10f);
			seismicEnergyRates.Add("Vall", 2.49E+09f);
			seismicEnergyRates.Add("Tylo", 3.39E+10f);
			seismicEnergyRates.Add("Bop", 29900000f);
			seismicEnergyRates.Add("Poll", 866000000f);
			seismicEnergyRates.Add("Eeloo", 894000000f);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("ScienceTimeDelay", ScienceTimeDelay);
		node.AddValue("DataSendFailedTimeDelay", DataSendFailedTimeDelay);
		ConfigNode configNode = node.AddNode("SCIENCECLUSTERS");
		for (int i = 0; i < deployedScienceClusters.ValuesList.Count; i++)
		{
			ConfigNode node2 = new ConfigNode("SCIENCECLUSTER");
			deployedScienceClusters.ValuesList[i].Save(node2);
			configNode.AddNode(node2);
		}
		ConfigNode configNode2 = node.AddNode("DIMINISHINGRETURNS");
		for (int j = 0; j < diminishingReturns.KeysList.Count; j++)
		{
			ConfigNode configNode3 = configNode2.AddNode("ENTRY");
			configNode3.AddValue("ExpCount", diminishingReturns.KeysList[j]);
			configNode3.AddValue("Rate", diminishingReturns.ValuesList[j]);
		}
		ConfigNode configNode4 = node.AddNode("SEISMICENERGY");
		configNode4.AddValue("MinimumEnergyRequired", MinimumSeismicEnergyRequired);
		configNode4.AddValue("SeismicScienceProcessingDelay", SeismicScienceProcessingDelay);
		for (int k = 0; k < seismicEnergyRates.KeysList.Count; k++)
		{
			ConfigNode configNode5 = configNode4.AddNode("ENTRY");
			configNode5.AddValue("BodyName", seismicEnergyRates.KeysList[k]);
			configNode5.AddValue("Energy", seismicEnergyRates.ValuesList[k]);
		}
	}
}
