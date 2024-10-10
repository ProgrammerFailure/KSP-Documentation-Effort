using UnityEngine;

public class Gauge : MonoBehaviour
{
	public float minRot;

	public float maxRot;

	public float maxValue;

	public float minValue;

	public float responsiveness;

	public float logarithmic;

	public Vector3 rotationAxis = Vector3.forward;

	public GaugeLEDRange[] ledRanges;

	public Transform pointer;

	public GClass5 led;

	public float rawValue;

	public float currValue;

	public float tgtValue;

	public float tgtRot;

	public int currentRange;

	public float currentTime;

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
		currentTime = Time.realtimeSinceStartup;
	}

	public void Update()
	{
		float num = Time.realtimeSinceStartup - currentTime;
		currentTime = Time.realtimeSinceStartup;
		currValue = Mathf.Clamp(tgtValue, minValue, maxValue);
		if (pointer.localRotation != Quaternion.AngleAxis(tgtRot, rotationAxis))
		{
			pointer.localRotation = Quaternion.Lerp(pointer.localRotation, Quaternion.AngleAxis(tgtRot, rotationAxis), responsiveness * num);
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
		rawValue = val;
		tgtValue = val;
		if (logarithmic != 0f)
		{
			if (val > 0f)
			{
				tgtValue = Mathf.Max(0f, Mathf.Log(val, logarithmic));
			}
			if (val < 0f)
			{
				val = Mathf.Abs(val);
				tgtValue = 0f - Mathf.Max(0f, Mathf.Log(val, logarithmic));
			}
		}
		tgtValue = Mathf.Clamp(tgtValue, minValue, maxValue);
		tgtRot = minRot + (maxRot - minRot) * ((tgtValue - minValue) / (maxValue - minValue));
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
