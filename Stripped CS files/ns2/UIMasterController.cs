using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class UIMasterController : MonoBehaviour
{
	[Serializable]
	public class CanvasWrapper
	{
		public string name;

		public Canvas canvas;

		public bool removeOnSceneSwitch;

		public CanvasWrapper(string name, Canvas canvas, bool removeOnSceneSwitch)
		{
			this.name = name;
			this.canvas = canvas;
			this.removeOnSceneSwitch = removeOnSceneSwitch;
		}
	}

	public const float TooltipStandoffLength = 5f;

	public Canvas mainCanvas;

	public RectTransform mainCanvasRt;

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

	public float uiScale = 1f;

	public bool forceNavigationMode = true;

	public Navigation.Mode navigationMode;

	public int screenWidth;

	public int screenHeight;

	[SerializeField]
	public bool isUIShowing;

	[SerializeField]
	public bool cameraMode;

	public List<CanvasWrapper> canvases = new List<CanvasWrapper>();

	public DictionaryValueList<Tooltip, Tooltip> tooltipBackups = new DictionaryValueList<Tooltip, Tooltip>();

	public ITooltipController currentTooltip;

	public List<CanvasGroup> modalDialogs = new List<CanvasGroup>();

	public List<CanvasGroup> nonModalDialogs = new List<CanvasGroup>();

	public static UIMasterController Instance { get; set; }

	public bool IsUIShowing => isUIShowing;

	public bool CameraMode => cameraMode;

	public ITooltipController CurrentTooltip => currentTooltip;

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("UIMasterController: Only one instance can exist at any time. Destroying potential usurper.", Instance.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		mainCanvasRt = mainCanvas.GetComponent<RectTransform>();
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		SetupCameraAndScale();
		SetUIMainCanvasPixelPerfect(GameSettings.UI_MAINCANVAS_PIXEL_PERFECT);
		SetUIActionCanvasPixelPerfect(GameSettings.UI_ACTIONCANVAS_PIXEL_PERFECT);
		SetUITooltipCanvasPixelPerfect(GameSettings.UI_TOOLTIPCANVAS_PIXEL_PERFECT);
	}

	public void Start()
	{
		GameEvents.onShowUI.Add(OnShowUI);
		GameEvents.onHideUI.Add(OnHideUI);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
	}

	public void OnDestroy()
	{
		GameEvents.onShowUI.Remove(OnShowUI);
		GameEvents.onHideUI.Remove(OnHideUI);
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnShowUI()
	{
		if (uiCamera != null)
		{
			uiCamera.enabled = true;
		}
		mainCanvas.enabled = true;
		appCanvas.enabled = true;
		actionCanvas.enabled = true;
		screenMessageCanvas.enabled = true;
		dialogCanvas.enabled = true;
		tooltipCanvas.enabled = true;
		isUIShowing = true;
	}

	public void OnHideUI()
	{
		if (uiCamera != null)
		{
			uiCamera.enabled = false;
		}
		mainCanvas.enabled = false;
		appCanvas.enabled = false;
		actionCanvas.enabled = false;
		screenMessageCanvas.enabled = false;
		dialogCanvas.enabled = false;
		tooltipCanvas.enabled = false;
		isUIShowing = false;
	}

	public void OnSceneChange(GameScenes scenes)
	{
		for (int i = 0; i < tooltipBackups.Count; i++)
		{
			Tooltip tooltip = tooltipBackups.At(i);
			if (tooltip != null)
			{
				UnityEngine.Object.Destroy(tooltip.gameObject);
			}
		}
		tooltipBackups.Clear();
		int count = canvases.Count;
		while (count-- > 0)
		{
			if (canvases[count].removeOnSceneSwitch)
			{
				UnityEngine.Object.Destroy(canvases[count].canvas.gameObject);
				canvases.RemoveAt(count);
			}
		}
		PopupDialog.ClearPopUps();
		UnregisterModalDialogs();
		OnShowUI();
	}

	public void SetupCameraAndScale()
	{
		if (uiCamera != null)
		{
			uiCamera.orthographicSize = (float)Screen.height / 2f;
		}
		SetScale(uiScale);
	}

	public void Update()
	{
		if (Application.isEditor)
		{
			SetupCameraAndScale();
		}
		cameraMode = FlightDriver.Pause && !Instance.IsUIShowing;
		SetScale(uiScale);
		CheckScreenResolution();
		UpdateTooltip();
	}

	public void LateUpdate()
	{
		if (Input.GetKeyUp(KeyCode.Escape) && cameraMode)
		{
			Instance.ShowUI();
			GameEvents.onShowUI.Fire();
		}
	}

	public void SetScale(float uiScale)
	{
		if (this.uiScale != uiScale)
		{
			this.uiScale = uiScale;
			if (mainCanvas != null)
			{
				mainCanvas.scaleFactor = uiScale;
			}
			if (actionCanvas != null)
			{
				actionCanvas.scaleFactor = uiScale;
			}
			if (screenMessageCanvas != null)
			{
				screenMessageCanvas.scaleFactor = uiScale;
			}
			if (dialogCanvas != null)
			{
				dialogCanvas.scaleFactor = uiScale;
			}
			if (tooltipCanvas != null)
			{
				tooltipCanvas.scaleFactor = uiScale;
			}
			if (tweeningCanvas != null)
			{
				tweeningCanvas.scaleFactor = uiScale;
			}
			if (dragDropCanvas != null)
			{
				dragDropCanvas.scaleFactor = uiScale;
			}
			if (debugCanvas != null)
			{
				debugCanvas.scaleFactor = uiScale;
			}
			if (appCanvas != null)
			{
				appCanvas.scaleFactor = uiScale * GameSettings.UI_SCALE_APPS;
			}
			Canvas.ForceUpdateCanvases();
		}
		if (appCanvas != null && appCanvas.scaleFactor != uiScale)
		{
			appCanvas.scaleFactor = uiScale * GameSettings.UI_SCALE_APPS;
			Canvas.ForceUpdateCanvases();
		}
	}

	public float GetMaxSuggestedUIScale()
	{
		return Mathf.Clamp((float)Instance.uiCamera.pixelWidth / 1000f, 1f, 2f);
	}

	public void SetAppScale(float appScale)
	{
		if (!(appCanvas == null) && appCanvas.scaleFactor != appScale)
		{
			appCanvas.scaleFactor = appScale;
			Canvas.ForceUpdateCanvases();
		}
	}

	public void CheckScreenResolution()
	{
		int width = Screen.width;
		int height = Screen.height;
		if (screenWidth != width || screenHeight != height)
		{
			screenWidth = width;
			screenHeight = height;
			GameEvents.onScreenResolutionModified.Fire(screenWidth, screenHeight);
		}
	}

	public void SetUIMainCanvasPixelPerfect(bool newValue)
	{
		if (mainCanvas != null)
		{
			mainCanvas.pixelPerfect = newValue;
		}
	}

	public void SetUIActionCanvasPixelPerfect(bool newValue)
	{
		if (actionCanvas != null)
		{
			actionCanvas.pixelPerfect = newValue;
		}
	}

	public void SetUITooltipCanvasPixelPerfect(bool newValue)
	{
		if (tooltipCanvas != null)
		{
			tooltipCanvas.pixelPerfect = newValue;
		}
	}

	public bool AddCanvas(UICanvasPrefab canvasPrefab, bool removeOnSceneSwitch = true)
	{
		return AddCanvas(mainCanvas, canvasPrefab, removeOnSceneSwitch);
	}

	public bool AddCanvas(Canvas parentCanvas, UICanvasPrefab canvasPrefab, bool removeOnSceneSwitch = true)
	{
		if (canvasPrefab == null)
		{
			return false;
		}
		string text = (string.IsNullOrEmpty(canvasPrefab.canvasName) ? canvasPrefab.gameObject.name : canvasPrefab.canvasName);
		if (GetCanvasWrapper(text) != null)
		{
			Debug.LogError("[UIMasterController]: Canvas named '" + text + "' already exists. Cannot spawn another of same name.");
			return false;
		}
		Canvas canvas = UnityEngine.Object.Instantiate(canvasPrefab.canvas);
		canvas.gameObject.name = text;
		RectTransform obj = (RectTransform)canvas.transform;
		obj.SetParent(parentCanvas.transform, worldPositionStays: false);
		obj.SetAsLastSibling();
		if (forceNavigationMode)
		{
			SetNavigationMode(canvas.gameObject);
		}
		canvases.Add(new CanvasWrapper(text, canvas, removeOnSceneSwitch));
		return true;
	}

	public bool AddCanvas(Canvas canvasPrefab, bool removeOnSceneSwitch = true)
	{
		if (canvasPrefab == null)
		{
			return false;
		}
		if (GetCanvasWrapper(canvasPrefab.gameObject.name) != null)
		{
			Debug.LogError("[UIMasterController]: Canvas named '" + canvasPrefab.gameObject.name + "' already exists. Cannot spawn another of same name.");
			return false;
		}
		Canvas canvas = UnityEngine.Object.Instantiate(canvasPrefab);
		canvas.gameObject.name = canvasPrefab.gameObject.name;
		RectTransform obj = (RectTransform)canvas.transform;
		obj.SetParent(mainCanvas.transform, worldPositionStays: false);
		obj.SetAsLastSibling();
		if (forceNavigationMode)
		{
			SetNavigationMode(canvas.gameObject);
		}
		canvases.Add(new CanvasWrapper(canvasPrefab.gameObject.name, canvas, removeOnSceneSwitch));
		return true;
	}

	public bool RemoveCanvas(string name)
	{
		int count = canvases.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(canvases[count].name == name));
		UnityEngine.Object.Destroy(canvases[count].canvas.gameObject);
		canvases.RemoveAt(count);
		return true;
	}

	public bool RemoveCanvas(UICanvasPrefab controller)
	{
		Debug.Log("[UIMasterController] RemoveCanvas:" + controller.canvasName);
		string text = "";
		text = ((!string.IsNullOrEmpty(controller.canvasName)) ? controller.canvasName : controller.gameObject.name);
		return RemoveCanvas(text);
	}

	public bool RemoveCanvas(Canvas canvasPrefab)
	{
		string text = canvasPrefab.gameObject.name;
		return RemoveCanvas(text);
	}

	public void SetNavigationMode(GameObject obj)
	{
		Selectable[] componentsInChildren = obj.GetComponentsInChildren<Selectable>();
		int num = componentsInChildren.Length;
		while (num-- > 0)
		{
			Navigation navigation = componentsInChildren[num].navigation;
			navigation.mode = navigationMode;
			componentsInChildren[num].navigation = navigation;
		}
	}

	public CanvasWrapper GetCanvasWrapper(string name)
	{
		int count = canvases.Count;
		do
		{
			if (count-- <= 0)
			{
				return null;
			}
		}
		while (!(canvases[count].name == name));
		return canvases[count];
	}

	public bool CanSpawnCanvasPrefab(UICanvasPrefab canvasPrefab)
	{
		string text = (string.IsNullOrEmpty(canvasPrefab.canvasName) ? canvasPrefab.gameObject.name : canvasPrefab.canvasName);
		return GetCanvasWrapper(text) == null;
	}

	public void ShowUI()
	{
		Debug.Log("[UIMasterController]: ShowUI");
		uiCamera.enabled = true;
		mainCanvas.enabled = true;
		appCanvas.enabled = true;
		actionCanvas.enabled = true;
		screenMessageCanvas.enabled = true;
		dialogCanvas.enabled = true;
		tooltipCanvas.enabled = true;
		tweeningCanvas.enabled = true;
		dragDropCanvas.enabled = true;
		debugCanvas.enabled = true;
	}

	public void HideUI()
	{
		Debug.Log("[UIMasterController]: HideUI");
		uiCamera.enabled = false;
		mainCanvas.enabled = false;
		appCanvas.enabled = false;
		actionCanvas.enabled = false;
		screenMessageCanvas.enabled = false;
		dialogCanvas.enabled = false;
		tweeningCanvas.enabled = false;
		debugCanvas.enabled = false;
	}

	public void PinTooltip(IPinnableTooltipController tooltipController)
	{
		if (currentTooltip == tooltipController)
		{
			tooltipController.OnTooltipPinned();
		}
	}

	public void UnpinTooltip(IPinnableTooltipController tooltipController)
	{
		tooltipController.OnTooltipUnpinned();
	}

	public void SpawnTooltip(ITooltipController tooltipController)
	{
		if (tooltipController == currentTooltip)
		{
			return;
		}
		DestroyCurrentTooltip();
		if (tooltipController.TooltipPrefabType == null)
		{
			Debug.LogError("TooltipController '" + tooltipController.name + "' has no prefab type defined");
		}
		else if (GameEvents.onTooltipAboutToSpawn.Fire(tooltipController.OnTooltipAboutToSpawn(), tooltipController))
		{
			currentTooltip = tooltipController;
			Tooltip val = null;
			tooltipBackups.TryGetValue(tooltipController.TooltipPrefabType, out val);
			if (val == null)
			{
				currentTooltip.TooltipPrefabInstance = UnityEngine.Object.Instantiate(tooltipController.TooltipPrefabType);
				tooltipBackups.Add(tooltipController.TooltipPrefabType, currentTooltip.TooltipPrefabInstance);
				currentTooltip.TooltipPrefabInstance.transform.SetParent(tooltipCanvas.transform, worldPositionStays: false);
			}
			else
			{
				currentTooltip.TooltipPrefabInstance = val;
				currentTooltip.TooltipPrefabInstance.gameObject.SetActive(value: true);
			}
			currentTooltip.TooltipPrefabInstanceTransform = (RectTransform)currentTooltip.TooltipPrefabInstance.transform;
			currentTooltip.OnTooltipSpawned(currentTooltip.TooltipPrefabInstance);
			GameEvents.onTooltipSpawned.Fire(currentTooltip, currentTooltip.TooltipPrefabInstance);
			currentTooltip.OnTooltipUpdate(currentTooltip.TooltipPrefabInstance);
			GameEvents.onTooltipUpdate.Fire(d1: true, currentTooltip.TooltipPrefabInstance);
			Canvas.ForceUpdateCanvases();
			UpdatePosition();
		}
	}

	public void DespawnTooltip(ITooltipController tooltipController)
	{
		if (tooltipController == currentTooltip && GameEvents.onTooltipAboutToDespawn.Fire(tooltipController.OnTooltipAboutToDespawn(), tooltipController))
		{
			DestroyCurrentTooltip();
		}
	}

	public void DestroyCurrentTooltip()
	{
		if (currentTooltip != null)
		{
			if (currentTooltip is IPinnableTooltipController pinnableTooltipController && pinnableTooltipController.IsPinned())
			{
				pinnableTooltipController.Unpin();
			}
			if (currentTooltip.TooltipPrefabInstance != null)
			{
				currentTooltip.OnTooltipDespawned(currentTooltip.TooltipPrefabInstance);
				GameEvents.onTooltipDespawned.Fire(currentTooltip.TooltipPrefabInstance);
				currentTooltip.TooltipPrefabInstance.gameObject.SetActive(value: false);
				currentTooltip.TooltipPrefabInstance = null;
				currentTooltip.TooltipPrefabInstanceTransform = null;
			}
			currentTooltip = null;
		}
	}

	public void UpdateTooltip()
	{
		if (currentTooltip == null || !(currentTooltip.TooltipPrefabInstance != null))
		{
			return;
		}
		if (currentTooltip.OnTooltipUpdate(currentTooltip.TooltipPrefabInstance) && GameEvents.onTooltipUpdate.Fire(d1: true, currentTooltip.TooltipPrefabInstance))
		{
			if (!(currentTooltip is IPinnableTooltipController pinnableTooltipController) || (pinnableTooltipController != null && !pinnableTooltipController.IsPinned()))
			{
				UpdatePosition();
			}
		}
		else
		{
			DestroyCurrentTooltip();
		}
	}

	public void UpdatePosition()
	{
		RepositionTooltip(currentTooltip.TooltipPrefabInstanceTransform, Vector2.one);
	}

	public static void RepositionTooltip(RectTransform rect, Vector2 cursorStandoff, float cursorBottomRightLength = 8f)
	{
		Vector3 mousePosition = Input.mousePosition;
		float num = 0f;
		float num2 = 0f;
		mousePosition.z = 0f;
		rect.pivot = new Vector2(0f, 0f);
		float num3 = Instance.uiCamera.pixelWidth;
		float num4 = Instance.uiCamera.pixelHeight;
		Vector2 vector = rect.sizeDelta * Instance.uiScale;
		if (!(cursorStandoff.magnitude > 5f))
		{
			num = ((!(vector.x + mousePosition.x + cursorBottomRightLength * cursorStandoff.x > num3)) ? ((5f + cursorBottomRightLength) * cursorStandoff.x) : ((0f - (vector.x + 5f)) * cursorStandoff.x));
			num2 = ((mousePosition.y - vector.y - cursorBottomRightLength * cursorStandoff.y > 0f) ? ((0f - (vector.y + 5f + cursorBottomRightLength)) * cursorStandoff.y) : ((!(vector.y + mousePosition.y + cursorBottomRightLength * cursorStandoff.y > num4)) ? (5f * cursorStandoff.y) : ((0f - (vector.y + 5f + cursorBottomRightLength)) * cursorStandoff.y / 2f)));
		}
		else
		{
			num = cursorStandoff.x;
			num2 = cursorStandoff.y;
		}
		rect.anchoredPosition = new Vector3(mousePosition.x + num, mousePosition.y + num2, mousePosition.z) / Instance.uiScale;
	}

	public static void DragTooltip(RectTransform rect, Vector2 mouseDelta, Vector2 cursorStandoff)
	{
		Vector3 position = rect.position;
		position.x += mouseDelta.x;
		position.y += mouseDelta.y;
		rect.position = position;
		ClampToWindow(Instance.mainCanvasRt, rect, cursorStandoff);
	}

	public static void ClampToWindow(RectTransform parentRectTransform, RectTransform panelRectTransform, Vector2 screenEdgeOffset)
	{
		Vector3 localPosition = panelRectTransform.localPosition;
		Vector3 vector = parentRectTransform.rect.min + screenEdgeOffset - panelRectTransform.rect.min;
		Vector3 vector2 = parentRectTransform.rect.max - screenEdgeOffset - panelRectTransform.rect.max;
		if (vector.y > vector2.y)
		{
			float y = vector.y;
			vector.y = vector2.y;
			vector2.y = y;
		}
		if (vector.x > vector2.x)
		{
			float x = vector.x;
			vector.x = vector2.x;
			vector2.x = x;
		}
		localPosition.x = Mathf.Clamp(panelRectTransform.localPosition.x, vector.x, vector2.x);
		localPosition.y = Mathf.Clamp(panelRectTransform.localPosition.y, vector.y, vector2.y);
		panelRectTransform.localPosition = localPosition;
	}

	public static void ClampToWindow(RectTransform parentRectTransform, RectTransform panelRectTransform, float topEdgeOffset, float bottomEdgeOffset, float leftEdgeOffset, float rightEdgeOffset)
	{
		Vector3 localPosition = panelRectTransform.localPosition;
		float num = parentRectTransform.rect.min.x + leftEdgeOffset - panelRectTransform.rect.min.x;
		float num2 = parentRectTransform.rect.min.y + bottomEdgeOffset - panelRectTransform.rect.min.y;
		float num3 = parentRectTransform.rect.max.x - rightEdgeOffset - panelRectTransform.rect.max.x;
		float num4 = parentRectTransform.rect.max.y - topEdgeOffset - panelRectTransform.rect.max.y;
		if (num2 > num4)
		{
			float num5 = num2;
			num2 = num4;
			num4 = num5;
		}
		if (num > num3)
		{
			float num6 = num;
			num = num3;
			num3 = num6;
		}
		localPosition.x = Mathf.Clamp(panelRectTransform.localPosition.x, num, num3);
		localPosition.y = Mathf.Clamp(panelRectTransform.localPosition.y, num2, num4);
		panelRectTransform.localPosition = localPosition;
	}

	public static void ClampToScreen(RectTransform panelRectTransform, Vector2 screenEdgeOffset)
	{
		ClampToWindow(Instance.mainCanvasRt, panelRectTransform, screenEdgeOffset);
	}

	public static void ClampToScreen(RectTransform panelRectTransform, float topEdgeOffset, float bottomEdgeOffset, float leftEdgeOffset, float rightEdgeOffset)
	{
		ClampToWindow(Instance.mainCanvasRt, panelRectTransform, topEdgeOffset, bottomEdgeOffset, leftEdgeOffset, rightEdgeOffset);
	}

	public static void CutToWindow(RectTransform parentRectTransform, RectTransform panelRectTransform)
	{
		panelRectTransform.offsetMin = new Vector2(Mathf.Max(panelRectTransform.offsetMin.x, parentRectTransform.offsetMin.x), Mathf.Max(panelRectTransform.offsetMin.y, parentRectTransform.offsetMin.y));
		panelRectTransform.offsetMax = new Vector2(Mathf.Min(panelRectTransform.offsetMax.x, parentRectTransform.offsetMax.x), Mathf.Min(panelRectTransform.offsetMax.y, parentRectTransform.offsetMax.y));
	}

	public static void CutToWindow(RectTransform parentRectTransform, RectTransform panelRectTransform, bool cutTop, bool cutBottom, bool cutLeft, bool cutRight)
	{
		if (cutTop)
		{
			panelRectTransform.offsetMax = new Vector2(panelRectTransform.offsetMax.x, Mathf.Min(panelRectTransform.offsetMax.y, parentRectTransform.offsetMax.y));
		}
		if (cutBottom)
		{
			panelRectTransform.offsetMin = new Vector2(panelRectTransform.offsetMin.x, Mathf.Max(panelRectTransform.offsetMin.y, parentRectTransform.offsetMin.y));
		}
		if (cutLeft)
		{
			panelRectTransform.offsetMin = new Vector2(Mathf.Max(panelRectTransform.offsetMin.x, parentRectTransform.offsetMin.x), panelRectTransform.offsetMin.y);
		}
		if (cutRight)
		{
			panelRectTransform.offsetMax = new Vector2(Mathf.Min(panelRectTransform.offsetMax.x, parentRectTransform.offsetMax.x), panelRectTransform.offsetMax.y);
		}
	}

	public static void CutToScreen(RectTransform panelRectTransform)
	{
		CutToWindow(Instance.mainCanvasRt, panelRectTransform);
	}

	public static void CutToScreen(RectTransform panelRectTransform, bool cutTop, bool cutBottom, bool cutLeft, bool cutRight)
	{
		CutToWindow(Instance.mainCanvasRt, panelRectTransform, cutTop, cutBottom, cutLeft, cutRight);
	}

	public static bool AnyCornerOffScreen(RectTransform rect)
	{
		Vector3[] array = new Vector3[4];
		rect.GetWorldCorners(array);
		int num = 0;
		while (true)
		{
			if (num < 4)
			{
				Vector3 vector = Instance.uiCamera.WorldToViewportPoint(array[num]);
				if (vector.x < 0f || vector.x > 1f || vector.y < 0f || !(vector.y <= 1f))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public static Vector3 WorldToMainCanvas(Vector3 worldPosition, Camera cam)
	{
		return cam.WorldToScreenPoint(worldPosition) / Instance.uiScale;
	}

	public void RegisterNonModalDialog(CanvasGroup d)
	{
		if (!nonModalDialogs.Contains(d))
		{
			nonModalDialogs.Add(d);
		}
	}

	public void UnregisterNonModalDialog(CanvasGroup d)
	{
		if (nonModalDialogs.Contains(d))
		{
			nonModalDialogs.Remove(d);
		}
	}

	public void RegisterModalDialog(CanvasGroup d)
	{
		if (!modalDialogs.Contains(d))
		{
			modalDialogs.Add(d);
			if (modalDialogs.Count == 1)
			{
				InputLockManager.SetControlLock(ControlTypes.UI_DIALOGS, "UIMasterController:ModalDialog");
			}
			FocusModalDialog(d);
		}
	}

	public void UnregisterModalDialog(CanvasGroup d)
	{
		if (!modalDialogs.Contains(d))
		{
			return;
		}
		if (modalDialogs.Count == 1)
		{
			InputLockManager.RemoveControlLock("UIMasterController:ModalDialog");
		}
		modalDialogs.Remove(d);
		if (modalDialogs.Count > 0)
		{
			FocusModalDialog(modalDialogs[modalDialogs.Count - 1]);
			return;
		}
		int i = 0;
		for (int count = nonModalDialogs.Count; i < count; i++)
		{
			nonModalDialogs[i].blocksRaycasts = true;
		}
	}

	public void UnregisterModalDialogs()
	{
		modalDialogs.Clear();
		InputLockManager.RemoveControlLock("UIMasterController:ModalDialog");
	}

	public void FocusModalDialog(CanvasGroup c)
	{
		if (modalDialogs.Contains(c))
		{
			if (modalDialogs[modalDialogs.Count - 1] != c)
			{
				modalDialogs.Remove(c);
				modalDialogs.Add(c);
			}
			c.transform.SetAsLastSibling();
			c.blocksRaycasts = true;
			int i = 0;
			for (int count = modalDialogs.Count; i < count - 1; i++)
			{
				modalDialogs[i].blocksRaycasts = false;
			}
			int j = 0;
			for (int count2 = nonModalDialogs.Count; j < count2; j++)
			{
				nonModalDialogs[j].blocksRaycasts = false;
			}
		}
	}

	public static void SetupNavigationMode(GameObject obj)
	{
		Instance.SetNavigationMode(obj);
	}
}
