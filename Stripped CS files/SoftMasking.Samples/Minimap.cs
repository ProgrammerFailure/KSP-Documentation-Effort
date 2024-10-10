using UnityEngine;

namespace SoftMasking.Samples;

public class Minimap : MonoBehaviour
{
	public RectTransform map;

	public RectTransform marker;

	[Space]
	public float minZoom = 0.8f;

	public float maxZoom = 1.4f;

	public float zoomStep = 0.2f;

	public float _zoom = 1f;

	public void LateUpdate()
	{
		map.anchoredPosition = -marker.anchoredPosition * _zoom;
	}

	public void ZoomIn()
	{
		_zoom = Clamp(_zoom + zoomStep);
		map.localScale = Vector3.one * _zoom;
	}

	public void ZoomOut()
	{
		_zoom = Clamp(_zoom - zoomStep);
		map.localScale = Vector3.one * _zoom;
	}

	public float Clamp(float zoom)
	{
		return Mathf.Clamp(zoom, minZoom, maxZoom);
	}
}
