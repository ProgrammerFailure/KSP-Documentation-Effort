using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ns9;

public static class EnumExtensions
{
	public static class EnumValueNames
	{
		public static Dictionary<Type, Dictionary<int, string>> enumValues;

		static EnumValueNames()
		{
			enumValues = new Dictionary<Type, Dictionary<int, string>>();
		}

		public static string GetName(Type type, Enum value)
		{
			int key = (int)(object)value;
			if (!enumValues.ContainsKey(type))
			{
				Dictionary<int, string> dictionary = new Dictionary<int, string>();
				_ = (int[])Enum.GetValues(type);
				foreach (Enum value3 in Enum.GetValues(type))
				{
					int key2 = ((IConvertible)value3).ToInt32((IFormatProvider)CultureInfo.InvariantCulture);
					if (!dictionary.ContainsKey(key2))
					{
						dictionary.Add(key2, value3.ToString());
					}
				}
				enumValues.Add(type, dictionary);
			}
			if (!enumValues[type].TryGetValue(key, out var value2))
			{
				value2 = value.ToString();
				enumValues[type].Add(key, value2);
			}
			return value2;
		}
	}

	public static string Description(this Enum e)
	{
		DescriptionAttribute[] array = (DescriptionAttribute[])e.GetType().GetMember(e.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
		if (array.Length != 0)
		{
			return array[0].Description;
		}
		return e.ToString();
	}

	public static string displayDescription(this Enum e)
	{
		return Localizer.Format(e.Description());
	}

	public static List<string> GetDescriptions<TEnum>(TEnum value) where TEnum : struct, IConvertible
	{
		List<string> list = new List<string>();
		string[] names = Enum.GetNames(typeof(TEnum));
		for (int i = 0; i < names.Length; i++)
		{
			Enum e = (Enum)Enum.Parse(typeof(TEnum), names[i]);
			list.Add(e.Description());
		}
		return list;
	}

	public static string ToStringCached(this Enum e)
	{
		return EnumValueNames.GetName(e.GetType(), e);
	}
}
