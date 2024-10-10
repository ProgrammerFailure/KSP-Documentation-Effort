using ns2;
using TMPro;
using UnityEngine.UI;

[UI_ChooseOption]
public class UIPartActionChooseOption : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldValue;

	public UIButtonToggle inc;

	public UIButtonToggle dec;

	public Slider slider;

	public int index;

	public UI_ChooseOption chooserControl => (UI_ChooseOption)control;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		index = GetFieldValue();
		fieldName.text = field.guiName;
		UpdateDisplay(null);
		inc.onToggle.AddListener(OnTap_inc);
		dec.onToggle.AddListener(OnTap_dec);
		slider.onValueChanged.AddListener(OnValueChanged);
	}

	public int GetFieldValue()
	{
		if (field.FieldInfo.FieldType == typeof(int))
		{
			int num = field.GetValue<int>(field.host);
			if (num < 0 || num >= chooserControl.options.Length)
			{
				num = -1;
			}
			return num;
		}
		string value = field.GetValue<string>(field.host);
		int num2 = 0;
		while (true)
		{
			if (num2 < chooserControl.options.Length)
			{
				if (chooserControl.options[num2] == value)
				{
					break;
				}
				num2++;
				continue;
			}
			return -1;
		}
		return num2;
	}

	public void SetFieldValue(int index)
	{
		if (index >= 0 && index < chooserControl.options.Length)
		{
			if (field.FieldInfo.FieldType == typeof(int))
			{
				SetFieldValue((object)index);
			}
			else
			{
				SetFieldValue(chooserControl.options[index]);
			}
		}
	}

	public string GetDisplay(int index)
	{
		if (index < 0)
		{
			return "**Not Found**";
		}
		if (chooserControl.display != null && chooserControl.display.Length > index)
		{
			return chooserControl.display[index];
		}
		return chooserControl.options[index];
	}

	public void UpdateDisplay(UIButtonToggle button)
	{
		int num = chooserControl.options.Length;
		if (num < 1)
		{
			num = 1;
		}
		slider.maxValue = num - 1;
		slider.minValue = 0f;
		slider.value = index;
		fieldValue.text = GetDisplay(index);
		if ((bool)button)
		{
			button.SetState(on: false);
		}
	}

	public void UpdateState(int dir, UIButtonToggle button)
	{
		if (dir != 0)
		{
			int num = chooserControl.options.Length;
			index = (index + dir + num) % num;
			SetFieldValue(index);
			UpdateDisplay(button);
		}
	}

	public void OnTap_inc()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			UpdateState(1, inc);
		}
	}

	public void OnTap_dec()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			UpdateState(-1, dec);
		}
	}

	public void OnValueChanged(float obj)
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			UpdateState((int)slider.value - index, null);
		}
	}
}
