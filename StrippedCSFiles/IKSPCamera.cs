using System;
using UnityEngine;

public interface IKSPCamera
{
	bool enabled { get; }

	Transform GetPivot();

	Transform GetCameraTransform();

	void SetCamCoordsFromPosition(Vector3 wPos);

	bool OnNavigatorRequestControl();

	Func<bool> OnNavigatorTakeOver(Callback RequestControl);

	void OnNavigatorHandoff();

	Quaternion getReferenceFrame();

	float getPitch();

	float getYaw();
}
