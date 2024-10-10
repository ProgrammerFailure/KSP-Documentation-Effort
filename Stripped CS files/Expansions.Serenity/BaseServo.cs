using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ns9;
using UnityEngine;

namespace Expansions.Serenity;

public abstract class BaseServo : PartModule, IRoboticServo, IResourceConsumer, IPartCostModifier, IPartMassModifier, IAxisFieldLimits, IJointLockState, IConstruction
{
	public enum ResourceConsumptionTypes
	{
		VelocityLimit,
		CurrentVelocity
	}

	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8002354")]
	[UI_Toggle(disabledText = "#autoLOC_439840", scene = UI_Scene.All, enabledText = "#autoLOC_439839", affectSymCounterparts = UI_Scene.All)]
	public bool servoIsLocked;

	[UI_Toggle(disabledText = "#autoLOC_439840", scene = UI_Scene.Editor, enabledText = "#autoLOC_439839", affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8005423")]
	public bool servoIsMotorized = true;

	[KSPField]
	public bool hideUIServoIsMotorized;

	[KSPField]
	public string jointParentName = "JointParent";

	public Transform jointParent;

	[KSPField(isPersistant = true)]
	public Quaternion jointParentRotation;

	public bool jointParentRotationLoaded;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_7000070")]
	[UI_Toggle(disabledText = "#autoLOC_8005431", scene = UI_Scene.All, enabledText = "#autoLOC_8005430", affectSymCounterparts = UI_Scene.All)]
	public bool servoMotorIsEngaged = true;

	[KSPField(isPersistant = true, guiActive = false)]
	public float launchPosition;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8002339")]
	public string servoCurrentTorque;

	[UI_FloatRange(scene = UI_Scene.Editor, stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8002340")]
	public float servoMotorSize = 100f;

	[KSPField]
	public bool hideUIServoMotorSize;

	[KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, maxValue = 100f, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8003237")]
	[UI_FloatRange(scene = UI_Scene.Flight, stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float servoMotorLimit = 100f;

	[KSPField]
	public bool hideUIServoMotorLimit;

	[KSPField(advancedTweakable = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_7000070")]
	[UI_Label(scene = UI_Scene.Editor)]
	public string motorOutputInformation = "";

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8005429")]
	[UI_Label(scene = UI_Scene.Flight)]
	public string resourceConsumption = "0.0";

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float maxMotorOutput = 100f;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_7000070")]
	public string motorState = "";

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005438")]
	[UI_Toggle(disabledText = "#autoLOC_8005439", scene = UI_Scene.All, enabledText = "#autoLOC_8002354", affectSymCounterparts = UI_Scene.All)]
	public bool lockPartOnPowerLoss = true;

	[KSPField]
	public string servoName = "Servo_0";

	[KSPField]
	public Vector3 servoCoMOffset = Vector3.zero;

	[KSPField]
	public string servoTransformName = "";

	[KSPField]
	public string baseTransformName = "";

	[KSPField]
	public string servoAttachNodes = "";

	[KSPField]
	public string servoSrfMeshNames = "";

	[KSPField]
	public string mainAxis = "Z";

	[KSPField]
	public bool useLimits;

	[KSPField]
	public Vector2 hardMinMaxLimits = new Vector2(0f, 180f);

	[KSPField]
	public Vector2 traverseVelocityLimits = new Vector2(0.1f, 10f);

	[KSPField]
	public float servoMass = 0.1f;

	[KSPField(isPersistant = true)]
	public Vector3 servoTransformPosition;

	[KSPField(isPersistant = true)]
	public Quaternion servoTransformRotation;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float efficiency = 1f;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float motorizedMassPerKN = 0.1f;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float motorizedCostPerDriveUnit = 1f;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float connectedMassScale = 1f;

	[KSPField]
	public float baseResourceConsumptionRate = 0.001f;

	[KSPField(isPersistant = true)]
	public bool useMultipleDragCubes;

	[KSPField]
	public float referenceConsumptionVelocity = 90f;

	public bool hasEnoughResources;

	public bool servoIsBraking;

	public bool motorManualDisengaged;

	public bool servoInitComplete;

	[SerializeField]
	public ConfigurableJoint servoJoint;

	[SerializeField]
	public Vector3 axis;

	[SerializeField]
	public Vector3 secAxis;

	[SerializeField]
	public Vector3 pivot;

	[SerializeField]
	public GameObject basePartObject;

	[SerializeField]
	public GameObject movingPartObject;

	[SerializeField]
	public Matrix4x4 servoParentTransform;

	[SerializeField]
	public Matrix4x4 servoParentTransformInverse;

	[SerializeField]
	public Rigidbody movingPartRB;

	[SerializeField]
	public List<AttachNode> attachNodes;

	[SerializeField]
	public Quaternion cachedStartingRotation;

	[SerializeField]
	public float cachedStartingRotationOffset;

	[SerializeField]
	public string[] servoSrfMeshes;

	[SerializeField]
	public Quaternion targetRotation;

	public float lockAngle;

	public bool servoTransformPosLoaded;

	public bool servoTransformRotLoaded;

	public double rate;

	public bool prevServoMotorIsEngaged;

	public bool prevServoIsMotorized;

	public bool prevServoIsLocked;

	public bool partActionMenuOpen;

	public bool wasMoving;

	public double timeStoppedMoving;

	public bool stoppedMovingTimerOn;

	public string OutputUnit;

	public float motorOutput;

	public float driveUnit;

	public float currentVelocityLimit;

	public UI_FloatRange servoMotorSizeField;

	public UI_FloatRange servoMotorLimitField;

	public float previousDisplacement;

	public float currentFrameVelocity;

	public float transformRateOfMotion;

	public int velocityAverageReadings = 20;

	public float[] velocityReadings;

	public int velocityReadingIndex;

	public float velocityReadingsSum;

	public HashSet<uint> upstreamParts;

	public HashSet<uint> downstreamParts;

	public List<CompoundPart> spanningParts;

	public bool partIsPhysicsLess;

	public List<PartResourceDefinition> consumedResources;

	[KSPField]
	public ResourceConsumptionTypes resourceConsumptionMode = ResourceConsumptionTypes.CurrentVelocity;

	public DictionaryValueList<string, AxisFieldLimit> axisFieldLimits;

	public static string cacheAutoLOC_6013039;

	public static string cacheAutoLOC_6013040;

	public static string cacheAutoLOC_8002350;

	public static string cacheAutoLOC_8002351;

	public static string cacheAutoLOC_8002352;

	public PartJoint pJoint { get; set; }

	public bool HasEnoughResources => hasEnoughResources;

	public bool ServoInitComplete => servoInitComplete;

	public ConfigurableJoint DebugServoJoint => servoJoint;

	public float CurrentVelocityLimit => currentVelocityLimit;

	public Callback<AxisFieldLimit> LimitsChanged { get; set; }

	public BaseServo()
	{
	}

	[KSPAction("#autoLOC_8002355", advancedTweakable = true)]
	public void ToggleServoLockedAction(KSPActionParam param)
	{
		if (servoIsLocked)
		{
			DisengageServoLock();
		}
		else if (!EngageServoLock())
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003335"), 5f);
		}
	}

	[KSPAction("#autoLOC_8003333", advancedTweakable = true)]
	public void ServoEngageLockAction(KSPActionParam param)
	{
		if (!EngageServoLock())
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003335"), 5f);
		}
	}

	[KSPAction("#autoLOC_8003334", advancedTweakable = true)]
	public void ServoDisgageLockAction(KSPActionParam param)
	{
		DisengageServoLock();
	}

	[KSPAction("#autoLOC_8005436")]
	public void ToggleMotorAction(KSPActionParam param)
	{
		if (servoMotorIsEngaged)
		{
			DisengageMotor();
		}
		else
		{
			EngageMotor();
		}
	}

	[KSPAction("#autoLOC_8003336")]
	public void MotorOnAction(KSPActionParam param)
	{
		EngageMotor();
	}

	[KSPAction("#autoLOC_8003337")]
	public void MotorOffAction(KSPActionParam param)
	{
		DisengageMotor();
	}

	[KSPAction]
	public void ResetPosition(KSPActionParam param)
	{
		ResetLaunchPosition();
	}

	[KSPEvent(guiActiveUncommand = true, active = true, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = false)]
	public void ResetPosition()
	{
		ResetLaunchPosition();
	}

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public abstract void OnJointInit(bool goodSetup);

	public abstract void OnPostStartJointInit();

	public abstract void OnPreModifyServo();

	public abstract void OnModifyServo();

	public abstract void OnVisualizeServo(bool rotateBase);

	public abstract void OnServoLockApplied();

	public abstract void OnServoLockRemoved();

	public abstract void OnServoMotorEngaged();

	public abstract void OnServoMotorDisengaged();

	public virtual double CalculatePower()
	{
		if (!ServoInitComplete)
		{
			return 0.0;
		}
		double result = 0.0;
		double num = 0.0;
		num = resourceConsumptionMode switch
		{
			ResourceConsumptionTypes.VelocityLimit => currentVelocityLimit / referenceConsumptionVelocity, 
			_ => transformRateOfMotion / referenceConsumptionVelocity, 
		};
		if (servoMotorIsEngaged && IsMoving())
		{
			result = (double)(baseResourceConsumptionRate * motorOutput) * num;
		}
		return result;
	}

	public abstract float GetFrameDisplacement();

	public abstract void SetInitialDisplacement();

	public void CalculateAverageRateOfMovement()
	{
		currentFrameVelocity = GetFrameDisplacement() / Time.fixedDeltaTime;
		velocityReadingsSum -= velocityReadings[velocityReadingIndex];
		velocityReadings[velocityReadingIndex] = currentFrameVelocity;
		velocityReadingsSum += velocityReadings[velocityReadingIndex];
		velocityReadingIndex = (velocityReadingIndex + 1) % velocityAverageReadings;
		transformRateOfMotion = velocityReadingsSum / (float)velocityAverageReadings;
	}

	public abstract bool IsMoving();

	public virtual void CalculateResourceDrain()
	{
		if (!HighLogic.LoadedSceneIsEditor)
		{
			rate = CalculatePower() / (double)efficiency;
			hasEnoughResources = true;
			if (Math.Abs(rate) < 0.001)
			{
				rate = 0.0;
			}
			double rateMultiplier;
			double rateMultiplier2;
			if (rate >= 0.0)
			{
				rateMultiplier = rate;
				rateMultiplier2 = 0.0;
			}
			else
			{
				rateMultiplier = 0.0;
				rateMultiplier2 = 0.0 - rate;
			}
			hasEnoughResources = resHandler.UpdateModuleResourceInputs(ref motorState, useFlowMode: true, rateMultiplier, 0.999, returnOnFirstLack: true, stringOps: false);
			resHandler.UpdateModuleResourceOutputs(rateMultiplier2);
			if (!hasEnoughResources && servoMotorIsEngaged)
			{
				servoMotorIsEngaged = false;
				rate = 0.0;
				OnModifyServo();
			}
		}
	}

	public new abstract void OnFixedUpdate();

	public virtual void OnDestroy()
	{
		GameEvents.onVesselWasModified.Remove(OnVesselModified);
		GameEvents.onAboutToSaveShip.Remove(OnSaveShip);
		GameEvents.onPartActionUIShown.Remove(OnPartMenuOpen);
		GameEvents.onPartActionUIDismiss.Remove(OnPartMenuClose);
		GameEvents.onEditorPartPlaced.Remove(OnEditorPartPlaced);
		GameEvents.onEditorPartPicked.Remove(OnEditorPartPicked);
		GameEvents.onEditorCompoundPartLinked.Remove(OnEditorCompoundPartLinked);
		base.Fields["servoMotorSize"].OnValueModified -= ModifyServo;
		base.Fields["servoMotorLimit"].OnValueModified -= ModifyServo;
		base.Fields["servoMotorIsEngaged"].OnValueModified -= ManuallyModifyEngaged;
		base.Fields["servoIsLocked"].OnValueModified -= ModifyLocked;
	}

	public virtual void OnSaveShip(ShipConstruct ship)
	{
		if (ServoInitComplete)
		{
			ApplyCoordsUpdate();
			base.part.UpdateAttachNodes();
			base.part.UpdateSrfAttachNode();
		}
	}

	[ContextMenu("Apply Coords Update")]
	public virtual void ApplyCoordsUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && !servoIsLocked && base.vessel != null && base.part.vessel != null)
		{
			RecurseCoordUpdate(base.part, base.part.vessel.rootPart);
			RecurseAttachNodeUpdate(base.part);
		}
		else if (HighLogic.LoadedSceneIsEditor && EditorLogic.fetch.ship != null && EditorLogic.fetch.ship.parts.Count > 0 && EditorLogic.fetch.ship.parts.Contains(base.part))
		{
			RecurseCoordUpdate(base.part, EditorLogic.fetch.ship.parts[0]);
			RecurseAttachNodeUpdate(base.part);
		}
	}

	public virtual void RecurseCoordUpdate(Part p, Part rootPart)
	{
		if (!(p == null) && !(rootPart == null))
		{
			p.UpdateOrgPosAndRot(rootPart);
			for (int i = 0; i < p.children.Count; i++)
			{
				RecurseCoordUpdate(p.children[i], rootPart);
			}
		}
	}

	public virtual void RecurseAttachNodeUpdate(Part p)
	{
		if (!ServoInitComplete || p == null)
		{
			return;
		}
		BaseServo servo = null;
		if (p.isBaseServo(out servo))
		{
			for (int i = 0; i < servo.attachNodes.Count; i++)
			{
				Vector3 position = p.transform.InverseTransformPoint(servo.attachNodes[i].nodeTransform.position);
				Vector3 orientation = p.transform.InverseTransformDirection(servo.attachNodes[i].nodeTransform.forward);
				servo.attachNodes[i].position = position;
				servo.attachNodes[i].orientation = orientation;
			}
		}
		for (int j = 0; j < p.children.Count; j++)
		{
			RecurseAttachNodeUpdate(p.children[j]);
		}
	}

	public virtual void SetLaunchPosition(float val)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			launchPosition = val;
		}
	}

	public abstract void ResetLaunchPosition();

	public override void OnAwake()
	{
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = resHandler.inputResources.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
		}
		axisFieldLimits = new DictionaryValueList<string, AxisFieldLimit>();
		InitAxisFieldLimits();
		velocityReadings = new float[velocityAverageReadings];
		velocityReadingIndex = 0;
		base.OnAwake();
	}

	public override void OnLoad(ConfigNode node)
	{
		servoTransformRotLoaded = false;
		servoTransformPosLoaded = false;
		if (node.HasValue("servoTransformPosition"))
		{
			servoTransformPosLoaded = true;
		}
		if (node.HasValue("servoTransformRotation"))
		{
			servoTransformRotLoaded = true;
		}
		if (node.HasValue("jointParentRotation"))
		{
			jointParentRotationLoaded = true;
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (!servoInitComplete || ((!HighLogic.LoadedSceneIsFlight || !(base.vessel != null) || !base.vessel.loaded) && !HighLogic.LoadedSceneIsEditor))
		{
			return;
		}
		ApplyCoordsUpdate();
		if (servoInitComplete && movingPartObject != null)
		{
			if (!servoIsLocked || HighLogic.LoadedSceneIsEditor)
			{
				servoTransformPosition = movingPartObject.transform.localPosition;
				servoTransformRotation = movingPartObject.transform.localRotation;
			}
			node.SetValue("servoTransformPosition", servoTransformPosition);
			node.SetValue("servoTransformRotation", servoTransformRotation);
		}
		if (jointParent != null)
		{
			node.SetValue("jointParentRotation", jointParent.localRotation);
		}
	}

	public override void OnStart(StartState state)
	{
		OutputUnit = cacheAutoLOC_6013039;
		if (!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
		prevServoMotorIsEngaged = !servoMotorIsEngaged;
		prevServoIsMotorized = !servoIsMotorized;
		if (!string.IsNullOrEmpty(servoTransformName))
		{
			movingPartObject = base.gameObject.GetChild(servoTransformName);
			if (movingPartObject != null)
			{
				if (!servoTransformPosLoaded)
				{
					servoTransformPosition = movingPartObject.transform.localPosition;
				}
				if (!servoTransformRotLoaded)
				{
					servoTransformRotation = movingPartObject.transform.localRotation;
				}
			}
		}
		if (!string.IsNullOrEmpty(baseTransformName))
		{
			basePartObject = base.gameObject.GetChild(baseTransformName);
		}
		if (!string.IsNullOrEmpty(jointParentName))
		{
			jointParent = base.part.FindModelTransform(jointParentName);
		}
		if (jointParent == null)
		{
			jointParent = movingPartObject.transform.parent;
		}
		if (jointParent != null)
		{
			if (jointParentRotationLoaded)
			{
				jointParent.localRotation = jointParentRotation;
			}
			servoParentTransform = jointParent.localToWorldMatrix;
		}
		else
		{
			servoParentTransform = movingPartObject.transform.parent.localToWorldMatrix;
		}
		servoParentTransform = base.part.gameObject.transform.worldToLocalMatrix * servoParentTransform;
		servoParentTransformInverse = servoParentTransform.inverse;
		motorOutput = servoMotorSize * 0.01f * maxMotorOutput;
		if (HighLogic.LoadedSceneIsFlight)
		{
			motorOutput *= servoMotorLimit * 0.01f;
		}
		if (attachNodes == null || attachNodes.Count == 0)
		{
			attachNodes = new List<AttachNode>();
		}
		if (!string.IsNullOrEmpty(servoAttachNodes))
		{
			string[] array = servoAttachNodes.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				AttachNode attachNode = base.part.FindAttachNode(array[i]);
				if (attachNode != null && !attachNodes.Contains(attachNode))
				{
					attachNodes.AddUnique(attachNode);
					GameObject gameObject = new GameObject("AttachNode_" + attachNode.id);
					gameObject.transform.position = base.part.transform.TransformPoint(attachNode.originalPosition);
					gameObject.transform.forward = base.part.transform.TransformDirection(attachNode.originalOrientation);
					attachNode.nodeTransform = gameObject.transform;
				}
			}
		}
		if (!string.IsNullOrEmpty(servoSrfMeshNames))
		{
			servoSrfMeshes = servoSrfMeshNames.Split(',');
		}
		if (movingPartObject != null)
		{
			for (int j = 0; j < attachNodes.Count; j++)
			{
				if (attachNodes[j].nodeTransform.parent == null)
				{
					attachNodes[j].nodeTransform.SetParent(movingPartObject.transform);
					attachNodes[j].nodeTransform.localScale = Vector3.one;
				}
			}
		}
		GameEvents.onVesselWasModified.Add(OnVesselModified);
		GameEvents.onAboutToSaveShip.Add(OnSaveShip);
		GameEvents.onPartActionUIShown.Add(OnPartMenuOpen);
		GameEvents.onPartActionUIDismiss.Add(OnPartMenuClose);
		if (HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onEditorPartPlaced.Add(OnEditorPartPlaced);
			GameEvents.onEditorPartPicked.Add(OnEditorPartPicked);
			GameEvents.onEditorCompoundPartLinked.Add(OnEditorCompoundPartLinked);
			upstreamParts = new HashSet<uint>();
			downstreamParts = new HashSet<uint>();
			spanningParts = new List<CompoundPart>();
			BuildPartSets();
		}
		InitJoint();
		if (HighLogic.LoadedSceneIsFlight && !servoIsMotorized && base.Fields["servoMotorIsEngaged"] != null)
		{
			base.Fields["servoMotorIsEngaged"].guiActive = false;
			servoMotorIsEngaged = false;
			prevServoMotorIsEngaged = true;
		}
		if (base.Fields["servoMotorIsEngaged"] != null && !(bool)base.Fields["servoMotorIsEngaged"].originalValue)
		{
			motorManualDisengaged = true;
		}
		if (base.Fields["servoMotorSize"] != null)
		{
			base.Fields.TryGetFieldUIControl<UI_FloatRange>("servoMotorSize", out servoMotorSizeField);
			if (HighLogic.LoadedSceneIsFlight)
			{
				base.Fields["servoMotorSize"].guiActive = false;
			}
			else
			{
				base.Fields["servoMotorSize"].guiActiveEditor = !hideUIServoMotorSize;
			}
		}
		if (base.Fields["servoMotorLimit"] != null && base.Fields.TryGetFieldUIControl<UI_FloatRange>("servoMotorLimit", out servoMotorLimitField))
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				base.Fields["servoMotorLimit"].guiActive = false;
			}
			else
			{
				base.Fields["servoMotorLimit"].guiActive = !hideUIServoMotorLimit;
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			UpdatePAWUI(UI_Scene.Flight);
		}
		base.Fields["servoIsMotorized"].guiActiveEditor = HighLogic.LoadedSceneIsEditor && !hideUIServoIsMotorized;
		base.Fields["servoMotorSize"].OnValueModified += ModifyServo;
		base.Fields["servoMotorLimit"].OnValueModified += ModifyServo;
		base.Fields["servoMotorIsEngaged"].OnValueModified += ManuallyModifyEngaged;
		base.Fields["servoIsLocked"].OnValueModified += ModifyLocked;
		if (HighLogic.LoadedSceneIsEditor)
		{
			StartCoroutine(WaitAndVisualizeServo());
		}
		CalculateResourceDrain();
	}

	public void RecurseBuildPartSets(Part p, HashSet<uint> partSet)
	{
		if (p == base.part)
		{
			partSet = downstreamParts;
		}
		else
		{
			if (p is CompoundPart)
			{
				return;
			}
			partSet.Add(p.craftID);
		}
		int count = p.children.Count;
		while (count-- > 0)
		{
			RecurseBuildPartSets(p.children[count], partSet);
		}
	}

	public void RecurseRemovePartSets(Part p, HashSet<uint> partSet)
	{
		if (p == base.part)
		{
			partSet = downstreamParts;
		}
		else
		{
			if (p is CompoundPart)
			{
				return;
			}
			partSet.Remove(p.craftID);
		}
		int count = p.children.Count;
		while (count-- > 0)
		{
			RecurseRemovePartSets(p.children[count], partSet);
		}
	}

	public void BuildPartSets()
	{
		upstreamParts.Clear();
		downstreamParts.Clear();
		spanningParts.Clear();
		RecurseBuildPartSets(base.part.localRoot, upstreamParts);
	}

	public IEnumerator WaitAndVisualizeServo()
	{
		yield return null;
		servoInitComplete = true;
		SetDriveUnit();
		VisualizeServo(moveChildren: false);
	}

	public override void OnStartBeforePartAttachJoint(StartState modStartState)
	{
		if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor)
		{
			if (movingPartRB != null && movingPartRB.isKinematic)
			{
				movingPartRB.isKinematic = false;
				partIsPhysicsLess = false;
			}
			if (modStartState != StartState.Editor)
			{
				PostStartInitJoint();
			}
			SetInitialDisplacement();
			servoInitComplete = true;
			ApplyCoordsUpdate();
		}
	}

	public override void PromoteToPhysicalPart()
	{
		if (servoJoint == null)
		{
			InitJoint();
		}
	}

	public override void DemoteToPhysicslessPart()
	{
		if (movingPartRB != null)
		{
			movingPartRB.isKinematic = true;
		}
		partIsPhysicsLess = true;
	}

	public override string GetInfo()
	{
		SetDriveUnit();
		string text = "";
		text = text + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format("#autoLOC_8002363") + "</color>";
		text = text + Localizer.Format("#autoLOC_8002367", base.part.partInfo.cost, base.part.partInfo.cost + motorizedCostPerDriveUnit * driveUnit) + "\n";
		text = text + Localizer.Format("#autoLOC_8002368") + "\n\n";
		text = text + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format("#autoLOC_8002364") + "</color>";
		text = text + Localizer.Format("#autoLOC_8002369", base.part.partInfo.partPrefab.mass + motorizedMassPerKN * driveUnit) + "\n";
		text += Localizer.Format("#autoLOC_8002370");
		text += "\n";
		text += resHandler.PrintModuleResources(showFlowModeDesc: true, baseResourceConsumptionRate * (servoMotorSize * 0.01f) * maxMotorOutput / efficiency);
		return text + "\n";
	}

	public override void OnCopy(PartModule fromModule)
	{
		for (int i = 0; i < attachNodes.Count; i++)
		{
			Transform nodeTransform = attachNodes[i].nodeTransform;
			AttachNode attachNode = base.part.FindAttachNode(attachNodes[i].id);
			if (attachNode != null)
			{
				attachNodes[i] = attachNode;
				attachNodes[i].nodeTransform = nodeTransform;
			}
		}
		RecurseAttachNodeUpdate(base.part);
	}

	public void Start()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity") && HighLogic.LoadedSceneIsGame)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void FixedUpdate()
	{
		if (!ServoInitComplete || partIsPhysicsLess)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsFlight && !FlightDriver.Pause)
		{
			CalculateAverageRateOfMovement();
			if (servoIsMotorized)
			{
				if (servoMotorIsEngaged && !servoIsBraking && !servoIsLocked)
				{
					CalculateResourceDrain();
				}
				else if (!servoMotorIsEngaged && resHandler.HasEnoughResourcesToAutoStart() && !motorManualDisengaged)
				{
					hasEnoughResources = true;
					if (lockPartOnPowerLoss && servoIsLocked)
					{
						DisengageServoLock();
					}
					if (servoIsBraking)
					{
						servoIsBraking = false;
					}
					EngageMotor();
					motorManualDisengaged = false;
				}
			}
		}
		OnFixedUpdate();
		if (HighLogic.LoadedSceneIsFlight && !FlightDriver.Pause)
		{
			RecurseAttachNodeUpdate(base.part);
		}
	}

	public virtual void Update()
	{
		if (partIsPhysicsLess)
		{
			return;
		}
		if (partActionMenuOpen)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				UpdatePAWUI(UI_Scene.Editor);
			}
			else
			{
				UpdatePAWUI(UI_Scene.Flight);
			}
		}
		else
		{
			prevServoIsLocked = servoIsLocked;
		}
		if (!HighLogic.LoadedSceneIsFlight || (!IsMoving() && !wasMoving))
		{
			return;
		}
		ApplyCoordsUpdate();
		wasMoving = true;
		if (!IsMoving())
		{
			if (stoppedMovingTimerOn)
			{
				if (Planetarium.GetUniversalTime() - timeStoppedMoving > 3.0)
				{
					stoppedMovingTimerOn = false;
					wasMoving = false;
				}
			}
			else
			{
				stoppedMovingTimerOn = true;
				timeStoppedMoving = Planetarium.GetUniversalTime();
			}
		}
		else
		{
			stoppedMovingTimerOn = false;
		}
	}

	public bool EngageServoLock()
	{
		if (servoIsLocked)
		{
			return true;
		}
		if (!servoIsLocked && !IsMoving())
		{
			base.Fields["servoIsLocked"].SetValue(true, this);
			ModifyLocked(null);
			return true;
		}
		return false;
	}

	public void DisengageServoLock()
	{
		if (servoIsLocked)
		{
			base.Fields["servoIsLocked"].SetValue(false, this);
			ModifyLocked(null);
		}
	}

	public void EngageMotor()
	{
		if (!servoMotorIsEngaged)
		{
			base.Fields["servoMotorIsEngaged"].SetValue(true, this);
			ModifyServo(null);
		}
	}

	public void DisengageMotor()
	{
		if (servoMotorIsEngaged)
		{
			base.Fields["servoMotorIsEngaged"].SetValue(false, this);
			ModifyServo(null);
		}
	}

	public void OnPartMenuOpen(UIPartActionWindow window, Part inpPart)
	{
		if (inpPart.persistentId == base.part.persistentId)
		{
			partActionMenuOpen = true;
			if (HighLogic.LoadedSceneIsEditor)
			{
				UpdatePAWUI(UI_Scene.Editor);
			}
			else
			{
				UpdatePAWUI(UI_Scene.Flight);
			}
		}
	}

	public void OnPartMenuClose(Part inpPart)
	{
		if (inpPart.persistentId == base.part.persistentId)
		{
			partActionMenuOpen = false;
		}
	}

	public void OnEditorPartPlaced(Part placedPart)
	{
		if (placedPart == null || placedPart.parent == null)
		{
			return;
		}
		if (placedPart == base.part)
		{
			RecurseBuildPartSets(placedPart.localRoot, upstreamParts);
		}
		else if (!(placedPart is CompoundPart))
		{
			uint craftID = placedPart.parent.craftID;
			if (upstreamParts.Contains(craftID))
			{
				RecurseBuildPartSets(placedPart, upstreamParts);
			}
			else
			{
				RecurseBuildPartSets(placedPart, downstreamParts);
			}
		}
	}

	public void OnEditorPartPicked(Part pickedPart)
	{
		if (pickedPart == base.part)
		{
			upstreamParts.Clear();
		}
		else if (pickedPart is CompoundPart)
		{
			spanningParts.Remove(pickedPart as CompoundPart);
		}
		else if (upstreamParts.Contains(pickedPart.craftID))
		{
			RecurseRemovePartSets(pickedPart, upstreamParts);
		}
		else
		{
			RecurseRemovePartSets(pickedPart, downstreamParts);
		}
	}

	public void OnEditorCompoundPartLinked(CompoundPart linkedPart)
	{
		if (!(linkedPart != null))
		{
			return;
		}
		if (linkedPart.target != null && linkedPart.parent != null)
		{
			uint craftID = linkedPart.parent.craftID;
			uint craftID2 = linkedPart.target.craftID;
			if (linkedPart.parent == base.part || linkedPart.target == base.part || (upstreamParts.Contains(craftID) && downstreamParts.Contains(craftID2)) || (upstreamParts.Contains(craftID2) && downstreamParts.Contains(craftID)))
			{
				spanningParts.AddUnique(linkedPart);
			}
		}
		else
		{
			spanningParts.Remove(linkedPart);
		}
	}

	public void InitJoint()
	{
		if (base.part != null)
		{
			pJoint = base.part.attachJoint;
		}
		if (servoJoint == null && pJoint != null)
		{
			servoJoint = pJoint.Joint;
			if (servoJoint != null)
			{
				axis = servoJoint.axis;
				secAxis = servoJoint.secondaryAxis;
			}
		}
		if (pJoint != null)
		{
			if (pJoint.Host == pJoint.Child)
			{
				pivot = Part.PartToVesselSpacePos(pJoint.HostAnchor, pJoint.Child, pJoint.Child.vessel, PartSpaceMode.Pristine);
			}
			else
			{
				pivot = Part.PartToVesselSpacePos(pJoint.TgtAnchor, pJoint.Child, pJoint.Child.vessel, PartSpaceMode.Pristine);
			}
		}
		OnJointInit(goodSetup: true);
		if (movingPartRB != null)
		{
			RoboticCollisions roboticCollisions = movingPartRB.gameObject.AddComponent<RoboticCollisions>();
			if (roboticCollisions != null)
			{
				roboticCollisions.part = base.part;
			}
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			VisualizeServo(moveChildren: false);
		}
	}

	public void PostStartInitJoint()
	{
		if (pJoint == null && base.part != null)
		{
			pJoint = base.part.attachJoint;
		}
		if (servoJoint == null && pJoint != null)
		{
			servoJoint = pJoint.Joint;
			if (servoJoint != null)
			{
				axis = servoJoint.axis;
				secAxis = servoJoint.secondaryAxis;
			}
		}
		if (pJoint != null)
		{
			if (pJoint.Host == pJoint.Child)
			{
				pivot = Part.PartToVesselSpacePos(pJoint.HostAnchor, pJoint.Child, pJoint.Child.vessel, PartSpaceMode.Pristine);
			}
			else
			{
				pivot = Part.PartToVesselSpacePos(pJoint.TgtAnchor, pJoint.Child, pJoint.Child.vessel, PartSpaceMode.Pristine);
			}
		}
		servoJoint.connectedBody = base.part.Rigidbody;
		servoJoint.connectedMassScale = connectedMassScale;
		if (movingPartObject != null)
		{
			if (servoTransformPosLoaded)
			{
				movingPartObject.transform.localPosition = servoTransformPosition;
			}
			if (servoTransformRotLoaded)
			{
				movingPartObject.transform.localRotation = servoTransformRotation;
			}
			targetRotation = movingPartObject.transform.localRotation;
			cachedStartingRotation = Quaternion.identity;
			cachedStartingRotationOffset = 0f;
		}
		OnPostStartJointInit();
	}

	public void OnPartPack()
	{
		ApplyCoordsUpdate();
	}

	public void OnPartUnpack()
	{
		ApplyCoordsUpdate();
	}

	public void SetServoMass()
	{
		if (movingPartRB != null)
		{
			movingPartRB.mass = servoMass;
			movingPartRB.centerOfMass = servoCoMOffset;
		}
	}

	public void CreateServoRigidbody()
	{
		if (movingPartObject != null)
		{
			movingPartRB = movingPartObject.GetComponent<Rigidbody>();
			if (movingPartRB == null)
			{
				movingPartRB = movingPartObject.AddComponent<Rigidbody>();
			}
			base.part.servoRb = movingPartRB;
			movingPartRB.isKinematic = !HighLogic.LoadedSceneIsFlight || base.part.packed;
			movingPartRB.useGravity = false;
			movingPartRB.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
			SetServoMass();
		}
	}

	public void ModifyServo(object field)
	{
		OnPreModifyServo();
		SetDriveUnit();
		motorOutput = driveUnit;
		if (HighLogic.LoadedSceneIsFlight)
		{
			motorOutput *= servoMotorLimit * 0.01f;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			VisualizeServo(moveChildren: true);
		}
		else if (servoJoint != null)
		{
			OnModifyServo();
		}
	}

	public void SetDriveUnit()
	{
		driveUnit = servoMotorSize * 0.01f * maxMotorOutput;
	}

	public void ModifyLocked(object field)
	{
		bool flag = false;
		if (servoIsLocked && !prevServoIsLocked)
		{
			GameEvents.onRoboticPartLockChanging.Fire(base.part, servoIsLocked);
			flag = true;
			servoTransformPosition = movingPartObject.transform.localPosition;
			servoTransformRotation = movingPartObject.transform.localRotation;
			if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor)
			{
				RecurseCoordUpdate(base.part, HighLogic.LoadedSceneIsFlight ? base.part.vessel.rootPart : EditorLogic.fetch.ship.parts[0]);
			}
			RecurseAttachNodeUpdate(base.part);
			OnServoLockApplied();
		}
		else if (!servoIsLocked && prevServoIsLocked)
		{
			GameEvents.onRoboticPartLockChanging.Fire(base.part, servoIsLocked);
			flag = true;
			OnServoLockRemoved();
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (base.vessel != null)
			{
				base.vessel.CycleAllAutoStrut();
			}
			ModifyServo(field);
		}
		else if (HighLogic.LoadedSceneIsEditor && EditorLogic.fetch.ship != null)
		{
			for (int i = 0; i < EditorLogic.fetch.ship.parts.Count; i++)
			{
				EditorLogic.fetch.ship.parts[i].CycleAutoStrut();
			}
		}
		if (flag)
		{
			GameEvents.onRoboticPartLockChanged.Fire(base.part, servoIsLocked);
		}
	}

	public void ModifyEngaged(object field)
	{
		if (servoMotorIsEngaged && !prevServoMotorIsEngaged)
		{
			CalculateResourceDrain();
		}
		else if (!servoMotorIsEngaged && prevServoMotorIsEngaged)
		{
			OnServoMotorDisengaged();
		}
		ModifyServo(field);
	}

	public void ManuallyModifyEngaged(object field)
	{
		if (servoMotorIsEngaged && !prevServoMotorIsEngaged)
		{
			OnServoMotorEngaged();
			CalculateResourceDrain();
		}
		else if (!servoMotorIsEngaged && prevServoMotorIsEngaged)
		{
			OnServoMotorDisengaged();
		}
		ModifyServo(field);
	}

	public void OnVesselModified(Vessel v)
	{
		if (!(base.part == null) && base.part.State != PartStates.DEAD)
		{
			if (v == base.part.vessel)
			{
				ApplyCoordsUpdate();
			}
		}
		else
		{
			Debug.Log("[BaseServo]: host part is null, servoJoint must terminate.");
		}
	}

	public void SaveSpanningPartTargets()
	{
		if (spanningParts == null)
		{
			return;
		}
		int count = spanningParts.Count;
		while (count-- > 0)
		{
			if (spanningParts[count].target == null)
			{
				spanningParts.RemoveAt(count);
			}
			else
			{
				spanningParts[count].UpdateWorldValues();
			}
		}
	}

	public void RestoreSpanningPartTargets()
	{
		if (spanningParts != null)
		{
			int count = spanningParts.Count;
			while (count-- > 0)
			{
				spanningParts[count].UpdateTargetCoords();
			}
		}
	}

	public void VisualizeServo(bool moveChildren)
	{
		if (movingPartObject == null && !string.IsNullOrEmpty(servoTransformName))
		{
			movingPartObject = base.gameObject.GetChild(servoTransformName);
		}
		if (basePartObject == null && !string.IsNullOrEmpty(baseTransformName))
		{
			basePartObject = base.gameObject.GetChild(baseTransformName);
		}
		if (movingPartObject == null)
		{
			return;
		}
		bool rotateBase = false;
		if (basePartObject != null)
		{
			for (int i = 0; i < attachNodes.Count; i++)
			{
				if (attachNodes[i].attachedPart != null && attachNodes[i].attachedPart == base.part.parent)
				{
					rotateBase = true;
					break;
				}
			}
		}
		SaveSpanningPartTargets();
		if (moveChildren && base.part.children.Count > 0)
		{
			for (int j = 0; j < base.part.children.Count; j++)
			{
				SetChildParentTransform(base.part.children[j]);
			}
		}
		OnVisualizeServo(rotateBase);
		RestoreSpanningPartTargets();
		RecurseAttachNodeUpdate(base.part);
	}

	public void SetChildParentTransform(Part p)
	{
		int num = 0;
		while (true)
		{
			if (num < attachNodes.Count)
			{
				if (attachNodes[num].attachedPart != null && attachNodes[num].attachedPart == p && movingPartObject != null)
				{
					break;
				}
				num++;
				continue;
			}
			if (base.part.srfAttachNode.attachedPart != null && base.part.srfAttachNode.attachedPart == p && movingPartObject != null)
			{
				int num2 = 0;
				while (true)
				{
					if (num2 < servoSrfMeshes.Length)
					{
						if (servoSrfMeshes[num2] == base.part.srfAttachNode.srfAttachMeshName)
						{
							break;
						}
						num2++;
						continue;
					}
					p.transform.SetParent(base.part.transform);
					p.transform.localScale = Vector3.one;
					return;
				}
				p.transform.SetParent(movingPartObject.transform);
				p.transform.localScale = Vector3.one;
			}
			else if (p.srfAttachNode.attachedPart != null && p.srfAttachNode.attachedPart == base.part && movingPartObject != null)
			{
				int num3 = 0;
				while (true)
				{
					if (num3 < servoSrfMeshes.Length)
					{
						if (servoSrfMeshes[num3] == p.srfAttachNode.srfAttachMeshName)
						{
							break;
						}
						num3++;
						continue;
					}
					p.transform.SetParent(base.part.transform);
					p.transform.localScale = Vector3.one;
					return;
				}
				p.transform.SetParent(movingPartObject.transform);
				p.transform.localScale = Vector3.one;
			}
			else
			{
				p.transform.SetParent(base.part.transform);
				p.transform.localScale = Vector3.one;
			}
			return;
		}
		p.transform.SetParent(movingPartObject.transform);
		p.transform.localScale = Vector3.one;
	}

	public Vector3 GetMainAxis()
	{
		Vector3 zero = Vector3.zero;
		return mainAxis switch
		{
			"Z-" => Vector3.back, 
			"Y" => Vector3.up, 
			"Y-" => Vector3.down, 
			"X" => Vector3.right, 
			"X-" => Vector3.left, 
			_ => Vector3.forward, 
		};
	}

	public virtual void UpdatePAWUI(UI_Scene currentScene)
	{
		motorState = ((!servoIsMotorized || !servoMotorIsEngaged) ? cacheAutoLOC_8002352 : cacheAutoLOC_8002351);
		if (HighLogic.LoadedSceneIsEditor)
		{
			base.Fields["motorOutputInformation"].guiActiveEditor = servoIsMotorized && GameSettings.ADVANCED_TWEAKABLES;
			motorOutputInformation = Localizer.Format("#autoLOC_8002342", motorOutput.ToString("F1"), (base.part.mass - base.part.prefabMass).ToString("F2"));
			servoCurrentTorque = cacheAutoLOC_8002350;
			if (servoMotorSizeField != null)
			{
				servoMotorSizeField.SetSceneVisibility(currentScene, servoIsMotorized && !hideUIServoMotorSize);
			}
			if (servoMotorLimitField != null)
			{
				servoMotorLimitField.SetSceneVisibility(currentScene, state: false);
			}
			base.Fields["servoIsMotorized"].guiActiveEditor = !hideUIServoIsMotorized;
			base.Fields["servoMotorIsEngaged"].guiActiveEditor = servoIsMotorized;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			base.Fields["servoIsLocked"].guiInteractable = (servoIsLocked || !IsMoving()) && GameSettings.ADVANCED_TWEAKABLES;
			base.Fields["resourceConsumption"].guiActive = servoMotorIsEngaged && GameSettings.ADVANCED_TWEAKABLES;
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			for (int i = 0; i < resHandler.inputResources.Count; i++)
			{
				ModuleResource moduleResource = resHandler.inputResources[i];
				double amount = moduleResource.currentRequest / (double)TimeWarp.fixedDeltaTime;
				stringBuilder.Append("\n    ").Append(Localizer.Format(moduleResource.title)).Append(" ")
					.Append(KSPUtil.PrintSI(amount, cacheAutoLOC_6013040));
			}
			string text = KSPUtil.PrintSI(motorOutput * 1000f, OutputUnit);
			resourceConsumption = Localizer.Format("#autoLOC_8005432", text, stringBuilder.ToStringAndRelease());
			if (servoMotorSizeField != null)
			{
				servoMotorSizeField.SetSceneVisibility(currentScene, state: false);
			}
			if (servoMotorLimitField != null)
			{
				servoMotorLimitField.SetSceneVisibility(currentScene, servoIsMotorized && servoMotorIsEngaged && !hideUIServoMotorLimit);
			}
		}
	}

	public float StartingRotationOffset()
	{
		float num = 0f;
		num = mainAxis switch
		{
			"Y" => cachedStartingRotation.eulerAngles.y, 
			"X" => cachedStartingRotation.eulerAngles.x, 
			_ => cachedStartingRotation.eulerAngles.z, 
		};
		if (num > 180f)
		{
			num = 360f - num;
		}
		return num;
	}

	public float currentTransformAngle()
	{
		float num = 0f;
		if (movingPartObject == null)
		{
			return num;
		}
		switch (mainAxis)
		{
		case "Z":
			num = movingPartObject.transform.localEulerAngles.z;
			break;
		case "Y":
			num = movingPartObject.transform.localEulerAngles.y;
			break;
		case "X":
			num = movingPartObject.transform.localEulerAngles.x;
			break;
		}
		if (num > 180f)
		{
			num -= 360f;
		}
		return num;
	}

	public float currentTransformPosition()
	{
		float result = 0f;
		switch (mainAxis)
		{
		case "Z":
			result = movingPartObject.transform.localPosition.z;
			break;
		case "Y":
			result = movingPartObject.transform.localPosition.y;
			break;
		case "X":
			result = movingPartObject.transform.localPosition.x;
			break;
		}
		return result;
	}

	public float UpdateFieldLimits(BaseAxisField axisField, Vector2 newlimits, float currentValue, UI_FloatRange uiField = null)
	{
		axisField.minValue = newlimits.x;
		axisField.maxValue = newlimits.y;
		if (uiField != null)
		{
			uiField.minValue = newlimits.x;
			uiField.maxValue = newlimits.y;
		}
		return Mathf.Clamp(currentValue, newlimits.x, newlimits.y);
	}

	public Rigidbody AttachServoRigidBody(AttachNode node)
	{
		if (movingPartRB != null)
		{
			if (node.nodeType == AttachNode.NodeType.Surface && !string.IsNullOrEmpty(node.srfAttachMeshName))
			{
				for (int i = 0; i < servoSrfMeshes.Length; i++)
				{
					if (servoSrfMeshes[i] == node.srfAttachMeshName)
					{
						return movingPartRB;
					}
				}
			}
			for (int j = 0; j < attachNodes.Count; j++)
			{
				if (attachNodes[j].id == node.id)
				{
					return movingPartRB;
				}
			}
		}
		return base.part.Rigidbody;
	}

	public Rigidbody NodeRigidBody(AttachNode node)
	{
		if (movingPartRB != null)
		{
			for (int i = 0; i < attachNodes.Count; i++)
			{
				if (attachNodes[i] == node)
				{
					return movingPartRB;
				}
			}
		}
		return base.part.Rigidbody;
	}

	public bool ServoTransformCollider(string colName)
	{
		int num = 0;
		while (true)
		{
			if (num < servoSrfMeshes.Length)
			{
				if (servoSrfMeshes[num] == colName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public GameObject MovingObject()
	{
		return movingPartObject;
	}

	public GameObject BaseObject()
	{
		return basePartObject;
	}

	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		if (servoIsMotorized)
		{
			SetDriveUnit();
			return motorizedCostPerDriveUnit * driveUnit;
		}
		return 0f;
	}

	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		return ModifierChangeWhen.CONSTANTLY;
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		float result = 0f;
		if (servoIsMotorized)
		{
			result = motorizedMassPerKN * driveUnit;
		}
		return result;
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.CONSTANTLY;
	}

	public abstract void InitAxisFieldLimits();

	public void UpdateAxisFieldLimit(string fieldName, Vector2 hardLimits, Vector2 softLimits)
	{
		if (axisFieldLimits.ContainsKey(fieldName))
		{
			axisFieldLimits[fieldName].hardLimits = hardLimits;
			axisFieldLimits[fieldName].softLimits = softLimits;
		}
	}

	public abstract void UpdateAxisFieldHardLimit(string fieldName, Vector2 newlimits);

	public abstract void UpdateAxisFieldSoftLimit(string fieldName, Vector2 newlimits);

	public void SetHardLimits(string fieldName, Vector2 newLimits)
	{
		UpdateAxisFieldHardLimit(fieldName, newLimits);
	}

	public void SetSoftLimits(string fieldName, Vector2 newLimits)
	{
		if (axisFieldLimits.ContainsKey(fieldName))
		{
			Vector2 hardLimits = GetHardLimits(fieldName);
			newLimits.x = Mathf.Max(newLimits.x, hardLimits.x);
			newLimits.y = Mathf.Min(newLimits.y, hardLimits.y);
			UpdateAxisFieldSoftLimit(fieldName, newLimits);
		}
	}

	public bool HasAxisFieldLimits()
	{
		if (axisFieldLimits != null)
		{
			return axisFieldLimits.Count > 0;
		}
		return false;
	}

	public bool HasAxisFieldLimit(string fieldName)
	{
		if (axisFieldLimits != null && axisFieldLimits.Count > 0)
		{
			return axisFieldLimits.ContainsKey(fieldName);
		}
		return false;
	}

	public List<AxisFieldLimit> GetAxisFieldLimits()
	{
		if (axisFieldLimits == null)
		{
			return null;
		}
		return axisFieldLimits.ValuesList;
	}

	public AxisFieldLimit GetAxisFieldLimit(string fieldName)
	{
		if (axisFieldLimits == null)
		{
			return null;
		}
		if (axisFieldLimits.ContainsKey(fieldName))
		{
			return axisFieldLimits[fieldName];
		}
		return null;
	}

	public Vector2 GetHardLimits(string fieldName)
	{
		if (axisFieldLimits.ContainsKey(fieldName))
		{
			return GetAxisFieldLimit(fieldName).hardLimits;
		}
		return Vector2.one;
	}

	public Vector2 GetSoftLimits(string fieldName)
	{
		if (axisFieldLimits.ContainsKey(fieldName))
		{
			return GetAxisFieldLimit(fieldName).softLimits;
		}
		return Vector2.one;
	}

	public bool IsJointUnlocked()
	{
		return !servoIsLocked;
	}

	public virtual bool CanBeDetached()
	{
		return !IsMoving();
	}

	public virtual bool CanBeOffset()
	{
		return !IsMoving();
	}

	public virtual bool CanBeRotated()
	{
		return !IsMoving();
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6013040 = Localizer.Format("#autoLOC_6013040");
		cacheAutoLOC_6013039 = Localizer.Format("#autoLOC_6013039");
		cacheAutoLOC_8002350 = Localizer.Format("#autoLOC_8002350");
		cacheAutoLOC_8002351 = Localizer.Format("#autoLOC_8002351");
		cacheAutoLOC_8002352 = Localizer.Format("#autoLOC_8002352");
	}
}
