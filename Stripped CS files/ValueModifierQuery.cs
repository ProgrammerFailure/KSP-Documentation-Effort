public class ValueModifierQuery
{
	public enum TextStyling
	{
		None,
		OnGUI,
		EzGUIRichText,
		OnGUI_LessIsGood,
		EzGUIRichText_LessIsGood
	}

	public string valueId;

	public float input;

	public float delta;

	public string ValueId => valueId;

	public ValueModifierQuery(string valueId, float v0)
	{
		this.valueId = valueId;
		input = v0;
		delta = 0f;
	}

	public float GetInput()
	{
		return input;
	}

	public void AddDelta(float val)
	{
		delta += val;
	}

	public float GetEffectDelta()
	{
		return delta;
	}

	public string GetEffectDeltaText(string format, TextStyling textStyle = TextStyling.None)
	{
		string text = "";
		if (delta == 0f)
		{
			return "";
		}
		text = delta.ToString(format);
		return textStyle switch
		{
			TextStyling.OnGUI => ((delta > 0f) ? "<color=#caff00>(+" : "<color=#feb200>(") + text + ")</color>", 
			TextStyling.EzGUIRichText => ((delta > 0f) ? "<#caff00>(+" : "<#feb200>(") + text + ")</>", 
			TextStyling.OnGUI_LessIsGood => ((delta > 0f) ? "<color=#feb200>(+" : "<color=#caff00>(") + text + ")</color>", 
			TextStyling.EzGUIRichText_LessIsGood => ((delta > 0f) ? "<#feb200>(+" : "<#caff00>(") + text + ")</>", 
			_ => ((delta > 0f) ? "(+" : "(") + text + ")", 
		};
	}

	public string GetEffectPercentageText(string format, TextStyling textStyle = TextStyling.None)
	{
		string text = "";
		if (delta == 0f)
		{
			return "";
		}
		text = (delta / input * 100f).ToString(format);
		return textStyle switch
		{
			TextStyling.OnGUI => ((delta > 0f) ? "<color=#caff00>(+" : "<color=#feb200>(") + text + "%)</color>", 
			TextStyling.EzGUIRichText => ((delta > 0f) ? "<#caff00>(+" : "<#feb200>(") + text + "%)</>", 
			TextStyling.OnGUI_LessIsGood => ((delta > 0f) ? "<color=#feb200>(+" : "<color=#caff00>(") + text + "%)</color>", 
			TextStyling.EzGUIRichText_LessIsGood => ((delta > 0f) ? "<#feb200>(+" : "<#caff00>(") + text + ")%</>", 
			_ => ((delta > 0f) ? "(+" : "(") + text + "%)", 
		};
	}

	public static ValueModifierQuery RunQuery(string valueId, float input)
	{
		ValueModifierQuery valueModifierQuery = new ValueModifierQuery(valueId, input);
		GameEvents.Modifiers.onValueModifierQuery.Fire(valueModifierQuery);
		return valueModifierQuery;
	}
}
