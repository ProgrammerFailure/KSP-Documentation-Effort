using System;
using System.Collections;
using System.ComponentModel;
using CameraFXModules;
using Highlighting;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FlightCamera : MonoBehaviour, IKSPCamera
{
	public enum TargetMode
	{
		None,
		Vessel,
		Part,
		Transform
	}

	public enum Modes
	{
		[Description("#autoLOC_6003102")]
		AUTO,
		[Description("#autoLOC_6003103")]
		FREE,
		[Description("#autoLOC_6002359")]
		ORBITAL,
		[Description("#autoLOC_6003104")]
		CHASE,
		[Description("#autoLOC_6003105")]
		LOCKED
	}

	public enum ChaseMode
	{
		Parked,
		Velocity,
		Target_SrfUp,
		Target_YUp,
		Null
	}

	public float minPitch = -0.95f;

	public float maxPitch = 1.55f;

	public float startDistance = 30f;

	public float maxDistance = 150000f;

	public float minDistance = 3f;

	public float minDistOnDestroy = 75f;

	public float maxDistOnDestroy = 400f;

	public float minHeightAtMaxDist = 10000f;

	public float minHeightAtMinDist = 10f;

	public float minHeight;

	[SerializeField]
	public float distance;

	[SerializeField]
	public Transform pivot;

	[SerializeField]
	public Vector3 endPos;

	public Quaternion frameOfReference;

	public Quaternion tgtFoR;

	public Quaternion lastFoR;

	public float orbitSensitivity = 0.05f;

	public float zoomScaleFactor = 1.2f;

	public float sharpness = 10f;

	public float pivotTranslateSharpness = 0.5f;

	public float orientationSharpness = 0.05f;

	public float cameraWobbleSensitivity = 0.1f;

	public float camPitch;

	public float camHdg;

	public float cameraAlt;

	[SerializeField]
	public float camera00IVANearClipPlane = 0.1f;

	[SerializeField]
	public float camera00DefaultNearClipPlane = 0.21f;

	[SerializeField]
	public float terrainPitch;

	[SerializeField]
	public float meshPitch;

	public RaycastHit hit;

	public Transform target;

	public Vessel vesselTarget;

	public Part partTarget;

	public TargetMode targetMode;

	public Vector3 upAxis;

	public Vector3 tgtUpAxis;

	public Camera[] cameras;

	public Camera mainCamera;

	public GameObject AudioListenerGameObject;

	public AudioListener listener;

	public Rigidbody _rigidbody;

	public Modes mode;

	public Modes autoMode = Modes.FREE;

	public FoRModes FoRMode;

	public ScreenMessage cameraModeReadout;

	public static FlightCamera fetch;

	public FlightReflectionProbe reflectionProbe;

	[SerializeField]
	public float endPitch;

	[SerializeField]
	public float localPitch;

	[SerializeField]
	public float lastLocalPitch;

	public float offsetPitch;

	public float offsetHdg;

	public float tIRpitch;

	public float tIRyaw;

	public float tIRroll;

	public Vector3 camFXPos;

	public Quaternion camFXRot;

	public Vector3 nextMove;

	public Vector3 targetDirection;

	public Vector3 endDirection;

	public float FoRlerp;

	public Vector3 chaseFwd;

	public Vector3 chaseFUp;

	public Vector3 chaseOrt;

	public Quaternion chaseBaseRot;

	public ChaseMode chaseMode;

	public ChaseMode chaseModePrev = ChaseMode.Null;

	public Modes chaseBaseMode;

	public bool updateActive = true;

	public float fovDefault = 60f;

	public float fovMin = 20f;

	public float fovMax = 160f;

	public float FieldOfView = 60f;

	public float vesselSwitchBackoffFOVFactor = 1.5f;

	public float vesselSwitchBackoffPadding = 10f;

	public Callback AbortExternalControl = delegate
	{
	};

	public float Distance => distance;

	public Quaternion pivotRotation => pivot.rotation;

	public static float CamPitch
	{
		get
		{
			return fetch.camPitch;
		}
		set
		{
			fetch.camPitch = value;
			fetch.endPitch = value;
		}
	}

	public static float CamHdg
	{
		get
		{
			return fetch.camHdg;
		}
		set
		{
			fetch.camHdg = value;
		}
	}

	public static int CamMode => (int)fetch.mode;

	public static FoRModes FrameOfReferenceMode => fetch.FoRMode;

	public Transform Target => target;

	bool IKSPCamera.enabled => base.enabled;

	public virtual void Awake()
	{
		if (fetch != null)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		fetch = this;
		cameraModeReadout = new ScreenMessage("", 3f, ScreenMessageStyle.UPPER_CENTER);
		listener = AudioListenerGameObject.GetComponent<AudioListener>();
		listener.enabled = false;
		GameEvents.onGameSceneLoadRequested.Add(OnSceneSwitch);
		GameEvents.onVesselChange.Add(OnVesselChange);
		GameEvents.OnCameraChange.Add(OnCameraChange);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public virtual void Start()
	{
		pivot = new GameObject("main camera pivot").transform;
		UnityEngine.Object.DontDestroyOnLoad(pivot.gameObject);
		base.transform.parent = pivot;
		mainCamera = cameras[0];
		int num = cameras.Length;
		while (num-- > 0)
		{
			Camera camera = cameras[num];
			if (GameSettings.GraphicsVersion == GameSettings.GraphicsType.D3D11)
			{
				if (camera.name == "Camera 00")
				{
					camera.farClipPlane = 750000f;
				}
				if (camera.name == "Camera 01")
				{
					camera.gameObject.SetActive(value: false);
				}
			}
			else if (camera.name == "Camera 01")
			{
				camera.farClipPlane = 750000f;
			}
			if (camera.name == "Camera 00" || camera.name == "Camera 01")
			{
				UnderwaterFog underwaterFog = camera.GetComponent<UnderwaterFog>();
				if (underwaterFog == null)
				{
					underwaterFog = camera.gameObject.AddComponent<UnderwaterFog>();
				}
				underwaterFog.isPQS = camera.name == ((GameSettings.GraphicsVersion == GameSettings.GraphicsType.D3D11) ? "Camera 00" : "Camera 01");
			}
		}
		reflectionProbe = FlightReflectionProbe.Spawn();
		reflectionProbe.transform.SetParent(pivot.transform);
	}

	public virtual IEnumerator Startup()
	{
		OnCameraChange(CameraManager.CameraMode.Flight);
		while (!FlightGlobals.ready)
		{
			yield return null;
		}
		camPitch = 0.2f;
		camHdg = 0.3f;
		offsetHdg = 0f;
		offsetPitch = 0f;
		mode = Modes.AUTO;
		distance = startDistance;
		AdjustDistanceToFit(FlightGlobals.ActiveVessel);
		base.transform.localPosition = Vector3.back * distance;
		base.transform.localRotation = Quaternion.identity;
		orbitSensitivity = GameSettings.FLT_CAMERA_ORBIT_SENS;
		zoomScaleFactor = GameSettings.FLT_CAMERA_ZOOM_SENS;
		cameraWobbleSensitivity = GameSettings.FLT_CAMERA_WOBBLE;
		lastFoR = FlightGlobals.GetFoR((GetAutoModeForVessel(FlightGlobals.ActiveVessel) == Modes.ORBITAL) ? FoRModes.OBT_ABS : FoRModes.SRF_NORTH);
		FoRlerp = 1f;
		SetTargetVessel(FlightGlobals.ActiveVessel);
		ResumeFoV();
	}

	public virtual void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneSwitch);
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.OnCameraChange.Remove(OnCameraChange);
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public virtual void OnLevelLoaded(GameScenes level)
	{
		StopCoroutine("Startup");
		if (base.gameObject.activeSelf)
		{
			StartCoroutine(Startup());
		}
	}

	public static void ClearTarget()
	{
		if ((bool)fetch)
		{
			fetch.SetTargetNone();
		}
	}

	public static void SetTarget(Vessel vessel)
	{
		if ((bool)fetch)
		{
			fetch.SetTargetVessel(vessel);
		}
	}

	public static void SetTarget(Part part)
	{
		if ((bool)fetch)
		{
			fetch.SetTargetPart(part);
		}
	}

	public static void SetTarget(Transform transform)
	{
		if ((bool)fetch)
		{
			fetch.SetTargetTransform(transform);
		}
	}

	public virtual void SetTargetNone()
	{
		SetTarget(null, TargetMode.None);
	}

	public virtual void SetTargetVessel(Vessel vessel)
	{
		SetTarget(vessel.vesselTransform, TargetMode.Vessel);
	}

	public virtual void SetTargetPart(Part part)
	{
		SetTarget(part.partTransform, TargetMode.Part);
	}

	public virtual void SetTargetTransform(Transform tgt)
	{
		SetTarget(tgt, TargetMode.Transform);
	}

	public virtual void SetTarget(Transform tgt, TargetMode targetMode = TargetMode.Transform)
	{
		SetTarget(tgt, keepWorldPos: false, targetMode);
	}

	public virtual void SetTarget(Transform tgt, bool keepWorldPos, TargetMode targetMode = TargetMode.Transform)
	{
		endPos.Zero();
		if (vesselTarget != null)
		{
			for (int i = 0; i < vesselTarget.parts.Count; i++)
			{
				vesselTarget.parts[i].LoseCameraAim();
			}
			Vessel vessel = vesselTarget;
			vessel.OnJustAboutToBeDestroyed = (Callback)Delegate.Remove(vessel.OnJustAboutToBeDestroyed, new Callback(OnTargetDestroyed));
			vesselTarget = null;
		}
		if (partTarget != null)
		{
			partTarget.LoseCameraAim();
			Part part = partTarget;
			part.OnJustAboutToBeDestroyed = (Callback)Delegate.Remove(part.OnJustAboutToBeDestroyed, new Callback(OnTargetDestroyed));
			partTarget = null;
		}
		AbortExternalControl();
		if (targetMode != 0 && tgt != null)
		{
			target = tgt;
			if (targetMode == TargetMode.Vessel && (vesselTarget = target.GetComponent<Vessel>()) != null)
			{
				this.targetMode = TargetMode.Vessel;
				Vessel vessel2 = vesselTarget;
				vessel2.OnJustAboutToBeDestroyed = (Callback)Delegate.Combine(vessel2.OnJustAboutToBeDestroyed, new Callback(OnTargetDestroyed));
				if (vesselTarget.isEVA)
				{
					cameraWobbleSensitivity = 0f;
				}
				else
				{
					cameraWobbleSensitivity = GameSettings.FLT_CAMERA_WOBBLE;
				}
				endPos = vesselTarget.localCoM;
				pivot.SetParent(target, keepWorldPos);
				if (!keepWorldPos)
				{
					pivot.localPosition = endPos;
				}
			}
			else if ((partTarget = target.GetComponent<Part>()) != null)
			{
				this.targetMode = TargetMode.Part;
				Part part2 = partTarget;
				part2.OnJustAboutToBeDestroyed = (Callback)Delegate.Combine(part2.OnJustAboutToBeDestroyed, new Callback(OnTargetDestroyed));
				partTarget.GainCameraAim();
				cameraWobbleSensitivity = GameSettings.FLT_CAMERA_WOBBLE;
				endPos = partTarget.CoMOffset;
				pivot.SetParent(target, keepWorldPos);
				if (!keepWorldPos)
				{
					pivot.localPosition = endPos;
				}
			}
			else
			{
				this.targetMode = TargetMode.Transform;
				endPos.Zero();
				pivot.SetParent(target, worldPositionStays: false);
				pivot.localPosition = endPos;
				distance = Mathf.Clamp(distance, minDistOnDestroy, maxDistOnDestroy);
			}
		}
		else
		{
			this.targetMode = TargetMode.None;
			target = pivot;
			distance = Mathf.Clamp(distance, minDistOnDestroy, maxDistOnDestroy);
			endPos = pivot.position;
			pivot.SetParent(null, worldPositionStays: true);
			mode = Modes.FREE;
		}
	}

	public virtual void SetDistance(float dist)
	{
		distance = Mathf.Clamp(dist, minDistance, maxDistance);
	}

	public virtual void SetDistanceImmediate(float dist)
	{
		distance = Mathf.Clamp(dist, minDistance, maxDistance);
		base.transform.localPosition = Vector3.back * distance;
	}

	public static void SetMode(Modes m)
	{
		fetch.setMode(m);
	}

	public virtual void setMode(Modes m)
	{
		mode = m;
		MonoBehaviour.print("Camera Mode: " + mode);
		cameraModeReadout.message = Localizer.Format("#autoLOC_133776", mode.displayDescription());
		ScreenMessages.PostScreenMessage(cameraModeReadout);
		if (mode == Modes.AUTO)
		{
			autoMode = GetAutoModeForVessel(FlightGlobals.ActiveVessel);
		}
		lastFoR = frameOfReference;
		FoRlerp = 0f;
		offsetHdg = 0f;
		offsetPitch = 0f;
		GameEvents.OnFlightCameraAngleChange.Fire(m);
	}

	public static void SetModeImmediate(Modes m)
	{
		fetch.setModeImmediate(m);
	}

	public virtual void setModeImmediate(Modes m)
	{
		setMode(m);
		FoRlerp = 1f;
	}

	public void SetNextMode()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.CAMERAMODES))
		{
			if (targetMode != TargetMode.Vessel || vesselTarget != FlightGlobals.ActiveVessel)
			{
				SetTargetVessel(FlightGlobals.ActiveVessel);
			}
			setMode((Modes)((int)(mode + 1) % 5));
		}
	}

	public virtual bool TrackIRisActive()
	{
		Vessel vessel = vesselTarget;
		if (vessel == null && partTarget != null)
		{
			vessel = partTarget.vessel;
		}
		if (vessel != null)
		{
			if (!vessel.isEVA)
			{
				return GameSettings.TRACKIR.Instance.activeFlight;
			}
			return GameSettings.TRACKIR.Instance.activeEVA;
		}
		return false;
	}

	public virtual void LateUpdate()
	{
		if (vesselTarget != null)
		{
			cameraAlt = FlightGlobals.getAltitudeAtPos(base.transform.position, vesselTarget.mainBody);
		}
		else if (partTarget != null)
		{
			cameraAlt = FlightGlobals.getAltitudeAtPos(base.transform.position, partTarget.vessel.mainBody);
			if (partTarget.vessel != FlightGlobals.ActiveVessel && (base.transform.position - FlightGlobals.ActiveVessel.transform.position).sqrMagnitude > 4000000f)
			{
				SetTargetVessel(FlightGlobals.ActiveVessel);
				return;
			}
		}
		else
		{
			cameraAlt = FlightGlobals.getAltitudeAtPos(base.transform.position);
		}
		if (!updateActive || target == null || (FlightDriver.Pause && UIMasterController.Instance.IsUIShowing))
		{
			return;
		}
		nextMove = Vector3.zero;
		if (InputLockManager.IsUnlocked(ControlTypes.CAMERACONTROLS) || (FlightDriver.Pause && !UIMasterController.Instance.IsUIShowing))
		{
			if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() != 0f && !EventSystem.current.IsPointerOverGameObject())
			{
				if (GameSettings.MODIFIER_KEY.GetKey())
				{
					FieldOfView = Mathf.Clamp(FieldOfView + GameSettings.AXIS_MOUSEWHEEL.GetAxis() * 5f, fovMin, fovMax);
					SetFoV(FieldOfView);
				}
				else
				{
					nextMove.z = Mathf.Clamp(distance * (1f - GameSettings.AXIS_MOUSEWHEEL.GetAxis()), minDistance, maxDistance) - distance;
				}
			}
			if (GameSettings.ZOOM_IN.GetKey())
			{
				if (GameSettings.MODIFIER_KEY.GetKey())
				{
					FieldOfView = Mathf.Clamp(FieldOfView + 5f * Time.unscaledDeltaTime, fovMin, fovMax);
					SetFoV(FieldOfView);
				}
				else
				{
					nextMove.z = Mathf.Clamp(distance / (1f + zoomScaleFactor * 0.04f), minDistance, maxDistance) - distance;
				}
			}
			if (GameSettings.ZOOM_OUT.GetKey())
			{
				if (GameSettings.MODIFIER_KEY.GetKey())
				{
					FieldOfView = Mathf.Clamp(FieldOfView - 5f * Time.unscaledDeltaTime, fovMin, fovMax);
					SetFoV(FieldOfView);
				}
				else
				{
					nextMove.z = Mathf.Clamp(distance * (1f + zoomScaleFactor * 0.04f), minDistance, maxDistance) - distance;
				}
			}
			if (CameraMouseLook.GetMouseLook())
			{
				nextMove.y = Input.GetAxis("Mouse X") * orbitSensitivity;
				nextMove.x = Input.GetAxis("Mouse Y") * orbitSensitivity;
			}
			nextMove.y += GameSettings.AXIS_CAMERA_HDG.GetAxis() * orbitSensitivity;
			nextMove.x += GameSettings.AXIS_CAMERA_PITCH.GetAxis() * orbitSensitivity;
			if (Input.GetMouseButton(2))
			{
				offsetHdg += Input.GetAxis("Mouse X") * orbitSensitivity * 0.5f;
				offsetPitch -= Input.GetAxis("Mouse Y") * orbitSensitivity * 0.5f;
			}
			if (Mouse.Middle.GetDoubleClick())
			{
				offsetHdg = 0f;
				offsetPitch = 0f;
				SetDefaultFoV();
			}
			offsetPitch = Mathf.Clamp(offsetPitch, (0f - mainCamera.fieldOfView) * 0.5f * ((float)Math.PI / 180f), mainCamera.fieldOfView * 0.5f * ((float)Math.PI / 180f));
			offsetHdg = Mathf.Clamp(offsetHdg, (0f - mainCamera.fieldOfView) * 0.5f * ((float)Math.PI / 180f), mainCamera.fieldOfView * 0.5f * ((float)Math.PI / 180f));
			if (GameSettings.TRACKIR_ENABLED && TrackIRisActive())
			{
				tIRyaw = TrackIR.Instance.Yaw.GetAxis();
				tIRpitch = TrackIR.Instance.Pitch.GetAxis();
				tIRroll = TrackIR.Instance.Roll.GetAxis();
			}
			else
			{
				tIRyaw = 0f;
				tIRpitch = 0f;
				tIRroll = 0f;
			}
			if (GameSettings.CAMERA_ORBIT_UP.GetKey())
			{
				nextMove.x = -1f * Time.unscaledDeltaTime;
			}
			if (GameSettings.CAMERA_ORBIT_DOWN.GetKey())
			{
				nextMove.x = 1f * Time.unscaledDeltaTime;
			}
			if (GameSettings.CAMERA_ORBIT_LEFT.GetKey())
			{
				nextMove.y = 1f * Time.unscaledDeltaTime;
			}
			if (GameSettings.CAMERA_ORBIT_RIGHT.GetKey())
			{
				nextMove.y = -1f * Time.unscaledDeltaTime;
			}
		}
		FoRlerp = Mathf.Clamp01(FoRlerp + Time.unscaledDeltaTime * orientationSharpness);
		switch (mode)
		{
		case Modes.AUTO:
		{
			switch (autoMode)
			{
			case Modes.ORBITAL:
				updateFoR(GetCameraFoR(FoRModes.OBT_ABS), FoRlerp);
				break;
			case Modes.FREE:
				updateFoR(GetCameraFoR(FoRModes.SRF_NORTH), FoRlerp);
				break;
			}
			Modes autoModeForVessel = GetAutoModeForVessel(FlightGlobals.ActiveVessel);
			if (autoMode != autoModeForVessel)
			{
				lastFoR = frameOfReference;
				FoRlerp = 0f;
				autoMode = autoModeForVessel;
			}
			break;
		}
		case Modes.FREE:
			updateFoR(GetCameraFoR(FoRModes.SRF_NORTH), FoRlerp);
			break;
		case Modes.ORBITAL:
			if (FlightGlobals.ActiveVessel.radarAltitude < 2000.0)
			{
				setMode((Modes)((int)(mode + 1) % 4));
			}
			else
			{
				updateFoR(GetCameraFoR(FoRModes.OBT_ABS), FoRlerp);
			}
			break;
		case Modes.CHASE:
			updateFoR(GetChaseFoR(FlightGlobals.ActiveVessel), FoRlerp);
			break;
		case Modes.LOCKED:
			if (FlightGlobals.ActiveVessel.isEVA)
			{
				setMode(Modes.FREE);
			}
			else
			{
				updateFoR(GetCameraFoR(FoRModes.SHP_REL), FoRlerp);
			}
			break;
		}
		if (FoRlerp != 1f)
		{
			SetCamCoordsFromPosition(base.transform.position);
		}
		camPitch -= nextMove.x;
		camHdg += nextMove.y;
		distance += nextMove.z;
		UpdateCameraTransform();
	}

	public virtual void UpdateCameraTransform()
	{
		upAxis = FlightGlobals.upAxis;
		terrainPitch = minPitch;
		meshPitch = minPitch;
		minHeight = Mathf.Lerp(minHeightAtMinDist, minHeightAtMaxDist, Mathf.InverseLerp(minDistance, maxDistance, distance));
		camPitch = Mathf.Min(maxPitch, camPitch);
		if ((FoRMode == FoRModes.SRF_HDG || FoRMode == FoRModes.SRF_NORTH || FoRMode == FoRModes.SRF_VEL) && Physics.Raycast(pivot.position + upAxis * distance, -upAxis, out hit, distance * 2f, 32768, QueryTriggerInteraction.Ignore))
		{
			meshPitch = Mathf.Atan2(FlightGlobals.getAltitudeAtPos(hit.point) + minHeight - FlightGlobals.getAltitudeAtPos(pivot.position), distance * Mathf.Cos(camPitch));
			terrainPitch = Mathf.Max(terrainPitch, meshPitch);
		}
		float num = 0f;
		switch (targetMode)
		{
		case TargetMode.None:
			num = FlightGlobals.getAltitudeAtPos(pivot.TransformPoint(endPos));
			endPos.Zero();
			break;
		case TargetMode.Vessel:
			num = FlightGlobals.getAltitudeAtPos(pivot.transform.TransformPoint(endPos), vesselTarget.mainBody);
			endPos = vesselTarget.localCoM;
			break;
		case TargetMode.Part:
			num = FlightGlobals.getAltitudeAtPos(pivot.transform.TransformPoint(endPos), partTarget.vessel.mainBody);
			endPos = partTarget.CoMOffset;
			break;
		case TargetMode.Transform:
			num = FlightGlobals.getAltitudeAtPos(pivot.transform.TransformPoint(endPos));
			endPos.Zero();
			break;
		}
		if (num > PhysicsGlobals.CameraDepthToUnlock)
		{
			endPitch = (float)Math.Max(camPitch, Math.Max(terrainPitch, Math.Atan2(minHeight - num, distance * Mathf.Cos(camPitch))));
		}
		else
		{
			endPitch = Math.Max(camPitch, terrainPitch);
		}
		pivot.rotation = frameOfReference * Quaternion.AngleAxis(camHdg * 57.29578f, Vector3.up) * Quaternion.AngleAxis(endPitch * 57.29578f, Vector3.right);
		if (targetMode != 0)
		{
			pivot.localPosition = Vector3.Lerp(pivot.localPosition, endPos, pivotTranslateSharpness * Time.unscaledDeltaTime);
		}
		if (target != pivot && target.GetComponentCached(ref _rigidbody) != null)
		{
			pivot.rotation *= Quaternion.Euler(Vector3.ProjectOnPlane((Quaternion.Inverse(pivot.rotation) * target.GetComponentCached(ref _rigidbody).angularVelocity).normalized, target.transform.up) * target.GetComponentCached(ref _rigidbody).angularVelocity.magnitude * cameraWobbleSensitivity);
		}
		camFXPos = Vector3.back * distance;
		camFXRot = Quaternion.LookRotation(-base.transform.localPosition, Vector3.up);
		if (CameraFX.Instance != null && CameraFX.Instance.cameraFXCollection_0.Count > 0)
		{
			camFXPos = CameraFX.Instance.cameraFXCollection_0.GetLocalPositionFX(camFXPos, 1f * GameSettings.CAMERA_FX_EXTERNAL, Views.FlightExternal);
			camFXRot = CameraFX.Instance.cameraFXCollection_0.GetLocalRotationFX(camFXRot, 0.06f * GameSettings.CAMERA_FX_EXTERNAL, Views.FlightExternal);
		}
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, camFXPos, sharpness * Time.unscaledDeltaTime);
		base.transform.localRotation = camFXRot;
		if (terrainPitch > minPitch && camPitch < terrainPitch)
		{
			localPitch = Mathf.Max(Mathf.Min(camPitch - endPitch, 0f), -0.87964594f);
			if (localPitch <= -0.87964594f)
			{
				camPitch = lastLocalPitch;
			}
			else
			{
				lastLocalPitch = camPitch;
			}
			base.transform.Rotate(Vector3.right, localPitch * 57.29578f, Space.Self);
		}
		else
		{
			camPitch = endPitch;
			localPitch = 0f;
			lastLocalPitch = 0f;
		}
		base.transform.Rotate(pivot.up, offsetHdg * 57.29578f, Space.World);
		base.transform.Rotate(Vector3.right, offsetPitch * 57.29578f, Space.Self);
		if (GameSettings.TRACKIR_ENABLED && TrackIRisActive())
		{
			base.transform.Rotate(Vector3.up, tIRyaw * 57.29578f, Space.Self);
			base.transform.Rotate(Vector3.right, tIRpitch * 57.29578f, Space.Self);
			base.transform.Rotate(Vector3.forward, tIRroll * 57.29578f, Space.Self);
		}
	}

	public Quaternion GetCameraFoR(FoRModes mode)
	{
		FoRMode = mode;
		return FlightGlobals.GetFoR(mode);
	}

	public virtual void updateFoR(Quaternion FoR, float lerpT)
	{
		tgtFoR = FoR;
		frameOfReference = Quaternion.Lerp(lastFoR, tgtFoR, Mathfx.Hermite(0f, 1f, lerpT));
	}

	public static Modes GetAutoModeForVessel(Vessel v)
	{
		if (v.LandedOrSplashed)
		{
			return Modes.FREE;
		}
		if (v.orbit.eccentricity < 1.0)
		{
			if (v.orbit.PeA > 0.0 && FlightGlobals.getStaticPressure(v.orbit.PeA, v.orbit.referenceBody) * PhysicsGlobals.KpaToAtmospheres < 0.009999999776482582)
			{
				return Modes.ORBITAL;
			}
			return Modes.FREE;
		}
		if (v.altitude > v.mainBody.Radius * 0.5)
		{
			return Modes.ORBITAL;
		}
		return Modes.FREE;
	}

	public Quaternion GetChaseFoR(Vessel v)
	{
		chaseBaseMode = GetAutoModeForVessel(v);
		Modes modes = chaseBaseMode;
		if (modes != Modes.FREE && modes == Modes.ORBITAL)
		{
			chaseBaseRot = GetCameraFoR(FoRModes.OBT_ABS);
		}
		else
		{
			chaseBaseRot = GetCameraFoR(FoRModes.SRF_NORTH);
		}
		chaseFUp = chaseBaseRot * Vector3.up;
		chaseFwd = chaseBaseRot * Vector3.forward;
		if (FlightGlobals.fetch.VesselTarget != null)
		{
			chaseOrt = (FlightGlobals.fetch.VesselTarget.GetTransform().position - v.vesselTransform.position).normalized;
			if (chaseMode == ChaseMode.Target_YUp || Mathf.Abs(Vector3.Dot(chaseOrt, chaseFUp)) < 0.7f)
			{
				chaseFwd = Vector3.ProjectOnPlane(chaseOrt, chaseFUp);
				chaseMode = ChaseMode.Target_YUp;
			}
			if (chaseBaseMode == Modes.ORBITAL && (chaseMode == ChaseMode.Target_SrfUp || Mathf.Abs(Vector3.Dot(chaseOrt, chaseFUp)) > 0.9f))
			{
				chaseFUp = chaseFwd;
				chaseFwd = Vector3.ProjectOnPlane(chaseOrt, chaseFwd);
				chaseMode = ChaseMode.Target_SrfUp;
			}
		}
		else if (FlightGlobals.GetDisplaySpeed() > 5.0)
		{
			chaseOrt = FlightGlobals.GetDisplayVelocity().normalized;
			if (Mathf.Abs(Vector3.Dot(chaseOrt, chaseFUp)) < 0.99f)
			{
				chaseFwd = Vector3.ProjectOnPlane(FlightGlobals.GetDisplayVelocity().normalized, chaseFUp);
				chaseMode = ChaseMode.Velocity;
			}
			else
			{
				chaseMode = ChaseMode.Parked;
			}
		}
		else
		{
			chaseMode = ChaseMode.Parked;
		}
		if (chaseModePrev != chaseMode)
		{
			FoRlerp = 0f;
			lastFoR = frameOfReference;
			chaseModePrev = chaseMode;
		}
		return Quaternion.LookRotation(chaseFwd, chaseFUp);
	}

	public virtual void TargetActiveVessel()
	{
		if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.gameObject.activeInHierarchy)
		{
			SetTargetVessel(FlightGlobals.ActiveVessel);
		}
		else
		{
			SetTargetTransform(null);
		}
	}

	public virtual void OnTargetDestroyed()
	{
		if (targetMode == TargetMode.Vessel)
		{
			if (vesselTarget != FlightGlobals.ActiveVessel)
			{
				TargetActiveVessel();
				return;
			}
		}
		else if (targetMode == TargetMode.Part && partTarget != null && partTarget.vessel != null)
		{
			if (partTarget == partTarget.vessel.rootPart)
			{
				if (partTarget.vessel != FlightGlobals.ActiveVessel)
				{
					TargetActiveVessel();
				}
				else
				{
					ClearTarget();
				}
			}
			else
			{
				SetTargetVessel(partTarget.vessel);
			}
			return;
		}
		ClearTarget();
	}

	public virtual void DeactivateUpdate()
	{
		updateActive = false;
	}

	public virtual void ActivateUpdate()
	{
		updateActive = true;
	}

	public virtual void EnableCamera()
	{
		int num = cameras.Length;
		while (num-- > 0)
		{
			Camera obj = cameras[num];
			obj.enabled = true;
			obj.fieldOfView = FieldOfView;
		}
		if (AudioListenerGameObject.transform.parent != mainCamera.transform)
		{
			AudioListenerGameObject.transform.SetParent(mainCamera.transform);
		}
		listener.enabled = true;
	}

	public virtual void DisableCamera()
	{
		int num = cameras.Length;
		while (num-- > 0)
		{
			cameras[num].enabled = false;
		}
		listener.enabled = false;
	}

	public virtual void DisableCamera(bool disableAudioListener)
	{
		if (disableAudioListener)
		{
			DisableCamera();
			return;
		}
		int num = cameras.Length;
		while (num-- > 0)
		{
			cameras[num].enabled = false;
		}
		AudioListenerGameObject.transform.SetParent(base.transform.parent);
	}

	public virtual void CycleCameraHighlighter()
	{
		int num = cameras.Length;
		while (num-- > 0)
		{
			HighlightingSystem componentInChildren = cameras[num].gameObject.GetComponentInChildren<HighlightingSystem>();
			if (componentInChildren != null)
			{
				componentInChildren.Cycle();
			}
		}
	}

	public virtual void DisableCameraHighlighter()
	{
		int num = cameras.Length;
		while (num-- > 0)
		{
			HighlightingSystem componentInChildren = cameras[num].gameObject.GetComponentInChildren<HighlightingSystem>();
			if (componentInChildren != null)
			{
				componentInChildren.Disable();
			}
		}
	}

	public virtual void SetDefaultFoV()
	{
		SetFoV(fovDefault);
	}

	public virtual void ResumeFoV()
	{
		SetFoV(FieldOfView);
	}

	public virtual void SetFoV(float fov)
	{
		int num = cameras.Length;
		while (num-- > 0)
		{
			cameras[num].fieldOfView = fov;
		}
		ScaledCamera.Instance.SetFoV(fov);
		FieldOfView = fov;
	}

	public virtual void ResetFoV()
	{
		SetFoV(fovDefault);
	}

	public virtual void OnSceneSwitch(GameScenes scene)
	{
		AbortExternalControl();
		SetTargetTransform(PSystemSetup.Instance.transform);
	}

	public virtual void OnVesselChange(Vessel vessel)
	{
		AbortExternalControl();
		if (vessel == null)
		{
			pivot.parent = PSystemSetup.Instance.transform;
			base.transform.parent = pivot;
		}
		else
		{
			pivot.parent = vessel.transform;
			base.transform.parent = pivot;
		}
		AdjustDistanceToFit(vessel);
	}

	public void AdjustDistanceToFit(Vessel vessel)
	{
		if (!(vessel == null))
		{
			Vector3 vector = ShipConstruction.CalculateCraftSize(vessel.parts, vessel.rootPart);
			float data = Mathf.Max(KSPCameraUtil.GetDistanceToFit(vector, FieldOfView) * vesselSwitchBackoffFOVFactor + vesselSwitchBackoffPadding, distance);
			SetDistance(data);
			GameEvents.OnCameraDistanceAdjustedToFitVessel.Fire(vessel, data, vector);
		}
	}

	public static void MinDistanceBackaway()
	{
		if ((bool)fetch)
		{
			fetch.AdjustDistanceToFit(FlightGlobals.ActiveVessel);
		}
	}

	public virtual Transform GetPivot()
	{
		return pivot;
	}

	public virtual Transform GetCameraTransform()
	{
		return base.transform;
	}

	public virtual void SetCamCoordsFromPosition(Vector3 wPos)
	{
		Vector3 vector = wPos - pivot.position;
		float magnitude = vector.magnitude;
		vector.Normalize();
		vector = Quaternion.Inverse(frameOfReference) * vector;
		camHdg = Mathf.Atan2(0f - vector.z, vector.x) - (float)Math.PI / 2f;
		camPitch = Mathf.Atan2(vector.y, Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z));
		distance = magnitude;
		pivot.rotation = getRotation(camPitch, camHdg);
	}

	public virtual bool OnNavigatorRequestControl()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.CAMERAMODES | ControlTypes.CAMERACONTROLS))
		{
			return CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Flight;
		}
		return false;
	}

	public virtual Func<bool> OnNavigatorTakeOver(Callback RequestControl)
	{
		base.enabled = false;
		AbortExternalControl = RequestControl;
		return KSPCameraUtil.AnyCameraInputDown;
	}

	public virtual void OnNavigatorHandoff()
	{
		base.enabled = true;
		AbortExternalControl = delegate
		{
		};
		pivot.rotation = getRotation(camPitch, camHdg);
	}

	public virtual Quaternion getRotation(float pitch, float hdg)
	{
		return tgtFoR * Quaternion.AngleAxis(hdg * 57.29578f, Vector3.up) * Quaternion.AngleAxis(pitch * 57.29578f, Vector3.right);
	}

	public Quaternion getReferenceFrame()
	{
		return tgtFoR;
	}

	public virtual float getPitch()
	{
		return camPitch * 57.29578f;
	}

	public virtual float getYaw()
	{
		return camHdg * 57.29578f;
	}

	public void OnCameraChange(CameraManager.CameraMode newMode)
	{
		Camera camera = null;
		int num = cameras.Length;
		while (num-- > 0)
		{
			camera = cameras[num];
			if (camera.name == "Camera 00")
			{
				if (newMode == CameraManager.CameraMode.const_3)
				{
					camera.nearClipPlane = camera00IVANearClipPlane;
				}
				else
				{
					camera.nearClipPlane = camera00DefaultNearClipPlane;
				}
			}
		}
	}
}
