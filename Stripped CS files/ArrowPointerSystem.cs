using UnityEngine;

public class ArrowPointerSystem : MonoBehaviour
{
	public Material material;

	public float baseSize = 0.1f;

	public static ArrowPointerSystem Instance { get; set; }

	public static Material Material => Instance.material;

	public static float BaseSize => Instance.baseSize;

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
