using UnityEngine;

public class SystemInformation : MonoBehaviour
{
	public enum Renderer
	{
		OpenGL,
		DirectX
	}

	public static bool isDirectX = true;

	public void Awake()
	{
		isDirectX = !SystemInfo.graphicsDeviceVersion.Contains("OpenGL");
	}
}
