using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using Vectrosity;

namespace Expansions.Serenity;

public class CurvePanel : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public class CurvePanelPointChangeInfo
	{
		public CurvePanelPoint point;

		public int index;

		public float time;

		public float value;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CurvePanelPointChangeInfo(CurvePanelPoint point, int index, float time, float value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CurvePanelPointChangeInfo()
		{
			throw null;
		}
	}

	[SerializeField]
	private RectTransform pointsParent;

	[SerializeField]
	private RectTransform lineParent;

	[SerializeField]
	private Transform xZeroAxis;

	[SerializeField]
	private bool showXZeroAxis;

	[SerializeField]
	internal bool noValues;

	[SerializeField]
	internal bool noEndPoints;

	[SerializeField]
	private RectTransform positionsParent;

	[SerializeField]
	private RectTransform minLimitObject;

	[SerializeField]
	private RectTransform maxLimitObject;

	[SerializeField]
	private CurvePanelPoint pointPrefab;

	private List<CurvePanelPoint> curvePoints;

	public int intersegmentCount;

	public Color lineColor;

	[SerializeField]
	private VectorObject2D lineObject;

	private VectorLine line;

	private string objectName;

	public int insertionDetectTolerance;

	public Callback<List<CurvePanelPoint>> OnPointDragging;

	public Callback OnPanelLeftClick;

	public Callback<int> OnPointDeleted;

	public Callback<CurvePanelPointChangeInfo> OnPointMoved;

	public Callback<CurvePanelPointChangeInfo> OnPointAdded;

	private CurvePanelPointChangeInfo tempChangeInfo;

	public Callback<CurvePanel, List<CurvePanelPoint>> OnPointSelectionChanged;

	private float prevHeight;

	private float prevWidth;

	private Vector2 xAxisPosition;

	private List<Vector2> linePoints;

	private Vector2 tempLineVector;

	private float pointsInterSegmentLength;

	private float interSegmentsForThisPoint;

	private float workingX;

	private float workingY;

	public bool draggingPoint;

	public float clampSpace;

	private Vector3 tempDragVector;

	private Keyframe kWorking;

	private CurvePanelPoint shiftStartPoint;

	private int shiftStartIndex;

	private Vector3 minLimitPosition;

	private Vector3 maxLimitPosition;

	private int tempIndex;

	private Vector2 doubleClickPoint;

	public List<CurvePanelPoint> SelectedPoints
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

	public int SelectedPointsCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<int> SelectedPointIndexes
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

	public FloatCurve curve
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

	public bool Editable
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

	public bool PointsVisible
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

	public bool MinLimitVisible
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

	public bool MaxLimitVisible
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

	public bool ClampValuesWithLimits
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

	public float YAxisMin
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

	public float YAxisMax
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

	public float YAxisMinLimit
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

	public float YAxisMaxLimit
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

	internal float PanelRectWidth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal float PanelRectHeight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal float PanelTimeDuration
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal float PanelValueRange
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal float PanelRectRatio
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal float PanelValueRatio
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurvePanel()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(FloatCurve curve, string objectName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(FloatCurve curve, string objectName, float axisMin, float axisMax)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetCurve(FloatCurve curve)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawAll(bool newline = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetBackgroundElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawPoints(bool newline = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawLine(bool newline = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal float PanelXPosFromTime(float timePos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal float PanelYPosFromValue(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal float CurveTimeFromXPos(float xPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal float CurveValueFromYPos(float yPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetEditable(bool editable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowPoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HidePoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InsertPoint(int linePointIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InsertPoint(float timeValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InsertPointAtPosition(float curveTime, float curveValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeletePoint(CurvePanelPoint p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeletePoint(int pointIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeleteSelectedPoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPoint(CurvePanelPoint p, bool hideDelete = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearSelectedPoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPoint(int pointIndex, bool hideDelete = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPoint(float time, bool hideDelete = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectAllPoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void BeginDragPoint(CurvePanelPoint p, PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void EndDragPoint(CurvePanelPoint p, PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnDragPoint(CurvePanelPoint p, PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnTangentDrag(CurvePanelPoint p, CurvePanelPoint.TangentTypes tangentType, float tanValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetTangentSharp(CurvePanelPoint p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetTangentSmooth(CurvePanelPoint p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void MovePoint(CurvePanelPoint p, float newTime, float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int GetIndexOfPoint(CurvePanelPoint p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int GetCurvePointsCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void MousePointClick(CurvePanelPoint p, bool withShift, bool withCtrl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RebuildSelectedPointIndexes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowMinLimit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowMaxLimit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideMinLimit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideMaxLimit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLimitClamping()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveLimitClamping()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLimits(Vector2 axisMinMax)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLimits(Vector2 axisMinMan, Vector2 limitsMinMax)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}
}
