using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIUtil
{
	public class Layout
	{
		public static void ProgressBar(float start, float end, GUIStyle bgStyle, GUIStyle barStyle, params GUILayoutOption[] options)
		{
			GUILayout.Box("", bgStyle, options);
			GUIUtil.ProgressBar(GUILayoutUtility.GetLastRect(), start, end, barStyle);
		}

		public static void ProgressBarVert(float start, float end, GUIStyle bgStyle, GUIStyle barStyle, params GUILayoutOption[] options)
		{
			GUILayout.Box("", bgStyle, options);
			ProgressBarVertical(GUILayoutUtility.GetLastRect(), start, end, barStyle);
		}
	}

	public static int progressBarBorderLeft;

	public static int progressBarBorderRight;

	public static int progressBarBorderTop;

	public static int progressBarBorderBottom;

	public static Dictionary<int, bool> EditableFieldStates;

	public static Vector2 enumSelectionPaneScrollPos;

	public static void ProgressBar(Rect rect, float start, float end, GUIStyle style)
	{
		rect.x += rect.width * start;
		rect.width *= end - start;
		progressBarBorderLeft = style.border.left;
		progressBarBorderRight = style.border.right;
		style.border.left = Mathf.Min(style.border.left, (int)(rect.width * 0.5f));
		style.border.right = Mathf.Min(style.border.right, (int)(rect.width * 0.5f));
		GUI.Box(rect, "", style);
		style.border.left = progressBarBorderLeft;
		style.border.right = progressBarBorderRight;
	}

	public static void ProgressBarVertical(Rect rect, float start, float end, GUIStyle style)
	{
		rect.y += rect.height * start;
		rect.height *= end - start;
		progressBarBorderBottom = style.border.bottom;
		progressBarBorderTop = style.border.top;
		style.border.bottom = Mathf.Min(style.border.bottom, (int)(rect.height * 0.5f));
		style.border.top = Mathf.Min(style.border.top, (int)(rect.height * 0.5f));
		GUI.Box(rect, "", style);
		style.border.top = progressBarBorderTop;
		style.border.bottom = progressBarBorderBottom;
	}

	public static Rect ScreenCenteredRect(float width, float height)
	{
		return new Rect((float)Screen.width * 0.5f - width * 0.5f, (float)Screen.height * 0.5f - height * 0.5f, width, height);
	}

	public static float LabelHorizontalSlider(string caption, float value, float leftValue, float rightValue, string valueStringFormat, params GUILayoutOption[] guiOptions)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(caption + value.ToString(valueStringFormat));
		value = GUILayout.HorizontalSlider(value, leftValue, rightValue, guiOptions);
		GUILayout.EndHorizontal();
		return value;
	}

	public static float LabelVerticalSlider(string caption, float value, float leftValue, float rightValue, string valueStringFormat, params GUILayoutOption[] guiOptions)
	{
		GUILayout.BeginVertical();
		value = GUILayout.VerticalSlider(value, leftValue, rightValue, guiOptions);
		GUILayout.Label(caption + value.ToString(valueStringFormat));
		GUILayout.EndVertical();
		return value;
	}

	public static T EnumFlagField<T>(string caption, T value, int ValueCount, int xCount, Func<int, T> ValueAt, Func<int, string> ValueNameAt, Func<T, int> IntCast, Func<int, T> TCast)
	{
		GUILayout.Label(caption);
		int num = 0;
		for (int i = 0; i < ValueCount; i++)
		{
			if (i % xCount == 0 || i == 0)
			{
				GUILayout.BeginHorizontal();
			}
			GUI.enabled = Mathf.NextPowerOfTwo(IntCast(ValueAt(i))) == IntCast(ValueAt(i));
			if (GUILayout.Toggle((IntCast(value) & IntCast(ValueAt(i))) == IntCast(ValueAt(i)), ValueNameAt(i), GUILayout.Width(90f)) && GUI.enabled)
			{
				num |= IntCast(ValueAt(i));
			}
			GUI.enabled = true;
			if (i % xCount == xCount - 1 || i == ValueCount - 1)
			{
				GUILayout.EndHorizontal();
			}
		}
		if (num != IntCast(value))
		{
			return TCast(num);
		}
		return value;
	}

	public static void ClearEditableFieldFlags()
	{
		if (EditableFieldStates != null)
		{
			EditableFieldStates.Clear();
		}
	}

	public static T EditableField<T>(string caption, T value, Func<T, string> ToStringMethod, Func<string, T> ParseMethod, Callback<T> onSubmit)
	{
		bool flag = false;
		if (EditableFieldStates == null)
		{
			EditableFieldStates = new Dictionary<int, bool>();
		}
		if (EditableFieldStates.ContainsKey(caption.GetHashCode_Net35()))
		{
			flag = EditableFieldStates[caption.GetHashCode_Net35()];
		}
		else
		{
			EditableFieldStates.Add(caption.GetHashCode_Net35(), value: false);
		}
		if (flag)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(caption + ": ");
			value = ParseMethod(GUILayout.TextField(ToStringMethod(value)));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Done"))
			{
				EditableFieldStates[caption.GetHashCode_Net35()] = false;
				onSubmit(value);
			}
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(caption + ": " + ToStringMethod(value));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Edit"))
			{
				EditableFieldStates[caption.GetHashCode_Net35()] = true;
			}
			GUILayout.EndHorizontal();
		}
		return value;
	}

	public static string EditableStringField(string caption, string value, Callback<string> onSubmit)
	{
		return EditableField(caption, value, (string f) => f, (string s) => s, onSubmit);
	}

	public static bool EditableBoolField(string caption, bool value, Callback<bool> onSubmit)
	{
		return EditableField(caption, value, (bool f) => f.ToString(), bool.Parse, onSubmit);
	}

	public static int EditableIntField(string caption, int value, Callback<int> onSubmit)
	{
		return EditableField(caption, value, (int f) => f.ToString(), int.Parse, onSubmit);
	}

	public static float EditableFloatField(string caption, float value, Callback<float> onSubmit)
	{
		return EditableField(caption, value, (float f) => f.ToString(), float.Parse, onSubmit);
	}

	public static double EditableDoubleField(string caption, double value, Callback<double> onSubmit)
	{
		return EditableField(caption, value, (double f) => f.ToString(), double.Parse, onSubmit);
	}

	public static string EditableTextArea(string caption, string value, Callback<string> onSubmit, params GUILayoutOption[] guiOptions)
	{
		bool flag = false;
		if (EditableFieldStates == null)
		{
			EditableFieldStates = new Dictionary<int, bool>();
		}
		if (EditableFieldStates.ContainsKey(caption.GetHashCode_Net35()))
		{
			flag = EditableFieldStates[caption.GetHashCode_Net35()];
		}
		else
		{
			EditableFieldStates.Add(caption.GetHashCode_Net35(), value: false);
		}
		if (flag)
		{
			GUILayout.Label(caption + ": ");
			value = GUILayout.TextArea(value, guiOptions);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Done"))
			{
				EditableFieldStates[caption.GetHashCode_Net35()] = false;
				onSubmit(value);
			}
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.Label(caption + ": ");
			GUI.enabled = false;
			GUILayout.TextArea(value, guiOptions);
			GUI.enabled = true;
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Edit"))
			{
				EditableFieldStates[caption.GetHashCode_Net35()] = true;
			}
			GUILayout.EndHorizontal();
		}
		return value;
	}

	public static int EditableEnumField(string caption, Type enumType, int selected, Callback<int> onSubmit, params GUILayoutOption[] guiOptions)
	{
		bool flag = false;
		if (EditableFieldStates == null)
		{
			EditableFieldStates = new Dictionary<int, bool>();
		}
		if (EditableFieldStates.ContainsKey(caption.GetHashCode_Net35()))
		{
			flag = EditableFieldStates[caption.GetHashCode_Net35()];
		}
		else
		{
			EditableFieldStates.Add(caption.GetHashCode_Net35(), value: false);
		}
		if (flag)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(caption + ": ");
			GUILayout.TextArea(Enum.GetNames(enumType)[selected].ToString(), guiOptions);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			int num = DrawEnumSelectionPane(enumType, selected, ref enumSelectionPaneScrollPos);
			if (num != selected)
			{
				selected = num;
				onSubmit(num);
				EditableFieldStates[caption.GetHashCode_Net35()] = false;
			}
		}
		else
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(caption + ": ");
			GUILayout.TextArea(Enum.GetNames(enumType)[selected].ToString(), guiOptions);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Edit"))
			{
				EditableFieldStates[caption.GetHashCode_Net35()] = true;
				enumSelectionPaneScrollPos = Vector2.zero;
			}
			GUILayout.EndHorizontal();
		}
		return selected;
	}

	public static int DrawEnumSelectionPane(Type enumType, int selected, ref Vector2 scrollPos)
	{
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		int num = 0;
		string[] names = Enum.GetNames(enumType);
		foreach (string text in names)
		{
			if (GUILayout.Toggle(selected == num, text, GUI.skin.button))
			{
				selected = num;
			}
			num++;
		}
		GUILayout.EndScrollView();
		return selected;
	}
}
