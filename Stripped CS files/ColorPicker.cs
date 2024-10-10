using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
	[SerializeField]
	public float _hue;

	[SerializeField]
	public float _saturation;

	[SerializeField]
	public float _brightness;

	[SerializeField]
	public float _red;

	[SerializeField]
	public float _green;

	[SerializeField]
	public float _blue;

	[SerializeField]
	public float _alpha = 1f;

	public Button modeButton;

	public ColorChangedEvent onValueChanged = new ColorChangedEvent();

	public HSVChangedEvent onHSVChanged = new HSVChangedEvent();

	public ColorChangedEvent onInternalValueChanged = new ColorChangedEvent();

	public GameObject[] rgbModeSliders;

	public GameObject[] hueModeSliders;

	public Color CurrentColor
	{
		get
		{
			return new Color(_red, _green, _blue, _alpha);
		}
		set
		{
			if (!(CurrentColor == value))
			{
				_red = value.r;
				_green = value.g;
				_blue = value.b;
				_alpha = value.a;
				RGBChanged();
				SendChangedEvent();
			}
		}
	}

	public float Single_0
	{
		get
		{
			return _hue;
		}
		set
		{
			if (_hue != value)
			{
				_hue = value;
				HSVChanged();
				SendChangedEvent();
			}
		}
	}

	public float Single_1
	{
		get
		{
			return _saturation;
		}
		set
		{
			if (_saturation != value)
			{
				_saturation = value;
				HSVChanged();
				SendChangedEvent();
			}
		}
	}

	public float Single_2
	{
		get
		{
			return _brightness;
		}
		set
		{
			if (_brightness != value)
			{
				_brightness = value;
				HSVChanged();
				SendChangedEvent();
			}
		}
	}

	public float Single_3
	{
		get
		{
			return _red;
		}
		set
		{
			if (_red != value)
			{
				_red = value;
				RGBChanged();
				SendChangedEvent();
			}
		}
	}

	public float Single_4
	{
		get
		{
			return _green;
		}
		set
		{
			if (_green != value)
			{
				_green = value;
				RGBChanged();
				SendChangedEvent();
			}
		}
	}

	public float Single_5
	{
		get
		{
			return _blue;
		}
		set
		{
			if (_blue != value)
			{
				_blue = value;
				RGBChanged();
				SendChangedEvent();
			}
		}
	}

	public float Single_6
	{
		get
		{
			return _alpha;
		}
		set
		{
			if (_alpha != value)
			{
				_alpha = value;
				SendChangedEvent();
			}
		}
	}

	public void SetColor(Color color)
	{
		if (!(CurrentColor == color))
		{
			_red = color.r;
			_green = color.g;
			_blue = color.b;
			_alpha = color.a;
			RGBChanged();
			SendInternalEvent();
		}
	}

	public void Start()
	{
		SendInternalEvent();
		modeButton.onClick.AddListener(ToggleModeSliders);
	}

	public void OnDestroy()
	{
		modeButton.onClick.RemoveListener(ToggleModeSliders);
	}

	public void OnEnable()
	{
		SendInternalEvent();
	}

	public void RGBChanged()
	{
		HsvColor hsvColor = HSVUtil.ConvertRgbToHsv(CurrentColor);
		_hue = hsvColor.normalizedH;
		_saturation = hsvColor.normalizedS;
		_brightness = hsvColor.normalizedV;
	}

	public void HSVChanged()
	{
		Color color = HSVUtil.ConvertHsvToRgb(_hue * 360f, _saturation, _brightness, _alpha);
		_red = color.r;
		_green = color.g;
		_blue = color.b;
	}

	public void SendChangedEvent()
	{
		if (base.gameObject.activeInHierarchy)
		{
			onValueChanged.Invoke(CurrentColor);
			onHSVChanged.Invoke(_hue, _saturation, _brightness);
		}
	}

	public void SendInternalEvent()
	{
		onInternalValueChanged.Invoke(CurrentColor);
		onHSVChanged.Invoke(_hue, _saturation, _brightness);
	}

	public void AssignColor(ColorValues type, float value)
	{
		switch (type)
		{
		case ColorValues.const_0:
			Single_3 = value;
			break;
		case ColorValues.const_1:
			Single_4 = value;
			break;
		case ColorValues.const_2:
			Single_5 = value;
			break;
		case ColorValues.const_3:
			Single_6 = value;
			break;
		case ColorValues.Hue:
			Single_0 = value;
			break;
		case ColorValues.Saturation:
			Single_1 = value;
			break;
		case ColorValues.Value:
			Single_2 = value;
			break;
		}
	}

	public float GetValue(ColorValues type)
	{
		return type switch
		{
			ColorValues.const_0 => Single_3, 
			ColorValues.const_1 => Single_4, 
			ColorValues.const_2 => Single_5, 
			ColorValues.const_3 => Single_6, 
			ColorValues.Hue => Single_0, 
			ColorValues.Saturation => Single_1, 
			ColorValues.Value => Single_2, 
			_ => throw new NotImplementedException(""), 
		};
	}

	public void ToggleModeSliders()
	{
		int i = 0;
		for (int num = rgbModeSliders.Length; i < num; i++)
		{
			rgbModeSliders[i].SetActive(!rgbModeSliders[i].activeSelf);
		}
		int j = 0;
		for (int num2 = hueModeSliders.Length; j < num2; j++)
		{
			hueModeSliders[j].SetActive(!hueModeSliders[j].activeSelf);
		}
	}
}
