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
