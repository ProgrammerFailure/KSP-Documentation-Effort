using UnityEngine;

public class FXDepthCamera : MonoBehaviour
{
	public Shader ReplacementShader;

	public Camera depthCamera;

	public float startFarClipPlane = 300f;

	public float farClipPlaneBuffer = 100f;

	public void Start()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnSceneSwitch);
		GameEvents.OnCameraDistanceAdjustedToFitVessel.Add(OnCameraDistanceAdjustedToFitVessel);
		depthCamera.SetReplacementShader(ReplacementShader, "RenderType");
		startFarClipPlane = depthCamera.farClipPlane;
	}

	public void OnSceneSwitch(GameScenes scene)
	{
		Object.DestroyImmediate(base.gameObject);
	}

	public void LateUpdate()
	{
		if (FlightCamera.fetch != null)
		{
			depthCamera.fieldOfView = FlightCamera.fetch.FieldOfView;
			depthCamera.farClipPlane = Mathf.Max(startFarClipPlane, FlightCamera.fetch.Distance + farClipPlaneBuffer);
		}
	}

	public void OnCameraDistanceAdjustedToFitVessel(Vessel v, float newDistance, Vector3 newBounds)
	{
		farClipPlaneBuffer = Mathf.Max(Mathf.Max(newBounds.x, newBounds.y), newBounds.z);
	}

	public void OnPreRender()
	{
		if (depthCamera.enabled)
		{
			depthCamera.SetReplacementShader(ReplacementShader, "RenderType");
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneSwitch);
		GameEvents.OnCameraDistanceAdjustedToFitVessel.Remove(OnCameraDistanceAdjustedToFitVessel);
	}
}
