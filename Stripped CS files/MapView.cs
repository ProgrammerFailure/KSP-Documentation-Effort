using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Runtime;
using ns16;
using ns23;
using ns36;
using ns9;
using UnityEngine;
using Vectrosity;

public class MapView : MonoBehaviour
{
	public bool ConstantMode;

	public Camera[] uiCameras;

	public MonoBehaviour[] scriptsToDisable;

	public float transitionDuration = 1f;

	public Material orbitLinesMaterial;

	public static Material OrbitLinesMaterialStatic = null;

	public Material orbitIconsMaterial;

	public Material dottedLineMaterial;

	public static Material DottedLineMaterialStatic = null;

	public GUISkin orbitIconsTextSkin;

	public UISkinDefSO orbitIconsTextSkinDef;

	public Texture2D orbitIconsMap;

	public GameObject maneuverNodePrefab;

	public float max3DlineDrawDist = 1500f;

	public Color[] patchColors;

	public Color[] targetPatchColors;

	public MapNode uiNodePrefab;

	public static bool _mapIsEnabled;

	public static bool ReportMapAsDisabled = false;

	public static int displayedOrbits;

	public static Callback OnEnterMapView = delegate
	{
	};

	public static Callback OnExitMapView = delegate
	{
	};

	public Camera vectorCam;

	[SerializeField]
	public AnimationCurve splineEccentricOffset;

	public bool updateMap;

	public float camDistance;

	public float camHdg;

	public float camPitch;

	public int camFocusTarget;

	public FlightCamera mainCamera;

	public Transform spaceCameraHome;

	public bool draw3Dlines;

	public bool highOrbitCountMode;

	public MapObject scaledVessel;

	public Camera mapFxCameraNear;

	public Camera mapFxCameraFar;

	public bool started;

	public List<SiteNode> siteNodes;

	[SerializeField]
	public KSCSiteNode kscSiteNode;

	public static MapView fetch { get; set; }

	public static Material OrbitLinesMaterial
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.orbitLinesMaterial;
		}
	}

	public static Material DottedLinesMaterial
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.dottedLineMaterial;
		}
	}

	public static Material OrbitIconsMaterial
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.orbitIconsMaterial;
		}
	}

	public static GUISkin OrbitIconsTextSkin
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.orbitIconsTextSkin;
		}
	}

	public static UISkinDef OrbitIconsTextSkinDef
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.orbitIconsTextSkinDef.SkinDef;
		}
	}

	public static Texture2D OrbitIconsMap
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.orbitIconsMap;
		}
	}

	public static PlanetariumCamera MapCamera => PlanetariumCamera.fetch;

	public static GameObject ManeuverNodePrefab
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.maneuverNodePrefab;
		}
	}

	public static bool Draw3DLines => fetch.draw3Dlines;

	public static bool HighOrbitCountMode => fetch.highOrbitCountMode;

	public static Color[] PatchColors => fetch.patchColors;

	public static Color[] TargetPatchColors => fetch.targetPatchColors;

	[Obsolete("Use NavBallToggle.Instance.ManeuverModeActive instead")]
	public static bool ManeuverModeActive => NavBallToggle.Instance.ManeuverModeActive;

	public static MapNode UINodePrefab => fetch.uiNodePrefab;

	public static bool MapIsEnabled
	{
		get
		{
			if (_mapIsEnabled)
			{
				return !ReportMapAsDisabled;
			}
			return false;
		}
		set
		{
			_mapIsEnabled = value;
		}
	}

	public Camera VectorCamera => vectorCam;

	public static AnimationCurve SplineEccentricOffset => fetch.splineEccentricOffset;

	public virtual void Awake()
	{
		fetch = this;
		started = false;
		spaceCameraHome = ScaledSpace.Instance.transform;
		mainCamera = FlightCamera.fetch;
		TimingManager.LateUpdateAdd(TimingManager.TimingStage.BetterLateThanNever, delegate
		{
			if (started && updateMap)
			{
				MapNode.MoveAwayFromEachOthers();
			}
		});
		if (OrbitLinesMaterialStatic != null)
		{
			orbitLinesMaterial = new Material(OrbitLinesMaterialStatic);
		}
		else
		{
			orbitLinesMaterial = new Material(orbitLinesMaterial);
		}
		if (DottedLineMaterialStatic != null)
		{
			dottedLineMaterial = DottedLineMaterialStatic;
		}
		GameEvents.LaunchSiteFound.Add(AddLaunchSiteNode);
	}

	public IEnumerator Start()
	{
		VectorLine.SetCamera3D(PlanetariumCamera.Camera);
		VectorLine.SetupVectorCanvas();
		VectorLine.canvas.gameObject.layer = 31;
		vectorCam = CreateVectorCanvasCam();
		vectorCam.transform.SetParent(VectorLine.canvas.transform, worldPositionStays: false);
		vectorCam.transform.localPosition = Vector3.back;
		VectorLine.canvas.worldCamera = vectorCam;
		VectorLine.canvas.renderMode = RenderMode.ScreenSpaceCamera;
		yield return null;
		yield return null;
		yield return null;
		CreateOrbitRenderers();
		CreateLaunchSiteNodes();
		CreateKSCNode();
		if (ConstantMode)
		{
			updateMap = true;
			MapIsEnabled = true;
			GameEvents.OnMapEntered.Fire();
			vectorCam.enabled = true;
			camFocusTarget = MapCamera.SetTarget(FlightGlobals.GetHomeBodyName());
			foreach (OrbitDriver orbit in Planetarium.Orbits)
			{
				if (orbit.Renderer != null)
				{
					orbit.Renderer.drawMode = OrbitRendererBase.DrawMode.REDRAW_AND_RECALCULATE;
					orbit.Renderer.drawNodes = true;
				}
			}
			FlightGlobals.ClearInverseRotation();
		}
		else
		{
			foreach (OrbitDriver orbit2 in Planetarium.Orbits)
			{
				if (orbit2.Renderer != null)
				{
					orbit2.Renderer.drawMode = OrbitRendererBase.DrawMode.const_0;
					orbit2.Renderer.drawNodes = false;
				}
			}
			vectorCam.enabled = false;
			mapFxCameraNear = createMapFXCamera(88, 0.3f, 10000f);
			mapFxCameraFar = createMapFXCamera(89, 10000f, 1E+10f);
			enableMapFXCameras(MapIsEnabled);
			if (HighLogic.LoadedSceneIsFlight)
			{
				camDistance = MapCamera.startDistance;
				camHdg = (0f - ((float)FlightGlobals.ship_longitude + 90f)) * ((float)Math.PI / 180f);
				camPitch = 0.5f;
				while (FlightGlobals.ActiveVessel == null)
				{
					yield return null;
				}
				scaledVessel = FlightGlobals.ActiveVessel.mapObject;
				camFocusTarget = MapCamera.AddTarget(scaledVessel);
				yield return null;
				MapCamera.SetTarget(scaledVessel);
				if (MapIsEnabled)
				{
					exitMapView();
				}
			}
			else
			{
				camDistance = MapCamera.startDistance;
				camHdg = -(float)Math.PI / 2f;
				camPitch = 0.5f;
			}
		}
		if (MapViewFiltering.Instance != null)
		{
			MapViewFiltering.Instance.RefreshCounts();
		}
		started = true;
	}

	public Camera CreateVectorCanvasCam()
	{
		Camera camera = new GameObject("Canvas Camera ").AddComponent<Camera>();
		camera.clearFlags = CameraClearFlags.Depth;
		camera.cullingMask = int.MinValue;
		camera.orthographic = true;
		camera.orthographicSize = (float)Screen.height * 0.5f;
		camera.allowHDR = false;
		return camera;
	}

	public Camera createMapFXCamera(int depthOffset, float nearPlane, float farPlane)
	{
		Camera camera = new GameObject("MapFX Camera " + depthOffset).AddComponent<Camera>();
		camera.transform.parent = MapCamera.transform;
		camera.transform.localPosition = Vector3.zero;
		camera.transform.localRotation = Quaternion.identity;
		camera.depth = depthOffset;
		camera.cullingMask = 16777216;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.nearClipPlane = nearPlane;
		camera.farClipPlane = farPlane;
		camera.allowHDR = false;
		return camera;
	}

	public void Update()
	{
		if (!started)
		{
			return;
		}
		if (!ConstantMode && GameSettings.MAP_VIEW_TOGGLE.GetKeyDown() && InputLockManager.IsUnlocked(ControlTypes.MAP_TOGGLE))
		{
			if (HighLogic.CurrentGame == null || HighLogic.LoadedSceneIsMissionBuilder)
			{
				return;
			}
			if (!HighLogic.CurrentGame.Parameters.Flight.CanUseMap)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003264"), 5f, ScreenMessageStyle.UPPER_CENTER);
				return;
			}
			if (!MapIsEnabled)
			{
				EnterMapView();
			}
			else
			{
				ExitMapView();
			}
		}
		if (updateMap)
		{
			UpdateMap();
		}
	}

	public void UpdateMap(bool forceUpdate = false)
	{
		displayedOrbits = 0;
		for (int i = 0; i < Planetarium.Orbits.Count; i++)
		{
			OrbitDriver orbitDriver = Planetarium.Orbits[i];
			if (orbitDriver.Renderer != null && orbitDriver.Renderer.isActive)
			{
				displayedOrbits++;
			}
		}
		highOrbitCountMode = displayedOrbits >= GameSettings.MAP_MAX_ORBIT_BEFORE_FORCE2D;
		if ((MapCamera.Distance > max3DlineDrawDist || highOrbitCountMode) && (draw3Dlines || forceUpdate))
		{
			draw3Dlines = false;
			PlanetariumCamera.Camera.cullingMask = PlanetariumCamera.Camera.cullingMask & 0x7FFFFFFF;
			if (vectorCam != null)
			{
				vectorCam.enabled = true;
			}
		}
		if (MapCamera.Distance < max3DlineDrawDist && !highOrbitCountMode && (!draw3Dlines || forceUpdate))
		{
			draw3Dlines = true;
			PlanetariumCamera.Camera.cullingMask = PlanetariumCamera.Camera.cullingMask | int.MinValue;
			if (vectorCam != null)
			{
				vectorCam.enabled = false;
			}
		}
	}

	public void OnDestroy()
	{
		if (scaledVessel != null)
		{
			MapCamera.RemoveTarget(scaledVessel);
			MapCamera.Deactivate();
		}
		if (ConstantMode)
		{
			MapIsEnabled = false;
			GameEvents.OnMapExited.Fire();
		}
		else
		{
			if (mapFxCameraFar != null && mapFxCameraFar.gameObject != null)
			{
				UnityEngine.Object.Destroy(mapFxCameraNear.gameObject);
			}
			if (mapFxCameraNear != null && mapFxCameraNear.gameObject != null)
			{
				UnityEngine.Object.Destroy(mapFxCameraFar.gameObject);
			}
			if (MapIsEnabled)
			{
				mainCamera.SetDistanceImmediate(mainCamera.startDistance);
			}
		}
		InputLockManager.RemoveControlLock("MapView");
		GameEvents.LaunchSiteFound.Remove(AddLaunchSiteNode);
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void CreateOrbitRenderers()
	{
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			if (!(celestialBody.orbitDriver == null))
			{
				OrbitRenderer orbitRenderer = celestialBody.gameObject.AddComponent<OrbitRenderer>();
				orbitRenderer.driver = celestialBody.orbitDriver;
				orbitRenderer.celestialBody = celestialBody;
				celestialBody.orbitDriver.Renderer = orbitRenderer;
			}
		}
	}

	public void CreateLaunchSiteNodes()
	{
		if (PSystemSetup.Instance == null)
		{
			return;
		}
		if (siteNodes == null)
		{
			siteNodes = new List<SiteNode>();
		}
		else
		{
			siteNodes.Clear();
		}
		for (int i = 0; i < PSystemSetup.Instance.LaunchSites.Count; i++)
		{
			LaunchSite siteNodeObject = PSystemSetup.Instance.LaunchSites[i];
			if (!PSystemSetup.Instance.LaunchSites[i].requiresPOIVisit || ((bool)ProgressTracking.Instance && ProgressTracking.Instance.NodeComplete(PSystemSetup.Instance.LaunchSites[i].poiName)))
			{
				siteNodes.Add(SiteNode.Spawn(siteNodeObject));
			}
		}
	}

	public void AddLaunchSiteNode(LaunchSite launchSite)
	{
		siteNodes.Add(SiteNode.Spawn(launchSite));
	}

	public void CreateKSCNode()
	{
		if (!(PSystemSetup.Instance == null))
		{
			if (kscSiteNode == null)
			{
				kscSiteNode = new KSCSiteNode();
			}
			siteNodes.Add(SiteNode.Spawn(kscSiteNode));
		}
	}

	public static void EnterMapView()
	{
		if ((bool)fetch && !MapIsEnabled && !fetch.ConstantMode)
		{
			if (!MissionSystem.AllowCameraSwitch(CameraManager.CameraMode.Map))
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003103"), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
			else
			{
				fetch.enterMapView();
			}
		}
	}

	public static void ExitMapView()
	{
		if ((bool)fetch && MapIsEnabled && !fetch.ConstantMode)
		{
			fetch.exitMapView();
		}
	}

	public void enterMapView()
	{
		if (!HighLogic.CurrentGame.Parameters.Flight.CanUseMap)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003264"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return;
		}
		MapCamera.Activate(enableAudioListener: false);
		vectorCam.enabled = true;
		enableMapFXCameras(enabledState: true);
		ScaledCamera.Instance.enabled = false;
		UpdateMap(forceUpdate: true);
		CameraManager.Instance.SetCameraMap();
		int i = 0;
		for (int num = scriptsToDisable.Length; i < num; i++)
		{
			scriptsToDisable[i].enabled = false;
		}
		for (int j = 0; j < Planetarium.Orbits.Count; j++)
		{
			OrbitDriver orbitDriver = Planetarium.Orbits[j];
			if (orbitDriver.Renderer != null)
			{
				if ((bool)orbitDriver.vessel && orbitDriver.vessel.PatchedConicsAttached)
				{
					orbitDriver.Renderer.drawMode = OrbitRendererBase.DrawMode.const_0;
				}
				else
				{
					orbitDriver.Renderer.drawMode = OrbitRendererBase.DrawMode.REDRAW_AND_RECALCULATE;
				}
				orbitDriver.Renderer.drawNodes = true;
			}
		}
		InputLockManager.SetControlLock(ControlTypes.MAPVIEW, "MapView");
		MapCamera.camHdg = camHdg;
		MapCamera.camPitch = camPitch;
		MapCamera.SetTarget(camFocusTarget);
		MapCamera.SetDistance(camDistance);
		mainCamera.SetDistance(camDistance * ScaledSpace.ScaleFactor);
		mainCamera.AbortExternalControl();
		mainCamera.enabled = false;
		MapIsEnabled = true;
		GameEvents.OnMapEntered.Fire();
		updateMap = true;
		OnEnterMapView();
		if (IsInvoking("endExitMapTransition"))
		{
			CancelInvoke();
		}
		if (!IsInvoking("endEnterMapTransition"))
		{
			Invoke("endEnterMapTransition", transitionDuration);
		}
	}

	public void endEnterMapTransition()
	{
		Camera[] array = uiCameras;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
	}

	public void exitMapView()
	{
		MapIsEnabled = false;
		GameEvents.OnMapExited.Fire();
		MapCamera.Deactivate();
		if (!started)
		{
			return;
		}
		vectorCam.enabled = false;
		PlanetariumCamera.Camera.cullingMask = PlanetariumCamera.Camera.cullingMask & 0x7FFFFFFF;
		enableMapFXCameras(enabledState: false);
		ScaledCamera.Instance.enabled = true;
		mainCamera.enabled = true;
		MapCamera.transform.parent = spaceCameraHome;
		camDistance = MapCamera.Distance;
		camHdg = MapCamera.camHdg;
		camPitch = MapCamera.camPitch;
		camFocusTarget = MapCamera.targets.IndexOf(MapCamera.target);
		ScaledCamera.Instance.enabled = true;
		MonoBehaviour[] array = scriptsToDisable;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		foreach (OrbitDriver orbit in Planetarium.Orbits)
		{
			if (orbit.Renderer != null)
			{
				orbit.Renderer.drawMode = OrbitRendererBase.DrawMode.const_0;
				orbit.Renderer.drawNodes = false;
			}
		}
		CameraManager.Instance.PreviousCameraMode();
		Camera[] array2 = uiCameras;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].enabled = true;
		}
		mainCamera.setModeImmediate(mainCamera.mode);
		mainCamera.SetDistanceImmediate(mainCamera.maxDistance);
		mainCamera.SetDistance(mainCamera.startDistance);
		mainCamera.AdjustDistanceToFit(FlightGlobals.ActiveVessel);
		OnExitMapView();
		if (IsInvoking("endEnterMapTransition"))
		{
			CancelInvoke();
		}
		if (!IsInvoking("endExitMapTransition"))
		{
			Invoke("endExitMapTransition", transitionDuration);
		}
	}

	public void endExitMapTransition()
	{
		InputLockManager.RemoveControlLock("MapView");
		updateMap = false;
	}

	public void enableMapFXCameras(bool enabledState)
	{
		if (mapFxCameraFar != null)
		{
			mapFxCameraFar.enabled = enabledState;
		}
		if (mapFxCameraNear != null)
		{
			mapFxCameraNear.enabled = enabledState;
		}
	}

	public static float GetEccOffset(float eccOffset, float ecc, float eccOffsetPower)
	{
		float num = SplineEccentricOffset.Evaluate(eccOffset);
		return 1f + (num - 1f) * (Mathf.Pow(ecc, eccOffsetPower) / Mathf.Pow(0.9f, eccOffsetPower));
	}
}
