using System;
using UnityEngine;

namespace EdyCommonTools;

public class Spline : MonoBehaviour
{
	public enum Type
	{
		Cardinal,
		const_1
	}

	[Serializable]
	public class Point
	{
		public Vector3 position;

		[NonSerialized]
		public Vector3 inTangent;

		[NonSerialized]
		public Vector3 outTangent;

		[NonSerialized]
		public float segmentLength;
	}

	public enum ProjectionMode
	{
		Off,
		XZPlane,
		Colliders
	}

	public enum WrapMode
	{
		Clamp,
		Repeat,
		PingPong
	}

	public Type type;

	[Range(0f, 1f)]
	public float a = 0.5f;

	[Range(-1f, 1f)]
	public float t;

	[Range(-1f, 1f)]
	public float c;

	[Range(-1f, 1f)]
	public float b;

	public bool uniform = true;

	public bool closed;

	public Point[] points = new Point[0];

	[Range(1f, 50f)]
	public int resolution = 20;

	[Header("Display")]
	public Color color = GColor.green;

	[Range(0f, 1f)]
	[Space(5f)]
	public float pointRadius = 0.05f;

	public Color pointColor = GColor.blue;

	[Space(5f)]
	public bool tangents;

	[Range(0.1f, 2f)]
	public float tangentLength = 0.5f;

	[Space(5f)]
	public ProjectionMode projectionMode = ProjectionMode.XZPlane;

	public Color projectionColor = GColor.Alpha(GColor.gray, 0.75f);

	public float projectionY;

	public float projectionMaxY = 500f;

	public float projectionMinY = -500f;

	[Header("Captions")]
	public bool identifiers = true;

	public bool segmentLength;

	public bool pointDistance;

	public bool projectionHeight;

	public float m_length;

	public float length => m_length;

	public void Awake()
	{
		ComputeTangents();
	}

	public void OnValidate()
	{
		ComputeTangents();
	}

	public void OnDrawGizmos()
	{
		if (!base.enabled || !base.gameObject.activeInHierarchy || points.Length < 1)
		{
			return;
		}
		for (int i = 0; i < points.Length; i++)
		{
			Point p = points[i];
			DrawPoint(p);
			if (i < points.Length - 1)
			{
				Point p2 = points[i + 1];
				DrawSpline(p, p2);
			}
			if (i == points.Length - 1 && closed && points.Length > 2)
			{
				Point p3 = points[0];
				DrawSpline(p, p3);
			}
			if (tangents)
			{
				DrawIncomingTangentGizmo(p, GColor.blue);
				DrawOutcomingTangentGizmo(p, GColor.orange);
			}
		}
		Gizmos.color = Color.white;
		DebugUtility.CrossMarkGizmo(base.transform.position, base.transform, pointRadius * 2f);
	}

	public Vector3 GetPosition(float s, WrapMode wrapMode = WrapMode.Clamp)
	{
		if (points.Length == 0)
		{
			return base.transform.position;
		}
		if (points.Length == 1)
		{
			return base.transform.TransformPoint(points[0].position);
		}
		s = GetPositionPoints(s, wrapMode, out var p, out var p2);
		return GetRelativePosition(p, p2, s);
	}

	public Vector3 GetPosition(float s, out Vector3 tangent, WrapMode wrapMode = WrapMode.Clamp)
	{
		if (points.Length == 0)
		{
			tangent = base.transform.forward;
			return base.transform.position;
		}
		if (points.Length == 1)
		{
			tangent = base.transform.forward;
			return base.transform.TransformPoint(points[0].position);
		}
		s = GetPositionPoints(s, wrapMode, out var p, out var p2);
		tangent = GetRelativeTangent(p, p2, s);
		return GetRelativePosition(p, p2, s);
	}

	public Vector3 GetPosition(float s, out Vector3 tangent, out Vector3 normal, Vector3 up, WrapMode wrapMode = WrapMode.Clamp)
	{
		if (points.Length == 0)
		{
			tangent = base.transform.forward;
			normal = base.transform.up;
			return base.transform.position;
		}
		if (points.Length == 1)
		{
			tangent = base.transform.forward;
			normal = base.transform.up;
			return base.transform.TransformPoint(points[0].position);
		}
		s = GetPositionPoints(s, wrapMode, out var p, out var p2);
		tangent = GetRelativeTangent(p, p2, s);
		normal = base.transform.TransformDirection(SplineUtility.GetNormal(tangent, up));
		return GetRelativePosition(p, p2, s);
	}

	public float GetPositionPoints(float s, WrapMode wrapMode, out Point p0, out Point p1)
	{
		if (uniform)
		{
			s = DistanceToPointPosition(s, wrapMode);
		}
		if (closed && points.Length > 2)
		{
			s = Mathf.Repeat(s, points.Length);
			int num = Mathf.FloorToInt(s);
			if (num == points.Length - 1)
			{
				p0 = points[num];
				p1 = points[0];
			}
			else
			{
				p0 = points[num];
				p1 = points[num + 1];
			}
		}
		else
		{
			int num2 = points.Length - 1;
			s = ClampPosition(s, num2, wrapMode);
			int num3 = Mathf.FloorToInt(s);
			if (num3 == num2)
			{
				p0 = points[num2];
				p1 = p0;
				s = 0f;
			}
			else
			{
				p0 = points[num3];
				p1 = points[num3 + 1];
			}
		}
		return s % 1f;
	}

	public float DistanceToPointPosition(float distance, WrapMode wrapMode)
	{
		if (points.Length < 2)
		{
			return 0f;
		}
		int num;
		if (closed && points.Length > 2)
		{
			distance = Mathf.Repeat(distance, m_length);
			num = points.Length - 1;
		}
		else
		{
			distance = ClampPosition(distance, m_length, wrapMode);
			num = points.Length - 2;
		}
		float num2 = 0f;
		float num3 = 0f;
		int i;
		for (i = 0; i <= num; i++)
		{
			num3 = points[i].segmentLength;
			if (num2 + num3 >= distance)
			{
				break;
			}
			num2 += num3;
		}
		return (float)i + (distance - num2) / num3;
	}

	public float ClampPosition(float s, float max, WrapMode wrapMode)
	{
		s = wrapMode switch
		{
			WrapMode.PingPong => Mathf.PingPong(s, max), 
			WrapMode.Repeat => Mathf.Repeat(s, max), 
			_ => Mathf.Clamp(s, 0f, max), 
		};
		return s;
	}

	public Vector3 GetRelativePosition(Point p0, Point p1, float s)
	{
		Vector3 position = SplineUtility.Hermite(p0.position, p1.position, p0.outTangent, p1.inTangent, s);
		return base.transform.TransformPoint(position);
	}

	public Vector3 GetRelativeTangent(Point p0, Point p1, float s)
	{
		Vector3 direction = SplineUtility.HermiteTangent(p0.position, p1.position, p0.outTangent, p1.inTangent, s);
		return base.transform.TransformDirection(direction);
	}

	public Vector3 GetMedianPointPosition()
	{
		Vector3 zero = Vector3.zero;
		if (points.Length != 0)
		{
			Point[] array = points;
			foreach (Point point in array)
			{
				zero += point.position;
			}
			zero /= (float)points.Length;
		}
		return base.transform.TransformPoint(zero);
	}

	public void MoveAllPointsLocally(Vector3 delta)
	{
		Point[] array = points;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].position += delta;
		}
	}

	public void ComputeTangents()
	{
		if (type == Type.Cardinal)
		{
			if (closed && points.Length > 2)
			{
				ComputeTangentsCardinalClosed();
			}
			else if (points.Length > 1)
			{
				ComputeTangentsCardinalOpen();
			}
			else
			{
				ClearTangents();
			}
		}
		else if (closed && points.Length > 2)
		{
			ComputeTangentsTCBClosed();
		}
		else if (points.Length > 1)
		{
			ComputeTangentsTCBOpen();
		}
		else
		{
			ClearTangents();
		}
		ComputeLength();
		if (uniform)
		{
			ComputeUniformTangents();
			ComputeLength();
		}
	}

	public void ComputeTangentsCardinalOpen()
	{
		for (int i = 0; i < points.Length; i++)
		{
			Vector3 vector;
			Vector3 outTangent;
			if (i == 0)
			{
				vector = Vector3.zero;
				outTangent = a * 2f * (points[i + 1].position - points[i].position);
			}
			else if (i == points.Length - 1)
			{
				vector = a * 2f * (points[i].position - points[i - 1].position);
				outTangent = Vector3.zero;
			}
			else
			{
				vector = a * (points[i + 1].position - points[i - 1].position);
				outTangent = vector;
			}
			points[i].inTangent = vector;
			points[i].outTangent = outTangent;
		}
	}

	public void ComputeTangentsCardinalClosed()
	{
		for (int i = 0; i < points.Length; i++)
		{
			Vector3 position;
			Vector3 position2;
			if (i == 0)
			{
				position = points[points.Length - 1].position;
				position2 = points[i + 1].position;
			}
			else if (i == points.Length - 1)
			{
				position = points[i - 1].position;
				position2 = points[0].position;
			}
			else
			{
				position = points[i - 1].position;
				position2 = points[i + 1].position;
			}
			Vector3 vector = a * (position2 - position);
			points[i].inTangent = vector;
			points[i].outTangent = vector;
		}
	}

	public void ComputeTangentsTCBOpen()
	{
		for (int i = 0; i < points.Length; i++)
		{
			Vector3 outTangent;
			Vector3 inTangent;
			if (i == 0)
			{
				outTangent = (1f - t) * (1f - c) * (1f - b) * (points[i + 1].position - points[i].position);
				inTangent = Vector3.zero;
			}
			else if (i == points.Length - 1)
			{
				inTangent = (1f - t) * (1f - c) * (1f + b) * (points[i].position - points[i - 1].position);
				outTangent = Vector3.zero;
			}
			else
			{
				Vector3 position = points[i - 1].position;
				Vector3 position2 = points[i].position;
				Vector3 position3 = points[i + 1].position;
				inTangent = (1f - t) * (1f - c) * (1f + b) * 0.5f * (position2 - position) + (1f - t) * (1f + c) * (1f - b) * 0.5f * (position3 - position2);
				outTangent = (1f - t) * (1f + c) * (1f + b) * 0.5f * (position2 - position) + (1f - t) * (1f - c) * (1f - b) * 0.5f * (position3 - position2);
			}
			points[i].inTangent = inTangent;
			points[i].outTangent = outTangent;
		}
	}

	public void ComputeTangentsTCBClosed()
	{
		for (int i = 0; i < points.Length; i++)
		{
			Vector3 position;
			Vector3 position2;
			if (i == 0)
			{
				position = points[points.Length - 1].position;
				position2 = points[i + 1].position;
			}
			else if (i == points.Length - 1)
			{
				position = points[i - 1].position;
				position2 = points[0].position;
			}
			else
			{
				position = points[i - 1].position;
				position2 = points[i + 1].position;
			}
			Vector3 position3 = points[i].position;
			points[i].inTangent = (1f - t) * (1f - c) * (1f + b) * 0.5f * (position3 - position) + (1f - t) * (1f + c) * (1f - b) * 0.5f * (position2 - position3);
			points[i].outTangent = (1f - t) * (1f + c) * (1f + b) * 0.5f * (position3 - position) + (1f - t) * (1f - c) * (1f - b) * 0.5f * (position2 - position3);
		}
	}

	public void ClearTangents()
	{
		for (int i = 0; i < points.Length; i++)
		{
			Point obj = points[i];
			obj.inTangent = Vector3.zero;
			obj.outTangent = Vector3.zero;
		}
	}

	public void ComputeLength()
	{
		m_length = 0f;
		int num = points.Length;
		for (int i = 0; i < num - 1; i++)
		{
			float num2 = SegmentLength(points[i], points[i + 1]);
			points[i].segmentLength = num2;
			m_length += num2;
		}
		if (closed && num > 2)
		{
			float num3 = SegmentLength(points[num - 1], points[0]);
			points[num - 1].segmentLength = num3;
			m_length += num3;
		}
		else if (num > 0)
		{
			points[num - 1].segmentLength = 0f;
		}
	}

	public float SegmentLength(Point p0, Point p1)
	{
		float num = 0f;
		Vector3 vector = base.transform.TransformPoint(p0.position);
		float num2 = 1f / (float)resolution;
		for (int i = 1; i <= resolution; i++)
		{
			Vector3 relativePosition = GetRelativePosition(p0, p1, (float)i * num2);
			num += Vector3.Distance(vector, relativePosition);
			vector = relativePosition;
		}
		return num;
	}

	public void ComputeUniformTangents()
	{
		int num = points.Length;
		if (num >= 3)
		{
			for (int i = 1; i < num - 1; i++)
			{
				float num2 = points[i - 1].segmentLength;
				float num3 = points[i].segmentLength;
				points[i].inTangent *= 2f * num2 / (num2 + num3);
				points[i].outTangent *= 2f * num3 / (num2 + num3);
			}
			if (closed)
			{
				float num4 = points[num - 1].segmentLength;
				float num5 = points[0].segmentLength;
				points[0].inTangent *= 2f * num4 / (num4 + num5);
				points[0].outTangent *= 2f * num5 / (num4 + num5);
				num4 = points[num - 2].segmentLength;
				num5 = points[num - 1].segmentLength;
				points[num - 1].inTangent *= 2f * num4 / (num4 + num5);
				points[num - 1].outTangent *= 2f * num5 / (num4 + num5);
			}
		}
	}

	public Vector3 GetProjectedPoint(Vector3 pos)
	{
		if (projectionMode == ProjectionMode.XZPlane)
		{
			pos.y = projectionY;
		}
		else if (projectionMode == ProjectionMode.Colliders)
		{
			Vector3 origin = pos;
			origin.y = projectionMaxY;
			if (Physics.Raycast(origin, -Vector3.up, out var hitInfo, projectionMaxY - projectionMinY))
			{
				pos = hitInfo.point;
			}
			else
			{
				pos.y = 0f;
			}
		}
		return pos;
	}

	public void DrawSplineGizmo(Point p0, Point p1, Color col, bool projected = false)
	{
		Gizmos.color = col;
		Vector3 vector = base.transform.TransformPoint(p0.position);
		if (projected)
		{
			vector = GetProjectedPoint(vector);
		}
		float num = 1f / (float)resolution;
		for (int i = 1; i <= resolution; i++)
		{
			Vector3 vector2 = GetRelativePosition(p0, p1, (float)i * num);
			if (projected)
			{
				vector2 = GetProjectedPoint(vector2);
			}
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	public void DrawProjectedPointGizmo(Point p0, float radius, Color col)
	{
		Vector3 vector = base.transform.TransformPoint(p0.position);
		Vector3 projectedPoint = GetProjectedPoint(vector);
		Gizmos.color = col;
		Gizmos.DrawLine(vector, projectedPoint);
		Gizmos.DrawSphere(projectedPoint, radius);
	}

	public void DrawIncomingTangentGizmo(Point p0, Color col)
	{
		if (!(p0.inTangent == Vector3.zero))
		{
			Vector3 from = base.transform.TransformPoint(p0.position);
			Vector3 to = base.transform.TransformPoint(p0.position - p0.inTangent.normalized * tangentLength);
			Gizmos.color = col;
			Gizmos.DrawLine(from, to);
		}
	}

	public void DrawOutcomingTangentGizmo(Point p0, Color col)
	{
		if (!(p0.outTangent == Vector3.zero))
		{
			Vector3 from = base.transform.TransformPoint(p0.position);
			Vector3 to = base.transform.TransformPoint(p0.position + p0.outTangent.normalized * tangentLength);
			Gizmos.color = col;
			Gizmos.DrawLine(from, to);
		}
	}

	public void DrawPointGizmo(Point p0, float radius, Color col)
	{
		Gizmos.color = col;
		Gizmos.DrawSphere(base.transform.TransformPoint(p0.position), radius);
	}

	public void DrawPoint(Point p0)
	{
		if (projectionMode != 0)
		{
			DrawProjectedPointGizmo(p0, pointRadius, projectionColor);
		}
		DrawPointGizmo(p0, pointRadius, pointColor);
	}

	public void DrawSpline(Point p0, Point p1)
	{
		if (projectionMode != 0)
		{
			DrawSplineGizmo(p0, p1, projectionColor, projected: true);
		}
		DrawSplineGizmo(p0, p1, color);
	}
}
