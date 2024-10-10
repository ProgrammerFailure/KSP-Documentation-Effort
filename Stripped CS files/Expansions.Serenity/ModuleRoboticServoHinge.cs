using ns9;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRoboticServoHinge : BaseServo, IMultipleDragCube
{
	[UI_MinMaxRange(maxValueY = 177f, maxValueX = 176f, stepIncrement = 1f, affectSymCounterparts = UI_Scene.All, minValueY = 1f, minValueX = 0f)]
	[KSPField(guiFormat = "F1", isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8002344")]
	public Vector2 softMinMaxAngles = new Vector2(0f, 177f);

	[KSPField]
	public bool hideUISoftMinMaxAngles;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 30f, guiFormat = "F1", isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005420")]
	public float targetAngle;

	[KSPField]
	public float modelInitialAngle;

	[KSPField(isPersistant = true)]
	public bool mirrorRotation;

	public bool checkSymmetry;

	public float workingJointTargetAngle;

	[KSPField(guiFormat = "N1", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8002346")]
	public float currentAngle;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 180f, minValue = 1f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 30f, guiFormat = "F1", isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005419")]
	public float traverseVelocity = 90f;

	[KSPField]
	public bool hideUITraverseVelocity;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 200f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(advancedTweakable = true, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8002347")]
	public float hingeDamping = 100f;

	[KSPField]
	public bool hideUIDamping;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float angularXLimitSpring;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float angularXLimitDamper;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float highAngularXLimitBounce;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float highAngularXLimitSurf;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float lowAngularXLimitBounce;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float lowAngularXLimitSurf;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float driveDampingMutliplier = 20f;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float driveSpringMutliplier = 100f;

	[KSPField(isPersistant = true)]
	public float previousTargetAngle;

	[SerializeField]
	public string pistonTransforms = "Bar1,Bar2,TopPlateKnuckle,BasePlateKnuckle";

	[SerializeField]
	public float displacementLimitIgnoreRange = 0.025f;

	[SerializeField]
	public float displacementLimitIgnoreFrameDistance = 0.025f;

	[SerializeField]
	public SoftJointLimit upperLimit;

	[SerializeField]
	public SoftJointLimit lowerLimit;

	[SerializeField]
	public SoftJointLimitSpring xSpringLimit;

	[SerializeField]
	public JointDrive xDrive;

	[SerializeField]
	public Quaternion editorRotation;

	[SerializeField]
	public float lockedTargetAngle;

	public bool initComplete;

	public UI_FloatRange targetAngleUIField;

	public BaseAxisField targetAngleAxisField;

	public UI_FloatRange traverseVelocityUIField;

	public BaseAxisField traverseVelocityAxisField;

	public UI_MinMaxRange softRangeField;

	public float lastDragCubeAngle;

	public float powerLossLockAngle;

	public float maxAnglePerFrame;

	public float driveTargetAngle;

	public float angleDiff;

	public float lowerDisplacementLimit;

	public float upperDisplacementLimit;

	public float JointTargetAngle
	{
		get
		{
			workingJointTargetAngle = Mathf.Clamp(targetAngle, softMinMaxAngles.x, softMinMaxAngles.y);
			workingJointTargetAngle = (mirrorRotation ? (0f - workingJointTargetAngle) : workingJointTargetAngle);
			workingJointTargetAngle -= modelInitialAngle;
			return workingJointTargetAngle;
		}
	}

	public bool IsMultipleCubesActive => useMultipleDragCubes;

	[KSPAction("#autoLOC_8003236")]
	public void MaximumAngleAction(KSPActionParam param)
	{
		SetMaximumAngle();
	}

	[KSPAction("#autoLOC_8003235")]
	public void MinimumAngleAction(KSPActionParam param)
	{
		SetMinimumAngle();
	}

	[KSPAction("#autoLOC_8003282")]
	public void ToggleHingeAction(KSPActionParam param)
	{
		if (targetAngle - softMinMaxAngles.x < (softMinMaxAngles.y - softMinMaxAngles.x) / 2f)
		{
			SetMaximumAngle();
		}
		else
		{
			SetMinimumAngle();
		}
	}

	public void SetMinimumAngle()
	{
		if (!servoIsLocked)
		{
			base.Fields["targetAngle"].SetValue(softMinMaxAngles.x, this);
			ModifyServo(null);
		}
		else
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8002356", 5f);
		}
	}

	public void SetMaximumAngle()
	{
		if (!servoIsLocked)
		{
			base.Fields["targetAngle"].SetValue(softMinMaxAngles.y, this);
			ModifyServo(null);
		}
		else
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8002356", 5f);
		}
	}

	public void CheckSymmetry()
	{
		Part obj = base.part.symmetryCounterparts[0];
		int index = base.part.Modules.IndexOf(this);
		ModuleRoboticServoHinge moduleRoboticServoHinge = obj.Modules[index] as ModuleRoboticServoHinge;
		if (base.part.symMethod == SymmetryMethod.Mirror)
		{
			Vector3 vector = base.transform.position - moduleRoboticServoHinge.transform.position;
			Vector3 lhs = moduleRoboticServoHinge.jointParent.TransformDirection(moduleRoboticServoHinge.GetMainAxis());
			Vector3 rhs = jointParent.TransformDirection(GetMainAxis());
			lhs -= 2f * (Vector3.Dot(lhs, vector) / Vector3.Dot(vector, vector)) * vector;
			if (Vector3.Dot(lhs, rhs) > 0f)
			{
				mirrorRotation = !moduleRoboticServoHinge.mirrorRotation;
			}
			else
			{
				mirrorRotation = moduleRoboticServoHinge.mirrorRotation;
			}
		}
		else
		{
			mirrorRotation = moduleRoboticServoHinge.mirrorRotation;
		}
	}

	public void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		Part parent = base.part;
		while (true)
		{
			if (parent != null)
			{
				if (!(parent == p))
				{
					parent = parent.parent;
					continue;
				}
				break;
			}
			if (base.part.symmetryCounterparts.Count > 0 && (evt == ConstructionEventType.PartAttached || evt == ConstructionEventType.PartOffsetting || evt == ConstructionEventType.PartRotating))
			{
				CheckSymmetry();
				OnVisualizeServo(mirrorRotation ? true : false);
			}
			break;
		}
	}

	public override void OnStart(StartState state)
	{
		base.Actions["ResetPosition"].guiName = Localizer.Format("#autoLOC_6012043");
		base.Events["ResetPosition"].guiName = Localizer.Format("#autoLOC_6012043");
		base.OnStart(state);
		if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor)
		{
			if (checkSymmetry && base.part.symmetryCounterparts.Count > 0)
			{
				CheckSymmetry();
			}
			checkSymmetry = false;
			if (HighLogic.LoadedSceneIsEditor)
			{
				GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
			}
			base.Fields["servoMotorLimit"].guiName = Localizer.Format("#autoLOC_8003238");
			if (base.Fields.TryGetFieldUIControl<UI_MinMaxRange>("softMinMaxAngles", out softRangeField))
			{
				softRangeField.minValueX = hardMinMaxLimits.x;
				softRangeField.minValueY = hardMinMaxLimits.x + 1f;
				softRangeField.maxValueX = hardMinMaxLimits.y - 1f;
				softRangeField.maxValueY = hardMinMaxLimits.y;
			}
			base.Fields.TryGetFieldUIControl<UI_FloatRange>("targetAngle", out targetAngleUIField);
			targetAngleAxisField = base.Fields["targetAngle"] as BaseAxisField;
			base.Fields.TryGetFieldUIControl<UI_FloatRange>("traverseVelocity", out traverseVelocityUIField);
			traverseVelocityAxisField = base.Fields["traverseVelocity"] as BaseAxisField;
			base.Fields["softMinMaxAngles"].guiActiveEditor = !hideUISoftMinMaxAngles;
			base.Fields["traverseVelocity"].guiActive = !hideUITraverseVelocity;
			BaseField baseField = base.Fields["hingeDamping"];
			bool guiActive = (base.Fields["hingeDamping"].guiActiveEditor = !hideUIDamping);
			baseField.guiActive = guiActive;
			if (servoIsMotorized)
			{
				GameEvents.onPartResourceNonemptyEmpty.Add(OnResourceNonemptyEmpty);
			}
			ModifyLimits(null);
			ModifyTraverseLimits(null);
			base.Fields["softMinMaxAngles"].OnValueModified += ModifyLimits;
			base.Fields["traverseVelocity"].OnValueModified += ModifyTraverseLimits;
			base.Fields["targetAngle"].OnValueModified += base.ModifyServo;
			base.Fields["hingeDamping"].OnValueModified += base.ModifyServo;
			TogglePistons(active: false);
			lastDragCubeAngle = float.MaxValue;
		}
	}

	public void TogglePistons(bool active)
	{
		string[] array = pistonTransforms.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			GameObject child = base.part.gameObject.GetChild(array[i]);
			if (child != null)
			{
				child.SetActive(active);
			}
		}
	}

	public override void OnInventoryModeDisable()
	{
		base.OnInventoryModeDisable();
		TogglePistons(active: false);
	}

	public override void OnInventoryModeEnable()
	{
		base.OnInventoryModeEnable();
		TogglePistons(active: false);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
		GameEvents.onPartResourceNonemptyEmpty.Remove(OnResourceNonemptyEmpty);
		base.Fields["softMinMaxAngles"].OnValueModified -= ModifyLimits;
		base.Fields["traverseVelocity"].OnValueModified -= ModifyTraverseLimits;
		base.Fields["targetAngle"].OnValueModified -= base.ModifyServo;
		base.Fields["hingeDamping"].OnValueModified -= base.ModifyServo;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004440");
	}

	public override void OnCopy(PartModule fromModule)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			checkSymmetry = true;
		}
		base.OnCopy(fromModule);
	}

	public override void OnJointInit(bool goodSetup)
	{
		if (movingPartObject == null && !string.IsNullOrEmpty(servoTransformName))
		{
			movingPartObject = base.gameObject.GetChild(servoTransformName);
		}
		if (basePartObject == null && !string.IsNullOrEmpty(baseTransformName))
		{
			basePartObject = base.gameObject.GetChild(baseTransformName);
		}
		if (!HighLogic.LoadedSceneIsFlight)
		{
			if (movingPartObject != null)
			{
				CreateServoRigidbody();
			}
		}
		else if (movingPartObject != null)
		{
			CreateServoRigidbody();
			servoJoint = movingPartObject.AddComponent<ConfigurableJoint>();
			servoJoint.axis = GetMainAxis();
			if (servoJoint.axis == Vector3.up)
			{
				servoJoint.secondaryAxis = Vector3.right;
			}
			axis = servoJoint.axis;
			secAxis = servoJoint.secondaryAxis;
			servoJoint.xMotion = ConfigurableJointMotion.Locked;
			servoJoint.yMotion = ConfigurableJointMotion.Locked;
			servoJoint.zMotion = ConfigurableJointMotion.Locked;
			servoJoint.angularXMotion = ConfigurableJointMotion.Limited;
			servoJoint.angularYMotion = ConfigurableJointMotion.Locked;
			servoJoint.angularZMotion = ConfigurableJointMotion.Locked;
			xDrive = servoJoint.angularXDrive;
			xDrive.positionSpring = 0f;
			xDrive.positionDamper = 0.1f;
			xDrive.maximumForce = float.MaxValue;
			servoJoint.angularXDrive = xDrive;
			xSpringLimit = servoJoint.angularXLimitSpring;
			servoJoint.targetAngularVelocity = Vector3.zero;
			lowerLimit = servoJoint.lowAngularXLimit;
			upperLimit = servoJoint.highAngularXLimit;
			upperLimit.limit = cachedStartingRotationOffset + Mathf.Abs(softMinMaxAngles.x - modelInitialAngle);
			lowerLimit.limit = cachedStartingRotationOffset - (softMinMaxAngles.y - modelInitialAngle);
			servoJoint.highAngularXLimit = upperLimit;
			servoJoint.lowAngularXLimit = lowerLimit;
			servoJoint.targetRotation = Quaternion.identity;
		}
	}

	public override void OnPostStartJointInit()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			servoJoint.SetTargetRotationLocal(targetRotation, cachedStartingRotation);
			previousTargetAngle = modelInitialAngle - currentTransformAngle();
			driveTargetAngle = currentTransformAngle();
			OnServoLockApplied();
			ModifyLimits(null);
			initComplete = true;
		}
	}

	public override void OnPreModifyServo()
	{
		if (targetAngleUIField != null)
		{
			targetAngleUIField.minValue = softMinMaxAngles.x;
			targetAngleUIField.maxValue = softMinMaxAngles.y;
			if (targetAngleUIField.partActionItem != null)
			{
				targetAngleUIField.partActionItem.UpdateItem();
			}
		}
		targetAngle = Mathf.Clamp(targetAngle, softMinMaxAngles.x, softMinMaxAngles.y);
	}

	public override void OnVisualizeServo(bool rotateBase)
	{
		float jointTargetAngle = JointTargetAngle;
		if (rotateBase)
		{
			editorRotation = base.part.transform.rotation;
			float num = jointTargetAngle - previousTargetAngle;
			base.part.transform.rotation = SetTargetRotation(editorRotation, 0f - num, setRotation: false);
			editorRotation = movingPartObject.transform.localRotation;
			movingPartObject.transform.localRotation = SetTargetRotation(editorRotation, jointTargetAngle, setRotation: true);
		}
		else
		{
			editorRotation = movingPartObject.transform.localRotation;
			movingPartObject.transform.localRotation = SetTargetRotation(editorRotation, jointTargetAngle, setRotation: true);
		}
		if (previousTargetAngle != jointTargetAngle)
		{
			previousTargetAngle = jointTargetAngle;
		}
	}

	public override void OnModifyServo()
	{
		if (TimeWarp.CurrentRateIndex > 0 && TimeWarp.WarpMode == TimeWarp.Modes.HIGH)
		{
			xDrive = servoJoint.xDrive;
			xDrive.positionSpring = 0f;
			xDrive.positionDamper = 0f;
			xDrive.maximumForce = 0f;
			servoJoint.angularXDrive = xDrive;
			return;
		}
		if ((!servoMotorIsEngaged && !lockPartOnPowerLoss) || !servoIsMotorized)
		{
			servoJoint.SetTargetRotationLocal(movingPartObject.transform.localRotation, cachedStartingRotation);
		}
		xSpringLimit.spring = angularXLimitSpring;
		xSpringLimit.damper = angularXLimitDamper;
		servoJoint.angularXLimitSpring = xSpringLimit;
		xDrive = servoJoint.angularXDrive;
		if (!servoMotorIsEngaged && !servoIsLocked)
		{
			if (!lockPartOnPowerLoss)
			{
				xDrive.positionSpring = 0f;
				xDrive.positionDamper = 0f;
				xDrive.maximumForce = 0f;
			}
		}
		else
		{
			xDrive.positionSpring = motorOutput * driveSpringMutliplier;
			xDrive.positionDamper = driveDampingMutliplier * maxMotorOutput * (servoMotorSize * 0.01f) * (hingeDamping * 0.01f);
			xDrive.maximumForce = motorOutput;
		}
		servoJoint.angularXDrive = xDrive;
	}

	public override void ResetLaunchPosition()
	{
		base.Fields["targetAngle"].SetValue(launchPosition, this);
		ModifyServo(null);
	}

	public override void OnFixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || FlightDriver.Pause)
		{
			return;
		}
		if (servoMotorIsEngaged)
		{
			if (!servoIsLocked)
			{
				maxAnglePerFrame = traverseVelocity * Time.fixedDeltaTime;
				if (!driveTargetAngle.Equals(JointTargetAngle))
				{
					driveTargetAngle = Mathf.MoveTowards(driveTargetAngle, JointTargetAngle, maxAnglePerFrame);
				}
				targetRotation = SetTargetRotation(movingPartObject.transform.localRotation, driveTargetAngle, setRotation: true);
				servoJoint.SetTargetRotationLocal(targetRotation, cachedStartingRotation);
			}
		}
		else
		{
			if (!base.HasEnoughResources && lockPartOnPowerLoss && !servoIsLocked)
			{
				EngageServoLock();
			}
			if (servoIsLocked)
			{
				driveTargetAngle = lockAngle;
			}
		}
		previousDisplacement = currentTransformAngle();
		if (useMultipleDragCubes)
		{
			SetDragCubes(currentTransformAngle());
		}
	}

	public override bool IsMoving()
	{
		return transformRateOfMotion > 0.049f;
	}

	public override float GetFrameDisplacement()
	{
		angleDiff = Mathf.Abs(currentTransformAngle() - previousDisplacement);
		if (angleDiff < displacementLimitIgnoreFrameDistance)
		{
			lowerDisplacementLimit = softMinMaxAngles.x + displacementLimitIgnoreRange;
			upperDisplacementLimit = softMinMaxAngles.y - displacementLimitIgnoreRange;
			if (targetAngle < lowerDisplacementLimit && currentAngle < lowerDisplacementLimit)
			{
				angleDiff = 0f;
			}
			else if (targetAngle > upperDisplacementLimit && currentAngle > upperDisplacementLimit)
			{
				angleDiff = 0f;
			}
		}
		return angleDiff;
	}

	public override void SetInitialDisplacement()
	{
		previousDisplacement = currentTransformAngle();
	}

	public override void UpdatePAWUI(UI_Scene currentScene)
	{
		float num = currentTransformAngle() + modelInitialAngle;
		currentAngle = num;
		if ((prevServoMotorIsEngaged == servoMotorIsEngaged || currentScene != UI_Scene.Flight) && (prevServoIsMotorized == servoIsMotorized || currentScene != UI_Scene.Editor) && prevServoIsLocked == servoIsLocked)
		{
			base.UpdatePAWUI(currentScene);
			return;
		}
		if (targetAngleUIField != null)
		{
			targetAngleUIField.SetSceneVisibility(currentScene, servoIsMotorized && servoMotorIsEngaged && !servoIsLocked);
		}
		if (traverseVelocityUIField != null)
		{
			traverseVelocityUIField.SetSceneVisibility(currentScene, servoIsMotorized && servoMotorIsEngaged && !servoIsLocked && !hideUITraverseVelocity);
		}
		base.Fields["softMinMaxAngles"].guiActiveEditor = !hideUISoftMinMaxAngles;
		BaseField baseField = base.Fields["hingeDamping"];
		bool guiActive = (base.Fields["hingeDamping"].guiActiveEditor = !hideUIDamping);
		baseField.guiActive = guiActive;
		prevServoMotorIsEngaged = servoMotorIsEngaged;
		prevServoIsMotorized = servoIsMotorized;
		prevServoIsLocked = servoIsLocked;
		base.UpdatePAWUI(currentScene);
	}

	public override void OnServoLockApplied()
	{
		lockAngle = currentTransformAngle() + modelInitialAngle;
	}

	public override void OnServoLockRemoved()
	{
		driveTargetAngle = currentTransformAngle();
	}

	public override void OnServoMotorEngaged()
	{
		driveTargetAngle = currentTransformAngle();
		motorManualDisengaged = false;
	}

	public override void OnServoMotorDisengaged()
	{
		motorManualDisengaged = true;
	}

	public override void OnSaveShip(ShipConstruct ship)
	{
		SetLaunchPosition(targetAngle);
		base.OnSaveShip(ship);
	}

	public override string GetInfo()
	{
		string info = base.GetInfo();
		info = info + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format(Localizer.Format("#autoLOC_8320089") + "</color>");
		return info + "\n";
	}

	public void ModifyLimits(object field)
	{
		if (targetAngleUIField != null)
		{
			targetAngleUIField.minValue = softMinMaxAngles.x;
			targetAngleUIField.maxValue = softMinMaxAngles.y;
		}
		targetAngleAxisField.minValue = softMinMaxAngles.x;
		targetAngleAxisField.maxValue = softMinMaxAngles.y;
		targetAngle = Mathf.Clamp(targetAngle, softMinMaxAngles.x, softMinMaxAngles.y);
		if (axisFieldLimits.ContainsKey("targetAngle"))
		{
			UpdateAxisFieldLimit("targetAngle", hardMinMaxLimits, softMinMaxAngles);
			if (base.LimitsChanged != null)
			{
				base.LimitsChanged(axisFieldLimits["targetAngle"]);
			}
		}
		ModifyServo(field);
	}

	public void ModifyTraverseLimits(object field)
	{
		if (traverseVelocityUIField != null)
		{
			traverseVelocityUIField.minValue = traverseVelocityLimits.x;
			traverseVelocityUIField.maxValue = traverseVelocityLimits.y;
		}
		traverseVelocityAxisField.minValue = traverseVelocityLimits.x;
		traverseVelocityAxisField.maxValue = traverseVelocityLimits.y;
		traverseVelocity = Mathf.Clamp(traverseVelocity, traverseVelocityLimits.x, traverseVelocityLimits.y);
		currentVelocityLimit = traverseVelocity;
		ModifyServo(field);
	}

	public Quaternion SetTargetRotation(Quaternion startingRotation, float rotationAngle, bool setRotation)
	{
		Quaternion quaternion = startingRotation;
		if (setRotation)
		{
			return Quaternion.AngleAxis(rotationAngle, GetMainAxis());
		}
		return quaternion * Quaternion.AngleAxis(rotationAngle, GetMainAxis());
	}

	public void OnResourceNonemptyEmpty(PartResource resource)
	{
		if (!base.part.vessel.persistentId.Equals(resource.part.vessel.persistentId) || !base.part.vessel.resourcePartSet.ContainsPart(resource.part) || !resHandler.IsResourceBelowShutOffLimit(resource))
		{
			return;
		}
		for (int i = 0; i < resHandler.inputResources.Count; i++)
		{
			if (resource.resourceName.Equals(resHandler.inputResources[i].name))
			{
				powerLossLockAngle = currentTransformAngle();
				targetRotation = SetTargetRotation(movingPartObject.transform.localRotation, powerLossLockAngle, setRotation: true);
				servoJoint.SetTargetRotationLocal(targetRotation, cachedStartingRotation);
			}
		}
	}

	public override void InitAxisFieldLimits()
	{
		axisFieldLimits.Add("targetAngle", new AxisFieldLimit
		{
			limitedField = targetAngleAxisField,
			softLimits = softMinMaxAngles,
			hardLimits = hardMinMaxLimits
		});
	}

	public override void UpdateAxisFieldHardLimit(string fieldName, Vector2 newlimits)
	{
		if (fieldName == "targetAngle")
		{
			hardMinMaxLimits = newlimits;
			ModifyLimits(null);
		}
	}

	public override void UpdateAxisFieldSoftLimit(string fieldName, Vector2 newlimits)
	{
		if (fieldName == "targetAngle")
		{
			softMinMaxAngles = newlimits;
			ModifyLimits(null);
		}
	}

	public string[] GetDragCubeNames()
	{
		if (useMultipleDragCubes)
		{
			return new string[3] { "0", "50", "100" };
		}
		return null;
	}

	public void AssumeDragCubePosition(string name)
	{
		if (!(name == "0") && !(name == "50"))
		{
			_ = name == "100";
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public void SetDragCubes(float angle)
	{
		if (!angle.Equals(lastDragCubeAngle))
		{
			float num = angle;
			if (angle < -90f)
			{
				num = -180f - angle;
			}
			else if (angle > 90f)
			{
				num = 180f - angle;
			}
			float weight = Mathf.Max((0f - num) * 0.01f, 0f);
			float weight2 = 1f - Mathf.Max(Mathf.Abs(0f - num) * 0.01f, 0f);
			float weight3 = Mathf.Max(num * 0.01f, 0f);
			base.part.DragCubes.SetCubeWeight("0", weight);
			base.part.DragCubes.SetCubeWeight("50", weight2);
			base.part.DragCubes.SetCubeWeight("100", weight3);
			lastDragCubeAngle = angle;
		}
	}
}
