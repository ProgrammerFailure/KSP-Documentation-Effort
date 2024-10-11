using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EdyCommonTools;

public class MiniJSON
{
	private const int TOKEN_NONE = 0;

	private const int TOKEN_CURLY_OPEN = 1;

	private const int TOKEN_CURLY_CLOSE = 2;

	private const int TOKEN_SQUARED_OPEN = 3;

	private const int TOKEN_SQUARED_CLOSE = 4;

	private const int TOKEN_COLON = 5;

	private const int TOKEN_COMMA = 6;

	private const int TOKEN_STRING = 7;

	private const int TOKEN_NUMBER = 8;

	private const int TOKEN_TRUE = 9;

	private const int TOKEN_FALSE = 10;

	private const int TOKEN_NULL = 11;

	private const int BUILDER_CAPACITY = 2000;

	protected static int lastErrorIndex;

	protected static string lastDecode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MiniJSON()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MiniJSON()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static object jsonDecode(string json)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string jsonEncode(object json)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool lastDecodeSuccessful()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int getLastErrorIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string getLastErrorSnippet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static Hashtable parseObject(char[] json, ref int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static ArrayList parseArray(char[] json, ref int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static object parseValue(char[] json, ref int index, ref bool success)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static string parseString(char[] json, ref int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static double parseNumber(char[] json, ref int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static int getLastIndexOfNumber(char[] json, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void eatWhitespace(char[] json, ref int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static int lookAhead(char[] json, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static int nextToken(char[] json, ref int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool serializeObjectOrArray(object objectOrArray, StringBuilder builder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool serializeObject(Hashtable anObject, StringBuilder builder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool serializeDictionary(Dictionary<string, string> dict, StringBuilder builder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool serializeArray(ArrayList anArray, StringBuilder builder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool serializeValue(object value, StringBuilder builder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void serializeString(string aString, StringBuilder builder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void serializeNumber(double number, StringBuilder builder)
	{
		throw null;
	}
}
