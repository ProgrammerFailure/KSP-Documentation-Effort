using System.Collections.Generic;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns21;

public class SettingsGraphicsResolution : SettingsControlBase
{
	public TextMeshProUGUI valueText;

	public Button buttonUp;

	public Button buttonDown;

	public List<Resolution> resolutions = new List<Resolution>();

	public int currentValue;

	public void Start()
	{
		buttonUp.onClick.AddListener(OnButtonUp);
		buttonDown.onClick.AddListener(OnButtonDown);
		OnRevert();
	}

	public void GetAvailableResolutions()
	{
		resolutions = new List<Resolution>();
		int num = Screen.resolutions.Length;
		for (int i = 0; i < num; i++)
		{
			Resolution item = Screen.resolutions[i];
			if (Application.isEditor || item.width < 960 || item.height < 720)
			{
				continue;
			}
			bool flag = false;
			for (int j = 0; j < resolutions.Count; j++)
			{
				if (resolutions[j].height == item.height && resolutions[j].width == item.width)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				resolutions.Add(item);
			}
		}
		currentValue = -1;
		int num2 = 0;
		int count = resolutions.Count;
		while (true)
		{
			if (num2 < count)
			{
				if (resolutions[num2].width == Screen.width && resolutions[num2].height == Screen.height)
				{
					break;
				}
				num2++;
				continue;
			}
			return;
		}
		currentValue = num2;
	}

	public void SetValue()
	{
		if (currentValue == -1)
		{
			valueText.text = Localizer.Format("#autoLOC_472671");
		}
		else
		{
			valueText.text = resolutions[currentValue].width + " x " + resolutions[currentValue].height;
		}
	}

	public void OnButtonUp()
	{
		if (currentValue < resolutions.Count - 1)
		{
			currentValue++;
			SetValue();
		}
	}

	public void OnButtonDown()
	{
		if (currentValue > 0)
		{
			currentValue--;
			SetValue();
		}
	}

	public override void OnApply()
	{
		if (resolutions.Count > currentValue && currentValue >= 0)
		{
			GameSettings.SCREEN_RESOLUTION_HEIGHT = resolutions[currentValue].height;
			GameSettings.SCREEN_RESOLUTION_WIDTH = resolutions[currentValue].width;
		}
	}

	public override void OnRevert()
	{
		GetAvailableResolutions();
		SetValue();
	}
}
