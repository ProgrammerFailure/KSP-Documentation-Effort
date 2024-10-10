using System.Collections;
using ns12;
using ns9;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody : ActionPaneDisplay_CelestialBody
{
	public GAPCelestialBodyCamera celestialCam;

	public GAPCelestialBodyState currentStateEnum;

	public Camera localCam;

	public Camera scaledCam;

	public Camera scaledCamMapFX;

	public bool planetLoaded;

	public bool usePQS;

	public CelestialBody celestialBody;

	public Vector3d celestialBodyPos = Vector3d.zero;

	public Quaternion celestialBodyRot = Quaternion.identity;

	public double distanceToSurface;

	public double surfaceHeight;

	public Vector3d pointInSurface;

	public bool isReady;

	public GAPCelestialBodyState_Base currentState;

	public Orbit orbit;

	public bool pqsStarted;

	public GAPCelestialBodyCollisionSphere collisionSphere;

	public float startingZoomValue;

	public KSPUtil.DefaultDateTimeFormatter dateFormatter;

	public double maxLightCycleUT;

	public double missionStartUT;

	public int resourceDisplayIndex;

	public bool IsSelected => isSelected;

	public bool PQSActive => usePQS;

	public bool PQSLoaded
	{
		get
		{
			if (usePQS)
			{
				return pqsStarted;
			}
			return false;
		}
	}

	public double DistanceToSurface => distanceToSurface;

	public double SurfaceHeight => surfaceHeight;

	public GAPCelestialBodyCamera CelestialCamera => celestialCam;

	public Camera DisplayCamera => displayCamera;

	public CelestialBody CelestialBody => celestialBody;

	public float StartingZoomValue
	{
		get
		{
			return startingZoomValue;
		}
		set
		{
			startingZoomValue = value;
		}
	}

	public Orbit BodyOrbit
	{
		get
		{
			return orbit;
		}
		set
		{
			orbit = value;
		}
	}

	public GAPCelestialBodyState_Biomes Biomes
	{
		get
		{
			if (currentStateEnum != GAPCelestialBodyState.BIOMES)
			{
				return null;
			}
			return (GAPCelestialBodyState_Biomes)currentState;
		}
	}

	public GAPCelestialBodyState_Orbit Orbits
	{
		get
		{
			if (currentStateEnum != GAPCelestialBodyState.ORBIT)
			{
				return null;
			}
			return (GAPCelestialBodyState_Orbit)currentState;
		}
	}

	public GAPCelestialBodyState_SurfaceGizmo SurfaceGizmo
	{
		get
		{
			if (currentStateEnum != GAPCelestialBodyState.POINT)
			{
				return null;
			}
			return (GAPCelestialBodyState_SurfaceGizmo)currentState;
		}
	}

	public Quaternion StoredCBRotation => celestialBodyRot;

	public void Initialize()
	{
		if (celestialCam == null)
		{
			GameObject original = MissionsUtils.MEPrefab("Prefabs/GAP_CelestialBody_CameraSetup.prefab");
			celestialCam = Object.Instantiate(original).GetComponent<GAPCelestialBodyCamera>();
		}
		if (collisionSphere == null)
		{
			collisionSphere = Object.Instantiate(MissionsUtils.MEPrefab("Prefabs/GAP_CelestialBody_CollisionSphere.prefab")).GetComponent<GAPCelestialBodyCollisionSphere>();
		}
		hitDistance = float.MaxValue;
		localCam = celestialCam.GetLocalSpaceCamera();
		scaledCam = ScaledCamera.Instance.cam;
		if (scaledCam.transform.childCount > 0)
		{
			scaledCamMapFX = scaledCam.transform.GetChild(0).GetComponent<Camera>();
		}
		if (scaledCamMapFX == null)
		{
			scaledCamMapFX = MissionEditorMapView.CreateMapFXCamera(1, scaledCam.nearClipPlane, scaledCam.farClipPlane);
			scaledCamMapFX.transform.SetParent(scaledCam.transform, worldPositionStays: false);
			scaledCamMapFX.targetTexture = displayTexture;
		}
		ScaledSpace.ToggleAll(toggleValue: false);
		LayerMask layerMask = default(LayerMask);
		layerMask = ~(1 << LayerMask.NameToLayer("TransparentFX"));
		Setup(celestialCam.GetLocalSpaceCamera(), layerMask);
		displayImage.rectTransform.SetSiblingIndex(0);
		displayImage.material = celestialCam.GAPCelestialBodyMaterial;
		selector.SetFooterText(string.Empty);
		selector.OnSliderHovered_Zoom = UpdateFooterText_Zoom;
		selector.OnSliderHovered_Light = UpdateFooterText_Light;
		startingZoomValue = 0.1f;
		resourceDisplayIndex = -1;
		dateFormatter = new KSPUtil.DefaultDateTimeFormatter();
		isReady = true;
	}

	public void LoadPlanet(CelestialBody newCelestialBody)
	{
		if (planetLoaded)
		{
			return;
		}
		if (newCelestialBody == null)
		{
			selector.SetTitleText(Localizer.Format("#autoLOC_8000273"));
			selector.containerLeftButtons.gameObject.SetActive(value: false);
			selector.containerFooter.gameObject.SetActive(value: false);
			celestialCam.pqsTarget.gameObject.SetActive(value: false);
			UnloadPlanet();
			return;
		}
		selector.containerLeftButtons.gameObject.SetActive(value: true);
		selector.containerFooter.gameObject.SetActive(value: true);
		selector.gameObject.SetActive(value: true);
		celestialCam.pqsTarget.gameObject.SetActive(value: true);
		celestialBody = newCelestialBody;
		celestialBodyPos = celestialBody.position;
		celestialBodyRot = celestialBody.rotation;
		celestialBody.position = Vector3d.zero;
		collisionSphere.Setup(celestialBody);
		ScaledCamera.Instance.SetTarget(celestialCam.transform);
		ScaledCamera.Instance.SetCameraClearFlag(CameraClearFlags.Color, Color.black);
		ScaledSpace.Toggle(celestialBody, toggleValue: true);
		if (celestialBody.hasSolidSurface && usePQS)
		{
			celestialBody.pqsController.SetTarget(celestialCam.pqsTarget);
			celestialBody.pqsController.SetSecondaryTarget(celestialCam.transform);
			pqsStarted = false;
			celestialBody.pqsController.OnReady.Add(CallbackPQSLoaded);
			TogglePQS(usePQS: true);
		}
		else
		{
			TogglePQS(usePQS: false);
			pqsStarted = false;
		}
		surfaceHeight = 0.0;
		celestialCam.Enable(celestialBody, usePQS);
		base.Selector.sliderZoom.value = startingZoomValue;
		celestialCam.OverrideDistance(selector.sliderZoom.value);
		selector.SetTitleText(celestialBody.displayName.LocalizeRemoveGender());
		maxLightCycleUT = GetLightTimeCycle(newCelestialBody);
		missionStartUT = MissionEditorLogic.Instance.EditorMission.situation.startUT;
		base.Selector.sliderLight.value = 0f;
		SliderAction_LightCycle(0f);
		scaledCam.backgroundColor = new Color(0.05f, 0.05f, 0.05f);
		celestialCam.camMapFX.targetTexture = displayTexture;
		scaledCamMapFX.enabled = !usePQS;
		celestialCam.camMapFX.enabled = usePQS;
		if (currentState != null)
		{
			currentState.LoadPlanet(newCelestialBody);
		}
		if (celestialBody.ResourceMap != null)
		{
			celestialBody.SetResourceMap(null);
		}
		planetLoaded = true;
	}

	public void UnloadPlanet()
	{
		if (planetLoaded)
		{
			if (currentState != null)
			{
				currentState.UnloadPlanet();
			}
			celestialBody.position = celestialBodyPos;
			celestialBody.CBUpdate();
			ScaledSpace.Toggle(celestialBody, toggleValue: false);
			if (celestialBody.hasSolidSurface && usePQS)
			{
				celestialBody.pqsController.SetTarget(null);
				celestialBody.pqsController.SetSecondaryTarget(FlightCamera.fetch.transform);
				celestialBody.pqsController.ResetSphere();
				celestialBody.pqsController.OnReady.Remove(CallbackPQSLoaded);
			}
			celestialBody = null;
			planetLoaded = false;
			pqsStarted = false;
			if (OverlayGenerator.Instance.IsActive)
			{
				DisplayResource(-1);
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (planetLoaded)
		{
			scaledCam.targetTexture = displayTexture;
			scaledCam.Render();
			if (PQSLoaded)
			{
				localCam.targetTexture = displayTexture;
				localCam.Render();
			}
			scaledCamMapFX.targetTexture = displayTexture;
			scaledCamMapFX.Render();
			if (celestialBody.hasSolidSurface && usePQS)
			{
				UpdateSurfaceValues();
			}
			celestialCam.UpdateTransform(surfaceHeight);
		}
		if (currentState != null)
		{
			currentState.Update();
		}
	}

	public virtual void LateUpdate()
	{
		if (currentState != null && celestialBody != null)
		{
			currentState.LateUpdate();
		}
	}

	public void UpdateSurfaceValues()
	{
		QuaternionD quaternionD = Quaternion.LookRotation(celestialCam.pqsTarget.transform.position.normalized);
		pointInSurface = quaternionD * Vector3d.forward;
		double num = celestialBody.pqsController.GetSurfaceHeight(pointInSurface);
		pointInSurface *= num;
		distanceToSurface = Vector3d.Distance(celestialCam.pqsTarget.transform.position, pointInSurface);
		surfaceHeight = num - celestialBody.Radius;
	}

	public override void OnDisplayClickUp(RaycastHit? hit)
	{
		if (celestialBody != null && celestialBody.pqsController != null && currentState != null)
		{
			currentState.OnClickUp(hit);
		}
	}

	public override void OnDisplayClick(RaycastHit? hit)
	{
		if (currentState != null && celestialBody != null && (celestialBody.pqsController != null || currentState == Orbits))
		{
			currentState.OnClick(hit);
		}
	}

	public override void OnMouseOver(Vector2 cameraPoint)
	{
		if (currentState != null && celestialBody != null && (!isDragging || currentState == Orbits))
		{
			currentState.OnMouseOver(cameraPoint);
			if (isMouseOver)
			{
				UpdateScroll();
			}
		}
	}

	public override void OnDisplayDrag(PointerEventData.InputButton arg0, Vector2 arg1)
	{
		if (celestialBody != null)
		{
			if (currentState != null)
			{
				currentState.OnDrag(arg0, arg1);
			}
			Vector2 normalized = arg1.normalized;
			celestialCam.UpdateMouse(normalized.x, normalized.y);
		}
	}

	public override void OnDisplayDragEnd(RaycastHit? hit)
	{
		if (celestialBody != null && celestialBody.pqsController != null && currentState != null)
		{
			currentState.OnDragEnd(hit);
		}
	}

	public void SliderAction_Zoom(float value)
	{
		if (celestialBody != null)
		{
			celestialCam.OverrideDistance(value);
			if (usePQS && !pqsStarted && value > 0.3f)
			{
				StartCoroutine("LoadPQS");
				pqsStarted = true;
			}
		}
	}

	public void SliderAction_LightCycle(float value)
	{
		if (celestialBody != null)
		{
			double lightCycle = maxLightCycleUT * (double)value + missionStartUT;
			celestialCam.SetLightCycle(lightCycle);
		}
	}

	public string GetUTFormattedString(double ut)
	{
		int num = (int)(ut / (double)dateFormatter.Year);
		ut -= (double)(num * dateFormatter.Year);
		int num2 = (int)(ut / (double)dateFormatter.Day);
		ut -= (double)(num2 * dateFormatter.Day);
		int num3 = (int)(ut / (double)dateFormatter.Hour);
		ut -= (double)(num3 * dateFormatter.Hour);
		int num4 = (int)(ut / (double)dateFormatter.Minute);
		ut -= (double)(num4 * dateFormatter.Minute);
		string empty = string.Empty;
		if (num > 0)
		{
			return $"{num}y-{num2}d-{num3}:{num4:00}";
		}
		return (num2 > 0) ? $"{num2}d-{num3}:{num4:00}" : $"{num3}:{num4:00}";
	}

	public double GetLightTimeCycle(CelestialBody body)
	{
		double num = 0.0;
		if (body.tidallyLocked)
		{
			return body.orbit.period;
		}
		return body.rotationPeriod;
	}

	public void OnResourcesButtonClicked()
	{
		if (OverlayGenerator.Instance != null && OverlayGenerator.Instance.ResourceList != null)
		{
			resourceDisplayIndex++;
			if (resourceDisplayIndex >= OverlayGenerator.Instance.ResourceList.Count)
			{
				resourceDisplayIndex = -1;
			}
			DisplayResource(resourceDisplayIndex);
		}
	}

	public void DisplayResource(int resourceIndex)
	{
		if (!(celestialBody == null))
		{
			if (resourceIndex >= 0 && resourceIndex < OverlayGenerator.Instance.ResourceList.Count)
			{
				ResourceMap.Instance.UnlockPlanet(CelestialBody.flightGlobalsIndex);
				OverlayGenerator.Instance.IsActive = true;
				OverlayGenerator.Instance.DisplayBody = CelestialBody;
				OverlayGenerator.Instance.OverlayStyle = 3;
				OverlayGenerator.Instance.DisplayResource = OverlayGenerator.Instance.ResourceList[resourceIndex];
				OverlayGenerator.Instance.Cutoff = 30;
				GetToolbarButton("buttonResources").GetComponent<TooltipController_Text>().textString = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8005049"), OverlayGenerator.Instance.DisplayResource.displayName);
				OverlayGenerator.Instance.GenerateOverlay(checkForLock: true);
			}
			else
			{
				OverlayGenerator.Instance.IsActive = false;
				OverlayGenerator.Instance.ClearDisplay();
				OverlayGenerator.Instance.DisplayBody = null;
				OverlayGenerator.Instance.DisplayResource = null;
				GetToolbarButton("buttonResources").GetComponent<TooltipController_Text>().textString = "#autoLOC_8005050";
			}
		}
	}

	public IEnumerator LoadPQS()
	{
		base.Selector.containerLoadScreen.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(0.1f);
		celestialBody.position = Vector3d.zero;
		PSystemSetup.Instance.SetPQSActive(celestialBody.pqsController);
		yield return null;
	}

	public void CallbackPQSLoaded(GClass4 pqs)
	{
		if (collisionSphere != null)
		{
			collisionSphere.ToggleVisibility(toggleValue: false);
		}
		base.Selector.containerLoadScreen.gameObject.SetActive(value: false);
		StartCoroutine("LoadPQSCities");
	}

	public IEnumerator LoadPQSCities()
	{
		yield return new WaitForSeconds(0.1f);
		GameEvents.Mission.onMissionsBuilderPQSLoaded.Fire();
	}

	public void SetState(GAPCelestialBodyState newState)
	{
		if (currentState != null)
		{
			currentState.End();
		}
		switch (newState)
		{
		default:
			currentState = null;
			break;
		case GAPCelestialBodyState.SIMPLE:
			currentState = new GAPCelestialBodyState_Base();
			break;
		case GAPCelestialBodyState.POINT:
			currentState = new GAPCelestialBodyState_SurfaceGizmo();
			break;
		case GAPCelestialBodyState.BIOMES:
			currentState = new GAPCelestialBodyState_Biomes();
			break;
		case GAPCelestialBodyState.ORBIT:
			currentState = new GAPCelestialBodyState_Orbit();
			break;
		}
		if (currentState != null)
		{
			currentState.Init(this);
		}
		currentStateEnum = newState;
		base.Selector.sliderZoom.onValueChanged.AddListener(SliderAction_Zoom);
		base.Selector.sliderLight.onValueChanged.AddListener(SliderAction_LightCycle);
		AddToolbarButton("buttonResources", "displayResourcesIcon", "#autoLOC_8006093").onClick.AddListener(OnResourcesButtonClicked);
	}

	public void Load(CelestialBody celestialBody)
	{
		int num = -1;
		if (isReady)
		{
			if (OverlayGenerator.Instance != null && OverlayGenerator.Instance.IsActive)
			{
				num = OverlayGenerator.Instance.ResourceList.IndexOf(OverlayGenerator.Instance.DisplayResource);
			}
			UnloadPlanet();
		}
		else
		{
			Initialize();
		}
		LoadPlanet(celestialBody);
		if (num >= 0)
		{
			DisplayResource(num);
		}
	}

	public override void Clean()
	{
		UnloadPlanet();
		ScaledCamera.Instance.cam.backgroundColor = Color.black;
		if (currentState != null)
		{
			currentState.End();
		}
		currentState = null;
		ClearToolbarEvents();
		selector.ClearEvents();
	}

	public override void Destroy()
	{
		Clean();
		base.Destroy();
		ScaledSpace.ToggleAll(toggleValue: true);
		ScaledCamera.Instance.ResetTarget();
		Object.Destroy(displayImage.gameObject);
		if (collisionSphere != null)
		{
			Object.Destroy(collisionSphere.gameObject);
		}
		if (celestialCam != null)
		{
			celestialCam.Destroy();
		}
		if (selector != null && selector.gameObject != null)
		{
			Object.Destroy(selector.gameObject);
		}
	}

	public void TogglePQS(bool usePQS)
	{
		this.usePQS = usePQS;
		LayerMask layerMask = default(LayerMask);
		if (usePQS)
		{
			layerMask = ~(1 << LayerMask.NameToLayer("TransparentFX"));
			base.layerMask = layerMask;
			displayCamera = localCam;
		}
		else
		{
			layerMask = ~((1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("Local Scenery")) | (1 << LayerMask.NameToLayer("SurfaceFX")));
			base.layerMask = layerMask;
			displayCamera = scaledCam;
		}
	}

	public void SetFooterText(string newText)
	{
		selector.SetFooterText(newText);
	}

	public bool DoLocalSpaceRay(Vector2 cameraPoint, out RaycastHit rayHit)
	{
		return Physics.Raycast(displayCamera.ScreenPointToRay(cameraPoint), out rayHit, hitDistance, layerMask);
	}

	public RaycastHit[] DoLocalSpaceRayAll(Vector2 cameraPoint)
	{
		return Physics.RaycastAll(displayCamera.ScreenPointToRay(cameraPoint), hitDistance, layerMask);
	}

	public void SuscribeToLeftButton(UnityAction action)
	{
		selector.leftButton.onClick.AddListener(action);
	}

	public void SuscribeToRightButton(UnityAction action)
	{
		selector.rightButton.onClick.AddListener(action);
	}

	public void UpdateScroll()
	{
		float value = base.Selector.sliderZoom.value + Input.GetAxis("Mouse ScrollWheel") * 0.1f;
		base.Selector.sliderZoom.value = value;
	}

	public void UpdateFooterText_Zoom()
	{
		selector.SetFooterText(Localizer.Format("#autoLOC_8007304", celestialCam.DistanceToSurface.ToString("f1")));
	}

	public void UpdateFooterText_Light()
	{
		double ut = maxLightCycleUT * (double)selector.sliderLight.value + missionStartUT;
		selector.SetFooterText(Localizer.Format("#autoLOC_8007305", GetUTFormattedString(missionStartUT), GetUTFormattedString(ut)));
	}
}
