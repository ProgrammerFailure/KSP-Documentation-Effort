using TMPro;
using UnityEngine;

public class ColorLabel : MonoBehaviour
{
	public ColorPicker picker;

	public ColorValues type;

	public string prefix = "R: ";

	public float minValue;

	public float maxValue = 255f;

	public int precision;

	public TextMeshProUGUI label;

	public void Awake()
	{
		label = GetComponent<TextMeshProUGUI>();
	}

	public void OnEnable()
	{
		if (Application.isPlaying && picker != null)
		{
			picker.onValueChanged.AddListener(ColorChanged);
			picker.onInternalValueChanged.AddListener(ColorChanged);
			picker.onHSVChanged.AddListener(HSVChanged);
			ColorChanged(picker.CurrentColor);
			HSVChanged(picker.Single_0, picker.Single_1, picker.Single_2);
		}
	}

	public void OnDestroy()
	{
		if (picker != null)
		{
			picker.onValueChanged.RemoveListener(ColorChanged);
			picker.onInternalValueChanged.RemoveListener(ColorChanged);
			picker.onHSVChanged.RemoveListener(HSVChanged);
		}
	}

	public void ColorChanged(Color color)
	{
		UpdateValue();
	}

	public void HSVChanged(float hue, float sateration, float value)
	{
		UpdateValue();
	}

	public void UpdateValue()
	{
		if (!(label == null))
		{
			if (picker == null)
			{
				label.text = prefix + "-";
				return;
			}
			float value = minValue + picker.GetValue(type) * (maxValue - minValue);
			label.text = prefix + ConvertToDisplayString(value);
		}
	}

	public string ConvertToDisplayString(float value)
	{
		if (precision > 0)
		{
			return value.ToString("f " + precision);
		}
		return Mathf.FloorToInt(value).ToString();
	}
}
