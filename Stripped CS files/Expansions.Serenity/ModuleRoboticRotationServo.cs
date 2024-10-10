using ns11;
using ns9;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRoboticRotationServo : BaseServo
{
	[KSPField(guiFormat = "F1", isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8002344")]
	[UI_MinMaxRange(maxValueY = 177f, maxValueX = 176f, stepIncrement = 1f, affectSymCounterparts = UI_Scene.All, minValueY = 1f, minValueX = 0f)]
	public Vector2 softMinMaxAngles = new Vector2(0f, 177f);

	public Vector2 invertedSoftMinMaxAngles = new Vector2(0f, -177f);

	[KSPField]
	public bool hideUISoftMinMaxAngles;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8003291")]
	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.Editor, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	public bool allowFullRotation = true;

	[KSPAxisField(isPersistant = true, incrementalSpeed = 30f, guiFormat = "F1", axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, guiName = "#autoLOC_8005420")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float targetAngle;

	[KSPField(guiFormat = "N1", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8002346")]
	public float currentAngle;

	[UI_Toggle(disabledText = "#autoLOC_8003303", enabledText = "#autoLOC_8003302", tipText = "#autoLOC_8003301", affectSymCounterparts = UI_Scene.None)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003304")]
	public bool inverted;

	[KSPField(isPersistant = true)]
	public bool mirrorRotation;

	public bool checkSymmetry;

	public float workingJointTargetAngle;

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
	public SoftJointLimit upperLimit;

	[SerializeField]
	public SoftJointLimit lowerLimit;

	[SerializeField]
	public SoftJointLimitSpring xSpringLimit;

	[SerializeField]
	public JointDrive xDrive;

	[SerializeField]
	public Quaternion editorRotation;

	public bool initComplete;

	public UI_FloatRange targetAngleUIField;

	public BaseAxisField targetAngleAxisField;

	public UI_FloatRange traverseVelocityUIField;

	public BaseAxisField traverseVelocityAxisField;

	public UI_MinMaxRange softRangeUIField;

	public float maxAnglePerFrame;

	public float driveTargetAngle;

	public float angleDiff;

	public Vector2 configHardMinMaxLimits;

	public Vector2 prevSoftMinMaxAngles;

	public float JointTargetAngle
	{
		get
		{
			workingJointTargetAngle = Mathf.Clamp(targetAngle, softMinMaxAngles.x, softMinMaxAngles.y);
			workingJointTargetAngle = (mirrorRotation ? (0f - workingJointTargetAngle) : workingJointTargetAngle);
			if (inverted)
			{
				workingJointTargetAngle = 0f - workingJointTargetAngle;
			}
			return workingJointTargetAngle;
		}
	}

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

	[KSPAction("#autoLOC_8003285")]
	public void ToggleServoAction(KSPActionParam param)
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
		ModuleRoboticRotationServo moduleRoboticRotationServo = obj.Modules[index] as ModuleRoboticRotationServo;
		if (base.part.symMethod == SymmetryMethod.Mirror)
		{
			Vector3 vector = base.transform.position - moduleRoboticRotationServo.transform.position;
			Vector3 lhs = moduleRoboticRotationServo.jointParent.TransformDirection(moduleRoboticRotationServo.GetMainAxis());
			Vector3 rhs = jointParent.TransformDirection(GetMainAxis());
			lhs -= 2f * (Vector3.Dot(lhs, vector) / Vector3.Dot(vector, vector)) * vector;
			if (Vector3.Dot(lhs, rhs) > 0f)
			{
				mirrorRotation = !moduleRoboticRotationServo.mirrorRotation;
			}
			else
			{
				mirrorRotation = moduleRoboticRotationServo.mirrorRotation;
			}
		}
		else
		{
			mirrorRotation = moduleRoboticRotationServo.mirrorRotation;
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
			if (base.part.symmetryCounterparts.Count > 0 && (evt == ConstructionEventType.PartOffsetting || evt == ConstructionEventType.PartRotating))
			{
				CheckSymmetry();
				OnVisualizeServo(rotateBase: false);
			}
			break;
		}
	}

	public override void OnStart(StartState state)
	{
		base.Actions["ResetPosition"].guiName = Localizer.Format("#autoLOC_6012043");
		base.Events["ResetPosition"].guiName = Localizer.Format("#autoLOC_6012043");
		base.OnStart(state);
		configHardMinMaxLimits = hardMinMaxLimits;
		prevSoftMinMaxAngles = softMinMaxAngles;
		invertedSoftMinMaxAngles.x = 0f - softMinMaxAngles.x;
		invertedSoftMinMaxAngles.y = 0f - softMinMaxAngles.y;
		if (!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
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
		if (base.Fields.TryGetFieldUIControl<UI_MinMaxRange>("softMinMaxAngles", out softRangeUIField))
		{
			softRangeUIField.minValueX = hardMinMaxLimits.x;
			softRangeUIField.minValueY = hardMinMaxLimits.x + 1f;
			softRangeUIField.maxValueX = hardMinMaxLimits.y - 1f;
			softRangeUIField.maxValueY = hardMinMaxLimits.y;
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
		ModifyLimitsMode(null);
		ModifyTraverseLimits(null);
		base.Fields["allowFullRotation"].OnValueModified += ModifyLimitsMode;
		base.Fields["softMinMaxAngles"].OnValueModified += ModifyLimits;
		base.Fields["traverseVelocity"].OnValueModified += ModifyTraverseLimits;
		base.Fields["targetAngle"].OnValueModified += ModifyTargetAngle;
		base.Fields["hingeDamping"].OnValueModified += base.ModifyServo;
		base.Fields["inverted"].OnValueModified += ModifyInverted;
		string[] array = pistonTransforms.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			GameObject child = base.part.gameObject.GetChild(array[i]);
			if (child != null)
			{
				child.SetActive(value: false);
			}
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
		GameEvents.onPartResourceNonemptyEmpty.Remove(OnResourceNonemptyEmpty);
		base.Fields["allowFullRotation"].OnValueModified -= ModifyLimitsMode;
		base.Fields["softMinMaxAngles"].OnValueModified -= ModifyLimits;
		base.Fields["traverseVelocity"].OnValueModified -= ModifyTraverseLimits;
		base.Fields["targetAngle"].OnValueModified -= ModifyTargetAngle;
		base.Fields["hingeDamping"].OnValueModified -= base.ModifyServo;
		base.Fields["inverted"].OnValueModified -= ModifyInverted;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004443");
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
			xDrive = servoJoint.xDrive;
			xDrive.positionSpring = 0f;
			xDrive.positionDamper = 0.1f;
			xDrive.maximumForce = float.MaxValue;
			servoJoint.angularXDrive = xDrive;
			xSpringLimit = servoJoint.angularXLimitSpring;
			servoJoint.targetAngularVelocity = Vector3.zero;
			SetJointHighLowLimits();
			servoJoint.targetRotation = Quaternion.identity;
		}
	}

	public void SetJointHighLowLimits()
	{
		lowerLimit = servoJoint.lowAngularXLimit;
		upperLimit = servoJoint.highAngularXLimit;
		if (inverted)
		{
			invertedSoftMinMaxAngles.x = (mirrorRotation ? (0f - softMinMaxAngles.y) : (0f - softMinMaxAngles.x));
			invertedSoftMinMaxAngles.y = (mirrorRotation ? (0f - softMinMaxAngles.x) : (0f - softMinMaxAngles.y));
			upperLimit.limit = (mirrorRotation ? softMinMaxAngles.x : softMinMaxAngles.y);
			lowerLimit.limit = (mirrorRotation ? (0f - softMinMaxAngles.y) : softMinMaxAngles.x);
		}
		else
		{
			upperLimit.limit = cachedStartingRotationOffset + (mirrorRotation ? Mathf.Abs(softMinMaxAngles.y) : Mathf.Abs(softMinMaxAngles.x));
			lowerLimit.limit = cachedStartingRotationOffset - (mirrorRotation ? (0f - softMinMaxAngles.x) : softMinMaxAngles.y);
		}
		servoJoint.highAngularXLimit = upperLimit;
		servoJoint.lowAngularXLimit = lowerLimit;
	}

	public override void OnPostStartJointInit()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			servoJoint.SetTargetRotationLocal(targetRotation, cachedStartingRotation);
			previousTargetAngle = currentTransformAngle();
			driveTargetAngle = currentTransformAngle();
			OnServoLockApplied();
			ModifyServo(null);
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
		if (!allowFullRotation)
		{
			xSpringLimit.spring = angularXLimitSpring;
			xSpringLimit.damper = angularXLimitDamper;
		}
		else
		{
			xSpringLimit.spring = 1E-16f;
			xSpringLimit.damper = 1E-16f;
		}
		servoJoint.angularXLimitSpring = xSpringLimit;
		xDrive = servoJoint.angularXDrive;
		if (!servoMotorIsEngaged && !servoIsLocked)
		{
			if (!lockPartOnPowerLoss)
			{
				xDrive.positionSpring = 0f;
				xDrive.positionDamper = 1000f;
				xDrive.maximumForce = 1f;
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

	public override void OnFixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || FlightDriver.Pause)
		{
			return;
		}
		if (!servoIsLocked && servoMotorIsEngaged)
		{
			maxAnglePerFrame = traverseVelocity * Time.fixedDeltaTime;
			if (!driveTargetAngle.Equals(JointTargetAngle))
			{
				if (!allowFullRotation)
				{
					driveTargetAngle = Mathf.MoveTowards(driveTargetAngle, JointTargetAngle, maxAnglePerFrame);
				}
				else
				{
					driveTargetAngle = Mathf.MoveTowardsAngle(driveTargetAngle, JointTargetAngle, maxAnglePerFrame);
				}
			}
			targetRotation = SetTargetRotation(movingPartObject.transform.localRotation, driveTargetAngle, setRotation: true);
			servoJoint.SetTargetRotationLocal(targetRotation, cachedStartingRotation);
		}
		else if (!base.HasEnoughResources && lockPartOnPowerLoss && !servoIsLocked)
		{
			EngageServoLock();
		}
		else
		{
			targetRotation = SetTargetRotation(movingPartObject.transform.localRotation, lockAngle, setRotation: true);
			servoJoint.SetTargetRotationLocal(targetRotation, cachedStartingRotation);
		}
		previousDisplacement = currentTransformAngle();
	}

	public override bool IsMoving()
	{
		return transformRateOfMotion > 0.049f;
	}

	public override float GetFrameDisplacement()
	{
		angleDiff = currentTransformAngle() - previousDisplacement;
		if (angleDiff >= 180f)
		{
			angleDiff -= 360f;
		}
		else if (angleDiff < -180f)
		{
			angleDiff += 360f;
		}
		angleDiff = Mathf.Abs(angleDiff);
		return angleDiff;
	}

	public override void SetInitialDisplacement()
	{
		previousDisplacement = currentTransformAngle();
	}

	public override void UpdatePAWUI(UI_Scene currentScene)
	{
		float num = currentTransformAngle();
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
		base.Fields["softMinMaxAngles"].guiActiveEditor = !allowFullRotation && !hideUIDamping;
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
		lockAngle = currentTransformAngle();
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

	public override void ResetLaunchPosition()
	{
		base.Fields["targetAngle"].SetValue(launchPosition, this);
		ModifyServo(null);
	}

	public override void OnSaveShip(ShipConstruct ship)
	{
		SetLaunchPosition(currentAngle);
		base.OnSaveShip(ship);
	}

	public override string GetInfo()
	{
		string info = base.GetInfo();
		info = info + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format(Localizer.Format("#autoLOC_8320089") + "</color>");
		return info + "\n";
	}

	public void ModifyTargetAngle(object field)
	{
		if (!allowFullRotation)
		{
			targetAngle = Mathf.Clamp(targetAngle, softMinMaxAngles.x, softMinMaxAngles.y);
		}
		else if (targetAngle < -179.99f)
		{
			targetAngle = 360f + targetAngle;
		}
		else if (targetAngle > 180f)
		{
			targetAngle -= 360f;
		}
		ModifyServo(field);
	}

	public void ModifyLimitsMode(object field)
	{
		if (!allowFullRotation)
		{
			SetHardLimits("targetAngle", configHardMinMaxLimits);
			SetSoftLimits("targetAngle", prevSoftMinMaxAngles);
		}
		else
		{
			prevSoftMinMaxAngles = softMinMaxAngles;
			SetHardLimits("targetAngle", new Vector2(-179.99f, 180f));
			SetSoftLimits("targetAngle", new Vector2(-179.99f, 180f));
		}
		base.Fields["softMinMaxAngles"].guiActiveEditor = !allowFullRotation && !hideUIDamping;
		base.Actions["MinimumAngleAction"].active = !allowFullRotation;
		base.Actions["MaximumAngleAction"].active = !allowFullRotation;
		base.Actions["ToggleServoAction"].active = !allowFullRotation;
		if (HighLogic.LoadedSceneIsEditor && EditorActionGroups.Instance != null && EditorActionGroups.Instance.interfaceActive)
		{
			EditorActionGroups.Instance.RebuildLists(fullRebuild: true, keepSelection: true);
		}
		ModifyServo(field);
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
		if (HighLogic.LoadedSceneIsFlight)
		{
			SetJointHighLowLimits();
		}
		ModifyServo(field);
	}

	public void ModifyInverted(object field)
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			SetJointHighLowLimits();
		}
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
				targetRotation = SetTargetRotation(movingPartObject.transform.localRotation, currentTransformAngle(), setRotation: true);
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
			invertedSoftMinMaxAngles.x = 0f - softMinMaxAngles.x;
			invertedSoftMinMaxAngles.y = 0f - softMinMaxAngles.y;
			ModifyLimits(null);
		}
	}
}
