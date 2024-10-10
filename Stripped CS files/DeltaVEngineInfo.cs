using System;
using System.Collections.Generic;
using ns11;
using UnityEngine;

[Serializable]
public class DeltaVEngineInfo
{
	public VesselDeltaV vesselDeltaV;

	public DeltaVPartInfo _partInfo;

	public ModuleEngines engine;

	public MultiModeEngine multiModeEngine;

	public List<DeltaVPropellantInfo> propellantInfo;

	public double atmosphere;

	public double ispVac;

	public double ispASL;

	public double ispActual;

	public Vector3 thrustVectorVac;

	public Vector3 thrustVectorASL;

	public Vector3 thrustVectorActual;

	public float thrustVac;

	public float thrustASL;

	public float thrustActual;

	public int startBurnStage;

	public bool deprived;

	public float maxTimeStep;

	[NonSerialized]
	public DictionaryValueList<int, DeltaVEngineBurnTotals> stageBurnTotals;

	public bool requiresAir;

	public bool throttleLimited;

	public bool lastStep;

	public static double epsilon = 1E-05;

	public DeltaVPartInfo partInfo
	{
		get
		{
			if (_partInfo == null && vesselDeltaV != null && engine != null)
			{
				_partInfo = vesselDeltaV.PartInfo.Get(engine.part);
			}
			return _partInfo;
		}
	}

	public DeltaVEngineInfo(VesselDeltaV inVesselDeltaV, ModuleEngines inEngine, MultiModeEngine inMultiModeEngine = null)
	{
		vesselDeltaV = inVesselDeltaV;
		if (inMultiModeEngine != null)
		{
			multiModeEngine = inMultiModeEngine;
			if (multiModeEngine.runningPrimary)
			{
				engine = multiModeEngine.PrimaryEngine;
			}
			else
			{
				engine = multiModeEngine.SecondaryEngine;
			}
		}
		else
		{
			multiModeEngine = null;
			engine = inEngine;
		}
		startBurnStage = engine.part.inverseStage;
		stageBurnTotals = new DictionaryValueList<int, DeltaVEngineBurnTotals>();
		requiresAir = false;
		propellantInfo = new List<DeltaVPropellantInfo>();
		for (int i = 0; i < engine.propellants.Count; i++)
		{
			if (engine.propellants[i].name == "IntakeAir")
			{
				requiresAir = true;
			}
			DeltaVPropellantInfo item = new DeltaVPropellantInfo(engine.propellants[i]);
			propellantInfo.Add(item);
		}
		CalculateISP();
		CalculateThrustVector();
	}

	public double GetSituationISP(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(ispASL, ispActual, ispVac);
	}

	public float GetSituationThrust(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(thrustASL, thrustActual, thrustVac);
	}

	public Vector3 GetSituationThrustVector(DeltaVSituationOptions situation)
	{
		return situation.GetSwitchedValue(thrustVectorASL, thrustVectorActual, thrustVectorVac);
	}

	public void Reset()
	{
		startBurnStage = engine.part.inverseStage;
		stageBurnTotals.Clear();
		if ((bool)multiModeEngine)
		{
			if (multiModeEngine.runningPrimary)
			{
				engine = multiModeEngine.PrimaryEngine;
			}
			else
			{
				engine = multiModeEngine.SecondaryEngine;
			}
		}
		ispActual = 0.0;
		ispASL = 0.0;
		ispVac = 0.0;
		thrustASL = 0f;
		thrustActual = 0f;
		thrustVac = 0f;
		_partInfo = null;
		CalculateISP();
		CalculateThrustVector();
		deprived = false;
	}

	public bool PropellantStarved()
	{
		bool result = false;
		int i = 0;
		for (int count = propellantInfo.Count; i < count; i++)
		{
			double maxAmount = 0.0;
			DeltaVPropellantInfo deltaVPropellantInfo = propellantInfo[i];
			if (!DeltaVGlobals.PropellantsToIgnore.Contains(deltaVPropellantInfo.propellant.id))
			{
				engine.part.GetConnectedResourceTotals(deltaVPropellantInfo.propellant.id, deltaVPropellantInfo.propellant.GetFlowMode(), simulate: true, out deltaVPropellantInfo.amountAvailable, out maxAmount);
				if (deltaVPropellantInfo.amountAvailable <= 0.0)
				{
					result = true;
				}
			}
		}
		return result;
	}

	public void CalculateBurn(DeltaVStageInfo deltaVStage, List<DeltaVEngineInfo> enginesStillActive, float deltaTime, bool runningActive, bool logMsgs, bool infiniteFuel, bool reCalc, out bool checkDeprived, out float minTimeStep)
	{
		maxTimeStep = (minTimeStep = deltaTime);
		if (!(engine == null) && !engine.IsEngineDead())
		{
			runningActive = runningActive && engine.EngineIgnited && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0f;
			checkDeprived = false;
			double maxAmount = 0.0;
			int num = 0;
			int count = propellantInfo.Count;
			DeltaVPropellantInfo deltaVPropellantInfo;
			while (true)
			{
				if (num < count)
				{
					deltaVPropellantInfo = propellantInfo[num];
					deltaVPropellantInfo.pendingDemand = 0.0;
					deltaVPropellantInfo.timeLeftSetThrottle = 0f;
					deltaVPropellantInfo.timeLeftCurrentThrottle = 0f;
					deltaVPropellantInfo.amountPerSecondMaxThrottle = engine.getMaxFuelFlow(deltaVPropellantInfo.propellant);
					float throttle = (engine.throttleLocked ? (engine.thrustPercentage * 0.01f) : (engine.independentThrottle ? (engine.independentThrottlePercentage * 0.01f * (engine.thrustPercentage * 0.01f)) : (engine.thrustPercentage * 0.01f)));
					throttle = engine.ApplyThrottleAdjustments(throttle);
					deltaVPropellantInfo.amountPerSecondSetThrottle = engine.getFuelFlow(deltaVPropellantInfo.propellant, engine.maxFuelFlow * throttle);
					deltaVPropellantInfo.amountPerSecondCurrentThrottle = engine.getFuelFlow(deltaVPropellantInfo.propellant, engine.requestedMassFlow);
					engine.part.GetConnectedResourceTotals(deltaVPropellantInfo.propellant.id, deltaVPropellantInfo.propellant.GetFlowMode(), simulate: true, out deltaVPropellantInfo.amountAvailable, out maxAmount);
					if (runningActive)
					{
						deltaVPropellantInfo.pendingDemand = deltaVPropellantInfo.amountPerSecondCurrentThrottle * (double)deltaTime;
					}
					else
					{
						deltaVPropellantInfo.pendingDemand = deltaVPropellantInfo.amountPerSecondSetThrottle * (double)deltaTime;
					}
					if (!(DeltaVGlobals.PropellantsToIgnore.Contains(deltaVPropellantInfo.propellant.id) || infiniteFuel))
					{
						deltaVPropellantInfo.timeLeftSetThrottle = (float)(deltaVPropellantInfo.amountAvailable / deltaVPropellantInfo.amountPerSecondSetThrottle);
						deltaVPropellantInfo.timeLeftCurrentThrottle = (float)(deltaVPropellantInfo.amountAvailable / deltaVPropellantInfo.amountPerSecondCurrentThrottle);
						float num2 = (float)Math.Round(deltaVPropellantInfo.amountAvailable, 5);
						float num3 = (float)Math.Round(deltaVPropellantInfo.pendingDemand, 5);
						if (logMsgs && reCalc)
						{
							Debug.LogFormat("[EngineInfo]: Engine {0} Prop {1} pending Demand {2} Available {3} time left {4}", engine.part.persistentId, deltaVPropellantInfo.propellant.name, deltaVPropellantInfo.pendingDemand, deltaVPropellantInfo.amountAvailable, runningActive ? deltaVPropellantInfo.timeLeftCurrentThrottle : deltaVPropellantInfo.timeLeftSetThrottle);
						}
						if (!(num2 > 0f) || !(deltaTime <= 0f))
						{
							if (!(num2 < num3) && (!(num2 <= 0f) || deltaTime > 0f))
							{
								if ((double)Math.Abs(num2 - num3) < epsilon)
								{
									break;
								}
							}
							else
							{
								lastStep = true;
								if (logMsgs)
								{
									Debug.LogFormat("[EngineInfo]: Engine {0} almost deprived. Propellant {1} Demand:{2} Available:{3:N5} Time Left:{4:N5}", engine.part.persistentId, deltaVPropellantInfo.propellant.name, deltaVPropellantInfo.pendingDemand, deltaVPropellantInfo.amountAvailable, runningActive ? deltaVPropellantInfo.timeLeftCurrentThrottle : deltaVPropellantInfo.timeLeftSetThrottle);
								}
								if (num2 <= 0f && deltaTime <= 0f)
								{
									deprived = true;
									if (logMsgs)
									{
										Debug.LogFormat("[EngineInfo]: Engine {0} deprived and StepTime is Zero. Propellant {1} Demand:{2} Available:{3:N5} Time Left:{4:N5}", engine.part.persistentId, deltaVPropellantInfo.propellant.name, deltaVPropellantInfo.pendingDemand, deltaVPropellantInfo.amountAvailable, runningActive ? deltaVPropellantInfo.timeLeftCurrentThrottle : deltaVPropellantInfo.timeLeftSetThrottle);
									}
								}
							}
						}
					}
					num++;
					continue;
				}
				if (lastStep)
				{
					maxTimeStep = deltaTime;
					int i = 0;
					for (int count2 = propellantInfo.Count; i < count2; i++)
					{
						DeltaVPropellantInfo deltaVPropellantInfo2 = propellantInfo[i];
						if (!DeltaVGlobals.PropellantsToIgnore.Contains(deltaVPropellantInfo2.propellant.id))
						{
							maxTimeStep = Mathf.Min(maxTimeStep, runningActive ? deltaVPropellantInfo2.timeLeftCurrentThrottle : deltaVPropellantInfo2.timeLeftSetThrottle);
						}
					}
					int j = 0;
					for (int count3 = propellantInfo.Count; j < count3; j++)
					{
						DeltaVPropellantInfo deltaVPropellantInfo3 = propellantInfo[j];
						if (!DeltaVGlobals.PropellantsToIgnore.Contains(deltaVPropellantInfo3.propellant.id) && enginesStillActive.Count <= 1)
						{
							deltaVPropellantInfo3.pendingDemand = (runningActive ? ((float)deltaVPropellantInfo3.amountPerSecondCurrentThrottle) : ((float)deltaVPropellantInfo3.amountPerSecondSetThrottle)) * maxTimeStep;
							if (logMsgs)
							{
								Debug.LogFormat("[EngineInfo]: Engine {0} almost deprived. Propellant {1} Updated Demand: {2}", engine.part.persistentId, deltaVPropellantInfo3.propellant.name, deltaVPropellantInfo3.pendingDemand);
							}
						}
					}
					if (logMsgs)
					{
						Debug.LogFormat("[EngineInfo]: Engine {0} TimeStep Set to: {1}", engine.part.persistentId, maxTimeStep);
					}
					minTimeStep = maxTimeStep;
				}
				checkDeprived = lastStep;
				return;
			}
			lastStep = true;
			if (logMsgs)
			{
				Debug.LogFormat("[EngineInfo]: Engine {0} almost deprived. Exactly Enough. Propellant {1} Demand:{2} Available:{3} Time Left:{4}", engine.part.persistentId, deltaVPropellantInfo.propellant.name, deltaVPropellantInfo.pendingDemand, deltaVPropellantInfo.amountAvailable, runningActive ? deltaVPropellantInfo.timeLeftCurrentThrottle : deltaVPropellantInfo.timeLeftSetThrottle);
			}
			checkDeprived = lastStep;
		}
		else
		{
			minTimeStep = 0f;
			checkDeprived = true;
			deprived = true;
		}
	}

	public void ApplyBurn(DeltaVStageInfo deltaVStage, float deltaTime, bool runningActive, bool logMsgs, bool infiniteFuel)
	{
		int num = 0;
		int count = propellantInfo.Count;
		DeltaVPropellantInfo deltaVPropellantInfo;
		double num2;
		while (true)
		{
			if (num < count)
			{
				deltaVPropellantInfo = propellantInfo[num];
				if (DeltaVGlobals.PropellantsToIgnore.Contains(deltaVPropellantInfo.propellant.id) || infiniteFuel)
				{
					deltaVPropellantInfo.amountBurnt = deltaVPropellantInfo.pendingDemand;
					deltaVPropellantInfo.pendingDemand = 0.0;
				}
				else
				{
					num2 = engine.part.RequestResource(deltaVPropellantInfo.propellant.id, deltaVPropellantInfo.pendingDemand, deltaVPropellantInfo.propellant.GetFlowMode(), simulate: true);
					deltaVPropellantInfo.amountBurnt += num2;
					if (num2 <= 0.0 && deltaTime <= 0f && !lastStep)
					{
						if (logMsgs)
						{
							Debug.LogFormat("[EngineInfo]: Engine {0} Timestep is zero. But engine is not deprived. Skipping this step", engine.part.persistentId);
						}
						deltaVPropellantInfo.pendingDemand = 0.0;
					}
					else
					{
						if (!(Math.Abs(num2 - deltaVPropellantInfo.pendingDemand) <= epsilon))
						{
							break;
						}
						deltaVPropellantInfo.pendingDemand = 0.0;
					}
				}
				num++;
				continue;
			}
			if (lastStep && !deprived)
			{
				if (logMsgs)
				{
					Debug.LogFormat("[EngineInfo]: Engine {0} last step completed. Set to Deprived.", engine.part.persistentId);
				}
				deprived = true;
			}
			return;
		}
		deprived = true;
		if (logMsgs)
		{
			Debug.LogFormat("[EngineInfo]: Engine {0} deprived of {1} {2} attempting to Apply Burn. Set to Deprived. Received {3}", engine.part.persistentId, deltaVPropellantInfo.pendingDemand, deltaVPropellantInfo.propellant.name, num2);
		}
		deltaVPropellantInfo.pendingDemand = 0.0;
	}

	public void ResetCalcVariables()
	{
		ResetResourceTotals();
		ResetStageInfo();
		ResetPropellantBurnValues();
	}

	public void SwitchEngine()
	{
		if (multiModeEngine.runningPrimary)
		{
			engine = multiModeEngine.PrimaryEngine;
		}
		else
		{
			engine = multiModeEngine.SecondaryEngine;
		}
		requiresAir = false;
		propellantInfo.Clear();
		for (int i = 0; i < engine.propellants.Count; i++)
		{
			if (engine.propellants[i].name == "IntakeAir")
			{
				requiresAir = true;
			}
			DeltaVPropellantInfo item = new DeltaVPropellantInfo(engine.propellants[i]);
			propellantInfo.Add(item);
		}
		CalculateISP();
		CalculateThrustVector();
	}

	public void CalculateFuelTime(int stage)
	{
		if (engine == null)
		{
			return;
		}
		DeltaVEngineBurnTotals deltaVEngineBurnTotals = null;
		if (stageBurnTotals.ContainsKey(stage))
		{
			deltaVEngineBurnTotals = stageBurnTotals[stage];
			deltaVEngineBurnTotals.propellantInfo.Clear();
		}
		else
		{
			deltaVEngineBurnTotals = new DeltaVEngineBurnTotals();
			stageBurnTotals.Add(stage, deltaVEngineBurnTotals);
		}
		if (partInfo != null && partInfo.decoupleBeforeBurn)
		{
			return;
		}
		DeltaVEngineBurnTotals deltaVEngineBurnTotals2 = deltaVEngineBurnTotals;
		DeltaVEngineBurnTotals deltaVEngineBurnTotals3 = deltaVEngineBurnTotals;
		DeltaVEngineBurnTotals deltaVEngineBurnTotals4 = deltaVEngineBurnTotals;
		double num = -1.0;
		deltaVEngineBurnTotals4.currentFuelFlowTotalBurnTime = -1.0;
		double setFuelFlowTotalBurnTime = num;
		num = -1.0;
		deltaVEngineBurnTotals3.setFuelFlowTotalBurnTime = setFuelFlowTotalBurnTime;
		deltaVEngineBurnTotals2.maxTotalBurnTime = num;
		int i = 0;
		for (int count = propellantInfo.Count; i < count; i++)
		{
			DeltaVPropellantInfo deltaVPropellantInfo = propellantInfo[i];
			float throttle = engine.thrustPercentage * 0.01f;
			throttle = engine.ApplyThrottleAdjustments(throttle);
			if (engine.IsEngineDead())
			{
				num = 0.0;
				deltaVPropellantInfo.amountPerSecondCurrentThrottle = 0.0;
				double amountPerSecondSetThrottle = num;
				num = 0.0;
				deltaVPropellantInfo.amountPerSecondSetThrottle = amountPerSecondSetThrottle;
				deltaVPropellantInfo.amountPerSecondMaxThrottle = num;
			}
			else
			{
				deltaVPropellantInfo.amountPerSecondMaxThrottle = engine.getMaxFuelFlow(deltaVPropellantInfo.propellant);
				deltaVPropellantInfo.amountPerSecondSetThrottle = engine.getFuelFlow(deltaVPropellantInfo.propellant, engine.maxFuelFlow * throttle);
				if (engine.requestedMassFlow > 0f)
				{
					deltaVPropellantInfo.amountPerSecondCurrentThrottle = engine.getFuelFlow(deltaVPropellantInfo.propellant, engine.requestedMassFlow);
				}
				else if (HighLogic.LoadedSceneIsFlight && vesselDeltaV.Vessel != null)
				{
					deltaVPropellantInfo.amountPerSecondCurrentThrottle = engine.getFuelFlow(deltaVPropellantInfo.propellant, engine.maxFuelFlow * (vesselDeltaV.Vessel.ctrlState.mainThrottle * throttle));
				}
				else
				{
					deltaVPropellantInfo.amountPerSecondCurrentThrottle = 0.0;
				}
			}
			double maxAmount = 0.0;
			if (deltaVPropellantInfo.propellant.name == "IntakeAir")
			{
				num = 0.0;
				deltaVPropellantInfo.currentBurnTime = 0.0;
				double maxBurnTime = num;
				num = 0.0;
				deltaVPropellantInfo.maxBurnTime = maxBurnTime;
				double setThrottleBurnTime = num;
				num = 0.0;
				deltaVPropellantInfo.setThrottleBurnTime = setThrottleBurnTime;
				deltaVPropellantInfo.amountAvailable = num;
				deltaVPropellantInfo.pendingDemand = 0.0;
			}
			else
			{
				engine.part.GetConnectedResourceTotals(deltaVPropellantInfo.propellant.id, deltaVPropellantInfo.propellant.GetFlowMode(), simulate: true, out deltaVPropellantInfo.amountAvailable, out maxAmount);
				num = 0.0;
				deltaVPropellantInfo.currentBurnTime = 0.0;
				double maxBurnTime2 = num;
				num = 0.0;
				deltaVPropellantInfo.maxBurnTime = maxBurnTime2;
				deltaVPropellantInfo.setThrottleBurnTime = num;
				if (deltaVPropellantInfo.amountPerSecondMaxThrottle != 0.0)
				{
					deltaVPropellantInfo.maxBurnTime = deltaVPropellantInfo.amountAvailable / deltaVPropellantInfo.amountPerSecondMaxThrottle;
				}
				if (deltaVPropellantInfo.amountPerSecondSetThrottle != 0.0)
				{
					deltaVPropellantInfo.setThrottleBurnTime = deltaVPropellantInfo.amountAvailable / deltaVPropellantInfo.amountPerSecondSetThrottle;
				}
				if (deltaVPropellantInfo.amountPerSecondCurrentThrottle != 0.0)
				{
					deltaVPropellantInfo.currentBurnTime = deltaVPropellantInfo.amountAvailable / deltaVPropellantInfo.amountPerSecondCurrentThrottle;
				}
				if (deltaVEngineBurnTotals.maxTotalBurnTime < 0.0)
				{
					deltaVEngineBurnTotals.maxTotalBurnTime = deltaVPropellantInfo.maxBurnTime;
				}
				else
				{
					deltaVEngineBurnTotals.maxTotalBurnTime = Math.Min(deltaVEngineBurnTotals.maxTotalBurnTime, deltaVPropellantInfo.maxBurnTime);
				}
				if (deltaVEngineBurnTotals.setFuelFlowTotalBurnTime < 0.0)
				{
					deltaVEngineBurnTotals.setFuelFlowTotalBurnTime = deltaVPropellantInfo.setThrottleBurnTime;
				}
				else
				{
					deltaVEngineBurnTotals.setFuelFlowTotalBurnTime = Math.Min(deltaVEngineBurnTotals.setFuelFlowTotalBurnTime, deltaVPropellantInfo.setThrottleBurnTime);
				}
				if (deltaVEngineBurnTotals.currentFuelFlowTotalBurnTime < 0.0)
				{
					deltaVEngineBurnTotals.currentFuelFlowTotalBurnTime = deltaVPropellantInfo.currentBurnTime;
				}
				else
				{
					deltaVEngineBurnTotals.currentFuelFlowTotalBurnTime = Math.Min(deltaVEngineBurnTotals.currentFuelFlowTotalBurnTime, deltaVPropellantInfo.currentBurnTime);
				}
			}
			DeltaVPropellantInfo item = deltaVPropellantInfo.Copy();
			deltaVEngineBurnTotals.propellantInfo.Add(item);
		}
	}

	public void ResetPropellantBurnValues()
	{
		for (int i = 0; i < propellantInfo.Count; i++)
		{
			propellantInfo[i].amountBurnt = 0.0;
		}
	}

	public void ResetResourceTotals()
	{
		double maxAmount = 0.0;
		int i = 0;
		for (int count = propellantInfo.Count; i < count; i++)
		{
			DeltaVPropellantInfo deltaVPropellantInfo = propellantInfo[i];
			deltaVPropellantInfo.amountAvailable = 0.0;
			if (engine != null && engine.part != null)
			{
				engine.part.GetConnectedResourceTotals(deltaVPropellantInfo.propellant.id, deltaVPropellantInfo.propellant.GetFlowMode(), simulate: true, out deltaVPropellantInfo.amountAvailable, out maxAmount);
			}
		}
	}

	public void ResetStageInfo()
	{
		maxTimeStep = 0f;
		lastStep = false;
	}

	public void CalcThrustActual(bool engineIngitedCheck = false)
	{
		if (HighLogic.LoadedSceneIsFlight && engine.part != null)
		{
			if (engine != null && !engine.EngineIgnited && engineIngitedCheck)
			{
				thrustActual = 0f;
				return;
			}
			if (engine != null && engine.EngineIgnited && engine.requestedMassFlow > 0f)
			{
				thrustActual = engine.finalThrust;
				return;
			}
			float num = (float)engine.part.staticPressureAtm;
			if (num > 0f)
			{
				thrustActual = engine.MaxThrustOutputAtm(runningActive: false, useThrustLimiter: true, num, engine.part.vessel.atmosphericTemperature, engine.part.vessel.atmDensity);
			}
			else
			{
				thrustActual = engine.MaxThrustOutputVac();
			}
		}
		else if (HighLogic.LoadedSceneIsEditor && engine.part != null && DeltaVGlobals.DeltaVAppValues != null)
		{
			thrustActual = engine.MaxThrustOutputAtm(runningActive: false, useThrustLimiter: true, DeltaVGlobals.DeltaVAppValues.atmPressure, 310.0, DeltaVGlobals.DeltaVAppValues.atmDensity);
		}
		else
		{
			thrustActual = engine.MaxThrustOutputAtm();
		}
	}

	public float PropellantMassBurnt()
	{
		float num = 0f;
		for (int i = 0; i < propellantInfo.Count; i++)
		{
			num += propellantInfo[i].MassBurnt();
		}
		return num;
	}

	public double GetFuelTimeMaxThrottle(int stage)
	{
		if (stageBurnTotals.ContainsKey(stage))
		{
			return stageBurnTotals[stage].maxTotalBurnTime;
		}
		return 0.0;
	}

	public double GetFuelTimeAtThrottle(int stage)
	{
		if (stageBurnTotals.ContainsKey(stage))
		{
			return stageBurnTotals[stage].setFuelFlowTotalBurnTime;
		}
		return 0.0;
	}

	public double GetFuelTimeAtActiveThrottle(int stage)
	{
		if (stageBurnTotals.ContainsKey(stage))
		{
			if (!(stageBurnTotals[stage].currentFuelFlowTotalBurnTime <= 0.0) && !double.IsInfinity(stageBurnTotals[stage].currentFuelFlowTotalBurnTime))
			{
				return stageBurnTotals[stage].currentFuelFlowTotalBurnTime;
			}
			return GetFuelTimeAtThrottle(stage);
		}
		return 0.0;
	}

	public void CalculateISP(bool engineIngitedCheck = false)
	{
		float time = 0f;
		float time2 = 1f;
		float time3 = (float)FlightGlobals.GetHomeBody().atmDensityASL;
		if (HighLogic.LoadedSceneIsFlight && engine.part != null)
		{
			time2 = (float)engine.part.vessel.mainBody.atmPressureASL;
			time3 = (float)engine.part.vessel.mainBody.atmDensityASL;
		}
		else if (HighLogic.LoadedSceneIsEditor && DeltaVGlobals.DeltaVAppValues != null && DeltaVGlobals.DeltaVAppValues.body != null)
		{
			time2 = (float)DeltaVGlobals.DeltaVAppValues.body.atmPressureASL;
			time3 = (float)DeltaVGlobals.DeltaVAppValues.body.atmDensityASL;
		}
		ispASL = engine.atmosphereCurve.Evaluate(time2);
		if (engine.useAtmCurveIsp)
		{
			ispASL *= engine.atmCurveIsp.Evaluate(time3);
		}
		ispVac = engine.atmosphereCurve.Evaluate(time);
		ispActual = ispVac;
		if (HighLogic.LoadedSceneIsFlight && engine.part != null)
		{
			if (engine != null && !engine.EngineIgnited && engineIngitedCheck)
			{
				ispActual = 0.0;
				return;
			}
			if (partInfo != null && partInfo.activationStage >= StageManager.CurrentStage && engine.EngineIgnited && engine.realIsp > 0f)
			{
				ispActual = engine.realIsp;
				return;
			}
			float num = (float)engine.part.staticPressureAtm;
			ispActual = engine.atmosphereCurve.Evaluate(num);
			if (engine.useThrottleIspCurve)
			{
				ispActual *= engine.GetThrottlingMult(num, engine.currentThrottle);
			}
			if (engine.useAtmCurveIsp)
			{
				ispActual *= engine.atmCurveIsp.Evaluate((float)(engine.part.atmDensity * 0.8163265306122448));
			}
			if (engine.useVelCurveIsp)
			{
				ispActual *= engine.velCurveIsp.Evaluate((float)engine.part.machNumber);
			}
		}
		else if (HighLogic.LoadedSceneIsEditor && engine.part != null && DeltaVGlobals.DeltaVAppValues != null)
		{
			ispActual = engine.atmosphereCurve.Evaluate(DeltaVGlobals.DeltaVAppValues.atmPressure);
			if (engine.useAtmCurveIsp)
			{
				ispActual *= engine.atmCurveIsp.Evaluate((float)(DeltaVGlobals.DeltaVAppValues.atmDensity * 0.8163265306122448));
			}
		}
	}

	public double GetISPVac()
	{
		CalculateISP();
		return ispVac;
	}

	public double GetISPASL()
	{
		CalculateISP();
		return ispASL;
	}

	public double GetISPActual()
	{
		CalculateISP();
		return ispActual;
	}

	public void CalculateThrustVector(bool engineIngitedCheck = false)
	{
		thrustVac = engine.MaxThrustOutputVac();
		if (HighLogic.LoadedSceneIsFlight && engine.part != null)
		{
			thrustASL = engine.MaxThrustOutputAtm(engineIngitedCheck, useThrustLimiter: true, (float)engine.part.vessel.mainBody.atmPressureASL, engine.part.vessel.mainBody.atmosphereTemperatureSeaLevel, engine.part.vessel.mainBody.atmDensityASL);
		}
		else if (HighLogic.LoadedSceneIsEditor && DeltaVGlobals.DeltaVAppValues != null && DeltaVGlobals.DeltaVAppValues.body != null)
		{
			thrustASL = engine.MaxThrustOutputAtm(runningActive: false, useThrustLimiter: true, (float)DeltaVGlobals.DeltaVAppValues.body.atmPressureASL, DeltaVGlobals.DeltaVAppValues.body.atmosphereTemperatureSeaLevel, DeltaVGlobals.DeltaVAppValues.body.atmDensityASL);
		}
		else
		{
			thrustASL = engine.MaxThrustOutputAtm();
		}
		CalcThrustActual(engineIngitedCheck);
		thrustVectorVac = Vector3.zero;
		thrustVectorASL = Vector3.zero;
		thrustVectorActual = Vector3.zero;
		CenterOfThrustQuery centerOfThrustQuery = new CenterOfThrustQuery();
		centerOfThrustQuery.Reset();
		engine.OnCenterOfThrustQuery(centerOfThrustQuery);
		thrustVectorVac = thrustVac * centerOfThrustQuery.dir;
		thrustVectorASL = thrustASL * centerOfThrustQuery.dir;
		thrustVectorActual = thrustActual * centerOfThrustQuery.dir;
	}

	public Vector3 GetThrustVectorVac()
	{
		CalculateThrustVector();
		return thrustVectorVac;
	}

	public Vector3 GetThrustVectorASL()
	{
		CalculateThrustVector();
		return thrustVectorASL;
	}

	public Vector3 GetThrustVectorActual()
	{
		CalculateThrustVector();
		return thrustVectorActual;
	}

	public float GetThrustVac()
	{
		CalculateThrustVector();
		return thrustVac;
	}

	public float GetThrustASL()
	{
		CalculateThrustVector();
		return thrustASL;
	}

	public float GetThrustActual()
	{
		CalculateThrustVector();
		return thrustActual;
	}

	public bool RequiresAir()
	{
		int num = 0;
		while (true)
		{
			if (num < propellantInfo.Count)
			{
				if (propellantInfo[num].propellant.name == "IntakeAir")
				{
					break;
				}
				num++;
				continue;
			}
			requiresAir = false;
			return false;
		}
		requiresAir = true;
		return true;
	}

	public bool ThrottleLimited()
	{
		if (engine != null && engine.thrustPercentage < 100f)
		{
			throttleLimited = true;
			return true;
		}
		throttleLimited = false;
		return false;
	}

	public double PropellantDemand(int id, bool runningActive)
	{
		double result = 0.0;
		runningActive = runningActive && engine.EngineIgnited && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0f;
		for (int i = 0; i < propellantInfo.Count; i++)
		{
			if (propellantInfo[i].propellant.id == id)
			{
				result = ((!runningActive) ? propellantInfo[i].amountPerSecondSetThrottle : propellantInfo[i].amountPerSecondCurrentThrottle);
				break;
			}
		}
		return result;
	}
}
