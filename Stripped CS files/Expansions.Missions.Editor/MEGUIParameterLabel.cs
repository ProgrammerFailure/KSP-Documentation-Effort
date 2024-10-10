using TMPro;

namespace Expansions.Missions.Editor;

[MEGUI_Label]
public class MEGUIParameterLabel : MEGUIParameter
{
	public TextMeshProUGUI labelText;

	public bool simpleLabel
	{
		get
		{
			if (field != null)
			{
				return (field.Attribute as MEGUI_Label).simpleLabel;
			}
			return true;
		}
	}

	public string FieldValue
	{
		get
		{
			if (field != null)
			{
				return field.GetValue() as string;
			}
			return string.Empty;
		}
		set
		{
			if (field != null)
			{
				field.SetValue(value);
			}
		}
	}

	public override void Setup(string name)
	{
		if (field != null)
		{
			RefreshUI();
		}
		labelText.gameObject.SetActive(!simpleLabel);
	}

	public override void RefreshUI()
	{
		if (simpleLabel)
		{
			title.text = FieldValue;
			return;
		}
		title.text = field.guiName;
		labelText.text = FieldValue;
	}
}
