using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class GAPUtil_CelestialBody : GAPUtil_BiSelector
{
	public enum GAPUtilFilter
	{
		Vessels,
		LaunchSites,
		Objectives,
		Orbits
	}

	public enum GAPUtilSlider
	{
		Zoom,
		Light,
		Simple,
		Double,
		None
	}

	public delegate void OnSliderHovered();

	public RectTransform containerLoadScreen;

	public TMP_Text textLoading;

	public Slider sliderZoom;

	public Slider sliderSimple;

	public Slider sliderLight;

	public DoubleSlider doubleSlider;

	public Material biomeShaderMaterial;

	public float scrollStep;

	[SerializeField]
	private ScrollRect partCategoryScroll;

	[SerializeField]
	private PointerClickAndHoldHandler GetScrollBtnDown;

	[SerializeField]
	private PointerClickAndHoldHandler GetScrollBtnUp;

	[SerializeField]
	private EventTrigger[] eventTriggersSliders;

	public OnSliderHovered OnSliderHovered_Zoom;

	public OnSliderHovered OnSliderHovered_Light;

	public OnSliderHovered OnSliderHovered_Simple;

	public OnSliderHovered OnSliderHovered_Double;

	private OnSliderHovered OnSliderHoveredDelegate;

	private GAPUtilSlider hoveredSlider;

	private bool sliderPressed;

	public GAPUtilSlider HoveredSlider
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPUtil_CelestialBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeSliderEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSliderMouseEnter(GAPUtilSlider sliderType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSliderMouseExit(GAPUtilSlider sliderType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSliderMouseDown(GAPUtilSlider sliderType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSliderMouseUp(GAPUtilSlider sliderType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearHoveredSlider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ScrollWithButtons(float direction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerDown(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerUp(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
