using UnityEngine;

public class SpaceNavigatorLocalCamera : SpaceNavigatorCamera
{
	public Transform cam;

	public Vector3 translation;

	public Vector3 upAxis;

	public Quaternion referenceFrame;

	public Quaternion rotation;

	public float pitch;

	public float roll;

	public float yaw;

	public float rollDominance;

	public bool LockRoll;

	public bool useBounds;

	public Bounds bounds;

	public override void OnGetControl()
	{
		cam = kspCam.GetCameraTransform();
		referenceFrame = kspCam.getReferenceFrame();
		pitch = kspCam.getPitch();
		yaw = kspCam.getYaw();
	}

	public override void OnCameraUpdate()
	{
		if (LockRoll)
		{
			roll = SpaceNavigator.Rotation.Roll();
			rollDominance = Mathf.Clamp01(Mathf.Abs(roll) / 0.5f);
			roll = roll * 60f * sensRot * Time.deltaTime;
			pitch += SpaceNavigator.Rotation.Pitch() * 50f * (1f - rollDominance) * sensRot * Time.deltaTime;
			yaw += SpaceNavigator.Rotation.Yaw() * 50f * (1f - rollDominance) * sensRot * Time.deltaTime;
			rotation = referenceFrame * (Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(pitch, Vector3.right));
			rotation *= Quaternion.AngleAxis(roll, Vector3.forward);
			cam.rotation = Quaternion.Slerp(cam.rotation, rotation, sharpnessRot * Time.deltaTime);
		}
		else
		{
			rotation = Quaternion.Slerp(rotation, SpaceNavigator.Rotation, sharpnessRot * Time.deltaTime);
			cam.Rotate(rotation.eulerAngles, Space.Self);
		}
		translation = getNextTranslation(cam, Vector3.Lerp(translation, SpaceNavigator.Translation * translateSpeed * sensLin, sharpnessLin * Time.deltaTime));
		cam.Translate(translation * Time.deltaTime, Space.Self);
		cam.localPosition = clampToBounds(cam.localPosition);
		cam.localPosition = clampToRadius(cam.localPosition);
	}

	public override void OnCameraWantsControl()
	{
		kspCam.SetCamCoordsFromPosition(cam.position);
	}

	public Vector3 clampToBounds(Vector3 rPos)
	{
		if (useBounds && !bounds.Contains(rPos))
		{
			return new Vector3(Mathf.Clamp(rPos.x, bounds.min.x, bounds.max.x), Mathf.Clamp(rPos.y, bounds.min.y, bounds.max.y), Mathf.Clamp(rPos.z, bounds.min.z, bounds.max.z));
		}
		return rPos;
	}
}
