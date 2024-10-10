using System;
using TMPro;
using UnityEngine;

public class MainMenuEnvLogic : MonoBehaviour
{
	[Serializable]
	public class MenuStage
	{
		public Transform targetPoint;

		public Transform initialPoint;
	}

	public Camera landscapeCamera;

	public GameObject[] areas;

	public GameObject startingArea;

	public MenuStage[] camPivots;

	public bool randomAreaAtStartup;

	public float cameraChaseSpeed = 2f;

	public MeshRenderer[] uiRenderers;

	public TextMeshPro[] uiTexts;

	public float fadeStartDistance = 1f;

	public float fadeEndDistance = 5f;

	public static int CameraStage;

	public int currentStage;

	public Vector3 tgtPos;

	public Vector3 cVel;

	public Quaternion tgtRot;

	public void Awake()
	{
		if (MainMenu.MainMenuVisited && randomAreaAtStartup)
		{
			UnityEngine.Random.InitState(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
			areas[UnityEngine.Random.Range(0, areas.Length)].SetActive(value: true);
		}
		else if (startingArea != null)
		{
			startingArea.SetActive(value: true);
		}
		else
		{
			areas[1].SetActive(value: true);
		}
		tgtPos = camPivots[CameraStage].targetPoint.position;
		tgtRot = camPivots[CameraStage].targetPoint.rotation;
		landscapeCamera.transform.position = camPivots[CameraStage].initialPoint.position;
		landscapeCamera.transform.rotation = camPivots[CameraStage].initialPoint.rotation;
		currentStage = CameraStage;
	}

	public void Update()
	{
		if ((bool)landscapeCamera)
		{
			landscapeCamera.transform.position = Vector3.SmoothDamp(landscapeCamera.transform.position, tgtPos, ref cVel, cameraChaseSpeed);
			landscapeCamera.transform.rotation = Quaternion.Lerp(landscapeCamera.transform.rotation, tgtRot, cameraChaseSpeed * Time.deltaTime);
			DistanceFadeUI();
		}
	}

	[ContextMenu("Go Current Stage")]
	public void GoCurrentStage()
	{
		GoToStage(CameraStage);
		currentStage = CameraStage;
	}

	public void GoToStage(int st)
	{
		CameraStage = Mathf.Clamp(st, 0, areas.Length);
		tgtPos = camPivots[CameraStage].targetPoint.position;
		tgtRot = camPivots[CameraStage].targetPoint.rotation;
		cVel = Vector3.zero;
		currentStage = CameraStage;
	}

	public void DistanceFadeUI()
	{
		int num = uiRenderers.Length;
		while (num-- > 0)
		{
			MeshRenderer meshRenderer = uiRenderers[num];
			float sqrMagnitude = (landscapeCamera.transform.position - meshRenderer.transform.position).sqrMagnitude;
			Color color = meshRenderer.material.color;
			color.a = Mathf.InverseLerp(fadeEndDistance * fadeEndDistance, fadeStartDistance * fadeStartDistance, sqrMagnitude);
			meshRenderer.material.color = color;
		}
		int num2 = uiTexts.Length;
		while (num2-- > 0)
		{
			TextMeshPro textMeshPro = uiTexts[num2];
			float sqrMagnitude2 = (landscapeCamera.transform.position - textMeshPro.transform.position).sqrMagnitude;
			Color color2 = textMeshPro.color;
			color2.a = Mathf.InverseLerp(fadeEndDistance * fadeEndDistance, fadeStartDistance * fadeStartDistance, sqrMagnitude2);
			textMeshPro.color = color2;
		}
	}
}
