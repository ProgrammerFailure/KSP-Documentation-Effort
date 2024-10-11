using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public class UITimeWarpScrubber : MonoBehaviour
{
	[SerializeField]
	private Slider timeSlider;

	private PointerClickAndHoldHandler pointerClickHandler;

	[SerializeField]
	private float timestepValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UITimeWarpScrubber()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointerDown(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointerUp(PointerEventData eventData)
	{
		throw null;
	}
}
