using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class VesselDeltaV : MonoBehaviour
{
	private enum PartStageComparisonOperator
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

	[CompilerGenerated]
	private sealed class _003CRunCalculations_003Ed__100 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public VesselDeltaV _003C_003E4__this;

		private List<DeltaVStageInfo> _003Cstages_003E5__2;

		private int _003Ci_003E5__3;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CRunCalculations_003Ed__100(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public DeltaVEngineStageSet engineStageSet;

	[SerializeField]
	private Vessel _vessel;

	[SerializeField]
	private ShipConstruct _ship;

	[SerializeField]
	private bool syncListInstances;

	[SerializeField]
	private bool useMultipleInfoLists;

	[SerializeField]
	private List<int> _separationStageIndexes;

	[SerializeField]
	private List<DeltaVPartInfo> _partInfo;

	[SerializeField]
	private double _totalDeltaVVac;

	[SerializeField]
	private double _totalDeltaVASL;

	[SerializeField]
	private double _totalDeltaVActual;

	[SerializeField]
	private double _totalBurnTime;

	[SerializeField]
	private Mode _activeMode;

	public bool currentStageActivated;

	public int lowestStageWithDeltaV;

	private Coroutine simulation;

	private bool _isReady;

	private bool _doStockSimulation;

	private bool updateFlightScene;

	private double flightLastFullUpdateTime;

	private double flightLastStageUpdateTime;

	[SerializeField]
	private bool calcsDirty;

	private bool resetPartCaches;

	[SerializeField]
	private int frames;

	private double vesselEventDelayTime;

	internal List<DeltaVEngineInfo> WorkingEngineInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<DeltaVEngineInfo> OperatingEngineInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vessel Vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ShipConstruct Ship
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[SerializeField]
	private List<DeltaVStageInfo> WorkingStageInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[SerializeField]
	public List<DeltaVStageInfo> OperatingStageInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<int> SeparationStageIndexes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<DeltaVPartInfo> PartInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double TotalDeltaVVac
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double TotalDeltaVASL
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double TotalDeltaVActual
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double TotalBurnTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Mode ActiveMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool SimulationRunning
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsReady
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool DoStockSimulation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool UpdateFlightScene
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselDeltaV()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool EnableStockSimluation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool DisableStockSimluation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSituationTotalDeltaV(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselDeltaV Create(Vessel vesselRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselDeltaV Create(ShipConstruct shipRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckDirtyAndRun()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselSOIChanged(GameEvents.HostedFromToAction<Vessel, CelestialBody> fromToActon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnStagingSeparationIndices()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUIStageSequenceModified()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartResourceFlowStateChange(GameEvents.HostedFromToAction<PartResource, bool> hostedFromTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnStageActivate(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNodeChangedVesselResources(MENode node, Vessel vsl, Part part, ProtoPartSnapshot protoPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEngineThrustPercentageChanged(ModuleEngines engine)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselGoOffRails(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEngineActiveChange(ModuleEngines engine)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartModuleAdjusterRemoved(PartModule module, AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartModuleAdjusterAdded(PartModule module, AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChange(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMultiModeEngineSwitchActive(MultiModeEngine engine)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCalcsDirty(bool resetPartCaches, bool syncListInstances = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDockingComplete(GameEvents.FromToAction<Part, Part> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StagesChanged(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateResourceSetsEvent(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateResourceSetsEventSetFL(GameEvents.HostedFromToAction<bool, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onShipModified(ShipConstruct modifiedShip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselWasModified(Vessel modifiedVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_CrewChange(VesselCrewManifest crewManifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onChangeEngineDVIncludeState(ModuleEngines engine)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateSeparationStageIndexes(List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStageInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessPayloadStages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetPartInfo(List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessEnginePartInfoChildren(DeltaVPartInfo partInfoItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessDecouplePartInfoChildren(DeltaVPartInfo partInfoItem, PartStageComparisonOperator comparisonOp, int decoupleStage, int activationStage, bool bypassFairings, bool bypassJettison, bool setToParentDecoupleStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void changePartInfoDecoupleStage(DeltaVPartInfo partInfoItem, DeltaVPartInfo childPart, bool setToParentDecoupleStage = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRunCalculations_003Ed__100))]
	private IEnumerator RunCalculations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SimulateFlightScene()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SimulateLastStage(bool recalcVesselTotals = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReCalculateVesselTotals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double stageDVActual(DeltaVStageInfo stageInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetSimulationResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetStagePartCaches()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateModuleEngines()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessActiveUpdateEngines(DeltaVStageInfo currentDeltaVStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DeltaVStageInfo GetWorkingStage(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVStageInfo GetStage(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetActivatedEngines()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetHighestSeparationStage(int inStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetSeparationIndexes()
	{
		throw null;
	}
}
