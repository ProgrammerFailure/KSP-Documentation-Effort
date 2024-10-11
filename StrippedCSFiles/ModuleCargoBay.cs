using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleCargoBay : PartModule, IAirstreamShield
{
	[Serializable]
	public class PartCollider
	{
		public Collider collider;

		public Part owner;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PartCollider(Part p, Collider c)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CscheduledColliderResetCoroutine_003Ed__40 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleCargoBay _003C_003E4__this;

		public bool enableShieldedVolume;

		public float delay;

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
		public _003CscheduledColliderResetCoroutine_003Ed__40(int _003C_003E1__state)
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
	public int DeployModuleIndex;

	private IScalarModule deployModule;

	[KSPField]
	public bool useBayContainer;

	[KSPField]
	public string bayContainerName;

	public Collider bayContainer;

	[KSPField]
	public float closedPosition;

	[KSPField]
	public float lookupRadius;

	[KSPField]
	public Vector3 lookupCenter;

	private List<Part> partsInCargoPrev;

	[SerializeField]
	private List<Part> partsInCargo;

	private List<Callback<IAirstreamShield>> onShdModifiedCallbacks;

	private List<PartCollider> ownColliders;

	[NonSerialized]
	private AttachNode NodeOuterFore;

	[KSPField]
	public string nodeOuterForeID;

	[NonSerialized]
	private AttachNode NodeOuterAft;

	[KSPField]
	public string nodeOuterAftID;

	[NonSerialized]
	private AttachNode NodeInnerFore;

	[KSPField]
	public string nodeInnerForeID;

	[NonSerialized]
	private AttachNode NodeInnerAft;

	[KSPField]
	public string nodeInnerAftID;

	[KSPField]
	public string partTypeName;

	private ModuleAnimateGeneric bayAnimator;

	private ModuleProceduralFairing fairing;

	private ModuleServiceModule serviceModule;

	private bool originalDeployFairingEventActive;

	private bool originalDeployFairingActionActive;

	private bool colliderResetCoroutineComplete;

	private bool shouldEnableShieldedVolume;

	private List<Part> connectingParts;

	private List<ModuleCargoBay> connectedCargoBays;

	protected int layerMask;

	private float colliderResetSchedule;

	private bool enableShieldedVolumeAfterColliderReset;

	private List<AdjusterCargoBayBase> adjusterCache;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleCargoBay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStartFinished(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindAttachNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorVesselModified(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void scheduleColliderReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CscheduledColliderResetCoroutine_003Ed__40))]
	private IEnumerator scheduledColliderResetCoroutine(float delay, bool enableShieldedVolume)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ColliderReset(bool enableShieldedVolume)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCommandSeatInteraction(KerbalEVA eva, bool entered)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselUnpack(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselPack(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCargoBayDoorsMoving(float from, float to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCargoBayDoorsStopped(float at)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartDestroyed(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ColliderListCleanUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private IScalarModule findModule(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<Part> FindEnclosedParts(List<Part> parts, List<PartCollider> ownColliders)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PartWithinBounds(Part p, List<PartCollider> ownColliders)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<Part> FindNearbyParts(float radius, Vector3 center)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableShieldedVolume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableShieldedVolume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupDynamicCargoOccluders(bool testActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<Callback<IAirstreamShield>> ShieldEnclosedParts(List<Part> enclosedParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnShieldEnclosedParts(List<Part> enclosedParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModifyShieldInEnclosedParts(bool shieldStatus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NotifyShieldModified(bool notifyConnected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<PartCollider> FindPartColliders(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindBayColliders(Part originalPart, ModuleCargoBay origin, List<PartCollider> cList, List<ModuleCargoBay> cBays)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<AttachNode> FindConnectingNodes(Part origin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private AttachNode GetOpposingNode(Part origin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddConnectingPart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveConnectingPart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearConnectingParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void scheduleColliderResetForAllBays()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLookupRadius(float radius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLookupCenter(Vector3 p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ClosedAndLocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SelfClosedAndLocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool EndCapped()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool TestAttachmentFit(AttachNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vessel GetVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part GetPart()
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
	public void ForceCargoBayStuck(AdjusterPartModuleBase.FailureOpenState stuckState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveAdjusterForcingCargoBayStuck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int CountAdjustersForcingCargoBayStuck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
