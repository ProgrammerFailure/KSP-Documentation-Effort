using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleGrappleNode : PartModule, IActiveJointHost, IJointLockState, IContractObjectiveModule
{
	[CompilerGenerated]
	private sealed class _003ClateFSMStart_003Ed__49 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleGrappleNode _003C_003E4__this;

		public StartState st;

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
		public _003ClateFSMStart_003Ed__49(int _003C_003E1__state)
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
	private sealed class _003CWaitAndSwitchFocus_003Ed__60 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleGrappleNode _003C_003E4__this;

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
		public _003CWaitAndSwitchFocus_003Ed__60(int _003C_003E1__state)
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
	public string nodeTransformName;

	[KSPField]
	public string controlTransformName;

	[KSPField]
	public float undockEjectionForce;

	[KSPField]
	public float minDistanceToReEngage;

	[KSPField]
	public float captureRange;

	[KSPField]
	public float captureMinFwdDot;

	[KSPField]
	public float captureMaxRvel;

	[KSPField]
	public float pivotRange;

	[KSPField]
	public int deployAnimationController;

	[KSPField]
	public float deployAnimationTarget;

	public DockedVesselInfo vesselInfo;

	public DockedVesselInfo otherVesselInfo;

	public string state;

	public Transform nodeTransform;

	public Transform controlTransform;

	public uint dockedPartUId;

	private Part otherPart;

	private ModuleAnimateGeneric deployAnimator;

	private bool originalDeployAnimatorAllowManualControl;

	private AttachNode grappleNode;

	private RaycastHit hit;

	private Vector3 rGrabPoint;

	private Vector3 initNodeForward;

	private Vector3 initNodeUp;

	private Vector3 grabPos;

	private Vector3 grabOrt;

	private Vector3 grabOrt2;

	private ActiveJointPivot pivotJoint;

	private PartJoint GrappleJoint;

	private bool grappledSameVessel;

	private KerbalFSM fsm;

	private KFSMState st_ready;

	private KFSMState st_grappled;

	private KFSMState st_grappled_sameVessel;

	private KFSMState st_disengage;

	private KFSMState st_disabled;

	private KFSMEvent on_nodeDistance;

	private KFSMEvent on_nodeLost;

	private KFSMEvent on_contact;

	private KFSMEvent on_undock;

	private KFSMEvent on_sameVessel_disconnect;

	private KFSMEvent on_disable;

	private KFSMEvent on_enable;

	private KFSMEvent on_decouple;

	private List<AdjusterGrappleNodeBase> adjusterCache;

	public bool IsDisabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleGrappleNode()
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
	public override void OnStart(StartState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003ClateFSMStart_003Ed__49))]
	private IEnumerator lateFSMStart(StartState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Part FindContactParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckGrappleContact(Transform nodeT, RaycastHit hit, float minDist, float minFwdDot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Grapple(Part other, Part dockerSide)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateGrappleTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001450", activeEditor = false)]
	public void ReleaseAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001451")]
	public void Release()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitAndSwitchFocus_003Ed__60))]
	private IEnumerator WaitAndSwitchFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GrappleSameVessel(Part other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroySameVesselJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001451")]
	public void ReleaseSameVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnOtherNodeSameVesselDisconnect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool OtherPartIsLost()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NodeIsTooFar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001452", activeEditor = false)]
	public void DecoupleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001452")]
	public void Decouple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001447")]
	public void MakeReferenceToggle(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001447")]
	public void MakeReferenceTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001453")]
	public void SetLoose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001454")]
	public void LockPivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsLoose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part GetHostPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnJointInit(ActiveJoint joint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDriveModeChanged(ActiveJoint.DriveMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetLocalTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsJointUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetContractObjectiveType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckContractObjectiveValidity()
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
	public void SetUpAdjusterBlockingGrappleNodeAbilityToRelease()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveAdjusterBlockingGrappleNodeAbilityToRelease()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int CountAdjustersBlockingGrappleRelease()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool IsAdjusterBlockingGrappleGrab()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
