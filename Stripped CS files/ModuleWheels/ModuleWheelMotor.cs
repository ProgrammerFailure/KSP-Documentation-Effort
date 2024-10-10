using System;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelMotor : ModuleWheelSubmodule, IResourceConsumer
{
	public enum MotorState
	{
		Inoperable = -2,
		NotEnoughResources,
		Disabled,
		Idle,
		Running
	}

	[KSPField]
	public float driveResponse = 3f;

	[KSPField]
	public double idleDrain = 0.3;

	[KSPField]
	public float wheelSpeedMax = 120f;

	[KSPField(isPersistant = true)]
	public bool autoTorque = true;

	[KSPField(guiActive = true, guiName = "#autoLOC_7000070")]
	public string motorStateString = "Ready";

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_7000070")]
	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	public bool motorEnabled = true;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001462")]
	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6001075", scene = UI_Scene.All, enabledText = "#autoLOC_6001076", affectSymCounterparts = UI_Scene.All)]
	public bool motorInverted;

	[KSPField(guiFormat = "0.0", isPersistant = true, guiActive = false, guiName = "#autoLOC_6001463", guiUnits = "%")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float driveLimiter = 100f;

	[KSPField(guiFormat = "0.0", isPersistant = true, guiActive = false, guiName = "#autoLOC_6001464")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, maxValue = 5f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float tractionControlScale = 1f;

	[KSPField(guiFormat = "0.0", guiActive = true, guiName = "#autoLOC_7000070", guiUnits = "%")]
	[UI_ProgressBar(controlEnabled = true, scene = UI_Scene.Flight, maxValue = 100f, minValue = 0f)]
	public float driveOutput;

	public float driveInput;

	public float maxTorque;

	public MotorState state;

	public double resourceFraction;

	public BaseEvent evtAutoTorqueToggle;

	public BaseField fldDriveLimiter;

	public BaseField fldTractionControl;

	public double avgResRate;

	[KSPField]
	public FloatCurve torqueCurve;

	public List<PartResourceDefinition> consumedResources;

	public List<AdjusterWheelMotorBase> adjusterCache = new List<AdjusterWheelMotorBase>();

	public static string cacheAutoLOC_247970;

	public static string cacheAutoLOC_247987;

	public static string cacheAutoLOC_6001071;

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public override void OnAwake()
	{
		if (torqueCurve == null)
		{
			torqueCurve = new FloatCurve();
		}
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
		base.OnAwake();
	}

	public override void OnStart(StartState state)
	{
		avgResRate = resHandler.GetAverageInput();
		base.OnStart(state);
		evtAutoTorqueToggle = base.Events["EvtAutoTorqueToggle"];
		fldDriveLimiter = base.Fields["driveLimiter"];
		fldTractionControl = base.Fields["tractionControlScale"];
		evtAutoTorqueToggle.guiName = Localizer.Format("#autoLOC_7001004", Convert.ToInt32(autoTorque));
		ActionUIUpdate();
	}

	public override void OnWheelSetup()
	{
		torqueCurve.FindMinMaxValue(out var _, out maxTorque);
		wheel.maxDriveTorque = maxTorque;
		wheel.maxDriveTorque = (float)Math.Round(wheel.maxDriveTorque * 100f) / 100f;
		wheel.maxRpm = WheelUtil.SpeedToRpm(wheelSpeedMax, wheelBase.radius);
		wheel.driveResponse = driveResponse;
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || !baseSetup)
		{
			return;
		}
		if (motorEnabled && !base.part.packed)
		{
			driveInput = Mathf.Clamp(base.vessel.ctrlState.wheelThrottle + base.vessel.ctrlState.wheelThrottleTrim, -1f, 1f);
			driveInput *= GetMotorOrientationSign();
			driveInput *= (motorInverted ? (-1f) : 1f);
			wheel.maxDriveTorque = torqueCurve.Evaluate(Mathf.Abs(wheel.currentState.angularVelocity * wheelBase.radius));
			wheel.maxDriveTorque = (float)Math.Round(wheel.maxDriveTorque * 100f) / 100f;
			driveInput = Mathf.Clamp(OnDriveUpdate(driveInput), -1f, 1f);
			if (autoTorque)
			{
				driveInput *= 1f - Mathf.Abs(wheelBase.slipDisplacement.y * tractionControlScale);
				driveLimiter = Mathf.Lerp(1f, (float)base.vessel.mainBody.GeeASL, Mathf.Clamp01(tractionControlScale / 2f)) * 100f;
			}
			driveInput *= driveLimiter * 0.01f;
			if (maxTorque > 0f)
			{
				driveInput = ApplyTorqueAdjustments(driveInput);
				if (CheckMotorState(Mathf.Abs(driveInput), ref motorStateString, out resourceFraction, wheel.maxDriveTorque / maxTorque))
				{
					wheel.driveInput = driveInput * (float)resourceFraction;
				}
				else
				{
					wheel.driveInput = 0f;
				}
				driveOutput = Mathf.Abs(driveInput * wheel.maxDriveTorque / maxTorque) * 100f * (float)resourceFraction;
			}
			else
			{
				driveOutput = 0f;
				state = MotorState.Disabled;
			}
		}
		else
		{
			driveInput = 0f;
			driveOutput = 0f;
			wheel.driveInput = 0f;
		}
	}

	public virtual float OnDriveUpdate(float motorInput)
	{
		return motorInput;
	}

	public float GetMotorOrientationSign()
	{
		float num = Vector3.Dot(base.part.partTransform.up, base.vessel.ReferenceTransform.up);
		float num2 = Vector3.Dot(base.part.partTransform.forward, base.vessel.ReferenceTransform.forward);
		float num3 = Vector3.Dot(base.part.partTransform.forward, base.vessel.ReferenceTransform.up);
		float num4 = float.MinValue;
		float f = num;
		float num5 = Mathf.Abs(num3);
		if (num5 > num4)
		{
			num4 = num5;
			f = num3;
		}
		num5 = Mathf.Abs(num2);
		if (num5 > num4)
		{
			num4 = num5;
			f = num2;
		}
		return Mathf.Sign(f);
	}

	public bool CheckMotorState(float driveInput, ref string stateString, out double rOutput, float torqueScalar)
	{
		if (GetMotorEnabled(motorEnabled, ref stateString))
		{
			double num = (double)driveInput * (double)torqueScalar + idleDrain;
			rOutput = resHandler.UpdateModuleResourceInputs(ref stateString, num, 0.999, returnOnFirstLack: false, average: false, stringOps: false);
			if (rOutput > 0.5)
			{
				if (driveInput == 0f)
				{
					if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
					{
						stateString = Localizer.Format("#autoLOC_247957", (num * avgResRate).ToString("0.0"));
					}
					state = MotorState.Idle;
					return true;
				}
				if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
				{
					stateString = Localizer.Format("#autoLOC_247963", (num * avgResRate).ToString("0.0"));
				}
				state = MotorState.Running;
				return true;
			}
			if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
			{
				stateString = cacheAutoLOC_247970;
			}
			state = MotorState.NotEnoughResources;
			return rOutput > 0.1;
		}
		rOutput = 0.0;
		return false;
	}

	public virtual bool GetMotorEnabled(bool baseMotorEnabled, ref string stateString)
	{
		if (wheelBase.InopSystems.HasType(WheelSubsystem.SystemTypes.Motor))
		{
			stateString = cacheAutoLOC_247987;
			state = MotorState.Inoperable;
			return false;
		}
		if (!baseMotorEnabled)
		{
			stateString = cacheAutoLOC_6001071;
			state = MotorState.Disabled;
		}
		return baseMotorEnabled;
	}

	public float GetMaxSpeed()
	{
		return torqueCurve.maxTime;
	}

	public override string OnGatherInfo()
	{
		return Localizer.Format("#autoLOC_248010", GetMaxSpeed().ToString("0.0"), resHandler.PrintModuleResources());
	}

	[KSPEvent(active = true, guiActive = true, guiActiveEditor = true, guiName = "")]
	public void EvtAutoTorqueToggle()
	{
		autoTorque = !autoTorque;
		ActionUIUpdate();
		ATsymPartUpdate();
	}

	[KSPAction("#autoLOC_6002419")]
	public void MotorToggle(KSPActionParam act)
	{
		motorEnabled = !motorEnabled;
	}

	[KSPAction("#autoLOC_6002485")]
	public void MotorEnable(KSPActionParam act)
	{
		motorEnabled = true;
	}

	[KSPAction("#autoLOC_6002486")]
	public void MotorDisable(KSPActionParam act)
	{
		motorEnabled = false;
	}

	[KSPAction("#autoLOC_6001464")]
	public void ActAutoTorqueToggle(KSPActionParam act)
	{
		autoTorque = act.type != 0 || (act.type == KSPActionType.Toggle && !autoTorque);
		ActionUIUpdate();
	}

	public void ActionUIUpdate()
	{
		if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
		{
			evtAutoTorqueToggle.guiName = Localizer.Format("#autoLOC_7001004", Convert.ToInt32(autoTorque));
		}
		fldDriveLimiter.guiActive = !autoTorque;
		fldDriveLimiter.guiActiveEditor = !autoTorque;
		fldTractionControl.guiActive = autoTorque;
		fldTractionControl.guiActiveEditor = autoTorque;
	}

	public void ATsymPartUpdate()
	{
		int i = 0;
		for (int count = base.part.symmetryCounterparts.Count; i < count; i++)
		{
			ModuleWheelMotor obj = base.part.symmetryCounterparts[i].Modules[base.part.Modules.IndexOf(this)] as ModuleWheelMotor;
			obj.autoTorque = autoTorque;
			obj.ActionUIUpdate();
		}
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterWheelMotorBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterWheelMotorBase item = adjuster as AdjusterWheelMotorBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public float ApplyTorqueAdjustments(float torque)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			torque = adjusterCache[i].ApplyTorqueAdjustment(torque);
		}
		return torque;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004216");
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_247970 = Localizer.Format("#autoLOC_247970");
		cacheAutoLOC_247987 = Localizer.Format("#autoLOC_247987");
		cacheAutoLOC_6001071 = Localizer.Format("#autoLOC_6001071");
	}
}
