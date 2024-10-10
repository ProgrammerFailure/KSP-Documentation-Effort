using UnityEngine;

public class ModuleJointMotorTest : PartModule, IActiveJointHost, IJointLockState
{
	[KSPField(guiActive = true, guiName = "#autoLOC_6001836")]
	public string motorState = "";

	[KSPField(guiActive = true, guiName = "#autoLOC_900381")]
	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 15f, minValue = 0f)]
	public float motorSpeed = 0.05f;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0.1f)]
	[KSPField(guiActive = true, guiName = "#autoLOC_6001837")]
	public float motorForce = 10f;

	public float lastSpeed;

	public float lastForce;

	public bool driveReverse;

	public bool linkSpeedToThrottle;

	public ActiveJoint motorJoint;

	public bool jointStarted;

	public override void OnStart(StartState state)
	{
		motorJoint = ActiveJoint.Create(this, "", ActiveJoint.JointMode.Motor);
	}

	public void OnJointInit(ActiveJoint joint)
	{
		if (joint != null)
		{
			Debug.Log("[ModuleJointMotor]: Have a Joint.", base.gameObject);
			motorState = motorJoint.driveMode.ToString();
			jointStarted = true;
		}
		else
		{
			Debug.Log("[ModuleJointMotor]: No Joint Present", base.gameObject);
			motorState = motorJoint.driveMode.ToString();
			jointStarted = false;
		}
	}

	public Part GetHostPart()
	{
		return base.part;
	}

	public void OnDriveModeChanged(ActiveJoint.DriveMode mode)
	{
		motorState = mode.ToString();
		Debug.Log("[ModuleJointMotor]: Mode changed to " + motorState);
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001838")]
	public void MotorDrive()
	{
		driveReverse = false;
		SetMotorSpeed(Mathf.Abs(motorSpeed));
		motorJoint.SetDriveMode(ActiveJoint.DriveMode.Drive);
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001839")]
	public void MotorNeutral()
	{
		motorJoint.SetDriveMode(ActiveJoint.DriveMode.Neutral);
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001840")]
	public void MotorPark()
	{
		motorJoint.SetDriveMode(ActiveJoint.DriveMode.Park);
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001841")]
	public void Reverse()
	{
		driveReverse = true;
		SetMotorSpeed(0f - Mathf.Abs(motorSpeed));
		motorJoint.SetDriveMode(ActiveJoint.DriveMode.Drive);
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001842")]
	public void LinkToThrottle()
	{
		linkSpeedToThrottle = !linkSpeedToThrottle;
	}

	public void LateUpdate()
	{
		if (jointStarted)
		{
			if (motorSpeed != lastSpeed)
			{
				SetMotorSpeed(driveReverse ? (0f - Mathf.Abs(motorSpeed)) : motorSpeed);
				lastSpeed = motorSpeed;
			}
			if (motorForce != lastForce)
			{
				SetMotorForce(motorForce);
				lastForce = GetMotorForce();
			}
			if (linkSpeedToThrottle)
			{
				motorSpeed = 20f * base.vessel.ctrlState.mainThrottle;
			}
			motorJoint.DrawDebug();
		}
	}

	public void SetMotorSpeed(float speed)
	{
		if (motorJoint.driveMode == ActiveJoint.DriveMode.NoJoint)
		{
			Debug.LogError("[ModuleJointMotor]: Cannot set speed, no joint present.");
		}
		else
		{
			motorJoint.joint.targetAngularVelocity = Vector3.right * speed;
		}
	}

	public float GetMotorSpeed()
	{
		if (motorJoint.driveMode == ActiveJoint.DriveMode.NoJoint)
		{
			return 0f;
		}
		return motorJoint.joint.targetAngularVelocity.x;
	}

	public void SetMotorForce(float force)
	{
		if (motorJoint.driveMode == ActiveJoint.DriveMode.NoJoint)
		{
			Debug.LogError("[ModuleJointMotor]: Cannot set force, no joint present.");
			return;
		}
		motorForce = force;
		motorJoint.targetDrive.maximumForce = force;
		motorJoint.SetJointDrive(motorJoint.targetDrive);
	}

	public float GetMotorForce()
	{
		if (motorJoint.driveMode == ActiveJoint.DriveMode.NoJoint)
		{
			return 0f;
		}
		return motorJoint.targetDrive.maximumForce;
	}

	public Transform GetLocalTransform()
	{
		return base.part.partTransform;
	}

	public bool IsJointUnlocked()
	{
		if (motorJoint != null)
		{
			return motorJoint.driveMode != ActiveJoint.DriveMode.Park;
		}
		return false;
	}
}
