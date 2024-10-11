using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleDataTransmitter : PartModule, IScienceDataTransmitter, IResourceConsumer, IContractObjectiveModule, ICommAntenna, IModuleInfo
{
	[CompilerGenerated]
	private sealed class _003CtransmitQueuedData_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleDataTransmitter _003C_003E4__this;

		public float dataPacketSize;

		public bool sendData;

		public float transmitInterval;

		private ScienceData _003Cdata_003E5__2;

		private float _003CtotalData_003E5__3;

		private int _003CpacketsToSend_003E5__4;

		private int _003CtotalPackets_003E5__5;

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
		public _003CtransmitQueuedData_003Ed__42(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CWaitForFixedSeconds_003Ed__43 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float seconds;

		private float _003CcurTime_003E5__2;

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
		public _003CWaitForFixedSeconds_003Ed__43(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CSetFXModules_003Ed__48 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public List<IScalarModule> modules;

		public float tgtValue;

		private bool _003CallFXDone_003E5__2;

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
		public _003CSetFXModules_003Ed__48(int _003C_003E1__state)
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

	[KSPField]
	public AntennaType antennaType;

	[KSPField]
	public float packetInterval;

	[KSPField]
	public float packetSize;

	[KSPField(isPersistant = true)]
	public bool xmitIncomplete;

	[KSPField]
	public double packetResourceCost;

	[KSPField]
	public int animationModuleIndex;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001428")]
	public string statusText;

	[KSPField(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001429")]
	public string powerText;

	[KSPField]
	public double antennaPower;

	[KSPField]
	public DoubleCurve rangeCurve;

	[KSPField]
	public DoubleCurve scienceCurve;

	[KSPField]
	public bool antennaCombinable;

	[KSPField]
	public double antennaCombinableExponent;

	protected bool busy;

	protected bool xmitAborted;

	protected bool xmitOnHold;

	protected float deployFxModuleStartPosition;

	protected double capacitorCharge;

	protected List<ScienceData> transmissionQueue;

	public int[] DeployFxModuleIndices;

	protected List<IScalarModule> deployFxModules;

	public int[] ProgressFxModuleIndices;

	protected List<IScalarModule> progressFxModules;

	protected ScreenMessage statusMessage;

	protected ScreenMessage progressMessage;

	protected ScreenMessage errorMessage;

	protected RnDCommsStream commStream;

	protected List<PartResourceDefinition> consumedResources;

	protected string errorStr;

	private List<string> tempAppliedUpgrades;

	private List<AdjusterDataTransmitterBase> adjusterCache;

	public virtual float DataRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual double DataResourceCost
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CommCombinable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double CommCombinableExponent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public AntennaType CommType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DoubleCurve CommRangeCurve
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DoubleCurve CommScienceCurve
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double CommPower
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDataTransmitter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePowerText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetVesselSignalStrength()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double EvaluateScienceMultiplier(double signalStrength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual List<IScalarModule> findFxModules(int[] indices, bool showUI)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001430")]
	public virtual void StartTransmissionAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001430")]
	public virtual void StartTransmission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onStartTransmission(List<IScienceDataContainer> experiments)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual List<ScienceData> queueVesselData(List<IScienceDataContainer> experiments)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CtransmitQueuedData_003Ed__42))]
	protected virtual IEnumerator transmitQueuedData(float transmitInterval, float dataPacketSize, Callback callback = null, bool sendData = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitForFixedSeconds_003Ed__43))]
	protected IEnumerator WaitForFixedSeconds(float seconds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CanSetFXModules(List<IScalarModule> modules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float GetFxModuleScalar(List<IScalarModule> modules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float GetModulesScalarMax(List<IScalarModule> modules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float GetModulesScalarMin(List<IScalarModule> modules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetFXModules_003Ed__48))]
	protected virtual IEnumerator SetFXModules(List<IScalarModule> modules, float tgtValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetFXModulesUI(List<IScalarModule> fxModules, bool readState, bool writeState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001860")]
	public virtual void TransmitIncompleteToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_7001226")]
	public virtual void StopTransmission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AbortTransmission(string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ReturnDataToContainer(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanTransmit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsBusy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void TransmitData(List<ScienceData> dataQueue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void TransmitData(List<ScienceData> dataQueue, Callback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetContractObjectiveType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CheckContractObjectiveValidity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanScienceTo(bool combined, double bPower, double sqrDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanComm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanCommUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double CommPowerUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double ApplyPowerAdjustments(double power, List<AdjusterDataTransmitterBase> adjusterList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool IsDataTransmitterBroken(List<AdjusterDataTransmitterBase> adjusterList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Callback<Rect> GetDrawModulePanelCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetPrimaryField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
