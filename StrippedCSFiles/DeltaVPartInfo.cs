using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public class DeltaVPartInfo
{
	public class PartStageFuelMass
	{
		public float startMass;

		public float endMass;

		public PartResourceList resources;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PartStageFuelMass(Part part)
		{
			throw null;
		}
	}

	public VesselDeltaV vesselDeltaV;

	public Part part;

	public int decoupleStage;

	public bool decoupleBeforeBurn;

	public int activationStage;

	public bool isDecoupler;

	public bool isStageSeparator;

	public bool isFairing;

	public bool isDockingPort;

	public bool isEngine;

	public bool isJettison;

	public bool isIntake;

	public bool isSolarPanel;

	public bool isGenerator;

	public ModuleResourceIntake moduleResourceIntake;

	public ModuleDeployableSolarPanel moduleDeployableSolarPanel;

	public ModuleGenerator moduleGenerator;

	public ModuleDockingNode moduleDockingNode;

	public ModuleDecouple moduleDecoupler;

	public ModuleAnchoredDecoupler moduleAnchoredDecoupler;

	public IStageSeparator moduleStageSeparator;

	public List<ModuleEngines> engines;

	public float dryMass;

	public float fuelMass;

	public float jettisonMass;

	public DictionaryValueList<int, PartStageFuelMass> stageFuelMass;

	public Part jettisonPart;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVPartInfo(Part part, VesselDeltaV vesselDeltaV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int CheckDecoupler(int chkStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetSeparationStage(int chkStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void StageStartFuelMass(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void StageEndFuelMass(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateMassValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateStagingValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool FindDecouplerParent(Part inPart, out Part decouplerParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetStartSimulationResources(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool JettisonInStage(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCurrentFuelMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetStageStartMass(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetStageEndMass(int stage)
	{
		throw null;
	}
}
