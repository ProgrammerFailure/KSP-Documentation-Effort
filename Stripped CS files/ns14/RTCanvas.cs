using UniLinq;
using UnityEngine;
using UnityEngine.UI;

namespace ns14;

public class RTCanvas : MonoBehaviour
{
	[SerializeField]
	public Camera canvasCam;

	[SerializeField]
	public Canvas canvas;

	[SerializeField]
	public int dstLayer;

	public RenderTexture rt;

	public Canvas dstCanvas;

	public CanvasGroup dstCanvasGroup;

	public Camera dstCam;

	public RawImage dstImg;

	[SerializeField]
	public Canvas[] otherCanvases;

	public void Awake()
	{
		GameEvents.OnGameSettingsApplied.Add(SetUIOpacity);
	}

	public void OnDestroy()
	{
		GameEvents.OnGameSettingsApplied.Remove(SetUIOpacity);
	}

	[ContextMenu("Find Canvases Using Same Camera")]
	public void FindCanvasesUsingCamera()
	{
		otherCanvases = (from c in canvas.transform.parent.gameObject.GetComponentsInChildren<Canvas>()
			where c.worldCamera == canvasCam
			select c).ToArray();
	}

	[ContextMenu("Setup")]
	public void Setup()
	{
		SetupRTCanvas();
		SetupRT();
	}

	public void SetupRTCanvas()
	{
		this.canvas.renderMode = RenderMode.ScreenSpaceCamera;
		GameObject gameObject = new GameObject(this.canvas.name + " RT Canvas");
		dstCanvas = gameObject.AddComponent<Canvas>();
		dstCanvasGroup = gameObject.AddComponent<CanvasGroup>();
		dstCanvasGroup.blocksRaycasts = false;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(this.canvas.transform.parent);
		component.SetSiblingIndex(this.canvas.transform.GetSiblingIndex());
		component.position = this.canvas.transform.position;
		GameObject gameObject2 = new GameObject("RT");
		dstImg = gameObject2.AddComponent<RawImage>();
		dstImg.color = Color.clear;
		RectTransform component2 = gameObject2.GetComponent<RectTransform>();
		component2.SetParent(component);
		component2.anchorMin = Vector2.zero;
		component2.anchorMax = Vector2.one;
		component2.sizeDelta = Vector2.zero;
		GameObject gameObject3 = new GameObject(canvasCam.name + " RT Camera");
		dstCam = gameObject3.AddComponent<Camera>();
		dstCam.orthographic = true;
		dstCam.orthographicSize = 1f;
		dstCam.cullingMask = canvasCam.cullingMask;
		dstCam.nearClipPlane = canvasCam.nearClipPlane;
		dstCam.farClipPlane = canvasCam.farClipPlane;
		dstCam.clearFlags = canvasCam.clearFlags;
		dstCam.depth = canvasCam.depth;
		dstCam.allowHDR = false;
		Transform obj = gameObject3.transform;
		obj.SetParent(canvasCam.transform.parent);
		obj.position = canvasCam.transform.position;
		dstCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		dstCanvas.worldCamera = dstCam;
		dstCanvas.gameObject.SetActive(value: false);
		dstCanvas.gameObject.SetLayerRecursive(dstLayer);
		gameObject3.SetLayerRecursive(dstLayer);
		dstCam.cullingMask = 1 << dstLayer;
		int num = otherCanvases.Length;
		while (num-- > 0)
		{
			Canvas canvas = otherCanvases[num];
			if (canvas.worldCamera == canvasCam)
			{
				canvas.worldCamera = dstCam;
			}
		}
	}

	[ContextMenu("Setup RT")]
	public void SetupRT()
	{
		SetupRT(Screen.height, Screen.width);
	}

	public void SetupRT(int height, int width)
	{
		rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		rt.autoGenerateMips = false;
		rt.name = canvas.name + " RT";
		canvasCam.clearFlags = CameraClearFlags.Color;
		canvasCam.targetTexture = rt;
		canvasCam.orthographicSize = 1f;
		dstCanvas.gameObject.SetActive(value: true);
		dstImg.texture = rt;
		dstImg.color = Color.white;
		SetUIOpacity();
	}

	public void SetUIOpacity()
	{
		if (dstCanvasGroup != null)
		{
			dstCanvasGroup.alpha = GameSettings.UI_OPACITY;
		}
	}
}
