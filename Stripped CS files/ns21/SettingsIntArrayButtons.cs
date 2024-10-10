using ns20;
using ns9;
using UnityEngine.UI;

namespace ns21;

public class SettingsIntArrayButtons : SettingsControlReflection
{
	public Button buttonUp;

	public Button buttonDown;

	[SettingsValue(new int[] { 0, 1 })]
	public int[] values = new int[2] { 0, 1 };

	[SettingsValue(new string[] { "A", "B" })]
	public string[] stringValues = new string[2] { "A", "B" };

	[SettingsValue(false)]
	public bool displayStringValue;

	public int currentValue;

	public override void OnStart()
	{
		buttonUp.onClick.AddListener(OnButtonUp);
		buttonDown.onClick.AddListener(OnButtonDown);
		if (!displayStringValue)
		{
			return;
		}
		if (stringValues.Length != 0 && stringValues.Length != values.Length)
		{
			string[] array = Localizer.Format(stringValues[0]).Split(',');
			if (array.Length == values.Length)
			{
				stringValues = array;
			}
			else
			{
				displayStringValue = false;
			}
		}
		else if (stringValues.Length != values.Length)
		{
			displayStringValue = false;
		}
	}

	public override void ValueInitialized()
	{
		int num = (int)base.Value;
		currentValue = values.IndexOf(num);
		if (currentValue == -1)
		{
			currentValue = 0;
		}
	}

	public override void ValueUpdated()
	{
		if (!(valueText == null))
		{
			if (displayStringValue)
			{
				valueText.text = stringValues[currentValue];
			}
			else
			{
				valueText.text = ((int)base.Value).ToString();
			}
		}
	}

	public void OnButtonUp()
	{
		if (currentValue < values.Length - 1)
		{
			currentValue++;
			base.Value = values[currentValue];
		}
	}

	public void OnButtonDown()
	{
		if (currentValue > 0)
		{
			currentValue--;
			base.Value = values[currentValue];
		}
	}
}
