using UnityEngine;

public class HelixGauge : MonoBehaviour
{
	public Transform helixCW;

	public Transform helixCCW;

	public Transform clipPlane;

	public float ZatMinValue;

	public float ZatMaxValue = 3f;

	public float MinValue;

	public float MaxValue = 1f;

	public bool positiveValuesClockwise = true;

	public float currentValue;

	public float currentAngle;

	public void Start()
	{
		currentValue = 0f;
		currentAngle = 0f;
		UpdateGauge();
	}

	public void UpdateGauge()
	{
		if (positiveValuesClockwise)
		{
			if (currentValue >= 0f)
			{
				if (!helixCW.gameObject.activeInHierarchy)
				{
					helixCW.gameObject.SetActive(value: true);
				}
				if (helixCCW.gameObject.activeInHierarchy)
				{
					helixCCW.gameObject.SetActive(value: false);
				}
			}
			else
			{
				if (helixCW.gameObject.activeInHierarchy)
				{
					helixCW.gameObject.SetActive(value: false);
				}
				if (!helixCCW.gameObject.activeInHierarchy)
				{
					helixCCW.gameObject.SetActive(value: true);
				}
			}
		}
		else if (currentValue >= 0f)
		{
			if (!helixCCW.gameObject.activeInHierarchy)
			{
				helixCCW.gameObject.SetActive(value: true);
			}
			if (helixCW.gameObject.activeInHierarchy)
			{
				helixCW.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (helixCCW.gameObject.activeInHierarchy)
			{
				helixCCW.gameObject.SetActive(value: false);
			}
			if (!helixCW.gameObject.activeInHierarchy)
			{
				helixCW.gameObject.SetActive(value: true);
			}
		}
		clipPlane.transform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(ZatMinValue, ZatMaxValue, Mathf.InverseLerp(MinValue, MaxValue, Mathf.Abs(currentValue))));
		currentAngle = Mathf.InverseLerp(0f, -3f, clipPlane.transform.localPosition.z) * 360f;
		if (positiveValuesClockwise ? (currentValue < 0f) : (currentValue > 0f))
		{
			currentAngle = 360f - currentAngle;
		}
	}

	public void LateUpdate()
	{
		UpdateGauge();
		Debug.DrawRay(base.transform.position, Quaternion.AngleAxis(currentAngle, base.transform.forward) * base.transform.up, Color.green);
	}
}
