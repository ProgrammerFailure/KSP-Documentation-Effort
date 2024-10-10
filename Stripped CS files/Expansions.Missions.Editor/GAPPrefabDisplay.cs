using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPPrefabDisplay : ActionPaneDisplay
{
	public GAPVesselCamera vesselCamera;

	public GameObject vesselCameraSetup;

	public GameObject targetObject;

	public bool isReady;

	public GameObject PrefabInstance => targetObject;

	public override void Update()
	{
		base.Update();
		if (isMouseOver && Input.mouseScrollDelta.magnitude > 0f)
		{
			vesselCamera.Zoom(Input.mouseScrollDelta.y);
		}
	}

	public override void Setup(Camera displayCamera, int layerMask)
	{
		base.Setup(displayCamera, layerMask);
		vesselCamera = displayCamera.GetComponent<GAPVesselCamera>();
		vesselCamera.maxZoom = -1f;
		vesselCamera.minZoom = -1f;
		vesselCamera.initialPitch = 0f;
		vesselCamera.initialHeading = 3.1416f;
		displayImage.gameObject.SetActive(value: true);
	}

	public void Setup(GameObject prefabTargetObject, float camDistance)
	{
		if (!isReady)
		{
			vesselCameraSetup = Object.Instantiate(MissionsUtils.MEPrefab("Prefabs/VesselDisplay_CameraSetup.prefab"));
			vesselCamera = vesselCameraSetup.GetComponentInChildren<GAPVesselCamera>();
			targetObject = null;
			Setup(vesselCameraSetup.GetComponentInChildren<Camera>(), LayerUtil.DefaultEquivalent | (1 << LayerMask.NameToLayer("WheelCollidersIgnore")) | (1 << LayerMask.NameToLayer("WheelColliders")));
			vesselCamera.cameraMode = GAPVesselCamera.CameraMode.ObjectMode;
			vesselCamera.maxZoom = 0f - camDistance;
			vesselCamera.minZoom = 0f - camDistance;
			isReady = true;
		}
		if (displayImage != null)
		{
			displayImage.gameObject.SetActive(value: true);
		}
		GameObject newTarget = ((prefabTargetObject != null) ? Object.Instantiate(prefabTargetObject) : null);
		LoadTarget(newTarget);
	}

	public void LoadTarget(GameObject newTarget)
	{
		if (targetObject != null)
		{
			Object.Destroy(targetObject);
		}
		if (newTarget != null)
		{
			newTarget.transform.position = Vector3.zero;
			vesselCamera.FocusPoint(newTarget.transform.position, 5f, 5f);
		}
		targetObject = newTarget;
	}

	public override void Clean()
	{
		base.Clean();
		displayImage.gameObject.SetActive(value: false);
		if (targetObject != null)
		{
			Object.Destroy(targetObject);
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		Object.Destroy(displayImage.gameObject);
		Object.Destroy(vesselCameraSetup);
		displayImage = null;
		if (targetObject != null)
		{
			Object.Destroy(targetObject);
		}
	}

	public override void OnDisplayDrag(PointerEventData.InputButton button, Vector2 delta)
	{
		if (button == PointerEventData.InputButton.Right)
		{
			vesselCamera.Rotate(delta.normalized);
		}
	}
}
