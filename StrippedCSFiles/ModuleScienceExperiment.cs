using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Flight.Dialogs;
using UnityEngine;

public class ModuleScienceExperiment : PartModule, IScienceDataContainer
{
	public delegate void ResetCallback();

	[Serializable]
	public class EVASituation : ScriptableObject, IConfigNode
	{
		public int priority;

		public uint situationMask;

		public bool RequiresAtmosphere;

		public bool RequiresNoAtmosphere;

		public float MinTemp;

		public float MaxTemp;

		public string KerbalAction;

		public List<string> ResultString;

		public float dialogDelay;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public EVASituation()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass76_0
	{
		public ModuleScienceExperiment _003C_003E4__this;

		public bool showDialog;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public _003C_003Ec__DisplayClass76_0()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void _003CgatherData_003Eb__0()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CgatherData_003Ed__76 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleScienceExperiment _003C_003E4__this;

		public bool showDialog;

		private _003C_003Ec__DisplayClass76_0 _003C_003E8__1;

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
		public _003CgatherData_003Ed__76(int _003C_003E1__state)
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
	private sealed class _003COnScienceCompleteDelay_003Ed__78 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleScienceExperiment _003C_003E4__this;

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
		public _003COnScienceCompleteDelay_003Ed__78(int _003C_003E1__state)
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
	private sealed class _003CSetFXModules_003Ed__79 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float tgt;

		public ModuleScienceExperiment _003C_003E4__this;

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
		public _003CSetFXModules_003Ed__79(int _003C_003E1__state)
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
	private sealed class _003CresetExperiment_003Ed__95 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleScienceExperiment _003C_003E4__this;

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
		public _003CresetExperiment_003Ed__95(int _003C_003E1__state)
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
	public string experimentID;

	[KSPField]
	public string experimentActionName;

	[KSPField]
	public string resetActionName;

	[KSPField]
	public string reviewActionName;

	[KSPField]
	public bool useStaging;

	[KSPField]
	public bool useActionGroups;

	[KSPField]
	public bool hideUIwhenUnavailable;

	[KSPField]
	public bool rerunnable;

	[KSPField]
	public bool resettable;

	[KSPField]
	public bool resettableOnEVA;

	[KSPField]
	public bool hideFxModuleUI;

	[KSPField]
	public string transmitWarningText;

	[KSPField]
	public string collectWarningText;

	[KSPField]
	public string resourceToReset;

	[KSPField]
	public float resourceResetCost;

	public ResetCallback OnExperimentReset;

	[KSPField]
	public float xmitDataScalar;

	[KSPField]
	public float scienceValueRatio;

	[KSPField]
	public bool showScienceValueRatio;

	[KSPField]
	public bool dataIsCollectable;

	[KSPField]
	public string collectActionName;

	[KSPField]
	public float interactionRange;

	[KSPField]
	public int usageReqMaskInternal;

	[KSPField]
	public int usageReqMaskExternal;

	[KSPField]
	public bool availableShielded;

	[KSPField]
	public bool deployableSeated;

	[KSPField]
	public bool requiresInventoryPart;

	[KSPField]
	public string requiredInventoryPart;

	[KSPField]
	public bool requiresEVASituation;

	public List<EVASituation> evaSituations;

	[KSPField]
	public float dialogDelay;

	[KSPField]
	public string extraResultString;

	private bool kerbalInSeat;

	[KSPField(isPersistant = true)]
	public bool Deployed;

	[KSPField(isPersistant = true)]
	public bool Inoperable;

	[KSPField(guiActive = false, guiName = "")]
	public string usageReqMessage;

	[KSPField]
	public double cooldownTimer;

	public bool useCooldown;

	[KSPField(isPersistant = true)]
	public double cooldownToGo;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001861")]
	public string cooldownString;

	public ScienceExperiment experiment;

	private ScienceSubject subject;

	private ScienceData experimentData;

	public bool DeployEventDisabled;

	private ExperimentsResultDialog resultsDialog;

	private PopupDialog overwriteDialog;

	public int[] fxModuleIndices;

	private List<IScalarModule> fxModules;

	private ModuleInventoryPart inventory;

	private EVASituation validEvaSituation;

	private bool resettingExperiment;

	private double dialogTimer;

	private bool showDialogAfter;

	private ExperimentSituations situation;

	public bool containersDirty;

	public bool hasContainer;

	public bool HasExperimentData
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleScienceExperiment()
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
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vcs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnModuleInventoryChanged(ModuleInventoryPart inventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCommandSeatInteraction(KerbalEVA eva, bool entered)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameUnpause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGamePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateModuleUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ValidEVASituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_502050")]
	public void DeployExperiment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("Deploy")]
	public void DeployAction(KSPActionParam actParams)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CgatherData_003Ed__76))]
	private IEnumerator gatherData(bool showDialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnScienceComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003COnScienceCompleteDelay_003Ed__78))]
	private IEnumerator OnScienceCompleteDelay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetFXModules_003Ed__79))]
	private IEnumerator SetFXModules(float tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void endExperiment(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void dumpData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sendDataToComms(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sendDataToLab(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1.5f)]
	public void CollectDataExternalEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TransferToContainer(ModuleScienceContainer container, string destName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTransferWarning(bool dontShowAgain)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onCollectData(ModuleScienceContainer vesselContainer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001439")]
	public void ReviewDataEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void reviewData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void keepData(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001436")]
	public void ResetExperiment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001436")]
	public void ResetAction(KSPActionParam actParams)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, guiName = "#autoLOC_6002397")]
	public void DeployExperimentExternal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, guiName = "#autoLOC_900305")]
	public void ResetExperimentExternal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CresetExperiment_003Ed__95))]
	private IEnumerator resetExperiment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceData[] GetData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReturnData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DumpData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReviewData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetScienceCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsRerunnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInoperable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, guiName = "#autoLOC_6001862")]
	public void CleanUpExperimentExternal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReviewDataItem(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLoc_6001496")]
	public void TransferDataEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindContainer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDataTransfer(PartItemTransfer.DismissAction dma, Part containerPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsStageable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingEnabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingToggleEnabledEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingToggleEnabledFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStagingEnableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStagingDisableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeDetached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeRotated()
	{
		throw null;
	}
}
