using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions;
using Expansions.Missions.Adjusters;
using ns11;
using ns2;
using UnityEngine;

public class VesselDeltaV : MonoBehaviour
{
	public enum PartStageComparisonOperator
	{
		LessThan,
		NotEqual,
		Equal
	}

	public enum Mode
	{
		Ship,
		Vessel
	}

	public DeltaVEngineStageSet engineStageSet;

	[SerializeField]
	public Vessel _vessel;

	[SerializeField]
	public ShipConstruct _ship;

	[SerializeField]
	public bool syncListInstances = true;

	[SerializeField]
	public bool useMultipleInfoLists = true;

	[SerializeField]
	public List<int> _separationStageIndexes;

	[SerializeField]
	public List<DeltaVPartInfo> _partInfo;

	[SerializeField]
	public double _totalDeltaVVac;

	[SerializeField]
	public double _totalDeltaVASL;

	[SerializeField]
	public double _totalDeltaVActual;

	[SerializeField]
	public double _totalBurnTime;

	[SerializeField]
	public Mode _activeMode;

	public bool currentStageActivated;

	public int lowestStageWithDeltaV;

	public Coroutine simulation;

	public bool _isReady;

	public bool _doStockSimulation = true;

	public bool updateFlightScene;

	public double flightLastFullUpdateTime;

	public double flightLastStageUpdateTime;

	[SerializeField]
	public bool calcsDirty;

	public bool resetPartCaches;

	[SerializeField]
	public int frames = 3;

	public double vesselEventDelayTime;

	public List<DeltaVEngineInfo> WorkingEngineInfo => engineStageSet.WorkingEngineInfo;

	public List<DeltaVEngineInfo> OperatingEngineInfo => engineStageSet.OperatingEngineInfo;

	public Vessel Vessel => _vessel;

	public ShipConstruct Ship => _ship;

	[SerializeField]
	public List<DeltaVStageInfo> WorkingStageInfo => engineStageSet.WorkingStageInfo;

	[SerializeField]
	public List<DeltaVStageInfo> OperatingStageInfo => engineStageSet.OperatingStageInfo;

	public List<int> SeparationStageIndexes => _separationStageIndexes;

	public List<DeltaVPartInfo> PartInfo => _partInfo;

	public double TotalDeltaVVac => _totalDeltaVVac;

	public double TotalDeltaVASL => _totalDeltaVASL;

	public double TotalDeltaVActual => _totalDeltaVActual;

	public double TotalBurnTime => _totalBurnTime;

	public Mode ActiveMode => _activeMode;

	public bool SimulationRunning
	{
		get
		{
			if (simulation == null)
			{
				return updateFlightScene;
			}
			return true;
		}
	}

	public bool IsReady => _isReady;

	public bool DoStockSimulation => _doStockSimulation;

	public bool UpdateFlightScene => updateFlightScene;

	public bool EnableStockSimluation()
	{
		if (!GameSettings.DELTAV_CALCULATIONS_ENABLED)
		{
			Debug.Log("[DeltaVStatics]: Unable to enable Stock Calculations - overridden by GameSettings");
			return false;
		}
		_doStockSimulation = true;
		return true;
	}

	public bool DisableStockSimluation()
	{
		_doStockSimulation = false;
		return true;
	}

	public double GetSituationTotalDeltaV(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(TotalDeltaVASL, TotalDeltaVActual, TotalDeltaVVac);
	}

	public static VesselDeltaV Create(Vessel vesselRef)
	{
		if (vesselRef.vesselType > VesselType.SpaceObject && vesselRef.vesselType != VesselType.Flag && vesselRef.vesselType != VesselType.const_11 && vesselRef.vesselType != VesselType.DeployedSciencePart && vesselRef.vesselType != VesselType.DeployedScienceController)
		{
			VesselDeltaV vesselDeltaV = vesselRef.gameObject.AddComponent<VesselDeltaV>();
			vesselDeltaV._vessel = vesselRef;
			vesselDeltaV._ship = null;
			vesselDeltaV.engineStageSet = new DeltaVEngineStageSet(vesselDeltaV);
			vesselDeltaV._separationStageIndexes = new List<int>();
			vesselDeltaV._partInfo = new List<DeltaVPartInfo>();
			vesselDeltaV._activeMode = Mode.Vessel;
			return vesselDeltaV;
		}
		return null;
	}

	public static VesselDeltaV Create(ShipConstruct shipRef)
	{
		VesselDeltaV vesselDeltaV = new GameObject("VesselDeltaV " + shipRef.shipName).AddComponent<VesselDeltaV>();
		vesselDeltaV._ship = shipRef;
		vesselDeltaV._vessel = null;
		vesselDeltaV.engineStageSet = new DeltaVEngineStageSet(vesselDeltaV);
		vesselDeltaV._separationStageIndexes = new List<int>();
		vesselDeltaV._partInfo = new List<DeltaVPartInfo>();
		shipRef.vesselDeltaV = vesselDeltaV;
		for (int i = 0; i < shipRef.parts.Count; i++)
		{
			shipRef.parts[i].ship = shipRef;
		}
		vesselDeltaV._activeMode = Mode.Ship;
		return vesselDeltaV;
	}

	public void Start()
	{
		if (ActiveMode == Mode.Vessel && _vessel != null)
		{
			CalculateSeparationStageIndexes(_vessel.parts);
			ResetPartInfo(_vessel.parts);
		}
		else if (ActiveMode == Mode.Ship && _ship != null)
		{
			CalculateSeparationStageIndexes(_ship.parts);
			ResetPartInfo(_ship.parts);
		}
		calcsDirty = true;
		_doStockSimulation = DeltaVGlobals.DoStockSimulations;
		if (HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onEditorShipModified.Add(onShipModified);
			GameEvents.onPartPriorityChanged.Add(UpdateResourceSetsEvent);
			GameEvents.onPartResourceListChange.Add(UpdateResourceSetsEvent);
			GameEvents.onPartFuelLookupStateChange.Add(UpdateResourceSetsEventSetFL);
			BaseCrewAssignmentDialog.onCrewDialogChange.Add(Callback_CrewChange);
		}
		else
		{
			GameEvents.onVesselWasModified.Add(onVesselWasModified);
			GameEvents.onDockingComplete.Add(OnDockingComplete);
			GameEvents.onVesselsUndocking.Add(OnVesselsUndocking);
			GameEvents.onVesselChange.Add(OnVesselChange);
			GameEvents.onVesselGoOffRails.Add(OnVesselGoOffRails);
			GameEvents.Mission.onNodeChangedVesselResources.Add(OnNodeChangedVesselResources);
			GameEvents.onStageActivate.Add(OnStageActivate);
		}
		GameEvents.onEngineThrustPercentageChanged.Add(OnEngineThrustPercentageChanged);
		GameEvents.StageManager.OnGUIStageSequenceModified.Add(OnGUIStageSequenceModified);
		GameEvents.StageManager.OnStagingSeparationIndices.Add(OnStagingSeparationIndices);
		GameEvents.StageManager.OnGUIStageAdded.Add(StagesChanged);
		GameEvents.StageManager.OnGUIStageRemoved.Add(StagesChanged);
		GameEvents.onMultiModeEngineSwitchActive.Add(OnMultiModeEngineSwitchActive);
		GameEvents.onPartModuleAdjusterRemoved.Add(OnPartModuleAdjusterRemoved);
		GameEvents.onPartModuleAdjusterAdded.Add(OnPartModuleAdjusterAdded);
		GameEvents.onEngineActiveChange.Add(OnEngineActiveChange);
		GameEvents.onPartResourceFlowStateChange.Add(onPartResourceFlowStateChange);
		GameEvents.onChangeEngineDVIncludeState.Add(onChangeEngineDVIncludeState);
		GameEvents.onPartCrossfeedStateChange.Add(UpdateResourceSetsEvent);
		GameEvents.onVesselSOIChanged.Add(onVesselSOIChanged);
	}

	public void FixedUpdate()
	{
		CheckDirtyAndRun();
	}

	public void Update()
	{
		if (HighLogic.LoadedSceneIsFlight && calcsDirty && FlightDriver.Pause)
		{
			CheckDirtyAndRun();
		}
	}

	public void CheckDirtyAndRun()
	{
		if ((!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor) || !DoStockSimulation || (HighLogic.LoadedSceneIsFlight && _vessel != null && (FlightGlobals.ActiveVessel == null || FlightGlobals.ActiveVessel != _vessel || !FlightGlobals.ready || !_vessel.loaded)))
		{
			return;
		}
		if (vesselEventDelayTime > 0.0)
		{
			if ((double)Time.time - vesselEventDelayTime < (double)GameSettings.DELTAV_VESSEL_EVENT_DELAY_SECS)
			{
				return;
			}
			SetCalcsDirty(resetPartCaches: true);
			vesselEventDelayTime = 0.0;
		}
		if (!SimulationRunning && (calcsDirty || (GameSettings.DELTAV_USE_TIMED_VESSELCALCS && HighLogic.LoadedSceneIsFlight && (double)Time.time - flightLastFullUpdateTime > (double)GameSettings.DELTAV_ALL_STAGES_UPDATE_SECS)))
		{
			simulation = StartCoroutine(CallbackUtil.DelayedCallback(frames, delegate
			{
				StartCoroutine(RunCalculations());
			}));
			return;
		}
		if (HighLogic.LoadedSceneIsFlight && simulation == null && !calcsDirty && (double)Time.time - flightLastStageUpdateTime > (double)GameSettings.DELTAV_ACTIVE_STAGE_UPDATE_SECS)
		{
			SimulateFlightScene();
		}
		if (!SimulationRunning && HighLogic.LoadedSceneIsEditor && syncListInstances)
		{
			simulation = StartCoroutine(CallbackUtil.DelayedCallback(frames, delegate
			{
				StartCoroutine(RunCalculations());
			}));
		}
	}

	public void OnDestroy()
	{
		GameEvents.onEditorShipModified.Remove(onShipModified);
		GameEvents.onPartResourceListChange.Remove(UpdateResourceSetsEvent);
		GameEvents.onPartCrossfeedStateChange.Remove(UpdateResourceSetsEvent);
		GameEvents.onPartFuelLookupStateChange.Remove(UpdateResourceSetsEventSetFL);
		GameEvents.onPartPriorityChanged.Remove(UpdateResourceSetsEvent);
		GameEvents.onVesselWasModified.Remove(onVesselWasModified);
		GameEvents.StageManager.OnStagingSeparationIndices.Remove(OnStagingSeparationIndices);
		GameEvents.StageManager.OnGUIStageSequenceModified.Remove(OnGUIStageSequenceModified);
		GameEvents.StageManager.OnGUIStageAdded.Remove(StagesChanged);
		GameEvents.StageManager.OnGUIStageRemoved.Remove(StagesChanged);
		GameEvents.onDockingComplete.Remove(OnDockingComplete);
		GameEvents.onVesselsUndocking.Remove(OnVesselsUndocking);
		GameEvents.onMultiModeEngineSwitchActive.Remove(OnMultiModeEngineSwitchActive);
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.onVesselGoOffRails.Remove(OnVesselGoOffRails);
		GameEvents.onEngineThrustPercentageChanged.Remove(OnEngineThrustPercentageChanged);
		GameEvents.onPartModuleAdjusterRemoved.Remove(OnPartModuleAdjusterRemoved);
		GameEvents.onPartModuleAdjusterAdded.Remove(OnPartModuleAdjusterAdded);
		GameEvents.onEngineActiveChange.Remove(OnEngineActiveChange);
		GameEvents.Mission.onNodeChangedVesselResources.Remove(OnNodeChangedVesselResources);
		GameEvents.onStageActivate.Remove(OnStageActivate);
		GameEvents.onPartResourceFlowStateChange.Remove(onPartResourceFlowStateChange);
		GameEvents.onChangeEngineDVIncludeState.Remove(onChangeEngineDVIncludeState);
		BaseCrewAssignmentDialog.onCrewDialogChange.Remove(Callback_CrewChange);
		GameEvents.onVesselSOIChanged.Remove(onVesselSOIChanged);
		StopAllCoroutines();
	}

	public void onVesselSOIChanged(GameEvents.HostedFromToAction<Vessel, CelestialBody> fromToActon)
	{
		if (HighLogic.LoadedSceneIsFlight && _vessel != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.persistentId == _vessel.persistentId && fromToActon.host.persistentId == _vessel.persistentId)
		{
			SetCalcsDirty(resetPartCaches: false, syncListInstances: true);
		}
	}

	public void OnStagingSeparationIndices()
	{
		if (HighLogic.LoadedSceneIsFlight && _vessel != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.persistentId == _vessel.persistentId)
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
	}

	public void OnGUIStageSequenceModified()
	{
		if (HighLogic.LoadedSceneIsFlight && _vessel != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.persistentId == _vessel.persistentId)
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
	}

	public void onPartResourceFlowStateChange(GameEvents.HostedFromToAction<PartResource, bool> hostedFromTo)
	{
		if (_partInfo.ContainsPart(hostedFromTo.host.part))
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
	}

	public void OnStageActivate(int stage)
	{
		if (_vessel != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.persistentId == _vessel.persistentId)
		{
			SetCalcsDirty(resetPartCaches: false, syncListInstances: true);
		}
	}

	public void OnNodeChangedVesselResources(MENode node, Vessel vsl, Part part, ProtoPartSnapshot protoPart)
	{
		if (_vessel != null && _vessel == vsl)
		{
			vesselEventDelayTime = Time.time;
			ResetStagePartCaches();
		}
	}

	public void OnEngineThrustPercentageChanged(ModuleEngines engine)
	{
		List<DeltaVEngineInfo> workingEngineInfo = WorkingEngineInfo;
		for (int i = 0; i < workingEngineInfo.Count; i++)
		{
			if (workingEngineInfo[i].engine == engine)
			{
				vesselEventDelayTime = Time.time;
			}
		}
	}

	public void OnVesselGoOffRails(Vessel vessel)
	{
		if (_vessel != null && _vessel == vessel)
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
	}

	public void OnEngineActiveChange(ModuleEngines engine)
	{
		if (!(_vessel != null) || engine.vessel.persistentId != _vessel.persistentId)
		{
			return;
		}
		DeltaVEngineInfo deltaVEngineInfo = null;
		for (int i = 0; i < WorkingEngineInfo.Count; i++)
		{
			if (WorkingEngineInfo[i].engine == engine)
			{
				deltaVEngineInfo = WorkingEngineInfo[i];
				break;
			}
		}
		if (deltaVEngineInfo != null)
		{
			if (engine.EngineIgnited)
			{
				deltaVEngineInfo.startBurnStage = StageManager.LastStage;
			}
			else
			{
				deltaVEngineInfo.startBurnStage = engine.part.inverseStage;
			}
		}
		ResetPartInfo(_vessel.parts);
		SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
	}

	public void OnPartModuleAdjusterRemoved(PartModule module, AdjusterPartModuleBase adjuster)
	{
		if (_vessel != null && module.vessel == _vessel && module is ModuleEngines)
		{
			SetCalcsDirty(resetPartCaches: false, syncListInstances: true);
		}
	}

	public void OnPartModuleAdjusterAdded(PartModule module, AdjusterPartModuleBase adjuster)
	{
		if (_vessel != null && module.vessel == _vessel && module is ModuleEngines)
		{
			SetCalcsDirty(resetPartCaches: false, syncListInstances: true);
		}
	}

	public void OnVesselChange(Vessel vessel)
	{
		if (_vessel != null && vessel.persistentId == _vessel.persistentId)
		{
			vesselEventDelayTime = Time.time;
		}
	}

	public void OnMultiModeEngineSwitchActive(MultiModeEngine engine)
	{
		List<DeltaVEngineInfo> engineInfo_InstanceOne = engineStageSet.engineInfo_InstanceOne;
		bool flag = false;
		for (int i = 0; i < engineInfo_InstanceOne.Count; i++)
		{
			if (engineInfo_InstanceOne[i].multiModeEngine != null && engineInfo_InstanceOne[i].multiModeEngine == engine)
			{
				engineInfo_InstanceOne[i].SwitchEngine();
				flag = true;
			}
		}
		List<DeltaVEngineInfo> engineInfo_InstanceTwo = engineStageSet.engineInfo_InstanceTwo;
		for (int j = 0; j < engineInfo_InstanceTwo.Count; j++)
		{
			if (engineInfo_InstanceTwo[j].multiModeEngine != null && engineInfo_InstanceTwo[j].multiModeEngine == engine)
			{
				engineInfo_InstanceTwo[j].SwitchEngine();
				flag = true;
			}
		}
		if (flag)
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
	}

	public void SetCalcsDirty(bool resetPartCaches, bool syncListInstances = false)
	{
		calcsDirty = true;
		this.resetPartCaches |= resetPartCaches;
		this.syncListInstances |= syncListInstances;
	}

	public void OnDockingComplete(GameEvents.FromToAction<Part, Part> action)
	{
		if ((_vessel != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.persistentId == _vessel.persistentId && action.from.vessel.persistentId == _vessel.persistentId) || action.to.vessel.persistentId == _vessel.persistentId)
		{
			vesselEventDelayTime = Time.time;
		}
	}

	public void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		if ((_vessel != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.persistentId == _vessel.persistentId && oldVessel.persistentId == _vessel.persistentId) || newVessel.persistentId == _vessel.persistentId)
		{
			vesselEventDelayTime = Time.time;
		}
	}

	public void StagesChanged(int stage)
	{
		if (HighLogic.LoadedSceneIsEditor && ActiveMode == Mode.Ship)
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
		else if (HighLogic.LoadedSceneIsFlight && ActiveMode == Mode.Vessel && _vessel != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.persistentId == _vessel.persistentId)
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
	}

	public void UpdateResourceSetsEvent(Part part)
	{
		if (ActiveMode == Mode.Ship && _ship != null && _ship.parts.Contains(part))
		{
			SetCalcsDirty(resetPartCaches: true);
		}
		else if (HighLogic.LoadedSceneIsFlight && ActiveMode == Mode.Vessel && _vessel != null && _vessel.parts.Contains(part))
		{
			SetCalcsDirty(resetPartCaches: true);
		}
	}

	public void UpdateResourceSetsEventSetFL(GameEvents.HostedFromToAction<bool, Part> data)
	{
		if (ActiveMode == Mode.Ship && _ship != null && _ship.parts.Contains(data.from))
		{
			SetCalcsDirty(resetPartCaches: true, syncListInstances: true);
		}
	}

	public void onShipModified(ShipConstruct modifiedShip)
	{
		if (ActiveMode == Mode.Ship && _ship != null && modifiedShip == _ship)
		{
			SetCalcsDirty(resetPartCaches: false, syncListInstances: true);
		}
	}

	public void onVesselWasModified(Vessel modifiedVessel)
	{
		if (HighLogic.LoadedSceneIsFlight && ActiveMode == Mode.Vessel && _vessel != null && modifiedVessel.persistentId == _vessel.persistentId)
		{
			SetCalcsDirty(resetPartCaches: false, syncListInstances: true);
		}
	}

	public void Callback_CrewChange(VesselCrewManifest crewManifest)
	{
		SetCalcsDirty(resetPartCaches: false);
	}

	public void onChangeEngineDVIncludeState(ModuleEngines engine)
	{
		if (_partInfo.Get(engine.part) != null)
		{
			SetCalcsDirty(resetPartCaches: true);
		}
	}

	public void CalculateSeparationStageIndexes(List<Part> parts)
	{
		_separationStageIndexes.Clear();
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].HasModuleImplementing<ModuleDecouple>() || parts[i].HasModuleImplementing<ModuleAnchoredDecoupler>())
			{
				IStageSeparatorChild stageSeparatorChild = parts[i].FindModuleImplementing<IStageSeparatorChild>();
				List<Part> decoupledChildParts = new List<Part>();
				if (stageSeparatorChild != null && !stageSeparatorChild.PartDetaches(out decoupledChildParts))
				{
					_separationStageIndexes.AddUnique(parts[i].inverseStage);
				}
				_separationStageIndexes.AddUnique(parts[i].separationIndex);
			}
		}
	}

	public void UpdateStageInfo()
	{
		if (_vessel != null)
		{
			CalculateSeparationStageIndexes(_vessel.parts);
			ResetPartInfo(_vessel.parts);
		}
		else if (_ship != null)
		{
			CalculateSeparationStageIndexes(_ship.parts);
			ResetPartInfo(_ship.parts);
		}
		engineStageSet.UpdateStageInfo();
		ProcessPayloadStages();
	}

	public void ProcessPayloadStages()
	{
		engineStageSet.payloadStages.Clear();
		List<DeltaVStageInfo> workingStageInfo = WorkingStageInfo;
		for (int i = 0; i < workingStageInfo.Count; i++)
		{
			DeltaVStageInfo deltaVStageInfo = workingStageInfo[i];
			if (deltaVStageInfo.stage == StageManager.CurrentStage)
			{
				continue;
			}
			bool flag = false;
			for (int j = 0; j < deltaVStageInfo.enginesInStage.Count; j++)
			{
				DeltaVEngineInfo deltaVEngineInfo = deltaVStageInfo.enginesInStage[j];
				if (deltaVEngineInfo.partInfo != null && !(deltaVEngineInfo.engine == null) && !(deltaVEngineInfo.engine.part == null) && deltaVEngineInfo.partInfo.decoupleBeforeBurn && deltaVEngineInfo.partInfo.part.inverseStage == deltaVStageInfo.stage && (!deltaVEngineInfo.engine.nonThrustMotor || (deltaVEngineInfo.engine.nonThrustMotor && deltaVEngineInfo.engine.includeinDVCalcs)))
				{
					if (GameSettings.LOG_DELTAV_VERBOSE)
					{
						Debug.LogFormat("[VesselDeltaV]: Engine {0} {1} Decouples before it Activates in Staging - Stage {2}", deltaVEngineInfo.engine.part.persistentId, deltaVEngineInfo.engine.part.partInfo.title, deltaVEngineInfo.engine.part.inverseStage);
					}
					flag = true;
				}
			}
			if (flag)
			{
				if (GameSettings.LOG_DELTAV_VERBOSE)
				{
					Debug.LogFormat("[VesselDeltaV]: Stage {0} - all Engines in this stage decouple before they Activate. Stage set to Payload Stage.", deltaVStageInfo.stage);
				}
				engineStageSet.payloadStages.AddUnique(deltaVStageInfo.stage);
			}
		}
	}

	public void ResetPartInfo(List<Part> parts)
	{
		int count = _partInfo.Count;
		while (count-- > 0)
		{
			if (_partInfo[count].part == null || !parts.Contains(_partInfo[count].part))
			{
				_partInfo.RemoveAt(count);
			}
		}
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isLaunchClamp())
			{
				continue;
			}
			bool flag = false;
			for (int j = 0; j < _partInfo.Count; j++)
			{
				if (_partInfo[j].part == parts[i])
				{
					_partInfo[j].decoupleBeforeBurn = false;
					_partInfo[j].CalculateMassValues();
					_partInfo[j].CalculateStagingValues();
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				DeltaVPartInfo item = new DeltaVPartInfo(parts[i], this);
				_partInfo.Add(item);
			}
		}
		for (int k = 0; k < _partInfo.Count; k++)
		{
			DeltaVPartInfo deltaVPartInfo = _partInfo[k];
			if (deltaVPartInfo.isEngine && deltaVPartInfo.engines.Count > 0)
			{
				AttachNode attachNode = deltaVPartInfo.part.FindAttachNode("bottom");
				if (attachNode != null && attachNode.attachedPart != null)
				{
					int num = WorkingEngineInfo.Get(deltaVPartInfo.part)?.startBurnStage ?? deltaVPartInfo.activationStage;
					DeltaVPartInfo deltaVPartInfo2 = _partInfo.Get(attachNode.attachedPart);
					if (deltaVPartInfo2 != null && !deltaVPartInfo.engines[0].nonThrustMotor && deltaVPartInfo2.decoupleStage <= num)
					{
						if (GameSettings.LOG_DELTAV_VERBOSE)
						{
							Debug.LogFormat("[VesselDeltaV]: Child {0} of Activated Part {1} decouple stage changed from {2} to {3}", deltaVPartInfo2.part.partInfo.title, deltaVPartInfo.part.partInfo.title, deltaVPartInfo2.decoupleStage, num);
						}
						deltaVPartInfo2.decoupleStage = num;
						if (deltaVPartInfo2.activationStage < deltaVPartInfo2.decoupleStage)
						{
							if (GameSettings.LOG_DELTAV_VERBOSE)
							{
								Debug.LogFormat("[VesselDeltaV]: Child {0} Activation Stage {1} changed to Decouple Stage {2}", deltaVPartInfo2.part.partInfo.title, deltaVPartInfo2.activationStage, deltaVPartInfo2.decoupleStage);
							}
							deltaVPartInfo2.activationStage = deltaVPartInfo2.decoupleStage;
						}
						if (deltaVPartInfo2.isEngine && (WorkingEngineInfo.Get(deltaVPartInfo2.part)?.startBurnStage ?? deltaVPartInfo2.activationStage) <= num)
						{
							deltaVPartInfo2.decoupleBeforeBurn = true;
						}
						ProcessEnginePartInfoChildren(deltaVPartInfo2);
					}
				}
			}
			if (deltaVPartInfo.isDockingPort && deltaVPartInfo.moduleDockingNode != null && deltaVPartInfo.moduleDockingNode.StagingEnabled() && (!HighLogic.LoadedSceneIsFlight || (HighLogic.LoadedSceneIsFlight && deltaVPartInfo.activationStage > 0) || (HighLogic.LoadedSceneIsFlight && deltaVPartInfo.activationStage == StageManager.CurrentStage)))
			{
				DeltaVPartInfo deltaVPartInfo3 = null;
				if (deltaVPartInfo.part != null && deltaVPartInfo.part.parent != null)
				{
					deltaVPartInfo3 = _partInfo.Get(deltaVPartInfo.part.parent);
				}
				if (deltaVPartInfo3 != null && deltaVPartInfo.decoupleStage != deltaVPartInfo3.decoupleStage && !deltaVPartInfo3.isEngine)
				{
					deltaVPartInfo.decoupleStage = deltaVPartInfo3.decoupleStage;
				}
				bool setToParentDecoupleStage = false;
				bool bypassFairings = false;
				if ((HighLogic.LoadedSceneIsFlight && deltaVPartInfo.activationStage == StageManager.CurrentStage) || GetActivatedEngines() > 0)
				{
					deltaVPartInfo.activationStage--;
					setToParentDecoupleStage = true;
					bypassFairings = true;
				}
				ProcessDecouplePartInfoChildren(deltaVPartInfo, PartStageComparisonOperator.NotEqual, deltaVPartInfo.decoupleStage, deltaVPartInfo.activationStage, bypassFairings, bypassJettison: true, setToParentDecoupleStage);
			}
			if (deltaVPartInfo.isDockingPort && deltaVPartInfo.moduleDockingNode != null && !deltaVPartInfo.moduleDockingNode.StagingEnabled())
			{
				ProcessDecouplePartInfoChildren(deltaVPartInfo, PartStageComparisonOperator.NotEqual, deltaVPartInfo.decoupleStage, deltaVPartInfo.activationStage, bypassFairings: true, bypassJettison: true, setToParentDecoupleStage: false);
			}
			if ((deltaVPartInfo.isDecoupler && ((deltaVPartInfo.moduleDecoupler != null && deltaVPartInfo.moduleDecoupler.stagingEnabled) || (deltaVPartInfo.moduleAnchoredDecoupler != null && deltaVPartInfo.moduleAnchoredDecoupler.stagingEnabled))) || deltaVPartInfo.isStageSeparator)
			{
				bool flag2 = false;
				bool bypassFairings2 = false;
				if (HighLogic.LoadedSceneIsFlight && (deltaVPartInfo.activationStage == StageManager.CurrentStage || GetActivatedEngines() > 0))
				{
					deltaVPartInfo.activationStage--;
					flag2 = true;
					bypassFairings2 = true;
				}
				List<Part> decoupledChildParts = new List<Part>();
				IStageSeparatorChild stageSeparatorChild = deltaVPartInfo.part.FindModuleImplementing<IStageSeparatorChild>();
				if (stageSeparatorChild != null)
				{
					flag2 = stageSeparatorChild.IsEnginePlate() || flag2;
					if (!stageSeparatorChild.PartDetaches(out decoupledChildParts))
					{
						if (decoupledChildParts.Count > 0)
						{
							for (int l = 0; l < decoupledChildParts.Count; l++)
							{
								DeltaVPartInfo deltaVPartInfo4 = _partInfo.Get(decoupledChildParts[l]);
								if (deltaVPartInfo4 != null && deltaVPartInfo4.decoupleStage < deltaVPartInfo.activationStage)
								{
									changePartInfoDecoupleStage(deltaVPartInfo, deltaVPartInfo4);
								}
								ProcessDecouplePartInfoChildren(deltaVPartInfo4, PartStageComparisonOperator.LessThan, deltaVPartInfo.decoupleStage, deltaVPartInfo.activationStage, bypassFairings2, bypassJettison: false, flag2);
							}
						}
						else if (stageSeparatorChild.IsEnginePlate())
						{
							ProcessDecouplePartInfoChildren(deltaVPartInfo, PartStageComparisonOperator.Equal, deltaVPartInfo.decoupleStage, deltaVPartInfo.activationStage, bypassFairings2, bypassJettison: false, flag2);
						}
						else
						{
							ProcessDecouplePartInfoChildren(deltaVPartInfo, PartStageComparisonOperator.LessThan, deltaVPartInfo.decoupleStage, deltaVPartInfo.activationStage, bypassFairings2, bypassJettison: false, flag2);
						}
					}
					else
					{
						ProcessDecouplePartInfoChildren(deltaVPartInfo, PartStageComparisonOperator.LessThan, deltaVPartInfo.decoupleStage, deltaVPartInfo.activationStage, bypassFairings2, bypassJettison: true, flag2);
					}
				}
			}
			if (deltaVPartInfo.isDecoupler && ((deltaVPartInfo.moduleDecoupler != null && !deltaVPartInfo.moduleDecoupler.stagingEnabled) || (deltaVPartInfo.moduleAnchoredDecoupler != null && !deltaVPartInfo.moduleAnchoredDecoupler.stagingEnabled)))
			{
				ProcessDecouplePartInfoChildren(deltaVPartInfo, PartStageComparisonOperator.LessThan, deltaVPartInfo.decoupleStage, deltaVPartInfo.activationStage, bypassFairings: false, bypassJettison: true, setToParentDecoupleStage: false);
			}
		}
	}

	public void ProcessEnginePartInfoChildren(DeltaVPartInfo partInfoItem)
	{
		for (int i = 0; i < partInfoItem.part.children.Count; i++)
		{
			DeltaVPartInfo deltaVPartInfo = _partInfo.Get(partInfoItem.part.children[i]);
			if (deltaVPartInfo != null && deltaVPartInfo.decoupleStage < partInfoItem.activationStage)
			{
				deltaVPartInfo.decoupleStage = partInfoItem.activationStage;
				if (deltaVPartInfo.activationStage < deltaVPartInfo.decoupleStage)
				{
					deltaVPartInfo.activationStage = deltaVPartInfo.decoupleStage;
				}
				if (deltaVPartInfo.isEngine && (WorkingEngineInfo.Get(deltaVPartInfo.part)?.startBurnStage ?? deltaVPartInfo.activationStage) <= partInfoItem.activationStage)
				{
					deltaVPartInfo.decoupleBeforeBurn = true;
				}
				ProcessEnginePartInfoChildren(deltaVPartInfo);
			}
		}
	}

	public void ProcessDecouplePartInfoChildren(DeltaVPartInfo partInfoItem, PartStageComparisonOperator comparisonOp, int decoupleStage, int activationStage, bool bypassFairings, bool bypassJettison, bool setToParentDecoupleStage)
	{
		for (int i = 0; i < partInfoItem.part.children.Count; i++)
		{
			DeltaVPartInfo deltaVPartInfo = _partInfo.Get(partInfoItem.part.children[i]);
			if (deltaVPartInfo == null || (!bypassJettison && partInfoItem.jettisonPart != null && deltaVPartInfo.part != partInfoItem.jettisonPart) || (deltaVPartInfo.isFairing && !bypassFairings && deltaVPartInfo.decoupleStage > decoupleStage) || deltaVPartInfo.isDecoupler || (deltaVPartInfo.isDockingPort && partInfoItem.moduleDockingNode != null && partInfoItem.moduleDockingNode.StagingEnabled() && deltaVPartInfo.decoupleStage > decoupleStage))
			{
				continue;
			}
			switch (comparisonOp)
			{
			case PartStageComparisonOperator.LessThan:
				if (deltaVPartInfo.decoupleStage < activationStage)
				{
					changePartInfoDecoupleStage(partInfoItem, deltaVPartInfo, setToParentDecoupleStage);
				}
				break;
			case PartStageComparisonOperator.NotEqual:
				if (partInfoItem.isFairing)
				{
					changePartInfoDecoupleStage(partInfoItem, deltaVPartInfo, setToParentDecoupleStage: true);
				}
				else if (deltaVPartInfo.decoupleStage != activationStage)
				{
					changePartInfoDecoupleStage(partInfoItem, deltaVPartInfo, setToParentDecoupleStage);
				}
				break;
			case PartStageComparisonOperator.Equal:
				if (deltaVPartInfo.decoupleStage == activationStage)
				{
					changePartInfoDecoupleStage(partInfoItem, deltaVPartInfo, setToParentDecoupleStage);
				}
				break;
			}
			ProcessDecouplePartInfoChildren(deltaVPartInfo, comparisonOp, decoupleStage, activationStage, bypassFairings, bypassJettison, setToParentDecoupleStage);
		}
	}

	public void changePartInfoDecoupleStage(DeltaVPartInfo partInfoItem, DeltaVPartInfo childPart, bool setToParentDecoupleStage = false)
	{
		if (setToParentDecoupleStage)
		{
			childPart.decoupleStage = partInfoItem.decoupleStage;
		}
		else
		{
			childPart.decoupleStage = partInfoItem.activationStage;
		}
		if (childPart.activationStage < childPart.decoupleStage)
		{
			childPart.activationStage = childPart.decoupleStage;
		}
		if (childPart.isEngine)
		{
			if (childPart.activationStage == childPart.decoupleStage)
			{
				childPart.decoupleBeforeBurn = true;
			}
			else
			{
				childPart.decoupleBeforeBurn = false;
			}
		}
	}

	public IEnumerator RunCalculations()
	{
		lowestStageWithDeltaV = int.MaxValue;
		if (useMultipleInfoLists)
		{
			engineStageSet.workingIndex = ((engineStageSet.operatingIndex == 0) ? 1 : 0);
		}
		else
		{
			DeltaVEngineStageSet deltaVEngineStageSet = engineStageSet;
			engineStageSet.operatingIndex = 0;
			deltaVEngineStageSet.workingIndex = 0;
		}
		if (resetPartCaches)
		{
			ResetStagePartCaches();
			resetPartCaches = false;
		}
		calcsDirty = false;
		UpdateModuleEngines();
		UpdateStageInfo();
		if (WorkingEngineInfo.Count <= 0)
		{
			ReCalculateVesselTotals();
			if (useMultipleInfoLists)
			{
				engineStageSet.operatingIndex = engineStageSet.workingIndex;
			}
			if (syncListInstances)
			{
				syncListInstances = false;
				SetCalcsDirty(resetPartCaches: true);
			}
			simulation = null;
			GameEvents.onDeltaVCalcsCompleted.Fire();
			yield break;
		}
		_totalDeltaVVac = 0.0;
		_totalDeltaVASL = 0.0;
		_totalDeltaVActual = 0.0;
		_totalBurnTime = 0.0;
		ResetSimulationResources();
		yield return null;
		List<DeltaVStageInfo> stages = WorkingStageInfo;
		for (int i = 0; i < stages.Count; i++)
		{
			stages[i].SimulateDeltaV(ActiveMode == Mode.Vessel, infiniteFuel: false, GameSettings.DELTAV_CALCULATIONS_TIMESTEP, GameSettings.LOG_DELTAV_VERBOSE);
			if (!double.IsNaN(stages[i].deltaVinVac))
			{
				_totalDeltaVVac += stages[i].deltaVinVac;
			}
			if (!double.IsNaN(stages[i].deltaVatASL))
			{
				_totalDeltaVASL += stages[i].deltaVatASL;
			}
			if (!double.IsNaN(stageDVActual(stages[i])))
			{
				_totalDeltaVActual += stageDVActual(stages[i]);
			}
			if (!double.IsNaN(stages[i].stageBurnTime))
			{
				_totalBurnTime += stages[i].stageBurnTime;
			}
			if ((stages[i].deltaVinVac > 0f || stages[i].deltaVatASL > 0f) && stages[i].stage < lowestStageWithDeltaV)
			{
				lowestStageWithDeltaV = stages[i].stage;
			}
			yield return null;
		}
		_ = Time.realtimeSinceStartup;
		ResetSimulationResources();
		flightLastFullUpdateTime = Time.time;
		if (useMultipleInfoLists)
		{
			engineStageSet.operatingIndex = engineStageSet.workingIndex;
			engineStageSet.flightSceneOperatingIndex = engineStageSet.operatingIndex;
		}
		if (syncListInstances)
		{
			syncListInstances = false;
			SetCalcsDirty(resetPartCaches: true);
		}
		else
		{
			_isReady = true;
			GameEvents.onDeltaVCalcsCompleted.Fire();
		}
		simulation = null;
	}

	public void SimulateFlightScene()
	{
		flightLastStageUpdateTime = Time.time;
		if (StageManager.StageCount <= 0)
		{
			return;
		}
		updateFlightScene = true;
		if (useMultipleInfoLists)
		{
			engineStageSet.workingIndex = ((engineStageSet.operatingIndex == 0) ? 1 : 0);
		}
		else
		{
			DeltaVEngineStageSet deltaVEngineStageSet = engineStageSet;
			engineStageSet.operatingIndex = 0;
			deltaVEngineStageSet.workingIndex = 0;
		}
		SimulateLastStage(recalcVesselTotals: false);
		List<DeltaVStageInfo> workingStageInfo = WorkingStageInfo;
		for (int i = 0; i < workingStageInfo.Count; i++)
		{
			if (workingStageInfo[i].stage != StageManager.LastStage)
			{
				workingStageInfo[i].CalcLerpDeltaV();
				if ((workingStageInfo[i].deltaVinVac > 0f || workingStageInfo[i].deltaVatASL > 0f) && workingStageInfo[i].stage < lowestStageWithDeltaV)
				{
					lowestStageWithDeltaV = workingStageInfo[i].stage;
				}
			}
		}
		ReCalculateVesselTotals();
		if (useMultipleInfoLists)
		{
			engineStageSet.operatingIndex = engineStageSet.workingIndex;
		}
		updateFlightScene = false;
	}

	public void SimulateLastStage(bool recalcVesselTotals = true)
	{
		DeltaVStageInfo workingStage = GetWorkingStage(StageManager.CurrentStage);
		if (workingStage == null && GetActivatedEngines() > 0)
		{
			workingStage = GetWorkingStage(StageManager.LastStage);
		}
		currentStageActivated = false;
		if (workingStage != null)
		{
			ProcessActiveUpdateEngines(workingStage);
			ResetSimulationResources();
			for (int i = 0; i < _partInfo.Count; i++)
			{
				_partInfo[i].CalculateMassValues();
			}
			workingStage.SimulateDeltaV(ActiveMode == Mode.Vessel, infiniteFuel: false, GameSettings.DELTAV_ACTIVE_VESSEL_TIMESTEP, GameSettings.LOG_DELTAV_VERBOSE, thisStageActive: true);
			if (recalcVesselTotals)
			{
				ReCalculateVesselTotals();
			}
			ResetSimulationResources();
			currentStageActivated = true;
			if (workingStage.deltaVinVac > 0f || workingStage.deltaVatASL > 0f)
			{
				lowestStageWithDeltaV = workingStage.stage;
			}
		}
	}

	public void ReCalculateVesselTotals()
	{
		_totalDeltaVVac = 0.0;
		_totalDeltaVASL = 0.0;
		_totalDeltaVActual = 0.0;
		_totalBurnTime = 0.0;
		List<DeltaVStageInfo> workingStageInfo = WorkingStageInfo;
		for (int i = 0; i < workingStageInfo.Count; i++)
		{
			if (!double.IsNaN(workingStageInfo[i].deltaVinVac))
			{
				_totalDeltaVVac += workingStageInfo[i].deltaVinVac;
			}
			if (!double.IsNaN(workingStageInfo[i].deltaVatASL))
			{
				_totalDeltaVASL += workingStageInfo[i].deltaVatASL;
			}
			if (!double.IsNaN(stageDVActual(workingStageInfo[i])))
			{
				_totalDeltaVActual += stageDVActual(workingStageInfo[i]);
			}
			if (!double.IsNaN(workingStageInfo[i].stageBurnTime))
			{
				_totalBurnTime += workingStageInfo[i].stageBurnTime;
			}
		}
	}

	public double stageDVActual(DeltaVStageInfo stageInfo)
	{
		if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
		{
			return stageInfo.deltaVActual;
		}
		if (HighLogic.LoadedSceneIsEditor && DeltaVGlobals.DeltaVAppValues != null)
		{
			return stageInfo.deltaVActual;
		}
		return stageInfo.deltaVinVac;
	}

	public void ResetSimulationResources()
	{
		if (ActiveMode == Mode.Ship && _ship != null)
		{
			for (int i = 0; i < _ship.parts.Count; i++)
			{
				_ship.parts[i].ResetSimulation();
			}
		}
		if (ActiveMode == Mode.Vessel && _vessel != null)
		{
			for (int j = 0; j < _vessel.parts.Count; j++)
			{
				_vessel.parts[j].ResetSimulation();
			}
		}
	}

	public void ResetStagePartCaches()
	{
		List<DeltaVStageInfo> workingStageInfo = WorkingStageInfo;
		for (int i = 0; i < workingStageInfo.Count; i++)
		{
			workingStageInfo[i].ResetPartCaches();
		}
		List<DeltaVStageInfo> operatingStageInfo = OperatingStageInfo;
		for (int j = 0; j < operatingStageInfo.Count; j++)
		{
			operatingStageInfo[j].ResetPartCaches();
		}
	}

	public void UpdateModuleEngines()
	{
		List<DeltaVEngineInfo> workingEngineInfo = WorkingEngineInfo;
		List<MultiModeEngine> list = new List<MultiModeEngine>();
		if (ActiveMode == Mode.Vessel && _vessel != null)
		{
			List<ModuleEngines> list2 = _vessel.FindPartModulesImplementing<ModuleEngines>();
			for (int i = 0; i < list2.Count; i++)
			{
				if ((list2[i].nonThrustMotor && !list2[i].includeinDVCalcs) || !(list2[i].part != null))
				{
					continue;
				}
				MultiModeEngine component = list2[i].part.GetComponent<MultiModeEngine>();
				if (component != null)
				{
					if (list.Contains(component))
					{
						continue;
					}
					list.Add(component);
				}
				DeltaVEngineInfo deltaVEngineInfo = workingEngineInfo.Get(list2[i].part);
				if (deltaVEngineInfo == null)
				{
					deltaVEngineInfo = new DeltaVEngineInfo(this, list2[i], component);
					engineStageSet.AddEngineWorkingSet(deltaVEngineInfo);
				}
				else
				{
					deltaVEngineInfo.Reset();
				}
				if (list2[i].EngineIgnited && deltaVEngineInfo.startBurnStage < StageManager.LastStage)
				{
					deltaVEngineInfo.startBurnStage = StageManager.LastStage;
				}
			}
		}
		else if (ActiveMode == Mode.Ship && _ship != null)
		{
			for (int j = 0; j < _ship.parts.Count; j++)
			{
				List<ModuleEngines> engines = new List<ModuleEngines>();
				if (!_ship.parts[j].isEngine(out engines))
				{
					continue;
				}
				for (int k = 0; k < engines.Count; k++)
				{
					if ((engines[k].nonThrustMotor && !engines[k].includeinDVCalcs) || !(engines[k].part != null))
					{
						continue;
					}
					MultiModeEngine component2 = engines[k].part.GetComponent<MultiModeEngine>();
					if (component2 != null)
					{
						if (list.Contains(component2))
						{
							continue;
						}
						list.Add(component2);
					}
					DeltaVEngineInfo deltaVEngineInfo2 = workingEngineInfo.Get(engines[k].part);
					if (deltaVEngineInfo2 == null)
					{
						deltaVEngineInfo2 = new DeltaVEngineInfo(this, engines[k], component2);
						engineStageSet.AddEngineWorkingSet(deltaVEngineInfo2);
					}
					else
					{
						deltaVEngineInfo2.Reset();
					}
				}
			}
		}
		engineStageSet.RemoveInvalidEnginesWorkingSet();
	}

	public void ProcessActiveUpdateEngines(DeltaVStageInfo currentDeltaVStage)
	{
		List<DeltaVEngineInfo> workingEngineInfo = WorkingEngineInfo;
		for (int i = 0; i < workingEngineInfo.Count; i++)
		{
			workingEngineInfo[i].deprived = false;
			if (currentDeltaVStage != null)
			{
				workingEngineInfo[i].stageBurnTotals.Remove(currentDeltaVStage.stage);
			}
		}
		List<DeltaVStageInfo> workingStageInfo = WorkingStageInfo;
		for (int j = 0; j < workingStageInfo.Count; j++)
		{
			workingStageInfo[j].ProcessActiveEngines();
		}
	}

	public DeltaVStageInfo GetWorkingStage(int stage)
	{
		List<DeltaVStageInfo> workingStageInfo = WorkingStageInfo;
		int num = 0;
		while (true)
		{
			if (num < workingStageInfo.Count)
			{
				if (workingStageInfo[num].stage == stage)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return workingStageInfo[num];
	}

	public DeltaVStageInfo GetStage(int stage)
	{
		List<DeltaVStageInfo> operatingStageInfo = OperatingStageInfo;
		int num = 0;
		while (true)
		{
			if (num < operatingStageInfo.Count)
			{
				if (operatingStageInfo[num].stage == stage)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return operatingStageInfo[num];
	}

	public int GetActivatedEngines()
	{
		int num = 0;
		List<DeltaVEngineInfo> workingEngineInfo = WorkingEngineInfo;
		for (int i = 0; i < workingEngineInfo.Count; i++)
		{
			if (workingEngineInfo[i].engine != null && workingEngineInfo[i].engine.isOperational)
			{
				num++;
			}
		}
		return num;
	}

	public int GetHighestSeparationStage(int inStage)
	{
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < _separationStageIndexes.Count)
			{
				if (_separationStageIndexes[num2] == inStage)
				{
					break;
				}
				if (_separationStageIndexes[num2] < inStage)
				{
					num = Math.Max(num, _separationStageIndexes[num2]);
				}
				num2++;
				continue;
			}
			return num;
		}
		return inStage;
	}

	public void ResetSeparationIndexes()
	{
		if (ActiveMode == Mode.Vessel && _vessel != null)
		{
			CalculateSeparationStageIndexes(_vessel.parts);
		}
		else if (ActiveMode == Mode.Ship && _ship != null)
		{
			CalculateSeparationStageIndexes(_ship.parts);
		}
	}
}
