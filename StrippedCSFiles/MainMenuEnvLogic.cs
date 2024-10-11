using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MainMenuEnvLogic : MonoBehaviour
{
	[Serializable]
	public class MenuStage
	{
		public Transform targetPoint;

		public Transform initialPoint;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MenuStage()
		{
			throw null;
		}
	}

	public Camera landscapeCamera;

	public GameObject[] areas;

	public GameObject startingArea;

	public MenuStage[] camPivots;

	public bool randomAreaAtStartup;

	public float cameraChaseSpeed;

	public MeshRenderer[] uiRenderers;

	public TextMeshPro[] uiTexts;

	public float fadeStartDistance;

	public float fadeEndDistance;

	private static int CameraStage;

	public int currentStage;

	private Vector3 tgtPos;

	private Vector3 cVel;

	private Quaternion tgtRot;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MainMenuEnvLogic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Go Current Stage")]
	public void GoCurrentStage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GoToStage(int st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DistanceFadeUI()
	{
		throw null;
	}
}
