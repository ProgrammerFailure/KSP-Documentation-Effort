using System;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class MENodeCanvas : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	public static MENodeCanvas Instance;

	[SerializeField]
	public RectTransform canvasRectTransform;

	[SerializeField]
	public RectTransform nodeRoot;

	[SerializeField]
	public Camera cameraCanvas;

	public Camera nodeCameraCanvas;

	[SerializeField]
	public AnimationCurve curveZoom;

	public float zoomPercentage;

	public float zoomDisplayPercentage;

	public float minZoomSize;

	public float maxZoomSize;

	public MissionEditorLogic meLogic;

	public Vector2 startCamPos;

	public Vector2 startDragPos;

	public Vector2 bordersMax;

	public Vector2 bordersMin;

	public Vector2 extraMargin = new Vector2(500f, 250f);

	public Vector2 nodeDragBorder = new Vector2(50f, 50f);

	public Vector2 nodeDragDir;

	public float nodeDragSpeed = 2000f;

	public float zoomSpeed = 0.1f;

	public float zoomMin = 0.2f;

	public float zoomMax = 1f;

	public float zoom = 1f;

	public bool snapToGrid;

	public bool lockScroll;

	public bool nodeDragActive;

	public MEGUINode nodeDragRef;

	public const float maxCameraSize = 10000f;

	public Camera UICamera => cameraCanvas;

	public RectTransform NodeRoot => nodeRoot;

	public bool SnapToGrid
	{
		get
		{
			return snapToGrid;
		}
		set
		{
			snapToGrid = value;
		}
	}

	public float ZoomPercentage
	{
		get
		{
			return zoomPercentage;
		}
		set
		{
			float num = maxZoomSize * (1f - value) + minZoomSize;
			OrthographicSize = num / UIMasterController.Instance.uiScale;
			zoomPercentage = num / maxZoomSize;
			float time = (OrthographicSize - minZoomSize) / 10000f;
			float num2 = curveZoom.Evaluate(time);
			zoomDisplayPercentage = 1f - num2;
		}
	}

	public float OrthographicSize
	{
		get
		{
			return cameraCanvas.orthographicSize;
		}
		set
		{
			cameraCanvas.orthographicSize = value;
			nodeCameraCanvas.orthographicSize = value;
		}
	}

	public void Awake()
	{
		Instance = this;
		minZoomSize = Screen.height / 2;
		maxZoomSize = 1600 + Screen.height / 2;
		nodeCameraCanvas = cameraCanvas.transform.GetChild(0).GetComponent<Camera>();
	}

	public void Update()
	{
		if (nodeDragActive)
		{
			Vector2 vector = nodeDragDir * (1f - zoom * 0.5f) * nodeDragSpeed * Time.deltaTime;
			Instance.cameraCanvas.transform.Translate(vector);
			if (nodeDragRef != null)
			{
				nodeDragRef.transform.Translate(vector, Space.Self);
				nodeDragRef.UpdateConnectors();
				nodeDragRef.NodeGroupDrag();
			}
		}
	}

	public void Initialize(MissionEditorLogic meLogicRef)
	{
		meLogic = meLogicRef;
		meLogic.zoom_buttonPlus.onClick.AddListener(ZoomIn);
		meLogic.zoom_buttonMinus.onClick.AddListener(ZoomOut);
		zoomMin = Mathf.Max(0.01f, GameSettings.MISSION_MINIMUM_CANVAS_ZOOM);
		GameEvents.Mission.onMissionLoaded.Add(OnMissionLoaded);
	}

	public void OnMissionLoaded()
	{
		CalculateBorders();
		ZoomPercentage = zoom;
		CheckZoomBoundries();
		UpdateTextPercentage();
	}

	public void UpdateTextPercentage()
	{
		float time = (OrthographicSize - minZoomSize) / 10000f;
		float num = curveZoom.Evaluate(time);
		meLogic.zoom_level.text = (1f - num).ToString("P0");
	}

	public void FocusStartNode()
	{
		FocusNode(meLogic.EditorMission.startNode.guiNode);
	}

	public void FocusNode(MEGUINode node)
	{
		cameraCanvas.transform.localPosition = node.transform.localPosition;
		SetZoom(1f);
	}

	public void SetZoom(float newZoom)
	{
		newZoom = Mathf.Clamp(newZoom, zoomMin, zoomMax);
		newZoom = Mathf.Round(newZoom * 100f) / 100f;
		zoom = newZoom;
		ZoomPercentage = newZoom;
		CheckZoomBoundries();
		UpdateTextPercentage();
	}

	public void Zoom(float increment)
	{
		float num = zoom + increment;
		num = Mathf.Clamp(num, num, zoomMax);
		num = Mathf.Round(num * 100f) / 100f;
		if (num <= zoomMax && increment > 0f)
		{
			zoom = num;
		}
		else if (num >= zoomMin && increment < 0f)
		{
			zoom = num;
		}
		ZoomPercentage = zoom;
		CheckZoomBoundries();
		UpdateTextPercentage();
	}

	public void ZoomIn()
	{
		Zoom(0.1f);
	}

	public void ZoomOut()
	{
		Zoom(-0.1f);
	}

	public void CheckZoomBoundries()
	{
		meLogic.zoom_buttonMinus.interactable = zoom > zoomMin;
		meLogic.zoom_buttonPlus.interactable = zoom < zoomMax;
		zoomChanged();
	}

	public void zoomChanged()
	{
		for (int i = 0; i < MissionEditorLogic.Instance.editorConnectorList.Count; i++)
		{
			MissionEditorLogic.Instance.editorConnectorList[i].ResetLineWidth(zoom < 0.25f);
		}
	}

	public void FitCameraToSelectedNodes()
	{
		FitCameraToSelectedNodes(clampMin: true, clampMax: true);
	}

	public void FitCameraToSelectedNodes(bool clampMin, bool clampMax)
	{
		if (MissionEditorLogic.Instance.selectedNodes.Count >= 1)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			float num5 = float.MaxValue;
			float num6 = float.MinValue;
			for (int i = 0; i < MissionEditorLogic.Instance.selectedNodes.Count; i++)
			{
				MEGUINode mEGUINode = MissionEditorLogic.Instance.selectedNodes[i];
				num3 = Mathf.Min(num3, mEGUINode.transform.localPosition.x);
				num4 = Mathf.Max(num4, mEGUINode.transform.localPosition.x);
				num5 = Mathf.Min(num5, mEGUINode.transform.localPosition.y);
				num6 = Mathf.Max(num6, mEGUINode.transform.localPosition.y);
			}
			num = Mathf.Abs(num6 - num5);
			num2 = Mathf.Abs(num4 - num3);
			num += 400f;
			num2 += 400f;
			Vector3 localPosition = canvasRectTransform.transform.localPosition;
			localPosition.x = (num4 + num3) / 2f;
			localPosition.y = (num6 + num5) / 2f;
			cameraCanvas.transform.localPosition = localPosition;
			if (num2 < num)
			{
				OrthographicSize = num * 0.5f;
			}
			else
			{
				float num7 = (float)Screen.height / (float)Screen.width;
				OrthographicSize = num2 * 0.5f * num7;
			}
			AdjustOrthographicsSizeAndCameraPosition(clampMin, clampMax);
		}
	}

	public void FitCameraToArea()
	{
		FitCameraToArea(clampMin: true, clampMax: true);
	}

	public void FitCameraToArea(bool clampMin, bool clampMax)
	{
		if (canvasRectTransform.rect.width < canvasRectTransform.rect.height)
		{
			OrthographicSize = canvasRectTransform.rect.height * 0.5f;
		}
		else
		{
			float num = (float)Screen.height / (float)Screen.width;
			OrthographicSize = canvasRectTransform.rect.width * 0.5f * num;
		}
		cameraCanvas.transform.localPosition = canvasRectTransform.transform.localPosition;
		AdjustOrthographicsSizeAndCameraPosition(clampMin, clampMax);
	}

	public void AdjustOrthographicsSizeAndCameraPosition(bool clampMin = false, bool clampMax = false)
	{
		Vector3 vector = default(Vector3);
		float num = meLogic.NodeCanvasUIRect.sizeDelta.x * UIMasterController.Instance.uiScale / (float)Screen.width;
		if (num > 0f)
		{
			OrthographicSize *= 1f / num;
		}
		OrthographicSize /= UIMasterController.Instance.uiScale;
		if (clampMin && OrthographicSize < minZoomSize)
		{
			OrthographicSize = minZoomSize;
		}
		else if (clampMax && OrthographicSize > maxZoomSize)
		{
			OrthographicSize = maxZoomSize;
		}
		float num2 = (float)Screen.width / (float)Screen.height;
		float num3 = ((float)Screen.width / 2f - meLogic.NodeCanvasUIRect.anchoredPosition.x * UIMasterController.Instance.uiScale) / ((float)Screen.width / 2f) * num2;
		vector.x += num3 * OrthographicSize;
		zoom = 1f - (OrthographicSize - minZoomSize) / maxZoomSize;
		cameraCanvas.transform.localPosition += vector;
		UpdateTextPercentage();
		CheckZoomBoundries();
	}

	public Vector3 GetClampedPosition(Vector2 canvasPos)
	{
		Vector3 result = canvasPos;
		if (result.x > canvasRectTransform.offsetMax.x && result.x > cameraCanvas.transform.localPosition.x)
		{
			result.x = cameraCanvas.transform.localPosition.x;
		}
		else if (result.x < canvasRectTransform.offsetMin.x && result.x < cameraCanvas.transform.localPosition.x)
		{
			result.x = cameraCanvas.transform.localPosition.x;
		}
		if (result.y > canvasRectTransform.offsetMax.y && result.y > cameraCanvas.transform.localPosition.y)
		{
			result.y = cameraCanvas.transform.localPosition.y;
		}
		else if (result.y < canvasRectTransform.offsetMin.y && result.y < cameraCanvas.transform.localPosition.y)
		{
			result.y = cameraCanvas.transform.localPosition.y;
		}
		return result;
	}

	public void GetNodeBounds(MEGUINode guiNode, ref Vector2 maxBounds, ref Vector2 minBounds)
	{
		maxBounds.x = guiNode.transform.position.x + guiNode.rectTransform.sizeDelta.x / 2f;
		minBounds.x = guiNode.transform.position.x - guiNode.rectTransform.sizeDelta.x / 2f;
		maxBounds.y = guiNode.transform.position.y;
		minBounds.y = guiNode.transform.position.y - guiNode.rectTransform.sizeDelta.y;
	}

	public void CalculateBorders()
	{
		bordersMax = new Vector2(float.MinValue, float.MinValue);
		bordersMin = new Vector2(float.MaxValue, float.MaxValue);
		for (int i = 0; i < MissionEditorLogic.Instance.EditorMission.nodes.Count; i++)
		{
			Vector2 maxBounds = new Vector2(float.MinValue, float.MinValue);
			Vector2 minBounds = new Vector2(float.MaxValue, float.MaxValue);
			GetNodeBounds(MissionEditorLogic.Instance.EditorMission.nodes.At(i).guiNode, ref maxBounds, ref minBounds);
			if (maxBounds.x > bordersMax.x)
			{
				bordersMax.x = maxBounds.x;
			}
			if (minBounds.x < bordersMin.x)
			{
				bordersMin.x = minBounds.x;
			}
			if (maxBounds.y > bordersMax.y)
			{
				bordersMax.y = maxBounds.y;
			}
			if (minBounds.y < bordersMin.y)
			{
				bordersMin.y = minBounds.y;
			}
		}
		bordersMax += extraMargin;
		bordersMin -= extraMargin;
		canvasRectTransform.offsetMax = bordersMax;
		canvasRectTransform.offsetMin = bordersMin;
	}

	public void OnDestroy()
	{
		meLogic.zoom_buttonPlus.onClick.RemoveListener(ZoomIn);
		meLogic.zoom_buttonMinus.onClick.RemoveListener(ZoomOut);
		GameEvents.Mission.onMissionLoaded.Remove(OnMissionLoaded);
	}

	public void OnScroll(PointerEventData eventData)
	{
		if (!lockScroll)
		{
			float num = ZoomPercentage;
			float num2 = Math.Sign(eventData.scrollDelta.y);
			if (num2 != 0f)
			{
				Zoom(num2 * zoomSpeed * GameSettings.AXIS_MOUSEWHEEL.scale);
			}
			if (num != ZoomPercentage)
			{
				Vector2 vector = ScrollDir(eventData) * num2;
				Vector3 vector2 = cameraCanvas.transform.localPosition + (Vector3)vector;
				cameraCanvas.transform.localPosition = GetClampedPosition(vector2);
			}
		}
	}

	public Vector2 ScrollDir(PointerEventData eventData)
	{
		Vector2 vector = new Vector2(Screen.width, Screen.height) * 0.5f;
		Vector2 vector2 = eventData.position - vector;
		vector2.x /= Screen.height;
		vector2.y /= Screen.height;
		return vector2 * 400f;
	}

	public Vector2 GetMousePointOnGrid()
	{
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(NodeRoot, Input.mousePosition, UICamera, out var localPoint))
		{
			return localPoint;
		}
		return Vector2.zero;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			meLogic.CanvasClicked(GetMousePointOnGrid(), eventData.dragging);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			startDragPos = eventData.position;
			startCamPos = cameraCanvas.transform.localPosition;
			ToggleScrollLock(newValue: true);
		}
	}

	public Vector3 GetCanvasDragPosition(Vector2 startDragPos, Vector2 currenDragPos, Vector2 startCamPos)
	{
		Vector2 vector = (startDragPos - currenDragPos) / Screen.height;
		float num = (maxZoomSize * (1f - zoom) + minZoomSize) * 2f;
		Vector3 vector2 = startCamPos + vector * num;
		return GetClampedPosition(vector2);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			cameraCanvas.transform.localPosition = GetCanvasDragPosition(startDragPos, eventData.position, startCamPos);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			cameraCanvas.transform.localPosition = GetCanvasDragPosition(startDragPos, eventData.position, startCamPos);
			ToggleScrollLock(newValue: false);
		}
	}

	public static Vector3 CheckSnap(Vector3 position)
	{
		if (Instance != null && Instance.snapToGrid)
		{
			float num = 0.04f;
			position.x = Mathf.Round(position.x * num) / num;
			position.y = Mathf.Round(position.y * num) / num;
		}
		return position;
	}

	public void TryCanvasMovement(PointerEventData pointerData, MEGUINode draggedNode)
	{
		nodeDragActive = false;
		Vector3 vector = pointerData.position;
		nodeDragDir = Vector2.zero;
		nodeDragRef = null;
		if (vector.x > meLogic.NodeCanvasUIRect.offsetMax.x * UIMasterController.Instance.uiScale - nodeDragBorder.x || vector.x < meLogic.NodeCanvasUIRect.offsetMin.x + nodeDragBorder.x * UIMasterController.Instance.uiScale)
		{
			nodeDragActive = true;
			nodeDragDir.x = (pointerData.position.x - meLogic.NodeCanvasUIRect.anchoredPosition.x * UIMasterController.Instance.uiScale) / (float)Screen.width;
			nodeDragRef = draggedNode;
		}
		if (vector.y > (0f - meLogic.NodeCanvasUIRect.offsetMin.y) * UIMasterController.Instance.uiScale - nodeDragBorder.y || vector.y < nodeDragBorder.y - meLogic.NodeCanvasUIRect.offsetMax.y * UIMasterController.Instance.uiScale)
		{
			nodeDragActive = true;
			nodeDragDir.y = (pointerData.position.y + meLogic.NodeCanvasUIRect.anchoredPosition.y * UIMasterController.Instance.uiScale) / (float)Screen.height;
			nodeDragRef = draggedNode;
		}
		nodeDragDir.Normalize();
	}

	public bool PointerInsideCanvasView(PointerEventData pointerData)
	{
		bool result = true;
		Vector3 vector = pointerData.position;
		if (vector.x > Instance.meLogic.NodeCanvasUIRect.offsetMax.x * UIMasterController.Instance.uiScale - nodeDragBorder.x || vector.x < Instance.meLogic.NodeCanvasUIRect.offsetMin.x * UIMasterController.Instance.uiScale + nodeDragBorder.x)
		{
			result = false;
		}
		if (vector.y > (0f - Instance.meLogic.NodeCanvasUIRect.offsetMin.y) * UIMasterController.Instance.uiScale - nodeDragBorder.y || vector.y < nodeDragBorder.y - Instance.meLogic.NodeCanvasUIRect.offsetMax.y * UIMasterController.Instance.uiScale)
		{
			result = false;
		}
		return result;
	}

	public void StopCanvasMovement()
	{
		nodeDragActive = false;
		nodeDragDir = Vector2.zero;
	}

	public void ToggleScrollLock(bool newValue)
	{
		lockScroll = newValue;
	}

	public void Load(ConfigNode node)
	{
		ConfigNode node2 = null;
		Vector3 value = Vector3.zero;
		if (node.TryGetNode("EDITOR_STATUS", ref node2))
		{
			node2.TryGetValue("canvasPosition", ref value);
			node2.TryGetValue("zoomLevel", ref zoom);
			cameraCanvas.transform.localPosition = value;
		}
	}

	public void Save(ConfigNode node)
	{
		ConfigNode configNode = node.AddNode("EDITOR_STATUS");
		configNode.AddValue("canvasPosition", cameraCanvas.transform.localPosition);
		configNode.AddValue("zoomLevel", zoom);
	}
}
