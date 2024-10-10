using System.Collections;
using Expansions.Missions.Editor;
using UnityEngine;

public class GAPVesselCamera : MonoBehaviour
{
	public enum CameraMode
	{
		VesselMode,
		ObjectMode
	}

	public float initialPitch = 0.2859999f;

	public float initialHeading = 1.265998f;

	public float initialHeight = 9f;

	public float camPitch;

	public float camHdg;

	public Vector3 offset;

	public float maxDisplaceX = 20f;

	public float maxDisplaceZ = 36f;

	public float offsetSensitivityAtMinDist = 0.5f;

	public float offsetSensitivityAtMaxDist = 2f;

	public float minZoom = -20f;

	public float maxZoom = -3f;

	public float minPitch = -1.6f;

	public float maxPitch = 1.6f;

	public float orbitSensitivity = 0.05f;

	public float zoomSensitivity = 1f;

	public float sharpness = 8f;

	public Transform vesselRoot;

	[SerializeField]
	public Quaternion endRot = Quaternion.identity;

	[SerializeField]
	public Vector3 endPos = Vector3.zero;

	[SerializeField]
	public Transform pivotTransform;

	[SerializeField]
	public Transform transformCache;

	[SerializeField]
	public Camera cameraCache;

	public float distance;

	public float scrollHeight = 5f;

	public float startDistance = 30f;

	public float maxDistance = 35f;

	public float minDistance = 1f;

	public float minHeight = -30f;

	public float maxHeight = 30f;

	public float clampedScrollHeight;

	public float mouseZoomSensitivity = 0.1f;

	public float offsetSensitivity = 0.1f;

	public ShipConstruct shipCache;

	public EditorBounds editorBounds;

	[SerializeField]
	public Vector3 SPHeditorBoundsExtents = Vector3.zero;

	[SerializeField]
	public Vector3 VABeditorBoundsExtents = Vector3.zero;

	public bool setUp;

	public CameraMode cameraMode;

	public bool Camera_VAB_Controls = true;

	[SerializeField]
	public Light vesselLight;

	public float Distance => distance;

	public IEnumerator Start()
	{
		if (!setUp)
		{
			Setup();
		}
		ResetCamera();
		yield return null;
		if (shipCache != null)
		{
			FocusVessel(shipCache);
		}
		initialHeight = scrollHeight;
	}

	public void Setup()
	{
		Camera_VAB_Controls = GameSettings.MISSION_GAP_CAMERA_VAB_CONTROLS;
		editorBounds = MissionEditorLogic.Instance.gameObject.GetComponent<EditorBounds>();
		pivotTransform = base.transform.parent;
		pivotTransform.localRotation = Quaternion.identity;
		cameraCache = GetComponent<Camera>();
		transformCache = GetComponent<Transform>();
		distance = startDistance;
		offset = Vector3.zero;
		orbitSensitivity = GameSettings.VAB_CAMERA_ORBIT_SENS;
		mouseZoomSensitivity = GameSettings.VAB_CAMERA_ZOOM_SENS;
		if (editorBounds != null)
		{
			editorBounds.SetExtents(Camera_VAB_Controls ? VABeditorBoundsExtents : SPHeditorBoundsExtents);
		}
		setUp = true;
	}

	public void Update()
	{
		if (cameraMode == CameraMode.ObjectMode)
		{
			pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, endRot, sharpness * Time.deltaTime);
			transformCache.localPosition = Vector3.Lerp(transformCache.localPosition, endPos, sharpness * Time.deltaTime);
		}
		else if (Camera_VAB_Controls)
		{
			UpdateVABCamera();
		}
		else
		{
			UpdateSPHCamera();
		}
	}

	public void UpdateCameraControls()
	{
		if (cameraMode != CameraMode.ObjectMode)
		{
			if (Camera_VAB_Controls)
			{
				UpdateVABCameraControls();
			}
			else
			{
				UpdateSPHCameraControls();
			}
		}
	}

	public void UpdateVABCameraControls()
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
	}

	public void UpdateVABCamera(bool smooth = true)
	{
		camPitch = Mathf.Clamp(camPitch, Mathf.Max(minPitch, Mathf.Atan2(minHeight - scrollHeight, distance)), maxPitch);
		endRot = Quaternion.AngleAxis(camHdg * 57.29578f, Vector3.up);
		endRot *= Quaternion.AngleAxis(camPitch * 57.29578f, Vector3.right);
		clampedScrollHeight = scrollHeight;
		endPos = EditorBounds.ClampToCameraBounds(Vector3.up * scrollHeight, endRot * Vector3.forward, ref clampedScrollHeight);
		distance = EditorBounds.ClampCameraDistance(distance);
		if (smooth)
		{
			pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, endRot, sharpness * Time.deltaTime);
			pivotTransform.position = Vector3.Lerp(pivotTransform.position, endPos, sharpness * Time.deltaTime);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, Vector3.back * distance, sharpness * Time.deltaTime);
		}
		else
		{
			pivotTransform.rotation = endRot;
			pivotTransform.position = endPos;
			base.transform.localPosition = Vector3.back * distance;
		}
	}

	public void UpdateSPHCameraControls()
	{
		if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() != 0f)
		{
			if (!GameSettings.Editor_zoomScrollModifier.GetKey())
			{
				distance = Mathf.Clamp(distance - GameSettings.AXIS_MOUSEWHEEL.GetAxis() * 5f, minDistance, maxDistance);
			}
			else
			{
				scrollHeight = Mathf.Clamp(clampedScrollHeight + GameSettings.AXIS_MOUSEWHEEL.GetAxis() * 5f, minHeight, maxHeight);
			}
		}
		if (GameSettings.ZOOM_IN.GetKey())
		{
			distance = Mathf.Clamp(distance - mouseZoomSensitivity, minDistance, maxDistance);
		}
		if (GameSettings.ZOOM_OUT.GetKey())
		{
			distance = Mathf.Clamp(distance + mouseZoomSensitivity, minDistance, maxDistance);
		}
		if (GameSettings.SCROLL_VIEW_UP.GetKey())
		{
			scrollHeight = Mathf.Clamp(clampedScrollHeight + 0.1f, minHeight, maxHeight);
		}
		if (GameSettings.SCROLL_VIEW_DOWN.GetKey())
		{
			scrollHeight = Mathf.Clamp(clampedScrollHeight - 0.1f, minHeight, maxHeight);
		}
		bool mouseLook;
		if ((mouseLook = CameraMouseLook.GetMouseLook()) && !Input.GetKey(KeyCode.LeftShift))
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
		offsetSensitivity = Mathf.Lerp(offsetSensitivityAtMinDist, offsetSensitivityAtMaxDist, Mathf.InverseLerp(minDistance, maxDistance, distance));
		if (!Input.GetMouseButton(2) && (!mouseLook || !Input.GetKey(KeyCode.LeftShift)))
		{
			offset = Vector3.zero;
		}
		else
		{
			offset = new Vector3(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y")) * offsetSensitivity;
		}
		if (GameSettings.CAMERA_ORBIT_UP.GetKey() && Input.GetKey(KeyCode.LeftShift))
		{
			offset.z += offsetSensitivity * 30f * Time.deltaTime;
		}
		if (GameSettings.CAMERA_ORBIT_DOWN.GetKey() && Input.GetKey(KeyCode.LeftShift))
		{
			offset.z -= offsetSensitivity * 30f * Time.deltaTime;
		}
		if (GameSettings.CAMERA_ORBIT_LEFT.GetKey() && Input.GetKey(KeyCode.LeftShift))
		{
			offset.x -= offsetSensitivity * 30f * Time.deltaTime;
		}
		if (GameSettings.CAMERA_ORBIT_RIGHT.GetKey() && Input.GetKey(KeyCode.LeftShift))
		{
			offset.x += offsetSensitivity * 30f * Time.deltaTime;
		}
	}

	public void UpdateSPHCamera()
	{
		camPitch = Mathf.Clamp(camPitch, Mathf.Max(minPitch, Mathf.Atan2(minHeight - scrollHeight, distance)), maxPitch);
		endRot = Quaternion.AngleAxis(camHdg * 57.29578f, Vector3.up);
		endRot *= Quaternion.AngleAxis(camPitch * 57.29578f, Vector3.right);
		if (offset != Vector3.zero)
		{
			offset = Quaternion.AngleAxis(camHdg * 57.29578f, Vector3.up) * offset;
		}
		endPos += offset;
		endPos = new Vector3(Mathf.Clamp(endPos.x, 0f - maxDisplaceX, maxDisplaceX), scrollHeight, Mathf.Clamp(endPos.z, 0f - maxDisplaceZ, maxDisplaceZ));
		clampedScrollHeight = scrollHeight;
		endPos = EditorBounds.ClampToCameraBounds(endPos, endRot * Vector3.forward, ref clampedScrollHeight);
		distance = EditorBounds.ClampCameraDistance(distance);
		pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, endRot, sharpness * Time.deltaTime);
		pivotTransform.position = Vector3.Lerp(pivotTransform.position, endPos, sharpness * Time.deltaTime);
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, Vector3.back * distance, sharpness * Time.deltaTime);
		base.transform.localRotation = Quaternion.identity;
	}

	public void ResetCamera()
	{
		camPitch = initialPitch;
		camHdg = initialHeading;
		Rotate(Vector2.zero);
		scrollHeight = initialHeight;
	}

	public void Rotate(Vector2 delta)
	{
		camHdg += delta.x * orbitSensitivity;
		camPitch -= delta.y * orbitSensitivity;
		camPitch = Mathf.Clamp(camPitch, minPitch, maxPitch);
		endRot = Quaternion.AngleAxis(camHdg * 57.29578f, Vector3.up);
		endRot *= Quaternion.AngleAxis(camPitch * 57.29578f, Vector3.right);
	}

	public void Zoom(float delta)
	{
		endPos.z = Mathf.Clamp(endPos.z + delta * zoomSensitivity, minZoom, maxZoom);
	}

	public void FocusVessel(ShipConstruct ship)
	{
		shipCache = ship;
		if (!setUp)
		{
			Setup();
		}
		pivotTransform.localPosition = Vector3.zero;
		vesselRoot.localPosition = pivotTransform.localPosition + new Vector3(0f, 0f, 0f);
		ShipConstruction.PutShipToGround(ship, vesselRoot);
		Vector3 vector = ShipConstruction.FindCraftCenter(ship);
		Vector3 objSize = ShipConstruction.CalculateCraftSize(ship);
		PlaceCamera(Vector3.up * vector.y, KSPCameraUtil.GetDistanceToFit(objSize, cameraCache.fieldOfView * 0.4f));
		minHeight = 0f - objSize.y - 5f;
		maxHeight = objSize.y + 5f;
		initialHeight = vector.y;
		ResetCamera();
	}

	public void PlaceCamera(Vector3 focusPoint, float dist)
	{
		scrollHeight = focusPoint.y;
		distance = dist;
		if (!Camera_VAB_Controls)
		{
			endPos = new Vector3(focusPoint.x, 0f, focusPoint.z);
		}
	}

	public void FocusPoint(Vector3 point, float minSize, float maxSize)
	{
		if (!setUp)
		{
			Setup();
		}
		pivotTransform.localPosition = point;
		vesselRoot.localPosition = pivotTransform.localPosition + point;
		minHeight = minSize;
		maxHeight = maxSize;
		initialHeight = point.y;
		if (cameraMode != CameraMode.ObjectMode && !Camera_VAB_Controls)
		{
			endPos = new Vector3(point.x, 0f, point.z);
		}
		else
		{
			endPos = new Vector3(0f, 0f, minZoom);
		}
		ResetCamera();
	}
}
