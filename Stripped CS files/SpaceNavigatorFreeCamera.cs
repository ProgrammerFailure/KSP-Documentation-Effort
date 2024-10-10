using UnityEngine;

public class SpaceNavigatorFreeCamera : SpaceNavigatorCamera
{
	public Transform pivot;

	public Transform cam;

	public bool HorizonLock = true;

	public float pitch;

	public float roll;

	public float yaw;

	public float rollDominance;

	public Quaternion rotation;

	public Vector3 offset;

	public Vector3 translation;

	public Vector3 nextStep;

	public override void OnGetControl()
	{
		pivot = kspCam.GetPivot();
		cam = kspCam.GetCameraTransform();
		pitch = kspCam.getPitch();
		yaw = kspCam.getYaw();
		roll = 0f;
		cam.parent = null;
		offset = cam.position - pivot.position;
	}

	public override void OnCameraUpdate()
	{
		if (HorizonLock)
		{
			roll = SpaceNavigator.Rotation.Roll();
			rollDominance = Mathf.Clamp01(Mathf.Abs(roll) / 0.5f);
			roll = roll * 60f * sensRot * Time.deltaTime;
			pitch += SpaceNavigator.Rotation.Pitch() * 50f * (1f - rollDominance) * sensRot * Time.deltaTime;
			yaw += SpaceNavigator.Rotation.Yaw() * 50f * (1f - rollDominance) * sensRot * Time.deltaTime;
			cam.rotation = Quaternion.Slerp(cam.rotation, kspCam.getReferenceFrame() * Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(pitch, Vector3.right), sharpnessRot * Time.deltaTime);
			cam.localRotation = Quaternion.Slerp(cam.localRotation, base.transform.localRotation * Quaternion.AngleAxis(roll, Vector3.forward), sharpnessRot * Time.deltaTime);
		}
		else
		{
			rotation = Quaternion.Slerp(rotation, SpaceNavigator.Rotation, sharpnessRot * Time.deltaTime);
			cam.Rotate(rotation.eulerAngles, Space.Self);
		}
		cam.position = pivot.position + offset;
		nextStep = SpaceNavigator.Translation * translateSpeed * sensLin;
		translation = getNextTranslation(cam, Vector3.Lerp(translation, nextStep, sharpnessLin * Time.deltaTime));
		cam.Translate(translation * Time.deltaTime, Space.Self);
		offset = cam.position - pivot.position;
		cam.position = clampToRadius(cam.position - pivot.position) + pivot.position;
	}

	public override void OnCameraWantsControl()
	{
		cam.parent = pivot;
		kspCam.SetCamCoordsFromPosition(cam.position);
	}
}
