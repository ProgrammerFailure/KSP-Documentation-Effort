using System;
using UnityEngine;

public class CurrencyModifierQuery
{
	public enum TextStyling
	{
		None,
		OnGUI,
		EzGUIRichText,
		OnGUI_LessIsGood,
		EzGUIRichText_LessIsGood
	}

	public TransactionReasons reason;

	public float inputFunds;

	public float inputScience;

	public float inputRep;

	public float deltaFunds;

	public float deltaScience;

	public float deltaRep;

	public CurrencyModifierQuery(TransactionReasons reason, float f0, float s0, float r0)
	{
		this.reason = reason;
		inputFunds = f0;
		inputScience = s0;
		inputRep = r0;
		deltaFunds = 0f;
		deltaScience = 0f;
		deltaRep = 0f;
	}

	public float GetInput(Currency c)
	{
		return c switch
		{
			Currency.Funds => inputFunds, 
			Currency.Science => inputScience, 
			Currency.Reputation => inputRep, 
			_ => 0f, 
		};
	}

	public void AddDelta(Currency c, float val)
	{
		switch (c)
		{
		case Currency.Funds:
			deltaFunds += val;
			break;
		case Currency.Science:
			deltaScience += val;
			break;
		case Currency.Reputation:
			deltaRep += val;
			break;
		}
	}

	public float GetEffectDelta(Currency c)
	{
		return c switch
		{
			Currency.Funds => deltaFunds, 
			Currency.Science => deltaScience, 
			Currency.Reputation => deltaRep, 
			_ => 0f, 
		};
	}

	public float GetTotal(Currency c)
	{
		return GetInput(c) + GetEffectDelta(c);
	}

	public string GetCostLine(bool displayInverted = true, bool useCurrencyColors = false, bool useInsufficientCurrencyColors = true, bool includePercentage = false, string seperator = ", ")
	{
		float num = GetTotal(Currency.Funds);
		float num2 = GetTotal(Currency.Science);
		float num3 = GetTotal(Currency.Reputation);
		if (displayInverted)
		{
			num = 0f - num;
			num2 = 0f - num2;
			num3 = 0f - num3;
		}
		bool flag = CanAfford(Currency.Funds);
		bool flag2 = CanAfford(Currency.Science);
		bool flag3 = CanAfford(Currency.Reputation);
		string text = ((!Mathf.Approximately(num, 0f)) ? ("<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1> " + KSPUtil.LocalizeNumber(num, "N2")) : "");
		string text2 = ((!Mathf.Approximately(num2, 0f)) ? ("<sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + KSPUtil.LocalizeNumber(num2, "N0")) : "");
		string text3 = ((!Mathf.Approximately(num3, 0f)) ? ("<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + KSPUtil.LocalizeNumber(num3, "F2")) : "");
		string text4 = "";
		if (!string.IsNullOrEmpty(text))
		{
			text4 = ((!((useInsufficientCurrencyColors && !flag) || useCurrencyColors)) ? text : StringBuilderCache.Format("<color={0}>{1}</color>", (!useInsufficientCurrencyColors || flag) ? "#B4D455" : XKCDColors.HexFormat.BrightOrange, text));
			if (includePercentage && deltaFunds != 0f)
			{
				text4 = text4 + " " + GetEffectPercentageText(Currency.Funds, "N1", TextStyling.OnGUI_LessIsGood);
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			if (!string.IsNullOrEmpty(text4))
			{
				text4 += seperator;
			}
			text4 = ((!((useInsufficientCurrencyColors && !flag2) || useCurrencyColors)) ? (text4 + text2) : (text4 + StringBuilderCache.Format("<color={0}>{1}</color>", (!useInsufficientCurrencyColors || flag2) ? "#6DCFF6" : XKCDColors.HexFormat.BrightOrange, text2)));
			if (includePercentage && deltaScience != 0f)
			{
				text4 = text4 + " " + GetEffectPercentageText(Currency.Science, "N1", TextStyling.OnGUI_LessIsGood);
			}
		}
		if (!string.IsNullOrEmpty(text3))
		{
			if (!string.IsNullOrEmpty(text4))
			{
				text4 += seperator;
			}
			text4 = ((!((useInsufficientCurrencyColors && !flag3) || useCurrencyColors)) ? (text4 + text3) : (text4 + StringBuilderCache.Format("<color={0}>{1}</color>", (!useInsufficientCurrencyColors || flag3) ? "#E0D503" : XKCDColors.HexFormat.BrightOrange, text3)));
			if (includePercentage && deltaRep != 0f)
			{
				text4 = text4 + " " + GetEffectPercentageText(Currency.Reputation, "N1", TextStyling.OnGUI_LessIsGood);
			}
		}
		return text4;
	}

	public bool CanAfford(Action<Currency> onInsufficientCurrency = null)
	{
		bool flag2;
		bool flag;
		bool flag3 = (flag2 = (flag = CanAfford(Currency.Funds)) && CanAfford(Currency.Science)) && CanAfford(Currency.Reputation);
		if (onInsufficientCurrency != null && !flag3)
		{
			if (!flag)
			{
				onInsufficientCurrency(Currency.Funds);
			}
			else if (!flag2)
			{
				onInsufficientCurrency(Currency.Science);
			}
			else if (!flag3)
			{
				onInsufficientCurrency(Currency.Reputation);
			}
		}
		return flag3;
	}

	public bool CanAfford(Currency c)
	{
		float num = 0f - (GetInput(c) + GetEffectDelta(c));
		if (Mathf.Approximately(num, 0f))
		{
			return true;
		}
		switch (c)
		{
		default:
			return true;
		case Currency.Funds:
			if (!(Funding.Instance == null))
			{
				return (double)num <= Funding.Instance.Funds;
			}
			return true;
		case Currency.Science:
			if (!(ResearchAndDevelopment.Instance == null))
			{
				return UtilMath.RoundToPlaces(ResearchAndDevelopment.Instance.Science, 1) >= UtilMath.RoundToPlaces(num, 1);
			}
			return true;
		case Currency.Reputation:
			return true;
		}
	}

	public string GetEffectDeltaText(Currency c, string format, TextStyling textStyle = TextStyling.None)
	{
		string text = "";
		float num = 0f;
		switch (c)
		{
		case Currency.Funds:
			num = deltaFunds;
			break;
		case Currency.Science:
			num = deltaScience;
			break;
		case Currency.Reputation:
			num = deltaRep;
			break;
		}
		if (num == 0f)
		{
			return "";
		}
		text = num.ToString(format);
		return textStyle switch
		{
			TextStyling.OnGUI => ((num > 0f) ? "<color=#caff00>(+" : "<color=#feb200>(") + text + ")</color>", 
			TextStyling.EzGUIRichText => ((num > 0f) ? "<#caff00>(+" : "<#feb200>(") + text + ")</>", 
			TextStyling.OnGUI_LessIsGood => ((num > 0f) ? "<color=#feb200>(+" : "<color=#caff00>(") + text + ")</color>", 
			TextStyling.EzGUIRichText_LessIsGood => ((num > 0f) ? "<#feb200>(+" : "<#caff00>(") + text + ")</>", 
			_ => ((num > 0f) ? "(+" : "(") + text + ")", 
		};
	}

	public string GetEffectPercentageText(Currency c, string format, TextStyling textStyle = TextStyling.None)
	{
		string text = "";
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		switch (c)
		{
		case Currency.Funds:
			num = deltaFunds;
			num2 = inputFunds;
			num3 = deltaFunds / inputFunds * 100f;
			break;
		case Currency.Science:
			num = deltaScience;
			num2 = inputScience;
			num3 = deltaScience / inputScience * 100f;
			break;
		case Currency.Reputation:
			num = deltaRep;
			num2 = inputRep;
			num3 = deltaRep / inputRep * 100f;
			break;
		}
		if (num != 0f && num2 != 0f)
		{
			text = num3.ToString(format);
			return textStyle switch
			{
				TextStyling.OnGUI => ((num > 0f) ? "<color=#caff00>(+" : "<color=#feb200>(") + text + "%)</color>", 
				TextStyling.EzGUIRichText => ((num > 0f) ? "<#caff00>(+" : "<#feb200>(") + text + "%)</>", 
				TextStyling.OnGUI_LessIsGood => ((num > 0f) ? "<color=#feb200><+" : "<color=#caff00><") + text + "%></color>", 
				TextStyling.EzGUIRichText_LessIsGood => ((num > 0f) ? "<#feb200>(+" : "<#caff00>(") + text + ")%</>", 
				_ => ((num > 0f) ? "(+" : "(") + text + "%)", 
			};
		}
		return "";
	}

	public static CurrencyModifierQuery RunQuery(TransactionReasons reason, float f0, float s0, float r0)
	{
		CurrencyModifierQuery currencyModifierQuery = new CurrencyModifierQuery(reason, f0, s0, r0);
		GameEvents.Modifiers.OnCurrencyModifierQuery.Fire(currencyModifierQuery);
		return currencyModifierQuery;
	}
}
