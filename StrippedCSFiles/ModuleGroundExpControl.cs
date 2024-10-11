using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommNet;
using Expansions.Serenity.DeployedScience.Runtime;

public class ModuleGroundExpControl : ModuleGroundCommsPart, ICommNetControlSource
{
	[CompilerGenerated]
	private sealed class _003CScanAndAddControlUnit_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleGroundExpControl _003C_003E4__this;

		private List<ModuleGroundSciencePart> _003CconnectedUnits_003E5__2;

		private List<Part> _003CloadedParts_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003CScanAndAddControlUnit_003Ed__21(int _003C_003E1__state)
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
	public int controlUnitRange;

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002246")]
	public int experimentsConnected;

	[KSPField(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_6001355")]
	public string commNetSignal;

	[KSPField(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_6001356")]
	public string commNetFirstHopDistance;

	[KSPField(guiActiveEditor = false, guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8005418")]
	public string powerNeeded;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	protected double boostedAntennaPower;

	private bool beingDestroyed;

	private static string cacheAutoLOC_7001411;

	private static string cacheAutoLOC_8002233;

	protected CommNetVessel Connection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override double CommPower
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleGroundExpControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUncommand = true, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_900678")]
	public void SetVesselNaming()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundScienceClusterPowerStateChanged(DeployedScienceCluster cluster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundScienceModuleRemoved(ModuleGroundSciencePart sciencePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundSciencePartEnabledStateChanged(ModuleGroundSciencePart sciencePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundScienceClusterUpdated(ModuleGroundExpControl controlUnit, DeployedScienceCluster cluster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CScanAndAddControlUnit_003Ed__21))]
	private IEnumerator ScanAndAddControlUnit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundSciencePartDeployed(ModuleGroundSciencePart sciencePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDeployedSciencePartVesselType(ModuleGroundSciencePart sciencePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartActionUIOpened(UIPartActionWindow window, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartActionUIDismiss(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdateModuleUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RecalcBoosterPower(ModuleGroundSciencePart sciencePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double CommPowerUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual VesselControlState GetControlSourceState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsCommCapable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateNetwork()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal new static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	string ICommNetControlSource.get_name()
	{
		throw null;
	}
}
