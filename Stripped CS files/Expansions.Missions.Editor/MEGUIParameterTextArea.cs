using ns9;
using TMPro;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_TextArea]
public class MEGUIParameterTextArea : MEGUIParameter
{
	public TMP_InputField inputField;

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
			string value = field.GetValue<string>();
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			return value;
		}
		set
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		GameEvents.Mission.onLocalizationLockOverriden.Add(onLocalizationLockOverriden);
		title.text = name;
		gapDisplayPartner = (RectTransform)inputField.gameObject.transform;
		contentType = ((MEGUI_TextArea)field.Attribute).ContentType;
		inputField.contentType = (TMP_InputField.ContentType)((contentType == MEGUI_Control.InputContentType.DecimalNumber) ? MEGUI_Control.InputContentType.DecimalNumber : contentType);
		inputField.characterLimit = ((MEGUI_TextArea)field.Attribute).CharacterLimit;
		if (Localizer.Tags.ContainsKey(FieldValue))
		{
			inputField.text = Localizer.Format(FieldValue);
		}
		else
		{
			inputField.text = FieldValue;
		}
		inputField.onValueChanged.AddListener(OnParameterValueChanged);
		inputField.onEndEdit.AddListener(OnParameterEndEdit);
	}

	public void onLocalizationLockOverriden()
	{
		LockLocalizedText();
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

	public void OnMouseOver()
	{
		LockLocalizedText();
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
			if (Localizer.Tags.ContainsKey(value))
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
		string value = "";
		if (data.TryGetValue("value", ref value))
		{
			field.SetValue(value);
			inputField.text = FieldValue;
		}
	}
}
