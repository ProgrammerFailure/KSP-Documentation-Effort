using System.Runtime.CompilerServices;
using UnityEngine;
using Vectrosity;

namespace KSP.UI;

public class UIWorldPointer : MonoBehaviour
{
	public enum UISnapType
	{
		Anchor,
		Dynamic
	}

	public enum LineType
	{
		Direct,
		Kinked
	}

	public RectTransform uiTransform;

	public Transform worldTransform;

	public Camera worldCam;

	public UISnapType uiSnapType;

	public Vector2 uiSnapAnchor;

	public LineType lineType;

	public int chamferSubdivisions;

	public float chamferDistance;

	public float kinkPercentage;

	public bool continuousUpdate;

	public Color lineColor;

	public float lineWidth;

	private VectorLine line;

	private Material lineMat;

	private Vector3[] localCorners;

	private static Texture2D lineTexture;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIWorldPointer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UIWorldPointer Create(RectTransform uiTransform, Transform worldTransform, Camera worldCam, Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(RectTransform uiTransform, Transform worldTransform, Camera worldCam, Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateLine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateLine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDirect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateKinked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2[] ChamferCorner(Vector2 inPoint, Vector2 cornerPoint, Vector2 outPoint, float chamferDist, int subdivs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Texture2D GetTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 GetUIPoint(Vector3 worldPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetClosestCornerOrMidpoint(Vector3 worldPoint, float midPointPreference)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetAnchoredPoint(Vector3 worldPoint)
	{
		throw null;
	}
}
