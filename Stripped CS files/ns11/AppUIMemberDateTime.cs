using System;
using ns2;
using TMPro;
using UnityEngine;

namespace ns11;

[AppUI_InputDateTime]
public class AppUIMemberDateTime : AppUIMember
{
	public enum DisplayModes
	{
		datetime,
		ut
	}

	public enum DateTimeModes
	{
		date,
		timespan
	}

	public TMP_InputField yInput;

	public TMP_InputField dInput;

	public TMP_InputField hInput;

	public TMP_InputField mInput;

	public TMP_InputField sInput;

	public TMP_InputField utInput;

	public GameObject dateTimeComponents;

	public GameObject utComponents;

	public ITypeParser parser;

	public DisplayModes displayMode;

	public DateTimeModes datetimeMode = DateTimeModes.timespan;

	public IDateTimeFormatter dateFormatter;

	public int year;

	public int day;

	public int hour;

	public int min;

	public int sec;

	public DisplayModes DisplayMode
	{
		get
		{
			return displayMode;
		}
		set
		{
			if (displayMode != value)
			{
				SetMode(value);
				displayMode = value;
			}
		}
	}

	public DateTimeModes DatetimeMode
	{
		get
		{
			return datetimeMode;
		}
		set
		{
			if (datetimeMode != value)
			{
				datetimeMode = value;
				RefreshUI();
			}
		}
	}

	public IDateTimeFormatter DateFormatter
	{
		get
		{
			return dateFormatter;
		}
		set
		{
			dateFormatter = value;
		}
	}

	public override void OnStart()
	{
		yInput.onEndEdit.AddListener(InputEdited);
		dInput.onEndEdit.AddListener(InputEdited);
		hInput.onEndEdit.AddListener(InputEdited);
		mInput.onEndEdit.AddListener(InputEdited);
		sInput.onEndEdit.AddListener(InputEdited);
		utInput.onEndEdit.AddListener(InputEdited);
	}

	public void OnDestroy()
	{
		yInput.onEndEdit.RemoveListener(InputEdited);
		dInput.onEndEdit.RemoveListener(InputEdited);
		hInput.onEndEdit.RemoveListener(InputEdited);
		mInput.onEndEdit.RemoveListener(InputEdited);
		sInput.onEndEdit.RemoveListener(InputEdited);
		utInput.onEndEdit.RemoveListener(InputEdited);
	}

	public override void OnInitialized()
	{
		if (_attribs is AppUI_InputDateTime appUI_InputDateTime)
		{
			datetimeMode = appUI_InputDateTime.datetimeMode;
			DisplayMode = appUI_InputDateTime.displayMode;
		}
		if (valueType != typeof(double))
		{
			Debug.LogError("[AppUIMemberDateTime]: Error - Cannot bind field as it is not a double - " + valueType.Name);
		}
		parser = new DoubleTypeParser();
		dateFormatter = KSPUtil.dateTimeFormatter;
	}

	public override void OnRefreshUI()
	{
		if (datetimeMode == DateTimeModes.date)
		{
			dateFormatter.GetDateComponents((double)GetValue(), out year, out day, out hour, out min, out sec);
		}
		else
		{
			dateFormatter.GetTimeComponents((double)GetValue(), out year, out day, out hour, out min, out sec);
		}
		yInput.text = year.ToString("0");
		dInput.text = day.ToString("0");
		hInput.text = hour.ToString("00");
		mInput.text = min.ToString("00");
		sInput.text = sec.ToString("00");
		utInput.text = string.Format("0", GetValue());
	}

	public override void OnUpdate()
	{
		if (!AnyTextFieldHasFocus() || !Input.GetKeyDown(KeyCode.Tab))
		{
			return;
		}
		if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
		{
			if (yInput.isFocused)
			{
				dInput.ActivateInputField();
			}
			else if (dInput.isFocused)
			{
				hInput.ActivateInputField();
			}
			else if (hInput.isFocused)
			{
				mInput.ActivateInputField();
			}
			else if (mInput.isFocused)
			{
				sInput.ActivateInputField();
			}
			else if (sInput.isFocused)
			{
				yInput.ActivateInputField();
			}
		}
		else if (yInput.isFocused)
		{
			sInput.ActivateInputField();
		}
		else if (dInput.isFocused)
		{
			yInput.ActivateInputField();
		}
		else if (hInput.isFocused)
		{
			dInput.ActivateInputField();
		}
		else if (mInput.isFocused)
		{
			hInput.ActivateInputField();
		}
		else if (sInput.isFocused)
		{
			mInput.ActivateInputField();
		}
	}

	public void SetMode(DisplayModes newMode)
	{
		displayMode = newMode;
		dateTimeComponents.SetActive(newMode == DisplayModes.datetime);
		utComponents.SetActive(newMode == DisplayModes.ut);
		RefreshUI();
	}

	public void InputEdited(string value)
	{
		double num = 0.0;
		switch (displayMode)
		{
		default:
			if (string.IsNullOrEmpty(utInput.text))
			{
				utInput.text = "0";
			}
			num = (double)parser.Parse(utInput.text);
			break;
		case DisplayModes.datetime:
		{
			if (string.IsNullOrEmpty(yInput.text))
			{
				yInput.text = "0";
			}
			if (string.IsNullOrEmpty(dInput.text))
			{
				dInput.text = "0";
			}
			if (string.IsNullOrEmpty(hInput.text))
			{
				hInput.text = "0";
			}
			if (string.IsNullOrEmpty(mInput.text))
			{
				mInput.text = "0";
			}
			if (string.IsNullOrEmpty(sInput.text))
			{
				sInput.text = "0";
			}
			double num2 = (double)parser.Parse(yInput.text);
			double num3 = (double)parser.Parse(dInput.text);
			if (datetimeMode == DateTimeModes.date)
			{
				num2 = Math.Max(num2, 1.0);
				num3 = Math.Max(num3, 1.0);
			}
			num = (double)parser.Parse(sInput.text);
			num += (double)parser.Parse(mInput.text) * (double)dateFormatter.Minute;
			num += (double)parser.Parse(hInput.text) * (double)dateFormatter.Hour;
			num += num3 * (double)dateFormatter.Day;
			num += num2 * (double)dateFormatter.Year;
			if (datetimeMode == DateTimeModes.date)
			{
				num -= (double)(dateFormatter.Year + dateFormatter.Day);
			}
			break;
		}
		}
		num = Math.Max(num, 0.0);
		SetValue(num);
		RefreshUI();
	}

	public override bool AnyTextFieldHasFocus()
	{
		if (!yInput.isFocused && !dInput.isFocused && !hInput.isFocused && !mInput.isFocused)
		{
			return sInput.isFocused;
		}
		return true;
	}
}
