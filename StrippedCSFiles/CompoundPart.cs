using System.Runtime.CompilerServices;
using CompoundParts;
using UnityEngine;

public class CompoundPart : Part
{
	public enum AttachState
	{
		Detached,
		Attaching,
		Attached
	}

	public Vector3 direction;

	public Vector3 targetPosition;

	public Quaternion targetRotation;

	public Part target;

	public string targetMeshColName;

	public float maxLength;

	public AttachState attachState;

	private RaycastHit hit;

	private RaycastHit[] hits;

	private bool hasSaveData;

	private uint tgtId;

	private uint targetPersistentId;

	private bool needsDirectionFlip;

	private CompoundPart original;

	private CompoundPartModule[] cmpModules;

	[KSPField]
	public string disconnectedEffectName;

	private Vector3 wTgtPos;

	private Quaternion wTgtRot;

	private bool tweakStarted;

	private bool tweakEnded;

	private ICMTweakTarget tweakTargetModule;

	[KSPField(isPersistant = true)]
	public bool disconnectAction;

	public bool isTweakingTarget
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CompoundPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6006112")]
	protected void DisconnectEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006112")]
	protected void DisconnectAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DisconnectCompoundPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onCopy(Part original, bool asSymCPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onPartAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onStartComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onPartAttach(Part parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onPartDetach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onPartDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void onEditorStartTweak()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void onEditorEndTweak()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleSymmetryCounterpartsTweak(bool toggleValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Transform GetReferenceTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Part GetReferenceParent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetSymmetryValues(Vector3 newPosition, Quaternion newRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleTweakTarget(bool tweakTargetValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Collider[] GetPartColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTargetDetach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTargetDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTargetReattach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onEditorUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnConstructionModeUpdate()
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
	public override void OnPartCreatedFomInventory(ModuleInventoryPart moduleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onPartFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onAttachUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 findTargetDirection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void lockEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LockEVAEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void unlockEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnLockEVAEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void schedule_raycast()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool raycastTarget(Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SetTarget(Part tgt, string tgtColName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateWorldValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DumpTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLink()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnsetLink()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PreviewAttachment(Vector3 rDir, Vector3 rPos, Quaternion rRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EndPreview()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Quaternion getAnchorRot(Vector3 rDir, Quaternion defaultRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorEvent(ConstructionEventType evt, Part selPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTargetCoords()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode ParseCustomPartData(string customPartData)
	{
		throw null;
	}
}
