using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns26;

public class DebugScreenSlider : MonoBehaviour
{
	public Slider slider;

	public float sliderMin;

	public float sliderMax = 100f;

	public float sliderDefault = 50f;

	public TextMeshProUGUI sliderText;

	public void Awake()
	{
		slider.minValue = sliderMin;
		slider.maxValue = sliderMax;
		slider.value = sliderDefault;
		SetupValues();
		slider.onValueChanged.AddListener(OnSliderChanged);
	}

	public void SetSlider(float value)
	{
		if (slider.value != value)
		{
			slider.value = value;
		}
	}

	public void SetSliderText(string text)
	{
		if (!string.IsNullOrEmpty(text) && sliderText.text != text)
		{
			sliderText.text = text;
		}
	}

	public virtual void SetupValues()
	{
	}

	public virtual void OnSliderChanged(float value)
	{
	}
}
