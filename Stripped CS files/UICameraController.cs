using ns2;
using UnityEngine;

[ExecuteInEditMode]
public class UICameraController : MonoBehaviour
{
	public Camera cam;

	public int screenHeight = -1;

	public void Awake()
	{
		screenHeight = Screen.height;
		cam = GetComponent<Camera>();
		cam.orthographicSize = (float)screenHeight / 2f;
	}

	public void Start()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
		GameEvents.onScreenResolutionModified.Add(ScreenResolutionModified);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
		GameEvents.onScreenResolutionModified.Remove(ScreenResolutionModified);
	}

	public void ScreenResolutionModified(int newWidth, int newHeight)
	{
		if (screenHeight != newHeight)
		{
			screenHeight = newHeight;
			cam.orthographicSize = (float)screenHeight * 0.5f;
		}
	}

	public void OnSceneChange(GameScenes scenes)
	{
		UIMasterController.Instance.uiCamera.enabled = true;
	}
}
