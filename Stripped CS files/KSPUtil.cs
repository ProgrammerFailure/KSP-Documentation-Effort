using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ns9;
using UniLinq;
using UnityEngine;

public static class KSPUtil
{
	public static class SystemDateTime
	{
		public static DateTime DateTimeNow()
		{
			if (localTimeZoneInvalid)
			{
				return DateTime.UtcNow;
			}
			try
			{
				return DateTime.Now;
			}
			catch (Exception)
			{
				localTimeZoneInvalid = true;
				return DateTime.UtcNow;
			}
		}
	}

	public class DefaultDateTimeFormatter : IDateTimeFormatter
	{
		public double cachedSecondsUTforTimeStampCompact = -1.0;

		public bool cachedDaysforTimeStampCompact;

		public bool cachedYearsforTimeStampCompact;

		public string cachedTimeStampCompact;

		public static int[] dateHolder = new int[5];

		public static int[] absDateHolder = new int[5];

		public static double secondsUTHolder = 0.0;

		public int cachedYearsforPrintDate;

		public int cachedDaysforPrintDate;

		public int cachedHoursforPrintDate;

		public int cachedMinsforPrintDate;

		public int cachedSecondsforPrintDate;

		public bool cachedIncludeTimeforPrintDate;

		public bool cachedIncludeSecondsforPrintDate;

		public string cachedPrintDate;

		public double cachedSecondsUTforPrintDateCompact = -1.0;

		public bool cachedIncludeTimeforPrintDateCompact;

		public bool cachedIncludeSecondsforPrintDateCompact;

		public string cachedPrintDateCompact;

		public int Minute => 60;

		public int Hour => 3600;

		public int Day
		{
			get
			{
				if (!GameSettings.KERBIN_TIME)
				{
					return EarthDay;
				}
				return KerbinDay;
			}
		}

		public int Year
		{
			get
			{
				if (!GameSettings.KERBIN_TIME)
				{
					return EarthYear;
				}
				return KerbinYear;
			}
		}

		public static int KerbinDay => 21600;

		public static int KerbinYear => 9201600;

		public static int EarthDay => 86400;

		public static int EarthYear => 31536000;

		public static StringBuilder AddUnits(StringBuilder sb, int val, string singular, string plural)
		{
			sb.Append(val.ToString());
			sb.Append((val == 1) ? singular : plural);
			return sb;
		}

		public static string IsBadNum(double time)
		{
			if (double.IsNaN(time))
			{
				return "NaN";
			}
			if (double.IsPositiveInfinity(time))
			{
				return "+Inf";
			}
			if (double.IsNegativeInfinity(time))
			{
				return "-Inf";
			}
			return null;
		}

		public string PrintTimeLong(double time)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			GetDateFromUT(time);
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			if (dateHolder[4] < 0 || dateHolder[3] < 0 || dateHolder[2] < 0 || dateHolder[1] < 0 || dateHolder[0] < 0)
			{
				stringBuilder.Append("-");
			}
			AddUnits(stringBuilder, absDateHolder[4], cacheAutoLOC_6002322, cacheAutoLOC_6002323).Append(", ");
			AddUnits(stringBuilder, absDateHolder[3], cacheAutoLOC_6002324, cacheAutoLOC_6002325).Append(", ");
			AddUnits(stringBuilder, absDateHolder[2], cacheAutoLOC_6002326, cacheAutoLOC_6002327).Append(", ");
			AddUnits(stringBuilder, absDateHolder[1], cacheAutoLOC_6002328, cacheAutoLOC_6002329).Append(", ");
			AddUnits(stringBuilder, absDateHolder[0], cacheAutoLOC_6002330, cacheAutoLOC_6002331);
			return stringBuilder.ToStringAndRelease();
		}

		public string PrintTimeStamp(double time, bool days = false, bool years = false)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			GetDateFromUT(time);
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			if (years)
			{
				stringBuilder.Append(cacheAutoLOC_6002322 + " ").Append(dateHolder[4]).Append(", ");
			}
			if (days)
			{
				stringBuilder.Append(cacheAutoLOC_6002324 + " ").Append(dateHolder[3]).Append(" - ");
			}
			AppendTimeToStringBuilder(stringBuilder, absDateHolder[4] < 10);
			return stringBuilder.ToStringAndRelease();
		}

		public string PrintTimeStampCompact(double time, bool days = false, bool years = false)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			GetDateFromUT(time);
			if (cachedSecondsUTforTimeStampCompact == secondsUTHolder && cachedDaysforTimeStampCompact == days && cachedYearsforTimeStampCompact == years)
			{
				return cachedTimeStampCompact;
			}
			cachedSecondsUTforTimeStampCompact = secondsUTHolder;
			cachedDaysforTimeStampCompact = days;
			cachedYearsforTimeStampCompact = years;
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			if (years)
			{
				stringBuilder.Append(dateHolder[4]).Append(cacheAutoLOC_7003243 + ", ");
			}
			if (days)
			{
				stringBuilder.Append(dateHolder[3]).Append(cacheAutoLOC_7003244 + ", ");
			}
			AppendTimeToStringBuilder(stringBuilder, absDateHolder[4] < 10);
			cachedTimeStampCompact = stringBuilder.ToStringAndRelease();
			return cachedTimeStampCompact;
		}

		public string PrintTime(double time, int valuesOfInterest, bool explicitPositive)
		{
			return PrintTime(time, valuesOfInterest, explicitPositive, logEnglish: false);
		}

		public string PrintTime(double time, int valuesOfInterest, bool explicitPositive, bool logEnglish)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			bool flag = time < 0.0;
			GetDateFromUT(time);
			string[] array = new string[5] { "#autoLOC_6002317", "#autoLOC_6002318", "#autoLOC_6002319", "#autoLOC_6002320", "#autoLOC_6002321" };
			if (logEnglish)
			{
				array = new string[5] { "s", "m", "h", "d", "y" };
			}
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			if (flag)
			{
				stringBuilder.Append("- ");
			}
			else if (explicitPositive)
			{
				stringBuilder.Append("+ ");
			}
			int num = dateHolder.Length;
			while (num-- > 0)
			{
				if (dateHolder[num] == 0)
				{
					continue;
				}
				for (int num2 = num; num2 > Mathf.Max(num - valuesOfInterest, -1); num2--)
				{
					stringBuilder.Append(Math.Abs(dateHolder[num2])).Append(Localizer.Format(array[num2]));
					if (num2 - 1 > Mathf.Max(num - valuesOfInterest, -1))
					{
						stringBuilder.Append(", ");
					}
				}
				break;
			}
			string text2 = stringBuilder.ToStringAndRelease();
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "0";
			}
			return text2;
		}

		public string PrintTimeCompact(double time, bool explicitPositive)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			bool num = time < 0.0;
			GetDateFromUT(time);
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			if (num)
			{
				stringBuilder.Append(cacheAutoLOC_6002332 + " ");
			}
			else if (explicitPositive)
			{
				stringBuilder.Append(cacheAutoLOC_6002333 + " ");
			}
			if (dateHolder[3] > 0)
			{
				stringBuilder.Append(Math.Abs(dateHolder[3])).Append(":");
			}
			AppendTimeToStringBuilder(stringBuilder, displaySeconds: true);
			return stringBuilder.ToStringAndRelease();
		}

		public void GetDateFromUT(double time)
		{
			if (GameSettings.KERBIN_TIME)
			{
				GetKerbinDateFromUT(time);
			}
			else
			{
				GetEarthDateFromUT(time);
			}
		}

		public static void get_date_from_UT(double time, int year_len, int day_len)
		{
			secondsUTHolder = Math.Floor(time);
			int num = (int)(time / (double)year_len);
			time -= (double)num * (double)year_len;
			int num2 = (int)time;
			int num3 = num2 / 60 % 60;
			int num4 = num2 / 3600 % (day_len / 3600);
			int num5 = num2 / day_len;
			absDateHolder[0] = Mathf.Abs(dateHolder[0] = num2 % 60);
			absDateHolder[1] = Mathf.Abs(dateHolder[1] = num3);
			absDateHolder[2] = Mathf.Abs(dateHolder[2] = num4);
			absDateHolder[3] = Mathf.Abs(dateHolder[3] = num5);
			absDateHolder[4] = Mathf.Abs(dateHolder[4] = num);
		}

		public static void GetEarthDateFromUT(double time)
		{
			get_date_from_UT(time, EarthYear, EarthDay);
		}

		public static void GetKerbinDateFromUT(double time)
		{
			get_date_from_UT(time, KerbinYear, KerbinDay);
		}

		public string PrintDateDelta(double time, bool includeTime, bool includeSeconds, bool useAbs)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			if (useAbs && time < 0.0)
			{
				time = 0.0 - time;
			}
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			GetDateFromUT(time);
			if (dateHolder[4] > 1)
			{
				stringBuilder.Append(dateHolder[4]).Append(" " + cacheAutoLOC_6002335);
			}
			else if (dateHolder[4] == 1)
			{
				stringBuilder.Append(dateHolder[4]).Append(" " + cacheAutoLOC_6002334);
			}
			if (dateHolder[3] > 1)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(dateHolder[3]).Append(" " + cacheAutoLOC_6002336);
			}
			else if (dateHolder[3] == 1)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(dateHolder[3]).Append(" " + cacheAutoLOC_6002337);
			}
			if (includeTime)
			{
				if (dateHolder[2] > 1)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(dateHolder[2]).Append(" " + cacheAutoLOC_6002339);
				}
				else if (dateHolder[2] == 1)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(dateHolder[2]).Append(" " + cacheAutoLOC_6002338);
				}
				if (dateHolder[1] > 1)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(dateHolder[1]).Append(" " + cacheAutoLOC_6002340);
				}
				else if (dateHolder[1] == 1)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(dateHolder[1]).Append(" " + cacheAutoLOC_6002341);
				}
				if (includeSeconds)
				{
					if (dateHolder[0] > 1)
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(dateHolder[0]).Append(" " + cacheAutoLOC_6002342);
					}
					else if (dateHolder[0] == 1)
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(dateHolder[0]).Append(" " + cacheAutoLOC_6002343);
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append((!includeTime) ? ("0 " + cacheAutoLOC_6002336) : (includeSeconds ? ("0 " + cacheAutoLOC_6002342) : ("0 " + cacheAutoLOC_6002340)));
			}
			return stringBuilder.ToStringAndRelease();
		}

		public string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, bool useAbs)
		{
			return PrintDateDeltaCompact(time, includeTime, includeSeconds, useAbs, 5);
		}

		public string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, bool useAbs, int interestedPlaces)
		{
			int num = 0;
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			if (useAbs && time < 0.0)
			{
				time = 0.0 - time;
			}
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			GetDateFromUT(time);
			if (dateHolder[4] > 0 && num < interestedPlaces)
			{
				stringBuilder.Append(dateHolder[4]).Append(cacheAutoLOC_6002321);
				num++;
			}
			if (dateHolder[3] > 0 && num < interestedPlaces)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(dateHolder[3]).Append(cacheAutoLOC_6002320);
				num++;
			}
			if (includeTime)
			{
				if (dateHolder[2] > 0 && num < interestedPlaces)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(dateHolder[2]).Append(cacheAutoLOC_6002319);
					num++;
				}
				if (dateHolder[1] > 0 && num < interestedPlaces)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(dateHolder[1]).Append(cacheAutoLOC_6002318);
					num++;
				}
				if (includeSeconds && dateHolder[0] > 0 && num < interestedPlaces)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(dateHolder[0]).Append(cacheAutoLOC_6002317);
				}
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append((!includeTime) ? ("0" + cacheAutoLOC_6002320) : (includeSeconds ? ("0" + cacheAutoLOC_6002317) : ("0" + cacheAutoLOC_6002318)));
			}
			return stringBuilder.ToStringAndRelease();
		}

		public int GetUTYear()
		{
			return dateHolder[4] + 1;
		}

		public int GetUTDay()
		{
			return dateHolder[3] + 1;
		}

		public int GetUTHour()
		{
			return dateHolder[2];
		}

		public int GetUTMinute()
		{
			return dateHolder[1];
		}

		public int GetUTSecond()
		{
			return dateHolder[0];
		}

		public int GetTimeSpanYear()
		{
			return dateHolder[4];
		}

		public int GetTimeSpanDay()
		{
			return dateHolder[3];
		}

		public int GetTimeSpanHour()
		{
			return dateHolder[2];
		}

		public int GetTimeSpanMinute()
		{
			return dateHolder[1];
		}

		public int GetTimeSpanSecond()
		{
			return dateHolder[0];
		}

		public string PrintDate(double time, bool includeTime, bool includeSeconds = false)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			GetDateFromUT(time);
			if (cachedIncludeTimeforPrintDate == includeTime && cachedIncludeSecondsforPrintDate == includeSeconds)
			{
				if (!includeSeconds && cachedYearsforPrintDate == dateHolder[4] && cachedDaysforPrintDate == dateHolder[3] && cachedHoursforPrintDate == dateHolder[2] && cachedMinsforPrintDate == dateHolder[1])
				{
					return cachedPrintDate;
				}
				if (includeSeconds && cachedYearsforPrintDate == dateHolder[4] && cachedDaysforPrintDate == dateHolder[3] && cachedHoursforPrintDate == dateHolder[2] && cachedMinsforPrintDate == dateHolder[1] && cachedSecondsforPrintDate == dateHolder[0])
				{
					return cachedPrintDate;
				}
			}
			cachedYearsforPrintDate = dateHolder[4];
			cachedDaysforPrintDate = dateHolder[3];
			cachedHoursforPrintDate = dateHolder[2];
			cachedMinsforPrintDate = dateHolder[1];
			cachedSecondsforPrintDate = dateHolder[0];
			cachedIncludeTimeforPrintDate = includeTime;
			cachedIncludeSecondsforPrintDate = includeSeconds;
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			stringBuilder.Append(cacheAutoLOC_6002322 + " ");
			stringBuilder.Append(dateHolder[4] + 1);
			stringBuilder.Append(", " + cacheAutoLOC_6002324 + " ");
			stringBuilder.Append(dateHolder[3] + 1);
			if (includeTime)
			{
				stringBuilder.Append(" - ");
				stringBuilder.Append(dateHolder[2]);
				stringBuilder.Append(cacheAutoLOC_6002319 + ", ");
				stringBuilder.Append(dateHolder[1]);
				stringBuilder.Append(cacheAutoLOC_6002318);
				if (includeSeconds)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(dateHolder[0]);
					stringBuilder.Append(cacheAutoLOC_6002317);
				}
			}
			cachedPrintDate = stringBuilder.ToStringAndRelease();
			return cachedPrintDate;
		}

		public string PrintDateNew(double time, bool includeTime)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			GetDateFromUT(time);
			stringBuilder.Append(Localizer.Format("#autoLOC_7003005", dateHolder[4] + 1, dateHolder[3] + 1));
			if (includeTime)
			{
				stringBuilder.Append(" - ");
				AppendTimeToStringBuilder(stringBuilder, displaySeconds: true);
			}
			return stringBuilder.ToStringAndRelease();
		}

		public string PrintDateCompact(double time, bool includeTime, bool includeSeconds = false)
		{
			string text = IsBadNum(time);
			if (text != null)
			{
				return text;
			}
			GetDateFromUT(time);
			if (cachedSecondsUTforPrintDateCompact == secondsUTHolder && cachedIncludeTimeforPrintDateCompact == includeTime && cachedIncludeSecondsforPrintDateCompact == includeSeconds)
			{
				return cachedPrintDateCompact;
			}
			cachedSecondsUTforPrintDateCompact = secondsUTHolder;
			cachedIncludeTimeforPrintDateCompact = includeTime;
			cachedIncludeSecondsforPrintDateCompact = includeSeconds;
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			stringBuilder.Append(cacheAutoLOC_6002344);
			stringBuilder.Append(dateHolder[4] + 1);
			stringBuilder.Append(", ");
			stringBuilder.Append(cacheAutoLOC_6002345);
			if (dateHolder[3] + 1 < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(dateHolder[3] + 1);
			if (includeTime)
			{
				stringBuilder.Append(", ");
				AppendTimeToStringBuilder(stringBuilder, includeSeconds);
			}
			cachedPrintDateCompact = stringBuilder.ToStringAndRelease();
			return cachedPrintDateCompact;
		}

		public static void AppendTimeToStringBuilder(StringBuilder sb, bool displaySeconds)
		{
			if (dateHolder[2] < 0 || dateHolder[1] < 0 || dateHolder[0] < 0)
			{
				sb.Append(" - ");
			}
			int num = absDateHolder.Length;
			while (num-- > 0)
			{
				absDateHolder[num] = Mathf.Abs(dateHolder[num]);
			}
			if (absDateHolder[2] < 10)
			{
				sb.Append("0");
			}
			sb.Append(absDateHolder[2]);
			sb.Append(":");
			if (absDateHolder[1] < 10)
			{
				sb.Append("0");
			}
			sb.Append(absDateHolder[1]);
			if (displaySeconds)
			{
				sb.Append(":");
				if (absDateHolder[0] < 10)
				{
					sb.Append("0");
				}
				sb.Append(absDateHolder[0]);
			}
		}
	}

	[Serializable]
	public class StringReplacement
	{
		public string badString;

		public string replacement;
	}

	public delegate T ObjectActivator<T>(params object[] args);

	public static string cacheAutoLOC_7003243;

	public static string cacheAutoLOC_7003244;

	public static string cacheAutoLOC_7003272;

	public static string cacheAutoLOC_7003273;

	public static string cacheAutoLOC_7003274;

	public static string cacheAutoLOC_7003275;

	public static string cacheAutoLOC_6002317;

	public static string cacheAutoLOC_6002318;

	public static string cacheAutoLOC_6002319;

	public static string cacheAutoLOC_6002320;

	public static string cacheAutoLOC_6002321;

	public static string cacheAutoLOC_6002322;

	public static string cacheAutoLOC_6002323;

	public static string cacheAutoLOC_6002324;

	public static string cacheAutoLOC_6002325;

	public static string cacheAutoLOC_6002326;

	public static string cacheAutoLOC_6002327;

	public static string cacheAutoLOC_6002328;

	public static string cacheAutoLOC_6002329;

	public static string cacheAutoLOC_6002330;

	public static string cacheAutoLOC_6002331;

	public static string cacheAutoLOC_6002332;

	public static string cacheAutoLOC_6002333;

	public static string cacheAutoLOC_6002334;

	public static string cacheAutoLOC_6002335;

	public static string cacheAutoLOC_6002336;

	public static string cacheAutoLOC_6002337;

	public static string cacheAutoLOC_6002338;

	public static string cacheAutoLOC_6002339;

	public static string cacheAutoLOC_6002340;

	public static string cacheAutoLOC_6002341;

	public static string cacheAutoLOC_6002342;

	public static string cacheAutoLOC_6002343;

	public static string cacheAutoLOC_6002344;

	public static string cacheAutoLOC_6002345;

	public static bool localTimeZoneInvalid;

	public static IDateTimeFormatter _dateTimeFormatter;

	public static string[] shortSIprefixes = new string[17]
	{
		"y", "z", "a", "f", "p", "n", "μ", "m", "", "k",
		"M", "G", "T", "P", "E", "Z", "Y"
	};

	public static string[] longSIprefixes = new string[17]
	{
		"yocto", "zepto", "atto", "femto", "pico", "nano", "micro", "milli", "", "kilo",
		"mega", "giga", "tera", "peta", "exa", "zetta", "yotta"
	};

	public static double[] prefixMults = new double[17]
	{
		1E-24, 1E-21, 1E-18, 1E-15, 1E-12, 1E-09, 1E-06, 0.001, 1.0, 1000.0,
		1000000.0, 1000000000.0, 1000000000000.0, 1000000000000000.0, 1E+18, 1E+21, 1E+24
	};

	public static double[] digitsScale = new double[3] { 1.0, 10.0, 100.0 };

	public static int unitIndex = 8;

	public static IDateTimeFormatter dateTimeFormatter
	{
		get
		{
			if (_dateTimeFormatter == null)
			{
				_dateTimeFormatter = new DefaultDateTimeFormatter();
			}
			return _dateTimeFormatter;
		}
		set
		{
			_dateTimeFormatter = value;
		}
	}

	public static string ApplicationRootPath
	{
		get
		{
			if (Application.platform != RuntimePlatform.OSXPlayer)
			{
				return Application.dataPath + "/../";
			}
			return Application.dataPath + "/../../";
		}
	}

	public static string ApplicationFileProtocol
	{
		get
		{
			if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				return "file://";
			}
			return "file:///";
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7003243 = Localizer.Format("#autoLOC_7003243");
		cacheAutoLOC_7003244 = Localizer.Format("#autoLOC_7003244");
		cacheAutoLOC_7003272 = Localizer.Format("#autoLOC_7003272");
		cacheAutoLOC_7003273 = Localizer.Format("#autoLOC_7003273");
		cacheAutoLOC_7003274 = Localizer.Format("#autoLOC_7003274");
		cacheAutoLOC_7003275 = Localizer.Format("#autoLOC_7003275");
		cacheAutoLOC_6002317 = Localizer.Format("#autoLOC_6002317");
		cacheAutoLOC_6002318 = Localizer.Format("#autoLOC_6002318");
		cacheAutoLOC_6002319 = Localizer.Format("#autoLOC_6002319");
		cacheAutoLOC_6002320 = Localizer.Format("#autoLOC_6002320");
		cacheAutoLOC_6002321 = Localizer.Format("#autoLOC_6002321");
		cacheAutoLOC_6002322 = Localizer.Format("#autoLOC_6002322");
		cacheAutoLOC_6002323 = Localizer.Format("#autoLOC_6002323");
		cacheAutoLOC_6002324 = Localizer.Format("#autoLOC_6002324");
		cacheAutoLOC_6002325 = Localizer.Format("#autoLOC_6002325");
		cacheAutoLOC_6002326 = Localizer.Format("#autoLOC_6002326");
		cacheAutoLOC_6002327 = Localizer.Format("#autoLOC_6002327");
		cacheAutoLOC_6002328 = Localizer.Format("#autoLOC_6002328");
		cacheAutoLOC_6002329 = Localizer.Format("#autoLOC_6002329");
		cacheAutoLOC_6002330 = Localizer.Format("#autoLOC_6002330");
		cacheAutoLOC_6002331 = Localizer.Format("#autoLOC_6002331");
		cacheAutoLOC_6002332 = Localizer.Format("#autoLOC_6002332");
		cacheAutoLOC_6002333 = Localizer.Format("#autoLOC_6002333");
		cacheAutoLOC_6002334 = Localizer.Format("#autoLOC_6002334");
		cacheAutoLOC_6002335 = Localizer.Format("#autoLOC_6002335");
		cacheAutoLOC_6002336 = Localizer.Format("#autoLOC_6002336");
		cacheAutoLOC_6002337 = Localizer.Format("#autoLOC_6002337");
		cacheAutoLOC_6002338 = Localizer.Format("#autoLOC_6002338");
		cacheAutoLOC_6002339 = Localizer.Format("#autoLOC_6002339");
		cacheAutoLOC_6002340 = Localizer.Format("#autoLOC_6002340");
		cacheAutoLOC_6002341 = Localizer.Format("#autoLOC_6002341");
		cacheAutoLOC_6002342 = Localizer.Format("#autoLOC_6002342");
		cacheAutoLOC_6002343 = Localizer.Format("#autoLOC_6002343");
		cacheAutoLOC_6002344 = Localizer.Format("#autoLOC_6002344");
		cacheAutoLOC_6002345 = Localizer.Format("#autoLOC_6002345");
	}

	public static string LocalizeNumber(double value, string format)
	{
		if (double.IsPositiveInfinity(value))
		{
			return Localizer.Format("#autoLOC_462439");
		}
		return Localizer.Format("<<1>>", value.ToString(format));
	}

	public static string LocalizeNumber(float value, string format)
	{
		return Localizer.Format("<<1>>", value.ToString(format));
	}

	public static string PrintSI(double amount, string unitName, int sigFigs = 3, bool longPrefix = false)
	{
		if (amount != 0.0 && !double.IsInfinity(amount) && !double.IsNaN(amount))
		{
			int num = (int)Math.Floor(Math.Log10(Math.Abs(amount)));
			int num2 = ((num >= 0) ? (num / 3) : ((num - 2) / 3));
			int num3 = num - num2 * 3 + 1;
			int num4 = sigFigs - num3;
			int num5 = Math.Max(0, Math.Min(num2 + unitIndex, prefixMults.Length - 1));
			string text = (longPrefix ? longSIprefixes[num5] : shortSIprefixes[num5]);
			amount /= prefixMults[num5];
			if (num4 < 0)
			{
				double num6 = digitsScale[-num4];
				amount = Math.Round(amount / num6) * num6;
				num4 = 0;
			}
			return Localizer.Format("<<1>><<2>><<3>>", amount.ToString("F" + num4), text, unitName);
		}
		return Localizer.Format("<<1>><<2>>", amount.ToString(), unitName);
	}

	public static string WriteVector(Vector2 vector)
	{
		return WriteVector(vector, ",");
	}

	public static string WriteVector(Vector2 vector, string separator)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(29);
		stringBuilder.Append(vector.x.ToString("G9")).Append(separator).Append(vector.y.ToString("G9"));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteVector(Vector3 vector)
	{
		return WriteVector(vector, ",");
	}

	public static string WriteVector(Vector3 vector, string separator)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(44);
		stringBuilder.Append(vector.x.ToString("G9")).Append(separator).Append(vector.y.ToString("G9"))
			.Append(separator)
			.Append(vector.z.ToString("G9"));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteVector(Vector3d vector)
	{
		return WriteVector(vector, ",");
	}

	public static string WriteVector(Vector3d vector, string separator)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(79);
		stringBuilder.Append(vector.x.ToString("G17")).Append(separator).Append(vector.y.ToString("G17"))
			.Append(separator)
			.Append(vector.z.ToString("G17"));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteVector(Vector4 vector)
	{
		return WriteVector(vector, ",");
	}

	public static string WriteVector(Vector4 vector, string separator)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(59);
		stringBuilder.Append(vector.x.ToString("G9")).Append(separator).Append(vector.y.ToString("G9"))
			.Append(separator)
			.Append(vector.z.ToString("G9"))
			.Append(separator)
			.Append(vector.w.ToString("G9"));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteQuaternion(Quaternion quaternion)
	{
		return WriteQuaternion(quaternion, ",");
	}

	public static string WriteQuaternion(Quaternion quaternion, string separator)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(59);
		stringBuilder.Append(quaternion.x.ToString("G9")).Append(separator).Append(quaternion.y.ToString("G9"))
			.Append(separator)
			.Append(quaternion.z.ToString("G9"))
			.Append(separator)
			.Append(quaternion.w.ToString("G9"));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteQuaternion(QuaternionD quaternion)
	{
		return WriteQuaternion(quaternion, ",");
	}

	public static string WriteQuaternion(QuaternionD quaternion, string separator)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(91);
		stringBuilder.Append(quaternion.x.ToString("G17")).Append(separator).Append(quaternion.y.ToString("G17"))
			.Append(separator)
			.Append(quaternion.z.ToString("G17"))
			.Append(separator)
			.Append(quaternion.w.ToString("G17"));
		return stringBuilder.ToStringAndRelease();
	}

	public static Vector2 ParseVector2(string vectorString)
	{
		string[] array = vectorString.Split(',');
		if (array.Length < 2)
		{
			Debug.Log("WARNING: Vector2 entry is not formatted properly! proper format for Vector2s is x,y");
			return Vector2.zero;
		}
		return new Vector2(float.Parse(array[0]), float.Parse(array[1]));
	}

	public static Vector2 ParseVector2(string vectorString, char separator)
	{
		string[] array = vectorString.Split(separator);
		if (array.Length < 2)
		{
			Debug.Log("WARNING: Vector2 entry is not formatted properly! proper format for Vector2s is x" + separator + "y");
			return Vector2.zero;
		}
		return new Vector2(float.Parse(array[0]), float.Parse(array[1]));
	}

	public static Vector2 ParseVector2(string x, string y)
	{
		return new Vector2(float.Parse(x), float.Parse(y));
	}

	public static Vector3 ParseVector3(string vectorString)
	{
		return ParseVector3(vectorString, ',');
	}

	public static Vector3 ParseVector3(string vectorString, char separator)
	{
		string[] array = vectorString.Split(separator);
		if (array.Length < 3)
		{
			Debug.Log("WARNING: Vector3 entry is not formatted properly! proper format for Vector3s is x" + separator + "y" + separator + "z");
			return Vector3.zero;
		}
		return new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
	}

	public static Vector3 ParseVector3(string x, string y, string z)
	{
		return new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
	}

	public static Vector3d ParseVector3d(string vectorString)
	{
		string[] array = vectorString.Split(',');
		if (array.Length < 3)
		{
			Debug.Log("WARNING: Vector3d entry is not formatted properly! proper format for Vector3s is x,y,z");
			return Vector3d.zero;
		}
		return new Vector3d(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]));
	}

	public static Vector3d ParseVector3d(string vectorString, char separator)
	{
		string[] array = vectorString.Split(separator);
		if (array.Length < 3)
		{
			Debug.Log("WARNING: Vector3d entry is not formatted properly! proper format for Vector3s is x" + separator + "y" + separator + "z");
			return Vector3d.zero;
		}
		return new Vector3d(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]));
	}

	public static Vector3d ParseVector3d(string x, string y, string z)
	{
		return new Vector3d(double.Parse(x), double.Parse(y), double.Parse(z));
	}

	public static Vector4 ParseVector4(string vectorString)
	{
		string[] array = vectorString.Split(',');
		if (array.Length < 4)
		{
			Debug.Log("WARNING: Vector4 entry is nor formatted properly! proper format for Vector4s is x,y,z,w");
			return Vector4.zero;
		}
		return new Vector4(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
	}

	public static Vector4 ParseVector4(string x, string y, string z, string w)
	{
		return new Vector4(float.Parse(x), float.Parse(y), float.Parse(z), float.Parse(w));
	}

	public static Vector4 ParseVector4(string vectorString, char separator)
	{
		string[] array = vectorString.Split(separator);
		if (array.Length < 4)
		{
			Debug.Log("WARNING: Vector4 entry is nor formatted properly! proper format for Vector4s is x" + separator + "y" + separator + "z" + separator + "w");
			return Vector4.zero;
		}
		return new Vector4(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
	}

	public static Quaternion ParseQuaternion(string quaternionString)
	{
		string[] array = quaternionString.Split(',');
		if (array.Length < 4)
		{
			Debug.Log("WARNING: Quaternion entry is nor formatted properly! proper format for Quaternion is x,y,z,w");
			return Quaternion.identity;
		}
		return new Quaternion(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
	}

	public static Quaternion ParseQuaternion(string quaternionString, char separator)
	{
		string[] array = quaternionString.Split(separator);
		if (array.Length < 4)
		{
			Debug.Log("WARNING: Quaternion entry is nor formatted properly! proper format for Quaternion is x" + separator + "y" + separator + "z" + separator + "w");
			return Quaternion.identity;
		}
		return new Quaternion(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
	}

	public static Quaternion ParseQuaternion(string x, string y, string z, string w)
	{
		return new Quaternion(float.Parse(x), float.Parse(y), float.Parse(z), float.Parse(w));
	}

	public static QuaternionD ParseQuaternionD(string quaternionString)
	{
		string[] array = quaternionString.Split(',');
		if (array.Length < 4)
		{
			Debug.Log("WARNING: QuaternionD entry is nor formatted properly! proper format for QuaternionD is x,y,z,w");
			return QuaternionD.identity;
		}
		return new QuaternionD(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]), double.Parse(array[3]));
	}

	public static QuaternionD ParseQuaternionD(string x, string y, string z, string w)
	{
		return new QuaternionD(double.Parse(x), double.Parse(y), double.Parse(z), double.Parse(w));
	}

	public static QuaternionD ParseQuaternionD(string quaternionString, char separator)
	{
		string[] array = quaternionString.Split(separator);
		if (array.Length < 4)
		{
			Debug.Log("WARNING: QuaternionD entry is nor formatted properly! proper format for QuaternionD is x" + separator + "y" + separator + "z" + separator + "w");
			return QuaternionD.identity;
		}
		return new QuaternionD(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]), double.Parse(array[3]));
	}

	public static string WriteArray<T>(T[] array) where T : IConvertible
	{
		string text = "";
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			text += array[i].ToString();
			if (i < array.Length - 1)
			{
				text += "; ";
			}
		}
		return text;
	}

	public static T[] ParseArray<T>(string arrayString, ParserMethod<T> parser)
	{
		string[] array = arrayString.Split(';');
		T[] array2 = new T[array.Length];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array2[i] = parser(array[i].Trim());
		}
		return array2;
	}

	public static Transform FindInPartModel(Transform part, string childName)
	{
		Transform transform = part.Find("model").Find(childName);
		if (!transform)
		{
			return recurseModels(part.Find("model"), childName);
		}
		return transform;
	}

	public static Transform recurseModels(Transform obj, string childName)
	{
		Transform transform = null;
		for (int i = 0; i < obj.childCount; i++)
		{
			Transform child = obj.GetChild(i);
			if (!(child.name == childName))
			{
				transform = recurseModels(child, childName);
				if (transform != null)
				{
					break;
				}
				continue;
			}
			return child;
		}
		return transform;
	}

	public static string PrintCoordinates(double latitude, double longitude, bool singleLine, bool includeMinutes = true, bool includeSeconds = true)
	{
		return PrintLatitude(latitude, includeMinutes, includeSeconds) + (singleLine ? ", " : "\n") + PrintLongitude(longitude, includeMinutes, includeSeconds);
	}

	public static string PrintLatitude(double latitude, bool includeMinutes = true, bool includeSeconds = true)
	{
		int num = (int)Math.Round(latitude * 3600.0);
		int value = num / 3600;
		num = Math.Abs(num % 3600);
		int num2 = num / 60;
		num %= 60;
		string text = Math.Abs(value) + "° ";
		if (includeMinutes)
		{
			text = text + num2 + "' ";
		}
		if (includeSeconds)
		{
			text = text + num + "\" ";
		}
		return text + ((latitude >= 0.0) ? cacheAutoLOC_7003272 : cacheAutoLOC_7003273);
	}

	public static string PrintLongitude(double longitude, bool includeMinutes = true, bool includeSeconds = true)
	{
		int num = (int)Math.Round(longitude * 3600.0);
		int value = num / 3600;
		num = Math.Abs(num % 3600);
		int num2 = num / 60;
		num %= 60;
		string text = Math.Abs(value) + "° ";
		if (includeMinutes)
		{
			text = text + num2 + "' ";
		}
		if (includeSeconds)
		{
			text = text + num + "\" ";
		}
		return text + ((longitude >= 0.0) ? cacheAutoLOC_7003274 : cacheAutoLOC_7003275);
	}

	public static string PrintTimeLong(double time)
	{
		return dateTimeFormatter.PrintTimeLong(time);
	}

	public static string PrintTimeStamp(double time, bool days = false, bool years = false)
	{
		return dateTimeFormatter.PrintTimeStamp(time, days, years);
	}

	public static string PrintTimeStampCompact(double time, bool days = false, bool years = false)
	{
		return dateTimeFormatter.PrintTimeStampCompact(time, days, years);
	}

	public static string PrintTime(double time, int valuesOfInterest, bool explicitPositive)
	{
		return dateTimeFormatter.PrintTime(time, valuesOfInterest, explicitPositive);
	}

	public static string PrintTime(double time, int valuesOfInterest, bool explicitPositive, bool logEnglish)
	{
		return dateTimeFormatter.PrintTime(time, valuesOfInterest, explicitPositive, logEnglish);
	}

	public static string PrintTimeCompact(double time, bool explicitPositive)
	{
		return dateTimeFormatter.PrintTimeCompact(time, explicitPositive);
	}

	public static string PrintDateDelta(double time, bool includeTime, bool includeSeconds = false, bool useAbs = false)
	{
		return dateTimeFormatter.PrintDateDelta(time, includeTime, includeSeconds, useAbs);
	}

	public static string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, bool useAbs = false)
	{
		return dateTimeFormatter.PrintDateDeltaCompact(time, includeTime, includeSeconds, useAbs);
	}

	public static string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, int interestedPlaces, bool useAbs = false)
	{
		return dateTimeFormatter.PrintDateDeltaCompact(time, includeTime, includeSeconds, useAbs, interestedPlaces);
	}

	public static string PrintDate(double time, bool includeTime, bool includeSeconds = false)
	{
		return dateTimeFormatter.PrintDate(time, includeTime, includeSeconds);
	}

	public static string PrintDateNew(double time, bool includeTime)
	{
		return dateTimeFormatter.PrintDateNew(time, includeTime);
	}

	public static string PrintDateCompact(double time, bool includeTime, bool includeSeconds = false)
	{
		return dateTimeFormatter.PrintDateCompact(time, includeTime, includeSeconds);
	}

	public static string PrintModuleName(string moduleName)
	{
		string text = moduleName;
		if (text.StartsWith("Module"))
		{
			text = moduleName.Remove(0, "Module".Length);
		}
		return text.PrintSpacedStringFromCamelcase();
	}

	public static string PrintLocalizedModuleName(string moduleName)
	{
		string text = moduleName;
		string text2 = "";
		if (text.StartsWith("Module"))
		{
			text = moduleName.Remove(0, "Module".Length);
		}
		return text switch
		{
			"Ablator" => "#autoLOC_501008", 
			"SpacePlaneHangar" => "#autoLOC_6001661", 
			"IntakeAir" => "#autoLOC_6002101", 
			"Oxidizer" => "#autoLOC_502035", 
			"Runway" => "#autoLOC_300899", 
			"LaunchPad" => "#autoLOC_300898", 
			"ElectricCharge" => "#autoLOC_313132", 
			"SolidFuel" => "#autoLOC_501001", 
			"Vertical" => "#autoLOC_300898", 
			"LiquidFuel" => "#autoLOC_502032", 
			"EVAPropellant" => "#autoLOC_501006", 
			"VehicleAssemblyBuilding" => "#autoLOC_6001660", 
			"MonoPropellant" => "#autoLOC_501002", 
			"XenonGas" => "#autoLOC_501003", 
			"Ore" => "#autoLOC_501007", 
			"SpaceplaneHangar" => "#autoLOC_6001661", 
			_ => text.PrintSpacedStringFromCamelcase(), 
		};
	}

	public static string PrintSpacedStringFromCamelcase(this string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		string text = s;
		int i = 0;
		for (int length = text.Length; i < length; i++)
		{
			if (i == 0 && !char.IsUpper(text[i]))
			{
				text = char.ToUpper(s[0]) + s.Substring(1);
			}
			else if (i != 0 && char.IsUpper(text[i]) && !char.IsUpper(text[i - 1]))
			{
				text = text.Insert(i, " ");
				i++;
			}
		}
		return text;
	}

	public static string GetRelativePath(string fullPath, string basePath)
	{
		string text = Path.DirectorySeparatorChar.ToString();
		if (!basePath.EndsWith(text))
		{
			basePath += text;
		}
		Uri uri = new Uri(basePath);
		Uri uri2 = new Uri(fullPath);
		Uri uri3 = uri.MakeRelativeUri(uri2);
		int num = 0;
		string text2 = uri3.ToString();
		for (int i = 0; i < text2.Length && !char.IsLetter(text2[i]); i++)
		{
			num++;
		}
		return text2.Substring(num);
	}

	public static string GetOrCreatePath(string relPath)
	{
		if (!Directory.Exists(ApplicationRootPath + relPath))
		{
			Directory.CreateDirectory(ApplicationRootPath + relPath);
		}
		return ApplicationRootPath + relPath;
	}

	public static string GenerateFilePathWithDate(string filePath)
	{
		return filePath.Insert(filePath.LastIndexOf("."), " (" + SystemDateTime.DateTimeNow().ToString("yyyy_MM_dd_HH_mm_ss") + ")");
	}

	public static bool MoveFile(string filePath, string destinationPath, bool overwrite = false)
	{
		try
		{
			if (overwrite && File.Exists(destinationPath))
			{
				File.Delete(destinationPath);
			}
			File.Move(filePath, destinationPath);
		}
		catch (Exception ex)
		{
			Debug.LogError("Unable to move file from " + filePath + " to " + destinationPath + "\n" + ex.Message);
			return false;
		}
		return true;
	}

	public static string GetTransformPathToRoot(Transform t, Transform root)
	{
		if (!t)
		{
			Debug.LogError("KSPUtil.GetTransformPathToRoot() passed null t!");
			return "";
		}
		if (t == root)
		{
			return "";
		}
		string text = t.name;
		Transform parent = t.parent;
		while (parent != null && parent != root)
		{
			text = text.Insert(0, parent.name + "/");
			parent = parent.parent;
		}
		return text;
	}

	public static string GetTransformIndexPathToRoot(Transform t, Transform root)
	{
		if (!t)
		{
			Debug.LogError("KSPUtil.GetTransformPathToRoot() passed null t!");
			return "";
		}
		if (t == root)
		{
			return "";
		}
		string text = t.GetSiblingIndex().ToString();
		Transform parent = t.parent;
		while (parent != null && parent != root)
		{
			text = text.Insert(0, parent.GetSiblingIndex() + "/");
			parent = parent.parent;
		}
		return text;
	}

	public static Transform FindTransformAtIndexPath(string indexPath, Transform root)
	{
		if (string.IsNullOrEmpty(indexPath))
		{
			return root;
		}
		int num = -1;
		int length = indexPath.Length;
		for (int i = 0; i < length; i++)
		{
			if (indexPath[i] == '/')
			{
				num = i;
				break;
			}
		}
		int index;
		if (num >= 0)
		{
			string s = indexPath.Substring(0, num);
			string indexPath2 = indexPath.Substring(num + 1);
			index = int.Parse(s);
			Transform child = root.GetChild(index);
			return FindTransformAtIndexPath(indexPath2, child);
		}
		index = int.Parse(indexPath);
		return root.GetChild(index);
	}

	public static string StripFileExtension(FileInfo file)
	{
		return file.Name.Substring(0, file.Name.Length - file.Extension.Length);
	}

	public static string SanitizeString(string originalString, char replacementChar, bool replaceEmpty)
	{
		originalString = originalString.Replace('\\', replacementChar);
		originalString = originalString.Replace('/', replacementChar);
		originalString = originalString.Replace('.', replacementChar);
		originalString = originalString.Replace(':', replacementChar);
		originalString = originalString.Replace('|', replacementChar);
		originalString = originalString.Replace('*', replacementChar);
		originalString = originalString.Replace('?', replacementChar);
		originalString = originalString.Replace('{', replacementChar);
		originalString = originalString.Replace('}', replacementChar);
		originalString = originalString.Replace('<', replacementChar);
		originalString = originalString.Replace('>', replacementChar);
		originalString = originalString.Replace('"', replacementChar);
		originalString = originalString.Trim();
		if (replaceEmpty && originalString == string.Empty)
		{
			originalString = "Unnamed";
		}
		else if (CheckDOSName(originalString))
		{
			originalString += replacementChar;
		}
		return originalString;
	}

	public static string SanitizeFilename(string originalFilename)
	{
		return SanitizeString(Localizer.Format(originalFilename), '_', replaceEmpty: true);
	}

	public static bool CheckDOSName(string originalString)
	{
		bool result = false;
		string[] array = new string[17]
		{
			"con", "prn", "aux", "nul", "clock$", "lst", "lpt", "lpt0", "lpt1", "lpt2",
			"lpt3", "lpt4", "com0", "com1", "com2", "com3", "com4"
		};
		for (int i = 0; i < array.Length; i++)
		{
			if (originalString.ToLowerInvariant() == array[i])
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public static string SanitizeInstanceName(string originalString)
	{
		string result = originalString;
		int num = originalString.IndexOf(" (", StringComparison.InvariantCultureIgnoreCase);
		if (num == -1)
		{
			num = originalString.IndexOf("(", StringComparison.InvariantCultureIgnoreCase);
		}
		if (num != -1)
		{
			result = originalString.Substring(0, num);
		}
		num = originalString.ToLower().IndexOf("(clone", StringComparison.InvariantCultureIgnoreCase);
		if (num != -1)
		{
			result = originalString.Substring(0, num);
		}
		return result;
	}

	public static float CalculateFolderSize(string folderPath, out int totalFiles)
	{
		float num = 0f;
		totalFiles = 0;
		try
		{
			if (!Directory.Exists(folderPath))
			{
				return num;
			}
			string[] files = Directory.GetFiles(folderPath);
			for (int i = 0; i < files.Length; i++)
			{
				if (File.Exists(files[i]))
				{
					FileInfo fileInfo = new FileInfo(files[i]);
					num += (float)fileInfo.Length;
					totalFiles++;
				}
			}
			string[] directories = Directory.GetDirectories(folderPath);
			for (int j = 0; j < directories.Length; j++)
			{
				num += CalculateFolderSize(directories[j], out totalFiles);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Unable to calculate folder size: {0}", ex.Message);
		}
		return num;
	}

	public static VersionCompareResult CheckVersion(string versionString, int lastMajor, int lastMinor, int lastRev)
	{
		string[] array = versionString.Split('.');
		if (array.Length != 3)
		{
			return VersionCompareResult.INVALID;
		}
		int version_major = int.Parse(array[0]);
		int version_minor = int.Parse(array[1]);
		int version_revision = int.Parse(array[2]);
		return CheckVersion(version_major, version_minor, version_revision, lastMajor, lastMinor, lastRev);
	}

	public static VersionCompareResult CheckVersion(int version_major, int version_minor, int version_revision, int lastMajor, int lastMinor, int lastRev)
	{
		bool flag = false;
		if (version_major > lastMajor)
		{
			flag = true;
		}
		else if (version_major == lastMajor)
		{
			if (version_minor > lastMinor)
			{
				flag = true;
			}
			else if (version_minor == lastMinor && version_revision >= lastRev)
			{
				flag = true;
			}
		}
		if (version_major <= Versioning.version_major && (version_major != Versioning.version_major || version_minor <= Versioning.version_minor) && (version_major != Versioning.version_major || version_minor != Versioning.version_minor || version_revision <= Versioning.Revision))
		{
			if (flag)
			{
				return VersionCompareResult.COMPATIBLE;
			}
			return VersionCompareResult.INCOMPATIBLE_TOO_EARLY;
		}
		return VersionCompareResult.INCOMPATIBLE_TOO_LATE;
	}

	public static bool HasAncestorTransform(Transform src, Transform ancestor)
	{
		if (ancestor == src)
		{
			return true;
		}
		if (src.parent != null)
		{
			return HasAncestorTransform(src.parent, ancestor);
		}
		return false;
	}

	public static bool HasDescendantTransform(Transform src, Transform child)
	{
		return HasAncestorTransform(child, src);
	}

	public static Rect ClampRectToScreen(Rect r)
	{
		r.x = Mathf.Clamp(r.x, 0f, (float)Screen.width - r.width);
		r.y = Mathf.Clamp(r.y, 0f, (float)Screen.height - r.height);
		return r;
	}

	public static string ReplaceString(string src, params StringReplacement[] replacements)
	{
		int num = replacements.Length;
		for (int i = 0; i < num; i++)
		{
			StringReplacement stringReplacement = replacements[i];
			src = src.Replace(stringReplacement.badString, stringReplacement.replacement);
		}
		return src;
	}

	public static List<T> FindComponentsImplementing<T>(GameObject go, bool returnInactive) where T : class
	{
		List<T> list = new List<T>();
		MonoBehaviour[] components = go.GetComponents<MonoBehaviour>();
		int num = components.Length;
		for (int i = 0; i < num; i++)
		{
			MonoBehaviour monoBehaviour = components[i];
			if (monoBehaviour is T && (returnInactive || monoBehaviour.enabled))
			{
				list.Add(monoBehaviour as T);
			}
		}
		return list;
	}

	public static float HeadingRadians(Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		if (v1 == v2)
		{
			return 0f;
		}
		return UtilMath.WrapAround(Mathf.Acos(Vector3.Dot(v1, v2)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(v1, v2), upAxis)), 0f, (float)Math.PI * 2f);
	}

	public static float HeadingDegrees(Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		if (v1 == v2)
		{
			return 0f;
		}
		return UtilMath.WrapAround(Mathf.Acos(Vector3.Dot(v1, v2)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(v1, v2), upAxis)) * 57.29578f, 0f, 360f);
	}

	public static float BearingRadians(Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		if (v1 == v2)
		{
			return 0f;
		}
		return Mathf.Acos(Vector3.Dot(v1, v2)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(v1, v2), upAxis));
	}

	public static float BearingDegrees(Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		if (v1 == v2)
		{
			return 0f;
		}
		return Mathf.Acos(Vector3.Dot(v1, v2)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(v1, v2), upAxis)) * 57.29578f;
	}

	public static string PrintCollection<T>(IEnumerable<T> collection, string separator = ", ")
	{
		return PrintCollection(collection, separator, (T c) => c.ToString());
	}

	public static string PrintCollection<T>(IEnumerable<T> collection, string separator, Func<T, string> stringAccessor)
	{
		string text = "";
		IEnumerator<T> enumerator = collection.GetEnumerator();
		int num = collection.Count();
		for (int i = 0; i < num; i++)
		{
			enumerator.MoveNext();
			text += stringAccessor(enumerator.Current);
			if (i != num - 1)
			{
				text += separator;
			}
		}
		return text;
	}

	public static string AppendValueToString(string s0, string val, char separator)
	{
		string text = string.Copy(s0);
		if (string.IsNullOrEmpty(s0))
		{
			text = val;
		}
		else if (!text.Contains(val))
		{
			text = text + separator + " " + val;
		}
		return text;
	}

	public static void FindTagsInChildren(List<string> tags, Transform trf)
	{
		tags.AddUnique(trf.gameObject.tag);
		for (int i = 0; i < trf.childCount; i++)
		{
			Transform child = trf.GetChild(i);
			FindTagsInChildren(tags, child);
		}
	}

	public static Matrix4x4 Add(Matrix4x4 left, Matrix4x4 right)
	{
		Matrix4x4 zero = Matrix4x4.zero;
		for (int i = 0; i < 4; i++)
		{
			zero.SetColumn(i, left.GetColumn(i) + right.GetColumn(i));
		}
		return zero;
	}

	public static void Add(ref Matrix4x4 left, Matrix4x4 right)
	{
		left.m00 += right.m00;
		left.m01 += right.m01;
		left.m02 += right.m02;
		left.m03 += right.m03;
		left.m10 += right.m10;
		left.m11 += right.m11;
		left.m12 += right.m12;
		left.m13 += right.m13;
		left.m20 += right.m20;
		left.m21 += right.m21;
		left.m22 += right.m22;
		left.m23 += right.m23;
		left.m30 += right.m30;
		left.m31 += right.m31;
		left.m32 += right.m32;
		left.m33 += right.m33;
	}

	public static void Add(ref Matrix4x4 left, ref Matrix4x4 right)
	{
		left.m00 += right.m00;
		left.m01 += right.m01;
		left.m02 += right.m02;
		left.m03 += right.m03;
		left.m10 += right.m10;
		left.m11 += right.m11;
		left.m12 += right.m12;
		left.m13 += right.m13;
		left.m20 += right.m20;
		left.m21 += right.m21;
		left.m22 += right.m22;
		left.m23 += right.m23;
		left.m30 += right.m30;
		left.m31 += right.m31;
		left.m32 += right.m32;
		left.m33 += right.m33;
	}

	public static Vector3d Diag(Matrix4x4 m)
	{
		return new Vector3d(m.m00, m.m11, m.m22);
	}

	public static Matrix4x4 ToDiagonalMatrix(float v)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = v;
		identity.m01 = 0f;
		identity.m02 = 0f;
		identity.m03 = 0f;
		identity.m10 = 0f;
		identity.m11 = v;
		identity.m12 = 0f;
		identity.m13 = 0f;
		identity.m20 = 0f;
		identity.m21 = 0f;
		identity.m22 = v;
		identity.m23 = 0f;
		identity.m30 = 0f;
		identity.m31 = 0f;
		identity.m32 = 0f;
		identity.m33 = v;
		return identity;
	}

	public static void ToDiagonalMatrix(float v, ref Matrix4x4 m)
	{
		m.m00 = v;
		m.m01 = 0f;
		m.m02 = 0f;
		m.m03 = 0f;
		m.m10 = 0f;
		m.m11 = v;
		m.m12 = 0f;
		m.m13 = 0f;
		m.m20 = 0f;
		m.m21 = 0f;
		m.m22 = v;
		m.m23 = 0f;
		m.m30 = 0f;
		m.m31 = 0f;
		m.m32 = 0f;
		m.m33 = v;
	}

	public static void ToDiagonalMatrix2(float v, ref Matrix4x4 m)
	{
		m.m00 = v;
		m.m11 = v;
		m.m22 = v;
		m.m33 = v;
	}

	public static Matrix4x4 ToDiagonalMatrix(Vector3 v)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = v.x;
		identity.m11 = v.y;
		identity.m22 = v.z;
		return identity;
	}

	public static Matrix4x4 ToDiagonalMatrix2(Vector3 v, ref Matrix4x4 m)
	{
		m.m00 = v.x;
		m.m11 = v.y;
		m.m22 = v.z;
		return m;
	}

	public static Matrix4x4 OuterProduct(Vector3 left, Vector3 right)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = left.x * right.x;
		identity.m01 = left.x * right.y;
		identity.m02 = left.x * right.z;
		identity.m10 = left.y * right.x;
		identity.m11 = left.y * right.y;
		identity.m12 = left.y * right.z;
		identity.m20 = left.z * right.x;
		identity.m21 = left.z * right.y;
		identity.m22 = left.z * right.z;
		return identity;
	}

	public static void OuterProduct(Vector3 left, Vector3 right, ref Matrix4x4 m)
	{
		m.m00 = left.x * right.x;
		m.m01 = left.x * right.y;
		m.m02 = left.x * right.z;
		m.m03 = 0f;
		m.m10 = left.y * right.x;
		m.m11 = left.y * right.y;
		m.m12 = left.y * right.z;
		m.m13 = 0f;
		m.m20 = left.z * right.x;
		m.m21 = left.z * right.y;
		m.m22 = left.z * right.z;
		m.m23 = 0f;
		m.m30 = 0f;
		m.m31 = 0f;
		m.m32 = 0f;
		m.m33 = 1f;
	}

	public static void OuterProduct2(Vector3 left, Vector3 right, ref Matrix4x4 m)
	{
		m.m00 = left.x * right.x;
		m.m01 = left.x * right.y;
		m.m02 = left.x * right.z;
		m.m10 = left.y * right.x;
		m.m11 = left.y * right.y;
		m.m12 = left.y * right.z;
		m.m20 = left.z * right.x;
		m.m21 = left.z * right.y;
		m.m22 = left.z * right.z;
	}

	public static bool DeepCompare<T>(this HashSet<T> a, HashSet<T> b)
	{
		if (a.Count != b.Count)
		{
			return false;
		}
		HashSet<T>.Enumerator enumerator = a.GetEnumerator();
		T current;
		do
		{
			if (enumerator.MoveNext())
			{
				current = enumerator.Current;
				continue;
			}
			return true;
		}
		while (b.Contains(current));
		return false;
	}

	public static string GetPartName(string configPartID)
	{
		int length = configPartID.IndexOf('_');
		return configPartID.Substring(0, length);
	}

	public static void GetPartInfo(string configPartID, ref string partName, ref string craftID)
	{
		int num = configPartID.IndexOf('_');
		partName = configPartID.Substring(0, num);
		craftID = configPartID.Substring(num + 1, configPartID.Length - num - 1);
	}

	public static bool GetAttachNodeInfo(string configAttnID, ref string nodeID, ref string attnPartID, ref Vector3 attnPos)
	{
		string attachMeshName = "";
		return GetAttachNodeInfo(configAttnID, ref nodeID, ref attnPartID, ref attnPos, ref attachMeshName);
	}

	public static bool GetAttachNodeInfo(string configAttnID, ref string nodeID, ref string attnPartID, ref Vector3 attnPos, ref string attachMeshName)
	{
		Vector3 attnRot = Vector3.zero;
		Vector3 secAxis = Vector3.zero;
		return GetAttachNodeInfo(configAttnID, ref nodeID, ref attnPartID, ref attnPos, ref attachMeshName, ref attnRot, ref secAxis);
	}

	public static bool GetAttachNodeInfo(string configAttnID, ref string nodeID, ref string attnPartID, ref Vector3 attnPos, ref string attachMeshName, ref Vector3 attnRot, ref Vector3 secAxis)
	{
		bool result = false;
		string[] array = configAttnID.Split(',');
		nodeID = array[0];
		string[] array2 = array[1].Split('_');
		attnPartID = array2[1];
		if (array2.Length > 2)
		{
			string vectorString = array2[2].Replace('|', ',');
			attnPos = ParseVector3(vectorString);
			result = true;
		}
		if (nodeID == "srfAttach" && array.Length > 2)
		{
			attachMeshName = array[2];
			if (array.Length > 3)
			{
				string vectorString2 = array[3].Replace('|', ',');
				string vectorString3 = array[4].Replace('|', ',');
				string vectorString4 = array[5].Replace('|', ',');
				attnPos = ParseVector3(vectorString2);
				attnRot = ParseVector3(vectorString3);
				secAxis = ParseVector3(vectorString4);
				result = true;
			}
		}
		return result;
	}

	public static bool GetAttachNodeInfo(string configAttnID, ref string nodeID, ref string attnPartID, ref Vector3 attnPos, ref Vector3 attnRot, ref Vector3 attnActualPos, ref Vector3 attnActualRot)
	{
		bool result = false;
		string[] array = configAttnID.Split(',');
		nodeID = array[0];
		string[] array2 = array[1].Split('_');
		attnPartID = array2[1];
		if (array2.Length >= 3)
		{
			result = true;
			attnPos = ParseVector3(array2[2], '|');
			if (array2.Length >= 4)
			{
				attnRot = ParseVector3(array2[3], '|');
			}
			else
			{
				attnRot = Vector3.positiveInfinity;
			}
			if (array2.Length >= 5)
			{
				attnActualPos = ParseVector3(array2[4], '|');
			}
			else
			{
				attnActualPos = Vector3.positiveInfinity;
			}
			if (array2.Length >= 6)
			{
				attnActualRot = ParseVector3(array2[5], '|');
			}
			else
			{
				attnActualRot = Vector3.positiveInfinity;
			}
		}
		return result;
	}

	public static string GetLinkID(string configAttnID)
	{
		int num = configAttnID.IndexOf('_');
		return configAttnID.Substring(num + 1, configAttnID.Length - num - 1);
	}

	public static Part GetPartByCraftID(this List<Part> parts, uint id)
	{
		if (id == 0)
		{
			return null;
		}
		int count = parts.Count;
		Part part;
		do
		{
			if (count-- > 0)
			{
				part = parts[count];
				continue;
			}
			return null;
		}
		while (part.craftID != id);
		return part;
	}

	public static void RemoveNonHighlightableRenderers(this List<Renderer> rends)
	{
		Type typeFromHandle = typeof(ParticleSystemRenderer);
		Part part = null;
		int count = rends.Count;
		while (count-- > 0)
		{
			Renderer renderer = rends[count];
			Type type = renderer.GetType();
			if (renderer.gameObject.layer == 1)
			{
				part = FlightGlobals.GetPartUpwardsCached(renderer.gameObject);
			}
			if (type == typeFromHandle || (HighLogic.LoadedSceneIsFlight && renderer.gameObject.layer == 1 && (part == null || part.State != PartStates.PLACEMENT)))
			{
				rends.RemoveAt(count);
			}
		}
	}

	public static int GenerateSuperSeed(Guid guid, int seed)
	{
		byte[] array = guid.ToByteArray();
		for (int i = 0; i < 3; i++)
		{
			array[i] ^= array[i + 4];
			array[i] ^= array[i + 8];
			array[i] ^= array[i + 12];
		}
		int num = BitConverter.ToInt32(array, 0);
		return (num + seed) * (num + seed + 1) / 2 + seed;
	}

	public static Texture2DArray GenerateTexture2DArray(List<Texture2D> textureList, TextureFormat format, bool createMipMaps)
	{
		if (textureList == null)
		{
			Debug.LogWarning("[KSPUtil.GenerateTexture2DArray] textureList was null!!;");
			return null;
		}
		if (textureList.Count == 0)
		{
			Debug.LogWarning("[KSPUtil.GenerateTexture2DArray] textureList has 0 elements!!;");
		}
		Texture2DArray texture2DArray = new Texture2DArray(textureList[0].width, textureList[0].height, textureList.Count, format, createMipMaps);
		texture2DArray.anisoLevel = textureList[0].anisoLevel;
		texture2DArray.filterMode = textureList[0].filterMode;
		texture2DArray.wrapMode = textureList[0].wrapMode;
		for (int i = 0; i < textureList.Count; i++)
		{
			if (textureList[i] == null)
			{
				Debug.LogWarning("[KSPUtil.GenerateTexture2DArray] textureList[" + i + "] was null!!;");
			}
			else
			{
				texture2DArray.SetPixels(textureList[i].GetPixels(0), i, 0);
			}
		}
		texture2DArray.Apply(createMipMaps);
		return texture2DArray;
	}

	public static ObjectActivator<T> GetActivator<T>(ConstructorInfo ctor)
	{
		_ = ctor.DeclaringType;
		ParameterInfo[] parameters = ctor.GetParameters();
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "args");
		Expression[] array = new Expression[parameters.Length];
		for (int i = 0; i < parameters.Length; i++)
		{
			Expression index = Expression.Constant(i);
			Type parameterType = parameters[i].ParameterType;
			Expression expression = Expression.Convert(Expression.ArrayIndex(parameterExpression, index), parameterType);
			array[i] = expression;
		}
		NewExpression body = Expression.New(ctor, array);
		return (ObjectActivator<T>)Expression.Lambda(typeof(ObjectActivator<T>), body, parameterExpression).Compile();
	}
}
