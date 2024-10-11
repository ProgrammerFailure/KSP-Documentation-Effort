using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

public class Spline : MonoBehaviour
{
	public enum Type
	{
		Cardinal,
		TCB
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Point()
		{
			throw null;
		}
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
	public float a;

	[Range(-1f, 1f)]
	public float t;

	[Range(-1f, 1f)]
	public float c;

	[Range(-1f, 1f)]
	public float b;

	public bool uniform;

	public bool closed;

	public Point[] points;

	[Range(1f, 50f)]
	public int resolution;

	[Header("Display")]
	public Color color;

	[Range(0f, 1f)]
	[Space(5f)]
	public float pointRadius;

	public Color pointColor;

	[Space(5f)]
	public bool tangents;

	[Range(0.1f, 2f)]
	public float tangentLength;

	[Space(5f)]
	public ProjectionMode projectionMode;

	public Color projectionColor;

	public float projectionY;

	public float projectionMaxY;

	public float projectionMinY;

	[Header("Captions")]
	public bool identifiers;

	public bool segmentLength;

	public bool pointDistance;

	public bool projectionHeight;

	private float m_length;

	public float length
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Spline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetPosition(float s, WrapMode wrapMode = WrapMode.Clamp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetPosition(float s, out Vector3 tangent, WrapMode wrapMode = WrapMode.Clamp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetPosition(float s, out Vector3 tangent, out Vector3 normal, Vector3 up, WrapMode wrapMode = WrapMode.Clamp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetPositionPoints(float s, WrapMode wrapMode, out Point p0, out Point p1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float DistanceToPointPosition(float distance, WrapMode wrapMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ClampPosition(float s, float max, WrapMode wrapMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetRelativePosition(Point p0, Point p1, float s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetRelativeTangent(Point p0, Point p1, float s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetMedianPointPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MoveAllPointsLocally(Vector3 delta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ComputeTangents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ComputeTangentsCardinalOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ComputeTangentsCardinalClosed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ComputeTangentsTCBOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ComputeTangentsTCBClosed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearTangents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ComputeLength()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float SegmentLength(Point p0, Point p1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ComputeUniformTangents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetProjectedPoint(Vector3 pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawSplineGizmo(Point p0, Point p1, Color col, bool projected = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawProjectedPointGizmo(Point p0, float radius, Color col)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawIncomingTangentGizmo(Point p0, Color col)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawOutcomingTangentGizmo(Point p0, Color col)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawPointGizmo(Point p0, float radius, Color col)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawPoint(Point p0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawSpline(Point p0, Point p1)
	{
		throw null;
	}
}
