using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class KSPUtil
{
	public static class SystemDateTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static DateTime DateTimeNow()
		{
			throw null;
		}
	}

	public class DefaultDateTimeFormatter : IDateTimeFormatter
	{
		private double cachedSecondsUTforTimeStampCompact;

		private bool cachedDaysforTimeStampCompact;

		private bool cachedYearsforTimeStampCompact;

		private string cachedTimeStampCompact;

		private static int[] dateHolder;

		private static int[] absDateHolder;

		private static double secondsUTHolder;

		private int cachedYearsforPrintDate;

		private int cachedDaysforPrintDate;

		private int cachedHoursforPrintDate;

		private int cachedMinsforPrintDate;

		private int cachedSecondsforPrintDate;

		private bool cachedIncludeTimeforPrintDate;

		private bool cachedIncludeSecondsforPrintDate;

		private string cachedPrintDate;

		private double cachedSecondsUTforPrintDateCompact;

		private bool cachedIncludeTimeforPrintDateCompact;

		private bool cachedIncludeSecondsforPrintDateCompact;

		private string cachedPrintDateCompact;

		public int Minute
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public int Hour
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public int Day
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public int Year
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		private static int KerbinDay
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		private static int KerbinYear
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		private static int EarthDay
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		private static int EarthYear
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public DefaultDateTimeFormatter()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static DefaultDateTimeFormatter()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static StringBuilder AddUnits(StringBuilder sb, int val, string singular, string plural)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static string IsBadNum(double time)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintTimeLong(double time)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintTimeStamp(double time, bool days = false, bool years = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintTimeStampCompact(double time, bool days = false, bool years = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintTime(double time, int valuesOfInterest, bool explicitPositive)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintTime(double time, int valuesOfInterest, bool explicitPositive, bool logEnglish)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintTimeCompact(double time, bool explicitPositive)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void GetDateFromUT(double time)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void get_date_from_UT(double time, int year_len, int day_len)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void GetEarthDateFromUT(double time)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void GetKerbinDateFromUT(double time)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintDateDelta(double time, bool includeTime, bool includeSeconds, bool useAbs)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, bool useAbs)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, bool useAbs, int interestedPlaces)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetUTYear()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetUTDay()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetUTHour()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetUTMinute()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetUTSecond()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetTimeSpanYear()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetTimeSpanDay()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetTimeSpanHour()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetTimeSpanMinute()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetTimeSpanSecond()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintDate(double time, bool includeTime, bool includeSeconds = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintDateNew(double time, bool includeTime)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string PrintDateCompact(double time, bool includeTime, bool includeSeconds = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void AppendTimeToStringBuilder(StringBuilder sb, bool displaySeconds)
		{
			throw null;
		}
	}

	[Serializable]
	public class StringReplacement
	{
		public string badString;

		public string replacement;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public StringReplacement()
		{
			throw null;
		}
	}

	public delegate T ObjectActivator<T>(params object[] args);

	private static string cacheAutoLOC_7003243;

	private static string cacheAutoLOC_7003244;

	private static string cacheAutoLOC_7003272;

	private static string cacheAutoLOC_7003273;

	private static string cacheAutoLOC_7003274;

	private static string cacheAutoLOC_7003275;

	private static string cacheAutoLOC_6002317;

	private static string cacheAutoLOC_6002318;

	private static string cacheAutoLOC_6002319;

	private static string cacheAutoLOC_6002320;

	private static string cacheAutoLOC_6002321;

	private static string cacheAutoLOC_6002322;

	private static string cacheAutoLOC_6002323;

	private static string cacheAutoLOC_6002324;

	private static string cacheAutoLOC_6002325;

	private static string cacheAutoLOC_6002326;

	private static string cacheAutoLOC_6002327;

	private static string cacheAutoLOC_6002328;

	private static string cacheAutoLOC_6002329;

	private static string cacheAutoLOC_6002330;

	private static string cacheAutoLOC_6002331;

	private static string cacheAutoLOC_6002332;

	private static string cacheAutoLOC_6002333;

	private static string cacheAutoLOC_6002334;

	private static string cacheAutoLOC_6002335;

	private static string cacheAutoLOC_6002336;

	private static string cacheAutoLOC_6002337;

	private static string cacheAutoLOC_6002338;

	private static string cacheAutoLOC_6002339;

	private static string cacheAutoLOC_6002340;

	private static string cacheAutoLOC_6002341;

	private static string cacheAutoLOC_6002342;

	private static string cacheAutoLOC_6002343;

	private static string cacheAutoLOC_6002344;

	private static string cacheAutoLOC_6002345;

	private static bool localTimeZoneInvalid;

	private static IDateTimeFormatter _dateTimeFormatter;

	public static string[] shortSIprefixes;

	public static string[] longSIprefixes;

	public static double[] prefixMults;

	public static double[] digitsScale;

	public static int unitIndex;

	public static IDateTimeFormatter dateTimeFormatter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public static string ApplicationRootPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string ApplicationFileProtocol
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static KSPUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string LocalizeNumber(double value, string format)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string LocalizeNumber(float value, string format)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintSI(double amount, string unitName, int sigFigs = 3, bool longPrefix = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector2 vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector2 vector, string separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector3 vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector3 vector, string separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector3d vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector3d vector, string separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector4 vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector4 vector, string separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteQuaternion(Quaternion quaternion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteQuaternion(Quaternion quaternion, string separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteQuaternion(QuaternionD quaternion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteQuaternion(QuaternionD quaternion, string separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2 ParseVector2(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2 ParseVector2(string vectorString, char separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2 ParseVector2(string x, string y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ParseVector3(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ParseVector3(string vectorString, char separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ParseVector3(string x, string y, string z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d ParseVector3d(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d ParseVector3d(string vectorString, char separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d ParseVector3d(string x, string y, string z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector4 ParseVector4(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector4 ParseVector4(string x, string y, string z, string w)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector4 ParseVector4(string vectorString, char separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion ParseQuaternion(string quaternionString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion ParseQuaternion(string quaternionString, char separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion ParseQuaternion(string x, string y, string z, string w)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD ParseQuaternionD(string quaternionString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD ParseQuaternionD(string x, string y, string z, string w)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD ParseQuaternionD(string quaternionString, char separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteArray<T>(T[] array) where T : IConvertible
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T[] ParseArray<T>(string arrayString, ParserMethod<T> parser)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindInPartModel(Transform part, string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Transform recurseModels(Transform obj, string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintCoordinates(double latitude, double longitude, bool singleLine, bool includeMinutes = true, bool includeSeconds = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintLatitude(double latitude, bool includeMinutes = true, bool includeSeconds = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintLongitude(double longitude, bool includeMinutes = true, bool includeSeconds = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintTimeLong(double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintTimeStamp(double time, bool days = false, bool years = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintTimeStampCompact(double time, bool days = false, bool years = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintTime(double time, int valuesOfInterest, bool explicitPositive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintTime(double time, int valuesOfInterest, bool explicitPositive, bool logEnglish)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintTimeCompact(double time, bool explicitPositive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintDateDelta(double time, bool includeTime, bool includeSeconds = false, bool useAbs = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, bool useAbs = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, int interestedPlaces, bool useAbs = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintDate(double time, bool includeTime, bool includeSeconds = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintDateNew(double time, bool includeTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintDateCompact(double time, bool includeTime, bool includeSeconds = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintModuleName(string moduleName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintLocalizedModuleName(string moduleName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintSpacedStringFromCamelcase(this string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetRelativePath(string fullPath, string basePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetOrCreatePath(string relPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GenerateFilePathWithDate(string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool MoveFile(string filePath, string destinationPath, bool overwrite = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetTransformPathToRoot(Transform t, Transform root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetTransformIndexPathToRoot(Transform t, Transform root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindTransformAtIndexPath(string indexPath, Transform root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string StripFileExtension(FileInfo file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SanitizeString(string originalString, char replacementChar, bool replaceEmpty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SanitizeFilename(string originalFilename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CheckDOSName(string originalString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SanitizeInstanceName(string originalString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float CalculateFolderSize(string folderPath, out int totalFiles)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VersionCompareResult CheckVersion(string versionString, int lastMajor, int lastMinor, int lastRev)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VersionCompareResult CheckVersion(int version_major, int version_minor, int version_revision, int lastMajor, int lastMinor, int lastRev)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HasAncestorTransform(Transform src, Transform ancestor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HasDescendantTransform(Transform src, Transform child)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Rect ClampRectToScreen(Rect r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ReplaceString(string src, params StringReplacement[] replacements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<T> FindComponentsImplementing<T>(GameObject go, bool returnInactive) where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float HeadingRadians(Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float HeadingDegrees(Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float BearingRadians(Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float BearingDegrees(Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintCollection<T>(IEnumerable<T> collection, string separator = ", ")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintCollection<T>(IEnumerable<T> collection, string separator, Func<T, string> stringAccessor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string AppendValueToString(string s0, string val, char separator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindTagsInChildren(List<string> tags, Transform trf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4 Add(Matrix4x4 left, Matrix4x4 right)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Add(ref Matrix4x4 left, Matrix4x4 right)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Add(ref Matrix4x4 left, ref Matrix4x4 right)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Diag(Matrix4x4 m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4 ToDiagonalMatrix(float v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ToDiagonalMatrix(float v, ref Matrix4x4 m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ToDiagonalMatrix2(float v, ref Matrix4x4 m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4 ToDiagonalMatrix(Vector3 v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4 ToDiagonalMatrix2(Vector3 v, ref Matrix4x4 m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4 OuterProduct(Vector3 left, Vector3 right)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void OuterProduct(Vector3 left, Vector3 right, ref Matrix4x4 m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void OuterProduct2(Vector3 left, Vector3 right, ref Matrix4x4 m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool DeepCompare<T>(this HashSet<T> a, HashSet<T> b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPartName(string configPartID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void GetPartInfo(string configPartID, ref string partName, ref string craftID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool GetAttachNodeInfo(string configAttnID, ref string nodeID, ref string attnPartID, ref Vector3 attnPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool GetAttachNodeInfo(string configAttnID, ref string nodeID, ref string attnPartID, ref Vector3 attnPos, ref string attachMeshName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool GetAttachNodeInfo(string configAttnID, ref string nodeID, ref string attnPartID, ref Vector3 attnPos, ref string attachMeshName, ref Vector3 attnRot, ref Vector3 secAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool GetAttachNodeInfo(string configAttnID, ref string nodeID, ref string attnPartID, ref Vector3 attnPos, ref Vector3 attnRot, ref Vector3 attnActualPos, ref Vector3 attnActualRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetLinkID(string configAttnID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part GetPartByCraftID(this List<Part> parts, uint id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveNonHighlightableRenderers(this List<Renderer> rends)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GenerateSuperSeed(Guid guid, int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2DArray GenerateTexture2DArray(List<Texture2D> textureList, TextureFormat format, bool createMipMaps)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ObjectActivator<T> GetActivator<T>(ConstructorInfo ctor)
	{
		throw null;
	}
}
