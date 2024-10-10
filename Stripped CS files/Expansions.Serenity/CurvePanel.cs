using System.Collections.Generic;
using ns2;
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

		public CurvePanelPointChangeInfo(CurvePanelPoint point, int index, float time, float value)
			: this()
		{
			this.point = point;
			this.index = index;
			this.time = time;
			this.value = value;
		}

		public CurvePanelPointChangeInfo()
		{
		}
	}

	[SerializeField]
	public RectTransform pointsParent;

	[SerializeField]
	public RectTransform lineParent;

	[SerializeField]
	public Transform xZeroAxis;

	[SerializeField]
	public bool showXZeroAxis;

	[SerializeField]
	public bool noValues;

	[SerializeField]
	public bool noEndPoints;

	[SerializeField]
	public RectTransform positionsParent;

	[SerializeField]
	public RectTransform minLimitObject;

	[SerializeField]
	public RectTransform maxLimitObject;

	[SerializeField]
	public CurvePanelPoint pointPrefab;

	public List<CurvePanelPoint> curvePoints;

	public int intersegmentCount = 20;

	public Color lineColor = Color.blue;

	[SerializeField]
	public VectorObject2D lineObject;

	public VectorLine line;

	public string objectName = "Line";

	public int insertionDetectTolerance = 5;

	public Callback<List<CurvePanelPoint>> OnPointDragging;

	public Callback OnPanelLeftClick;

	public Callback<int> OnPointDeleted;

	public Callback<CurvePanelPointChangeInfo> OnPointMoved;

	public Callback<CurvePanelPointChangeInfo> OnPointAdded;

	public CurvePanelPointChangeInfo tempChangeInfo;

	public Callback<CurvePanel, List<CurvePanelPoint>> OnPointSelectionChanged;

	public float prevHeight;

	public float prevWidth;

	public Vector2 xAxisPosition;

	public List<Vector2> linePoints;

	public Vector2 tempLineVector;

	public float pointsInterSegmentLength;

	public float interSegmentsForThisPoint;

	public float workingX;

	public float workingY;

	public bool draggingPoint;

	public float clampSpace = 0.01f;

	public Vector3 tempDragVector;

	public Keyframe kWorking;

	public CurvePanelPoint shiftStartPoint;

	public int shiftStartIndex = -1;

	public Vector3 minLimitPosition;

	public Vector3 maxLimitPosition;

	public int tempIndex;

	public Vector2 doubleClickPoint;

	public List<CurvePanelPoint> SelectedPoints { get; set; }

	public int SelectedPointsCount => SelectedPoints.Count;

	public List<int> SelectedPointIndexes { get; set; }

	public FloatCurve curve { get; set; }

	public bool Editable { get; set; }

	public bool PointsVisible { get; set; }

	public bool MinLimitVisible { get; set; }

	public bool MaxLimitVisible { get; set; }

	public bool ClampValuesWithLimits { get; set; }

	public float YAxisMin { get; set; }

	public float YAxisMax { get; set; }

	public float YAxisMinLimit { get; set; }

	public float YAxisMaxLimit { get; set; }

	public float PanelRectWidth => positionsParent.rect.width;

	public float PanelRectHeight => positionsParent.rect.height;

	public float PanelTimeDuration
	{
		get
		{
			if (!noEndPoints)
			{
				return curve.maxTime;
			}
			return 1f;
		}
	}

	public float PanelValueRange => YAxisMax - YAxisMin;

	public float PanelRectRatio => PanelRectHeight / PanelRectWidth;

	public float PanelValueRatio => PanelValueRange / PanelTimeDuration;

	public void Awake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (lineObject == null)
		{
			Debug.LogError("[CurvePanel]: No LineObject2D assigned, destroying");
			Object.Destroy(base.gameObject);
			return;
		}
		SelectedPoints = new List<CurvePanelPoint>();
		SelectedPointIndexes = new List<int>();
		tempChangeInfo = new CurvePanelPointChangeInfo();
		minLimitPosition = minLimitObject.localPosition;
		maxLimitPosition = maxLimitObject.localPosition;
		YAxisMin = 0f;
		YAxisMax = 100f;
		YAxisMinLimit = YAxisMin;
		YAxisMaxLimit = YAxisMax;
		ClampValuesWithLimits = false;
		HideLimits();
		PointsVisible = true;
		Editable = false;
		line = lineObject.vectorLine;
	}

	public void Start()
	{
		if (curve != null)
		{
			DrawAll();
		}
	}

	public void OnDestroy()
	{
		if (line != null)
		{
			VectorLine.Destroy(ref line);
		}
		if (lineObject != null)
		{
			lineObject.Destroy();
			Object.Destroy(lineObject.gameObject);
		}
	}

	public void Update()
	{
		if (PanelRectWidth != prevWidth || PanelRectWidth != prevWidth)
		{
			DrawAll(newline: false);
		}
		prevWidth = PanelRectWidth;
		prevHeight = PanelRectHeight;
	}

	public void Setup(FloatCurve curve, string objectName)
	{
		this.curve = curve;
		this.objectName = objectName;
		xZeroAxis.gameObject.SetActive(showXZeroAxis);
		xAxisPosition = xZeroAxis.localPosition;
	}

	public void Setup(FloatCurve curve, string objectName, float axisMin, float axisMax)
	{
		Setup(curve, objectName);
		YAxisMin = axisMin;
		YAxisMax = axisMax;
	}

	public void SetCurve(FloatCurve curve)
	{
		this.curve = curve;
	}

	public void DrawAll(bool newline = true)
	{
		SetBackgroundElements();
		DrawPoints(newline);
		DrawLine(newline);
		if (newline)
		{
			ClearSelectedPoints();
		}
		UpdateLimits();
	}

	public void SetBackgroundElements()
	{
		xAxisPosition.y = PanelYPosFromValue(0f);
		xZeroAxis.localPosition = xAxisPosition;
	}

	public void DrawPoints(bool newline = true)
	{
		if (curve == null)
		{
			Debug.LogError("[CurvePanel]: Unable to Draw points, No AnimationCurve assigned");
			return;
		}
		if (curvePoints == null)
		{
			newline = true;
			curvePoints = new List<CurvePanelPoint>();
		}
		if (newline)
		{
			int count = curvePoints.Count;
			while (count-- > 0)
			{
				Object.Destroy(curvePoints[count].gameObject);
				curvePoints.RemoveAt(count);
			}
			for (int i = 0; i < curve.Curve.keys.Length; i++)
			{
				Keyframe keyframe = curve.Curve.keys[i];
				CurvePanelPoint curvePanelPoint = Object.Instantiate(pointPrefab);
				curvePanelPoint.name = "Point " + i;
				curvePanelPoint.transform.SetParent(pointsParent);
				curvePanelPoint.transform.localPosition = new Vector3(PanelXPosFromTime(keyframe.time), PanelYPosFromValue(keyframe.value), 0f);
				curvePanelPoint.transform.localScale = Vector3.one;
				curvePanelPoint.Setup(this, keyframe);
				if ((noEndPoints && keyframe.time < 0f) || keyframe.time > 1f)
				{
					curvePanelPoint.unselectable = true;
				}
				curvePanelPoint.gameObject.SetActive(Editable || PointsVisible);
				curvePoints.Add(curvePanelPoint);
			}
		}
		else
		{
			for (int j = 0; j < curvePoints.Count; j++)
			{
				curvePoints[j].transform.localPosition = new Vector3(PanelXPosFromTime(curvePoints[j].Keyframe.time), PanelYPosFromValue(curvePoints[j].Keyframe.value), 0f);
			}
		}
	}

	public void DrawLine(bool newline = true)
	{
		if (curve == null)
		{
			Debug.LogError("[CurvePanel]: Unable to Draw line, No AnimationCurve assigned");
			return;
		}
		if (linePoints == null || newline)
		{
			newline = true;
			linePoints = line.points2;
			linePoints.Clear();
		}
		for (int i = 0; i < curvePoints.Count; i++)
		{
			if (i < curvePoints.Count - 1)
			{
				pointsInterSegmentLength = (curvePoints[i + 1].Keyframe.time - curvePoints[i].Keyframe.time) / (float)intersegmentCount;
				interSegmentsForThisPoint = intersegmentCount;
			}
			else
			{
				interSegmentsForThisPoint = 1f;
			}
			for (int j = 0; (float)j < interSegmentsForThisPoint; j++)
			{
				float num = curvePoints[i].Keyframe.time + pointsInterSegmentLength * (float)j;
				float value = curve.Evaluate(num);
				if (newline)
				{
					linePoints.Add(new Vector2(PanelXPosFromTime(num), PanelYPosFromValue(value)));
					continue;
				}
				tempLineVector = linePoints[i * intersegmentCount + j];
				tempLineVector.x = PanelXPosFromTime(num);
				tempLineVector.y = PanelYPosFromValue(value);
				linePoints[i * intersegmentCount + j] = tempLineVector;
			}
		}
		line.color = lineColor;
		line.Draw();
		lineObject.UpdateTris();
	}

	public float PanelXPosFromTime(float timePos)
	{
		workingX = timePos / PanelTimeDuration;
		workingX *= positionsParent.rect.width;
		return workingX;
	}

	public float PanelYPosFromValue(float value)
	{
		workingY = (value - YAxisMin) / (YAxisMax - YAxisMin);
		workingY *= positionsParent.rect.height;
		return workingY;
	}

	public float CurveTimeFromXPos(float xPos)
	{
		workingX = xPos / positionsParent.rect.width;
		workingX *= PanelTimeDuration;
		return workingX;
	}

	public float CurveValueFromYPos(float yPos)
	{
		workingY = yPos / positionsParent.rect.height;
		workingY = workingY * (YAxisMax - YAxisMin) + YAxisMin;
		return workingY;
	}

	public void SetEditable(bool editable)
	{
		if (Editable != editable)
		{
			Editable = editable;
			if (Editable)
			{
				ShowPoints();
			}
			else if (!PointsVisible)
			{
				HidePoints();
			}
		}
	}

	public void ShowPoints()
	{
		for (int i = 0; i < curvePoints.Count; i++)
		{
			curvePoints[i].gameObject.SetActive(value: true);
		}
		PointsVisible = true;
	}

	public void HidePoints()
	{
		ClearSelectedPoints();
		for (int i = 0; i < curvePoints.Count; i++)
		{
			curvePoints[i].gameObject.SetActive(value: false);
		}
		Editable = false;
		PointsVisible = false;
	}

	public void InsertPoint(int linePointIndex)
	{
		float curveTime = CurveTimeFromXPos(linePoints[linePointIndex].x);
		float curveValue = CurveValueFromYPos(linePoints[linePointIndex].y);
		InsertPointAtPosition(curveTime, curveValue);
	}

	public void InsertPoint(float timeValue)
	{
		float curveValue = curve.Evaluate(timeValue);
		InsertPointAtPosition(timeValue, curveValue);
	}

	public void InsertPointAtPosition(float curveTime, float curveValue)
	{
		int num4;
		if (curveTime > 0.01f && curveTime < PanelTimeDuration - 0.01f)
		{
			Keyframe key = new Keyframe(curveTime, curveValue);
			float num = curve.Evaluate(curveTime - 0.01f);
			float num2 = (curve.Evaluate(curveTime + 0.01f) - num) / 0.02f;
			float inTangent = (key.outTangent = num2);
			key.inTangent = inTangent;
			num4 = curve.Curve.AddKey(key);
		}
		else
		{
			num4 = curve.Curve.AddKey(curveTime, curveValue);
		}
		if (OnPointAdded != null)
		{
			tempChangeInfo.point = null;
			tempChangeInfo.index = num4;
			tempChangeInfo.time = curveTime;
			tempChangeInfo.value = curveValue;
			OnPointAdded(tempChangeInfo);
		}
		DrawAll();
		if (num4 > -1)
		{
			ClearSelectedPoints();
			SelectPoint(curvePoints[num4]);
		}
	}

	public void DeletePoint(CurvePanelPoint p)
	{
		int num = curvePoints.IndexOf(p);
		if (num > -1)
		{
			DeletePoint(num);
		}
	}

	public void DeletePoint(int pointIndex)
	{
		if (noEndPoints || (pointIndex >= 1 && pointIndex < linePoints.Count - 1))
		{
			curve.Curve.RemoveKey(pointIndex);
			if (OnPointDeleted != null)
			{
				OnPointDeleted(pointIndex);
			}
			DrawAll();
		}
	}

	public void DeleteSelectedPoints()
	{
		bool flag = false;
		int count = SelectedPointIndexes.Count;
		while (count-- > 0)
		{
			int num = SelectedPointIndexes[count];
			if (num >= 1 && num < curvePoints.Count - 1)
			{
				curve.Curve.RemoveKey(num);
				flag = true;
				if (OnPointDeleted != null)
				{
					OnPointDeleted(num);
				}
			}
		}
		if (flag)
		{
			DrawAll();
		}
	}

	public void SelectPoint(CurvePanelPoint p, bool hideDelete = false)
	{
		if (!p.unselectable)
		{
			int indexOfPoint = GetIndexOfPoint(p);
			bool num = !p.Selected;
			SelectedPoints.AddUnique(p);
			SelectedPointIndexes.AddUnique(indexOfPoint);
			p.Select(hideDelete);
			if (num && OnPointSelectionChanged != null)
			{
				OnPointSelectionChanged(this, SelectedPoints);
			}
		}
	}

	public void ClearSelectedPoints()
	{
		bool flag = SelectedPointsCount > 0;
		SelectedPoints.Clear();
		SelectedPointIndexes.Clear();
		for (int i = 0; i < curvePoints.Count; i++)
		{
			curvePoints[i].Deselect();
		}
		if (flag && OnPointSelectionChanged != null)
		{
			OnPointSelectionChanged(this, SelectedPoints);
		}
	}

	public void SelectPoint(int pointIndex, bool hideDelete = false)
	{
		if (pointIndex >= 0 && pointIndex <= curvePoints.Count - 1)
		{
			SelectPoint(curvePoints[pointIndex], hideDelete);
		}
	}

	public void SelectPoint(float time, bool hideDelete = false)
	{
		int num = 0;
		while (true)
		{
			if (num < curvePoints.Count)
			{
				if (curvePoints[num].Keyframe.time == time)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		ClearSelectedPoints();
		SelectPoint(curvePoints[num], hideDelete);
	}

	public void SelectAllPoints()
	{
		bool flag = SelectedPointsCount < curvePoints.Count;
		for (int i = 0; i < curvePoints.Count; i++)
		{
			SelectPoint(i);
		}
		RebuildSelectedPointIndexes();
		if (flag && OnPointSelectionChanged != null)
		{
			OnPointSelectionChanged(this, SelectedPoints);
		}
	}

	public void BeginDragPoint(CurvePanelPoint p, PointerEventData eventData)
	{
		if (!SelectedPoints.Contains(p))
		{
			ClearSelectedPoints();
			SelectPoint(p, hideDelete: true);
		}
		else
		{
			for (int i = 0; i < SelectedPoints.Count; i++)
			{
				SelectedPoints[i].HideDelete();
			}
		}
		draggingPoint = true;
	}

	public void EndDragPoint(CurvePanelPoint p, PointerEventData eventData)
	{
		draggingPoint = false;
		for (int i = 0; i < SelectedPointIndexes.Count; i++)
		{
			if (noEndPoints || (SelectedPointIndexes[i] > 0 && SelectedPointIndexes[i] < curvePoints.Count - 1))
			{
				curvePoints[SelectedPointIndexes[i]].ShowDelete();
			}
		}
	}

	public void OnDragPoint(CurvePanelPoint p, PointerEventData eventData)
	{
		float num = 0f;
		for (int i = 0; i < SelectedPoints.Count; i++)
		{
			num = ((!noValues) ? eventData.delta.y : 0f);
			MovePoint(SelectedPoints[i], CurveTimeFromXPos(SelectedPoints[i].transform.localPosition.x + eventData.delta.x), CurveValueFromYPos(SelectedPoints[i].transform.localPosition.y + num));
		}
		if (OnPointDragging != null)
		{
			OnPointDragging(SelectedPoints);
		}
	}

	public void OnTangentDrag(CurvePanelPoint p, CurvePanelPoint.TangentTypes tangentType, float tanValue)
	{
		int index = curvePoints.IndexOf(p);
		float tangentValue = tanValue * PanelValueRatio / PanelRectRatio;
		p.SetKeyFrameTangent(tangentType, tangentValue);
		curve.Curve.MoveKey(index, curvePoints[index].Keyframe);
		DrawLine(newline: false);
	}

	public void SetTangentSharp(CurvePanelPoint p)
	{
		int num = curvePoints.IndexOf(p);
		kWorking = curvePoints[num].Keyframe;
		if (num > 0)
		{
			kWorking.inTangent = (curve.Curve.keys[num].value - curve.Curve.keys[num - 1].value) / (curve.Curve.keys[num].time - curve.Curve.keys[num - 1].time);
		}
		if (num < curvePoints.Count - 1)
		{
			kWorking.outTangent = (curve.Curve.keys[num + 1].value - curve.Curve.keys[num].value) / (curve.Curve.keys[num + 1].time - curve.Curve.keys[num].time);
		}
		curvePoints[num].SetKeyFrameTangents(kWorking.inTangent, kWorking.outTangent);
		curve.Curve.MoveKey(num, curvePoints[num].Keyframe);
		if (num > 0)
		{
			kWorking = curvePoints[num - 1].Keyframe;
			kWorking.outTangent = (curve.Curve.keys[num].value - curve.Curve.keys[num - 1].value) / (curve.Curve.keys[num].time - curve.Curve.keys[num - 1].time);
			curvePoints[num - 1].SetKeyFrameTangents(kWorking.inTangent, kWorking.outTangent);
			curve.Curve.MoveKey(num - 1, curvePoints[num - 1].Keyframe);
		}
		if (num < curvePoints.Count - 1)
		{
			kWorking = curvePoints[num + 1].Keyframe;
			kWorking.inTangent = (curve.Curve.keys[num + 1].value - curve.Curve.keys[num].value) / (curve.Curve.keys[num + 1].time - curve.Curve.keys[num].time);
			curvePoints[num + 1].SetKeyFrameTangents(kWorking.inTangent, kWorking.outTangent);
			curve.Curve.MoveKey(num + 1, curvePoints[num + 1].Keyframe);
		}
		DrawLine(newline: false);
		p.SetTangentHandles();
	}

	public void SetTangentSmooth(CurvePanelPoint p)
	{
		int num = curvePoints.IndexOf(p);
		kWorking = curvePoints[num].Keyframe;
		if (num > 0 && num < curvePoints.Count - 1)
		{
			ref Keyframe reference = ref kWorking;
			float inTangent = (kWorking.outTangent = (curve.Curve.keys[num + 1].value - curve.Curve.keys[num - 1].value) / (curve.Curve.keys[num + 1].time - curve.Curve.keys[num - 1].time));
			reference.inTangent = inTangent;
			curvePoints[num].SetKeyFrameTangents(kWorking.inTangent, kWorking.outTangent);
			curve.Curve.MoveKey(num, curvePoints[num].Keyframe);
		}
		DrawLine(newline: false);
		p.SetTangentHandles();
	}

	public void MovePoint(CurvePanelPoint p, float newTime, float newValue)
	{
		if (noValues)
		{
			newValue = p.Keyframe.value;
		}
		int num = curvePoints.IndexOf(p);
		float num2 = 0f;
		float num3 = PanelTimeDuration;
		if (noEndPoints)
		{
			if (num > 0)
			{
				num2 = curvePoints[num - 1].Keyframe.time + clampSpace;
			}
			if (num < curvePoints.Count - 1)
			{
				num3 = curvePoints[num + 1].Keyframe.time - clampSpace;
			}
		}
		else if (num < 1)
		{
			num3 = 0f;
		}
		else if (num >= curvePoints.Count - 1)
		{
			num2 = PanelTimeDuration;
		}
		else
		{
			num2 = curvePoints[num - 1].Keyframe.time + clampSpace;
			num3 = curvePoints[num + 1].Keyframe.time - clampSpace;
		}
		if (noEndPoints)
		{
			num2 = Mathf.Max(num2, 0f);
			num3 = Mathf.Min(num3, PanelTimeDuration);
		}
		newTime = Mathf.Clamp(newTime, num2, num3);
		newValue = ((!ClampValuesWithLimits) ? Mathf.Clamp(newValue, YAxisMin, YAxisMax) : Mathf.Clamp(newValue, YAxisMinLimit, YAxisMaxLimit));
		p.SetKeyFrame(newTime, newValue);
		tempDragVector.x = PanelXPosFromTime(newTime);
		tempDragVector.y = PanelYPosFromValue(newValue);
		p.transform.localPosition = tempDragVector;
		curve.Curve.MoveKey(num, curvePoints[num].Keyframe);
		if (OnPointMoved != null)
		{
			tempChangeInfo.point = p;
			tempChangeInfo.index = num;
			tempChangeInfo.time = newTime;
			tempChangeInfo.value = newValue;
			OnPointMoved(tempChangeInfo);
		}
		DrawLine(newline: false);
	}

	public int GetIndexOfPoint(CurvePanelPoint p)
	{
		int num = 0;
		while (true)
		{
			if (num < curvePoints.Count)
			{
				if (curvePoints[num].Equals(p))
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public int GetCurvePointsCount()
	{
		if (curvePoints != null)
		{
			return curvePoints.Count;
		}
		return -1;
	}

	public void MousePointClick(CurvePanelPoint p, bool withShift, bool withCtrl)
	{
		if (draggingPoint)
		{
			return;
		}
		bool flag = false;
		int indexOfPoint = GetIndexOfPoint(p);
		if (withShift)
		{
			if (SelectedPointsCount == 1 && p.Selected)
			{
				p.Deselect();
				SelectedPoints.Remove(p);
				flag = true;
			}
			else
			{
				for (int i = 0; i < curvePoints.Count; i++)
				{
					if ((shiftStartIndex <= i && i <= indexOfPoint) || (indexOfPoint <= i && i <= shiftStartIndex))
					{
						if (!curvePoints[i].unselectable)
						{
							if (!curvePoints[i].Selected)
							{
								flag = true;
							}
							curvePoints[i].Select();
							SelectedPoints.AddUnique(curvePoints[i]);
						}
					}
					else
					{
						if (curvePoints[i].Selected)
						{
							flag = true;
						}
						curvePoints[i].Deselect();
						SelectedPoints.Remove(curvePoints[i]);
					}
				}
			}
		}
		else if (withCtrl)
		{
			if (p.Selected)
			{
				p.Deselect();
				SelectedPoints.Remove(p);
				flag = true;
			}
			else if (!p.unselectable)
			{
				p.Select();
				SelectedPoints.AddUnique(p);
				flag = true;
			}
		}
		else if (SelectedPointsCount != 1 || !(p == SelectedPoints[0]))
		{
			flag = true;
			ClearSelectedPoints();
			shiftStartPoint = p;
			shiftStartIndex = GetIndexOfPoint(p);
			SelectPoint(p);
			SelectedPoints.AddUnique(p);
		}
		RebuildSelectedPointIndexes();
		if (flag && OnPointSelectionChanged != null)
		{
			OnPointSelectionChanged(this, SelectedPoints);
		}
	}

	public void RebuildSelectedPointIndexes()
	{
		SelectedPointIndexes.Clear();
		for (int i = 0; i < curvePoints.Count; i++)
		{
			if (curvePoints[i].Selected)
			{
				SelectedPointIndexes.AddUnique(i);
			}
		}
	}

	public void ShowLimits()
	{
		ShowMinLimit();
		ShowMaxLimit();
	}

	public void ShowMinLimit()
	{
		MinLimitVisible = true;
		minLimitObject.gameObject.SetActive(YAxisMinLimit > YAxisMin);
	}

	public void ShowMaxLimit()
	{
		MaxLimitVisible = true;
		maxLimitObject.gameObject.SetActive(YAxisMaxLimit < YAxisMax);
	}

	public void HideLimits()
	{
		HideMinLimit();
		HideMaxLimit();
	}

	public void HideMinLimit()
	{
		MinLimitVisible = false;
		minLimitObject.gameObject.SetActive(value: false);
	}

	public void HideMaxLimit()
	{
		MinLimitVisible = false;
		maxLimitObject.gameObject.SetActive(value: false);
	}

	public void SetLimitClamping()
	{
		ClampValuesWithLimits = true;
	}

	public void RemoveLimitClamping()
	{
		ClampValuesWithLimits = false;
	}

	public void SetLimits(Vector2 axisMinMax)
	{
		YAxisMin = axisMinMax.x;
		YAxisMax = axisMinMax.y;
	}

	public void SetLimits(Vector2 axisMinMan, Vector2 limitsMinMax)
	{
		SetLimits(axisMinMan);
		YAxisMinLimit = limitsMinMax.x;
		YAxisMaxLimit = limitsMinMax.y;
		UpdateLimits();
	}

	public void UpdateLimits()
	{
		if (MinLimitVisible)
		{
			minLimitPosition = minLimitObject.offsetMax;
			minLimitPosition.y = PanelYPosFromValue(YAxisMinLimit) + 2f;
			minLimitObject.offsetMax = minLimitPosition;
			minLimitObject.gameObject.SetActive(YAxisMinLimit > YAxisMin);
		}
		if (MaxLimitVisible)
		{
			maxLimitPosition = maxLimitObject.offsetMin;
			maxLimitPosition.y = PanelYPosFromValue(YAxisMaxLimit);
			maxLimitObject.offsetMin = maxLimitPosition;
			maxLimitObject.gameObject.SetActive(YAxisMaxLimit < YAxisMax);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != 0)
		{
			return;
		}
		if (Editable && eventData.clickCount == 2)
		{
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(lineParent, eventData.position, UIMainCamera.Camera, out doubleClickPoint))
			{
				line.Selected(doubleClickPoint);
				if (line.Selected(doubleClickPoint, insertionDetectTolerance, out tempIndex))
				{
					InsertPoint(tempIndex);
				}
			}
		}
		else if (eventData.clickCount == 1 && OnPanelLeftClick != null)
		{
			OnPanelLeftClick();
		}
	}
}
