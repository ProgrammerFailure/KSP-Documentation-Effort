using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManeuverNodeEditorSlider : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Slider slider;

	public bool hover;

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
	}

	public void Update()
	{
		if (hover)
		{
			if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() > 0f)
			{
				Slider obj = slider;
				float value = obj.value + 1f;
				obj.value = value;
			}
			else if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() < 0f)
			{
				Slider obj2 = slider;
				float value = obj2.value - 1f;
				obj2.value = value;
			}
		}
	}
}
