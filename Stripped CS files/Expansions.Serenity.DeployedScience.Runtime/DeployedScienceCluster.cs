using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Serenity.DeployedScience.Runtime;

public class DeployedScienceCluster : MonoBehaviour, IConfigNode
{
	[SerializeField]
	public bool isPowered;

	public CelestialBody DeployedBody;

	[SerializeField]
	public bool partialPower;

	[SerializeField]
	public float partialPowerMultiplier = 1f;

	public List<DeployedSciencePart> DeployedScienceParts;

	[SerializeField]
	public int powerRequired;

	[SerializeField]
	public int powerAvailable;

	[SerializeField]
	public List<DeployedSciencePart> solarPanelParts;

	[SerializeField]
	public List<DeployedSciencePart> antennaParts;

	[SerializeField]
	public uint controlModulePartId;

	[SerializeField]
	public bool controllerPartEnabled;

	[SerializeField]
	public double lastScienceGeneratedUT;

	[SerializeField]
	public double lastScienceTransmittedUT;

	[SerializeField]
	public bool updatingScience;

	[SerializeField]
	public bool dataSendFailed;

	public bool IsPowered => isPowered;

	public bool PartialPower => partialPower;

	public float PartialPowerMultiplier => partialPowerMultiplier;

	public int PowerRequired => powerRequired;

	public int PowerAvailable => powerAvailable;

	public List<DeployedSciencePart> SolarPanelParts => solarPanelParts;

	public bool HasSolarPanels
	{
		get
		{
			if (SolarPanelParts != null)
			{
				return SolarPanelParts.Count > 0;
			}
			return false;
		}
	}

	public List<DeployedSciencePart> AntennaParts => antennaParts;

	public uint ControlModulePartId => controlModulePartId;

	public bool ControllerPartEnabled => controllerPartEnabled;

	public double LastScienceGeneratedUT => lastScienceGeneratedUT;

	public double LastScienceTransmittedUT => lastScienceTransmittedUT;

	public bool UpdatingScience
	{
		get
		{
			return updatingScience;
		}
		set
		{
			updatingScience = value;
		}
	}

	public bool DataSendFailed => dataSendFailed;

	public static DeployedScienceCluster Spawn()
	{
		GameObject obj = new GameObject("ScienceCluster");
		obj.transform.SetParent(DeployedScience.DeployedScienceGameObject.transform);
		DeployedScienceCluster deployedScienceCluster = obj.gameObject.AddComponent<DeployedScienceCluster>();
		deployedScienceCluster.DeployedScienceParts = new List<DeployedSciencePart>();
		deployedScienceCluster.solarPanelParts = new List<DeployedSciencePart>();
		deployedScienceCluster.antennaParts = new List<DeployedSciencePart>();
		deployedScienceCluster.lastScienceGeneratedUT = Planetarium.GetUniversalTime();
		deployedScienceCluster.controllerPartEnabled = true;
		return deployedScienceCluster;
	}

	public static DeployedScienceCluster Spawn(ModuleGroundExpControl controlUnit)
	{
		if (controlUnit == null)
		{
			return null;
		}
		DeployedScienceCluster deployedScienceCluster = Spawn();
		deployedScienceCluster.controlModulePartId = controlUnit.ControlUnitId;
		deployedScienceCluster.gameObject.name = "ScienceCluster " + controlUnit.ControlUnitId;
		if (controlUnit.vessel != null)
		{
			deployedScienceCluster.DeployedBody = controlUnit.vessel.mainBody;
		}
		ModuleGroundSciencePart component = controlUnit.gameObject.GetComponent<ModuleGroundSciencePart>();
		if (component != null)
		{
			DeployedSciencePart deployedSciencePart = DeployedSciencePart.Spawn(component, controlUnit, deployedScienceCluster);
			deployedScienceCluster.DeployedScienceParts.Add(deployedSciencePart);
			if (component.IsSolarPanel)
			{
				deployedScienceCluster.solarPanelParts.Add(deployedSciencePart);
			}
			if (deployedSciencePart.IsAntenna)
			{
				deployedScienceCluster.antennaParts.AddUnique(deployedSciencePart);
			}
		}
		return deployedScienceCluster;
	}

	public static DeployedScienceCluster SpawnandLoad(ConfigNode node)
	{
		DeployedScienceCluster deployedScienceCluster = Spawn();
		deployedScienceCluster.Load(node);
		deployedScienceCluster.UpdatePowerState();
		deployedScienceCluster.gameObject.name = "ScienceCluster " + deployedScienceCluster.ControlModulePartId;
		return deployedScienceCluster;
	}

	public void Awake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Object.Destroy(this);
		}
	}

	public void Start()
	{
		GameEvents.onGroundScienceControllerChanged.Add(UpdateCluster);
		GameEvents.onGroundSciencePartRemoved.Add(GroundScienceModuleRemoved);
		GameEvents.onGroundSciencePartEnabledStateChanged.Add(GroundSciencePartEnabledStateChanged);
	}

	public void OnDestroy()
	{
		GameEvents.onGroundScienceControllerChanged.Remove(UpdateCluster);
		GameEvents.onGroundSciencePartRemoved.Remove(GroundScienceModuleRemoved);
		GameEvents.onGroundSciencePartEnabledStateChanged.Remove(GroundSciencePartEnabledStateChanged);
		for (int i = 0; i < DeployedScienceParts.Count; i++)
		{
			DeployedScienceParts[i].gameObject.DestroyGameObject();
		}
	}

	public int SolarPanelUnitsProduced()
	{
		int num = 0;
		for (int i = 0; i < solarPanelParts.Count; i++)
		{
			num += solarPanelParts[i].PowerUnitsProduced;
		}
		return num;
	}

	public void UpdatePowerState()
	{
		powerRequired = 0;
		powerAvailable = 0;
		bool flag = false;
		bool flag2 = isPowered;
		for (int i = 0; i < DeployedScienceParts.Count; i++)
		{
			if (DeployedScienceParts[i].Enabled && DeployedScienceParts[i].DeployedOnGround)
			{
				Part part = DeployedScienceParts[i].PartIsLoaded();
				if (part != null)
				{
					flag = true;
				}
				if (DeployedScienceParts[i].IsSolarPanel)
				{
					if (part != null)
					{
						powerAvailable += DeployedScienceParts[i].ActualPowerUnitsProduced;
					}
					else
					{
						powerAvailable += DeployedScienceParts[i].PowerUnitsProduced;
					}
				}
				else
				{
					powerAvailable += DeployedScienceParts[i].PowerUnitsProduced;
				}
				powerRequired += DeployedScienceParts[i].PowerUnitsRequired;
			}
			if (DeployedScienceParts[i].PartId == ControlModulePartId)
			{
				controllerPartEnabled = DeployedScienceParts[i].Enabled;
			}
		}
		flag2 = controllerPartEnabled;
		if (flag)
		{
			if (powerAvailable >= powerRequired)
			{
				partialPower = false;
				partialPowerMultiplier = 1f;
			}
			else
			{
				partialPower = true;
				partialPowerMultiplier = 0f;
				flag2 = false;
			}
		}
		else if (PowerAvailable < PowerRequired)
		{
			partialPower = true;
			partialPowerMultiplier = 0f;
			flag2 = false;
		}
		else if (PowerAvailable - SolarPanelUnitsProduced() < PowerRequired)
		{
			partialPower = true;
			partialPowerMultiplier = 0.5f;
		}
		else
		{
			partialPower = false;
			partialPowerMultiplier = 1f;
		}
		if (flag2 != IsPowered)
		{
			isPowered = flag2;
			GameEvents.onGroundScienceClusterPowerStateChanged.Fire(this);
		}
	}

	public void UpdateCluster(ModuleGroundExpControl controlUnit, bool replaceList, List<ModuleGroundSciencePart> scienceParts)
	{
		if (ControlModulePartId != controlUnit.part.persistentId)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < scienceParts.Count; i++)
		{
			if (scienceParts[i].part.persistentId == controlUnit.part.persistentId)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			scienceParts.Add(controlUnit);
		}
		bool flag2 = false;
		for (int j = 0; j < scienceParts.Count; j++)
		{
			DeployedSciencePart deployedSciencePart = DeployedScienceParts.Get(scienceParts[j].part.persistentId);
			if (deployedSciencePart == null)
			{
				deployedSciencePart = DeployedSciencePart.Spawn(scienceParts[j], controlUnit, this);
				DeployedScienceParts.Add(deployedSciencePart);
				if (deployedSciencePart.IsSolarPanel)
				{
					solarPanelParts.Add(deployedSciencePart);
				}
				if (deployedSciencePart.Experiment != null)
				{
					flag2 = true;
				}
				if (deployedSciencePart.IsAntenna)
				{
					antennaParts.AddUnique(deployedSciencePart);
				}
			}
			else
			{
				deployedSciencePart.UpdateSciencePart(scienceParts[j]);
			}
		}
		bool flag3 = false;
		if (replaceList)
		{
			int count = DeployedScienceParts.Count;
			while (count-- > 0)
			{
				if (scienceParts.Get(DeployedScienceParts[count].PartId) == null)
				{
					Debug.LogFormat("[DeployedScienceCluster]: Removing unknown Science Part {0} from Cluster {1}", DeployedScienceParts[count].PartId, ControlModulePartId);
					if (solarPanelParts.Contains(DeployedScienceParts[count]))
					{
						solarPanelParts.Remove(DeployedScienceParts[count]);
					}
					if (antennaParts.Contains(DeployedScienceParts[count]))
					{
						antennaParts.Remove(DeployedScienceParts[count]);
					}
					if (DeployedScienceParts[count].Experiment != null)
					{
						flag3 = true;
					}
					DeployedScienceParts[count].gameObject.DestroyGameObject();
					DeployedScienceParts.RemoveAt(count);
				}
			}
		}
		UpdatePowerState();
		if (flag2 || flag3)
		{
			UpdateExperimentDiminishReturns();
		}
		GameEvents.onGroundScienceClusterUpdated.Fire(controlUnit, this);
	}

	public void UpdateExperimentDiminishReturns()
	{
		for (int i = 0; i < DeployedScienceParts.Count; i++)
		{
			if (DeployedScienceParts[i].Experiment != null)
			{
				DeployedScience.Instance.SetDiminishingScienceRate(DeployedScienceParts[i].Experiment.ExperimentId, DeployedBody.name);
			}
		}
	}

	public void GroundSciencePartEnabledStateChanged(ModuleGroundSciencePart sciencePart)
	{
		if (sciencePart.ControlUnitId == ControlModulePartId)
		{
			if (sciencePart.part.persistentId == ControlModulePartId)
			{
				controllerPartEnabled = sciencePart.Enabled && sciencePart.DeployedOnGround;
				UpdatePowerState();
			}
			ModuleGroundExperiment component = sciencePart.gameObject.GetComponent<ModuleGroundExperiment>();
			if (component != null)
			{
				DeployedScience.Instance.SetDiminishingScienceRate(component.experimentId, sciencePart.vessel.mainBody.name);
			}
		}
	}

	public void GroundScienceModuleRemoved(ModuleGroundSciencePart part)
	{
		DeployedSciencePart deployedSciencePart = DeployedScienceParts.Get(part.part.persistentId);
		if (deployedSciencePart != null)
		{
			if (solarPanelParts.Contains(deployedSciencePart))
			{
				solarPanelParts.Remove(deployedSciencePart);
			}
			if (antennaParts.Contains(deployedSciencePart))
			{
				antennaParts.Remove(deployedSciencePart);
			}
			string text = "";
			if (deployedSciencePart.Experiment != null)
			{
				text = deployedSciencePart.Experiment.ExperimentId;
			}
			DeployedScienceParts.Remove(deployedSciencePart);
			deployedSciencePart.gameObject.DestroyGameObject();
			UpdatePowerState();
			if (!string.IsNullOrEmpty(text))
			{
				DeployedScience.Instance.SetDiminishingScienceRate(text, DeployedBody.name);
			}
		}
	}

	public IEnumerator UpdateScience()
	{
		if (IsPowered)
		{
			for (int i = 0; i < DeployedScienceParts.Count; i++)
			{
				if (DeployedScienceParts[i].Experiment != null)
				{
					if (DeployedScienceParts[i].Enabled && DeployedScienceParts[i].Experiment.ExperimentId != DeployedScience.SeismicExperimentId)
					{
						DeployedScienceParts[i].Experiment.CalculateScience(partialPowerMultiplier);
					}
					if (DeployedScienceParts[i].Experiment.StoredScienceData > 0f && DeployedScienceParts[i].Experiment.TimeToSendStoredData())
					{
						dataSendFailed = !DeployedScienceParts[i].Experiment.SendDataToComms();
					}
					lastScienceTransmittedUT = Planetarium.GetUniversalTime();
					yield return null;
				}
			}
		}
		else
		{
			double universalTime = Planetarium.GetUniversalTime();
			for (int j = 0; j < DeployedScienceParts.Count; j++)
			{
				if (DeployedScienceParts[j].Experiment != null)
				{
					DeployedScienceParts[j].Experiment.LastScienceGeneratedUT = universalTime;
				}
			}
		}
		lastScienceGeneratedUT = Planetarium.GetUniversalTime();
		updatingScience = false;
		yield return null;
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("ControlModulePartId", ref controlModulePartId);
		string value = "";
		node.TryGetValue("DeployedBody", ref value);
		if (!string.IsNullOrEmpty(value))
		{
			DeployedBody = FlightGlobals.GetBodyByName(value);
		}
		node.TryGetValue("ControlModuleEnabled", ref controllerPartEnabled);
		node.TryGetValue("LastScienceGeneratedUT", ref lastScienceGeneratedUT);
		ConfigNode node2 = new ConfigNode();
		if (!node.TryGetNode("MANNEDSCIENCEPARTS", ref node2))
		{
			return;
		}
		ConfigNode[] nodes = node2.GetNodes("MANNEDSCIENCEPART");
		for (int i = 0; i < nodes.Length; i++)
		{
			DeployedSciencePart deployedSciencePart = DeployedSciencePart.SpawnandLoad(nodes[i], this);
			DeployedScienceParts.Add(deployedSciencePart);
			if (deployedSciencePart.IsSolarPanel)
			{
				solarPanelParts.Add(deployedSciencePart);
			}
			if (deployedSciencePart.IsAntenna && deployedSciencePart.PartId != ControlModulePartId)
			{
				antennaParts.AddUnique(deployedSciencePart);
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("ControlModulePartId", ControlModulePartId);
		if (DeployedBody != null)
		{
			node.AddValue("DeployedBody", DeployedBody.name);
		}
		node.AddValue("ControlModuleEnabled", ControllerPartEnabled);
		node.AddValue("LastScienceGeneratedUT", LastScienceGeneratedUT);
		ConfigNode configNode = node.AddNode("MANNEDSCIENCEPARTS");
		for (int i = 0; i < DeployedScienceParts.Count; i++)
		{
			ConfigNode node2 = configNode.AddNode("MANNEDSCIENCEPART");
			DeployedScienceParts[i].Save(node2);
		}
	}
}
