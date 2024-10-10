using System.Collections;
using CameraFXModules;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class InternalCamera : IGameCamera
{
	public float minZoom = 2f;

	public float baseZoom = 1f;

	public float maxZoom = 0.333f;

	public float modZoom = 0.5f;

	public float minPitch = -30f;

	public float maxPitch = 60f;

	public float maxRot = 60f;

	public float distanceCenter;

	public float distanceMaxRot = 1f;

	public float orbitFactor = 20f;

	public float currentZoom;

	public float currentFoV;

	public float currentPitch;

	public float currentRot;

	public float currentModZoom = 1f;

	public float tIRpitch;

	public float tIRyaw;

	public float tIRroll;

	public Vector3 tIRoffset;

	public float initialZoom;

	public Vector3 initialPosition;

	public Quaternion initialRotation;

	public Vector3 currentPosition;

	public Quaternion currentRotation;

	public Vector3 viewPointOffset;

	public float maxViewPointOffset = 0.1f;

	public float orbitSensitivity;

	public float mouseZoomSensitivity;

	public Camera _camera;

	public bool mouseLocked;

	public bool middleClickDouble;

	public static InternalCamera Instance { get; set; }

	public bool isActive => this.GetComponentCached(ref _camera).enabled;

	public void Awake()
	{
		Instance = this;
	}

	public virtual void Start()
	{
		orbitSensitivity = GameSettings.VAB_CAMERA_ORBIT_SENS * orbitFactor;
		mouseZoomSensitivity = GameSettings.VAB_CAMERA_ZOOM_SENS * 10f;
		initialZoom = this.GetComponentCached(ref _camera).fieldOfView;
		ManualReset(resetFov: true);
		DisableCamera();
	}

	public virtual void UpdateState()
	{
		currentRotation = initialRotation * Quaternion.Euler(currentPitch + tIRpitch * 57.29578f, currentRot + tIRyaw * 57.29578f, tIRroll * 57.29578f);
		currentPosition = initialPosition;
		if (CameraFX.Instance != null && CameraFX.Instance.cameraFXCollection_0.Count > 0)
		{
			currentPosition = CameraFX.Instance.cameraFXCollection_0.GetLocalPositionFX(initialPosition, 0.01f * GameSettings.CAMERA_FX_INTERNAL, Views.FlightInternal);
			currentRotation = CameraFX.Instance.cameraFXCollection_0.GetLocalRotationFX(currentRotation, 0.1f * GameSettings.CAMERA_FX_INTERNAL, Views.FlightInternal);
		}
		base.transform.localPosition = currentPosition + currentRotation * viewPointOffset;
		base.transform.localRotation = currentRotation;
		currentFoV = Mathf.Lerp(currentFoV, currentModZoom * initialZoom * currentZoom, Time.deltaTime * 10f);
		this.GetComponentCached(ref _camera).fieldOfView = currentFoV;
	}

	public virtual void Update()
	{
		if (!this.GetComponentCached(ref _camera).enabled || FlightDriver.Pause)
		{
			return;
		}
		if (CameraMouseLook.GetMouseLook(Input.GetMouseButtonDown(1)))
		{
			currentRot += Input.GetAxis("Mouse X") * orbitSensitivity;
			currentPitch -= Input.GetAxis("Mouse Y") * orbitSensitivity;
		}
		if (GameSettings.CAMERA_RESET.GetKeyDown())
		{
			ManualReset(resetFov: true);
		}
		if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() != 0f && !EventSystem.current.IsPointerOverGameObject())
		{
			currentZoom -= GameSettings.AXIS_MOUSEWHEEL.GetAxis() * mouseZoomSensitivity;
		}
		if (Input.GetMouseButtonDown(2) && !middleClickDouble)
		{
			StartCoroutine(MiddleDoubleClick());
		}
		if (Input.GetMouseButton(2))
		{
			currentModZoom = modZoom;
		}
		else
		{
			currentModZoom = 1f;
		}
		if (GameSettings.ZOOM_IN.GetKey())
		{
			currentZoom -= GameSettings.AXIS_MOUSEWHEEL.GetAxis() * mouseZoomSensitivity;
		}
		if (GameSettings.ZOOM_OUT.GetKey())
		{
			currentZoom += GameSettings.AXIS_MOUSEWHEEL.GetAxis() * mouseZoomSensitivity;
		}
		currentRot += GameSettings.AXIS_CAMERA_HDG.GetAxis() * orbitSensitivity;
		currentPitch -= GameSettings.AXIS_CAMERA_PITCH.GetAxis() * orbitSensitivity;
		if (SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice) && !FlightInputHandler.SPACENAV_USE_AS_FLIGHT_CONTROL)
		{
			currentRot += SpaceNavigator.Rotation.Yaw() * 50f * GameSettings.SPACENAV_CAMERA_SENS_ROT * Time.deltaTime;
			currentPitch += SpaceNavigator.Rotation.Pitch() * 50f * GameSettings.SPACENAV_CAMERA_SENS_ROT * Time.deltaTime;
			if (SpaceNavigator.Translation != Vector3.zero)
			{
				viewPointOffset = Vector3.Lerp(viewPointOffset, Vector3.ClampMagnitude(SpaceNavigator.Translation * GameSettings.SPACENAV_CAMERA_SENS_LIN * 0.03f, maxViewPointOffset), GameSettings.SPACENAV_CAMERA_SHARPNESS_LIN * Time.deltaTime);
			}
			else
			{
				viewPointOffset = Vector3.zero;
			}
		}
		else
		{
			viewPointOffset = Vector3.zero;
		}
		if (GameSettings.TRACKIR_ENABLED && GameSettings.TRACKIR.Instance.activeIVA)
		{
			tIRpitch = TrackIR.Instance.Pitch.GetAxis();
			tIRroll = TrackIR.Instance.Roll.GetAxis();
			tIRyaw = TrackIR.Instance.Yaw.GetAxis();
			viewPointOffset += Vector3.ClampMagnitude(TrackIR.Instance.HeadPosition * 0.2f, maxViewPointOffset);
		}
		if (GameSettings.CAMERA_ORBIT_UP.GetKey() && !Input.GetKey(KeyCode.LeftShift))
		{
			currentPitch -= orbitSensitivity * Time.deltaTime * 20f;
		}
		if (GameSettings.CAMERA_ORBIT_DOWN.GetKey() && !Input.GetKey(KeyCode.LeftShift))
		{
			currentPitch += orbitSensitivity * Time.deltaTime * 20f;
		}
		if (GameSettings.CAMERA_ORBIT_LEFT.GetKey() && !Input.GetKey(KeyCode.LeftShift))
		{
			currentRot -= orbitSensitivity * Time.deltaTime * 20f;
		}
		if (GameSettings.CAMERA_ORBIT_RIGHT.GetKey() && !Input.GetKey(KeyCode.LeftShift))
		{
			currentRot += orbitSensitivity * Time.deltaTime * 20f;
		}
		currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
		currentRot = Mathf.Clamp(currentRot, 0f - maxRot, maxRot);
		currentZoom = Mathf.Clamp(currentZoom, maxZoom, minZoom);
		UpdateState();
		FlightCamera.fetch.transform.position = InternalSpace.InternalToWorld(base.transform.position);
		FlightCamera.fetch.transform.rotation = InternalSpace.InternalToWorld(base.transform.rotation);
		FlightCamera.fetch.SetFoV(currentFoV);
	}

	public IEnumerator MiddleDoubleClick()
	{
		middleClickDouble = true;
		float endTime = Time.realtimeSinceStartup + 0.25f;
		while (Time.realtimeSinceStartup < endTime)
		{
			yield return null;
			if (Input.GetMouseButtonDown(2))
			{
				currentZoom = baseZoom;
				middleClickDouble = false;
				yield break;
			}
		}
		middleClickDouble = false;
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void SetTransform(Transform cameraTransform, bool resetCamera)
	{
		base.transform.parent = cameraTransform;
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		if (resetCamera)
		{
			ResetCamera();
		}
	}

	public void SetFOV(float fieldOfView)
	{
		this.GetComponentCached(ref _camera).fieldOfView = fieldOfView;
		initialZoom = fieldOfView;
		currentZoom = baseZoom;
	}

	public void SetTransform()
	{
		base.transform.parent = null;
	}

	public void ManualReset(bool resetFov)
	{
		currentPitch = 0f;
		currentRot = 0f;
		if (resetFov)
		{
			currentZoom = baseZoom;
		}
	}

	public override void ResetCamera()
	{
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		initialPosition = base.transform.localPosition;
		initialRotation = base.transform.localRotation;
		ManualReset(resetFov: false);
		FlightCamera.fetch.SetFoV(initialZoom);
	}

	public override void EnableCamera()
	{
		this.GetComponentCached(ref _camera).enabled = true;
	}

	public override void DisableCamera()
	{
		base.transform.parent = InternalSpace.Instance.transform;
		this.GetComponentCached(ref _camera).enabled = false;
	}
}
