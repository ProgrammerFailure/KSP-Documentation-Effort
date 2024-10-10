using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class UITimeWarpScrubber : MonoBehaviour
{
	[SerializeField]
	public Slider timeSlider;

	public PointerClickAndHoldHandler pointerClickHandler;

	[SerializeField]
	public float timestepValue = 1000f;

	public void Start()
	{
		TimeWarp.SetRate(0, instant: false);
		timeSlider.value = 0f;
		pointerClickHandler = timeSlider.gameObject.GetComponent<PointerClickAndHoldHandler>();
		pointerClickHandler.onPointerUp.AddListener(OnPointerUp);
		pointerClickHandler.onPointerDownHold.AddListener(OnPointerDown);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!(Planetarium.fetch == null))
		{
			TimeWarp.SetRate(0, instant: false);
			float value = timeSlider.value;
			double universalTime = Planetarium.GetUniversalTime();
			if (value < 0f && universalTime <= 0.0)
			{
				Planetarium.SetUniversalTime(0.0);
			}
			else
			{
				Planetarium.SetUniversalTime(universalTime + (double)(timestepValue * value));
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		timeSlider.value = 0f;
		TimeWarp.SetRate(0, instant: false);
	}
}
