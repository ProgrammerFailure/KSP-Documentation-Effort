using UnityEngine;

namespace ns11;

public class LinearGauge : MonoBehaviour
{
	public RectTransform pointer;

	public float minValue;

	public Vector2 minValuePosition = Vector2.zero;

	public float maxValue = 1f;

	public Vector2 maxValuePosition = Vector2.right;

	public float sharpness = 10f;

	public float logarithmic;

	public float exponential;

	public float currentValue;

	[SerializeField]
	public float value;

	public float Value
	{
		get
		{
			return value;
		}
		set
		{
			float num = value;
			if (logarithmic != 0f)
			{
				if (num > 0f)
				{
					this.value = Mathf.Max(0f, Mathf.Log(num, logarithmic));
				}
				else if (num < 0f)
				{
					num = Mathf.Abs(num);
					this.value = 0f - Mathf.Max(0f, Mathf.Log(num, logarithmic));
				}
				else
				{
					num = 0f;
					this.value = 0f;
				}
			}
			else if (exponential != 0f)
			{
				this.value = Mathf.Pow(Mathf.Abs(num), exponential) * Mathf.Sign(num);
			}
			else
			{
				this.value = num;
			}
			this.value = Mathf.Clamp(this.value, minValue, maxValue);
		}
	}

	public void SetValue(float value)
	{
		Value = value;
	}

	public void SetValue(double value)
	{
		Value = (float)value;
	}

	public void Reset()
	{
		pointer = GetComponent<RectTransform>();
	}

	public void Awake()
	{
		currentValue = Mathf.Clamp(value, minValue, maxValue);
	}

	public void LateUpdate()
	{
		if ((object)pointer != null)
		{
			currentValue = Mathf.Lerp(currentValue, value, Time.deltaTime * sharpness);
			pointer.anchoredPosition = Vector2.Lerp(minValuePosition, maxValuePosition, (currentValue - minValue) / (maxValue - minValue));
		}
	}
}
