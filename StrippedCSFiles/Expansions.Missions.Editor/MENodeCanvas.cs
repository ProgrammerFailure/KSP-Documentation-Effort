using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class MENodeCanvas : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	public static MENodeCanvas Instance;

	[SerializeField]
	private RectTransform canvasRectTransform;

	[SerializeField]
	private RectTransform nodeRoot;

	[SerializeField]
	private Camera cameraCanvas;

	private Camera nodeCameraCanvas;

	[SerializeField]
	private AnimationCurve curveZoom;

	internal float zoomPercentage;

	internal float zoomDisplayPercentage;

	internal float minZoomSize;

	internal float maxZoomSize;

	private MissionEditorLogic meLogic;

	private Vector2 startCamPos;

	private Vector2 startDragPos;

	private Vector2 bordersMax;

	private Vector2 bordersMin;

	private Vector2 extraMargin;

	private Vector2 nodeDragBorder;

	private Vector2 nodeDragDir;

	private float nodeDragSpeed;

	private float zoomSpeed;

	private float zoomMin;

	private float zoomMax;

	private float zoom;

	private bool snapToGrid;

	private bool lockScroll;

	private bool nodeDragActive;

	private MEGUINode nodeDragRef;

	private const float maxCameraSize = 10000f;

	public Camera UICamera
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public RectTransform NodeRoot
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool SnapToGrid
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float ZoomPercentage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	private float OrthographicSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENodeCanvas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(MissionEditorLogic meLogicRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMissionLoaded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTextPercentage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FocusStartNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FocusNode(MEGUINode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetZoom(float newZoom)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Zoom(float increment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ZoomIn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ZoomOut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CheckZoomBoundries()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void zoomChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FitCameraToSelectedNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FitCameraToSelectedNodes(bool clampMin, bool clampMax)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FitCameraToArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FitCameraToArea(bool clampMin, bool clampMax)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AdjustOrthographicsSizeAndCameraPosition(bool clampMin = false, bool clampMax = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetClampedPosition(Vector2 canvasPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetNodeBounds(MEGUINode guiNode, ref Vector2 maxBounds, ref Vector2 minBounds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateBorders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnScroll(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 ScrollDir(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetMousePointOnGrid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerDown(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetCanvasDragPosition(Vector2 startDragPos, Vector2 currenDragPos, Vector2 startCamPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerUp(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 CheckSnap(Vector3 position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TryCanvasMovement(PointerEventData pointerData, MEGUINode draggedNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PointerInsideCanvasView(PointerEventData pointerData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StopCanvasMovement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleScrollLock(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
