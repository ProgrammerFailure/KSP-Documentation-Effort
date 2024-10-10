using UnityEngine;

public class RichTextUtil
{
	public static Color32 colorTitle = new Color32(237, 139, 11, byte.MaxValue);

	public static Color32 colorAdvance = new Color32(237, 237, 139, byte.MaxValue);

	public static Color32 colorAwards = new Color32(139, 237, 139, byte.MaxValue);

	public static Color32 colorPenalty = new Color32(237, 11, 11, byte.MaxValue);

	public static Color32 colorAgent = new Color32(237, 139, 11, byte.MaxValue);

	public static Color32 colorParams = new Color32(190, 194, 174, byte.MaxValue);

	public static string Title(string title, int lines = 2)
	{
		return "<b><color=#" + RUIutils.ColorToHex(colorTitle) + ">" + title + "</color></b>" + new string('\n', lines);
	}

	public static string Text(string text, int lines = 2)
	{
		return text + new string('\n', lines);
	}

	public static string Text(string title, string value, int lines = 2)
	{
		return "<b><color=#" + RUIutils.ColorToHex(colorTitle) + ">" + title + ":</color></b> " + value + new string('\n', lines);
	}

	public static string TextParam(string title, string value, int lines = 2)
	{
		return "<b><color=#" + RUIutils.ColorToHex(colorParams) + ">" + title + ":</color></b> " + value + new string('\n', lines);
	}

	public static string TextParamsAward(string title, string value, int lines = 2)
	{
		return "<b><color=#" + RUIutils.ColorToHex(colorParams) + ">" + title + ":</color></b> " + value + new string('\n', lines);
	}

	public static string TextAgent(string title, string value, int lines = 2)
	{
		return "<b><color=#" + RUIutils.ColorToHex(colorAgent) + ">" + title + ":</color></b> " + value + new string('\n', lines);
	}

	public static string TextAdvance(string title, string value, int lines = 2)
	{
		return "<b><color=#" + RUIutils.ColorToHex(colorAdvance) + ">" + title + ":</color></b> " + value + new string('\n', lines);
	}

	public static string TextAward(string title, string value, int lines = 2)
	{
		return "<b><color=#" + RUIutils.ColorToHex(colorAwards) + ">" + title + ":</color></b> " + value + new string('\n', lines);
	}

	public static string TextPenalty(string title, string value, int lines = 2)
	{
		return "<b><color=#" + RUIutils.ColorToHex(colorPenalty) + ">" + title + ":</color></b> " + value + new string('\n', lines);
	}
}
