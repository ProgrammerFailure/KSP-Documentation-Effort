using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoftMasking.Samples;

public class Minimap : MonoBehaviour
{
	public RectTransform map;

	public RectTransform marker;

	[Space]
	public float minZoom;

	public float maxZoom;

	public float zoomStep;

	private float _zoom;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Minimap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ZoomIn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ZoomOut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float Clamp(float zoom)
	{
		throw null;
	}
}
