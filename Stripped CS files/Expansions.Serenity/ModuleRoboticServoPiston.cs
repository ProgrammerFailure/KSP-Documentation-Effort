using ns9;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRoboticServoPiston : BaseServo, IMultipleDragCube
{
	[KSPField(guiFormat = "F2", isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6013028")]
	[UI_MinMaxRange(maxValueY = 100f, maxValueX = 99f, stepIncrement = 1f, affectSymCounterparts = UI_Scene.All, minValueY = 1f, minValueX = 0f)]
	public Vector2 softMinMaxExtension = new Vector2(0f, 100f);

	public UI_MinMaxRange softRangeField;

	[KSPField]
	public bool hideUISoftMinMaxExtension;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 0.5f, guiFormat = "F2", isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6013029")]
	public float targetExtension;

	public float powerLossExtension;

	public UI_FloatRange targetExtensionField;

	public BaseAxisField targetExtensionAxisField;

	[KSPField(guiFormat = "F2", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6013033", guiUnits = "#autoLOC_7001411")]
	public float currentExtension;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 5f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 1f, isPersistant = true, maxValue = 5f, minValue = 0.05f, guiFormat = "F2", axisMode = KSPAxisMode.Absolute, guiActiveEditor = true, guiActive = true, guiName = "#autoLOC_8005419")]
	public float traverseVelocity = 1f;

	[KSPField]
	public bool hideUITraverseVelocity;

	[KSPAxisField(incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6013030")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 200f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float pistonDamping = 100f;

	[KSPField]
	public bool hideUIDamping;

	[KSPField]
	public float linearLimitBounce = 0.2f;

	[KSPField]
	public float linearLimitSpringSpring = 100000f;

	[KSPField]
	public float linearLimitSpringDamper = 100000f;

	[KSPField]
	public float linearLimitContactDistance = 1f;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float positionDampingMutliplier = 0.1f;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float positionSpringMutliplier = 100f;

	[KSPField]
	public string slaveTransformNames = "";

	[SerializeField]
	public SoftJointLimit linearLimit;

	[SerializeField]
	public SoftJointLimitSpring linearLimitSpring;

	[SerializeField]
	public JointDrive jointDrive;

	[SerializeField]
	public Vector3 editorPostion;

	[SerializeField]
	public float previousTargetPosition;

	public float driveTargetPosition;

	public float targetPosition;

	public Transform[] slaveTransforms;

	public bool initComplete;

	public static string outputUnit;

	public float positionDiff;

	public bool IsMultipleCubesActive => true;

	public new static void CacheLocalStrings()
	{
		outputUnit = Localizer.Format("#autoLOC_6013038");
	}

	public override void OnStart(StartState state)
	{
		base.Actions["ResetPosition"].guiName = Localizer.Format("#autoLOC_6012045");
		base.Events["ResetPosition"].guiName = Localizer.Format("#autoLOC_6012045");
		base.Fields.TryGetFieldUIControl<UI_MinMaxRange>("softMinMaxExtension", out softRangeField);
		if (softRangeField != null)
		{
			softRangeField.stepIncrement = (hardMinMaxLimits.y - hardMinMaxLimits.x) / 100f;
			softRangeField.minValueX = hardMinMaxLimits.x;
			softRangeField.minValueY = hardMinMaxLimits.x + softRangeField.stepIncrement;
			softRangeField.maxValueX = hardMinMaxLimits.y - softRangeField.stepIncrement;
			softRangeField.maxValueY = hardMinMaxLimits.y;
			softMinMaxExtension.x = Mathf.Clamp(softMinMaxExtension.x, softRangeField.minValueX, softRangeField.maxValueX);
			softMinMaxExtension.y = Mathf.Clamp(softMinMaxExtension.y, softRangeField.minValueY, softRangeField.maxValueY);
		}
		base.Fields.TryGetFieldUIControl<UI_FloatRange>("traverseVelocity", out var control);
		if (control != null)
		{
			control.minValue = traverseVelocityLimits.x;
			control.maxValue = traverseVelocityLimits.y;
		}
		if (base.Fields.TryGetFieldUIControl<UI_FloatRange>("targetExtension", out targetExtensionField))
		{
			targetExtensionAxisField = base.Fields["targetExtension"] as BaseAxisField;
		}
		if (targetExtensionField != null)
		{
			targetExtensionField.minValue = softMinMaxExtension.x;
			targetExtensionField.maxValue = softMinMaxExtension.y;
			targetExtensionField.stepIncrement = (softMinMaxExtension.y - softMinMaxExtension.x) / 100f;
		}
		if (targetExtensionAxisField != null)
		{
			targetExtensionAxisField.minValue = softMinMaxExtension.x;
			targetExtensionAxisField.maxValue = softMinMaxExtension.y;
			targetExtensionAxisField.incrementalSpeed = (hardMinMaxLimits.y - hardMinMaxLimits.x) * 0.2f;
		}
		targetExtension = Mathf.Min(Mathf.Max(softMinMaxExtension.x, targetExtension), softMinMaxExtension.y);
		targetPosition = targetExtension;
		previousTargetPosition = targetPosition;
		driveTargetPosition = targetPosition;
		base.OnStart(state);
		UpdateTraverseVelocity(null);
		UpdateTargetPosition(null);
		if (servoIsMotorized)
		{
			GameEvents.onPartResourceNonemptyEmpty.Add(OnResourceNonemptyEmpty);
		}
		base.Fields["servoMotorLimit"].guiName = Localizer.Format("#autoLOC_8003239");
		base.Fields["softMinMaxExtension"].guiActiveEditor = !hideUISoftMinMaxExtension;
		BaseField baseField = base.Fields["traverseVelocity"];
		bool guiActive = (base.Fields["traverseVelocity"].guiActiveEditor = !hideUITraverseVelocity);
		baseField.guiActive = guiActive;
		BaseField baseField2 = base.Fields["pistonDamping"];
		guiActive = (base.Fields["pistonDamping"].guiActiveEditor = !hideUIDamping);
		baseField2.guiActive = guiActive;
		base.Fields["softMinMaxExtension"].OnValueModified += ModifyLimits;
		base.Fields["targetExtension"].OnValueModified += UpdateTargetPosition;
		base.Fields["pistonDamping"].OnValueModified += base.ModifyServo;
		base.Fields["traverseVelocity"].OnValueModified += UpdateTraverseVelocity;
		OutputUnit = outputUnit;
		if (string.IsNullOrEmpty(slaveTransformNames))
		{
			return;
		}
		Transform parent = movingPartObject.transform.parent;
		string[] array = ParseExtensions.ParseArray(slaveTransformNames);
		if (array.Length == 0)
		{
			return;
		}
		slaveTransforms = new Transform[array.Length];
		int num = 0;
		while (true)
		{
			if (num < array.Length)
			{
				slaveTransforms[num] = parent.Find(array[num]);
				if (slaveTransforms[num] == null)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		Debug.LogError("[ModuleRoboticServoPiston] No slave transform named " + array[num]);
		slaveTransforms = null;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onPartResourceNonemptyEmpty.Remove(OnResourceNonemptyEmpty);
		base.Fields["softMinMaxExtension"].OnValueModified -= ModifyLimits;
		base.Fields["targetExtension"].OnValueModified -= UpdateTargetPosition;
		base.Fields["pistonDamping"].OnValueModified -= base.ModifyServo;
		base.Fields["traverseVelocity"].OnValueModified -= UpdateTraverseVelocity;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004442");
	}

	public void ModifyLimits(object field)
	{
		if (base.Fields["TargetExtension"] is BaseAxisField baseAxisField)
		{
			baseAxisField.minValue = softMinMaxExtension.x;
			baseAxisField.maxValue = softMinMaxExtension.y;
		}
		if (targetExtensionField != null)
		{
			targetExtensionField.minValue = softMinMaxExtension.x;
			targetExtensionField.maxValue = softMinMaxExtension.y;
			targetExtensionField.stepIncrement = (softMinMaxExtension.y - softMinMaxExtension.x) / 100f;
		}
		targetExtension = Mathf.Min(Mathf.Max(softMinMaxExtension.x, targetExtension), softMinMaxExtension.y);
		if (axisFieldLimits.ContainsKey("targetExtension"))
		{
			UpdateAxisFieldLimit("targetExtension", hardMinMaxLimits, softMinMaxExtension);
			if (base.LimitsChanged != null)
			{
				base.LimitsChanged(axisFieldLimits["targetExtension"]);
			}
		}
		UpdateTargetPosition(field);
		if (HighLogic.LoadedSceneIsFlight)
		{
			Vector3 vector = CalcTargetPosition(Vector3.zero, softMinMaxExtension.x, relative: false);
			Vector3 vector2 = CalcTargetPosition(Vector3.zero, softMinMaxExtension.y, relative: false);
			Vector3 vector3 = (vector + vector2) / 2f;
			servoJoint.autoConfigureConnectedAnchor = false;
			servoJoint.connectedAnchor = servoParentTransform * vector3;
			linearLimit.limit = (softMinMaxExtension.y - softMinMaxExtension.x) * 1.01f / 2f;
			servoJoint.linearLimit = linearLimit;
		}
		ModifyServo(field);
	}

	public void UpdateTargetPosition(object field)
	{
		targetExtension = Mathf.Min(Mathf.Max(softMinMaxExtension.x, targetExtension), softMinMaxExtension.y);
		targetPosition = targetExtension;
		ModifyServo(field);
	}

	public void UpdateTraverseVelocity(object field)
	{
		currentVelocityLimit = traverseVelocity;
		ModifyServo(field);
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
			servoJoint.anchor = Vector3.zero;
			Quaternion localRotation = movingPartObject.transform.localRotation;
			servoJoint.axis = Quaternion.Inverse(localRotation) * GetMainAxis();
			if (servoJoint.axis == Vector3.up)
			{
				servoJoint.secondaryAxis = Vector3.right;
			}
			axis = servoJoint.axis;
			secAxis = servoJoint.secondaryAxis;
			servoJoint.xMotion = ConfigurableJointMotion.Limited;
			servoJoint.yMotion = ConfigurableJointMotion.Locked;
			servoJoint.zMotion = ConfigurableJointMotion.Locked;
			servoJoint.angularXMotion = ConfigurableJointMotion.Locked;
			servoJoint.angularYMotion = ConfigurableJointMotion.Locked;
			servoJoint.angularZMotion = ConfigurableJointMotion.Locked;
			linearLimitSpring = servoJoint.linearLimitSpring;
			servoJoint.targetVelocity = Vector3.zero;
			linearLimit = servoJoint.linearLimit;
			linearLimit.limit = 0f;
			servoJoint.linearLimit = linearLimit;
		}
	}

	public override void OnPostStartJointInit()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			initComplete = true;
			ModifyLimits(null);
			if (servoIsLocked)
			{
				currentExtension = targetPosition;
				lockAngle = currentExtension;
				SetJointTargetPosition();
			}
		}
	}

	public override void OnPreModifyServo()
	{
	}

	public override void OnVisualizeServo(bool moveBase)
	{
		float num = Mathf.Clamp(targetPosition, softMinMaxExtension.x, softMinMaxExtension.y);
		if (moveBase)
		{
			Quaternion localRotation = base.part.transform.localRotation;
			Vector3 startingPosition = Quaternion.Inverse(localRotation) * base.part.transform.localPosition;
			float num2 = num - previousTargetPosition;
			base.part.transform.localPosition = localRotation * CalcTargetPosition(startingPosition, 0f - num2, relative: true);
			startingPosition = movingPartObject.transform.localPosition;
			movingPartObject.transform.localPosition = CalcTargetPosition(Vector3.zero, num, relative: false);
		}
		else
		{
			movingPartObject.transform.localPosition = CalcTargetPosition(Vector3.zero, num, relative: false);
		}
		if (previousTargetPosition != num)
		{
			previousTargetPosition = num;
		}
	}

	public void LateUpdate()
	{
		if (slaveTransforms != null)
		{
			Vector3 localPosition = movingPartObject.transform.localPosition;
			Vector3 localPosition2 = basePartObject.transform.localPosition;
			float num = 1f / (float)(slaveTransforms.Length + 1);
			for (int i = 0; i < slaveTransforms.Length; i++)
			{
				float num2 = (float)(i + 1) * num;
				slaveTransforms[i].localPosition = localPosition * (1f - num2) + localPosition2 * num2;
			}
		}
	}

	public override void OnFixedUpdate()
	{
		if (!initComplete || !HighLogic.LoadedSceneIsFlight || FlightDriver.Pause)
		{
			return;
		}
		if (servoMotorIsEngaged && !servoIsLocked)
		{
			float num = traverseVelocity * Time.fixedDeltaTime;
			if (driveTargetPosition < targetPosition)
			{
				driveTargetPosition = Mathf.Min(driveTargetPosition + num, targetPosition);
			}
			else if (driveTargetPosition > targetPosition)
			{
				driveTargetPosition = Mathf.Max(driveTargetPosition - num, targetPosition);
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
				driveTargetPosition = lockAngle;
			}
		}
		previousDisplacement = currentTransformPosition();
		SetJointTargetPosition();
		float num2 = currentExtension;
		UpdateCurrentExtension();
		if (num2 != currentExtension)
		{
			SetDragCubes(currentExtension);
		}
	}

	public void SetJointTargetPosition()
	{
		Vector3 vector = Vector3.right * ((softMinMaxExtension.y + softMinMaxExtension.x) / 2f - driveTargetPosition);
		servoJoint.targetPosition = vector;
	}

	public override void OnModifyServo()
	{
		if (TimeWarp.CurrentRateIndex > 0 && TimeWarp.WarpMode == TimeWarp.Modes.HIGH)
		{
			jointDrive = servoJoint.xDrive;
			jointDrive.positionSpring = 0f;
			jointDrive.positionDamper = 0f;
			jointDrive.maximumForce = 0f;
			servoJoint.xDrive = jointDrive;
			return;
		}
		linearLimit.limit = (softMinMaxExtension.y - softMinMaxExtension.x) * 1.01f / 2f;
		linearLimit.bounciness = linearLimitBounce;
		linearLimit.contactDistance = linearLimitContactDistance;
		servoJoint.linearLimit = linearLimit;
		linearLimitSpring.spring = linearLimitSpringSpring;
		linearLimitSpring.damper = linearLimitSpringDamper;
		servoJoint.linearLimitSpring = linearLimitSpring;
		jointDrive = servoJoint.xDrive;
		if (!servoMotorIsEngaged && !servoIsLocked)
		{
			if (!lockPartOnPowerLoss)
			{
				jointDrive.positionSpring = 0f;
				jointDrive.positionDamper = 0f;
				jointDrive.maximumForce = 0f;
			}
		}
		else
		{
			jointDrive.positionSpring = motorOutput * positionSpringMutliplier;
			jointDrive.positionDamper = positionDampingMutliplier * maxMotorOutput * (servoMotorSize * 0.01f) * (pistonDamping * 0.01f);
			jointDrive.maximumForce = motorOutput;
		}
		servoJoint.xDrive = jointDrive;
	}

	public void UpdateCurrentExtension()
	{
		if (servoJoint != null)
		{
			Vector3 vector = servoJoint.transform.localRotation * (servoJoint.targetPosition.x * axis) + servoJoint.transform.localPosition;
			Vector3 vector2 = servoParentTransformInverse * servoJoint.connectedAnchor;
			currentExtension = Vector3.Dot(vector - vector2, GetMainAxis()) + driveTargetPosition;
		}
	}

	public override void OnServoLockApplied()
	{
		UpdateCurrentExtension();
		lockAngle = currentExtension;
		driveTargetPosition = currentExtension;
	}

	public override void OnServoLockRemoved()
	{
		UpdateCurrentExtension();
		currentExtension = lockAngle;
		driveTargetPosition = currentExtension;
	}

	public override void OnServoMotorEngaged()
	{
		UpdateCurrentExtension();
		driveTargetPosition = currentExtension;
		motorManualDisengaged = false;
	}

	public override void OnServoMotorDisengaged()
	{
		motorManualDisengaged = true;
	}

	public override bool IsMoving()
	{
		return transformRateOfMotion > 0.049f;
	}

	public override float GetFrameDisplacement()
	{
		positionDiff = Mathf.Abs(currentTransformPosition() - previousDisplacement);
		return positionDiff;
	}

	public override void SetInitialDisplacement()
	{
		previousDisplacement = currentTransformPosition();
	}

	public override void ResetLaunchPosition()
	{
		base.Fields["targetExtension"].SetValue(Mathf.Min(Mathf.Max(softMinMaxExtension.x, launchPosition), softMinMaxExtension.y), this);
		targetPosition = targetExtension;
	}

	public override void OnSaveShip(ShipConstruct ship)
	{
		SetLaunchPosition(targetExtension);
		base.OnSaveShip(ship);
	}

	public override void UpdatePAWUI(UI_Scene currentScene)
	{
		if (initComplete)
		{
			if (servoJoint != null)
			{
				UpdateCurrentExtension();
			}
			if (targetExtensionField != null)
			{
				targetExtensionField.SetSceneVisibility(currentScene, servoIsMotorized && servoMotorIsEngaged && !servoIsLocked);
			}
			base.Fields["softMinMaxExtension"].guiActiveEditor = !hideUISoftMinMaxExtension;
			BaseField baseField = base.Fields["traverseVelocity"];
			bool guiActive = (base.Fields["traverseVelocity"].guiActiveEditor = !hideUITraverseVelocity);
			baseField.guiActive = guiActive;
			BaseField baseField2 = base.Fields["pistonDamping"];
			guiActive = (base.Fields["pistonDamping"].guiActiveEditor = !hideUIDamping);
			baseField2.guiActive = guiActive;
			prevServoIsLocked = servoIsLocked;
			base.UpdatePAWUI(currentScene);
		}
	}

	public override string GetInfo()
	{
		string info = base.GetInfo();
		info = info + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format(Localizer.Format("#autoLOC_8320089") + "</color>");
		return info + "\n";
	}

	public Vector3 CalcTargetPosition(Vector3 startingPosition, float offset, bool relative)
	{
		Vector3 vector = mainAxis switch
		{
			"Z" => Vector3.forward, 
			"Y" => Vector3.up, 
			_ => Vector3.right, 
		};
		if (relative)
		{
			return startingPosition + vector * offset;
		}
		return vector * offset;
	}

	[KSPAction("#autoLOC_6013034")]
	public void ExtendPistonAction(KSPActionParam param)
	{
		ExtendPiston();
	}

	[KSPAction("#autoLOC_6013035")]
	public void RetractPistonAction(KSPActionParam param)
	{
		RetractPiston();
	}

	[KSPAction("#autoLOC_6013036")]
	public void TogglePistonAction(KSPActionParam param)
	{
		if (targetExtension - softMinMaxExtension.x < (softMinMaxExtension.y - softMinMaxExtension.x) / 2f)
		{
			ExtendPiston();
		}
		else
		{
			RetractPiston();
		}
	}

	public void ExtendPiston()
	{
		if (!servoIsLocked)
		{
			base.Fields["targetExtension"].SetValue(softMinMaxExtension.y, this);
			UpdateTargetPosition(null);
		}
		else
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8002356", 5f);
		}
	}

	public void RetractPiston()
	{
		if (!servoIsLocked)
		{
			base.Fields["targetExtension"].SetValue(softMinMaxExtension.x, this);
			UpdateTargetPosition(null);
		}
		else
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8002356", 5f);
		}
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
				powerLossExtension = currentTransformPosition();
				ModifyServo(null);
			}
		}
	}

	public override void InitAxisFieldLimits()
	{
		axisFieldLimits.Add("targetExtension", new AxisFieldLimit
		{
			limitedField = targetExtensionAxisField,
			softLimits = softMinMaxExtension,
			hardLimits = hardMinMaxLimits
		});
	}

	public override void UpdateAxisFieldHardLimit(string fieldName, Vector2 newlimits)
	{
		if (fieldName == "targetExtension")
		{
			hardMinMaxLimits = newlimits;
			ModifyLimits(null);
		}
	}

	public override void UpdateAxisFieldSoftLimit(string fieldName, Vector2 newlimits)
	{
		if (fieldName == "targetExtension")
		{
			softMinMaxExtension = newlimits;
			ModifyLimits(null);
		}
	}

	public string[] GetDragCubeNames()
	{
		return new string[3] { "0", "50", "100" };
	}

	public void AssumeDragCubePosition(string name)
	{
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public void SetDragCubes(float extension)
	{
		float x = hardMinMaxLimits.x;
		float y = hardMinMaxLimits.y;
		float num = (x + y) / 2f;
		float num2 = Mathf.Clamp01((num - extension) / (num - x));
		float num3 = Mathf.Clamp01(extension - num) / (y - num);
		float weight = (1f - num2) * (1f - num3);
		base.part.DragCubes.SetCubeWeight("0", num2);
		base.part.DragCubes.SetCubeWeight("50", weight);
		base.part.DragCubes.SetCubeWeight("100", num3);
	}
}
