using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RUI.Algorithms;
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

	private bool partsDisplayListDirty;

	private StringBuilder partsDisplayList;

	private Vector3 enginesThrustVac;

	private Vector3 enginesThrustASL;

	private Vector3 enginesThrustActual;

	private List<Part> cachedResourcePartSetParts;

	private List<DeltaVPartInfo> cachedActivatedParts;

	private SCCFlowGraph cachedFlowGraph;

	private Dictionary<uint, DeltaVPartInfo> partInfoDictionary;

	private float removedFuelMass;

	[SerializeField]
	private bool stageContainsOnlyLaunchClamps;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVStageInfo(ShipConstruct ship, int inStage, VesselDeltaV vesselDeltaV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVStageInfo(Vessel vessel, int inStage, VesselDeltaV vesselDeltaV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSituationISP(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetSituationTWR(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetSituationThrust(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetSituationVectoredThrust(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetSituationDeltaV(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetSituationTotalExhaustVelocity(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetSituationVectoredExhaustVelocity(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset(int inStage, VesselDeltaV vesselDeltaV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessParts(int inStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateStartMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StoreInterimDeltaV(List<DeltaVEngineInfo> engines, List<DeltaVCalc> deltaVCalcsList, float startingMass, float currentMass, double simulationTime, bool logMsgs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetHighestSeparationIndex(List<DeltaVEngineInfo> engines)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<DeltaVEngineInfo> MatchingSeparationIndex(List<DeltaVEngineInfo> engines, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<DeltaVEngineResourcePartInfo> GetEngineResourceParts(List<DeltaVEngineInfo> engines)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateEngineResourceFuelMass(List<DeltaVEngineResourcePartInfo> resourceParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool EnginesDeprived(List<DeltaVEngineInfo> enginesToStageNext, bool allEngines)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StoreStartFuelMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StoreEndFuelMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float CheckTimeStep(float timeStep, float simulationBurnTime, float fuelMassRemoved, List<DeltaVEngineInfo> enginesStillActive, List<DeltaVEngineResourcePartInfo> resourcePartsToStageNext, bool logMsgs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SimulateDeltaV(bool runningActive, bool infiniteFuel, float timeStep = 0.2f, bool logMsgs = true, bool thisStageActive = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal double CalculateTimeRequiredDV(bool runningActive, float deltaVRequested)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CalcLerpDeltaV()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal string GetPartDisplayInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ProcessActiveEngines()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetPartCaches()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetStageMass(out float dryMass, out float fuelMass, out float decoupledDryMass, out float decoupledFuelMass, out float jettisonedDryMass, out int decoupledPartCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCurrentStageMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsAnchoredDecoupler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsDecoupler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Ray CoTForStage(bool activeOnly = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateISP(List<DeltaVEngineInfo> engines = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateTWR(float mass = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int PartsActiveInStage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int PartsActivateInStage(out List<DeltaVPartInfo> activeParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int PartsDecoupledInStage()
	{
		throw null;
	}
}
