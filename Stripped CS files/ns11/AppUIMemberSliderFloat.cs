using ns2;
using UnityEngine.UI;

namespace ns11;

[AppUI_SliderFloat]
public class AppUIMemberSliderFloat : AppUIMember
{
	public Slider valueSlider;

	public float minValue;

	public float maxValue = 1f;

	public bool wholeNumbers;

	public void OnDestroy()
	{
		valueSlider.onValueChanged.RemoveListener(OnSliderChanged);
	}

	public override void OnInitialized()
	{
		if (_attribs is AppUI_SliderFloat appUI_SliderFloat)
		{
			minValue = appUI_SliderFloat.minValue;
			maxValue = appUI_SliderFloat.maxValue;
			wholeNumbers = appUI_SliderFloat.wholeNumbers;
		}
		valueSlider.onValueChanged.AddListener(OnSliderChanged);
	}

	public override void OnRefreshUI()
	{
		valueSlider.minValue = minValue;
		valueSlider.maxValue = maxValue;
		valueSlider.wholeNumbers = wholeNumbers;
		valueSlider.value = GetValue<float>();
	}

	public void OnSliderChanged(float value)
	{
		SetValue(value);
	}
}
