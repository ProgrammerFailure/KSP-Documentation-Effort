public class KSPRichTextUtil : RichTextUtil
{
	public static string TextDate(string title, double value, int lines = 2)
	{
		return RichTextUtil.TextParam(title, KSPUtil.PrintDateNew(value, includeTime: true), lines);
	}

	public static string TextDurationCompact(string title, double value, int lines = 2)
	{
		return RichTextUtil.TextParam((!(title == "Duration") || !GameSettings.SHOW_DEADLINES_AS_DATES) ? title : "Deadline", (!GameSettings.SHOW_DEADLINES_AS_DATES) ? KSPUtil.PrintDateDeltaCompact(0.0 - value, includeTime: true, includeSeconds: true, useAbs: true) : KSPUtil.PrintDate(Planetarium.fetch.time + value, includeTime: true, includeSeconds: true), lines);
	}

	public static string TextDateCompact(string title, double value, int lines = 2)
	{
		return RichTextUtil.TextParam(title, (!GameSettings.SHOW_DEADLINES_AS_DATES) ? KSPUtil.PrintDateDeltaCompact(Planetarium.fetch.time - value, includeTime: true, includeSeconds: true, useAbs: true) : KSPUtil.PrintDate(value, includeTime: true, includeSeconds: true), lines);
	}
}
