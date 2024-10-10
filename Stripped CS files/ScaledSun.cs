using UnityEngine;

public class ScaledSun : MonoBehaviour
{
	public static ScaledSun Instance { get; set; }

	public void Awake()
	{
		if (Instance != null)
		{
			Object.DestroyImmediate(this);
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
