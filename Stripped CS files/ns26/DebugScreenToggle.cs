using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns26;

public class DebugScreenToggle : MonoBehaviour
{
	public Toggle toggle;

	public TextMeshProUGUI toggleText;

	public bool initiallyOn;

	public string text = "";

	public void Awake()
	{
		SetToggle(initiallyOn);
		SetToggleText(text);
		SetupValues();
		toggle.onValueChanged.AddListener(OnToggleChanged);
	}

	public void SetToggleText(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			if (toggleText.text != text)
			{
				toggleText.text = text;
			}
			this.text = text;
		}
	}

	public void SetToggle(bool state)
	{
		toggle.isOn = state;
	}

	public virtual void SetupValues()
	{
	}

	public virtual void OnToggleChanged(bool state)
	{
	}

	public void Set(bool b)
	{
		if (toggle.isOn != b)
		{
			toggle.isOn = b;
		}
	}
}
