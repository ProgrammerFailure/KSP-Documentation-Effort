using UnityEngine;

namespace ns2;

public class UIMainCamera : UICameraBase
{
	public static UIMainCamera Instance;

	public static Camera Camera
	{
		get
		{
			if (!Instance)
			{
				return Camera.main;
			}
			return Instance.cam;
		}
	}

	public void Awake()
	{
		Instance = this;
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}
}
