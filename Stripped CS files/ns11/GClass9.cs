using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class GClass9 : MonoBehaviour
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

	public Image image;

	public Sprite[] ledColors;

	public colorIndices currentColor;

	public bool blinking;

	public bool blinkingEnded;

	public bool blinkState;

	public float blinkInterval;

	public colorIndices color => currentColor;

	public bool IsOn
	{
		get
		{
			if (image.sprite != ledColors[0])
			{
				return !IsBlinking;
			}
			return false;
		}
	}

	public bool IsBlinking => blinking;

	public void Reset()
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}
	}

	public void Awake()
	{
		currentColor = colorIndices.off;
		blinking = false;
		blinkingEnded = true;
		image.sprite = ledColors[(int)currentColor];
	}

	public void SetColor(colorIndices color)
	{
		currentColor = color;
	}

	public void SetOn()
	{
		blinking = false;
		image.sprite = ledColors[(int)((currentColor == colorIndices.off) ? colorIndices.red : currentColor)];
	}

	public void SetOn(bool on)
	{
		if (on)
		{
			SetOn();
		}
		else
		{
			setOff();
		}
	}

	public void setOff()
	{
		blinking = false;
		image.sprite = ledColors[0];
	}

	public void Blink(float interval)
	{
		blinkInterval = interval;
		if (!blinking)
		{
			blinking = true;
			blinkState = false;
			currentColor = ((currentColor == colorIndices.off) ? colorIndices.red : currentColor);
			if (blinkingEnded)
			{
				StartCoroutine(BlinkCoroutine());
			}
		}
	}

	public IEnumerator BlinkCoroutine()
	{
		blinkingEnded = false;
		while (blinking && currentColor != 0)
		{
			blinkState = !blinkState;
			int num = (int)(blinkState ? currentColor : colorIndices.off);
			image.sprite = ledColors[num];
			yield return new WaitForSeconds(blinkInterval * (Time.fixedDeltaTime / 0.02f));
		}
		blinkingEnded = true;
	}
}
