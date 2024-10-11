using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class DeltaVEngineInfo
{
	public VesselDeltaV vesselDeltaV;

	private DeltaVPartInfo _partInfo;

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

	private bool lastStep;

	private static double epsilon;

	public DeltaVPartInfo partInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVEngineInfo(VesselDeltaV inVesselDeltaV, ModuleEngines inEngine, MultiModeEngine inMultiModeEngine = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DeltaVEngineInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSituationISP(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetSituationThrust(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetSituationThrustVector(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool PropellantStarved()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CalculateBurn(DeltaVStageInfo deltaVStage, List<DeltaVEngineInfo> enginesStillActive, float deltaTime, bool runningActive, bool logMsgs, bool infiniteFuel, bool reCalc, out bool checkDeprived, out float minTimeStep)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ApplyBurn(DeltaVStageInfo deltaVStage, float deltaTime, bool runningActive, bool logMsgs, bool infiniteFuel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetCalcVariables()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SwitchEngine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CalculateFuelTime(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetPropellantBurnValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetResourceTotals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetStageInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CalcThrustActual(bool engineIngitedCheck = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float PropellantMassBurnt()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetFuelTimeMaxThrottle(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetFuelTimeAtThrottle(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetFuelTimeAtActiveThrottle(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateISP(bool engineIngitedCheck = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetISPVac()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetISPASL()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetISPActual()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateThrustVector(bool engineIngitedCheck = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetThrustVectorVac()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetThrustVectorASL()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetThrustVectorActual()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetThrustVac()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetThrustASL()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetThrustActual()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RequiresAir()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ThrottleLimited()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double PropellantDemand(int id, bool runningActive)
	{
		throw null;
	}
}
