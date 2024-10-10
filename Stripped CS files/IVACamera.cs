using UnityEngine;

[RequireComponent(typeof(Camera))]
public class IVACamera : IGameCamera
{
	public float minZoom = 1f;

	public float maxZoom = 0.333f;

	public float minPitch = -30f;

	public float maxPitch = 60f;

	public float maxRot = 60f;

	public float distanceCenter;

	public float distanceMaxRot = 1f;

	public float orbitFactor = 7.5f;

	public float currentZoom;

	public float currentPitch;

	public float currentRot;

	public float initialZoom;

	public Vector3 initialPosition;

	public Quaternion initialRotation;

	public Vector3 currentPosition;

	public Quaternion currentRotation;

	public float orbitSensitivity;

	public float mouseZoomSensitivity;

	[HideInInspector]
	public Kerbal kerbal;

	public Camera _camera;

	public bool isActive => this.GetComponentCached(ref _camera).enabled;

	public void Start()
	{
		initialPosition = base.transform.localPosition;
		initialRotation = base.transform.localRotation;
		initialZoom = this.GetComponentCached(ref _camera).fieldOfView;
		orbitSensitivity = GameSettings.VAB_CAMERA_ORBIT_SENS * orbitFactor;
		mouseZoomSensitivity = GameSettings.VAB_CAMERA_ZOOM_SENS;
		ResetState();
		this.GetComponentCached(ref _camera).enabled = false;
	}

	public void ResetState()
	{
		currentPitch = 0f;
		currentRot = 0f;
		currentZoom = minZoom;
		UpdateState();
	}

	public void UpdateState()
	{
		currentRotation = initialRotation * Quaternion.Euler(currentPitch, currentRot, 0f);
		currentPosition = initialPosition;
		base.transform.localPosition = currentPosition;
		base.transform.localRotation = currentRotation;
		this.GetComponentCached(ref _camera).fieldOfView = initialZoom * currentZoom;
	}

	public void Update()
	{
		if (this.GetComponentCached(ref _camera).enabled)
		{
			if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() != 0f)
			{
				currentZoom -= GameSettings.AXIS_MOUSEWHEEL.GetAxis() * mouseZoomSensitivity;
			}
			if (GameSettings.ZOOM_IN.GetKey())
			{
				currentZoom -= GameSettings.AXIS_MOUSEWHEEL.GetAxis() * mouseZoomSensitivity;
			}
			if (GameSettings.ZOOM_OUT.GetKey())
			{
				currentZoom += GameSettings.AXIS_MOUSEWHEEL.GetAxis() * mouseZoomSensitivity;
			}
			if (Input.GetMouseButton(1))
			{
				currentRot += Input.GetAxis("Mouse X") * orbitSensitivity;
				currentPitch -= Input.GetAxis("Mouse Y") * orbitSensitivity;
			}
			currentRot += GameSettings.AXIS_CAMERA_HDG.GetAxis() * orbitSensitivity;
			currentPitch -= GameSettings.AXIS_CAMERA_PITCH.GetAxis() * orbitSensitivity;
			if (GameSettings.CAMERA_ORBIT_UP.GetKey() && !Input.GetKey(KeyCode.LeftShift))
			{
				currentPitch -= orbitSensitivity * Time.deltaTime;
			}
			if (GameSettings.CAMERA_ORBIT_DOWN.GetKey() && !Input.GetKey(KeyCode.LeftShift))
			{
				currentPitch -= orbitSensitivity * Time.deltaTime;
			}
			if (GameSettings.CAMERA_ORBIT_LEFT.GetKey() && !Input.GetKey(KeyCode.LeftShift))
			{
				currentRot += orbitSensitivity * Time.deltaTime;
			}
			if (GameSettings.CAMERA_ORBIT_RIGHT.GetKey() && !Input.GetKey(KeyCode.LeftShift))
			{
				currentRot += orbitSensitivity * Time.deltaTime;
			}
			currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
			currentRot = Mathf.Clamp(currentRot, 0f - maxRot, maxRot);
			currentZoom = Mathf.Clamp(currentZoom, maxZoom, minZoom);
			UpdateState();
			FlightCamera.fetch.transform.position = kerbal.protoCrewMember.seat.internalModel.InternalToWorld(base.transform.position);
			FlightCamera.fetch.transform.rotation = kerbal.protoCrewMember.seat.internalModel.InternalToWorld(base.transform.rotation);
		}
	}

	public static void DeactivateAll()
	{
		IVACamera[] array = (IVACamera[])Object.FindObjectsOfType(typeof(IVACamera));
		int num = array.Length;
		while (num-- > 0)
		{
			array[num].ResetCamera();
			array[num].Deactivate();
		}
	}

	public void Activate()
	{
		this.GetComponentCached(ref _camera).enabled = true;
		UIPartActionController.Instance.Deactivate();
		FlightCamera.fetch.DeactivateUpdate();
	}

	public void Deactivate()
	{
		this.GetComponentCached(ref _camera).enabled = false;
		FlightCamera.fetch.ActivateUpdate();
		UIPartActionController.Instance.Activate();
	}

	public override void ResetCamera()
	{
		ResetState();
	}

	public override void EnableCamera()
	{
		this.GetComponentCached(ref _camera).enabled = true;
		FlightCamera.fetch.ActivateUpdate();
		int num = FlightCamera.fetch.cameras.Length;
		while (num-- > 0)
		{
			FlightCamera.fetch.cameras[num].enabled = true;
		}
	}

	public override void DisableCamera()
	{
		this.GetComponentCached(ref _camera).enabled = false;
		FlightCamera.fetch.DeactivateUpdate();
		int num = FlightCamera.fetch.cameras.Length;
		while (num-- > 0)
		{
			FlightCamera.fetch.cameras[num].enabled = false;
		}
	}
}
