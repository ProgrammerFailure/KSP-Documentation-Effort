using System;
using UnityEngine;

public static class ParseExtensions
{
	public static readonly char[] comma = new char[1] { ',' };

	public static T[] ArrayMinSize<T>(int minSize, T[] array)
	{
		if (array == null)
		{
			return null;
		}
		if (array.Length >= minSize)
		{
			return array;
		}
		T[] array2 = new T[minSize];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = array[i];
		}
		return array2;
	}

	public static string[] ParseArray(string text, params char[] separators)
	{
		return ParseArray(text, StringSplitOptions.RemoveEmptyEntries, separators);
	}

	public static string[] ParseArray(string text, StringSplitOptions options, params char[] separators)
	{
		if (string.IsNullOrEmpty(text))
		{
			return new string[0];
		}
		if (separators.Length == 0)
		{
			separators = comma;
		}
		string[] array = text.Split(separators, options);
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i] = array[i].Trim();
		}
		return array;
	}

	public static bool TryParseBoolArray(string text, out bool[] flags)
	{
		string[] array = ParseArray(text);
		flags = new bool[array.Length];
		int num = 0;
		while (true)
		{
			if (num < flags.Length)
			{
				if (!bool.TryParse(array[num], out flags[num]))
				{
					break;
				}
				num++;
				continue;
			}
			return true;
		}
		return false;
	}

	public static bool TryParseEnumIntArray<T>(string text, out T[] flags)
	{
		string[] array = ParseArray(text);
		flags = new T[array.Length];
		for (int i = 0; i < flags.Length; i++)
		{
			try
			{
				flags[i] = (T)Enum.Parse(typeof(T), array[i]);
			}
			catch (ArgumentException)
			{
				return false;
			}
		}
		return true;
	}

	public static bool TryParseVector3(string text, out Vector3 result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 3)
		{
			result = Vector3.zero;
			return false;
		}
		if (!float.TryParse(array[0], out var result2))
		{
			result = Vector3.zero;
			return false;
		}
		if (!float.TryParse(array[1], out var result3))
		{
			result = Vector3.zero;
			return false;
		}
		if (!float.TryParse(array[2], out var result4))
		{
			result = Vector3.zero;
			return false;
		}
		result = new Vector3(result2, result3, result4);
		return true;
	}

	public static bool TryParseVector3d(string text, out Vector3d result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 3)
		{
			result = Vector3d.zero;
			return false;
		}
		if (!double.TryParse(array[0], out var result2))
		{
			result = Vector3d.zero;
			return false;
		}
		if (!double.TryParse(array[1], out var result3))
		{
			result = Vector3d.zero;
			return false;
		}
		if (!double.TryParse(array[2], out var result4))
		{
			result = Vector3d.zero;
			return false;
		}
		result = new Vector3d(result2, result3, result4);
		return true;
	}

	public static bool TryParseVector2(string text, out Vector2 result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 2)
		{
			result = Vector2.zero;
			return false;
		}
		if (!float.TryParse(array[0], out var result2))
		{
			result = Vector2.zero;
			return false;
		}
		if (!float.TryParse(array[1], out var result3))
		{
			result = Vector2.zero;
			return false;
		}
		result = new Vector2(result2, result3);
		return true;
	}

	public static bool TryParseVector2d(string text, out Vector2d result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 2)
		{
			result = Vector2d.zero;
			return false;
		}
		if (!double.TryParse(array[0], out var result2))
		{
			result = Vector2d.zero;
			return false;
		}
		if (!double.TryParse(array[1], out var result3))
		{
			result = Vector2d.zero;
			return false;
		}
		result = new Vector2d(result2, result3);
		return true;
	}

	public static bool TryParseVector4(string text, out Vector4 result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 4)
		{
			result = Vector4.zero;
			return false;
		}
		if (!float.TryParse(array[0], out var result2))
		{
			result = Vector4.zero;
			return false;
		}
		if (!float.TryParse(array[1], out var result3))
		{
			result = Vector4.zero;
			return false;
		}
		if (!float.TryParse(array[2], out var result4))
		{
			result = Vector4.zero;
			return false;
		}
		if (!float.TryParse(array[3], out var result5))
		{
			result = Vector4.zero;
			return false;
		}
		result = new Vector4(result2, result3, result4, result5);
		return true;
	}

	public static bool TryParseVector4d(string text, out Vector4d result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 4)
		{
			result = Vector4d.zero;
			return false;
		}
		if (!double.TryParse(array[0], out var result2))
		{
			result = Vector4d.zero;
			return false;
		}
		if (!double.TryParse(array[1], out var result3))
		{
			result = Vector4d.zero;
			return false;
		}
		if (!double.TryParse(array[2], out var result4))
		{
			result = Vector4d.zero;
			return false;
		}
		if (!double.TryParse(array[3], out var result5))
		{
			result = Vector4d.zero;
			return false;
		}
		result = new Vector4d(result2, result3, result4, result5);
		return true;
	}

	public static bool TryParseQuaternion(string text, out Quaternion result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 4)
		{
			result = Quaternion.identity;
			return false;
		}
		if (!float.TryParse(array[0], out var result2))
		{
			result = Quaternion.identity;
			return false;
		}
		if (!float.TryParse(array[1], out var result3))
		{
			result = Quaternion.identity;
			return false;
		}
		if (!float.TryParse(array[2], out var result4))
		{
			result = Quaternion.identity;
			return false;
		}
		if (!float.TryParse(array[3], out var result5))
		{
			result = Quaternion.identity;
			return false;
		}
		result = new Quaternion(result2, result3, result4, result5);
		return true;
	}

	public static bool TryParseQuaternionD(string text, out QuaternionD result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 4)
		{
			result = QuaternionD.identity;
			return false;
		}
		if (!double.TryParse(array[0], out var result2))
		{
			result = QuaternionD.identity;
			return false;
		}
		if (!double.TryParse(array[1], out var result3))
		{
			result = QuaternionD.identity;
			return false;
		}
		if (!double.TryParse(array[2], out var result4))
		{
			result = QuaternionD.identity;
			return false;
		}
		if (!double.TryParse(array[3], out var result5))
		{
			result = QuaternionD.identity;
			return false;
		}
		result = new QuaternionD(result2, result3, result4, result5);
		return true;
	}

	public static bool TryParseRect(string text, out Rect result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 4)
		{
			result = default(Rect);
			return false;
		}
		if (!float.TryParse(array[0], out var result2))
		{
			result = default(Rect);
			return false;
		}
		if (!float.TryParse(array[1], out var result3))
		{
			result = default(Rect);
			return false;
		}
		if (!float.TryParse(array[2], out var result4))
		{
			result = default(Rect);
			return false;
		}
		if (!float.TryParse(array[3], out var result5))
		{
			result = default(Rect);
			return false;
		}
		result = new Rect(result2, result3, result4, result5);
		return true;
	}

	public static bool TryParseColor(string text, out Color result)
	{
		string[] array = ParseArray(text);
		if (array.Length == 3)
		{
			if (!float.TryParse(array[0], out var result2))
			{
				result = Color.white;
				return false;
			}
			if (!float.TryParse(array[1], out var result3))
			{
				result = Color.white;
				return false;
			}
			if (!float.TryParse(array[2], out var result4))
			{
				result = Color.white;
				return false;
			}
			result = new Color(Mathf.Clamp01(result2), Mathf.Clamp01(result3), Mathf.Clamp01(result4));
			return true;
		}
		if (array.Length == 4)
		{
			if (!float.TryParse(array[0], out var result5))
			{
				result = Color.white;
				return false;
			}
			if (!float.TryParse(array[1], out var result6))
			{
				result = Color.white;
				return false;
			}
			if (!float.TryParse(array[2], out var result7))
			{
				result = Color.white;
				return false;
			}
			if (!float.TryParse(array[3], out var result8))
			{
				result = Color.white;
				return false;
			}
			result = new Color(Mathf.Clamp01(result5), Mathf.Clamp01(result6), Mathf.Clamp01(result7), Mathf.Clamp01(result8));
			return true;
		}
		result = Color.white;
		return false;
	}

	public static bool TryParseColor32(string text, out Color32 result)
	{
		string[] array = ParseArray(text);
		if (array.Length != 4)
		{
			result = default(Color32);
			return false;
		}
		if (!byte.TryParse(array[0], out var result2))
		{
			result = default(Color32);
			return false;
		}
		if (!byte.TryParse(array[1], out var result3))
		{
			result = default(Color32);
			return false;
		}
		if (!byte.TryParse(array[2], out var result4))
		{
			result = default(Color32);
			return false;
		}
		if (!byte.TryParse(array[3], out var result5))
		{
			result = default(Color32);
			return false;
		}
		result = new Color32(result2, result3, result4, result5);
		return true;
	}
}
