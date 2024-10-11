using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.Mapview;

public class MapViewCanvasUtil : MonoBehaviour
{
	private static Transform nodeCanvasContainer;

	private static bool nodeCanvasContainerSetup;

	private static Camera canvasCamera;

	private static Canvas mapViewCanvas;

	private static RectTransform mapViewCanvasRect;

	public static Transform NodeContainer
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Camera CanvasCamera
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Canvas MapViewCanvas
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static RectTransform MapViewCanvasRect
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapViewCanvasUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MapViewCanvasUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ScaledToUISpacePos(Vector3d scaledSpacePos, ref bool zPositive, float zFlattenEasing, float zFlattenMidPoint, float zUIstart, float zUIlength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ScaledToUISpacePos(Vector3d scaledSpacePos, RectTransform uiSpace, Camera uiCamera, ref bool zPositive, float zFlattenEasing, float zFlattenMidPoint, float zUIstart, float zUIlength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform GetNodeCanvasContainer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform ResetNodeCanvasContainer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResetNodeCanvasContainerScale(float uiScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void OnUIScaleChange()
	{
		throw null;
	}
}
