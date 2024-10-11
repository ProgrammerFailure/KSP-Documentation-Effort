using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;

public class ModuleScienceLab : PartModule, IResourceConsumer, IScienceDataContainer
{
	[CompilerGenerated]
	private sealed class _003CProcessData_003Ed__36 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleScienceLab _003C_003E4__this;

		public ScienceData item;

		public Callback<ScienceData> onComplete;

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
		public _003CProcessData_003Ed__36(int _003C_003E1__state)
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
	private sealed class _003CCleanUpVesselExperiments_003Ed__39 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Vessel v;

		public ModuleScienceLab _003C_003E4__this;

		private Part _003Cp_003E5__2;

		private int _003Ci_003E5__3;

		private int _003Cj_003E5__4;

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
		public _003CCleanUpVesselExperiments_003Ed__39(int _003C_003E1__state)
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
	private sealed class _003CCleanUpExperiment_003Ed__40 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleScienceExperiment exp;

		public ModuleScienceLab _003C_003E4__this;

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
		public _003CCleanUpExperiment_003Ed__40(int _003C_003E1__state)
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
	private sealed class _003ClockdownAndGatherProcessResources_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleScienceLab _003C_003E4__this;

		public float amount;

		public string expParttitle;

		public string progressMessageText;

		public string shortageMessageText;

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
		public _003ClockdownAndGatherProcessResources_003Ed__41(int _003C_003E1__state)
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
	private sealed class _003CgatherProcessResources_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleScienceLab _003C_003E4__this;

		public float amount;

		public string shortageMessageText;

		public string expParttitle;

		public string progressMessageText;

		private string _003CresourceName_003E5__2;

		private ModuleResource _003Cmr_003E5__3;

		private int _003Ci_003E5__4;

		private int _003CiC_003E5__5;

		private double _003Crequired_003E5__6;

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
		public _003CgatherProcessResources_003Ed__42(int _003C_003E1__state)
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

	public List<string> ExperimentData;

	[KSPField]
	public float crewsRequired;

	[KSPField]
	public float SurfaceBonus;

	[KSPField]
	public float ContextBonus;

	[KSPField]
	public float homeworldMultiplier;

	[KSPField]
	public int containerModuleIndex;

	[KSPField]
	public bool canResetConnectedModules;

	[KSPField]
	public bool canResetNearbyModules;

	[KSPField]
	public float interactionRange;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001440")]
	public string statusText;

	private ModuleScienceConverter _converter;

	[KSPField(isPersistant = true)]
	public float dataStored;

	[KSPField]
	public float dataStorage;

	[KSPField(isPersistant = true)]
	public float storedScience;

	public bool processingData;

	private bool transmittingData;

	public List<ModuleResource> processResources;

	private ModuleScienceContainer container;

	private BaseEvent cleanModulesEvent;

	private BaseEvent transmitScienceEvent;

	private ScreenMessage progressMessage;

	private bool allResourcesAvailable;

	private List<PartResourceDefinition> consumedResources;

	private ScienceData[] emptyData;

	public ModuleScienceConverter Converter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleScienceLab()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001441")]
	public void TransmitScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTransmissionComplete(ScienceData data, Vessel origin, bool xmitAborted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
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
	private void onPartActionUI(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateModuleUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsStorable(ScienceData item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CProcessData_003Ed__36))]
	public IEnumerator ProcessData(ScienceData item, Callback<ScienceData> onComplete = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StoreData(ScienceData item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001864")]
	public void CleanModulesEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CCleanUpVesselExperiments_003Ed__39))]
	public IEnumerator CleanUpVesselExperiments(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CCleanUpExperiment_003Ed__40))]
	public IEnumerator CleanUpExperiment(ModuleScienceExperiment exp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003ClockdownAndGatherProcessResources_003Ed__41))]
	private IEnumerator lockdownAndGatherProcessResources(float amount, string expParttitle, string progressMessageText, string shortageMessageText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CgatherProcessResources_003Ed__42))]
	private IEnumerator gatherProcessResources(float amount, string expParttitle, string progressMessageText, string shortageMessageText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsOperational()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TransmissionErrorScreenMessage(string reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sendDataToComms()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RecoverScienceLabs(ProtoVessel protoVessel, MissionRecoveryDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	ScienceData[] IScienceDataContainer.GetData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IScienceDataContainer.ReturnData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IScienceDataContainer.DumpData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IScienceDataContainer.ReviewData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IScienceDataContainer.ReviewDataItem(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	int IScienceDataContainer.GetScienceCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	bool IScienceDataContainer.IsRerunnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
