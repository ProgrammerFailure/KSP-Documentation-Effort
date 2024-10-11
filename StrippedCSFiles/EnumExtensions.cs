using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class EnumExtensions
{
	internal static class EnumValueNames
	{
		private static Dictionary<Type, Dictionary<int, string>> enumValues;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static EnumValueNames()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static string GetName(Type type, Enum value)
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string Description(this Enum e)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string displayDescription(this Enum e)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetDescriptions<TEnum>(TEnum value) where TEnum : struct, IConvertible
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ToStringCached(this Enum e)
	{
		throw null;
	}
}
