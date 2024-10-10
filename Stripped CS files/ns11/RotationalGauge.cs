using UnityEngine;

namespace ns11;

public class RotationalGauge : MonoBehaviour
{
	[SerializeField]
	public float currentAngle;

	public RectTransform pointer;

	public float minValue;

	public float maxValue = 1f;

	public float minRot;

	public float maxRot = 90f;

	public Vector3 rotationAxis = Vector3.forward;

	public float responsiveness = 10f;

	public float logarithmic;

	[SerializeField]
	public float currentValue;

	[SerializeField]
	public float targetValue;

	public float CurrentAngle => currentAngle;

	public float CurrentValue => currentValue;

	public float Value
	{
		get
		{
			return targetValue;
		}
		set
		{
			SetValue(value);
		}
	}

	public virtual void Awake()
	{
		if (logarithmic > 0f)
		{
			if (maxValue < 0f)
			{
				maxValue = 0f - Mathf.Log(Mathf.Abs(maxValue), logarithmic);
			}
			else
			{
				maxValue = Mathf.Log(maxValue, logarithmic);
			}
			if (minValue < 0f)
			{
				minValue = 0f - Mathf.Log(Mathf.Abs(minValue), logarithmic);
			}
			else
			{
				minValue = Mathf.Log(minValue, logarithmic);
			}
		}
		SetValue(0f);
	}

	public virtual void LateUpdate()
	{
		if (responsiveness > 100f)
		{
			currentValue = targetValue;
		}
		else
		{
			currentValue = Mathf.Lerp(currentValue, targetValue, responsiveness * Time.deltaTime);
		}
		currentAngle = Mathf.Lerp(minRot, maxRot, Mathf.InverseLerp(minValue, maxValue, currentValue));
		pointer.localRotation = Quaternion.AngleAxis(currentAngle, rotationAxis);
	}

	public void SetValue(double val)
	{
		SetValue((float)val);
	}

	public virtual void SetValue(float val)
	{
		if (logarithmic != 0f)
		{
			if (val > 0f)
			{
				targetValue = Mathf.Max(0f, Mathf.Log(val, logarithmic));
			}
			else if (val < 0f)
			{
				targetValue = 0f - Mathf.Max(0f, Mathf.Log(Mathf.Abs(val), logarithmic));
			}
			else
			{
				val = 0f;
				targetValue = 0f;
			}
		}
		else
		{
			targetValue = val;
		}
		targetValue = Mathf.Clamp(targetValue, minValue, maxValue);
	}
}
