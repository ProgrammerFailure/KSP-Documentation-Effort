public static class ArrayExtensions
{
	public static int IndexOf<T>(this T[] array, T value)
	{
		int num = 0;
		int num2 = array.Length;
		while (true)
		{
			if (num < num2)
			{
				if (array[num].Equals(value))
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}
}
