namespace ns15;

public static class StringExtensions
{
	public static string Translate(this string baseString, params object[] args)
	{
		if (Language.Instance == null)
		{
			return baseString;
		}
		return Language.Instance.TranslateWithFormat(baseString, args);
	}
}
