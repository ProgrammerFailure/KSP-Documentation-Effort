using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleAnimateGeneric : PartModule, IScalarModule, IMultipleDragCube
{
	public enum animationStates
	{
		LOCKED,
		MOVING,
		CLAMPED,
		FIXED
	}

	[KSPField]
	public bool instantAnimInEditor;

	[KSPField]
	public string animationName;

	[KSPField]
	public int CrewCapacity;

	[KSPField]
	public int layer;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
	public string status;

	[KSPField]
	public bool showStatus;

	[KSPField(isPersistant = true)]
	public animationStates aniState;

	[KSPField(isPersistant = true)]
	public bool animSwitch;

	[KSPField(isPersistant = true)]
	public float animTime;

	[KSPField(isPersistant = true)]
	public float animSpeed;

	private float revertAnimSpeedOnSave;

	[KSPField]
	public bool isOneShot;

	[KSPField]
	public bool actionAvailable;

	[KSPField]
	public bool eventAvailableEditor;

	[KSPField]
	public bool eventAvailableFlight;

	[KSPField]
	public bool eventAvailableEVA;

	[KSPField]
	public float evaDistance;

	[KSPField]
	public string startEventGUIName;

	[KSPField]
	public string endEventGUIName;

	[KSPField]
	public string actionGUIName;

	[KSPField]
	public KSPActionGroup defaultActionGroup;

	[KSPField]
	public bool allowManualControl;

	private bool originalAllowManualControl;

	[KSPField]
	public bool allowAnimationWhileShielded;

	[UI_FloatRange(minValue = 0f, stepIncrement = 1f, maxValue = 100f)]
	[KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001353")]
	public float deployPercent;

	private float revertDeployPercentOnSave;

	private bool originalDeployPercentGUIActive;

	[KSPField]
	public bool revClampDirection;

	[KSPField]
	public bool revClampSpeed;

	[KSPField]
	public bool revClampPercent;

	[KSPField]
	public bool allowDeployLimit;

	[KSPField]
	public string restrictedNode;

	[KSPField]
	public bool disableAfterPlaying;

	[KSPField(isPersistant = true)]
	public bool animationIsDisabled;

	[KSPField]
	public bool useMultipleDragCubes;

	private bool animationIsDisabledByVariant;

	private bool stopAnimation;

	private bool isManuallyMoving;

	protected Animation anim;

	private bool needsColliderFlagsReset;

	private float scalarLerpMaxDelta;

	private EventData<float, float> onMove;

	private EventData<float> onStop;

	private BaseEvent toggleEvt;

	private BaseAction toggleAct;

	private float lastClamp;

	[KSPField]
	public string moduleID;

	private static string cacheAutoLOC_215506;

	private static string cacheAutoLOC_215639;

	private static string cacheAutoLOC_215714;

	private static string cacheAutoLOC_215719;

	private static string cacheAutoLOC_215760;

	public float Progress
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool AnimationIsDisabledByVariant
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ScalarModuleID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float GetScalar
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CanMove
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float, float> OnMoving
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float> OnStop
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsMultipleCubesActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleAnimateGeneric()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001329", KSPActionGroup.REPLACEWITHDEFAULT)]
	public void ToggleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AnimationState GetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Animation GetAnimation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 5f, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001329")]
	public void Toggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateAnimSwitch()
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
	private void FindAnimation()
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
	private void CheckForRestrictions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AnimationHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScalar(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIRead(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIWrite(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsMoving()
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
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckCrewState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void VariantToggleAnimationDisabled(bool disabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void FailAnimationForCargoBay(float newDeployPercent, bool freezeMidMovement)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RestartFailedAnimationForCargoBay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
