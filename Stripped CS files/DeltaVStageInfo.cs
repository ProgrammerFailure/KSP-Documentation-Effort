using System;
using System.Collections.Generic;
using System.Text;
using ns11;
using ns3;
using ns9;
using UnityEngine;

[Serializable]
public class DeltaVStageInfo
{
	public int stage;

	public int separationIndex;

	[NonSerialized]
	public VesselDeltaV vesselDeltaV;

	public bool payloadStage;

	public List<DeltaVPartInfo> parts;

	public List<ModuleResourceIntake> airIntakeParts;

	public List<DeltaVEngineInfo> enginesActiveInStage;

	public List<DeltaVEngineInfo> enginesInStage;

	public float stageMass;

	public float dryMass;

	public float fuelMass;

	public float startMass;

	public float endMass;

	public float decoupledMass;

	public double ispVac;

	public double ispASL;

	public double ispActual;

	public float TWRVac;

	public float TWRASL;

	public float TWRActual;

	public float thrustVac;

	public float thrustASL;

	public float thrustActual;

	public float vectoredThrustVac;

	public float vectoredThrustASL;

	public float vectoredThrustActual;

	public float deltaVinVac;

	public float deltaVatASL;

	public float deltaVActual;

	public double stageBurnTime;

	public List<DeltaVCalc> deltaVCalcs;

	public float totalExhaustVelocityVAC;

	public float totalExhaustVelocityASL;

	public float totalExhaustVelocityActual;

	public Vector3 vectoredExhaustVelocityVAC;

	public Vector3 vectoredExhaustVelocityASL;

	public Vector3 vectoredExhaustVelocityActual;

	public bool partsDisplayListDirty;

	public StringBuilder partsDisplayList;

	public Vector3 enginesThrustVac = Vector3.zero;

	public Vector3 enginesThrustASL = Vector3.zero;

	public Vector3 enginesThrustActual = Vector3.zero;

	public List<Part> cachedResourcePartSetParts;

	public List<DeltaVPartInfo> cachedActivatedParts;

	public SCCFlowGraph cachedFlowGraph;

	public Dictionary<uint, DeltaVPartInfo> partInfoDictionary;

	public float removedFuelMass;

	[SerializeField]
	public bool stageContainsOnlyLaunchClamps;

	public DeltaVStageInfo(ShipConstruct ship, int inStage, VesselDeltaV vesselDeltaV)
	{
		parts = new List<DeltaVPartInfo>();
		partInfoDictionary = new Dictionary<uint, DeltaVPartInfo>();
		enginesActiveInStage = new List<DeltaVEngineInfo>();
		enginesInStage = new List<DeltaVEngineInfo>();
		deltaVCalcs = new List<DeltaVCalc>();
		airIntakeParts = new List<ModuleResourceIntake>();
		this.vesselDeltaV = vesselDeltaV;
		stage = inStage;
		if (ship != null)
		{
			ProcessParts(inStage);
		}
	}

	public DeltaVStageInfo(Vessel vessel, int inStage, VesselDeltaV vesselDeltaV)
	{
		parts = new List<DeltaVPartInfo>();
		partInfoDictionary = new Dictionary<uint, DeltaVPartInfo>();
		enginesActiveInStage = new List<DeltaVEngineInfo>();
		enginesInStage = new List<DeltaVEngineInfo>();
		deltaVCalcs = new List<DeltaVCalc>();
		airIntakeParts = new List<ModuleResourceIntake>();
		this.vesselDeltaV = vesselDeltaV;
		stage = inStage;
		if (vessel != null)
		{
			ProcessParts(inStage);
		}
	}

	public double GetSituationISP(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(ispASL, ispActual, ispVac);
	}

	public float GetSituationTWR(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(TWRASL, TWRActual, TWRVac);
	}

	public float GetSituationThrust(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(thrustASL, thrustActual, thrustVac);
	}

	public float GetSituationVectoredThrust(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(vectoredThrustASL, vectoredThrustActual, vectoredThrustVac);
	}

	public float GetSituationDeltaV(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(deltaVatASL, deltaVActual, deltaVinVac);
	}

	public float GetSituationTotalExhaustVelocity(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(totalExhaustVelocityASL, totalExhaustVelocityActual, totalExhaustVelocityVAC);
	}

	public Vector3 GetSituationVectoredExhaustVelocity(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(vectoredExhaustVelocityASL, vectoredExhaustVelocityActual, vectoredExhaustVelocityVAC);
	}

	public void Reset(int inStage, VesselDeltaV vesselDeltaV)
	{
		this.vesselDeltaV = vesselDeltaV;
		if (airIntakeParts == null)
		{
			airIntakeParts = new List<ModuleResourceIntake>();
		}
		if (parts == null)
		{
			parts = new List<DeltaVPartInfo>();
			partInfoDictionary = new Dictionary<uint, DeltaVPartInfo>();
		}
		if (enginesActiveInStage == null)
		{
			enginesActiveInStage = new List<DeltaVEngineInfo>();
		}
		if (enginesInStage == null)
		{
			enginesInStage = new List<DeltaVEngineInfo>();
		}
		if (deltaVCalcs == null)
		{
			deltaVCalcs = new List<DeltaVCalc>();
		}
		stage = inStage;
		payloadStage = false;
		deltaVinVac = 0f;
		deltaVatASL = 0f;
		deltaVActual = 0f;
		stageMass = 0f;
		endMass = 0f;
		dryMass = 0f;
		fuelMass = 0f;
		decoupledMass = 0f;
		TWRActual = 0f;
		TWRASL = 0f;
		TWRVac = 0f;
		ispActual = 0.0;
		ispASL = 0.0;
		ispVac = 0.0;
		thrustActual = 0f;
		thrustASL = 0f;
		thrustVac = 0f;
		stageBurnTime = 0.0;
		vectoredThrustVac = 0f;
		vectoredThrustASL = 0f;
		vectoredThrustActual = 0f;
		totalExhaustVelocityVAC = 0f;
		totalExhaustVelocityASL = 0f;
		totalExhaustVelocityActual = 0f;
		ProcessParts(inStage);
	}

	public void ProcessParts(int inStage)
	{
		partsDisplayListDirty = true;
		stageContainsOnlyLaunchClamps = false;
		airIntakeParts.Clear();
		bool flag = false;
		int count = parts.Count;
		while (count-- > 0)
		{
			bool flag2 = false;
			if (vesselDeltaV.ActiveMode == VesselDeltaV.Mode.Ship && vesselDeltaV.Ship != null)
			{
				for (int i = 0; i < vesselDeltaV.Ship.parts.Count; i++)
				{
					if (vesselDeltaV.Ship.parts[i].persistentId == parts[count].part.persistentId)
					{
						flag2 = true;
						break;
					}
				}
			}
			else if (vesselDeltaV.ActiveMode == VesselDeltaV.Mode.Vessel)
			{
				for (int j = 0; j < vesselDeltaV.Vessel.parts.Count; j++)
				{
					if (vesselDeltaV.Vessel.parts[j].persistentId == parts[count].part.persistentId)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				if (partInfoDictionary.ContainsKey(parts[count].part.persistentId))
				{
					partInfoDictionary.Remove(parts[count].part.persistentId);
				}
				parts.RemoveAt(count);
				flag = true;
			}
		}
		for (int k = 0; k < vesselDeltaV.PartInfo.Count; k++)
		{
			DeltaVPartInfo deltaVPartInfo = vesselDeltaV.PartInfo[k];
			if (deltaVPartInfo.decoupleStage <= inStage)
			{
				if (!partInfoDictionary.ContainsKey(deltaVPartInfo.part.persistentId))
				{
					partInfoDictionary.Add(deltaVPartInfo.part.persistentId, deltaVPartInfo);
					parts.Add(deltaVPartInfo);
					flag = true;
				}
				if (deltaVPartInfo.isIntake && deltaVPartInfo.moduleResourceIntake != null)
				{
					airIntakeParts.Add(deltaVPartInfo.moduleResourceIntake);
				}
				separationIndex = Math.Max(separationIndex, deltaVPartInfo.decoupleStage);
			}
			else if (partInfoDictionary.ContainsKey(vesselDeltaV.PartInfo[k].part.persistentId))
			{
				partInfoDictionary.Remove(vesselDeltaV.PartInfo[k].part.persistentId);
				parts.Remove(vesselDeltaV.PartInfo[k]);
				flag = true;
			}
		}
		ProcessActiveEngines();
		if (parts.Count == 0)
		{
			separationIndex = vesselDeltaV.GetHighestSeparationStage(inStage);
		}
		if (flag)
		{
			ResetPartCaches();
		}
		PartsActivateInStage(out cachedActivatedParts);
		bool flag3 = true;
		for (int l = 0; l < cachedActivatedParts.Count; l++)
		{
			if (!cachedActivatedParts[l].part.isLaunchClamp())
			{
				flag3 = false;
				break;
			}
		}
		stageContainsOnlyLaunchClamps = flag3;
	}

	public void CalculateStartMass()
	{
		float decoupledDryMass = 0f;
		float decoupledFuelMass = 0f;
		float jettisonedDryMass = 0f;
		int decoupledPartCount = 0;
		stageMass = GetStageMass(out dryMass, out fuelMass, out decoupledDryMass, out decoupledFuelMass, out jettisonedDryMass, out decoupledPartCount);
		decoupledMass = decoupledDryMass + decoupledFuelMass + jettisonedDryMass;
		dryMass = dryMass - decoupledDryMass - jettisonedDryMass;
		startMass = stageMass - decoupledMass;
		StoreStartFuelMass();
	}

	public void StoreInterimDeltaV(List<DeltaVEngineInfo> engines, List<DeltaVCalc> deltaVCalcsList, float startingMass, float currentMass, double simulationTime, bool logMsgs)
	{
		enginesThrustVac = Vector3.zero;
		enginesThrustASL = Vector3.zero;
		enginesThrustActual = Vector3.zero;
		for (int i = 0; i < engines.Count; i++)
		{
			engines[i].CalculateThrustVector(stage == StageManager.CurrentStage);
			enginesThrustVac += engines[i].thrustVectorVac;
			enginesThrustASL += engines[i].thrustVectorASL;
			enginesThrustActual += engines[i].thrustVectorActual;
		}
		Vector3 vector = enginesThrustVac * (float)(Math.Log(startingMass / currentMass) * ispVac * PhysicsGlobals.GravitationalAcceleration / (double)thrustVac);
		Vector3 vector2 = enginesThrustASL * (float)(Math.Log(startingMass / currentMass) * ispASL * PhysicsGlobals.GravitationalAcceleration / (double)thrustASL);
		Vector3 vector3 = enginesThrustActual * (float)(Math.Log(startingMass / currentMass) * ispActual * PhysicsGlobals.GravitationalAcceleration / (double)thrustActual);
		CalculateTWR(startingMass);
		deltaVCalcsList.Add(new DeltaVCalc(vector.magnitude, vector2.magnitude, vector3.magnitude, simulationTime, engines, ispVac, ispASL, ispActual, TWRVac, TWRASL, TWRActual, startingMass, currentMass, thrustVac, thrustASL, thrustActual));
		if (logMsgs)
		{
			Debug.LogFormat("[StageInfo]: Interim DeltaV Calc. Stage:{0} ThrustVector:{1} Simulation Time:{2:N3} Start Mass:{3} End Mass:{4} ISP:{5} Thrust:{6} ASL dV:{7} VAC dV:{8} Actual dV:{9}", stage, enginesThrustVac, simulationTime, startingMass, currentMass, ispVac, thrustVac, vector2.magnitude, vector.magnitude, vector3.magnitude);
		}
	}

	public int GetHighestSeparationIndex(List<DeltaVEngineInfo> engines)
	{
		int num = int.MinValue;
		for (int i = 0; i < engines.Count; i++)
		{
			num = Math.Max(num, engines[i].partInfo.decoupleStage);
		}
		return num;
	}

	public List<DeltaVEngineInfo> MatchingSeparationIndex(List<DeltaVEngineInfo> engines, int index)
	{
		List<DeltaVEngineInfo> list = new List<DeltaVEngineInfo>();
		for (int i = 0; i < engines.Count; i++)
		{
			if (engines[i].partInfo.decoupleStage == index)
			{
				list.Add(engines[i]);
			}
		}
		if (list.Count == 0)
		{
			return engines;
		}
		return list;
	}

	public List<DeltaVEngineResourcePartInfo> GetEngineResourceParts(List<DeltaVEngineInfo> engines)
	{
		List<DeltaVEngineResourcePartInfo> list = new List<DeltaVEngineResourcePartInfo>();
		for (int i = 0; i < engines.Count; i++)
		{
			HashSet<Part>.Enumerator enumerator = engines[i].partInfo.part.simulationCrossfeedPartSet.GetParts().GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!partInfoDictionary.TryGetValue(enumerator.Current.persistentId, out var value))
				{
					continue;
				}
				if (value.decoupleStage == stage)
				{
					for (int j = 0; j < value.part.SimulationResources.Count; j++)
					{
						value.part.SimulationResources[j].amount = 0.0;
					}
					continue;
				}
				for (int k = 0; k < engines[i].propellantInfo.Count; k++)
				{
					if (value.part.Resources.Contains(engines[i].propellantInfo[k].propellant.id))
					{
						DeltaVEngineResourcePartInfo deltaVEngineResourcePartInfo = list.Get(value);
						if (deltaVEngineResourcePartInfo == null)
						{
							deltaVEngineResourcePartInfo = new DeltaVEngineResourcePartInfo(value);
							list.Add(deltaVEngineResourcePartInfo);
						}
						deltaVEngineResourcePartInfo.resourceIdsUsed.AddUnique(engines[i].propellantInfo[k].propellant.id);
					}
				}
			}
			enumerator.Dispose();
		}
		return list;
	}

	public void CalculateEngineResourceFuelMass(List<DeltaVEngineResourcePartInfo> resourceParts)
	{
		for (int i = 0; i < resourceParts.Count; i++)
		{
			DeltaVEngineResourcePartInfo deltaVEngineResourcePartInfo = resourceParts[i];
			for (int j = 0; j < deltaVEngineResourcePartInfo.resourceIdsUsed.Count; j++)
			{
				PartResource partResource = deltaVEngineResourcePartInfo.resourcePart.part.SimulationResources.Get(deltaVEngineResourcePartInfo.resourceIdsUsed[j]);
				if (partResource != null)
				{
					deltaVEngineResourcePartInfo.fuelMassUsed += partResource.info.density * (float)partResource.amount;
				}
			}
		}
	}

	public bool EnginesDeprived(List<DeltaVEngineInfo> enginesToStageNext, bool allEngines)
	{
		int num = 0;
		if (enginesToStageNext.Count == 0)
		{
			return true;
		}
		int num2 = 0;
		while (true)
		{
			if (num2 < enginesToStageNext.Count)
			{
				if (enginesToStageNext[num2].deprived)
				{
					num++;
					if (!allEngines)
					{
						break;
					}
				}
				num2++;
				continue;
			}
			if (allEngines && num == enginesToStageNext.Count)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public void StoreStartFuelMass()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].StageStartFuelMass(stage);
		}
	}

	public void StoreEndFuelMass()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].StageEndFuelMass(stage);
		}
	}

	public float CheckTimeStep(float timeStep, float simulationBurnTime, float fuelMassRemoved, List<DeltaVEngineInfo> enginesStillActive, List<DeltaVEngineResourcePartInfo> resourcePartsToStageNext, bool logMsgs)
	{
		float num = 0f;
		float num2 = 0f;
		if (resourcePartsToStageNext.Count == 0)
		{
			return timeStep;
		}
		for (int i = 0; i < resourcePartsToStageNext.Count; i++)
		{
			num += resourcePartsToStageNext[i].fuelMassUsed;
		}
		for (int j = 0; j < enginesStillActive.Count; j++)
		{
			bool flag = false;
			for (int k = 0; k < resourcePartsToStageNext.Count; k++)
			{
				if (enginesStillActive[j].partInfo.part.simulationCrossfeedPartSet.ContainsPart(resourcePartsToStageNext[k].resourcePart.part))
				{
					flag = true;
					break;
				}
			}
			if (!(enginesStillActive[j].maxTimeStep <= 0f) && flag)
			{
				float throttle = enginesStillActive[j].engine.thrustPercentage * 0.01f;
				throttle = enginesStillActive[j].engine.ApplyThrottleAdjustments(throttle);
				num2 += (float)((double)enginesStillActive[j].thrustActual / enginesStillActive[j].ispActual / PhysicsGlobals.GravitationalAcceleration * (double)throttle);
			}
		}
		float num3 = (fuelMassRemoved + num) / num2;
		float num4 = num3 - simulationBurnTime;
		if (logMsgs)
		{
			Debug.LogFormat("[StageInfo]: CheckTimeStep Fuel Mass To burn:{0} Engines Fuel Flow:{1} TimeStep:{2} Simulation Time:{3} Burn Time:{4} Remaining Time:{5}", num, num2, timeStep, simulationBurnTime, num3, num4);
		}
		if (num4 >= timeStep)
		{
			return timeStep;
		}
		if (logMsgs)
		{
			Debug.LogFormat("[StageInfo]: Minimum Timestep changed from {0} to {1}", timeStep, num4);
		}
		return num4;
	}

	public void SimulateDeltaV(bool runningActive, bool infiniteFuel, float timeStep = 0.2f, bool logMsgs = true, bool thisStageActive = false)
	{
		if (logMsgs)
		{
			Debug.LogFormat("[StageInfo]: DeltaV Simulation starting stage {0} Running Active:{1}", stage, runningActive);
		}
		CalculateStartMass();
		if (enginesActiveInStage != null && enginesActiveInStage.Count != 0 && (thisStageActive || !stageContainsOnlyLaunchClamps))
		{
			deltaVCalcs.Clear();
			if (cachedResourcePartSetParts == null)
			{
				cachedResourcePartSetParts = parts.PartsInStage(stage);
				cachedFlowGraph = new SCCFlowGraph(cachedResourcePartSetParts);
				PartSet.BuildPartSimulationSets(cachedResourcePartSetParts, cachedFlowGraph);
			}
			if (HighLogic.LoadedSceneIsEditor && vesselDeltaV != null && vesselDeltaV.Ship != null)
			{
				vesselDeltaV.Ship.UpdateResourceSets(cachedResourcePartSetParts, cachedFlowGraph);
			}
			int count = enginesActiveInStage.Count;
			while (count-- > 0)
			{
				if (!enginesActiveInStage[count].deprived && !enginesActiveInStage[count].PropellantStarved())
				{
					if (enginesActiveInStage[count].partInfo.decoupleBeforeBurn)
					{
						if (logMsgs)
						{
							Debug.LogFormat("[StageInfo]: Engine {0}  {1} Removed from Stage {2} as it decouples before activating.", enginesActiveInStage[count].engine.part.persistentId, enginesActiveInStage[count].engine.part.partInfo.title, stage);
						}
						enginesActiveInStage.RemoveAt(count);
					}
					else if (HighLogic.LoadedSceneIsFlight && StageManager.CurrentStage == stage && enginesActiveInStage[count].RequiresAir() && enginesActiveInStage[count].engine.part.staticPressureAtm <= 0.0)
					{
						if (logMsgs)
						{
							Debug.LogFormat("[StageInfo]: Engine {0}  {1} Removed from Stage {2} as it is deprived of Air in a Vacuum.", enginesActiveInStage[count].engine.part.persistentId, enginesActiveInStage[count].engine.part.partInfo.title, stage);
						}
						enginesActiveInStage.RemoveAt(count);
					}
				}
				else
				{
					if (logMsgs)
					{
						Debug.LogFormat("[StageInfo]: Engine {0}  {1} Removed from Stage {2} as it is already deprived.", enginesActiveInStage[count].engine.part.persistentId, enginesActiveInStage[count].engine.part.partInfo.title, stage);
					}
					enginesActiveInStage.RemoveAt(count);
				}
			}
			if (payloadStage && enginesActiveInStage.Count > 0)
			{
				payloadStage = false;
				if (logMsgs)
				{
					Debug.LogFormat("[StageInfo]: Reset PayLoad Stage as there are still engines active in the stage that are not deprived. Stage {0}", stage);
				}
			}
			int highestSeparationIndex = GetHighestSeparationIndex(enginesActiveInStage);
			List<DeltaVEngineInfo> list = MatchingSeparationIndex(enginesActiveInStage, highestSeparationIndex);
			if (logMsgs)
			{
				Debug.LogFormat("[StageInfo]: Resource Tanks:");
			}
			List<DeltaVEngineResourcePartInfo> engineResourceParts = GetEngineResourceParts(list);
			CalculateEngineResourceFuelMass(engineResourceParts);
			int highestPartSeparationIndex = engineResourceParts.GetHighestPartSeparationIndex(stage);
			List<DeltaVEngineResourcePartInfo> list2 = engineResourceParts.PartsMatchingSeparationIndex(highestPartSeparationIndex);
			if (logMsgs)
			{
				for (int i = 0; i < engineResourceParts.Count; i++)
				{
					Debug.LogFormat("[StageInfo]: Resource Parts in Stage {0} - {1}", engineResourceParts[i].resourcePart.part.persistentId, engineResourceParts[i].resourcePart.part.partInfo.title);
				}
				for (int j = 0; j < list2.Count; j++)
				{
					Debug.LogFormat("[StageInfo]: Resource Part to Stage Next {0} - {1}", list2[j].resourcePart.part.persistentId, list2[j].resourcePart.part.partInfo.title);
				}
			}
			if (logMsgs)
			{
				Debug.LogFormat("[StageInfo]: Engines Active in Stage:");
			}
			for (int k = 0; k < enginesActiveInStage.Count; k++)
			{
				enginesActiveInStage[k].ResetCalcVariables();
				enginesActiveInStage[k].CalculateFuelTime(stage);
				if (enginesActiveInStage[k].requiresAir)
				{
					timeStep = GameSettings.DELTAV_CALCULATIONS_BIGTIMESTEP;
					if (logMsgs)
					{
						Debug.Log("[StageInfo]: TimeStep changed as Engines requiring Air in stage");
					}
				}
				if (logMsgs)
				{
					Debug.LogFormat("[StageInfo]: Engine {0} - {1} Is In StageNext List? {2}", enginesActiveInStage[k].engine.part.persistentId, enginesActiveInStage[k].engine.part.partInfo.title, list.Contains(enginesActiveInStage[k]) ? "Yes" : "No");
				}
			}
			List<DeltaVEngineInfo> list3 = new List<DeltaVEngineInfo>(enginesActiveInStage);
			CalculateISP();
			CalculateTWR();
			float startingMass = startMass;
			float num = 0f;
			removedFuelMass = 0f;
			List<DeltaVEngineInfo> list4 = new List<DeltaVEngineInfo>();
			do
			{
				bool flag = false;
				bool flag2 = false;
				float num2 = timeStep;
				for (int l = 0; l < list3.Count; l++)
				{
					if (!list3[l].deprived)
					{
						bool checkDeprived = false;
						float minTimeStep = timeStep;
						list3[l].CalculateBurn(this, list3, timeStep, runningActive, logMsgs, infiniteFuel, reCalc: false, out checkDeprived, out minTimeStep);
						if (flag = flag || checkDeprived)
						{
							num2 = Mathf.Min(num2, minTimeStep);
						}
					}
				}
				if (list3.Count > 0 || list2.Count > 1)
				{
					float num3 = CheckTimeStep(timeStep, num, removedFuelMass, list3, list2, logMsgs);
					if (num3 < num2)
					{
						flag = true;
						num2 = num3;
					}
					if (num2 <= 0f)
					{
						for (int m = 0; m < list.Count; m++)
						{
							list[m].deprived = true;
							list4.Add(list[m]);
						}
						if (logMsgs)
						{
							Debug.Log("[StageInfo]: TimeStep Zero. Engines deprived.");
						}
					}
				}
				if (flag && list3.Count > 0)
				{
					for (int n = 0; n < list3.Count; n++)
					{
						if (!list3[n].deprived)
						{
							bool checkDeprived2 = false;
							float minTimeStep2 = num2;
							list3[n].CalculateBurn(this, list3, num2, runningActive, logMsgs, infiniteFuel, reCalc: true, out checkDeprived2, out minTimeStep2);
							if (logMsgs)
							{
								Debug.LogFormat("[StageInfo]: Recalc Minimum Time Step {0} For Engine {1} - {2}", num2, list3[n].engine.part.partInfo.title, list3[n].engine.part.persistentId);
							}
						}
					}
					if (logMsgs)
					{
						Debug.Log("[StageInfo]: Recalc Minimum Time Step For Engines Completed");
					}
				}
				if (!EnginesDeprived(list, allEngines: true))
				{
					int count2 = list3.Count;
					while (count2-- > 0)
					{
						if (list3[count2].deprived)
						{
							if (logMsgs)
							{
								Debug.LogFormat("[StageInfo]: Removed Deprived Engine {0} - {1} from list. TimeStep for engine was {2}", list3[count2].engine.part.partInfo.title, list3[count2].engine.part.persistentId, list3[count2].maxTimeStep);
							}
							list4.Add(list3[count2]);
							continue;
						}
						list3[count2].ApplyBurn(this, num2, runningActive, logMsgs, infiniteFuel);
						if (list3[count2].deprived)
						{
							if (logMsgs)
							{
								Debug.LogFormat("[StageInfo]: Removed Deprived Engine {0} - {1} from list. TimeStep for engine was {2}", list3[count2].engine.part.partInfo.title, list3[count2].engine.part.persistentId, list3[count2].maxTimeStep);
							}
							list4.Add(list3[count2]);
						}
					}
				}
				if (list2.Count > 0)
				{
					int count3 = list2.Count;
					while (count3-- > 0)
					{
						bool flag3 = false;
						for (int num4 = 0; num4 < list2[count3].resourcePart.part.SimulationResources.Count; num4++)
						{
							flag3 = ((list2[count3].resourceIdsUsed.Contains(list2[count3].resourcePart.part.SimulationResources[num4].info.id) && list2[count3].resourcePart.part.SimulationResources[num4].amount <= 0.0) ? true : false);
						}
						if (flag3)
						{
							if (logMsgs)
							{
								Debug.LogFormat("[StageInfo]: Removed Empty Tank {0} - {1} from list.", list2[count3].resourcePart.part.partInfo.title, list2[count3].resourcePart.part.persistentId);
							}
							removedFuelMass += list2[count3].fuelMassUsed;
							list2.RemoveAt(count3);
						}
					}
					if (list2.Count == 0)
					{
						flag2 = true;
					}
				}
				if (list4.Count > 0 || num2 < timeStep || flag2)
				{
					CalculateISP(list3);
					float currentStageMass = GetCurrentStageMass();
					StoreInterimDeltaV(list3, deltaVCalcs, startingMass, currentStageMass, num + num2, logMsgs);
					startingMass = currentStageMass;
					for (int num5 = 0; num5 < list4.Count; num5++)
					{
						list3.Remove(list4[num5]);
					}
					list4.Clear();
				}
				if (EnginesDeprived(list, allEngines: true) || flag2)
				{
					list3.Clear();
					if (logMsgs)
					{
						Debug.LogFormat("[StageInfo]: DeltaV Simulation All engines to stage next are deprived. Stage {0} Last Timestep = {1}", stage, num2);
					}
				}
				num += num2;
				if (num > 1000f && timeStep < GameSettings.DELTAV_CALCULATIONS_BIGTIMESTEP)
				{
					if (logMsgs)
					{
						Debug.Log("[StageInfo]: Simulation Running too slow. Time Step increased");
					}
					timeStep = GameSettings.DELTAV_CALCULATIONS_BIGTIMESTEP;
				}
			}
			while (list3.Count != 0 && num < 100000f);
			if (logMsgs && num >= 100000f)
			{
				Debug.LogWarning("[StageInfo]: Simulation Time exceeded!");
			}
			for (int num6 = 0; num6 < list.Count; num6++)
			{
				if (list[num6].partInfo.decoupleStage < stage)
				{
					list[num6].deprived = false;
				}
			}
			stageBurnTime = num;
			endMass = GetCurrentStageMass();
			StoreEndFuelMass();
			CalculateISP();
			CalculateTWR();
			deltaVinVac = 0f;
			deltaVatASL = 0f;
			deltaVActual = 0f;
			for (int num7 = 0; num7 < deltaVCalcs.Count; num7++)
			{
				if (!double.IsNaN(deltaVCalcs[num7].dVinVac))
				{
					deltaVinVac += (float)deltaVCalcs[num7].dVinVac;
				}
				if (!double.IsNaN(deltaVCalcs[num7].dVatASL))
				{
					deltaVatASL += (float)deltaVCalcs[num7].dVatASL;
				}
				if (!double.IsNaN(deltaVCalcs[num7].dVActual))
				{
					deltaVActual += (float)deltaVCalcs[num7].dVActual;
				}
			}
			float num8 = 0f;
			totalExhaustVelocityActual = 0f;
			float num9 = num8;
			num8 = 0f;
			totalExhaustVelocityASL = num9;
			totalExhaustVelocityVAC = num8;
			vectoredExhaustVelocityVAC = (vectoredExhaustVelocityASL = (vectoredExhaustVelocityActual = Vector3.zero));
			num8 = 0f;
			vectoredThrustActual = 0f;
			float num10 = num8;
			num8 = 0f;
			vectoredThrustASL = num10;
			vectoredThrustVac = num8;
			enginesThrustVac = (enginesThrustASL = (enginesThrustActual = Vector3.zero));
			for (int num11 = 0; num11 < enginesActiveInStage.Count; num11++)
			{
				if (enginesActiveInStage[num11].thrustVac > 0f)
				{
					totalExhaustVelocityVAC += (enginesActiveInStage[num11].thrustVectorVac * ((float)enginesActiveInStage[num11].ispVac * (float)PhysicsGlobals.GravitationalAcceleration) / enginesActiveInStage[num11].thrustVac).magnitude;
					vectoredExhaustVelocityVAC += enginesActiveInStage[num11].thrustVectorVac * ((float)enginesActiveInStage[num11].ispVac * (float)PhysicsGlobals.GravitationalAcceleration) / enginesActiveInStage[num11].thrustVac;
				}
				if (enginesActiveInStage[num11].thrustASL > 0f)
				{
					vectoredExhaustVelocityASL += enginesActiveInStage[num11].thrustVectorASL * ((float)enginesActiveInStage[num11].ispASL * (float)PhysicsGlobals.GravitationalAcceleration) / enginesActiveInStage[num11].thrustASL;
				}
				if (enginesActiveInStage[num11].thrustActual > 0f)
				{
					vectoredExhaustVelocityActual += enginesActiveInStage[num11].thrustVectorActual * ((float)enginesActiveInStage[num11].ispActual * (float)PhysicsGlobals.GravitationalAcceleration) / enginesActiveInStage[num11].thrustActual;
				}
				enginesThrustVac += enginesActiveInStage[num11].thrustVectorVac;
				enginesThrustASL += enginesActiveInStage[num11].thrustVectorASL;
				enginesThrustActual += enginesActiveInStage[num11].thrustVectorActual;
			}
			vectoredThrustVac = enginesThrustVac.magnitude;
			vectoredThrustASL = enginesThrustASL.magnitude;
			vectoredThrustActual = enginesThrustActual.magnitude;
			if (logMsgs)
			{
				Debug.LogFormat("[StageInfo]: Final DeltaV Calc. Stage:{0} Start Mass:{1} End Mass:{2} ASL ISP:{3} VAC ISP:{4} Actual ISP:{5} ASL dV:{6} VAC dV:{7} Actual dV:{8} BurnTime:{9}", stage, startMass, endMass, ispASL, ispVac, ispActual, deltaVatASL, deltaVinVac, deltaVActual, stageBurnTime);
				Debug.LogFormat("[StageInfo]: DeltaV Simulation ended stage {0}", stage);
			}
			return;
		}
		endMass = startMass;
		StoreEndFuelMass();
		if (logMsgs)
		{
			if (enginesActiveInStage == null || enginesActiveInStage.Count == 0)
			{
				Debug.LogFormat("[StageInfo]: No Active Engines in Stage. DeltaV Simulation skipped for stage {0}", stage);
			}
			if (!thisStageActive && stageContainsOnlyLaunchClamps)
			{
				Debug.LogFormat("[StageInfo]: Stage Not active && Stage Contains Only LaunchClamps for stage {0}", stage);
			}
		}
	}

	public double CalculateTimeRequiredDV(bool runningActive, float deltaVRequested)
	{
		double num = 0.0;
		float num2 = (runningActive ? vectoredThrustActual : ((!(FlightGlobals.ActiveVessel != null)) ? vectoredThrustVac : ((FlightGlobals.ActiveVessel.atmDensity > 0.0) ? vectoredThrustASL : vectoredThrustVac)));
		float num3 = (runningActive ? ((float)ispActual) : ((!(FlightGlobals.ActiveVessel != null)) ? ((float)ispVac) : ((FlightGlobals.ActiveVessel.atmDensity > 0.0) ? ((float)ispASL) : ((float)ispVac))));
		float num4 = (runningActive ? vectoredExhaustVelocityActual.magnitude : ((!(FlightGlobals.ActiveVessel != null)) ? vectoredExhaustVelocityVAC.magnitude : ((FlightGlobals.ActiveVessel.atmDensity > 0.0) ? vectoredExhaustVelocityASL.magnitude : vectoredExhaustVelocityVAC.magnitude)));
		float num5 = (runningActive ? totalExhaustVelocityActual : ((!(FlightGlobals.ActiveVessel != null)) ? totalExhaustVelocityVAC : ((FlightGlobals.ActiveVessel.atmDensity > 0.0) ? totalExhaustVelocityASL : totalExhaustVelocityVAC)));
		float num6 = 1f;
		if (FlightGlobals.ActiveVessel != null && num5 > 0f)
		{
			num6 = num4 / num5;
			Mathf.Clamp(num6, -1f, 1f);
		}
		float num7 = startMass / Mathf.Exp(deltaVRequested / (float)PhysicsGlobals.GravitationalAcceleration / num3 / num6);
		float num8 = num2 / startMass;
		float num9 = num2 / num7;
		num = deltaVRequested / Mathf.Sqrt(num8 * num9);
		if (GameSettings.LOG_DELTAV_VERBOSE)
		{
			Debug.LogFormat("[DeltaVStageInfo]: Calculated Time for Required DV. Stage {0} DeltaV Requested:{1} Time Required:{2}", stage, deltaVRequested, num);
		}
		return num;
	}

	public void CalcLerpDeltaV()
	{
		for (int i = 0; i < enginesActiveInStage.Count; i++)
		{
			enginesActiveInStage[i].CalcThrustActual();
			enginesActiveInStage[i].CalculateISP();
		}
		CalculateISP();
		CalculateTWR();
		if (vesselDeltaV.Vessel != null)
		{
			float num = (float)vesselDeltaV.Vessel.rootPart.staticPressureAtm;
			if (num > 0f)
			{
				num /= (float)vesselDeltaV.Vessel.mainBody.atmPressureASL;
			}
			deltaVActual = Mathf.Lerp(deltaVinVac, deltaVatASL, num);
		}
	}

	public string GetPartDisplayInfo()
	{
		if (!partsDisplayListDirty && partsDisplayList != null)
		{
			return partsDisplayList.ToString();
		}
		if (partsDisplayList == null)
		{
			partsDisplayList = StringBuilderCache.Acquire();
		}
		else
		{
			partsDisplayList.Release();
			partsDisplayList = StringBuilderCache.Acquire();
		}
		if (parts != null && (parts == null || parts.Count != 0))
		{
			for (int i = 0; i < parts.Count; i++)
			{
				if (parts[i].decoupleStage == stage)
				{
					partsDisplayList.Append("<color=red>");
				}
				else
				{
					partsDisplayList.Append("<color=green>");
				}
				float stageStartMass = parts[i].GetStageStartMass(stage);
				partsDisplayList.Append(Localizer.Format("#autoLOC_8002202", parts[i].part.partInfo.title, parts[i].dryMass.ToString("N3"), stageStartMass.ToString("N3")) + "</color>");
				if (parts[i].JettisonInStage(stage))
				{
					partsDisplayList.Append("<color=yellow>  " + Localizer.Format("#autoLOC_8002203", parts[i].jettisonMass.ToString("N3")) + "</color>");
				}
				if (i < parts.Count - 1)
				{
					partsDisplayList.Append("\n");
				}
			}
			partsDisplayListDirty = false;
			return partsDisplayList.ToString();
		}
		partsDisplayList.Append(Localizer.Format("#autoLOC_6003083"));
		partsDisplayListDirty = false;
		return partsDisplayList.ToString();
	}

	public void ProcessActiveEngines()
	{
		enginesActiveInStage.Clear();
		enginesInStage.Clear();
		List<DeltaVEngineInfo> workingEngineInfo = vesselDeltaV.WorkingEngineInfo;
		for (int i = 0; i < workingEngineInfo.Count; i++)
		{
			if (workingEngineInfo[i].partInfo == null)
			{
				vesselDeltaV.SetCalcsDirty(resetPartCaches: true);
			}
			else if (stage <= workingEngineInfo[i].startBurnStage)
			{
				enginesInStage.Add(workingEngineInfo[i]);
				if (partInfoDictionary.ContainsKey(workingEngineInfo[i].engine.part.persistentId) && workingEngineInfo[i].partInfo.decoupleStage < stage && (!HighLogic.LoadedSceneIsFlight || StageManager.CurrentStage != stage || workingEngineInfo[i].engine.EngineIgnited))
				{
					enginesActiveInStage.Add(workingEngineInfo[i]);
				}
			}
		}
	}

	public void ResetPartCaches()
	{
		cachedResourcePartSetParts = null;
		cachedFlowGraph = null;
	}

	public float GetStageMass(out float dryMass, out float fuelMass, out float decoupledDryMass, out float decoupledFuelMass, out float jettisonedDryMass, out int decoupledPartCount)
	{
		dryMass = 0f;
		fuelMass = 0f;
		decoupledDryMass = 0f;
		decoupledFuelMass = 0f;
		jettisonedDryMass = 0f;
		decoupledPartCount = 0;
		int count = parts.Count;
		while (count-- > 0)
		{
			DeltaVPartInfo deltaVPartInfo = parts[count];
			dryMass += deltaVPartInfo.dryMass;
			fuelMass += deltaVPartInfo.GetCurrentFuelMass();
			if (deltaVPartInfo.decoupleStage == stage)
			{
				decoupledPartCount++;
				decoupledDryMass += deltaVPartInfo.dryMass;
				decoupledFuelMass += deltaVPartInfo.GetCurrentFuelMass();
			}
			if (deltaVPartInfo.JettisonInStage(stage))
			{
				jettisonedDryMass += deltaVPartInfo.jettisonMass;
			}
		}
		return dryMass + fuelMass;
	}

	public float GetCurrentStageMass()
	{
		float num = startMass;
		float num2 = 0f;
		for (int i = 0; i < enginesActiveInStage.Count; i++)
		{
			num2 += enginesActiveInStage[i].PropellantMassBurnt();
		}
		num -= num2;
		if (GameSettings.LOG_DELTAV_VERBOSE)
		{
			Debug.LogFormat("[StageInfo]: End of Stage {0} Mass. Start Mass: {1} Fuel Mass Lost = {2} Final Mass {3}", stage, startMass, num2, num);
		}
		return num;
	}

	public bool ContainsAnchoredDecoupler()
	{
		int num = 0;
		while (true)
		{
			if (num < parts.Count)
			{
				if (parts[num].moduleAnchoredDecoupler != null)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool ContainsDecoupler()
	{
		int num = 0;
		while (true)
		{
			if (num < parts.Count)
			{
				if (parts[num].isDecoupler)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public Ray CoTForStage(bool activeOnly = false)
	{
		float num = 0f;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		CenterOfThrustQuery centerOfThrustQuery = new CenterOfThrustQuery();
		int count = enginesActiveInStage.Count;
		while (count-- > 0)
		{
			if (!activeOnly || enginesActiveInStage[count].engine.EngineIgnited)
			{
				centerOfThrustQuery.Reset();
				enginesActiveInStage[count].engine.OnCenterOfThrustQuery(centerOfThrustQuery);
				zero += centerOfThrustQuery.pos * centerOfThrustQuery.thrust;
				zero2 += centerOfThrustQuery.dir * centerOfThrustQuery.thrust;
				num += centerOfThrustQuery.thrust;
			}
		}
		if (num != 0f)
		{
			float num2 = 1f / num;
			zero *= num2;
			zero2 *= num2;
			return new Ray(zero, zero2);
		}
		return new Ray(Vector3.zero, Vector3.zero);
	}

	public void CalculateISP(List<DeltaVEngineInfo> engines = null)
	{
		bool flag = true;
		ispVac = 0.0;
		ispASL = 0.0;
		ispActual = 0.0;
		thrustVac = 0f;
		thrustASL = 0f;
		thrustActual = 0f;
		if (engines == null)
		{
			engines = enginesActiveInStage;
		}
		for (int i = 0; i < engines.Count; i++)
		{
			DeltaVEngineInfo deltaVEngineInfo = engines[i];
			deltaVEngineInfo.CalculateThrustVector(stage == StageManager.CurrentStage);
			thrustVac += deltaVEngineInfo.thrustVac;
			thrustASL += deltaVEngineInfo.thrustASL;
			thrustActual += deltaVEngineInfo.thrustActual;
			deltaVEngineInfo.CalculateISP(stage == StageManager.CurrentStage);
			if (deltaVEngineInfo.ispVac != ispVac)
			{
				if (ispVac == 0.0)
				{
					ispVac = deltaVEngineInfo.ispVac;
					ispASL = deltaVEngineInfo.ispASL;
					ispActual = deltaVEngineInfo.ispActual;
				}
				else
				{
					flag = false;
				}
			}
		}
		if (!flag)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			for (int j = 0; j < engines.Count; j++)
			{
				DeltaVEngineInfo deltaVEngineInfo2 = engines[j];
				num += (double)deltaVEngineInfo2.thrustVac / deltaVEngineInfo2.ispVac;
				num2 += (double)deltaVEngineInfo2.thrustASL / deltaVEngineInfo2.ispASL;
				num3 += (double)deltaVEngineInfo2.thrustActual / deltaVEngineInfo2.ispActual;
			}
			ispVac = (double)thrustVac / num;
			ispASL = (double)thrustASL / num2;
			ispActual = (double)thrustActual / num3;
		}
	}

	public void CalculateTWR(float mass = -1f)
	{
		float num = (float)FlightGlobals.GetHomeBody().GeeASL;
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (vesselDeltaV != null && vesselDeltaV.Vessel != null && vesselDeltaV.Vessel.mainBody != null && vesselDeltaV.Vessel.mainBody != null)
			{
				num = (float)vesselDeltaV.Vessel.mainBody.GeeASL;
			}
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			DeltaVAppValues deltaVAppValues = DeltaVGlobals.DeltaVAppValues;
			if (deltaVAppValues != null && deltaVAppValues.body != null)
			{
				num = (float)deltaVAppValues.body.GeeASL;
			}
		}
		TWRVac = thrustVac / (startMass * ((float)PhysicsGlobals.GravitationalAcceleration * num));
		TWRASL = thrustASL / (startMass * ((float)PhysicsGlobals.GravitationalAcceleration * num));
		TWRActual = thrustActual / (((mass == -1f) ? startMass : mass) * ((float)PhysicsGlobals.GravitationalAcceleration * num));
	}

	public int PartsActiveInStage()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].decoupleStage != stage)
			{
				num++;
			}
		}
		return num;
	}

	public int PartsActivateInStage(out List<DeltaVPartInfo> activeParts)
	{
		int num = 0;
		activeParts = new List<DeltaVPartInfo>();
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].activationStage == stage)
			{
				num++;
				activeParts.Add(parts[i]);
			}
			else if (parts[i].isEngine && parts[i].activationStage > stage)
			{
				DeltaVEngineInfo deltaVEngineInfo = enginesActiveInStage.Get(parts[i].part);
				if (deltaVEngineInfo != null && !deltaVEngineInfo.deprived)
				{
					num++;
					activeParts.Add(parts[i]);
				}
			}
		}
		return num;
	}

	public int PartsDecoupledInStage()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].decoupleStage == stage)
			{
				num++;
			}
		}
		return num;
	}
}
