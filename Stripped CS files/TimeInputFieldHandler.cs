using TMPro;
using UnityEngine;

public class TimeInputFieldHandler : InputFieldHandler
{
	public TimeInputFieldHandler(ManeuverNodeEditorTabVectorInput owner, TMP_InputField field, GetValueDelegate getValue, SetValueDelegate setValue)
		: base(owner, field, getValue, setValue)
	{
	}

	public override string GetFormattedValue(double timeValue, bool fullPrecision)
	{
		if (inputTab.UTMode)
		{
			string text = KSPUtil.PrintTimeStampCompact(timeValue, days: true, years: true);
			if (fullPrecision)
			{
				string text2 = ((int)timeValue).ToString();
				string text3 = timeValue.ToString("F3");
				text += text3.Substring(text2.Length);
			}
			return text;
		}
		return base.GetFormattedValue(timeValue, fullPrecision);
	}

	public bool ParseUT(string capturedUT, out double translatedUT)
	{
		translatedUT = 0.0;
		KSPUtil.DefaultDateTimeFormatter defaultDateTimeFormatter = new KSPUtil.DefaultDateTimeFormatter();
		string[] array = capturedUT.Split(':');
		if (array.Length <= 5)
		{
			int num = array.Length - 1;
			while (num >= 0)
			{
				if (double.TryParse(array[num], out var result))
				{
					if (num == array.Length - 1)
					{
						translatedUT += result;
					}
					else if (num == array.Length - 2)
					{
						translatedUT += result * (double)defaultDateTimeFormatter.Minute;
					}
					else if (num == array.Length - 3)
					{
						translatedUT += result * (double)defaultDateTimeFormatter.Hour;
					}
					else if (num == array.Length - 4)
					{
						translatedUT += result * (double)defaultDateTimeFormatter.Day;
					}
					else if (num == array.Length - 5)
					{
						translatedUT += result * (double)defaultDateTimeFormatter.Year;
					}
					num--;
					continue;
				}
				return false;
			}
		}
		return true;
	}

	public override bool ParseText(string text, out double timeValue)
	{
		if (inputTab.UTMode)
		{
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == ',')
				{
					num++;
				}
			}
			if (num > 1)
			{
				text = text.Replace(",", "");
			}
			else if (num == 1)
			{
				text = text.Replace(",", ".");
			}
			return ParseUT(text, out timeValue);
		}
		return double.TryParse(text, out timeValue);
	}

	public void ChangeInputMode(bool formattedTime)
	{
		if (formattedTime)
		{
			inputField.contentType = TMP_InputField.ContentType.Custom;
			inputField.lineType = TMP_InputField.LineType.SingleLine;
			inputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
			inputField.characterValidation = TMP_InputField.CharacterValidation.None;
		}
		else
		{
			inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
		}
		UpdateField();
	}
}
