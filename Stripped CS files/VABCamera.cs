using System;
using System.Collections;
using ns11;
using UnityEngine;
using UnityEngine.EventSystems;

public class VABCamera : MonoBehaviour, IKSPCamera
{
	public float minHeight = 2f;

	public float maxHeight = 50f;

	public float minPitch = -30f;

	public float maxPitch = 60f;

	public float startDistance = 30f;

	public float maxDistance = 35f;

	public float minDistance = 3f;

	public float distance;

	public GameObject pivot;

	public Vector3 endPos;

	public Vector3 offset;

	public Quaternion endRot;

	public float orbitSensitivity = 0.05f;

	public float mouseZoomSensitivity = 0.1f;

	public float offsetSensitivity = 0.1f;

	public float sharpness = 0.2f;

	public float initialHeight = 9f;

	public float initialPitch = 0.2859999f;

	public float initialHeading = 1.265998f;

	public float scrollHeight = 5f;

	public float clampedScrollHeight;

	public float camPitch;

	public float camHdg;

	public float camInitialMinDistance = 5f;

	public Cubemap VABReflection;

	public float Distance => distance;

	public Quaternion pivotRotation => pivot.transform.rotation;

	bool IKSPCamera.enabled => base.enabled;

	public IEnumerator Start()
	{
		pivot = new GameObject();
		pivot.name = "VAB camera pivot";
		base.transform.parent = pivot.transform;
		pivot.transform.localRotation = Quaternion.identity;
		distance = startDistance;
		orbitSensitivity = GameSettings.VAB_CAMERA_ORBIT_SENS;
		mouseZoomSensitivity = GameSettings.VAB_CAMERA_ZOOM_SENS;
		ResetCamera();
		UpdateCamera(smooth: false);
		yield return null;
		SetInitialCameraPosition();
		initialHeight = scrollHeight;
		GameEvents.onEditorRestart.Add(OnEditorRestart);
		GameEvents.onEditorLoad.Add(OnEditorShipLoad);
	}

	public void OnEnable()
	{
		if (pivot != null)
		{
			base.transform.parent = pivot.transform;
		}
	}

	public void OnDestroy()
	{
		GameEvents.onEditorRestart.Remove(OnEditorRestart);
		GameEvents.onEditorLoad.Remove(OnEditorShipLoad);
	}

	public void OnEditorRestart()
	{
		camPitch = initialPitch;
		camHdg = initialHeading;
		scrollHeight = initialHeight;
	}

	public void OnEditorShipLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		SetInitialCameraPosition();
	}

	public void SetInitialCameraPosition()
	{
		if (EditorLogic.fetch.ship.Parts.Count > 0)
		{
			Vector3 vector = ShipConstruction.FindCraftCenter(EditorLogic.fetch.ship, excludeClamps: true);
			float dist = Mathf.Max(KSPCameraUtil.GetDistanceToFit(ShipConstruction.CalculateCraftSize(EditorLogic.fetch.ship), GetComponent<Camera>().fieldOfView * 0.4f), camInitialMinDistance);
			PlaceCamera(Vector3.up * vector.y, dist);
		}
		else if (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			PlaceCamera(new Vector3(0f, 15f, 0f), startDistance);
		}
		else
		{
			PlaceCamera(EditorBounds.Instance.rootPartSpawnPoint, startDistance);
		}
	}

	public void PlaceCamera(Vector3 focusPoint, float dist)
	{
		scrollHeight = focusPoint.y;
		distance = dist;
	}

	public void Update()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.CAMERACONTROLS) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() != 0f)
			{
				if (GameSettings.Editor_zoomScrollModifier.GetKey())
				{
					distance = Mathf.Clamp(distance - GameSettings.AXIS_MOUSEWHEEL.GetAxis() * 5f, minDistance, maxDistance);
				}
				else
				{
					scrollHeight = Mathf.Clamp(clampedScrollHeight + GameSettings.AXIS_MOUSEWHEEL.GetAxis() * 5f, minHeight, maxHeight);
				}
			}
			if (GameSettings.SCROLL_VIEW_UP.GetKey())
			{
				scrollHeight = Mathf.Clamp(clampedScrollHeight + 0.1f, minHeight, maxHeight);
			}
			if (GameSettings.SCROLL_VIEW_DOWN.GetKey())
			{
				scrollHeight = Mathf.Clamp(clampedScrollHeight - 0.1f, minHeight, maxHeight);
			}
			if (Input.GetMouseButton(2))
			{
				distance = Mathf.Clamp(distance - Input.GetAxis("Mouse Y") * mouseZoomSensitivity, minDistance, maxDistance);
			}
			if (GameSettings.ZOOM_IN.GetKey())
			{
				distance = Mathf.Clamp(distance - mouseZoomSensitivity, minDistance, maxDistance);
			}
			if (GameSettings.ZOOM_OUT.GetKey())
			{
				distance = Mathf.Clamp(distance + mouseZoomSensitivity, minDistance, maxDistance);
			}
			if (CameraMouseLook.GetMouseLook())
			{
				camHdg += Input.GetAxis("Mouse X") * orbitSensitivity;
				camPitch -= Input.GetAxis("Mouse Y") * orbitSensitivity;
			}
			camHdg -= GameSettings.AXIS_CAMERA_HDG.GetAxis() * orbitSensitivity;
			camPitch -= GameSettings.AXIS_CAMERA_PITCH.GetAxis() * orbitSensitivity;
			if (!InputLockManager.IsLocked(ControlTypes.flag_53))
			{
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
			}
		}
		UpdateCamera();
		if (GameSettings.TRACKIR_ENABLED && GameSettings.TRACKIR.Instance.activeEditors)
		{
			base.transform.localRotation = TrackIR.Instance.HeadRotation;
		}
		else
		{
			base.transform.localRotation = Quaternion.identity;
		}
	}

	public void UpdateCamera(bool smooth = true)
	{
		camPitch = Mathf.Clamp(camPitch, Mathf.Max(minPitch, Mathf.Atan2(minHeight - scrollHeight, distance)), maxPitch);
		endRot = Quaternion.AngleAxis(camHdg * 57.29578f, Vector3.up);
		endRot *= Quaternion.AngleAxis(camPitch * 57.29578f, Vector3.right);
		clampedScrollHeight = scrollHeight;
		endPos = EditorBounds.ClampToCameraBounds(Vector3.up * scrollHeight, endRot * Vector3.forward, ref clampedScrollHeight);
		distance = EditorBounds.ClampCameraDistance(distance);
		if (smooth)
		{
			pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, endRot, sharpness * Time.deltaTime);
			pivot.transform.position = Vector3.Lerp(pivot.transform.position, endPos, sharpness * Time.deltaTime);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, Vector3.back * distance, sharpness * Time.deltaTime);
		}
		else
		{
			pivot.transform.rotation = endRot;
			pivot.transform.position = endPos;
			base.transform.localPosition = Vector3.back * distance;
		}
	}

	public void ResetCamera()
	{
		scrollHeight = initialHeight;
		camPitch = initialPitch;
		camHdg = initialHeading;
	}

	public Transform GetPivot()
	{
		return null;
	}

	public Transform GetCameraTransform()
	{
		return pivot.transform;
	}

	public void SetCamCoordsFromPosition(Vector3 wPos)
	{
		Debug.LogError(wPos);
		Vector3 camCoordsFromPosition = KSPCameraUtil.GetCamCoordsFromPosition(wPos, Vector3.up * scrollHeight, Quaternion.identity);
		camPitch = camCoordsFromPosition.x;
		camHdg = camCoordsFromPosition.y;
		distance = camCoordsFromPosition.z;
	}

	public Func<bool> OnNavigatorTakeOver(Callback onRequestControl)
	{
		base.enabled = false;
		pivot.transform.position = base.transform.position;
		pivot.transform.rotation = base.transform.rotation;
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		return KSPCameraUtil.AnyCameraInputDown;
	}

	public void OnNavigatorHandoff()
	{
		base.enabled = true;
		base.transform.localRotation = Quaternion.identity;
	}

	public Quaternion getReferenceFrame()
	{
		return Quaternion.identity;
	}

	public float getPitch()
	{
		return camPitch * 57.29578f;
	}

	public float getYaw()
	{
		return camHdg * 57.29578f;
	}

	public bool OnNavigatorRequestControl()
	{
		return InputLockManager.IsUnlocked(ControlTypes.CAMERACONTROLS);
	}
}
