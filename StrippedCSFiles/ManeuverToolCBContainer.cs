using System;
using System.Runtime.CompilerServices;
using KSP.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManeuverToolCBContainer : MonoBehaviour
{
	public RawImage planetRawImage;

	[NonSerialized]
	public GameObject planet;

	[NonSerialized]
	public bool selected;

	public UIRadioButton background;

	public LayoutElement layoutElement;

	public PointerEnterExitHandler hoverController;

	[NonSerialized]
	public new string name;

	[NonSerialized]
	public string displayName;

	private float scale_original;

	private bool over;

	private bool isRotating;

	private float rotationStartTime;

	private float rotationTimeOffset;

	private float xOffset;

	[NonSerialized]
	public Camera thumbnailCamera;

	[NonSerialized]
	public RenderTexture thumbnailRenderTexture;

	private float thumbnailCameraSize;

	public LayerMask thumbnailCameraMask;

	public int thumbnailSize;

	private int renderTextureDepth;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverToolCBContainer()
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
	private void SetLightDirection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(GameObject parent, string bodyName, string displayName, GameObject planet, float scale, float container_height, float xOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateThumbnailCamera(string bodyName, Transform anchor, out Camera camRef, float camSize, LayerMask layerMask, out RenderTexture rtRef, int rtSize, int rtDepth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointerEnter(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointerExit(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StartRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StopRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RotatePlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show(Vector2 position, float scale)
	{
		throw null;
	}
}
