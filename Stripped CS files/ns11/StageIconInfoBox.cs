using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class StageIconInfoBox : MonoBehaviour
{
	[SerializeField]
	public Image titleBg;

	[SerializeField]
	public TextMeshProUGUI title;

	[SerializeField]
	public Slider slider;

	[SerializeField]
	public Image sliderBg;

	[SerializeField]
	public Image sliderFill;

	[SerializeField]
	public TextMeshProUGUI caption;

	public float valueDifferenceBeforeSliderUpdate = 0.01f;

	public float lastSliderValue = -1f;

	[HideInInspector]
	public bool expanded;

	public void Awake()
	{
		base.gameObject.SetActive(value: false);
		expanded = false;
		lastSliderValue = slider.value;
	}

	public void Expand()
	{
		expanded = true;
		base.gameObject.SetActive(value: true);
	}

	public void Collapse()
	{
		expanded = false;
		base.gameObject.SetActive(value: false);
	}

	public void SetMsgTextColor(Color c)
	{
		title.color = c;
	}

	public void SetMsgBgColor(Color c)
	{
		titleBg.color = c;
	}

	public void SetProgressBarColor(Color c)
	{
		sliderFill.color = c;
	}

	public void SetProgressBarBgColor(Color c)
	{
		sliderBg.color = c;
	}

	public void SetMessage(string m)
	{
		title.text = m;
	}

	public void SetValue(float value)
	{
		if (Mathf.Abs(lastSliderValue - value) > valueDifferenceBeforeSliderUpdate || (value == 0f && lastSliderValue != 0f) || (value == 1f && lastSliderValue != 1f))
		{
			slider.value = value;
			lastSliderValue = value;
		}
	}

	public void SetValue(float value, float min, float max)
	{
		SetValue(Mathf.InverseLerp(min, max, value));
	}

	public void SetCaption(string cap)
	{
		caption.text = cap;
	}
}
