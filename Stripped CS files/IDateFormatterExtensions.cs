public static class IDateFormatterExtensions
{
	public static void GetTimeComponents(this IDateTimeFormatter formatter, double time, out int years, out int days, out int hours, out int minutes, out int seconds)
	{
		int year = formatter.Year;
		int day = formatter.Day;
		years = (int)(time / (double)year);
		time -= (double)years * (double)year;
		seconds = (int)time;
		minutes = seconds / 60 % 60;
		hours = seconds / 3600 % (day / 3600);
		days = seconds / day;
		seconds %= 60;
	}

	public static void GetDateComponents(this IDateTimeFormatter formatter, double time, out int years, out int days, out int hours, out int minutes, out int seconds)
	{
		formatter.GetTimeComponents(time, out years, out days, out hours, out minutes, out seconds);
		years++;
		days++;
	}
}
