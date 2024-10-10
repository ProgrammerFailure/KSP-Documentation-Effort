using System;
using System.Text;

public static class StringBuilderCache
{
	public const int MAX_BUILDER_SIZE = 360;

	[ThreadStatic]
	public static StringBuilder CachedInstance;

	public static StringBuilder Acquire(int capacity = 256)
	{
		if (capacity <= 360)
		{
			StringBuilder cachedInstance = CachedInstance;
			if (cachedInstance != null && capacity <= cachedInstance.Capacity)
			{
				CachedInstance = null;
				cachedInstance.Length = 0;
				return cachedInstance;
			}
		}
		return new StringBuilder(capacity);
	}

	public static void Release(this StringBuilder sb)
	{
		if (sb.Capacity <= 360)
		{
			CachedInstance = sb;
		}
	}

	public static string ToStringAndRelease(this StringBuilder sb)
	{
		string result = sb.ToString();
		sb.Release();
		return result;
	}

	public static string Format(string format, params object[] args)
	{
		StringBuilder stringBuilder = Acquire(format.Length + 8);
		stringBuilder.AppendFormat(format, args);
		string result = stringBuilder.ToString();
		stringBuilder.Release();
		return result;
	}
}
