using CameraFXModules;
using UnityEngine;

public class CameraFX : MonoBehaviour
{
	public static CameraFX Instance;

	public CameraFXCollection cameraFXCollection_0;

	public void Awake()
	{
		if ((bool)Instance)
		{
			Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}
}
