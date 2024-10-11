using System.Runtime.CompilerServices;
using UnityEngine;

public class RetractableLadder : PartModule, IMultipleDragCube
{
	[KSPField(isPersistant = true)]
	public string StateName;

	[KSPField]
	public string ladderAnimationRootName;

	[KSPField]
	public string ladderColliderName;

	[KSPField]
	public string ladderRetractAnimationName;

	[KSPField]
	public float externalActivationRange;

	private Animation anim;

	private Collider ladder;

	private KerbalFSM ladderFSM;

	private KFSMState st_retracted;

	private KFSMState st_extended;

	private KFSMState st_retracting;

	private KFSMState st_extending;

	private KFSMEvent On_retractStart;

	private KFSMEvent On_retractEnd;

	private KFSMEvent On_extendStart;

	private KFSMEvent On_extendEnd;

	private KFSMEvent On_retractEditor;

	private KFSMEvent On_extendEditor;

	public bool IsMultipleCubesActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RetractableLadder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001410")]
	public void ToggleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001411")]
	public void ExtendAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001412")]
	public void RetractAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveEditor = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001411")]
	public void Extend()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveEditor = true, guiActiveUnfocused = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001412")]
	public void Retract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPartCreatedFomInventory(ModuleInventoryPart moduleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DemoteToPhysicslessPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDragState(float b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string[] GetDragCubeNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AssumeDragCubePosition(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool UsesProceduralDragCubes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetAnimationToRetracted()
	{
		throw null;
	}
}
