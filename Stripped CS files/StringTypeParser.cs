public class StringTypeParser : ITypeParser
{
	public object Parse(string s)
	{
		return s;
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
