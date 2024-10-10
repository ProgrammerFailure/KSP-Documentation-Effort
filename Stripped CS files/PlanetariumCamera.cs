using System;
using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlanetariumCamera : MonoBehaviour, IKSPCamera
{
	public static PlanetariumCamera _fetch;

	public float minPitch = -0.95f;

	public float maxPitch = 0.95f;

	public float startDistance = 30f;

	public float maxDistance = 150000f;

	public float minDistance = 3f;

	public float orbitSensitivity = 0.5f;

	public float zoomScaleFactor = 1.2f;

	public float sharpness = 10f;

	public float camPitch;

	public float camHdg;

	public MapObject initialTarget;

	public MapObject target;

	public List<MapObject> targets;

	public bool TabSwitchTargets;

	public static Camera camRef;

	public ScreenMessage cameraTgtReadout;

	public float minRadiusDistance;

	public float translateSmooth = 0.1f;

	public float targetHeading;

	public float endHeading;

	public Vector3 cameraVel;

	public float distance;

	public Vector3 endPos;

	public Quaternion testRot;

	public Quaternion endRot;

	public Transform pivot;

	public AudioListener listener;

	public bool externalControl;

	public float tIRpitch;

	public float tIRyaw;

	public float tIRroll;

	public double timeSceneLoadRequested;

	public Callback AbortExternalControl = delegate
	{
	};

	[HideInInspector]
	public CelestialBody b;

	[HideInInspector]
	public double nearest;

	public CelestialBody nearestBody;

	public static PlanetariumCamera fetch
	{
		get
		{
			if (_fetch == null)
			{
				_fetch = (PlanetariumCamera)UnityEngine.Object.FindObjectOfType(typeof(PlanetariumCamera));
			}
			return _fetch;
		}
	}

	public float Distance => distance;

	public Quaternion pivotRotation => pivot.rotation;

	public static Camera Camera
	{
		get
		{
			if (camRef == null)
			{
				camRef = fetch.GetComponent<Camera>();
			}
			return camRef;
		}
	}

	bool IKSPCamera.enabled => base.enabled;

	public void Awake()
	{
		_fetch = this;
		cameraTgtReadout = new ScreenMessage("", 3f, ScreenMessageStyle.UPPER_CENTER);
		CreatePivot();
		listener = GetComponent<AudioListener>();
		listener.enabled = false;
		GameEvents.onVesselDestroy.Add(OnVesselDestroy);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
		GameEvents.onVesselSwitching.Add(onVesselSwitching);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void CreatePivot()
	{
		GameObject gameObject = new GameObject("Scaled Camera Pivot");
		gameObject.AddComponent<DoNotDestroy>();
		gameObject.AddComponent<ResetTransformOnSceneSwitch>();
		pivot = gameObject.transform;
		base.transform.parent = pivot;
	}

	public void Start()
	{
		distance = startDistance;
		base.transform.localPosition = Vector3.back * distance;
		SetTarget(target);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselSwitching.Remove(onVesselSwitching);
		GameEvents.onVesselDestroy.Remove(OnVesselDestroy);
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
		if (_fetch != null && _fetch == this)
		{
			_fetch = null;
		}
	}

	public void OnSceneChange(GameScenes scene)
	{
		timeSceneLoadRequested = Planetarium.GetUniversalTime();
		base.transform.parent = ScaledSpace.Instance.transform;
		ResetPivot();
	}

	public void ResetPivot()
	{
		if (pivot == null)
		{
			CreatePivot();
		}
		pivot.parent = ScaledSpace.Instance.transform;
		pivot.localRotation = Quaternion.identity;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.TRACKSTATION)
		{
			OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
		}
	}

	public void OnLevelLoaded(GameScenes level)
	{
		if (pivot != null)
		{
			SetTarget(initialTarget);
		}
	}

	public void LateUpdate()
	{
		if (target == null || FlightDriver.Pause)
		{
			return;
		}
		if (InputLockManager.IsUnlocked(ControlTypes.CAMERACONTROLS) && GameSettings.CAMERA_RESET.GetKeyDown())
		{
			AbortExternalControl();
			if (FlightGlobals.ActiveVessel != null)
			{
				SetTarget(FlightGlobals.ActiveVessel.mapObject);
				float a = (float)target.vessel.orbit.semiMajorAxis * ScaledSpace.InverseScaleFactor * 0.5f;
				SetDistance(Mathf.Clamp(distance, Mathf.Max(a, minDistance), maxDistance));
			}
			else
			{
				SetTarget(FlightGlobals.GetHomeBodyName());
				SetDistance(Mathf.Clamp(distance, startDistance, (float)(target.celestialBody.sphereOfInfluence * 1.5 * (double)ScaledSpace.InverseScaleFactor)));
			}
		}
		if (InputLockManager.IsUnlocked(ControlTypes.MAP_UI) && !EventSystem.current.IsPointerOverGameObject() && Mouse.Left.GetDoubleClick() && !ManeuverGizmoBase.HasMouseFocus)
		{
			Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
			int count = targets.Count;
			while (count-- > 0)
			{
				Renderer component = targets[count].GetComponent<Renderer>();
				if (!(component != null) || !component.bounds.IntersectRay(ray))
				{
					Vector3 vector = Camera.WorldToScreenPoint(targets[count].transform.position);
					vector.z = 0f;
					Vector3 mousePosition = Input.mousePosition;
					mousePosition.z = 0f;
					if (!((vector - mousePosition).sqrMagnitude >= 144f))
					{
						SetTarget(targets[count]);
						return;
					}
					continue;
				}
				SetTarget(targets[count]);
				return;
			}
		}
		if (InputLockManager.IsUnlocked(ControlTypes.CAMERACONTROLS))
		{
			if (TabSwitchTargets && Input.GetKeyDown(KeyCode.Tab) && !Input.GetKey(KeyCode.LeftShift))
			{
				SetTarget(targets[(targets.IndexOf(target) + 1) % targets.Count]);
			}
			if (TabSwitchTargets && Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
			{
				SetTarget(targets[targets.IndexOf(target) - 1 + ((targets.IndexOf(target) - 1 < 0) ? targets.Count : 0)]);
			}
			if (!externalControl)
			{
				if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() != 0f && !ManeuverGizmoBase.HasMouseFocus && !EventSystem.current.IsPointerOverGameObject())
				{
					distance = Mathf.Clamp(distance * (1f - GameSettings.AXIS_MOUSEWHEEL.GetAxis() * zoomScaleFactor), Mathf.Max(minDistance, minRadiusDistance), maxDistance);
				}
				if (GameSettings.ZOOM_IN.GetKey())
				{
					distance = Mathf.Clamp(distance / (1f + zoomScaleFactor * 0.04f), Mathf.Max(minDistance, minRadiusDistance), maxDistance);
				}
				if (GameSettings.ZOOM_OUT.GetKey())
				{
					distance = Mathf.Clamp(distance * (1f + zoomScaleFactor * 0.04f), Mathf.Max(minDistance, minRadiusDistance), maxDistance);
				}
				if (CameraMouseLook.GetMouseLook())
				{
					camHdg += Input.GetAxis("Mouse X") * orbitSensitivity;
					camPitch -= Input.GetAxis("Mouse Y") * orbitSensitivity;
				}
				camHdg -= GameSettings.AXIS_CAMERA_HDG.GetAxis() * orbitSensitivity;
				camPitch -= GameSettings.AXIS_CAMERA_PITCH.GetAxis() * orbitSensitivity;
				if (GameSettings.CAMERA_ORBIT_UP.GetKey())
				{
					camPitch += 1f * Time.deltaTime;
				}
				if (GameSettings.CAMERA_ORBIT_DOWN.GetKey())
				{
					camPitch -= 1f * Time.deltaTime;
				}
				if (GameSettings.CAMERA_ORBIT_LEFT.GetKey())
				{
					camHdg += 1f * Time.deltaTime;
				}
				if (GameSettings.CAMERA_ORBIT_RIGHT.GetKey())
				{
					camHdg -= 1f * Time.deltaTime;
				}
				camPitch = Mathf.Clamp(camPitch, minPitch, maxPitch);
				endRot = Quaternion.AngleAxis(camHdg * 57.29578f + (float)Planetarium.InverseRotAngle, Vector3.up);
				pivot.rotation = endRot;
				pivot.Rotate(base.transform.right, camPitch * 57.29578f, Space.World);
				base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, Vector3.back * distance, sharpness * Time.deltaTime);
				base.transform.localRotation = Quaternion.LookRotation(-base.transform.localPosition, Vector3.up);
				if ((!GameSettings.TRACKIR_ENABLED || !(GameSettings.TRACKIR.Instance != null) || !HighLogic.LoadedSceneIsFlight) ? GameSettings.TRACKIR.Instance.activeTrackingStation : GameSettings.TRACKIR.Instance.activeMap)
				{
					tIRpitch = TrackIR.Instance.Pitch.GetAxis();
					tIRroll = TrackIR.Instance.Roll.GetAxis();
					tIRyaw = TrackIR.Instance.Yaw.GetAxis();
					base.transform.Rotate(Vector3.right, tIRpitch * 57.29578f, Space.Self);
					base.transform.Rotate(Vector3.up, tIRyaw * 57.29578f, Space.Self);
					base.transform.Rotate(Vector3.forward, tIRroll * 57.29578f, Space.Self);
				}
				else
				{
					tIRpitch = 0f;
					tIRroll = 0f;
					tIRyaw = 0f;
				}
			}
		}
		if (InputLockManager.IsAllLocked(ControlTypes.All) || AlarmClockApp.AppFrameHasLock())
		{
			endRot = Quaternion.AngleAxis(camHdg * 57.29578f + (float)Planetarium.InverseRotAngle, Vector3.up);
			pivot.rotation = endRot;
			pivot.Rotate(base.transform.right, camPitch * 57.29578f, Space.World);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, Vector3.back * distance, sharpness * Time.deltaTime);
			base.transform.localRotation = Quaternion.LookRotation(-base.transform.localPosition, Vector3.up);
		}
		pivot.localPosition = Vector3.SmoothDamp(pivot.localPosition, endPos, ref cameraVel, translateSmooth);
	}

	public void OnVesselDestroy(Vessel vessel)
	{
		if (target != null && target.vessel != null && target.vessel == vessel && Planetarium.GetUniversalTime() - timeSceneLoadRequested > 2.0)
		{
			SetTarget(FindNearestTarget());
		}
		if (targets.Contains(vessel.mapObject))
		{
			RemoveTarget(vessel.mapObject);
		}
	}

	public void onVesselSwitching(Vessel from, Vessel to)
	{
		if (from != null)
		{
			AddTarget(to.mapObject);
			SetTarget(to.mapObject);
			RemoveTarget(from.mapObject);
		}
	}

	public int AddTarget(MapObject tgt)
	{
		targets.Add(tgt);
		return targets.Count - 1;
	}

	public void RemoveTarget(MapObject tgt)
	{
		targets.Remove(tgt);
		if (tgt == target)
		{
			SetTarget(FindNearestTarget());
		}
	}

	public int SetTarget(CelestialBody body)
	{
		return SetTarget(body.name);
	}

	public int SetTarget(string name)
	{
		return SetTarget(GetTargetIndex(name));
	}

	public int SetTarget(int tgtIdx)
	{
		if (tgtIdx < targets.Count && tgtIdx >= 0)
		{
			SetTarget(targets[tgtIdx]);
			return tgtIdx;
		}
		SetTarget(FlightGlobals.ActiveVessel.mapObject);
		return -1;
	}

	public void SetTarget(MapObject tgt)
	{
		if (tgt == null || (HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION))
		{
			return;
		}
		AbortExternalControl();
		target = tgt;
		pivot.parent = tgt.transform;
		endPos = Vector3.zero;
		CelestialBody celestialBody = tgt.celestialBody;
		Vessel vessel = tgt.vessel;
		if (celestialBody != null)
		{
			minRadiusDistance = (float)celestialBody.Radius * ScaledSpace.InverseScaleFactor * 2f;
			if (distance < minRadiusDistance)
			{
				distance = minRadiusDistance;
			}
		}
		else
		{
			minRadiusDistance = 0f;
			if (vessel != null)
			{
				float a = (float)vessel.orbit.semiMajorAxis * ScaledSpace.InverseScaleFactor * 0.5f;
				distance = Mathf.Clamp(distance, Mathf.Max(a, minDistance), maxDistance);
			}
		}
		cameraVel = Vector3.zero;
		distance = Mathf.Clamp(distance, Mathf.Max(minDistance, minRadiusDistance), maxDistance);
		Debug.Log("[PlanetariumCamera]: Focus: " + Localizer.Format(target.GetName()));
		cameraTgtReadout.message = Localizer.Format("#autoLOC_201093", target.GetDisplayName());
		ScreenMessages.PostScreenMessage(cameraTgtReadout);
		GameEvents.onPlanetariumTargetChanged.Fire(target);
		GameEvents.OnMapFocusChange.Fire(target);
	}

	public MapObject FindNearestTarget()
	{
		return FindNearestTarget(pivot.position);
	}

	public MapObject FindNearestTarget(Vector3 position)
	{
		MapObject result = null;
		float num = float.MaxValue;
		int i = 0;
		for (int count = targets.Count; i < count; i++)
		{
			MapObject mapObject = targets[i];
			if (!(mapObject == null) && !(mapObject.celestialBody == null))
			{
				float sqrMagnitude = (position - mapObject.transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = mapObject;
				}
			}
		}
		return result;
	}

	public void SetDistance(float dist)
	{
		distance = Mathf.Clamp(dist, minDistance, maxDistance);
	}

	public int GetTargetIndex(string targetName)
	{
		int num = 0;
		int count = targets.Count;
		while (true)
		{
			if (num < count)
			{
				if (targets[num].gameObject.name == targetName)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public MapObject GetTarget(int index)
	{
		return targets[index];
	}

	public void Activate()
	{
		listener.enabled = true;
		base.transform.parent = pivot;
		base.enabled = true;
		AbortExternalControl();
	}

	public void Activate(bool enableAudioListener)
	{
		if (enableAudioListener)
		{
			listener.enabled = true;
		}
		base.transform.parent = pivot;
		base.enabled = true;
		AbortExternalControl();
	}

	public void Deactivate()
	{
		listener.enabled = false;
		base.transform.parent = ScaledSpace.Instance.transform;
		ResetPivot();
		AbortExternalControl();
		base.enabled = false;
		GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
	}

	public Transform GetPivot()
	{
		return pivot;
	}

	public Transform GetCameraTransform()
	{
		return base.transform;
	}

	public void SetCamCoordsFromPosition(Vector3 wPos)
	{
		Vector3 camCoordsFromPosition = KSPCameraUtil.GetCamCoordsFromPosition(wPos, pivot.position, Planetarium.Rotation);
		camPitch = camCoordsFromPosition.x;
		camHdg = camCoordsFromPosition.y;
		distance = camCoordsFromPosition.z;
	}

	public bool OnNavigatorRequestControl()
	{
		if (MapView.MapIsEnabled)
		{
			return InputLockManager.IsUnlocked(ControlTypes.CAMERACONTROLS);
		}
		return false;
	}

	public Func<bool> OnNavigatorTakeOver(Callback RequestControl)
	{
		externalControl = true;
		AbortExternalControl = RequestControl;
		return updateAbortCondition;
	}

	public bool updateAbortCondition()
	{
		updateDistance();
		if (!(target == null) && !FlightDriver.Pause)
		{
			return KSPCameraUtil.AnyCameraInputDown();
		}
		return false;
	}

	public void OnNavigatorHandoff()
	{
		AbortExternalControl = delegate
		{
		};
		externalControl = false;
	}

	public Quaternion getReferenceFrame()
	{
		return Planetarium.Rotation;
	}

	public float getPitch()
	{
		return camPitch * 57.29578f;
	}

	public float getYaw()
	{
		return camHdg * 57.29578f;
	}

	public void updateDistance()
	{
		nearestBody = getNearestBody(ScaledSpace.ScaledToLocalSpace(base.transform.position));
		distance = (float)Math.Sqrt((base.transform.position - ScaledSpace.LocalToScaledSpace(nearestBody.position)).sqrMagnitude);
	}

	public CelestialBody getNearestBody(Vector3d cameraLocalSpacePos)
	{
		nearest = double.MaxValue;
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			b = FlightGlobals.Bodies[i];
			if ((b.position - cameraLocalSpacePos).sqrMagnitude < nearest)
			{
				nearest = (b.position - cameraLocalSpacePos).sqrMagnitude;
				nearestBody = b;
			}
		}
		return nearestBody;
	}
}
