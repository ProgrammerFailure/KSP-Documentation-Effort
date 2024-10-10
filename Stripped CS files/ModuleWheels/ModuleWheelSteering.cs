using System;
using ns9;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelSteering : ModuleWheelSubmodule
{
	[KSPField]
	public string caliperTransformName = "";

	[KSPField]
	public FloatCurve steeringCurve;

	[KSPField]
	public FloatCurve steeringMaxAngleCurve;

	[KSPField]
	public float steeringResponse = 8f;

	[KSPField(isPersistant = true)]
	public bool autoSteeringAdjust = true;

	public Transform caliperTransform;

	public bool caliperUpdating;

	public Quaternion caliperRot0 = Quaternion.identity;

	[KSPField]
	public float steeringRange;

	public float steeringInput;

	public float steeringInputLast;

	public Vector3 upAxis;

	public Vector3 fwdAxis;

	public Vector3 leftAxis;

	public float steerAngle;

	public float steerRange;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001467")]
	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	public bool steeringEnabled = true;

	[UI_Toggle(disabledText = "#autoLOC_6001075", scene = UI_Scene.All, enabledText = "#autoLOC_6001077", affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001468")]
	public bool steeringInvert;

	public Vector3 CoM;

	public Vector3 wCenter;

	public Vector3 wLeft0;

	public Vector3 wLeft;

	public Vector3 wRight;

	public Vector3 wAxis;

	public Vector3 sAxis;

	public float CoMfwdLength;

	public BaseEvent evtAutoSteeringAdjustToggle;

	public BaseField fldSteeringResponseTweakable;

	public BaseField fldSteeringAngleTweakable;

	public bool partActionWindowOpen;

	public bool steeringAngleCurveDefined;

	public bool steeringCurveDefined;

	public Vector2 prevLocalWheelVelocity;

	[KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6002672")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 1f, maxValue = 10f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	public float angleTweakable = -1f;

	[KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6002673")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 10f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	public float responseTweakable = -1f;

	public Vector3 updateCoordFrameReferenceForward;

	public Vector3 tPivot;

	public Vector3 tOrt;

	public Vector3 tCenter;

	public float h;

	[KSPEvent(advancedTweakable = true, active = true, guiActive = true, guiActiveEditor = true, guiName = "")]
	public void EvtAutoSteeringAdjustToggle()
	{
		autoSteeringAdjust = !autoSteeringAdjust;
		ActionSteeringAdjustUIUpdate();
		ATsymPartUpdate();
	}

	public void ActionSteeringAdjustUIUpdate()
	{
		if (evtAutoSteeringAdjustToggle != null)
		{
			evtAutoSteeringAdjustToggle.guiActive = true;
			evtAutoSteeringAdjustToggle.guiActiveEditor = true;
			evtAutoSteeringAdjustToggle.guiName = Localizer.Format("#autoLOC_6002674", Convert.ToInt32(autoSteeringAdjust));
		}
		if (fldSteeringResponseTweakable != null)
		{
			fldSteeringResponseTweakable.guiActive = !autoSteeringAdjust || !GameSettings.WHEEL_AUTO_STEERINGADJUST;
			fldSteeringResponseTweakable.guiActiveEditor = !autoSteeringAdjust || !GameSettings.WHEEL_AUTO_STEERINGADJUST;
		}
		if (fldSteeringAngleTweakable != null)
		{
			fldSteeringAngleTweakable.guiActive = !autoSteeringAdjust || !GameSettings.WHEEL_AUTO_STEERINGADJUST;
			fldSteeringAngleTweakable.guiActiveEditor = !autoSteeringAdjust || !GameSettings.WHEEL_AUTO_STEERINGADJUST;
		}
		SetSteeringTweakablesSliders();
	}

	public void ATsymPartUpdate()
	{
		int i = 0;
		for (int count = base.part.symmetryCounterparts.Count; i < count; i++)
		{
			ModuleWheelSteering moduleWheelSteering = base.part.symmetryCounterparts[i].FindModuleImplementing<ModuleWheelSteering>();
			if (moduleWheelSteering != null)
			{
				moduleWheelSteering.autoSteeringAdjust = autoSteeringAdjust;
				moduleWheelSteering.ActionSteeringAdjustUIUpdate();
			}
		}
	}

	public override void OnAwake()
	{
		base.OnAwake();
		if (steeringCurve == null)
		{
			steeringCurve = new FloatCurve();
		}
		if (steeringMaxAngleCurve == null)
		{
			steeringMaxAngleCurve = new FloatCurve();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
	}

	public override void OnSave(ConfigNode node)
	{
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		steeringCurveDefined = steeringCurve.maxTime != float.MinValue;
		steeringAngleCurveDefined = steeringMaxAngleCurve.maxTime != float.MinValue;
		evtAutoSteeringAdjustToggle = base.Events["EvtAutoSteeringAdjustToggle"];
		fldSteeringResponseTweakable = base.Fields["responseTweakable"];
		fldSteeringAngleTweakable = base.Fields["angleTweakable"];
		if (!string.IsNullOrEmpty(caliperTransformName))
		{
			caliperTransform = base.part.FindModelTransform(caliperTransformName);
			if (caliperTransform == null)
			{
				Debug.LogError("[ModuleWheelBase]: No transform called " + caliperTransformName + " found in " + base.part.partName + " hierarchy", base.gameObject);
				return;
			}
			caliperRot0 = caliperTransform.localRotation;
		}
		base.part.PartValues.SteeringRadius.Add(GetCoMFwdLength);
		if (steeringRange < 0.01f)
		{
			steeringCurve.FindMinMaxValue(out var _, out var max);
			steeringRange = Mathf.Max(max, 0.01f);
		}
		if (angleTweakable < 0f)
		{
			angleTweakable = steeringRange;
		}
		if (responseTweakable < 0f)
		{
			responseTweakable = steeringResponse;
		}
		ActionSteeringAdjustUIUpdate();
		GameEvents.onWheelRepaired.Add(OnWheelRepaired);
		GameEvents.onPartActionUIShown.Add(OnPartActionUIShown);
		GameEvents.onPartActionUIDismiss.Add(OnPartActionUIDismiss);
	}

	[KSPAction("#autoLOC_6002418")]
	public void SteeringToggle(KSPActionParam act)
	{
		steeringEnabled = act.type == KSPActionType.Activate || (act.type == KSPActionType.Toggle && !steeringEnabled);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		base.part.PartValues.SteeringRadius.Remove(GetCoMFwdLength);
		GameEvents.onWheelRepaired.Remove(OnWheelRepaired);
		GameEvents.onPartActionUIShown.Remove(OnPartActionUIShown);
		GameEvents.onPartActionUIDismiss.Remove(OnPartActionUIDismiss);
	}

	public override void OnWheelSetup()
	{
		if (caliperTransform != null)
		{
			wheel.wheelCollider.caliperTransform = caliperTransform;
		}
		if (angleTweakable < 0f)
		{
			angleTweakable = steeringResponse;
		}
		wheel.maxSteerAngle = angleTweakable;
		if (responseTweakable < 0f)
		{
			responseTweakable = steeringRange;
		}
		wheel.steeringResponse = steeringResponse;
		SetCaliperUpdate(!wheelBase.InopSystems.HasType(WheelSubsystem.SystemTypes.Steering));
		if (wheelBase.wheelTransform != null)
		{
			wLeft0 = base.part.partTransform.InverseTransformDirection(-wheelBase.wheelTransform.right);
		}
		else
		{
			wLeft0 = Vector3.left;
		}
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || !baseSetup)
		{
			return;
		}
		if (!base.part.packed && steeringEnabled && caliperUpdating)
		{
			updateCoordFrame();
			steeringInput = Mathf.Clamp(base.vessel.ctrlState.wheelSteer + base.vessel.ctrlState.wheelSteerTrim, -1f, 1f);
			if (steeringInvert)
			{
				steeringInput *= -1f;
			}
			wheel.steerInput = updateSteering(steeringInput, steeringRange);
			steeringInputLast = steeringInput;
			wheel.steeringResponse = steeringResponse * GetSteeringResponseScale(steeringInput - steeringInputLast);
		}
		else
		{
			wheel.steeringResponse = steeringResponse;
			wheel.steerInput = 0f;
		}
	}

	public float GetSteeringResponseScale(float steerDelta)
	{
		float num = Mathf.Abs(wheel.currentState.grounded ? wheel.currentState.localWheelVelocity.y : prevLocalWheelVelocity.y);
		if (wheel.currentState.grounded)
		{
			prevLocalWheelVelocity = wheel.currentState.localWheelVelocity;
		}
		if (steerDelta < 0f)
		{
			num = 0f;
		}
		num *= 1f - wheel.currentState.hit.sidewaysSlip;
		if (autoSteeringAdjust)
		{
			wheel.maxSteerAngle = angleTweakable * (steeringAngleCurveDefined ? Mathf.Clamp01(steeringMaxAngleCurve.Evaluate(num)) : 1f);
			wheel.maxSteerAngle = (float)Math.Round(wheel.maxSteerAngle * 100f) / 100f;
			if (steeringCurveDefined)
			{
				return steeringCurve.Evaluate(num) / steeringRange;
			}
			return responseTweakable;
		}
		wheel.maxSteerAngle = angleTweakable;
		return responseTweakable;
	}

	public void updateCoordFrame()
	{
		float num = Vector3.Dot(base.vessel.ReferenceTransform.up, base.vessel.upAxis);
		upAxis = base.vessel.ReferenceTransform.up;
		if (num < Vector3.Dot(-base.vessel.ReferenceTransform.forward, base.vessel.upAxis))
		{
			upAxis = -base.vessel.ReferenceTransform.forward;
		}
		updateCoordFrameReferenceForward = -base.vessel.ReferenceTransform.forward;
		if (upAxis == base.vessel.ReferenceTransform.up)
		{
			updateCoordFrameReferenceForward = base.vessel.ReferenceTransform.forward;
		}
		fwdAxis = base.vessel.ReferenceTransform.up;
		if (num > Vector3.Dot(updateCoordFrameReferenceForward, base.vessel.upAxis))
		{
			fwdAxis = updateCoordFrameReferenceForward;
		}
		leftAxis = Vector3.Cross(fwdAxis, upAxis);
		CoM = base.vessel.CurrentCoM;
		wCenter = base.part.partTransform.TransformPoint(wheelBase.WheelOrgPosR);
		wLeft = Vector3.ProjectOnPlane(base.part.partTransform.TransformDirection(wLeft0), upAxis).normalized;
		wRight = -wLeft;
	}

	public float updateSteering(float input, float steeringRange)
	{
		float num = steeringRange * input;
		Vector3 vector = findTurnCenter(num, base.vessel.VesselValues.SteeringRadius.value, CoM);
		if (vector != Vector3.zero)
		{
			sAxis = Vector3.ProjectOnPlane(vector - wCenter, upAxis).normalized;
			Debug.DrawRay(wCenter, sAxis, XKCDColors.Teal);
			Debug.DrawRay(wCenter, wLeft, XKCDColors.DarkYellow);
			float a = Vector3.Dot(wRight, sAxis);
			a = Mathf.Max(a, Vector3.Dot(wLeft, sAxis));
			steerAngle = Mathf.Acos(a) * 57.29578f * Mathf.Sign(input);
		}
		else
		{
			steerAngle = 0f;
		}
		if (CoMfwdLength > 0f)
		{
			steerAngle = 0f - steerAngle;
		}
		if (steerAngle > 90f)
		{
			steerAngle = 180f - steerAngle;
		}
		else if (steerAngle < -90f)
		{
			steerAngle = -180f - steerAngle;
		}
		return Mathf.Clamp(steerAngle / wheel.maxSteerAngle, -1f, 1f);
	}

	public float findCoMfwdLength(Vector3 vesselCoM, Vector3 wheelCenter, Vector3 fwdAxis)
	{
		return Vector3.Dot(wheelCenter - vesselCoM, fwdAxis);
	}

	public float GetCoMFwdLength()
	{
		CoMfwdLength = findCoMfwdLength(CoM, wCenter, fwdAxis);
		return Mathf.Abs(CoMfwdLength);
	}

	public Vector3 findTurnCenter(float steerAngle, float length, Vector3 CoM)
	{
		if (steerAngle != 0f)
		{
			tPivot = fwdAxis * length;
			tOrt = Quaternion.AngleAxis(Mathf.Abs(steerAngle), -upAxis * Mathf.Sign(steerAngle)) * leftAxis * Mathf.Sign(steerAngle);
			h = length * (1f / Mathf.Tan(Mathf.Abs(steerAngle) * ((float)Math.PI / 180f)));
			tCenter = CoM + tPivot + tOrt * h;
		}
		else
		{
			tCenter = Vector3.zero;
		}
		Debug.DrawRay(CoM + tPivot, tOrt * h, XKCDColors.Magenta);
		DebugDrawUtil.DrawCrosshairs(tCenter, 0.5f, XKCDColors.BrightCyan, 0f);
		return tCenter;
	}

	public void OnPartActionUIShown(UIPartActionWindow window, Part p)
	{
		if (p.persistentId == base.part.persistentId)
		{
			partActionWindowOpen = true;
			SetSteeringTweakablesSliders();
		}
	}

	public void SetSteeringTweakablesSliders()
	{
		if (fldSteeringAngleTweakable != null && fldSteeringAngleTweakable.uiControlFlight != null && fldSteeringAngleTweakable.uiControlFlight is UI_FloatRange uI_FloatRange)
		{
			uI_FloatRange.maxValue = steeringRange;
		}
		if (fldSteeringAngleTweakable != null && fldSteeringAngleTweakable.uiControlEditor != null && fldSteeringAngleTweakable.uiControlEditor is UI_FloatRange uI_FloatRange2)
		{
			uI_FloatRange2.maxValue = steeringRange;
		}
	}

	public void OnPartActionUIDismiss(Part p)
	{
		if (p.persistentId == base.part.persistentId)
		{
			partActionWindowOpen = false;
		}
	}

	public override string OnGatherInfo()
	{
		return Localizer.Format("#autoLOC_248404", steeringCurve.maxTime.ToString("0.0"));
	}

	public override void OnSubsystemsModified(WheelSubsystems s)
	{
		if (s == wheelBase.InopSystems)
		{
			SetCaliperUpdate(!s.HasType(WheelSubsystem.SystemTypes.Steering));
		}
	}

	public void SetCaliperUpdate(bool update)
	{
		caliperUpdating = update;
		if (wheel != null && wheel.wheelCollider != null)
		{
			wheel.wheelCollider.updateCaliper = update;
		}
		ResetCaliper(!caliperUpdating);
	}

	public void ResetCaliper(bool resetTransform)
	{
		steerAngle = 0f;
		steeringInput = 0f;
		if (resetTransform && caliperTransform != null)
		{
			caliperTransform.localRotation = caliperRot0;
		}
	}

	public void OnWheelRepaired(Part p)
	{
		if (p != null && p == base.part)
		{
			ResetCaliper(resetTransform: true);
		}
	}
}
