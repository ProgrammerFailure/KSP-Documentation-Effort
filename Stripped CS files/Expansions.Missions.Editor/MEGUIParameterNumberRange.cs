using System;
using System.Collections;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_NumberRange]
public class MEGUIParameterNumberRange : MEGUIParameter
{
	public Slider slider;

	public TMP_Text valueText;

	public TMP_Text unitsText;

	public TMP_InputField inputText;

	public Button inputFieldButton;

	public EventTrigger sliderEventTrigger;

	public EventTrigger inputEventTrigger;

	public int roundToPlaces;

	public float displayMultiply;

	public string displayFormat;

	public string displayUnits;

	public bool clampTextInput;

	public bool usingSlider;

	public bool usingButton;

	public bool overInputField;

	public double maxValue;

	public double minValue;

	public RectTransform _unitsTextRect;

	public RectTransform _inputTextRect;

	public string unitsSuffix;

	public override bool IsInteractable
	{
		get
		{
			if (slider.interactable)
			{
				return inputText.interactable;
			}
			return false;
		}
		set
		{
			slider.interactable = value;
			inputText.interactable = value;
		}
	}

	public object FieldValue
	{
		get
		{
			return field.GetValue();
		}
		set
		{
			try
			{
				if (field.GetValue() != value)
				{
					if (!usingSlider)
					{
						MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
						usingSlider = true;
						UpdateSlider(Convert.ToSingle(value));
						usingSlider = false;
					}
					UpdateValueText(value);
					field.SetValue(value);
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("[MEGUINumberRange] Unable to set value {0} - {1}", value, ex.Message);
			}
		}
	}

	public double MinValue
	{
		get
		{
			return minValue;
		}
		set
		{
			minValue = value;
			if (minValue < -3.4028234663852886E+38)
			{
				slider.minValue = float.MinValue;
			}
			else if (minValue > 3.4028234663852886E+38)
			{
				slider.minValue = float.MinValue;
			}
			else
			{
				slider.minValue = (float)minValue;
			}
		}
	}

	public double MaxValue
	{
		get
		{
			return maxValue;
		}
		set
		{
			maxValue = value;
			if (maxValue < -3.4028234663852886E+38)
			{
				slider.maxValue = float.MinValue;
			}
			else if (maxValue > 3.4028234663852886E+38)
			{
				slider.maxValue = float.MaxValue;
			}
			else
			{
				slider.maxValue = (float)maxValue;
			}
		}
	}

	public override void Setup(string name)
	{
		title.text = name;
		gapDisplayPartner = (RectTransform)inputFieldButton.gameObject.transform;
		MinValue = ((MEGUI_NumberRange)field.Attribute).minValue;
		MaxValue = ((MEGUI_NumberRange)field.Attribute).maxValue;
		roundToPlaces = ((MEGUI_NumberRange)field.Attribute).roundToPlaces;
		displayMultiply = ((MEGUI_NumberRange)field.Attribute).displayMultiply;
		displayFormat = ((MEGUI_NumberRange)field.Attribute).displayFormat;
		displayUnits = ((MEGUI_NumberRange)field.Attribute).displayUnits;
		clampTextInput = ((MEGUI_NumberRange)field.Attribute).clampTextInput;
		if (!(FieldValue is int) && !(FieldValue is float) && !(FieldValue is double))
		{
			Debug.LogErrorFormat("[MEGUIParameterNumberRange]: Acceptable Types are int, float and double, Field is ", FieldValue.GetType().Name);
		}
		SetUnitsText();
		inputText.contentType = TMP_InputField.ContentType.DecimalNumber;
		UpdateSlider(Convert.ToSingle(FieldValue));
		UpdateValueText(FieldValue);
		slider.onValueChanged.AddListener(OnSliderValueChange);
		inputFieldButton.onClick.AddListener(OnButtonClicked);
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerUp;
		entry.callback.AddListener(delegate
		{
			OnSliderMouserUp();
		});
		sliderEventTrigger.triggers.Add(entry);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerEnter;
		entry2.callback.AddListener(delegate
		{
			OnInputHoverStart();
		});
		inputEventTrigger.triggers.Add(entry2);
		EventTrigger.Entry entry3 = new EventTrigger.Entry();
		entry3.eventID = EventTriggerType.PointerExit;
		entry3.callback.AddListener(delegate
		{
			OnInputHoverEnd();
		});
		inputEventTrigger.triggers.Add(entry3);
		EventTrigger.Entry entry4 = new EventTrigger.Entry();
		entry4.eventID = EventTriggerType.Scroll;
		entry4.callback.AddListener(delegate(BaseEventData eventData)
		{
			OnSAPScroll(eventData);
		});
		sliderEventTrigger.triggers.Add(entry4);
		inputEventTrigger.triggers.Add(entry4);
	}

	public void OnSliderMouserUp()
	{
		usingSlider = false;
	}

	public void OnInputHoverStart()
	{
		overInputField = true;
	}

	public void OnInputHoverEnd()
	{
		overInputField = false;
	}

	public void OnSAPScroll(BaseEventData data)
	{
		MEActionPane.fetch.UpdateSAPScroll(data as PointerEventData);
	}

	public void OnButtonClicked()
	{
		inputText.enabled = true;
		inputFieldButton.gameObject.SetActive(value: false);
		inputText.gameObject.SetActive(value: true);
		unitsText.gameObject.SetActive(value: true);
		usingButton = true;
		slider.interactable = false;
		usingSlider = false;
	}

	public void SetUnitsText()
	{
		unitsSuffix = Localizer.Format(displayUnits);
		if (unitsText != null)
		{
			unitsText.text = unitsSuffix;
			unitsText.gameObject.GetComponentCached(ref _unitsTextRect);
			inputText.gameObject.GetComponentCached(ref _inputTextRect);
			_unitsTextRect.sizeDelta = new Vector2(unitsText.GetPreferredValues().x, _unitsTextRect.sizeDelta.y);
			_inputTextRect.offsetMax = new Vector2(0f - (_unitsTextRect.sizeDelta.x + (gapDisplayIndicator.activeSelf ? ((RectTransform)gapDisplayIndicator.transform).sizeDelta.x : 0f)), 0f);
		}
	}

	public override void Update()
	{
		base.Update();
		if (usingButton && ((!overInputField && Input.GetMouseButtonDown(0)) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			OnInputFieldLoseFocus();
		}
	}

	public void OnInputFieldLoseFocus()
	{
		inputFieldButton.gameObject.SetActive(value: true);
		inputText.gameObject.SetActive(value: false);
		unitsText.gameObject.SetActive(value: false);
		UpdateNodeBodyUI();
		StartCoroutine(EnableSlider());
	}

	public IEnumerator EnableSlider()
	{
		yield return null;
		double num = Convert.ToDouble(inputText.text);
		if (clampTextInput)
		{
			if (num < minValue)
			{
				num = MinValue;
			}
			else if (num > maxValue)
			{
				num = MaxValue;
			}
			inputText.text = num.ToString();
		}
		slider.interactable = true;
		usingButton = false;
		if (FieldValue is int)
		{
			FieldValue = Convert.ToInt32(num / (double)displayMultiply);
		}
		else if (FieldValue is float)
		{
			FieldValue = Convert.ToSingle(num / (double)displayMultiply);
		}
		else if (FieldValue is double)
		{
			FieldValue = Convert.ToDouble(num / (double)displayMultiply);
		}
	}

	public void OnSliderValueChange(float newValue)
	{
		if (!usingButton)
		{
			if (!usingSlider)
			{
				MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
				usingSlider = true;
			}
			if (roundToPlaces > -1)
			{
				newValue = ((roundToPlaces != 0) ? (Mathf.Round(newValue * Mathf.Pow(10f, roundToPlaces)) / Mathf.Pow(10f, roundToPlaces)) : Mathf.Round(newValue));
			}
			if (FieldValue is int)
			{
				FieldValue = (int)newValue;
			}
			else if (FieldValue is float)
			{
				FieldValue = newValue;
			}
			else if (FieldValue is double)
			{
				FieldValue = (double)newValue;
			}
			UpdateNodeBodyUI();
		}
	}

	public override void ResetDefaultValue(string value)
	{
		if (FieldValue is int)
		{
			FieldValue = int.Parse(value);
		}
		else if (FieldValue is float)
		{
			FieldValue = float.Parse(value);
		}
		else if (FieldValue is double)
		{
			FieldValue = double.Parse(value);
		}
		else
		{
			Debug.LogErrorFormat("[MEGUIParameterNumberRange]: Acceptable Types are int, float and double, Field is ", FieldValue.GetType().Name);
		}
		UpdateNodeBodyUI();
	}

	public void UpdateValueText(object value)
	{
		if (!(valueText == null))
		{
			if (FieldValue is int)
			{
				valueText.text = ((float)Convert.ToInt32(value) * displayMultiply).ToString(displayFormat);
			}
			else if (FieldValue is float)
			{
				valueText.text = (Convert.ToSingle(value) * displayMultiply).ToString(displayFormat);
			}
			else if (FieldValue is double)
			{
				valueText.text = (Convert.ToDouble(value) * (double)displayMultiply).ToString(displayFormat);
			}
			else
			{
				Debug.LogErrorFormat("[MEGUIParameterNumberRange]: Acceptable Types are int, float and double, Field is ", FieldValue.GetType().Name);
			}
			inputText.text = valueText.text;
			valueText.text += unitsSuffix;
		}
	}

	public override void RefreshUI()
	{
		UpdateSlider(FieldValue);
		UpdateValueText(FieldValue);
		usingSlider = false;
	}

	public void UpdateSlider(object value)
	{
		if (value is int)
		{
			slider.value = (int)value;
			return;
		}
		if (value is float)
		{
			slider.value = (float)value;
			return;
		}
		if (value is double)
		{
			slider.value = (float)(double)value;
			return;
		}
		Debug.LogErrorFormat("[MEGUIParameterNumberRange]: Acceptable Types are int, float and double, value is ", value.GetType().Name);
	}

	public void Toggle(bool value)
	{
		base.gameObject.SetActive(value);
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		string value = null;
		if (data.TryGetValue("value", ref value))
		{
			object obj = null;
			if (FieldValue is int)
			{
				obj = int.Parse(value);
			}
			else if (FieldValue is float)
			{
				obj = float.Parse(value);
			}
			else
			{
				if (!(FieldValue is double))
				{
					Debug.LogErrorFormat("[MEGUIParameterNumberRange]: Acceptable Types are int, float and double, Field is ", FieldValue.GetType().Name);
					return;
				}
				obj = double.Parse(value);
			}
			field.SetValue(obj);
			UpdateSlider(obj);
			UpdateValueText(obj);
		}
		usingSlider = false;
	}
}
