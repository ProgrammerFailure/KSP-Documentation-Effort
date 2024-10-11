using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class UIMasterController : MonoBehaviour
{
	[Serializable]
	public class CanvasWrapper
	{
		public string name;

		public Canvas canvas;

		public bool removeOnSceneSwitch;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CanvasWrapper(string name, Canvas canvas, bool removeOnSceneSwitch)
		{
			throw null;
		}
	}

	private const float TooltipStandoffLength = 5f;

	public Canvas mainCanvas;

	private RectTransform mainCanvasRt;

	public Canvas appCanvas;

	public Canvas actionCanvas;

	public Canvas screenMessageCanvas;

	public Canvas dialogCanvas;

	public Canvas tooltipCanvas;

	public Canvas tweeningCanvas;

	public Canvas dragDropCanvas;

	public Canvas debugCanvas;

	public Camera uiCamera;

	public Camera vectorCamera;

	public float uiScale;

	public bool forceNavigationMode;

	public Navigation.Mode navigationMode;

	private int screenWidth;

	private int screenHeight;

	[SerializeField]
	private bool isUIShowing;

	[SerializeField]
	private bool cameraMode;

	private List<CanvasWrapper> canvases;

	private DictionaryValueList<Tooltip, Tooltip> tooltipBackups;

	private ITooltipController currentTooltip;

	private List<CanvasGroup> modalDialogs;

	private List<CanvasGroup> nonModalDialogs;

	public static UIMasterController Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool IsUIShowing
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CameraMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ITooltipController CurrentTooltip
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIMasterController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnShowUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHideUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneChange(GameScenes scenes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupCameraAndScale()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScale(float uiScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetMaxSuggestedUIScale()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAppScale(float appScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckScreenResolution()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetUIMainCanvasPixelPerfect(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetUIActionCanvasPixelPerfect(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetUITooltipCanvasPixelPerfect(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddCanvas(UICanvasPrefab canvasPrefab, bool removeOnSceneSwitch = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddCanvas(Canvas parentCanvas, UICanvasPrefab canvasPrefab, bool removeOnSceneSwitch = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddCanvas(Canvas canvasPrefab, bool removeOnSceneSwitch = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveCanvas(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveCanvas(UICanvasPrefab controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveCanvas(Canvas canvasPrefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetNavigationMode(GameObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private CanvasWrapper GetCanvasWrapper(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanSpawnCanvasPrefab(UICanvasPrefab canvasPrefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PinTooltip(IPinnableTooltipController tooltipController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnpinTooltip(IPinnableTooltipController tooltipController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnTooltip(ITooltipController tooltipController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnTooltip(ITooltipController tooltipController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyCurrentTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RepositionTooltip(RectTransform rect, Vector2 cursorStandoff, float cursorBottomRightLength = 8f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DragTooltip(RectTransform rect, Vector2 mouseDelta, Vector2 cursorStandoff)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClampToWindow(RectTransform parentRectTransform, RectTransform panelRectTransform, Vector2 screenEdgeOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClampToWindow(RectTransform parentRectTransform, RectTransform panelRectTransform, float topEdgeOffset, float bottomEdgeOffset, float leftEdgeOffset, float rightEdgeOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClampToScreen(RectTransform panelRectTransform, Vector2 screenEdgeOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClampToScreen(RectTransform panelRectTransform, float topEdgeOffset, float bottomEdgeOffset, float leftEdgeOffset, float rightEdgeOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CutToWindow(RectTransform parentRectTransform, RectTransform panelRectTransform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CutToWindow(RectTransform parentRectTransform, RectTransform panelRectTransform, bool cutTop, bool cutBottom, bool cutLeft, bool cutRight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CutToScreen(RectTransform panelRectTransform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CutToScreen(RectTransform panelRectTransform, bool cutTop, bool cutBottom, bool cutLeft, bool cutRight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool AnyCornerOffScreen(RectTransform rect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 WorldToMainCanvas(Vector3 worldPosition, Camera cam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RegisterNonModalDialog(CanvasGroup d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnregisterNonModalDialog(CanvasGroup d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RegisterModalDialog(CanvasGroup d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnregisterModalDialog(CanvasGroup d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnregisterModalDialogs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FocusModalDialog(CanvasGroup c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetupNavigationMode(GameObject obj)
	{
		throw null;
	}
}
