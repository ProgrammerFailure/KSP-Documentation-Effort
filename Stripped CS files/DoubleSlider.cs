using UnityEngine;
using UnityEngine.UI;

public class DoubleSlider : MonoBehaviour
{
	public delegate void OnValueChanged(float minValue, float maxValue);

	public Slider sliderMax;

	public Slider sliderMin;

	public Slider sliderFill;

	public OnValueChanged onValueChanged;

	public float baseMinValue;

	public float baseMaxValue = 1f;

	public RectTransform rectTransform;

	public bool draggingFill;

	public bool draggingHandles;

	public float minFillOffset;

	public float maxFillOffset;

	public bool allowClipping;

	public bool AllowClipping
	{
		get
		{
			return allowClipping;
		}
		set
		{
			allowClipping = value;
		}
	}

	public float MinValue
	{
		get
		{
			return sliderMin.value;
		}
		set
		{
			sliderMin.value = value;
		}
	}

	public float MaxValue
	{
		get
		{
			return sliderMax.value;
		}
		set
		{
			sliderMax.value = value;
		}
	}

	public float BaseMinValue
	{
		get
		{
			return baseMinValue;
		}
		set
		{
			baseMinValue = value;
		}
	}

	public float BaseMaxValue
	{
		get
		{
			return baseMaxValue;
		}
		set
		{
			baseMaxValue = value;
		}
	}

	public void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		sliderMax.minValue = 0f;
		sliderMin.minValue = 0f;
		sliderMax.maxValue = 1f;
		sliderMin.maxValue = 1f;
		minFillOffset = (0f - (sliderMin.value + sliderMax.value)) / 2f;
		maxFillOffset = (sliderMin.value + sliderMax.value) / 2f;
		ResizeFillSection();
		sliderMin.onValueChanged.AddListener(OnValueChanged_SliderMin);
		sliderMax.onValueChanged.AddListener(OnValueChanged_SliderMax);
		sliderFill.onValueChanged.AddListener(OnValueChanged_SliderFill);
	}

	public void OnValueChanged_SliderMax(float value)
	{
		if (draggingFill)
		{
			return;
		}
		draggingHandles = true;
		if (!allowClipping)
		{
			float num = (sliderMax.handleRect.rect.width * 0.5f - 1f) / rectTransform.rect.width;
			if (sliderMax.value < sliderMin.value + num)
			{
				sliderMax.value = sliderMin.value + num;
			}
		}
		ResizeFillSection();
		CalculateFinalValues(sliderMin.value, sliderMax.value);
		draggingHandles = false;
	}

	public void OnValueChanged_SliderMin(float value)
	{
		if (draggingFill)
		{
			return;
		}
		draggingHandles = true;
		if (!allowClipping)
		{
			float num = (sliderMin.handleRect.rect.width * 0.5f - 1f) / rectTransform.rect.width;
			if (sliderMin.value + num > sliderMax.value)
			{
				sliderMin.value = sliderMax.value - num;
			}
		}
		ResizeFillSection();
		CalculateFinalValues(sliderMin.value, sliderMax.value);
		draggingHandles = false;
	}

	public void OnRectTransformDimensionsChange()
	{
		ResizeFillSection();
	}

	public void OnValueChanged_SliderFill(float value)
	{
		if (!draggingHandles)
		{
			draggingFill = true;
			float num = maxFillOffset + value;
			float num2 = minFillOffset + value;
			sliderMin.value = num2;
			sliderMax.value = num;
			if (num > sliderMax.value || num2 < sliderMin.value)
			{
				sliderFill.value = (sliderMax.value + sliderMin.value) / 2f;
			}
			CalculateFinalValues(sliderMin.value, sliderMax.value);
			draggingFill = false;
		}
	}

	public void ResizeFillSection()
	{
		if (rectTransform == null)
		{
			rectTransform = GetComponent<RectTransform>();
		}
		float maxValue = sliderMax.maxValue;
		float num = sliderMin.value / maxValue;
		float num2 = (sliderMax.value - maxValue) / maxValue;
		float y = sliderFill.handleRect.sizeDelta.y;
		float x = rectTransform.rect.width * (1f - (num - num2)) * maxValue;
		sliderFill.handleRect.sizeDelta = new Vector2(x, y);
		sliderFill.value = (sliderMax.value + sliderMin.value) / 2f;
		maxFillOffset = sliderFill.value - sliderMin.value / sliderMin.maxValue;
		minFillOffset = sliderFill.value - sliderMax.value / sliderMax.maxValue;
	}

	public void CalculateFinalValues(float minValue, float maxValue)
	{
		float maxValue2 = sliderMax.value * baseMaxValue - sliderMax.value * baseMinValue + baseMinValue;
		float minValue2 = sliderMin.value * baseMaxValue - sliderMin.value * baseMinValue + baseMinValue;
		if (onValueChanged != null)
		{
			onValueChanged(minValue2, maxValue2);
		}
	}
}
