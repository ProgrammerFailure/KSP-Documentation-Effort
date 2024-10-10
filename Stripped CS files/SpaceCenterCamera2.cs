using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceCenterCamera2 : MonoBehaviour, IKSPCamera
{
	public enum CameraMode
	{
		Rover,
		Overview
	}

	public string pqsName;

	public string initialPositionTransformName;

	public CameraMode mode = CameraMode.Overview;

	public KeyCode keyModeSwitch = KeyCode.Space;

	public KeyCode keyReset = KeyCode.Backspace;

	public bool useAlphaNumKeysFactor = true;

	public float rotationInitial = 90f;

	public float rotateSpeed = 90f;

	public float mouseSpeed = 90f;

	public float movementRadius = 1000f;

	public float elevationSpeed = 90f;

	public float elevationInitial = 45f;

	public float elevationMin;

	public float elevationMax = 85f;

	public float zoomSpeed = 90f;

	public float zoomInitial = 45f;

	public float zoomMin;

	public float zoomMax = 1000f;

	public float altitudeMin = 10f;

	public float altitudeMax = 10f;

	public float altitudeInitial = 10f;

	public GClass4 pqs;

	public Transform initialPosition;

	public Transform cameraTransform;

	public Transform t;

	public SurfaceObject srfPivot;

	public bool isActive = true;

	public float speed = 1f;

	public float mousePitch;

	public float zoom = 10f;

	public float altitude = 10f;

	public float rotationAngle;

	public float elevationAngle;

	public float height;

	public Vector3 pos = Vector3.zero;

	public float tIRpitch;

	public float tIRyaw;

	public float tIRroll;

	public Transform camPivot;

	bool IKSPCamera.enabled => base.enabled;

	public void Reset()
	{
		rotateSpeed = 90f;
		mouseSpeed = 90f;
	}

	public void Start()
	{
		t = base.transform;
		GameEvents.onGameSceneLoadRequested.Add(OnSceneSwitch);
		GClass4[] array = (GClass4[])UnityEngine.Object.FindObjectsOfType(typeof(GClass4));
		foreach (GClass4 gClass in array)
		{
			if (gClass.gameObject.name == pqsName)
			{
				pqs = gClass;
				break;
			}
		}
		if (pqs == null)
		{
			Debug.LogError("SpaceCenterCamera: Cannot find PQS of name '" + pqsName + "'");
			return;
		}
		initialPosition = pqs.transform.Find(initialPositionTransformName);
		if (initialPosition == null)
		{
			Debug.LogError("SpaceCenterCamera: Cannot find transform of name '" + initialPositionTransformName + "'");
			return;
		}
		t.NestToParent(initialPosition);
		cameraTransform = new GameObject("CameraTransform").transform;
		cameraTransform.NestToParent(base.transform);
		FlightCamera.fetch.transform.NestToParent(cameraTransform);
		FlightCamera.fetch.updateActive = false;
		FlightCamera.fetch.gameObject.SetActive(value: true);
		ResetCamera();
		srfPivot = SurfaceObject.Create(initialPosition.gameObject, FlightGlobals.currentMainBody, 3, KFSMUpdateMode.FIXEDUPDATE);
	}

	public void OnSceneSwitch(GameScenes scene)
	{
		srfPivot.Terminate();
		if (FlightCamera.fetch.transform.parent == cameraTransform)
		{
			FlightCamera.fetch.transform.parent = LocalSpace.fetch.transform;
		}
		isActive = false;
		pqs = null;
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		UnityEngine.Object.DestroyImmediate(this);
	}

	public void Update()
	{
		if (!(pqs == null) && !(t == null) && isActive && !InputLockManager.IsLocked(ControlTypes.CAMERACONTROLS))
		{
			if (Input.GetKeyDown(keyModeSwitch))
			{
				SwitchMode();
			}
			if (useAlphaNumKeysFactor)
			{
				InputSpeedFactor();
			}
			InputMovement();
			InputCamera();
			UpdateTransform();
		}
	}

	public void OnDestroy()
	{
		Cursor.visible = true;
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneSwitch);
	}

	public void SwitchMode()
	{
		if (mode == CameraMode.Rover)
		{
			mode = CameraMode.Overview;
		}
		else
		{
			mode = CameraMode.Rover;
		}
	}

	public void ResetCamera()
	{
		altitude = altitudeInitial;
		elevationAngle = elevationInitial;
		zoom = zoomInitial;
		rotationAngle = rotationInitial;
		pos = Vector3.zero;
		tIRpitch = 0f;
		tIRroll = 0f;
		tIRyaw = 0f;
	}

	public float GetZoom()
	{
		return zoom;
	}

	public void InputSpeedFactor()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			speed = 0.1f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			speed = 1f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			speed = 10f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			speed = 100f;
		}
	}

	public void InputMovement()
	{
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			Translate(new Vector3(0f, 0f, GetSpeed()));
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			Translate(new Vector3(0f, 0f, 0f - GetSpeed()));
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			Translate(new Vector3(0f - GetSpeed(), 0f, 0f));
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			Translate(new Vector3(GetSpeed(), 0f, 0f));
		}
		if (Input.GetKey(KeyCode.PageUp) || Input.GetKey(KeyCode.C))
		{
			Altitude(GetSpeed() / 1000f);
		}
		if (Input.GetKey(KeyCode.PageDown) || Input.GetKey(KeyCode.Z))
		{
			Altitude((0f - GetSpeed()) / 1000f);
		}
		if (Input.GetKey(KeyCode.X))
		{
			altitude = 10f;
		}
	}

	public void InputCamera()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			bool mouseLook;
			if ((mouseLook = CameraMouseLook.GetMouseLook()) || Input.GetMouseButton(2))
			{
				if (mouseLook)
				{
					Rotate(Input.GetAxis("Mouse X") * rotateSpeed);
					if (mode == CameraMode.Overview)
					{
						Elevate((0f - Input.GetAxis("Mouse Y")) * elevationSpeed);
					}
					else
					{
						Pitch((0f - Input.GetAxis("Mouse Y")) * elevationSpeed);
					}
				}
				else if (Input.GetMouseButton(2))
				{
					Translate(new Vector3(Input.GetAxis("Mouse X") * GetSpeed() * 10f, 0f, Input.GetAxis("Mouse Y") * GetSpeed() * 10f));
				}
			}
			float axis = GameSettings.AXIS_MOUSEWHEEL.GetAxis();
			if (axis != 0f)
			{
				Zoom(axis * (0f - zoomSpeed / Time.deltaTime));
			}
		}
		if (GameSettings.TRACKIR_ENABLED && GameSettings.TRACKIR.Instance.activeKSC)
		{
			tIRyaw = TrackIR.Instance.Yaw.GetAxis();
			tIRpitch = TrackIR.Instance.Pitch.GetAxis();
			tIRroll = TrackIR.Instance.Roll.GetAxis();
		}
		else
		{
			tIRpitch = 0f;
			tIRroll = 0f;
			tIRyaw = 0f;
		}
	}

	public float GetSpeed()
	{
		return speed * (Input.GetKey(KeyCode.LeftShift) ? 10f : 1f) * (Input.GetKey(KeyCode.LeftControl) ? 0.1f : 1f);
	}

	public void Pitch(float amount)
	{
		mousePitch += amount * Time.deltaTime;
		mousePitch = Mathf.Clamp(mousePitch, -85f, 85f);
	}

	public void Altitude(float amount)
	{
		altitude += amount * Time.deltaTime;
		altitude = Mathf.Clamp(altitude, altitudeMin, altitudeMax);
	}

	public void Rotate(float amount)
	{
		rotationAngle += amount * Time.deltaTime;
	}

	public void Elevate(float amount)
	{
		elevationAngle += amount * Time.deltaTime;
		elevationAngle = Mathf.Clamp(elevationAngle, elevationMin, elevationMax);
	}

	public void Zoom(float amount)
	{
		zoom += amount * Time.deltaTime;
		zoom = Math.Min(zoom, zoomMax);
		zoom = Math.Max(zoom, zoomMin);
	}

	public void Translate(Vector3 movement)
	{
		Vector3 vector = pos + Quaternion.AngleAxis(rotationAngle, Vector3.up) * movement;
		if (Vector3.SqrMagnitude(vector) < movementRadius * movementRadius)
		{
			pos = vector;
		}
		else
		{
			pos = vector.normalized * movementRadius;
		}
	}

	public void UpdateTransform()
	{
		if (!(pqs == null) && !(t == null))
		{
			if (mode == CameraMode.Rover)
			{
				UpdateTransformRover();
			}
			else
			{
				UpdateTransformOverview();
			}
		}
	}

	public void UpdateTransformRover()
	{
		t.localRotation = Quaternion.AngleAxis(rotationAngle, Vector3.up) * Quaternion.AngleAxis(mousePitch, Vector3.right);
		height = altitude + (float)(pqs.GetSurfaceHeight(pqs.transform.InverseTransformPoint(t.parent.TransformPoint(pos))) - pqs.radius);
		t.localPosition = pos + new Vector3(0f, height, 0f);
		cameraTransform.localPosition = Vector3.zero;
		cameraTransform.localRotation = Quaternion.identity;
	}

	public void UpdateTransformOverview()
	{
		t.localRotation = Quaternion.AngleAxis(rotationAngle, Vector3.up) * Quaternion.AngleAxis(mousePitch, Vector3.right);
		height = altitude + (float)(pqs.GetSurfaceHeight(pqs.transform.InverseTransformPoint(t.parent.TransformPoint(pos))) - pqs.radius);
		t.localPosition = pos + new Vector3(0f, height, 0f);
		Vector3 localPosition = Quaternion.AngleAxis(elevationAngle, Vector3.right) * new Vector3(0f, 0f, 0f - zoom);
		cameraTransform.localPosition = localPosition;
		cameraTransform.localRotation = Quaternion.AngleAxis(elevationAngle, Vector3.right);
		if (GameSettings.TRACKIR_ENABLED && GameSettings.TRACKIR.Instance.activeKSC)
		{
			cameraTransform.Rotate(Vector3.up, tIRyaw * 57.29578f, Space.Self);
			cameraTransform.Rotate(Vector3.right, tIRpitch * 57.29578f, Space.Self);
			cameraTransform.Rotate(Vector3.forward, tIRroll * 57.29578f, Space.Self);
		}
	}

	public Transform GetPivot()
	{
		return camPivot;
	}

	public Transform GetCameraTransform()
	{
		return cameraTransform;
	}

	public void SetCamCoordsFromPosition(Vector3 wPos)
	{
		camPivot.position = initialPosition.position;
		cameraTransform.position = wPos;
		Vector3 vector = wPos - GetPivot().position;
		float magnitude = vector.magnitude;
		vector.Normalize();
		vector = Quaternion.Inverse(GetPivot().rotation) * vector;
		rotationAngle = (Mathf.Atan2(0f - vector.z, vector.x) - (float)Math.PI / 2f) * 57.29578f;
		elevationAngle = Mathf.Atan2(vector.y, Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z)) * 57.29578f;
		zoom = magnitude;
	}

	public Func<bool> OnNavigatorTakeOver(Callback onRequestControl)
	{
		base.enabled = false;
		camPivot = cameraTransform.parent;
		return KSPCameraUtil.AnyCameraInputDown;
	}

	public void OnNavigatorHandoff()
	{
		base.enabled = true;
	}

	public Quaternion getReferenceFrame()
	{
		return FlightGlobals.GetFoR(FoRModes.SRF_NORTH, GetPivot());
	}

	public float getPitch()
	{
		return elevationAngle;
	}

	public float getYaw()
	{
		return rotationAngle;
	}

	public bool OnNavigatorRequestControl()
	{
		if (pqs != null)
		{
			return InputLockManager.IsUnlocked(ControlTypes.CAMERACONTROLS);
		}
		return false;
	}
}
