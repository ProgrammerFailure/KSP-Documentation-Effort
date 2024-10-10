using System;

public class KnowledgeItem<T>
{
	public delegate T UpdateDataCallback();

	public string caption;

	public string valueFallback;

	public T value;

	public decimal decimalValue;

	public string suffix;

	public UpdateDataCallback updateValue;

	public DiscoveryLevels ItemLevel;

	public DiscoveryInfo host;

	public string formatString;

	public string Caption => caption;

	public string Value
	{
		get
		{
			if (host.HaveKnowledgeAbout(ItemLevel))
			{
				value = updateValue();
				if (value is IFormattable)
				{
					return ((IFormattable)(object)value).ToString(formatString, null) + suffix;
				}
				return value.ToString() + suffix;
			}
			return valueFallback;
		}
	}

	public string OneLiner
	{
		get
		{
			if (host.HaveKnowledgeAbout(ItemLevel))
			{
				value = updateValue();
				if (value is IFormattable)
				{
					return caption + ((IFormattable)(object)value).ToString(formatString, null) + suffix;
				}
				return caption + value.ToString() + suffix;
			}
			return valueFallback;
		}
	}

	public KnowledgeItem(DiscoveryInfo host, DiscoveryLevels itemLevel, string caption, string valueFallback, UpdateDataCallback updateValueCallback, string suffix = "", string format = "")
	{
		this.host = host;
		ItemLevel = itemLevel;
		this.caption = caption;
		this.valueFallback = valueFallback;
		updateValue = updateValueCallback;
		this.suffix = suffix;
		formatString = format;
	}

	public string GetValue(string noValueFallback)
	{
		if (host.HaveKnowledgeAbout(ItemLevel))
		{
			value = updateValue();
			if (value is IFormattable)
			{
				return ((IFormattable)(object)value).ToString(formatString, null) + suffix;
			}
			return value.ToString() + suffix;
		}
		return noValueFallback;
	}
}
