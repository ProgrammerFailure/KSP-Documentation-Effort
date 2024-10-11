using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

public abstract class BaseServo : PartModule, IRoboticServo, IResourceConsumer, IPartCostModifier, IPartMassModifier, IAxisFieldLimits, IJointLockState, IConstruction
{
	public enum ResourceConsumptionTypes
	{
		VelocityLimit,
		CurrentVelocity
	}

	[CompilerGenerated]
	private sealed class _003CWaitAndVisualizeServo_003Ed__139 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BaseServo _003C_003E4__this;

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
		public _003CWaitAndVisualizeServo_003Ed__139(int _003C_003E1__state)
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

	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8002354")]
	[UI_Toggle(disabledText = "#autoLOC_439840", scene = UI_Scene.All, enabledText = "#autoLOC_439839", affectSymCounterparts = UI_Scene.All)]
	public bool servoIsLocked;

	[UI_Toggle(disabledText = "#autoLOC_439840", scene = UI_Scene.Editor, enabledText = "#autoLOC_439839", affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8005423")]
	public bool servoIsMotorized;

	[KSPField]
	public bool hideUIServoIsMotorized;

	[KSPField]
	public string jointParentName;

	protected Transform jointParent;

	[KSPField(isPersistant = true)]
	public Quaternion jointParentRotation;

	private bool jointParentRotationLoaded;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_7000070")]
	[UI_Toggle(disabledText = "#autoLOC_8005431", scene = UI_Scene.All, enabledText = "#autoLOC_8005430", affectSymCounterparts = UI_Scene.All)]
	public bool servoMotorIsEngaged;

	[KSPField(isPersistant = true, guiActive = false)]
	public float launchPosition;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8002339")]
	public string servoCurrentTorque;

	[UI_FloatRange(scene = UI_Scene.Editor, stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8002340")]
	public float servoMotorSize;

	[KSPField]
	public bool hideUIServoMotorSize;

	[KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, maxValue = 100f, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8003237")]
	[UI_FloatRange(scene = UI_Scene.Flight, stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float servoMotorLimit;

	[KSPField]
	public bool hideUIServoMotorLimit;

	[KSPField(advancedTweakable = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_7000070")]
	[UI_Label(scene = UI_Scene.Editor)]
	public string motorOutputInformation;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8005429")]
	[UI_Label(scene = UI_Scene.Flight)]
	public string resourceConsumption;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float maxMotorOutput;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_7000070")]
	public string motorState;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005438")]
	[UI_Toggle(disabledText = "#autoLOC_8005439", scene = UI_Scene.All, enabledText = "#autoLOC_8002354", affectSymCounterparts = UI_Scene.All)]
	public bool lockPartOnPowerLoss;

	[KSPField]
	public string servoName;

	[KSPField]
	public Vector3 servoCoMOffset;

	[KSPField]
	public string servoTransformName;

	[KSPField]
	public string baseTransformName;

	[KSPField]
	public string servoAttachNodes;

	[KSPField]
	public string servoSrfMeshNames;

	[KSPField]
	public string mainAxis;

	[KSPField]
	public bool useLimits;

	[KSPField]
	public Vector2 hardMinMaxLimits;

	[KSPField]
	public Vector2 traverseVelocityLimits;

	[KSPField]
	public float servoMass;

	[KSPField(isPersistant = true)]
	public Vector3 servoTransformPosition;

	[KSPField(isPersistant = true)]
	public Quaternion servoTransformRotation;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float efficiency;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float motorizedMassPerKN;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float motorizedCostPerDriveUnit;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float connectedMassScale;

	[KSPField]
	public float baseResourceConsumptionRate;

	[KSPField(isPersistant = true)]
	public bool useMultipleDragCubes;

	[KSPField]
	public float referenceConsumptionVelocity;

	protected bool hasEnoughResources;

	protected bool servoIsBraking;

	protected bool motorManualDisengaged;

	private bool servoInitComplete;

	[SerializeField]
	protected ConfigurableJoint servoJoint;

	[SerializeField]
	protected Vector3 axis;

	[SerializeField]
	protected Vector3 secAxis;

	[SerializeField]
	protected Vector3 pivot;

	[SerializeField]
	protected GameObject basePartObject;

	[SerializeField]
	protected GameObject movingPartObject;

	[SerializeField]
	protected Matrix4x4 servoParentTransform;

	[SerializeField]
	protected Matrix4x4 servoParentTransformInverse;

	[SerializeField]
	protected Rigidbody movingPartRB;

	[SerializeField]
	protected List<AttachNode> attachNodes;

	[SerializeField]
	protected Quaternion cachedStartingRotation;

	[SerializeField]
	protected float cachedStartingRotationOffset;

	[SerializeField]
	protected string[] servoSrfMeshes;

	[SerializeField]
	protected Quaternion targetRotation;

	protected float lockAngle;

	private bool servoTransformPosLoaded;

	private bool servoTransformRotLoaded;

	protected double rate;

	protected bool prevServoMotorIsEngaged;

	protected bool prevServoIsMotorized;

	protected bool prevServoIsLocked;

	private bool partActionMenuOpen;

	private bool wasMoving;

	private double timeStoppedMoving;

	private bool stoppedMovingTimerOn;

	protected string OutputUnit;

	protected float motorOutput;

	protected float driveUnit;

	internal float currentVelocityLimit;

	protected UI_FloatRange servoMotorSizeField;

	protected UI_FloatRange servoMotorLimitField;

	internal float previousDisplacement;

	internal float currentFrameVelocity;

	internal float transformRateOfMotion;

	private int velocityAverageReadings;

	private float[] velocityReadings;

	private int velocityReadingIndex;

	private float velocityReadingsSum;

	private HashSet<uint> upstreamParts;

	private HashSet<uint> downstreamParts;

	private List<CompoundPart> spanningParts;

	protected bool partIsPhysicsLess;

	private List<PartResourceDefinition> consumedResources;

	[KSPField]
	public ResourceConsumptionTypes resourceConsumptionMode;

	protected DictionaryValueList<string, AxisFieldLimit> axisFieldLimits;

	private static string cacheAutoLOC_6013039;

	private static string cacheAutoLOC_6013040;

	private static string cacheAutoLOC_8002350;

	private static string cacheAutoLOC_8002351;

	private static string cacheAutoLOC_8002352;

	public PartJoint pJoint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public bool HasEnoughResources
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool ServoInitComplete
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal ConfigurableJoint DebugServoJoint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float CurrentVelocityLimit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Callback<AxisFieldLimit> LimitsChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected BaseServo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8002355", advancedTweakable = true)]
	protected void ToggleServoLockedAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003333", advancedTweakable = true)]
	protected void ServoEngageLockAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003334", advancedTweakable = true)]
	protected void ServoDisgageLockAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8005436")]
	protected void ToggleMotorAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003336")]
	protected void MotorOnAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003337")]
	protected void MotorOffAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction]
	protected void ResetPosition(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUncommand = true, active = true, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = false)]
	protected void ResetPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	protected abstract void OnJointInit(bool goodSetup);

	protected abstract void OnPostStartJointInit();

	public abstract void OnPreModifyServo();

	public abstract void OnModifyServo();

	protected abstract void OnVisualizeServo(bool rotateBase);

	protected abstract void OnServoLockApplied();

	protected abstract void OnServoLockRemoved();

	protected abstract void OnServoMotorEngaged();

	protected abstract void OnServoMotorDisengaged();

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculatePower()
	{
		throw null;
	}

	protected abstract float GetFrameDisplacement();

	protected abstract void SetInitialDisplacement();

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CalculateAverageRateOfMovement()
	{
		throw null;
	}

	protected abstract bool IsMoving();

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CalculateResourceDrain()
	{
		throw null;
	}

	protected new abstract void OnFixedUpdate();

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSaveShip(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Apply Coords Update")]
	protected virtual void ApplyCoordsUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RecurseCoordUpdate(Part p, Part rootPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RecurseAttachNodeUpdate(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetLaunchPosition(float val)
	{
		throw null;
	}

	protected abstract void ResetLaunchPosition();

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
	private void RecurseBuildPartSets(Part p, HashSet<uint> partSet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RecurseRemovePartSets(Part p, HashSet<uint> partSet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildPartSets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitAndVisualizeServo_003Ed__139))]
	private IEnumerator WaitAndVisualizeServo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStartBeforePartAttachJoint(StartState modStartState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PromoteToPhysicalPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DemoteToPhysicslessPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnCopy(PartModule fromModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool EngageServoLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisengageServoLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EngageMotor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisengageMotor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartMenuOpen(UIPartActionWindow window, Part inpPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartMenuClose(Part inpPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEditorPartPlaced(Part placedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEditorPartPicked(Part pickedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEditorCompoundPartLinked(CompoundPart linkedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void InitJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void PostStartInitJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartPack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartUnpack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetServoMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateServoRigidbody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyServo(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDriveUnit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyLocked(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyEngaged(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ManuallyModifyEngaged(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SaveSpanningPartTargets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void RestoreSpanningPartTargets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void VisualizeServo(bool moveChildren)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetChildParentTransform(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetMainAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdatePAWUI(UI_Scene currentScene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float StartingRotationOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float currentTransformAngle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float currentTransformPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float UpdateFieldLimits(BaseAxisField axisField, Vector2 newlimits, float currentValue, UI_FloatRange uiField = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Rigidbody AttachServoRigidBody(AttachNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Rigidbody NodeRigidBody(AttachNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ServoTransformCollider(string colName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject MovingObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject BaseObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		throw null;
	}

	protected abstract void InitAxisFieldLimits();

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateAxisFieldLimit(string fieldName, Vector2 hardLimits, Vector2 softLimits)
	{
		throw null;
	}

	protected abstract void UpdateAxisFieldHardLimit(string fieldName, Vector2 newlimits);

	protected abstract void UpdateAxisFieldSoftLimit(string fieldName, Vector2 newlimits);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHardLimits(string fieldName, Vector2 newLimits)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSoftLimits(string fieldName, Vector2 newLimits)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasAxisFieldLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasAxisFieldLimit(string fieldName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<AxisFieldLimit> GetAxisFieldLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisFieldLimit GetAxisFieldLimit(string fieldName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetHardLimits(string fieldName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetSoftLimits(string fieldName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsJointUnlocked()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}
}
