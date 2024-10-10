using UnityEngine;

public class EVACamera : MonoBehaviour
{
	public float minPitch = -30f;

	public float maxPitch = 60f;

	public float startDistance = 30f;

	public float maxDistance = 35f;

	public float minDistance = 3f;

	public float distance;

	public GameObject pivot;

	public Vector3 endPos;

	public Quaternion endRot;

	public float orbitSensitivity = 0.05f;

	public float mouseZoomSensitivity = 0.1f;

	public float sharpness = 0.2f;

	public float camPitch;

	public float camHdg;

	public float tgtHdg;

	public Vessel target;

	public float Distance => distance;

	public Quaternion pivotRotation => pivot.transform.rotation;

	public Vector3 pivotPosition => pivot.transform.position;

	public void Start()
	{
		pivot = GameObject.Find("main camera pivot");
		if (!pivot)
		{
			pivot = new GameObject();
			pivot.name = "main camera pivot";
			base.transform.parent = pivot.transform;
		}
		pivot.transform.localRotation = Quaternion.identity;
		distance = startDistance;
		orbitSensitivity = GameSettings.VAB_CAMERA_ORBIT_SENS;
		mouseZoomSensitivity = GameSettings.VAB_CAMERA_ZOOM_SENS;
	}

	public void FixedUpdate()
	{
		if (target != FlightGlobals.ActiveVessel)
		{
			target = FlightGlobals.ActiveVessel;
		}
		if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() != 0f)
		{
			distance = Mathf.Clamp(distance - GameSettings.AXIS_MOUSEWHEEL.GetAxis() * 5f, minDistance, maxDistance);
		}
		if (GameSettings.ZOOM_IN.GetKey())
		{
			distance = Mathf.Clamp(distance - mouseZoomSensitivity, minDistance, maxDistance);
		}
		if (GameSettings.ZOOM_OUT.GetKey())
		{
			distance = Mathf.Clamp(distance + mouseZoomSensitivity, minDistance, maxDistance);
		}
		if (Input.GetMouseButton(1))
		{
			camHdg += Input.GetAxis("Mouse X") * orbitSensitivity;
			camPitch -= Input.GetAxis("Mouse Y") * orbitSensitivity;
		}
		camHdg -= GameSettings.AXIS_CAMERA_HDG.GetAxis() * orbitSensitivity;
		camPitch -= GameSettings.AXIS_CAMERA_PITCH.GetAxis() * orbitSensitivity;
		if (GameSettings.CAMERA_ORBIT_UP.GetKey() && !Input.GetKey(KeyCode.LeftShift))
		{
			camPitch += 1f * Time.deltaTime;
		}
		if (GameSettings.CAMERA_ORBIT_DOWN.GetKey() && !Input.GetKey(KeyCode.LeftShift))
		{
			camPitch -= 1f * Time.deltaTime;
		}
		if (GameSettings.CAMERA_ORBIT_LEFT.GetKey() && !Input.GetKey(KeyCode.LeftShift))
		{
			camHdg += 1f * Time.deltaTime;
		}
		if (GameSettings.CAMERA_ORBIT_RIGHT.GetKey() && !Input.GetKey(KeyCode.LeftShift))
		{
			camHdg -= 1f * Time.deltaTime;
		}
		camPitch = Mathf.Clamp(camPitch, Mathf.Max(minPitch, Mathf.Atan2(0.1f, distance)), maxPitch);
		endRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.ReferenceTransform.forward, target.upAxis).normalized, target.upAxis);
		tgtHdg = (Quaternion.FromToRotation(target.upAxis, Vector3.up) * endRot).eulerAngles.y;
		endRot *= Quaternion.AngleAxis(camHdg * 57.29578f - tgtHdg, Vector3.up);
		endRot *= Quaternion.AngleAxis(camPitch * 57.29578f, Vector3.right);
		pivot.transform.position = target.transform.position + target.rb_velocity * Time.deltaTime;
		pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, endRot, sharpness * Time.deltaTime);
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, Vector3.back * distance, sharpness * Time.deltaTime);
	}
}
