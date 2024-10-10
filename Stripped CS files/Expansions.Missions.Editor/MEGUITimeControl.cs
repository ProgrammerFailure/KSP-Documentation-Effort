using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUITimeControl : MonoBehaviour
{
	[Serializable]
	public class TimeControlValueChange : UnityEvent<double>
	{
	}

	public Button timeButton;

	public TextMeshProUGUI timeButtonText;

	public TMP_InputField timeYears;

	public TMP_InputField timeDays;

	public TMP_InputField timeHours;

	public TMP_InputField timeMins;

	public TMP_InputField timeSecs;

	public GameObject timeEntrySection;

	[SerializeField]
	public LayoutElement paramLayoutElement;

	public float initialParamheight;

	public bool isDate;

	public KSPUtil.DefaultDateTimeFormatter dateFormatter;

	public double years;

	public double days;

	public double hours;

	public double minutes;

	public double seconds;

	[SerializeField]
	public TimeControlValueChange onValueChange;

	public double time { get; set; }

	public void Awake()
	{
		dateFormatter = new KSPUtil.DefaultDateTimeFormatter();
		timeButtonText = timeButton.GetComponentInChildren<TextMeshProUGUI>();
	}

	public void Start()
	{
		UpdateValues();
		timeEntrySection.gameObject.SetActive(value: false);
		if (paramLayoutElement != null)
		{
			initialParamheight = paramLayoutElement.minHeight;
			paramLayoutElement.minHeight = initialParamheight / 2f;
		}
		timeButton.onClick.AddListener(EditTime);
		timeYears.onEndEdit.AddListener(UpdateTimeFromInput);
		timeDays.onEndEdit.AddListener(UpdateTimeFromInput);
		timeHours.onEndEdit.AddListener(UpdateTimeFromInput);
		timeMins.onEndEdit.AddListener(UpdateTimeFromInput);
		timeSecs.onEndEdit.AddListener(UpdateTimeFromInput);
	}

	public void UpdateValues()
	{
		UpdateButtonText();
		UpdateTextValues();
	}

	public void UpdateButtonText()
	{
		if (timeButtonText != null)
		{
			if (!isDate)
			{
				timeButtonText.text = dateFormatter.PrintTimeStampCompact(time, days: true, years: true);
			}
			else
			{
				timeButtonText.text = KSPUtil.PrintDateCompact(time, includeTime: true, includeSeconds: true);
			}
		}
	}

	public void UpdateTextValues()
	{
		if (!isDate)
		{
			double num = time;
			years = Math.Floor(num / (double)dateFormatter.Year);
			num -= years * (double)dateFormatter.Year;
			days = Math.Floor(num / (double)dateFormatter.Day);
			num -= days * (double)dateFormatter.Day;
			hours = Math.Floor(num / (double)dateFormatter.Hour);
			num -= hours * (double)dateFormatter.Hour;
			minutes = Math.Floor(num / (double)dateFormatter.Minute);
			num -= minutes * (double)dateFormatter.Minute;
			seconds = num;
			timeYears.text = years.ToString();
			timeDays.text = days.ToString();
			timeHours.text = hours.ToString();
			timeMins.text = minutes.ToString();
			timeSecs.text = seconds.ToString();
		}
		else
		{
			timeYears.text = dateFormatter.GetUTYear().ToString();
			timeDays.text = dateFormatter.GetUTDay().ToString();
			timeHours.text = dateFormatter.GetUTHour().ToString();
			timeMins.text = dateFormatter.GetUTMinute().ToString();
			timeSecs.text = dateFormatter.GetUTSecond().ToString();
		}
	}

	public void EditTime()
	{
		if (!timeEntrySection.gameObject.activeSelf)
		{
			UpdateTextValues();
		}
		timeEntrySection.gameObject.SetActive(!timeEntrySection.gameObject.activeSelf);
		if (paramLayoutElement != null)
		{
			paramLayoutElement.minHeight = (timeEntrySection.gameObject.activeSelf ? initialParamheight : (initialParamheight / 2f));
		}
	}

	public void UpdateTimeFromInput(string value)
	{
		if (!double.TryParse(timeYears.text, out years))
		{
			years = 0.0;
		}
		if (!double.TryParse(timeDays.text, out days))
		{
			days = 0.0;
		}
		if (!double.TryParse(timeHours.text, out hours))
		{
			hours = 0.0;
		}
		if (!double.TryParse(timeMins.text, out minutes))
		{
			minutes = 0.0;
		}
		if (!double.TryParse(timeSecs.text, out seconds))
		{
			seconds = 0.0;
		}
		try
		{
			if (isDate)
			{
				if (years > 0.0)
				{
					years -= 1.0;
				}
				if (days > 0.0)
				{
					days -= 1.0;
				}
			}
			time = seconds + minutes * (double)dateFormatter.Minute + hours * (double)dateFormatter.Hour + days * (double)dateFormatter.Day + years * (double)dateFormatter.Year;
			if (time > 2000000000000000.0)
			{
				time = 2000000000000000.0;
			}
			else if (time < -2E+150)
			{
				time = -2000000000000000.0;
			}
			onValueChange.Invoke(time);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("[MEGUIParameterTime] Unable to update value from text boxes. y='{0}',d='{1}',h='{2}',m='{3}',s='{4}'\n{5}", timeYears.text, timeDays.text, timeHours.text, timeMins.text, timeSecs.text, ex.Message);
		}
		if (isDate)
		{
			UpdateValues();
		}
		else
		{
			UpdateButtonText();
		}
	}
}
