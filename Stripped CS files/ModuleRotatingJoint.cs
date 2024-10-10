using System;
using UnityEngine;

public class ModuleRotatingJoint : ModuleJointMotor
{
	public enum DriveModes
	{
		DRIVE,
		NEUTRAL,
		LOCKED
	}

	public enum ControlModes
	{
		ActionGroups,
		Throttle,
		Pitch,
		Roll,
		Yaw
	}

	public DriveModes driveMode = DriveModes.LOCKED;

	public ControlModes controlMode;

	public bool debug;

	[KSPField]
	public float maxTorque = 150f;

	[KSPField]
	public float minimumTorque = 3f;

	[KSPField]
	public float targetSpeed;

	[KSPField]
	public float maxSpeed = 20f;

	[KSPField]
	public float minimumSpeed = 0.1f;

	public float motorThrottle;

	[KSPField(isPersistant = true)]
	public float invertDrive = 1f;

	[KSPField(isPersistant = true)]
	public bool driveEngaged;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 1f, minValue = 0f)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001843")]
	public float torqueRatio = 1f;

	[KSPField(isPersistant = true)]
	public string savedMode = "LOCKED";

	[KSPField(isPersistant = true)]
	public string savedControlMode = "ActionGroups";

	public float effectiveTopSpeed => Mathf.Lerp(maxSpeed, minimumSpeed, torqueRatio);

	public float effectiveTorque => Mathf.Lerp(minimumTorque, maxTorque, torqueRatio);

	[KSPAction("Engage Drive")]
	public void EngageAction(KSPActionParam param)
	{
		EngageDrive();
	}

	[KSPAction("Lock Drive")]
	public void LockAction(KSPActionParam param)
	{
		LockDrive();
	}

	[KSPAction("Neutral Drive")]
	public void NeutralAction(KSPActionParam param)
	{
		NeutralDrive();
	}

	[KSPAction("Invert Drive")]
	public void InvertAction(KSPActionParam param)
	{
		InvertDrive();
	}

	[KSPAction("Rotate")]
	public void ForwardDrive(KSPActionParam param)
	{
		if (driveMode != 0)
		{
			EngageDrive();
		}
		SetMotorSpeed(effectiveTopSpeed * invertDrive);
	}

	[KSPAction("Counter-Rotate")]
	public void ReverseDrive(KSPActionParam param)
	{
		if (driveMode != 0)
		{
			EngageDrive();
		}
		SetMotorSpeed((0f - effectiveTopSpeed) * invertDrive);
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001838")]
	public void EngageDrive()
	{
		if (!HighLogic.LoadedSceneIsEditor)
		{
			SetMotorMode(Mode.Drive);
		}
		base.Events["LockDrive"].active = true;
		base.Events["EngageDrive"].active = false;
		base.Events["NeutralDrive"].active = true;
		driveMode = DriveModes.DRIVE;
		UpdateSavedState();
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001844")]
	public void LockDrive()
	{
		if (!HighLogic.LoadedSceneIsEditor)
		{
			SetMotorMode(Mode.Park);
		}
		base.Events["LockDrive"].active = false;
		base.Events["EngageDrive"].active = true;
		base.Events["NeutralDrive"].active = true;
		driveMode = DriveModes.LOCKED;
		UpdateSavedState();
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001845")]
	public void NeutralDrive()
	{
		if (!HighLogic.LoadedSceneIsEditor)
		{
			SetMotorMode(Mode.Neutral);
		}
		base.Events["LockDrive"].active = true;
		base.Events["EngageDrive"].active = true;
		base.Events["NeutralDrive"].active = false;
		driveMode = DriveModes.NEUTRAL;
		UpdateSavedState();
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001846")]
	public void InvertDrive()
	{
		invertDrive = 0f - invertDrive;
		UpdateGuiNames();
	}

	[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = " ")]
	public void ToggleControlMode()
	{
		switch (controlMode)
		{
		case ControlModes.ActionGroups:
			controlMode = ControlModes.Pitch;
			break;
		case ControlModes.Throttle:
			controlMode = ControlModes.ActionGroups;
			break;
		case ControlModes.Pitch:
			controlMode = ControlModes.Roll;
			break;
		case ControlModes.Roll:
			controlMode = ControlModes.Yaw;
			break;
		case ControlModes.Yaw:
			controlMode = ControlModes.Throttle;
			break;
		}
		UpdateGuiNames();
		UpdateSavedState();
	}

	public void UpdateSavedState()
	{
		savedMode = driveMode.ToString();
		savedControlMode = controlMode.ToString();
	}

	public void UpdateGuiNames()
	{
		if (invertDrive == 1f)
		{
			base.Events["InvertDrive"].guiName = "#autoLOC_6001847";
		}
		else
		{
			base.Events["InvertDrive"].guiName = "#autoLOC_6001848";
		}
		base.Events["ToggleControlMode"].guiName = controlMode.ToString();
	}

	public override void OnUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && GetMotorMode() != 0 && driveMode == DriveModes.DRIVE)
		{
			switch (controlMode)
			{
			case ControlModes.Throttle:
				motorThrottle = base.vessel.ctrlState.mainThrottle;
				SetMotorSpeed(effectiveTopSpeed * invertDrive * motorThrottle);
				break;
			case ControlModes.Pitch:
				motorThrottle = base.vessel.ctrlState.pitch;
				SetMotorSpeed(effectiveTopSpeed * invertDrive * motorThrottle);
				break;
			case ControlModes.Roll:
				motorThrottle = base.vessel.ctrlState.roll;
				SetMotorSpeed(effectiveTopSpeed * invertDrive * motorThrottle);
				break;
			case ControlModes.Yaw:
				motorThrottle = base.vessel.ctrlState.yaw;
				SetMotorSpeed(effectiveTopSpeed * invertDrive * motorThrottle);
				break;
			case ControlModes.ActionGroups:
				break;
			}
		}
	}

	public override void OnModuleSave(ConfigNode node)
	{
		UpdateGuiNames();
	}

	public override void OnModuleLoad(ConfigNode node)
	{
		driveMode = (DriveModes)Enum.Parse(typeof(DriveModes), savedMode);
		controlMode = (ControlModes)Enum.Parse(typeof(ControlModes), savedControlMode);
	}

	public override void OnModuleStart(StartState st)
	{
	}

	public override void OnJointInit(bool goodSetup)
	{
		if (goodSetup)
		{
			if (debug)
			{
				Debug.Log("[ModuleJointMotor]: Started With a valid joint.");
			}
			switch (driveMode)
			{
			case DriveModes.DRIVE:
				EngageDrive();
				break;
			case DriveModes.NEUTRAL:
				NeutralDrive();
				break;
			case DriveModes.LOCKED:
				LockDrive();
				break;
			}
			UpdateGuiNames();
			SetMotorForce(effectiveTorque);
		}
		else if (debug)
		{
			Debug.Log("[ModuleJointMotor]: Started, no valid joint on reference node " + base.refNode.id);
		}
	}

	public override void OnMotorModeChanged(Mode mode)
	{
		if (debug)
		{
			Debug.Log("[ModuleJointMotor]: Mode changed to ");
		}
	}
}
