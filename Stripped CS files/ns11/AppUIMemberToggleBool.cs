using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

[AppUI_ToggleBool]
public class AppUIMemberToggleBool : AppUIMember
{
	public Toggle trueToggle;

	public Toggle falseToggle;

	public TextMeshProUGUI trueLabel;

	public TextMeshProUGUI falseLabel;

	[SerializeField]
	public string _trueText = "True";

	[SerializeField]
	public string _falseText = "False";

	public bool valueState;

	public string trueText
	{
		get
		{
			return _trueText;
		}
		set
		{
			_trueText = Localizer.Format(value);
		}
	}

	public string falseText
	{
		get
		{
			return _falseText;
		}
		set
		{
			_falseText = Localizer.Format(value);
		}
	}

	public void OnDestroy()
	{
		trueToggle.onValueChanged.RemoveListener(OnTrueToggleChanged);
	}

	public override void OnInitialized()
	{
		if (_attribs is AppUI_ToggleBool appUI_ToggleBool)
		{
			trueText = (string.IsNullOrEmpty(appUI_ToggleBool.trueText) ? "" : appUI_ToggleBool.trueText);
			falseText = (string.IsNullOrEmpty(appUI_ToggleBool.falseText) ? "" : appUI_ToggleBool.falseText);
		}
		trueToggle.onValueChanged.AddListener(OnTrueToggleChanged);
	}

	public override void OnRefreshUI()
	{
		trueLabel.text = trueText;
		falseLabel.text = falseText;
		valueState = GetValue<bool>();
		if (valueState)
		{
			trueToggle.isOn = true;
		}
		else
		{
			falseToggle.isOn = true;
		}
	}

	public void OnTrueToggleChanged(bool value)
	{
		SetValue(value);
	}
}
