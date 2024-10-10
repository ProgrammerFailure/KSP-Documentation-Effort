using UnityEngine;

public class LinearGauge : MonoBehaviour
{
	public Vector3 minPos;

	public Vector3 maxPos;

	public float maxValue;

	public float minValue;

	public float responsiveness;

	public float logarithmic;

	public float exponent;

	public GaugeLEDRange[] ledRanges;

	public Transform pointer;

	public GClass5 led;

	public float rawValue;

	public float currValue;

	public float tgtValue;

	public Vector3 tgtPos;

	public int currentRange;

	public float Value => tgtValue;

	public void Start()
	{
		setValue(0f);
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
	}

	public void Update()
	{
		currValue += (rawValue - currValue) * (responsiveness * Time.deltaTime);
		currValue = Mathf.Clamp(currValue, minValue, maxValue);
		pointer.transform.localPosition = Vector3.Lerp(pointer.transform.localPosition, tgtPos, responsiveness * Time.deltaTime);
		if (float.IsNaN(currValue))
		{
			return;
		}
		currentRange = -1;
		for (int i = 0; i < ledRanges.Length; i++)
		{
			if (currValue >= ledRanges[i].minValue && !(currValue >= ledRanges[i].maxValue))
			{
				currentRange = i;
				break;
			}
		}
		if (currentRange != -1)
		{
			led.setColor(ledRanges[currentRange].color);
			switch (ledRanges[currentRange].ledAction)
			{
			case GaugeLEDRange.LedAction.off:
				led.setOff();
				break;
			case GaugeLEDRange.LedAction.on:
				led.setOn();
				break;
			case GaugeLEDRange.LedAction.blink:
				led.blink(ledRanges[currentRange].blinkInterval);
				break;
			}
			startSound(ledRanges[currentRange].soundClip != null && HighLogic.LoadedSceneIsFlight);
		}
	}

	public void setValue(double val)
	{
		setValue((float)val);
	}

	public void setValue(float val)
	{
		if (float.IsNaN(val))
		{
			val = 0f;
		}
		rawValue = val;
		tgtValue = val;
		if (logarithmic != 0f)
		{
			if (val > 0f)
			{
				tgtValue = Mathf.Max(0f, Mathf.Log(val, logarithmic));
			}
			else if (val < 0f)
			{
				val = Mathf.Abs(val);
				tgtValue = 0f - Mathf.Max(0f, Mathf.Log(val, logarithmic));
			}
			else
			{
				val = 0f;
				tgtValue = 0f;
			}
		}
		else if (exponent != 0f)
		{
			tgtValue = Mathf.Pow(Mathf.Abs(val), exponent) * Mathf.Sign(val);
		}
		tgtValue = Mathf.Clamp(tgtValue, minValue, maxValue);
		tgtPos = minPos + (maxPos - minPos) * ((tgtValue - minValue) / (maxValue - minValue));
	}

	public void startSound(bool start)
	{
		if (start)
		{
			if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().clip = ledRanges[currentRange].soundClip;
				GetComponent<AudioSource>().Play();
			}
		}
		else if (GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().Stop();
		}
	}
}
