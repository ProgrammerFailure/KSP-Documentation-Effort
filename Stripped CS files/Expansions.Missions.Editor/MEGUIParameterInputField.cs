using System;
using ns9;
using TMPro;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_InputField]
public class MEGUIParameterInputField : MEGUIParameter
{
	public interface ITypeParser
	{
		object Parse(string s);

		string Convert(object o);
	}

	public class FloatTypeParser : ITypeParser
	{
		public object Parse(string s)
		{
			if (float.TryParse(s, out var result))
			{
				return result;
			}
			return 0;
		}

		public string Convert(object o)
		{
			if (o != null)
			{
				return o.ToString();
			}
			return string.Empty;
		}
	}

	public class IntTypeParser : ITypeParser
	{
		public object Parse(string s)
		{
			if (int.TryParse(s, out var result))
			{
				return result;
			}
			if (long.TryParse(s, out var result2))
			{
				if (result2 > 2147483647L)
				{
					return int.MaxValue;
				}
				if (result2 < -2147483648L)
				{
					return int.MinValue;
				}
			}
			return 0;
		}

		public string Convert(object o)
		{
			if (o != null)
			{
				return o.ToString();
			}
			return string.Empty;
		}
	}

	public class DoubleTypeParser : ITypeParser
	{
		public object Parse(string s)
		{
			if (double.TryParse(s, out var result))
			{
				return result;
			}
			return 0;
		}

		public string Convert(object o)
		{
			if (o != null)
			{
				return o.ToString();
			}
			return string.Empty;
		}
	}

	public class StringTypeParser : ITypeParser
	{
		public object Parse(string s)
		{
			return s;
		}

		public string Convert(object o)
		{
			if (o != null)
			{
				return o.ToString();
			}
			return string.Empty;
		}
	}

	public TMP_InputField inputField;

	public ITypeParser parser;

	public MEGUI_Control.InputContentType contentType;

	public bool isDirty;

	public override bool IsInteractable
	{
		get
		{
			return inputField.interactable;
		}
		set
		{
			inputField.interactable = value;
		}
	}

	public string FieldValue
	{
		get
		{
			return parser.Convert(field.GetValue());
		}
		set
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
			field.SetValue(parser.Parse(value));
			inputField.text = FieldValue;
		}
	}

	public override void Setup(string name)
	{
		title.text = name;
		gapDisplayPartner = (RectTransform)inputField.gameObject.transform;
		contentType = ((MEGUI_InputField)field.Attribute).ContentType;
		inputField.contentType = (TMP_InputField.ContentType)((contentType == MEGUI_Control.InputContentType.DecimalNumber) ? MEGUI_Control.InputContentType.DecimalNumber : contentType);
		inputField.characterLimit = ((MEGUI_InputField)field.Attribute).CharacterLimit;
		if (field.FieldType == typeof(int))
		{
			parser = new IntTypeParser();
		}
		else if (field.FieldType == typeof(float))
		{
			parser = new FloatTypeParser();
		}
		else if (field.FieldType == typeof(double))
		{
			parser = new DoubleTypeParser();
		}
		else
		{
			parser = new StringTypeParser();
		}
		if (field.FieldType == typeof(string) && Localizer.Tags.ContainsKey(FieldValue))
		{
			inputField.text = Localizer.Format(FieldValue);
		}
		else
		{
			inputField.text = FieldValue;
		}
		if (contentType == MEGUI_Control.InputContentType.Percentage)
		{
			TMP_InputField tMP_InputField = inputField;
			tMP_InputField.onValidateInput = (TMP_InputField.OnValidateInput)Delegate.Combine(tMP_InputField.onValidateInput, new TMP_InputField.OnValidateInput(OnValidatePercentageInput));
			inputField.onSelect.AddListener(OnSelectInput);
			inputField.text += "%";
		}
		inputField.onValueChanged.AddListener(OnParameterValueChanged);
		inputField.onEndEdit.AddListener(OnParameterEndEdit);
	}

	public override void LockLocalizedText()
	{
		inputField.interactable = true;
		if (!Localizer.OverrideMELock && Localizer.Tags.ContainsKey(FieldValue))
		{
			inputField.interactable = false;
			SetTooltipActive(state: true);
			SetTooltipText(Localizer.Format("#autoLOC_8005000"));
		}
	}

	public override void ResetDefaultValue(string value)
	{
		FieldValue = value;
	}

	public override void RefreshUI()
	{
		inputField.text = FieldValue;
	}

	public char OnValidatePercentageInput(string text, int charIndex, char addedChar)
	{
		if (!char.IsNumber(addedChar) && (addedChar != '.' || text.IndexOf('.') != -1))
		{
			return '\0';
		}
		return addedChar;
	}

	public void OnSelectInput(string value)
	{
		inputField.text = inputField.text.Replace("%", "");
	}

	public void OnParameterValueChanged(string value)
	{
		isDirty = true;
	}

	public void OnParameterEndEdit(string value)
	{
		if (isDirty)
		{
			FieldValue = value;
			if (contentType == MEGUI_Control.InputContentType.Percentage)
			{
				if (string.IsNullOrEmpty(value))
				{
					inputField.text += "0";
				}
				inputField.text += "%";
			}
			if (field.FieldType == typeof(string) && Localizer.Tags.ContainsKey(value))
			{
				inputField.text = Localizer.Format(value);
				if (!Localizer.OverrideMELock)
				{
					SetTooltipActive(state: true);
					inputField.interactable = false;
					SetTooltipText(Localizer.Format("#autoLOC_8005000"));
				}
			}
			else
			{
				inputField.interactable = true;
				SetTooltipActive(state: false);
			}
			UpdateNodeBodyUI();
		}
		isDirty = false;
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		string value = null;
		if (data.TryGetValue("value", ref value))
		{
			field.SetValue(parser.Parse(value));
			inputField.text = FieldValue;
		}
	}
}
