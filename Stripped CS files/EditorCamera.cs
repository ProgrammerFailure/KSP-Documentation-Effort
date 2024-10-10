using Highlighting;
using UnityEngine;

public class EditorCamera : MonoBehaviour
{
	public Camera cam;

	public HighlightingSystem highLightingSystem;

	public static EditorCamera Instance { get; set; }

	public void Awake()
	{
		Instance = this;
		cam = GetComponentInChildren<Camera>();
		highLightingSystem = GetComponentInChildren<HighlightingSystem>();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}
}
