using System.Collections;
using UnityEngine;

public class GClass5 : MonoBehaviour
{
	public enum colorIndices
	{
		off,
		red,
		yellow,
		green,
		blue,
		purple
	}

	public Texture2D[] ledColors;

	public colorIndices currentColor;

	public bool blinking;

	public bool blinkState;

	public float blinkInterval;

	public colorIndices color => currentColor;

	public bool isOn
	{
		get
		{
			if (currentColor != 0)
			{
				return !isBlinking;
			}
			return false;
		}
	}

	public bool isBlinking => blinking;

	public void Awake()
	{
		currentColor = colorIndices.off;
		blinking = false;
		GetComponent<Renderer>().material.mainTexture = ledColors[(int)currentColor];
	}

	public void setColor(colorIndices color)
	{
		currentColor = color;
	}

	public void setOn()
	{
		blinking = false;
		GetComponent<Renderer>().material.mainTexture = ledColors[(int)((currentColor == colorIndices.off) ? colorIndices.red : currentColor)];
	}

	public void setOn(bool on)
	{
		if (on)
		{
			setOn();
		}
		else
		{
			setOff();
		}
	}

	public void setOff()
	{
		blinking = false;
		GetComponent<Renderer>().material.mainTexture = ledColors[0];
	}

	public void blink(float interval)
	{
		blinkInterval = interval;
		if (!blinking)
		{
			blinking = true;
			blinkState = false;
			currentColor = ((currentColor == colorIndices.off) ? colorIndices.red : currentColor);
			StartCoroutine(doBlink());
		}
	}

	public IEnumerator doBlink()
	{
		blinkState = !blinkState;
		int num = (int)(blinkState ? currentColor : colorIndices.off);
		GetComponent<Renderer>().material.mainTexture = ledColors[num];
		yield return new WaitForSeconds(blinkInterval);
		if (blinking && currentColor != 0)
		{
			StartCoroutine(doBlink());
		}
	}
}
