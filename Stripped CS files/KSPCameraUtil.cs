using System;
using UnityEngine;

public class KSPCameraUtil
{
	public static bool AnyCameraInputDown()
	{
		if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() == 0f && !GameSettings.ZOOM_IN.GetKey() && !GameSettings.ZOOM_OUT.GetKey() && !GameSettings.CAMERA_RESET.GetKey() && !GameSettings.CAMERA_ORBIT_UP.GetKey() && !GameSettings.CAMERA_ORBIT_DOWN.GetKey() && !GameSettings.CAMERA_ORBIT_RIGHT.GetKey() && !GameSettings.CAMERA_ORBIT_LEFT.GetKey() && !GameSettings.CAMERA_NEXT.GetKey() && !GameSettings.CAMERA_MODE.GetKey())
		{
			if (!Mouse.Right.GetButton() && !Mouse.Middle.GetButton())
			{
				return false;
			}
			return Mouse.IsMoving;
		}
		return true;
	}

	public static Vector3 GetCamCoordsFromPosition(Vector3 wPos, Vector3 refPos, Quaternion referenceRotation)
	{
		Vector3 vector = wPos - refPos;
		float magnitude = vector.magnitude;
		vector.Normalize();
		vector = Quaternion.Inverse(referenceRotation) * vector;
		float y = Mathf.Atan2(0f - vector.z, vector.x) - (float)Math.PI / 2f;
		return new Vector3(Mathf.Atan2(vector.y, Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z)), y, magnitude);
	}

	public static float GetDistanceToFit(Vector3 objSize, float cameraFOV)
	{
		return Mathf.Max(objSize.x, Mathf.Max(objSize.y, objSize.z)) * 0.5f * (1f / Mathf.Tan(cameraFOV * ((float)Math.PI / 180f) * 0.5f));
	}

	public static float GetDistanceToFit(float largestDimmension, float cameraFOV, int pixelWidth)
	{
		return Mathf.Max(1f, largestDimmension) * cameraFOV / ((float)pixelWidth * 0.7f);
	}
}
