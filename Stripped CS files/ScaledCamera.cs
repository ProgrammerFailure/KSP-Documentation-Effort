using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScaledCamera : MonoBehaviour
{
	public float fovDefault = 60f;

	public Camera galaxyCamera;

	public Camera cam;

	public Transform tgtRef;

	public static ScaledCamera Instance { get; set; }

	public void Awake()
	{
		Instance = this;
		cam = GetComponent<Camera>();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void SetFoV(float fov)
	{
		cam.fieldOfView = fov;
		galaxyCamera.fieldOfView = fov;
	}

	public void ResetFoV()
	{
		cam.fieldOfView = fovDefault;
		galaxyCamera.fieldOfView = fovDefault;
	}

	public void LateUpdate()
	{
		if ((bool)tgtRef)
		{
			base.transform.position = ScaledSpace.LocalToScaledSpace(tgtRef.position);
			base.transform.localRotation = tgtRef.rotation;
		}
	}

	public void SetTarget(Transform newTarget)
	{
		tgtRef = newTarget;
	}

	public void SetCameraClearFlag(CameraClearFlags clearFlag, Color color)
	{
		cam.clearFlags = clearFlag;
		cam.backgroundColor = color;
	}

	public void ResetTarget()
	{
		tgtRef = FlightCamera.fetch.transform;
		cam.clearFlags = CameraClearFlags.Depth;
		cam.targetTexture = null;
	}
}
