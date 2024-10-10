using ns2;
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

	public float scrollStep = 0.1f;

	[SerializeField]
	public ScrollRect partCategoryScroll;

	[SerializeField]
	public PointerClickAndHoldHandler GetScrollBtnDown;

	[SerializeField]
	public PointerClickAndHoldHandler GetScrollBtnUp;

	[SerializeField]
	public EventTrigger[] eventTriggersSliders;

	public OnSliderHovered OnSliderHovered_Zoom;

	public OnSliderHovered OnSliderHovered_Light;

	public OnSliderHovered OnSliderHovered_Simple;

	public OnSliderHovered OnSliderHovered_Double;

	public OnSliderHovered OnSliderHoveredDelegate;

	public GAPUtilSlider hoveredSlider;

	public bool sliderPressed;

	public GAPUtilSlider HoveredSlider => hoveredSlider;

	public override void Awake()
	{
		base.Awake();
		InitializeSliderEvents();
		GetScrollBtnDown.onPointerDownHold.AddListener(OnPointerDown);
		GetScrollBtnUp.onPointerDownHold.AddListener(OnPointerUp);
	}

	public void Update()
	{
		if (OnSliderHoveredDelegate != null)
		{
			OnSliderHoveredDelegate();
		}
	}

	public void ClearEvents()
	{
		sliderZoom.onValueChanged.RemoveAllListeners();
		leftButton.onClick.RemoveAllListeners();
		rightButton.onClick.RemoveAllListeners();
		doubleSlider.onValueChanged = null;
		sliderZoom.gameObject.SetActive(value: false);
		sliderSimple.gameObject.SetActive(value: false);
		doubleSlider.gameObject.SetActive(value: false);
		containerFooter.gameObject.SetActive(value: false);
		containerLoadScreen.gameObject.SetActive(value: false);
		OnSliderHovered_Zoom = null;
		OnSliderHovered_Light = null;
		OnSliderHovered_Simple = null;
		OnSliderHovered_Double = null;
		OnSliderHoveredDelegate = null;
		ClearSidebar();
	}

	public void InitializeSliderEvents()
	{
		for (int i = 0; i < eventTriggersSliders.Length; i++)
		{
			GAPUtilSlider currentSlider = (GAPUtilSlider)i;
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate
			{
				OnSliderMouseEnter(currentSlider);
			});
			eventTriggersSliders[i].triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerExit;
			entry2.callback.AddListener(delegate
			{
				OnSliderMouseExit(currentSlider);
			});
			eventTriggersSliders[i].triggers.Add(entry2);
			EventTrigger.Entry entry3 = new EventTrigger.Entry();
			entry3.eventID = EventTriggerType.PointerUp;
			entry3.callback.AddListener(delegate
			{
				OnSliderMouseUp(currentSlider);
			});
			eventTriggersSliders[i].triggers.Add(entry3);
			EventTrigger.Entry entry4 = new EventTrigger.Entry();
			entry4.eventID = EventTriggerType.PointerDown;
			entry4.callback.AddListener(delegate
			{
				OnSliderMouseDown(currentSlider);
			});
			eventTriggersSliders[i].triggers.Add(entry4);
		}
		hoveredSlider = GAPUtilSlider.None;
	}

	public void OnSliderMouseEnter(GAPUtilSlider sliderType)
	{
		if (!sliderPressed)
		{
			switch (sliderType)
			{
			case GAPUtilSlider.Zoom:
				OnSliderHoveredDelegate = OnSliderHovered_Zoom;
				break;
			case GAPUtilSlider.Light:
				OnSliderHoveredDelegate = OnSliderHovered_Light;
				break;
			case GAPUtilSlider.Simple:
				OnSliderHoveredDelegate = OnSliderHovered_Simple;
				break;
			case GAPUtilSlider.Double:
				OnSliderHoveredDelegate = OnSliderHovered_Double;
				break;
			}
			hoveredSlider = sliderType;
		}
	}

	public void OnSliderMouseExit(GAPUtilSlider sliderType)
	{
		if (!sliderPressed)
		{
			ClearHoveredSlider();
		}
	}

	public void OnSliderMouseDown(GAPUtilSlider sliderType)
	{
		if (hoveredSlider != GAPUtilSlider.None)
		{
			sliderPressed = true;
		}
	}

	public void OnSliderMouseUp(GAPUtilSlider sliderType)
	{
		if (hoveredSlider != GAPUtilSlider.None)
		{
			sliderPressed = false;
			ClearHoveredSlider();
		}
	}

	public void ClearHoveredSlider()
	{
		hoveredSlider = GAPUtilSlider.None;
		OnSliderHoveredDelegate = null;
		footerText.text = string.Empty;
	}

	public void ScrollWithButtons(float direction)
	{
		partCategoryScroll.verticalNormalizedPosition = Mathf.Clamp(partCategoryScroll.verticalNormalizedPosition + scrollStep * direction, 0f, 1f);
	}

	public void OnPointerDown(PointerEventData data)
	{
		ScrollWithButtons(-1f);
	}

	public void OnPointerUp(PointerEventData data)
	{
		ScrollWithButtons(1f);
	}

	public void OnDestroy()
	{
		GetScrollBtnDown.onPointerDownHold.RemoveListener(OnPointerDown);
		GetScrollBtnUp.onPointerDownHold.RemoveListener(OnPointerDown);
	}
}
