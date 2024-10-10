public static class StringExtensions
{
	public static int GetHashCode_Net35(this string str)
	{
		int i = 0;
		int num = 0;
		for (; i < str.Length - 1; i += 2)
		{
			int num2 = (num << 5) - num + str[i];
			num = (num2 << 5) - num2 + str[i + 1];
		}
		if (i < str.Length)
		{
			num = (num << 5) - num + str[i];
		}
		return num;
	}
}
