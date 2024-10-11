using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using EditorGizmos;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EVAConstructionModeEditor : EditorLogicBase
{
	public class SelectedPartCollider
	{
		public Collider collider;

		public bool isTrigger;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SelectedPartCollider(Collider col)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CResetToPhysicalPart_003Ed__69 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part p;

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
		public _003CResetToPhysicalPart_003Ed__69(int _003C_003E1__state)
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

	private ConstructionMode evaConstructionMode;

	private Part lastAttachedPart;

	private Part rootPart;

	[SerializeField]
	private AudioSource audioSource;

	public AudioClip attachClip;

	public AudioClip deletePartClip;

	public AudioClip partGrabClip;

	public AudioClip partReleaseClip;

	public AudioClip cannotPlaceClip;

	public AudioClip tweakGrabClip;

	public AudioClip tweakReleaseClip;

	public AudioClip reRootClip;

	private Part backedUpPart;

	private AttachNode backedUpAttachment;

	private AttachNode backedUpParentAttachment;

	[SerializeField]
	private UIOnClick angleSnapButton;

	[SerializeField]
	private UIStateImage angleSnapSprite;

	[SerializeField]
	private List<Vessel> attachVessels;

	private Quaternion gizmoAttRotate;

	private Quaternion gizmoAttRotate0;

	private GizmoRotate gizmoRotate;

	private GizmoOffset gizmoOffset;

	[SerializeField]
	private Button coordSpaceBtn;

	[SerializeField]
	private TextMeshProUGUI coordSpaceText;

	private Space symmetryCoordSpace;

	private bool skipPartAttach;

	private Ray ray;

	private RaycastHit hit;

	internal Collider FairingHitCollider;

	[SerializeField]
	private Attachment attachment;

	private Attachment[] cPartAttachments;

	private bool allowSrfAttachment;

	private bool allowNodeAttachment;

	private bool attachSuccesful;

	private bool isPlacementValid;

	private bool isPlacementOnGround;

	internal Quaternion selectedPartOriginalRotation;

	public float placementGroundOffset;

	private Quaternion vesselRotation;

	public float dragSharpness;

	public float srfAttachAngleSnap;

	public float srfAttachAngleSnapFine;

	private List<SelectedPartCollider> selectedPartColliders;

	private int assistingKerbals;

	private double combinedConstructionWeightLimit;

	[SerializeField]
	private bool attachmentPossible;

	private Vector3 offsetGap;

	private float threshold;

	private AttachNode childToParent;

	private AttachNode parentToChild;

	private Vector3 diff;

	public ConstructionMode EVAConstructionMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part SelectedPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part LastAttachedPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part RootPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsPlacementValid
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int AssistingKerbals
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double CombinedConstructionWeightLimit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EVAConstructionModeEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCurrentMousePartChanged(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnConstructionModeChanged(ConstructionMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameAboutToQuicksave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapViewEntered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CResetToPhysicalPart_003Ed__69))]
	private IEnumerator ResetToPhysicalPart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePartPlacementPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighLightPossibleAttach(bool highlight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PickupPartInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PickupPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveAttachNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessAttachNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselsInConstructionRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselUnloaded(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttachInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void partRotationResetUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Part AttachPart(Part part, Attachment attach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetModuleCargoPartValue(Part part, string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DetachInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DetachPart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CopyActionGroups(Part sPart, Part cPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartDropped()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BackupPart(Part part, Part partParent, bool droppedPart = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RestoreLastAttachedPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RestoreLastDroppedPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Attachment CheckAttach(Part selPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForceDrop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DropPartInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DropAttachablePart(string partName, Vector3 partPosition, Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode GetProtoVesselNode(string partName, Vector3 partPosition, Quaternion rotation, Vessel vessel, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode CreatePartNode(uint id, Part part, AvailablePart availablePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAssistingKerbals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleRotateGizmo(bool activate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRotateGizmoUpdate(Quaternion dRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRotateGizmoUpdated(Quaternion dRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetPivotOffset(Vector3 pivot0, Vector3 pivot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleOffsetGizmo(bool activate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOffsetGizmoUpdate(Vector3 dPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOffsetGizmoUpdated(Vector3 dPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CoordToggleInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleCoordSpaceButton(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeCoordSpace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SnapInputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SnapButton(PointerEventData evtData = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PutPartBackInInventory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectPartInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetRootPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FetchColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleCollidersTrigger(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnConstructionMode(bool opened)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyHeldPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BreakSymmetry(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyBackupPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CanPartBeEdited(Part part, bool weightOnlyCheck)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CannotInteract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InterruptKerbalWelding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputCheckDetachCompundPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayAudioClip(AudioClip clip)
	{
		throw null;
	}
}
