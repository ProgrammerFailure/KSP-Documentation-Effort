using ns9;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelMotorSteering : ModuleWheelMotor
{
	[KSPField]
	public float steeringTorque = 50f;

	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001465")]
	public bool steeringEnabled = true;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001466")]
	[UI_Toggle(disabledText = "#autoLOC_6001075", scene = UI_Scene.All, enabledText = "#autoLOC_6001077", affectSymCounterparts = UI_Scene.All)]
	public bool steeringInvert;

	public Vector3 wFwd;

	public Vector3 Rcom;

	public Vector3 wXcom;

	public Vector3 steerAxisInput;

	public float wDot;

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
	}

	public override float OnDriveUpdate(float motorInput)
	{
		if (steeringEnabled)
		{
			wFwd = base.part.partTransform.forward;
			Rcom = (base.part.partTransform.position - base.vessel.CurrentCoM).normalized;
			wXcom = Vector3.Cross(wFwd, Rcom);
			steerAxisInput = -base.vessel.ReferenceTransform.forward * (base.vessel.ctrlState.wheelSteer + base.vessel.ctrlState.wheelSteerTrim);
			wDot = Vector3.Dot(wXcom, steerAxisInput);
			if (Mathf.Abs(wDot) > 0.1f)
			{
				wDot = Mathf.Sign(wDot);
				if (steeringInvert)
				{
					wDot *= -1f;
				}
				wheel.maxDriveTorque = steeringTorque;
			}
			else
			{
				wDot = 0f;
			}
			return motorInput + wDot;
		}
		return motorInput;
	}

	public override bool GetMotorEnabled(bool baseMotorEnabled, ref string stateString)
	{
		if (wheelBase.InopSystems.HasType(WheelSubsystem.SystemTypes.Steering))
		{
			stateString = "Inoperative";
			return false;
		}
		if (!baseMotorEnabled && !steeringEnabled)
		{
			stateString = "Disabled";
			return false;
		}
		return true;
	}

	public override string OnGatherInfo()
	{
		return Localizer.Format("#autoLOC_248141", base.OnGatherInfo());
	}
}
