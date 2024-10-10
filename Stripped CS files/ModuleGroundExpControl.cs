using System.Collections;
using System.Collections.Generic;
using CommNet;
using Expansions;
using Expansions.Serenity.DeployedScience.Runtime;
using ns11;
using ns9;
using UnityEngine;

public class ModuleGroundExpControl : ModuleGroundCommsPart, ICommNetControlSource
{
	[KSPField]
	public int controlUnitRange = 20;

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002246")]
	public int experimentsConnected;

	[KSPField(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_6001355")]
	public string commNetSignal = Localizer.Format("#autoLOC_217146");

	[KSPField(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_6001356")]
	public string commNetFirstHopDistance = Localizer.Format("#autoLOC_217149");

	[KSPField(guiActiveEditor = false, guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8005418")]
	public string powerNeeded;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public double boostedAntennaPower;

	public bool beingDestroyed;

	public static string cacheAutoLOC_7001411;

	public static string cacheAutoLOC_8002233;

	public CommNetVessel Connection => base.vessel.connection;

	public override double CommPower => antennaPower + boostedAntennaPower;

	string ICommNetControlSource.name => base.name;

	[KSPEvent(guiActiveUncommand = true, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_900678")]
	public void SetVesselNaming()
	{
		base.part.SetVesselNaming();
	}

	public new void Start()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity") && HighLogic.LoadedSceneIsGame)
		{
			base.enabled = false;
			Object.Destroy(this);
		}
	}

	public override void OnAwake()
	{
		base.OnAwake();
		if (rangeCurve == null || rangeCurve.keys == null || rangeCurve.keys.Count < 2)
		{
			rangeCurve = new DoubleCurve();
			rangeCurve.Add(0.0, 0.0, 0.0, 0.0);
			rangeCurve.Add(1.0, 1.0, 0.0, 0.0);
		}
		if (scienceCurve == null || scienceCurve.keys == null || scienceCurve.keys.Count < 2)
		{
			scienceCurve = new DoubleCurve();
			scienceCurve.Add(0.0, 0.0, 0.0, 0.0);
			scienceCurve.Add(0.44999998807907104, 0.07999999821186066, 0.4951019, 0.4951019);
			scienceCurve.Add(0.8100000023841858, 0.3499999940395355, 0.8020515, 0.8020515);
			scienceCurve.Add(1.0, 0.4000000059604645, 0.0, 0.0);
		}
		DistanceToController = 0f;
	}

	public override void OnDestroy()
	{
		GameEvents.onGroundSciencePartDeployed.Remove(OnGroundSciencePartDeployed);
		GameEvents.onPartActionUIShown.Remove(OnPartActionUIOpened);
		GameEvents.onPartActionUIDismiss.Remove(OnPartActionUIDismiss);
		GameEvents.onGroundScienceClusterUpdated.Remove(OnGroundScienceClusterUpdated);
		GameEvents.onGroundSciencePartEnabledStateChanged.Remove(OnGroundSciencePartEnabledStateChanged);
		GameEvents.onGroundSciencePartRemoved.Remove(OnGroundScienceModuleRemoved);
		GameEvents.onGroundScienceClusterPowerStateChanged.Remove(OnGroundScienceClusterPowerStateChanged);
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (HighLogic.LoadedSceneIsFlight && (state & StartState.Landed) != 0 && base.vessel.vesselType != VesselType.DroppedPart)
		{
			ControlUnitId = base.part.persistentId;
			if ((bool)DeployedScience.Instance)
			{
				StartCoroutine(ScanAndAddControlUnit());
			}
			GameEvents.onGroundSciencePartDeployed.Add(OnGroundSciencePartDeployed);
		}
		if (base.vessel != null && base.vessel.vesselType != VesselType.DroppedPart)
		{
			GameEvents.onPartActionUIShown.Add(OnPartActionUIOpened);
			GameEvents.onPartActionUIDismiss.Add(OnPartActionUIDismiss);
			GameEvents.onGroundScienceClusterUpdated.Add(OnGroundScienceClusterUpdated);
			GameEvents.onGroundSciencePartEnabledStateChanged.Add(OnGroundSciencePartEnabledStateChanged);
			GameEvents.onGroundSciencePartRemoved.Add(OnGroundScienceModuleRemoved);
			GameEvents.onGroundScienceClusterPowerStateChanged.Add(OnGroundScienceClusterPowerStateChanged);
		}
		base.Fields["PowerState"].guiName = Localizer.Format("#autoLOC_8002247");
	}

	public override void OnSave(ConfigNode node)
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			node.AddValue("canComm", CanComm());
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (partActionWindow != null)
		{
			UpdateModuleUI();
		}
	}

	public override string GetInfo()
	{
		string text = Localizer.Format("#autoLOC_8002220");
		text += "\n\n";
		text = text + Localizer.Format("#autoLOC_443357") + " " + (base.part.partInfo.partPrefab.mass + base.part.GetModuleMass(base.part.partInfo.partPrefab.mass)).ToString("F3") + " t\\n";
		if (packedVolume > 0f)
		{
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8003414"), Localizer.Format("<<1>><<2>>", packedVolume.ToString("0.0"), "L")) + "\n";
		}
		if (stackableQuantity > 1)
		{
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8003418"), stackableQuantity.ToString("0")) + "\n";
		}
		if (packedVolume > 0f || stackableQuantity > 1)
		{
			text += "\n";
		}
		text = text + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format("#autoLOC_8002227") + "</color>\n";
		text = text + Localizer.Format("#autoLOC_8002328", controlUnitRange) + "\n";
		text += "\n";
		return text + base.GetInfo();
	}

	public new void OnGroundScienceClusterPowerStateChanged(DeployedScienceCluster cluster)
	{
		if (ControlUnitId == cluster.ControlModulePartId)
		{
			UpdateModuleUI();
		}
	}

	public void OnGroundScienceModuleRemoved(ModuleGroundSciencePart sciencePart)
	{
		if (ControlUnitId == sciencePart.ControlUnitId)
		{
			if (sciencePart.part.persistentId == ControlUnitId)
			{
				beingDestroyed = true;
			}
			else if (sciencePart.GetComponent<ModuleGroundCommsPart>() != null)
			{
				RecalcBoosterPower(sciencePart);
			}
		}
	}

	public void OnGroundSciencePartEnabledStateChanged(ModuleGroundSciencePart sciencePart)
	{
		if (ControlUnitId == sciencePart.ControlUnitId && sciencePart.GetComponent<ModuleGroundCommsPart>() != null)
		{
			RecalcBoosterPower(sciencePart);
		}
	}

	public new void OnGroundScienceClusterUpdated(ModuleGroundExpControl controlUnit, DeployedScienceCluster cluster)
	{
		if (ControlUnitId == controlUnit.ControlUnitId)
		{
			UpdateModuleUI();
			RecalcBoosterPower(null);
		}
	}

	public IEnumerator ScanAndAddControlUnit()
	{
		yield return new WaitForSeconds(2f);
		List<ModuleGroundSciencePart> connectedUnits = new List<ModuleGroundSciencePart>();
		List<Part> loadedParts = FlightGlobals.fetch.persistentLoadedPartIds.ValuesList;
		for (int i = 0; i < loadedParts.Count; i++)
		{
			ModuleGroundSciencePart moduleGroundSciencePart = loadedParts[i].FindModuleImplementing<ModuleGroundSciencePart>();
			if (moduleGroundSciencePart != null && moduleGroundSciencePart.part.persistentId != base.part.persistentId)
			{
				float num = Vector3.Distance(base.transform.position, moduleGroundSciencePart.part.transform.position);
				if (moduleGroundSciencePart.ControlUnitId == 0 && num <= (float)controlUnitRange && num < moduleGroundSciencePart.DistanceToController)
				{
					Debug.LogFormat("[ModuleGroundExpControl]: Part {0}{1} - Connecting to Controller. Distance to Controller {2}{3} is {4}", moduleGroundSciencePart.part.partInfo.title, moduleGroundSciencePart.part.persistentId, base.part.partInfo.title, base.part.persistentId, num);
					moduleGroundSciencePart.ControlUnitId = base.part.persistentId;
					SetDeployedSciencePartVesselType(moduleGroundSciencePart);
					connectedUnits.Add(moduleGroundSciencePart);
				}
			}
			yield return null;
		}
		if (DeployedScience.Instance.DeployedScienceClusters.ContainsKey(base.part.persistentId))
		{
			GameEvents.onGroundScienceControllerChanged.Fire(this, data1: false, connectedUnits);
		}
		else
		{
			GameEvents.onGroundScienceRegisterCluster.Fire(this, connectedUnits);
		}
	}

	public void OnGroundSciencePartDeployed(ModuleGroundSciencePart sciencePart)
	{
		if (beingDestroyed)
		{
			return;
		}
		float num = Vector3.Distance(base.transform.position, sciencePart.part.transform.position);
		Debug.LogFormat("[ModuleGroundExpControl]: Part {0}{1} - Distance to Controller {2}{3} is {4}", sciencePart.part.partInfo.title, sciencePart.part.persistentId, base.part.partInfo.title, base.part.persistentId, num);
		if (num <= (float)controlUnitRange && num < sciencePart.DistanceToController && base.ScienceClusterData != null)
		{
			ModuleGroundExperiment component = sciencePart.GetComponent<ModuleGroundExperiment>();
			if (component != null && (bool)base.ScienceClusterData.DeployedScienceParts.GetExperiment(component.experimentId))
			{
				Debug.LogFormat("[ModuleGroundExpControl]: Unable to connect {0}{1} as Science Cluster already has this Experiment Connected.", sciencePart.part.persistentId, component.experimentId);
				return;
			}
			sciencePart.DistanceToController = num;
			List<ModuleGroundSciencePart> list = new List<ModuleGroundSciencePart>();
			list.Add(sciencePart);
			sciencePart.ControlUnitId = base.part.persistentId;
			SetDeployedSciencePartVesselType(sciencePart);
			GameEvents.onGroundScienceControllerChanged.Fire(this, data1: false, list);
		}
	}

	public void SetDeployedSciencePartVesselType(ModuleGroundSciencePart sciencePart)
	{
		if (sciencePart.part.persistentId != base.part.persistentId && sciencePart.vessel != null)
		{
			sciencePart.vessel.vesselType = VesselType.DeployedSciencePart;
			if (sciencePart.vessel.orbitRenderer != null)
			{
				sciencePart.vessel.orbitRenderer.RefreshMapObject();
			}
			if ((bool)KSCVesselMarkers.fetch)
			{
				KSCVesselMarkers.fetch.RefreshMarkers();
			}
			GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(sciencePart.vessel, sciencePart.vessel.vesselName, sciencePart.vessel.vesselName));
		}
	}

	public new void OnPartActionUIOpened(UIPartActionWindow window, Part p)
	{
		if (p == base.part)
		{
			UpdateModuleUI();
			partActionWindow = window;
		}
	}

	public new void OnPartActionUIDismiss(Part p)
	{
		if (p == base.part)
		{
			partActionWindow = null;
		}
	}

	public override void UpdateModuleUI()
	{
		base.UpdateModuleUI();
		base.Fields["ConnectionState"].guiActive = false;
		if (base.ScienceClusterData != null)
		{
			PowerState = base.ScienceClusterData.PowerAvailable.ToString();
			if (!base.ScienceClusterData.IsPowered)
			{
				powerNeeded = Localizer.Format("#autoLOC_6002375", base.ScienceClusterData.PowerRequired, cacheAutoLOC_8002233);
			}
			else
			{
				powerNeeded = base.ScienceClusterData.PowerRequired.ToString();
			}
		}
		if (!(base.ScienceClusterData != null))
		{
			return;
		}
		experimentsConnected = 0;
		for (int i = 0; i < base.ScienceClusterData.DeployedScienceParts.Count; i++)
		{
			if (base.ScienceClusterData.DeployedScienceParts[i].Enabled && base.ScienceClusterData.DeployedScienceParts[i].Experiment != null)
			{
				experimentsConnected++;
			}
		}
	}

	public void RecalcBoosterPower(ModuleGroundSciencePart sciencePart)
	{
		boostedAntennaPower = 0.0;
		if (!(base.ScienceClusterData != null))
		{
			return;
		}
		for (int i = 0; i < base.ScienceClusterData.AntennaParts.Count; i++)
		{
			bool flag = base.ScienceClusterData.antennaParts[i].Enabled;
			if (sciencePart != null && base.ScienceClusterData.antennaParts[i].PartId == sciencePart.part.persistentId)
			{
				flag = sciencePart.Enabled && sciencePart.DeployedOnGround;
			}
			if (flag && base.ScienceClusterData.antennaParts[i].PartId != base.part.persistentId)
			{
				boostedAntennaPower += base.ScienceClusterData.antennaParts[i].AntennaBoosterPower;
			}
		}
	}

	public override double CommPowerUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		double num = base.CommPowerUnloaded(mSnap);
		double value = 0.0;
		if (mSnap != null && mSnap.moduleValues.TryGetValue("boostedAntennaPower", ref value))
		{
			num += value;
		}
		return num;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8002227");
	}

	public override string GetTooltip()
	{
		if (string.IsNullOrEmpty(inventoryTooltip))
		{
			return GetInfo();
		}
		return Localizer.Format(inventoryTooltip, controlUnitRange);
	}

	public virtual VesselControlState GetControlSourceState()
	{
		if (base.ScienceClusterData != null)
		{
			if (base.ScienceClusterData.IsPowered && base.Enabled && base.DeployedOnGround)
			{
				return VesselControlState.ProbeFull;
			}
			return VesselControlState.None;
		}
		return VesselControlState.None;
	}

	public virtual bool IsCommCapable()
	{
		if (base.ScienceClusterData != null)
		{
			if (base.ScienceClusterData.IsPowered && base.Enabled && base.DeployedOnGround)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public virtual void UpdateNetwork()
	{
		if (partActionWindow != null && Connection != null)
		{
			commNetSignal = KSPUtil.LocalizeNumber(Connection.SignalStrength, "F2");
			if (Connection.IsConnected)
			{
				commNetFirstHopDistance = KSPUtil.PrintSI(Connection.ControlPath.First.cost, cacheAutoLOC_7001411);
			}
			else
			{
				commNetFirstHopDistance = "-";
			}
		}
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_7001411 = Localizer.Format("#autoLOC_7001411");
		cacheAutoLOC_8002233 = Localizer.Format("#autoLOC_8002233");
	}
}
