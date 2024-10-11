using UnityEngine;

public interface IActiveJointHost
{
	Part GetHostPart();

	Transform GetLocalTransform();

	void OnJointInit(ActiveJoint joint);

	void OnDriveModeChanged(ActiveJoint.DriveMode mode);
}
