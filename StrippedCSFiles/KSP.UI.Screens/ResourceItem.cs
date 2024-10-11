using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class ResourceItem : MonoBehaviour
{
	public UIStateButton btnStage;

	public Button background;

	public Slider resourceBar;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI amountText;

	public TextMeshProUGUI maxText;

	public TextMeshProUGUI deltaText;

	public PointerEnterExitHandler hoverHandler;

	public double vesselResourceTotal;

	public double vesselResourceCurrent;

	public float vesselResourcePrevious;

	public int resourceID;

	public float delta;

	public double previousAmount;

	public bool isSelected;

	public bool mouseOver;

	public bool showStage;

	private float deltaSmoothed;

	private double resourceValue;

	private bool firstTime;

	private PartSet stageSet;

	private TooltipController_Text tooltip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(PartResourceDefinition resource, bool showStage, PartSet set)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStaged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_click()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_pointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_pointerExit(PointerEventData eventData)
	{
		throw null;
	}
}
