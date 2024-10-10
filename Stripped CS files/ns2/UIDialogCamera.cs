using UnityEngine;

namespace ns2;

public class UIDialogCamera : UICameraBase
{
	public static UIDialogCamera Instance;

	public static Camera Camera => Instance.cam;

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
