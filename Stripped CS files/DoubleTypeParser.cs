public class DoubleTypeParser : ITypeParser
{
	public object Parse(string s)
	{
		if (double.TryParse(s, out var result))
		{
			return result;
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
