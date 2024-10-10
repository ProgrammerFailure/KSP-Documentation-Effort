using System;
using ns9;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRoboticServoRotor : BaseServo
{
	[KSPAxisField(incrementalSpeed = 100f, guiFormat = "F1", isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005424")]
	[UI_FloatRange(stepIncrement = 5f, maxValue = 460f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float rpmLimit = 230f;

	[KSPField(guiFormat = "F1", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8005437")]
	public float currentRPM;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005425")]
	[UI_Toggle(disabledText = "#autoLOC_8005426", scene = UI_Scene.All, enabledText = "#autoLOC_8005427", affectSymCounterparts = UI_Scene.All)]
	public bool rotateCounterClockwise;

	[UI_Toggle(disabledText = "#autoLOC_8003303", enabledText = "#autoLOC_8003302", tipText = "#autoLOC_8003301", affectSymCounterparts = UI_Scene.None)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003304")]
	public bool inverted;

	[UI_Cycle(stateNames = new string[] { "#autoLOC_8002243", "#autoLOC_8005426", "#autoLOC_8005427" })]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8005428")]
	public int ratcheted;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001458")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 200f, minValue = 0f)]
	public float brakePercentage = 100f;

	[KSPField]
	public float rotationMatch = 20f;

	[KSPField]
	public float LFPerkN = 0.1f;

	[KSPField]
	public float OxidizerPerkN = 0.1f;

	[KSPField]
	public float rotorSpoolTime = 3f;

	[KSPField]
	public float brakeTorque = 1f;

	[KSPField]
	public float maxTorque = 400f;

	[KSPField]
	public float angularPositionDamper = 1f;

	[KSPField]
	public float angularPositionSpring = 1E-10f;

	[KSPField]
	public string angularDriveMode = "XYAndZ";

	[SerializeField]
	public SoftJointLimitSpring xSpringLimit;

	[SerializeField]
	public SoftJointLimit lowLimits;

	[SerializeField]
	public SoftJointLimit highLimits;

	[SerializeField]
	public JointDrive xDrive;

	[SerializeField]
	public Quaternion editorRotation;

	public bool initComplete;

	public UI_Cycle ratchetedField;

	public UI_Toggle rotationDirectionField;

	public UI_FloatRange rpmLimitUIField;

	public BaseAxisField rpmLimitAxisField;

	public UI_FloatRange motorPowerField;

	public bool brakingMode;

	public float freespinCurrentRotation;

	public float freespinPreviousRotation;

	public Quaternion ratchetLockRotation;

	[SerializeField]
	public float ratchetTolerance = 0.2f;

	public bool resourceStateChange;

	public float positiveAngularVelocity;

	public float brakingTorque;

	public float totalTorque;

	public float tempAngularVelocity;

	public float workingRPM;

	public float loadMass;

	[SerializeField]
	public float rpmRefreshSeconds = 0.3f;

	public float timeSinceRPMRefresh;

	public bool refreshRPMReadouts;

	public float rotationDiff;

	public float normalizedOutput => transformRateOfMotion / rpmLimit;

	public override void OnStart(StartState state)
	{
		base.Actions["ResetPosition"].guiName = Localizer.Format("#autoLOC_6012044");
		base.Events["ResetPosition"].guiName = Localizer.Format("#autoLOC_6012044");
		base.OnStart(state);
		if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				servoMotorLimit = 0f;
			}
			base.Fields["servoMotorLimit"].guiName = Localizer.Format("#autoLOC_8003238");
			base.Fields.TryGetFieldUIControl<UI_Toggle>("rotateCounterClockwise", out rotationDirectionField);
			base.Fields.TryGetFieldUIControl<UI_Cycle>("ratcheted", out ratchetedField);
			base.Fields.TryGetFieldUIControl<UI_FloatRange>("servoMotorPower", out motorPowerField);
			base.Fields.TryGetFieldUIControl<UI_FloatRange>("rpmLimit", out rpmLimitUIField);
			rpmLimitAxisField = base.Fields["rpmLimit"] as BaseAxisField;
			ModifyRPMLimits(null);
			base.Fields["rpmLimit"].OnValueModified += ModifyRPMLimits;
			base.Fields["brakePercentage"].OnValueModified += base.ModifyServo;
			base.Fields["rotateCounterClockwise"].OnValueModified += ModifyDirection;
			base.Fields["ratcheted"].OnValueModified += base.ModifyServo;
			base.Fields["inverted"].OnValueModified += base.ModifyServo;
			GameEvents.onVesselChange.Add(OnVesselChange);
			if (servoIsMotorized)
			{
				GameEvents.onPartResourceNonemptyEmpty.Add(OnResourceNonemptyEmpty);
			}
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		base.Fields["rpmLimit"].OnValueModified -= ModifyRPMLimits;
		base.Fields["brakePercentage"].OnValueModified -= base.ModifyServo;
		base.Fields["rotateCounterClockwise"].OnValueModified -= ModifyDirection;
		base.Fields["ratcheted"].OnValueModified -= base.ModifyServo;
		base.Fields["inverted"].OnValueModified -= base.ModifyServo;
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.onPartResourceNonemptyEmpty.Remove(OnResourceNonemptyEmpty);
	}

	public void ModifyRPMLimits(object field)
	{
		if (rpmLimitUIField != null)
		{
			rpmLimitUIField.minValue = traverseVelocityLimits.x;
			rpmLimitUIField.maxValue = traverseVelocityLimits.y;
		}
		rpmLimitAxisField.minValue = traverseVelocityLimits.x;
		rpmLimitAxisField.maxValue = traverseVelocityLimits.y;
		rpmLimit = Mathf.Clamp(rpmLimit, traverseVelocityLimits.x, traverseVelocityLimits.y);
		currentVelocityLimit = rpmLimit;
		ModifyServo(field);
	}

	public void ModifyDirection(object field)
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			ModifyServo(field);
		}
	}

	public override void OnUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			RatchetLock();
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004441");
	}

	[KSPAction("#autoLOC_6001458", KSPActionGroup.Brakes)]
	public void BrakeAction(KSPActionParam kPar)
	{
		KSPActionType kSPActionType = kPar.type;
		if (kSPActionType == KSPActionType.Toggle)
		{
			kSPActionType = (brakingMode ? KSPActionType.Deactivate : KSPActionType.Activate);
		}
		switch (kSPActionType)
		{
		case KSPActionType.Deactivate:
			brakingMode = false;
			servoIsBraking = false;
			ModifyServo(null);
			break;
		case KSPActionType.Activate:
			brakingMode = true;
			servoIsBraking = true;
			ModifyServo(null);
			break;
		}
		ModifyServo(null);
	}

	[KSPAction("#autoLOC_8005434")]
	public void MotorPowerAction(KSPActionParam param)
	{
		ToggleMotorPower();
	}

	[KSPAction("#autoLOC_8005435")]
	public void MotorDirectionAction(KSPActionParam param)
	{
		ToggleMotorDirection();
	}

	public void ToggleMotorPower()
	{
		if (!servoIsLocked)
		{
			if (servoMotorLimit > 0f)
			{
				base.Fields["servoMotorLimit"].SetValue(0f, this);
			}
			else
			{
				base.Fields["servoMotorLimit"].SetValue(100f, this);
			}
			ModifyServo(null);
		}
		else
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8002356", 5f);
		}
	}

	public void ToggleMotorDirection()
	{
		if (!servoIsLocked)
		{
			base.Fields["rotateCounterClockwise"].SetValue(!rotateCounterClockwise, this);
			ModifyServo(null);
		}
		else
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8002356", 5f);
		}
	}

	public override void OnJointInit(bool goodSetup)
	{
		if (movingPartObject == null && !string.IsNullOrEmpty(servoTransformName))
		{
			movingPartObject = base.gameObject.GetChild(servoTransformName);
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
			UpdateLoadMass();
			movingPartRB.isKinematic = base.part.packed;
			movingPartRB.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
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
			highLimits.limit = 0.1f;
			lowLimits.limit = 0.01f;
			servoJoint.highAngularXLimit = highLimits;
			servoJoint.lowAngularXLimit = lowLimits;
			SetJointDrive(0f);
			xSpringLimit = servoJoint.angularXLimitSpring;
			xSpringLimit.spring = 1E-08f;
			xSpringLimit.damper = 1E-08f;
			servoJoint.angularXLimitSpring = xSpringLimit;
			servoJoint.targetAngularVelocity = Vector3.zero;
			servoJoint.transform.localRotation.ToAngleAxis(out freespinCurrentRotation, out var _);
		}
	}

	public override void OnPostStartJointInit()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			OnServoLockApplied();
			ModifyServo(null);
			initComplete = true;
		}
	}

	public override void OnPreModifyServo()
	{
	}

	public override void OnVisualizeServo(bool rotateBase)
	{
		if (servoIsMotorized && initComplete)
		{
			editorRotation = movingPartObject.transform.localRotation;
			if ((rotateCounterClockwise && !inverted) || (!rotateCounterClockwise && inverted))
			{
				movingPartObject.transform.localRotation = RotateOnMainAxis(editorRotation, rpmLimit * UtilMath.RPM2RadPerSec * rotationMatch * -1f * Mathf.Min(servoMotorLimit, servoMotorSize));
			}
			else
			{
				movingPartObject.transform.localRotation = RotateOnMainAxis(editorRotation, rpmLimit * UtilMath.RPM2RadPerSec * rotationMatch * Mathf.Min(servoMotorLimit, servoMotorSize));
			}
		}
	}

	public override void OnModifyServo()
	{
		maxTorque = maxMotorOutput * (servoMotorSize * 0.01f);
		if (!servoIsBraking)
		{
			brakingMode = false;
		}
		brakingTorque = (brakingMode ? (brakeTorque * brakePercentage * 0.01f) : 0f);
		totalTorque = Mathf.Max(0f, maxTorque * (servoMotorLimit * 0.01f));
		workingRPM = (brakingMode ? 0f : Mathf.Max(0f, rpmLimit * UtilMath.RPM2RadPerSec));
		string text = angularDriveMode;
		if (!(text == "XYAndZ") && text == "slerp")
		{
			if ((!rotateCounterClockwise && !inverted) || (rotateCounterClockwise && inverted))
			{
				workingRPM *= -1f;
			}
		}
		else if ((rotateCounterClockwise && !inverted) || (!rotateCounterClockwise && inverted))
		{
			workingRPM *= -1f;
		}
		if (servoMotorIsEngaged)
		{
			if (TimeWarp.CurrentRateIndex > 0 && TimeWarp.WarpMode == TimeWarp.Modes.HIGH)
			{
				servoJoint.targetAngularVelocity = Vector3.zero;
				SetJointDrive(0f);
			}
			else if (hasEnoughResources)
			{
				servoJoint.angularXMotion = ConfigurableJointMotion.Limited;
				if (brakingMode)
				{
					SetJointDrive(brakingTorque);
				}
				else
				{
					SetJointDrive(totalTorque);
				}
			}
			else
			{
				servoJoint.targetAngularVelocity = Vector3.zero;
				SetJointDrive(1f);
			}
		}
		else
		{
			servoJoint.targetAngularVelocity = Vector3.zero;
			if (brakingMode)
			{
				SetJointDrive(brakingTorque);
			}
			else
			{
				SetJointDrive(1f);
			}
		}
	}

	public override void OnServoLockApplied()
	{
		lockAngle = currentTransformAngle();
	}

	public override void OnServoLockRemoved()
	{
	}

	public override void OnServoMotorEngaged()
	{
		motorManualDisengaged = false;
	}

	public override void OnServoMotorDisengaged()
	{
		motorManualDisengaged = true;
	}

	public override void OnFixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || !initComplete)
		{
			return;
		}
		timeSinceRPMRefresh += Time.deltaTime;
		if (timeSinceRPMRefresh > rpmRefreshSeconds)
		{
			timeSinceRPMRefresh = 0f;
			refreshRPMReadouts = true;
		}
		if (servoMotorIsEngaged && !servoIsLocked)
		{
			if (servoMotorLimit > 0f)
			{
				resourceStateChange = false;
				if (Mathf.Abs(tempAngularVelocity) <= Mathf.Abs(workingRPM))
				{
					string text = angularDriveMode;
					if (!(text == "XYAndZ") && text == "slerp")
					{
						if ((!rotateCounterClockwise && !inverted) || (rotateCounterClockwise && inverted))
						{
							tempAngularVelocity -= rotorSpoolTime * Time.fixedDeltaTime;
						}
						else
						{
							tempAngularVelocity += rotorSpoolTime * Time.fixedDeltaTime;
						}
					}
					else if ((rotateCounterClockwise && !inverted) || (!rotateCounterClockwise && inverted))
					{
						tempAngularVelocity -= rotorSpoolTime * Time.fixedDeltaTime;
					}
					else
					{
						tempAngularVelocity += rotorSpoolTime * Time.fixedDeltaTime;
					}
					servoJoint.targetAngularVelocity = Vector3.right * tempAngularVelocity;
				}
				else
				{
					servoJoint.targetAngularVelocity = Vector3.right * workingRPM;
				}
			}
			else
			{
				tempAngularVelocity = 0f;
				servoJoint.targetAngularVelocity = Vector3.zero;
			}
		}
		else
		{
			tempAngularVelocity = 0f;
			servoJoint.targetAngularVelocity = Vector3.zero;
			if (lockPartOnPowerLoss)
			{
				if (!servoIsLocked && resourceStateChange)
				{
					brakingMode = true;
					servoIsBraking = true;
					EngageServoLock();
				}
				SetJointDrive(brakeTorque);
			}
			if (servoIsLocked)
			{
				targetRotation = Quaternion.AngleAxis(lockAngle, GetMainAxis());
				servoJoint.SetTargetRotationLocal(targetRotation, cachedStartingRotation);
				SetJointDrive(1f);
			}
		}
		previousDisplacement = currentTransformAngle();
	}

	public void RatchetLock()
	{
		if (servoMotorIsEngaged)
		{
			return;
		}
		servoJoint.angularXMotion = ConfigurableJointMotion.Free;
		servoJoint.targetAngularVelocity = Vector3.zero;
		resourceConsumption = "0.0";
		motorState = "Unpowered";
		movingPartRB.centerOfMass = Vector3.zero;
		movingPartRB.inertiaTensorRotation = Quaternion.identity;
		ratchetLockRotation = Quaternion.identity;
		float num = 0f;
		servoJoint.transform.localRotation.ToAngleAxis(out freespinCurrentRotation, out var vector);
		num = freespinCurrentRotation - freespinPreviousRotation;
		RigidbodyConstraints constraints = RigidbodyConstraints.None;
		switch (mainAxis)
		{
		case "Y":
			constraints = RigidbodyConstraints.FreezeRotationY;
			num *= vector.y;
			ratchetLockRotation = Quaternion.AngleAxis(freespinPreviousRotation, Vector3.right);
			break;
		case "X":
			constraints = RigidbodyConstraints.FreezeRotationX;
			num *= vector.x;
			ratchetLockRotation = Quaternion.AngleAxis(freespinPreviousRotation, Vector3.up);
			break;
		case "Z":
			constraints = RigidbodyConstraints.FreezeRotationZ;
			num *= vector.z;
			ratchetLockRotation = Quaternion.AngleAxis(freespinPreviousRotation, Vector3.forward);
			break;
		}
		if (num > 0f && num > ratchetTolerance)
		{
			if (ratcheted.Equals(2))
			{
				movingPartRB.constraints = constraints;
				servoJoint.transform.localRotation = ratchetLockRotation;
			}
			else
			{
				movingPartRB.constraints = RigidbodyConstraints.None;
			}
		}
		else if (num < 0f && Mathf.Abs(num) > ratchetTolerance)
		{
			if (ratcheted.Equals(1))
			{
				movingPartRB.constraints = constraints;
				servoJoint.transform.localRotation = ratchetLockRotation;
			}
			else
			{
				movingPartRB.constraints = RigidbodyConstraints.None;
			}
		}
		else
		{
			movingPartRB.constraints = RigidbodyConstraints.None;
		}
		freespinPreviousRotation = freespinCurrentRotation;
	}

	public override bool IsMoving()
	{
		return transformRateOfMotion > 0.5f;
	}

	public override float GetFrameDisplacement()
	{
		rotationDiff = currentTransformAngle() - previousDisplacement;
		if (rotationDiff >= 180f)
		{
			rotationDiff -= 360f;
		}
		else if (rotationDiff < -180f)
		{
			rotationDiff += 360f;
		}
		rotationDiff = Mathf.Abs(rotationDiff) * 60f / 360f;
		return rotationDiff;
	}

	public override void SetInitialDisplacement()
	{
		previousDisplacement = currentTransformAngle();
	}

	public override void OnSaveShip(ShipConstruct ship)
	{
		SetLaunchPosition(rpmLimit);
		base.OnSaveShip(ship);
	}

	public override void ResetLaunchPosition()
	{
		base.Fields["rpmLimit"].SetValue(launchPosition, this);
		ModifyServo(null);
	}

	public override string GetInfo()
	{
		string info = base.GetInfo();
		info = info + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format(Localizer.Format("#autoLOC_8320090") + "</color>");
		return info + "\n";
	}

	public void OnVesselChange(Vessel vsl)
	{
		if (base.part.vessel != null && vsl.persistentId == base.part.vessel.persistentId)
		{
			UpdateLoadMass();
		}
	}

	public void UpdateLoadMass()
	{
		loadMass = 0f;
		for (int i = 0; i < base.part.children.Count; i++)
		{
			base.part.children[i].UpdateMass();
			loadMass += base.part.children[i].mass;
		}
	}

	public void SetJointDrive(float torque)
	{
		string text = angularDriveMode;
		if (!(text == "XYAndZ") && text == "slerp")
		{
			servoJoint.rotationDriveMode = RotationDriveMode.Slerp;
			xDrive = servoJoint.slerpDrive;
			xDrive.positionSpring = angularPositionSpring;
			xDrive.positionDamper = angularPositionDamper;
			xDrive.maximumForce = torque;
			servoJoint.slerpDrive = xDrive;
		}
		else
		{
			servoJoint.rotationDriveMode = RotationDriveMode.XYAndZ;
			xDrive = servoJoint.xDrive;
			xDrive.positionSpring = angularPositionSpring;
			xDrive.positionDamper = ((servoMotorIsEngaged || brakingMode) ? angularPositionDamper : 1E-11f);
			xDrive.maximumForce = torque;
			servoJoint.angularXDrive = xDrive;
		}
	}

	public Quaternion RotateOnMainAxis(Quaternion originalRotation, float rotationAngle)
	{
		Vector3 vector = GetMainAxis() * Mathf.Sin(rotationAngle * 0.5f * ((float)Math.PI / 180f));
		Quaternion quaternion = default(Quaternion);
		quaternion.x = vector.x;
		quaternion.y = vector.y;
		quaternion.z = vector.z;
		quaternion.w = Mathf.Cos(rotationAngle * 0.5f * ((float)Math.PI / 180f));
		return originalRotation * quaternion;
	}

	public override void UpdatePAWUI(UI_Scene currentScene)
	{
		bool num = (prevServoMotorIsEngaged != servoMotorIsEngaged && currentScene == UI_Scene.Flight) || (prevServoIsMotorized != servoIsMotorized && currentScene == UI_Scene.Editor) || prevServoIsLocked != servoIsLocked;
		if (HighLogic.LoadedSceneIsFlight && refreshRPMReadouts)
		{
			refreshRPMReadouts = false;
			currentRPM = transformRateOfMotion;
		}
		if (!num)
		{
			base.UpdatePAWUI(currentScene);
			return;
		}
		if (currentScene == UI_Scene.Editor)
		{
			if (rotationDirectionField != null)
			{
				rotationDirectionField.SetSceneVisibility(currentScene, servoIsMotorized);
			}
			if (ratchetedField != null)
			{
				ratchetedField.SetSceneVisibility(currentScene, !servoIsMotorized);
			}
			if (rpmLimitUIField != null)
			{
				rpmLimitUIField.SetSceneVisibility(currentScene, servoIsMotorized);
			}
			if (motorPowerField != null)
			{
				motorPowerField.SetSceneVisibility(currentScene, state: false);
			}
		}
		else
		{
			if (rotationDirectionField != null)
			{
				rotationDirectionField.SetSceneVisibility(currentScene, servoIsMotorized);
			}
			if (ratchetedField != null)
			{
				ratchetedField.SetSceneVisibility(currentScene, state: false);
			}
			if (rpmLimitUIField != null)
			{
				rpmLimitUIField.SetSceneVisibility(currentScene, servoMotorIsEngaged);
			}
			if (motorPowerField != null)
			{
				motorPowerField.SetSceneVisibility(currentScene, servoMotorIsEngaged && !servoIsLocked);
			}
		}
		prevServoMotorIsEngaged = servoMotorIsEngaged;
		prevServoIsMotorized = servoIsMotorized;
		prevServoIsLocked = servoIsLocked;
		base.UpdatePAWUI(currentScene);
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
				resourceStateChange = true;
			}
		}
	}

	public override void InitAxisFieldLimits()
	{
	}

	public override void UpdateAxisFieldHardLimit(string fieldName, Vector2 newlimits)
	{
	}

	public override void UpdateAxisFieldSoftLimit(string fieldName, Vector2 newlimits)
	{
	}
}
