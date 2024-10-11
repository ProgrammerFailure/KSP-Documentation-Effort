using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

[RequireComponent(typeof(UIListItem))]
public class RDPlanetListItemContainer : MonoBehaviour
{
	public delegate void SelectionCallback(RDPlanetListItemContainer container, bool selected);

	private SelectionCallback selectionCallback;

	public RawImage planetRawImage;

	[NonSerialized]
	public GameObject planet;

	[NonSerialized]
	public bool cascading;

	[NonSerialized]
	public bool selected;

	[NonSerialized]
	public int hierarchy_level;

	[NonSerialized]
	public RDPlanetListItemContainer parent;

	public UIRadioButton background;

	public TextMeshProUGUI label_planetName;

	public LayoutElement layoutElement;

	public PointerEnterExitHandler hoverController;

	private List<RDPlanetListItemContainer> children;

	[NonSerialized]
	public new string name;

	[NonSerialized]
	public string displayName;

	private float scale_original;

	private float scale_popped;

	private bool over;

	private bool isRotating;

	private float rotationStartTime;

	private float rotationTimeOffset;

	private UIList list;

	[NonSerialized]
	public Camera thumbnailCamera;

	[NonSerialized]
	public RenderTexture thumbnailRenderTexture;

	private float thumbnailCameraSize;

	public LayerMask thumbnailCameraMask;

	public int thumbnailSize;

	private int renderTextureDepth;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDPlanetListItemContainer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLightDirection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string bodyName, string displayName, GameObject planet, bool cascading, float offset_pos, float scale, float container_height, int hierarchy_level, UIList list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateThumbnailCamera(string bodyName, Transform anchor, out Camera camRef, float camSize, LayerMask layerMask, out RenderTexture rtRef, int rtSize, int rtDepth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSelectionCallback(SelectionCallback del)
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
	private void PopOutPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PopInPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddChild(RDPlanetListItemContainer child)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDPlanetListItemContainer GetParentInHierarchy(int hierarchy_level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideChildren()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowChildren()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show(int index)
	{
		throw null;
	}
}
