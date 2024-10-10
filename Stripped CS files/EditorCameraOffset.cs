using UnityEngine;

[RequireComponent(typeof(CameraOffCenter))]
public class EditorCameraOffset : MonoBehaviour
{
	public float SidebarPixelWidth = 256f;

	public float sidebarScreenWidth;

	public CameraOffCenter cameraOffCenter;

	public void Start()
	{
		cameraOffCenter = GetComponent<CameraOffCenter>();
	}

	public void Update()
	{
		sidebarScreenWidth = SidebarPixelWidth / (float)Screen.width;
		cameraOffCenter.x = sidebarScreenWidth;
		cameraOffCenter.y = 0f;
	}
}
