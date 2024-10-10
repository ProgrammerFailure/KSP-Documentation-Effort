using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class TMPProUtil
{
	public static TextAlignmentOptions TextAlignment(TextAnchor anchor)
	{
		return anchor switch
		{
			TextAnchor.UpperLeft => TextAlignmentOptions.TopLeft, 
			TextAnchor.UpperCenter => TextAlignmentOptions.Top, 
			TextAnchor.UpperRight => TextAlignmentOptions.TopRight, 
			TextAnchor.MiddleLeft => TextAlignmentOptions.MidlineLeft, 
			TextAnchor.MiddleCenter => TextAlignmentOptions.Center, 
			TextAnchor.MiddleRight => TextAlignmentOptions.MidlineRight, 
			TextAnchor.LowerLeft => TextAlignmentOptions.BottomLeft, 
			TextAnchor.LowerCenter => TextAlignmentOptions.Bottom, 
			TextAnchor.LowerRight => TextAlignmentOptions.BottomRight, 
			_ => TextAlignmentOptions.Left, 
		};
	}

	public static Component GetComponentByInstanceID(this GameObject go, int objectID)
	{
		Component[] componentsInChildren = go.GetComponentsInChildren<Component>();
		int num = 0;
		while (true)
		{
			if (num < componentsInChildren.Length)
			{
				if (componentsInChildren[num].GetInstanceID() == objectID)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return componentsInChildren[num];
	}

	public static FontStyles FontStyle(FontStyle style)
	{
		return style switch
		{
			UnityEngine.FontStyle.Normal => FontStyles.Normal, 
			UnityEngine.FontStyle.Bold => FontStyles.Bold, 
			UnityEngine.FontStyle.Italic => FontStyles.Italic, 
			UnityEngine.FontStyle.BoldAndItalic => FontStyles.Italic, 
			_ => FontStyles.Normal, 
		};
	}

	public static TMP_InputField.LineType LineType(InputField.LineType type)
	{
		return type switch
		{
			InputField.LineType.SingleLine => TMP_InputField.LineType.SingleLine, 
			InputField.LineType.MultiLineSubmit => TMP_InputField.LineType.MultiLineSubmit, 
			_ => TMP_InputField.LineType.MultiLineNewline, 
		};
	}
}
