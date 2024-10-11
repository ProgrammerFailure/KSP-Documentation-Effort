using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;

namespace FinePrint.Utilities;

public class StringUtilities
{
	private static string cacheAutoLOC_289926;

	private static string cacheAutoLOC_289929;

	private static string cacheAutoLOC_289933;

	private static string cacheAutoLOC_289969;

	private static string cacheAutoLOC_289916;

	private static string cacheAutoLOC_290048;

	private static string cacheAutoLOC_290050;

	private static string cacheAutoLOC_290052;

	private static string cacheAutoLOC_290054;

	private static string cacheAutoLOC_290056;

	private static string cacheAutoLOC_290058;

	private static string cacheAutoLOC_290060;

	private static string cacheAutoLOC_290062;

	private static string cacheAutoLOC_7001031;

	private static string cacheAutoLOC_7001032;

	private static string cacheAutoLOC_7001033;

	private static string cacheAutoLOC_6001220;

	private static List<string> greekMap;

	private static List<string> alphaNumeric;

	private static List<string> uniqueSites;

	private static List<string> genericPrefixes;

	private static List<string> developerPrefixes;

	private static List<string> abstractPrefixes;

	private static List<string> abstractSuffixes;

	private static List<string> planetSuffixes;

	private static List<string> landSuffixes;

	private static List<string> homeSuffixes;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StringUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static StringUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadSiteGenerationInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string GetSiteGeneratorInfoFileName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<string> FillListWithElements(ConfigNode node, string nodeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GenerateSiteName(int seed, CelestialBody body, bool landLocked, bool allowNamed = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string NamedSitePrefix(KSPRandom generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string NamedSiteSuffix(CelestialBody body, bool landLocked, Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string AlphaNumericDesignation(int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string IntegerToGreek(int x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ShortName(string verbose)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PossessiveString(string thing)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ThisThisAndThat(List<string> stringList, string connector = "#autoLOC_6002373", string conjugation = "#autoLOC_6002374")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ShortKerbalName(string fullKerbalName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string TitleCase(string str)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryParseGeneric<T>(string input, out T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PackDelimitedString<T>(List<T> items, char delimiter = '|')
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<T> UnpackDelimitedString<T>(string items, char delimiter = '|')
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KeyValuePair<T1, T2> UnpackDelimitedPair<T1, T2>(string items, char delimiter = '|')
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ValueAndUnits(RecordTrackType trackType, double trackValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GrammaticalGender KerbalGrammaticalGender(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string AppropriatePronoun(PronounCasing casing, GrammaticalGender gender, bool postFix = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string CardinalDirectionBetween(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsVowel(char c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float CalculateReadDuration(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> FormattedCurrencies(float funds, float science, float reputation, bool symbols = false, bool verbose = false, TransactionReasons reason = TransactionReasons.None, CurrencyModifierQuery.TextStyling style = CurrencyModifierQuery.TextStyling.OnGUI)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SpecificVesselName(Contract contract)
	{
		throw null;
	}
}
