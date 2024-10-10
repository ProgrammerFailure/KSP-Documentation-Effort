using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

[AppUI_RadioBool]
public class AppUIMemberRadioBool : AppUIMember
{
	public Toggle valueToggle;

	public TextMeshProUGUI valueLabel;

	[SerializeField]
	public string _valueText = "Value";

	public bool valueState;

	public string valueText
	{
		get
		{
			return _valueText;
		}
		set
		{
			_valueText = Localizer.Format(value);
		}
	}

	public void OnDestroy()
	{
		valueToggle.onValueChanged.RemoveListener(OnTrueToggleChanged);
	}

	public override void OnInitialized()
	{
		if (_attribs is AppUI_RadioBool appUI_RadioBool)
		{
			valueText = (string.IsNullOrEmpty(appUI_RadioBool.valueText) ? "" : appUI_RadioBool.valueText);
		}
		valueToggle.onValueChanged.AddListener(OnTrueToggleChanged);
	}

	public override void OnRefreshUI()
	{
		valueLabel.text = valueText;
		valueState = GetValue<bool>();
		valueToggle.isOn = valueState;
	}

	public void OnTrueToggleChanged(bool value)
	{
		SetValue(value);
	}
}
